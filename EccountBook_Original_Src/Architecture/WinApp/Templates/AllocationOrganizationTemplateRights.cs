using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Templates;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Templates;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Templates;

public class AllocationOrganizationTemplateRights : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip tsItemType;

	private ToolStripButton btnRefresh;

	internal UltraCombo CmbTemplateName;

	private Panel panel6;

	private Label label15;

	private Panel panel7;

	private Label lbltocompany;

	private Panel panel3;

	private Label label4;

	private Button BtnAllocateItems;

	private Panel panel4;

	private Label label1;

	private Button btnDeAllocate;

	internal GridEX GridUnAllocatedItems;

	internal GridEX GridAllocatedItems;

	private ToolStripButton btnshortcutkeys;

	internal UltraCombo CmbModuleName;

	private Label label2;

	private Button btnshow;

	private Label label36;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripButton BtnTemplateName;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripButton BtnfrmAllScreensInProject;

	public AllocationOrganizationTemplateRights(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void frmItemsCustomizedGroups_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(820, 450);
			TemplateNameFill();
			AppModuleFill();
			CmbTemplateName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TemplateNameFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbTemplateName.Value);
			DataTable dt = OrganizationTemplates.FormHistory();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, CmbTemplateName, "Id", "TemplateName", "Template Name", true);
			((UltraGridBase)CmbTemplateName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbTemplateName.Value = Id;
				}
				else
				{
					((Control)(object)CmbTemplateName).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AppModuleFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbModuleName.Value);
			DataTable dt = ScreenDefinition.GetAllModules();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbModuleName, "Id", "ModuleDescription", "Module Name", ZeroIndex: true);
				((UltraGridBase)CmbModuleName).DisplayLayout.Bands[0].Columns[1].Width = 168;
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbModuleName.Value = Id;
					}
					else
					{
						((Control)(object)CmbModuleName).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbModuleName).Text = "";
				((UltraGridBase)CmbModuleName).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemCutomGroup_TextChanged(object sender, EventArgs e)
	{
		try
		{
			ShowData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			ShowData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ShowData()
	{
		try
		{
			if (Conversion.ToInt(CmbTemplateName.Value) > 0)
			{
				UnAllocatedItemsGridFill();
				AllocatedItemsGridFill();
			}
			else
			{
				MessageBox.Show("Select Template Name First");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void UnAllocatedItemsGridFill()
	{
		try
		{
			DataTable dtUnAllocat = OrganizationTemplateRights.GetAllocatedAndUnAllocatedScreens(Conversion.ToInt(CmbTemplateName.Value), 1, Conversion.ToInt(CmbModuleName.Value));
			if (dtUnAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ScreenId");
				dt.Columns.Add("ScreenName");
				for (int i = 0; i < dtUnAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtUnAllocat.Rows[i]["ScreenId"], dtUnAllocat.Rows[i]["ScreenAlias"]);
				}
				GridUnAllocatedItems.DataSource = dt;
				GridUnAllocatedItems.RetrieveStructure();
				GridUnAllocatedItemsSetting();
			}
			else
			{
				GridUnAllocatedItems.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridUnAllocatedItemsSetting()
	{
		try
		{
			GridUnAllocatedItems.RootTable.Columns["ScreenId"].Visible = false;
			GridUnAllocatedItems.RootTable.Columns["ScreenName"].Width = 340;
			GridUnAllocatedItems.RootTable.Columns.Add("Select");
			GridUnAllocatedItems.RootTable.Columns["Select"].ActAsSelector = true;
			GridUnAllocatedItems.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridUnAllocatedItems.RootTable.Columns["Select"].Width = 60;
			GridUnAllocatedItems.RootTable.Columns["Select"].Position = 0;
			GridUnAllocatedItems.ColumnAutoResize = true;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AllocatedItemsGridFill()
	{
		try
		{
			DataTable dtAllocat = OrganizationTemplateRights.GetAllocatedAndUnAllocatedScreens(Conversion.ToInt(CmbTemplateName.Value), 2, Conversion.ToInt(CmbModuleName.Value));
			if (dtAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ScreenId");
				dt.Columns.Add("ScreenName");
				for (int i = 0; i < dtAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtAllocat.Rows[i]["ScreenId"], dtAllocat.Rows[i]["ScreenAlias"]);
				}
				GridAllocatedItems.DataSource = dt;
				GridAllocatedItems.RetrieveStructure();
				GridAllocatedItemsSetting();
			}
			else
			{
				GridAllocatedItems.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridAllocatedItemsSetting()
	{
		try
		{
			GridAllocatedItems.RootTable.Columns["ScreenId"].Visible = false;
			GridAllocatedItems.RootTable.Columns["ScreenName"].Width = 340;
			GridAllocatedItems.RootTable.Columns.Add("Select");
			GridAllocatedItems.RootTable.Columns["Select"].ActAsSelector = true;
			GridAllocatedItems.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridAllocatedItems.RootTable.Columns["Select"].Width = 60;
			GridAllocatedItems.RootTable.Columns["Select"].Position = 0;
			GridAllocatedItems.ColumnAutoResize = true;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnAllocateItems_Click(object sender, EventArgs e)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbTemplateName.Value) == 0)
			{
				MessageBox.Show("Select Template Name First");
				CmbTemplateName.Focus();
				return;
			}
			if (GridUnAllocatedItems.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked Row's first To Allocate Items");
			}
			OrganizationTemplateRights obj = new OrganizationTemplateRights();
			obj.OrgTemplatesRightsList = new List<OrganizationTemplateRights>();
			GridEXRow[] checkedRows = GridUnAllocatedItems.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				OrganizationTemplateRights item = new OrganizationTemplateRights();
				item.OrganizationTemplateId = Conversion.ToInt(CmbTemplateName.Value);
				item.ScreenId = Conversion.ToInt(r.Cells["ScreenId"].Value);
				obj.OrgTemplatesRightsList.Add(item);
			}
			OrganizationTemplateRights.Save(obj);
			MessageBox.Show("Screen's Allocated Successfully");
			UnAllocatedItemsGridFill();
			AllocatedItemsGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnDeAllocate_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbTemplateName.Value) == 0)
			{
				MessageBox.Show("Select Product Type First");
				CmbTemplateName.Focus();
				return;
			}
			if (GridAllocatedItems.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked Row's first To Un-Allocate Items");
			}
			if (Conversion.ToInt(CmbTemplateName.Value) > 0)
			{
				if (MessageBox.Show("Are you sure to UnAllocate Item's?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
				{
					string ids = null;
					GridEXRow[] checkedRows = GridAllocatedItems.GetCheckedRows();
					foreach (GridEXRow item in checkedRows)
					{
						ids = ids + Conversion.ToString(item.Cells["ScreenId"].Value) + ",";
					}
					OrganizationTemplateRights.DeleteById(Conversion.ToInt(CmbTemplateName.Value), ids);
					MessageBox.Show("Screen's UnAllocated Successfully");
					UnAllocatedItemsGridFill();
					AllocatedItemsGridFill();
				}
				return;
			}
			throw new Exception("Select Company First To Un-Allocate Item's");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			TemplateNameFill();
			AppModuleFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemAllocationToCustomGroup_FormClosing(object sender, FormClosingEventArgs e)
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

	private void BtnTemplateName_Click(object sender, EventArgs e)
	{
		new OrganizationTemplates(UserAccount).Show();
	}

	private void frmItemsCustomizedGroups_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.Control && e.KeyCode == Keys.R)
		{
			btnRefresh_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.F5)
		{
			CmbTemplateName.Focus();
		}
		if (e.Control && e.KeyCode == Keys.A)
		{
			BtnAllocateItems_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.D)
		{
			btnDeAllocate_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.Down)
		{
			if (Conversion.ToString(base.ActiveControl.Tag) == "UnAllocatedGrid")
			{
				((Control)(object)GridAllocatedItems).Focus();
			}
			else
			{
				((Control)(object)GridUnAllocatedItems).Focus();
			}
		}
		if (e.Control && e.KeyCode == Keys.Right)
		{
			if (((Control)(object)GridAllocatedItems).Focused)
			{
				((Control)(object)GridUnAllocatedItems).Focus();
			}
			else
			{
				((Control)(object)GridAllocatedItems).Focus();
			}
		}
		if (e.Control && e.Alt)
		{
			MakeShortCutKeys();
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
			dt.Rows.Add("Ctrl+A", "For Press Allocate Button to Allocate Items");
			dt.Rows.Add("Ctrl+D", "For Press UnAllocate Button to UnAllocate Items");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+F5", "For Focus on ProductType");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowRight", "to change focus from one grid to another");
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

	private void BtnfrmAllScreensInProject_Click(object sender, EventArgs e)
	{
		try
		{
			new frmAllScreensInProject(UserAccount).Show();
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
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Expected O, but got Unknown
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.GridAllocatedItems = new GridEX();
		this.btnDeAllocate = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.GridUnAllocatedItems = new GridEX();
		this.BtnAllocateItems = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label36 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.CmbModuleName = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.lbltocompany = new System.Windows.Forms.Label();
		this.CmbTemplateName = new UltraCombo();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.BtnTemplateName = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnfrmAllScreensInProject = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridAllocatedItems).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocatedItems).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbModuleName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTemplateName).BeginInit();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		this.tsItemType.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel7);
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(802, 586);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.GridAllocatedItems);
		this.panel4.Controls.Add(this.btnDeAllocate);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(400, 94);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(400, 492);
		this.panel4.TabIndex = 80;
		this.GridAllocatedItems.AllowEdit = (InheritableBoolean)2;
		this.GridAllocatedItems.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridAllocatedItems).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridAllocatedItems.FilterMode = (FilterMode)2;
		this.GridAllocatedItems.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridAllocatedItems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridAllocatedItems.GroupByBoxVisible = false;
		this.GridAllocatedItems.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridAllocatedItems).Location = new System.Drawing.Point(0, 31);
		((System.Windows.Forms.Control)(object)this.GridAllocatedItems).Name = "GridAllocatedItems";
		((ScrollableBase)this.GridAllocatedItems).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridAllocatedItems).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridAllocatedItems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridAllocatedItems).Size = new System.Drawing.Size(400, 461);
		((System.Windows.Forms.Control)(object)this.GridAllocatedItems).TabIndex = 0;
		this.GridAllocatedItems.TotalRow = (InheritableBoolean)1;
		this.GridAllocatedItems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridAllocatedItems).VisualStyle = (VisualStyle)4;
		this.btnDeAllocate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnDeAllocate.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDeAllocate.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDeAllocate.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDeAllocate.Location = new System.Drawing.Point(371, 3);
		this.btnDeAllocate.Margin = new System.Windows.Forms.Padding(0);
		this.btnDeAllocate.Name = "btnDeAllocate";
		this.btnDeAllocate.Size = new System.Drawing.Size(25, 25);
		this.btnDeAllocate.TabIndex = 1;
		this.btnDeAllocate.UseVisualStyleBackColor = true;
		this.btnDeAllocate.Click += new System.EventHandler(btnDeAllocate_Click);
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(400, 31);
		this.label1.TabIndex = 4;
		this.label1.Text = "Allocated Screen";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems);
		this.panel3.Controls.Add(this.BtnAllocateItems);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 94);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(400, 492);
		this.panel3.TabIndex = 79;
		this.GridUnAllocatedItems.AllowEdit = (InheritableBoolean)2;
		this.GridUnAllocatedItems.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridUnAllocatedItems.FilterMode = (FilterMode)2;
		this.GridUnAllocatedItems.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridUnAllocatedItems.GroupByBoxVisible = false;
		this.GridUnAllocatedItems.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems).Location = new System.Drawing.Point(0, 31);
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems).Name = "GridUnAllocatedItems";
		((ScrollableBase)this.GridUnAllocatedItems).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridUnAllocatedItems).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridUnAllocatedItems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems).Size = new System.Drawing.Size(400, 461);
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems).Tag = "UnAllocatedGrid";
		this.GridUnAllocatedItems.TotalRow = (InheritableBoolean)1;
		this.GridUnAllocatedItems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridUnAllocatedItems).VisualStyle = (VisualStyle)4;
		this.BtnAllocateItems.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnAllocateItems.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnAllocateItems.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnAllocateItems.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnAllocateItems.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnAllocateItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAllocateItems.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.BtnAllocateItems.Location = new System.Drawing.Point(371, 3);
		this.BtnAllocateItems.Margin = new System.Windows.Forms.Padding(0);
		this.BtnAllocateItems.Name = "BtnAllocateItems";
		this.BtnAllocateItems.Size = new System.Drawing.Size(25, 25);
		this.BtnAllocateItems.TabIndex = 1;
		this.BtnAllocateItems.UseVisualStyleBackColor = true;
		this.BtnAllocateItems.Click += new System.EventHandler(BtnAllocateItems_Click);
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Dock = System.Windows.Forms.DockStyle.Top;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(0, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(400, 31);
		this.label4.TabIndex = 3;
		this.label4.Text = "Pending Screens For Allocation";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.label36);
		this.panel7.Controls.Add(this.btnshow);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbModuleName);
		this.panel7.Controls.Add(this.label2);
		this.panel7.Controls.Add(this.lbltocompany);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTemplateName);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 57);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(802, 37);
		this.panel7.TabIndex = 0;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.Red;
		this.label36.Location = new System.Drawing.Point(93, 10);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(14, 17);
		this.label36.TabIndex = 573;
		this.label36.Text = "*";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(698, 5);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(61, 26);
		this.btnshow.TabIndex = 3;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.CmbModuleName.AllowNull = (DefaultableBoolean)1;
		this.CmbModuleName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbModuleName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbModuleName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbModuleName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance13;
		((SpecialBoxBase)((UltraGridBase)this.CmbModuleName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance19).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbModuleName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbModuleName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbModuleName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbModuleName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbModuleName).Location = new System.Drawing.Point(463, 5);
		((System.Windows.Forms.Control)(object)this.CmbModuleName).Name = "CmbModuleName";
		((System.Windows.Forms.Control)(object)this.CmbModuleName).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbModuleName).TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(366, 10);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(94, 17);
		this.label2.TabIndex = 73;
		this.label2.Text = "Module Name";
		this.lbltocompany.AutoSize = true;
		this.lbltocompany.BackColor = System.Drawing.Color.White;
		this.lbltocompany.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.lbltocompany.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbltocompany.Location = new System.Drawing.Point(5, 11);
		this.lbltocompany.Name = "lbltocompany";
		this.lbltocompany.Size = new System.Drawing.Size(91, 15);
		this.lbltocompany.TabIndex = 16;
		this.lbltocompany.Text = "Template Name";
		this.CmbTemplateName.AllowNull = (DefaultableBoolean)1;
		this.CmbTemplateName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTemplateName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTemplateName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTemplateName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbTemplateName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance18).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTemplateName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTemplateName).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTemplateName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTemplateName).Location = new System.Drawing.Point(108, 5);
		((System.Windows.Forms.Control)(object)this.CmbTemplateName).Name = "CmbTemplateName";
		((System.Windows.Forms.Control)(object)this.CmbTemplateName).Size = new System.Drawing.Size(253, 26);
		((System.Windows.Forms.Control)(object)this.CmbTemplateName).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbTemplateName).Leave += new System.EventHandler(CmbItemCutomGroup_TextChanged);
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 25);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(802, 32);
		this.panel6.TabIndex = 45;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(9, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(330, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Screen Allocation To Comany Template";
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(802, 25);
		this.panel2.TabIndex = 0;
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.toolStripSeparator5, this.btnRefresh, this.toolStripSeparator4, this.btnshortcutkeys, this.BtnTemplateName, this.toolStripSeparator1, this.toolStripSeparator6, this.BtnfrmAllScreensInProject });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(802, 27);
		this.tsItemType.TabIndex = 2;
		this.tsItemType.Text = "toolStrip4";
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.BtnTemplateName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnTemplateName.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnTemplateName.Name = "BtnTemplateName";
		this.BtnTemplateName.Size = new System.Drawing.Size(112, 24);
		this.BtnTemplateName.Text = "Template Name";
		this.BtnTemplateName.Click += new System.EventHandler(BtnTemplateName_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
		this.BtnfrmAllScreensInProject.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnfrmAllScreensInProject.Image = Architecture.WinApp.Properties.Resources.Export_Reports;
		this.BtnfrmAllScreensInProject.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnfrmAllScreensInProject.Name = "BtnfrmAllScreensInProject";
		this.BtnfrmAllScreensInProject.Size = new System.Drawing.Size(174, 24);
		this.BtnfrmAllScreensInProject.Text = "All Screens In Project";
		this.BtnfrmAllScreensInProject.Click += new System.EventHandler(BtnfrmAllScreensInProject_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(802, 586);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "AllocationOrganizationTemplateRights";
		this.Text = "Eccountbook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ItemAllocationToCustomGroup_FormClosing);
		base.Load += new System.EventHandler(frmItemsCustomizedGroups_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmItemsCustomizedGroups_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridAllocatedItems).EndInit();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocatedItems).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbModuleName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTemplateName).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		base.ResumeLayout(false);
	}
}
