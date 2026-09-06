using System;
using System.Collections;
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
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
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

public class ItemAttributeVarient_Engr : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private DataTable dtHistory = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtVariantNotallocated = new DataTable();

	private IContainer components;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnshortcut;

	private Label label1;

	internal UltraCombo CmbvarientType;

	private Panel panel1;

	private Panel panel2;

	private Label label4;

	internal GridEX grdHistory;

	private TextBox txtcomputename;

	private Label label3;

	private ToolStripButton btnAllocation;

	private Label lbltocompany;

	internal UltraCombo CmbItem;

	private Panel panel3;

	internal GridEX grdHistoryWithItem;

	private Panel panel5;

	private Panel panel6;

	private Label label2;

	private Panel panel8;

	internal GridEX grdNotallocatedVariant;

	private Panel panel7;

	private Label label5;

	internal GridEX grdItemWithoutVariant;

	private Panel panel4;

	private Label label15;

	internal UltraCombo CmbCategory;

	private Label label64;

	private CtrlGrdBar ctrlGrdBar2;

	private CtrlGrdBar ctrlGrdBar1;

	private Button BtnAllocate;

	private Label label6;

	internal UltraCombo CmbModalSample;

	public ItemAttributeVarient_Engr(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void ItemAttributeVarient_Engr_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1100, 650);
			ctrlGrdBar1.mGridChooseFielder.Visible = false;
			ctrlGrdBar1.mGridSaveLayouts.Visible = false;
			ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
			ctrlGrdBar2.mGridChooseFielder.Visible = false;
			ctrlGrdBar2.mGridSaveLayouts.Visible = false;
			ctrlGrdBar2.removeLayoutToolStripMenuItem.Visible = false;
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			dtitem.Columns.Add("ItemCode");
			dtitem.Columns.Add("SampleNo");
			dtitem.Columns.Add("ItemCategoryId", typeof(int));
			varientattributefill();
			GetItemCategory();
			ItemdtFill();
			ItemDetailBind();
			dtHistoryFill();
			FormHistory(dtHistory);
			HistoryWithItem(dtHistory, 0);
			grdItemWithoutVariantFill();
			GrdNotallocatedVariantFill();
			CmbvarientType.Focus();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void varientattributefill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			int SupId = Conversion.ToInt(CmbvarientType.Value);
			DataTable dt = ItemAttribute.FormHistory(new ItemAttribute
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbvarientType, "Id", "AttributeDescription", "Varient Type", true);
				((UltraGridBase)CmbvarientType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbvarientType).Rows[((DisposableObjectCollectionBase)((UltraGridBase)CmbvarientType).Rows).Count - 1].Activate();
				if (SupId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == SupId))
					{
						CmbvarientType.Value = SupId;
					}
					else
					{
						((Control)(object)CmbvarientType).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbvarientType).Text = string.Empty;
				((UltraGridBase)CmbvarientType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			dtitem.Clear();
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

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbvarientType).ActiveRow == null || ((Control)(object)CmbvarientType).Text == "")
		{
			MessageBox.Show("Varient Type Field is Required");
			CmbvarientType.Focus();
			return false;
		}
		if (txtcomputename.Text == "")
		{
			MessageBox.Show("Compute Name Field is Required");
			txtcomputename.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			ItemAttributeVarient obj = new ItemAttributeVarient();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.ItemAttributeId = Conversion.ToInt(CmbvarientType.Value);
			obj.ComputedName = Conversion.ToString((object)txtcomputename.Text);
			obj.VarientDescription = Conversion.ToString((object)txtcomputename.Text);
			obj.Feet = 0m;
			obj.Inch = 0m;
			obj.ItemId = Conversion.ToInt(CmbItem.Value);
			ItemAttributeVarient.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully");
			}
			else
			{
				MessageBox.Show("Record Save Successfully");
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById(int Id)
	{
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			RecId = Id;
			ItemAttributeVarient pro = ItemAttributeVarient.ReadById(RecId);
			if (pro != null)
			{
				CmbvarientType.Value = pro.ItemAttributeId;
				txtcomputename.Text = pro.ComputedName;
				CmbItem.Value = pro.ItemId;
				CmbvarientType.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void dtHistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ItemAttributeVarient obj = new ItemAttributeVarient();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			dtHistory = ItemAttributeVarient.FormHistory(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FormHistory(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				DataTable dtDisType = new DataTable();
				dtDisType.Columns.Add("Id", typeof(int));
				dtDisType.Columns.Add("ItemAttributeId", typeof(int));
				dtDisType.Columns.Add("ItemAttribute", typeof(string));
				dtDisType.Columns.Add("SerialNo", typeof(int));
				dtDisType.Columns.Add("VariantDescription", typeof(string));
				dtDisType.Clear();
				foreach (IGrouping<int, DataRow> item in from row in dt.AsEnumerable()
					group row by row.Field<int>("ItemAttributeId"))
				{
					int serialNo = 1;
					foreach (DataRow row2 in item)
					{
						dtDisType.Rows.Add(row2.Field<int>("Id"), row2.Field<int>("ItemAttributeId"), row2.Field<string>("ItemAttribute"), serialNo++, row2.Field<string>("VarientDescription"));
					}
				}
				grdHistory.DataSource = dtDisType;
				grdHistory.RetrieveStructure();
				GrdSetting();
			}
			else
			{
				grdHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdSetting()
	{
		try
		{
			grdHistory.AllowEdit = (InheritableBoolean)1;
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["ItemAttributeId"].Visible = false;
			grdHistory.RootTable.Columns["ItemAttribute"].Visible = false;
			grdHistory.RootTable.Groups.Add("ItemAttribute");
			grdHistory.RootTable.Columns["ItemAttribute"].Width = 150;
			grdHistory.RootTable.Columns["Id"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["ItemAttributeId"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["SerialNo"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["VariantDescription"].EditType = (EditType)1;
			grdHistory.RootTable.Columns["ItemAttribute"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["SerialNo"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["VariantDescription"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["ItemAttribute"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.HeaderLines = 2;
			grdHistory.RootTable.Columns["SerialNo"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["VariantDescription"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["SerialNo"].Width = 55;
			grdHistory.RootTable.Columns["VariantDescription"].Width = 220;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				ReadById(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItem_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(CmbItem.Value);
			HistoryWithItem(dtHistory, Id);
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

	private void HistoryWithItem(DataTable dthistory, int ItemId)
	{
		try
		{
			grdHistoryWithItem.DataSource = null;
			if (dthistory.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			if (ItemId > 0)
			{
				DataRow[] filteredRows = dthistory.Select($"ItemId = {ItemId}");
				dt = (filteredRows.Any() ? filteredRows.CopyToDataTable() : new DataTable());
			}
			else
			{
				dt = dthistory;
			}
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtDisType = new DataTable();
			dtDisType.Columns.Add("Id", typeof(int));
			dtDisType.Columns.Add("ItemId", typeof(int));
			dtDisType.Columns.Add("ItemName", typeof(string));
			dtDisType.Columns.Add("ItemAttributeId", typeof(int));
			dtDisType.Columns.Add("ItemAttribute", typeof(string));
			dtDisType.Columns.Add("SerialNo", typeof(int));
			dtDisType.Columns.Add("VariantDescription", typeof(string));
			dtDisType.Clear();
			foreach (IGrouping<int, DataRow> item in dt.AsEnumerable().GroupBy(delegate(DataRow row)
			{
				int? num = row.Field<int?>("ItemId");
				return (num.HasValue && num.Value != 0) ? num.Value : 0;
			}))
			{
				int serialNo = 1;
				foreach (DataRow row2 in item)
				{
					dtDisType.Rows.Add(Conversion.ToInt((object)row2.Field<int?>("Id").GetValueOrDefault()), Conversion.ToInt((object)row2.Field<int?>("ItemId").GetValueOrDefault()), row2.Field<string>("ItemName") ?? string.Empty, Conversion.ToInt((object)row2.Field<int?>("ItemAttributeId").GetValueOrDefault()), row2.Field<string>("ItemAttribute") ?? string.Empty, serialNo++, row2.Field<string>("VarientDescription") ?? string.Empty);
				}
			}
			grdHistoryWithItem.DataSource = dtDisType;
			grdHistoryWithItem.RetrieveStructure();
			GrdHistoryWithItemSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistoryWithItemSetting()
	{
		try
		{
			grdHistoryWithItem.RootTable.Columns["Id"].Visible = false;
			grdHistoryWithItem.RootTable.Columns["ItemAttributeId"].Visible = false;
			grdHistoryWithItem.RootTable.Columns["ItemAttribute"].Visible = false;
			grdHistoryWithItem.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryWithItem.RootTable.Columns["ItemName"].Visible = false;
			grdHistoryWithItem.RootTable.Groups.Add("ItemName");
			grdHistoryWithItem.RootTable.HeaderLines = 2;
			grdHistoryWithItem.RootTable.Columns["SerialNo"].HeaderAlignment = (TextAlignment)2;
			grdHistoryWithItem.RootTable.Columns["VariantDescription"].HeaderAlignment = (TextAlignment)2;
			grdHistoryWithItem.RootTable.Columns["SerialNo"].Width = 60;
			grdHistoryWithItem.RootTable.Columns["VariantDescription"].Width = 230;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryWithItem_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistoryWithItem.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				ReadById(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdItemWithoutVariantFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			DataTable dt = ItemAttributeVarient.GetItemsWhichDoesNotHaveVariant(new ItemAttributeVarient
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtDisType = new DataTable();
				dtDisType.Columns.Add("ItemCategoryId", typeof(int));
				dtDisType.Columns.Add("ItemCategory", typeof(string));
				dtDisType.Columns.Add("ItemId", typeof(int));
				dtDisType.Columns.Add("ItemName", typeof(string));
				dtDisType.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtDisType.Rows.Add(Conversion.ToInt(dt.Rows[i]["ItemCategoryId"]), Conversion.ToString(dt.Rows[i]["ItemCategory"]), Conversion.ToInt(dt.Rows[i]["Id"]), Conversion.ToString(dt.Rows[i]["ItemName"]));
				}
				grdItemWithoutVariant.DataSource = dtDisType;
				grdItemWithoutVariant.RetrieveStructure();
				grdItemWithoutVariantSetting();
			}
			else
			{
				grdItemWithoutVariant.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdItemWithoutVariantSetting()
	{
		try
		{
			grdItemWithoutVariant.RootTable.Columns["ItemCategoryId"].Visible = false;
			grdItemWithoutVariant.RootTable.Columns["ItemId"].Visible = false;
			grdItemWithoutVariant.RootTable.Columns["ItemName"].Width = 290;
			grdItemWithoutVariant.RootTable.Columns["ItemCategory"].Width = 130;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdNotallocatedVariantFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			dtVariantNotallocated.Rows.Clear();
			DataTable dt = ItemAttributeVarient.GetVariantsNotAllocatedToAnyItem(new ItemAttributeVarient
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("ItemAttributeId", typeof(int));
				dtcol.Columns.Add("ItemAttribute", typeof(string));
				dtcol.Columns.Add("SerialNo", typeof(int));
				dtcol.Columns.Add("VariantDescription", typeof(string));
				dtcol.Columns.Add("ItemId", typeof(int));
				dtcol.Clear();
				foreach (IGrouping<int, DataRow> item in from row in dt.AsEnumerable()
					group row by row.Field<int>("ItemAttributeId"))
				{
					int serialNo = 1;
					foreach (DataRow row2 in item)
					{
						dtcol.Rows.Add(row2.Field<int>("Id"), row2.Field<int>("ItemAttributeId"), row2.Field<string>("ItemAttribute"), serialNo++, row2.Field<string>("VarientDescription"), 0);
					}
				}
				dtVariantNotallocated = dtcol.Copy();
				grdNotallocatedVariant.DataSource = dtcol;
				grdNotallocatedVariant.RetrieveStructure();
				GrdNotallocatedVariantSetting();
			}
			else
			{
				grdNotallocatedVariant.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdNotallocatedVariantSetting()
	{
		try
		{
			grdNotallocatedVariant.AllowEdit = (InheritableBoolean)1;
			grdNotallocatedVariant.RootTable.Columns["Id"].EditType = (EditType)0;
			grdNotallocatedVariant.RootTable.Columns["ItemAttributeId"].EditType = (EditType)0;
			grdNotallocatedVariant.RootTable.Columns["ItemAttribute"].EditType = (EditType)0;
			grdNotallocatedVariant.RootTable.Columns["SerialNo"].EditType = (EditType)0;
			grdNotallocatedVariant.RootTable.Columns["VariantDescription"].EditType = (EditType)0;
			grdNotallocatedVariant.RootTable.Columns["ItemAttribute"].FilterEditType = (FilterEditType)1;
			grdNotallocatedVariant.RootTable.Columns["SerialNo"].FilterEditType = (FilterEditType)1;
			grdNotallocatedVariant.RootTable.Columns["VariantDescription"].FilterEditType = (FilterEditType)1;
			grdNotallocatedVariant.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			grdNotallocatedVariant.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdNotallocatedVariant.RootTable.Columns["ItemId"].HasValueList = true;
			grdNotallocatedVariant.RootTable.Columns["ItemId"].LimitToList = true;
			grdNotallocatedVariant.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdNotallocatedVariant.RootTable.Columns["ItemId"].Caption = "Item Name";
			grdNotallocatedVariant.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtitem.DefaultView, "Id", "ItemName");
			grdNotallocatedVariant.RootTable.Columns["Id"].Visible = false;
			grdNotallocatedVariant.RootTable.Columns["ItemAttributeId"].Visible = false;
			grdNotallocatedVariant.RootTable.Columns["ItemAttribute"].Visible = false;
			grdNotallocatedVariant.RootTable.Groups.Add("ItemAttribute");
			grdNotallocatedVariant.RootTable.Columns["ItemAttribute"].Width = 150;
			grdNotallocatedVariant.RootTable.HeaderLines = 2;
			grdNotallocatedVariant.RootTable.Columns["SerialNo"].HeaderAlignment = (TextAlignment)2;
			grdNotallocatedVariant.RootTable.Columns["VariantDescription"].HeaderAlignment = (TextAlignment)2;
			grdNotallocatedVariant.RootTable.Columns["ItemId"].HeaderAlignment = (TextAlignment)2;
			grdNotallocatedVariant.RootTable.Columns["SerialNo"].Width = 60;
			grdNotallocatedVariant.RootTable.Columns["VariantDescription"].Width = 200;
			grdNotallocatedVariant.RootTable.Columns["ItemId"].Width = 150;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdNotallocatedVariant_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdNotallocatedVariant.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				ReadById(Id);
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
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			txtcomputename.Text = string.Empty;
			CmbvarientType.Focus();
			varientattributefill();
			GetItemCategory();
			ItemdtFill();
			ItemDetailBind();
			dtHistoryFill();
			FormHistory(dtHistory);
			HistoryWithItem(dtHistory, Conversion.ToInt(CmbItem.Value));
			grdItemWithoutVariantFill();
			GrdNotallocatedVariantFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemAttributeVarient_Engr_FormClosed(object sender, FormClosedEventArgs e)
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

	private void ItemAttributeVarient_Engr_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnsave_Click(null, null);
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				CmbvarientType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				CmbvarientType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdDetail_DoubleClick(null, null);
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Attribute");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item Combo Attribute");
			dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid ");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshortcut_Click(object sender, EventArgs e)
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

	private void BtnAllocate_Click(object sender, EventArgs e)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			List<DataRow> SaveList = ((DataTable)grdNotallocatedVariant.DataSource).AsEnumerable().Where(delegate(DataRow originalRow)
			{
				DataRow dataRow = dtVariantNotallocated.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == originalRow.Field<int>("Id"));
				return dataRow != null && originalRow.Field<int>("ItemId") != dataRow.Field<int>("ItemId");
			}).ToList();
			if (SaveList.Count > 0)
			{
				ItemAttributeVarientAllocationtoItem obj = new ItemAttributeVarientAllocationtoItem();
				obj.ItemAttributeVarientAllocationtoItemList = new List<ItemAttributeVarientAllocationtoItem>();
				for (int i = 0; i < SaveList.Count; i++)
				{
					ItemAttributeVarientAllocationtoItem d = new ItemAttributeVarientAllocationtoItem();
					d.OrganizationId = UserAccount.OrganizationId;
					d.CompanyId = UserAccount.CompanyId;
					d.BranchesId = UserAccount.BranchesId;
					d.ItemId = Conversion.ToInt(SaveList[i]["ItemId"]);
					d.ItemAttributeVarientId = Conversion.ToInt(SaveList[i]["Id"]);
					obj.ItemAttributeVarientAllocationtoItemList.Add(d);
				}
				ItemAttributeVarientAllocationtoItem.Save(obj);
				MessageBox.Show("Attribute Variant's Allocated Successfully");
				Reset();
			}
			else
			{
				MessageBox.Show("No changes Found In Grid");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnAllocation_Click(object sender, EventArgs e)
	{
		try
		{
			new AttributeVarientAllocationToItemsEngr(UserAccount).Show();
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
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Expected O, but got Unknown
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Expected O, but got Unknown
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Expected O, but got Unknown
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Expected O, but got Unknown
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		GridEXLayout grdHistoryWithItem_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout grdNotallocatedVariant_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdItemWithoutVariant_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Definition.ItemAttributeVarient_Engr));
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnAllocation = new System.Windows.Forms.ToolStripButton();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbvarientType = new UltraCombo();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdHistoryWithItem = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.CmbCategory = new UltraCombo();
		this.label64 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.lbltocompany = new System.Windows.Forms.Label();
		this.CmbModalSample = new UltraCombo();
		this.CmbItem = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.txtcomputename = new System.Windows.Forms.TextBox();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdNotallocatedVariant = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.grdItemWithoutVariant = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.BtnAllocate = new System.Windows.Forms.Button();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnshortcut = new System.Windows.Forms.ToolStripButton();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbvarientType).BeginInit();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryWithItem).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel2.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCategory).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbModalSample).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdNotallocatedVariant).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdItemWithoutVariant).BeginInit();
		this.panel6.SuspendLayout();
		base.SuspendLayout();
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnsave, this.btnUpdate, this.btnshortcut, this.btnAllocation });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1084, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnAllocation.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAllocation.Name = "btnAllocation";
		this.btnAllocation.Size = new System.Drawing.Size(131, 24);
		this.btnAllocation.Text = "Allocation To Item";
		this.btnAllocation.Click += new System.EventHandler(btnAllocation_Click);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(3, 31);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(72, 15);
		this.label1.TabIndex = 10;
		this.label1.Text = "Varient Type";
		this.CmbvarientType.AllowNull = (DefaultableBoolean)1;
		this.CmbvarientType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbvarientType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbvarientType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbvarientType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbvarientType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbvarientType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbvarientType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbvarientType).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbvarientType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbvarientType).Location = new System.Drawing.Point(111, 26);
		((System.Windows.Forms.Control)(object)this.CmbvarientType).Name = "CmbvarientType";
		((System.Windows.Forms.Control)(object)this.CmbvarientType).Size = new System.Drawing.Size(221, 24);
		((System.Windows.Forms.Control)(object)this.CmbvarientType).TabIndex = 0;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryWithItem);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel8);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(636, 634);
		this.panel1.TabIndex = 0;
		this.grdHistoryWithItem.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistoryWithItem).BackColor = System.Drawing.Color.White;
		this.grdHistoryWithItem.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdHistoryWithItem_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryWithItem.DesignTimeLayout = grdHistoryWithItem_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryWithItem).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryWithItem.FilterMode = (FilterMode)2;
		this.grdHistoryWithItem.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryWithItem).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryWithItem.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistoryWithItem).Location = new System.Drawing.Point(310, 114);
		((System.Windows.Forms.Control)(object)this.grdHistoryWithItem).Name = "grdHistoryWithItem";
		((ScrollableBase)this.grdHistoryWithItem).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryWithItem).Size = new System.Drawing.Size(326, 520);
		((System.Windows.Forms.Control)(object)this.grdHistoryWithItem).TabIndex = 1;
		this.grdHistoryWithItem.TotalRow = (InheritableBoolean)1;
		this.grdHistoryWithItem.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryWithItem).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdHistoryWithItem).DoubleClick += new System.EventHandler(grdHistoryWithItem_DoubleClick);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel3.Controls.Add(this.panel2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 114);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(310, 520);
		this.panel3.TabIndex = 14;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistory).BackColor = System.Drawing.Color.White;
		this.grdHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 10);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(310, 510);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label4);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(310, 10);
		this.panel2.TabIndex = 13;
		this.panel2.Visible = false;
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label4.Location = new System.Drawing.Point(3, 3);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(67, 19);
		this.label4.TabIndex = 9;
		this.label4.Text = "History";
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCategory);
		this.panel8.Controls.Add(this.label64);
		this.panel8.Controls.Add(this.panel4);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbvarientType);
		this.panel8.Controls.Add(this.label6);
		this.panel8.Controls.Add(this.lbltocompany);
		this.panel8.Controls.Add(this.label1);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbModalSample);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.panel8.Controls.Add(this.label3);
		this.panel8.Controls.Add(this.txtcomputename);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(636, 114);
		this.panel8.TabIndex = 0;
		this.CmbCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance24;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance30).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Location = new System.Drawing.Point(111, 81);
		((System.Windows.Forms.Control)(object)this.CmbCategory).Name = "CmbCategory";
		((System.Windows.Forms.Control)(object)this.CmbCategory).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbCategory).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Leave += new System.EventHandler(CmbCategory_Leave);
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(3, 87);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(82, 15);
		this.label64.TabIndex = 155;
		this.label64.Text = "Item Category";
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label15);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(636, 22);
		this.panel4.TabIndex = 98;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(5, 1);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(193, 19);
		this.label15.TabIndex = 1;
		this.label15.Text = "Item Attribute Varient";
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(335, 59);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(84, 15);
		this.label6.TabIndex = 97;
		this.label6.Text = "Modal Sample";
		this.lbltocompany.AutoSize = true;
		this.lbltocompany.BackColor = System.Drawing.Color.White;
		this.lbltocompany.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.lbltocompany.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbltocompany.Location = new System.Drawing.Point(335, 31);
		this.lbltocompany.Name = "lbltocompany";
		this.lbltocompany.Size = new System.Drawing.Size(67, 15);
		this.lbltocompany.TabIndex = 97;
		this.lbltocompany.Text = "Item Name";
		this.CmbModalSample.AllowNull = (DefaultableBoolean)1;
		this.CmbModalSample.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbModalSample.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbModalSample.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbModalSample).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbModalSample).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance38).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbModalSample).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbModalSample).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbModalSample.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbModalSample).Location = new System.Drawing.Point(422, 53);
		((System.Windows.Forms.Control)(object)this.CmbModalSample).Name = "CmbModalSample";
		((System.Windows.Forms.Control)(object)this.CmbModalSample).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbModalSample).TabIndex = 4;
		this.CmbModalSample.ValueChanged += new System.EventHandler(CmbModalSample_ValueChanged);
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
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
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(422, 25);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 3;
		this.CmbItem.ValueChanged += new System.EventHandler(CmbItem_Leave);
		((System.Windows.Forms.Control)(object)this.CmbItem).Leave += new System.EventHandler(CmbItem_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(3, 59);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(106, 15);
		this.label3.TabIndex = 92;
		this.label3.Text = "Variant description";
		this.txtcomputename.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcomputename.Location = new System.Drawing.Point(111, 55);
		this.txtcomputename.Name = "txtcomputename";
		this.txtcomputename.Size = new System.Drawing.Size(221, 23);
		this.txtcomputename.TabIndex = 1;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdNotallocatedVariant);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdItemWithoutVariant);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(636, 27);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(446, 634);
		this.panel5.TabIndex = 15;
		this.grdNotallocatedVariant.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdNotallocatedVariant).BackColor = System.Drawing.Color.White;
		this.grdNotallocatedVariant.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdNotallocatedVariant_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdNotallocatedVariant.DesignTimeLayout = grdNotallocatedVariant_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdNotallocatedVariant).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdNotallocatedVariant.FilterMode = (FilterMode)2;
		this.grdNotallocatedVariant.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdNotallocatedVariant).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdNotallocatedVariant.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdNotallocatedVariant).Location = new System.Drawing.Point(0, 350);
		((System.Windows.Forms.Control)(object)this.grdNotallocatedVariant).Name = "grdNotallocatedVariant";
		((ScrollableBase)this.grdNotallocatedVariant).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdNotallocatedVariant).Size = new System.Drawing.Size(446, 284);
		((System.Windows.Forms.Control)(object)this.grdNotallocatedVariant).TabIndex = 1;
		this.grdNotallocatedVariant.TotalRow = (InheritableBoolean)1;
		this.grdNotallocatedVariant.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdNotallocatedVariant).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdNotallocatedVariant).DoubleClick += new System.EventHandler(grdNotallocatedVariant_DoubleClick);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.BtnAllocate);
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label5);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 328);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(446, 22);
		this.panel7.TabIndex = 11;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(405, 1);
		this.ctrlGrdBar2.MyGrid = this.grdNotallocatedVariant;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(39, 20);
		this.ctrlGrdBar2.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.Teal;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label5.Location = new System.Drawing.Point(2, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(279, 14);
		this.label5.TabIndex = 1;
		this.label5.Text = "Variant Which are not Allocated to any Item";
		this.grdItemWithoutVariant.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdItemWithoutVariant).BackColor = System.Drawing.Color.White;
		grdItemWithoutVariant_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdItemWithoutVariant.DesignTimeLayout = grdItemWithoutVariant_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdItemWithoutVariant).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grdItemWithoutVariant).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdItemWithoutVariant.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdItemWithoutVariant).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdItemWithoutVariant).Name = "grdItemWithoutVariant";
		((ScrollableBase)this.grdItemWithoutVariant).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdItemWithoutVariant).Size = new System.Drawing.Size(446, 306);
		((System.Windows.Forms.Control)(object)this.grdItemWithoutVariant).TabIndex = 0;
		this.grdItemWithoutVariant.TotalRow = (InheritableBoolean)1;
		this.grdItemWithoutVariant.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdItemWithoutVariant).VisualStyle = (VisualStyle)4;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(446, 22);
		this.panel6.TabIndex = 9;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(401, 1);
		this.ctrlGrdBar1.MyGrid = this.grdItemWithoutVariant;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 19);
		this.ctrlGrdBar1.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label2.Location = new System.Drawing.Point(5, 4);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(265, 14);
		this.label2.TabIndex = 1;
		this.label2.Text = "Item Which Does not Contain Any Variant";
		this.BtnAllocate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnAllocate.BackColor = System.Drawing.Color.White;
		this.BtnAllocate.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnAllocate.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnAllocate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnAllocate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnAllocate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAllocate.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.BtnAllocate.Location = new System.Drawing.Point(377, 2);
		this.BtnAllocate.Margin = new System.Windows.Forms.Padding(0);
		this.BtnAllocate.Name = "BtnAllocate";
		this.BtnAllocate.Size = new System.Drawing.Size(25, 19);
		this.BtnAllocate.TabIndex = 6;
		this.BtnAllocate.UseVisualStyleBackColor = false;
		this.BtnAllocate.Click += new System.EventHandler(BtnAllocate_Click);
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnshortcut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcut.Name = "btnshortcut";
		this.btnshortcut.Size = new System.Drawing.Size(126, 24);
		this.btnshortcut.Text = "ShortCut Keys";
		this.btnshortcut.Click += new System.EventHandler(btnshortcut_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1084, 661);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.toolStrip1);
		base.KeyPreview = true;
		base.Name = "ItemAttributeVarient_Engr";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ItemAttributeVarient_Engr_FormClosed);
		base.Load += new System.EventHandler(ItemAttributeVarient_Engr_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ItemAttributeVarient_Engr_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbvarientType).EndInit();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryWithItem).EndInit();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCategory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbModalSample).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdNotallocatedVariant).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdItemWithoutVariant).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
