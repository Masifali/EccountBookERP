// using Attachment = global::Architecture.Model.Attachment;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using SupplierCustomer = global::Architecture.Model.Inventory.SupplierCustomer;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using BankReconciliation = global::Architecture.Model.Accounts.BankReconciliation;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Account_Definition
{

public class frmAccountsAllocateToCustomAccountGroupByDocumentType : Form
{
	private UserAccount UserAccount = new UserAccount();

	private int Success;

	private int RecId;

	private bool UpdateMood;

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private Panel panel2;

	internal GridEX grdAllocated;

	private Panel panel6;

	internal UltraCombo cmbAccountTypes;

	private Label label2;

	internal GridEX grdUnAllocated;

	internal UltraCombo CmbCustomGroup;

	private Label label3;

	private Button btnAddAc;

	private Button btnAcClear;

	private Button btnDelete;

	private Panel panel10;

	private Panel panel9;

	private Panel panel11;

	private Panel panel7;

	private Panel panel5;

	private Label label4;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnResetAllocatedUnAllocated;

	private ToolStripButton BtnAllocate;

	private ToolStripButton BtnDeAllocate;

	private ToolStripButton BtnRefresh;

	private Button BtnShow;

	public frmAccountsAllocateToCustomAccountGroupByDocumentType(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmAccountsAllocateToCustomAccountGroupByDocumentType_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1000, 600);
			CustomGroupBind(AccountsAllocateToCustomAccountGroupByDocumentType.GetCustomAccountGroupsByDocumentType(0, (string)null));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomGroupBind(DataTable dtFromDb)
	{
		try
		{
			int Id = Conversion.ToInt(CmbCustomGroup.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("CustomGroupName");
			dt.Columns.Add("Activity");
			dt.Columns.Add("AccountTypeIds");
			dt.Rows.Clear();
			foreach (DataRow dr in dtFromDb.Rows)
			{
				dt.Rows.Add(dr["Id"], dr["CustomGroupName"], dr["Activity"], dr["AccountTypeIds"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCustomGroup, "Id", "CustomGroupName", "Custom Group", ZeroIndex: false);
				((UltraGridBase)CmbCustomGroup).DisplayLayout.Bands[0].Columns[1].Width = 150;
				((UltraGridBase)CmbCustomGroup).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbCustomGroup).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				CommonServices.SetComboValue(CmbCustomGroup, "Id", Id, dt, ActivateRow: true);
			}
			else
			{
				((Control)(object)CmbCustomGroup).Text = string.Empty;
				((UltraGridBase)CmbCustomGroup).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountTypeCombo(string Ids)
	{
		try
		{
			int Id = Conversion.ToInt(cmbAccountTypes.Value);
			DataTable dtRefAccount = AccountTypes.AccountTypes_ReadAllByIds(Ids);
			if (dtRefAccount.Rows.Count > 0)
			{
				DDL.BindDDL(dtRefAccount, cmbAccountTypes, "Id", "AccountType", "Account Type", ZeroIndex: false);
				((UltraGridBase)cmbAccountTypes).DisplayLayout.Bands[0].Columns[1].Width = 171;
				CommonServices.SetComboValue(cmbAccountTypes, "Id", Id, dtRefAccount, ActivateRow: true);
			}
			else
			{
				((Control)(object)cmbAccountTypes).Text = string.Empty;
				((UltraGridBase)cmbAccountTypes).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCustomGroup_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbCustomGroup.Value) > 0)
			{
				AccountTypeCombo(Conversion.ToString(((UltraDropDownBase)CmbCustomGroup).SelectedRow.Cells["AccountTypeIds"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAcClear_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbAccountTypes).Text = string.Empty;
			((Control)(object)CmbCustomGroup).Text = string.Empty;
			grdAllocated.DataSource = null;
			grdUnAllocated.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmAccountsAllocateToCustomAccountGroupByDocumentType_FormClosing(object sender, FormClosingEventArgs e)
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

	private void BtnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			CustomGroupBind(AccountsAllocateToCustomAccountGroupByDocumentType.GetCustomAccountGroupsByDocumentType(0, (string)null));
			if (Conversion.ToInt(CmbCustomGroup.Value) > 0)
			{
				AccountTypeCombo(Conversion.ToString(((UltraDropDownBase)CmbCustomGroup).SelectedRow.Cells["AccountTypeIds"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddAc_Click(object sender, EventArgs e)
	{
		try
		{
			SaveAccountCustomGroup();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SaveAccountCustomGroup()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		try
		{
			if (grdAllocated.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Checked row first");
			}
			AccountsAllocateToCustomAccountGroupByDocumentType accounts = new AccountsAllocateToCustomAccountGroupByDocumentType();
			accounts.AccountsAllocateToCustomAccountGroupByDocumentTypeList = new List<AccountsAllocateToCustomAccountGroupByDocumentType>();
			GridEXRow[] checkedRows = grdAllocated.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				AccountsAllocateToCustomAccountGroupByDocumentType groups = new AccountsAllocateToCustomAccountGroupByDocumentType();
				groups.AccountId = Conversion.ToInt(r.Cells["ChartOfAccountId"].Value);
				groups.CustomAccountGroupsByDocumentTypeId = Conversion.ToInt(CmbCustomGroup.Value);
				groups.CompanyId = UserAccount.CompanyId;
				groups.EntryUserId = UserAccount.ID;
				groups.ModifyUserId = UserAccount.ID;
				accounts.AccountsAllocateToCustomAccountGroupByDocumentTypeList.Add(groups);
			}
			if (AccountsAllocateToCustomAccountGroupByDocumentType.Save(accounts) > 0)
			{
				MessageBox.Show("Saved Successfully");
			}
			BtnShow_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdUnAllocated.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked Row's first To Un-Allocate");
			}
			int CustomGroupId = Conversion.ToInt(CmbCustomGroup.Value);
			if (CustomGroupId > 0)
			{
				if (MessageBox.Show("Are you sure to UnAllocate?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
				{
					string ids = null;
					GridEXRow[] checkedRows = grdUnAllocated.GetCheckedRows();
					foreach (GridEXRow item in checkedRows)
					{
						ids = ids + Conversion.ToString(item.Cells["ChartOfAccountId"].Value) + ",";
					}
					AccountsAllocateToCustomAccountGroupByDocumentType.DeleteById(UserAccount.OrganizationId, UserAccount.CompanyId, ids, CustomGroupId);
					MessageBox.Show("Record Remove successfully");
					BtnShow_Click(null, null);
				}
				return;
			}
			throw new Exception("Select a Row in CustomGroup First To Un-Allocate");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnShow_Click(object sender, EventArgs e)
	{
		try
		{
			int OrganizationId = UserAccount.OrganizationId;
			int CompanyId = UserAccount.CompanyId;
			int CustomGroupId = Conversion.ToInt(CmbCustomGroup.Value);
			int AccountTypeId = Conversion.ToInt(cmbAccountTypes.Value);
			UnAllocatedData(OrganizationId, CompanyId, CustomGroupId, AccountTypeId);
			AllocatedData(OrganizationId, CompanyId, CustomGroupId, AccountTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllocatedData(int OrganizationId, int CompanyId, int CustomGroupId, int AccountTypeId)
	{
		try
		{
			DataTable dtGrd = AccountsAllocateToCustomAccountGroupByDocumentType.AccountCustomGroup_UnAllocatedData(OrganizationId, CompanyId, CustomGroupId, AccountTypeId);
			if (dtGrd.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ChartOfAccountId");
				dt.Columns.Add("AccountCode");
				dt.Columns.Add("AccountTitle");
				dt.Rows.Clear();
				for (int i = 0; i < dtGrd.Rows.Count; i++)
				{
					dt.Rows.Add(dtGrd.Rows[i]["CustomGroupId"], dtGrd.Rows[i]["ChartOfAccountId"], dtGrd.Rows[i]["AccountCode"], dtGrd.Rows[i]["AccountTitle"]);
				}
				grdAllocated.DataSource = dt;
				grdAllocated.RetrieveStructure();
				grdAllocated.RootTable.Columns["Id"].Visible = false;
				grdAllocated.RootTable.Columns["ChartOfAccountId"].Visible = false;
				grdAllocated.RootTable.Columns["AccountTitle"].Width = 270;
				grdAllocated.RootTable.Columns["AccountCode"].Width = 100;
				grdAllocated.RootTable.Columns.Add("Select");
				grdAllocated.RootTable.Columns["Select"].ActAsSelector = true;
				grdAllocated.RootTable.Columns["Select"].UseHeaderSelector = true;
				grdAllocated.RootTable.Columns["Select"].Width = 30;
				grdAllocated.RootTable.Columns["Select"].Position = 0;
				grdAllocated.FrozenColumns = 1;
			}
			else
			{
				grdAllocated.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UnAllocatedData(int OrganizationId, int CompanyId, int CustomGroupId, int AccountTypeId)
	{
		try
		{
			DataTable dtCustom = AccountsAllocateToCustomAccountGroupByDocumentType.AccountCustomGroup_AllocatedData(OrganizationId, CompanyId, CustomGroupId, AccountTypeId);
			if (dtCustom.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ChartOfAccountId");
				dt.Columns.Add("AccountCode");
				dt.Columns.Add("AccountTitle");
				for (int i = 0; i < dtCustom.Rows.Count; i++)
				{
					dt.Rows.Add(dtCustom.Rows[i]["CustomGroupId"], dtCustom.Rows[i]["ChartOfAccountId"], dtCustom.Rows[i]["AccountCode"], dtCustom.Rows[i]["AccountTitle"]);
				}
				grdUnAllocated.DataSource = dt;
				grdUnAllocated.RetrieveStructure();
				grdUnAllocated.RootTable.Columns["Id"].Visible = false;
				grdUnAllocated.RootTable.Columns["ChartOfAccountId"].Visible = false;
				grdUnAllocated.RootTable.Columns["AccountTitle"].Width = 270;
				grdUnAllocated.RootTable.Columns["AccountCode"].Width = 100;
				grdUnAllocated.RootTable.Columns.Add("Select");
				grdUnAllocated.RootTable.Columns["Select"].ActAsSelector = true;
				grdUnAllocated.RootTable.Columns["Select"].UseHeaderSelector = true;
				grdUnAllocated.RootTable.Columns["Select"].Width = 30;
				grdUnAllocated.RootTable.Columns["Select"].Position = 0;
				grdUnAllocated.FrozenColumns = 1;
			}
			else
			{
				grdUnAllocated.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmAccountsAllocateToCustomAccountGroupByDocumentType_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnAcClear_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.F5)
		{
			CmbCustomGroup.Focus();
		}
		if (e.Control && e.KeyCode == Keys.Delete)
		{
			btnDelete_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.R)
		{
			BtnRefresh_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.Down)
		{
			((Control)(object)grdAllocated).Focus();
		}
		if (e.Control && e.KeyCode == Keys.Up)
		{
			CmbCustomGroup.Focus();
		}
		if (e.Control && e.Alt)
		{
			MakeShortCutKeys();
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+A", "For Allocate");
			dt.Rows.Add("Ctrl+Delete", "For UnAllocate");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on CustomGroup");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On CustomGroup");
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

	private void btnShortCutKeys_Click(object sender, EventArgs e)
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
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		GridEXLayout grdUnAllocated_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdAllocated_DesignTimeLayout = new GridEXLayout();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.frmAccountsAllocateToCustomAccountGroupByDocumentType));
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdUnAllocated = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.btnDelete = new System.Windows.Forms.Button();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdAllocated = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.btnAddAc = new System.Windows.Forms.Button();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnResetAllocatedUnAllocated = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.BtnAllocate = new System.Windows.Forms.ToolStripButton();
		this.BtnDeAllocate = new System.Windows.Forms.ToolStripButton();
		this.BtnShow = new System.Windows.Forms.Button();
		this.btnAcClear = new System.Windows.Forms.Button();
		this.CmbCustomGroup = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbAccountTypes = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.panel2.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdUnAllocated).BeginInit();
		this.panel11.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAllocated).BeginInit();
		this.panel5.SuspendLayout();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTypes).BeginInit();
		this.panel10.SuspendLayout();
		base.SuspendLayout();
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.panel9);
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel2.Location = new System.Drawing.Point(0, 27);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(981, 534);
		this.panel2.TabIndex = 1;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdUnAllocated);
		this.panel9.Controls.Add(this.panel11);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(495, 75);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(486, 459);
		this.panel9.TabIndex = 71;
		this.grdUnAllocated.AllowEdit = (InheritableBoolean)2;
		grdUnAllocated_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdUnAllocated.DesignTimeLayout = grdUnAllocated_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdUnAllocated).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdUnAllocated.DynamicFiltering = true;
		this.grdUnAllocated.FilterMode = (FilterMode)2;
		this.grdUnAllocated.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdUnAllocated).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdUnAllocated.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdUnAllocated).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdUnAllocated).Name = "grdUnAllocated";
		((ScrollableBase)this.grdUnAllocated).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdUnAllocated).Size = new System.Drawing.Size(486, 432);
		((System.Windows.Forms.Control)(object)this.grdUnAllocated).TabIndex = 0;
		this.grdUnAllocated.TotalRow = (InheritableBoolean)1;
		this.grdUnAllocated.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdUnAllocated).VisualStyle = (VisualStyle)4;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.btnDelete);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(486, 27);
		this.panel11.TabIndex = 70;
		this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnDelete.AutoSize = true;
		this.btnDelete.BackColor = System.Drawing.Color.White;
		this.btnDelete.FlatAppearance.BorderSize = 0;
		this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.btnDelete.ForeColor = System.Drawing.Color.White;
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnDelete.Location = new System.Drawing.Point(451, -3);
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(33, 30);
		this.btnDelete.TabIndex = 1;
		this.btnDelete.UseVisualStyleBackColor = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdAllocated);
		this.panel7.Controls.Add(this.panel5);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel7.Location = new System.Drawing.Point(0, 75);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(495, 459);
		this.panel7.TabIndex = 1;
		this.grdAllocated.AllowEdit = (InheritableBoolean)2;
		grdAllocated_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdAllocated.DesignTimeLayout = grdAllocated_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdAllocated).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdAllocated.DynamicFiltering = true;
		this.grdAllocated.FilterMode = (FilterMode)2;
		this.grdAllocated.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdAllocated).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAllocated.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdAllocated).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdAllocated).Name = "grdAllocated";
		((ScrollableBase)this.grdAllocated).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAllocated).Size = new System.Drawing.Size(495, 432);
		((System.Windows.Forms.Control)(object)this.grdAllocated).TabIndex = 0;
		this.grdAllocated.TotalRow = (InheritableBoolean)1;
		this.grdAllocated.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAllocated).VisualStyle = (VisualStyle)4;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.btnAddAc);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(495, 27);
		this.panel5.TabIndex = 69;
		this.btnAddAc.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnAddAc.BackColor = System.Drawing.Color.Teal;
		this.btnAddAc.FlatAppearance.BorderSize = 0;
		this.btnAddAc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddAc.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnAddAc.ForeColor = System.Drawing.Color.White;
		this.btnAddAc.Image = Architecture.WinApp.Properties.Resources.Forward2;
		this.btnAddAc.Location = new System.Drawing.Point(464, 2);
		this.btnAddAc.Name = "btnAddAc";
		this.btnAddAc.Size = new System.Drawing.Size(28, 24);
		this.btnAddAc.TabIndex = 2;
		this.btnAddAc.UseVisualStyleBackColor = false;
		this.btnAddAc.Click += new System.EventHandler(btnAddAc_Click);
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Controls.Add(this.BtnShow);
		this.panel6.Controls.Add(this.btnAcClear);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomGroup);
		this.panel6.Controls.Add(this.label3);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccountTypes);
		this.panel6.Controls.Add(this.label2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(981, 75);
		this.panel6.TabIndex = 0;
		this.toolStrip2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.BtnResetAllocatedUnAllocated, this.BtnRefresh, this.BtnAllocate, this.BtnDeAllocate });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(981, 27);
		this.toolStrip2.TabIndex = 10;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnResetAllocatedUnAllocated.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnResetAllocatedUnAllocated.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnResetAllocatedUnAllocated.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnResetAllocatedUnAllocated.Name = "BtnResetAllocatedUnAllocated";
		this.BtnResetAllocatedUnAllocated.Size = new System.Drawing.Size(68, 24);
		this.BtnResetAllocatedUnAllocated.Text = "&Reset";
		this.BtnResetAllocatedUnAllocated.Click += new System.EventHandler(btnAcClear_Click);
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "&Refresh";
		this.BtnRefresh.Click += new System.EventHandler(BtnRefresh_Click);
		this.BtnAllocate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAllocate.Image = Architecture.WinApp.Properties.Resources.Forward2_removebg_preview;
		this.BtnAllocate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnAllocate.Name = "BtnAllocate";
		this.BtnAllocate.Size = new System.Drawing.Size(84, 24);
		this.BtnAllocate.Text = "&Allocate";
		this.BtnAllocate.Click += new System.EventHandler(btnAddAc_Click);
		this.BtnDeAllocate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnDeAllocate.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.BtnDeAllocate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDeAllocate.Name = "BtnDeAllocate";
		this.BtnDeAllocate.Size = new System.Drawing.Size(108, 24);
		this.BtnDeAllocate.Text = "&Un-Allocate";
		this.BtnDeAllocate.Click += new System.EventHandler(btnDelete_Click);
		this.BtnShow.BackColor = System.Drawing.Color.Teal;
		this.BtnShow.FlatAppearance.BorderSize = 0;
		this.BtnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnShow.ForeColor = System.Drawing.Color.White;
		this.BtnShow.Location = new System.Drawing.Point(540, 45);
		this.BtnShow.Name = "BtnShow";
		this.BtnShow.Size = new System.Drawing.Size(55, 24);
		this.BtnShow.TabIndex = 2;
		this.BtnShow.Text = "Show";
		this.BtnShow.UseVisualStyleBackColor = false;
		this.BtnShow.Click += new System.EventHandler(BtnShow_Click);
		this.btnAcClear.BackColor = System.Drawing.Color.Teal;
		this.btnAcClear.FlatAppearance.BorderSize = 0;
		this.btnAcClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAcClear.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnAcClear.ForeColor = System.Drawing.Color.White;
		this.btnAcClear.Location = new System.Drawing.Point(598, 45);
		this.btnAcClear.Name = "btnAcClear";
		this.btnAcClear.Size = new System.Drawing.Size(55, 24);
		this.btnAcClear.TabIndex = 2;
		this.btnAcClear.Text = "Reset";
		this.btnAcClear.UseVisualStyleBackColor = false;
		this.btnAcClear.Click += new System.EventHandler(btnAcClear_Click);
		this.CmbCustomGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCustomGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Location = new System.Drawing.Point(79, 44);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Name = "CmbCustomGroup";
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Size = new System.Drawing.Size(186, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).TextChanged += new System.EventHandler(CmbCustomGroup_TextChanged);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(2, 50);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(75, 15);
		this.label3.TabIndex = 9;
		this.label3.Text = "Group Name";
		this.cmbAccountTypes.AllowNull = (DefaultableBoolean)1;
		this.cmbAccountTypes.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccountTypes.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccountTypes.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTypes).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTypes).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbAccountTypes.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).Location = new System.Drawing.Point(365, 44);
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).Name = "cmbAccountTypes";
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(271, 50);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(90, 15);
		this.label2.TabIndex = 8;
		this.label2.Text = "Accounts Types";
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label4);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(984, 27);
		this.panel10.TabIndex = 3;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 5);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(416, 16);
		this.label4.TabIndex = 9;
		this.label4.Text = "Accounts Allocate To Custom Account Group By DocumentType";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(984, 561);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel10);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmAccountsAllocateToCustomAccountGroupByDocumentType";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmAccountsAllocateToCustomAccountGroupByDocumentType_FormClosing);
		base.Load += new System.EventHandler(frmAccountsAllocateToCustomAccountGroupByDocumentType_Load);
		this.panel2.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdUnAllocated).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAllocated).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTypes).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
