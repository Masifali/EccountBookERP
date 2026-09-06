using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Configurations;

public class UserRightsByCompany : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtScreens = new DataTable();

	private DataTable dtScreensTab4 = new DataTable();

	private DataTable OriginalRightsData = new DataTable();

	private DataTable RightsReportData = new DataTable();

	private DataTable FilteredRightsReportData = new DataTable();

	private List<string> FilterByColumnList = new List<string>();

	private IContainer components;

	private Panel panel1;

	private Panel panel5;

	private TabPage tabPage1;

	internal GridEX grdAllocatedScreens;

	internal GridEX grdUnAllocatedScreen;

	internal GridEX grdModules;

	private Panel panel3;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnSave;

	private ToolStripButton btnDelete;

	internal UltraCombo CmbCompany;

	private Label label5;

	private ToolStrip miniToolStrip;

	internal UltraCombo CmbUserName;

	private Label label1;

	private Panel panel8;

	private GroupBox groupBox1;

	private Panel panel7;

	private Label label2;

	private Panel panel17;

	private Panel panel18;

	private Panel panel19;

	private Panel panel14;

	private Panel panel15;

	private Panel panel16;

	private Panel panel11;

	private Panel panel13;

	private Panel panel12;

	private Label label4;

	private Label label10;

	private Label label7;

	private Label label9;

	private Label label8;

	private Label label6;

	private ToolStripButton BtnAllocateRightsToCompany;

	private ToolStrip toolStrip1;

	private Button btnShow;

	private TabPage tabPage2;

	private Panel panel2;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewFortab2;

	private Panel panel4;

	private Panel panel6;

	private GroupBox groupBox2;

	private Button btnShowUserAgainstScreen;

	private Label label3;

	internal UltraCombo CmbScreenName;

	private Panel panel9;

	private Panel panel10;

	private Label label13;

	internal UltraCombo CmbUserNameForTab2;

	internal GridEX GrdUnAllocatedUsers;

	private Label lblShowUnAllocated;

	private Label LblShowAllocated;

	internal GridEX GrdAllocatedUsers;

	private Label label11;

	internal UltraCombo CmbRightName;

	private TabPage tabPage3;

	private Panel panel20;

	private Label label12;

	private ToolStrip toolStrip3;

	private ToolStripButton toolStripButton1;

	private Panel panel21;

	private GroupBox groupBox3;

	private Label label14;

	internal UltraCombo CmbUserTab3;

	private Label label15;

	internal UltraCombo CmbModulesTab3;

	private Button btnShowTab3;

	private Label label16;

	internal GridEX grdTab3;

	private Label label17;

	internal UltraCombo CmbModuleTypeTab3;

	private GroupBox gBForAllCheck;

	private CheckBox chkGridGroupCollapseAll;

	private CheckBox chkGridExportAll;

	private CheckBox chkGridPrintAll;

	private CheckBox chkSaveLayoutAll;

	private CheckBox chkFieldChooserAll;

	private CheckBox chkCanViewAllRecordAll;

	private CheckBox chkPrintAll;

	private CheckBox chkDeleteAll;

	private CheckBox chkUpdateAll;

	private CheckBox chkSaveAll;

	private CheckBox chkGridGroupExpandAll;

	private CheckBox chkEnableGroupBox;

	private ToolStripButton btnSaveRights;

	private TabPage tabPage4;

	private ToolStripButton btnRefreshTab3;

	private ToolStrip toolStrip4;

	private ToolStripButton btnResetTab4;

	private ToolStripButton btnRefreshTab4;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel22;

	private Label label18;

	private Panel panel23;

	private GroupBox groupBox4;

	private CheckBox chkFilterByGroupExpand;

	private CheckBox chkFilterByGridCollapse;

	private CheckBox chkFilterByGridExport;

	private CheckBox chkFilterByGridPrint;

	private CheckBox chkFilterBySaveLayout;

	private CheckBox chkFilterByFieldChooser;

	private CheckBox chkFilterByCanViewAllRecord;

	private CheckBox chkFilterByPrint;

	private CheckBox chkFilterByView;

	private CheckBox chkFilterByUpdate;

	private CheckBox chkFilterBySave;

	private GroupBox groupBox5;

	private Label label19;

	internal UltraCombo CmbUserTab4;

	private Label label20;

	internal UltraCombo CmbModulesTab4;

	private Label label21;

	internal GridEX grdScreenRightsReport;

	private CheckBox chkFilterByDelete;

	private RadioButton rdNotHaveRight;

	private RadioButton rdHaveRight;

	private PictureBox btnFilter;

	private ToolStripButton btnPrint;

	private TabControl tabControl2;

	private TabPage tabPage5;

	private TabPage tabPage6;

	internal GridEX grdUserSpecialRights;

	private Panel panel24;

	private ToolStrip toolStrip5;

	private ToolStripButton BtnupdateSpecialRights;

	private ToolStripSeparator toolStripSeparator8;

	private Panel panel25;

	internal UltraCombo CmbCompanyNameSpecialRights;

	private Button BtnFetchRightsSpecialRights;

	internal UltraCombo CmbUserNameSpecialRights;

	private Label label22;

	private Label label23;

	private Panel panel26;

	private Label label24;

	private Panel panel27;

	private Label label25;

	public TabControl tabControl1;

	public UltraCombo CmbScreensTab4;

	public Button btnShowTab4;

	public UltraCombo CmbScreensTab3;

	private ToolStripButton BtnfrmUserModuleAdmin;

	private Label label26;

	internal UltraCombo CmbApplications;

	public UserRightsByCompany(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void UserRightsByCompany_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 750);
			tabControl1.TabPages.Remove(tabPage2);
			groupBox4.Enabled = true;
			CompanyFill();
			UserNameFillForTab2();
			ScreenFills();
			RightNameFill();
			ModuleTypeFill();
			ModulesFill();
			AllScreensFill();
			ModulesFillForTab4();
			AllScreensFillForTab4();
			UserNameFillForSpecialRights();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CompanyFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		try
		{
			DataTable dt = Company.GetAlldt(new Company
			{
				OrgCompanyTypeId = UserAccount.OrganizationId
			});
			if (dt.Rows.Count > 0)
			{
				DataTable table = new DataTable();
				table.Columns.Add("Id");
				table.Columns.Add("Company");
				table.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					table.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["CompName"]);
				}
				if (table.Rows.Count > 0)
				{
					DDL.BindDDL(table, CmbCompany, "Id", "Company", "Company Name", ZeroIndex: false);
					((UltraGridBase)CmbCompany).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				else
				{
					((Control)(object)CmbCompany).Text = string.Empty;
					((UltraGridBase)CmbCompany).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbCompany).Text = string.Empty;
				((UltraGridBase)CmbCompany).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UserNameFill()
	{
		try
		{
			DataTable dt = UserAccount.ReadUserByCompanyId(Conversion.ToInt(CmbCompany.Value), 1);
			if (dt.Rows.Count > 0)
			{
				DataTable dtUser = new DataTable();
				dtUser.Columns.Add("Id", typeof(int));
				dtUser.Columns.Add("UserName", typeof(string));
				dtUser.Columns.Add("UserRole", typeof(string));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtUser.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["UserName"], dt.Rows[i]["Title"]);
				}
				DDL.BindDDL(dtUser, CmbUserName, "Id", "UserName", "UserName", ZeroIndex: false);
			}
			else
			{
				((UltraGridBase)CmbUserName).DataSource = null;
				((UltraGridBase)CmbUserTab3).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ScreenFills()
	{
		try
		{
			DataTable dt = Company.GetAllScreensAgainstCompanyId(UserAccount.CompanyId);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbScreenName, "ScreenId", "ScreenAlias", "Screen Name", ZeroIndex: false);
				((UltraGridBase)CmbScreenName).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbScreenName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UserNameFillForTab2()
	{
		try
		{
			DataTable dt = UserAccount.ReadUserByCompanyId(UserAccount.CompanyId, 1);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbUserNameForTab2, "Id", "UserName", "User Name", false);
				DDL.BindDDLNew(dt, CmbUserTab3, "Id", "UserName", "User Name", false);
				DDL.BindDDLNew(dt, CmbUserTab4, "Id", "UserName", "User Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RightNameFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add("View", "View");
			dt.Rows.Add("Save", "Save");
			dt.Rows.Add("Update", "Update");
			DDL.BindDDL(dt, CmbRightName, "Id", "Name", "Right Name", ZeroIndex: false);
			((UltraGridBase)CmbRightName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCompanyName_Leave(object sender, EventArgs e)
	{
		try
		{
			UserNameFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ModuleTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Type", typeof(string));
			dt.Rows.Add(1, "Forms");
			dt.Rows.Add(2, "Reports");
			DDL.BindDDL(dt, CmbModuleTypeTab3, "Id", "Type", "Module Type", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ModulesFill(int UserId = 0)
	{
		try
		{
			DataTable dt = tblUserRights.GetModulesByCompany(UserAccount.CompanyId, 1, UserId);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbModulesTab3, "Id", "ModuleDescription", "Module Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllScreensFill(int UserId = 0)
	{
		try
		{
			dtScreens = tblUserRights.getScreensByModuleId(UserAccount.CompanyId, 0, 1, UserId);
			DDL.BindDDLNew(dtScreens, CmbScreensTab3, "Id", "ScreenAlias", "Screen Name", false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbModulesTab3_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbModulesTab3.Value) > 0)
			{
				EnumerableRowCollection<DataRow> rows = from row in dtScreens.AsEnumerable()
					where row.Field<int>("ModuleId") == Conversion.ToInt(CmbModulesTab3.Value)
					select row;
				DataTable dt = (rows.Any() ? rows.CopyToDataTable() : dtScreens.Clone());
				DDL.BindDDLNew(dt, CmbScreensTab3, "Id", "ScreenAlias", "Screen Name", false);
			}
			else
			{
				DDL.BindDDLNew(dtScreens, CmbScreensTab3, "Id", "ScreenAlias", "Screen Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ModulesFillForTab4(int UserId = 0)
	{
		try
		{
			DataTable dt = tblUserRights.GetModulesByCompany(UserAccount.CompanyId, 0, UserId);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbModulesTab4, "Id", "ModuleDescription", "Module Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllScreensFillForTab4(int UserId = 0)
	{
		try
		{
			dtScreensTab4 = tblUserRights.getScreensByModuleId(UserAccount.CompanyId, 0, 0, UserId);
			DDL.BindDDLNew(dtScreensTab4, CmbScreensTab4, "Id", "ScreenAlias", "Screen Name", false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbModulesTab4_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbModulesTab4.Value) > 0)
			{
				EnumerableRowCollection<DataRow> rows = from row in dtScreensTab4.AsEnumerable()
					where row.Field<int>("ModuleId") == Conversion.ToInt(CmbModulesTab4.Value)
					select row;
				DataTable dt = (rows.Any() ? rows.CopyToDataTable() : dtScreensTab4.Clone());
				DDL.BindDDLNew(dt, CmbScreensTab4, "Id", "ScreenAlias", "Screen Name", false);
			}
			else
			{
				DDL.BindDDLNew(dtScreensTab4, CmbScreensTab4, "Id", "ScreenAlias", "Screen Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			ModulesNameFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ModulesNameFill()
	{
		try
		{
			if (Conversion.ToInt(CmbCompany.Value) > 0 && Conversion.ToInt(CmbUserName.Value) > 0)
			{
				DataTable dt = tblUserRights.GetModulesAgainstCompanyId(Conversion.ToInt(CmbCompany.Value), Conversion.ToInt(CmbUserName.Value));
				if (dt.Rows.Count > 0)
				{
					grdModules.DataSource = dt;
					grdModules.RetrieveStructure();
					grdModules.RootTable.Columns["ModuleId"].Visible = false;
					grdModules.RootTable.Columns["ModuleType"].Visible = false;
					grdModules.RootTable.Columns["Allocated"].Width = 60;
					grdModules.RootTable.Columns["UnAllocated"].Width = 60;
					grdModules.RootTable.Columns["ModuleDescription"].EditType = (EditType)0;
					grdModules.RootTable.Columns["Allocated"].EditType = (EditType)0;
					grdModules.RootTable.Columns["UnAllocated"].EditType = (EditType)0;
					grdModules.RootTable.Columns["ModuleDescription"].FilterEditType = (FilterEditType)1;
					grdModules.RootTable.Columns["Allocated"].FilterEditType = (FilterEditType)1;
					grdModules.RootTable.Columns["UnAllocated"].FilterEditType = (FilterEditType)1;
					CommonServices.GridAutoAdjustmentNew(grdModules);
				}
				else
				{
					grdModules.ClearStructure();
				}
			}
			else
			{
				grdModules.ClearStructure();
				grdAllocatedScreens.ClearStructure();
				grdUnAllocatedScreen.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdModules_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbUserName).ActiveRow != null)
			{
				GetAllocatedScreensbyUser();
				GetUnAllocatedScreensbyUser();
				return;
			}
			throw new Exception("Please Select User First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void GetAllocatedScreensbyUser()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		try
		{
			DataTable dtAllocat = CompanyRights.UserWiseAllocatedOrUnAllocatedScreens(new ReportsParameters
			{
				AppModuleId = Conversion.ToInt(grdModules.CurrentRow.Cells["ModuleId"].Value),
				CompanyId = Convert.ToInt32(CmbCompany.Value),
				UserId = Convert.ToInt32(CmbUserName.Value),
				AppId = Conversion.ToInt(CmbApplications.Value),
				ActionId = 2
			});
			if (dtAllocat.Rows.Count > 0)
			{
				grdAllocatedScreens.DataSource = dtAllocat;
				grdAllocatedScreens.RetrieveStructure();
				grdSettingsAllocatedScreen();
			}
			else
			{
				grdAllocatedScreens.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void grdSettingsAllocatedScreen()
	{
		try
		{
			grdAllocatedScreens.RootTable.Columns["Id"].Visible = false;
			grdAllocatedScreens.RootTable.Columns["ScreenName"].Width = 320;
			grdAllocatedScreens.RootTable.Columns.Add("Select");
			grdAllocatedScreens.RootTable.Columns["Select"].ActAsSelector = true;
			grdAllocatedScreens.RootTable.Columns["Select"].UseHeaderSelector = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetUnAllocatedScreensbyUser()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		try
		{
			DataTable dt = CompanyRights.UserWiseAllocatedOrUnAllocatedScreens(new ReportsParameters
			{
				AppModuleId = Conversion.ToInt(grdModules.CurrentRow.Cells["ModuleId"].Value),
				CompanyId = Convert.ToInt32(CmbCompany.Value),
				UserId = Convert.ToInt32(CmbUserName.Value),
				AppId = Conversion.ToInt(CmbApplications.Value),
				ActionId = 1
			});
			if (dt.Rows.Count > 0)
			{
				grdUnAllocatedScreen.DataSource = dt;
				grdUnAllocatedScreen.RetrieveStructure();
				grdSettingsUnAllocatedScreen();
			}
			else
			{
				grdUnAllocatedScreen.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsUnAllocatedScreen()
	{
		try
		{
			grdUnAllocatedScreen.RootTable.Columns["Id"].Visible = false;
			grdUnAllocatedScreen.RootTable.Columns["ScreenName"].Width = 300;
			grdUnAllocatedScreen.RootTable.Columns.Add("Select");
			grdUnAllocatedScreen.RootTable.Columns["Select"].ActAsSelector = true;
			grdUnAllocatedScreen.RootTable.Columns["Select"].UseHeaderSelector = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)CmbCompany).ActiveRow == null)
			{
				MessageBox.Show("Company field Required");
				CmbCompany.Focus();
				return;
			}
			if (((UltraGridBase)CmbUserName).ActiveRow == null)
			{
				CmbUserName.Focus();
				throw new Exception("Username field Required");
			}
			if (((UltraGridBase)CmbApplications).ActiveRow == null)
			{
				CmbApplications.Focus();
				throw new Exception("Application field Required");
			}
			if (grdUnAllocatedScreen.GetCheckedRows().Count() > 0)
			{
				if (((UltraGridBase)CmbUserName).ActiveRow != null)
				{
					string secreenId = "";
					GridEXRow[] checkedRows = grdUnAllocatedScreen.GetCheckedRows();
					for (int i = 0; i < checkedRows.Length; i++)
					{
						secreenId = Convert.ToInt32(checkedRows[i].Cells["Id"].Value) + "," + secreenId;
					}
					CompanyRights.SaveAdminRights(new ReportsParameters
					{
						CompanyId = Convert.ToInt32(CmbCompany.Value),
						Id = Convert.ToInt32(CmbUserName.Value),
						SelectedScreedIds = secreenId,
						EntryUser = UserAccount.ID,
						AppId = Conversion.ToInt(CmbApplications.Value),
						OrganizationId = UserAccount.OrganizationId
					});
					MessageBox.Show("Screen Allocated To User Successfully");
					GetUnAllocatedScreensbyUser();
					GetAllocatedScreensbyUser();
				}
			}
			else
			{
				MessageBox.Show("Please select the rows first");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbUserName.Value) == 0 || ((Control)(object)CmbUserName).Text == string.Empty)
			{
				CmbUserName.Focus();
				throw new Exception("Please select User first");
			}
			if (Conversion.ToInt(CmbApplications.Value) == 0 || ((Control)(object)CmbApplications).Text == string.Empty)
			{
				CmbApplications.Focus();
				throw new Exception("Please select Application first");
			}
			if (grdAllocatedScreens.GetCheckedRows().Count() > 0)
			{
				CompanyRightsInsertAndDelete companyRights = new CompanyRightsInsertAndDelete();
				companyRights.UserId = Conversion.ToInt(CmbUserName.Value);
				companyRights.CompanyRightsInsertAndDeletelist = new List<CompanyRightsInsertAndDelete>();
				GridEXRow[] checkedRows = grdAllocatedScreens.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					CompanyRightsInsertAndDelete rights = new CompanyRightsInsertAndDelete();
					rights.OrganizationId = UserAccount.OrganizationId;
					rights.EntryUserId = UserAccount.ID;
					rights.AppId = Conversion.ToInt(CmbApplications.Value);
					rights.CompanyId = Convert.ToInt32(CmbCompany.Value);
					rights.UserId = Conversion.ToInt(CmbUserName.Value);
					rights.ScreenId = Convert.ToInt32(r.Cells["Id"].Value);
					companyRights.CompanyRightsInsertAndDeletelist.Add(rights);
				}
				CompanyRights.DeleteByUserId(companyRights);
				MessageBox.Show("Record Delete Successfully");
				GetUnAllocatedScreensbyUser();
				GetAllocatedScreensbyUser();
			}
			else
			{
				MessageBox.Show("Please select the rows first");
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
			GetUnAllocatedScreensbyUser();
			GetAllocatedScreensbyUser();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		((Control)(object)CmbCompany).Text = string.Empty;
		Reset();
	}

	private void UserRightsByCompany_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.F10)
			{
				BtnAllocateRightsToCompany.Visible = true;
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.F12)
			{
				BtnAllocateRightsToCompany.Visible = false;
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.G && tabControl1.SelectedIndex == 2)
			{
				ctrlGrdBar2.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAllocateRightsToCompany_Click(object sender, EventArgs e)
	{
		try
		{
			new ScreensAllocateToCompany(UserAccount).Show();
			BtnAllocateRightsToCompany.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowUserAgainstScreen_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbScreenName).ActiveRow != null || ((UltraGridBase)CmbUserNameForTab2).ActiveRow != null || ((UltraGridBase)CmbRightName).ActiveRow != null)
			{
				if (((UltraGridBase)CmbUserNameForTab2).ActiveRow != null && ((UltraGridBase)CmbScreenName).ActiveRow == null && ((UltraGridBase)CmbRightName).ActiveRow == null)
				{
					LblShowAllocated.Text = "Allocated Screen's To User With Rights";
					lblShowUnAllocated.Text = "Un-Allocated Screen's To User With Rights";
				}
				else if (((UltraGridBase)CmbScreenName).ActiveRow != null && ((UltraGridBase)CmbUserNameForTab2).ActiveRow == null && ((UltraGridBase)CmbRightName).ActiveRow == null)
				{
					LblShowAllocated.Text = "Allocated User's To Screen";
					lblShowUnAllocated.Text = "Un-Allocated User's To Screen";
				}
				else if (((UltraGridBase)CmbScreenName).ActiveRow != null && ((UltraGridBase)CmbUserNameForTab2).ActiveRow != null)
				{
					LblShowAllocated.Text = "Allocated User's To Screen";
					lblShowUnAllocated.Text = "Un-Allocated User's To Screen";
				}
				else if (((UltraGridBase)CmbScreenName).ActiveRow != null && ((UltraGridBase)CmbRightName).ActiveRow != null && ((UltraGridBase)CmbUserNameForTab2).ActiveRow == null)
				{
					LblShowAllocated.Text = "Allocated Screen's Rights To User's";
					lblShowUnAllocated.Text = "Un-Allocated Screen's To User's";
				}
				else if (((UltraGridBase)CmbUserNameForTab2).ActiveRow != null && ((UltraGridBase)CmbRightName).ActiveRow != null && ((UltraGridBase)CmbScreenName).ActiveRow == null)
				{
					LblShowAllocated.Text = "Allocated User's Rights To Screen's";
					lblShowUnAllocated.Text = "Un-Allocated User's Rights To Screen's";
				}
				else if (((UltraGridBase)CmbRightName).ActiveRow != null && ((UltraGridBase)CmbUserNameForTab2).ActiveRow == null && ((UltraGridBase)CmbScreenName).ActiveRow == null)
				{
					LblShowAllocated.Text = "Allocated Right's To Screen's And User's";
					lblShowUnAllocated.Text = "Un-Allocated Right's To Screen's And User's";
				}
				GetAllocatedUsers();
				GetUnAllocatedUsers();
				return;
			}
			GrdAllocatedUsers.DataSource = null;
			GrdUnAllocatedUsers.DataSource = null;
			LblShowAllocated.Text = "Allocated ";
			lblShowUnAllocated.Text = "Un-Allocated";
			throw new Exception("Please Select Atleast One Criteria First!");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetAllocatedUsers()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.CompanyId = UserAccount.CompanyId;
			obj.ActionId = 1;
			if (((UltraGridBase)CmbScreenName).ActiveRow != null)
			{
				obj.ScreenName = Conversion.ToString(((UltraDropDownBase)CmbScreenName).SelectedRow.Cells[2].Value);
			}
			obj.UserId = Conversion.ToInt(CmbUserNameForTab2.Value);
			obj.Activity = ((Control)(object)CmbRightName).Text;
			DataTable dt = UserAccount.AllocatedOrUnAllocatedUsersAgainstScreen(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dtAllocated = new DataTable();
				dtAllocated.Columns.Add("ModuleName");
				dtAllocated.Columns.Add("ScreenName");
				dtAllocated.Columns.Add("UserName");
				dtAllocated.Columns.Add("ViewRight");
				dtAllocated.Columns.Add("SaveRight");
				dtAllocated.Columns.Add("UpdateRight");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtAllocated.Rows.Add(dt.Rows[i]["ModuleDescription"], dt.Rows[i]["ScreenAlias"], dt.Rows[i]["UserName"], dt.Rows[i]["RightNameView"], dt.Rows[i]["RightNameSave"], dt.Rows[i]["RightNameUpdate"]);
				}
				GrdAllocatedUsers.DataSource = dtAllocated;
				GrdAllocatedUsers.RetrieveStructure();
				GrdAllocatedUsers.RootTable.Columns["UserName"].Width = 65;
				GrdAllocatedUsers.RootTable.Columns["ViewRight"].Width = 65;
				GrdAllocatedUsers.RootTable.Columns["SaveRight"].Width = 65;
				GrdAllocatedUsers.RootTable.Columns["UpdateRight"].Width = 80;
			}
			else
			{
				GrdAllocatedUsers.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GetUnAllocatedUsers()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.CompanyId = UserAccount.CompanyId;
			obj.ActionId = 0;
			if (((UltraGridBase)CmbScreenName).ActiveRow != null)
			{
				obj.ScreenName = Conversion.ToString(((UltraDropDownBase)CmbScreenName).SelectedRow.Cells[2].Value);
			}
			obj.UserId = Conversion.ToInt(CmbUserNameForTab2.Value);
			obj.Activity = ((Control)(object)CmbRightName).Text;
			DataTable dt = UserAccount.AllocatedOrUnAllocatedUsersAgainstScreen(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dtUnAllocated = new DataTable();
				dtUnAllocated.Columns.Add("ModuleName");
				dtUnAllocated.Columns.Add("ScreenName");
				dtUnAllocated.Columns.Add("UserName");
				dtUnAllocated.Columns.Add("ViewRight");
				dtUnAllocated.Columns.Add("SaveRight");
				dtUnAllocated.Columns.Add("UpdateRight");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtUnAllocated.Rows.Add(dt.Rows[i]["ModuleDescription"], dt.Rows[i]["ScreenAlias"], dt.Rows[i]["UserName"], dt.Rows[i]["RightNameView"], dt.Rows[i]["RightNameSave"], dt.Rows[i]["RightNameUpdate"]);
				}
				GrdUnAllocatedUsers.DataSource = dtUnAllocated;
				GrdUnAllocatedUsers.RetrieveStructure();
				GrdUnAllocatedUsers.RootTable.Columns["UserName"].Width = 65;
				GrdUnAllocatedUsers.RootTable.Columns["ViewRight"].Width = 65;
				GrdUnAllocatedUsers.RootTable.Columns["SaveRight"].Width = 65;
				GrdUnAllocatedUsers.RootTable.Columns["UpdateRight"].Width = 80;
			}
			else
			{
				GrdUnAllocatedUsers.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnNewFortab2_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbUserNameForTab2).Text = string.Empty;
			((Control)(object)CmbScreenName).Text = string.Empty;
			((Control)(object)CmbRightName).Text = string.Empty;
			GrdAllocatedUsers.DataSource = null;
			GrdUnAllocatedUsers.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void UserRightsByCompany_FormClosing(object sender, FormClosingEventArgs e)
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

	public void btnShowTab3_Click(object sender, EventArgs e)
	{
		try
		{
			OriginalRightsData.Rows.Clear();
			int ModuleId = Conversion.ToInt(CmbModulesTab3.Value);
			int ScreenId = Conversion.ToInt(CmbScreensTab3.Value);
			int UserId = Conversion.ToInt(CmbUserTab3.Value);
			DataTable dt = tblUserRights.getScreenRightsByModuleAndUserId(UserAccount.CompanyId, ModuleId, ScreenId, UserId, UserAccount.AppId);
			if (dt.Rows.Count > 0)
			{
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("Id", typeof(int));
				dtHistory.Columns.Add("ModuleId", typeof(int));
				dtHistory.Columns.Add("ModuleName");
				dtHistory.Columns.Add("ScreenId", typeof(int));
				dtHistory.Columns.Add("ScreenName", typeof(string));
				dtHistory.Columns.Add("UserId", typeof(int));
				dtHistory.Columns.Add("UserName", typeof(string));
				dtHistory.Columns.Add("SaveId", typeof(int));
				dtHistory.Columns.Add("Save", typeof(bool));
				dtHistory.Columns.Add("UpdateId", typeof(int));
				dtHistory.Columns.Add("Update", typeof(bool));
				dtHistory.Columns.Add("DeleteId", typeof(int));
				dtHistory.Columns.Add("Delete", typeof(bool));
				dtHistory.Columns.Add("PrintId", typeof(int));
				dtHistory.Columns.Add("Print", typeof(bool));
				dtHistory.Columns.Add("CanViewAllRecordId", typeof(int));
				dtHistory.Columns.Add("CanViewAllRecord", typeof(bool));
				dtHistory.Columns.Add("FieldChooserId", typeof(int));
				dtHistory.Columns.Add("FieldChooser", typeof(bool));
				dtHistory.Columns.Add("SaveLayoutId", typeof(int));
				dtHistory.Columns.Add("SaveLayout", typeof(bool));
				dtHistory.Columns.Add("GridPrintId", typeof(int));
				dtHistory.Columns.Add("GridPrint", typeof(bool));
				dtHistory.Columns.Add("GridExportId", typeof(int));
				dtHistory.Columns.Add("GridExport", typeof(bool));
				dtHistory.Columns.Add("GridGroupCollapseId", typeof(int));
				dtHistory.Columns.Add("GridGroupCollapse", typeof(bool));
				dtHistory.Columns.Add("GridGroupExpandId", typeof(int));
				dtHistory.Columns.Add("GridGroupExpand", typeof(bool));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtHistory.Rows.Add(i + 1, dt.Rows[i]["ModuleId"], dt.Rows[i]["ModuleDescription"], dt.Rows[i]["ScreenId"], dt.Rows[i]["ScreenAlias"], dt.Rows[i]["UserId"], dt.Rows[i]["UserName"], dt.Rows[i]["SaveId"], dt.Rows[i]["Save"], dt.Rows[i]["UpdateId"], dt.Rows[i]["Update"], dt.Rows[i]["DeleteId"], dt.Rows[i]["Delete"], dt.Rows[i]["PrintId"], dt.Rows[i]["Print"], dt.Rows[i]["CanViewAllRecordId"], dt.Rows[i]["CanViewAllRecord"], dt.Rows[i]["FieldChooserId"], dt.Rows[i]["FieldChooser"], dt.Rows[i]["SaveLayoutId"], dt.Rows[i]["SaveLayout"], dt.Rows[i]["GridPrintId"], dt.Rows[i]["GridPrint"], dt.Rows[i]["GridExportId"], dt.Rows[i]["GridExport"], dt.Rows[i]["GroupCollapseId"], dt.Rows[i]["GroupCollapse"], dt.Rows[i]["GroupExpandId"], dt.Rows[i]["GroupExpand"]);
				}
				grdTab3.DataSource = dtHistory;
				OriginalRightsData = dtHistory.Copy();
				grdTab3.RetrieveStructure();
				grdTab3.ColumnAutoResize = false;
				grdTab3.RootTable.Columns["Id"].Visible = false;
				grdTab3.RootTable.Columns["ModuleId"].Visible = false;
				grdTab3.RootTable.Columns["ScreenId"].Visible = false;
				grdTab3.RootTable.Columns["UserId"].Visible = false;
				grdTab3.RootTable.Columns["SaveId"].Visible = false;
				grdTab3.RootTable.Columns["UpdateId"].Visible = false;
				grdTab3.RootTable.Columns["DeleteId"].Visible = false;
				grdTab3.RootTable.Columns["PrintId"].Visible = false;
				grdTab3.RootTable.Columns["CanViewAllRecordId"].Visible = false;
				grdTab3.RootTable.Columns["GridPrintId"].Visible = false;
				grdTab3.RootTable.Columns["GridExportId"].Visible = false;
				grdTab3.RootTable.Columns["FieldChooserId"].Visible = false;
				grdTab3.RootTable.Columns["SaveLayoutId"].Visible = false;
				grdTab3.RootTable.Columns["GridGroupCollapseId"].Visible = false;
				grdTab3.RootTable.Columns["GridGroupExpandId"].Visible = false;
				grdTab3.RootTable.Columns["FieldChooser"].Visible = false;
				grdTab3.RootTable.Columns["SaveLayout"].Visible = false;
				grdTab3.RootTable.Columns["GridGroupCollapse"].Visible = false;
				grdTab3.RootTable.Columns["GridGroupExpand"].Visible = false;
				grdTab3.RootTable.Columns["ModuleName"].Width = 120;
				grdTab3.RootTable.Columns["ScreenName"].Width = 150;
				grdTab3.RootTable.Columns["UserName"].Width = 120;
				grdTab3.RootTable.Columns["Save"].Width = 40;
				grdTab3.RootTable.Columns["Update"].Width = 60;
				grdTab3.RootTable.Columns["Delete"].Width = 50;
				grdTab3.RootTable.Columns["Print"].Width = 40;
				grdTab3.RootTable.Columns["CanViewAllRecord"].Width = 100;
				grdTab3.RootTable.Columns["FieldChooser"].Width = 80;
				grdTab3.RootTable.Columns["SaveLayout"].Width = 90;
				grdTab3.RootTable.Columns["GridPrint"].Width = 70;
				grdTab3.RootTable.Columns["GridExport"].Width = 80;
				grdTab3.RootTable.Columns["GridGroupCollapse"].Width = 110;
				grdTab3.RootTable.Columns["GridGroupExpand"].Width = 110;
				grdTab3.AllowEdit = (InheritableBoolean)1;
				grdTab3.RootTable.Columns["Id"].EditType = (EditType)0;
				grdTab3.RootTable.Columns["ModuleName"].EditType = (EditType)0;
				grdTab3.RootTable.Columns["ModuleId"].EditType = (EditType)0;
				grdTab3.RootTable.Columns["ScreenName"].EditType = (EditType)0;
				grdTab3.RootTable.Columns["ScreenId"].EditType = (EditType)0;
				grdTab3.RootTable.Columns["UserName"].EditType = (EditType)0;
				grdTab3.RootTable.Columns["UserId"].EditType = (EditType)0;
				grdTab3.RootTable.Columns["SaveId"].EditType = (EditType)0;
				grdTab3.RootTable.Columns["UpdateId"].EditType = (EditType)0;
				grdTab3.RootTable.Columns["DeleteId"].EditType = (EditType)0;
				grdTab3.RootTable.Columns["PrintId"].EditType = (EditType)0;
				grdTab3.RootTable.Columns["CanViewAllRecordId"].EditType = (EditType)0;
				grdTab3.RootTable.Columns["FieldChooserId"].EditType = (EditType)0;
				grdTab3.RootTable.Columns["SaveLayoutId"].EditType = (EditType)0;
				grdTab3.RootTable.Columns["GridPrintId"].EditType = (EditType)0;
				grdTab3.RootTable.Columns["GridExportId"].EditType = (EditType)0;
				grdTab3.RootTable.Columns["GridGroupCollapseId"].EditType = (EditType)0;
				grdTab3.RootTable.Columns["GridGroupExpandId"].EditType = (EditType)0;
				grdTab3.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
				grdTab3.RootTable.Columns["ModuleName"].FilterEditType = (FilterEditType)1;
				grdTab3.RootTable.Columns["ModuleId"].FilterEditType = (FilterEditType)1;
				grdTab3.RootTable.Columns["ScreenName"].FilterEditType = (FilterEditType)1;
				grdTab3.RootTable.Columns["ScreenId"].FilterEditType = (FilterEditType)1;
				grdTab3.RootTable.Columns["UserName"].FilterEditType = (FilterEditType)1;
				grdTab3.RootTable.Columns["UserId"].FilterEditType = (FilterEditType)1;
				grdTab3.RootTable.Columns["SaveId"].FilterEditType = (FilterEditType)1;
				grdTab3.RootTable.Columns["UpdateId"].FilterEditType = (FilterEditType)1;
				grdTab3.RootTable.Columns["DeleteId"].FilterEditType = (FilterEditType)1;
				grdTab3.RootTable.Columns["PrintId"].FilterEditType = (FilterEditType)1;
				grdTab3.RootTable.Columns["CanViewAllRecordId"].FilterEditType = (FilterEditType)1;
				grdTab3.RootTable.Columns["GridPrintId"].FilterEditType = (FilterEditType)1;
				grdTab3.RootTable.Columns["GridExportId"].FilterEditType = (FilterEditType)1;
			}
			else
			{
				grdTab3.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkEnableGroupBox_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (chkEnableGroupBox.Checked)
			{
				gBForAllCheck.Enabled = true;
			}
			else
			{
				gBForAllCheck.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateSpecificColumn(CheckBox chkBox, string column)
	{
		try
		{
			GridEXRow[] rows = grdTab3.GetRows();
			foreach (GridEXRow obj in rows)
			{
				obj.BeginEdit();
				obj.Cells[column].Value = chkBox.Checked;
				obj.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkSaveAll_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateSpecificColumn(chkSaveAll, "Save");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdTab3_Click(object sender, EventArgs e)
	{
	}

	private void btnSaveRights_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_0679: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Expected O, but got Unknown
		try
		{
			tblUserRights obj = new tblUserRights();
			obj.CompanyId = UserAccount.CompanyId;
			obj.TblUserRightslst = new List<tblUserRights>();
			DataTable source = (DataTable)grdTab3.DataSource;
			List<DataRow> SaveList = source.AsEnumerable().Where(delegate(DataRow originalRow)
			{
				DataRow dataRow7 = OriginalRightsData.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == originalRow.Field<int>("Id"));
				return dataRow7 != null && originalRow.Field<bool>("Save") != dataRow7.Field<bool>("Save");
			}).ToList();
			List<DataRow> UpdateList = source.AsEnumerable().Where(delegate(DataRow originalRow)
			{
				DataRow dataRow6 = OriginalRightsData.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == originalRow.Field<int>("Id"));
				return dataRow6 != null && originalRow.Field<bool>("Update") != dataRow6.Field<bool>("Update");
			}).ToList();
			List<DataRow> DeleteList = source.AsEnumerable().Where(delegate(DataRow originalRow)
			{
				DataRow dataRow5 = OriginalRightsData.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == originalRow.Field<int>("Id"));
				return dataRow5 != null && originalRow.Field<bool>("Delete") != dataRow5.Field<bool>("Delete");
			}).ToList();
			List<DataRow> PrintList = source.AsEnumerable().Where(delegate(DataRow originalRow)
			{
				DataRow dataRow4 = OriginalRightsData.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == originalRow.Field<int>("Id"));
				return dataRow4 != null && originalRow.Field<bool>("Print") != dataRow4.Field<bool>("Print");
			}).ToList();
			List<DataRow> CanViewAllRecordList = source.AsEnumerable().Where(delegate(DataRow originalRow)
			{
				DataRow dataRow3 = OriginalRightsData.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == originalRow.Field<int>("Id"));
				return dataRow3 != null && originalRow.Field<bool>("CanViewAllRecord") != dataRow3.Field<bool>("CanViewAllRecord");
			}).ToList();
			List<DataRow> GridPrintList = source.AsEnumerable().Where(delegate(DataRow originalRow)
			{
				DataRow dataRow2 = OriginalRightsData.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == originalRow.Field<int>("Id"));
				return dataRow2 != null && originalRow.Field<bool>("GridPrint") != dataRow2.Field<bool>("GridPrint");
			}).ToList();
			List<DataRow> GridExportList = source.AsEnumerable().Where(delegate(DataRow originalRow)
			{
				DataRow dataRow = OriginalRightsData.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == originalRow.Field<int>("Id"));
				return dataRow != null && originalRow.Field<bool>("GridExport") != dataRow.Field<bool>("GridExport");
			}).ToList();
			if (SaveList.Count > 0)
			{
				for (int i2 = 0; i2 < SaveList.Count; i2++)
				{
					tblUserRights d7 = new tblUserRights();
					d7.UserId = Conversion.ToInt(SaveList[i2]["UserId"]);
					d7.ScreenID = Conversion.ToInt(SaveList[i2]["ScreenId"]);
					d7.RightsID = Conversion.ToInt(SaveList[i2]["SaveId"]);
					d7.Value = Conversion.ToBool(SaveList[i2]["Save"]);
					d7.RightName = "Save";
					d7.CompanyId = UserAccount.CompanyId;
					d7.AppId = UserAccount.AppId;
					obj.TblUserRightslst.Add(d7);
				}
			}
			if (UpdateList.Count > 0)
			{
				for (int n = 0; n < UpdateList.Count; n++)
				{
					tblUserRights d6 = new tblUserRights();
					d6.UserId = Conversion.ToInt(UpdateList[n]["UserId"]);
					d6.ScreenID = Conversion.ToInt(UpdateList[n]["ScreenId"]);
					d6.RightsID = Conversion.ToInt(UpdateList[n]["UpdateId"]);
					d6.Value = Conversion.ToBool(UpdateList[n]["Update"]);
					d6.RightName = "Update";
					d6.CompanyId = UserAccount.CompanyId;
					d6.AppId = UserAccount.AppId;
					obj.TblUserRightslst.Add(d6);
				}
			}
			if (DeleteList.Count > 0)
			{
				for (int m = 0; m < DeleteList.Count; m++)
				{
					tblUserRights d5 = new tblUserRights();
					d5.UserId = Conversion.ToInt(DeleteList[m]["UserId"]);
					d5.ScreenID = Conversion.ToInt(DeleteList[m]["ScreenId"]);
					d5.RightsID = Conversion.ToInt(DeleteList[m]["DeleteId"]);
					d5.Value = Conversion.ToBool(DeleteList[m]["Delete"]);
					d5.RightName = "Delete";
					d5.CompanyId = UserAccount.CompanyId;
					d5.AppId = UserAccount.AppId;
					obj.TblUserRightslst.Add(d5);
				}
			}
			if (PrintList.Count > 0)
			{
				for (int l = 0; l < PrintList.Count; l++)
				{
					tblUserRights d4 = new tblUserRights();
					d4.UserId = Conversion.ToInt(PrintList[l]["UserId"]);
					d4.ScreenID = Conversion.ToInt(PrintList[l]["ScreenId"]);
					d4.RightsID = Conversion.ToInt(PrintList[l]["PrintId"]);
					d4.Value = Conversion.ToBool(PrintList[l]["Print"]);
					d4.RightName = "Print";
					d4.CompanyId = UserAccount.CompanyId;
					d4.AppId = UserAccount.AppId;
					obj.TblUserRightslst.Add(d4);
				}
			}
			if (CanViewAllRecordList.Count > 0)
			{
				for (int k = 0; k < CanViewAllRecordList.Count; k++)
				{
					tblUserRights d3 = new tblUserRights();
					d3.UserId = Conversion.ToInt(CanViewAllRecordList[k]["UserId"]);
					d3.ScreenID = Conversion.ToInt(CanViewAllRecordList[k]["ScreenId"]);
					d3.RightsID = Conversion.ToInt(CanViewAllRecordList[k]["CanViewAllRecordId"]);
					d3.Value = Conversion.ToBool(CanViewAllRecordList[k]["CanViewAllRecord"]);
					d3.RightName = "CanView AllRecord";
					d3.CompanyId = UserAccount.CompanyId;
					d3.AppId = UserAccount.AppId;
					obj.TblUserRightslst.Add(d3);
				}
			}
			if (GridPrintList.Count > 0)
			{
				for (int j = 0; j < GridPrintList.Count; j++)
				{
					tblUserRights d2 = new tblUserRights();
					d2.UserId = Conversion.ToInt(GridPrintList[j]["UserId"]);
					d2.ScreenID = Conversion.ToInt(GridPrintList[j]["ScreenId"]);
					d2.RightsID = Conversion.ToInt(GridPrintList[j]["GridPrintId"]);
					d2.Value = Conversion.ToBool(GridPrintList[j]["GridPrint"]);
					d2.RightName = "Grid Print";
					d2.CompanyId = UserAccount.CompanyId;
					d2.AppId = UserAccount.AppId;
					obj.TblUserRightslst.Add(d2);
				}
			}
			if (GridExportList.Count > 0)
			{
				for (int i = 0; i < GridExportList.Count; i++)
				{
					tblUserRights d = new tblUserRights();
					d.UserId = Conversion.ToInt(GridExportList[i]["UserId"]);
					d.ScreenID = Conversion.ToInt(GridExportList[i]["ScreenId"]);
					d.RightsID = Conversion.ToInt(GridExportList[i]["GridExportId"]);
					d.Value = Conversion.ToBool(GridExportList[i]["GridExport"]);
					d.RightName = "Grid Export";
					d.CompanyId = UserAccount.CompanyId;
					d.AppId = UserAccount.AppId;
					obj.TblUserRightslst.Add(d);
				}
			}
			tblUserRights.UpdateRights(obj);
			MessageBox.Show("Rights has been updated Successfully!");
			ResetChecks();
			btnShowTab3_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetChecks()
	{
		try
		{
			chkSaveAll.Checked = false;
			chkUpdateAll.Checked = false;
			chkDeleteAll.Checked = false;
			chkPrintAll.Checked = false;
			chkCanViewAllRecordAll.Checked = false;
			chkGridExportAll.Checked = false;
			chkGridPrintAll.Checked = false;
			chkSaveLayoutAll.Checked = false;
			chkFieldChooserAll.Checked = false;
			chkGridGroupCollapseAll.Checked = false;
			chkGridGroupExpandAll.Checked = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkUpdateAll_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateSpecificColumn(chkUpdateAll, "Update");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkDeleteAll_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateSpecificColumn(chkDeleteAll, "Delete");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkPrintAll_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateSpecificColumn(chkPrintAll, "Print");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkCanViewAllRecordAll_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateSpecificColumn(chkCanViewAllRecordAll, "CanViewAllRecord");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkFieldChooserAll_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateSpecificColumn(chkFieldChooserAll, "FieldChooser");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkSaveLayoutAll_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateSpecificColumn(chkSaveLayoutAll, "SaveLayout");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkGridPrintAll_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateSpecificColumn(chkGridPrintAll, "GridPrint");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkGridExportAll_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateSpecificColumn(chkGridExportAll, "GridExport");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkGridGroupCollapseAll_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateSpecificColumn(chkGridGroupCollapseAll, "GridGroupCollapse");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkGridGroupExpandAll_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateSpecificColumn(chkGridGroupExpandAll, "GridGroupExpand");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdTab3_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdTab3.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdTab3_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdTab3.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshTab3_Click(object sender, EventArgs e)
	{
		try
		{
			AllScreensFill();
			ModulesFill();
			UserNameFillForTab2();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdTab3).Name);
			if (fs != null)
			{
				grdTab3.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void btnShowTab4_Click(object sender, EventArgs e)
	{
		try
		{
			RightsReportData.Rows.Clear();
			int ModuleId = Conversion.ToInt(CmbModulesTab4.Value);
			int ScreenId = Conversion.ToInt(CmbScreensTab4.Value);
			int UserId = Conversion.ToInt(CmbUserTab4.Value);
			DataTable dt = tblUserRights.getScreenRightsByModuleAndUserId(UserAccount.CompanyId, ModuleId, ScreenId, UserId, 0);
			if (dt.Rows.Count > 0)
			{
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("ModuleId", typeof(int));
				dtHistory.Columns.Add("ModuleDescription");
				dtHistory.Columns.Add("ScreenId", typeof(int));
				dtHistory.Columns.Add("ScreenAlias", typeof(string));
				dtHistory.Columns.Add("UserId", typeof(int));
				dtHistory.Columns.Add("UserName", typeof(string));
				dtHistory.Columns.Add("View", typeof(bool));
				dtHistory.Columns.Add("Save", typeof(bool));
				dtHistory.Columns.Add("Update", typeof(bool));
				dtHistory.Columns.Add("Delete", typeof(bool));
				dtHistory.Columns.Add("Print", typeof(bool));
				dtHistory.Columns.Add("CanViewAllRecord", typeof(bool));
				dtHistory.Columns.Add("FieldChooser", typeof(bool));
				dtHistory.Columns.Add("SaveLayout", typeof(bool));
				dtHistory.Columns.Add("GridPrint", typeof(bool));
				dtHistory.Columns.Add("GridExport", typeof(bool));
				dtHistory.Columns.Add("GroupCollapse", typeof(bool));
				dtHistory.Columns.Add("GroupExpand", typeof(bool));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtHistory.Rows.Add(dt.Rows[i]["ModuleId"], dt.Rows[i]["ModuleDescription"], dt.Rows[i]["ScreenId"], dt.Rows[i]["ScreenAlias"], dt.Rows[i]["UserId"], dt.Rows[i]["UserName"], dt.Rows[i]["View"], dt.Rows[i]["Save"], dt.Rows[i]["Update"], dt.Rows[i]["Delete"], dt.Rows[i]["Print"], dt.Rows[i]["CanViewAllRecord"], dt.Rows[i]["FieldChooser"], dt.Rows[i]["SaveLayout"], dt.Rows[i]["GridPrint"], dt.Rows[i]["GridExport"], dt.Rows[i]["GroupCollapse"], dt.Rows[i]["GroupExpand"]);
				}
				grdScreenRightsReport.DataSource = dtHistory;
				RightsReportData = dtHistory.Copy();
				FilteredRightsReportData = dtHistory.Copy();
				grdScreenRightsReport.RetrieveStructure();
				GridSettingForRightsReport();
			}
			else
			{
				grdScreenRightsReport.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForRightsReport()
	{
		try
		{
			grdScreenRightsReport.ColumnAutoResize = false;
			grdScreenRightsReport.RootTable.Columns["ModuleDescription"].Caption = "Module Name";
			grdScreenRightsReport.RootTable.Columns["ScreenAlias"].Caption = "Screen Name";
			grdScreenRightsReport.RootTable.Columns["ModuleId"].Visible = false;
			grdScreenRightsReport.RootTable.Columns["ScreenId"].Visible = false;
			grdScreenRightsReport.RootTable.Columns["UserId"].Visible = false;
			grdScreenRightsReport.RootTable.Columns["FieldChooser"].Visible = false;
			grdScreenRightsReport.RootTable.Columns["SaveLayout"].Visible = false;
			grdScreenRightsReport.RootTable.Columns["GroupCollapse"].Visible = false;
			grdScreenRightsReport.RootTable.Columns["GroupExpand"].Visible = false;
			grdScreenRightsReport.RootTable.Columns["ModuleDescription"].Width = 120;
			grdScreenRightsReport.RootTable.Columns["ScreenAlias"].Width = 150;
			grdScreenRightsReport.RootTable.Columns["UserName"].Width = 120;
			grdScreenRightsReport.RootTable.Columns["View"].Width = 40;
			grdScreenRightsReport.RootTable.Columns["Save"].Width = 40;
			grdScreenRightsReport.RootTable.Columns["Update"].Width = 60;
			grdScreenRightsReport.RootTable.Columns["Delete"].Width = 50;
			grdScreenRightsReport.RootTable.Columns["Print"].Width = 40;
			grdScreenRightsReport.RootTable.Columns["CanViewAllRecord"].Width = 100;
			grdScreenRightsReport.RootTable.Columns["FieldChooser"].Width = 80;
			grdScreenRightsReport.RootTable.Columns["SaveLayout"].Width = 80;
			grdScreenRightsReport.RootTable.Columns["GridPrint"].Width = 70;
			grdScreenRightsReport.RootTable.Columns["GridExport"].Width = 80;
			grdScreenRightsReport.RootTable.Columns["GroupCollapse"].Width = 90;
			grdScreenRightsReport.RootTable.Columns["GroupExpand"].Width = 90;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateFilterColumnList(CheckBox checkBox, string columnName)
	{
		try
		{
			if (checkBox.Checked)
			{
				FilterByColumnList.Add(columnName);
			}
			else
			{
				FilterByColumnList.Remove(columnName);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkFilterByView_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateFilterColumnList(chkFilterByView, "View");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkFilterBySave_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateFilterColumnList(chkFilterBySave, "Save");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkFilterByUpdate_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateFilterColumnList(chkFilterByUpdate, "Update");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkFilterByPrint_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateFilterColumnList(chkFilterByPrint, "Prrint");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkFilterByCanViewAllRecord_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateFilterColumnList(chkFilterByCanViewAllRecord, "CanViewAllRecord");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkFilterByGroupExpand_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateFilterColumnList(chkFilterByGroupExpand, "GroupExpand");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkFilterByFieldChooser_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateFilterColumnList(chkFilterByFieldChooser, "FieldChooser");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkFilterBySaveLayout_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateFilterColumnList(chkFilterBySaveLayout, "SaveLayout");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkFilterByDelete_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateFilterColumnList(chkFilterByDelete, "Delete");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkFilterByGridPrint_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateFilterColumnList(chkFilterByGridPrint, "GridPrint");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkFilterByGridExport_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateFilterColumnList(chkFilterByGridExport, "GridExport");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkFilterByGridCollapse_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateFilterColumnList(chkFilterByGridCollapse, "GroupCollapse");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnFilter_Click(object sender, EventArgs e)
	{
		try
		{
			DataTable filteredTable = RightsReportData.Clone();
			if (rdHaveRight.Checked)
			{
				foreach (DataRow row2 in RightsReportData.Rows)
				{
					if (FilterByColumnList.All((string column) => row2.Field<bool>(column)))
					{
						filteredTable.ImportRow(row2);
					}
				}
			}
			else if (rdNotHaveRight.Checked)
			{
				foreach (DataRow row in RightsReportData.Rows)
				{
					if (FilterByColumnList.Any((string column) => !row.Field<bool>(column)))
					{
						filteredTable.ImportRow(row);
					}
				}
			}
			FilteredRightsReportData = filteredTable;
			grdScreenRightsReport.DataSource = FilteredRightsReportData;
			grdScreenRightsReport.RetrieveStructure();
			GridSettingForRightsReport();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetTab4_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbModulesTab4).Text = string.Empty;
			((Control)(object)CmbScreensTab4).Text = string.Empty;
			((Control)(object)CmbUserTab4).Text = string.Empty;
			chkFilterByView.Checked = false;
			chkFilterBySave.Checked = false;
			chkFilterByUpdate.Checked = false;
			chkFilterByDelete.Checked = false;
			chkFilterByPrint.Checked = false;
			chkFilterByCanViewAllRecord.Checked = false;
			chkFilterByGridPrint.Checked = false;
			chkFilterByGridExport.Checked = false;
			chkFilterBySaveLayout.Checked = false;
			chkFilterByFieldChooser.Checked = false;
			chkFilterByGroupExpand.Checked = false;
			chkFilterByGridCollapse.Checked = false;
			grdScreenRightsReport.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshTab4_Click(object sender, EventArgs e)
	{
		try
		{
			ModulesFillForTab4();
			AllScreensFillForTab4();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (FilteredRightsReportData.Rows.Count > 0 && FilteredRightsReportData != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(FilteredRightsReportData, "005-UserReightsReport.rpt");
				return;
			}
			throw new Exception("Record not found for Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbUserTab3_Leave(object sender, EventArgs e)
	{
		try
		{
			ModulesFill(Conversion.ToInt(CmbUserTab3.Value));
			AllScreensFill(Conversion.ToInt(CmbUserTab3.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbUserTab4_Leave(object sender, EventArgs e)
	{
		try
		{
			ModulesFillForTab4(Conversion.ToInt(CmbUserTab4.Value));
			AllScreensFillForTab4(Conversion.ToInt(CmbUserTab4.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UserNameFillForSpecialRights()
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
				DDL.BindDDLNew(dt, CmbUserNameSpecialRights, "Id", "UserName", "UserName", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbUserNameSpecialRights_Leave(object sender, EventArgs e)
	{
		try
		{
			CompanyNameFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CompanyNameFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			DataTable dt = UserAccountAllocation.GetAllCompaniesByUserId(new UserAccountAllocation
			{
				OrganizationId = UserAccount.OrganizationId,
				IsActive = 1,
				UserAccountId = Conversion.ToInt(CmbUserNameSpecialRights.Value)
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCompanyNameSpecialRights, "CompanyId", "CompName", "Company", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbUserNameSpecialRights).ActiveRow == null)
		{
			MessageBox.Show("User Name Field is Required");
			CmbUserNameSpecialRights.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCompanyNameSpecialRights).ActiveRow == null)
		{
			MessageBox.Show("Company Field is Required");
			CmbCompanyNameSpecialRights.Focus();
			return false;
		}
		return true;
	}

	private void BtnFetchSpecialRights_Click(object sender, EventArgs e)
	{
		try
		{
			FormValidation();
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		try
		{
			DataTable lst = tblUserRights.UserScreenRights_Special(new ReportsParameters
			{
				CompanyId = Conversion.ToInt(CmbCompanyNameSpecialRights.Value),
				UserId = Conversion.ToInt(CmbUserNameSpecialRights.Value)
			});
			if (lst.Rows.Count > 0)
			{
				grdUserSpecialRights.DataSource = null;
				DataTable dt = new DataTable();
				dt.Columns.Add("ModuleID");
				dt.Columns.Add("RightName");
				dt.Columns.Add("ModuleDescription");
				dt.Columns.Add("ScreenName");
				dt.Columns.Add("ScreenAlias");
				dt.Columns.Add("RightsID");
				dt.Columns.Add("ScreenID");
				dt.Columns.Add("Value");
				dt.Clear();
				foreach (DataRow r in lst.Rows)
				{
					dt.Rows.Add(r["ModuleID"], r["RightName"], r["ModuleDescription"], r["ScreenName"], r["ScreenAlias"], r["RightId"], r["ScreenId"], r["Value"]);
				}
				grdUserSpecialRights.DataSource = dt;
				grdUserSpecialRightsSettings();
			}
			else
			{
				grdUserSpecialRights.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdUserSpecialRightsSettings()
	{
		try
		{
			grdUserSpecialRights.RootTable.Columns["RightName"].EditType = (EditType)0;
			grdUserSpecialRights.RootTable.Columns["ScreenAlias"].EditType = (EditType)0;
			grdUserSpecialRights.RootTable.Columns["RightsID"].EditType = (EditType)0;
			grdUserSpecialRights.RootTable.Columns["ScreenID"].EditType = (EditType)0;
			grdUserSpecialRights.RootTable.Columns["Value"].EditType = (EditType)8;
			grdUserSpecialRights.RootTable.Columns["RightName"].FilterEditType = (FilterEditType)1;
			grdUserSpecialRights.RootTable.Columns["ScreenAlias"].FilterEditType = (FilterEditType)1;
			grdUserSpecialRights.RootTable.Columns["RightsID"].FilterEditType = (FilterEditType)1;
			grdUserSpecialRights.RootTable.Columns["ScreenID"].FilterEditType = (FilterEditType)1;
			grdUserSpecialRights.RootTable.Columns["Value"].FilterEditType = (FilterEditType)7;
			grdUserSpecialRights.RootTable.Columns["Value"].ColumnType = (ColumnType)3;
			grdUserSpecialRights.RootTable.Columns["Value"].CheckBoxTrueValue = "True";
			grdUserSpecialRights.RootTable.Columns["Value"].CheckBoxFalseValue = "False";
			grdUserSpecialRights.RootTable.Columns["Value"].UseHeaderSelector = true;
			grdUserSpecialRights.CollapseGroups();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdUserSpecialRights_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		grdUserSpecialRights.UpdateData();
	}

	private void BtnupdateSpecialRights_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		try
		{
			tblUserRights tblUser1 = new tblUserRights();
			tblUser1.CompanyId = UserAccount.CompanyId;
			tblUser1.UserId = Conversion.ToInt(CmbUserNameSpecialRights.Value);
			tblUser1.TblUserRightslst = new List<tblUserRights>();
			GridEXRow[] dataRows = grdUserSpecialRights.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				tblUserRights tblUserRights = new tblUserRights();
				tblUserRights.UserId = Conversion.ToInt(CmbUserNameSpecialRights.Value);
				tblUserRights.ScreenID = Conversion.ToInt((object)r.Cells["ScreenID"].Value.ToString());
				tblUserRights.RightsID = Conversion.ToInt((object)r.Cells["RightsID"].Value.ToString());
				tblUserRights.Value = Conversion.ToBool((object)r.Cells["Value"].Value.ToString());
				tblUserRights.CompanyId = Conversion.ToInt(CmbCompanyNameSpecialRights.Value);
				tblUser1.TblUserRightslst.Add(tblUserRights);
			}
			tblUserRights.SpecialRightsUpdate(tblUser1);
			MessageBox.Show("User Rights for " + ((Control)(object)CmbUserNameSpecialRights).Text + " has been updated Successfully!");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BtnfrmUserModuleAdmin_Click(object sender, EventArgs e)
	{
		try
		{
			new frmUserModuleAdmin(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbUserName_Leave(object sender, EventArgs e)
	{
		try
		{
			DataTable dt = ApplicationsAllocateToUser.ApplicationsAllocateToUser_AllocatedData(UserAccount.CompanyId, Conversion.ToInt(CmbUserName.Value));
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbApplications, "AppId", "App", "Applications", false);
				((UltraGridBase)CmbApplications).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((Control)(object)CmbApplications).Text = string.Empty;
				((UltraGridBase)CmbApplications).DataSource = null;
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
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
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
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
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
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Expected O, but got Unknown
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Expected O, but got Unknown
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Expected O, but got Unknown
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Expected O, but got Unknown
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
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
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Expected O, but got Unknown
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Expected O, but got Unknown
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Expected O, but got Unknown
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Expected O, but got Unknown
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Expected O, but got Unknown
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Expected O, but got Unknown
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Expected O, but got Unknown
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Expected O, but got Unknown
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0715: Expected O, but got Unknown
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Expected O, but got Unknown
		//IL_0758: Unknown result type (might be due to invalid IL or missing references)
		//IL_0762: Expected O, but got Unknown
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Expected O, but got Unknown
		//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d0: Expected O, but got Unknown
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a1: Expected O, but got Unknown
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b7: Expected O, but got Unknown
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c2: Expected O, but got Unknown
		//IL_08ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d8: Expected O, but got Unknown
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_095c: Expected O, but got Unknown
		//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b4: Expected O, but got Unknown
		//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ca: Expected O, but got Unknown
		//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f6: Expected O, but got Unknown
		//IL_0aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab1: Expected O, but got Unknown
		//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac7: Expected O, but got Unknown
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad2: Expected O, but got Unknown
		//IL_0b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6c: Expected O, but got Unknown
		//IL_4626: Unknown result type (might be due to invalid IL or missing references)
		//IL_4630: Expected O, but got Unknown
		//IL_463d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4647: Expected O, but got Unknown
		//IL_6a52: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a5c: Expected O, but got Unknown
		GridEXLayout grdAllocatedScreens_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdUnAllocatedScreen_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdModules_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Configurations.UserRightsByCompany));
		GridEXLayout GrdUnAllocatedUsers_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdAllocatedUsers_DesignTimeLayout = new GridEXLayout();
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
		GridEXLayout grdTab3_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		GridEXLayout grdUserSpecialRights_DesignTimeLayout = new GridEXLayout();
		Appearance appearance125 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		GridEXLayout grdScreenRightsReport_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.panel18 = new System.Windows.Forms.Panel();
		this.grdAllocatedScreens = new GridEX();
		this.panel19 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.panel15 = new System.Windows.Forms.Panel();
		this.grdUnAllocatedScreen = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdModules = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnShow = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbCompany = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbUserName = new UltraCombo();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.BtnAllocateRightsToCompany = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.GrdUnAllocatedUsers = new GridEX();
		this.lblShowUnAllocated = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.GrdAllocatedUsers = new GridEX();
		this.LblShowAllocated = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbRightName = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.CmbUserNameForTab2 = new UltraCombo();
		this.btnShowUserAgainstScreen = new System.Windows.Forms.Button();
		this.CmbScreenName = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewFortab2 = new System.Windows.Forms.ToolStripButton();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.grdTab3 = new GridEX();
		this.panel27 = new System.Windows.Forms.Panel();
		this.label25 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel21 = new System.Windows.Forms.Panel();
		this.chkGridGroupExpandAll = new System.Windows.Forms.CheckBox();
		this.gBForAllCheck = new System.Windows.Forms.GroupBox();
		this.chkGridExportAll = new System.Windows.Forms.CheckBox();
		this.chkGridPrintAll = new System.Windows.Forms.CheckBox();
		this.chkCanViewAllRecordAll = new System.Windows.Forms.CheckBox();
		this.chkPrintAll = new System.Windows.Forms.CheckBox();
		this.chkDeleteAll = new System.Windows.Forms.CheckBox();
		this.chkUpdateAll = new System.Windows.Forms.CheckBox();
		this.chkSaveAll = new System.Windows.Forms.CheckBox();
		this.chkGridGroupCollapseAll = new System.Windows.Forms.CheckBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.chkEnableGroupBox = new System.Windows.Forms.CheckBox();
		this.btnShowTab3 = new System.Windows.Forms.Button();
		this.label14 = new System.Windows.Forms.Label();
		this.CmbUserTab3 = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.CmbModulesTab3 = new UltraCombo();
		this.CmbScreensTab3 = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.CmbModuleTypeTab3 = new UltraCombo();
		this.chkSaveLayoutAll = new System.Windows.Forms.CheckBox();
		this.label17 = new System.Windows.Forms.Label();
		this.chkFieldChooserAll = new System.Windows.Forms.CheckBox();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshTab3 = new System.Windows.Forms.ToolStripButton();
		this.btnSaveRights = new System.Windows.Forms.ToolStripButton();
		this.BtnfrmUserModuleAdmin = new System.Windows.Forms.ToolStripButton();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.grdUserSpecialRights = new GridEX();
		this.panel24 = new System.Windows.Forms.Panel();
		this.panel25 = new System.Windows.Forms.Panel();
		this.panel26 = new System.Windows.Forms.Panel();
		this.toolStrip5 = new System.Windows.Forms.ToolStrip();
		this.BtnupdateSpecialRights = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbCompanyNameSpecialRights = new UltraCombo();
		this.BtnFetchRightsSpecialRights = new System.Windows.Forms.Button();
		this.CmbUserNameSpecialRights = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdScreenRightsReport = new GridEX();
		this.panel23 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.btnFilter = new System.Windows.Forms.PictureBox();
		this.rdNotHaveRight = new System.Windows.Forms.RadioButton();
		this.rdHaveRight = new System.Windows.Forms.RadioButton();
		this.chkFilterByDelete = new System.Windows.Forms.CheckBox();
		this.chkFilterByView = new System.Windows.Forms.CheckBox();
		this.chkFilterByGridExport = new System.Windows.Forms.CheckBox();
		this.chkFilterByGridPrint = new System.Windows.Forms.CheckBox();
		this.chkFilterByCanViewAllRecord = new System.Windows.Forms.CheckBox();
		this.chkFilterByPrint = new System.Windows.Forms.CheckBox();
		this.chkFilterByUpdate = new System.Windows.Forms.CheckBox();
		this.chkFilterBySave = new System.Windows.Forms.CheckBox();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.btnShowTab4 = new System.Windows.Forms.Button();
		this.label19 = new System.Windows.Forms.Label();
		this.CmbUserTab4 = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.CmbModulesTab4 = new UltraCombo();
		this.CmbScreensTab4 = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.chkFilterBySaveLayout = new System.Windows.Forms.CheckBox();
		this.chkFilterByFieldChooser = new System.Windows.Forms.CheckBox();
		this.chkFilterByGridCollapse = new System.Windows.Forms.CheckBox();
		this.chkFilterByGroupExpand = new System.Windows.Forms.CheckBox();
		this.panel22 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnResetTab4 = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshTab4 = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.label26 = new System.Windows.Forms.Label();
		this.CmbApplications = new UltraCombo();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel17.SuspendLayout();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAllocatedScreens).BeginInit();
		this.panel19.SuspendLayout();
		this.panel14.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdUnAllocatedScreen).BeginInit();
		this.panel16.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdModules).BeginInit();
		this.panel12.SuspendLayout();
		this.panel8.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCompany).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUserName).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdUnAllocatedUsers).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdAllocatedUsers).BeginInit();
		this.panel6.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRightName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUserNameForTab2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbScreenName).BeginInit();
		this.toolStrip2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdTab3).BeginInit();
		this.panel27.SuspendLayout();
		this.panel21.SuspendLayout();
		this.gBForAllCheck.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbUserTab3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbModulesTab3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbScreensTab3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbModuleTypeTab3).BeginInit();
		this.panel20.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabPage6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdUserSpecialRights).BeginInit();
		this.panel25.SuspendLayout();
		this.panel26.SuspendLayout();
		this.toolStrip5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCompanyNameSpecialRights).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUserNameSpecialRights).BeginInit();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdScreenRightsReport).BeginInit();
		this.panel23.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnFilter).BeginInit();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbUserTab4).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbModulesTab4).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbScreensTab4).BeginInit();
		this.panel22.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbApplications).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1039, 615);
		this.panel1.TabIndex = 0;
		this.panel5.Controls.Add(this.tabControl1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1039, 615);
		this.panel5.TabIndex = 3;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Controls.Add(this.tabPage4);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1039, 615);
		this.tabControl1.TabIndex = 51;
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1031, 586);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Screens Allocate To User";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.panel17);
		this.panel3.Controls.Add(this.panel14);
		this.panel3.Controls.Add(this.panel11);
		this.panel3.Controls.Add(this.panel8);
		this.panel3.Controls.Add(this.panel7);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1025, 580);
		this.panel3.TabIndex = 48;
		this.panel17.Controls.Add(this.panel18);
		this.panel17.Controls.Add(this.panel19);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel17.Location = new System.Drawing.Point(626, 169);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(338, 411);
		this.panel17.TabIndex = 29;
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.grdAllocatedScreens);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel18.Location = new System.Drawing.Point(0, 46);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(338, 365);
		this.panel18.TabIndex = 1;
		this.grdAllocatedScreens.AllowEdit = (InheritableBoolean)2;
		this.grdAllocatedScreens.ColumnAutoResize = true;
		this.grdAllocatedScreens.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdAllocatedScreens_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdAllocatedScreens.DesignTimeLayout = grdAllocatedScreens_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdAllocatedScreens).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdAllocatedScreens.DynamicFiltering = true;
		this.grdAllocatedScreens.FilterMode = (FilterMode)2;
		this.grdAllocatedScreens.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdAllocatedScreens).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAllocatedScreens.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdAllocatedScreens).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdAllocatedScreens).Name = "grdAllocatedScreens";
		((ScrollableBase)this.grdAllocatedScreens).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAllocatedScreens).Size = new System.Drawing.Size(338, 365);
		((System.Windows.Forms.Control)(object)this.grdAllocatedScreens).TabIndex = 46;
		this.grdAllocatedScreens.TotalRow = (InheritableBoolean)1;
		this.grdAllocatedScreens.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAllocatedScreens).VisualStyle = (VisualStyle)4;
		this.panel19.BackColor = System.Drawing.Color.Teal;
		this.panel19.Controls.Add(this.label10);
		this.panel19.Controls.Add(this.label7);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(338, 46);
		this.panel19.TabIndex = 0;
		this.label10.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.label10.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.label10.ForeColor = System.Drawing.Color.LavenderBlush;
		this.label10.Location = new System.Drawing.Point(0, 19);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(338, 27);
		this.label10.TabIndex = 32;
		this.label10.Text = "Allocated screens of the selected module will appear here. Select the screen you want to deallocate for the user here";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(6, 2);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(129, 17);
		this.label7.TabIndex = 29;
		this.label7.Text = "Allocated Screens";
		this.panel14.Controls.Add(this.panel15);
		this.panel14.Controls.Add(this.panel16);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel14.Location = new System.Drawing.Point(331, 169);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(295, 411);
		this.panel14.TabIndex = 28;
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.grdUnAllocatedScreen);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(0, 46);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(295, 365);
		this.panel15.TabIndex = 1;
		this.grdUnAllocatedScreen.AllowEdit = (InheritableBoolean)2;
		this.grdUnAllocatedScreen.ColumnAutoResize = true;
		this.grdUnAllocatedScreen.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdUnAllocatedScreen_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdUnAllocatedScreen.DesignTimeLayout = grdUnAllocatedScreen_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdUnAllocatedScreen).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdUnAllocatedScreen.DynamicFiltering = true;
		this.grdUnAllocatedScreen.FilterMode = (FilterMode)2;
		this.grdUnAllocatedScreen.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdUnAllocatedScreen).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdUnAllocatedScreen.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdUnAllocatedScreen).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdUnAllocatedScreen).Name = "grdUnAllocatedScreen";
		((ScrollableBase)this.grdUnAllocatedScreen).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdUnAllocatedScreen).Size = new System.Drawing.Size(295, 365);
		((System.Windows.Forms.Control)(object)this.grdUnAllocatedScreen).TabIndex = 46;
		this.grdUnAllocatedScreen.TotalRow = (InheritableBoolean)1;
		this.grdUnAllocatedScreen.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdUnAllocatedScreen).VisualStyle = (VisualStyle)4;
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label9);
		this.panel16.Controls.Add(this.label8);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(295, 46);
		this.panel16.TabIndex = 0;
		this.label9.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.label9.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.label9.ForeColor = System.Drawing.Color.LavenderBlush;
		this.label9.Location = new System.Drawing.Point(0, 19);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(295, 27);
		this.label9.TabIndex = 31;
		this.label9.Text = "\"Un-Allocated screens of the selected module will appear here. Select the screen you want to allocate for the user here.";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(3, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(154, 17);
		this.label8.TabIndex = 30;
		this.label8.Text = "Un-Allocated Screens";
		this.panel11.Controls.Add(this.panel13);
		this.panel11.Controls.Add(this.panel12);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel11.Location = new System.Drawing.Point(0, 169);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(331, 411);
		this.panel11.TabIndex = 27;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdModules);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 46);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(331, 365);
		this.panel13.TabIndex = 1;
		this.grdModules.AllowEdit = (InheritableBoolean)2;
		this.grdModules.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdModules_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdModules.DesignTimeLayout = grdModules_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdModules).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdModules.DynamicFiltering = true;
		this.grdModules.FilterMode = (FilterMode)2;
		this.grdModules.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdModules).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdModules.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdModules).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdModules).Name = "grdModules";
		((ScrollableBase)this.grdModules).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdModules).Size = new System.Drawing.Size(331, 365);
		((System.Windows.Forms.Control)(object)this.grdModules).TabIndex = 46;
		this.grdModules.TotalRow = (InheritableBoolean)1;
		this.grdModules.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdModules).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdModules).Click += new System.EventHandler(grdModules_Click);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label6);
		this.panel12.Controls.Add(this.label4);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(331, 46);
		this.panel12.TabIndex = 0;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(2, 2);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(173, 17);
		this.label6.TabIndex = 27;
		this.label6.Text = "Modules in Organization";
		this.label4.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.label4.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.LavenderBlush;
		this.label4.Location = new System.Drawing.Point(0, 19);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(331, 27);
		this.label4.TabIndex = 28;
		this.label4.Text = "The selected module's screen will be filtered in the allocated or deallocated grid next to it";
		this.panel8.Controls.Add(this.groupBox1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 59);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1025, 110);
		this.panel8.TabIndex = 26;
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbApplications);
		this.groupBox1.Controls.Add(this.btnShow);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCompany);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUserName);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(6, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(948, 102);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(359, 67);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(55, 29);
		this.btnShow.TabIndex = 3;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(14, 19);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(93, 15);
		this.label5.TabIndex = 21;
		this.label5.Text = "Company Name";
		this.CmbCompany.AllowNull = (DefaultableBoolean)1;
		this.CmbCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance94).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbCompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCompany).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCompany.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCompany).Location = new System.Drawing.Point(115, 13);
		((System.Windows.Forms.Control)(object)this.CmbCompany).Name = "CmbCompany";
		((System.Windows.Forms.Control)(object)this.CmbCompany).Size = new System.Drawing.Size(238, 26);
		((System.Windows.Forms.Control)(object)this.CmbCompany).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbCompany).Leave += new System.EventHandler(CmbCompanyName_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(14, 47);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(65, 15);
		this.label1.TabIndex = 23;
		this.label1.Text = "User Name";
		this.CmbUserName.AllowNull = (DefaultableBoolean)1;
		this.CmbUserName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUserName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUserName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbUserName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUserName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance114;
		((SpecialBoxBase)((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbUserName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUserName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance120).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbUserName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUserName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUserName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUserName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbUserName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUserName).Location = new System.Drawing.Point(115, 41);
		((System.Windows.Forms.Control)(object)this.CmbUserName).Name = "CmbUserName";
		((System.Windows.Forms.Control)(object)this.CmbUserName).Size = new System.Drawing.Size(238, 26);
		((System.Windows.Forms.Control)(object)this.CmbUserName).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbUserName).Leave += new System.EventHandler(CmbUserName_Leave);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label2);
		this.panel7.Controls.Add(this.toolStrip1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1025, 59);
		this.panel7.TabIndex = 25;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(3, 4);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(227, 19);
		this.label2.TabIndex = 26;
		this.label2.Text = "Screens allocated to users.";
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnRefresh, this.btnSave, this.btnDelete, this.BtnAllocateRightsToCompany });
		this.toolStrip1.Location = new System.Drawing.Point(0, 32);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1025, 27);
		this.toolStrip1.TabIndex = 9;
		this.toolStrip1.Text = "toolStrip1";
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(59, 24);
		this.btnRefresh.Text = "&New";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(84, 24);
		this.btnSave.Text = "&Allocate";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(101, 24);
		this.btnDelete.Text = "&UnAllocate";
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.BtnAllocateRightsToCompany.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnAllocateRightsToCompany.Image = (System.Drawing.Image)resources.GetObject("BtnAllocateRightsToCompany.Image");
		this.BtnAllocateRightsToCompany.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnAllocateRightsToCompany.Name = "BtnAllocateRightsToCompany";
		this.BtnAllocateRightsToCompany.Size = new System.Drawing.Size(195, 24);
		this.BtnAllocateRightsToCompany.Text = "Allocate Rights To Company";
		this.BtnAllocateRightsToCompany.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnAllocateRightsToCompany.Visible = false;
		this.BtnAllocateRightsToCompany.Click += new System.EventHandler(BtnAllocateRightsToCompany_Click);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Size = new System.Drawing.Size(1031, 586);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Check Screen Rights To User";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel10);
		this.panel2.Controls.Add(this.panel9);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1031, 586);
		this.panel2.TabIndex = 0;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.GrdUnAllocatedUsers);
		this.panel10.Controls.Add(this.lblShowUnAllocated);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(600, 172);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(600, 414);
		this.panel10.TabIndex = 14;
		this.GrdUnAllocatedUsers.AllowEdit = (InheritableBoolean)2;
		this.GrdUnAllocatedUsers.ColumnAutoResize = true;
		this.GrdUnAllocatedUsers.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GrdUnAllocatedUsers_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdUnAllocatedUsers.DesignTimeLayout = GrdUnAllocatedUsers_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdUnAllocatedUsers).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdUnAllocatedUsers.DynamicFiltering = true;
		this.GrdUnAllocatedUsers.FilterMode = (FilterMode)2;
		this.GrdUnAllocatedUsers.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdUnAllocatedUsers).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdUnAllocatedUsers.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdUnAllocatedUsers).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.GrdUnAllocatedUsers).Name = "GrdUnAllocatedUsers";
		((ScrollableBase)this.GrdUnAllocatedUsers).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdUnAllocatedUsers).Size = new System.Drawing.Size(600, 388);
		((System.Windows.Forms.Control)(object)this.GrdUnAllocatedUsers).TabIndex = 47;
		this.GrdUnAllocatedUsers.TotalRow = (InheritableBoolean)1;
		this.GrdUnAllocatedUsers.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdUnAllocatedUsers).VisualStyle = (VisualStyle)4;
		this.lblShowUnAllocated.BackColor = System.Drawing.Color.Teal;
		this.lblShowUnAllocated.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblShowUnAllocated.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblShowUnAllocated.ForeColor = System.Drawing.Color.White;
		this.lblShowUnAllocated.Location = new System.Drawing.Point(0, 0);
		this.lblShowUnAllocated.Name = "lblShowUnAllocated";
		this.lblShowUnAllocated.Size = new System.Drawing.Size(600, 26);
		this.lblShowUnAllocated.TabIndex = 0;
		this.lblShowUnAllocated.Text = "UnAllocated";
		this.lblShowUnAllocated.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.GrdAllocatedUsers);
		this.panel9.Controls.Add(this.LblShowAllocated);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel9.Location = new System.Drawing.Point(0, 172);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(600, 414);
		this.panel9.TabIndex = 13;
		this.GrdAllocatedUsers.AllowEdit = (InheritableBoolean)2;
		this.GrdAllocatedUsers.ColumnAutoResize = true;
		this.GrdAllocatedUsers.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GrdAllocatedUsers_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdAllocatedUsers.DesignTimeLayout = GrdAllocatedUsers_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdAllocatedUsers).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdAllocatedUsers.DynamicFiltering = true;
		this.GrdAllocatedUsers.FilterMode = (FilterMode)2;
		this.GrdAllocatedUsers.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdAllocatedUsers).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdAllocatedUsers.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdAllocatedUsers).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.GrdAllocatedUsers).Name = "GrdAllocatedUsers";
		((ScrollableBase)this.GrdAllocatedUsers).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdAllocatedUsers).Size = new System.Drawing.Size(600, 388);
		((System.Windows.Forms.Control)(object)this.GrdAllocatedUsers).TabIndex = 47;
		this.GrdAllocatedUsers.TotalRow = (InheritableBoolean)1;
		this.GrdAllocatedUsers.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdAllocatedUsers).VisualStyle = (VisualStyle)4;
		this.LblShowAllocated.BackColor = System.Drawing.Color.Teal;
		this.LblShowAllocated.Dock = System.Windows.Forms.DockStyle.Top;
		this.LblShowAllocated.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.LblShowAllocated.ForeColor = System.Drawing.Color.White;
		this.LblShowAllocated.Location = new System.Drawing.Point(0, 0);
		this.LblShowAllocated.Name = "LblShowAllocated";
		this.LblShowAllocated.Size = new System.Drawing.Size(600, 26);
		this.LblShowAllocated.TabIndex = 1;
		this.LblShowAllocated.Text = "Allocated";
		this.LblShowAllocated.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel6.Controls.Add(this.groupBox2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 55);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1031, 117);
		this.panel6.TabIndex = 12;
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRightName);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUserNameForTab2);
		this.groupBox2.Controls.Add(this.btnShowUserAgainstScreen);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbScreenName);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1195, 111);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filter";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(12, 79);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(70, 15);
		this.label11.TabIndex = 31;
		this.label11.Text = "Right Name";
		this.CmbRightName.AllowNull = (DefaultableBoolean)1;
		this.CmbRightName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRightName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRightName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRightName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbRightName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRightName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRightName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRightName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance141;
		((SpecialBoxBase)((UltraGridBase)this.CmbRightName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRightName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbRightName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRightName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRightName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRightName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbRightName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRightName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRightName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance146).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRightName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbRightName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRightName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRightName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance149).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRightName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbRightName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRightName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRightName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbRightName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRightName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbRightName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRightName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRightName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRightName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRightName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRightName).Location = new System.Drawing.Point(96, 73);
		((System.Windows.Forms.Control)(object)this.CmbRightName).Name = "CmbRightName";
		((System.Windows.Forms.Control)(object)this.CmbRightName).Size = new System.Drawing.Size(298, 26);
		((System.Windows.Forms.Control)(object)this.CmbRightName).TabIndex = 2;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(12, 23);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(62, 15);
		this.label13.TabIndex = 29;
		this.label13.Text = "UserName";
		this.CmbUserNameForTab2.AllowNull = (DefaultableBoolean)1;
		this.CmbUserNameForTab2.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUserNameForTab2.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUserNameForTab2.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.Appearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance154;
		((SpecialBoxBase)((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance160).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUserNameForTab2).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUserNameForTab2).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbUserNameForTab2.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUserNameForTab2).Location = new System.Drawing.Point(96, 17);
		((System.Windows.Forms.Control)(object)this.CmbUserNameForTab2).Name = "CmbUserNameForTab2";
		((System.Windows.Forms.Control)(object)this.CmbUserNameForTab2).Size = new System.Drawing.Size(298, 26);
		((System.Windows.Forms.Control)(object)this.CmbUserNameForTab2).TabIndex = 0;
		this.btnShowUserAgainstScreen.BackColor = System.Drawing.Color.Teal;
		this.btnShowUserAgainstScreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowUserAgainstScreen.ForeColor = System.Drawing.Color.White;
		this.btnShowUserAgainstScreen.Location = new System.Drawing.Point(400, 72);
		this.btnShowUserAgainstScreen.Name = "btnShowUserAgainstScreen";
		this.btnShowUserAgainstScreen.Size = new System.Drawing.Size(75, 29);
		this.btnShowUserAgainstScreen.TabIndex = 3;
		this.btnShowUserAgainstScreen.Text = "Show";
		this.btnShowUserAgainstScreen.UseVisualStyleBackColor = false;
		this.btnShowUserAgainstScreen.Click += new System.EventHandler(btnShowUserAgainstScreen_Click);
		this.CmbScreenName.AllowNull = (DefaultableBoolean)1;
		this.CmbScreenName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbScreenName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbScreenName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbScreenName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance167;
		((SpecialBoxBase)((UltraGridBase)this.CmbScreenName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance173).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbScreenName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbScreenName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbScreenName).Location = new System.Drawing.Point(96, 45);
		((System.Windows.Forms.Control)(object)this.CmbScreenName).Name = "CmbScreenName";
		((System.Windows.Forms.Control)(object)this.CmbScreenName).Size = new System.Drawing.Size(298, 26);
		((System.Windows.Forms.Control)(object)this.CmbScreenName).TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(12, 51);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(78, 15);
		this.label3.TabIndex = 26;
		this.label3.Text = "Screen Name";
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 27);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1031, 28);
		this.panel4.TabIndex = 11;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNewFortab2 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1031, 27);
		this.toolStrip2.TabIndex = 10;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewFortab2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewFortab2.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewFortab2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewFortab2.Name = "btnNewFortab2";
		this.btnNewFortab2.Size = new System.Drawing.Size(59, 24);
		this.btnNewFortab2.Text = "&New";
		this.btnNewFortab2.Click += new System.EventHandler(btnNewFortab2_Click);
		this.tabPage3.Controls.Add(this.tabControl2);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1031, 586);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "Screen Rights By Module Or User";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.tabControl2.Controls.Add(this.tabPage5);
		this.tabControl2.Controls.Add(this.tabPage6);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1031, 586);
		this.tabControl2.TabIndex = 49;
		this.tabPage5.Controls.Add((System.Windows.Forms.Control)(object)this.grdTab3);
		this.tabPage5.Controls.Add(this.panel27);
		this.tabPage5.Controls.Add(this.panel21);
		this.tabPage5.Controls.Add(this.panel20);
		this.tabPage5.Controls.Add(this.toolStrip3);
		this.tabPage5.Location = new System.Drawing.Point(4, 25);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage5.Size = new System.Drawing.Size(1023, 557);
		this.tabPage5.TabIndex = 0;
		this.tabPage5.Text = "Rights By Module and User";
		this.tabPage5.UseVisualStyleBackColor = true;
		this.grdTab3.AllowEdit = (InheritableBoolean)2;
		this.grdTab3.ColumnAutoResize = true;
		this.grdTab3.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdTab3_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdTab3.DesignTimeLayout = grdTab3_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdTab3).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdTab3.DynamicFiltering = true;
		this.grdTab3.FilterMode = (FilterMode)2;
		this.grdTab3.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdTab3).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdTab3.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdTab3).Location = new System.Drawing.Point(3, 165);
		((System.Windows.Forms.Control)(object)this.grdTab3).Name = "grdTab3";
		((ScrollableBase)this.grdTab3).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdTab3).Size = new System.Drawing.Size(1017, 389);
		((System.Windows.Forms.Control)(object)this.grdTab3).TabIndex = 0;
		this.grdTab3.TotalRow = (InheritableBoolean)1;
		this.grdTab3.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdTab3).VisualStyle = (VisualStyle)4;
		this.grdTab3.CellValueChanged += new ColumnActionEventHandler(grdTab3_CellValueChanged);
		this.grdTab3.CellUpdated += new ColumnActionEventHandler(grdTab3_CellUpdated);
		((System.Windows.Forms.Control)(object)this.grdTab3).Click += new System.EventHandler(grdTab3_Click);
		this.panel27.BackColor = System.Drawing.Color.Teal;
		this.panel27.Controls.Add(this.label25);
		this.panel27.Controls.Add(this.ctrlGrdBar2);
		this.panel27.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel27.Location = new System.Drawing.Point(3, 142);
		this.panel27.Name = "panel27";
		this.panel27.Size = new System.Drawing.Size(1017, 23);
		this.panel27.TabIndex = 49;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label25.ForeColor = System.Drawing.Color.White;
		this.label25.Location = new System.Drawing.Point(3, 3);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(119, 17);
		this.label25.TabIndex = 27;
		this.label25.Text = "Filtered Records";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(973, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdTab3;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(40, 21);
		this.ctrlGrdBar2.TabIndex = 36;
		this.ctrlGrdBar2.Visible = false;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel21.Controls.Add(this.chkGridGroupExpandAll);
		this.panel21.Controls.Add(this.gBForAllCheck);
		this.panel21.Controls.Add(this.chkGridGroupCollapseAll);
		this.panel21.Controls.Add(this.groupBox3);
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.CmbModuleTypeTab3);
		this.panel21.Controls.Add(this.chkSaveLayoutAll);
		this.panel21.Controls.Add(this.label17);
		this.panel21.Controls.Add(this.chkFieldChooserAll);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(3, 58);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1017, 84);
		this.panel21.TabIndex = 14;
		this.chkGridGroupExpandAll.AutoSize = true;
		this.chkGridGroupExpandAll.Location = new System.Drawing.Point(1129, 7);
		this.chkGridGroupExpandAll.Name = "chkGridGroupExpandAll";
		this.chkGridGroupExpandAll.Size = new System.Drawing.Size(146, 20);
		this.chkGridGroupExpandAll.TabIndex = 10;
		this.chkGridGroupExpandAll.Text = "Grid Group Expand";
		this.chkGridGroupExpandAll.UseVisualStyleBackColor = true;
		this.chkGridGroupExpandAll.Visible = false;
		this.chkGridGroupExpandAll.CheckedChanged += new System.EventHandler(chkGridGroupExpandAll_CheckedChanged);
		this.gBForAllCheck.Controls.Add(this.chkGridExportAll);
		this.gBForAllCheck.Controls.Add(this.chkGridPrintAll);
		this.gBForAllCheck.Controls.Add(this.chkCanViewAllRecordAll);
		this.gBForAllCheck.Controls.Add(this.chkPrintAll);
		this.gBForAllCheck.Controls.Add(this.chkDeleteAll);
		this.gBForAllCheck.Controls.Add(this.chkUpdateAll);
		this.gBForAllCheck.Controls.Add(this.chkSaveAll);
		this.gBForAllCheck.Enabled = false;
		this.gBForAllCheck.Location = new System.Drawing.Point(673, 0);
		this.gBForAllCheck.Name = "gBForAllCheck";
		this.gBForAllCheck.Size = new System.Drawing.Size(341, 79);
		this.gBForAllCheck.TabIndex = 1;
		this.gBForAllCheck.TabStop = false;
		this.chkGridExportAll.AutoSize = true;
		this.chkGridExportAll.Location = new System.Drawing.Point(89, 51);
		this.chkGridExportAll.Name = "chkGridExportAll";
		this.chkGridExportAll.Size = new System.Drawing.Size(98, 20);
		this.chkGridExportAll.TabIndex = 5;
		this.chkGridExportAll.Text = "Grid Export";
		this.chkGridExportAll.UseVisualStyleBackColor = true;
		this.chkGridExportAll.CheckedChanged += new System.EventHandler(chkGridExportAll_CheckedChanged);
		this.chkGridPrintAll.AutoSize = true;
		this.chkGridPrintAll.Location = new System.Drawing.Point(89, 31);
		this.chkGridPrintAll.Name = "chkGridPrintAll";
		this.chkGridPrintAll.Size = new System.Drawing.Size(86, 20);
		this.chkGridPrintAll.TabIndex = 4;
		this.chkGridPrintAll.Text = "Grid Print";
		this.chkGridPrintAll.UseVisualStyleBackColor = true;
		this.chkGridPrintAll.CheckedChanged += new System.EventHandler(chkGridPrintAll_CheckedChanged);
		this.chkCanViewAllRecordAll.AutoSize = true;
		this.chkCanViewAllRecordAll.Location = new System.Drawing.Point(200, 12);
		this.chkCanViewAllRecordAll.Name = "chkCanViewAllRecordAll";
		this.chkCanViewAllRecordAll.Size = new System.Drawing.Size(142, 20);
		this.chkCanViewAllRecordAll.TabIndex = 6;
		this.chkCanViewAllRecordAll.Text = "CanViewAllRecord";
		this.chkCanViewAllRecordAll.UseVisualStyleBackColor = true;
		this.chkCanViewAllRecordAll.CheckedChanged += new System.EventHandler(chkCanViewAllRecordAll_CheckedChanged);
		this.chkPrintAll.AutoSize = true;
		this.chkPrintAll.Location = new System.Drawing.Point(89, 12);
		this.chkPrintAll.Name = "chkPrintAll";
		this.chkPrintAll.Size = new System.Drawing.Size(56, 20);
		this.chkPrintAll.TabIndex = 3;
		this.chkPrintAll.Text = "Print";
		this.chkPrintAll.UseVisualStyleBackColor = true;
		this.chkPrintAll.CheckedChanged += new System.EventHandler(chkPrintAll_CheckedChanged);
		this.chkDeleteAll.AutoSize = true;
		this.chkDeleteAll.Location = new System.Drawing.Point(7, 51);
		this.chkDeleteAll.Name = "chkDeleteAll";
		this.chkDeleteAll.Size = new System.Drawing.Size(68, 20);
		this.chkDeleteAll.TabIndex = 2;
		this.chkDeleteAll.Text = "Delete";
		this.chkDeleteAll.UseVisualStyleBackColor = true;
		this.chkDeleteAll.CheckedChanged += new System.EventHandler(chkDeleteAll_CheckedChanged);
		this.chkUpdateAll.AutoSize = true;
		this.chkUpdateAll.Location = new System.Drawing.Point(7, 31);
		this.chkUpdateAll.Name = "chkUpdateAll";
		this.chkUpdateAll.Size = new System.Drawing.Size(73, 20);
		this.chkUpdateAll.TabIndex = 1;
		this.chkUpdateAll.Text = "Update";
		this.chkUpdateAll.UseVisualStyleBackColor = true;
		this.chkUpdateAll.CheckedChanged += new System.EventHandler(chkUpdateAll_CheckedChanged);
		this.chkSaveAll.AutoSize = true;
		this.chkSaveAll.Location = new System.Drawing.Point(7, 12);
		this.chkSaveAll.Name = "chkSaveAll";
		this.chkSaveAll.Size = new System.Drawing.Size(59, 20);
		this.chkSaveAll.TabIndex = 0;
		this.chkSaveAll.Text = "Save";
		this.chkSaveAll.UseVisualStyleBackColor = true;
		this.chkSaveAll.CheckedChanged += new System.EventHandler(chkSaveAll_CheckedChanged);
		this.chkGridGroupCollapseAll.AutoSize = true;
		this.chkGridGroupCollapseAll.Location = new System.Drawing.Point(1129, 26);
		this.chkGridGroupCollapseAll.Name = "chkGridGroupCollapseAll";
		this.chkGridGroupCollapseAll.Size = new System.Drawing.Size(153, 20);
		this.chkGridGroupCollapseAll.TabIndex = 9;
		this.chkGridGroupCollapseAll.Text = "Grid Group Collapse";
		this.chkGridGroupCollapseAll.UseVisualStyleBackColor = true;
		this.chkGridGroupCollapseAll.Visible = false;
		this.chkGridGroupCollapseAll.CheckedChanged += new System.EventHandler(chkGridGroupCollapseAll_CheckedChanged);
		this.groupBox3.Controls.Add(this.chkEnableGroupBox);
		this.groupBox3.Controls.Add(this.btnShowTab3);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUserTab3);
		this.groupBox3.Controls.Add(this.label15);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbModulesTab3);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbScreensTab3);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(0, 0);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(672, 79);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.chkEnableGroupBox.AutoSize = true;
		this.chkEnableGroupBox.Location = new System.Drawing.Point(654, 17);
		this.chkEnableGroupBox.Name = "chkEnableGroupBox";
		this.chkEnableGroupBox.Size = new System.Drawing.Size(15, 14);
		this.chkEnableGroupBox.TabIndex = 4;
		this.chkEnableGroupBox.UseVisualStyleBackColor = true;
		this.chkEnableGroupBox.CheckedChanged += new System.EventHandler(chkEnableGroupBox_CheckedChanged);
		this.btnShowTab3.BackColor = System.Drawing.Color.Teal;
		this.btnShowTab3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowTab3.ForeColor = System.Drawing.Color.White;
		this.btnShowTab3.Location = new System.Drawing.Point(591, 44);
		this.btnShowTab3.Name = "btnShowTab3";
		this.btnShowTab3.Size = new System.Drawing.Size(60, 29);
		this.btnShowTab3.TabIndex = 3;
		this.btnShowTab3.Text = "Show";
		this.btnShowTab3.UseVisualStyleBackColor = false;
		this.btnShowTab3.Click += new System.EventHandler(btnShowTab3_Click);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(3, 51);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(83, 15);
		this.label14.TabIndex = 31;
		this.label14.Text = "Module Name";
		this.CmbUserTab3.AllowNull = (DefaultableBoolean)1;
		this.CmbUserTab3.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUserTab3.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUserTab3.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUserTab3).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbUserTab3).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUserTab3).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUserTab3).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbUserTab3.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUserTab3).Location = new System.Drawing.Point(92, 15);
		((System.Windows.Forms.Control)(object)this.CmbUserTab3).Name = "CmbUserTab3";
		((System.Windows.Forms.Control)(object)this.CmbUserTab3).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.CmbUserTab3).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbUserTab3).Leave += new System.EventHandler(CmbUserTab3_Leave);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(7, 21);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(62, 15);
		this.label15.TabIndex = 29;
		this.label15.Text = "UserName";
		this.CmbModulesTab3.AllowNull = (DefaultableBoolean)1;
		this.CmbModulesTab3.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbModulesTab3.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbModulesTab3.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbModulesTab3).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbModulesTab3).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbModulesTab3).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbModulesTab3).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbModulesTab3.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbModulesTab3).Location = new System.Drawing.Point(92, 45);
		((System.Windows.Forms.Control)(object)this.CmbModulesTab3).Name = "CmbModulesTab3";
		((System.Windows.Forms.Control)(object)this.CmbModulesTab3).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.CmbModulesTab3).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbModulesTab3).Leave += new System.EventHandler(CmbModulesTab3_Leave);
		this.CmbScreensTab3.AllowNull = (DefaultableBoolean)1;
		this.CmbScreensTab3.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbScreensTab3.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbScreensTab3.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbScreensTab3).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbScreensTab3).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbScreensTab3).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbScreensTab3).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbScreensTab3.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbScreensTab3).Location = new System.Drawing.Point(388, 15);
		((System.Windows.Forms.Control)(object)this.CmbScreensTab3).Name = "CmbScreensTab3";
		((System.Windows.Forms.Control)(object)this.CmbScreensTab3).Size = new System.Drawing.Size(263, 26);
		((System.Windows.Forms.Control)(object)this.CmbScreensTab3).TabIndex = 2;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(304, 21);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(78, 15);
		this.label16.TabIndex = 26;
		this.label16.Text = "Screen Name";
		this.CmbModuleTypeTab3.AllowNull = (DefaultableBoolean)1;
		this.CmbModuleTypeTab3.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbModuleTypeTab3.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbModuleTypeTab3.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbModuleTypeTab3).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbModuleTypeTab3).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbModuleTypeTab3.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbModuleTypeTab3).Location = new System.Drawing.Point(1129, 48);
		((System.Windows.Forms.Control)(object)this.CmbModuleTypeTab3).Name = "CmbModuleTypeTab3";
		((System.Windows.Forms.Control)(object)this.CmbModuleTypeTab3).Size = new System.Drawing.Size(117, 26);
		((System.Windows.Forms.Control)(object)this.CmbModuleTypeTab3).TabIndex = 32;
		((System.Windows.Forms.Control)(object)this.CmbModuleTypeTab3).Visible = false;
		this.chkSaveLayoutAll.AutoSize = true;
		this.chkSaveLayoutAll.Location = new System.Drawing.Point(1018, 26);
		this.chkSaveLayoutAll.Name = "chkSaveLayoutAll";
		this.chkSaveLayoutAll.Size = new System.Drawing.Size(109, 20);
		this.chkSaveLayoutAll.TabIndex = 6;
		this.chkSaveLayoutAll.Text = "Save Layout";
		this.chkSaveLayoutAll.UseVisualStyleBackColor = true;
		this.chkSaveLayoutAll.Visible = false;
		this.chkSaveLayoutAll.CheckedChanged += new System.EventHandler(chkSaveLayoutAll_CheckedChanged);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(1018, 54);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(76, 15);
		this.label17.TabIndex = 33;
		this.label17.Text = "Module Type";
		this.label17.Visible = false;
		this.chkFieldChooserAll.AutoSize = true;
		this.chkFieldChooserAll.Location = new System.Drawing.Point(1018, 7);
		this.chkFieldChooserAll.Name = "chkFieldChooserAll";
		this.chkFieldChooserAll.Size = new System.Drawing.Size(109, 20);
		this.chkFieldChooserAll.TabIndex = 5;
		this.chkFieldChooserAll.Text = "FieldChooser";
		this.chkFieldChooserAll.UseVisualStyleBackColor = true;
		this.chkFieldChooserAll.Visible = false;
		this.chkFieldChooserAll.CheckedChanged += new System.EventHandler(chkFieldChooserAll_CheckedChanged);
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.label12);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(3, 30);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1017, 28);
		this.panel20.TabIndex = 12;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(3, 3);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(289, 19);
		this.label12.TabIndex = 27;
		this.label12.Text = "Screen Rights By Module And User";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.toolStripButton1, this.btnRefreshTab3, this.btnSaveRights, this.BtnfrmUserModuleAdmin });
		this.toolStrip3.Location = new System.Drawing.Point(3, 3);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1017, 27);
		this.toolStrip3.TabIndex = 11;
		this.toolStrip3.Text = "toolStrip3";
		this.toolStripButton1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(59, 24);
		this.toolStripButton1.Text = "&New";
		this.btnRefreshTab3.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshTab3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshTab3.Name = "btnRefreshTab3";
		this.btnRefreshTab3.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshTab3.Text = "Refresh";
		this.btnRefreshTab3.Click += new System.EventHandler(btnRefreshTab3_Click);
		this.btnSaveRights.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnSaveRights.Image = (System.Drawing.Image)resources.GetObject("btnSaveRights.Image");
		this.btnSaveRights.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveRights.Name = "btnSaveRights";
		this.btnSaveRights.Size = new System.Drawing.Size(103, 24);
		this.btnSaveRights.Text = "Update Rights";
		this.btnSaveRights.Click += new System.EventHandler(btnSaveRights_Click);
		this.BtnfrmUserModuleAdmin.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnfrmUserModuleAdmin.Image = Architecture.WinApp.Properties.Resources.Export_Reports;
		this.BtnfrmUserModuleAdmin.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnfrmUserModuleAdmin.Name = "BtnfrmUserModuleAdmin";
		this.BtnfrmUserModuleAdmin.Size = new System.Drawing.Size(155, 24);
		this.BtnfrmUserModuleAdmin.Text = "Module Admin User";
		this.BtnfrmUserModuleAdmin.Click += new System.EventHandler(BtnfrmUserModuleAdmin_Click);
		this.tabPage6.Controls.Add((System.Windows.Forms.Control)(object)this.grdUserSpecialRights);
		this.tabPage6.Controls.Add(this.panel24);
		this.tabPage6.Controls.Add(this.panel25);
		this.tabPage6.Location = new System.Drawing.Point(4, 25);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage6.Size = new System.Drawing.Size(1023, 557);
		this.tabPage6.TabIndex = 1;
		this.tabPage6.Text = "Special Rights";
		this.tabPage6.UseVisualStyleBackColor = true;
		this.grdUserSpecialRights.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdUserSpecialRights).BackColor = System.Drawing.Color.White;
		grdUserSpecialRights_DesignTimeLayout.LayoutString = resources.GetString("grdUserSpecialRights_DesignTimeLayout.LayoutString");
		this.grdUserSpecialRights.DesignTimeLayout = grdUserSpecialRights_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdUserSpecialRights).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdUserSpecialRights.DynamicFiltering = true;
		this.grdUserSpecialRights.FilterMode = (FilterMode)2;
		this.grdUserSpecialRights.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdUserSpecialRights).Font = new System.Drawing.Font("Verdana", 10.25f);
		this.grdUserSpecialRights.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdUserSpecialRights).Location = new System.Drawing.Point(3, 115);
		((System.Windows.Forms.Control)(object)this.grdUserSpecialRights).Name = "grdUserSpecialRights";
		((ScrollableBase)this.grdUserSpecialRights).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdUserSpecialRights).Size = new System.Drawing.Size(1017, 439);
		((System.Windows.Forms.Control)(object)this.grdUserSpecialRights).TabIndex = 1;
		this.grdUserSpecialRights.TotalRow = (InheritableBoolean)1;
		this.grdUserSpecialRights.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdUserSpecialRights).VisualStyle = (VisualStyle)4;
		this.grdUserSpecialRights.CellUpdated += new ColumnActionEventHandler(grdUserSpecialRights_CellUpdated);
		this.panel24.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel24.Location = new System.Drawing.Point(3, 91);
		this.panel24.Margin = new System.Windows.Forms.Padding(2);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1017, 24);
		this.panel24.TabIndex = 50;
		this.panel25.Controls.Add(this.panel26);
		this.panel25.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCompanyNameSpecialRights);
		this.panel25.Controls.Add(this.BtnFetchRightsSpecialRights);
		this.panel25.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUserNameSpecialRights);
		this.panel25.Controls.Add(this.label22);
		this.panel25.Controls.Add(this.label23);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Location = new System.Drawing.Point(3, 3);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(1017, 88);
		this.panel25.TabIndex = 0;
		this.panel26.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel26.Controls.Add(this.toolStrip5);
		this.panel26.Controls.Add(this.label24);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel26.Location = new System.Drawing.Point(0, 0);
		this.panel26.Margin = new System.Windows.Forms.Padding(2);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(1017, 52);
		this.panel26.TabIndex = 51;
		this.toolStrip5.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnupdateSpecialRights, this.toolStripSeparator8 });
		this.toolStrip5.Location = new System.Drawing.Point(0, 0);
		this.toolStrip5.Name = "toolStrip5";
		this.toolStrip5.Size = new System.Drawing.Size(1017, 27);
		this.toolStrip5.TabIndex = 20;
		this.toolStrip5.Text = "toolStrip5";
		this.BtnupdateSpecialRights.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnupdateSpecialRights.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.BtnupdateSpecialRights.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnupdateSpecialRights.Name = "BtnupdateSpecialRights";
		this.BtnupdateSpecialRights.Size = new System.Drawing.Size(78, 24);
		this.BtnupdateSpecialRights.Text = "&Update";
		this.BtnupdateSpecialRights.Click += new System.EventHandler(BtnupdateSpecialRights_Click);
		this.toolStripSeparator8.Name = "toolStripSeparator8";
		this.toolStripSeparator8.Size = new System.Drawing.Size(6, 27);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.White;
		this.label24.Location = new System.Drawing.Point(4, 30);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(183, 18);
		this.label24.TabIndex = 10;
		this.label24.Text = "Special Screen's Rights";
		this.CmbCompanyNameSpecialRights.AllowNull = (DefaultableBoolean)1;
		this.CmbCompanyNameSpecialRights.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCompanyNameSpecialRights.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCompanyNameSpecialRights.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.Appearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance128;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance133).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCompanyNameSpecialRights).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCompanyNameSpecialRights).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCompanyNameSpecialRights.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCompanyNameSpecialRights).Location = new System.Drawing.Point(354, 55);
		((System.Windows.Forms.Control)(object)this.CmbCompanyNameSpecialRights).Name = "CmbCompanyNameSpecialRights";
		((System.Windows.Forms.Control)(object)this.CmbCompanyNameSpecialRights).Size = new System.Drawing.Size(279, 26);
		((System.Windows.Forms.Control)(object)this.CmbCompanyNameSpecialRights).TabIndex = 1;
		this.BtnFetchRightsSpecialRights.BackColor = System.Drawing.Color.Teal;
		this.BtnFetchRightsSpecialRights.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnFetchRightsSpecialRights.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnFetchRightsSpecialRights.ForeColor = System.Drawing.Color.White;
		this.BtnFetchRightsSpecialRights.Location = new System.Drawing.Point(639, 55);
		this.BtnFetchRightsSpecialRights.Name = "BtnFetchRightsSpecialRights";
		this.BtnFetchRightsSpecialRights.Size = new System.Drawing.Size(61, 27);
		this.BtnFetchRightsSpecialRights.TabIndex = 2;
		this.BtnFetchRightsSpecialRights.Text = "Fetch Rights";
		this.BtnFetchRightsSpecialRights.UseVisualStyleBackColor = false;
		this.BtnFetchRightsSpecialRights.Click += new System.EventHandler(BtnFetchSpecialRights_Click);
		this.CmbUserNameSpecialRights.AllowNull = (DefaultableBoolean)1;
		this.CmbUserNameSpecialRights.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUserNameSpecialRights.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUserNameSpecialRights.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.Appearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance101;
		((SpecialBoxBase)((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance107).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUserNameSpecialRights).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUserNameSpecialRights).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbUserNameSpecialRights.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUserNameSpecialRights).Location = new System.Drawing.Point(75, 55);
		((System.Windows.Forms.Control)(object)this.CmbUserNameSpecialRights).Name = "CmbUserNameSpecialRights";
		((System.Windows.Forms.Control)(object)this.CmbUserNameSpecialRights).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.CmbUserNameSpecialRights).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbUserNameSpecialRights).Leave += new System.EventHandler(CmbUserNameSpecialRights_Leave);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(298, 61);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(58, 15);
		this.label22.TabIndex = 12;
		this.label22.Text = "Company";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(4, 61);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(65, 15);
		this.label23.TabIndex = 10;
		this.label23.Text = "User Name";
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdScreenRightsReport);
		this.tabPage4.Controls.Add(this.panel23);
		this.tabPage4.Controls.Add(this.panel22);
		this.tabPage4.Controls.Add(this.toolStrip4);
		this.tabPage4.Location = new System.Drawing.Point(4, 4);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Size = new System.Drawing.Size(1031, 586);
		this.tabPage4.TabIndex = 3;
		this.tabPage4.Text = "Rights Report";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.grdScreenRightsReport.AllowEdit = (InheritableBoolean)2;
		this.grdScreenRightsReport.ColumnAutoResize = true;
		this.grdScreenRightsReport.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdScreenRightsReport_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdScreenRightsReport.DesignTimeLayout = grdScreenRightsReport_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdScreenRightsReport).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdScreenRightsReport.DynamicFiltering = true;
		this.grdScreenRightsReport.FilterMode = (FilterMode)2;
		this.grdScreenRightsReport.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdScreenRightsReport).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdScreenRightsReport.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdScreenRightsReport).Location = new System.Drawing.Point(0, 122);
		((System.Windows.Forms.Control)(object)this.grdScreenRightsReport).Name = "grdScreenRightsReport";
		((ScrollableBase)this.grdScreenRightsReport).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdScreenRightsReport).Size = new System.Drawing.Size(1031, 464);
		((System.Windows.Forms.Control)(object)this.grdScreenRightsReport).TabIndex = 49;
		this.grdScreenRightsReport.TotalRow = (InheritableBoolean)1;
		this.grdScreenRightsReport.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdScreenRightsReport).VisualStyle = (VisualStyle)4;
		this.panel23.Controls.Add(this.groupBox4);
		this.panel23.Controls.Add(this.groupBox5);
		this.panel23.Controls.Add(this.chkFilterBySaveLayout);
		this.panel23.Controls.Add(this.chkFilterByFieldChooser);
		this.panel23.Controls.Add(this.chkFilterByGridCollapse);
		this.panel23.Controls.Add(this.chkFilterByGroupExpand);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(0, 55);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1031, 67);
		this.panel23.TabIndex = 15;
		this.groupBox4.Controls.Add(this.btnFilter);
		this.groupBox4.Controls.Add(this.rdNotHaveRight);
		this.groupBox4.Controls.Add(this.rdHaveRight);
		this.groupBox4.Controls.Add(this.chkFilterByDelete);
		this.groupBox4.Controls.Add(this.chkFilterByView);
		this.groupBox4.Controls.Add(this.chkFilterByGridExport);
		this.groupBox4.Controls.Add(this.chkFilterByGridPrint);
		this.groupBox4.Controls.Add(this.chkFilterByCanViewAllRecord);
		this.groupBox4.Controls.Add(this.chkFilterByPrint);
		this.groupBox4.Controls.Add(this.chkFilterByUpdate);
		this.groupBox4.Controls.Add(this.chkFilterBySave);
		this.groupBox4.Enabled = false;
		this.groupBox4.Location = new System.Drawing.Point(795, 1);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(566, 68);
		this.groupBox4.TabIndex = 34;
		this.groupBox4.TabStop = false;
		this.btnFilter.Image = Architecture.WinApp.Properties.Resources.filter_line;
		this.btnFilter.Location = new System.Drawing.Point(379, 13);
		this.btnFilter.Name = "btnFilter";
		this.btnFilter.Size = new System.Drawing.Size(36, 36);
		this.btnFilter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
		this.btnFilter.TabIndex = 14;
		this.btnFilter.TabStop = false;
		this.btnFilter.Click += new System.EventHandler(btnFilter_Click);
		this.rdNotHaveRight.AutoSize = true;
		this.rdNotHaveRight.Location = new System.Drawing.Point(184, 5);
		this.rdNotHaveRight.Name = "rdNotHaveRight";
		this.rdNotHaveRight.Size = new System.Drawing.Size(124, 20);
		this.rdNotHaveRight.TabIndex = 13;
		this.rdNotHaveRight.Text = "Not Have Right";
		this.rdNotHaveRight.UseVisualStyleBackColor = true;
		this.rdHaveRight.AutoSize = true;
		this.rdHaveRight.Checked = true;
		this.rdHaveRight.Location = new System.Drawing.Point(88, 5);
		this.rdHaveRight.Name = "rdHaveRight";
		this.rdHaveRight.Size = new System.Drawing.Size(96, 20);
		this.rdHaveRight.TabIndex = 12;
		this.rdHaveRight.TabStop = true;
		this.rdHaveRight.Text = "Have Right";
		this.rdHaveRight.UseVisualStyleBackColor = true;
		this.chkFilterByDelete.AutoSize = true;
		this.chkFilterByDelete.Location = new System.Drawing.Point(123, 43);
		this.chkFilterByDelete.Name = "chkFilterByDelete";
		this.chkFilterByDelete.Size = new System.Drawing.Size(68, 20);
		this.chkFilterByDelete.TabIndex = 11;
		this.chkFilterByDelete.Text = "Delete";
		this.chkFilterByDelete.UseVisualStyleBackColor = true;
		this.chkFilterByDelete.CheckedChanged += new System.EventHandler(chkFilterByDelete_CheckedChanged);
		this.chkFilterByView.AutoSize = true;
		this.chkFilterByView.Location = new System.Drawing.Point(5, 25);
		this.chkFilterByView.Name = "chkFilterByView";
		this.chkFilterByView.Size = new System.Drawing.Size(57, 20);
		this.chkFilterByView.TabIndex = 2;
		this.chkFilterByView.Text = "View";
		this.chkFilterByView.UseVisualStyleBackColor = true;
		this.chkFilterByView.CheckedChanged += new System.EventHandler(chkFilterByView_CheckedChanged);
		this.chkFilterByGridExport.AutoSize = true;
		this.chkFilterByGridExport.Location = new System.Drawing.Point(295, 43);
		this.chkFilterByGridExport.Name = "chkFilterByGridExport";
		this.chkFilterByGridExport.Size = new System.Drawing.Size(98, 20);
		this.chkFilterByGridExport.TabIndex = 8;
		this.chkFilterByGridExport.Text = "Grid Export";
		this.chkFilterByGridExport.UseVisualStyleBackColor = true;
		this.chkFilterByGridExport.CheckedChanged += new System.EventHandler(chkFilterByGridExport_CheckedChanged);
		this.chkFilterByGridPrint.AutoSize = true;
		this.chkFilterByGridPrint.Location = new System.Drawing.Point(199, 43);
		this.chkFilterByGridPrint.Name = "chkFilterByGridPrint";
		this.chkFilterByGridPrint.Size = new System.Drawing.Size(86, 20);
		this.chkFilterByGridPrint.TabIndex = 7;
		this.chkFilterByGridPrint.Text = "Grid Print";
		this.chkFilterByGridPrint.UseVisualStyleBackColor = true;
		this.chkFilterByGridPrint.CheckedChanged += new System.EventHandler(chkFilterByGridPrint_CheckedChanged);
		this.chkFilterByCanViewAllRecord.AutoSize = true;
		this.chkFilterByCanViewAllRecord.Location = new System.Drawing.Point(6, 43);
		this.chkFilterByCanViewAllRecord.Name = "chkFilterByCanViewAllRecord";
		this.chkFilterByCanViewAllRecord.Size = new System.Drawing.Size(142, 20);
		this.chkFilterByCanViewAllRecord.TabIndex = 4;
		this.chkFilterByCanViewAllRecord.Text = "CanViewAllRecord";
		this.chkFilterByCanViewAllRecord.UseVisualStyleBackColor = true;
		this.chkFilterByCanViewAllRecord.CheckedChanged += new System.EventHandler(chkFilterByCanViewAllRecord_CheckedChanged);
		this.chkFilterByPrint.AutoSize = true;
		this.chkFilterByPrint.Location = new System.Drawing.Point(295, 25);
		this.chkFilterByPrint.Name = "chkFilterByPrint";
		this.chkFilterByPrint.Size = new System.Drawing.Size(56, 20);
		this.chkFilterByPrint.TabIndex = 3;
		this.chkFilterByPrint.Text = "Print";
		this.chkFilterByPrint.UseVisualStyleBackColor = true;
		this.chkFilterByPrint.CheckedChanged += new System.EventHandler(chkFilterByPrint_CheckedChanged);
		this.chkFilterByUpdate.AutoSize = true;
		this.chkFilterByUpdate.Location = new System.Drawing.Point(199, 25);
		this.chkFilterByUpdate.Name = "chkFilterByUpdate";
		this.chkFilterByUpdate.Size = new System.Drawing.Size(73, 20);
		this.chkFilterByUpdate.TabIndex = 1;
		this.chkFilterByUpdate.Text = "Update";
		this.chkFilterByUpdate.UseVisualStyleBackColor = true;
		this.chkFilterByUpdate.CheckedChanged += new System.EventHandler(chkFilterByUpdate_CheckedChanged);
		this.chkFilterBySave.AutoSize = true;
		this.chkFilterBySave.Location = new System.Drawing.Point(123, 25);
		this.chkFilterBySave.Name = "chkFilterBySave";
		this.chkFilterBySave.Size = new System.Drawing.Size(59, 20);
		this.chkFilterBySave.TabIndex = 0;
		this.chkFilterBySave.Text = "Save";
		this.chkFilterBySave.UseVisualStyleBackColor = true;
		this.chkFilterBySave.CheckedChanged += new System.EventHandler(chkFilterBySave_CheckedChanged);
		this.groupBox5.Controls.Add(this.btnShowTab4);
		this.groupBox5.Controls.Add(this.label19);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUserTab4);
		this.groupBox5.Controls.Add(this.label20);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbModulesTab4);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbScreensTab4);
		this.groupBox5.Controls.Add(this.label21);
		this.groupBox5.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(0, 0);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(792, 67);
		this.groupBox5.TabIndex = 13;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Filters";
		this.btnShowTab4.BackColor = System.Drawing.Color.Teal;
		this.btnShowTab4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowTab4.ForeColor = System.Drawing.Color.White;
		this.btnShowTab4.Location = new System.Drawing.Point(731, 32);
		this.btnShowTab4.Name = "btnShowTab4";
		this.btnShowTab4.Size = new System.Drawing.Size(60, 29);
		this.btnShowTab4.TabIndex = 3;
		this.btnShowTab4.Text = "Show";
		this.btnShowTab4.UseVisualStyleBackColor = false;
		this.btnShowTab4.Click += new System.EventHandler(btnShowTab4_Click);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(196, 16);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(83, 15);
		this.label19.TabIndex = 31;
		this.label19.Text = "Module Name";
		this.CmbUserTab4.AllowNull = (DefaultableBoolean)1;
		this.CmbUserTab4.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUserTab4.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUserTab4.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUserTab4).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbUserTab4).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUserTab4).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUserTab4).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbUserTab4.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUserTab4).Location = new System.Drawing.Point(4, 33);
		((System.Windows.Forms.Control)(object)this.CmbUserTab4).Name = "CmbUserTab4";
		((System.Windows.Forms.Control)(object)this.CmbUserTab4).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.CmbUserTab4).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbUserTab4).Leave += new System.EventHandler(CmbUserTab4_Leave);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(4, 14);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(62, 15);
		this.label20.TabIndex = 29;
		this.label20.Text = "UserName";
		this.CmbModulesTab4.AllowNull = (DefaultableBoolean)1;
		this.CmbModulesTab4.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbModulesTab4.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbModulesTab4.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.Appearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbModulesTab4).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance180;
		((SpecialBoxBase)((UltraGridBase)this.CmbModulesTab4).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbModulesTab4).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbModulesTab4).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbModulesTab4.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbModulesTab4).Location = new System.Drawing.Point(196, 33);
		((System.Windows.Forms.Control)(object)this.CmbModulesTab4).Name = "CmbModulesTab4";
		((System.Windows.Forms.Control)(object)this.CmbModulesTab4).Size = new System.Drawing.Size(269, 26);
		((System.Windows.Forms.Control)(object)this.CmbModulesTab4).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbModulesTab4).Leave += new System.EventHandler(CmbModulesTab4_Leave);
		this.CmbScreensTab4.AllowNull = (DefaultableBoolean)1;
		this.CmbScreensTab4.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbScreensTab4.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbScreensTab4.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbScreensTab4).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance104;
		((SpecialBoxBase)((UltraGridBase)this.CmbScreensTab4).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance159).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbScreensTab4).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbScreensTab4).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbScreensTab4.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbScreensTab4).Location = new System.Drawing.Point(466, 33);
		((System.Windows.Forms.Control)(object)this.CmbScreensTab4).Name = "CmbScreensTab4";
		((System.Windows.Forms.Control)(object)this.CmbScreensTab4).Size = new System.Drawing.Size(263, 26);
		((System.Windows.Forms.Control)(object)this.CmbScreensTab4).TabIndex = 2;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(463, 16);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(78, 15);
		this.label21.TabIndex = 26;
		this.label21.Text = "Screen Name";
		this.chkFilterBySaveLayout.AutoSize = true;
		this.chkFilterBySaveLayout.Location = new System.Drawing.Point(888, 116);
		this.chkFilterBySaveLayout.Name = "chkFilterBySaveLayout";
		this.chkFilterBySaveLayout.Size = new System.Drawing.Size(104, 20);
		this.chkFilterBySaveLayout.TabIndex = 6;
		this.chkFilterBySaveLayout.Text = "SaveLayout";
		this.chkFilterBySaveLayout.UseVisualStyleBackColor = true;
		this.chkFilterBySaveLayout.Visible = false;
		this.chkFilterBySaveLayout.CheckedChanged += new System.EventHandler(chkFilterBySaveLayout_CheckedChanged);
		this.chkFilterByFieldChooser.AutoSize = true;
		this.chkFilterByFieldChooser.Location = new System.Drawing.Point(800, 116);
		this.chkFilterByFieldChooser.Name = "chkFilterByFieldChooser";
		this.chkFilterByFieldChooser.Size = new System.Drawing.Size(109, 20);
		this.chkFilterByFieldChooser.TabIndex = 5;
		this.chkFilterByFieldChooser.Text = "FieldChooser";
		this.chkFilterByFieldChooser.UseVisualStyleBackColor = true;
		this.chkFilterByFieldChooser.Visible = false;
		this.chkFilterByFieldChooser.CheckedChanged += new System.EventHandler(chkFilterByFieldChooser_CheckedChanged);
		this.chkFilterByGridCollapse.AutoSize = true;
		this.chkFilterByGridCollapse.Location = new System.Drawing.Point(974, 116);
		this.chkFilterByGridCollapse.Name = "chkFilterByGridCollapse";
		this.chkFilterByGridCollapse.Size = new System.Drawing.Size(118, 20);
		this.chkFilterByGridCollapse.TabIndex = 9;
		this.chkFilterByGridCollapse.Text = "GroupCollapse";
		this.chkFilterByGridCollapse.UseVisualStyleBackColor = true;
		this.chkFilterByGridCollapse.Visible = false;
		this.chkFilterByGridCollapse.CheckedChanged += new System.EventHandler(chkFilterByGridCollapse_CheckedChanged);
		this.chkFilterByGroupExpand.AutoSize = true;
		this.chkFilterByGroupExpand.Location = new System.Drawing.Point(1075, 116);
		this.chkFilterByGroupExpand.Name = "chkFilterByGroupExpand";
		this.chkFilterByGroupExpand.Size = new System.Drawing.Size(111, 20);
		this.chkFilterByGroupExpand.TabIndex = 10;
		this.chkFilterByGroupExpand.Text = "GroupExpand";
		this.chkFilterByGroupExpand.UseVisualStyleBackColor = true;
		this.chkFilterByGroupExpand.Visible = false;
		this.chkFilterByGroupExpand.CheckedChanged += new System.EventHandler(chkFilterByGroupExpand_CheckedChanged);
		this.panel22.BackColor = System.Drawing.Color.Teal;
		this.panel22.Controls.Add(this.label18);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 27);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1031, 28);
		this.panel22.TabIndex = 13;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(3, 3);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(182, 19);
		this.label18.TabIndex = 27;
		this.label18.Text = "Screen Rights Report";
		this.toolStrip4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnResetTab4, this.btnRefreshTab4, this.btnPrint });
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1031, 27);
		this.toolStrip4.TabIndex = 12;
		this.toolStrip4.Text = "toolStrip4";
		this.btnResetTab4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetTab4.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetTab4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetTab4.Name = "btnResetTab4";
		this.btnResetTab4.Size = new System.Drawing.Size(59, 24);
		this.btnResetTab4.Text = "&New";
		this.btnResetTab4.Click += new System.EventHandler(btnResetTab4_Click);
		this.btnRefreshTab4.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshTab4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshTab4.Name = "btnRefreshTab4";
		this.btnRefreshTab4.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshTab4.Text = "Refresh";
		this.btnRefreshTab4.Click += new System.EventHandler(btnRefreshTab4_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "005-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(70, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(73, 27);
		this.miniToolStrip.TabIndex = 20;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(14, 74);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(73, 15);
		this.label26.TabIndex = 25;
		this.label26.Text = "Applications";
		this.CmbApplications.AllowNull = (DefaultableBoolean)1;
		this.CmbApplications.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbApplications.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbApplications.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbApplications).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbApplications).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbApplications).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApplications).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbApplications).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApplications).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbApplications).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbApplications).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbApplications).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbApplications).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbApplications).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbApplications).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbApplications).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbApplications.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbApplications).Location = new System.Drawing.Point(115, 68);
		((System.Windows.Forms.Control)(object)this.CmbApplications).Name = "CmbApplications";
		((System.Windows.Forms.Control)(object)this.CmbApplications).Size = new System.Drawing.Size(238, 26);
		((System.Windows.Forms.Control)(object)this.CmbApplications).TabIndex = 2;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1039, 615);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "UserRightsByCompany";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(UserRightsByCompany_FormClosing);
		base.Load += new System.EventHandler(UserRightsByCompany_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(UserRightsByCompany_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		this.panel18.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAllocatedScreens).EndInit();
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdUnAllocatedScreen).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdModules).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCompany).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUserName).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdUnAllocatedUsers).EndInit();
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdAllocatedUsers).EndInit();
		this.panel6.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRightName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUserNameForTab2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbScreenName).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage5.ResumeLayout(false);
		this.tabPage5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdTab3).EndInit();
		this.panel27.ResumeLayout(false);
		this.panel27.PerformLayout();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		this.gBForAllCheck.ResumeLayout(false);
		this.gBForAllCheck.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbUserTab3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbModulesTab3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbScreensTab3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbModuleTypeTab3).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabPage6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdUserSpecialRights).EndInit();
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		this.panel26.ResumeLayout(false);
		this.panel26.PerformLayout();
		this.toolStrip5.ResumeLayout(false);
		this.toolStrip5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCompanyNameSpecialRights).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUserNameSpecialRights).EndInit();
		this.tabPage4.ResumeLayout(false);
		this.tabPage4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdScreenRightsReport).EndInit();
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnFilter).EndInit();
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbUserTab4).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbModulesTab4).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbScreensTab4).EndInit();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbApplications).EndInit();
		base.ResumeLayout(false);
	}
}
