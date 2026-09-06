using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Templates;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Templates;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.Templates
{

public class frmModuleAllocateToOrganizationTemplates : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private ToolStrip tsItemType;

	private ToolStripButton btnReset;

	private TabControl tabControl1;

	private TabPage tabPartyToItem;

	private Panel panel2;

	private Panel PanelMainTemplateII;

	private Panel panel1;

	internal GridEX GridAllocatedTemplateII;

	private Button btnDeleteTemplateII;

	private Label labelAllocateGridTemplateII;

	private Panel panel5;

	internal GridEX GridUnAllocatedTemplateII;

	private Button BtnSaveTemplateII;

	private Label labelUnAllocateGridTemplateII;

	private ToolStripButton BtnTemplateDefine;

	private Panel panel10;

	internal GridEX grdTemplateName;

	private Label label7;

	private ToolStripButton btnshortcutkeys;

	private Panel panel8;

	private Panel panel6;

	private Panel panel7;

	internal GridEX grdInActiveScreens;

	private Panel panel9;

	private Label label2;

	private Panel panel3;

	internal GridEX grdActiveScreens;

	private Panel panel4;

	private Button BtnSaveInActiveScreens;

	private Label label1;

	private Button BtnDeleteInActiveScreens;

	private Panel panel11;

	private ToolStrip toolStrip1;

	private ToolStripButton BtnRefreshScreensData;

	private RadioButton rdGroupeExpand;

	private RadioButton rdGroupCollapsed;

	public frmModuleAllocateToOrganizationTemplates(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void frmModuleAllocateToOrganizationTemplates_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(5, 5);
			base.AutoScrollMinSize = new Size(1020, 500);
			TemPlateGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TemPlateGridFill()
	{
		try
		{
			DataTable dtGrid = OrganizationTemplates.FormHistory();
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("TemplateName", typeof(string));
				dt.Columns.Add("TemplateDescription", typeof(string));
				foreach (DataRow row in dtGrid.Rows)
				{
					dt.Rows.Add(row["Id"], row["TemplateName"], row["TemplateDescription"]);
				}
				grdTemplateName.DataSource = dt;
				grdTemplateName.RetrieveStructure();
				GridTemplateSetting();
			}
			else
			{
				GridUnAllocatedTemplateII.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridTemplateSetting()
	{
		try
		{
			grdTemplateName.RootTable.Columns["Id"].Visible = false;
			grdTemplateName.RootTable.Columns["TemplateDescription"].Visible = false;
			grdTemplateName.RootTable.HeaderLines = 2;
			grdTemplateName.RootTable.Columns["TemplateName"].Width = 200;
			grdTemplateName.RootTable.Columns["TemplateDescription"].Width = 100;
			grdTemplateName.ColumnAutoResize = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdTemplateName_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdTemplateName.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int TemplateId = Conversion.ToInt(r.Cells["Id"].Value);
				string TemplateName = Conversion.ToString(r.Cells["TemplateName"].Value);
				UnAllocatedModulesGridFill(TemplateId);
				AllocatedModulesGridFill(TemplateId);
				labelUnAllocateGridTemplateII.Text = "UnAllocated Modules Of Template : '" + TemplateName + "'";
				labelAllocateGridTemplateII.Text = "Allocated Modules Of Template : '" + TemplateName + "'";
			}
			else
			{
				GridUnAllocatedTemplateII.DataSource = null;
				GridAllocatedTemplateII.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UnAllocatedModulesGridFill(int TemplateId)
	{
		try
		{
			DataTable dtUnAllocat = ModuleAllocateToOrganizationTemplates.ModuleAllocateToOrganizationTemplates_UnAllocatedData(TemplateId);
			if (dtUnAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("TemplateId");
				dt.Columns.Add("TemplateName");
				dt.Columns.Add("ModuleId");
				dt.Columns.Add("ModuleName");
				dt.Columns.Add("IsActive");
				for (int i = 0; i < dtUnAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtUnAllocat.Rows[i]["Id"], TemplateId, dtUnAllocat.Rows[i]["TemplateName"], dtUnAllocat.Rows[i]["ModuleId"], dtUnAllocat.Rows[i]["ModuleName"], dtUnAllocat.Rows[i]["IsActive"]);
				}
				GridUnAllocatedTemplateII.DataSource = dt;
				GridUnAllocatedTemplateII.RetrieveStructure();
				GridUnAllocatedModulesSetting();
			}
			else
			{
				GridUnAllocatedTemplateII.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridUnAllocatedModulesSetting()
	{
		try
		{
			GridUnAllocatedTemplateII.AllowEdit = (InheritableBoolean)2;
			GridUnAllocatedTemplateII.RootTable.Columns["Id"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.Columns["ModuleId"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.Columns["TemplateId"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.Columns["TemplateName"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.Columns["IsActive"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.HeaderLines = 2;
			GridUnAllocatedTemplateII.RootTable.Columns["ModuleName"].Width = 180;
			GridUnAllocatedTemplateII.RootTable.Columns.Add("Select");
			GridUnAllocatedTemplateII.RootTable.Columns["Select"].ActAsSelector = true;
			GridUnAllocatedTemplateII.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridUnAllocatedTemplateII.RootTable.Columns["Select"].Width = 30;
			GridUnAllocatedTemplateII.RootTable.Columns["Select"].Position = 0;
			GridUnAllocatedTemplateII.ColumnAutoResize = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AllocatedModulesGridFill(int TemplateId)
	{
		try
		{
			DataTable dtAllocat = ModuleAllocateToOrganizationTemplates.ModuleAllocateToOrganizationTemplates_AllocatedData(TemplateId);
			if (dtAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("TemplateId");
				dt.Columns.Add("TemplateName");
				dt.Columns.Add("ModuleId");
				dt.Columns.Add("ModuleName");
				dt.Columns.Add("IsActive");
				for (int i = 0; i < dtAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtAllocat.Rows[i]["Id"], dtAllocat.Rows[i]["TemplateId"], dtAllocat.Rows[i]["TemplateName"], dtAllocat.Rows[i]["ModuleId"], dtAllocat.Rows[i]["ModuleName"], dtAllocat.Rows[i]["IsActive"]);
				}
				GridAllocatedTemplateII.DataSource = dt;
				GridAllocatedTemplateII.RetrieveStructure();
				GridAllocatedModulesSetting();
			}
			else
			{
				GridAllocatedTemplateII.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridAllocatedModulesSetting()
	{
		try
		{
			GridAllocatedTemplateII.AllowEdit = (InheritableBoolean)2;
			GridAllocatedTemplateII.RootTable.Columns["Id"].Visible = false;
			GridAllocatedTemplateII.RootTable.Columns["ModuleId"].Visible = false;
			GridAllocatedTemplateII.RootTable.Columns["TemplateId"].Visible = false;
			GridAllocatedTemplateII.RootTable.Columns["TemplateName"].Visible = false;
			GridAllocatedTemplateII.RootTable.Columns["IsActive"].Visible = false;
			GridAllocatedTemplateII.RootTable.HeaderLines = 2;
			GridAllocatedTemplateII.RootTable.Columns["ModuleName"].Width = 180;
			GridAllocatedTemplateII.RootTable.Columns.Add("Select");
			GridAllocatedTemplateII.RootTable.Columns["Select"].ActAsSelector = true;
			GridAllocatedTemplateII.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridAllocatedTemplateII.RootTable.Columns["Select"].Width = 30;
			GridAllocatedTemplateII.RootTable.Columns["Select"].Position = 0;
			GridAllocatedTemplateII.ColumnAutoResize = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridAllocatedTemplateII_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			ScreensGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnSaveTemplateII_Click(object sender, EventArgs e)
	{
		try
		{
			InsertTemplate(GridUnAllocatedTemplateII, SaveRecord: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void InsertTemplate(GridEX grd, bool SaveRecord)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		try
		{
			string Msg = (SaveRecord ? "Allocate" : "UnAllocate");
			if (grd.GetCheckedRows().Length == 0)
			{
				throw new Exception("Check Row's Which You want To " + Msg);
			}
			if (MessageBox.Show("Are you sure to " + Msg + " Modules?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				ModuleAllocateToOrganizationTemplates obj = new ModuleAllocateToOrganizationTemplates();
				obj.ModuleAllocateToOrganizationTemplatesList = new List<ModuleAllocateToOrganizationTemplates>();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ModuleAllocateToOrganizationTemplates Template = new ModuleAllocateToOrganizationTemplates();
					Template.OrganizationTemplateId = Conversion.ToInt(r.Cells["TemplateId"].Value);
					Template.Id = Conversion.ToInt(r.Cells["Id"].Value);
					Template.ModuleId = Conversion.ToInt(r.Cells["ModuleId"].Value);
					Template.IsActive = SaveRecord;
					Template.EntryUserId = UserAccount.ID;
					Template.EntryDate = DateTime.Now;
					Template.ModifyDate = DateTime.Now;
					Template.ModifyUserId = UserAccount.ID;
					obj.ModuleAllocateToOrganizationTemplatesList.Add(Template);
				}
				ModuleAllocateToOrganizationTemplates.Save(obj);
				MessageBox.Show("Row's " + Msg + " Successfully");
				grdTemplateName_SelectionChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnUpdateTemplateII_Click(object sender, EventArgs e)
	{
		try
		{
			InsertTemplate(GridAllocatedTemplateII, SaveRecord: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ScreensGridFill(bool AllModules = false)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int TemplateId = 0;
			int ModuleId = 0;
			string ModuleName = "All Allocated Modules";
			GridEXRow r = GridAllocatedTemplateII.CurrentRow;
			if (!AllModules && r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				TemplateId = Conversion.ToInt(r.Cells["TemplateId"].Value);
				ModuleId = Conversion.ToInt(r.Cells["ModuleId"].Value);
				ModuleName = ((Conversion.ToInt(r.Cells["ModuleId"].Value) == 0) ? "All Allocated Modules" : Conversion.ToString(r.Cells["ModuleName"].Value));
			}
			else
			{
				grdActiveScreens.DataSource = null;
				grdInActiveScreens.DataSource = null;
			}
			if (TemplateId == 0)
			{
				GridEXRow rrr = grdTemplateName.CurrentRow;
				if (rrr != null && (int)rrr.RowType == 0 && rrr.Cells.Count > 0)
				{
					TemplateId = Conversion.ToInt(rrr.Cells["Id"].Value);
				}
			}
			if (TemplateId > 0)
			{
				UnAllocatedScreensGridFill(ModuleId, TemplateId);
				AllocatedScreensGridFill(ModuleId, TemplateId);
				label1.Text = "Active Screens Of Module : '" + ModuleName + "'";
				label2.Text = "InActive Screens Of Module : '" + ModuleName + "'";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnRefreshScreensData_Click(object sender, EventArgs e)
	{
		try
		{
			ScreensGridFill(AllModules: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UnAllocatedScreensGridFill(int ModuleId, int TemplateId)
	{
		try
		{
			DataTable dtUnAllocat = InActiveScreenOfTemplatesModule.InActiveScreenOfTemplatesModule_UnAllocatedData(TemplateId, ModuleId);
			if (dtUnAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ModuleId");
				dt.Columns.Add("ModuleName");
				dt.Columns.Add("TemplateId");
				dt.Columns.Add("TemplateName");
				dt.Columns.Add("ScreenId");
				dt.Columns.Add("ScreenName");
				dt.Columns.Add("ScreenAlias");
				dt.Columns.Add("IsActive");
				dt.Columns.Add("TargetUrl");
				for (int i = 0; i < dtUnAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtUnAllocat.Rows[i]["Id"], dtUnAllocat.Rows[i]["ModuleId"], dtUnAllocat.Rows[i]["ModuleName"], TemplateId, dtUnAllocat.Rows[i]["TemplateName"], dtUnAllocat.Rows[i]["ScreenId"], dtUnAllocat.Rows[i]["ScreenName"], dtUnAllocat.Rows[i]["ScreenAlias"], dtUnAllocat.Rows[i]["IsActive"], dtUnAllocat.Rows[i]["TargetUrl"]);
				}
				grdActiveScreens.DataSource = dt;
				grdActiveScreens.RetrieveStructure();
				grdActiveScreensSetting();
			}
			else
			{
				grdActiveScreens.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdActiveScreensSetting()
	{
		try
		{
			grdActiveScreens.AllowEdit = (InheritableBoolean)2;
			grdActiveScreens.RootTable.Columns["Id"].Visible = false;
			grdActiveScreens.RootTable.Columns["ScreenId"].Visible = false;
			grdActiveScreens.RootTable.Columns["ModuleId"].Visible = false;
			grdActiveScreens.RootTable.Columns["ModuleName"].Visible = false;
			grdActiveScreens.RootTable.Columns["TemplateId"].Visible = false;
			grdActiveScreens.RootTable.Columns["TemplateName"].Visible = false;
			grdActiveScreens.RootTable.Columns["ScreenName"].Visible = false;
			grdActiveScreens.RootTable.Columns["IsActive"].Visible = false;
			grdActiveScreens.RootTable.Columns["TargetUrl"].Visible = false;
			grdActiveScreens.RootTable.Columns["ModuleName"].Caption = "Module";
			grdActiveScreens.RootTable.Groups.Add("ModuleName");
			grdActiveScreens.GroupTotals = (GroupTotals)2;
			grdActiveScreens.TotalRow = (InheritableBoolean)2;
			grdActiveScreens.RootTable.HeaderLines = 2;
			grdActiveScreens.RootTable.Columns["ScreenAlias"].Caption = "Screen Name";
			grdActiveScreens.RootTable.Columns["ScreenAlias"].Width = 170;
			grdActiveScreens.RootTable.Columns["ScreenAlias"].ColumnType = (ColumnType)5;
			grdActiveScreens.RootTable.Columns.Add("Select");
			grdActiveScreens.RootTable.Columns["Select"].ActAsSelector = true;
			grdActiveScreens.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdActiveScreens.RootTable.Columns["Select"].Width = 30;
			grdActiveScreens.RootTable.Columns["Select"].Position = 0;
			grdActiveScreens.ColumnAutoResize = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AllocatedScreensGridFill(int ModuleId, int TemplateId)
	{
		try
		{
			DataTable dtAllocat = InActiveScreenOfTemplatesModule.InActiveScreenOfTemplatesModule_AllocatedData(TemplateId, ModuleId);
			if (dtAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ModuleId");
				dt.Columns.Add("ModuleName");
				dt.Columns.Add("TemplateId");
				dt.Columns.Add("TemplateName");
				dt.Columns.Add("ScreenId");
				dt.Columns.Add("ScreenName");
				dt.Columns.Add("ScreenAlias");
				dt.Columns.Add("IsActive");
				dt.Columns.Add("TargetUrl");
				for (int i = 0; i < dtAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtAllocat.Rows[i]["Id"], dtAllocat.Rows[i]["ModuleId"], dtAllocat.Rows[i]["ModuleName"], TemplateId, dtAllocat.Rows[i]["TemplateName"], dtAllocat.Rows[i]["ScreenId"], dtAllocat.Rows[i]["ScreenName"], dtAllocat.Rows[i]["ScreenAlias"], dtAllocat.Rows[i]["IsActive"], dtAllocat.Rows[i]["TargetUrl"]);
				}
				grdInActiveScreens.DataSource = dt;
				grdInActiveScreens.RetrieveStructure();
				GridAllocatedScreensSetting();
			}
			else
			{
				grdInActiveScreens.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridAllocatedScreensSetting()
	{
		try
		{
			grdInActiveScreens.AllowEdit = (InheritableBoolean)2;
			grdInActiveScreens.RootTable.Columns["Id"].Visible = false;
			grdInActiveScreens.RootTable.Columns["ScreenId"].Visible = false;
			grdInActiveScreens.RootTable.Columns["ModuleId"].Visible = false;
			grdInActiveScreens.RootTable.Columns["ModuleName"].Visible = false;
			grdInActiveScreens.RootTable.Columns["TemplateId"].Visible = false;
			grdInActiveScreens.RootTable.Columns["TemplateName"].Visible = false;
			grdInActiveScreens.RootTable.Columns["ScreenName"].Visible = false;
			grdInActiveScreens.RootTable.Columns["IsActive"].Visible = false;
			grdInActiveScreens.RootTable.Columns["TargetUrl"].Visible = false;
			grdInActiveScreens.RootTable.Columns["ModuleName"].Caption = "Module";
			grdInActiveScreens.RootTable.Groups.Add("ModuleName");
			grdInActiveScreens.GroupTotals = (GroupTotals)2;
			grdInActiveScreens.TotalRow = (InheritableBoolean)2;
			grdInActiveScreens.RootTable.HeaderLines = 2;
			grdInActiveScreens.RootTable.Columns["ScreenAlias"].Caption = "Screen Name";
			grdInActiveScreens.RootTable.Columns["ScreenAlias"].Width = 170;
			grdInActiveScreens.RootTable.Columns["ScreenAlias"].ColumnType = (ColumnType)5;
			grdInActiveScreens.RootTable.Columns.Add("Select");
			grdInActiveScreens.RootTable.Columns["Select"].ActAsSelector = true;
			grdInActiveScreens.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdInActiveScreens.RootTable.Columns["Select"].Width = 30;
			grdInActiveScreens.RootTable.Columns["Select"].Position = 0;
			grdInActiveScreens.ColumnAutoResize = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void InsertScreen(GridEX grd, bool SaveRecord)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		try
		{
			string Msg = (SaveRecord ? "Allocate" : "UnAllocate");
			if (grd.GetCheckedRows().Length == 0)
			{
				throw new Exception("Check Row's Which You want To " + Msg);
			}
			if (MessageBox.Show("Are you sure to " + Msg + " Modules?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				InActiveScreenOfTemplatesModule obj = new InActiveScreenOfTemplatesModule();
				obj.InActiveScreenOfTemplatesModuleList = new List<InActiveScreenOfTemplatesModule>();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					InActiveScreenOfTemplatesModule Template = new InActiveScreenOfTemplatesModule();
					Template.OrganizationTemplateId = Conversion.ToInt(r.Cells["TemplateId"].Value);
					Template.Id = Conversion.ToInt(r.Cells["Id"].Value);
					Template.ModuleId = Conversion.ToInt(r.Cells["ModuleId"].Value);
					Template.ScreenName = Conversion.ToString(r.Cells["ScreenName"].Value);
					Template.IsActive = SaveRecord;
					Template.EntryUserId = UserAccount.ID;
					Template.EntryDate = DateTime.Now;
					Template.ModifyDate = DateTime.Now;
					Template.ModifyUserId = UserAccount.ID;
					obj.InActiveScreenOfTemplatesModuleList.Add(Template);
				}
				InActiveScreenOfTemplatesModule.Save(obj);
				MessageBox.Show("Row's " + Msg + " Successfully");
				ScreensGridFill(AllModules: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnSaveInActiveScreens_Click(object sender, EventArgs e)
	{
		try
		{
			InsertScreen(grdActiveScreens, SaveRecord: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnDeleteInActiveScreens_Click(object sender, EventArgs e)
	{
		try
		{
			InsertScreen(grdInActiveScreens, SaveRecord: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void rdGroupCollapsed_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (rdGroupCollapsed.Checked)
			{
				grdInActiveScreens.CollapseRecords();
				grdActiveScreens.CollapseRecords();
			}
			else
			{
				grdInActiveScreens.ExpandRecords();
				grdActiveScreens.ExpandRecords();
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
			TemPlateGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmModuleAllocateToOrganizationTemplates_FormClosing(object sender, FormClosingEventArgs e)
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

	private void BtnTemplateDefine_Click(object sender, EventArgs e)
	{
		try
		{
			new OrganizationTemplates(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmModuleAllocateToOrganizationTemplates_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.Control && e.KeyCode == Keys.R)
		{
			btnRefresh_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.Alt)
		{
			MakeShortCutKeys();
		}
		if (e.Control && e.KeyCode == Keys.F5)
		{
			((Control)(object)grdTemplateName).Focus();
		}
		if (e.Control && e.KeyCode == Keys.U)
		{
			BtnUpdateTemplateII_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.A)
		{
			BtnSaveTemplateII_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.Up)
		{
			((Control)(object)grdTemplateName).Focus();
		}
		if (e.Control && e.KeyCode == Keys.Down)
		{
			if (Conversion.ToString(base.ActiveControl.Tag) == "UnAllocatedGrid")
			{
				((Control)(object)GridAllocatedTemplateII).Focus();
			}
			else
			{
				((Control)(object)GridUnAllocatedTemplateII).Focus();
			}
		}
		if (e.Control && e.KeyCode == Keys.Left)
		{
			((Control)(object)GridUnAllocatedTemplateII).Focus();
		}
		if (e.Control && e.KeyCode == Keys.Right)
		{
			((Control)(object)GridAllocatedTemplateII).Focus();
		}
	}

	private void btnshortcutkeys_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+A/Ctrl+S", "For Press Allocate Button");
			dt.Rows.Add("Ctrl+U", "For Press Update Button");
			dt.Rows.Add("Ctrl+D/Ctrl+Delete", "For Press UnAllocate Button");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+F5", "For Focus on Template Grid");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowRight", "to change focus from one grid to another");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdActiveScreens_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdActiveScreens.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "ScreenAlias")
			{
				string targetUrl = Conversion.ToString(r.Cells["TargetUrl"].Value);
				string ScreenAlias = Conversion.ToString(r.Cells["ScreenAlias"].Value);
				string ScreenName = Conversion.ToString(r.Cells["ScreenName"].Value);
				CommonServices.OpenDynamicallyScreen(targetUrl, ScreenAlias, ScreenName, UserAccount);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInActiveScreens_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdInActiveScreens.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "ScreenAlias")
			{
				string targetUrl = Conversion.ToString(r.Cells["TargetUrl"].Value);
				string ScreenAlias = Conversion.ToString(r.Cells["ScreenAlias"].Value);
				string ScreenName = Conversion.ToString(r.Cells["ScreenName"].Value);
				CommonServices.OpenDynamicallyScreen(targetUrl, ScreenAlias, ScreenName, UserAccount);
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
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_087c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0886: Expected O, but got Unknown
		//IL_0c65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6f: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Templates.frmModuleAllocateToOrganizationTemplates));
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnReset = new System.Windows.Forms.ToolStripButton();
		this.BtnTemplateDefine = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPartyToItem = new System.Windows.Forms.TabPage();
		this.PanelMainTemplateII = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdInActiveScreens = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.BtnDeleteInActiveScreens = new System.Windows.Forms.Button();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdActiveScreens = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.BtnSaveInActiveScreens = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.rdGroupeExpand = new System.Windows.Forms.RadioButton();
		this.rdGroupCollapsed = new System.Windows.Forms.RadioButton();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.BtnRefreshScreensData = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.GridAllocatedTemplateII = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.btnDeleteTemplateII = new System.Windows.Forms.Button();
		this.labelAllocateGridTemplateII = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.GridUnAllocatedTemplateII = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.BtnSaveTemplateII = new System.Windows.Forms.Button();
		this.labelUnAllocateGridTemplateII = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdTemplateName = new GridEX();
		this.label7 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tsItemType.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPartyToItem.SuspendLayout();
		this.PanelMainTemplateII.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInActiveScreens).BeginInit();
		this.panel9.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdActiveScreens).BeginInit();
		this.panel4.SuspendLayout();
		this.panel11.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridAllocatedTemplateII).BeginInit();
		this.panel8.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocatedTemplateII).BeginInit();
		this.panel6.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdTemplateName).BeginInit();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnReset, this.BtnTemplateDefine, this.btnshortcutkeys });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(1184, 27);
		this.tsItemType.TabIndex = 2;
		this.tsItemType.Text = "toolStrip4";
		this.btnReset.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnReset.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(68, 24);
		this.btnReset.Text = "&Reset";
		this.btnReset.Click += new System.EventHandler(btnRefresh_Click);
		this.BtnTemplateDefine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnTemplateDefine.Image = (System.Drawing.Image)resources.GetObject("BtnTemplateDefine.Image");
		this.BtnTemplateDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnTemplateDefine.Name = "BtnTemplateDefine";
		this.BtnTemplateDefine.Size = new System.Drawing.Size(117, 24);
		this.BtnTemplateDefine.Text = "Define Template";
		this.BtnTemplateDefine.Click += new System.EventHandler(BtnTemplateDefine_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.tabControl1.Controls.Add(this.tabPartyToItem);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 30);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1184, 621);
		this.tabControl1.TabIndex = 1;
		this.tabPartyToItem.Controls.Add(this.PanelMainTemplateII);
		this.tabPartyToItem.Location = new System.Drawing.Point(4, 25);
		this.tabPartyToItem.Name = "tabPartyToItem";
		this.tabPartyToItem.Padding = new System.Windows.Forms.Padding(3);
		this.tabPartyToItem.Size = new System.Drawing.Size(1176, 592);
		this.tabPartyToItem.TabIndex = 1;
		this.tabPartyToItem.Text = "Module Allocation To Organization Template";
		this.tabPartyToItem.UseVisualStyleBackColor = true;
		this.PanelMainTemplateII.Controls.Add(this.panel7);
		this.PanelMainTemplateII.Controls.Add(this.panel3);
		this.PanelMainTemplateII.Controls.Add(this.panel11);
		this.PanelMainTemplateII.Controls.Add(this.panel1);
		this.PanelMainTemplateII.Controls.Add(this.panel5);
		this.PanelMainTemplateII.Controls.Add(this.panel10);
		this.PanelMainTemplateII.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelMainTemplateII.Location = new System.Drawing.Point(3, 3);
		this.PanelMainTemplateII.Name = "PanelMainTemplateII";
		this.PanelMainTemplateII.Size = new System.Drawing.Size(1170, 586);
		this.PanelMainTemplateII.TabIndex = 1;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdInActiveScreens);
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel7.Location = new System.Drawing.Point(938, 32);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(230, 554);
		this.panel7.TabIndex = 83;
		this.grdInActiveScreens.AllowEdit = (InheritableBoolean)2;
		this.grdInActiveScreens.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdInActiveScreens).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdInActiveScreens.FilterMode = (FilterMode)2;
		this.grdInActiveScreens.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdInActiveScreens).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInActiveScreens.GroupByBoxVisible = false;
		this.grdInActiveScreens.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdInActiveScreens).Location = new System.Drawing.Point(0, 29);
		((System.Windows.Forms.Control)(object)this.grdInActiveScreens).Name = "grdInActiveScreens";
		((ScrollableBase)this.grdInActiveScreens).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdInActiveScreens).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdInActiveScreens).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInActiveScreens).Size = new System.Drawing.Size(230, 525);
		((System.Windows.Forms.Control)(object)this.grdInActiveScreens).TabIndex = 67;
		((System.Windows.Forms.Control)(object)this.grdInActiveScreens).Tag = "UnAllocatedGrid";
		this.grdInActiveScreens.TotalRow = (InheritableBoolean)1;
		this.grdInActiveScreens.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInActiveScreens).VisualStyle = (VisualStyle)4;
		this.grdInActiveScreens.LinkClicked += new ColumnActionEventHandler(grdInActiveScreens_LinkClicked);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label2);
		this.panel9.Controls.Add(this.BtnDeleteInActiveScreens);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(230, 29);
		this.panel9.TabIndex = 68;
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(6, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(179, 27);
		this.label2.TabIndex = 3;
		this.label2.Text = "InActive Screens Of Module : Accounts Definition";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnDeleteInActiveScreens.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnDeleteInActiveScreens.BackColor = System.Drawing.Color.White;
		this.BtnDeleteInActiveScreens.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnDeleteInActiveScreens.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnDeleteInActiveScreens.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnDeleteInActiveScreens.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnDeleteInActiveScreens.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnDeleteInActiveScreens.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.BtnDeleteInActiveScreens.Location = new System.Drawing.Point(199, 2);
		this.BtnDeleteInActiveScreens.Margin = new System.Windows.Forms.Padding(0);
		this.BtnDeleteInActiveScreens.Name = "BtnDeleteInActiveScreens";
		this.BtnDeleteInActiveScreens.Size = new System.Drawing.Size(26, 24);
		this.BtnDeleteInActiveScreens.TabIndex = 5;
		this.BtnDeleteInActiveScreens.UseVisualStyleBackColor = false;
		this.BtnDeleteInActiveScreens.Click += new System.EventHandler(BtnDeleteInActiveScreens_Click);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdActiveScreens);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(708, 32);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(230, 554);
		this.panel3.TabIndex = 82;
		this.grdActiveScreens.AllowEdit = (InheritableBoolean)2;
		this.grdActiveScreens.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdActiveScreens).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdActiveScreens.FilterMode = (FilterMode)2;
		this.grdActiveScreens.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdActiveScreens).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdActiveScreens.GroupByBoxVisible = false;
		this.grdActiveScreens.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdActiveScreens).Location = new System.Drawing.Point(0, 29);
		((System.Windows.Forms.Control)(object)this.grdActiveScreens).Name = "grdActiveScreens";
		((ScrollableBase)this.grdActiveScreens).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdActiveScreens).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdActiveScreens).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdActiveScreens).Size = new System.Drawing.Size(230, 525);
		((System.Windows.Forms.Control)(object)this.grdActiveScreens).TabIndex = 67;
		((System.Windows.Forms.Control)(object)this.grdActiveScreens).Tag = "UnAllocatedGrid";
		this.grdActiveScreens.TotalRow = (InheritableBoolean)1;
		this.grdActiveScreens.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdActiveScreens).VisualStyle = (VisualStyle)4;
		this.grdActiveScreens.LinkClicked += new ColumnActionEventHandler(grdActiveScreens_LinkClicked);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.BtnSaveInActiveScreens);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(230, 29);
		this.panel4.TabIndex = 68;
		this.BtnSaveInActiveScreens.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnSaveInActiveScreens.BackColor = System.Drawing.Color.White;
		this.BtnSaveInActiveScreens.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnSaveInActiveScreens.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnSaveInActiveScreens.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnSaveInActiveScreens.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnSaveInActiveScreens.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSaveInActiveScreens.Image = Architecture.WinApp.Properties.Resources.Forward;
		this.BtnSaveInActiveScreens.Location = new System.Drawing.Point(201, 2);
		this.BtnSaveInActiveScreens.Margin = new System.Windows.Forms.Padding(0);
		this.BtnSaveInActiveScreens.Name = "BtnSaveInActiveScreens";
		this.BtnSaveInActiveScreens.Size = new System.Drawing.Size(26, 24);
		this.BtnSaveInActiveScreens.TabIndex = 66;
		this.BtnSaveInActiveScreens.UseVisualStyleBackColor = false;
		this.BtnSaveInActiveScreens.Click += new System.EventHandler(BtnSaveInActiveScreens_Click);
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(3, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(151, 27);
		this.label1.TabIndex = 3;
		this.label1.Text = "Active Screens Of Module : Accounts Definition";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel11.Controls.Add(this.rdGroupeExpand);
		this.panel11.Controls.Add(this.rdGroupCollapsed);
		this.panel11.Controls.Add(this.toolStrip1);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(708, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(462, 32);
		this.panel11.TabIndex = 84;
		this.rdGroupeExpand.AutoSize = true;
		this.rdGroupeExpand.Checked = true;
		this.rdGroupeExpand.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdGroupeExpand.Location = new System.Drawing.Point(98, 5);
		this.rdGroupeExpand.Name = "rdGroupeExpand";
		this.rdGroupeExpand.Size = new System.Drawing.Size(71, 21);
		this.rdGroupeExpand.TabIndex = 10;
		this.rdGroupeExpand.TabStop = true;
		this.rdGroupeExpand.Text = "Expand";
		this.rdGroupeExpand.UseVisualStyleBackColor = true;
		this.rdGroupeExpand.Visible = false;
		this.rdGroupCollapsed.AutoSize = true;
		this.rdGroupCollapsed.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdGroupCollapsed.Location = new System.Drawing.Point(172, 5);
		this.rdGroupCollapsed.Name = "rdGroupCollapsed";
		this.rdGroupCollapsed.Size = new System.Drawing.Size(76, 21);
		this.rdGroupCollapsed.TabIndex = 9;
		this.rdGroupCollapsed.Text = "Collapse";
		this.rdGroupCollapsed.UseVisualStyleBackColor = true;
		this.rdGroupCollapsed.Visible = false;
		this.rdGroupCollapsed.CheckedChanged += new System.EventHandler(rdGroupCollapsed_CheckedChanged);
		this.toolStrip1.AutoSize = false;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnRefreshScreensData });
		this.toolStrip1.Location = new System.Drawing.Point(0, 2);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(95, 27);
		this.toolStrip1.TabIndex = 3;
		this.toolStrip1.Text = "toolStrip4";
		this.BtnRefreshScreensData.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshScreensData.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshScreensData.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshScreensData.Name = "BtnRefreshScreensData";
		this.BtnRefreshScreensData.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshScreensData.Text = "&Refresh";
		this.BtnRefreshScreensData.Click += new System.EventHandler(BtnRefreshScreensData_Click);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateII);
		this.panel1.Controls.Add(this.panel8);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(476, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(232, 586);
		this.panel1.TabIndex = 80;
		this.GridAllocatedTemplateII.AllowEdit = (InheritableBoolean)2;
		this.GridAllocatedTemplateII.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateII).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridAllocatedTemplateII.FilterMode = (FilterMode)2;
		this.GridAllocatedTemplateII.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateII).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridAllocatedTemplateII.GroupByBoxVisible = false;
		this.GridAllocatedTemplateII.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateII).Location = new System.Drawing.Point(0, 32);
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateII).Name = "GridAllocatedTemplateII";
		((ScrollableBase)this.GridAllocatedTemplateII).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridAllocatedTemplateII).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridAllocatedTemplateII).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateII).Size = new System.Drawing.Size(232, 554);
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateII).TabIndex = 64;
		this.GridAllocatedTemplateII.TotalRow = (InheritableBoolean)1;
		this.GridAllocatedTemplateII.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridAllocatedTemplateII).VisualStyle = (VisualStyle)4;
		this.GridAllocatedTemplateII.SelectionChanged += new System.EventHandler(GridAllocatedTemplateII_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateII).Click += new System.EventHandler(GridAllocatedTemplateII_SelectionChanged);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.btnDeleteTemplateII);
		this.panel8.Controls.Add(this.labelAllocateGridTemplateII);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(232, 32);
		this.panel8.TabIndex = 69;
		this.btnDeleteTemplateII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnDeleteTemplateII.BackColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDeleteTemplateII.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDeleteTemplateII.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDeleteTemplateII.Location = new System.Drawing.Point(195, 2);
		this.btnDeleteTemplateII.Margin = new System.Windows.Forms.Padding(0);
		this.btnDeleteTemplateII.Name = "btnDeleteTemplateII";
		this.btnDeleteTemplateII.Size = new System.Drawing.Size(33, 27);
		this.btnDeleteTemplateII.TabIndex = 5;
		this.btnDeleteTemplateII.UseVisualStyleBackColor = false;
		this.btnDeleteTemplateII.Click += new System.EventHandler(BtnUpdateTemplateII_Click);
		this.labelAllocateGridTemplateII.BackColor = System.Drawing.Color.Teal;
		this.labelAllocateGridTemplateII.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelAllocateGridTemplateII.ForeColor = System.Drawing.Color.White;
		this.labelAllocateGridTemplateII.Location = new System.Drawing.Point(2, 0);
		this.labelAllocateGridTemplateII.Name = "labelAllocateGridTemplateII";
		this.labelAllocateGridTemplateII.Size = new System.Drawing.Size(186, 29);
		this.labelAllocateGridTemplateII.TabIndex = 4;
		this.labelAllocateGridTemplateII.Text = "Allocated Modules Of Template : RETAIL";
		this.labelAllocateGridTemplateII.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateII);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(244, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(232, 586);
		this.panel5.TabIndex = 79;
		this.GridUnAllocatedTemplateII.AllowEdit = (InheritableBoolean)2;
		this.GridUnAllocatedTemplateII.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateII).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridUnAllocatedTemplateII.FilterMode = (FilterMode)2;
		this.GridUnAllocatedTemplateII.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateII).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridUnAllocatedTemplateII.GroupByBoxVisible = false;
		this.GridUnAllocatedTemplateII.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateII).Location = new System.Drawing.Point(0, 32);
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateII).Name = "GridUnAllocatedTemplateII";
		((ScrollableBase)this.GridUnAllocatedTemplateII).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridUnAllocatedTemplateII).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridUnAllocatedTemplateII).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateII).Size = new System.Drawing.Size(232, 554);
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateII).TabIndex = 67;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateII).Tag = "UnAllocatedGrid";
		this.GridUnAllocatedTemplateII.TotalRow = (InheritableBoolean)1;
		this.GridUnAllocatedTemplateII.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridUnAllocatedTemplateII).VisualStyle = (VisualStyle)4;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.BtnSaveTemplateII);
		this.panel6.Controls.Add(this.labelUnAllocateGridTemplateII);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(232, 32);
		this.panel6.TabIndex = 68;
		this.BtnSaveTemplateII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnSaveTemplateII.BackColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnSaveTemplateII.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSaveTemplateII.Image = Architecture.WinApp.Properties.Resources.Forward;
		this.BtnSaveTemplateII.Location = new System.Drawing.Point(195, 3);
		this.BtnSaveTemplateII.Margin = new System.Windows.Forms.Padding(0);
		this.BtnSaveTemplateII.Name = "BtnSaveTemplateII";
		this.BtnSaveTemplateII.Size = new System.Drawing.Size(33, 27);
		this.BtnSaveTemplateII.TabIndex = 66;
		this.BtnSaveTemplateII.UseVisualStyleBackColor = false;
		this.BtnSaveTemplateII.Click += new System.EventHandler(BtnSaveTemplateII_Click);
		this.labelUnAllocateGridTemplateII.BackColor = System.Drawing.Color.Teal;
		this.labelUnAllocateGridTemplateII.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelUnAllocateGridTemplateII.ForeColor = System.Drawing.Color.White;
		this.labelUnAllocateGridTemplateII.Location = new System.Drawing.Point(6, 0);
		this.labelUnAllocateGridTemplateII.Name = "labelUnAllocateGridTemplateII";
		this.labelUnAllocateGridTemplateII.Size = new System.Drawing.Size(186, 29);
		this.labelUnAllocateGridTemplateII.TabIndex = 3;
		this.labelUnAllocateGridTemplateII.Text = "UnAllocated Modules Of Template : RETAIL";
		this.labelUnAllocateGridTemplateII.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdTemplateName);
		this.panel10.Controls.Add(this.label7);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(244, 586);
		this.panel10.TabIndex = 81;
		this.grdTemplateName.AllowEdit = (InheritableBoolean)2;
		this.grdTemplateName.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdTemplateName).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdTemplateName.FilterMode = (FilterMode)2;
		this.grdTemplateName.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdTemplateName).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdTemplateName.GroupByBoxVisible = false;
		this.grdTemplateName.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdTemplateName).Location = new System.Drawing.Point(0, 32);
		((System.Windows.Forms.Control)(object)this.grdTemplateName).Name = "grdTemplateName";
		((ScrollableBase)this.grdTemplateName).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdTemplateName).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdTemplateName).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdTemplateName).Size = new System.Drawing.Size(244, 554);
		((System.Windows.Forms.Control)(object)this.grdTemplateName).TabIndex = 67;
		((System.Windows.Forms.Control)(object)this.grdTemplateName).Tag = "UnAllocatedGrid";
		this.grdTemplateName.TotalRow = (InheritableBoolean)1;
		this.grdTemplateName.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdTemplateName).VisualStyle = (VisualStyle)4;
		this.grdTemplateName.SelectionChanged += new System.EventHandler(grdTemplateName_SelectionChanged);
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Dock = System.Windows.Forms.DockStyle.Top;
		this.label7.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(0, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(244, 32);
		this.label7.TabIndex = 3;
		this.label7.Text = "Template Name";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1184, 30);
		this.panel2.TabIndex = 3;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 651);
		base.Controls.Add(this.tabControl1);
		base.Controls.Add(this.panel2);
		base.KeyPreview = true;
		base.Name = "frmModuleAllocateToOrganizationTemplates";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmModuleAllocateToOrganizationTemplates_FormClosing);
		base.Load += new System.EventHandler(frmModuleAllocateToOrganizationTemplates_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmModuleAllocateToOrganizationTemplates_KeyDown);
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabPartyToItem.ResumeLayout(false);
		this.PanelMainTemplateII.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInActiveScreens).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdActiveScreens).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridAllocatedTemplateII).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocatedTemplateII).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdTemplateName).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
