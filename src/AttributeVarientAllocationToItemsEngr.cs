using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
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
namespace Architecture.WinApp.Inventory_Definition
{

public class AttributeVarientAllocationToItemsEngr : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtitem = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip tsItemType;

	private ToolStripButton btnRefresh;

	internal UltraCombo CmbItem;

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

	private Button BtnShow;

	internal UltraCombo CmbCategory;

	private Label label64;

	private Label label6;

	internal UltraCombo CmbModalSample;

	public AttributeVarientAllocationToItemsEngr(UserAccount pUserAccount)
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
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 750);
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			dtitem.Columns.Add("ItemCode");
			dtitem.Columns.Add("SampleNo");
			dtitem.Columns.Add("ItemCategoryId", typeof(int));
			GetItemCategory();
			ItemdtFill();
			ItemDetailBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetItemCategory()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int SupId = Conversion.ToInt(CmbCategory.Value);
			DataTable dtItemCategory = ItemCategory.Getall(new ItemCategory
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtItemCategory.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItemCategory, CmbCategory, "Id", "CategoryDescription", "ItemCategory", false);
				if (SupId > 0)
				{
					if (dtItemCategory.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == SupId))
					{
						CmbCategory.Value = SupId;
					}
					else
					{
						((Control)(object)CmbCategory).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCategory).Text = string.Empty;
				((UltraGridBase)CmbCategory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemdtFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = Item.AllItemsBindForEngrWithModalOnly(new Item
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCodeNew"], dt.Rows[i]["SampleNo"], dt.Rows[i]["ItemCategoryId"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCategory_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemDetailBind(Conversion.ToInt(CmbCategory.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailBind(int CategoryId = 0)
	{
		try
		{
			int Id = Conversion.ToInt(CmbItem.Value);
			int Id2 = Conversion.ToInt(CmbModalSample.Value);
			DataTable FilterItems = new DataTable();
			if (CategoryId > 0)
			{
				DataRow[] filteredRows = dtitem.Select($"ItemCategoryId = {CategoryId}");
				FilterItems = (filteredRows.Any() ? filteredRows.CopyToDataTable() : new DataTable());
			}
			else
			{
				FilterItems = dtitem;
			}
			if (FilterItems.Rows.Count > 0)
			{
				DDL.BindDDL(FilterItems, CmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				DDL.BindDDLNew(FilterItems, CmbModalSample, "Id", "SampleNo", "Sample No", true);
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)CmbModalSample).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (Id > 0)
				{
					if (FilterItems.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbItem.Value = Id;
					}
					else
					{
						((Control)(object)CmbItem).Text = string.Empty;
					}
				}
				if (Id2 > 0)
				{
					if (FilterItems.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id2))
					{
						CmbModalSample.Value = Id;
					}
					else
					{
						((Control)(object)CmbModalSample).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbItem).Text = string.Empty;
				((UltraGridBase)CmbItem).DataSource = null;
				((Control)(object)CmbModalSample).Text = string.Empty;
				((UltraGridBase)CmbModalSample).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbModalSample_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(CmbModalSample.Value);
			if (((UltraGridBase)CmbModalSample).ActiveRow != null && Id > 0)
			{
				CmbItem.Value = Id;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbItem_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(CmbItem.Value);
			if (((UltraGridBase)CmbItem).ActiveRow != null && Id > 0)
			{
				CmbModalSample.Value = Id;
			}
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
		if (e.Control && e.KeyCode == Keys.F5)
		{
			CmbItem.Focus();
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

	private void BtnShow_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbItem.Value) > 0)
			{
				UnAllocatedItemsGridFill();
				AllocatedItemsGridFill();
				return;
			}
			CmbItem.Focus();
			throw new Exception("Select Item First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UnAllocatedItemsGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		try
		{
			DataTable dtUnAllocat = ItemAttributeVarientAllocationtoItem.GetAllocatedAndUnAllocatedAttributeVarient(new ItemAttributeVarientAllocationtoItem
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				ItemId = Conversion.ToInt(CmbItem.Value),
				Id = 1
			});
			if (dtUnAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemAttributeVarientId");
				dt.Columns.Add("ItemAttributeVarient");
				for (int i = 0; i < dtUnAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtUnAllocat.Rows[i]["ItemAttributeVarientId"], dtUnAllocat.Rows[i]["ItemAttributeVarient"]);
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
			GridUnAllocatedItems.RootTable.Columns["ItemAttributeVarientId"].Visible = false;
			GridUnAllocatedItems.RootTable.Columns["ItemAttributeVarientId"].Width = 340;
			CommonServices.GridAutoAdjustmentNew(GridUnAllocatedItems);
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		try
		{
			DataTable dtAllocat = ItemAttributeVarientAllocationtoItem.GetAllocatedAndUnAllocatedAttributeVarient(new ItemAttributeVarientAllocationtoItem
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				ItemId = Conversion.ToInt(CmbItem.Value),
				Id = 2
			});
			if (dtAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemAttributeVarientId");
				dt.Columns.Add("ItemAttributeVarient");
				for (int i = 0; i < dtAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtAllocat.Rows[i]["ItemAttributeVarientId"], dtAllocat.Rows[i]["ItemAttributeVarient"]);
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
			GridAllocatedItems.RootTable.Columns["ItemAttributeVarientId"].Visible = false;
			GridAllocatedItems.RootTable.Columns["ItemAttributeVarient"].Width = 340;
			CommonServices.GridAutoAdjustmentNew(GridAllocatedItems);
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
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbItem.Value) == 0)
			{
				MessageBox.Show("Select Item Name First");
				CmbItem.Focus();
				return;
			}
			if (GridUnAllocatedItems.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked Row's first To Allocate");
			}
			ItemAttributeVarientAllocationtoItem obj = new ItemAttributeVarientAllocationtoItem();
			obj.ItemAttributeVarientAllocationtoItemList = new List<ItemAttributeVarientAllocationtoItem>();
			GridEXRow[] checkedRows = GridUnAllocatedItems.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				ItemAttributeVarientAllocationtoItem item = new ItemAttributeVarientAllocationtoItem();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.BranchesId = UserAccount.BranchesId;
				item.ItemId = Conversion.ToInt(CmbItem.Value);
				item.ItemAttributeVarientId = Conversion.ToInt(r.Cells["ItemAttributeVarientId"].Value);
				obj.ItemAttributeVarientAllocationtoItemList.Add(item);
			}
			ItemAttributeVarientAllocationtoItem.Save(obj);
			MessageBox.Show("Attribute Variant's Allocated Successfully");
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
			if (GridAllocatedItems.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked Row's first To Un-Allocate");
			}
			if (Conversion.ToInt(CmbItem.Value) > 0)
			{
				if (MessageBox.Show("Are you sure to UnAllocate Attribute Variant's?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
				{
					string Ids = "";
					GridEXRow[] checkedRows = GridAllocatedItems.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						Ids = Ids + "," + Conversion.ToInt(r.Cells["ItemAttributeVarientId"].Value);
					}
					ItemAttributeVarientAllocationtoItem.DeleteByID(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(CmbItem.Value), Ids);
					MessageBox.Show("Item's UnAllocated Successfully");
					UnAllocatedItemsGridFill();
					AllocatedItemsGridFill();
				}
				return;
			}
			throw new Exception("Select Item First To Un-Allocate Item's");
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
			GetItemCategory();
			ItemdtFill();
			ItemDetailBind();
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

	private void btnDefineItemCustomGroup_Click(object sender, EventArgs e)
	{
		try
		{
			new ItemAttributeVarient_Engr(UserAccount).Show();
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
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
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
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Expected O, but got Unknown
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Definition.AttributeVarientAllocationToItemsEngr));
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
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
		this.BtnShow = new System.Windows.Forms.Button();
		this.lbltocompany = new System.Windows.Forms.Label();
		this.CmbItem = new UltraCombo();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnDefineItemCustomGroup = new System.Windows.Forms.ToolStripButton();
		this.CmbCategory = new UltraCombo();
		this.label64 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbModalSample = new UltraCombo();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridAllocatedItems).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocatedItems).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		this.tsItemType.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbModalSample).BeginInit();
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
		this.panel4.Location = new System.Drawing.Point(400, 113);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(400, 473);
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
		((System.Windows.Forms.Control)(object)this.GridAllocatedItems).Size = new System.Drawing.Size(400, 442);
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
		this.btnDeAllocate.TabIndex = 5;
		this.btnDeAllocate.UseVisualStyleBackColor = true;
		this.btnDeAllocate.Click += new System.EventHandler(btnDeAllocate_Click);
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(400, 31);
		this.label1.TabIndex = 4;
		this.label1.Text = "Allocated Attribute Variant ";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems);
		this.panel3.Controls.Add(this.BtnAllocateItems);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 113);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(400, 473);
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
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedItems).Size = new System.Drawing.Size(400, 442);
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
		this.label4.Text = "Pending Attribute Variant Allocation";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCategory);
		this.panel7.Controls.Add(this.label64);
		this.panel7.Controls.Add(this.label6);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbModalSample);
		this.panel7.Controls.Add(this.BtnShow);
		this.panel7.Controls.Add(this.lbltocompany);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 57);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(802, 56);
		this.panel7.TabIndex = 0;
		this.BtnShow.BackColor = System.Drawing.Color.Teal;
		this.BtnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShow.ForeColor = System.Drawing.Color.White;
		this.BtnShow.Location = new System.Drawing.Point(736, 23);
		this.BtnShow.Name = "BtnShow";
		this.BtnShow.Size = new System.Drawing.Size(63, 26);
		this.BtnShow.TabIndex = 3;
		this.BtnShow.Text = "Show";
		this.BtnShow.UseVisualStyleBackColor = false;
		this.BtnShow.Click += new System.EventHandler(BtnShow_Click);
		this.lbltocompany.AutoSize = true;
		this.lbltocompany.BackColor = System.Drawing.Color.White;
		this.lbltocompany.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.lbltocompany.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbltocompany.Location = new System.Drawing.Point(217, 6);
		this.lbltocompany.Name = "lbltocompany";
		this.lbltocompany.Size = new System.Drawing.Size(67, 15);
		this.lbltocompany.TabIndex = 16;
		this.lbltocompany.Text = "Item Name";
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(217, 23);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(307, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 1;
		this.CmbItem.ValueChanged += new System.EventHandler(CmbItem_ValueChanged);
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
		this.label15.Size = new System.Drawing.Size(302, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Attribute Variant Allocation to Item";
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(802, 25);
		this.panel2.TabIndex = 0;
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnRefresh, this.btnDefineItemCustomGroup });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(802, 27);
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
		this.btnDefineItemCustomGroup.Size = new System.Drawing.Size(84, 24);
		this.btnDefineItemCustomGroup.Text = "Define Variant";
		this.btnDefineItemCustomGroup.Click += new System.EventHandler(btnDefineItemCustomGroup_Click);
		this.CmbCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance12;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance18).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Location = new System.Drawing.Point(7, 23);
		((System.Windows.Forms.Control)(object)this.CmbCategory).Name = "CmbCategory";
		((System.Windows.Forms.Control)(object)this.CmbCategory).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbCategory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Leave += new System.EventHandler(CmbCategory_Leave);
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(7, 6);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(82, 15);
		this.label64.TabIndex = 159;
		this.label64.Text = "Item Category";
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(527, 6);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(84, 15);
		this.label6.TabIndex = 158;
		this.label6.Text = "Modal Sample";
		this.CmbModalSample.AllowNull = (DefaultableBoolean)1;
		this.CmbModalSample.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbModalSample.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbModalSample.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbModalSample).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.CmbModalSample).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbModalSample).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbModalSample.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbModalSample).Location = new System.Drawing.Point(527, 23);
		((System.Windows.Forms.Control)(object)this.CmbModalSample).Name = "CmbModalSample";
		((System.Windows.Forms.Control)(object)this.CmbModalSample).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbModalSample).TabIndex = 2;
		this.CmbModalSample.ValueChanged += new System.EventHandler(CmbModalSample_ValueChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(802, 586);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "AttributeVarientAllocationToItemsEngr";
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
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbModalSample).EndInit();
		base.ResumeLayout(false);
	}
}

}
