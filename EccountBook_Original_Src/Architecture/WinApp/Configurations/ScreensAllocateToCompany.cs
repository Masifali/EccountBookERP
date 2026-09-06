using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Templates;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Configurations;

public class ScreensAllocateToCompany : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private Panel panel1;

	private Panel panel5;

	private TabControl tabControl1;

	private TabPage tabPage1;

	internal GridEX grdAllocatedScreens;

	internal GridEX grdUnAllocatedScreens;

	internal GridEX grdModules;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnSave;

	private ToolStripButton btnDelete;

	internal UltraCombo CmbCompany;

	private Label label5;

	private ToolStrip miniToolStrip;

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

	private ToolStripButton BtnScreenDefinition;

	private ToolStripButton btnModuleAllocateToTemplete;

	public ScreensAllocateToCompany(UserAccount user)
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
			base.AutoScrollMargin = new Size(5, 5);
			base.AutoScrollMinSize = new Size(980, 500);
			CompanyFill();
			BtnScreenDefinition.Visible = true;
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

	private void CmbCompanyName_Leave(object sender, EventArgs e)
	{
		try
		{
			ModulesNameFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ModulesNameFill()
	{
		try
		{
			DataTable dt = tblUserRights.GetModulesAgainstCompanyId(Conversion.ToInt(CmbCompany.Value), 0);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtGrid = new DataTable();
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("ModuleDescription", typeof(string));
			dtGrid.Columns.Add("Allocated", typeof(int));
			dtGrid.Columns.Add("UnAllocated", typeof(int));
			foreach (DataRow item in dt.Rows)
			{
				dtGrid.Rows.Add(item["ModuleId"], item["ModuleDescription"], item["Allocated"], item["UnAllocated"]);
			}
			grdModules.DataSource = dtGrid;
			grdModules.RetrieveStructure();
			grdModules.RootTable.Columns["Id"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdModules, 3, 2);
			grdModules.RootTable.Columns["ModuleDescription"].Width = 150;
			grdModules.RootTable.Columns["Allocated"].Width = 65;
			grdModules.RootTable.Columns["UnAllocated"].Width = 65;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdModules_Click(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdModules.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int ModuleId = Conversion.ToInt(r.Cells["Id"].Value);
				int CompanyId = Conversion.ToInt(CmbCompany.Value);
				GetUnAllocatedScreens(CompanyId, ModuleId);
				GetAllocatedScreens(CompanyId, ModuleId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAllocatedScreens(int CompanyId, int ModuleId = 0)
	{
		try
		{
			DataTable dt = CompanyRights.CompanyRights_AllocatedData(CompanyId, ModuleId);
			if (dt.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("CompanyId", typeof(int));
				dtcol.Columns.Add("CompanyName", typeof(string));
				dtcol.Columns.Add("ScreenId", typeof(int));
				dtcol.Columns.Add("ScreenName", typeof(string));
				dtcol.Columns.Add("ScreenAlias", typeof(string));
				dtcol.Columns.Add("Active", typeof(bool));
				dtcol.Rows.Clear();
				foreach (DataRow row in dt.Rows)
				{
					dtcol.Rows.Add(row["Id"], row["CompanyId"], row["CompanyName"], row["ScreenId"], row["ScreenName"], row["ScreenAlias"], row["IsActive"]);
				}
				grdAllocatedScreens.DataSource = dtcol;
				grdAllocatedScreens.RetrieveStructure();
				grdSettingsCompanyScreen();
			}
			else
			{
				grdAllocatedScreens.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public void grdSettingsCompanyScreen()
	{
		try
		{
			grdAllocatedScreens.RootTable.Columns["Id"].Visible = false;
			grdAllocatedScreens.RootTable.Columns["ScreenId"].Visible = false;
			grdAllocatedScreens.RootTable.Columns["CompanyId"].Visible = false;
			grdAllocatedScreens.RootTable.Columns["CompanyName"].Visible = false;
			grdAllocatedScreens.RootTable.Columns["ScreenName"].Visible = false;
			grdAllocatedScreens.RootTable.Columns["ScreenAlias"].Width = 295;
			grdAllocatedScreens.RootTable.Columns["Active"].Visible = false;
			grdAllocatedScreens.RootTable.Columns["ScreenAlias"].Caption = "Screen Name";
			grdAllocatedScreens.RootTable.Columns["ScreenName"].EditType = (EditType)0;
			grdAllocatedScreens.RootTable.Columns["ScreenAlias"].EditType = (EditType)0;
			grdAllocatedScreens.RootTable.Columns.Add("Select");
			grdAllocatedScreens.RootTable.Columns["Select"].ActAsSelector = true;
			grdAllocatedScreens.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdAllocatedScreens.RootTable.Columns["Select"].Width = 20;
			grdAllocatedScreens.RootTable.Columns["Select"].Position = 0;
			grdAllocatedScreens.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void GetUnAllocatedScreens(int CompanyId, int ModuleId = 0)
	{
		try
		{
			DataTable dt = CompanyRights.CompanyRights_UnAllocatedData(CompanyId, ModuleId);
			if (dt.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("CompanyId", typeof(int));
				dtcol.Columns.Add("CompanyName", typeof(string));
				dtcol.Columns.Add("ScreenId", typeof(int));
				dtcol.Columns.Add("ScreenName", typeof(string));
				dtcol.Columns.Add("ScreenAlias", typeof(string));
				dtcol.Columns.Add("Active", typeof(bool));
				dtcol.Rows.Clear();
				foreach (DataRow row in dt.Rows)
				{
					dtcol.Rows.Add(row["Id"], row["CompanyId"], row["CompanyName"], row["ScreenId"], row["ScreenName"], row["ScreenAlias"], row["IsActive"]);
				}
				grdUnAllocatedScreens.DataSource = dtcol;
				grdUnAllocatedScreens.RetrieveStructure();
				grdSettingsScreenName();
			}
			else
			{
				grdUnAllocatedScreens.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public void grdSettingsScreenName()
	{
		try
		{
			grdUnAllocatedScreens.RootTable.Columns["Id"].Visible = false;
			grdUnAllocatedScreens.RootTable.Columns["ScreenId"].Visible = false;
			grdUnAllocatedScreens.RootTable.Columns["CompanyId"].Visible = false;
			grdUnAllocatedScreens.RootTable.Columns["CompanyName"].Visible = false;
			grdUnAllocatedScreens.RootTable.Columns["ScreenName"].Visible = false;
			grdUnAllocatedScreens.RootTable.Columns["ScreenAlias"].Width = 290;
			grdUnAllocatedScreens.RootTable.Columns["Active"].Visible = false;
			grdUnAllocatedScreens.RootTable.Columns["ScreenAlias"].Caption = "Screen Name";
			grdUnAllocatedScreens.RootTable.Columns["ScreenName"].EditType = (EditType)0;
			grdUnAllocatedScreens.RootTable.Columns["ScreenAlias"].EditType = (EditType)0;
			grdUnAllocatedScreens.RootTable.Columns.Add("Select");
			grdUnAllocatedScreens.RootTable.Columns["Select"].ActAsSelector = true;
			grdUnAllocatedScreens.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdUnAllocatedScreens.RootTable.Columns["Select"].Width = 20;
			grdUnAllocatedScreens.RootTable.Columns["Select"].Position = 0;
			grdUnAllocatedScreens.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)CmbCompany).ActiveRow == null)
			{
				MessageBox.Show("Company field Required");
			}
			else if (grdUnAllocatedScreens.GetCheckedRows().Count() > 0)
			{
				CompanyRights companyRights = new CompanyRights();
				companyRights.CompanyRightslst = new List<CompanyRights>();
				string secreenId = "";
				GridEXRow[] checkedRows = grdUnAllocatedScreens.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					CompanyRights rights = new CompanyRights();
					rights.CompanyId = Convert.ToInt32(CmbCompany.Value);
					rights.Id = Convert.ToInt32(r.Cells["Id"].Value);
					rights.ScreenId = Convert.ToInt32(r.Cells["ScreenId"].Value);
					rights.IsActive = true;
					secreenId = Convert.ToInt32(r.Cells["Id"].Value) + "," + secreenId;
					companyRights.CompanyRightslst.Add(rights);
				}
				int num = CompanyRights.Save(companyRights);
				Reset();
				if (num > 0)
				{
					MessageBox.Show("Record Save Successfully");
				}
				grdModules_Click(null, null);
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
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)CmbCompany).ActiveRow == null)
			{
				throw new Exception("Please select Company First");
			}
			if (grdAllocatedScreens.GetCheckedRows().Count() > 0)
			{
				if (MessageBox.Show("Are you sure you want to deactivate? If you deactivate, the selected screens will also be deactivated for all users in this company.", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					CompanyRights companyRights = new CompanyRights();
					companyRights.CompanyRightslst = new List<CompanyRights>();
					GridEXRow[] checkedRows = grdAllocatedScreens.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						CompanyRights rights = new CompanyRights();
						rights.CompanyId = Convert.ToInt32(CmbCompany.Value);
						rights.Id = Convert.ToInt32(r.Cells["Id"].Value);
						rights.ScreenId = Convert.ToInt32(r.Cells["ScreenId"].Value);
						rights.IsActive = false;
						companyRights.CompanyRightslst.Add(rights);
					}
					CompanyRights.Save(companyRights);
					MessageBox.Show("Record Updated Successfully");
					Reset();
					grdModules_Click(null, null);
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

	private void Reset()
	{
		try
		{
			CompanyFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		Reset();
		((Control)(object)CmbCompany).Text = string.Empty;
	}

	private void ScreensAllocateToCompany_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.F10)
			{
				BtnScreenDefinition.Visible = true;
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.F12)
			{
				BtnScreenDefinition.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnScreenDefinition_Click(object sender, EventArgs e)
	{
		try
		{
			new frmScreenDefinetion(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnModuleAllocateToTemplete_Click(object sender, EventArgs e)
	{
		try
		{
			new frmModuleAllocateToOrganizationTemplates(UserAccount).Show();
		}
		catch (Exception)
		{
			throw;
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
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		GridEXLayout grdAllocatedScreens_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdUnAllocatedScreens_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdModules_DesignTimeLayout = new GridEXLayout();
		Appearance appearance13 = new Appearance();
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
		Appearance appearance24 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Configurations.ScreensAllocateToCompany));
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
		this.grdUnAllocatedScreens = new GridEX();
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
		this.label5 = new System.Windows.Forms.Label();
		this.CmbCompany = new UltraCombo();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.BtnScreenDefinition = new System.Windows.Forms.ToolStripButton();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.btnModuleAllocateToTemplete = new System.Windows.Forms.ToolStripButton();
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
		((System.ComponentModel.ISupportInitialize)this.grdUnAllocatedScreens).BeginInit();
		this.panel16.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdModules).BeginInit();
		this.panel12.SuspendLayout();
		this.panel8.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCompany).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(971, 661);
		this.panel1.TabIndex = 0;
		this.panel5.Controls.Add(this.tabControl1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(971, 661);
		this.panel5.TabIndex = 3;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(971, 661);
		this.tabControl1.TabIndex = 51;
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(963, 635);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "AllocateToCompany";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.panel17);
		this.panel3.Controls.Add(this.panel14);
		this.panel3.Controls.Add(this.panel11);
		this.panel3.Controls.Add(this.panel8);
		this.panel3.Controls.Add(this.panel7);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(957, 629);
		this.panel3.TabIndex = 48;
		this.panel17.Controls.Add(this.panel18);
		this.panel17.Controls.Add(this.panel19);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel17.Location = new System.Drawing.Point(616, 149);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(338, 480);
		this.panel17.TabIndex = 29;
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.grdAllocatedScreens);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel18.Location = new System.Drawing.Point(0, 46);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(338, 434);
		this.panel18.TabIndex = 1;
		this.grdAllocatedScreens.AllowEdit = (InheritableBoolean)2;
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
		((System.Windows.Forms.Control)(object)this.grdAllocatedScreens).Size = new System.Drawing.Size(338, 434);
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
		this.panel14.Location = new System.Drawing.Point(284, 149);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(332, 480);
		this.panel14.TabIndex = 28;
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.grdUnAllocatedScreens);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(0, 46);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(332, 434);
		this.panel15.TabIndex = 1;
		this.grdUnAllocatedScreens.AllowEdit = (InheritableBoolean)2;
		this.grdUnAllocatedScreens.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdUnAllocatedScreens_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdUnAllocatedScreens.DesignTimeLayout = grdUnAllocatedScreens_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdUnAllocatedScreens).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdUnAllocatedScreens.DynamicFiltering = true;
		this.grdUnAllocatedScreens.FilterMode = (FilterMode)2;
		this.grdUnAllocatedScreens.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdUnAllocatedScreens).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdUnAllocatedScreens.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdUnAllocatedScreens).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdUnAllocatedScreens).Name = "grdUnAllocatedScreens";
		((ScrollableBase)this.grdUnAllocatedScreens).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdUnAllocatedScreens).Size = new System.Drawing.Size(332, 434);
		((System.Windows.Forms.Control)(object)this.grdUnAllocatedScreens).TabIndex = 46;
		this.grdUnAllocatedScreens.TotalRow = (InheritableBoolean)1;
		this.grdUnAllocatedScreens.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdUnAllocatedScreens).VisualStyle = (VisualStyle)4;
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label9);
		this.panel16.Controls.Add(this.label8);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(332, 46);
		this.panel16.TabIndex = 0;
		this.label9.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.label9.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.label9.ForeColor = System.Drawing.Color.LavenderBlush;
		this.label9.Location = new System.Drawing.Point(0, 19);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(332, 27);
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
		this.panel11.Location = new System.Drawing.Point(0, 149);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(284, 480);
		this.panel11.TabIndex = 27;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdModules);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 46);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(284, 434);
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
		((System.Windows.Forms.Control)(object)this.grdModules).Size = new System.Drawing.Size(284, 434);
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
		this.panel12.Size = new System.Drawing.Size(284, 46);
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
		this.label4.Size = new System.Drawing.Size(284, 27);
		this.label4.TabIndex = 28;
		this.label4.Text = "The selected module's screen will be filtered in the allocated or deallocated grid next to it";
		this.panel8.Controls.Add(this.groupBox1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 59);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(957, 90);
		this.panel8.TabIndex = 26;
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCompany);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(10, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(944, 81);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(10, 19);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(93, 15);
		this.label5.TabIndex = 21;
		this.label5.Text = "Company Name";
		this.CmbCompany.AllowNull = (DefaultableBoolean)1;
		this.CmbCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCompany).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCompany.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCompany).Location = new System.Drawing.Point(10, 40);
		((System.Windows.Forms.Control)(object)this.CmbCompany).Name = "CmbCompany";
		((System.Windows.Forms.Control)(object)this.CmbCompany).Size = new System.Drawing.Size(287, 26);
		((System.Windows.Forms.Control)(object)this.CmbCompany).TabIndex = 20;
		((System.Windows.Forms.Control)(object)this.CmbCompany).Leave += new System.EventHandler(CmbCompanyName_Leave);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label2);
		this.panel7.Controls.Add(this.toolStrip1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(957, 59);
		this.panel7.TabIndex = 25;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(3, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(391, 19);
		this.label2.TabIndex = 26;
		this.label2.Text = "Screens allocated to location and admin rights.";
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnRefresh, this.btnSave, this.btnDelete, this.BtnScreenDefinition, this.btnModuleAllocateToTemplete });
		this.toolStrip1.Location = new System.Drawing.Point(0, 32);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(957, 27);
		this.toolStrip1.TabIndex = 9;
		this.toolStrip1.Text = "toolStrip1";
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
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
		this.BtnScreenDefinition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnScreenDefinition.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnScreenDefinition.Image = (System.Drawing.Image)resources.GetObject("BtnScreenDefinition.Image");
		this.BtnScreenDefinition.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnScreenDefinition.Name = "BtnScreenDefinition";
		this.BtnScreenDefinition.Size = new System.Drawing.Size(123, 24);
		this.BtnScreenDefinition.Text = "Screen Definition";
		this.BtnScreenDefinition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnScreenDefinition.Visible = false;
		this.BtnScreenDefinition.Click += new System.EventHandler(BtnScreenDefinition_Click);
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
		this.btnModuleAllocateToTemplete.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnModuleAllocateToTemplete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnModuleAllocateToTemplete.Name = "btnModuleAllocateToTemplete";
		this.btnModuleAllocateToTemplete.Size = new System.Drawing.Size(188, 24);
		this.btnModuleAllocateToTemplete.Text = "Modules Allocate to Template";
		this.btnModuleAllocateToTemplete.Click += new System.EventHandler(btnModuleAllocateToTemplete_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(971, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ScreensAllocateToCompany";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(UserRightsByCompany_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ScreensAllocateToCompany_KeyDown);
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
		((System.ComponentModel.ISupportInitialize)this.grdUnAllocatedScreens).EndInit();
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
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
