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
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Inventory_Definition;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Taxation;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.FixedAsset;

public class frmDefinceAssets : Form
{
	private bool DoHaveSaveRight;

	private bool DoHaveUpdateRights;

	private bool DoHaveCanViewAllRecordRights;

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private DataTable dtAllCoaAccounts = new DataTable();

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

	private Panel panel2;

	private Panel panel6;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnattachment;

	private Panel panel1;

	private TabPage tpItemDefinitionAddItemHistory;

	private Panel panel3;

	private Panel panel4;

	internal GridEX grdhistory;

	private Panel panel9;

	private Button btnsearch;

	private Label label32;

	private Panel panel10;

	private Label label31;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnimg1;

	private Button btnimg2;

	private PictureBox picbox2;

	private PictureBox picbox1;

	private ToolStripButton toolStripButton1;

	private Panel panel7;

	private CheckBox chkstatus;

	private TextBox txtAssetsCode;

	private TextBox txtAssetsName;

	private Label label6;

	private Label label5;

	private Label label4;

	private Label label3;

	private Label label2;

	private Label label1;

	internal UltraCombo cmbAssetsClass;

	internal UltraCombo cmbAssetsType;

	internal UltraCombo cmbBaseUnit;

	internal UltraCombo cmbAssetsCategory;

	internal UltraCombo cmbitemcathistory;

	private Button btnLoadAll;

	internal UltraCombo CmbAssetGl;

	private Label label10;

	internal UltraCombo cmbAccumolativeACC;

	private Label label7;

	internal UltraCombo cmbDepriciationExpensesACC;

	private Label label8;

	private Button btnFormBaseUnit;

	private Button btnFormAssetsType;

	private GroupBox groupBox1;

	internal GridEX grdAllocation;

	private Button btnDefineAssets;

