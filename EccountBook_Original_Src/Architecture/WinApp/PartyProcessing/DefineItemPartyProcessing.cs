using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.PartyProcessing;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.PartyProcessing;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory_Definition;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Taxation;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.PartyProcessing;

public class DefineItemPartyProcessing : Form
{
	private bool DoHaveSaveRight;

	private bool DoHavePrintRights;

	private bool DoHaveUpdateRights;

	private bool DoHaveCanViewAllRecordRights;

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	public UserAccount UserAccount = new UserAccount();

	public bool UpdateMode;

	private Country country = new Country();

	private bool flag;

	private int RecId;

	private string saveDirectory = "";

	private string fileSavePath = "";

	private string fileSavePath2 = "";

	private DataTable dtCatergory = new DataTable();

	private IContainer components;

	private Label label13;

	private TabControl tabCAddItem;

	private TabPage tpItemDefinitionAddItem;

	private Button btnitmuomtaxsch;

	private Button btnitmuom;

	private Button btnitmtype;

	private Button btnitmcat;

	private Panel panel5;

	private Panel panel2;

	private Panel panel6;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnprint;

	private ToolStripButton btnattachment;

	private Panel panel1;

	private ToolStripButton toolStripButton1;

	private Button btnItemGroup;

	private Panel panel7;

	private CheckBox chkstatus;

	internal UltraCombo CmbItemGroup;

	private Label label9;

	private TextBox txtItemCode;

	private TextBox txtItemName;

	private Label label5;

	private Label label4;

	private Label label3;

	private Label label2;

	private Label label1;

	internal UltraCombo cmbItemType;

	internal UltraCombo cmbBaseUnit;

	internal UltraCombo cmbItemCategory;

	private Button btnDefineLots;

	private GroupBox groupBox1;

	internal GridEX grdAllocation;

	private Button btnItemGroupSchedule;

	private Button btnCropYear;

	private TabPage tpItemDefinitionAddItemHistory;

	private Panel panel3;

	private Panel panel8;

	private Panel panel4;

	internal GridEX grdhistory;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnWarehouse;

