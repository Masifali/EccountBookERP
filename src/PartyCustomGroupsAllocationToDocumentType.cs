using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
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
namespace Architecture.WinApp.Account_Definition
{

public class PartyCustomGroupsAllocationToDocumentType : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private IContainer components;

	private ToolStrip tsItemType;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnDefineItemCustomGroup;

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

	private Panel panel10;

	internal GridEX grdItemName;

	private Label label7;

	private ToolStripButton btnshortcutkeys;

	private Panel panel8;

	private Panel panel6;

	public PartyCustomGroupsAllocationToDocumentType(UserAccount pUserAccount)
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
			base.AutoScrollMargin = new Size(5, 5);
			base.AutoScrollMinSize = new Size(1200, 500);
			DocumentTypeGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DocumentTypeGridFill()
	{
		try
		{
			DataTable dtDocType = DocumentType.SpecificDocumentTypes();
			if (dtDocType.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("Code", typeof(string));
				dt.Columns.Add("Description", typeof(string));
				dt.Columns.Add("DescriptionOtherLing", typeof(string));
				dt.Columns.Add("ScreenName", typeof(string));
				dt.Columns.Add("TargetUrl", typeof(string));
				dt.Columns.Add("Remarks", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUserName", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUserName", typeof(string));
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("ApprovedUserName", typeof(string));
				foreach (DataRow row in dtDocType.Rows)
				{
					dt.Rows.Add(row["Id"], row["DocumentTypeCode"], row["DocumentTypeDescription"], row["DocumentTypeDescriptionOtherLing"], row["ScreenName"], row["TargetUrl"], row["Remarks"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["ApprovedDate"], row["ApprovedUserName"]);
				}
				grdItemName.DataSource = dt;
				grdItemName.RetrieveStructure();
				GridItemsSetting();
			}
			else
			{
				grdItemName.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridItemsSetting()
	{
		try
		{
			grdItemName.AllowEdit = (InheritableBoolean)2;
			grdItemName.RootTable.Columns["Id"].Visible = false;
			grdItemName.RootTable.Columns["DescriptionOtherLing"].Visible = false;
			grdItemName.RootTable.Columns["ScreenName"].Visible = false;
			grdItemName.RootTable.Columns["TargetUrl"].Visible = false;
			grdItemName.RootTable.Columns["Remarks"].Visible = false;
			grdItemName.RootTable.Columns["EntryDate"].Visible = false;
			grdItemName.RootTable.Columns["EntryUserName"].Visible = false;
			grdItemName.RootTable.Columns["ModifyDate"].Visible = false;
			grdItemName.RootTable.Columns["ModifyUserName"].Visible = false;
			grdItemName.RootTable.Columns["ApprovedDate"].Visible = false;
			grdItemName.RootTable.Columns["ApprovedUserName"].Visible = false;
			grdItemName.RootTable.HeaderLines = 1;
			grdItemName.RootTable.Columns["Code"].Width = 90;
			grdItemName.RootTable.Columns["Description"].Width = 340;
			grdItemName.RootTable.Columns["Code"].HeaderAlignment = (TextAlignment)2;
			grdItemName.RootTable.Columns["Description"].HeaderAlignment = (TextAlignment)2;
			grdItemName.ColumnAutoResize = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdItemName_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdItemName.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				string Description = Conversion.ToString(r.Cells["Description"].Value);
				UnAllocatedCustomGroupGridFill(Id);
				AllocatedCustomGroupGridFill(Id);
				labelUnAllocateGridTemplateII.Text = "UnAllocated Custom Group Of DocumentType : '" + Description + "'";
				labelAllocateGridTemplateII.Text = "Allocated Custom Group Of DocumentType : '" + Description + "'";
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

	private void UnAllocatedCustomGroupGridFill(int DocumentTypeId)
	{
		try
		{
			DataTable dtUnAllocat = PartyCustomGroupsAllocateToDocumentType.PartyCustomGroupsAllocateToDocumentType_UnAllocatedData(UserAccount.OrganizationId, UserAccount.CompanyId, DocumentTypeId);
			if (dtUnAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("DocumentTypeId");
				dt.Columns.Add("CustomGroupId");
				dt.Columns.Add("CustomGroup");
				for (int i = 0; i < dtUnAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtUnAllocat.Rows[i]["Id"], DocumentTypeId, dtUnAllocat.Rows[i]["CustomGroupId"], dtUnAllocat.Rows[i]["CustomGroup"]);
				}
				GridUnAllocatedTemplateII.DataSource = dt;
				GridUnAllocatedTemplateII.RetrieveStructure();
				GridUnAllocatedCustomGroupSetting();
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

	private void GridUnAllocatedCustomGroupSetting()
	{
		try
		{
			GridUnAllocatedTemplateII.AllowEdit = (InheritableBoolean)2;
			GridUnAllocatedTemplateII.RootTable.Columns["Id"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.Columns["DocumentTypeId"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.Columns["CustomGroupId"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.HeaderLines = 1;
			GridUnAllocatedTemplateII.RootTable.Columns["CustomGroup"].Width = 300;
			GridUnAllocatedTemplateII.RootTable.Columns["CustomGroup"].HeaderAlignment = (TextAlignment)2;
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

	private void AllocatedCustomGroupGridFill(int DocumentTypeId)
	{
		try
		{
			DataTable dtAllocat = PartyCustomGroupsAllocateToDocumentType.PartyCustomGroupsAllocateToDocumentType_AllocatedData(UserAccount.OrganizationId, UserAccount.CompanyId, DocumentTypeId);
			if (dtAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("DocumentTypeId");
				dt.Columns.Add("CustomGroupId");
				dt.Columns.Add("CustomGroup");
				for (int i = 0; i < dtAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtAllocat.Rows[i]["Id"], DocumentTypeId, dtAllocat.Rows[i]["CustomGroupId"], dtAllocat.Rows[i]["CustomGroup"]);
				}
				GridAllocatedTemplateII.DataSource = dt;
				GridAllocatedTemplateII.RetrieveStructure();
				GridAllocatedCustomGroupSetting();
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

	private void GridAllocatedCustomGroupSetting()
	{
		try
		{
			GridAllocatedTemplateII.AllowEdit = (InheritableBoolean)2;
			GridAllocatedTemplateII.RootTable.Columns["Id"].Visible = false;
			GridAllocatedTemplateII.RootTable.Columns["DocumentTypeId"].Visible = false;
			GridAllocatedTemplateII.RootTable.Columns["CustomGroupId"].Visible = false;
			GridAllocatedTemplateII.RootTable.HeaderLines = 1;
			GridAllocatedTemplateII.RootTable.Columns["CustomGroup"].Width = 300;
			GridAllocatedTemplateII.RootTable.Columns["CustomGroup"].HeaderAlignment = (TextAlignment)2;
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

	private void BtnSaveTemplateII_Click(object sender, EventArgs e)
	{
		try
		{
			InsertTemplateII(GridUnAllocatedTemplateII, "Allocate");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void InsertTemplateII(GridEX grd, string Message)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		try
		{
			if (grd.GetCheckedRows().Length == 0)
			{
				throw new Exception("Check Row's Which You want To " + Message);
			}
			if (MessageBox.Show("Are you sure to " + Message + " CustomGroup?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				PartyCustomGroupsAllocateToDocumentType obj = new PartyCustomGroupsAllocateToDocumentType();
				obj.PartyCustomGroupsAllocateToDocumentTypeList = new List<PartyCustomGroupsAllocateToDocumentType>();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					PartyCustomGroupsAllocateToDocumentType item = new PartyCustomGroupsAllocateToDocumentType();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.Id = Conversion.ToInt(r.Cells["Id"].Value);
					item.DocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
					item.PartyCustomGroupsId = Conversion.ToInt(r.Cells["CustomGroupId"].Value);
					item.EntryUserId = UserAccount.ID;
					item.ModifyUserId = UserAccount.ID;
					obj.PartyCustomGroupsAllocateToDocumentTypeList.Add(item);
				}
				PartyCustomGroupsAllocateToDocumentType.Save(obj);
				MessageBox.Show("Row's " + Message + " Successfully");
				grdItemName_SelectionChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnDeleteTemplateII_Click(object sender, EventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (GridAllocatedTemplateII.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked Row's first To Un-Allocate CustomGroup");
			}
			GridEXRow r = grdItemName.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (MessageBox.Show("Are you sure to UnAllocate Custom Group?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
				{
					string ids = null;
					GridEXRow[] checkedRows = GridAllocatedTemplateII.GetCheckedRows();
					foreach (GridEXRow item in checkedRows)
					{
						ids = ids + Conversion.ToString(item.Cells["Id"].Value) + ",";
					}
					PartyCustomGroupsAllocateToDocumentType.DeleteByIDs(UserAccount.OrganizationId, UserAccount.CompanyId, ids);
					MessageBox.Show("CustomGroup's UnAllocated Successfully");
					grdItemName_SelectionChanged(null, null);
				}
				return;
			}
			throw new Exception("Select a Row in DocumentType Grid First To Un-Allocate Custom Group");
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
			DocumentTypeGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PartyCustomGroupsAllocationToDocumentType_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnDefineItemCustomGroup_Click(object sender, EventArgs e)
	{
		try
		{
			new frmPartyCustomGroup(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
			((Control)(object)grdItemName).Focus();
		}
		if (e.Control && e.KeyCode == Keys.U)
		{
			BtnSaveTemplateII_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.A)
		{
			BtnSaveTemplateII_Click(null, null);
		}
		if (e.Control && (e.KeyCode == Keys.D || e.KeyCode == Keys.Delete))
		{
			btnDeleteTemplateII_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.Up)
		{
			((Control)(object)grdItemName).Focus();
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
			if (tabControl1.TabPages.ContainsKey("tabItemToParty"))
			{
				dt.Rows.Add("Ctrl+F5", "For Focus on PartyName");
			}
			else
			{
				dt.Rows.Add("Ctrl+F5", "For Focus on Item Grid");
			}
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
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.PartyCustomGroupsAllocationToDocumentType));
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnDefineItemCustomGroup = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPartyToItem = new System.Windows.Forms.TabPage();
		this.PanelMainTemplateII = new System.Windows.Forms.Panel();
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
		this.grdItemName = new GridEX();
		this.label7 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tsItemType.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPartyToItem.SuspendLayout();
		this.PanelMainTemplateII.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridAllocatedTemplateII).BeginInit();
		this.panel8.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocatedTemplateII).BeginInit();
		this.panel6.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdItemName).BeginInit();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnRefresh, this.btnDefineItemCustomGroup, this.btnshortcutkeys });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(1184, 27);
		this.tsItemType.TabIndex = 2;
		this.tsItemType.Text = "toolStrip4";
		this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnDefineItemCustomGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnDefineItemCustomGroup.Image = (System.Drawing.Image)resources.GetObject("btnDefineItemCustomGroup.Image");
		this.btnDefineItemCustomGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefineItemCustomGroup.Name = "btnDefineItemCustomGroup";
		this.btnDefineItemCustomGroup.Size = new System.Drawing.Size(189, 24);
		this.btnDefineItemCustomGroup.Text = "Define Party Custom Group";
		this.btnDefineItemCustomGroup.Click += new System.EventHandler(btnDefineItemCustomGroup_Click);
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
		this.tabControl1.Size = new System.Drawing.Size(1184, 431);
		this.tabControl1.TabIndex = 1;
		this.tabPartyToItem.Controls.Add(this.PanelMainTemplateII);
		this.tabPartyToItem.Location = new System.Drawing.Point(4, 25);
		this.tabPartyToItem.Name = "tabPartyToItem";
		this.tabPartyToItem.Padding = new System.Windows.Forms.Padding(3);
		this.tabPartyToItem.Size = new System.Drawing.Size(1176, 402);
		this.tabPartyToItem.TabIndex = 1;
		this.tabPartyToItem.Text = "Party Custom Group Allocatation To DocumentType";
		this.tabPartyToItem.UseVisualStyleBackColor = true;
		this.PanelMainTemplateII.Controls.Add(this.panel1);
		this.PanelMainTemplateII.Controls.Add(this.panel5);
		this.PanelMainTemplateII.Controls.Add(this.panel10);
		this.PanelMainTemplateII.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelMainTemplateII.Location = new System.Drawing.Point(3, 3);
		this.PanelMainTemplateII.Name = "PanelMainTemplateII";
		this.PanelMainTemplateII.Size = new System.Drawing.Size(1170, 396);
		this.PanelMainTemplateII.TabIndex = 1;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateII);
		this.panel1.Controls.Add(this.panel8);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(800, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(370, 396);
		this.panel1.TabIndex = 80;
		this.GridAllocatedTemplateII.AllowEdit = (InheritableBoolean)2;
		this.GridAllocatedTemplateII.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateII).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridAllocatedTemplateII.FilterMode = (FilterMode)2;
		this.GridAllocatedTemplateII.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateII).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridAllocatedTemplateII.GroupByBoxVisible = false;
		this.GridAllocatedTemplateII.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateII).Location = new System.Drawing.Point(0, 39);
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateII).Name = "GridAllocatedTemplateII";
		((ScrollableBase)this.GridAllocatedTemplateII).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridAllocatedTemplateII).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridAllocatedTemplateII).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateII).Size = new System.Drawing.Size(370, 357);
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateII).TabIndex = 64;
		this.GridAllocatedTemplateII.TotalRow = (InheritableBoolean)1;
		this.GridAllocatedTemplateII.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridAllocatedTemplateII).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.btnDeleteTemplateII);
		this.panel8.Controls.Add(this.labelAllocateGridTemplateII);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(370, 39);
		this.panel8.TabIndex = 69;
		this.btnDeleteTemplateII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnDeleteTemplateII.BackColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDeleteTemplateII.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDeleteTemplateII.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDeleteTemplateII.Location = new System.Drawing.Point(333, 5);
		this.btnDeleteTemplateII.Margin = new System.Windows.Forms.Padding(0);
		this.btnDeleteTemplateII.Name = "btnDeleteTemplateII";
		this.btnDeleteTemplateII.Size = new System.Drawing.Size(33, 30);
		this.btnDeleteTemplateII.TabIndex = 5;
		this.btnDeleteTemplateII.UseVisualStyleBackColor = false;
		this.btnDeleteTemplateII.Click += new System.EventHandler(btnDeleteTemplateII_Click);
		this.labelAllocateGridTemplateII.BackColor = System.Drawing.Color.Teal;
		this.labelAllocateGridTemplateII.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.labelAllocateGridTemplateII.ForeColor = System.Drawing.Color.White;
		this.labelAllocateGridTemplateII.Location = new System.Drawing.Point(2, 0);
		this.labelAllocateGridTemplateII.Name = "labelAllocateGridTemplateII";
		this.labelAllocateGridTemplateII.Size = new System.Drawing.Size(271, 35);
		this.labelAllocateGridTemplateII.TabIndex = 4;
		this.labelAllocateGridTemplateII.Text = "Allocated Custom Group Of DocumentType : Purchase Invoice";
		this.labelAllocateGridTemplateII.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateII);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(430, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(370, 396);
		this.panel5.TabIndex = 79;
		this.GridUnAllocatedTemplateII.AllowEdit = (InheritableBoolean)2;
		this.GridUnAllocatedTemplateII.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateII).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridUnAllocatedTemplateII.FilterMode = (FilterMode)2;
		this.GridUnAllocatedTemplateII.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateII).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridUnAllocatedTemplateII.GroupByBoxVisible = false;
		this.GridUnAllocatedTemplateII.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateII).Location = new System.Drawing.Point(0, 39);
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateII).Name = "GridUnAllocatedTemplateII";
		((ScrollableBase)this.GridUnAllocatedTemplateII).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridUnAllocatedTemplateII).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridUnAllocatedTemplateII).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateII).Size = new System.Drawing.Size(370, 357);
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
		this.panel6.Size = new System.Drawing.Size(370, 39);
		this.panel6.TabIndex = 68;
		this.BtnSaveTemplateII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnSaveTemplateII.BackColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnSaveTemplateII.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSaveTemplateII.Image = Architecture.WinApp.Properties.Resources.Forward;
		this.BtnSaveTemplateII.Location = new System.Drawing.Point(333, 5);
		this.BtnSaveTemplateII.Margin = new System.Windows.Forms.Padding(0);
		this.BtnSaveTemplateII.Name = "BtnSaveTemplateII";
		this.BtnSaveTemplateII.Size = new System.Drawing.Size(33, 30);
		this.BtnSaveTemplateII.TabIndex = 66;
		this.BtnSaveTemplateII.UseVisualStyleBackColor = false;
		this.BtnSaveTemplateII.Click += new System.EventHandler(BtnSaveTemplateII_Click);
		this.labelUnAllocateGridTemplateII.BackColor = System.Drawing.Color.Teal;
		this.labelUnAllocateGridTemplateII.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.labelUnAllocateGridTemplateII.ForeColor = System.Drawing.Color.White;
		this.labelUnAllocateGridTemplateII.Location = new System.Drawing.Point(6, 0);
		this.labelUnAllocateGridTemplateII.Name = "labelUnAllocateGridTemplateII";
		this.labelUnAllocateGridTemplateII.Size = new System.Drawing.Size(271, 35);
		this.labelUnAllocateGridTemplateII.TabIndex = 3;
		this.labelUnAllocateGridTemplateII.Text = "UnAllocated Custom Group Of DocumentType : Purchase Invoice";
		this.labelUnAllocateGridTemplateII.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdItemName);
		this.panel10.Controls.Add(this.label7);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(430, 396);
		this.panel10.TabIndex = 81;
		this.grdItemName.AllowEdit = (InheritableBoolean)2;
		this.grdItemName.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdItemName).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdItemName.FilterMode = (FilterMode)2;
		this.grdItemName.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdItemName).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdItemName.GroupByBoxVisible = false;
		this.grdItemName.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdItemName).Location = new System.Drawing.Point(0, 39);
		((System.Windows.Forms.Control)(object)this.grdItemName).Name = "grdItemName";
		((ScrollableBase)this.grdItemName).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdItemName).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdItemName).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdItemName).Size = new System.Drawing.Size(430, 357);
		((System.Windows.Forms.Control)(object)this.grdItemName).TabIndex = 67;
		((System.Windows.Forms.Control)(object)this.grdItemName).Tag = "UnAllocatedGrid";
		this.grdItemName.TotalRow = (InheritableBoolean)1;
		this.grdItemName.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdItemName).VisualStyle = (VisualStyle)4;
		this.grdItemName.SelectionChanged += new System.EventHandler(grdItemName_SelectionChanged);
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Dock = System.Windows.Forms.DockStyle.Top;
		this.label7.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(0, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(430, 39);
		this.label7.TabIndex = 3;
		this.label7.Text = "Document Type";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1184, 30);
		this.panel2.TabIndex = 3;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 461);
		base.Controls.Add(this.tabControl1);
		base.Controls.Add(this.panel2);
		base.KeyPreview = true;
		base.Name = "PartyCustomGroupsAllocationToDocumentType";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PartyCustomGroupsAllocationToDocumentType_FormClosing);
		base.Load += new System.EventHandler(frmItemsCustomizedGroups_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmItemsCustomizedGroups_KeyDown);
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabPartyToItem.ResumeLayout(false);
		this.PanelMainTemplateII.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridAllocatedTemplateII).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocatedTemplateII).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdItemName).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
