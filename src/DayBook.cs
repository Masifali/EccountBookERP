// using Architecture.Reports;
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
using FcYBankChargesTypes = System.Object;
using SpecialSreenRightsUserWise = System.Object;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
using UserAccount = global::Architecture.Model.UserAccount;
using VoucherHead = global::Architecture.Model.Accounts.VoucherHead;
using DMSAttachments = global::Architecture.Model.DMSAttachments;
using tblUserRights = global::Architecture.Model.tblUserRights;
using CheqBookHeader = global::Architecture.BLL.Accounts.CheqBookHeader;
using TaxScheduleMain = global::Architecture.BLL.Inventory.TaxScheduleMain;
using PdcInventory = global::Architecture.Model.Accounts.PdcInventory;
using MultiCurrency = global::Architecture.BLL.MultiCurrency;
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

namespace Architecture.WinApp.Account_Definition
{

public class DayBook : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private Attachment AT = new Attachment();

	private AttachmentForDetailRows ATd = new AttachmentForDetailRows();

	private AttachmentForDetailRows ATdDebit = new AttachmentForDetailRows();

	private int RecId;

	private int UpdateDetailIndexCredit = -1;

	private int UpdateDetailIndexDedit = -1;

	private DataTable dtCredit = new DataTable();

	private DataTable dtDedit = new DataTable();

	private DataTable dtSubsidory = new DataTable();

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool AfterCloupReceipt;

	private bool AfterCloupPayment;

	private bool InventoryRelatedAccountsShowInVouchers;

	private int SubsdiaryAccountDatasourceLengthPayment;

	private int SubsdiaryAccountDatasourceLengthReceipt;

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private ToolStrip toolstrip;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnAttachment;

	private Panel panel6;

	private Label label18;

	private Panel panel1;

	private GroupBox groupBox1;

	private TextBox txtDocNo;

	private Label label8;

	private DateTimePicker txtDocDate;

	private Label label2;

	private Label label1;

	internal UltraCombo cmbCashAccount;

	private Label label4;

	private Label lblClosingBalance;

	private Label lblOpeningBalance;

	private Label lblCreditOrDebitForClosing;

	private Label lblCreditOrDebit;

	private Panel panel2;

	private GroupBox groupBox2;

	private Panel panel3;

	internal UltraCombo CmbCreditAccount;

	private Label labelCreditAccount;

	private TextBox txtCrRemarks;

	private Label label11;

	private TextBox txtCreditAmount;

	private Label label12;

	private Button btnAddCrditRow;

	private Button btnUpdateCreditRow;

	private Label label13;

	internal GridEX ReceiptGrid;

	private Panel panel4;

	private Panel panel5;

	private GroupBox groupBox3;

	private Button btnUpdateDebitRow;

	private Button btnAddDebitRow;

	private TextBox txtDebitAmount;

	private Label label14;

	private TextBox txtDrRemarks;

	private Label label15;

	internal UltraCombo CmbDebitAccount;

	private Label LabelDebitAccount;

	internal GridEX PaymentGrid;

	private Label label17;

	private CtrlGrdBar ctrlGrdBar1;

	private Label lblDebitAccountBalance;

	private Label lblDebitAccountBalanceCaption;

	private Label lblCreditAccountBalance;

	private Label lblCreditAccountBalanceCaption;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label3;

	private Button btnDebitRowCancel;

	private Button btnCreditRowCancel;

	private ToolStrip toolStrip1;

	private ToolStripButton btnPrint144;

	private Panel panel11;

	private Label label51;

	private Panel panel7;

	private Button BtnShow;

	private Label label5;

	private Label label6;

	internal UltraCombo CmbCashAccountForHistory;

	private Panel panel8;

	private Label label7;

	internal GridEX DataGridCreditHistory;

	private Panel panel9;

	private Label label9;

	internal GridEX DataGridDebitHistory;

	private CtrlGrdBar ctrlGrdBar3;

	private CtrlGrdBar ctrlGrdBar4;

	private CheckBox ChkPrintPreview;

	private TextBox txtHeaderRemarks;

	private TextBox txtCrPageNo;

	private Label label19;

	private TextBox txtDrPageNo;

	private Label label20;

	private Label label21;

	private DateTimePicker datDrCheqDate;

	private Label lblCheqDate;

	private DateTimePicker datCrChequeDate;

	private Label label22;

	private TextBox txtDrCheqRef;

	private Label label23;

	internal UltraCombo CmbSubsidiaryAccountPayment;

	private Label lblSubsidiaryaccountPayment;

	internal UltraCombo CmbSubsidiaryAccountReceipt;

	private Label lblSubsidiaryaccountReceipt;

	private TextBox txtCrCheqRef;

	internal UltraCombo datVouchderDateForHistory;

	private ToolStripButton btnRefreshHistory;