	public DefineItemPartyProcessing(UserAccount pUserAccount)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void InvDefrmAddItem_Load(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			List<tblUserRights> lstRights = tblUserRights.GetByUserId(new tblUserRights
			{
				UserId = UserAccount.ID,
				ScreenName = "DefPartProcessingAddItem",
				RightName = UserAccount.RoleName
			});
			if (lstRights.Count > 0)
			{
				List<tblUserRights> newList = lstRights.Where((tblUserRights x) => x.RightName == "Save").ToList();
				DoHaveSaveRight = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Print").ToList();
				DoHavePrintRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Update").ToList();
				DoHaveUpdateRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "CanView AllRecord").ToList();
				DoHaveCanViewAllRecordRights = Conversion.ToBool((object)newList[0].Value.ToString());
			}
			btnsave.Enabled = DoHaveSaveRight;
			btnprint.Enabled = DoHavePrintRights;
			btnupdate.Enabled = DoHaveUpdateRights;
			dtViewAttachments.Columns.Add("AttchmentPath");
			ItemTypeFill();
			ItemCatagoryHistoryFill();
			ItemGroupNameFill();
			BaseUnitFill();
			CompaniesBindInGrid();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			cmbItemCategory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemCatagoryHistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ItemCategoryPartyProcessing cat = new ItemCategoryPartyProcessing();
			cat.OrganizationId = UserAccount.OrganizationId;
			cat.CompanyId = UserAccount.CompanyId;
			dtCatergory = ItemCategoryPartyProcessing.Getall(cat);
			if (dtCatergory.Rows.Count > 0)
			{
				DataTable dtcath = new DataTable();
				dtcath.Columns.Add("Id");
				dtcath.Columns.Add("CategoryDescription");
				for (int i = 0; i < dtCatergory.Rows.Count; i++)
				{
					dtcath.Rows.Add(dtCatergory.Rows[i]["Id"], dtCatergory.Rows[i]["CategoryDescription"]);
				}
				if (dtcath.Rows.Count > 0)
				{
					DDL.BindDDL(dtcath, cmbItemCategory, "Id", "CategoryDescription", "Item Category", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BaseUnitFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtuom = UOM.Getall(new UOM
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtuom.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtuom, cmbBaseUnit, "Id", "UOMCode", "UOM Code", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemTypeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = ItemTypePartyProcessing.Getall(new ItemTypePartyProcessing
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbItemType, "Id", "TypeDescription", "Item Type", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemGroupNameFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = ItemGroup.GetAll(new ItemGroup
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("GroupId");
				dt.Columns.Add("ItemGroupName");
				dt.Rows.Clear();
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dt.Rows.Add(lst.Rows[i]["GroupId"], lst.Rows[i]["ItemGroupName"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbItemGroup, "GroupId", "ItemGroupName", "Group Name", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CompaniesBindInGrid()
	{
		try
		{
			DataTable dt = CommonServices.CompanyServiceBind();
			if (dt.Rows.Count > 0)
			{
				DataTable table = new DataTable();
				table.Columns.Add("Id");
				table.Columns.Add("Location");
				table.Columns.Add("Value");
				table.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					table.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["CompName"], true);
				}
				if (table.Rows.Count > 0)
				{
					grdAllocation.DataSource = table;
					grdAllocation.RetrieveStructure();
					grdAllocation.RootTable.Columns["Id"].Visible = false;
					grdAllocation.RootTable.Columns["Location"].Width = 250;
					grdAllocation.RootTable.Columns["Value"].EditType = (EditType)8;
					grdAllocation.RootTable.Columns["Value"].FilterEditType = (FilterEditType)7;
					grdAllocation.RootTable.Columns["Value"].ColumnType = (ColumnType)3;
					grdAllocation.RootTable.Columns["Value"].CheckBoxTrueValue = "True";
					grdAllocation.RootTable.Columns["Value"].CheckBoxFalseValue = "False";
					grdAllocation.RootTable.Columns["Value"].UseHeaderSelector = true;
				}
				else
				{
					grdAllocation.ClearStructure();
				}
			}
			else
			{
				grdAllocation.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationForAddItem()
	{
		if (txtItemName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Item Name Field is Required");
			txtItemName.Focus();
			return false;
		}
		if (((UltraGridBase)cmbBaseUnit).ActiveRow == null)
		{
			MessageBox.Show("Base Unit Field is Required");
			cmbBaseUnit.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemCategory).ActiveRow == null)
		{
			MessageBox.Show("Item Category Field is Required");
			cmbItemCategory.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemType).ActiveRow == null)
		{
			MessageBox.Show("Item Type Field is Required");
			cmbItemType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemGroup).ActiveRow == null)
		{
			MessageBox.Show("Item Group Field is Required");
			CmbItemGroup.Focus();
			return false;
		}
		return true;
	}

	private void refresh()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			((Control)(object)cmbBaseUnit).Text = string.Empty;
			((Control)(object)cmbItemType).Text = string.Empty;
			txtItemCode.Clear();
			txtItemName.Clear();
			((Control)(object)cmbItemCategory).Text = string.Empty;
			((Control)(object)CmbItemGroup).Text = string.Empty;
			cmbItemCategory.Focus();
			RecId = 0;
			CompaniesBindInGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Expected O, but got Unknown
		try
		{
			if (!FormValidationForAddItem())
			{
				return;
			}
			ItemPartyProcessing items = new ItemPartyProcessing();
			items.Id = RecId;
			items.ItemCode = Conversion.ToString((object)txtItemCode.Text.Trim());
			items.ItemName = Conversion.ToString((object)txtItemName.Text.Trim());
			items.ItemBaseUnitId = Conversion.ToInt(cmbBaseUnit.Value);
			items.ItemCategoryId = Conversion.ToInt(cmbItemCategory.Value);
			items.ItemTypeId = Conversion.ToInt(cmbItemType.Value);
			items.UomGroupId = Conversion.ToInt(CmbItemGroup.Value);
			items.OrganizationId = UserAccount.OrganizationId;
			items.CompanyId = UserAccount.CompanyId;
			items.EntryUser = UserAccount.ID;
			items.AttachmentsList = new List<DMSAttachments>();
			items.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<ItemPartyProcessing>(items, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			items.ItemAllocationlist = new List<ItemAllocation>();
			int countfalsevalue = 0;
			int count = grdAllocation.GetDataRows().Count();
			if (count == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			GridEXRow[] dataRows = grdAllocation.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				if (count == 1)
				{
					ItemAllocation allocation = new ItemAllocation();
					allocation.CompanyId = Conversion.ToInt(r.Cells["Id"].Value);
					allocation.IsActive = true;
					allocation.BranchId = UserAccount.BranchesId;
					allocation.OrganizationId = UserAccount.OrganizationId;
					items.ItemAllocationlist.Add(allocation);
				}
				else if (bool.Parse(r.Cells["Value"].Value.ToString()))
				{
					ItemAllocation allocation2 = new ItemAllocation();
					allocation2.CompanyId = Conversion.ToInt(r.Cells["Id"].Value);
					allocation2.IsActive = Conversion.ToBool(r.Cells["Value"].Value);
					allocation2.BranchId = UserAccount.BranchesId;
					allocation2.OrganizationId = UserAccount.OrganizationId;
					items.ItemAllocationlist.Add(allocation2);
				}
				else
				{
					countfalsevalue++;
				}
			}
			if (count > 1 && count == countfalsevalue)
			{
				throw new Exception("Please select Location first");
			}
			ItemPartyProcessing.Save(items);
			if (RecId > 0)
			{
				MessageBox.Show("Update Successfully");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 0, 0);
				}
			}
			else
			{
				MessageBox.Show("Save Successfully");
			}
			refresh();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnSave_Click(object sender, EventArgs e)
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

	private void btnupdate_Click(object sender, EventArgs e)
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

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ItemPartyProcessing items = new ItemPartyProcessing();
			RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
			items = ItemPartyProcessing.GetByID(RecId);
			if (items != null)
			{
				txtItemCode.Text = Conversion.ToString((object)items.ItemCode);
				txtItemName.Text = Conversion.ToString((object)items.ItemName);
				cmbBaseUnit.Value = Conversion.ToInt((object)items.ItemBaseUnitId);
				CmbItemGroup.Value = Conversion.ToInt((object)items.UomGroupId);
				cmbItemCategory.Value = items.ItemCategoryId;
				cmbItemType.Value = items.ItemTypeId;
				chkstatus.Checked = Conversion.ToBool((object)items.IsActive);
				tabCAddItem.SelectedIndex = 0;
				btnsave.Visible = false;
				btnupdate.Visible = true;
				((UltraGridBase)CmbItemGroup).Rows[0].Activate();
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		refresh();
	}

	private void grdfrmfill(int NoOfRecords)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters reports = new ReportsParameters();
			reports.OrganizationId = UserAccount.OrganizationId;
			reports.CompanyId = UserAccount.CompanyId;
			reports.NoOfRecords = NoOfRecords;
			reports.CanViewAllRecord = DoHaveCanViewAllRecordRights;
			if (!reports.CanViewAllRecord)
			{
				reports.EntryUser = UserAccount.ID;
			}
			DataTable grdlst = ItemPartyProcessing.Getall(reports);
			if (grdlst.Rows.Count > 0)
			{
				grdhistory.DataSource = grdlst;
				grdhistory.RetrieveStructure();
				gridsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsetting()
	{
		try
		{
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["RecordNo"].Visible = false;
			grdhistory.RootTable.Columns["ItemBaseUnitId"].Visible = false;
			grdhistory.RootTable.Columns["ItemCategoryId"].Visible = false;
			grdhistory.RootTable.Columns["ItemTypeId"].Visible = false;
			grdhistory.RootTable.Columns["UomGroupId"].Visible = false;
			grdhistory.RootTable.Columns["EntryDate"].Visible = false;
			grdhistory.RootTable.Columns["EntryUser"].Visible = false;
			grdhistory.RootTable.Columns["ModifyDate"].Visible = false;
			grdhistory.RootTable.Columns["ModifyUser"].Visible = false;
			grdhistory.RootTable.Columns["OrganizationId"].Visible = false;
			grdhistory.RootTable.Columns["CompanyId"].Visible = false;
			grdhistory.RootTable.Columns["IsActive"].Visible = false;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvDefrmAddItem_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (btnsave.Visible && e.Control && e.KeyCode == Keys.S && tabCAddItem.SelectedIndex == 0)
		{
			BtnSave_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			if (tabCAddItem.SelectedIndex == 0)
			{
				btnnew_Click(null, null);
			}
			else
			{
				btnnew_Click(null, null);
			}
		}
		if (e.Control && e.KeyCode == Keys.T)
		{
			if (tabCAddItem.SelectedIndex == 1)
			{
				tabCAddItem.SelectedIndex = 0;
			}
			else
			{
				tabCAddItem.SelectedIndex = 1;
			}
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (btnupdate.Visible && e.Control && e.KeyCode == Keys.U && (UpdateMode = true))
		{
			btnupdate_Click(null, null);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdhistory).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdhistory).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdhistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void btnitmcat_Click(object sender, EventArgs e)
	{
		try
		{
			new DefItemCatagoryPartyProcessing(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnitmtaxsch_Click(object sender, EventArgs e)
	{
		try
		{
			new InvDeffrmItemTaxSchdule(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnitmtype_Click(object sender, EventArgs e)
	{
		try
		{
			new DefPartyProcessingItemType(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnitmuom_Click(object sender, EventArgs e)
	{
		try
		{
			new InvDeffrmItemItemUom(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnitmuomtaxsch_Click(object sender, EventArgs e)
	{
		try
		{
			new InvDeffrmItemUomSchedule(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			ItemTypeFill();
			ItemCatagoryHistoryFill();
			ItemGroupNameFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCropYear_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineCropYear(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnItemGroup_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineItemGroup(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnItemGroupSchedule_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineItemGroupUOMSchedule(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabCAddItem_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabCAddItem.SelectedIndex == 1)
		{
			grdfrmfill(50);
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
		try
		{
			grdfrmfill(0);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "NoOfAttachments"))
			{
				return;
			}
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			List<DMSAttachments> dMSList = DMSAttachments.GetByID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), base.Name);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (DMSAttachments itemList in dMSList)
				{
					dtViewAttachments.Rows.Add(itemList.Attachment);
				}
			}
			else
			{
				dtViewAttachments.Clear();
			}
			new AttachmentView(dtViewAttachments).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUomGroupAssignToItem_Click(object sender, EventArgs e)
	{
	}

	private void button1_Click(object sender, EventArgs e)
	{
		try
		{
			new ItemAllocateToWarehouse(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnItemAllocation_Click(object sender, EventArgs e)
	{
		try
		{
			new ItemsAllocationToCompany(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItemCategory_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable lst = ItemPartyProcessing.GenerateCode(new ItemPartyProcessing
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemCategoryId = Convert.ToInt32(cmbItemCategory.Value)
			});
			if (lst.Rows.Count > 0)
			{
				txtItemCode.Text = lst.Rows[0]["ItemCode"].ToString();
			}
			else
			{
				txtItemCode.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnWarehouse_Click(object sender, EventArgs e)
	{
		try
		{
			new InvDeffrmWarehousePartyProcessing(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefineLots_Click(object sender, EventArgs e)
	{
		try
		{
			new AcfrmDefineJobLotPartyProcessing(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DefineItemPartyProcessing_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Expected O, but got Unknown
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
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
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.PartyProcessing.DefineItemPartyProcessing));
		this.label13 = new System.Windows.Forms.Label();
		this.tabCAddItem = new System.Windows.Forms.TabControl();
		this.tpItemDefinitionAddItem = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.grdAllocation = new GridEX();
		this.cmbItemType = new UltraCombo();
		this.cmbBaseUnit = new UltraCombo();
		this.cmbItemCategory = new UltraCombo();
		this.chkstatus = new System.Windows.Forms.CheckBox();
		this.CmbItemGroup = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.txtItemCode = new System.Windows.Forms.TextBox();
		this.txtItemName = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.panel5 = new System.Windows.Forms.Panel();
		this.btnWarehouse = new System.Windows.Forms.Button();
		this.btnDefineLots = new System.Windows.Forms.Button();
		this.btnItemGroupSchedule = new System.Windows.Forms.Button();
		this.btnItemGroup = new System.Windows.Forms.Button();
		this.btnCropYear = new System.Windows.Forms.Button();
		this.btnitmcat = new System.Windows.Forms.Button();
		this.btnitmuomtaxsch = new System.Windows.Forms.Button();
		this.btnitmuom = new System.Windows.Forms.Button();
		this.btnitmtype = new System.Windows.Forms.Button();
		this.tpItemDefinitionAddItemHistory = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.tabCAddItem.SuspendLayout();
		this.tpItemDefinitionAddItem.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAllocation).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBaseUnit).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemGroup).BeginInit();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.tpItemDefinitionAddItemHistory.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(9, 9);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(83, 23);
		this.label13.TabIndex = 0;
		this.label13.Text = "Add Item";
		this.tabCAddItem.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabCAddItem.Controls.Add(this.tpItemDefinitionAddItem);
		this.tabCAddItem.Controls.Add(this.tpItemDefinitionAddItemHistory);
		this.tabCAddItem.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabCAddItem.Location = new System.Drawing.Point(0, 0);
		this.tabCAddItem.Multiline = true;
		this.tabCAddItem.Name = "tabCAddItem";
		this.tabCAddItem.SelectedIndex = 0;
		this.tabCAddItem.Size = new System.Drawing.Size(1050, 433);
		this.tabCAddItem.TabIndex = 0;
		this.tabCAddItem.SelectedIndexChanged += new System.EventHandler(tabCAddItem_SelectedIndexChanged);
		this.tpItemDefinitionAddItem.BackColor = System.Drawing.Color.White;
		this.tpItemDefinitionAddItem.Controls.Add(this.panel1);
		this.tpItemDefinitionAddItem.Controls.Add(this.panel2);
		this.tpItemDefinitionAddItem.Controls.Add(this.panel6);
		this.tpItemDefinitionAddItem.Controls.Add(this.panel5);
		this.tpItemDefinitionAddItem.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tpItemDefinitionAddItem.Location = new System.Drawing.Point(4, 4);
		this.tpItemDefinitionAddItem.Name = "tpItemDefinitionAddItem";
		this.tpItemDefinitionAddItem.Padding = new System.Windows.Forms.Padding(3);
		this.tpItemDefinitionAddItem.Size = new System.Drawing.Size(1042, 407);
		this.tpItemDefinitionAddItem.TabIndex = 0;
		this.tpItemDefinitionAddItem.Text = "Add Item";
		this.panel1.Controls.Add(this.panel7);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 76);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1036, 233);
		this.panel1.TabIndex = 7;
		this.panel7.Controls.Add(this.groupBox1);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemType);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBaseUnit);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCategory);
		this.panel7.Controls.Add(this.chkstatus);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemGroup);
		this.panel7.Controls.Add(this.label9);
		this.panel7.Controls.Add(this.txtItemCode);
		this.panel7.Controls.Add(this.txtItemName);
		this.panel7.Controls.Add(this.label5);
		this.panel7.Controls.Add(this.label4);
		this.panel7.Controls.Add(this.label3);
		this.panel7.Controls.Add(this.label2);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1036, 230);
		this.panel7.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.grdAllocation);
		this.groupBox1.Location = new System.Drawing.Point(13, 64);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(353, 141);
		this.groupBox1.TabIndex = 15;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Item Allocation ";
		this.grdAllocation.AllowDelete = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdAllocation).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdAllocation).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAllocation.GroupByBoxVisible = false;
		this.grdAllocation.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdAllocation).Location = new System.Drawing.Point(3, 19);
		((System.Windows.Forms.Control)(object)this.grdAllocation).Name = "grdAllocation";
		((ScrollableBase)this.grdAllocation).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdAllocation).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdAllocation).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAllocation).Size = new System.Drawing.Size(347, 119);
		((System.Windows.Forms.Control)(object)this.grdAllocation).TabIndex = 0;
		this.grdAllocation.TotalRow = (InheritableBoolean)1;
		this.grdAllocation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAllocation).VisualStyle = (VisualStyle)4;
		this.cmbItemType.AllowNull = (DefaultableBoolean)1;
		this.cmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Location = new System.Drawing.Point(428, 33);
		((System.Windows.Forms.Control)(object)this.cmbItemType).Name = "cmbItemType";
		((System.Windows.Forms.Control)(object)this.cmbItemType).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemType).TabIndex = 4;
		this.cmbBaseUnit.AllowNull = (DefaultableBoolean)1;
		this.cmbBaseUnit.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBaseUnit.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBaseUnit.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Appearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBaseUnit.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Location = new System.Drawing.Point(116, 32);
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Name = "cmbBaseUnit";
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).TabIndex = 3;
		this.cmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Location = new System.Drawing.Point(116, 4);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Name = "cmbItemCategory";
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Leave += new System.EventHandler(cmbItemCategory_Leave);
		this.chkstatus.AutoSize = true;
		this.chkstatus.Checked = true;
		this.chkstatus.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkstatus.Location = new System.Drawing.Point(954, 40);
		this.chkstatus.Name = "chkstatus";
		this.chkstatus.Size = new System.Drawing.Size(71, 19);
		this.chkstatus.TabIndex = 6;
		this.chkstatus.Text = "Is Active";
		this.chkstatus.UseVisualStyleBackColor = true;
		this.CmbItemGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbItemGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance147;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance104).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemGroup).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemGroup).Location = new System.Drawing.Point(727, 35);
		((System.Windows.Forms.Control)(object)this.CmbItemGroup).Name = "CmbItemGroup";
		((System.Windows.Forms.Control)(object)this.CmbItemGroup).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemGroup).TabIndex = 5;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(634, 43);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(71, 15);
		this.label9.TabIndex = 11;
		this.label9.Text = "Uom Group";
		this.txtItemCode.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemCode.Location = new System.Drawing.Point(428, 6);
		this.txtItemCode.Name = "txtItemCode";
		this.txtItemCode.ReadOnly = true;
		this.txtItemCode.Size = new System.Drawing.Size(198, 23);
		this.txtItemCode.TabIndex = 1;
		this.txtItemCode.TabStop = false;
		this.txtItemName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemName.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemName.Location = new System.Drawing.Point(727, 6);
		this.txtItemName.Name = "txtItemName";
		this.txtItemName.Size = new System.Drawing.Size(220, 23);
		this.txtItemName.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(343, 35);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(60, 15);
		this.label5.TabIndex = 4;
		this.label5.Text = "Item Type";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(8, 8);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(82, 15);
		this.label4.TabIndex = 3;
		this.label4.Text = "Item Category";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(8, 35);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(60, 15);
		this.label3.TabIndex = 2;
		this.label3.Text = "Base Unit";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(634, 8);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(68, 15);
		this.label2.TabIndex = 1;
		this.label2.Text = "Item Name";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(343, 8);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Item Code";
		this.panel2.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel2.Controls.Add(this.label13);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 35);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1036, 41);
		this.panel2.TabIndex = 6;
		this.panel6.Controls.Add(this.toolStrip1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1036, 32);
		this.panel6.TabIndex = 5;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnnew, this.btnupdate, this.btnsave, this.btnprint, this.btnattachment, this.toolStripButton1 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1036, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(69, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(BtnSave_Click);
		this.btnprint.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnprint.Image = (System.Drawing.Image)resources.GetObject("btnprint.Image");
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(56, 24);
		this.btnprint.Text = "&Print";
		this.btnattachment.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = (System.Drawing.Image)resources.GetObject("btnattachment.Image");
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(94, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(50, 24);
		this.toolStripButton1.Text = "Refresh";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.panel5.Controls.Add(this.btnWarehouse);
		this.panel5.Controls.Add(this.btnDefineLots);
		this.panel5.Controls.Add(this.btnItemGroupSchedule);
		this.panel5.Controls.Add(this.btnItemGroup);
		this.panel5.Controls.Add(this.btnCropYear);
		this.panel5.Controls.Add(this.btnitmcat);
		this.panel5.Controls.Add(this.btnitmuomtaxsch);
		this.panel5.Controls.Add(this.btnitmuom);
		this.panel5.Controls.Add(this.btnitmtype);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel5.Location = new System.Drawing.Point(3, 311);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1036, 93);
		this.panel5.TabIndex = 4;
		this.btnWarehouse.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnWarehouse.FlatAppearance.BorderSize = 0;
		this.btnWarehouse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnWarehouse.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnWarehouse.ForeColor = System.Drawing.Color.White;
		this.btnWarehouse.Location = new System.Drawing.Point(460, 49);
		this.btnWarehouse.Name = "btnWarehouse";
		this.btnWarehouse.Size = new System.Drawing.Size(113, 35);
		this.btnWarehouse.TabIndex = 9;
		this.btnWarehouse.Text = "WareHouse";
		this.btnWarehouse.UseVisualStyleBackColor = false;
		this.btnWarehouse.Click += new System.EventHandler(btnWarehouse_Click);
		this.btnDefineLots.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnDefineLots.FlatAppearance.BorderSize = 0;
		this.btnDefineLots.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDefineLots.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDefineLots.ForeColor = System.Drawing.Color.White;
		this.btnDefineLots.Location = new System.Drawing.Point(231, 49);
		this.btnDefineLots.Name = "btnDefineLots";
		this.btnDefineLots.Size = new System.Drawing.Size(223, 35);
		this.btnDefineLots.TabIndex = 8;
		this.btnDefineLots.Text = "Define JobLots";
		this.btnDefineLots.UseVisualStyleBackColor = false;
		this.btnDefineLots.Click += new System.EventHandler(btnDefineLots_Click);
		this.btnItemGroupSchedule.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnItemGroupSchedule.FlatAppearance.BorderSize = 0;
		this.btnItemGroupSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnItemGroupSchedule.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnItemGroupSchedule.ForeColor = System.Drawing.Color.White;
		this.btnItemGroupSchedule.Location = new System.Drawing.Point(8, 49);
		this.btnItemGroupSchedule.Name = "btnItemGroupSchedule";
		this.btnItemGroupSchedule.Size = new System.Drawing.Size(219, 35);
		this.btnItemGroupSchedule.TabIndex = 7;
		this.btnItemGroupSchedule.Text = "Item Uom Group Schedule";
		this.btnItemGroupSchedule.UseVisualStyleBackColor = false;
		this.btnItemGroupSchedule.Click += new System.EventHandler(btnItemGroupSchedule_Click);
		this.btnItemGroup.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnItemGroup.FlatAppearance.BorderSize = 0;
		this.btnItemGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnItemGroup.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnItemGroup.ForeColor = System.Drawing.Color.White;
		this.btnItemGroup.Location = new System.Drawing.Point(579, 8);
		this.btnItemGroup.Name = "btnItemGroup";
		this.btnItemGroup.Size = new System.Drawing.Size(229, 35);
		this.btnItemGroup.TabIndex = 6;
		this.btnItemGroup.Text = "Item Uom Group";
		this.btnItemGroup.UseVisualStyleBackColor = false;
		this.btnItemGroup.Click += new System.EventHandler(btnItemGroup_Click);
		this.btnCropYear.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnCropYear.FlatAppearance.BorderSize = 0;
		this.btnCropYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCropYear.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCropYear.ForeColor = System.Drawing.Color.White;
		this.btnCropYear.Location = new System.Drawing.Point(478, 8);
		this.btnCropYear.Name = "btnCropYear";
		this.btnCropYear.Size = new System.Drawing.Size(95, 35);
		this.btnCropYear.TabIndex = 5;
		this.btnCropYear.Text = "Crop Year";
		this.btnCropYear.UseVisualStyleBackColor = false;
		this.btnCropYear.Click += new System.EventHandler(btnCropYear_Click);
		this.btnitmcat.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnitmcat.FlatAppearance.BorderSize = 0;
		this.btnitmcat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnitmcat.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnitmcat.ForeColor = System.Drawing.Color.White;
		this.btnitmcat.Location = new System.Drawing.Point(8, 8);
		this.btnitmcat.Name = "btnitmcat";
		this.btnitmcat.Size = new System.Drawing.Size(127, 35);
		this.btnitmcat.TabIndex = 0;
		this.btnitmcat.Text = "Item Category";
		this.btnitmcat.UseVisualStyleBackColor = false;
		this.btnitmcat.Click += new System.EventHandler(btnitmcat_Click);
		this.btnitmuomtaxsch.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnitmuomtaxsch.FlatAppearance.BorderSize = 0;
		this.btnitmuomtaxsch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnitmuomtaxsch.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnitmuomtaxsch.ForeColor = System.Drawing.Color.White;
		this.btnitmuomtaxsch.Location = new System.Drawing.Point(304, 8);
		this.btnitmuomtaxsch.Name = "btnitmuomtaxsch";
		this.btnitmuomtaxsch.Size = new System.Drawing.Size(168, 35);
		this.btnitmuomtaxsch.TabIndex = 4;
		this.btnitmuomtaxsch.Text = "Item Uom Schedule";
		this.btnitmuomtaxsch.UseVisualStyleBackColor = false;
		this.btnitmuomtaxsch.Click += new System.EventHandler(btnitmuomtaxsch_Click);
		this.btnitmuom.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnitmuom.FlatAppearance.BorderSize = 0;
		this.btnitmuom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnitmuom.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnitmuom.ForeColor = System.Drawing.Color.White;
		this.btnitmuom.Location = new System.Drawing.Point(243, 8);
		this.btnitmuom.Name = "btnitmuom";
		this.btnitmuom.Size = new System.Drawing.Size(55, 35);
		this.btnitmuom.TabIndex = 3;
		this.btnitmuom.Text = "Uom";
		this.btnitmuom.UseVisualStyleBackColor = false;
		this.btnitmuom.Click += new System.EventHandler(btnitmuom_Click);
		this.btnitmtype.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnitmtype.FlatAppearance.BorderSize = 0;
		this.btnitmtype.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnitmtype.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnitmtype.ForeColor = System.Drawing.Color.White;
		this.btnitmtype.Location = new System.Drawing.Point(141, 8);
		this.btnitmtype.Name = "btnitmtype";
		this.btnitmtype.Size = new System.Drawing.Size(96, 35);
		this.btnitmtype.TabIndex = 2;
		this.btnitmtype.Text = "Item Type";
		this.btnitmtype.UseVisualStyleBackColor = false;
		this.btnitmtype.Click += new System.EventHandler(btnitmtype_Click);
		this.tpItemDefinitionAddItemHistory.Controls.Add(this.panel3);
		this.tpItemDefinitionAddItemHistory.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.tpItemDefinitionAddItemHistory.Location = new System.Drawing.Point(4, 4);
		this.tpItemDefinitionAddItemHistory.Name = "tpItemDefinitionAddItemHistory";
		this.tpItemDefinitionAddItemHistory.Size = new System.Drawing.Size(1042, 407);
		this.tpItemDefinitionAddItemHistory.TabIndex = 6;
		this.tpItemDefinitionAddItemHistory.Text = "History";
		this.tpItemDefinitionAddItemHistory.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.panel8);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1042, 407);
		this.panel3.TabIndex = 0;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 28);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1042, 379);
		this.panel8.TabIndex = 1;
		this.grdhistory.AllowDelete = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupByBoxVisible = false;
		this.grdhistory.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdhistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1042, 379);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 1;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Controls.Add(this.toolStrip2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1042, 28);
		this.panel4.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(980, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grdhistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(52, 27);
		this.ctrlGrdBar1.TabIndex = 69;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1042, 27);
		this.toolStrip2.TabIndex = 68;
		this.toolStrip2.Text = "toolStrip2";
		this.btnLoadAll.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(55, 24);
		this.btnLoadAll.Text = "&New";
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1050, 433);
		base.Controls.Add(this.tabCAddItem);
		base.KeyPreview = true;
		base.Name = "DefineItemPartyProcessing";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DefineItemPartyProcessing_FormClosing);
		base.Load += new System.EventHandler(InvDefrmAddItem_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvDefrmAddItem_KeyDown);
		this.tabCAddItem.ResumeLayout(false);
		this.tpItemDefinitionAddItem.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAllocation).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBaseUnit).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemGroup).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.tpItemDefinitionAddItemHistory.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
