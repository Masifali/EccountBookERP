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
using Architecture.Model.Main;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory_Definition;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.StoreManagement;

public class ItemAllocationToStoreRack : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip tsItemType;

	private ToolStripButton btnRefresh;

	internal UltraCombo CmbWarehouseRack;

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

	private ToolStripButton btnDefineItemCustomGroup;

	public ItemAllocationToStoreRack(UserAccount pUserAccount)
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
			storeRackFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void storeRackFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbWarehouseRack.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("RackName", typeof(string));
			dt.Columns.Add("invWarehouseId", typeof(int));
			dt.Columns.Add("WarehouseName", typeof(string));
			dt.Clear();
			List<GlobalWarehouseRack> dtAll = clsGlobalVariables.globalWarehouseRacks;
			if (dtAll.Count > 0)
			{
				for (int i = 0; i < dtAll.Count; i++)
				{
					dt.Rows.Add(dtAll[i].Id, dtAll[i].rackName, dtAll[i].invWarehouseId, dtAll[i].WareHouseName);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbWarehouseRack, dt, "Id", "RackName", "Store Rack", AllColumns: true, Id, insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbWarehouseRack).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
		if (e.Control && e.KeyCode == Keys.F5)
		{
			CmbWarehouseRack.Focus();
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
	}

	private void CmbItemCutomGroup_TextChanged(object sender, EventArgs e)
	{
		if (Conversion.ToInt(CmbWarehouseRack.Value) > 0)
		{
			UnAllocatedItemsGridFill();
			AllocatedItemsGridFill();
		}
		else
		{
			MessageBox.Show("Select Store Rack First!");
		}
	}

	private void UnAllocatedItemsGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		try
		{
			DataTable dtUnAllocat = ItemAllocationToStoreRack.GetItemsByRackAllocation(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				StoreRackId = Conversion.ToInt(CmbWarehouseRack.Value),
				ActionId = 1
			});
			if (dtUnAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemId");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("ParentCategory");
				for (int i = 0; i < dtUnAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtUnAllocat.Rows[i]["Id"], dtUnAllocat.Rows[i]["ItemName"], dtUnAllocat.Rows[i]["ParentCategory"]);
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
			GridUnAllocatedItems.RootTable.Columns["ItemId"].Visible = false;
			GridUnAllocatedItems.RootTable.Columns["ItemName"].Width = 230;
			GridUnAllocatedItems.RootTable.Columns["ParentCategory"].Width = 110;
			GridUnAllocatedItems.RootTable.Columns.Add("Select");
			GridUnAllocatedItems.RootTable.Columns["Select"].ActAsSelector = true;
			GridUnAllocatedItems.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridUnAllocatedItems.RootTable.Columns["Select"].Width = 30;
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		try
		{
			DataTable dtAllocat = ItemAllocationToStoreRack.GetItemsByRackAllocation(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				StoreRackId = Conversion.ToInt(CmbWarehouseRack.Value),
				ActionId = 2
			});
			if (dtAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemId");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("IsActive", typeof(bool));
				dt.Columns.Add("ParentCategory");
				for (int i = 0; i < dtAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtAllocat.Rows[i]["Id"], dtAllocat.Rows[i]["ItemName"], dtAllocat.Rows[i]["isActive"], dtAllocat.Rows[i]["ParentCategory"]);
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
			GridAllocatedItems.RootTable.Columns["ItemId"].Visible = false;
			GridAllocatedItems.RootTable.Columns["ItemName"].Width = 230;
			GridAllocatedItems.RootTable.Columns["ParentCategory"].Width = 110;
			GridAllocatedItems.RootTable.Columns["isActive"].Visible = false;
			GridAllocatedItems.RootTable.Columns.Add("Select");
			GridAllocatedItems.RootTable.Columns["Select"].ActAsSelector = true;
			GridAllocatedItems.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridAllocatedItems.RootTable.Columns["Select"].Width = 25;
			GridAllocatedItems.RootTable.Columns["Select"].Position = 0;
			GridEX_Helper.AddButton(GridAllocatedItems, "Action", "", 50);
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
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbWarehouseRack.Value) == 0)
			{
				MessageBox.Show("Select Store Rack First");
				CmbWarehouseRack.Focus();
				return;
			}
			if (GridUnAllocatedItems.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked Row's first To Allocate Items");
			}
			ItemAllocationToStoreRack obj = new ItemAllocationToStoreRack();
			obj.ItemAllocationToStoreRackList = new List<ItemAllocationToStoreRack>();
			GridEXRow[] checkedRows = GridUnAllocatedItems.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				ItemAllocationToStoreRack item = new ItemAllocationToStoreRack();
				item.EntryDate = DateTime.Now;
				item.ModifyDate = DateTime.Now;
				item.EntryUserId = UserAccount.ID;
				item.ModifyUserId = UserAccount.ID;
				item.storeRackId = Conversion.ToInt(CmbWarehouseRack.Value);
				item.itemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				item.isActive = true;
				obj.ItemAllocationToStoreRackList.Add(item);
			}
			ItemAllocationToStoreRack.Save(obj);
			MessageBox.Show("Item's Allocated Successfully");
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
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		try
		{
			if (GridAllocatedItems.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked Row's first To Un-Allocate Items");
			}
			if (Conversion.ToInt(CmbWarehouseRack.Value) > 0)
			{
				if (MessageBox.Show("Are you sure to UnAllocate Item's?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
				{
					ItemAllocationToStoreRack obj = new ItemAllocationToStoreRack();
					obj.ItemAllocationToStoreRackList = new List<ItemAllocationToStoreRack>();
					GridEXRow[] checkedRows = GridAllocatedItems.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						ItemAllocationToStoreRack item = new ItemAllocationToStoreRack();
						item.EntryDate = DateTime.Now;
						item.ModifyDate = DateTime.Now;
						item.EntryUserId = UserAccount.ID;
						item.ModifyUserId = UserAccount.ID;
						item.storeRackId = Conversion.ToInt(CmbWarehouseRack.Value);
						item.itemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						item.isActive = false;
						obj.ItemAllocationToStoreRackList.Add(item);
					}
					ItemAllocationToStoreRack.Save(obj);
					MessageBox.Show("Item's Deactivated Successfully");
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
			List<string> allServices = new List<string> { "WarehouseRacks" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			storeRackFill();
			((Control)(object)CmbWarehouseRack).Text = string.Empty;
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
			frmRackDefine frmRackDefine = new frmRackDefine(UserAccount);
			frmRackDefine.StartPosition = FormStartPosition.CenterScreen;
			frmRackDefine.Show();
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

	private void GridAllocatedItems_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.Row.RowType == 0 && e.Row.Table.Columns.Contains("Action"))
		{
			bool isActive = Convert.ToBoolean(e.Row.Cells["IsActive"].Value);
			e.Row.Cells["Action"].Text = (isActive ? "Inactive" : "Active");
		}
	}

	private void GridAllocatedItems_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		try
		{
			GridEXRow r = GridAllocatedItems.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Action")
			{
				bool flag = Conversion.ToBool(r.Cells["IsActive"].Value);
				string mesg = (flag ? "Inactive" : "Active");
				if (MessageBox.Show("Are you sure to " + mesg + " Item for allocation?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
				{
					ItemAllocationToStoreRack val = new ItemAllocationToStoreRack
					{
						ItemAllocationToStoreRackList = new List<ItemAllocationToStoreRack>()
					};
					ItemAllocationToStoreRack item = new ItemAllocationToStoreRack();
					item.EntryDate = DateTime.Now;
					item.ModifyDate = DateTime.Now;
					item.EntryUserId = UserAccount.ID;
					item.ModifyUserId = UserAccount.ID;
					item.storeRackId = Conversion.ToInt(CmbWarehouseRack.Value);
					item.itemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					item.isActive = !flag;
					val.ItemAllocationToStoreRackList.Add(item);
					ItemAllocationToStoreRack.Save(val);
					MessageBox.Show("Item " + mesg + " Successfully");
					AllocatedItemsGridFill();
				}
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
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Expected O, but got Unknown
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Expected O, but got Unknown
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
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.StoreManagement.ItemAllocationToStoreRack));
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
		this.lbltocompany = new System.Windows.Forms.Label();
		this.CmbWarehouseRack = new UltraCombo();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnDefineItemCustomGroup = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridAllocatedItems).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocatedItems).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseRack).BeginInit();
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
		this.panel1.Size = new System.Drawing.Size(858, 586);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.GridAllocatedItems);
		this.panel4.Controls.Add(this.btnDeAllocate);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(400, 97);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(434, 489);
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
		((System.Windows.Forms.Control)(object)this.GridAllocatedItems).Size = new System.Drawing.Size(434, 458);
		((System.Windows.Forms.Control)(object)this.GridAllocatedItems).TabIndex = 64;
		this.GridAllocatedItems.TotalRow = (InheritableBoolean)1;
		this.GridAllocatedItems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridAllocatedItems).VisualStyle = (VisualStyle)4;
		this.GridAllocatedItems.FormattingRow += new RowLoadEventHandler(GridAllocatedItems_FormattingRow);
		this.GridAllocatedItems.ColumnButtonClick += new ColumnActionEventHandler(GridAllocatedItems_ColumnButtonClick);
		this.btnDeAllocate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnDeAllocate.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDeAllocate.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDeAllocate.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDeAllocate.Location = new System.Drawing.Point(405, 3);
		this.btnDeAllocate.Margin = new System.Windows.Forms.Padding(0);
		this.btnDeAllocate.Name = "btnDeAllocate";
		this.btnDeAllocate.Size = new System.Drawing.Size(25, 25);
		this.btnDeAllocate.TabIndex = 5;
		this.btnDeAllocate.UseVisualStyleBackColor = true;
		this.btnDeAllocate.Click += new System.EventHandler(btnDeAllocate_Click);
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(434, 31);
		this.label1.TabIndex = 4;
		this.label1.Text = "Allocated Items";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems);
		this.panel3.Controls.Add(this.BtnAllocateItems);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 97);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(400, 489);
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
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems).Size = new System.Drawing.Size(400, 458);
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems).TabIndex = 67;
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
		this.BtnAllocateItems.TabIndex = 66;
		this.BtnAllocateItems.UseVisualStyleBackColor = true;
		this.BtnAllocateItems.Click += new System.EventHandler(BtnAllocateItems_Click);
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Dock = System.Windows.Forms.DockStyle.Top;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(0, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(400, 31);
		this.label4.TabIndex = 3;
		this.label4.Text = "Pending Items For Allocation";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.lbltocompany);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseRack);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 57);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(858, 40);
		this.panel7.TabIndex = 59;
		this.lbltocompany.AutoSize = true;
		this.lbltocompany.BackColor = System.Drawing.Color.White;
		this.lbltocompany.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.lbltocompany.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbltocompany.Location = new System.Drawing.Point(8, 12);
		this.lbltocompany.Name = "lbltocompany";
		this.lbltocompany.Size = new System.Drawing.Size(63, 15);
		this.lbltocompany.TabIndex = 16;
		this.lbltocompany.Text = "Store Rack";
		this.CmbWarehouseRack.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseRack.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseRack.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseRack.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseRack).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseRack).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouseRack.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseRack).Location = new System.Drawing.Point(75, 6);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseRack).Name = "CmbWarehouseRack";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseRack).Size = new System.Drawing.Size(264, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseRack).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseRack).TextChanged += new System.EventHandler(CmbItemCutomGroup_TextChanged);
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 25);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(858, 32);
		this.panel6.TabIndex = 45;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(5, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(263, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Items Allocation To Store Rack";
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(858, 25);
		this.panel2.TabIndex = 0;
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnRefresh, this.btnDefineItemCustomGroup });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(858, 27);
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
		this.btnDefineItemCustomGroup.Size = new System.Drawing.Size(129, 24);
		this.btnDefineItemCustomGroup.Text = "Define Store Rack";
		this.btnDefineItemCustomGroup.Click += new System.EventHandler(btnDefineItemCustomGroup_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(858, 586);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "ItemAllocationToStoreRack";
		this.Text = "Item Allocation To Custom Group";
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
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseRack).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		base.ResumeLayout(false);
	}
}
