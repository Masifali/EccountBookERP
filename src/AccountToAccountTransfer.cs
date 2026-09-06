using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp
{

using DropDownBind = global::Architecture.WinApp.Common.DropDownBind;
using Rightsobjects = global::Architecture.WinApp.Common.Rightsobjects;

using UserAccount = global::Architecture.Model.UserAccount;
using FinancialYear = global::Architecture.Model.FinancialYear;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using global::Architecture.BLL.Accounts;
using global::Architecture.Common;
using global::Architecture.Model;
using global::Architecture.Model.Main;
// // using Architecture.WinApp.Helper;
// using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

public class AccountToAccountTransfer : Form
{
	private DropDownBind DDL = new DropDownBind();

	private DataTable table = new DataTable();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private ContextMenuStrip contextMenuStrip1;

	private PrintDialog printDialog1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

	private Panel panel1;

	private Panel panel7;

	private Label lbltocompany;

	private Panel panel6;

	private Label label15;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnRefresh;

	internal UltraCombo CmbDetailAccount;

	internal UltraCombo CmbThirdLevelAccounts;

	private Label lblThirdLevelAccount;

	internal UltraCombo CmbParentAccountToFilter;

	private Label label1;

	private ToolStripButton btnUpdate;

	private CheckBox chkAccountTransactionsMerge;

	internal UltraCombo CmbToChartOfAccount;

	private Label lblToAccountTitle;

	public AccountToAccountTransfer(UserAccount puserAccount)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
	}

	private void AcfrmAcAllocation_Load(object sender, EventArgs e)
	{
		try
		{
			ThirdLevelAccountsFill();
			AccountFillFromGlobal();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountFillFromGlobal(int[] parentAccountIds = null)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("AccountTitle", typeof(string));
			dt.Columns.Add("AccountCode", typeof(string));
			dt.Columns.Add("AccountClass", typeof(string));
			dt.Columns.Add("AccountType", typeof(string));
			dt.Columns.Add("ParentAccountId", typeof(int));
			dt.Columns.Add("ParentAccountTitle", typeof(string));
			dt.Columns.Add("AccountClassId", typeof(int));
			dt.Columns.Add("AccountTypeId", typeof(int));
			List<getGlobalAllAccountsWithCustomGroup> allAccounts = clsGlobalVariables.AllAccountsWithCustomGroupId;
			if (parentAccountIds != null && parentAccountIds.Length != 0)
			{
				allAccounts = allAccounts.Where((getGlobalAllAccountsWithCustomGroup r) => parentAccountIds.Contains(r.ParentAccountId)).ToList();
			}
			HashSet<int> seenIds = new HashSet<int>();
			foreach (getGlobalAllAccountsWithCustomGroup acc in allAccounts)
			{
				int id = Conversion.ToInt((object)acc.ChartOfAccountId);
				if (seenIds.Add(id))
				{
					dt.Rows.Add(id, acc.AccountTitle, acc.AccountCode, acc.AccountClassName, acc.AccountType, acc.ParentAccountId, acc.ParentAccountTitle, acc.AccountClass, acc.AccountTypeId);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbDetailAccount, dt, "Id", "AccountTitle", "Account Title", AllColumns: true, Conversion.ToInt(CmbDetailAccount.Value), insertDefaultRow: true);
			if (dt.Rows.Count > 0)
			{
				UltraGridLayout displayLayout = ((UltraGridBase)CmbDetailAccount).DisplayLayout;
				if (displayLayout != null && ((DisposableObjectCollectionBase)displayLayout.Bands).Count > 0)
				{
					((UltraGridBase)CmbDetailAccount).DisplayLayout.Bands[0].Columns["ParentAccountId"].Hidden = true;
					((UltraGridBase)CmbDetailAccount).DisplayLayout.Bands[0].Columns["AccountClassId"].Hidden = true;
					((UltraGridBase)CmbDetailAccount).DisplayLayout.Bands[0].Columns["AccountTypeId"].Hidden = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading accounts: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ThirdLevelAccountsFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		try
		{
			DataTable dt = ChartofAccount.ReadAllAccountgroup(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Account_Level = 3
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtGroupAc = new DataTable();
				dtGroupAc.Columns.Add("Id", typeof(int));
				dtGroupAc.Columns.Add("AccountTitle", typeof(string));
				dtGroupAc.Columns.Add("AccountCode", typeof(string));
				dtGroupAc.Columns.Add("AccountType", typeof(string));
				dtGroupAc.Columns.Add("AccountClass", typeof(string));
				dtGroupAc.Columns.Add("ParentAccount", typeof(string));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtGroupAc.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["AccountTitle"], dt.Rows[i]["AccountCode"], dt.Rows[i]["AccountType"], dt.Rows[i]["ClassName"], dt.Rows[i]["ParentAccountTitle"]);
				}
				DDL.BindDDL(dtGroupAc, CmbParentAccountToFilter, "Id", "AccountTitle", "From Parent", ZeroIndex: false);
				DDL.BindDDL(dtGroupAc, CmbThirdLevelAccounts, "Id", "AccountTitle", "3rd Level Account", ZeroIndex: false);
			}
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
			AccountFillFromGlobal();
			ThirdLevelAccountsFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AcfrmAcAllocation_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AcfrmAcAllocation_FormClosed(object sender, FormClosedEventArgs e)
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

	private void CmbParentAccountToFilter_Leave(object sender, EventArgs e)
	{
		try
		{
			AccountFillFromGlobal((Conversion.ToInt(CmbParentAccountToFilter.Value) <= 0) ? null : new int[1] { Conversion.ToInt(CmbParentAccountToFilter.Value) });
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
			if (((UltraGridBase)CmbDetailAccount).ActiveRow == null || Conversion.ToInt(CmbDetailAccount.Value) == 0)
			{
				MessageBox.Show("Detail Account field is required");
				CmbDetailAccount.Focus();
				return;
			}
			if (chkAccountTransactionsMerge.Checked)
			{
				if (((UltraGridBase)CmbToChartOfAccount).ActiveRow == null || Conversion.ToInt(CmbToChartOfAccount.Value) == 0)
				{
					MessageBox.Show("To Account Title field is required");
					CmbToChartOfAccount.Focus();
					return;
				}
				int FromAccountClassId = Conversion.ToInt(((UltraDropDownBase)CmbDetailAccount).SelectedRow.Cells["AccountClassId"].Value);
				int ToAccountClassId = Conversion.ToInt(((UltraDropDownBase)CmbToChartOfAccount).SelectedRow.Cells["AccountClassId"].Value);
				int FromAccountAcTyped = Conversion.ToInt(((UltraDropDownBase)CmbDetailAccount).SelectedRow.Cells["AccountTypeId"].Value);
				int ToAccountAcTypeId = Conversion.ToInt(((UltraDropDownBase)CmbToChartOfAccount).SelectedRow.Cells["AccountTypeId"].Value);
				if (FromAccountClassId == 4 && ToAccountClassId != 4)
				{
					CmbToChartOfAccount.Focus();
					throw new Exception("Expense Class Account Transactions can be transfer to only Expense Class Accounts");
				}
				if (FromAccountClassId == 5 && ToAccountClassId != 5)
				{
					CmbToChartOfAccount.Focus();
					throw new Exception("Revenue Class Account Transactions can be transfer to only Revenue Class Accounts");
				}
				if ((FromAccountAcTyped == 2 || FromAccountAcTyped == 15) && ToAccountAcTypeId != 2 && ToAccountAcTypeId != 15)
				{
					string obj = ((FromAccountAcTyped == 2) ? "Cash Equivalent" : "Bank Equivalent");
					CmbToChartOfAccount.Focus();
					throw new Exception(obj + " account transactions can only be transferred to Cash Equivalent or Bank Equivalent accounts.");
				}
			}
			else if (((UltraGridBase)CmbThirdLevelAccounts).ActiveRow == null || Conversion.ToInt(CmbThirdLevelAccounts.Value) == 0)
			{
				MessageBox.Show("Third Level Account field is required");
				CmbThirdLevelAccounts.Focus();
				return;
			}
			VoucherHead.ChartOfAccountParentUpdate(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, Conversion.ToInt(CmbDetailAccount.Value), chkAccountTransactionsMerge.Checked ? Conversion.ToInt(CmbToChartOfAccount.Value) : Conversion.ToInt(CmbThirdLevelAccounts.Value), (!chkAccountTransactionsMerge.Checked) ? 1 : 2);
			MessageBox.Show("Parent Account Change Successfully");
			((Control)(object)CmbDetailAccount).Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkAccountTransactionsMerge_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (chkAccountTransactionsMerge.Checked)
			{
				lblThirdLevelAccount.Visible = false;
				((Control)(object)CmbThirdLevelAccounts).Visible = false;
				lblToAccountTitle.Visible = true;
				((Control)(object)CmbToChartOfAccount).Visible = true;
			}
			else
			{
				lblThirdLevelAccount.Visible = true;
				((Control)(object)CmbThirdLevelAccounts).Visible = true;
				lblToAccountTitle.Visible = false;
				((Control)(object)CmbToChartOfAccount).Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDetailAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			if (!chkAccountTransactionsMerge.Checked)
			{
				return;
			}
			int fromAccountClassId = Conversion.ToInt(((UltraDropDownBase)CmbDetailAccount).SelectedRow.Cells["AccountClassId"].Value);
			int fromAccountTypeId = Conversion.ToInt(((UltraDropDownBase)CmbDetailAccount).SelectedRow.Cells["AccountTypeId"].Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("AccountTitle", typeof(string));
			dt.Columns.Add("AccountCode", typeof(string));
			dt.Columns.Add("AccountClass", typeof(string));
			dt.Columns.Add("AccountType", typeof(string));
			dt.Columns.Add("ParentAccountId", typeof(int));
			dt.Columns.Add("ParentAccountTitle", typeof(string));
			dt.Columns.Add("AccountClassId", typeof(int));
			dt.Columns.Add("AccountTypeId", typeof(int));
			List<getGlobalAllAccountsWithCustomGroup> all = clsGlobalVariables.AllAccountsWithCustomGroupId;
			if (fromAccountClassId == 4)
			{
				all = all.Where((getGlobalAllAccountsWithCustomGroup a) => a.AccountClass == 4).ToList();
			}
			else if (fromAccountClassId == 5)
			{
				all = all.Where((getGlobalAllAccountsWithCustomGroup a) => a.AccountClass == 5).ToList();
			}
			else if (fromAccountClassId == 2 && (fromAccountTypeId == 2 || fromAccountTypeId == 15))
			{
				all = all.Where((getGlobalAllAccountsWithCustomGroup a) => a.AccountTypeId == 2 || a.AccountTypeId == 15).ToList();
			}
			else
			{
				all = all.Where((getGlobalAllAccountsWithCustomGroup a) => a.AccountClass != 4 && a.AccountClass != 5).ToList();
				all = all.Where((getGlobalAllAccountsWithCustomGroup a) => a.AccountTypeId != 2 && a.AccountTypeId != 15).ToList();
			}
			HashSet<int> seen = new HashSet<int>();
			foreach (getGlobalAllAccountsWithCustomGroup acc in all)
			{
				int id = Conversion.ToInt((object)acc.ChartOfAccountId);
				if (seen.Add(id))
				{
					dt.Rows.Add(id, acc.AccountTitle, acc.AccountCode, acc.AccountClassName, acc.AccountType, acc.ParentAccountId, acc.ParentAccountTitle, acc.AccountClass, acc.AccountTypeId);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbToChartOfAccount, dt, "Id", "AccountTitle", "To Account Title", AllColumns: true, Conversion.ToInt(CmbToChartOfAccount.Value), insertDefaultRow: true);
			UltraGridLayout displayLayout = ((UltraGridBase)CmbToChartOfAccount).DisplayLayout;
			UltraGridBand band = ((displayLayout != null) ? displayLayout.Bands[0] : null);
			if (band != null)
			{
				band.Columns["ParentAccountId"].Hidden = true;
				band.Columns["AccountClassId"].Hidden = true;
				band.Columns["AccountTypeId"].Hidden = true;
			}
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
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.printDialog1 = new System.Windows.Forms.PrintDialog();
		this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.CmbParentAccountToFilter = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbThirdLevelAccounts = new UltraCombo();
		this.lblThirdLevelAccount = new System.Windows.Forms.Label();
		this.CmbDetailAccount = new UltraCombo();
		this.lbltocompany = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.chkAccountTransactionsMerge = new System.Windows.Forms.CheckBox();
		this.CmbToChartOfAccount = new UltraCombo();
		this.lblToAccountTitle = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbParentAccountToFilter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbThirdLevelAccounts).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDetailAccount).BeginInit();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbToChartOfAccount).BeginInit();
		base.SuspendLayout();
		this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
		this.printDialog1.UseEXDialog = true;
		this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
		this.dataGridViewTextBoxColumn8.HeaderText = "✓";
		this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
		this.panel1.Controls.Add(this.panel7);
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(537, 156);
		this.panel1.TabIndex = 37;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentAccountToFilter);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDetailAccount);
		this.panel7.Controls.Add(this.lbltocompany);
		this.panel7.Controls.Add(this.lblThirdLevelAccount);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbThirdLevelAccounts);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbToChartOfAccount);
		this.panel7.Controls.Add(this.lblToAccountTitle);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 58);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(537, 98);
		this.panel7.TabIndex = 58;
		this.CmbParentAccountToFilter.AllowNull = (DefaultableBoolean)1;
		this.CmbParentAccountToFilter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentAccountToFilter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentAccountToFilter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentAccountToFilter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentAccountToFilter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentAccountToFilter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentAccountToFilter).Location = new System.Drawing.Point(160, 4);
		((System.Windows.Forms.Control)(object)this.CmbParentAccountToFilter).Name = "CmbParentAccountToFilter";
		((System.Windows.Forms.Control)(object)this.CmbParentAccountToFilter).Size = new System.Drawing.Size(295, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentAccountToFilter).TabIndex = 38;
		((System.Windows.Forms.Control)(object)this.CmbParentAccountToFilter).Leave += new System.EventHandler(CmbParentAccountToFilter_Leave);
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(32, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(84, 17);
		this.label1.TabIndex = 39;
		this.label1.Text = "From Parent";
		this.CmbThirdLevelAccounts.AllowNull = (DefaultableBoolean)1;
		this.CmbThirdLevelAccounts.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbThirdLevelAccounts.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbThirdLevelAccounts.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbThirdLevelAccounts).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbThirdLevelAccounts.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbThirdLevelAccounts).Location = new System.Drawing.Point(160, 58);
		((System.Windows.Forms.Control)(object)this.CmbThirdLevelAccounts).Name = "CmbThirdLevelAccounts";
		((System.Windows.Forms.Control)(object)this.CmbThirdLevelAccounts).Size = new System.Drawing.Size(295, 26);
		((System.Windows.Forms.Control)(object)this.CmbThirdLevelAccounts).TabIndex = 36;
		this.lblThirdLevelAccount.AutoSize = true;
		this.lblThirdLevelAccount.BackColor = System.Drawing.Color.White;
		this.lblThirdLevelAccount.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.lblThirdLevelAccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblThirdLevelAccount.Location = new System.Drawing.Point(32, 63);
		this.lblThirdLevelAccount.Name = "lblThirdLevelAccount";
		this.lblThirdLevelAccount.Size = new System.Drawing.Size(122, 17);
		this.lblThirdLevelAccount.TabIndex = 37;
		this.lblThirdLevelAccount.Text = "3rd Level Accounts";
		this.CmbDetailAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbDetailAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDetailAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDetailAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDetailAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbDetailAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDetailAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDetailAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDetailAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDetailAccount).Location = new System.Drawing.Point(160, 31);
		((System.Windows.Forms.Control)(object)this.CmbDetailAccount).Name = "CmbDetailAccount";
		((System.Windows.Forms.Control)(object)this.CmbDetailAccount).Size = new System.Drawing.Size(295, 26);
		((System.Windows.Forms.Control)(object)this.CmbDetailAccount).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbDetailAccount).Leave += new System.EventHandler(CmbDetailAccount_Leave);
		this.lbltocompany.AutoSize = true;
		this.lbltocompany.BackColor = System.Drawing.Color.White;
		this.lbltocompany.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.lbltocompany.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbltocompany.Location = new System.Drawing.Point(32, 36);
		this.lbltocompany.Name = "lbltocompany";
		this.lbltocompany.Size = new System.Drawing.Size(87, 17);
		this.lbltocompany.TabIndex = 16;
		this.lbltocompany.Text = "Account Title";
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.chkAccountTransactionsMerge);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 29);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(537, 29);
		this.panel6.TabIndex = 44;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(5, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(182, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Accounts To Transfer";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(537, 29);
		this.panel2.TabIndex = 45;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnRefresh, this.btnUpdate });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(537, 27);
		this.toolStrip1.TabIndex = 55;
		this.toolStrip1.Text = "toolStrip1";
		this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(70, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.chkAccountTransactionsMerge.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkAccountTransactionsMerge.AutoSize = true;
		this.chkAccountTransactionsMerge.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkAccountTransactionsMerge.ForeColor = System.Drawing.Color.White;
		this.chkAccountTransactionsMerge.Location = new System.Drawing.Point(351, 5);
		this.chkAccountTransactionsMerge.Name = "chkAccountTransactionsMerge";
		this.chkAccountTransactionsMerge.Size = new System.Drawing.Size(183, 19);
		this.chkAccountTransactionsMerge.TabIndex = 1;
		this.chkAccountTransactionsMerge.Text = "Account Transactions Merge";
		this.chkAccountTransactionsMerge.UseVisualStyleBackColor = true;
		this.chkAccountTransactionsMerge.CheckedChanged += new System.EventHandler(chkAccountTransactionsMerge_CheckedChanged);
		this.CmbToChartOfAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbToChartOfAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbToChartOfAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbToChartOfAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbToChartOfAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbToChartOfAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbToChartOfAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbToChartOfAccount).Location = new System.Drawing.Point(160, 58);
		((System.Windows.Forms.Control)(object)this.CmbToChartOfAccount).Name = "CmbToChartOfAccount";
		((System.Windows.Forms.Control)(object)this.CmbToChartOfAccount).Size = new System.Drawing.Size(295, 26);
		((System.Windows.Forms.Control)(object)this.CmbToChartOfAccount).TabIndex = 40;
		this.lblToAccountTitle.AutoSize = true;
		this.lblToAccountTitle.BackColor = System.Drawing.Color.White;
		this.lblToAccountTitle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.lblToAccountTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblToAccountTitle.Location = new System.Drawing.Point(32, 63);
		this.lblToAccountTitle.Name = "lblToAccountTitle";
		this.lblToAccountTitle.Size = new System.Drawing.Size(105, 17);
		this.lblToAccountTitle.TabIndex = 41;
		this.lblToAccountTitle.Text = "To Account Title";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(537, 156);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "AccountToAccountTransfer";
		this.Text = "Account Transfer";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(AcfrmAcAllocation_FormClosed);
		base.Load += new System.EventHandler(AcfrmAcAllocation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(AcfrmAcAllocation_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbParentAccountToFilter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbThirdLevelAccounts).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDetailAccount).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbToChartOfAccount).EndInit();
		base.ResumeLayout(false);
	}
}

}
