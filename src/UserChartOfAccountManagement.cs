using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
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
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Account_Definition
{

public class UserChartOfAccountManagement : Form
{
	private UserAccount UserAccount = new UserAccount();

	private int Success;

	private int RecId;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtAcStatus = new DataTable();

	private IContainer components;

	private Button btnAdd;

	private TextBox txtGroupName;

	private Label label1;

	internal GridEX grdDeAllocatedChartOfAccountToCustomGroup;

	internal UltraCombo cmbAccountTypes;

	internal GridEX GrdAllocatedChartofAccount;

	internal UltraCombo cmbGroupName;

	private Label label3;

	private Panel panel10;

	private Label ExpenseVoucherLable;

	private Panel panel11;

	internal UltraCombo CmbUsers;

	private Label label5;

	private CheckBox ChBoxIsActive;

	private Panel panel1;

	private Button btnAddAc;

	private Panel panel5;

	private Panel panel7;

	private Label label11;

	private Panel panel4;

	private Label label10;

	internal UltraCombo CmbThirdLevelAccounts;

	private Label label9;

	internal UltraCombo Cmb2ndLevelAccounts;

	private Label label8;

	private Label label7;

	private Button btnDeAllocate;

	private Panel panel13;

	internal GridEX GridCustomGroup;

	private Label label4;

	private Panel panel12;

	private Button BtnAllocateCustomGroup;

	private Label label6;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel3;

	private Panel panel15;

	internal GridEX grdDeAllocatedCustomGroupsToUser;

	private Label label13;

	private Panel panel6;

	private Label label12;

	private TabPage tabPage2;

	private Panel panel8;

	internal GridEX grdAllocatedCustomGroupToUser;

	private Label label14;

	private Button BtnDeAllocateCustomGroup;

	private Panel panel9;

	private Label label15;

	private Button btnShow;

	private TabPage tabPage3;

	private Panel panel2;

	internal GridEX grdhistorybyuser;

	private Label label2;

	private Panel panel14;

	internal UltraCombo cmbUserforhistory;

	private Label label16;

	private Label label17;

	private Button btnShowHistory;

	private ToolStrip ToolStripUserCoaManage;

	private ToolStripButton btnrefresh;

	private GroupBox groupBox1;

	private Label label18;

	internal UltraCombo cmbCoaCustomgroup;

	private TabPage tabPage4;

	private Panel panel16;

	private Panel panel18;

	private Panel panel17;

	private Label label19;

	private Button button1;

	internal GridEX grdUserAllcoateForAccountStatus;

	public UserChartOfAccountManagement(UserAccount userAccount)
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
			base.AutoScrollMinSize = new Size(980, 700);
			GridBind();
			AllUsers();
			CutomGroupFill();
			SecondLevelAccountsFill();
			ThirdLevelAccountsFill();
			AccountTypeCombo();
			UsersBindForHistory();
			CoacustomgroupCombo();
			AccountsStatusShowHide();
			AllUsersForAllocationAccountsStatus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CutomGroupFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = CoaCustomGroupForUsers.BindCustomGroupForAlocToCoa(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbGroupName, "Id", "CoaCustomGroupNameForUsers", "Custom Group", false);
				((UltraGridBase)cmbGroupName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((Control)(object)cmbGroupName).Text = string.Empty;
				((UltraGridBase)cmbGroupName).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			DataTable dtCustomGroup = CoaCustomGroupForUsers.GetAll(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ApprovedFilter = "All"
			});
			if (dtCustomGroup.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("Description");
				dt.Columns.Add("IsActive");
				for (int i = 0; i < dtCustomGroup.Rows.Count; i++)
				{
					dt.Rows.Add(dtCustomGroup.Rows[i]["Id"], dtCustomGroup.Rows[i]["CoaCustomGroupNameForUsers"], Conversion.ToBool(dtCustomGroup.Rows[i]["IsActive"]));
				}
				GridCustomGroup.DataSource = dt;
				GridCustomGroup.RetrieveStructure();
				GridCustomGroup.RootTable.Columns["Id"].Visible = false;
				GridCustomGroup.ColumnAutoResize = true;
			}
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

	private void InsertCustomGroup()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!Validation())
			{
				return;
			}
			CoaCustomGroupForUsers obj = new CoaCustomGroupForUsers();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchId = UserAccount.BranchesId;
			obj.CoaCustomGroupNameForUsers = txtGroupName.Text;
			obj.IsActive = (ChBoxIsActive.Checked ? 1 : 0);
			Success = CoaCustomGroupForUsers.save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Custom Group Update Successfully....  " + Success);
			}
			else
			{
				MessageBox.Show("Custom Group Save Successfully....  " + Success);
			}
			GridBind();
			Reset();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			InsertCustomGroup();
			CutomGroupFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdLookUp_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RecId = Conversion.ToInt(GridCustomGroup.CurrentRow.Cells["Id"].Value);
			txtGroupName.Text = Conversion.ToString(GridCustomGroup.CurrentRow.Cells["Description"].Value);
			ChBoxIsActive.Checked = Conversion.ToBool(GridCustomGroup.CurrentRow.Cells["IsActive"].Value);
			btnAdd.Text = "Update";
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
			btnAdd.Text = "Add";
			RecId = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsStatusShowHide()
	{
		try
		{
			dtAcStatus.Columns.Add("Id");
			dtAcStatus.Columns.Add("AccountStatus");
			dtAcStatus.Rows.Add(1, "Accounts Hide");
			dtAcStatus.Rows.Add(2, "Accounts Show");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllUsers()
	{
		try
		{
			DataTable dt = UserAccount.GetUsersFromAccountsShowHideStatus(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbUsers, "Id", "UserName", "User Name", ZeroIndex: false);
				((UltraGridBase)CmbUsers).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllUsersForAllocationAccountsStatus()
	{
		try
		{
			DataTable dtUsr = UserAccount.GetUsersForAccountsStatusAllocation(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dtUsr.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("UserId", typeof(int));
				dt.Columns.Add("UserName");
				dt.Columns.Add("AccountStatus");
				dt.Columns.Add("IsActive", typeof(bool));
				for (int i = 0; i < dtUsr.Rows.Count; i++)
				{
					dt.Rows.Add(dtUsr.Rows[i]["Id"], dtUsr.Rows[i]["UserId"], dtUsr.Rows[i]["UserName"], dtUsr.Rows[i]["AccountStatus"], Conversion.ToBool(dtUsr.Rows[i]["IsActive"]));
				}
				if (dt.Rows.Count > 0)
				{
					grdUserAllcoateForAccountStatus.DataSource = dt;
					grdUserAllcoateForAccountStatus.RetrieveStructure();
					gridUserAllocationAccountsStatusSetting();
				}
				else
				{
					grdUserAllcoateForAccountStatus.ClearStructure();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridUserAllocationAccountsStatusSetting()
	{
		try
		{
			grdUserAllcoateForAccountStatus.RootTable.Columns["Id"].Visible = false;
			grdUserAllcoateForAccountStatus.RootTable.Columns["UserId"].Visible = false;
			grdUserAllcoateForAccountStatus.RootTable.Columns["IsActive"].EditType = (EditType)8;
			grdUserAllcoateForAccountStatus.RootTable.Columns["AccountStatus"].ColumnType = (ColumnType)0;
			grdUserAllcoateForAccountStatus.RootTable.Columns["AccountStatus"].HasValueList = true;
			grdUserAllcoateForAccountStatus.RootTable.Columns["AccountStatus"].LimitToList = true;
			grdUserAllcoateForAccountStatus.RootTable.Columns["AccountStatus"].EditType = (EditType)4;
			grdUserAllcoateForAccountStatus.RootTable.Columns["AccountStatus"].Caption = "Account Status";
			grdUserAllcoateForAccountStatus.RootTable.Columns["AccountStatus"].ValueList.PopulateValueList((IEnumerable)dtAcStatus.DefaultView, "Id", "AccountStatus");
			grdUserAllcoateForAccountStatus.RootTable.Columns["AccountStatus"].Width = 150;
			grdUserAllcoateForAccountStatus.RootTable.Columns.Add("Select");
			grdUserAllcoateForAccountStatus.RootTable.Columns["Select"].ActAsSelector = true;
			grdUserAllcoateForAccountStatus.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdUserAllcoateForAccountStatus.RootTable.Columns["Select"].Width = 50;
			grdUserAllcoateForAccountStatus.ColumnAutoResize = true;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbUsers_Leave(object sender, EventArgs e)
	{
		try
		{
			AllocatedCustomGroupToUser();
			DeAllocatedCustomGroupToUser();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void AllocatedCustomGroupToUser()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = CoaCustomGroupAlocToUsers.GetAllocatedCutomGroupsToUser(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserId = Conversion.ToInt(CmbUsers.Value)
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtAllocated = new DataTable();
				dtAllocated.Columns.Add("CustomGroupId");
				dtAllocated.Columns.Add("CustomGroupName");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtAllocated.Rows.Add(dt.Rows[i]["CoaAlocToCustomGroupForUsersId"], dt.Rows[i]["CoaCustomGroupNameForUsers"]);
				}
				grdAllocatedCustomGroupToUser.DataSource = dtAllocated;
				grdAllocatedCustomGroupToUser.RetrieveStructure();
				grdAllocatedCustomGroupToUser.ColumnAutoResize = true;
				grdAllocatedCustomGroupToUser.RootTable.Columns["CustomGroupId"].Visible = false;
				grdAllocatedCustomGroupToUser.RootTable.Columns.Add("Select");
				grdAllocatedCustomGroupToUser.RootTable.Columns["Select"].ActAsSelector = true;
				grdAllocatedCustomGroupToUser.RootTable.Columns["Select"].UseHeaderSelector = true;
				grdAllocatedCustomGroupToUser.RootTable.Columns["Select"].Width = 50;
			}
			else
			{
				grdAllocatedCustomGroupToUser.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DeAllocatedCustomGroupToUser()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		try
		{
			DataTable dt = CoaCustomGroupForUsers.GetAll(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserId = Conversion.ToInt(CmbUsers.Value),
				IsApproved = true
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtUnAllocated = new DataTable();
				dtUnAllocated.Columns.Add("CustomGroupId");
				dtUnAllocated.Columns.Add("CustomGroupName");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtUnAllocated.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["CoaCustomGroupNameForUsers"]);
				}
				grdDeAllocatedCustomGroupsToUser.DataSource = dtUnAllocated;
				grdDeAllocatedCustomGroupsToUser.RetrieveStructure();
				grdDeAllocatedCustomGroupsToUser.ColumnAutoResize = true;
				grdDeAllocatedCustomGroupsToUser.RootTable.Columns["CustomGroupId"].Visible = false;
				grdDeAllocatedCustomGroupsToUser.RootTable.Columns.Add("Select");
				grdDeAllocatedCustomGroupsToUser.RootTable.Columns["Select"].ActAsSelector = true;
				grdDeAllocatedCustomGroupsToUser.RootTable.Columns["Select"].UseHeaderSelector = true;
				grdDeAllocatedCustomGroupsToUser.RootTable.Columns["Select"].Width = 50;
			}
			else
			{
				grdDeAllocatedCustomGroupsToUser.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnAllocateCustomGroup_Click(object sender, EventArgs e)
	{
		try
		{
			AllocateCustomGroupToUser();
			CutomGroupFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllocateCustomGroupToUser()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			if (grdDeAllocatedCustomGroupsToUser.GetCheckedRows().Length != 0)
			{
				CoaCustomGroupAlocToUsers obj = new CoaCustomGroupAlocToUsers();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.CoaCustomGroupAlocToUsersList = new List<CoaCustomGroupAlocToUsers>();
				GridEXRow[] checkedRows = grdDeAllocatedCustomGroupsToUser.GetCheckedRows();
				foreach (GridEXRow item in checkedRows)
				{
					CoaCustomGroupAlocToUsers coaCustomGroupToUser = new CoaCustomGroupAlocToUsers();
					coaCustomGroupToUser.UsersId = Conversion.ToInt(CmbUsers.Value);
					coaCustomGroupToUser.CoaAlocToCustomGroupForUsersId = Conversion.ToInt(item.Cells["CustomGroupId"].Value);
					obj.CoaCustomGroupAlocToUsersList.Add(coaCustomGroupToUser);
				}
				if (CoaCustomGroupAlocToUsers.Save(obj) > 0)
				{
					MessageBox.Show("Custom Group's Allocated To User Successfully");
				}
				AllocatedCustomGroupToUser();
				DeAllocatedCustomGroupToUser();
				return;
			}
			throw new Exception("Please Check Rows First To Allocate");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnDeAllocateCustomGroup_Click(object sender, EventArgs e)
	{
		try
		{
			DeAllocateCustomGroupToUser();
			CutomGroupFill();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DeAllocateCustomGroupToUser()
	{
		try
		{
			if (grdAllocatedCustomGroupToUser.GetCheckedRows().Length != 0)
			{
				if (MessageBox.Show("Are you sure to DeAllocate Custom Group's?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
				{
					string ids = null;
					GridEXRow[] checkedRows = grdAllocatedCustomGroupToUser.GetCheckedRows();
					foreach (GridEXRow item in checkedRows)
					{
						ids = ids + Conversion.ToString(item.Cells["CustomGroupId"].Value) + ",";
					}
					CoaCustomGroupAlocToUsers.DeleteById(ids, Conversion.ToInt(CmbUsers.Value), UserAccount.OrganizationId, UserAccount.CompanyId);
					MessageBox.Show("Record's UnAllocated Successfully");
					AllocatedCustomGroupToUser();
					DeAllocatedCustomGroupToUser();
				}
				return;
			}
			throw new Exception("Please Check Rows First To Allocate");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ResetCustom()
	{
		try
		{
			((Control)(object)cmbGroupName).Text = string.Empty;
			((Control)(object)cmbAccountTypes).Text = string.Empty;
			grdDeAllocatedChartOfAccountToCustomGroup.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountTypeCombo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		try
		{
			DataTable dtAcc = AccountTypes.GetAll(new AccountTypes());
			if (dtAcc.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtAcc, cmbAccountTypes, "Id", "AccountType", "Account Type", false);
				((UltraGridBase)cmbAccountTypes).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CoacustomgroupCombo()
	{
		try
		{
			DataTable dtAcc = CommonServices.CustomeGroupsDefine(1);
			if (dtAcc.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtAcc, cmbCoaCustomgroup, "Id", "AcLookUpsDescription", "Account Custom Group", false);
				((UltraGridBase)cmbCoaCustomgroup).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SecondLevelAccountsFill()
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
				Account_Level = 2
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, Cmb2ndLevelAccounts, "Id", "AccountTitle", "Account Title", false);
				((UltraGridBase)Cmb2ndLevelAccounts).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
				DDL.BindDDLNew(dt, CmbThirdLevelAccounts, "Id", "AccountTitle", "Account Title", false);
				((UltraGridBase)CmbThirdLevelAccounts).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbGroupName).ActiveRow == null)
			{
				MessageBox.Show("Custom Group Field Is Cumpulsory");
				cmbGroupName.Focus();
				grdDeAllocatedChartOfAccountToCustomGroup.ClearStructure();
				GrdAllocatedChartofAccount.ClearStructure();
			}
			else
			{
				UnAllocatedChartOfAccountToCustomGroup();
				AllocatedChartOfAccountToCustomGroup();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void UnAllocatedChartOfAccountToCustomGroup()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		try
		{
			DataTable dt = CoaAlocToCustomGroupForUsers.GetUnAllocatedChartOfAccountsToCustomGroup(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				CustomGroupId = Conversion.ToInt(cmbGroupName.Value),
				AccountId = Conversion.ToInt(CmbThirdLevelAccounts.Value),
				AccountTypeId = Conversion.ToInt(cmbAccountTypes.Value),
				CustomerGroupId = Conversion.ToInt(cmbCoaCustomgroup.Value)
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtUnAllocated = new DataTable();
				dtUnAllocated.Columns.Add("ChartOfAccountId");
				dtUnAllocated.Columns.Add("AccountCode");
				dtUnAllocated.Columns.Add("AccountTitle");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtUnAllocated.Rows.Add(dt.Rows[i]["ChartOfAccountId"], dt.Rows[i]["AccountCode"], dt.Rows[i]["AccountTitle"]);
				}
				grdDeAllocatedChartOfAccountToCustomGroup.DataSource = dtUnAllocated;
				grdDeAllocatedChartOfAccountToCustomGroup.RetrieveStructure();
				grdDeAllocatedChartOfAccountToCustomGroup.RootTable.Columns["ChartOfAccountId"].Visible = false;
				grdDeAllocatedChartOfAccountToCustomGroup.ColumnAutoResize = true;
				grdDeAllocatedChartOfAccountToCustomGroup.RootTable.Columns.Add("Select");
				grdDeAllocatedChartOfAccountToCustomGroup.RootTable.Columns["Select"].ActAsSelector = true;
				grdDeAllocatedChartOfAccountToCustomGroup.RootTable.Columns["Select"].UseHeaderSelector = true;
				grdDeAllocatedChartOfAccountToCustomGroup.RootTable.Columns["Select"].Width = 50;
			}
			else
			{
				grdDeAllocatedChartOfAccountToCustomGroup.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AllocatedChartOfAccountToCustomGroup()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		try
		{
			DataTable dt = CoaAlocToCustomGroupForUsers.GetAllocatedChartOfAccountsToCustomGroup(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				CustomGroupId = Conversion.ToInt(cmbGroupName.Value)
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtAllocated = new DataTable();
				dtAllocated.Columns.Add("ChartOfAccountId");
				dtAllocated.Columns.Add("AccountCode");
				dtAllocated.Columns.Add("AccountTitle");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtAllocated.Rows.Add(dt.Rows[i]["ChartOfAccountId"], dt.Rows[i]["AccountCode"], dt.Rows[i]["AccountTitle"]);
				}
				GrdAllocatedChartofAccount.DataSource = dtAllocated;
				GrdAllocatedChartofAccount.RetrieveStructure();
				GrdAllocatedChartofAccount.RootTable.Columns["ChartOfAccountId"].Visible = false;
				GrdAllocatedChartofAccount.ColumnAutoResize = true;
				GrdAllocatedChartofAccount.RootTable.Columns.Add("Select");
				GrdAllocatedChartofAccount.RootTable.Columns["Select"].ActAsSelector = true;
				GrdAllocatedChartofAccount.RootTable.Columns["Select"].UseHeaderSelector = true;
				GrdAllocatedChartofAccount.RootTable.Columns["Select"].Width = 50;
			}
			else
			{
				GrdAllocatedChartofAccount.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnAddAc_Click(object sender, EventArgs e)
	{
		try
		{
			SaveChartOfAccountToCustomGroup();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SaveChartOfAccountToCustomGroup()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		try
		{
			if (grdDeAllocatedChartOfAccountToCustomGroup.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Checked row first");
			}
			CoaAlocToCustomGroupForUsers accounts = new CoaAlocToCustomGroupForUsers();
			accounts.CoaAlocToCustomGroupForUsersList = new List<CoaAlocToCustomGroupForUsers>();
			GridEXRow[] checkedRows = grdDeAllocatedChartOfAccountToCustomGroup.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				CoaAlocToCustomGroupForUsers groups = new CoaAlocToCustomGroupForUsers();
				groups.CompanyId = UserAccount.CompanyId;
				groups.OrganizationId = UserAccount.OrganizationId;
				groups.BranchId = UserAccount.BranchesId;
				groups.ChartOfAccountId = Conversion.ToInt(r.Cells["ChartOfAccountId"].Value);
				groups.CoaCustomGroupForUsersId = Conversion.ToInt(cmbGroupName.Value);
				accounts.CoaAlocToCustomGroupForUsersList.Add(groups);
			}
			if (CoaAlocToCustomGroupForUsers.Save(accounts) > 0)
			{
				MessageBox.Show("Allocated Successfully");
			}
			UnAllocatedChartOfAccountToCustomGroup();
			AllocatedChartOfAccountToCustomGroup();
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
			if (GrdAllocatedChartofAccount.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked row first");
			}
			if (MessageBox.Show("Are you sure to DeAllocate Chart Of Acccount's?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				string ids = null;
				GridEXRow[] checkedRows = GrdAllocatedChartofAccount.GetCheckedRows();
				foreach (GridEXRow item in checkedRows)
				{
					ids = ids + Conversion.ToString(item.Cells["ChartOfAccountId"].Value) + ",";
				}
				CoaAlocToCustomGroupForUsers.DeleteById(ids, Conversion.ToInt(cmbGroupName.Value), UserAccount.OrganizationId, UserAccount.CompanyId);
				MessageBox.Show("Record's UnAllocated Successfully");
				UnAllocatedChartOfAccountToCustomGroup();
				AllocatedChartOfAccountToCustomGroup();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void UsersBindForHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = UserAccount.ReadAll(new UserAccount
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbUserforhistory, "Id", "UserName", "User Name", false);
				((UltraGridBase)cmbUserforhistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			BindHistorybyUser();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindHistorybyUser()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = CoaAlocToCustomGroupForUsers.CoaAlocToCustomGroupHistorybyUser(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserId = Conversion.ToInt(cmbUserforhistory.Value)
			});
			if (dt.Rows.Count > 0)
			{
				grdhistorybyuser.DataSource = dt;
				grdhistorybyuser.RetrieveStructure();
			}
			else
			{
				grdhistorybyuser.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnrefresh_Click(object sender, EventArgs e)
	{
		GridBind();
		AllUsers();
		CutomGroupFill();
		SecondLevelAccountsFill();
		ThirdLevelAccountsFill();
		AccountTypeCombo();
		CoacustomgroupCombo();
		UsersBindForHistory();
		txtGroupName.Focus();
	}

	private void UserChartOfAccountManagement_FormClosing(object sender, FormClosingEventArgs e)
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

	private void button1_Click(object sender, EventArgs e)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Allocate Accounts Show & Hide Status?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			if (grdUserAllcoateForAccountStatus.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Checked row first");
			}
			AccountsShowAndHideStatusUserWise accounts = new AccountsShowAndHideStatusUserWise();
			accounts.AccountsShowAndHideStatusUserWiseslist = new List<AccountsShowAndHideStatusUserWise>();
			GridEXRow[] checkedRows = grdUserAllcoateForAccountStatus.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				AccountsShowAndHideStatusUserWise ObjDt = new AccountsShowAndHideStatusUserWise();
				ObjDt.CompanyId = UserAccount.CompanyId;
				ObjDt.OrganizationId = UserAccount.OrganizationId;
				ObjDt.EntryUserId = UserAccount.ID;
				ObjDt.ModifyUserId = UserAccount.ID;
				ObjDt.EntryDate = DateTime.Now;
				ObjDt.ModifyDate = DateTime.Now;
				ObjDt.Id = Conversion.ToInt(r.Cells["Id"].Value);
				ObjDt.UserId = Conversion.ToInt(r.Cells["UserId"].Value);
				ObjDt.AccountStatus = Conversion.ToInt(r.Cells["AccountStatus"].Value);
				ObjDt.IsActive = Conversion.ToBool(r.Cells["IsActive"].Value);
				if (ObjDt.AccountStatus == 0)
				{
					throw new Exception("AccountStatus Field is required");
				}
				accounts.AccountsShowAndHideStatusUserWiseslist.Add(ObjDt);
			}
			CoaAlocToCustomGroupForUsers.AccountsStatusShowHideSave(accounts);
			AllUsersForAllocationAccountsStatus();
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
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Expected O, but got Unknown
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Expected O, but got Unknown
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Expected O, but got Unknown
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Expected O, but got Unknown
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Expected O, but got Unknown
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		GridEXLayout grdDeAllocatedChartOfAccountToCustomGroup_DesignTimeLayout = new GridEXLayout();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		GridEXLayout GrdAllocatedChartofAccount_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridCustomGroup_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		GridEXLayout grdAllocatedCustomGroupToUser_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDeAllocatedCustomGroupsToUser_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistorybyuser_DesignTimeLayout = new GridEXLayout();
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
		GridEXLayout grdUserAllcoateForAccountStatus_DesignTimeLayout = new GridEXLayout();
		this.btnAdd = new System.Windows.Forms.Button();
		this.txtGroupName = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.grdDeAllocatedChartOfAccountToCustomGroup = new GridEX();
		this.cmbGroupName = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbAccountTypes = new UltraCombo();
		this.GrdAllocatedChartofAccount = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ToolStripUserCoaManage = new System.Windows.Forms.ToolStrip();
		this.btnrefresh = new System.Windows.Forms.ToolStripButton();
		this.ExpenseVoucherLable = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.GridCustomGroup = new GridEX();
		this.label4 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.ChBoxIsActive = new System.Windows.Forms.CheckBox();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbUsers = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.btnDeAllocate = new System.Windows.Forms.Button();
		this.label15 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.btnAddAc = new System.Windows.Forms.Button();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label18 = new System.Windows.Forms.Label();
		this.btnShow = new System.Windows.Forms.Button();
		this.CmbThirdLevelAccounts = new UltraCombo();
		this.cmbCoaCustomgroup = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.Cmb2ndLevelAccounts = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel15 = new System.Windows.Forms.Panel();
		this.BtnDeAllocateCustomGroup = new System.Windows.Forms.Button();
		this.grdAllocatedCustomGroupToUser = new GridEX();
		this.label14 = new System.Windows.Forms.Label();
		this.BtnAllocateCustomGroup = new System.Windows.Forms.Button();
		this.grdDeAllocatedCustomGroupsToUser = new GridEX();
		this.label13 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdhistorybyuser = new GridEX();
		this.label2 = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.cmbUserforhistory = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel16 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.grdUserAllcoateForAccountStatus = new GridEX();
		((System.ComponentModel.ISupportInitialize)this.grdDeAllocatedChartOfAccountToCustomGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGroupName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTypes).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.GrdAllocatedChartofAccount).BeginInit();
		this.panel10.SuspendLayout();
		this.ToolStripUserCoaManage.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridCustomGroup).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbUsers).BeginInit();
		this.panel1.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbThirdLevelAccounts).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCoaCustomgroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.Cmb2ndLevelAccounts).BeginInit();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAllocatedCustomGroupToUser).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdDeAllocatedCustomGroupsToUser).BeginInit();
		this.panel6.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel8.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistorybyuser).BeginInit();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbUserforhistory).BeginInit();
		this.tabPage4.SuspendLayout();
		this.panel16.SuspendLayout();
		this.panel17.SuspendLayout();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdUserAllcoateForAccountStatus).BeginInit();
		base.SuspendLayout();
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatAppearance.BorderSize = 0;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(352, 22);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(46, 23);
		this.btnAdd.TabIndex = 2;
		this.btnAdd.Text = "Add";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.txtGroupName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGroupName.Location = new System.Drawing.Point(10, 23);
		this.txtGroupName.Name = "txtGroupName";
		this.txtGroupName.Size = new System.Drawing.Size(280, 23);
		this.txtGroupName.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(75, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Group Name";
		this.grdDeAllocatedChartOfAccountToCustomGroup.AllowEdit = (InheritableBoolean)2;
		this.grdDeAllocatedChartOfAccountToCustomGroup.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdDeAllocatedChartOfAccountToCustomGroup_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDeAllocatedChartOfAccountToCustomGroup.DesignTimeLayout = grdDeAllocatedChartOfAccountToCustomGroup_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDeAllocatedChartOfAccountToCustomGroup).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDeAllocatedChartOfAccountToCustomGroup.DynamicFiltering = true;
		this.grdDeAllocatedChartOfAccountToCustomGroup.FilterMode = (FilterMode)2;
		this.grdDeAllocatedChartOfAccountToCustomGroup.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDeAllocatedChartOfAccountToCustomGroup).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDeAllocatedChartOfAccountToCustomGroup.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDeAllocatedChartOfAccountToCustomGroup).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDeAllocatedChartOfAccountToCustomGroup).Name = "grdDeAllocatedChartOfAccountToCustomGroup";
		((ScrollableBase)this.grdDeAllocatedChartOfAccountToCustomGroup).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDeAllocatedChartOfAccountToCustomGroup).Size = new System.Drawing.Size(460, 368);
		((System.Windows.Forms.Control)(object)this.grdDeAllocatedChartOfAccountToCustomGroup).TabIndex = 0;
		this.grdDeAllocatedChartOfAccountToCustomGroup.TotalRow = (InheritableBoolean)1;
		this.grdDeAllocatedChartOfAccountToCustomGroup.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDeAllocatedChartOfAccountToCustomGroup).VisualStyle = (VisualStyle)4;
		this.cmbGroupName.AllowNull = (DefaultableBoolean)1;
		this.cmbGroupName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGroupName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGroupName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGroupName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.cmbGroupName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGroupName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbGroupName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbGroupName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGroupName).Location = new System.Drawing.Point(23, 31);
		((System.Windows.Forms.Control)(object)this.cmbGroupName).Name = "cmbGroupName";
		((System.Windows.Forms.Control)(object)this.cmbGroupName).Size = new System.Drawing.Size(270, 26);
		((System.Windows.Forms.Control)(object)this.cmbGroupName).TabIndex = 0;
		this.label3.BackColor = System.Drawing.SystemColors.ControlLight;
		this.label3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(23, 7);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(270, 19);
		this.label3.TabIndex = 9;
		this.label3.Text = "Custom Group Name";
		this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.cmbAccountTypes.AllowNull = (DefaultableBoolean)1;
		this.cmbAccountTypes.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccountTypes.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccountTypes.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTypes).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTypes).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccountTypes).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAccountTypes.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).Location = new System.Drawing.Point(283, 49);
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).Name = "cmbAccountTypes";
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).Size = new System.Drawing.Size(270, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccountTypes).TabIndex = 1;
		this.GrdAllocatedChartofAccount.AllowEdit = (InheritableBoolean)2;
		this.GrdAllocatedChartofAccount.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GrdAllocatedChartofAccount_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdAllocatedChartofAccount.DesignTimeLayout = GrdAllocatedChartofAccount_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdAllocatedChartofAccount).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdAllocatedChartofAccount.DynamicFiltering = true;
		this.GrdAllocatedChartofAccount.FilterMode = (FilterMode)2;
		this.GrdAllocatedChartofAccount.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdAllocatedChartofAccount).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdAllocatedChartofAccount.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdAllocatedChartofAccount).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.GrdAllocatedChartofAccount).Name = "GrdAllocatedChartofAccount";
		((ScrollableBase)this.GrdAllocatedChartofAccount).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdAllocatedChartofAccount).Size = new System.Drawing.Size(487, 368);
		((System.Windows.Forms.Control)(object)this.GrdAllocatedChartofAccount).TabIndex = 0;
		this.GrdAllocatedChartofAccount.TotalRow = (InheritableBoolean)1;
		this.GrdAllocatedChartofAccount.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdAllocatedChartofAccount).VisualStyle = (VisualStyle)4;
		this.panel10.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel10.Controls.Add(this.ToolStripUserCoaManage);
		this.panel10.Controls.Add(this.ExpenseVoucherLable);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(990, 57);
		this.panel10.TabIndex = 3;
		this.ToolStripUserCoaManage.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnrefresh });
		this.ToolStripUserCoaManage.Location = new System.Drawing.Point(0, 0);
		this.ToolStripUserCoaManage.Name = "ToolStripUserCoaManage";
		this.ToolStripUserCoaManage.Size = new System.Drawing.Size(990, 25);
		this.ToolStripUserCoaManage.TabIndex = 1;
		this.ToolStripUserCoaManage.Text = "toolStrip1";
		this.btnrefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnrefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnrefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnrefresh.Name = "btnrefresh";
		this.btnrefresh.Size = new System.Drawing.Size(76, 22);
		this.btnrefresh.Text = "Refresh";
		this.btnrefresh.Click += new System.EventHandler(btnrefresh_Click);
		this.ExpenseVoucherLable.AutoSize = true;
		this.ExpenseVoucherLable.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ExpenseVoucherLable.ForeColor = System.Drawing.Color.White;
		this.ExpenseVoucherLable.Location = new System.Drawing.Point(8, 31);
		this.ExpenseVoucherLable.Name = "ExpenseVoucherLable";
		this.ExpenseVoucherLable.Size = new System.Drawing.Size(711, 19);
		this.ExpenseVoucherLable.TabIndex = 0;
		this.ExpenseVoucherLable.Text = "User Chart Of Accounts Management  (Allocating accounts to custom groups for users)";
		this.panel11.BackColor = System.Drawing.Color.White;
		this.panel11.Controls.Add(this.panel13);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(407, 569);
		this.panel11.TabIndex = 4;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.GridCustomGroup);
		this.panel13.Controls.Add(this.label4);
		this.panel13.Controls.Add(this.panel12);
		this.panel13.Controls.Add(this.label6);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(407, 569);
		this.panel13.TabIndex = 70;
		this.GridCustomGroup.AllowEdit = (InheritableBoolean)2;
		GridCustomGroup_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridCustomGroup.DesignTimeLayout = GridCustomGroup_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridCustomGroup).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridCustomGroup.DynamicFiltering = true;
		this.GridCustomGroup.FilterMode = (FilterMode)2;
		this.GridCustomGroup.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridCustomGroup).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridCustomGroup.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridCustomGroup).Location = new System.Drawing.Point(0, 114);
		((System.Windows.Forms.Control)(object)this.GridCustomGroup).Name = "GridCustomGroup";
		((ScrollableBase)this.GridCustomGroup).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridCustomGroup).Size = new System.Drawing.Size(407, 455);
		((System.Windows.Forms.Control)(object)this.GridCustomGroup).TabIndex = 69;
		this.GridCustomGroup.TotalRow = (InheritableBoolean)1;
		this.GridCustomGroup.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridCustomGroup).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.GridCustomGroup).DoubleClick += new System.EventHandler(grdLookUp_DoubleClick);
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Dock = System.Windows.Forms.DockStyle.Top;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(0, 87);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(407, 27);
		this.label4.TabIndex = 0;
		this.label4.Text = "Custom Groups Grid";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel12.Controls.Add(this.label1);
		this.panel12.Controls.Add(this.txtGroupName);
		this.panel12.Controls.Add(this.btnAdd);
		this.panel12.Controls.Add(this.ChBoxIsActive);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 27);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(407, 60);
		this.panel12.TabIndex = 4;
		this.ChBoxIsActive.AutoSize = true;
		this.ChBoxIsActive.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChBoxIsActive.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChBoxIsActive.Location = new System.Drawing.Point(295, 25);
		this.ChBoxIsActive.Margin = new System.Windows.Forms.Padding(2);
		this.ChBoxIsActive.Name = "ChBoxIsActive";
		this.ChBoxIsActive.Size = new System.Drawing.Size(59, 19);
		this.ChBoxIsActive.TabIndex = 1;
		this.ChBoxIsActive.TabStop = false;
		this.ChBoxIsActive.Text = "Active";
		this.ChBoxIsActive.UseVisualStyleBackColor = true;
		this.label6.BackColor = System.Drawing.Color.Teal;
		this.label6.Dock = System.Windows.Forms.DockStyle.Top;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(0, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(407, 27);
		this.label6.TabIndex = 2;
		this.label6.Text = "Define Accounts Custom Group";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CmbUsers.AllowNull = (DefaultableBoolean)1;
		this.CmbUsers.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUsers.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUsers.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUsers).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbUsers).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUsers).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUsers).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUsers).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbUsers).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUsers).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbUsers).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUsers).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUsers).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUsers).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbUsers).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUsers).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUsers).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUsers).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbUsers).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUsers).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUsers).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUsers).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbUsers).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUsers).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUsers).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbUsers).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUsers).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbUsers).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUsers).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUsers).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUsers).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbUsers.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUsers).Location = new System.Drawing.Point(136, 18);
		((System.Windows.Forms.Control)(object)this.CmbUsers).Name = "CmbUsers";
		((System.Windows.Forms.Control)(object)this.CmbUsers).Size = new System.Drawing.Size(368, 26);
		((System.Windows.Forms.Control)(object)this.CmbUsers).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbUsers).TextChanged += new System.EventHandler(CmbUsers_Leave);
		((System.Windows.Forms.Control)(object)this.CmbUsers).Leave += new System.EventHandler(CmbUsers_Leave);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 24);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(113, 15);
		this.label5.TabIndex = 11;
		this.label5.Text = "Select User Account";
		this.panel1.Controls.Add(this.panel9);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.label7);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(947, 569);
		this.panel1.TabIndex = 10;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.GrdAllocatedChartofAccount);
		this.panel9.Controls.Add(this.btnDeAllocate);
		this.panel9.Controls.Add(this.label15);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(460, 174);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(487, 395);
		this.panel9.TabIndex = 12;
		this.btnDeAllocate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnDeAllocate.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDeAllocate.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDeAllocate.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDeAllocate.Location = new System.Drawing.Point(460, 1);
		this.btnDeAllocate.Margin = new System.Windows.Forms.Padding(0);
		this.btnDeAllocate.Name = "btnDeAllocate";
		this.btnDeAllocate.Size = new System.Drawing.Size(25, 25);
		this.btnDeAllocate.TabIndex = 1;
		this.btnDeAllocate.UseVisualStyleBackColor = true;
		this.btnDeAllocate.Click += new System.EventHandler(btnDelete_Click);
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Dock = System.Windows.Forms.DockStyle.Top;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(0, 0);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(487, 27);
		this.label15.TabIndex = 4;
		this.label15.Text = "Allocated Accounts";
		this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel5.Controls.Add(this.btnAddAc);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Controls.Add(this.label11);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(0, 174);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(460, 395);
		this.panel5.TabIndex = 4;
		this.btnAddAc.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnAddAc.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnAddAc.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnAddAc.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnAddAc.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnAddAc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddAc.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.btnAddAc.Location = new System.Drawing.Point(432, 2);
		this.btnAddAc.Margin = new System.Windows.Forms.Padding(0);
		this.btnAddAc.Name = "btnAddAc";
		this.btnAddAc.Size = new System.Drawing.Size(25, 24);
		this.btnAddAc.TabIndex = 0;
		this.btnAddAc.UseVisualStyleBackColor = true;
		this.btnAddAc.Click += new System.EventHandler(btnAddAc_Click);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdDeAllocatedChartOfAccountToCustomGroup);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 27);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(460, 368);
		this.panel7.TabIndex = 4;
		this.label11.BackColor = System.Drawing.Color.Teal;
		this.label11.Dock = System.Windows.Forms.DockStyle.Top;
		this.label11.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(0, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(460, 27);
		this.label11.TabIndex = 3;
		this.label11.Text = "DeAllocated Detail Accounts";
		this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.Cmb2ndLevelAccounts);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGroupName);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 27);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(947, 147);
		this.panel4.TabIndex = 0;
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.btnShow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccountTypes);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbThirdLevelAccounts);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCoaCustomgroup);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Location = new System.Drawing.Point(17, 62);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(903, 81);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters For Deallocated Accounts";
		this.label18.BackColor = System.Drawing.SystemColors.ControlLight;
		this.label18.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(6, 26);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(270, 19);
		this.label18.TabIndex = 17;
		this.label18.Text = "Coa Custom Group";
		this.label18.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatAppearance.BorderSize = 0;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(839, 53);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(61, 23);
		this.btnShow.TabIndex = 3;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.CmbThirdLevelAccounts.AllowNull = (DefaultableBoolean)1;
		this.CmbThirdLevelAccounts.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbThirdLevelAccounts.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbThirdLevelAccounts.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbThirdLevelAccounts).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbThirdLevelAccounts.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbThirdLevelAccounts).Location = new System.Drawing.Point(563, 50);
		((System.Windows.Forms.Control)(object)this.CmbThirdLevelAccounts).Name = "CmbThirdLevelAccounts";
		((System.Windows.Forms.Control)(object)this.CmbThirdLevelAccounts).Size = new System.Drawing.Size(270, 26);
		((System.Windows.Forms.Control)(object)this.CmbThirdLevelAccounts).TabIndex = 2;
		this.cmbCoaCustomgroup.AllowNull = (DefaultableBoolean)1;
		this.cmbCoaCustomgroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCoaCustomgroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCoaCustomgroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCoaCustomgroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCoaCustomgroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCoaCustomgroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCoaCustomgroup).Location = new System.Drawing.Point(6, 49);
		((System.Windows.Forms.Control)(object)this.cmbCoaCustomgroup).Name = "cmbCoaCustomgroup";
		((System.Windows.Forms.Control)(object)this.cmbCoaCustomgroup).Size = new System.Drawing.Size(270, 26);
		((System.Windows.Forms.Control)(object)this.cmbCoaCustomgroup).TabIndex = 0;
		this.label9.BackColor = System.Drawing.SystemColors.ControlLight;
		this.label9.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(563, 26);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(270, 19);
		this.label9.TabIndex = 13;
		this.label9.Text = "3rd Level Account";
		this.label9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label10.BackColor = System.Drawing.SystemColors.ControlLight;
		this.label10.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(283, 26);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(270, 19);
		this.label10.TabIndex = 15;
		this.label10.Text = "Account Types";
		this.label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.Cmb2ndLevelAccounts.AllowNull = (DefaultableBoolean)1;
		this.Cmb2ndLevelAccounts.AutoCompleteMode = (AutoCompleteMode)4;
		this.Cmb2ndLevelAccounts.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.Cmb2ndLevelAccounts.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.Cmb2ndLevelAccounts).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.Cmb2ndLevelAccounts).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Cmb2ndLevelAccounts.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.Cmb2ndLevelAccounts).Location = new System.Drawing.Point(57, 265);
		((System.Windows.Forms.Control)(object)this.Cmb2ndLevelAccounts).Name = "Cmb2ndLevelAccounts";
		((System.Windows.Forms.Control)(object)this.Cmb2ndLevelAccounts).Size = new System.Drawing.Size(303, 26);
		((System.Windows.Forms.Control)(object)this.Cmb2ndLevelAccounts).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.Cmb2ndLevelAccounts).Visible = false;
		this.label8.BackColor = System.Drawing.SystemColors.ControlLight;
		this.label8.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(57, 244);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(303, 16);
		this.label8.TabIndex = 11;
		this.label8.Text = "2nd Level Account";
		this.label8.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label8.Visible = false;
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Dock = System.Windows.Forms.DockStyle.Top;
		this.label7.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(0, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(947, 27);
		this.label7.TabIndex = 2;
		this.label7.Text = "Accounts Allocation To Custom Group";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.tabControl1.Controls.Add(this.tabPage4);
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 57);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(990, 604);
		this.tabControl1.TabIndex = 12;
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(982, 575);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Custom Group Definition";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.panel15);
		this.panel3.Controls.Add(this.panel11);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(976, 569);
		this.panel3.TabIndex = 2;
		this.panel15.Controls.Add(this.BtnDeAllocateCustomGroup);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.grdAllocatedCustomGroupToUser);
		this.panel15.Controls.Add(this.label14);
		this.panel15.Controls.Add(this.BtnAllocateCustomGroup);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.grdDeAllocatedCustomGroupsToUser);
		this.panel15.Controls.Add(this.label13);
		this.panel15.Controls.Add(this.panel6);
		this.panel15.Controls.Add(this.label12);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel15.Location = new System.Drawing.Point(407, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(569, 569);
		this.panel15.TabIndex = 5;
		this.BtnDeAllocateCustomGroup.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnDeAllocateCustomGroup.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnDeAllocateCustomGroup.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnDeAllocateCustomGroup.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnDeAllocateCustomGroup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnDeAllocateCustomGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnDeAllocateCustomGroup.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.BtnDeAllocateCustomGroup.Location = new System.Drawing.Point(541, 387);
		this.BtnDeAllocateCustomGroup.Margin = new System.Windows.Forms.Padding(0);
		this.BtnDeAllocateCustomGroup.Name = "BtnDeAllocateCustomGroup";
		this.BtnDeAllocateCustomGroup.Size = new System.Drawing.Size(25, 25);
		this.BtnDeAllocateCustomGroup.TabIndex = 3;
		this.BtnDeAllocateCustomGroup.UseVisualStyleBackColor = true;
		this.BtnDeAllocateCustomGroup.Click += new System.EventHandler(BtnDeAllocateCustomGroup_Click);
		this.grdAllocatedCustomGroupToUser.AllowEdit = (InheritableBoolean)2;
		this.grdAllocatedCustomGroupToUser.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdAllocatedCustomGroupToUser_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdAllocatedCustomGroupToUser.DesignTimeLayout = grdAllocatedCustomGroupToUser_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdAllocatedCustomGroupToUser).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdAllocatedCustomGroupToUser.DynamicFiltering = true;
		this.grdAllocatedCustomGroupToUser.FilterMode = (FilterMode)2;
		this.grdAllocatedCustomGroupToUser.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdAllocatedCustomGroupToUser).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAllocatedCustomGroupToUser.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdAllocatedCustomGroupToUser).Location = new System.Drawing.Point(0, 413);
		((System.Windows.Forms.Control)(object)this.grdAllocatedCustomGroupToUser).Name = "grdAllocatedCustomGroupToUser";
		((ScrollableBase)this.grdAllocatedCustomGroupToUser).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAllocatedCustomGroupToUser).Size = new System.Drawing.Size(569, 156);
		((System.Windows.Forms.Control)(object)this.grdAllocatedCustomGroupToUser).TabIndex = 2;
		this.grdAllocatedCustomGroupToUser.TotalRow = (InheritableBoolean)1;
		this.grdAllocatedCustomGroupToUser.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAllocatedCustomGroupToUser).VisualStyle = (VisualStyle)4;
		this.label14.BackColor = System.Drawing.Color.Teal;
		this.label14.Dock = System.Windows.Forms.DockStyle.Top;
		this.label14.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(0, 386);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(569, 27);
		this.label14.TabIndex = 72;
		this.label14.Text = "Allocated Custom Groups To User";
		this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.BtnAllocateCustomGroup.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnAllocateCustomGroup.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnAllocateCustomGroup.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnAllocateCustomGroup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnAllocateCustomGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAllocateCustomGroup.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.BtnAllocateCustomGroup.Location = new System.Drawing.Point(541, 89);
		this.BtnAllocateCustomGroup.Margin = new System.Windows.Forms.Padding(0);
		this.BtnAllocateCustomGroup.Name = "BtnAllocateCustomGroup";
		this.BtnAllocateCustomGroup.Size = new System.Drawing.Size(25, 24);
		this.BtnAllocateCustomGroup.TabIndex = 1;
		this.BtnAllocateCustomGroup.UseVisualStyleBackColor = true;
		this.BtnAllocateCustomGroup.Click += new System.EventHandler(BtnAllocateCustomGroup_Click);
		this.grdDeAllocatedCustomGroupsToUser.AllowEdit = (InheritableBoolean)2;
		this.grdDeAllocatedCustomGroupsToUser.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdDeAllocatedCustomGroupsToUser_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDeAllocatedCustomGroupsToUser.DesignTimeLayout = grdDeAllocatedCustomGroupsToUser_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDeAllocatedCustomGroupsToUser).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdDeAllocatedCustomGroupsToUser.DynamicFiltering = true;
		this.grdDeAllocatedCustomGroupsToUser.FilterMode = (FilterMode)2;
		this.grdDeAllocatedCustomGroupsToUser.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDeAllocatedCustomGroupsToUser).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDeAllocatedCustomGroupsToUser.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDeAllocatedCustomGroupsToUser).Location = new System.Drawing.Point(0, 114);
		((System.Windows.Forms.Control)(object)this.grdDeAllocatedCustomGroupsToUser).Name = "grdDeAllocatedCustomGroupsToUser";
		((ScrollableBase)this.grdDeAllocatedCustomGroupsToUser).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDeAllocatedCustomGroupsToUser).Size = new System.Drawing.Size(569, 272);
		((System.Windows.Forms.Control)(object)this.grdDeAllocatedCustomGroupsToUser).TabIndex = 0;
		this.grdDeAllocatedCustomGroupsToUser.TotalRow = (InheritableBoolean)1;
		this.grdDeAllocatedCustomGroupsToUser.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDeAllocatedCustomGroupsToUser).VisualStyle = (VisualStyle)4;
		this.label13.BackColor = System.Drawing.Color.Teal;
		this.label13.Dock = System.Windows.Forms.DockStyle.Top;
		this.label13.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(0, 87);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(569, 27);
		this.label13.TabIndex = 69;
		this.label13.Text = "DeAllocated Custom Groups To User";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUsers);
		this.panel6.Controls.Add(this.label5);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 27);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(569, 60);
		this.panel6.TabIndex = 13;
		this.label12.BackColor = System.Drawing.Color.Teal;
		this.label12.Dock = System.Windows.Forms.DockStyle.Top;
		this.label12.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(0, 0);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(569, 27);
		this.label12.TabIndex = 12;
		this.label12.Text = "Allocate Custom Group To User";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(982, 575);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Coa Accounts Allocation To Custom Group";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(976, 569);
		this.panel8.TabIndex = 3;
		this.tabPage3.Controls.Add(this.panel2);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(956, 575);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History By User";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistorybyuser);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.panel14);
		this.panel2.Controls.Add(this.label17);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(956, 575);
		this.panel2.TabIndex = 0;
		this.grdhistorybyuser.AllowEdit = (InheritableBoolean)2;
		this.grdhistorybyuser.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdhistorybyuser_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistorybyuser.DesignTimeLayout = grdhistorybyuser_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistorybyuser).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistorybyuser.DynamicFiltering = true;
		this.grdhistorybyuser.FilterMode = (FilterMode)2;
		this.grdhistorybyuser.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistorybyuser).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistorybyuser.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdhistorybyuser).Location = new System.Drawing.Point(0, 100);
		((System.Windows.Forms.Control)(object)this.grdhistorybyuser).Name = "grdhistorybyuser";
		((ScrollableBase)this.grdhistorybyuser).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistorybyuser).Size = new System.Drawing.Size(956, 475);
		((System.Windows.Forms.Control)(object)this.grdhistorybyuser).TabIndex = 70;
		this.grdhistorybyuser.TotalRow = (InheritableBoolean)1;
		this.grdhistorybyuser.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistorybyuser).VisualStyle = (VisualStyle)4;
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.Dock = System.Windows.Forms.DockStyle.Top;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(0, 73);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(956, 27);
		this.label2.TabIndex = 73;
		this.label2.Text = "Detail";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel14.Controls.Add(this.btnShowHistory);
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUserforhistory);
		this.panel14.Controls.Add(this.label16);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 27);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(956, 46);
		this.panel14.TabIndex = 72;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatAppearance.BorderSize = 0;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(511, 11);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(55, 23);
		this.btnShowHistory.TabIndex = 12;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.cmbUserforhistory.AllowNull = (DefaultableBoolean)1;
		this.cmbUserforhistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUserforhistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUserforhistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUserforhistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbUserforhistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUserforhistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUserforhistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUserforhistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUserforhistory).Location = new System.Drawing.Point(136, 9);
		((System.Windows.Forms.Control)(object)this.cmbUserforhistory).Name = "cmbUserforhistory";
		((System.Windows.Forms.Control)(object)this.cmbUserforhistory).Size = new System.Drawing.Size(368, 26);
		((System.Windows.Forms.Control)(object)this.cmbUserforhistory).TabIndex = 0;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(6, 15);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(78, 15);
		this.label16.TabIndex = 11;
		this.label16.Text = "User Account";
		this.label17.BackColor = System.Drawing.Color.Teal;
		this.label17.Dock = System.Windows.Forms.DockStyle.Top;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(0, 0);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(956, 27);
		this.label17.TabIndex = 71;
		this.label17.Text = "Allocating accounts to custom groups for  users";
		this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPage4.Controls.Add(this.panel16);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Size = new System.Drawing.Size(956, 575);
		this.tabPage4.TabIndex = 3;
		this.tabPage4.Text = "User Allocate For Accounts Status";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel16.Controls.Add(this.panel18);
		this.panel16.Controls.Add(this.panel17);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(956, 575);
		this.panel16.TabIndex = 0;
		this.panel17.Controls.Add(this.button1);
		this.panel17.Controls.Add(this.label19);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(956, 29);
		this.panel17.TabIndex = 0;
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.grdUserAllcoateForAccountStatus);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel18.Location = new System.Drawing.Point(0, 29);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(956, 546);
		this.panel18.TabIndex = 1;
		this.label19.BackColor = System.Drawing.Color.Teal;
		this.label19.Dock = System.Windows.Forms.DockStyle.Top;
		this.label19.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(0, 0);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(956, 27);
		this.label19.TabIndex = 3;
		this.label19.Text = "User Allocate For Chart of Accounts Status (Show Or Hide)";
		this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
		this.button1.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.button1.Location = new System.Drawing.Point(2, 1);
		this.button1.Margin = new System.Windows.Forms.Padding(0);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(25, 24);
		this.button1.TabIndex = 4;
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.grdUserAllcoateForAccountStatus.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdUserAllcoateForAccountStatus_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdUserAllcoateForAccountStatus.DesignTimeLayout = grdUserAllcoateForAccountStatus_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdUserAllcoateForAccountStatus).Dock = System.Windows.Forms.DockStyle.Left;
		this.grdUserAllcoateForAccountStatus.DynamicFiltering = true;
		this.grdUserAllcoateForAccountStatus.FilterMode = (FilterMode)2;
		this.grdUserAllcoateForAccountStatus.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdUserAllcoateForAccountStatus).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdUserAllcoateForAccountStatus.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdUserAllcoateForAccountStatus).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdUserAllcoateForAccountStatus).Name = "grdUserAllcoateForAccountStatus";
		((ScrollableBase)this.grdUserAllcoateForAccountStatus).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdUserAllcoateForAccountStatus).Size = new System.Drawing.Size(554, 546);
		((System.Windows.Forms.Control)(object)this.grdUserAllcoateForAccountStatus).TabIndex = 1;
		this.grdUserAllcoateForAccountStatus.TotalRow = (InheritableBoolean)1;
		this.grdUserAllcoateForAccountStatus.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdUserAllcoateForAccountStatus).VisualStyle = (VisualStyle)4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(990, 661);
		base.Controls.Add(this.tabControl1);
		base.Controls.Add(this.panel10);
		base.KeyPreview = true;
		base.Name = "UserChartOfAccountManagement";
		this.Text = "User Chart Of Account Management";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(UserChartOfAccountManagement_FormClosing);
		base.Load += new System.EventHandler(frmAccountCustomGroup_Load);
		((System.ComponentModel.ISupportInitialize)this.grdDeAllocatedChartOfAccountToCustomGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGroupName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTypes).EndInit();
		((System.ComponentModel.ISupportInitialize)this.GrdAllocatedChartofAccount).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.ToolStripUserCoaManage.ResumeLayout(false);
		this.ToolStripUserCoaManage.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridCustomGroup).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbUsers).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbThirdLevelAccounts).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCoaCustomgroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.Cmb2ndLevelAccounts).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAllocatedCustomGroupToUser).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdDeAllocatedCustomGroupsToUser).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistorybyuser).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbUserforhistory).EndInit();
		this.tabPage4.ResumeLayout(false);
		this.panel16.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		this.panel18.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdUserAllcoateForAccountStatus).EndInit();
		base.ResumeLayout(false);
	}
}

}
