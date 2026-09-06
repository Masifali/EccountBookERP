// using Architecture.WinApp.StoreManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Supplier_Purchase;
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

public class ItemAllocationToSupplierCustomer : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private IContainer components;

	private Panel PanelMainTemplateI;

	private ToolStrip tsItemType;

	private ToolStripButton btnRefresh;

	internal UltraCombo CmbPartyNameTemplateI;

	private Panel panel7;

	private Label lbltocompany;

	private Panel panel3;

	private Label label4;

	private Button BtnSaveTemplateI;

	private Panel panel4;

	private Label label1;

	private Button btnDeleteTemplateI;

	internal GridEX GridUnAllocatedTemplateI;

	internal GridEX GridAllocatedTemplateI;

	private ToolStripButton btnDefineItemCustomGroup;

	private Button BtnUpdateTemplateI;

	private Button BtnShowTemplateI;

	private TabControl tabControl1;

	private TabPage tabItemToParty;

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

	public ItemAllocationToSupplierCustomer(UserAccount pUserAccount)
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
			formright = CommonServices.SetRightsValueInRightsObject(Conversion.ToString((object)base.Name));
			BtnSaveTemplateI.Enabled = formright.DoHaveSaveRight;
			BtnUpdateTemplateI.Enabled = formright.DoHaveUpdateRights;
			btnDeleteTemplateI.Enabled = formright.DoHaveCanDelete;
			BtnSaveTemplateII.Enabled = formright.DoHaveSaveRight;
			BtnUpdateTemplateII.Enabled = formright.DoHaveUpdateRights;
			btnDeleteTemplateII.Enabled = formright.DoHaveCanDelete;
			ItemsGridFill();
			SupplierCustomerFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void SupplierCustomerFill()
	{
		try
		{
			int id = Conversion.ToInt(CmbPartyNameTemplateI.Value);
			DataTable dt = CommonServices.GetSupplierustomerByCustomerGroupId("14");
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, CmbPartyNameTemplateI, "Id", "CompanyName", "SupplierCustomer", true);
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbPartyNameTemplateI.Value = id;
				}
				else
				{
					((Control)(object)CmbPartyNameTemplateI).Text = string.Empty;
				}
			}
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
			int PartyId = Conversion.ToInt(CmbPartyNameTemplateI.Value);
			if (PartyId > 0)
			{
				UnAllocatedItemsGridFill(PartyId);
				AllocatedItemsGridFill(PartyId);
			}
			else
			{
				MessageBox.Show("Select Item SupplierCustomer First");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UnAllocatedItemsGridFill(int PartyId)
	{
		try
		{
			DataTable dtUnAllocat = ItemAllocationToSupplierCustomer.ItemAllocationToSupplierCustomer_UnAllocatedData(UserAccount.OrganizationId, UserAccount.CompanyId, PartyId, 0);
			if (dtUnAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ItemId");
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("SupplierCustomerId");
				dt.Columns.Add("SupplierCustomer");
				dt.Columns.Add("IsActive");
				dt.Columns.Add("LeadTimeDay");
				for (int i = 0; i < dtUnAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtUnAllocat.Rows[i]["Id"], dtUnAllocat.Rows[i]["ItemId"], dtUnAllocat.Rows[i]["ItemCode"], dtUnAllocat.Rows[i]["ItemName"], dtUnAllocat.Rows[i]["SupplierCustomerId"], dtUnAllocat.Rows[i]["SupplierCustomer"], dtUnAllocat.Rows[i]["IsActive"], dtUnAllocat.Rows[i]["LeadTimeDay"]);
				}
				GridUnAllocatedTemplateI.DataSource = dt;
				GridUnAllocatedTemplateI.RetrieveStructure();
				GridUnAllocatedItemsSetting();
			}
			else
			{
				GridUnAllocatedTemplateI.ClearStructure();
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
			GridUnAllocatedTemplateI.AllowEdit = (InheritableBoolean)1;
			GridUnAllocatedTemplateI.RootTable.Columns["Id"].Visible = false;
			GridUnAllocatedTemplateI.RootTable.Columns["ItemId"].Visible = false;
			GridUnAllocatedTemplateI.RootTable.Columns["SupplierCustomerId"].Visible = false;
			GridUnAllocatedTemplateI.RootTable.Columns["SupplierCustomer"].Visible = false;
			GridUnAllocatedTemplateI.RootTable.Columns["IsActive"].Visible = false;
			GridUnAllocatedTemplateI.RootTable.Columns["Id"].EditType = (EditType)0;
			GridUnAllocatedTemplateI.RootTable.Columns["ItemId"].EditType = (EditType)0;
			GridUnAllocatedTemplateI.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			GridUnAllocatedTemplateI.RootTable.Columns["ItemName"].EditType = (EditType)0;
			GridUnAllocatedTemplateI.RootTable.Columns["SupplierCustomerId"].EditType = (EditType)0;
			GridUnAllocatedTemplateI.RootTable.Columns["SupplierCustomer"].EditType = (EditType)0;
			GridUnAllocatedTemplateI.RootTable.Columns["IsActive"].EditType = (EditType)0;
			GridUnAllocatedTemplateI.RootTable.Columns["LeadTimeDay"].EditType = (EditType)1;
			GridUnAllocatedTemplateI.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateI.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateI.RootTable.Columns["ItemCode"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateI.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateI.RootTable.Columns["SupplierCustomerId"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateI.RootTable.Columns["SupplierCustomer"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateI.RootTable.Columns["IsActive"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateI.RootTable.Columns["LeadTimeDay"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateI.RootTable.HeaderLines = 3;
			GridUnAllocatedTemplateI.RootTable.Columns["ItemCode"].Width = 70;
			GridUnAllocatedTemplateI.RootTable.Columns["ItemName"].Width = 420;
			GridUnAllocatedTemplateI.RootTable.Columns["LeadTimeDay"].Width = 40;
			GridUnAllocatedTemplateI.RootTable.Columns["LeadTimeDay"].HeaderAlignment = (TextAlignment)2;
			GridUnAllocatedTemplateI.RootTable.Columns["LeadTimeDay"].TextAlignment = (TextAlignment)3;
			GridUnAllocatedTemplateI.RootTable.Columns["LeadTimeDay"].FormatString = "#,##";
			GridUnAllocatedTemplateI.RootTable.Columns["LeadTimeDay"].TotalFormatString = "#,##";
			GridUnAllocatedTemplateI.RootTable.Columns.Add("Select");
			GridUnAllocatedTemplateI.RootTable.Columns["Select"].ActAsSelector = true;
			GridUnAllocatedTemplateI.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridUnAllocatedTemplateI.RootTable.Columns["Select"].Width = 30;
			GridUnAllocatedTemplateI.RootTable.Columns["Select"].Position = 0;
			GridUnAllocatedTemplateI.ColumnAutoResize = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AllocatedItemsGridFill(int PartyId)
	{
		try
		{
			DataTable dtAllocat = ItemAllocationToSupplierCustomer.ItemAllocationToSupplierCustomer_AllocatedData(UserAccount.OrganizationId, UserAccount.CompanyId, PartyId, 0);
			if (dtAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ItemId");
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("SupplierCustomerId");
				dt.Columns.Add("SupplierCustomer");
				dt.Columns.Add("IsActive");
				dt.Columns.Add("LeadTimeDay");
				for (int i = 0; i < dtAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtAllocat.Rows[i]["Id"], dtAllocat.Rows[i]["ItemId"], dtAllocat.Rows[i]["ItemCode"], dtAllocat.Rows[i]["ItemName"], dtAllocat.Rows[i]["SupplierCustomerId"], dtAllocat.Rows[i]["SupplierCustomer"], dtAllocat.Rows[i]["IsActive"], dtAllocat.Rows[i]["LeadTimeDay"]);
				}
				GridAllocatedTemplateI.DataSource = dt;
				GridAllocatedTemplateI.RetrieveStructure();
				GridAllocatedItemsSetting();
			}
			else
			{
				GridAllocatedTemplateI.ClearStructure();
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
			GridAllocatedTemplateI.AllowEdit = (InheritableBoolean)1;
			GridAllocatedTemplateI.RootTable.Columns["Id"].Visible = false;
			GridAllocatedTemplateI.RootTable.Columns["ItemId"].Visible = false;
			GridAllocatedTemplateI.RootTable.Columns["SupplierCustomerId"].Visible = false;
			GridAllocatedTemplateI.RootTable.Columns["SupplierCustomer"].Visible = false;
			GridAllocatedTemplateI.RootTable.Columns["IsActive"].Visible = false;
			GridAllocatedTemplateI.RootTable.Columns["Id"].EditType = (EditType)0;
			GridAllocatedTemplateI.RootTable.Columns["ItemId"].EditType = (EditType)0;
			GridAllocatedTemplateI.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			GridAllocatedTemplateI.RootTable.Columns["ItemName"].EditType = (EditType)0;
			GridAllocatedTemplateI.RootTable.Columns["SupplierCustomerId"].EditType = (EditType)0;
			GridAllocatedTemplateI.RootTable.Columns["SupplierCustomer"].EditType = (EditType)0;
			GridAllocatedTemplateI.RootTable.Columns["IsActive"].EditType = (EditType)0;
			GridAllocatedTemplateI.RootTable.Columns["LeadTimeDay"].EditType = (EditType)1;
			GridAllocatedTemplateI.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateI.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateI.RootTable.Columns["ItemCode"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateI.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateI.RootTable.Columns["SupplierCustomerId"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateI.RootTable.Columns["SupplierCustomer"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateI.RootTable.Columns["IsActive"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateI.RootTable.Columns["LeadTimeDay"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateI.RootTable.HeaderLines = 3;
			GridAllocatedTemplateI.RootTable.Columns["ItemCode"].Width = 70;
			GridAllocatedTemplateI.RootTable.Columns["ItemName"].Width = 420;
			GridAllocatedTemplateI.RootTable.Columns["LeadTimeDay"].Width = 40;
			GridAllocatedTemplateI.RootTable.Columns["LeadTimeDay"].HeaderAlignment = (TextAlignment)2;
			GridAllocatedTemplateI.RootTable.Columns["LeadTimeDay"].TextAlignment = (TextAlignment)3;
			GridAllocatedTemplateI.RootTable.Columns["LeadTimeDay"].FormatString = "#,##";
			GridAllocatedTemplateI.RootTable.Columns["LeadTimeDay"].TotalFormatString = "#,##";
			GridAllocatedTemplateI.RootTable.Columns.Add("Select");
			GridAllocatedTemplateI.RootTable.Columns["Select"].ActAsSelector = true;
			GridAllocatedTemplateI.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridAllocatedTemplateI.RootTable.Columns["Select"].Width = 30;
			GridAllocatedTemplateI.RootTable.Columns["Select"].Position = 0;
			GridAllocatedTemplateI.ColumnAutoResize = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnSaveTemplateI_Click(object sender, EventArgs e)
	{
		try
		{
			Insert(GridUnAllocatedTemplateI, "Allocate");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert(GridEX grd, string Message)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbPartyNameTemplateI.Value) == 0)
			{
				MessageBox.Show("Select Item SupplierCustomer First");
				CmbPartyNameTemplateI.Focus();
				return;
			}
			if (grd.GetCheckedRows().Length == 0)
			{
				throw new Exception("Check Row's Which You want To " + Message);
			}
			if (MessageBox.Show("Are you sure to " + Message + " Parties?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			ItemAllocationToSupplierCustomer obj = new ItemAllocationToSupplierCustomer();
			obj.ItemAllocationToSupplierCustomerList = new List<ItemAllocationToSupplierCustomer>();
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				ItemAllocationToSupplierCustomer item = new ItemAllocationToSupplierCustomer();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.SupplierCustomerId = Conversion.ToInt(CmbPartyNameTemplateI.Value);
				item.Id = Conversion.ToInt(r.Cells["Id"].Value);
				item.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				item.LeadTimeDay = Conversion.ToInt(r.Cells["LeadTimeDay"].Value);
				if (item.LeadTimeDay == 0 && Message == "Update")
				{
					throw new Exception($"LeadTimeDay Field is Required in Row no : {r.RowIndex + 1}");
				}
				item.IsActive = true;
				item.EntryUserId = UserAccount.ID;
				item.EntryDate = DateTime.Now;
				item.ModifyDate = DateTime.Now;
				item.ModifyUserId = UserAccount.ID;
				obj.ItemAllocationToSupplierCustomerList.Add(item);
			}
			ItemAllocationToSupplierCustomer.Save(obj);
			MessageBox.Show("Row's " + Message + " Successfully");
			BtnShow_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnUpdateTemplateI_Click(object sender, EventArgs e)
	{
		try
		{
			Insert(GridAllocatedTemplateI, "Update");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnDeleteTemplateI_Click(object sender, EventArgs e)
	{
		try
		{
			if (GridAllocatedTemplateI.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked Row's first To Un-Allocate Items");
			}
			if (Conversion.ToInt(CmbPartyNameTemplateI.Value) > 0)
			{
				if (MessageBox.Show("Are you sure to UnAllocate Item's?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
				{
					string ids = null;
					GridEXRow[] checkedRows = GridAllocatedTemplateI.GetCheckedRows();
					foreach (GridEXRow item in checkedRows)
					{
						ids = ids + Conversion.ToString(item.Cells["ItemId"].Value) + ",";
					}
					ItemAllocationToSupplierCustomer.DeleteById(UserAccount.OrganizationId, UserAccount.CompanyId, ids, Conversion.ToInt(CmbPartyNameTemplateI.Value), 0);
					MessageBox.Show("Item's UnAllocated Successfully");
					BtnShow_Click(null, null);
				}
				return;
			}
			throw new Exception("Select SupplierCustomer First To Un-Allocate Item's");
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
				dt.Columns.Add("LeadTimeDay");
				for (int i = 0; i < dtItem.Rows.Count; i++)
				{
					dt.Rows.Add(dtItem.Rows[i]["Id"], dtItem.Rows[i]["ItemCode"], dtItem.Rows[i]["ItemName"], dtItem.Rows[i]["LeadTimeDay"]);
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
			grdItemName.AllowEdit = (InheritableBoolean)1;
			grdItemName.RootTable.Columns["ItemId"].Visible = false;
			grdItemName.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdItemName.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grdItemName.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdItemName.RootTable.Columns["LeadTimeDay"].EditType = (EditType)0;
			grdItemName.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			grdItemName.RootTable.Columns["ItemCode"].FilterEditType = (FilterEditType)1;
			grdItemName.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			grdItemName.RootTable.Columns["LeadTimeDay"].FilterEditType = (FilterEditType)1;
			grdItemName.RootTable.HeaderLines = 3;
			grdItemName.RootTable.Columns["ItemCode"].Width = 60;
			grdItemName.RootTable.Columns["ItemName"].Width = 310;
			grdItemName.RootTable.Columns["LeadTimeDay"].Width = 40;
			grdItemName.RootTable.Columns["LeadTimeDay"].HeaderAlignment = (TextAlignment)2;
			grdItemName.RootTable.Columns["LeadTimeDay"].TextAlignment = (TextAlignment)3;
			grdItemName.RootTable.Columns["LeadTimeDay"].FormatString = "#,##";
			grdItemName.RootTable.Columns["LeadTimeDay"].TotalFormatString = "#,##";
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
				UnAllocatedPartiesGridFill(ItemId);
				AllocatedPartiesGridFill(ItemId);
				labelUnAllocateGridTemplateII.Text = "UnAllocated Parties Of Item : '" + Item + "'";
				labelAllocateGridTemplateII.Text = "Allocated Parties Of Item : '" + Item + "'";
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

	private void UnAllocatedPartiesGridFill(int ItemId)
	{
		try
		{
			DataTable dtUnAllocat = ItemAllocationToSupplierCustomer.ItemAllocationToSupplierCustomer_UnAllocatedData(UserAccount.OrganizationId, UserAccount.CompanyId, 0, ItemId);
			if (dtUnAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ItemId");
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("SupplierCustomerId");
				dt.Columns.Add("SupplierCustomer");
				dt.Columns.Add("IsActive");
				dt.Columns.Add("LeadTimeDay");
				for (int i = 0; i < dtUnAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtUnAllocat.Rows[i]["Id"], ItemId, dtUnAllocat.Rows[i]["ItemCode"], dtUnAllocat.Rows[i]["ItemName"], dtUnAllocat.Rows[i]["SupplierCustomerId"], dtUnAllocat.Rows[i]["SupplierCustomer"], dtUnAllocat.Rows[i]["IsActive"], dtUnAllocat.Rows[i]["LeadTimeDay"]);
				}
				GridUnAllocatedTemplateII.DataSource = dt;
				GridUnAllocatedTemplateII.RetrieveStructure();
				GridUnAllocatedPartiesSetting();
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

	private void GridUnAllocatedPartiesSetting()
	{
		try
		{
			GridUnAllocatedTemplateII.AllowEdit = (InheritableBoolean)1;
			GridUnAllocatedTemplateII.RootTable.Columns["Id"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.Columns["ItemId"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.Columns["SupplierCustomerId"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.Columns["ItemCode"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.Columns["ItemName"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.Columns["IsActive"].Visible = false;
			GridUnAllocatedTemplateII.RootTable.Columns["SupplierCustomer"].Caption = "Party Name";
			GridUnAllocatedTemplateII.RootTable.Columns["Id"].EditType = (EditType)0;
			GridUnAllocatedTemplateII.RootTable.Columns["ItemId"].EditType = (EditType)0;
			GridUnAllocatedTemplateII.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			GridUnAllocatedTemplateII.RootTable.Columns["ItemName"].EditType = (EditType)0;
			GridUnAllocatedTemplateII.RootTable.Columns["SupplierCustomerId"].EditType = (EditType)0;
			GridUnAllocatedTemplateII.RootTable.Columns["SupplierCustomer"].EditType = (EditType)0;
			GridUnAllocatedTemplateII.RootTable.Columns["IsActive"].EditType = (EditType)0;
			GridUnAllocatedTemplateII.RootTable.Columns["LeadTimeDay"].EditType = (EditType)1;
			GridUnAllocatedTemplateII.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateII.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateII.RootTable.Columns["ItemCode"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateII.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateII.RootTable.Columns["SupplierCustomerId"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateII.RootTable.Columns["SupplierCustomer"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateII.RootTable.Columns["IsActive"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateII.RootTable.Columns["LeadTimeDay"].FilterEditType = (FilterEditType)1;
			GridUnAllocatedTemplateII.RootTable.HeaderLines = 3;
			GridUnAllocatedTemplateII.RootTable.Columns["SupplierCustomer"].Width = 280;
			GridUnAllocatedTemplateII.RootTable.Columns["LeadTimeDay"].Width = 40;
			GridUnAllocatedTemplateII.RootTable.Columns["LeadTimeDay"].HeaderAlignment = (TextAlignment)2;
			GridUnAllocatedTemplateII.RootTable.Columns["LeadTimeDay"].TextAlignment = (TextAlignment)3;
			GridUnAllocatedTemplateII.RootTable.Columns["LeadTimeDay"].FormatString = "#,##";
			GridUnAllocatedTemplateII.RootTable.Columns["LeadTimeDay"].TotalFormatString = "#,##";
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

	private void AllocatedPartiesGridFill(int ItemId)
	{
		try
		{
			DataTable dtAllocat = ItemAllocationToSupplierCustomer.ItemAllocationToSupplierCustomer_AllocatedData(UserAccount.OrganizationId, UserAccount.CompanyId, 0, ItemId);
			if (dtAllocat.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ItemId");
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("SupplierCustomerId");
				dt.Columns.Add("SupplierCustomer");
				dt.Columns.Add("IsActive");
				dt.Columns.Add("LeadTimeDay");
				for (int i = 0; i < dtAllocat.Rows.Count; i++)
				{
					dt.Rows.Add(dtAllocat.Rows[i]["Id"], ItemId, dtAllocat.Rows[i]["ItemCode"], dtAllocat.Rows[i]["ItemName"], dtAllocat.Rows[i]["SupplierCustomerId"], dtAllocat.Rows[i]["SupplierCustomer"], dtAllocat.Rows[i]["IsActive"], dtAllocat.Rows[i]["LeadTimeDay"]);
				}
				GridAllocatedTemplateII.DataSource = dt;
				GridAllocatedTemplateII.RetrieveStructure();
				GridAllocatedPartiesSetting();
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

	private void GridAllocatedPartiesSetting()
	{
		try
		{
			GridAllocatedTemplateII.AllowEdit = (InheritableBoolean)1;
			GridAllocatedTemplateII.RootTable.Columns["Id"].Visible = false;
			GridAllocatedTemplateII.RootTable.Columns["ItemId"].Visible = false;
			GridAllocatedTemplateII.RootTable.Columns["SupplierCustomerId"].Visible = false;
			GridAllocatedTemplateII.RootTable.Columns["IsActive"].Visible = false;
			GridAllocatedTemplateII.RootTable.Columns["ItemCode"].Visible = false;
			GridAllocatedTemplateII.RootTable.Columns["ItemName"].Visible = false;
			GridAllocatedTemplateII.RootTable.Columns["SupplierCustomer"].Caption = "Party Name";
			GridAllocatedTemplateII.RootTable.Columns["Id"].EditType = (EditType)0;
			GridAllocatedTemplateII.RootTable.Columns["ItemId"].EditType = (EditType)0;
			GridAllocatedTemplateII.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			GridAllocatedTemplateII.RootTable.Columns["ItemName"].EditType = (EditType)0;
			GridAllocatedTemplateII.RootTable.Columns["SupplierCustomerId"].EditType = (EditType)0;
			GridAllocatedTemplateII.RootTable.Columns["SupplierCustomer"].EditType = (EditType)0;
			GridAllocatedTemplateII.RootTable.Columns["IsActive"].EditType = (EditType)0;
			GridAllocatedTemplateII.RootTable.Columns["LeadTimeDay"].EditType = (EditType)1;
			GridAllocatedTemplateII.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateII.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateII.RootTable.Columns["ItemCode"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateII.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateII.RootTable.Columns["SupplierCustomerId"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateII.RootTable.Columns["SupplierCustomer"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateII.RootTable.Columns["IsActive"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateII.RootTable.Columns["LeadTimeDay"].FilterEditType = (FilterEditType)1;
			GridAllocatedTemplateII.RootTable.HeaderLines = 3;
			GridAllocatedTemplateII.RootTable.Columns["SupplierCustomer"].Width = 280;
			GridAllocatedTemplateII.RootTable.Columns["LeadTimeDay"].Width = 40;
			GridAllocatedTemplateII.RootTable.Columns["LeadTimeDay"].HeaderAlignment = (TextAlignment)2;
			GridAllocatedTemplateII.RootTable.Columns["LeadTimeDay"].TextAlignment = (TextAlignment)3;
			GridAllocatedTemplateII.RootTable.Columns["LeadTimeDay"].FormatString = "#,##";
			GridAllocatedTemplateII.RootTable.Columns["LeadTimeDay"].TotalFormatString = "#,##";
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
			if (MessageBox.Show("Are you sure to " + Message + " Parties?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			ItemAllocationToSupplierCustomer obj = new ItemAllocationToSupplierCustomer();
			obj.ItemAllocationToSupplierCustomerList = new List<ItemAllocationToSupplierCustomer>();
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				ItemAllocationToSupplierCustomer item = new ItemAllocationToSupplierCustomer();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				item.Id = Conversion.ToInt(r.Cells["Id"].Value);
				item.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
				item.LeadTimeDay = Conversion.ToInt(r.Cells["LeadTimeDay"].Value);
				if (item.LeadTimeDay == 0 && Message == "Update")
				{
					throw new Exception($"LeadTimeDay Field is Required in Row no : {r.RowIndex + 1}");
				}
				item.IsActive = true;
				item.EntryUserId = UserAccount.ID;
				item.EntryDate = DateTime.Now;
				item.ModifyDate = DateTime.Now;
				item.ModifyUserId = UserAccount.ID;
				obj.ItemAllocationToSupplierCustomerList.Add(item);
			}
			ItemAllocationToSupplierCustomer.Save(obj);
			MessageBox.Show("Row's " + Message + " Successfully");
			grdItemName_SelectionChanged(null, null);
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
				throw new Exception("Checked Row's first To Un-Allocate Parties");
			}
			GridEXRow r = grdItemName.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				if (MessageBox.Show("Are you sure to UnAllocate Parties?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
				{
					string ids = null;
					GridEXRow[] checkedRows = GridAllocatedTemplateII.GetCheckedRows();
					foreach (GridEXRow item in checkedRows)
					{
						ids = ids + Conversion.ToString(item.Cells["SupplierCustomerId"].Value) + ",";
					}
					ItemAllocationToSupplierCustomer.DeleteById(UserAccount.OrganizationId, UserAccount.CompanyId, ids, 0, ItemId);
					MessageBox.Show("Item's UnAllocated Successfully");
					grdItemName_SelectionChanged(null, null);
				}
				return;
			}
			throw new Exception("Select a Row in ItemGrid First To Un-Allocate Parties");
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
			SupplierCustomerFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemAllocationToSupplierCustomer_FormClosing(object sender, FormClosingEventArgs e)
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
			new supfrmDefineSupplier(UserAccount).Show();
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
		if (tabControl1.TabPages.ContainsKey("tabItemToParty"))
		{
			if (e.Control && e.KeyCode == Keys.F5)
			{
				CmbPartyNameTemplateI.Focus();
			}
			if (e.Control && e.KeyCode == Keys.U)
			{
				BtnUpdateTemplateI_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.A)
			{
				BtnSaveTemplateI_Click(null, null);
			}
			if (e.Control && (e.KeyCode == Keys.D || e.KeyCode == Keys.Delete))
			{
				btnDeleteTemplateI_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				CmbPartyNameTemplateI.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (Conversion.ToString(base.ActiveControl.Tag) == "UnAllocatedGrid")
				{
					((Control)(object)GridAllocatedTemplateI).Focus();
				}
				else
				{
					((Control)(object)GridUnAllocatedTemplateI).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Left)
			{
				((Control)(object)GridUnAllocatedTemplateI).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				((Control)(object)GridAllocatedTemplateI).Focus();
			}
			return;
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
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Definition.ItemAllocationToSupplierCustomer));
		this.PanelMainTemplateI = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.GridAllocatedTemplateI = new GridEX();
		this.BtnUpdateTemplateI = new System.Windows.Forms.Button();
		this.btnDeleteTemplateI = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.GridUnAllocatedTemplateI = new GridEX();
		this.BtnSaveTemplateI = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.lbltocompany = new System.Windows.Forms.Label();
		this.BtnShowTemplateI = new System.Windows.Forms.Button();
		this.CmbPartyNameTemplateI = new UltraCombo();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.BtnItemDefine = new System.Windows.Forms.ToolStripButton();
		this.btnDefineItemCustomGroup = new System.Windows.Forms.ToolStripButton();
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
		this.tabItemToParty = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.PanelMainTemplateI.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridAllocatedTemplateI).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocatedTemplateI).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyNameTemplateI).BeginInit();
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
		this.tabItemToParty.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.PanelMainTemplateI.Controls.Add(this.panel4);
		this.PanelMainTemplateI.Controls.Add(this.panel3);
		this.PanelMainTemplateI.Controls.Add(this.panel7);
		this.PanelMainTemplateI.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelMainTemplateI.Location = new System.Drawing.Point(3, 3);
		this.PanelMainTemplateI.Name = "PanelMainTemplateI";
		this.PanelMainTemplateI.Size = new System.Drawing.Size(1170, 396);
		this.PanelMainTemplateI.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateI);
		this.panel4.Controls.Add(this.BtnUpdateTemplateI);
		this.panel4.Controls.Add(this.btnDeleteTemplateI);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(585, 40);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(585, 356);
		this.panel4.TabIndex = 80;
		this.GridAllocatedTemplateI.AllowEdit = (InheritableBoolean)2;
		this.GridAllocatedTemplateI.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateI).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridAllocatedTemplateI.FilterMode = (FilterMode)2;
		this.GridAllocatedTemplateI.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateI).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridAllocatedTemplateI.GroupByBoxVisible = false;
		this.GridAllocatedTemplateI.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateI).Location = new System.Drawing.Point(0, 31);
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateI).Name = "GridAllocatedTemplateI";
		((ScrollableBase)this.GridAllocatedTemplateI).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridAllocatedTemplateI).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridAllocatedTemplateI).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateI).Size = new System.Drawing.Size(585, 325);
		((System.Windows.Forms.Control)(object)this.GridAllocatedTemplateI).TabIndex = 64;
		this.GridAllocatedTemplateI.TotalRow = (InheritableBoolean)1;
		this.GridAllocatedTemplateI.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridAllocatedTemplateI).VisualStyle = (VisualStyle)4;
		this.BtnUpdateTemplateI.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnUpdateTemplateI.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnUpdateTemplateI.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnUpdateTemplateI.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnUpdateTemplateI.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnUpdateTemplateI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateTemplateI.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.BtnUpdateTemplateI.Location = new System.Drawing.Point(524, 3);
		this.BtnUpdateTemplateI.Margin = new System.Windows.Forms.Padding(0);
		this.BtnUpdateTemplateI.Name = "BtnUpdateTemplateI";
		this.BtnUpdateTemplateI.Size = new System.Drawing.Size(25, 25);
		this.BtnUpdateTemplateI.TabIndex = 5;
		this.BtnUpdateTemplateI.UseVisualStyleBackColor = true;
		this.BtnUpdateTemplateI.Click += new System.EventHandler(BtnUpdateTemplateI_Click);
		this.btnDeleteTemplateI.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnDeleteTemplateI.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDeleteTemplateI.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnDeleteTemplateI.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnDeleteTemplateI.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnDeleteTemplateI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDeleteTemplateI.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDeleteTemplateI.Location = new System.Drawing.Point(556, 3);
		this.btnDeleteTemplateI.Margin = new System.Windows.Forms.Padding(0);
		this.btnDeleteTemplateI.Name = "btnDeleteTemplateI";
		this.btnDeleteTemplateI.Size = new System.Drawing.Size(25, 25);
		this.btnDeleteTemplateI.TabIndex = 5;
		this.btnDeleteTemplateI.UseVisualStyleBackColor = true;
		this.btnDeleteTemplateI.Click += new System.EventHandler(btnDeleteTemplateI_Click);
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(585, 31);
		this.label1.TabIndex = 4;
		this.label1.Text = "Allocated Items";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateI);
		this.panel3.Controls.Add(this.BtnSaveTemplateI);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 40);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(585, 356);
		this.panel3.TabIndex = 79;
		this.GridUnAllocatedTemplateI.AllowEdit = (InheritableBoolean)2;
		this.GridUnAllocatedTemplateI.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateI).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridUnAllocatedTemplateI.FilterMode = (FilterMode)2;
		this.GridUnAllocatedTemplateI.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateI).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridUnAllocatedTemplateI.GroupByBoxVisible = false;
		this.GridUnAllocatedTemplateI.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateI).Location = new System.Drawing.Point(0, 31);
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateI).Name = "GridUnAllocatedTemplateI";
		((ScrollableBase)this.GridUnAllocatedTemplateI).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridUnAllocatedTemplateI).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridUnAllocatedTemplateI).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateI).Size = new System.Drawing.Size(585, 325);
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateI).TabIndex = 67;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedTemplateI).Tag = "UnAllocatedGrid";
		this.GridUnAllocatedTemplateI.TotalRow = (InheritableBoolean)1;
		this.GridUnAllocatedTemplateI.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridUnAllocatedTemplateI).VisualStyle = (VisualStyle)4;
		this.BtnSaveTemplateI.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnSaveTemplateI.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnSaveTemplateI.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnSaveTemplateI.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnSaveTemplateI.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnSaveTemplateI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSaveTemplateI.Image = Architecture.WinApp.Properties.Resources.Forward;
		this.BtnSaveTemplateI.Location = new System.Drawing.Point(556, 3);
		this.BtnSaveTemplateI.Margin = new System.Windows.Forms.Padding(0);
		this.BtnSaveTemplateI.Name = "BtnSaveTemplateI";
		this.BtnSaveTemplateI.Size = new System.Drawing.Size(25, 25);
		this.BtnSaveTemplateI.TabIndex = 66;
		this.BtnSaveTemplateI.UseVisualStyleBackColor = true;
		this.BtnSaveTemplateI.Click += new System.EventHandler(BtnSaveTemplateI_Click);
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Dock = System.Windows.Forms.DockStyle.Top;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(0, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(585, 31);
		this.label4.TabIndex = 3;
		this.label4.Text = "Pending Items For Allocation";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.lbltocompany);
		this.panel7.Controls.Add(this.BtnShowTemplateI);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPartyNameTemplateI);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1170, 40);
		this.panel7.TabIndex = 59;
		this.lbltocompany.AutoSize = true;
		this.lbltocompany.BackColor = System.Drawing.Color.White;
		this.lbltocompany.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.lbltocompany.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbltocompany.Location = new System.Drawing.Point(5, 13);
		this.lbltocompany.Name = "lbltocompany";
		this.lbltocompany.Size = new System.Drawing.Size(69, 15);
		this.lbltocompany.TabIndex = 16;
		this.lbltocompany.Text = "Party Name";
		this.BtnShowTemplateI.BackColor = System.Drawing.Color.Teal;
		this.BtnShowTemplateI.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnShowTemplateI.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnShowTemplateI.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Teal;
		this.BtnShowTemplateI.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Teal;
		this.BtnShowTemplateI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowTemplateI.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnShowTemplateI.ForeColor = System.Drawing.Color.White;
		this.BtnShowTemplateI.Location = new System.Drawing.Point(368, 8);
		this.BtnShowTemplateI.Margin = new System.Windows.Forms.Padding(0);
		this.BtnShowTemplateI.Name = "BtnShowTemplateI";
		this.BtnShowTemplateI.Size = new System.Drawing.Size(58, 25);
		this.BtnShowTemplateI.TabIndex = 66;
		this.BtnShowTemplateI.Text = "Show";
		this.BtnShowTemplateI.UseVisualStyleBackColor = false;
		this.BtnShowTemplateI.Click += new System.EventHandler(BtnShow_Click);
		this.CmbPartyNameTemplateI.AllowNull = (DefaultableBoolean)1;
		this.CmbPartyNameTemplateI.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPartyNameTemplateI.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPartyNameTemplateI.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPartyNameTemplateI).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPartyNameTemplateI).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPartyNameTemplateI.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPartyNameTemplateI).Location = new System.Drawing.Point(75, 7);
		((System.Windows.Forms.Control)(object)this.CmbPartyNameTemplateI).Name = "CmbPartyNameTemplateI";
		((System.Windows.Forms.Control)(object)this.CmbPartyNameTemplateI).Size = new System.Drawing.Size(290, 26);
		((System.Windows.Forms.Control)(object)this.CmbPartyNameTemplateI).TabIndex = 0;
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnRefresh, this.BtnItemDefine, this.btnDefineItemCustomGroup, this.btnshortcutkeys });
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
		this.BtnItemDefine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnItemDefine.Image = (System.Drawing.Image)resources.GetObject("BtnItemDefine.Image");
		this.BtnItemDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnItemDefine.Name = "BtnItemDefine";
		this.BtnItemDefine.Size = new System.Drawing.Size(87, 24);
		this.BtnItemDefine.Text = "Define Item";
		this.BtnItemDefine.Click += new System.EventHandler(BtnItemDefine_Click);
		this.btnDefineItemCustomGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnDefineItemCustomGroup.Image = (System.Drawing.Image)resources.GetObject("btnDefineItemCustomGroup.Image");
		this.btnDefineItemCustomGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefineItemCustomGroup.Name = "btnDefineItemCustomGroup";
		this.btnDefineItemCustomGroup.Size = new System.Drawing.Size(92, 24);
		this.btnDefineItemCustomGroup.Text = "Define Party";
		this.btnDefineItemCustomGroup.Click += new System.EventHandler(btnDefineItemCustomGroup_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.tabControl1.Controls.Add(this.tabPartyToItem);
		this.tabControl1.Controls.Add(this.tabItemToParty);
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
		this.tabPartyToItem.Text = "Party  Allocatation To Item";
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
		this.panel8.Controls.Add(this.BtnUpdateTemplateII);
		this.panel8.Controls.Add(this.btnDeleteTemplateII);
		this.panel8.Controls.Add(this.labelAllocateGridTemplateII);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(370, 39);
		this.panel8.TabIndex = 69;
		this.BtnUpdateTemplateII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnUpdateTemplateII.BackColor = System.Drawing.Color.White;
		this.BtnUpdateTemplateII.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnUpdateTemplateII.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnUpdateTemplateII.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnUpdateTemplateII.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnUpdateTemplateII.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateTemplateII.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.BtnUpdateTemplateII.Location = new System.Drawing.Point(296, 5);
		this.BtnUpdateTemplateII.Margin = new System.Windows.Forms.Padding(0);
		this.BtnUpdateTemplateII.Name = "BtnUpdateTemplateII";
		this.BtnUpdateTemplateII.Size = new System.Drawing.Size(33, 30);
		this.BtnUpdateTemplateII.TabIndex = 5;
		this.BtnUpdateTemplateII.UseVisualStyleBackColor = false;
		this.BtnUpdateTemplateII.Click += new System.EventHandler(BtnUpdateTemplateII_Click);
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
		this.labelAllocateGridTemplateII.Text = "Allocated Parties Of Item : TAG AL ANOOD BRAND 35 KG";
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
		this.labelUnAllocateGridTemplateII.Text = "UnAllocated Parties Of Item : TAG AL ANOOD BRAND 35 KG";
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
		this.label7.Text = "Item Name";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabItemToParty.Controls.Add(this.PanelMainTemplateI);
		this.tabItemToParty.Location = new System.Drawing.Point(4, 25);
		this.tabItemToParty.Name = "tabItemToParty";
		this.tabItemToParty.Padding = new System.Windows.Forms.Padding(3);
		this.tabItemToParty.Size = new System.Drawing.Size(1176, 402);
		this.tabItemToParty.TabIndex = 0;
		this.tabItemToParty.Text = "Item Allocatation To Party";
		this.tabItemToParty.UseVisualStyleBackColor = true;
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
		base.Name = "ItemAllocationToSupplierCustomer";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ItemAllocationToSupplierCustomer_FormClosing);
		base.Load += new System.EventHandler(frmItemsCustomizedGroups_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmItemsCustomizedGroups_KeyDown);
		this.PanelMainTemplateI.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridAllocatedTemplateI).EndInit();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocatedTemplateI).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyNameTemplateI).EndInit();
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
		this.tabItemToParty.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
