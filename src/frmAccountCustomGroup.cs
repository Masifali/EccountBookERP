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
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using SupplierCustomer = global::Architecture.Model.Inventory.SupplierCustomer;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using BankReconciliation = global::Architecture.Model.Accounts.BankReconciliation;
namespace Architecture.WinApp.Account_Definition
{
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using CommonServices = global::Architecture.WinApp.Common.CommonServices;
using Bank = global::Architecture.Model.Bank;
using City = global::Architecture.BLL.City;
using CheqBookHeader = global::Architecture.Model.Accounts.CheqBookHeader;
using CheqBookDetail = global::Architecture.Model.Accounts.CheqBookDetail;
using COAAllocation = global::Architecture.Model.Accounts.COAAllocation;
using ChartOfAccountMultiLingo = global::Architecture.Model.Accounts.ChartOfAccountMultiLingo;
using TaxesTypes = global::Architecture.Model.Inventory.TaxesTypes;
using SupplierCustomerShipToAddress = global::Architecture.Model.Inventory.SupplierCustomerShipToAddress;
using AccountsOpeningBalances = global::Architecture.BLL.Accounts.AccountsOpeningBalances;
using ChartofAccount = global::Architecture.BLL.Accounts.ChartofAccount;
using ShortCutKeyPopUp = global::Architecture.WinApp.Common.ShortCutKeyPopUp;
using Constants = global::Architecture.WinApp.Common.Constants;
using GridEX_Helper = global::Architecture.WinApp.Common.GridEX_Helper;
using InfragisticsHelper = global::Architecture.WinApp.Common.InfragisticsHelper;
using HierarchicalMode = Janus.Windows.GridEX.HierarchicalMode;
using SelfReferencingAggregationMode = Janus.Windows.GridEX.SelfReferencingAggregationMode;

using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
using AcLookUps = Architecture.Model.Accounts.AcLookUps;
using AccountsCustomGroups = global::Architecture.Model.Accounts.AccountsCustomGroups;

public class frmAccountCustomGroup : Form
{
	private UserAccount UserAccount = new UserAccount();

	private int Success;

	private int RecId;

	private bool UpdateMood;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtParentAccount = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Button btnAdd;

	private TextBox txtGroupName;

	private Label label1;

	private Panel panel2;

	internal GridEX grdLookUp;

	internal GridEX grdAllocated;

	private Panel panel6;

	internal UltraCombo cmbAccountTypes;

	private Label label2;

	internal GridEX grdUnAllocated;

	internal UltraCombo cmbGroupName;

	private Label label3;

	private Button btnAddAc;

	private Button btnClear;

	private Button btnAcClear;

	private Button btnDelete;

	private ToolStrip toolStrip1;

	private ToolStripButton BtnResetGroup;

	private ToolStripButton BtnSaveGroup;

	private ToolStripButton btnShortCutKeys;

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

	private ToolStripButton btnPartyCustomGroup;

	internal UltraCombo CmbParentAccount;

	private Label label5;

	public frmAccountCustomGroup(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmAccountCustomGroup_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 600);
			ThirdLevelAccountsDbCall();
			BindThirdLevelAccounts();
			GridBind();
			AccountTypeCombo();
			LookUpBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountTypeCombo()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(cmbAccountTypes.Value);
			DataTable dtAcc = global::Architecture.BLL.Accounts.AccountTypes.GetAll(new global::Architecture.Model.Accounts.AccountTypes());
			if (dtAcc.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtAcc, cmbAccountTypes, "Id", "AccountType", "Account Type", false);
				((UltraGridBase)cmbAccountTypes).DisplayLayout.Bands[0].Columns[1].Width = 171;
				if (Id > 0)
				{
					if (dtAcc.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbAccountTypes.Value = Id;
					}
					else
					{
						((Control)(object)cmbAccountTypes).Text = string.Empty;
					}
				}
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

