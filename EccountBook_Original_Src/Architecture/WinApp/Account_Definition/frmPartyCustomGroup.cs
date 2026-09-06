using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Account_Definition;

public class frmPartyCustomGroup : Form
{
	private UserAccount UserAccount = new UserAccount();

	private int Success;

	private int RecId;

	private bool UpdateMood;

	private DropDownBind DDL = new DropDownBind();

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

	private ToolStripButton btnCustomGroupallocationToDocumentType;

	public frmPartyCustomGroup(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmPartyCustomGroup_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 600);
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
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(cmbAccountTypes.Value);
			DataTable dtCG = CustomerGroup.GetAll(new CustomerGroup
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtCG.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("Description");
				for (int i = 0; i < dtCG.Rows.Count; i++)
				{
					dt.Rows.Add(dtCG.Rows[i]["Id"], dtCG.Rows[i]["Description"]);
				}
				if (dt.Rows.Count <= 0)
				{
					return;
				}
				DDL.BindDDL(dt, cmbAccountTypes, "Id", "Description", "Party Group", ZeroIndex: false);
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
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
			DataTable dtLookUp = AcLookUps.GetByProfileTypeId(new AcLookUps
			{
				AcLookUpTypesId = 2,
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
				acLook.AcLookUpTypesId = 2;
				acLook.OrganizationId = UserAccount.OrganizationId;
				acLook.CompanyId = UserAccount.CompanyId;
				Success = AcLookUps.Save(acLook);
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
				Success = AcLookUps.Save(acLook);
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

	private void frmPartyCustomGroup_FormClosing(object sender, FormClosingEventArgs e)
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
			dtLookUp = AcLookUps.GetAll(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				TypeId = 2
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
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		try
		{
			RecId = Conversion.ToInt((object)grdLookUp.CurrentRow.Cells["Id"].Value.ToString());
			int recId = RecId;
			AcLookUps ups = new AcLookUps();
			ups = AcLookUps.GetById(recId);
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
			PartyCustomGroups accounts = new PartyCustomGroups();
			accounts.GetList = new List<PartyCustomGroups>();
			GridEXRow[] checkedRows = grdAllocated.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				PartyCustomGroups groups = new PartyCustomGroups();
				groups.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
				groups.CustomGroupId = Conversion.ToInt(cmbGroupName.Value);
				groups.organizationId = UserAccount.OrganizationId;
				groups.companyId = UserAccount.CompanyId;
				groups.EntryUserId = UserAccount.ID;
				groups.ModifyUserId = UserAccount.ID;
				accounts.GetList.Add(groups);
			}
			if (PartyCustomGroups.Save(accounts) > 0)
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
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			if (grdUnAllocated.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked row first");
			}
			PartyCustomGroups obj = new PartyCustomGroups();
			obj.GetList = new List<PartyCustomGroups>();
			GridEXRow[] checkedRows = grdUnAllocated.GetCheckedRows();
			foreach (GridEXRow item in checkedRows)
			{
				PartyCustomGroups detail = new PartyCustomGroups();
				detail.organizationId = UserAccount.OrganizationId;
				detail.companyId = UserAccount.CompanyId;
				detail.CustomGroupId = Conversion.ToInt(item.Cells["Id"].Value);
				detail.SupplierCustomerId = Conversion.ToInt(item.Cells["SupplierCustomerId"].Value);
				obj.GetList.Add(detail);
			}
			PartyCustomGroups.DeletePartyCustomGroup(obj);
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
			DataTable dtGrd = PartyCustomGroups.PartyCustomGroups_UnAllocatedData(OrganizationId, CompanyId, CustomGroupId, AccountTypeId);
			if (dtGrd.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("SupplierCustomerId");
				dt.Columns.Add("PartyCode");
				dt.Columns.Add("PartyName");
				dt.Rows.Clear();
				for (int i = 0; i < dtGrd.Rows.Count; i++)
				{
					dt.Rows.Add(dtGrd.Rows[i]["CustomGroupId"], dtGrd.Rows[i]["SupplierCustomerId"], dtGrd.Rows[i]["PartyCode"], dtGrd.Rows[i]["PartyName"]);
				}
				grdAllocated.DataSource = dt;
				grdAllocated.RetrieveStructure();
				grdAllocated.RootTable.Columns["Id"].Visible = false;
				grdAllocated.RootTable.Columns["SupplierCustomerId"].Visible = false;
				grdAllocated.RootTable.Columns["PartyName"].Width = 270;
				grdAllocated.RootTable.Columns["PartyCode"].Width = 100;
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
			DataTable dtGrd = PartyCustomGroups.PartyCustomGroups_AllocatedData(OrganizationId, CompanyId, CustomGroupId, AccountTypeId);
			if (dtGrd.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("SupplierCustomerId");
				dt.Columns.Add("PartyCode");
				dt.Columns.Add("PartyName");
				dt.Rows.Clear();
				for (int i = 0; i < dtGrd.Rows.Count; i++)
				{
					dt.Rows.Add(dtGrd.Rows[i]["CustomGroupId"], dtGrd.Rows[i]["SupplierCustomerId"], dtGrd.Rows[i]["PartyCode"], dtGrd.Rows[i]["PartyName"]);
				}
				grdUnAllocated.DataSource = dt;
				grdUnAllocated.RetrieveStructure();
				grdUnAllocated.RootTable.Columns["Id"].Visible = false;
				grdUnAllocated.RootTable.Columns["SupplierCustomerId"].Visible = false;
				grdUnAllocated.RootTable.Columns["PartyName"].Width = 270;
				grdUnAllocated.RootTable.Columns["PartyCode"].Width = 100;
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

	private void btnCustomGroupallocationToDocumentType_Click(object sender, EventArgs e)
	{
		try
		{
			new PartyCustomGroupsAllocationToDocumentType(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmPartyCustomGroup_KeyDown(object sender, KeyEventArgs e)
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
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Expected O, but got Unknown
		GridEXLayout grdLookUp_DesignTimeLayout = new GridEXLayout();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.frmPartyCustomGroup));
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
		this.BtnShow = new System.Windows.Forms.Button();
		this.btnAcClear = new System.Windows.Forms.Button();
		this.cmbGroupName = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbAccountTypes = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.btnCustomGroupallocationToDocumentType = new System.Windows.Forms.ToolStripButton();
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
		((System.Windows.Forms.Control)(object)this.grdLookUp).Location = new System.Drawing.Point(0, 75);
		((System.Windows.Forms.Control)(object)this.grdLookUp).Name = "grdLookUp";
		((ScrollableBase)this.grdLookUp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdLookUp).Size = new System.Drawing.Size(386, 530);
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
		this.panel4.Size = new System.Drawing.Size(386, 75);
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
		this.btnAdd.Location = new System.Drawing.Point(262, 46);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(65, 23);
		this.btnAdd.TabIndex = 1;
		this.btnAdd.Text = "Add";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.txtGroupName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGroupName.Location = new System.Drawing.Point(82, 46);
		this.txtGroupName.Name = "txtGroupName";
		this.txtGroupName.Size = new System.Drawing.Size(177, 23);
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
		this.panel9.Location = new System.Drawing.Point(408, 75);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(387, 530);
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
		((System.Windows.Forms.Control)(object)this.grdUnAllocated).Size = new System.Drawing.Size(387, 503);
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
		this.panel7.Location = new System.Drawing.Point(0, 75);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(408, 530);
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
		((System.Windows.Forms.Control)(object)this.grdAllocated).Size = new System.Drawing.Size(408, 503);
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
		this.panel6.Size = new System.Drawing.Size(795, 75);
		this.panel6.TabIndex = 0;
		this.toolStrip2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.BtnResetAllocatedUnAllocated, this.BtnRefresh, this.BtnAllocate, this.BtnDeAllocate, this.btnCustomGroupallocationToDocumentType });
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
		this.cmbGroupName.AllowNull = (DefaultableBoolean)1;
		this.cmbGroupName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGroupName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGroupName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGroupName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.cmbGroupName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbGroupName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbGroupName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGroupName).Location = new System.Drawing.Point(79, 44);
		((System.Windows.Forms.Control)(object)this.cmbGroupName).Name = "cmbGroupName";
		((System.Windows.Forms.Control)(object)this.cmbGroupName).Size = new System.Drawing.Size(186, 26);
		((System.Windows.Forms.Control)(object)this.cmbGroupName).TabIndex = 0;
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
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).Location = new System.Drawing.Point(347, 44);
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).Name = "cmbAccountTypes";
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(271, 50);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(70, 15);
		this.label2.TabIndex = 8;
		this.label2.Text = "Party Group";
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label4);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1184, 27);
		this.panel10.TabIndex = 3;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 5);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(137, 16);
		this.label4.TabIndex = 9;
		this.label4.Text = "Party Custom Group";
		this.btnCustomGroupallocationToDocumentType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnCustomGroupallocationToDocumentType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCustomGroupallocationToDocumentType.Image = (System.Drawing.Image)resources.GetObject("btnCustomGroupallocationToDocumentType.Image");
		this.btnCustomGroupallocationToDocumentType.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCustomGroupallocationToDocumentType.Name = "btnCustomGroupallocationToDocumentType";
		this.btnCustomGroupallocationToDocumentType.Size = new System.Drawing.Size(202, 24);
		this.btnCustomGroupallocationToDocumentType.Text = "Allocation to Document Type";
		this.btnCustomGroupallocationToDocumentType.Click += new System.EventHandler(btnCustomGroupallocationToDocumentType_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 632);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel10);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmPartyCustomGroup";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmPartyCustomGroup_FormClosing);
		base.Load += new System.EventHandler(frmPartyCustomGroup_Load);
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
		base.ResumeLayout(false);
	}
}