	public DayBook(UserAccount Puser)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void DayBook_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1270, 550);
			formrights = CommonServices.SetRightsValueInRightsObject("DayBook");
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			InventoryRelatedAccountsShowInVouchers = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("InventoryRelatedAccountsShowInVouchers"));
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			if (SubsidiaryAccountAllownOnVouchers)
			{
				GetSubsidoryAccounts();
				BindSubsidiaryAccountReceipt();
				BindSubsidiaryAccountPayment();
				lblSubsidiaryaccountPayment.Visible = true;
				((Control)(object)CmbSubsidiaryAccountPayment).Visible = true;
				lblSubsidiaryaccountReceipt.Visible = true;
				((Control)(object)CmbSubsidiaryAccountReceipt).Visible = true;
				lblDebitAccountBalance.Location = new Point(90, 17);
				lblDebitAccountBalanceCaption.Location = new Point(202, 17);
				((Control)(object)CmbDebitAccount).Size = new Size(214, 26);
				lblCreditAccountBalance.Location = new Point(90, 19);
				lblCreditAccountBalanceCaption.Location = new Point(202, 19);
				((Control)(object)CmbCreditAccount).Size = new Size(214, 26);
			}
			else
			{
				lblSubsidiaryaccountPayment.Visible = false;
				((Control)(object)CmbSubsidiaryAccountPayment).Visible = false;
				lblSubsidiaryaccountReceipt.Visible = false;
				((Control)(object)CmbSubsidiaryAccountReceipt).Visible = false;
				lblDebitAccountBalance.Location = new Point(255, 17);
				lblDebitAccountBalanceCaption.Location = new Point(370, 17);
				((Control)(object)CmbDebitAccount).Size = new Size(382, 26);
				lblCreditAccountBalance.Location = new Point(255, 19);
				lblCreditAccountBalanceCaption.Location = new Point(370, 19);
				((Control)(object)CmbCreditAccount).Size = new Size(382, 26);
			}
			dtCredit.Columns.Add("AccountId", typeof(int));
			dtCredit.Columns.Add("CreditAccount");
			dtCredit.Columns.Add("SubsidiaryAccountId");
			dtCredit.Columns.Add("SubsidiaryAccount");
			dtCredit.Columns.Add("SubsidiaryAccountTypeId");
			dtCredit.Columns.Add("Amount", typeof(double));
			dtCredit.Columns.Add("PageNo", typeof(int));
			dtCredit.Columns.Add("ChequeDate", typeof(DateTime));
			dtCredit.Columns.Add("ChequeRef", typeof(string));
			dtCredit.Columns.Add("Remarks");
			dtCredit.Columns.Add("LineId");
			dtCredit.Columns.Add("Attachments");
			ReceiptGrid.DataSource = dtCredit;
			ReceiptGrid.RetrieveStructure();
			ReceiptGrdSetting();
			dtDedit.Columns.Add("AccountId", typeof(int));
			dtDedit.Columns.Add("DebitAccount");
			dtDedit.Columns.Add("SubsidiaryAccountId");
			dtDedit.Columns.Add("SubsidiaryAccount");
			dtDedit.Columns.Add("SubsidiaryAccountTypeId");
			dtDedit.Columns.Add("Amount", typeof(double));
			dtDedit.Columns.Add("PageNo", typeof(int));
			dtDedit.Columns.Add("ChequeDate", typeof(DateTime));
			dtDedit.Columns.Add("ChequeRef", typeof(string));
			dtDedit.Columns.Add("Remarks");
			dtDedit.Columns.Add("LineId", typeof(int));
			dtDedit.Columns.Add("Attachments", typeof(int));
			PaymentGrid.DataSource = dtDedit;
			PaymentGrid.RetrieveStructure();
			PaymentGrdSetting();
			GenerateCode();
			CashAccountFill();
			AccountsFill();
			btnUpdate.Visible = false;
			btnUpdateCreditRow.Visible = false;
			btnCreditRowCancel.Visible = false;
			GetVoucherDateDropdownForHistory();
			btnUpdateDebitRow.Visible = false;
			btnDebitRowCancel.Visible = false;
			cmbCashAccount.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetVoucherDateDropdownForHistory()
	{
		try
		{
			DataTable dt = CommonServices.GetVoucherDateForDaybookDropdown(9);
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

	private void GetSubsidoryAccounts()
	{
		try
		{
			dtSubsidory = global::Architecture.BLL.Accounts.VoucherHead.BindSubsidiaryAccount(UserAccount.CompanyId, 0, (string)null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindSubsidiaryAccountPayment()
	{
		try
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
						if (((Control)(object)CmbSubsidiaryAccountPayment).Text != string.Empty)
						{
							SubsidiaryIdInCaseOfUpdateDetail = Conversion.ToInt(CmbSubsidiaryAccountPayment.Value);
						}
						if (matchingTable.Rows.Count > 0)
						{
							SubsdiaryAccountDatasourceLengthPayment = matchingTable.Rows.Count;
							DDL.BindDDL(matchingTable, CmbSubsidiaryAccountPayment, "Id", "SubsidiaryAccount", "Subsidiary Account", ZeroIndex: false);
							((UltraGridBase)CmbSubsidiaryAccountPayment).DisplayLayout.Bands[0].Columns[2].Hidden = true;
							((UltraGridBase)CmbSubsidiaryAccountPayment).DisplayLayout.Bands[0].Columns[3].Hidden = true;
							((UltraGridBase)CmbSubsidiaryAccountPayment).DisplayLayout.Bands[0].Columns[4].Hidden = true;
							((Control)(object)CmbSubsidiaryAccountPayment).Text = string.Empty;
							if (matchingTable.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == SubsidiaryIdInCaseOfUpdateDetail))
							{
								CmbSubsidiaryAccountPayment.Value = SubsidiaryIdInCaseOfUpdateDetail;
							}
							else
							{
								((Control)(object)CmbSubsidiaryAccountPayment).Text = string.Empty;
							}
						}
						else
						{
							((UltraGridBase)CmbSubsidiaryAccountPayment).DataSource = null;
							((Control)(object)CmbSubsidiaryAccountPayment).Text = string.Empty;
							SubsdiaryAccountDatasourceLengthPayment = 0;
						}
					}
					else
					{
						((UltraGridBase)CmbSubsidiaryAccountPayment).DataSource = null;
						((Control)(object)CmbSubsidiaryAccountPayment).Text = string.Empty;
						SubsdiaryAccountDatasourceLengthPayment = 0;
					}
				}
				else
				{
					SubsdiaryAccountDatasourceLengthPayment = dtSubsidory.Rows.Count;
					DDL.BindDDL(dtSubsidory, CmbSubsidiaryAccountPayment, "Id", "SubsidiaryAccount", "Subsidiary Account", ZeroIndex: false);
					((UltraGridBase)CmbSubsidiaryAccountPayment).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)CmbSubsidiaryAccountPayment).DisplayLayout.Bands[0].Columns[3].Hidden = true;
					((UltraGridBase)CmbSubsidiaryAccountPayment).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				}
			}
			else
			{
				((UltraGridBase)CmbSubsidiaryAccountPayment).DataSource = null;
				((Control)(object)CmbSubsidiaryAccountPayment).Text = string.Empty;
				SubsdiaryAccountDatasourceLengthPayment = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindSubsidiaryAccountReceipt()
	{
		try
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
						if (((Control)(object)CmbSubsidiaryAccountReceipt).Text != string.Empty)
						{
							SubsidiaryIdInCaseOfUpdateDetail = Conversion.ToInt(CmbSubsidiaryAccountReceipt.Value);
						}
						if (matchingTable.Rows.Count > 0)
						{
							SubsdiaryAccountDatasourceLengthReceipt = matchingTable.Rows.Count;
							DDL.BindDDL(matchingTable, CmbSubsidiaryAccountReceipt, "Id", "SubsidiaryAccount", "Subsidiary Account", ZeroIndex: false);
							((UltraGridBase)CmbSubsidiaryAccountReceipt).DisplayLayout.Bands[0].Columns[2].Hidden = true;
							((UltraGridBase)CmbSubsidiaryAccountReceipt).DisplayLayout.Bands[0].Columns[3].Hidden = true;
							((UltraGridBase)CmbSubsidiaryAccountReceipt).DisplayLayout.Bands[0].Columns[4].Hidden = true;
							((Control)(object)CmbSubsidiaryAccountReceipt).Text = string.Empty;
							if (matchingTable.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == SubsidiaryIdInCaseOfUpdateDetail))
							{
								CmbSubsidiaryAccountReceipt.Value = SubsidiaryIdInCaseOfUpdateDetail;
							}
							else
							{
								((Control)(object)CmbSubsidiaryAccountReceipt).Text = string.Empty;
							}
						}
						else
						{
							((UltraGridBase)CmbSubsidiaryAccountReceipt).DataSource = null;
							((Control)(object)CmbSubsidiaryAccountReceipt).Text = string.Empty;
							SubsdiaryAccountDatasourceLengthReceipt = 0;
						}
					}
					else
					{
						((UltraGridBase)CmbSubsidiaryAccountReceipt).DataSource = null;
						((Control)(object)CmbSubsidiaryAccountReceipt).Text = string.Empty;
						SubsdiaryAccountDatasourceLengthReceipt = 0;
					}
				}
				else
				{
					SubsdiaryAccountDatasourceLengthReceipt = dtSubsidory.Rows.Count;
					DDL.BindDDL(dtSubsidory, CmbSubsidiaryAccountReceipt, "Id", "SubsidiaryAccount", "Subsidiary Account", ZeroIndex: false);
					((UltraGridBase)CmbSubsidiaryAccountReceipt).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)CmbSubsidiaryAccountReceipt).DisplayLayout.Bands[0].Columns[3].Hidden = true;
					((UltraGridBase)CmbSubsidiaryAccountReceipt).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				}
			}
			else
			{
				((UltraGridBase)CmbSubsidiaryAccountReceipt).DataSource = null;
				((Control)(object)CmbSubsidiaryAccountReceipt).Text = string.Empty;
				SubsdiaryAccountDatasourceLengthReceipt = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			DataTable dt = global::Architecture.BLL.Accounts.VoucherHead.GenerateVoucherCodeByDocumentTypeId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 9
			});
			if (dt.Rows.Count > 0)
			{
				txtDocNo.Text = Conversion.ToString(dt.Rows[0]["VoucherCode"]);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			throw new Exception(ex.Message);
		}
	}

	private void AccountsFill()
	{
		try
		{
			int DrAccountId = Conversion.ToInt(CmbDebitAccount.Value);
			int CrAccountId = Conversion.ToInt(CmbCreditAccount.Value);
			int[] baseExclude = (InventoryRelatedAccountsShowInVouchers ? new int[1] : new int[2] { 4, 12 });
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(null, baseExclude, null, null, null, null, InventoryRelatedAccountsShowInVouchers ? null : new int[1] { 2 });
			InfragisticsHelper.BindAndRetainSelection(CmbDebitAccount, dt, "Id", "AccountTitle", "Debit Account", AllColumns: true, DrAccountId, insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbCreditAccount, dt, "Id", "AccountTitle", "Credit Account", AllColumns: true, CrAccountId, insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private bool FormValidationCreditDetail()
	{
		if (((UltraGridBase)CmbCreditAccount).ActiveRow == null)
		{
			MessageBox.Show("Credit Account Field Required");
			CmbCreditAccount.Focus();
			return false;
		}
		if (SubsidiaryAccountAllownOnVouchers && SubsdiaryAccountDatasourceLengthReceipt > 0 && ((UltraGridBase)CmbSubsidiaryAccountReceipt).ActiveRow == null)
		{
			MessageBox.Show("Subsidiary Account Field is Required");
			CmbSubsidiaryAccountReceipt.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtCreditAmount.Text) == 0.0)
		{
			MessageBox.Show("Amount Field Is Required");
			txtCreditAmount.Focus();
			return false;
		}
		if (txtCrRemarks.Text == string.Empty)
		{
			MessageBox.Show("Remarks Field Is Required");
			txtCrRemarks.Focus();
			return false;
		}
		return true;
	}

	private void btnAddCrditRow_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationCreditDetail())
			{
				return;
			}
			int LineId = 1;
			if (dtCredit.Rows.Count > 0)
			{
				LineId = Conversion.ToInt((object)ReceiptGrid.GetRows().Max((GridEXRow o) => Conversion.ToInt(o.Cells["LineId"].Value)));
				LineId++;
			}
			dtCredit.Rows.Add(CmbCreditAccount.Value, ((Control)(object)CmbCreditAccount).Text, (!SubsidiaryAccountAllownOnVouchers) ? ((object)0) : ((SubsdiaryAccountDatasourceLengthReceipt > 0) ? CmbSubsidiaryAccountReceipt.Value : CmbCreditAccount.Value), (!SubsidiaryAccountAllownOnVouchers) ? "" : ((SubsdiaryAccountDatasourceLengthReceipt > 0) ? ((Control)(object)CmbSubsidiaryAccountReceipt).Text : ((Control)(object)CmbCreditAccount).Text), (!((Control)(object)CmbSubsidiaryAccountReceipt).Visible) ? ((object)0) : ((SubsdiaryAccountDatasourceLengthReceipt > 0) ? ((UltraDropDownBase)CmbSubsidiaryAccountReceipt).SelectedRow.Cells[2].Value : ((object)4)), txtCreditAmount.Text, Conversion.ToInt((object)txtCrPageNo.Text), datCrChequeDate.Value.ToString("dd-MMM-yy"), txtCrCheqRef.Text, txtCrRemarks.Text, LineId, 0);
			ReceiptGrid.DataSource = dtCredit;
			ReceiptGrid.RetrieveStructure();
			ReceiptGrdSetting();
			txtCreditAmount.Text = string.Empty;
			((UltraGridBase)CmbSubsidiaryAccountReceipt).DataSource = null;
			((Control)(object)CmbSubsidiaryAccountReceipt).Text = string.Empty;
			txtCrRemarks.Text = string.Empty;
			txtCrCheqRef.Text = string.Empty;
			CmbCreditAccount.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReceiptGrdSetting()
	{
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReceiptGrid.RootTable.Columns["AccountId"].Visible = false;
			ReceiptGrid.RootTable.Columns["LineId"].Visible = false;
			ReceiptGrid.RootTable.Columns["SubsidiaryAccountId"].Visible = false;
			ReceiptGrid.RootTable.Columns["SubsidiaryAccountTypeId"].Visible = false;
			ReceiptGrid.RootTable.Columns["SubsidiaryAccount"].Visible = SubsidiaryAccountAllownOnVouchers;
			ReceiptGrid.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)ReceiptGrid.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			ReceiptGrid.RootTable.Columns["CreditAccount"].Width = 140;
			ReceiptGrid.RootTable.Columns["SubsidiaryAccount"].Width = 180;
			ReceiptGrid.RootTable.Columns["Remarks"].Width = 150;
			ReceiptGrid.RootTable.Columns["Amount"].Width = 80;
			ReceiptGrid.RootTable.Columns["PageNo"].Width = 60;
			ReceiptGrid.RootTable.Columns["ChequeDate"].Width = 75;
			ReceiptGrid.RootTable.Columns["ChequeRef"].Width = 80;
			ReceiptGrid.RootTable.Columns["Attachments"].Width = 80;
			ReceiptGrid.RootTable.Columns["Attachments"].ColumnType = (ColumnType)5;
			ReceiptGrid.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			ReceiptGrid.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			ReceiptGrid.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			ReceiptGrid.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			ReceiptGrid.RootTable.Columns.Add("Delete");
			ReceiptGrid.RootTable.Columns["Delete"].Key = "Delete";
			ReceiptGrid.RootTable.Columns["Delete"].Caption = "X";
			ReceiptGrid.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			ReceiptGrid.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			ReceiptGrid.RootTable.Columns["Delete"].ButtonText = "X";
			ReceiptGrid.RootTable.Columns["Delete"].Width = 20;
			ReceiptGrid.RootTable.Columns["Delete"].Position = 0;
			ReceiptGrid.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtCreditAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtCreditAmount_Leave(object sender, EventArgs e)
	{
		try
		{
			txtCreditAmount.Text = Conversion.ToDouble((object)txtCreditAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)ReceiptGrid).Name);
			if (fs != null)
			{
				ReceiptGrid.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReceiptGrid_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = ReceiptGrid.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				UpdateDetailIndexCredit = row.RowIndex;
				CmbCreditAccount.Value = Conversion.ToInt(row.Cells["AccountId"].Value);
				CmbCreditAccount_Leave(null, null);
				if (SubsdiaryAccountDatasourceLengthReceipt == 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id");
					dt.Columns.Add("SubsidiaryAccount");
					dt.Columns.Add("TypeId");
					dt.Columns.Add("AccountId");
					dt.Columns.Add("AccountTitle");
					dt.Rows.Add(Conversion.ToInt(row.Cells["SubsidiaryAccountId"].Value), row.Cells["SubsidiaryAccount"].Text, 4, Conversion.ToInt(row.Cells["AccountId"].Value), row.Cells["CreditAccount"].Text);
					DDL.BindDDL(dt, CmbSubsidiaryAccountReceipt, "Id", "SubsidiaryAccount", "Subsidiary Account ", ZeroIndex: false);
					((UltraGridBase)CmbSubsidiaryAccountReceipt).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
				CmbSubsidiaryAccountReceipt.Value = Conversion.ToInt(row.Cells["SubsidiaryAccountId"].Value);
				txtCreditAmount.Text = Conversion.ToDouble(row.Cells["Amount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				txtCrPageNo.Text = Conversion.ToString(row.Cells["PageNo"].Value);
				datCrChequeDate.Value = Conversion.ToDateTime(row.Cells["ChequeDate"].Value);
				txtCrCheqRef.Text = Conversion.ToString(row.Cells["ChequeRef"].Value);
				txtCrRemarks.Text = Conversion.ToString(row.Cells["Remarks"].Value);
				btnAddCrditRow.Visible = false;
				btnUpdateCreditRow.Visible = true;
				btnCreditRowCancel.Visible = true;
				CmbCreditAccount.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdateCreditRow_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationCreditDetail())
			{
				return;
			}
			dtCredit.Rows[UpdateDetailIndexCredit]["AccountId"] = CmbCreditAccount.Value;
			dtCredit.Rows[UpdateDetailIndexCredit]["CreditAccount"] = ((Control)(object)CmbCreditAccount).Text;
			if (((Control)(object)CmbSubsidiaryAccountReceipt).Visible)
			{
				if (((UltraGridBase)CmbSubsidiaryAccountReceipt).ActiveRow != null && ((Control)(object)CmbSubsidiaryAccountReceipt).Text != string.Empty)
				{
					dtCredit.Rows[UpdateDetailIndexCredit]["SubsidiaryAccountId"] = CmbSubsidiaryAccountReceipt.Value;
					dtCredit.Rows[UpdateDetailIndexCredit]["SubsidiaryAccount"] = ((Control)(object)CmbSubsidiaryAccountReceipt).Text;
					dtCredit.Rows[UpdateDetailIndexCredit]["SubsidiaryAccountTypeId"] = ((UltraDropDownBase)CmbSubsidiaryAccountReceipt).SelectedRow.Cells[2].Value;
				}
				else
				{
					dtCredit.Rows[UpdateDetailIndexCredit]["SubsidiaryAccountId"] = CmbCreditAccount.Value;
					dtCredit.Rows[UpdateDetailIndexCredit]["SubsidiaryAccount"] = ((Control)(object)CmbCreditAccount).Text;
					dtCredit.Rows[UpdateDetailIndexCredit]["SubsidiaryAccountTypeId"] = 4;
				}
			}
			else
			{
				dtCredit.Rows[UpdateDetailIndexCredit]["SubsidiaryAccountId"] = 0;
				dtCredit.Rows[UpdateDetailIndexCredit]["SubsidiaryAccount"] = "";
				dtCredit.Rows[UpdateDetailIndexCredit]["SubsidiaryAccountTypeId"] = 0;
			}
			dtCredit.Rows[UpdateDetailIndexCredit]["Amount"] = txtCreditAmount.Text.Trim();
			dtCredit.Rows[UpdateDetailIndexCredit]["PageNo"] = Conversion.ToInt((object)txtCrPageNo.Text.Trim());
			dtCredit.Rows[UpdateDetailIndexCredit]["ChequeDate"] = datCrChequeDate.Value.ToString("dd-MMM-yy");
			dtCredit.Rows[UpdateDetailIndexCredit]["ChequeRef"] = txtCrCheqRef.Text.Trim();
			dtCredit.Rows[UpdateDetailIndexCredit]["Remarks"] = txtCrRemarks.Text.Trim();
			((Control)(object)CmbCreditAccount).Text = string.Empty;
			txtCrRemarks.Clear();
			txtCreditAmount.Clear();
			CmbCreditAccount.Focus();
			txtCrCheqRef.Clear();
			txtCrPageNo.Clear();
			((UltraGridBase)CmbSubsidiaryAccountReceipt).DataSource = null;
			((Control)(object)CmbSubsidiaryAccountReceipt).Text = string.Empty;
			btnAddCrditRow.Visible = true;
			btnUpdateCreditRow.Visible = false;
			btnCreditRowCancel.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbCreditAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbCreditAccount).ActiveRow != null)
			{
				if (Conversion.ToInt(CmbCreditAccount.Value) == Conversion.ToInt(cmbCashAccount.Value))
				{
					((Control)(object)CmbCreditAccount).Text = string.Empty;
					CmbCreditAccount.Focus();
					throw new Exception("Cash Account And Detail Account Can't be Same! Please Select Other Account");
				}
				double balance = CommonServices.GetCurrentGlBalance(Conversion.ToInt(CmbCreditAccount.Value), Conversion.ToDateTime((object)txtDocDate.Text));
				lblCreditAccountBalance.Text = balance.ToString(clsGlobalVariables.stringFormatboth);
				if (balance > 0.0)
				{
					lblCreditAccountBalanceCaption.Text = "Dr";
				}
				else if (balance < 0.0)
				{
					lblCreditAccountBalanceCaption.Text = "Cr";
				}
				else if (balance == 0.0)
				{
					lblCreditAccountBalanceCaption.Text = "Nill";
				}
			}
			else
			{
				lblCreditAccountBalance.Text = "0";
				lblDebitAccountBalanceCaption.Text = "";
			}
			BindSubsidiaryAccountReceipt();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReceiptGrid_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = ReceiptGrid.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					ATd.DeleteAttachments(Conversion.ToInt(item.Cells["LineId"].Value));
					item.Delete();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnCreditRowCancel_Click(object sender, EventArgs e)
	{
		try
		{
			btnAddCrditRow.Visible = true;
			btnCreditRowCancel.Visible = false;
			btnUpdateCreditRow.Visible = false;
			txtCreditAmount.Text = string.Empty;
			txtCrRemarks.Text = string.Empty;
			txtCrCheqRef.Text = string.Empty;
			((UltraGridBase)CmbSubsidiaryAccountReceipt).DataSource = null;
			((Control)(object)CmbSubsidiaryAccountReceipt).Text = string.Empty;
			CmbCreditAccount.Focus();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)PaymentGrid).Name);
			if (fs != null)
			{
				PaymentGrid.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDebitAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbDebitAccount).ActiveRow != null)
			{
				if (Conversion.ToInt(CmbDebitAccount.Value) == Conversion.ToInt(cmbCashAccount.Value))
				{
					((Control)(object)CmbDebitAccount).Text = string.Empty;
					CmbDebitAccount.Focus();
					throw new Exception("Cash Account And Detail Account Can't be Same! Please Select Other Account");
				}
				double balance = CommonServices.GetCurrentGlBalance(Conversion.ToInt(CmbDebitAccount.Value), Conversion.ToDateTime((object)txtDocDate.Text));
				lblDebitAccountBalance.Text = balance.ToString(clsGlobalVariables.stringFormatboth);
				if (balance > 0.0)
				{
					lblDebitAccountBalanceCaption.Text = "Dr";
				}
				else if (balance < 0.0)
				{
					lblDebitAccountBalanceCaption.Text = "Cr";
				}
				else if (balance == 0.0)
				{
					lblDebitAccountBalanceCaption.Text = "Nill";
				}
			}
			else
			{
				lblDebitAccountBalance.Text = "0";
				lblDebitAccountBalanceCaption.Text = "";
			}
			BindSubsidiaryAccountPayment();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private bool FormValidationDebitDetail()
	{
		if (((UltraGridBase)CmbDebitAccount).ActiveRow == null)
		{
			MessageBox.Show("Debit Account Field is Required");
			CmbDebitAccount.Focus();
			return false;
		}
		if (SubsidiaryAccountAllownOnVouchers && SubsdiaryAccountDatasourceLengthPayment > 0 && ((UltraGridBase)CmbSubsidiaryAccountPayment).ActiveRow == null)
		{
			MessageBox.Show("Subsidiary Account Field is Required");
			CmbSubsidiaryAccountPayment.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtDebitAmount.Text) == 0.0)
		{
			MessageBox.Show("Amount Field is Required");
			txtDebitAmount.Focus();
			return false;
		}
		if (txtDrRemarks.Text == string.Empty)
		{
			MessageBox.Show("Remarks Field is Required");
			txtDrRemarks.Focus();
			return false;
		}
		return true;
	}

	private void btnAddDebitRow_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDebitDetail())
			{
				return;
			}
			int LineId = 1;
			if (dtDedit.Rows.Count > 0)
			{
				LineId = Conversion.ToInt((object)PaymentGrid.GetRows().Max((GridEXRow o) => Conversion.ToInt(o.Cells["LineId"].Value)));
				LineId++;
			}
			dtDedit.Rows.Add(CmbDebitAccount.Value, ((Control)(object)CmbDebitAccount).Text, (!SubsidiaryAccountAllownOnVouchers) ? ((object)0) : ((SubsdiaryAccountDatasourceLengthPayment > 0) ? CmbSubsidiaryAccountPayment.Value : CmbDebitAccount.Value), (!SubsidiaryAccountAllownOnVouchers) ? "" : ((SubsdiaryAccountDatasourceLengthPayment > 0) ? ((Control)(object)CmbSubsidiaryAccountPayment).Text : ((Control)(object)CmbDebitAccount).Text), (!((Control)(object)CmbSubsidiaryAccountPayment).Visible) ? ((object)0) : ((SubsdiaryAccountDatasourceLengthPayment > 0) ? ((UltraDropDownBase)CmbSubsidiaryAccountPayment).SelectedRow.Cells[2].Value : ((object)4)), txtDebitAmount.Text, Conversion.ToInt((object)txtDrPageNo.Text), datDrCheqDate.Value.ToString("dd-MMM-yy"), txtDrCheqRef.Text, txtDrRemarks.Text, LineId, 0);
			PaymentGrid.DataSource = dtDedit;
			PaymentGrid.RetrieveStructure();
			PaymentGrdSetting();
			txtDebitAmount.Text = string.Empty;
			txtDrRemarks.Text = string.Empty;
			((UltraGridBase)CmbSubsidiaryAccountPayment).DataSource = null;
			((Control)(object)CmbSubsidiaryAccountPayment).Text = string.Empty;
			txtDrCheqRef.Clear();
			CmbDebitAccount.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PaymentGrdSetting()
	{
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			PaymentGrid.RootTable.Columns["AccountId"].Visible = false;
			PaymentGrid.RootTable.Columns["LineId"].Visible = false;
			PaymentGrid.RootTable.Columns["SubsidiaryAccountId"].Visible = false;
			PaymentGrid.RootTable.Columns["SubsidiaryAccountTypeId"].Visible = false;
			PaymentGrid.RootTable.Columns["SubsidiaryAccount"].Visible = SubsidiaryAccountAllownOnVouchers;
			PaymentGrid.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)PaymentGrid.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			PaymentGrid.RootTable.Columns["SubsidiaryAccount"].Width = 180;
			PaymentGrid.RootTable.Columns["DebitAccount"].Width = 150;
			PaymentGrid.RootTable.Columns["Amount"].Width = 80;
			PaymentGrid.RootTable.Columns["Remarks"].Width = 150;
			PaymentGrid.RootTable.Columns["PageNo"].Width = 60;
			PaymentGrid.RootTable.Columns["ChequeDate"].Width = 75;
			PaymentGrid.RootTable.Columns["ChequeRef"].Width = 80;
			PaymentGrid.RootTable.Columns["Attachments"].Width = 80;
			PaymentGrid.RootTable.Columns["Attachments"].ColumnType = (ColumnType)5;
			PaymentGrid.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			PaymentGrid.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			PaymentGrid.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			PaymentGrid.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			PaymentGrid.RootTable.Columns.Add("Delete");
			PaymentGrid.RootTable.Columns["Delete"].Key = "Delete";
			PaymentGrid.RootTable.Columns["Delete"].Caption = "X";
			PaymentGrid.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			PaymentGrid.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			PaymentGrid.RootTable.Columns["Delete"].ButtonText = "X";
			PaymentGrid.RootTable.Columns["Delete"].Width = 20;
			PaymentGrid.RootTable.Columns["Delete"].Position = 0;
			PaymentGrid.FrozenColumns = 1;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnUpdateDebitRow_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDebitDetail())
			{
				return;
			}
			dtDedit.Rows[UpdateDetailIndexDedit]["AccountId"] = CmbDebitAccount.Value;
			dtDedit.Rows[UpdateDetailIndexDedit]["DebitAccount"] = ((Control)(object)CmbDebitAccount).Text;
			if (((Control)(object)CmbSubsidiaryAccountPayment).Visible)
			{
				if (((UltraGridBase)CmbSubsidiaryAccountPayment).ActiveRow != null && ((Control)(object)CmbSubsidiaryAccountPayment).Text != string.Empty)
				{
					dtDedit.Rows[UpdateDetailIndexDedit]["SubsidiaryAccountId"] = CmbSubsidiaryAccountPayment.Value;
					dtDedit.Rows[UpdateDetailIndexDedit]["SubsidiaryAccount"] = ((Control)(object)CmbSubsidiaryAccountPayment).Text;
					dtDedit.Rows[UpdateDetailIndexDedit]["SubsidiaryAccountTypeId"] = ((UltraDropDownBase)CmbSubsidiaryAccountPayment).SelectedRow.Cells[2].Value;
				}
				else
				{
					dtDedit.Rows[UpdateDetailIndexDedit]["SubsidiaryAccountId"] = CmbDebitAccount.Value;
					dtDedit.Rows[UpdateDetailIndexDedit]["SubsidiaryAccount"] = ((Control)(object)CmbDebitAccount).Text;
					dtDedit.Rows[UpdateDetailIndexDedit]["SubsidiaryAccountTypeId"] = 4;
				}
			}
			else
			{
				dtDedit.Rows[UpdateDetailIndexDedit]["SubsidiaryAccountId"] = 0;
				dtDedit.Rows[UpdateDetailIndexDedit]["SubsidiaryAccount"] = "";
				dtDedit.Rows[UpdateDetailIndexDedit]["SubsidiaryAccountTypeId"] = 0;
			}
			dtDedit.Rows[UpdateDetailIndexDedit]["Amount"] = txtDebitAmount.Text.Trim();
			dtDedit.Rows[UpdateDetailIndexDedit]["PageNo"] = Conversion.ToInt((object)txtDrPageNo.Text.Trim());
			dtDedit.Rows[UpdateDetailIndexDedit]["ChequeDate"] = datDrCheqDate.Value.ToString("dd-MMM-yy");
			dtDedit.Rows[UpdateDetailIndexDedit]["ChequeRef"] = txtDrCheqRef.Text.Trim();
			dtDedit.Rows[UpdateDetailIndexDedit]["Remarks"] = txtDrRemarks.Text.Trim();
			((Control)(object)CmbDebitAccount).Text = string.Empty;
			txtDrRemarks.Clear();
			txtDebitAmount.Clear();
			txtDrCheqRef.Clear();
			CmbDebitAccount.Focus();
			((UltraGridBase)CmbSubsidiaryAccountPayment).DataSource = null;
			((Control)(object)CmbSubsidiaryAccountPayment).Text = string.Empty;
			btnAddDebitRow.Visible = true;
			btnUpdateDebitRow.Visible = false;
			btnDebitRowCancel.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PaymentGrid_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = PaymentGrid.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				UpdateDetailIndexDedit = row.RowIndex;
				CmbDebitAccount.Value = Conversion.ToInt(row.Cells["AccountId"].Value);
				CmbDebitAccount_Leave(null, null);
				if (SubsdiaryAccountDatasourceLengthPayment == 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id");
					dt.Columns.Add("SubsidiaryAccount");
					dt.Columns.Add("TypeId");
					dt.Columns.Add("AccountId");
					dt.Columns.Add("AccountTitle");
					dt.Rows.Add(Conversion.ToInt(row.Cells["SubsidiaryAccountId"].Value), row.Cells["SubsidiaryAccount"].Text, 4, Conversion.ToInt(row.Cells["AccountId"].Value), row.Cells["DebitAccount"].Text);
					DDL.BindDDL(dt, CmbSubsidiaryAccountPayment, "Id", "SubsidiaryAccount", "Subsidiary Account ", ZeroIndex: false);
					((UltraGridBase)CmbSubsidiaryAccountPayment).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
				CmbSubsidiaryAccountPayment.Value = Conversion.ToInt(row.Cells["SubsidiaryAccountId"].Value);
				txtDebitAmount.Text = Conversion.ToDouble(row.Cells["Amount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				txtDrPageNo.Text = Conversion.ToString(row.Cells["PageNo"].Value);
				datDrCheqDate.Value = Conversion.ToDateTime(row.Cells["ChequeDate"].Value);
				txtDrCheqRef.Text = Conversion.ToString(row.Cells["ChequeRef"].Value);
				txtDrRemarks.Text = Conversion.ToString(row.Cells["Remarks"].Value);
				btnAddDebitRow.Visible = false;
				btnUpdateDebitRow.Visible = true;
				btnDebitRowCancel.Visible = true;
				CmbDebitAccount.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PaymentGrid_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = PaymentGrid.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					ATdDebit.DeleteAttachments(Conversion.ToInt(item.Cells["LineId"].Value));
					item.Delete();
				}
			}
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtDebitAmount_Leave(object sender, EventArgs e)
	{
		try
		{
			txtDebitAmount.Text = Conversion.ToDouble((object)txtDebitAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnDebitRowCancel_Click(object sender, EventArgs e)
	{
		try
		{
			btnAddDebitRow.Visible = true;
			btnDebitRowCancel.Visible = false;
			btnUpdateDebitRow.Visible = false;
			((UltraGridBase)CmbSubsidiaryAccountPayment).DataSource = null;
			((Control)(object)CmbSubsidiaryAccountPayment).Text = string.Empty;
			txtDebitAmount.Text = string.Empty;
			txtDrRemarks.Text = string.Empty;
			CmbDebitAccount.Focus();
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
			Reset();
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
			ATd = new AttachmentForDetailRows();
			ATdDebit = new AttachmentForDetailRows();
			CustomAttachmentsValues = "";
			RecId = 0;
			UpdateDetailIndexCredit = -1;
			UpdateDetailIndexDedit = -1;
			dtCredit.Rows.Clear();
			ReceiptGrid.DataSource = dtCredit;
			dtDedit.Rows.Clear();
			PaymentGrid.DataSource = dtDedit;
			GenerateCode();
			((Control)(object)CmbCreditAccount).Text = string.Empty;
			txtCreditAmount.Text = string.Empty;
			((Control)(object)CmbDebitAccount).Text = string.Empty;
			txtDebitAmount.Text = string.Empty;
			cmbCashAccount.Focus();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			InventoryRelatedAccountsShowInVouchers = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("InventoryRelatedAccountsShowInVouchers"));
			CashAccountFill();
			AccountsFill();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				GetSubsidoryAccounts();
				BindSubsidiaryAccountPayment();
				BindSubsidiaryAccountReceipt();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbCashAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbCashAccount).ActiveRow != null)
			{
				if (ReceiptGrid.GetRows().Count() > 0)
				{
					GridEXRow[] rows = ReceiptGrid.GetRows();
					for (int i = 0; i < rows.Length; i++)
					{
						if (Conversion.ToInt(rows[i].Cells["AccountId"].Value) == Conversion.ToInt(cmbCashAccount.Value))
						{
							((Control)(object)cmbCashAccount).Text = string.Empty;
							throw new Exception("You Can't Select This Cash Account Because Receipt Grid Has Entry Against This Account");
						}
					}
				}
				if (PaymentGrid.GetRows().Count() > 0)
				{
					GridEXRow[] rows = PaymentGrid.GetRows();
					for (int i = 0; i < rows.Length; i++)
					{
						if (Conversion.ToInt(rows[i].Cells["AccountId"].Value) == Conversion.ToInt(cmbCashAccount.Value))
						{
							((Control)(object)cmbCashAccount).Text = string.Empty;
							throw new Exception("You Can't Select This Cash Account Because Payment Grid Has Entry Against This Account");
						}
					}
				}
				double op = CommonServices.GetCurrentGlBalance(Conversion.ToInt(cmbCashAccount.Value), Conversion.ToDateTime((object)txtDocDate.Text).AddDays(-1.0));
				lblOpeningBalance.Text = op.ToString(clsGlobalVariables.stringFormatboth);
				if (op > 0.0)
				{
					lblCreditOrDebit.Text = "Dr";
				}
				else if (op < 0.0)
				{
					lblCreditOrDebit.Text = "Cr";
				}
				else if (op == 0.0)
				{
					lblCreditOrDebit.Text = "Nill";
				}
				double closing = CommonServices.GetCurrentGlBalance(Conversion.ToInt(cmbCashAccount.Value), Conversion.ToDateTime((object)txtDocDate.Text));
				lblClosingBalance.Text = closing.ToString(clsGlobalVariables.stringFormatboth);
				if (closing > 0.0)
				{
					lblCreditOrDebitForClosing.Text = "Dr";
				}
				else if (closing < 0.0)
				{
					lblCreditOrDebitForClosing.Text = "Cr";
				}
				else if (closing == 0.0)
				{
					lblCreditOrDebitForClosing.Text = "Nill";
				}
			}
			else
			{
				lblOpeningBalance.Text = "0";
				lblCreditOrDebit.Text = "";
				lblClosingBalance.Text = "0";
				lblCreditOrDebitForClosing.Text = "";
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private bool FormValidation()
	{
		if (Conversion.ToInt((object)txtDocNo.Text) == 0)
		{
			MessageBox.Show("Doc No Field is Required");
			txtDocNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCashAccount).ActiveRow == null)
		{
			MessageBox.Show("cash Account Field is Required");
			cmbCashAccount.Focus();
			return false;
		}
		return true;
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Expected O, but got Unknown
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Expected O, but got Unknown
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Expected O, but got Unknown
		//IL_070f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Expected O, but got Unknown
		//IL_07dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Expected O, but got Unknown
		//IL_08bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c2: Expected O, but got Unknown
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		//IL_0938: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (ReceiptGrid.GetRows().Count() == 0 && PaymentGrid.GetRows().Count() == 0)
			{
				throw new Exception("Add AtLeast One Row In Either Payment Side Or Receipt Side");
			}
			VoucherHead Vh = new VoucherHead();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				Vh.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			Vh.DocumentTypeId = 9;
			Vh.VoucherDate = txtDocDate.Value;
			Vh.OrganizationId = UserAccount.OrganizationId;
			Vh.CompanyId = UserAccount.CompanyId;
			Vh.BranchId = UserAccount.BranchesId;
			Vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Vh.EntryUser = UserAccount.ID;
			Vh.EntryDate = DateTime.Now;
			Vh.ModifyUser = UserAccount.ID;
			Vh.ModifyDate = DateTime.Now;
			Vh.VoucherCode = Conversion.ToInt((object)txtDocNo.Text);
			Vh.Remarks = Conversion.ToString((object)txtHeaderRemarks.Text);
			Vh.RefAccountId = Conversion.ToInt(cmbCashAccount.Value);
			Vh.AgainstAccountId = Conversion.ToInt(cmbCashAccount.Value);
			_ = Vh.RefAccountId;
			double CreditAmount = 0.0;
			double DebitAmount = 0.0;
			Vh.voucherDetailList = new List<VoucherDetail>();
			GridEXRow[] rows = ReceiptGrid.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				VoucherDetail vd2 = new VoucherDetail();
				vd2.AccountId = Conversion.ToInt(r2.Cells["AccountId"].Value);
				vd2.SubsidiaryTypeId = Conversion.ToInt(r2.Cells["SubsidiaryAccountTypeId"].Value);
				int SubsidiaryAccountId2 = Conversion.ToInt(r2.Cells["SubsidiaryAccountId"].Value);
				if (SubsidiaryAccountId2 > 0)
				{
					vd2.SupplierCustomerId = ((vd2.SubsidiaryTypeId == 1) ? SubsidiaryAccountId2 : 0);
					vd2.EmployeeId = ((vd2.SubsidiaryTypeId == 2) ? SubsidiaryAccountId2 : 0);
					vd2.SubsidiaryAccountId = SubsidiaryAccountId2;
				}
				vd2.AgainstAccountId = Conversion.ToInt(cmbCashAccount.Value);
				vd2.CreditAmount = Conversion.ToDouble(r2.Cells["Amount"].Value);
				vd2.SortNo = Conversion.ToInt(r2.Cells["PageNo"].Value);
				vd2.DCheqDate = Conversion.ToDateTime(r2.Cells["ChequeDate"].Value);
				vd2.CheqNoDetail = Conversion.ToString(r2.Cells["ChequeRef"].Value);
				vd2.Comments = Conversion.ToString(r2.Cells["Remarks"].Value);
				vd2.LineId = Conversion.ToInt(r2.Cells["LineId"].Value);
				CreditAmount += vd2.CreditAmount;
				Vh.voucherDetailList.Add(vd2);
			}
			rows = PaymentGrid.GetRows();
			foreach (GridEXRow r in rows)
			{
				VoucherDetail vd = new VoucherDetail();
				vd.AccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
				vd.SubsidiaryTypeId = Conversion.ToInt(r.Cells["SubsidiaryAccountTypeId"].Value);
				int SubsidiaryAccountId = Conversion.ToInt(r.Cells["SubsidiaryAccountId"].Value);
				if (SubsidiaryAccountId > 0)
				{
					vd.SupplierCustomerId = ((vd.SubsidiaryTypeId == 1) ? SubsidiaryAccountId : 0);
					vd.EmployeeId = ((vd.SubsidiaryTypeId == 2) ? SubsidiaryAccountId : 0);
					vd.SubsidiaryAccountId = SubsidiaryAccountId;
				}
				vd.AgainstAccountId = Conversion.ToInt(cmbCashAccount.Value);
				vd.DebitAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
				vd.SortNo = Conversion.ToInt(r.Cells["PageNo"].Value);
				vd.DCheqDate = Conversion.ToDateTime(r.Cells["ChequeDate"].Value);
				vd.CheqNoDetail = Conversion.ToString(r.Cells["ChequeRef"].Value);
				vd.Comments = Conversion.ToString(r.Cells["Remarks"].Value);
				vd.LineId = Conversion.ToInt(r.Cells["LineId"].Value);
				DebitAmount += vd.DebitAmount;
				Vh.voucherDetailList.Add(vd);
			}
			VoucherDetail CashAcList = new VoucherDetail();
			CashAcList.AccountId = Conversion.ToInt(cmbCashAccount.Value);
			CashAcList.AgainstAccountId = Conversion.ToInt(cmbCashAccount.Value);
			double difference = DebitAmount - CreditAmount;
			if (Math.Abs(difference) > 0.0)
			{
				if (DebitAmount == 0.0)
				{
					CashAcList.DebitAmount = CreditAmount;
					CashAcList.CreditAmount = 0.0;
				}
				else if (CreditAmount == 0.0)
				{
					CashAcList.CreditAmount = DebitAmount;
					CashAcList.DebitAmount = 0.0;
				}
				else
				{
					CashAcList.CreditAmount = ((difference > 0.0) ? difference : 0.0);
					CashAcList.DebitAmount = ((difference < 0.0) ? (0.0 - difference) : 0.0);
				}
				CashAcList.Comments = ((difference > 0.0) ? ("Cash Account Credit With Difference Of " + difference) : ((difference < 0.0) ? ("Cash Account Debit With Difference Of " + (0.0 - difference)) : ""));
				Vh.voucherDetailList.Add(CashAcList);
			}
			if (string.IsNullOrEmpty(AttachmentsValues) || AT.lst.Count > 0)
			{
				AttachmentsValues = "";
			}
			Vh.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			Vh.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<VoucherHead>(Vh, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			Vh.AttachmentsValues = ((AttachmentsValues != "") ? AttachmentsValues : Vh.AttachmentsValues);
			Vh.CustomAttachmentsValues = CustomAttachmentsValues;
			Vh.DetailAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			for (int j = 0; j < ATd.lst.Count; j++)
			{
				DMSAttachments attach2 = new DMSAttachments();
				attach2.Attachment = ATd.lst[j].Path;
				attach2.UploadedFileCustomName = ATd.lst[j].CustomPath;
				attach2.RefAccountId = ATd.lst[j].RefAccountId;
				attach2.LineId = ATd.lst[j].DetailRefId;
				attach2.DetailWiseAttachment = true;
				attach2.ScreenName = base.Name;
				attach2.DMSFoldersLabelsId = 0;
				Vh.DetailAttachmentsList.Add(attach2);
			}
			for (int i = 0; i < ATdDebit.lst.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = ATdDebit.lst[i].Path;
				attach.UploadedFileCustomName = ATdDebit.lst[i].CustomPath;
				attach.RefAccountId = ATdDebit.lst[i].RefAccountId;
				attach.LineId = ATdDebit.lst[i].DetailRefId;
				attach.DetailWiseAttachment = true;
				attach.ScreenName = base.Name;
				attach.DMSFoldersLabelsId = 1;
				Vh.DetailAttachmentsList.Add(attach);
			}
			if (ATd.RemovedAttachmentListInUpdateCase.Count > 0)
			{
				for (int l = 0; l < ATd.RemovedAttachmentListInUpdateCase.Count; l++)
				{
					DMSAttachments delete2 = new DMSAttachments();
					delete2.Attachment = Path.GetFileName(ATd.RemovedAttachmentListInUpdateCase[l].Path);
					delete2.ScreenName = base.Name;
					Vh.DeleteAttachmentsList.Add(delete2);
				}
			}
			if (ATdDebit.RemovedAttachmentListInUpdateCase.Count > 0)
			{
				for (int k = 0; k < ATdDebit.RemovedAttachmentListInUpdateCase.Count; k++)
				{
					DMSAttachments delete = new DMSAttachments();
					delete.Attachment = Path.GetFileName(ATdDebit.RemovedAttachmentListInUpdateCase[k].Path);
					delete.ScreenName = base.Name;
					Vh.DeleteAttachmentsList.Add(delete);
				}
			}
			int success = global::Architecture.BLL.Accounts.VoucherHead.Save(Vh);
			if (RecId == 0)
			{
				MessageBox.Show("Record Save Successfully...[" + Vh.VoucherCode + "]");
			}
			else
			{
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 9, 1);
				}
				if (ATd.RemovedAttachmentListInUpdateCase.Count > 0 && ATdDebit.RemovedAttachmentListInUpdateCase.Count > 0 && ATd.lst.Count == 0 && ATdDebit.lst.Count == 0)
				{
					global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 9, 2);
				}
				MessageBox.Show("Record Update Successfully...[" + Vh.VoucherCode + "]");
			}
			Reset();
			if (ChkPrintPreview.Checked)
			{
				CommonServices.DayBookSlip(success, 9);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryFill()
	{
		try
		{
			DataTable dt1 = new DataTable();
			dt1.Columns.Add("Id", typeof(int));
			dt1.Columns.Add("VoucherCode");
			dt1.Columns.Add("DetailAccount");
			dt1.Columns.Add("SubsidiaryAccount");
			dt1.Columns.Add("CreditAmount", typeof(double));
			dt1.Columns.Add("CashAccountId", typeof(int));
			dt1.Columns.Add("CashAccount");
			dt1.Columns.Add("Remarks");
			dt1.Columns.Add("PageNo", typeof(int));
			dt1.Columns.Add("ChequeDate", typeof(DateTime));
			dt1.Columns.Add("ChequeRef", typeof(string));
			dt1.Columns.Add("EntryUserName");
			dt1.Columns.Add("EntryDate", typeof(DateTime));
			dt1.Columns.Add("ModifyUserName");
			dt1.Columns.Add("ModifyDate", typeof(DateTime));
			dt1.Columns.Add("NoOfAttachments", typeof(int));
			DataTable dt2 = new DataTable();
			dt2.Columns.Add("Id", typeof(int));
			dt2.Columns.Add("VoucherCode");
			dt2.Columns.Add("DetailAccount");
			dt2.Columns.Add("SubsidiaryAccount");
			dt2.Columns.Add("DebitAmount", typeof(double));
			dt2.Columns.Add("CashAccountId", typeof(int));
			dt2.Columns.Add("CashAccount");
			dt2.Columns.Add("Remarks");
			dt2.Columns.Add("PageNo", typeof(int));
			dt2.Columns.Add("ChequeDate", typeof(DateTime));
			dt2.Columns.Add("ChequeRef", typeof(string));
			dt2.Columns.Add("EntryUserName");
			dt2.Columns.Add("EntryDate", typeof(DateTime));
			dt2.Columns.Add("ModifyUserName");
			dt2.Columns.Add("ModifyDate", typeof(DateTime));
			dt2.Columns.Add("NoOfAttachments", typeof(int));
			DataTable dtHistory = global::Architecture.BLL.Accounts.VoucherHead.DayBookHistory(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, Conversion.ToDateTime((object)((Control)(object)datVouchderDateForHistory).Text), Conversion.ToInt(CmbCashAccountForHistory.Value), 9);
			if (dtHistory.Rows.Count > 0)
			{
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					if (Conversion.ToDouble(dtHistory.Rows[i]["CreditAmount"]) > 0.0)
					{
						dt1.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["VoucherCode"], dtHistory.Rows[i]["DetailAccount"], dtHistory.Rows[i]["SubsidiaryAccountTitle"], dtHistory.Rows[i]["CreditAmount"], dtHistory.Rows[i]["AgainstAccountId"], dtHistory.Rows[i]["CashAccount"], dtHistory.Rows[i]["Comments"], dtHistory.Rows[i]["SortNo"], dtHistory.Rows[i]["DCheqDate"], dtHistory.Rows[i]["CheqNoDetail"], dtHistory.Rows[i]["EntryUserName"], dtHistory.Rows[i]["EntryDate"], dtHistory.Rows[i]["ModifyUserName"], dtHistory.Rows[i]["ModifyDate"], dtHistory.Rows[i]["NoOfAttachments"]);
					}
					else if (Conversion.ToDouble(dtHistory.Rows[i]["DebitAmount"]) > 0.0)
					{
						dt2.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["VoucherCode"], dtHistory.Rows[i]["DetailAccount"], dtHistory.Rows[i]["SubsidiaryAccountTitle"], dtHistory.Rows[i]["DebitAmount"], dtHistory.Rows[i]["AgainstAccountId"], dtHistory.Rows[i]["CashAccount"], dtHistory.Rows[i]["Comments"], dtHistory.Rows[i]["SortNo"], dtHistory.Rows[i]["DCheqDate"], dtHistory.Rows[i]["CheqNoDetail"], dtHistory.Rows[i]["EntryUserName"], dtHistory.Rows[i]["EntryDate"], dtHistory.Rows[i]["ModifyUserName"], dtHistory.Rows[i]["ModifyDate"], dtHistory.Rows[i]["NoOfAttachments"]);
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
			throw new Exception(ex.Message);
		}
	}

	private void DataGridCreditHistorySetting()
	{
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataGridCreditHistory.RootTable.Columns["Id"].Visible = false;
			DataGridCreditHistory.RootTable.Columns["CashAccountId"].Visible = false;
			DataGridCreditHistory.RootTable.Columns["SubsidiaryAccount"].Visible = SubsidiaryAccountAllownOnVouchers;
			DataGridCreditHistory.RootTable.Groups.Add("CashAccount");
			DataGridCreditHistory.HideColumnsWhenGrouped = (InheritableBoolean)1;
			DataGridCreditHistory.RootTable.HeaderLines = 2;
			DataGridCreditHistory.RootTable.GroupTotals = (GroupTotals)2;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridCreditHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			DataGridCreditHistory.RootTable.Columns["CashAccount"].Width = 150;
			DataGridCreditHistory.RootTable.Columns["SubsidiaryAccount"].Width = 180;
			DataGridCreditHistory.RootTable.Columns["VoucherCode"].Width = 65;
			DataGridCreditHistory.RootTable.Columns["DetailAccount"].Width = 150;
			DataGridCreditHistory.RootTable.Columns["Remarks"].Width = 150;
			DataGridCreditHistory.RootTable.Columns["CreditAmount"].Width = 80;
			DataGridCreditHistory.RootTable.Columns["PageNo"].Width = 60;
			DataGridCreditHistory.RootTable.Columns["ChequeDate"].Width = 75;
			DataGridCreditHistory.RootTable.Columns["ChequeRef"].Width = 80;
			DataGridCreditHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			DataGridCreditHistory.RootTable.Columns["EntryUserName"].Width = 120;
			DataGridCreditHistory.RootTable.Columns["ModifyUserName"].Width = 120;
			DataGridCreditHistory.RootTable.Columns["EntryDate"].Width = 145;
			DataGridCreditHistory.RootTable.Columns["ModifyDate"].Width = 145;
			DataGridCreditHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridCreditHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridCreditHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridCreditHistory.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridCreditHistory.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			DataGridCreditHistory.RootTable.Columns["CreditAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridCreditHistory.RootTable.Columns["CreditAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridCreditHistory.RootTable.Columns.Add("Edit");
			DataGridCreditHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridCreditHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridCreditHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridCreditHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridCreditHistory.RootTable.Columns["Edit"].Width = 40;
			DataGridCreditHistory.RootTable.Columns.Add("Print");
			DataGridCreditHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridCreditHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridCreditHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridCreditHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridCreditHistory.RootTable.Columns["Print"].Width = 50;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridDebitHistorySetting()
	{
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataGridDebitHistory.RootTable.Columns["Id"].Visible = false;
			DataGridDebitHistory.RootTable.Columns["CashAccountId"].Visible = false;
			DataGridDebitHistory.RootTable.Columns["SubsidiaryAccount"].Visible = SubsidiaryAccountAllownOnVouchers;
			DataGridDebitHistory.RootTable.Groups.Add("CashAccount");
			DataGridDebitHistory.HideColumnsWhenGrouped = (InheritableBoolean)1;
			DataGridDebitHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridDebitHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			DataGridDebitHistory.RootTable.Columns["SubsidiaryAccount"].Width = 180;
			DataGridDebitHistory.RootTable.Columns["CashAccount"].Width = 150;
			DataGridDebitHistory.RootTable.Columns["VoucherCode"].Width = 65;
			DataGridDebitHistory.RootTable.Columns["DetailAccount"].Width = 150;
			DataGridDebitHistory.RootTable.Columns["Remarks"].Width = 150;
			DataGridDebitHistory.RootTable.Columns["DebitAmount"].Width = 80;
			DataGridDebitHistory.RootTable.Columns["PageNo"].Width = 60;
			DataGridDebitHistory.RootTable.Columns["ChequeDate"].Width = 75;
			DataGridDebitHistory.RootTable.Columns["ChequeRef"].Width = 80;
			DataGridDebitHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			DataGridDebitHistory.RootTable.Columns["EntryUserName"].Width = 120;
			DataGridDebitHistory.RootTable.Columns["ModifyUserName"].Width = 120;
			DataGridDebitHistory.RootTable.Columns["EntryDate"].Width = 145;
			DataGridDebitHistory.RootTable.Columns["ModifyDate"].Width = 145;
			DataGridDebitHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridDebitHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridDebitHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridDebitHistory.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridDebitHistory.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			DataGridDebitHistory.RootTable.Columns["DebitAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridDebitHistory.RootTable.Columns["DebitAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridDebitHistory.RootTable.Columns.Add("Edit");
			DataGridDebitHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridDebitHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridDebitHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridDebitHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridDebitHistory.RootTable.Columns["Edit"].Width = 50;
			DataGridDebitHistory.RootTable.Columns.Add("Print");
			DataGridDebitHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridDebitHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridDebitHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridDebitHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridDebitHistory.RootTable.Columns["Print"].Width = 50;
			ctrlGrdBar4_Load(null, null);
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

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
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

	private void BtnShow_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			RecId = ID;
			VoucherHead Obj = global::Architecture.BLL.Accounts.VoucherHead.GetByID(ID);
			if (Obj == null)
			{
				return;
			}
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			tabControl1.SelectedIndex = 0;
			txtDocDate.Value = Obj.VoucherDate;
			txtDocNo.Text = Conversion.ToString((object)Obj.VoucherCode);
			cmbCashAccount.Value = Obj.RefAccountId;
			cmbCashAccount_Leave(null, null);
			txtHeaderRemarks.Text = Obj.Remarks;
			dtCredit.Rows.Clear();
			dtDedit.Rows.Clear();
			AttachmentsValues = Obj.AttachmentsValues;
			CustomAttachmentsValues = Obj.CustomAttachmentsValues;
			IEnumerable<VoucherDetail> rows = from r in Obj.voucherDetailList.AsEnumerable()
				where r.DebitAmount > 0.0
				select r;
			List<VoucherDetail> DebitList = (rows.Any() ? rows.ToList() : new List<VoucherDetail>());
			IEnumerable<VoucherDetail> rowscr = from r in Obj.voucherDetailList.AsEnumerable()
				where r.CreditAmount > 0.0
				select r;
			List<VoucherDetail> CreditList = (rowscr.Any() ? rowscr.ToList() : new List<VoucherDetail>());
			List<global::Architecture.Model.DMSAttachments> dMSList = global::Architecture.BLL.DMSAttachments.GetByID(ID, base.Name);
			List<DMSAttachments> list = dMSList.Where((DMSAttachments r) => !r.DetailWiseAttachment).ToList();
			AT = new Attachment();
			foreach (DMSAttachments Attachment3 in list)
			{
				TempAttachments TA3 = new TempAttachments();
				TA3.Id = 0;
				TA3.Path = Attachment3.Attachment;
				TA3.CustomFileName = Attachment3.UploadedFileCustomName;
				TA3.ActionId = 2;
				AT.lst.Add(TA3);
			}
			List<DMSAttachments> detailLst = dMSList.Where((DMSAttachments r) => r.DetailWiseAttachment).ToList();
			List<DMSAttachments> list2 = detailLst.Where((DMSAttachments r) => r.DMSFoldersLabelsId == 0).ToList();
			List<DMSAttachments> debitLst = detailLst.Where((DMSAttachments r) => r.DMSFoldersLabelsId == 1).ToList();
			ATd = new AttachmentForDetailRows();
			foreach (DMSAttachments Attachment2 in list2)
			{
				TempAttachmentsDetail TA2 = new TempAttachmentsDetail();
				TA2.Id = Attachment2.Id;
				TA2.Path = Attachment2.Attachment;
				TA2.RefAccountId = Attachment2.RefAccountId;
				TA2.DetailRefId = Attachment2.LineId;
				TA2.CustomPath = Attachment2.UploadedFileCustomName;
				ATd.lst.Add(TA2);
			}
			ATdDebit = new AttachmentForDetailRows();
			foreach (DMSAttachments Attachment in debitLst)
			{
				TempAttachmentsDetail TA = new TempAttachmentsDetail();
				TA.Id = Attachment.Id;
				TA.Path = Attachment.Attachment;
				TA.RefAccountId = Attachment.RefAccountId;
				TA.DetailRefId = Attachment.LineId;
				TA.CustomPath = Attachment.UploadedFileCustomName;
				ATdDebit.lst.Add(TA);
			}
			if (CreditList != null && CreditList.Count > 0)
			{
				int j;
				for (j = 0; j < CreditList.Count; j++)
				{
					if (CreditList[j].AccountId != CreditList[j].AgainstAccountId)
					{
						int count2 = ATd.lst.Count((TempAttachmentsDetail r) => r.DetailRefId == Conversion.ToInt((object)CreditList[j].LineId));
						dtCredit.Rows.Add(CreditList[j].AccountId, CreditList[j].AccountTitle, (CreditList[j].SubsidiaryTypeId == 1) ? CreditList[j].SupplierCustomerId : ((CreditList[j].SubsidiaryTypeId == 2) ? CreditList[j].EmployeeId : ((CreditList[j].SubsidiaryTypeId == 3) ? CreditList[j].SubsidiaryAccountId : ((CreditList[j].SubsidiaryTypeId == 4) ? CreditList[j].SubsidiaryAccountId : 0))), CreditList[j].SubsidiaryAccountTitle, CreditList[j].SubsidiaryTypeId, CreditList[j].CreditAmount, CreditList[j].SortNo, CreditList[j].DCheqDate, CreditList[j].CheqNoDetail, CreditList[j].Comments, CreditList[j].LineId, count2);
					}
				}
			}
			ReceiptGrid.DataSource = dtCredit;
			ReceiptGrid.RetrieveStructure();
			ReceiptGrdSetting();
			if (DebitList != null && DebitList.Count > 0)
			{
				int i;
				for (i = 0; i < DebitList.Count; i++)
				{
					if (DebitList[i].AccountId != DebitList[i].AgainstAccountId)
					{
						int count = ATdDebit.lst.Count((TempAttachmentsDetail r) => r.DetailRefId == Conversion.ToInt((object)debitLst[i].LineId));
						dtDedit.Rows.Add(DebitList[i].AccountId, DebitList[i].AccountTitle, (DebitList[i].SubsidiaryTypeId == 1) ? DebitList[i].SupplierCustomerId : ((DebitList[i].SubsidiaryTypeId == 2) ? DebitList[i].EmployeeId : ((DebitList[i].SubsidiaryTypeId == 3) ? DebitList[i].SubsidiaryAccountId : ((DebitList[i].SubsidiaryTypeId == 4) ? DebitList[i].SubsidiaryAccountId : 0))), DebitList[i].SubsidiaryAccountTitle, DebitList[i].SubsidiaryTypeId, DebitList[i].DebitAmount, DebitList[i].SortNo, DebitList[i].DCheqDate, DebitList[i].CheqNoDetail, DebitList[i].Comments, DebitList[i].LineId, count);
					}
				}
			}
			PaymentGrid.DataSource = dtDedit;
			PaymentGrid.RetrieveStructure();
			PaymentGrdSetting();
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
				CommonServices.DayBookSlip(Conversion.ToInt(DataGridCreditHistory.CurrentRow.Cells["Id"].Value), 9);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridCreditHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridCreditHistory.CurrentRow.Cells["Id"].Value), 9);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridCreditHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RecId = Conversion.ToInt(DataGridCreditHistory.CurrentRow.Cells["Id"].Value);
			ReadById(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
				CommonServices.DayBookSlip(Conversion.ToInt(DataGridDebitHistory.CurrentRow.Cells["Id"].Value), 9);
			}
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
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridDebitHistory.CurrentRow.Cells["Id"].Value), 9);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridDebitHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RecId = Conversion.ToInt(DataGridDebitHistory.CurrentRow.Cells["Id"].Value);
			ReadById(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnPrint144_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = GeneralReprots.DayBookSlip(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocDate = Conversion.ToDateTime(datVouchderDateForHistory.Value),
				AccountId = Conversion.ToInt(CmbCashAccountForHistory.Value),
				DocumentTypeId = 9
			});
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
			DataTable obj = (rowscr.Any() ? rowscr.CopyToDataTable() : dt.Clone());
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject DayBookSubReport_DebitTransactions = new SubReportObject(dtDebit, "DayBookDebitSubReport.rpt");
			SubReportObject DayBookSubReport_CreditTransactions = new SubReportObject(obj, "DayBookCreditSubReport.rpt");
			lstSubReports.Add(DayBookSubReport_DebitTransactions);
			lstSubReports.Add(DayBookSubReport_CreditTransactions);
			Reporting val = null;
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "144-DayBookSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DayBook_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			else if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnnew_Click(null, null);
				}
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				if (btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
			}
			else if (e.Control && e.KeyCode == Keys.U)
			{
				if (btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
			}
			else if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
				}
				else
				{
					tabControl1.SelectedIndex = 1;
				}
			}
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			else if (e.Control && e.KeyCode == Keys.F10)
			{
				btnAttachment_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Left)
			{
				CmbCreditAccount.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Right)
			{
				CmbDebitAccount.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDocDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			cmbCashAccount_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DayBook_FormClosed(object sender, FormClosedEventArgs e)
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

	private void ReceiptGrid_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = ReceiptGrid.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "Attachments")
			{
				ATd.ReferecneId = Conversion.ToInt(item.Cells["LineId"].Value);
				ATd.RefAccountId = Conversion.ToInt(item.Cells["AccountId"].Value);
				ATd.ShowDialog();
				int count = ATd.lst.Count((TempAttachmentsDetail r) => r.DetailRefId == Conversion.ToInt(item.Cells["LineId"].Value));
				DataRow[] array = dtCredit.Select(string.Format("LineId = '{0}'", Conversion.ToInt(item.Cells["LineId"].Value)));
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetField("Attachments", count);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentGrid_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = PaymentGrid.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "Attachments")
			{
				ATdDebit.ReferecneId = Conversion.ToInt(item.Cells["LineId"].Value);
				ATdDebit.RefAccountId = Conversion.ToInt(item.Cells["AccountId"].Value);
				ATdDebit.ShowDialog();
				int count = ATdDebit.lst.Count((TempAttachmentsDetail r) => r.DetailRefId == Conversion.ToInt(item.Cells["LineId"].Value));
				DataRow[] array = dtDedit.Select(string.Format("LineId = '{0}'", Conversion.ToInt(item.Cells["LineId"].Value)));
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetField("Attachments", count);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSubsidiaryAccountReceipt_Leave(object sender, EventArgs e)
	{
		try
		{
			if (!AfterCloupReceipt && ((UltraGridBase)CmbSubsidiaryAccountReceipt).ActiveRow != null && Conversion.ToInt(CmbSubsidiaryAccountReceipt.Value) != 0)
			{
				int GLOfSubSidory = Conversion.ToInt(((UltraDropDownBase)CmbSubsidiaryAccountReceipt).SelectedRow.Cells[4].Value);
				if (GLOfSubSidory != 0)
				{
					CmbCreditAccount.Value = GLOfSubSidory;
				}
			}
			AfterCloupReceipt = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSubsidiaryAccountReceipt_AfterCloseUp(object sender, EventArgs e)
	{
		try
		{
			UltraGridRow selectedRow = ((UltraDropDownBase)CmbSubsidiaryAccountReceipt).SelectedRow;
			if (selectedRow != null)
			{
				AfterCloupReceipt = true;
				int GLOfSubSidory = Conversion.ToInt(selectedRow.Cells[4].Value);
				if (GLOfSubSidory != 0)
				{
					CmbCreditAccount.Value = GLOfSubSidory;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSubsidiaryAccountPayment_Leave(object sender, EventArgs e)
	{
		try
		{
			if (!AfterCloupPayment && ((UltraGridBase)CmbSubsidiaryAccountPayment).ActiveRow != null && Conversion.ToInt(CmbSubsidiaryAccountPayment.Value) != 0)
			{
				int GLOfSubSidory = Conversion.ToInt(((UltraDropDownBase)CmbSubsidiaryAccountPayment).SelectedRow.Cells[4].Value);
				if (GLOfSubSidory != 0)
				{
					CmbDebitAccount.Value = GLOfSubSidory;
				}
			}
			AfterCloupPayment = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSubsidiaryAccountPayment_AfterCloseUp(object sender, EventArgs e)
	{
		try
		{
			UltraGridRow selectedRow = ((UltraDropDownBase)CmbSubsidiaryAccountPayment).SelectedRow;
			if (selectedRow != null)
			{
				AfterCloupPayment = true;
				int GLOfSubSidory = Conversion.ToInt(selectedRow.Cells[4].Value);
				if (GLOfSubSidory != 0)
				{
					CmbDebitAccount.Value = GLOfSubSidory;
				}
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
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Expected O, but got Unknown
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Expected O, but got Unknown
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Expected O, but got Unknown
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_05db: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Expected O, but got Unknown
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Expected O, but got Unknown
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Expected O, but got Unknown
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Expected O, but got Unknown
		//IL_0bc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bca: Expected O, but got Unknown
		//IL_0bd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be1: Expected O, but got Unknown
		//IL_22f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_22fe: Expected O, but got Unknown
		//IL_230b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2315: Expected O, but got Unknown
		//IL_4c8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c94: Expected O, but got Unknown
		//IL_4ca1: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cab: Expected O, but got Unknown
		//IL_4f96: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fa0: Expected O, but got Unknown
		//IL_4fad: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fb7: Expected O, but got Unknown
		GridEXLayout PaymentGrid_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance15 = new Appearance();
		GridEXLayout ReceiptGrid_DesignTimeLayout = new GridEXLayout();
		Appearance appearance3 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
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
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.DayBook));
		GridEXLayout DataGridDebitHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridCreditHistory_DesignTimeLayout = new GridEXLayout();
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
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.PaymentGrid = new GridEX();
		this.label17 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.CmbSubsidiaryAccountPayment = new UltraCombo();
		this.lblSubsidiaryaccountPayment = new System.Windows.Forms.Label();
		this.txtDrCheqRef = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.lblCheqDate = new System.Windows.Forms.Label();
		this.datDrCheqDate = new System.Windows.Forms.DateTimePicker();
		this.txtDrPageNo = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.btnDebitRowCancel = new System.Windows.Forms.Button();
		this.lblDebitAccountBalance = new System.Windows.Forms.Label();
		this.lblDebitAccountBalanceCaption = new System.Windows.Forms.Label();
		this.btnAddDebitRow = new System.Windows.Forms.Button();
		this.txtDebitAmount = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.txtDrRemarks = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.CmbDebitAccount = new UltraCombo();
		this.LabelDebitAccount = new System.Windows.Forms.Label();
		this.btnUpdateDebitRow = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ReceiptGrid = new GridEX();
		this.label13 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtCrCheqRef = new System.Windows.Forms.TextBox();
		this.CmbSubsidiaryAccountReceipt = new UltraCombo();
		this.lblSubsidiaryaccountReceipt = new System.Windows.Forms.Label();
		this.datCrChequeDate = new System.Windows.Forms.DateTimePicker();
		this.label22 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtCrPageNo = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.btnCreditRowCancel = new System.Windows.Forms.Button();
		this.lblCreditAccountBalance = new System.Windows.Forms.Label();
		this.lblCreditAccountBalanceCaption = new System.Windows.Forms.Label();
		this.btnAddCrditRow = new System.Windows.Forms.Button();
		this.txtCreditAmount = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtCrRemarks = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbCreditAccount = new UltraCombo();
		this.labelCreditAccount = new System.Windows.Forms.Label();
		this.btnUpdateCreditRow = new System.Windows.Forms.Button();
		this.panel1 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.ChkPrintPreview = new System.Windows.Forms.CheckBox();
		this.label3 = new System.Windows.Forms.Label();
		this.lblClosingBalance = new System.Windows.Forms.Label();
		this.lblOpeningBalance = new System.Windows.Forms.Label();
		this.lblCreditOrDebitForClosing = new System.Windows.Forms.Label();
		this.lblCreditOrDebit = new System.Windows.Forms.Label();
		this.txtHeaderRemarks = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbCashAccount = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.toolstrip = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.DataGridDebitHistory = new GridEX();
		this.label9 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.DataGridCreditHistory = new GridEX();
		this.label7 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.datVouchderDateForHistory = new UltraCombo();
		this.BtnShow = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbCashAccountForHistory = new UltraCombo();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label51 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnPrint144 = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.PaymentGrid).BeginInit();
		this.panel5.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccountPayment).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccount).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ReceiptGrid).BeginInit();
		this.panel3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccountReceipt).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).BeginInit();
		this.panel1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCashAccount).BeginInit();
		this.panel6.SuspendLayout();
		this.toolstrip.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridDebitHistory).BeginInit();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridCreditHistory).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.datVouchderDateForHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCashAccountForHistory).BeginInit();
		this.panel11.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1393, 741);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.toolstrip);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1385, 715);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add(this.ctrlGrdBar2);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.PaymentGrid);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(573, 159);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(570, 553);
		this.panel4.TabIndex = 509;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(530, 112);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.PaymentGrid;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 49;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.PaymentGrid.AllowEdit = (InheritableBoolean)2;
		PaymentGrid_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.PaymentGrid.DesignTimeLayout = PaymentGrid_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.PaymentGrid).Dock = System.Windows.Forms.DockStyle.Fill;
		this.PaymentGrid.DynamicFiltering = true;
		this.PaymentGrid.FilterMode = (FilterMode)2;
		this.PaymentGrid.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.PaymentGrid).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.PaymentGrid.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.PaymentGrid).Location = new System.Drawing.Point(0, 135);
		((System.Windows.Forms.Control)(object)this.PaymentGrid).Name = "PaymentGrid";
		((ScrollableBase)this.PaymentGrid).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.PaymentGrid).Size = new System.Drawing.Size(570, 418);
		((System.Windows.Forms.Control)(object)this.PaymentGrid).TabIndex = 4;
		this.PaymentGrid.TotalRow = (InheritableBoolean)1;
		this.PaymentGrid.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.PaymentGrid).VisualStyle = (VisualStyle)4;
		this.PaymentGrid.ColumnButtonClick += new ColumnActionEventHandler(PaymentGrid_ColumnButtonClick);
		this.PaymentGrid.LinkClicked += new ColumnActionEventHandler(PaymentGrid_LinkClicked);
		((System.Windows.Forms.Control)(object)this.PaymentGrid).DoubleClick += new System.EventHandler(PaymentGrid_DoubleClick);
		this.label17.BackColor = System.Drawing.Color.Teal;
		this.label17.Dock = System.Windows.Forms.DockStyle.Top;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(0, 112);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(570, 23);
		this.label17.TabIndex = 3;
		this.label17.Text = "Payment";
		this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel5.Controls.Add(this.groupBox3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(570, 112);
		this.panel5.TabIndex = 2;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountPayment);
		this.groupBox3.Controls.Add(this.lblSubsidiaryaccountPayment);
		this.groupBox3.Controls.Add(this.txtDrCheqRef);
		this.groupBox3.Controls.Add(this.label23);
		this.groupBox3.Controls.Add(this.lblCheqDate);
		this.groupBox3.Controls.Add(this.datDrCheqDate);
		this.groupBox3.Controls.Add(this.txtDrPageNo);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add(this.btnDebitRowCancel);
		this.groupBox3.Controls.Add(this.lblDebitAccountBalance);
		this.groupBox3.Controls.Add(this.lblDebitAccountBalanceCaption);
		this.groupBox3.Controls.Add(this.btnAddDebitRow);
		this.groupBox3.Controls.Add(this.txtDebitAmount);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Controls.Add(this.txtDrRemarks);
		this.groupBox3.Controls.Add(this.label15);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDebitAccount);
		this.groupBox3.Controls.Add(this.LabelDebitAccount);
		this.groupBox3.Controls.Add(this.btnUpdateDebitRow);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(6, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(557, 107);
		this.groupBox3.TabIndex = 2;
		this.groupBox3.TabStop = false;
		this.groupBox3.Tag = "";
		this.groupBox3.Text = "Payment";
		this.CmbSubsidiaryAccountPayment.AllowNull = (DefaultableBoolean)1;
		this.CmbSubsidiaryAccountPayment.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSubsidiaryAccountPayment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSubsidiaryAccountPayment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance59;
		((SpecialBoxBase)((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance64).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSubsidiaryAccountPayment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountPayment).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbSubsidiaryAccountPayment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountPayment).Location = new System.Drawing.Point(222, 33);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountPayment).Name = "CmbSubsidiaryAccountPayment";
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountPayment).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountPayment).TabIndex = 1;
		this.CmbSubsidiaryAccountPayment.AfterCloseUp += new System.EventHandler(CmbSubsidiaryAccountPayment_AfterCloseUp);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountPayment).Leave += new System.EventHandler(CmbSubsidiaryAccountPayment_Leave);
		this.lblSubsidiaryaccountPayment.AutoSize = true;
		this.lblSubsidiaryaccountPayment.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSubsidiaryaccountPayment.Location = new System.Drawing.Point(219, 17);
		this.lblSubsidiaryaccountPayment.Name = "lblSubsidiaryaccountPayment";
		this.lblSubsidiaryaccountPayment.Size = new System.Drawing.Size(110, 15);
		this.lblSubsidiaryaccountPayment.TabIndex = 538;
		this.lblSubsidiaryaccountPayment.Text = "Subsidiary Account";
		this.txtDrCheqRef.BackColor = System.Drawing.Color.White;
		this.txtDrCheqRef.Cursor = System.Windows.Forms.Cursors.IBeam;
		this.txtDrCheqRef.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDrCheqRef.Location = new System.Drawing.Point(113, 77);
		this.txtDrCheqRef.Name = "txtDrCheqRef";
		this.txtDrCheqRef.Size = new System.Drawing.Size(89, 23);
		this.txtDrCheqRef.TabIndex = 5;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label23.Location = new System.Drawing.Point(112, 61);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(62, 15);
		this.label23.TabIndex = 535;
		this.label23.Text = "Cheq / Ref";
		this.lblCheqDate.AutoSize = true;
		this.lblCheqDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCheqDate.Location = new System.Drawing.Point(6, 61);
		this.lblCheqDate.Name = "lblCheqDate";
		this.lblCheqDate.Size = new System.Drawing.Size(75, 15);
		this.lblCheqDate.TabIndex = 531;
		this.lblCheqDate.Text = "Cheque Date";
		this.datDrCheqDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDrCheqDate.CustomFormat = "dd-MMM-yy";
		this.datDrCheqDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.datDrCheqDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDrCheqDate.Location = new System.Drawing.Point(6, 77);
		this.datDrCheqDate.Name = "datDrCheqDate";
		this.datDrCheqDate.Size = new System.Drawing.Size(106, 22);
		this.datDrCheqDate.TabIndex = 4;
		this.txtDrPageNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDrPageNo.Location = new System.Drawing.Point(494, 35);
		this.txtDrPageNo.Name = "txtDrPageNo";
		this.txtDrPageNo.Size = new System.Drawing.Size(61, 23);
		this.txtDrPageNo.TabIndex = 3;
		this.txtDrPageNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(494, 17);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(52, 15);
		this.label20.TabIndex = 536;
		this.label20.Text = "Page No";
		this.btnDebitRowCancel.BackColor = System.Drawing.Color.Teal;
		this.btnDebitRowCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDebitRowCancel.ForeColor = System.Drawing.Color.White;
		this.btnDebitRowCancel.Location = new System.Drawing.Point(491, 76);
		this.btnDebitRowCancel.Name = "btnDebitRowCancel";
		this.btnDebitRowCancel.Size = new System.Drawing.Size(64, 25);
		this.btnDebitRowCancel.TabIndex = 8;
		this.btnDebitRowCancel.Text = "Cancel";
		this.btnDebitRowCancel.UseVisualStyleBackColor = false;
		this.btnDebitRowCancel.Click += new System.EventHandler(btnDebitRowCancel_Click);
		this.lblDebitAccountBalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDebitAccountBalance.Location = new System.Drawing.Point(90, 17);
		this.lblDebitAccountBalance.Name = "lblDebitAccountBalance";
		this.lblDebitAccountBalance.Size = new System.Drawing.Size(113, 15);
		this.lblDebitAccountBalance.TabIndex = 534;
		this.lblDebitAccountBalance.Text = "0";
		this.lblDebitAccountBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblDebitAccountBalanceCaption.AutoSize = true;
		this.lblDebitAccountBalanceCaption.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDebitAccountBalanceCaption.Location = new System.Drawing.Point(202, 17);
		this.lblDebitAccountBalanceCaption.Name = "lblDebitAccountBalanceCaption";
		this.lblDebitAccountBalanceCaption.Size = new System.Drawing.Size(18, 15);
		this.lblDebitAccountBalanceCaption.TabIndex = 533;
		this.lblDebitAccountBalanceCaption.Text = "Cr";
		this.btnAddDebitRow.BackColor = System.Drawing.Color.Teal;
		this.btnAddDebitRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddDebitRow.ForeColor = System.Drawing.Color.White;
		this.btnAddDebitRow.Location = new System.Drawing.Point(428, 76);
		this.btnAddDebitRow.Name = "btnAddDebitRow";
		this.btnAddDebitRow.Size = new System.Drawing.Size(27, 25);
		this.btnAddDebitRow.TabIndex = 7;
		this.btnAddDebitRow.Text = "+";
		this.btnAddDebitRow.UseVisualStyleBackColor = false;
		this.btnAddDebitRow.Click += new System.EventHandler(btnAddDebitRow_Click);
		this.txtDebitAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDebitAmount.Location = new System.Drawing.Point(389, 35);
		this.txtDebitAmount.Name = "txtDebitAmount";
		this.txtDebitAmount.Size = new System.Drawing.Size(104, 23);
		this.txtDebitAmount.TabIndex = 2;
		this.txtDebitAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDebitAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDebitAmount_KeyPress);
		this.txtDebitAmount.Leave += new System.EventHandler(txtDebitAmount_Leave);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(389, 17);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(51, 15);
		this.label14.TabIndex = 530;
		this.label14.Text = "Amount";
		this.txtDrRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDrRemarks.Location = new System.Drawing.Point(203, 77);
		this.txtDrRemarks.Name = "txtDrRemarks";
		this.txtDrRemarks.Size = new System.Drawing.Size(224, 23);
		this.txtDrRemarks.TabIndex = 6;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(201, 61);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(52, 15);
		this.label15.TabIndex = 528;
		this.label15.Text = "Remarks";
		this.CmbDebitAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbDebitAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDebitAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDebitAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance83).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDebitAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Location = new System.Drawing.Point(6, 33);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Name = "CmbDebitAccount";
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Leave += new System.EventHandler(CmbDebitAccount_Leave);
		this.LabelDebitAccount.AutoSize = true;
		this.LabelDebitAccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.LabelDebitAccount.Location = new System.Drawing.Point(3, 17);
		this.LabelDebitAccount.Name = "LabelDebitAccount";
		this.LabelDebitAccount.Size = new System.Drawing.Size(84, 15);
		this.LabelDebitAccount.TabIndex = 526;
		this.LabelDebitAccount.Text = "Debit Account";
		this.btnUpdateDebitRow.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDebitRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDebitRow.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDebitRow.Location = new System.Drawing.Point(428, 76);
		this.btnUpdateDebitRow.Name = "btnUpdateDebitRow";
		this.btnUpdateDebitRow.Size = new System.Drawing.Size(64, 25);
		this.btnUpdateDebitRow.TabIndex = 7;
		this.btnUpdateDebitRow.Text = "Update";
		this.btnUpdateDebitRow.UseVisualStyleBackColor = false;
		this.btnUpdateDebitRow.Click += new System.EventHandler(btnUpdateDebitRow_Click);
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.ReceiptGrid);
		this.panel2.Controls.Add(this.label13);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel2.Location = new System.Drawing.Point(3, 159);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(570, 553);
		this.panel2.TabIndex = 508;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(529, 112);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.ReceiptGrid;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 48;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ReceiptGrid.AllowEdit = (InheritableBoolean)2;
		ReceiptGrid_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.ReceiptGrid.DesignTimeLayout = ReceiptGrid_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.ReceiptGrid).Dock = System.Windows.Forms.DockStyle.Fill;
		this.ReceiptGrid.DynamicFiltering = true;
		this.ReceiptGrid.FilterMode = (FilterMode)2;
		this.ReceiptGrid.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.ReceiptGrid).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.ReceiptGrid.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.ReceiptGrid).Location = new System.Drawing.Point(0, 135);
		((System.Windows.Forms.Control)(object)this.ReceiptGrid).Name = "ReceiptGrid";
		((ScrollableBase)this.ReceiptGrid).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.ReceiptGrid).Size = new System.Drawing.Size(570, 418);
		((System.Windows.Forms.Control)(object)this.ReceiptGrid).TabIndex = 3;
		this.ReceiptGrid.TotalRow = (InheritableBoolean)1;
		this.ReceiptGrid.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.ReceiptGrid).VisualStyle = (VisualStyle)4;
		this.ReceiptGrid.ColumnButtonClick += new ColumnActionEventHandler(ReceiptGrid_ColumnButtonClick);
		this.ReceiptGrid.LinkClicked += new ColumnActionEventHandler(ReceiptGrid_LinkClicked);
		((System.Windows.Forms.Control)(object)this.ReceiptGrid).DoubleClick += new System.EventHandler(ReceiptGrid_DoubleClick);
		this.label13.BackColor = System.Drawing.Color.Teal;
		this.label13.Dock = System.Windows.Forms.DockStyle.Top;
		this.label13.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(0, 112);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(570, 23);
		this.label13.TabIndex = 2;
		this.label13.Text = "Receipt";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(570, 112);
		this.panel3.TabIndex = 1;
		this.groupBox2.Controls.Add(this.txtCrCheqRef);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountReceipt);
		this.groupBox2.Controls.Add(this.lblSubsidiaryaccountReceipt);
		this.groupBox2.Controls.Add(this.datCrChequeDate);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.txtCrPageNo);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.btnCreditRowCancel);
		this.groupBox2.Controls.Add(this.lblCreditAccountBalance);
		this.groupBox2.Controls.Add(this.lblCreditAccountBalanceCaption);
		this.groupBox2.Controls.Add(this.btnAddCrditRow);
		this.groupBox2.Controls.Add(this.txtCreditAmount);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.txtCrRemarks);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAccount);
		this.groupBox2.Controls.Add(this.labelCreditAccount);
		this.groupBox2.Controls.Add(this.btnUpdateCreditRow);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(7, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(557, 107);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Tag = "";
		this.groupBox2.Text = "Receipt";
		this.txtCrCheqRef.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCrCheqRef.Location = new System.Drawing.Point(113, 79);
		this.txtCrCheqRef.Name = "txtCrCheqRef";
		this.txtCrCheqRef.Size = new System.Drawing.Size(89, 23);
		this.txtCrCheqRef.TabIndex = 5;
		this.CmbSubsidiaryAccountReceipt.AllowNull = (DefaultableBoolean)1;
		this.CmbSubsidiaryAccountReceipt.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSubsidiaryAccountReceipt.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSubsidiaryAccountReceipt.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSubsidiaryAccountReceipt).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountReceipt).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbSubsidiaryAccountReceipt.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountReceipt).Location = new System.Drawing.Point(221, 35);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountReceipt).Name = "CmbSubsidiaryAccountReceipt";
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountReceipt).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountReceipt).TabIndex = 1;
		this.CmbSubsidiaryAccountReceipt.AfterCloseUp += new System.EventHandler(CmbSubsidiaryAccountReceipt_AfterCloseUp);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccountReceipt).Leave += new System.EventHandler(CmbSubsidiaryAccountReceipt_Leave);
		this.lblSubsidiaryaccountReceipt.AutoSize = true;
		this.lblSubsidiaryaccountReceipt.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSubsidiaryaccountReceipt.Location = new System.Drawing.Point(218, 19);
		this.lblSubsidiaryaccountReceipt.Name = "lblSubsidiaryaccountReceipt";
		this.lblSubsidiaryaccountReceipt.Size = new System.Drawing.Size(110, 15);
		this.lblSubsidiaryaccountReceipt.TabIndex = 535;
		this.lblSubsidiaryaccountReceipt.Text = "Subsidiary Account";
		this.datCrChequeDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datCrChequeDate.CustomFormat = "dd-MMM-yy";
		this.datCrChequeDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.datCrChequeDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datCrChequeDate.Location = new System.Drawing.Point(6, 79);
		this.datCrChequeDate.Name = "datCrChequeDate";
		this.datCrChequeDate.Size = new System.Drawing.Size(106, 22);
		this.datCrChequeDate.TabIndex = 4;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(6, 62);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(75, 15);
		this.label22.TabIndex = 533;
		this.label22.Text = "Cheque Date";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(113, 62);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(62, 15);
		this.label21.TabIndex = 529;
		this.label21.Text = "Cheq / Ref";
		this.txtCrPageNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCrPageNo.Location = new System.Drawing.Point(493, 37);
		this.txtCrPageNo.Name = "txtCrPageNo";
		this.txtCrPageNo.Size = new System.Drawing.Size(61, 23);
		this.txtCrPageNo.TabIndex = 3;
		this.txtCrPageNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(493, 19);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(52, 15);
		this.label19.TabIndex = 533;
		this.label19.Text = "Page No";
		this.btnCreditRowCancel.BackColor = System.Drawing.Color.Teal;
		this.btnCreditRowCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCreditRowCancel.ForeColor = System.Drawing.Color.White;
		this.btnCreditRowCancel.Location = new System.Drawing.Point(490, 78);
		this.btnCreditRowCancel.Name = "btnCreditRowCancel";
		this.btnCreditRowCancel.Size = new System.Drawing.Size(64, 25);
		this.btnCreditRowCancel.TabIndex = 8;
		this.btnCreditRowCancel.Text = "Cancel";
		this.btnCreditRowCancel.UseVisualStyleBackColor = false;
		this.btnCreditRowCancel.Click += new System.EventHandler(btnCreditRowCancel_Click);
		this.lblCreditAccountBalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCreditAccountBalance.Location = new System.Drawing.Point(89, 19);
		this.lblCreditAccountBalance.Name = "lblCreditAccountBalance";
		this.lblCreditAccountBalance.Size = new System.Drawing.Size(113, 15);
		this.lblCreditAccountBalance.TabIndex = 527;
		this.lblCreditAccountBalance.Text = "0";
		this.lblCreditAccountBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblCreditAccountBalanceCaption.AutoSize = true;
		this.lblCreditAccountBalanceCaption.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCreditAccountBalanceCaption.Location = new System.Drawing.Point(202, 19);
		this.lblCreditAccountBalanceCaption.Name = "lblCreditAccountBalanceCaption";
		this.lblCreditAccountBalanceCaption.Size = new System.Drawing.Size(18, 15);
		this.lblCreditAccountBalanceCaption.TabIndex = 526;
		this.lblCreditAccountBalanceCaption.Text = "Cr";
		this.btnAddCrditRow.BackColor = System.Drawing.Color.Teal;
		this.btnAddCrditRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddCrditRow.ForeColor = System.Drawing.Color.White;
		this.btnAddCrditRow.Location = new System.Drawing.Point(428, 78);
		this.btnAddCrditRow.Name = "btnAddCrditRow";
		this.btnAddCrditRow.Size = new System.Drawing.Size(27, 25);
		this.btnAddCrditRow.TabIndex = 7;
		this.btnAddCrditRow.Text = "+";
		this.btnAddCrditRow.UseVisualStyleBackColor = false;
		this.btnAddCrditRow.Click += new System.EventHandler(btnAddCrditRow_Click);
		this.txtCreditAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCreditAmount.Location = new System.Drawing.Point(388, 37);
		this.txtCreditAmount.Name = "txtCreditAmount";
		this.txtCreditAmount.Size = new System.Drawing.Size(104, 23);
		this.txtCreditAmount.TabIndex = 2;
		this.txtCreditAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCreditAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCreditAmount_KeyPress);
		this.txtCreditAmount.Leave += new System.EventHandler(txtCreditAmount_Leave);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(388, 19);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(51, 15);
		this.label12.TabIndex = 530;
		this.label12.Text = "Amount";
		this.txtCrRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCrRemarks.Location = new System.Drawing.Point(203, 79);
		this.txtCrRemarks.Name = "txtCrRemarks";
		this.txtCrRemarks.Size = new System.Drawing.Size(224, 23);
		this.txtCrRemarks.TabIndex = 6;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(201, 62);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(52, 15);
		this.label11.TabIndex = 528;
		this.label11.Text = "Remarks";
		this.CmbCreditAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCreditAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Location = new System.Drawing.Point(6, 35);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Name = "CmbCreditAccount";
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Leave += new System.EventHandler(CmbCreditAccount_Leave);
		this.labelCreditAccount.AutoSize = true;
		this.labelCreditAccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelCreditAccount.Location = new System.Drawing.Point(3, 19);
		this.labelCreditAccount.Name = "labelCreditAccount";
		this.labelCreditAccount.Size = new System.Drawing.Size(86, 15);
		this.labelCreditAccount.TabIndex = 526;
		this.labelCreditAccount.Text = "Credit Account";
		this.btnUpdateCreditRow.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateCreditRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateCreditRow.ForeColor = System.Drawing.Color.White;
		this.btnUpdateCreditRow.Location = new System.Drawing.Point(427, 78);
		this.btnUpdateCreditRow.Name = "btnUpdateCreditRow";
		this.btnUpdateCreditRow.Size = new System.Drawing.Size(64, 25);
		this.btnUpdateCreditRow.TabIndex = 7;
		this.btnUpdateCreditRow.Text = "Update";
		this.btnUpdateCreditRow.UseVisualStyleBackColor = false;
		this.btnUpdateCreditRow.Click += new System.EventHandler(btnUpdateCreditRow_Click);
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 58);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1379, 101);
		this.panel1.TabIndex = 507;
		this.groupBox1.Controls.Add(this.ChkPrintPreview);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.lblClosingBalance);
		this.groupBox1.Controls.Add(this.lblOpeningBalance);
		this.groupBox1.Controls.Add(this.lblCreditOrDebitForClosing);
		this.groupBox1.Controls.Add(this.lblCreditOrDebit);
		this.groupBox1.Controls.Add(this.txtHeaderRemarks);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCashAccount);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtDocNo);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.txtDocDate);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(6, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1133, 95);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.ChkPrintPreview.AutoSize = true;
		this.ChkPrintPreview.Checked = true;
		this.ChkPrintPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrintPreview.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintPreview.Location = new System.Drawing.Point(646, 69);
		this.ChkPrintPreview.Name = "ChkPrintPreview";
		this.ChkPrintPreview.Size = new System.Drawing.Size(95, 19);
		this.ChkPrintPreview.TabIndex = 527;
		this.ChkPrintPreview.Text = "Print Preview";
		this.ChkPrintPreview.UseVisualStyleBackColor = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(9, 50);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(114, 38);
		this.label3.TabIndex = 526;
		this.label3.Text = "Vocher Remarks";
		this.lblClosingBalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblClosingBalance.Location = new System.Drawing.Point(699, 34);
		this.lblClosingBalance.Name = "lblClosingBalance";
		this.lblClosingBalance.Size = new System.Drawing.Size(129, 15);
		this.lblClosingBalance.TabIndex = 524;
		this.lblClosingBalance.Text = "0";
		this.lblClosingBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblOpeningBalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOpeningBalance.Location = new System.Drawing.Point(699, 16);
		this.lblOpeningBalance.Name = "lblOpeningBalance";
		this.lblOpeningBalance.Size = new System.Drawing.Size(129, 15);
		this.lblOpeningBalance.TabIndex = 523;
		this.lblOpeningBalance.Text = "0";
		this.lblOpeningBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblCreditOrDebitForClosing.AutoSize = true;
		this.lblCreditOrDebitForClosing.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCreditOrDebitForClosing.Location = new System.Drawing.Point(829, 35);
		this.lblCreditOrDebitForClosing.Name = "lblCreditOrDebitForClosing";
		this.lblCreditOrDebitForClosing.Size = new System.Drawing.Size(20, 15);
		this.lblCreditOrDebitForClosing.TabIndex = 522;
		this.lblCreditOrDebitForClosing.Text = "Dr";
		this.lblCreditOrDebit.AutoSize = true;
		this.lblCreditOrDebit.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCreditOrDebit.Location = new System.Drawing.Point(829, 17);
		this.lblCreditOrDebit.Name = "lblCreditOrDebit";
		this.lblCreditOrDebit.Size = new System.Drawing.Size(18, 15);
		this.lblCreditOrDebit.TabIndex = 521;
		this.lblCreditOrDebit.Text = "Cr";
		this.txtHeaderRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtHeaderRemarks.Location = new System.Drawing.Point(124, 47);
		this.txtHeaderRemarks.Multiline = true;
		this.txtHeaderRemarks.Name = "txtHeaderRemarks";
		this.txtHeaderRemarks.Size = new System.Drawing.Size(515, 44);
		this.txtHeaderRemarks.TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(645, 34);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(46, 15);
		this.label2.TabIndex = 520;
		this.label2.Text = "Closing";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(645, 16);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(53, 15);
		this.label1.TabIndex = 519;
		this.label1.Text = "Opening";
		this.cmbCashAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbCashAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCashAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCashAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCashAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.cmbCashAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance38).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCashAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCashAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCashAccount).Location = new System.Drawing.Point(423, 19);
		((System.Windows.Forms.Control)(object)this.cmbCashAccount).Name = "cmbCashAccount";
		((System.Windows.Forms.Control)(object)this.cmbCashAccount).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.cmbCashAccount).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbCashAccount).Leave += new System.EventHandler(cmbCashAccount_Leave);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(339, 23);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(80, 15);
		this.label4.TabIndex = 518;
		this.label4.Text = "Cash Account";
		this.txtDocNo.Enabled = false;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(124, 19);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.ReadOnly = true;
		this.txtDocNo.Size = new System.Drawing.Size(82, 23);
		this.txtDocNo.TabIndex = 0;
		this.txtDocNo.TabStop = false;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(9, 23);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(114, 15);
		this.label8.TabIndex = 514;
		this.label8.Text = "Vocher Code && Date";
		this.txtDocDate.CustomFormat = "dd-MMM-yyyy";
		this.txtDocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(208, 19);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(130, 23);
		this.txtDocDate.TabIndex = 1;
		this.txtDocDate.ValueChanged += new System.EventHandler(txtDocDate_ValueChanged);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label18);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 30);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1379, 28);
		this.panel6.TabIndex = 505;
		this.label18.AutoSize = true;
		this.label18.BackColor = System.Drawing.Color.Teal;
		this.label18.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.Transparent;
		this.label18.Location = new System.Drawing.Point(2, 4);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(86, 19);
		this.label18.TabIndex = 25;
		this.label18.Text = "Day Book";
		this.toolstrip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnsave, this.btnUpdate, this.btnRefresh, this.btnAttachment });
		this.toolstrip.Location = new System.Drawing.Point(3, 3);
		this.toolstrip.Name = "toolstrip";
		this.toolstrip.Size = new System.Drawing.Size(1379, 27);
		this.toolstrip.TabIndex = 6;
		this.toolstrip.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Controls.Add(this.toolStrip1);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1385, 715);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add(this.ctrlGrdBar4);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridDebitHistory);
		this.panel9.Controls.Add(this.label9);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel9.Location = new System.Drawing.Point(623, 113);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(620, 599);
		this.panel9.TabIndex = 527;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(580, 2);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.DataGridDebitHistory;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar4.TabIndex = 49;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.DataGridDebitHistory.AllowEdit = (InheritableBoolean)2;
		DataGridDebitHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridDebitHistory.DesignTimeLayout = DataGridDebitHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridDebitHistory.DynamicFiltering = true;
		this.DataGridDebitHistory.FilterMode = (FilterMode)2;
		this.DataGridDebitHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridDebitHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).Name = "DataGridDebitHistory";
		((ScrollableBase)this.DataGridDebitHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).Size = new System.Drawing.Size(620, 574);
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).TabIndex = 5;
		this.DataGridDebitHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridDebitHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridDebitHistory).VisualStyle = (VisualStyle)4;
		this.DataGridDebitHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridDebitHistory_ColumnButtonClick);
		this.DataGridDebitHistory.LinkClicked += new ColumnActionEventHandler(DataGridDebitHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).DoubleClick += new System.EventHandler(DataGridDebitHistory_DoubleClick);
		this.label9.BackColor = System.Drawing.Color.Teal;
		this.label9.Dock = System.Windows.Forms.DockStyle.Top;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(0, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(620, 25);
		this.label9.TabIndex = 1;
		this.label9.Text = "Payment Side";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel8.Controls.Add(this.ctrlGrdBar3);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridCreditHistory);
		this.panel8.Controls.Add(this.label7);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel8.Location = new System.Drawing.Point(3, 113);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(620, 599);
		this.panel8.TabIndex = 526;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(577, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.DataGridCreditHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 50;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.DataGridCreditHistory.AllowEdit = (InheritableBoolean)2;
		DataGridCreditHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridCreditHistory.DesignTimeLayout = DataGridCreditHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridCreditHistory.DynamicFiltering = true;
		this.DataGridCreditHistory.FilterMode = (FilterMode)2;
		this.DataGridCreditHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).Name = "DataGridCreditHistory";
		((ScrollableBase)this.DataGridCreditHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).Size = new System.Drawing.Size(620, 574);
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).TabIndex = 4;
		this.DataGridCreditHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridCreditHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridCreditHistory).VisualStyle = (VisualStyle)4;
		this.DataGridCreditHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridCreditHistory_ColumnButtonClick);
		this.DataGridCreditHistory.LinkClicked += new ColumnActionEventHandler(DataGridCreditHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).DoubleClick += new System.EventHandler(DataGridCreditHistory_DoubleClick);
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Dock = System.Windows.Forms.DockStyle.Top;
		this.label7.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(0, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(620, 25);
		this.label7.TabIndex = 0;
		this.label7.Text = "Receipt Side";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.datVouchderDateForHistory);
		this.panel7.Controls.Add(this.BtnShow);
		this.panel7.Controls.Add(this.label5);
		this.panel7.Controls.Add(this.label6);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCashAccountForHistory);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 60);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1379, 53);
		this.panel7.TabIndex = 525;
		this.datVouchderDateForHistory.AllowNull = (DefaultableBoolean)1;
		this.datVouchderDateForHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.datVouchderDateForHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.datVouchderDateForHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance51).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).TextHAlignAsString = "Left";
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.datVouchderDateForHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datVouchderDateForHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.datVouchderDateForHistory).Location = new System.Drawing.Point(6, 22);
		((System.Windows.Forms.Control)(object)this.datVouchderDateForHistory).Name = "datVouchderDateForHistory";
		((System.Windows.Forms.Control)(object)this.datVouchderDateForHistory).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.datVouchderDateForHistory).TabIndex = 0;
		this.BtnShow.BackColor = System.Drawing.Color.Teal;
		this.BtnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShow.ForeColor = System.Drawing.Color.White;
		this.BtnShow.Location = new System.Drawing.Point(422, 22);
		this.BtnShow.Name = "BtnShow";
		this.BtnShow.Size = new System.Drawing.Size(56, 26);
		this.BtnShow.TabIndex = 2;
		this.BtnShow.Text = "Show";
		this.BtnShow.UseVisualStyleBackColor = false;
		this.BtnShow.Click += new System.EventHandler(BtnShow_Click);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(79, 15);
		this.label5.TabIndex = 465;
		this.label5.Text = "Voucher Date";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(186, 4);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(80, 15);
		this.label6.TabIndex = 518;
		this.label6.Text = "Cash Account";
		this.CmbCashAccountForHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCashAccountForHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCashAccountForHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCashAccountForHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance72;
		((SpecialBoxBase)((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance77).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance80).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCashAccountForHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCashAccountForHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCashAccountForHistory).Location = new System.Drawing.Point(186, 22);
		((System.Windows.Forms.Control)(object)this.CmbCashAccountForHistory).Name = "CmbCashAccountForHistory";
		((System.Windows.Forms.Control)(object)this.CmbCashAccountForHistory).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.CmbCashAccountForHistory).TabIndex = 1;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label51);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 30);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1379, 30);
		this.panel11.TabIndex = 524;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.ForeColor = System.Drawing.Color.White;
		this.label51.Location = new System.Drawing.Point(8, 5);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(175, 19);
		this.label51.TabIndex = 25;
		this.label51.Text = "DAY BOOK HISTORY";
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnPrint144, this.btnRefreshHistory });
		this.toolStrip1.Location = new System.Drawing.Point(3, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1379, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnPrint144.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint144.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint144.Name = "btnPrint144";
		this.btnPrint144.Size = new System.Drawing.Size(79, 24);
		this.btnPrint144.Text = "144-Print";
		this.btnPrint144.Click += new System.EventHandler(btnPrint144_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1393, 741);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "DayBook";
		this.Text = "DayBook";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(DayBook_FormClosed);
		base.Load += new System.EventHandler(DayBook_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DayBook_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.PaymentGrid).EndInit();
		this.panel5.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccountPayment).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccount).EndInit();
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ReceiptGrid).EndInit();
		this.panel3.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccountReceipt).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).EndInit();
		this.panel1.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCashAccount).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolstrip.ResumeLayout(false);
		this.toolstrip.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridDebitHistory).EndInit();
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridCreditHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.datVouchderDateForHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCashAccountForHistory).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}