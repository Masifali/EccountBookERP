using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
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
namespace Architecture.WinApp.StoreManagement
{

public class frmAttributeAllocationToItem : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private IContainer components;

	private ToolStrip tsItemType;

	private ToolStripButton btnReset;

	private TabControl tabControl1;

	private TabPage tabPartyToItem;

	private Panel panel2;

	private Panel PanelMainTemplateII;

	private Panel panel1;

	internal GridEX GridAllocatedTemplateII;

	private Button BtnUpdateTemplateII;

	private Button btnDeleteTemplateII;

	private Label labelAllocateGridTemplateII;

	private Panel panel5;

	internal GridEX GridUnAllocatedTemplateII;

	private Button BtnSaveTemplateII;

	private Label labelUnAllocateGridTemplateII;

	private ToolStripButton BtnItemDefine;

	private Panel panel10;

	internal GridEX grdItemName;

	private Label label7;

	private ToolStripButton btnshortcutkeys;

	private Panel panel8;

	private Panel panel6;

	public frmAttributeAllocationToItem(UserAccount pUserAccount)
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
			base.AutoScrollMinSize = new Size(1020, 500);
			formright = CommonServices.SetRightsValueInRightsObject(Conversion.ToString((object)base.Name));
			BtnSaveTemplateII.Enabled = formright.DoHaveSaveRight;
			btnDeleteTemplateII.Enabled = formright.DoHaveCanDelete;
			ItemsGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ItemsGridFill()
	{
		try
		{
			DataTable dtItem = Item.GetPMItemOnly(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dtItem.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemId");
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("ItemName");
				for (int i = 0; i < dtItem.Rows.Count; i++)
				{
					dt.Rows.Add(dtItem.Rows[i]["Id"], dtItem.Rows[i]["ItemCode"], dtItem.Rows[i]["ItemName"]);
				}
				grdItemName.DataSource = dt;
				grdItemName.RetrieveStructure();
				GridItemsSetting();
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

	private void GridItemsSetting()
	{
		try
		{
			grdItemName.AllowEdit = (InheritableBoolean)1;
			grdItemName.RootTable.Columns["ItemId"].Visible = false;
			grdItemName.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdItemName.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grdItemName.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdItemName.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			grdItemName.RootTable.Columns["ItemCode"].FilterEditType = (FilterEditType)1;
			grdItemName.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			grdItemName.RootTable.HeaderLines = 3;
			grdItemName.RootTable.Columns["ItemCode"].Width = 60;
			grdItemName.RootTable.Columns["ItemName"].Width = 350;
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
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				string Item = Conversion.ToString(r.Cells["ItemName"].Value);
				UnAllocatedAttributesGridFill(ItemId);
				AllocatedAttributesGridFill(ItemId);
				labelUnAllocateGridTemplateII.Text = "UnAllocated Attributes Of Item : '" + Item + "'";
				labelAllocateGridTemplateII.Text = "Allocated Attributes Of Item : '" + Item + "'";
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

	private void UnAllocatedAttributesGridFill(int ItemId)
	{
		try
		{
			DataTable dtUnAllocat = ItemAttribute.ItemAttribute_UnAllocatedData(UserAccount.OrganizationId, UserAccount.CompanyId, ItemId);
			if (dtUnAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ItemId");
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("AttributeId");
				dt.Columns.Add("AttributeName");
				dt.Columns.Add("IsActive");
				for (int i = 0; i < dtUnAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtUnAllocat.Rows[i]["itemAttributeId"], ItemId, dtUnAllocat.Rows[i]["ItemCode"], dtUnAllocat.Rows[i]["ItemName"], dtUnAllocat.Rows[i]["AttributeId"], dtUnAllocat.Rows[i]["attributeName"], dtUnAllocat.Rows[i]["IsActive"]);
				}
				GridUnAllocatedTemplateII.DataSource = dt;
				GridUnAllocatedTemplateII.RetrieveStructure();
				GridUnAllocatedAttributesSetting();
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

	private void GridUnAllocatedAttributesSetting()
	{
		try
		{
			GridUnAllocatedTemplateII.AllowEdit = (InheritableBoolean)1;
			GridUnAllocatedTemplateII.RootTable.Columns["Id"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.Columns["ItemId"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.Columns["AttributeId"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.Columns["ItemCode"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.Columns["ItemName"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.Columns["IsActive"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.Columns["Id"].EditType = (EditType)0;
			GridUnAllocatedTemplateII.RootTable.Columns["ItemId"].EditType = (EditType)0;
			GridUnAllocatedTemplateII.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			GridUnAllocatedTemplateII.RootTable.Columns["ItemName"].EditType = (EditType)0;
			GridUnAllocatedTemplateII.RootTable.Columns["AttributeId"].EditType = (EditType)0;
			GridUnAllocatedTemplateII.RootTable.Columns["AttributeName"].EditType = (EditType)0;
			GridUnAllocatedTemplateII.RootTable.Columns["IsActive"].EditType = (EditType)0;
			GridUnAllocatedTemplateII.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateII.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateII.RootTable.Columns["ItemCode"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateII.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateII.RootTable.Columns["AttributeId"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateII.RootTable.Columns["AttributeName"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateII.RootTable.Columns["IsActive"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateII.RootTable.HeaderLines = 3;
			GridUnAllocatedTemplateII.RootTable.Columns["AttributeName"].Width = 230;
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

	private void AllocatedAttributesGridFill(int ItemId)
	{
		try
		{
			DataTable dtAllocat = ItemAttribute.ItemAttribute_AllocatedData(UserAccount.OrganizationId, UserAccount.CompanyId, ItemId);
			if (dtAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ItemId");
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("AttributeId");
				dt.Columns.Add("AttributeName");
				dt.Columns.Add("IsActive");
				for (int i = 0; i < dtAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtAllocat.Rows[i]["itemAttributeId"], dtAllocat.Rows[i]["ItemId"], dtAllocat.Rows[i]["ItemCode"], dtAllocat.Rows[i]["ItemName"], dtAllocat.Rows[i]["AttributeId"], dtAllocat.Rows[i]["attributeName"], dtAllocat.Rows[i]["IsActive"]);
				}
				GridAllocatedTemplateII.DataSource = dt;
				GridAllocatedTemplateII.RetrieveStructure();
				GridAllocatedAttributesSetting();
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

	private void GridAllocatedAttributesSetting()
	{
		try
		{
			GridAllocatedTemplateII.AllowEdit = (InheritableBoolean)1;
			GridAllocatedTemplateII.RootTable.Columns["Id"].Visible = false;
			GridAllocatedTemplateII.RootTable.Columns["ItemId"].Visible = false;
			GridAllocatedTemplateII.RootTable.Columns["AttributeId"].Visible = false;
			GridAllocatedTemplateII.RootTable.Columns["IsActive"].Visible = false;
			GridAllocatedTemplateII.RootTable.Columns["ItemCode"].Visible = false;
			GridAllocatedTemplateII.RootTable.Columns["ItemName"].Visible = false;
			GridAllocatedTemplateII.RootTable.Columns["Id"].EditType = (EditType)0;
			GridAllocatedTemplateII.RootTable.Columns["ItemId"].EditType = (EditType)0;
			GridAllocatedTemplateII.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			GridAllocatedTemplateII.RootTable.Columns["ItemName"].EditType = (EditType)0;
			GridAllocatedTemplateII.RootTable.Columns["AttributeId"].EditType = (EditType)0;
			GridAllocatedTemplateII.RootTable.Columns["AttributeName"].EditType = (EditType)0;
			GridAllocatedTemplateII.RootTable.Columns["IsActive"].EditType = (EditType)0;
			GridAllocatedTemplateII.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateII.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateII.RootTable.Columns["ItemCode"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateII.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateII.RootTable.Columns["AttributeId"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateII.RootTable.Columns["AttributeName"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateII.RootTable.Columns["IsActive"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateII.RootTable.HeaderLines = 3;
			GridAllocatedTemplateII.RootTable.Columns["AttributeName"].Width = 230;
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
			if (MessageBox.Show("Are you sure to " + Message + " Attributes?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				ItemAttribute obj = new ItemAttribute();
				obj.ItemAttributeList = new List<ItemAttribute>();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ItemAttribute item = new ItemAttribute();
					item.organizationId = UserAccount.OrganizationId;
					item.companyId = UserAccount.CompanyId;
					item.itemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					item.itemAttributeId = Conversion.ToInt(r.Cells["Id"].Value);
					item.attributeId = Conversion.ToInt(r.Cells["AttributeId"].Value);
					item.isActive = true;
					item.EntryUserId = UserAccount.ID;
					item.EntryDate = DateTime.Now;
					item.ModifyDate = DateTime.Now;
					item.ModifyUserId = UserAccount.ID;
					obj.ItemAttributeList.Add(item);
				}
				ItemAttribute.Save(obj);
				MessageBox.Show("Row's " + Message + " Successfully");
				grdItemName_SelectionChanged(null, null);
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
			InsertTemplateII(GridAllocatedTemplateII, "Update");
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
				throw new Exception("Checked Row's first To Un-Allocate Attributes");
			}
			GridEXRow r = grdItemName.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				if (MessageBox.Show("Are you sure to UnAllocate Attributes?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
				{
					string ids = null;
					GridEXRow[] checkedRows = GridAllocatedTemplateII.GetCheckedRows();
					foreach (GridEXRow item in checkedRows)
					{
						ids = ids + Conversion.ToString(item.Cells["AttributeId"].Value) + ",";
					}
					ItemAttribute.DeleteById(UserAccount.OrganizationId, UserAccount.CompanyId, ids, ItemId);
					MessageBox.Show("Item's UnAllocated Successfully");
					grdItemName_SelectionChanged(null, null);
				}
				return;
			}
			throw new Exception("Select a Row in ItemGrid First To Un-Allocate Attributes");
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
			ItemsGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmAttributeAllocationToItem_FormClosing(object sender, FormClosingEventArgs e)
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

	private void BtnItemDefine_Click(object sender, EventArgs e)
	{
		try
		{
			new AddItemStore(UserAccount).Show();
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
			BtnUpdateTemplateII_Click(null, null);
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
			dt.Rows.Add("Ctrl+F5", "For Focus on Item Grid");
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
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.StoreManagement.frmAttributeAllocationToItem));
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnReset = new System.Windows.Forms.ToolStripButton();
		this.BtnItemDefine = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPartyToItem = new System.Windows.Forms.TabPage();
		this.PanelMainTemplateII = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.GridAllocatedTemplateII = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.BtnUpdateTemplateII = new System.Windows.Forms.Button();
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
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnReset, this.BtnItemDefine, this.btnshortcutkeys });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(1004, 27);
		this.tsItemType.TabIndex = 2;
		this.tsItemType.Text = "toolStrip4";
		this.btnReset.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnReset.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(68, 24);
		this.btnReset.Text = "&Reset";
		this.btnReset.Click += new System.EventHandler(btnRefresh_Click);
		this.BtnItemDefine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnItemDefine.Image = (System.Drawing.Image)resources.GetObject("BtnItemDefine.Image");
		this.BtnItemDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnItemDefine.Name = "BtnItemDefine";
		this.BtnItemDefine.Size = new System.Drawing.Size(87, 24);
		this.BtnItemDefine.Text = "Define Item";
		this.BtnItemDefine.Click += new System.EventHandler(BtnItemDefine_Click);
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
		this.tabControl1.Size = new System.Drawing.Size(1004, 431);
		this.tabControl1.TabIndex = 1;
		this.tabPartyToItem.Controls.Add(this.PanelMainTemplateII);
		this.tabPartyToItem.Location = new System.Drawing.Point(4, 25);
		this.tabPartyToItem.Name = "tabPartyToItem";
		this.tabPartyToItem.Padding = new System.Windows.Forms.Padding(3);
		this.tabPartyToItem.Size = new System.Drawing.Size(996, 402);
		this.tabPartyToItem.TabIndex = 1;
		this.tabPartyToItem.Text = "Attribute Allocatation To Item";
		this.tabPartyToItem.UseVisualStyleBackColor = true;
		this.PanelMainTemplateII.Controls.Add(this.panel1);
		this.PanelMainTemplateII.Controls.Add(this.panel5);
		this.PanelMainTemplateII.Controls.Add(this.panel10);
		this.PanelMainTemplateII.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelMainTemplateII.Location = new System.Drawing.Point(3, 3);
		this.PanelMainTemplateII.Name = "PanelMainTemplateII";
		this.PanelMainTemplateII.Size = new System.Drawing.Size(990, 396);
		this.PanelMainTemplateII.TabIndex = 1;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateII);
		this.panel1.Controls.Add(this.panel8);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(710, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(280, 396);
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
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateII).Size = new System.Drawing.Size(280, 364);
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateII).TabIndex = 64;
		this.GridAllocatedTemplateII.TotalRow = (InheritableBoolean)1;
		this.GridAllocatedTemplateII.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridAllocatedTemplateII).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.BtnUpdateTemplateII);
		this.panel8.Controls.Add(this.btnDeleteTemplateII);
		this.panel8.Controls.Add(this.labelAllocateGridTemplateII);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(280, 32);
		this.panel8.TabIndex = 69;
		this.BtnUpdateTemplateII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnUpdateTemplateII.BackColor = System.Drawing.Color.White;
		this.BtnUpdateTemplateII.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnUpdateTemplateII.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnUpdateTemplateII.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnUpdateTemplateII.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnUpdateTemplateII.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateTemplateII.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.BtnUpdateTemplateII.Location = new System.Drawing.Point(206, 2);
		this.BtnUpdateTemplateII.Margin = new System.Windows.Forms.Padding(0);
		this.BtnUpdateTemplateII.Name = "BtnUpdateTemplateII";
		this.BtnUpdateTemplateII.Size = new System.Drawing.Size(33, 27);
		this.BtnUpdateTemplateII.TabIndex = 5;
		this.BtnUpdateTemplateII.UseVisualStyleBackColor = false;
		this.BtnUpdateTemplateII.Visible = false;
		this.BtnUpdateTemplateII.Click += new System.EventHandler(BtnUpdateTemplateII_Click);
		this.btnDeleteTemplateII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnDeleteTemplateII.BackColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDeleteTemplateII.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDeleteTemplateII.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDeleteTemplateII.Location = new System.Drawing.Point(243, 2);
		this.btnDeleteTemplateII.Margin = new System.Windows.Forms.Padding(0);
		this.btnDeleteTemplateII.Name = "btnDeleteTemplateII";
		this.btnDeleteTemplateII.Size = new System.Drawing.Size(33, 27);
		this.btnDeleteTemplateII.TabIndex = 5;
		this.btnDeleteTemplateII.UseVisualStyleBackColor = false;
		this.btnDeleteTemplateII.Click += new System.EventHandler(btnDeleteTemplateII_Click);
		this.labelAllocateGridTemplateII.BackColor = System.Drawing.Color.Teal;
		this.labelAllocateGridTemplateII.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelAllocateGridTemplateII.ForeColor = System.Drawing.Color.White;
		this.labelAllocateGridTemplateII.Location = new System.Drawing.Point(2, 0);
		this.labelAllocateGridTemplateII.Name = "labelAllocateGridTemplateII";
		this.labelAllocateGridTemplateII.Size = new System.Drawing.Size(234, 29);
		this.labelAllocateGridTemplateII.TabIndex = 4;
		this.labelAllocateGridTemplateII.Text = "Allocated Attributes Of Item: ";
		this.labelAllocateGridTemplateII.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateII);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(430, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(280, 396);
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
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateII).Size = new System.Drawing.Size(280, 364);
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
		this.panel6.Size = new System.Drawing.Size(280, 32);
		this.panel6.TabIndex = 68;
		this.BtnSaveTemplateII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnSaveTemplateII.BackColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnSaveTemplateII.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSaveTemplateII.Image = Architecture.WinApp.Properties.Resources.Forward;
		this.BtnSaveTemplateII.Location = new System.Drawing.Point(243, 3);
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
		this.labelUnAllocateGridTemplateII.Size = new System.Drawing.Size(234, 29);
		this.labelUnAllocateGridTemplateII.TabIndex = 3;
		this.labelUnAllocateGridTemplateII.Text = "UnAllocated Attributes Of Item : ";
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
		((System.Windows.Forms.Control)(object)this.grdItemName).Location = new System.Drawing.Point(0, 29);
		((System.Windows.Forms.Control)(object)this.grdItemName).Name = "grdItemName";
		((ScrollableBase)this.grdItemName).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdItemName).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdItemName).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdItemName).Size = new System.Drawing.Size(430, 367);
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
		this.label7.Size = new System.Drawing.Size(430, 29);
		this.label7.TabIndex = 3;
		this.label7.Text = "Item Name";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1004, 30);
		this.panel2.TabIndex = 3;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1004, 461);
		base.Controls.Add(this.tabControl1);
		base.Controls.Add(this.panel2);
		base.KeyPreview = true;
		base.Name = "frmAttributeAllocationToItem";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmAttributeAllocationToItem_FormClosing);
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