	public frmDefinceAssets(UserAccount pUserAccount)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void InvDefrmAddItem_Load(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			List<tblUserRights> lstRights = tblUserRights.GetByUserId(new tblUserRights
			{
				UserId = UserAccount.ID,
				ScreenName = base.Name,
				RightName = UserAccount.RoleName
			});
			if (lstRights.Count > 0)
			{
				List<tblUserRights> newList = lstRights.Where((tblUserRights x) => x.RightName == "Save").ToList();
				DoHaveSaveRight = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Update").ToList();
				DoHaveUpdateRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "CanView AllRecord").ToList();
				DoHaveCanViewAllRecordRights = Conversion.ToBool((object)newList[0].Value.ToString());
			}
			btnsave.Enabled = DoHaveSaveRight;
			btnupdate.Enabled = DoHaveUpdateRights;
			dtViewAttachments.Columns.Add("AttchmentPath");
			btnsave.Visible = true;
			btnupdate.Visible = false;
			cmbAssetsCategory.Focus();
			DepriciationAccountFill();
			AccountBind();
			ItemTypeFill();
			ItemClassFill();
			ItemCatagoryHistoryFill();
			BaseUnitFill();
			CompaniesBindInGrid();
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
			ItemCategory cat = new ItemCategory();
			cat.OrganizationId = UserAccount.OrganizationId;
			cat.CompanyId = UserAccount.CompanyId;
			dtCatergory = ItemCategory.FormHistoryForAssetsCategory(cat);
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
					DDL.BindDDL(dtcath, cmbitemcathistory, "Id", "CategoryDescription", "Asset Category", ZeroIndex: false);
					DDL.BindDDL(dtcath, cmbAssetsCategory, "Id", "CategoryDescription", "Asset Category", ZeroIndex: false);
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
		try
		{
			DataTable dt = CommonServices.GetItemTypeForComboServiceBind(29);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbAssetsType, "Id", "TypeDescription", "Asset Type", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemClassFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new ItemClass
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dt = ItemClass.GetAll();
			if (dt.Rows.Count > 0)
			{
				DataTable dtType = new DataTable();
				dtType.Columns.Add("ClassId");
				dtType.Columns.Add("ClassDescription");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtType.Rows.Add(dt.Rows[i]["ClassId"], dt.Rows[i]["ClassDescription"]);
				}
				if (dtType.Rows.Count > 0)
				{
					DDL.BindDDL(dtType, cmbAssetsClass, "ClassId", "ClassDescription", "Asset Class", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DepriciationAccountFill()
	{
		try
		{
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("14");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbDepriciationExpensesACC, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountBind()
	{
		try
		{
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("1");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbAccumolativeACC, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				DDL.BindDDL(dt, CmbAssetGl, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
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

	private void cmbItemCategory_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_007d: Expected O, but got Unknown
		try
		{
			Item val = new Item
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemCategoryId = Convert.ToInt32(cmbAssetsCategory.Value)
			};
			DataTable lst = Item.GenerateCode(val);
			if (lst.Rows.Count > 0)
			{
				txtAssetsCode.Text = lst.Rows[0]["ItemCode"].ToString();
			}
			DataTable dt = Item.GetGLAccountbyItemCategoryIdForFixedAssets(val);
			if (dt.Rows.Count > 0)
			{
				cmbAccumolativeACC.Value = dt.Rows[0]["accumulatedDepreciationAcId"].ToString();
				cmbDepriciationExpensesACC.Value = dt.Rows[0]["DepreciationExpenseAcId"].ToString();
				CmbAssetGl.Value = dt.Rows[0]["CapitalWipAcId"].ToString();
			}
			((UltraGridBase)cmbAssetsClass).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsearch_Click(object sender, EventArgs e)
	{
		try
		{
			grdfrmfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationForAddItem()
	{
		if (txtAssetsName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Item Name Field is Required");
			txtAssetsName.Focus();
			return false;
		}
		if (((UltraGridBase)cmbBaseUnit).ActiveRow == null)
		{
			MessageBox.Show("Base Unit Field is Required");
			cmbBaseUnit.Focus();
			return false;
		}
		if (((UltraGridBase)cmbAssetsCategory).ActiveRow == null)
		{
			MessageBox.Show("Item Category Field is Required");
			cmbAssetsCategory.Focus();
			return false;
		}
		if (((UltraGridBase)cmbAssetsType).ActiveRow == null)
		{
			MessageBox.Show("Item Type Field is Required");
			cmbAssetsType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbAssetsClass).ActiveRow == null)
		{
			MessageBox.Show("Item Class Field is Required");
			cmbAssetsClass.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Expected O, but got Unknown
		try
		{
			if (!FormValidationForAddItem())
			{
				return;
			}
			if (RecId == 0)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			try
			{
				Item items = new Item();
				items.Id = RecId;
				items.ItemCode = Conversion.ToString((object)txtAssetsCode.Text.Trim());
				items.ItemName = Conversion.ToString((object)txtAssetsName.Text.Trim());
				items.BaseUnitId = Conversion.ToInt(cmbBaseUnit.Value);
				items.Equivalent = Conversion.ToDouble((object)((Control)(object)cmbBaseUnit).Text.Trim());
				items.ItemCategoryId = Conversion.ToInt(cmbAssetsCategory.Value);
				items.ItemTypeId = Conversion.ToInt(cmbAssetsType.Value);
				items.ItemGroupId = 0;
				items.ItemClassId = Conversion.ToInt(cmbAssetsClass.Value);
				items.DepreciationExpenseAcId = Conversion.ToInt(cmbDepriciationExpensesACC.Value);
				items.accumulatedDepreciationAcId = Conversion.ToInt(cmbAccumolativeACC.Value);
				items.CapitalWipAcId = Conversion.ToInt(CmbAssetGl.Value);
				items.ItemStatus = Conversion.ToBool((object)chkstatus.Checked);
				items.Pic1 = Conversion.ToString((object)fileSavePath);
				items.Pic2 = Conversion.ToString((object)fileSavePath2);
				items.OrganizationId = UserAccount.OrganizationId;
				items.CompanyId = UserAccount.CompanyId;
				items.EntryUser = UserAccount.ID;
				items.BranchesId = UserAccount.BranchesId;
				items.AttachmentsList = new List<DMSAttachments>();
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
				for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
				{
					DMSAttachments attach = new DMSAttachments();
					attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
					attach.ScreenName = base.Name;
					items.AttachmentsList.Add(attach);
				}
				Item.Save(items, true);
				if (RecId > 0)
				{
					MessageBox.Show("Update Successfully");
				}
				else
				{
					MessageBox.Show("Save Successfully");
				}
				grdfrmfill();
				Reset();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
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
			txtAssetsName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnupdate.Visible = true;
			Item items = new Item();
			RecId = Conversion.ToInt((object)grdhistory.CurrentRow.Cells[1].Value.ToString());
			items = Item.GetByID(RecId);
			items.Id = RecId;
			tabCAddItem.SelectedIndex = 0;
			txtAssetsCode.Text = Conversion.ToString((object)items.ItemCode);
			txtAssetsName.Text = Conversion.ToString((object)items.ItemName);
			cmbBaseUnit.Value = Conversion.ToInt((object)items.BaseUnitId);
			cmbDepriciationExpensesACC.Value = Conversion.ToInt((object)items.DepreciationExpenseAcId);
			cmbAccumolativeACC.Value = Conversion.ToInt((object)items.accumulatedDepreciationAcId);
			CmbAssetGl.Value = Conversion.ToInt((object)items.CapitalWipAcId);
			cmbAssetsCategory.Value = items.ItemCategoryId;
			cmbAssetsType.Value = items.ItemTypeId;
			cmbAssetsClass.Value = items.ItemClassId;
			chkstatus.Checked = Conversion.ToBool((object)items.ItemStatus);
			fileSavePath = items.Pic1;
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
			CompaniesBindInGrid();
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
			txtAssetsCode.Text = string.Empty;
			txtAssetsName.Text = string.Empty;
			((Control)(object)cmbAssetsClass).Text = string.Empty;
			((Control)(object)cmbBaseUnit).Text = string.Empty;
			((Control)(object)cmbAssetsType).Text = string.Empty;
			((Control)(object)cmbAccumolativeACC).Text = string.Empty;
			((Control)(object)cmbDepriciationExpensesACC).Text = string.Empty;
			((Control)(object)CmbAssetGl).Text = string.Empty;
			picbox1.Image = null;
			picbox2.Image = null;
			picbox1.Invalidate();
			picbox2.Invalidate();
			cmbItemCategory_Leave(null, null);
			btnsave.Visible = true;
			btnupdate.Visible = false;
			RecId = 0;
			txtAssetsName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrmfill(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters reports = new ReportsParameters();
			reports.OrganizationId = UserAccount.OrganizationId;
			reports.CompanyId = UserAccount.CompanyId;
			reports.NoOfRecords = NoOfRecords;
			if (Convert.ToInt32(cmbitemcathistory.Value) != 0)
			{
				reports.Id = Convert.ToInt32(cmbitemcathistory.Value);
			}
			reports.ScreenName = base.Name;
			reports.CanViewAllRecord = DoHaveCanViewAllRecordRights;
			if (!reports.CanViewAllRecord)
			{
				reports.EntryUser = UserAccount.ID;
			}
			DataTable grdlst = Item.FormHistoryForFixedAssets(reports);
			if (grdlst.Rows.Count > 0)
			{
				grdhistory.DataSource = grdlst;
				grdhistory.RetrieveStructure();
				gridsetting();
			}
			else
			{
				grdhistory.ClearStructure();
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
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns.Add("Select");
			grdhistory.RootTable.Columns["Select"].ActAsSelector = true;
			grdhistory.RootTable.Columns["Select"].UseHeaderSelector = true;
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

	private void definition()
	{
		string value = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Attachment Folder Path").ToList()[0].ConfigKey.ToString();
		if (value != null)
		{
			saveDirectory = value;
			flag = true;
		}
		else
		{
			MessageBox.Show("Please Create the Path in Configration");
		}
	}

	private void btnimg1_Click(object sender, EventArgs e)
	{
		definition();
		if (flag)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "(*.jpg; *.jpeg; *.gif; *.PNG; *.png)| *.jpg; *.jpeg; *.gif; *.PNG;*.png; ";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string fileName = Path.GetFileName(openFileDialog.FileName);
				fileSavePath = Path.Combine(saveDirectory, fileName);
				picbox1.Image = new Bitmap(openFileDialog.FileName);
			}
		}
	}

	private void btnimg2_Click(object sender, EventArgs e)
	{
		definition();
		if (flag)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "(*.jpg; *.jpeg; *.gif; *.PNG; *.png)| *.jpg; *.jpeg; *.gif; *.PNG;*.png; ";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string fileName = Path.GetFileName(openFileDialog.FileName);
				fileSavePath2 = Path.Combine(saveDirectory, fileName);
				picbox2.Image = new Bitmap(openFileDialog.FileName);
			}
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
	}

	private void btnitmtaxsch_Click(object sender, EventArgs e)
	{
		new InvDeffrmItemTaxSchdule(UserAccount).Show();
	}

	private void btnitmtype_Click(object sender, EventArgs e)
	{
	}

	private void btnitmuom_Click(object sender, EventArgs e)
	{
	}

	private void btnitmuomtaxsch_Click(object sender, EventArgs e)
	{
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			AccountBind();
			DepriciationAccountFill();
			ItemTypeFill();
			ItemClassFill();
			ItemCatagoryHistoryFill();
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
			grdfrmfill();
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

	private void btnUomUpdate_Click(object sender, EventArgs e)
	{
	}

	private void btnFormBaseUnit_Click(object sender, EventArgs e)
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

	private void btnFormAssetsType_Click(object sender, EventArgs e)
	{
		try
		{
			InvDeffrmItemType invDeffrmItemType = new InvDeffrmItemType(UserAccount);
			invDeffrmItemType.FormTypeId = 3;
			invDeffrmItemType.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefineAssets_Click(object sender, EventArgs e)
	{
		try
		{
			InvDeffrmItemCatagory invDeffrmItemCatagory = new InvDeffrmItemCatagory(UserAccount);
			invDeffrmItemCatagory.ParentCategoryId = 5;
			invDeffrmItemCatagory.Show();
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
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Expected O, but got Unknown
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Expected O, but got Unknown
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Expected O, but got Unknown
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Expected O, but got Unknown
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Expected O, but got Unknown
		//IL_391c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3926: Expected O, but got Unknown
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance3 = new Appearance();
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
		Appearance appearance16 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.FixedAsset.frmDefinceAssets));
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		this.label13 = new System.Windows.Forms.Label();
		this.tabCAddItem = new System.Windows.Forms.TabControl();
		this.tpItemDefinitionAddItem = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.grdAllocation = new GridEX();
		this.btnFormAssetsType = new System.Windows.Forms.Button();
		this.btnFormBaseUnit = new System.Windows.Forms.Button();
		this.CmbAssetGl = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbAccumolativeACC = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbDepriciationExpensesACC = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.btnimg1 = new System.Windows.Forms.Button();
		this.cmbAssetsClass = new UltraCombo();
		this.btnimg2 = new System.Windows.Forms.Button();
		this.picbox2 = new System.Windows.Forms.PictureBox();
		this.picbox1 = new System.Windows.Forms.PictureBox();
		this.cmbAssetsType = new UltraCombo();
		this.cmbBaseUnit = new UltraCombo();
		this.cmbAssetsCategory = new UltraCombo();
		this.chkstatus = new System.Windows.Forms.CheckBox();
		this.txtAssetsCode = new System.Windows.Forms.TextBox();
		this.txtAssetsName = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
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
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.tpItemDefinitionAddItemHistory = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.cmbitemcathistory = new UltraCombo();
		this.btnsearch = new System.Windows.Forms.Button();
		this.label32 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.btnLoadAll = new System.Windows.Forms.Button();
		this.label31 = new System.Windows.Forms.Label();
		this.btnDefineAssets = new System.Windows.Forms.Button();
		this.tabCAddItem.SuspendLayout();
		this.tpItemDefinitionAddItem.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAllocation).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAssetGl).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccumolativeACC).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepriciationExpensesACC).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAssetsClass).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picbox2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picbox1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAssetsType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBaseUnit).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAssetsCategory).BeginInit();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tpItemDefinitionAddItemHistory.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbitemcathistory).BeginInit();
		this.panel10.SuspendLayout();
		base.SuspendLayout();
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(9, 9);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(110, 23);
		this.label13.TabIndex = 0;
		this.label13.Text = "Fixed Assets";
		this.tabCAddItem.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabCAddItem.Controls.Add(this.tpItemDefinitionAddItem);
		this.tabCAddItem.Controls.Add(this.tpItemDefinitionAddItemHistory);
		this.tabCAddItem.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabCAddItem.Location = new System.Drawing.Point(0, 0);
		this.tabCAddItem.Multiline = true;
		this.tabCAddItem.Name = "tabCAddItem";
		this.tabCAddItem.SelectedIndex = 0;
		this.tabCAddItem.Size = new System.Drawing.Size(1111, 616);
		this.tabCAddItem.TabIndex = 0;
		this.tabCAddItem.SelectedIndexChanged += new System.EventHandler(tabCAddItem_SelectedIndexChanged);
		this.tpItemDefinitionAddItem.BackColor = System.Drawing.Color.White;
		this.tpItemDefinitionAddItem.Controls.Add(this.panel1);
		this.tpItemDefinitionAddItem.Controls.Add(this.panel2);
		this.tpItemDefinitionAddItem.Controls.Add(this.panel6);
		this.tpItemDefinitionAddItem.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tpItemDefinitionAddItem.Location = new System.Drawing.Point(4, 4);
		this.tpItemDefinitionAddItem.Name = "tpItemDefinitionAddItem";
		this.tpItemDefinitionAddItem.Padding = new System.Windows.Forms.Padding(3);
		this.tpItemDefinitionAddItem.Size = new System.Drawing.Size(1103, 590);
		this.tpItemDefinitionAddItem.TabIndex = 0;
		this.tpItemDefinitionAddItem.Text = "Form";
		this.panel1.Controls.Add(this.panel7);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 76);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1097, 511);
		this.panel1.TabIndex = 0;
		this.panel7.Controls.Add(this.btnDefineAssets);
		this.panel7.Controls.Add(this.groupBox1);
		this.panel7.Controls.Add(this.btnFormAssetsType);
		this.panel7.Controls.Add(this.btnFormBaseUnit);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAssetGl);
		this.panel7.Controls.Add(this.label10);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccumolativeACC);
		this.panel7.Controls.Add(this.label7);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDepriciationExpensesACC);
		this.panel7.Controls.Add(this.label8);
		this.panel7.Controls.Add(this.btnimg1);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAssetsClass);
		this.panel7.Controls.Add(this.btnimg2);
		this.panel7.Controls.Add(this.picbox2);
		this.panel7.Controls.Add(this.picbox1);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAssetsType);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBaseUnit);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAssetsCategory);
		this.panel7.Controls.Add(this.chkstatus);
		this.panel7.Controls.Add(this.txtAssetsCode);
		this.panel7.Controls.Add(this.txtAssetsName);
		this.panel7.Controls.Add(this.label6);
		this.panel7.Controls.Add(this.label5);
		this.panel7.Controls.Add(this.label4);
		this.panel7.Controls.Add(this.label3);
		this.panel7.Controls.Add(this.label2);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1097, 295);
		this.panel7.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.grdAllocation);
		this.groupBox1.Location = new System.Drawing.Point(9, 150);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(764, 141);
		this.groupBox1.TabIndex = 108;
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
		((System.Windows.Forms.Control)(object)this.grdAllocation).Size = new System.Drawing.Size(758, 119);
		((System.Windows.Forms.Control)(object)this.grdAllocation).TabIndex = 0;
		this.grdAllocation.TotalRow = (InheritableBoolean)1;
		this.grdAllocation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAllocation).VisualStyle = (VisualStyle)4;
		this.btnFormAssetsType.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnFormAssetsType.Location = new System.Drawing.Point(849, 60);
		this.btnFormAssetsType.Name = "btnFormAssetsType";
		this.btnFormAssetsType.Size = new System.Drawing.Size(30, 26);
		this.btnFormAssetsType.TabIndex = 7;
		this.btnFormAssetsType.UseVisualStyleBackColor = true;
		this.btnFormAssetsType.Click += new System.EventHandler(btnFormAssetsType_Click);
		this.btnFormBaseUnit.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnFormBaseUnit.Location = new System.Drawing.Point(422, 60);
		this.btnFormBaseUnit.Name = "btnFormBaseUnit";
		this.btnFormBaseUnit.Size = new System.Drawing.Size(30, 26);
		this.btnFormBaseUnit.TabIndex = 107;
		this.btnFormBaseUnit.UseVisualStyleBackColor = true;
		this.btnFormBaseUnit.Click += new System.EventHandler(btnFormBaseUnit_Click);
		this.CmbAssetGl.AllowNull = (DefaultableBoolean)1;
		this.CmbAssetGl.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAssetGl.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAssetGl.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAssetGl).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.CmbAssetGl).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance24).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAssetGl).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAssetGl).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.CmbAssetGl.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAssetGl).Location = new System.Drawing.Point(200, 118);
		((System.Windows.Forms.Control)(object)this.CmbAssetGl).Name = "CmbAssetGl";
		((System.Windows.Forms.Control)(object)this.CmbAssetGl).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.CmbAssetGl).TabIndex = 8;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(16, 124);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(79, 15);
		this.label10.TabIndex = 106;
		this.label10.Text = "Assets GL A/c";
		this.cmbAccumolativeACC.AllowNull = (DefaultableBoolean)1;
		this.cmbAccumolativeACC.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccumolativeACC.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccumolativeACC.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccumolativeACC).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbAccumolativeACC.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAccumolativeACC).Location = new System.Drawing.Point(200, 89);
		((System.Windows.Forms.Control)(object)this.cmbAccumolativeACC).Name = "cmbAccumolativeACC";
		((System.Windows.Forms.Control)(object)this.cmbAccumolativeACC).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccumolativeACC).TabIndex = 6;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(16, 95);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(172, 15);
		this.label7.TabIndex = 105;
		this.label7.Text = "Accumulated Depreciation A/c";
		this.cmbDepriciationExpensesACC.AllowNull = (DefaultableBoolean)1;
		this.cmbDepriciationExpensesACC.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDepriciationExpensesACC.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDepriciationExpensesACC.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDepriciationExpensesACC).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDepriciationExpensesACC).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbDepriciationExpensesACC.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDepriciationExpensesACC).Location = new System.Drawing.Point(616, 89);
		((System.Windows.Forms.Control)(object)this.cmbDepriciationExpensesACC).Name = "cmbDepriciationExpensesACC";
		((System.Windows.Forms.Control)(object)this.cmbDepriciationExpensesACC).Size = new System.Drawing.Size(231, 26);
		((System.Windows.Forms.Control)(object)this.cmbDepriciationExpensesACC).TabIndex = 7;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(460, 95);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(145, 15);
		this.label8.TabIndex = 104;
		this.label8.Text = "Depreciation Expense A/c";
		this.btnimg1.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnimg1.Location = new System.Drawing.Point(803, 255);
		this.btnimg1.Name = "btnimg1";
		this.btnimg1.Size = new System.Drawing.Size(75, 27);
		this.btnimg1.TabIndex = 13;
		this.btnimg1.Text = "Browse";
		this.btnimg1.UseVisualStyleBackColor = true;
		this.btnimg1.Click += new System.EventHandler(btnimg1_Click);
		this.cmbAssetsClass.AllowNull = (DefaultableBoolean)1;
		this.cmbAssetsClass.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAssetsClass.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAssetsClass.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.Appearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAssetsClass).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.cmbAssetsClass).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance45).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAssetsClass).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAssetsClass).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbAssetsClass.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAssetsClass).Location = new System.Drawing.Point(616, 32);
		((System.Windows.Forms.Control)(object)this.cmbAssetsClass).Name = "cmbAssetsClass";
		((System.Windows.Forms.Control)(object)this.cmbAssetsClass).Size = new System.Drawing.Size(231, 26);
		((System.Windows.Forms.Control)(object)this.cmbAssetsClass).TabIndex = 3;
		this.btnimg2.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnimg2.Location = new System.Drawing.Point(916, 255);
		this.btnimg2.Name = "btnimg2";
		this.btnimg2.Size = new System.Drawing.Size(75, 27);
		this.btnimg2.TabIndex = 14;
		this.btnimg2.Text = "Browse";
		this.btnimg2.UseVisualStyleBackColor = true;
		this.btnimg2.Click += new System.EventHandler(btnimg2_Click);
		this.picbox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.picbox2.Location = new System.Drawing.Point(907, 150);
		this.picbox2.Name = "picbox2";
		this.picbox2.Size = new System.Drawing.Size(111, 100);
		this.picbox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picbox2.TabIndex = 36;
		this.picbox2.TabStop = false;
		this.picbox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.picbox1.Location = new System.Drawing.Point(790, 150);
		this.picbox1.Name = "picbox1";
		this.picbox1.Size = new System.Drawing.Size(111, 100);
		this.picbox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picbox1.TabIndex = 35;
		this.picbox1.TabStop = false;
		this.cmbAssetsType.AllowNull = (DefaultableBoolean)1;
		this.cmbAssetsType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAssetsType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAssetsType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAssetsType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbAssetsType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance4).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAssetsType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAssetsType).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbAssetsType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAssetsType).Location = new System.Drawing.Point(616, 60);
		((System.Windows.Forms.Control)(object)this.cmbAssetsType).Name = "cmbAssetsType";
		((System.Windows.Forms.Control)(object)this.cmbAssetsType).Size = new System.Drawing.Size(231, 26);
		((System.Windows.Forms.Control)(object)this.cmbAssetsType).TabIndex = 5;
		this.cmbBaseUnit.AllowNull = (DefaultableBoolean)1;
		this.cmbBaseUnit.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBaseUnit.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBaseUnit.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance51).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbBaseUnit.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Location = new System.Drawing.Point(200, 60);
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Name = "cmbBaseUnit";
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).TabIndex = 4;
		this.cmbAssetsCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbAssetsCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAssetsCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAssetsCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAssetsCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAssetsCategory).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbAssetsCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAssetsCategory).Location = new System.Drawing.Point(200, 4);
		((System.Windows.Forms.Control)(object)this.cmbAssetsCategory).Name = "cmbAssetsCategory";
		((System.Windows.Forms.Control)(object)this.cmbAssetsCategory).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.cmbAssetsCategory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbAssetsCategory).Leave += new System.EventHandler(cmbItemCategory_Leave);
		this.chkstatus.AutoSize = true;
		this.chkstatus.Checked = true;
		this.chkstatus.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkstatus.Location = new System.Drawing.Point(422, 125);
		this.chkstatus.Name = "chkstatus";
		this.chkstatus.Size = new System.Drawing.Size(71, 19);
		this.chkstatus.TabIndex = 9;
		this.chkstatus.Text = "Is Active";
		this.chkstatus.UseVisualStyleBackColor = true;
		this.txtAssetsCode.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtAssetsCode.Location = new System.Drawing.Point(616, 6);
		this.txtAssetsCode.Name = "txtAssetsCode";
		this.txtAssetsCode.ReadOnly = true;
		this.txtAssetsCode.Size = new System.Drawing.Size(231, 23);
		this.txtAssetsCode.TabIndex = 1;
		this.txtAssetsCode.TabStop = false;
		this.txtAssetsName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAssetsName.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtAssetsName.Location = new System.Drawing.Point(200, 34);
		this.txtAssetsName.Name = "txtAssetsName";
		this.txtAssetsName.Size = new System.Drawing.Size(219, 23);
		this.txtAssetsName.TabIndex = 2;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label6.Location = new System.Drawing.Point(460, 38);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(75, 15);
		this.label6.TabIndex = 5;
		this.label6.Text = "Assets Class";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label5.Location = new System.Drawing.Point(460, 66);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(69, 15);
		this.label5.TabIndex = 4;
		this.label5.Text = "Assets Type";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label4.Location = new System.Drawing.Point(16, 10);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(93, 15);
		this.label4.TabIndex = 3;
		this.label4.Text = "Assets Category";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label3.Location = new System.Drawing.Point(16, 66);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(59, 15);
		this.label3.TabIndex = 2;
		this.label3.Text = "Base Unit";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label2.Location = new System.Drawing.Point(16, 38);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(76, 15);
		this.label2.TabIndex = 1;
		this.label2.Text = "Assets Name";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label1.Location = new System.Drawing.Point(458, 10);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(72, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Assets Code";
		this.panel2.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel2.Controls.Add(this.label13);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 35);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1097, 41);
		this.panel2.TabIndex = 6;
		this.panel6.Controls.Add(this.toolStrip1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1097, 32);
		this.panel6.TabIndex = 5;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnupdate, this.btnsave, this.btnattachment, this.toolStripButton1 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1097, 27);
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
		this.tpItemDefinitionAddItemHistory.Controls.Add(this.panel3);
		this.tpItemDefinitionAddItemHistory.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.tpItemDefinitionAddItemHistory.Location = new System.Drawing.Point(4, 4);
		this.tpItemDefinitionAddItemHistory.Name = "tpItemDefinitionAddItemHistory";
		this.tpItemDefinitionAddItemHistory.Size = new System.Drawing.Size(1103, 590);
		this.tpItemDefinitionAddItemHistory.TabIndex = 6;
		this.tpItemDefinitionAddItemHistory.Text = "History";
		this.tpItemDefinitionAddItemHistory.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Controls.Add(this.panel9);
		this.panel3.Controls.Add(this.panel10);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1103, 590);
		this.panel3.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 95);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1103, 495);
		this.panel4.TabIndex = 69;
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
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1103, 495);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 70;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		this.panel9.BackColor = System.Drawing.Color.White;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitemcathistory);
		this.panel9.Controls.Add(this.btnsearch);
		this.panel9.Controls.Add(this.label32);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 40);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1103, 55);
		this.panel9.TabIndex = 68;
		this.cmbitemcathistory.AllowNull = (DefaultableBoolean)1;
		this.cmbitemcathistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitemcathistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitemcathistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance86;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance92).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitemcathistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Location = new System.Drawing.Point(160, 16);
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Name = "cmbitemcathistory";
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).TabIndex = 16;
		this.btnsearch.FlatAppearance.BorderSize = 0;
		this.btnsearch.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.btnsearch.Location = new System.Drawing.Point(392, 16);
		this.btnsearch.Name = "btnsearch";
		this.btnsearch.Size = new System.Drawing.Size(90, 26);
		this.btnsearch.TabIndex = 6;
		this.btnsearch.Text = "Search";
		this.btnsearch.UseVisualStyleBackColor = true;
		this.btnsearch.Click += new System.EventHandler(btnsearch_Click);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label32.Location = new System.Drawing.Point(13, 20);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(136, 17);
		this.label32.TabIndex = 0;
		this.label32.Text = "Fixed Assets Category";
		this.panel10.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel10.Controls.Add(this.btnLoadAll);
		this.panel10.Controls.Add(this.label31);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1103, 40);
		this.panel10.TabIndex = 65;
		this.btnLoadAll.FlatAppearance.BorderSize = 0;
		this.btnLoadAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnLoadAll.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.ForeColor = System.Drawing.Color.White;
		this.btnLoadAll.Location = new System.Drawing.Point(11, 6);
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(62, 26);
		this.btnLoadAll.TabIndex = 17;
		this.btnLoadAll.Text = "LoadAll";
		this.btnLoadAll.UseVisualStyleBackColor = true;
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.White;
		this.label31.Location = new System.Drawing.Point(90, 7);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(172, 23);
		this.label31.TabIndex = 0;
		this.label31.Text = "Fixed Assets History";
		this.btnDefineAssets.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefineAssets.Location = new System.Drawing.Point(422, 4);
		this.btnDefineAssets.Name = "btnDefineAssets";
		this.btnDefineAssets.Size = new System.Drawing.Size(30, 26);
		this.btnDefineAssets.TabIndex = 109;
		this.btnDefineAssets.UseVisualStyleBackColor = true;
		this.btnDefineAssets.Click += new System.EventHandler(btnDefineAssets_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1111, 616);
		base.Controls.Add(this.tabCAddItem);
		base.KeyPreview = true;
		base.Name = "frmDefinceAssets";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(InvDefrmAddItem_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvDefrmAddItem_KeyDown);
		this.tabCAddItem.ResumeLayout(false);
		this.tpItemDefinitionAddItem.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAllocation).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAssetGl).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccumolativeACC).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepriciationExpensesACC).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAssetsClass).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picbox2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picbox1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAssetsType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBaseUnit).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAssetsCategory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tpItemDefinitionAddItemHistory.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbitemcathistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		base.ResumeLayout(false);
	}
}