	private void LookUpBind()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(cmbGroupName.Value);
			DataTable dtLookUp = global::Architecture.BLL.Accounts.AcLookUps.GetByProfileTypeId(new AcLookUps
			{
				AcLookUpTypesId = 1,
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtLookUp.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtLookUp, cmbGroupName, "Id", "AcLookUpsDescription", "Account Group", false);
				((UltraGridBase)cmbGroupName).DisplayLayout.Bands[0].Columns[1].Width = 171;
				if (Id > 0)
				{
					if (dtLookUp.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbGroupName.Value = Id;
					}
					else
					{
						((Control)(object)cmbGroupName).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbGroupName).Text = string.Empty;
				((UltraGridBase)cmbGroupName).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ThirdLevelAccountsDbCall()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtParentAccount.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Account_Level = 3;
			dtParentAccount = ChartofAccount.ReadAllAccountgroup(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindThirdLevelAccounts(int AccountTypeId = 0)
	{
		try
		{
			if (dtParentAccount.Rows.Count <= 0 || dtParentAccount == null || dtParentAccount.Rows.Count == 0)
			{
				return;
			}
			IEnumerable<DataRow> rows = dtParentAccount.AsEnumerable();
			if (AccountTypeId > 0)
			{
				rows = rows.Where((DataRow r) => r.Field<int>("AccountTypeId") == AccountTypeId);
			}
			if (!rows.Any())
			{
				return;
			}
			DataTable dtGroupAc = new DataTable();
			dtGroupAc.Columns.Add("Id", typeof(int));
			dtGroupAc.Columns.Add("AccountTitle", typeof(string));
			dtGroupAc.Columns.Add("AccountCode", typeof(string));
			dtGroupAc.Columns.Add("AccountType", typeof(string));
			dtGroupAc.Columns.Add("AccountClass", typeof(string));
			dtGroupAc.Columns.Add("ParentAccount(2nd)", typeof(string));
			foreach (DataRow row in rows)
			{
				dtGroupAc.Rows.Add(row["Id"], row["AccountTitle"], row["AccountCode"], row["AccountType"], row["ClassName"], row["ParentAccountTitle"]);
			}
			DDL.BindDDL(dtGroupAc, CmbParentAccount, "Id", "AccountTitle", "Parent Account(3rd)", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private bool Validation()
	{
		try
		{
			if (txtGroupName.Text.Trim() == string.Empty)
			{
				MessageBox.Show("Group Name Required");
				txtGroupName.Focus();
				return false;
			}
			return true;
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (btnAdd.Text == "Add")
			{
				InsertLookUp();
				LookUpBind();
			}
			if (btnAdd.Text == "Update")
			{
				UpdateLookUp();
				LookUpBind();
			}
			BtnRefresh_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InsertLookUp()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		try
		{
			if (Validation())
			{
				AcLookUps acLook = new AcLookUps();
				acLook.AcLookUpsDescription = txtGroupName.Text;
				acLook.AcLookUpTypesId = 1;
				acLook.OrganizationId = UserAccount.OrganizationId;
				acLook.CompanyId = UserAccount.CompanyId;
				Success = global::Architecture.BLL.Accounts.AcLookUps.Save(acLook);
				if (Success > 0)
				{
					MessageBox.Show("Record Saved Successfully");
					Reset();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateLookUp()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (Validation())
			{
				AcLookUps acLook = new AcLookUps();
				acLook.Id = RecId;
				acLook.AcLookUpsDescription = txtGroupName.Text;
				acLook.AcLookUpTypesId = 1;
				acLook.OrganizationId = UserAccount.OrganizationId;
				acLook.CompanyId = UserAccount.CompanyId;
				Success = global::Architecture.BLL.Accounts.AcLookUps.Save(acLook);
				if (Success > 0)
				{
					MessageBox.Show("Record Update Successfully");
					Reset();
				}
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
			txtGroupName.Text = string.Empty;
			UpdateMood = false;
			btnAdd.Text = "Add";
			BtnSaveGroup.Text = "Save";
			GridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnClear_Click(object sender, EventArgs e)
	{
		try
		{
			btnAdd.Text = "Add";
			BtnSaveGroup.Text = "Save";
			txtGroupName.Text = string.Empty;
			GridBind();
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
			((Control)(object)cmbGroupName).Text = string.Empty;
			grdAllocated.DataSource = null;
			grdUnAllocated.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmAccountCustomGroup_FormClosing(object sender, FormClosingEventArgs e)
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
			AccountTypeCombo();
			LookUpBind();
			ThirdLevelAccountsDbCall();
			BindThirdLevelAccounts(Conversion.ToInt(cmbAccountTypes.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridBind()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		try
		{
			DataTable dtLookUp = new DataTable();
			dtLookUp = global::Architecture.BLL.Accounts.AcLookUps.GetAll(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				TypeId = 1
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Description");
			dt.Columns.Add("LookTypeId");
			for (int i = 0; i < dtLookUp.Rows.Count; i++)
			{
				dt.Rows.Add(dtLookUp.Rows[i]["Id"], dtLookUp.Rows[i]["AcLookUpsDescription"], dtLookUp.Rows[i]["AcLookUpTypesId"]);
			}
			grdLookUp.DataSource = dt;
			grdLookUp.RetrieveStructure();
			grdLookUp.RootTable.Columns["Id"].Visible = false;
			grdLookUp.RootTable.Columns["LookTypeId"].Visible = false;
			grdLookUp.RootTable.Columns["Description"].Width = 300;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdLookUp_DoubleClick(object sender, EventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			RecId = Conversion.ToInt((object)grdLookUp.CurrentRow.Cells[0].Value.ToString());
			int recId = RecId;
			AcLookUps ups = new AcLookUps();
			ups = global::Architecture.BLL.Accounts.AcLookUps.GetById(recId);
			txtGroupName.Text = Conversion.ToString((object)ups.AcLookUpsDescription);
			btnAdd.Text = "Update";
			BtnSaveGroup.Text = "Update";
			UpdateMood = true;
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
			AccountsCustomGroups accounts = new AccountsCustomGroups();
			accounts.GetList = new List<AccountsCustomGroups>();
			GridEXRow[] checkedRows = grdAllocated.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				AccountsCustomGroups groups = new AccountsCustomGroups();
				groups.ChartOfAccountId = Conversion.ToInt(r.Cells["ChartOfAccountId"].Value);
				groups.AcLookUpsId = Conversion.ToInt(cmbGroupName.Value);
				groups.organizationId = UserAccount.OrganizationId;
				groups.companyId = UserAccount.CompanyId;
				groups.EntryUserId = UserAccount.ID;
				groups.ModifyUserId = UserAccount.ID;
				accounts.GetList.Add(groups);
			}
			if (global::Architecture.BLL.Accounts.AccountsCustomGroups.Save(accounts) > 0)
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
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		try
		{
			if (grdUnAllocated.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked row first");
			}
			AccountsCustomGroups obj = new AccountsCustomGroups();
			obj.GetList = new List<AccountsCustomGroups>();
			GridEXRow[] checkedRows = grdUnAllocated.GetCheckedRows();
			foreach (GridEXRow item in checkedRows)
			{
				AccountsCustomGroups detail = new AccountsCustomGroups();
				detail.AcLookUpsId = Conversion.ToInt(item.Cells["Id"].Value);
				detail.ChartOfAccountId = Conversion.ToInt(item.Cells["ChartOfAccountId"].Value);
				obj.GetList.Add(detail);
			}
			ChartofAccount.DeleteAccountsFromCustomGroup(obj);
			MessageBox.Show("Record Remove successfully");
			BtnShow_Click(null, null);
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
			int OrganizationId = UserAccount.OrganizationId;
			int CompanyId = UserAccount.CompanyId;
			int CustomGroupId = Conversion.ToInt(cmbGroupName.Value);
			int AccountTypeId = Conversion.ToInt(cmbAccountTypes.Value);
			int ParentAccountId = Conversion.ToInt(CmbParentAccount.Value);
			UnAllocatedData(OrganizationId, CompanyId, CustomGroupId, AccountTypeId, ParentAccountId);
			AllocatedData(OrganizationId, CompanyId, CustomGroupId, AccountTypeId, ParentAccountId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllocatedData(int OrganizationId, int CompanyId, int CustomGroupId, int AccountTypeId, int ParentAccountId)
	{
		try
		{
			DataTable dtGrd = global::Architecture.BLL.Accounts.AccountsCustomGroups.AccountCustomGroup_UnAllocatedData(OrganizationId, CompanyId, CustomGroupId, AccountTypeId, ParentAccountId);
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
					dt.Rows.Add(dtGrd.Rows[i]["AcLookUpsId"], dtGrd.Rows[i]["ChartOfAccountId"], dtGrd.Rows[i]["AccountCode"], dtGrd.Rows[i]["AccountTitle"]);
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

	private void UnAllocatedData(int OrganizationId, int CompanyId, int CustomGroupId, int AccountTypeId, int ParentAccountId)
	{
		try
		{
			DataTable dtCustom = global::Architecture.BLL.Accounts.AccountsCustomGroups.AccountCustomGroup_AllocatedData(OrganizationId, CompanyId, CustomGroupId, AccountTypeId, ParentAccountId);
			if (dtCustom.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ChartOfAccountId");
				dt.Columns.Add("AccountCode");
				dt.Columns.Add("AccountTitle");
				for (int i = 0; i < dtCustom.Rows.Count; i++)
				{
					dt.Rows.Add(dtCustom.Rows[i]["AcLookUpsId"], dtCustom.Rows[i]["ChartOfAccountId"], dtCustom.Rows[i]["AccountCode"], dtCustom.Rows[i]["AccountTitle"]);
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

	private void frmAccountCustomGroup_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.Control && e.KeyCode == Keys.S)
		{
			btnAdd_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.U)
		{
			btnAdd_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			if (((Control)(object)grdAllocated).Focus() || ((Control)(object)grdUnAllocated).Focus())
			{
				btnAcClear_Click(null, null);
			}
			else
			{
				btnClear_Click(null, null);
			}
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.F5)
		{
			if (((Control)(object)grdAllocated).Focus() || ((Control)(object)grdUnAllocated).Focus() || panel6.Focused || panel6.ContainsFocus)
			{
				cmbGroupName.Focus();
			}
			else
			{
				txtGroupName.Focus();
			}
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
			if (panel4.Focused || panel4.ContainsFocus)
			{
				((Control)(object)grdLookUp).Focus();
			}
			else
			{
				((Control)(object)grdAllocated).Focus();
			}
		}
		if (e.Control && e.KeyCode == Keys.Up)
		{
			if (((Control)(object)grdAllocated).Focus() || ((Control)(object)grdUnAllocated).Focus())
			{
				cmbGroupName.Focus();
			}
			else
			{
				txtGroupName.Focus();
			}
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
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

	private void btnPartyCustomGroup_Click(object sender, EventArgs e)
	{
		try
		{
			// new frmPartyCustomGroup(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbAccountTypes_Leave(object sender, EventArgs e)
	{
		try
		{
			BindThirdLevelAccounts(Conversion.ToInt(cmbAccountTypes.Value));
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
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		GridEXLayout grdLookUp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdUnAllocated_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdAllocated_DesignTimeLayout = new GridEXLayout();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.frmAccountCustomGroup));
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdLookUp = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.BtnResetGroup = new System.Windows.Forms.ToolStripButton();
		this.BtnSaveGroup = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.btnClear = new System.Windows.Forms.Button();
		this.btnAdd = new System.Windows.Forms.Button();
		this.txtGroupName = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
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
		this.btnPartyCustomGroup = new System.Windows.Forms.ToolStripButton();
		this.BtnShow = new System.Windows.Forms.Button();
		this.btnAcClear = new System.Windows.Forms.Button();
		this.cmbGroupName = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbAccountTypes = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbParentAccount = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdLookUp).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdUnAllocated).BeginInit();
		this.panel11.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAllocated).BeginInit();
		this.panel5.SuspendLayout();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbGroupName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTypes).BeginInit();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbParentAccount).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdLookUp);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(386, 605);
		this.panel1.TabIndex = 0;
		this.grdLookUp.AllowEdit = (InheritableBoolean)2;
		grdLookUp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdLookUp.DesignTimeLayout = grdLookUp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdLookUp).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdLookUp.DynamicFiltering = true;
		this.grdLookUp.FilterMode = (FilterMode)2;
		this.grdLookUp.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdLookUp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdLookUp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdLookUp).Location = new System.Drawing.Point(0, 87);
		((System.Windows.Forms.Control)(object)this.grdLookUp).Name = "grdLookUp";
		((ScrollableBase)this.grdLookUp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdLookUp).Size = new System.Drawing.Size(386, 518);
		((System.Windows.Forms.Control)(object)this.grdLookUp).TabIndex = 1;
		this.grdLookUp.TotalRow = (InheritableBoolean)1;
		this.grdLookUp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdLookUp).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdLookUp).DoubleClick += new System.EventHandler(grdLookUp_DoubleClick);
		this.panel4.Controls.Add(this.toolStrip1);
		this.panel4.Controls.Add(this.btnClear);
		this.panel4.Controls.Add(this.btnAdd);
		this.panel4.Controls.Add(this.txtGroupName);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(386, 87);
		this.panel4.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.BtnResetGroup, this.BtnSaveGroup, this.btnShortCutKeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(386, 27);
		this.toolStrip1.TabIndex = 3;
		this.toolStrip1.Text = "toolStrip1";
		this.BtnResetGroup.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnResetGroup.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnResetGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnResetGroup.Name = "BtnResetGroup";
		this.BtnResetGroup.Size = new System.Drawing.Size(68, 24);
		this.BtnResetGroup.Text = "&Reset";
		this.BtnResetGroup.Click += new System.EventHandler(btnClear_Click);
		this.BtnSaveGroup.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSaveGroup.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.BtnSaveGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSaveGroup.Name = "BtnSaveGroup";
		this.BtnSaveGroup.Size = new System.Drawing.Size(64, 24);
		this.BtnSaveGroup.Text = "&Save";
		this.BtnSaveGroup.Click += new System.EventHandler(btnAdd_Click);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKeys.Text = "ShortCut Keys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.btnClear.BackColor = System.Drawing.Color.Teal;
		this.btnClear.FlatAppearance.BorderSize = 0;
		this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnClear.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnClear.ForeColor = System.Drawing.Color.White;
		this.btnClear.Location = new System.Drawing.Point(328, 46);
		this.btnClear.Name = "btnClear";
		this.btnClear.Size = new System.Drawing.Size(52, 23);
		this.btnClear.TabIndex = 2;
		this.btnClear.Text = "Reset";
		this.btnClear.UseVisualStyleBackColor = false;
		this.btnClear.Click += new System.EventHandler(btnClear_Click);
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatAppearance.BorderSize = 0;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(270, 46);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(56, 23);
		this.btnAdd.TabIndex = 1;
		this.btnAdd.Text = "Add";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.txtGroupName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGroupName.Location = new System.Drawing.Point(82, 46);
		this.txtGroupName.Name = "txtGroupName";
		this.txtGroupName.Size = new System.Drawing.Size(186, 23);
		this.txtGroupName.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 50);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(75, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Group Name";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.panel9);
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel2.Location = new System.Drawing.Point(386, 27);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(795, 605);
		this.panel2.TabIndex = 1;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdUnAllocated);
		this.panel9.Controls.Add(this.panel11);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(408, 87);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(387, 518);
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
		((System.Windows.Forms.Control)(object)this.grdUnAllocated).Size = new System.Drawing.Size(387, 491);
		((System.Windows.Forms.Control)(object)this.grdUnAllocated).TabIndex = 0;
		this.grdUnAllocated.TotalRow = (InheritableBoolean)1;
		this.grdUnAllocated.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdUnAllocated).VisualStyle = (VisualStyle)4;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.btnDelete);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(387, 27);
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
		this.btnDelete.Location = new System.Drawing.Point(352, -3);
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(33, 30);
		this.btnDelete.TabIndex = 1;
		this.btnDelete.UseVisualStyleBackColor = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdAllocated);
		this.panel7.Controls.Add(this.panel5);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel7.Location = new System.Drawing.Point(0, 87);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(408, 518);
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
		((System.Windows.Forms.Control)(object)this.grdAllocated).Size = new System.Drawing.Size(408, 491);
		((System.Windows.Forms.Control)(object)this.grdAllocated).TabIndex = 0;
		this.grdAllocated.TotalRow = (InheritableBoolean)1;
		this.grdAllocated.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAllocated).VisualStyle = (VisualStyle)4;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.btnAddAc);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(408, 27);
		this.panel5.TabIndex = 69;
		this.btnAddAc.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnAddAc.BackColor = System.Drawing.Color.Teal;
		this.btnAddAc.FlatAppearance.BorderSize = 0;
		this.btnAddAc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddAc.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnAddAc.ForeColor = System.Drawing.Color.White;
		this.btnAddAc.Image = Architecture.WinApp.Properties.Resources.Forward2;
		this.btnAddAc.Location = new System.Drawing.Point(377, 2);
		this.btnAddAc.Name = "btnAddAc";
		this.btnAddAc.Size = new System.Drawing.Size(28, 24);
		this.btnAddAc.TabIndex = 2;
		this.btnAddAc.UseVisualStyleBackColor = false;
		this.btnAddAc.Click += new System.EventHandler(btnAddAc_Click);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentAccount);
		this.panel6.Controls.Add(this.label5);
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Controls.Add(this.BtnShow);
		this.panel6.Controls.Add(this.btnAcClear);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGroupName);
		this.panel6.Controls.Add(this.label3);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccountTypes);
		this.panel6.Controls.Add(this.label2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(795, 87);
		this.panel6.TabIndex = 0;
		this.toolStrip2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.BtnResetAllocatedUnAllocated, this.BtnRefresh, this.BtnAllocate, this.BtnDeAllocate, this.btnPartyCustomGroup });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(795, 27);
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
		this.btnPartyCustomGroup.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnPartyCustomGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPartyCustomGroup.Name = "btnPartyCustomGroup";
		this.btnPartyCustomGroup.Size = new System.Drawing.Size(150, 24);
		this.btnPartyCustomGroup.Text = "Party Custom Group";
		this.btnPartyCustomGroup.Click += new System.EventHandler(btnPartyCustomGroup_Click);
		this.BtnShow.BackColor = System.Drawing.Color.Teal;
		this.BtnShow.FlatAppearance.BorderSize = 0;
		this.BtnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnShow.ForeColor = System.Drawing.Color.White;
		this.BtnShow.Location = new System.Drawing.Point(457, 58);
		this.BtnShow.Name = "BtnShow";
		this.BtnShow.Size = new System.Drawing.Size(55, 24);
		this.BtnShow.TabIndex = 3;
		this.BtnShow.Text = "Show";
		this.BtnShow.UseVisualStyleBackColor = false;
		this.BtnShow.Click += new System.EventHandler(BtnShow_Click);
		this.btnAcClear.BackColor = System.Drawing.Color.Teal;
		this.btnAcClear.FlatAppearance.BorderSize = 0;
		this.btnAcClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAcClear.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnAcClear.ForeColor = System.Drawing.Color.White;
		this.btnAcClear.Location = new System.Drawing.Point(515, 58);
		this.btnAcClear.Name = "btnAcClear";
		this.btnAcClear.Size = new System.Drawing.Size(55, 24);
		this.btnAcClear.TabIndex = 4;
		this.btnAcClear.Text = "Reset";
		this.btnAcClear.UseVisualStyleBackColor = false;
		this.btnAcClear.Click += new System.EventHandler(btnAcClear_Click);
		this.cmbGroupName.AllowNull = (DefaultableBoolean)1;
		this.cmbGroupName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGroupName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGroupName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGroupName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbGroupName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbGroupName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbGroupName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGroupName).Location = new System.Drawing.Point(97, 30);
		((System.Windows.Forms.Control)(object)this.cmbGroupName).Name = "cmbGroupName";
		((System.Windows.Forms.Control)(object)this.cmbGroupName).Size = new System.Drawing.Size(188, 26);
		((System.Windows.Forms.Control)(object)this.cmbGroupName).TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 36);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(75, 15);
		this.label3.TabIndex = 9;
		this.label3.Text = "Group Name";
		this.cmbAccountTypes.AllowNull = (DefaultableBoolean)1;
		this.cmbAccountTypes.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccountTypes.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccountTypes.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTypes).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTypes).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbAccountTypes.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).Location = new System.Drawing.Point(97, 57);
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).Name = "cmbAccountTypes";
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).Size = new System.Drawing.Size(188, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).Leave += new System.EventHandler(cmbAccountTypes_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(4, 63);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(90, 15);
		this.label2.TabIndex = 8;
		this.label2.Text = "Accounts Types";
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label4);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1231, 27);
		this.panel10.TabIndex = 3;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 5);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(155, 16);
		this.label4.TabIndex = 9;
		this.label4.Text = "Account Custom Group";
		this.CmbParentAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbParentAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbParentAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Location = new System.Drawing.Point(383, 30);
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Name = "CmbParentAccount";
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Size = new System.Drawing.Size(188, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(290, 36);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(89, 15);
		this.label5.TabIndex = 12;
		this.label5.Text = "Parent Account";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1231, 632);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel10);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmAccountCustomGroup";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmAccountCustomGroup_FormClosing);
		base.Load += new System.EventHandler(frmAccountCustomGroup_Load);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdLookUp).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
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
		((System.ComponentModel.ISupportInitialize)this.cmbGroupName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTypes).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbParentAccount).EndInit();
		base.ResumeLayout(false);
	}
}

}