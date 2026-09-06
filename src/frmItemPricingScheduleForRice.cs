// using Architecture.Reports;
using System;
using System.Collections;
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
using Architecture.WinApp.Common;
using Architecture.WinApp.Production;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.Lookups
{

public class frmItemPricingScheduleForRice : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtCategory = new DataTable();

	private DataTable dtItemType = new DataTable();

	private DataTable dt = new DataTable();

	private Rightsobjects formrights = new Rightsobjects();

	private int RecId;

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel5;

	private Panel panel4;

	private Panel panel3;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private Panel panel6;

	private Label label15;

	private Panel panel7;

	private TextBox txtNewPrice;

	private Label label9;

	internal UltraCombo cmbItem;

	private Button btnAdd;

	internal GridEX grd;

	private Panel panel9;

	internal GridEX ActiveGrid;

	private Panel panel8;

	internal UltraCombo CmbRateUom;

	private Label label2;

	internal UltraCombo CmbParentCatagory;

	private Label label4;

	private TextBox txtLastRate;

	private Label label5;

	private GroupBox groupBox2;

	private Label label6;

	private DateTimePicker datEffectedTime;

	private Label label7;

	private Label label8;

	private DateTimePicker txtEffectedDate;

	private GroupBox groupBox1;

	private Button btnSearch;

	internal UltraCombo cmbItemType;

	private Label label12;

	internal UltraCombo cmbItemCategory;

	private Label label13;

	private Label label1;

	private TextBox txtApplyRateUom;

	private Label label3;

	private DateTimePicker datEffectedForFilter;

	private CheckBox chkApplyDate;

	private Label label10;

	private DateTimePicker datPreviousDate;

	private Label label11;

	internal UltraCombo cmbCropYear;

	private ToolStripButton btnrefresh;

	private ToolStripButton btnRecepi;

	internal UltraCombo CmbItemTypeHistory;

	private Label label14;

	internal UltraCombo CmbItemCategoryHistory;

	private Label label16;

	private Label label17;

	private DateTimePicker datEffectedDateHistory;

	private CheckBox CbOnlyShowRatedItems;

	private Label label18;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel10;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStrip toolStrip2;

	private ToolStripButton btnPrint325;

	private Panel panel12;

	private Label label19;

	private Panel panel11;

	private GroupBox groupBox3;

	internal UltraCombo CmbCropYearHistory;

	private Label label20;

	private Label label21;

	internal UltraCombo CmbParentCategoryHistory;

	private Button btnShow;

	private Panel panel13;

	private Label label22;

	private Panel panel14;

	private Panel panel15;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label23;

	internal GridEX InActiveGrid;

	private ToolStripButton btnRefreshHistory;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnPrintForInactive;

	internal UltraCombo CmbItemHistory;

	private Label label24;

	private Button btnDoActiveSchedule;

	private Button btnDelSchedule;

	public frmItemPricingScheduleForRice(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmItemPricingSchedule_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1300, 700);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			dt.Columns.Add("ItemId");
			dt.Columns.Add("RateUomId");
			dt.Columns.Add("BaseUomId");
			dt.Columns.Add("ItemCategory");
			dt.Columns.Add("ItemName");
			dt.Columns.Add("CropYearId");
			dt.Columns.Add("BaseUom");
			dt.Columns.Add("PreviousDate", typeof(DateTime));
			dt.Columns.Add("PreviousRate", typeof(double));
			dt.Columns.Add("RateUom");
			dt.Columns.Add("NewPrice", typeof(double));
			ParentCategoryBind();
			CropYearbind();
			ItemCategoryAndTypeFill();
			ItemFill(Conversion.ToInt(CmbParentCatagory.Value));
			CmbParentCatagory_Leave(null, null);
			HistoryComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = ItemPricingScheduleForRice.GetDataForDropDownFromIPSForRice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtParent = new DataTable();
			dtParent.Columns.Add("Id", typeof(int));
			dtParent.Columns.Add("Name", typeof(string));
			DataTable dtItemCategory = new DataTable();
			dtItemCategory.Columns.Add("Id", typeof(int));
			dtItemCategory.Columns.Add("Name", typeof(string));
			DataTable dtItemType = new DataTable();
			dtItemType.Columns.Add("Id", typeof(int));
			dtItemType.Columns.Add("Name", typeof(string));
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("Name", typeof(string));
			DataTable dtCrop = new DataTable();
			dtCrop.Columns.Add("Id", typeof(int));
			dtCrop.Columns.Add("Name", typeof(string));
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ParentCategories")
				{
					dtParent.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ItemCategories")
				{
					dtItemCategory.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ItemTypes")
				{
					dtItemType.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Item")
				{
					dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "CropYear")
				{
					dtCrop.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			DDL.BindDDL(dtParent, CmbParentCategoryHistory, "Id", "Name", "Parent Category", ZeroIndex: false);
			DDL.BindDDL(dtItemCategory, CmbItemCategoryHistory, "Id", "Name", "Item Category", ZeroIndex: false);
			DDL.BindDDL(dtItemType, CmbItemTypeHistory, "Id", "Name", "Item Type", ZeroIndex: false);
			DDL.BindDDL(dtItem, CmbItemHistory, "Id", "Name", "Item Name", ZeroIndex: false);
			DDL.BindDDL(dtCrop, CmbCropYearHistory, "Id", "Name", "Crop Year", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFill(int ParentId)
	{
		try
		{
			if (ParentId == 0)
			{
				((Control)(object)cmbItem).Text = string.Empty;
				((UltraGridBase)cmbItem).DataSource = null;
				return;
			}
			DataTable dtItem = CommonServices.ItemGetForComboServiceBind(ParentId);
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItem, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
			else
			{
				((Control)(object)cmbItem).Text = string.Empty;
				((UltraGridBase)cmbItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParentCategoryBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = Item.InventoryParentCategories(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtpart = new DataTable();
			dtpart.Columns.Add("Id");
			dtpart.Columns.Add("InvParentCateDescription");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToInt(dt.Rows[i]["Id"]) == 1 || Conversion.ToInt(dt.Rows[i]["Id"]) == 2 || Conversion.ToInt(dt.Rows[i]["Id"]) == 4)
				{
					dtpart.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["InvParentCateDescription"]);
				}
			}
			DDL.BindDDL(dtpart, CmbParentCatagory, "Id", "InvParentCateDescription", "Parent Category", ZeroIndex: false);
			((UltraGridBase)CmbParentCatagory).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemCategoryAndTypeFill()
	{
		try
		{
			dtCategory = CommonServices.GetItemCategoryForComboServiceBind();
			dtItemType = CommonServices.GetItemTypeForComboServiceBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemCategoryByParentIdFill()
	{
		try
		{
			EnumerableRowCollection<DataRow> rows = from r in dtCategory.AsEnumerable()
				where r.Field<int?>("InventoryParentCategoriesId") == Conversion.ToInt(CmbParentCatagory.Value)
				select r;
			DataTable dt = (rows.Any() ? rows.CopyToDataTable() : dtCategory.Clone());
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbItemCategory, "Id", "CategoryDescription", "Item Category", false);
				((UltraGridBase)cmbItemCategory).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
			else
			{
				DDL.BindDDLNew(dtCategory, cmbItemCategory, "Id", "CategoryDescription", "Item Category", false);
				((UltraGridBase)cmbItemCategory).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemTypeByCategoryIdFill()
	{
		try
		{
			EnumerableRowCollection<DataRow> rows = from r in dtItemType.AsEnumerable()
				where r.Field<int?>("ItemCategoryId") == Conversion.ToInt(cmbItemCategory.Value)
				select r;
			DataTable dt = (rows.Any() ? rows.CopyToDataTable() : dtItemType.Clone());
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbItemType, "Id", "TypeDescription", "ItemType", false);
				((UltraGridBase)cmbItemType).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
			else
			{
				DDL.BindDDLNew(dtItemType, cmbItemType, "Id", "TypeDescription", "ItemType", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYearbind()
	{
		try
		{
			DataTable dtCrop = CommonServices.CropYearGetAllService();
			if (dtCrop.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCrop, cmbCropYear, "Id", "CropYear", "CropYear", false);
			}
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

	private void btnsave_Click(object sender, EventArgs e)
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

	private void frmItemPricingSchedule_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
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
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
				}
				else
				{
					tabControl1.SelectedIndex = 1;
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Length == 0)
			{
				throw new Exception("Grid record not found");
			}
			ItemPricingScheduleForRice obj = new ItemPricingScheduleForRice();
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			obj.ItemPricingScheduleForRicelist = new List<ItemPricingScheduleForRice>();
			GridEXRow[] dataRows = grd.GetDataRows();
			foreach (GridEXRow item in dataRows)
			{
				ItemPricingScheduleForRice objdetail = new ItemPricingScheduleForRice();
				objdetail.EffectiveDate = Conversion.ToDateTime((object)txtEffectedDate.Value);
				string EffDate = Conversion.ToString((object)objdetail.EffectiveDate.ToString("dd/MMM/yyyy"));
				objdetail.EffectiveDate = Convert.ToDateTime(EffDate + " " + datEffectedTime.Value.TimeOfDay.ToString());
				objdetail.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				if ((Conversion.ToInt(item.Cells["CropYearId"].Value) == 0 || Conversion.ToString((object)item.Cells["CropYearId"].Text) == string.Empty) && Conversion.ToInt(item.Cells["NewPrice"].Value) != 0)
				{
					MessageBox.Show(" Crop Year Field Required In Grid ");
					return;
				}
				objdetail.CropYearId = Conversion.ToInt(item.Cells["CropYearId"].Value);
				objdetail.RateUomId = Conversion.ToInt(item.Cells["RateUomId"].Value);
				DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(item.Cells["ItemId"].Value));
				if (dt.Rows.Count > 0)
				{
					DataRow[] dr = dt.Select("Equivalent='" + Conversion.ToInt(item.Cells["RateUom"].Value) + "'");
					if (dr.Length == 0)
					{
						MessageBox.Show(Conversion.ToString(item.Cells["RateUom"].Value) + " RateUom not define against " + Conversion.ToString(item.Cells["ItemName"].Value));
						return;
					}
					objdetail.RateUomId = Conversion.ToInt(dr[0].ItemArray[0]);
				}
				objdetail.ItemRate = Conversion.ToDouble(item.Cells["NewPrice"].Value);
				objdetail.OrganizationId = UserAccount.OrganizationId;
				objdetail.CompanyId = UserAccount.CompanyId;
				objdetail.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				objdetail.EntryUserId = UserAccount.ID;
				objdetail.ModifyUserId = UserAccount.ID;
				objdetail.DocumentTypeId = 182;
				objdetail.EntryDate = DateTime.Now;
				objdetail.ModifyDate = DateTime.Now;
				objdetail.ApprovedDate = DateTime.Now;
				objdetail.IsActive = true;
				obj.ItemPricingScheduleForRicelist.Add(objdetail);
			}
			ItemPricingScheduleForRice.Save(obj);
			MessageBox.Show("Save Successfully");
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FormHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.ParentCategoryId = Conversion.ToInt(CmbParentCategoryHistory.Value);
			obj.ItemCategoryId = Conversion.ToInt(CmbItemCategoryHistory.Value);
			obj.ItemTypeId = Conversion.ToInt(CmbItemTypeHistory.Value);
			obj.ItemId = Conversion.ToInt(CmbItemHistory.Value);
			obj.CropYearId = Conversion.ToInt(CmbCropYearHistory.Value);
			if (datEffectedDateHistory.Checked)
			{
				obj.EffectedDate = datEffectedDateHistory.Value;
			}
			DataTable dtHistory = ItemPricingScheduleForRice.FormHistory(obj);
			if (dtHistory.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> ActiveRows = from row in dtHistory.AsEnumerable()
					where row.Field<bool>("IsActive")
					select row;
				EnumerableRowCollection<DataRow> InActiveRows = from row in dtHistory.AsEnumerable()
					where !row.Field<bool>("IsActive")
					select row;
				DataTable dtActive = (ActiveRows.Any() ? ActiveRows.CopyToDataTable() : dtHistory.Clone());
				DataTable dtInActive = (InActiveRows.Any() ? InActiveRows.CopyToDataTable() : dtHistory.Clone());
				if (dtActive.Rows.Count > 0)
				{
					DataTable dt2 = new DataTable();
					dt2.Columns.Add("Id", typeof(int));
					dt2.Columns.Add("EffectiveDate", typeof(DateTime));
					dt2.Columns.Add("ItemId", typeof(int));
					dt2.Columns.Add("ItemName", typeof(string));
					dt2.Columns.Add("CropYearId", typeof(int));
					dt2.Columns.Add("CropYear", typeof(string));
					dt2.Columns.Add("ItemRate", typeof(double));
					dt2.Columns.Add("RateUom", typeof(string));
					dt2.Columns.Add("EntryDate", typeof(DateTime));
					dt2.Columns.Add("UserName", typeof(string));
					for (int j = 0; j < dtActive.Rows.Count; j++)
					{
						dt2.Rows.Add(dtActive.Rows[j]["Id"], dtActive.Rows[j]["EffectiveDate"], dtActive.Rows[j]["ItemId"], dtActive.Rows[j]["ItemName"], dtActive.Rows[j]["CropYearId"], dtActive.Rows[j]["CropYear"], dtActive.Rows[j]["ItemRate"], dtActive.Rows[j]["RateUom"], dtActive.Rows[j]["EntryDate"], dtActive.Rows[j]["UserName"]);
					}
					ActiveGrid.DataSource = dt2;
					ActiveGrid.RetrieveStructure();
					DataGridHistorySetting();
				}
				else
				{
					ActiveGrid.ClearStructure();
				}
				if (dtInActive.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id", typeof(int));
					dt.Columns.Add("EffectiveDate", typeof(DateTime));
					dt.Columns.Add("ItemId", typeof(int));
					dt.Columns.Add("ItemName", typeof(string));
					dt.Columns.Add("CropYearId", typeof(int));
					dt.Columns.Add("CropYear", typeof(string));
					dt.Columns.Add("ItemRate", typeof(double));
					dt.Columns.Add("RateUom", typeof(string));
					dt.Columns.Add("EntryDate", typeof(DateTime));
					dt.Columns.Add("UserName", typeof(string));
					for (int i = 0; i < dtInActive.Rows.Count; i++)
					{
						dt.Rows.Add(dtInActive.Rows[i]["Id"], dtInActive.Rows[i]["EffectiveDate"], dtInActive.Rows[i]["ItemId"], dtInActive.Rows[i]["ItemName"], dtInActive.Rows[i]["CropYearId"], dtInActive.Rows[i]["CropYear"], dtInActive.Rows[i]["ItemRate"], dtInActive.Rows[i]["RateUom"], dtInActive.Rows[i]["EntryDate"], dtInActive.Rows[i]["UserName"]);
					}
					InActiveGrid.DataSource = dt;
					InActiveGrid.RetrieveStructure();
					GridSettingsForInActive();
				}
				else
				{
					InActiveGrid.ClearStructure();
				}
			}
			else
			{
				ActiveGrid.ClearStructure();
				InActiveGrid.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistorySetting()
	{
		try
		{
			ActiveGrid.RootTable.Columns["Id"].Visible = false;
			ActiveGrid.RootTable.Columns["ItemId"].Visible = false;
			ActiveGrid.RootTable.Columns["CropYearId"].Visible = false;
			ActiveGrid.RootTable.Columns.Add("Select");
			ActiveGrid.RootTable.Columns["Select"].ActAsSelector = true;
			ActiveGrid.RootTable.Columns["Select"].UseHeaderSelector = true;
			ActiveGrid.RootTable.Columns["Select"].Width = 40;
			ActiveGrid.RootTable.Columns["Select"].Position = 0;
			ActiveGrid.FrozenColumns = 1;
			CommonServices.GridAutoAdjustment(AryColWidth, ActiveGrid);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingsForInActive()
	{
		try
		{
			InActiveGrid.RootTable.Columns["Id"].Visible = false;
			InActiveGrid.RootTable.Columns["ItemId"].Visible = false;
			InActiveGrid.RootTable.Columns["CropYearId"].Visible = false;
			InActiveGrid.RootTable.Columns.Add("Select");
			InActiveGrid.RootTable.Columns["Select"].ActAsSelector = true;
			InActiveGrid.RootTable.Columns["Select"].UseHeaderSelector = true;
			InActiveGrid.RootTable.Columns["Select"].Width = 40;
			InActiveGrid.RootTable.Columns["Select"].Position = 0;
			InActiveGrid.FrozenColumns = 1;
			CommonServices.GridAutoAdjustmentNew(InActiveGrid);
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItem_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(cmbItem.Value));
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbRateUom, "Id", "UOMCode", "RateUom", ZeroIndex: false);
				((UltraGridBase)CmbRateUom).DisplayLayout.Bands[0].Columns[1].Width = 200;
			}
			else
			{
				((UltraGridBase)CmbRateUom).DataSource = null;
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
			dt.Rows.Clear();
			grd.ClearStructure();
			((Control)(object)cmbItem).Text = string.Empty;
			txtNewPrice.Text = string.Empty;
			txtLastRate.Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			RecId = 0;
			btnAdd.Enabled = true;
			cmbItem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidation())
			{
				return;
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToInt(item.Cells["ItemId"].Value) == Conversion.ToInt(cmbItem.Value) && Conversion.ToInt(item.Cells["CropYearId"].Value) == Conversion.ToInt(cmbCropYear.Value))
				{
					cmbItem.Focus();
					throw new Exception("You cannot add same item With Same CropYear in grid");
				}
			}
			dt.Rows.Add(Conversion.ToInt(cmbItem.Value), Conversion.ToInt(CmbRateUom.Value), 0, ((UltraDropDownBase)cmbItem).SelectedRow.Cells[2].Value, Conversion.ToString((object)((Control)(object)cmbItem).Text), Conversion.ToInt(cmbCropYear.Value), 0, Conversion.ToDateTime((object)datPreviousDate.Value), Conversion.ToDouble((object)txtLastRate.Text), Conversion.ToInt(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtNewPrice.Text));
			grd.DataSource = dt;
			grd.RetrieveStructure();
			GridSetting();
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ResetDetail()
	{
		try
		{
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			txtNewPrice.Text = string.Empty;
			cmbItem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSetting()
	{
		try
		{
			grd.AllowEdit = (InheritableBoolean)1;
			grd.AllowDelete = (InheritableBoolean)2;
			grd.ColumnAutoResize = false;
			grd.GroupByBoxVisible = true;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["RateUomId"].Visible = false;
			grd.RootTable.Columns["BaseUomId"].Visible = false;
			grd.RootTable.Columns["BaseUom"].Visible = false;
			grd.RootTable.Columns["ItemName"].Width = 350;
			grd.RootTable.Groups.Add("ItemCategory");
			grd.RootTable.Columns["ItemCategory"].Width = 150;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["RateUomId"].EditType = (EditType)0;
			grd.RootTable.Columns["BaseUomId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemCategory"].EditType = (EditType)0;
			grd.RootTable.Columns["PreviousRate"].EditType = (EditType)0;
			grd.RootTable.Columns["BaseUom"].EditType = (EditType)0;
			grd.RootTable.Columns["RateUom"].EditType = (EditType)0;
			grd.RootTable.Columns["PreviousDate"].EditType = (EditType)0;
			grd.RootTable.Columns["NewPrice"].EditType = (EditType)1;
			grd.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["PreviousRate"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["NewPrice"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["PreviousRate"].FormatString = "#,#.##";
			grd.RootTable.Columns["NewPrice"].FormatString = "#,#.##";
			grd.RootTable.Columns["PreviousRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NewPrice"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Caption = "Delete";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].Image = Resources.icons8_delete_bin_16;
			grd.RootTable.Columns["Delete"].Width = 35;
			DataTable dtgrdCropYear = CommonServices.CropYearGetAllService();
			grd.RootTable.Columns["CropYearId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["CropYearId"].HasValueList = true;
			grd.RootTable.Columns["CropYearId"].LimitToList = true;
			grd.RootTable.Columns["CropYearId"].EditType = (EditType)4;
			grd.RootTable.Columns["CropYearId"].Caption = "Crop Year";
			grd.RootTable.Columns["CropYearId"].ValueList.PopulateValueList((IEnumerable)dtgrdCropYear.DefaultView, "Id", "CropYear");
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)cmbItem).ActiveRow == null)
		{
			cmbItem.Focus();
			MessageBox.Show("Item Field Required");
			return false;
		}
		if (((UltraGridBase)cmbCropYear).ActiveRow == null)
		{
			cmbCropYear.Focus();
			MessageBox.Show("Crop Year Field Required");
			return false;
		}
		if (Conversion.ToDouble((object)txtNewPrice.Text) == 0.0)
		{
			txtNewPrice.Focus();
			MessageBox.Show("Price Field Required");
			return false;
		}
		if (((UltraGridBase)CmbRateUom).ActiveRow == null)
		{
			CmbRateUom.Focus();
			MessageBox.Show("RateUom Field Required");
			return false;
		}
		return true;
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				CmbParentCategoryHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
	}

	private void CmbParentCatagory_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemCategoryByParentIdFill();
			ItemFill(Conversion.ToInt(CmbParentCatagory.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbRateUom_Leave(object sender, EventArgs e)
	{
		try
		{
			getLastRateAndDate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void getLastRateAndDate()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		try
		{
			DataTable dt = ItemPricingScheduleForRice.GetLastItemRateByItemIdAndUomId(new ItemPricingScheduleForRice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemId = Conversion.ToInt(cmbItem.Value),
				CropYearId = Conversion.ToInt(cmbCropYear.Value),
				RateUomId = Conversion.ToInt(CmbRateUom.Value)
			});
			if (dt.Rows.Count > 0)
			{
				txtLastRate.Text = Conversion.ToString(dt.Rows[0]["ItemRate"]);
				datPreviousDate.Value = Conversion.ToDateTime(dt.Rows[0]["PreviousDate"]);
			}
			else
			{
				txtLastRate.Text = "0";
				datPreviousDate.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSearch_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters rptPrms = new ReportsParameters();
			rptPrms.OrganizationId = UserAccount.OrganizationId;
			rptPrms.CompanyId = UserAccount.CompanyId;
			rptPrms.InventoryParentCategories = Conversion.ToInt(CmbParentCatagory.Value);
			rptPrms.ItemCategoryId = Conversion.ToInt(cmbItemCategory.Value);
			rptPrms.ItemTypeId = Conversion.ToInt(cmbItemType.Value);
			if (chkApplyDate.Checked)
			{
				rptPrms.EffectedDate = Conversion.ToDateTime((object)datEffectedForFilter.Value);
			}
			DataTable dtData = ItemPricingScheduleForRice.GetItemWithRatesForItemPricingSchedule(rptPrms);
			if (dtData.Rows.Count > 0)
			{
				dt.Rows.Clear();
				double applyRateUom = Conversion.ToDouble((object)txtApplyRateUom.Text.Trim());
				if (applyRateUom == 0.0)
				{
					applyRateUom = 40.0;
				}
				for (int i = 0; i < dtData.Rows.Count; i++)
				{
					if (Conversion.ToDouble(dtData.Rows[i]["RateUom"]) != 0.0 && applyRateUom == 0.0)
					{
						applyRateUom = Conversion.ToDouble(dtData.Rows[i]["RateUom"]);
					}
					dt.Rows.Add(dtData.Rows[i]["ItemId"], dtData.Rows[i]["RateUomId"], dtData.Rows[i]["BaseUomId"], dtData.Rows[i]["CategoryDescription"], dtData.Rows[i]["ItemName"], dtData.Rows[i]["CropYearId"], dtData.Rows[i]["BaseUom"], dtData.Rows[i]["PreviousDate"], dtData.Rows[i]["PreviousRate"], applyRateUom, 0);
				}
				if (CbOnlyShowRatedItems.Checked)
				{
					EnumerableRowCollection<DataRow> rows = from row in dt.AsEnumerable()
						where row.Field<double>("PreviousRate") != 0.0
						select row;
					dt = (rows.Any() ? rows.CopyToDataTable() : dt.Clone());
				}
				if (dt.Rows.Count > 0)
				{
					grd.DataSource = dt;
					grd.RetrieveStructure();
					GridSetting();
				}
			}
			else
			{
				grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItemCategory_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemTypeByCategoryIdFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void checkBox1_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (chkApplyDate.Checked)
			{
				datEffectedForFilter.Enabled = true;
			}
			else
			{
				datEffectedForFilter.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (btnsave.Visible && btnsave.Enabled)
			{
				if (e.Column.Key == "Delete")
				{
					GridEXRow item = grd.CurrentRow;
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
				}
			}
			else
			{
				MessageBox.Show("You Can not Delete Detail Record In Update Mode... ");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnrefresh_Click(object sender, EventArgs e)
	{
		ParentCategoryBind();
		CropYearbind();
		ItemCategoryAndTypeFill();
		ItemFill(Conversion.ToInt(CmbParentCatagory.Value));
	}

	private void btnRecepi_Click(object sender, EventArgs e)
	{
		try
		{
			frmBillOfMaterial_Manufacturing obj = new frmBillOfMaterial_Manufacturing(UserAccount);
			if (clsGlobalVariables.ScreenViewReights.Count > 0)
			{
				if (bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmBillOfMaterialManufacturing").ToList()[0].Value.ToString()))
				{
					obj.Show();
				}
				else
				{
					MessageBox.Show("You dont have right");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)ActiveGrid).Name);
			if (fs != null)
			{
				ActiveGrid.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)InActiveGrid).Name);
			if (fs != null)
			{
				InActiveGrid.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint325_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string Ids = "";
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.ParentCategoryId = Conversion.ToInt(CmbParentCategoryHistory.Value);
			obj.ItemCategoryId = Conversion.ToInt(CmbItemCategoryHistory.Value);
			obj.ItemTypeId = Conversion.ToInt(CmbItemTypeHistory.Value);
			obj.ItemId = Conversion.ToInt(CmbItemHistory.Value);
			obj.CropYearId = Conversion.ToInt(CmbCropYearHistory.Value);
			obj.ApprovedFilter = "Not All";
			obj.IsApproved = true;
			if (datEffectedDateHistory.Checked)
			{
				obj.EffectedDate = Conversion.ToDateTime((object)datEffectedDateHistory.Value);
			}
			if (ActiveGrid.GetCheckedRows().Count() > 0)
			{
				GridEXRow[] checkedRows = ActiveGrid.GetCheckedRows();
				foreach (GridEXRow item in checkedRows)
				{
					Ids = Ids + item.Cells["Id"].Value?.ToString() + ",";
				}
			}
			obj.Ids = Ids;
			DataTable dt = ItemPricingScheduleForRice.GetItemSchedulePricingSlip(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "325-ItemPricingList.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			FormHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbParentCategoryHistory).Text = string.Empty;
			((Control)(object)CmbItemCategoryHistory).Text = string.Empty;
			((Control)(object)CmbItemTypeHistory).Text = string.Empty;
			((Control)(object)CmbCropYearHistory).Text = string.Empty;
			ActiveGrid.ClearStructure();
			InActiveGrid.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintForInactive_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string Ids = "";
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.ParentCategoryId = Conversion.ToInt(CmbParentCategoryHistory.Value);
			obj.ItemCategoryId = Conversion.ToInt(CmbItemCategoryHistory.Value);
			obj.ItemTypeId = Conversion.ToInt(CmbItemTypeHistory.Value);
			obj.ItemId = Conversion.ToInt(CmbItemHistory.Value);
			obj.CropYearId = Conversion.ToInt(CmbCropYearHistory.Value);
			obj.ApprovedFilter = "Not All";
			obj.IsApproved = false;
			if (datEffectedDateHistory.Checked)
			{
				obj.EffectedDate = Conversion.ToDateTime((object)datEffectedDateHistory.Value);
			}
			if (InActiveGrid.GetCheckedRows().Count() > 0)
			{
				GridEXRow[] checkedRows = InActiveGrid.GetCheckedRows();
				foreach (GridEXRow item in checkedRows)
				{
					Ids = Ids + item.Cells["Id"].Value?.ToString() + ",";
				}
			}
			obj.Ids = Ids;
			DataTable dt = ItemPricingScheduleForRice.GetItemSchedulePricingSlip(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "325-ItemPricingList.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDoActiveSchedule_Click(object sender, EventArgs e)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		try
		{
			if (InActiveGrid.GetCheckedRows().Count() == 0)
			{
				MessageBox.Show("No Rows Selected To Active Schedule", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			GridEXRow[] checkedRows = InActiveGrid.GetCheckedRows();
			foreach (GridEXRow item in checkedRows)
			{
				ItemPricingScheduleForRice.UpdateActiveStatus(new ReportsParameters
				{
					Id = Conversion.ToInt(item.Cells["Id"].Value),
					IsApproved = true
				});
			}
			MessageBox.Show("Schedule's Active Successfully!");
			FormHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelSchedule_Click(object sender, EventArgs e)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		try
		{
			if (ActiveGrid.GetCheckedRows().Count() == 0)
			{
				MessageBox.Show("No Rows Selected To InActive Schedule", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			GridEXRow[] checkedRows = ActiveGrid.GetCheckedRows();
			foreach (GridEXRow item in checkedRows)
			{
				ItemPricingScheduleForRice.UpdateActiveStatus(new ReportsParameters
				{
					Id = Conversion.ToInt(item.Cells["Id"].Value),
					IsApproved = false
				});
			}
			MessageBox.Show("Schedule's InActive Successfully!");
			FormHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbCropYear_Leave(object sender, EventArgs e)
	{
		try
		{
			getLastRateAndDate();
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
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
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
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Expected O, but got Unknown
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Expected O, but got Unknown
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Expected O, but got Unknown
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Expected O, but got Unknown
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Expected O, but got Unknown
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Expected O, but got Unknown
		//IL_0632: Unknown result type (might be due to invalid IL or missing references)
		//IL_063c: Expected O, but got Unknown
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Expected O, but got Unknown
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Expected O, but got Unknown
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d6: Expected O, but got Unknown
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Expected O, but got Unknown
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_0723: Expected O, but got Unknown
		//IL_0724: Unknown result type (might be due to invalid IL or missing references)
		//IL_072e: Expected O, but got Unknown
		//IL_0d8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d97: Expected O, but got Unknown
		//IL_4622: Unknown result type (might be due to invalid IL or missing references)
		//IL_462c: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Lookups.frmItemPricingScheduleForRice));
		GridEXLayout InActiveGrid_DesignTimeLayout = new GridEXLayout();
		GridEXLayout ActiveGrid_DesignTimeLayout = new GridEXLayout();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
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
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label18 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbCropYear = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.datEffectedTime = new System.Windows.Forms.DateTimePicker();
		this.label7 = new System.Windows.Forms.Label();
		this.btnAdd = new System.Windows.Forms.Button();
		this.CmbRateUom = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtLastRate = new System.Windows.Forms.TextBox();
		this.txtNewPrice = new System.Windows.Forms.TextBox();
		this.datPreviousDate = new System.Windows.Forms.DateTimePicker();
		this.txtEffectedDate = new System.Windows.Forms.DateTimePicker();
		this.cmbItem = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CbOnlyShowRatedItems = new System.Windows.Forms.CheckBox();
		this.chkApplyDate = new System.Windows.Forms.CheckBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtApplyRateUom = new System.Windows.Forms.TextBox();
		this.datEffectedForFilter = new System.Windows.Forms.DateTimePicker();
		this.btnSearch = new System.Windows.Forms.Button();
		this.cmbItemType = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbParentCatagory = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbItemCategory = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnrefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnRecepi = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel15 = new System.Windows.Forms.Panel();
		this.btnDoActiveSchedule = new System.Windows.Forms.Button();
		this.InActiveGrid = new GridEX();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.btnDelSchedule = new System.Windows.Forms.Button();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ActiveGrid = new GridEX();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label22 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.CmbItemHistory = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.btnShow = new System.Windows.Forms.Button();
		this.CmbCropYearHistory = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.CmbParentCategoryHistory = new UltraCombo();
		this.datEffectedDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label16 = new System.Windows.Forms.Label();
		this.CmbItemCategoryHistory = new UltraCombo();
		this.CmbItemTypeHistory = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnPrint325 = new System.Windows.Forms.ToolStripButton();
		this.btnPrintForInactive = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCatagory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.InActiveGrid).BeginInit();
		this.panel13.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ActiveGrid).BeginInit();
		this.panel11.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategoryHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategoryHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemTypeHistory).BeginInit();
		this.panel12.SuspendLayout();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1359, 719);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1359, 719);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1351, 693);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1345, 687);
		this.panel2.TabIndex = 0;
		this.panel7.Controls.Add(this.panel10);
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label18);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 234);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1345, 453);
		this.panel7.TabIndex = 3;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 27);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1345, 426);
		this.panel10.TabIndex = 7;
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.ColumnAutoResize = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1345, 426);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1304, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grd;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 6;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label18.BackColor = System.Drawing.Color.Teal;
		this.label18.Dock = System.Windows.Forms.DockStyle.Top;
		this.label18.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(0, 0);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(1345, 27);
		this.label18.TabIndex = 1;
		this.label18.Text = "Detail Grid";
		this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel5.Controls.Add(this.groupBox2);
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 55);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1345, 179);
		this.panel5.TabIndex = 2;
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCropYear);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.datEffectedTime);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.btnAdd);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUom);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.txtLastRate);
		this.groupBox2.Controls.Add(this.txtNewPrice);
		this.groupBox2.Controls.Add(this.datPreviousDate);
		this.groupBox2.Controls.Add(this.txtEffectedDate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(453, 6);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(552, 163);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "By One by One Item";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(224, 50);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(54, 15);
		this.label11.TabIndex = 580;
		this.label11.Text = "CropYear";
		this.cmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.cmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance85).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Location = new System.Drawing.Point(294, 44);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Name = "cmbCropYear";
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Leave += new System.EventHandler(cmbCropYear_Leave);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(7, 50);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(80, 15);
		this.label6.TabIndex = 578;
		this.label6.Text = "Effected Time";
		this.datEffectedTime.CustomFormat = "";
		this.datEffectedTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datEffectedTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
		this.datEffectedTime.Location = new System.Drawing.Point(98, 46);
		this.datEffectedTime.Name = "datEffectedTime";
		this.datEffectedTime.Size = new System.Drawing.Size(123, 23);
		this.datEffectedTime.TabIndex = 1;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(224, 22);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(67, 15);
		this.label7.TabIndex = 573;
		this.label7.Text = "Item Name";
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(485, 99);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(48, 26);
		this.btnAdd.TabIndex = 8;
		this.btnAdd.Text = "Add";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance70;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance75).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance91).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(294, 72);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Leave += new System.EventHandler(CmbRateUom_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(224, 78);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(59, 15);
		this.label2.TabIndex = 573;
		this.label2.Text = "Rate Uom";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(7, 105);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(80, 15);
		this.label10.TabIndex = 576;
		this.label10.Text = "Previous Date";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(7, 22);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(78, 15);
		this.label8.TabIndex = 576;
		this.label8.Text = "Effected Date";
		this.txtLastRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLastRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLastRate.Location = new System.Drawing.Point(98, 74);
		this.txtLastRate.Name = "txtLastRate";
		this.txtLastRate.ReadOnly = true;
		this.txtLastRate.Size = new System.Drawing.Size(123, 23);
		this.txtLastRate.TabIndex = 2;
		this.txtNewPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNewPrice.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtNewPrice.Location = new System.Drawing.Point(294, 101);
		this.txtNewPrice.Name = "txtNewPrice";
		this.txtNewPrice.Size = new System.Drawing.Size(183, 23);
		this.txtNewPrice.TabIndex = 7;
		this.datPreviousDate.CustomFormat = "dd-MMM-yyyy";
		this.datPreviousDate.Enabled = false;
		this.datPreviousDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datPreviousDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datPreviousDate.Location = new System.Drawing.Point(98, 101);
		this.datPreviousDate.Name = "datPreviousDate";
		this.datPreviousDate.Size = new System.Drawing.Size(123, 23);
		this.datPreviousDate.TabIndex = 3;
		this.txtEffectedDate.CustomFormat = "dd-MMM-yyyy";
		this.txtEffectedDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEffectedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEffectedDate.Location = new System.Drawing.Point(98, 18);
		this.txtEffectedDate.Name = "txtEffectedDate";
		this.txtEffectedDate.Size = new System.Drawing.Size(123, 23);
		this.txtEffectedDate.TabIndex = 0;
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(294, 16);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_ValueChanged);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(224, 105);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(57, 15);
		this.label9.TabIndex = 574;
		this.label9.Text = "New Rate";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(7, 78);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(54, 15);
		this.label5.TabIndex = 574;
		this.label5.Text = "Last Rate";
		this.groupBox1.Controls.Add(this.CbOnlyShowRatedItems);
		this.groupBox1.Controls.Add(this.chkApplyDate);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtApplyRateUom);
		this.groupBox1.Controls.Add(this.datEffectedForFilter);
		this.groupBox1.Controls.Add(this.btnSearch);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemType);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentCatagory);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCategory);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(9, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(438, 163);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "By Filter Item's";
		this.CbOnlyShowRatedItems.AutoSize = true;
		this.CbOnlyShowRatedItems.Checked = true;
		this.CbOnlyShowRatedItems.CheckState = System.Windows.Forms.CheckState.Checked;
		this.CbOnlyShowRatedItems.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CbOnlyShowRatedItems.Location = new System.Drawing.Point(109, 144);
		this.CbOnlyShowRatedItems.Name = "CbOnlyShowRatedItems";
		this.CbOnlyShowRatedItems.Size = new System.Drawing.Size(161, 17);
		this.CbOnlyShowRatedItems.TabIndex = 586;
		this.CbOnlyShowRatedItems.Text = "Show Only Rated Items";
		this.CbOnlyShowRatedItems.UseVisualStyleBackColor = true;
		this.chkApplyDate.AutoSize = true;
		this.chkApplyDate.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.chkApplyDate.Location = new System.Drawing.Point(109, 122);
		this.chkApplyDate.Name = "chkApplyDate";
		this.chkApplyDate.Size = new System.Drawing.Size(89, 17);
		this.chkApplyDate.TabIndex = 4;
		this.chkApplyDate.Text = "Apply Date";
		this.chkApplyDate.UseVisualStyleBackColor = true;
		this.chkApplyDate.CheckedChanged += new System.EventHandler(checkBox1_CheckedChanged);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(12, 123);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(78, 15);
		this.label3.TabIndex = 580;
		this.label3.Text = "Effected Date";
		this.txtApplyRateUom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtApplyRateUom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtApplyRateUom.Location = new System.Drawing.Point(109, 95);
		this.txtApplyRateUom.Name = "txtApplyRateUom";
		this.txtApplyRateUom.Size = new System.Drawing.Size(252, 23);
		this.txtApplyRateUom.TabIndex = 3;
		this.datEffectedForFilter.CustomFormat = "dd-MMM-yyyy";
		this.datEffectedForFilter.Enabled = false;
		this.datEffectedForFilter.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datEffectedForFilter.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datEffectedForFilter.Location = new System.Drawing.Point(206, 119);
		this.datEffectedForFilter.Name = "datEffectedForFilter";
		this.datEffectedForFilter.Size = new System.Drawing.Size(155, 23);
		this.datEffectedForFilter.TabIndex = 5;
		this.btnSearch.BackColor = System.Drawing.Color.Teal;
		this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSearch.ForeColor = System.Drawing.Color.White;
		this.btnSearch.Location = new System.Drawing.Point(366, 119);
		this.btnSearch.Name = "btnSearch";
		this.btnSearch.Size = new System.Drawing.Size(66, 25);
		this.btnSearch.TabIndex = 6;
		this.btnSearch.Text = "Search";
		this.btnSearch.UseVisualStyleBackColor = false;
		this.btnSearch.Click += new System.EventHandler(btnSearch_Click);
		this.cmbItemType.AllowNull = (DefaultableBoolean)1;
		this.cmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance112).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Location = new System.Drawing.Point(109, 67);
		((System.Windows.Forms.Control)(object)this.cmbItemType).Name = "cmbItemType";
		((System.Windows.Forms.Control)(object)this.cmbItemType).Size = new System.Drawing.Size(252, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemType).TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(12, 99);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(93, 15);
		this.label1.TabIndex = 585;
		this.label1.Text = "Apply Rate Uom";
		this.CmbParentCatagory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentCatagory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentCatagory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentCatagory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCatagory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCatagory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance111).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentCatagory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentCatagory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentCatagory).Location = new System.Drawing.Point(109, 13);
		((System.Windows.Forms.Control)(object)this.CmbParentCatagory).Name = "CmbParentCatagory";
		((System.Windows.Forms.Control)(object)this.CmbParentCatagory).Size = new System.Drawing.Size(252, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentCatagory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbParentCatagory).Leave += new System.EventHandler(CmbParentCatagory_Leave);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(12, 73);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(60, 15);
		this.label12.TabIndex = 585;
		this.label12.Text = "Item Type";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(12, 21);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(91, 15);
		this.label4.TabIndex = 573;
		this.label4.Text = "Parent Catagory";
		this.cmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance119;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance125).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance126).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Location = new System.Drawing.Point(109, 40);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Name = "cmbItemCategory";
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Size = new System.Drawing.Size(252, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Leave += new System.EventHandler(cmbItemCategory_Leave);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(12, 46);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(82, 15);
		this.label13.TabIndex = 583;
		this.label13.Text = "Item Category";
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 28);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1345, 27);
		this.panel4.TabIndex = 1;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(6, 3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(187, 19);
		this.label15.TabIndex = 26;
		this.label15.Text = "Item Pricing Schedule";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1345, 28);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnnew, this.btnrefresh, this.btnsave, this.btnUpdate, this.toolStripSeparator, this.btnRecepi });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1345, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnrefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnrefresh.Image = (System.Drawing.Image)resources.GetObject("btnrefresh.Image");
		this.btnrefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnrefresh.Name = "btnrefresh";
		this.btnrefresh.Size = new System.Drawing.Size(80, 24);
		this.btnrefresh.Text = "Refresh";
		this.btnrefresh.Click += new System.EventHandler(btnrefresh_Click);
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
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnRecepi.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRecepi.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRecepi.Name = "btnRecepi";
		this.btnRecepi.Size = new System.Drawing.Size(100, 24);
		this.btnRecepi.Text = "Define Recipe";
		this.btnRecepi.Click += new System.EventHandler(btnRecepi_Click);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1351, 693);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel9);
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1345, 687);
		this.panel6.TabIndex = 0;
		this.panel9.Controls.Add(this.panel15);
		this.panel9.Controls.Add(this.panel13);
		this.panel9.Controls.Add(this.panel11);
		this.panel9.Controls.Add(this.panel12);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 30);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1345, 657);
		this.panel9.TabIndex = 1;
		this.panel15.Controls.Add(this.btnDoActiveSchedule);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.InActiveGrid);
		this.panel15.Controls.Add(this.ctrlGrdBar3);
		this.panel15.Controls.Add(this.label23);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(728, 104);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(617, 553);
		this.panel15.TabIndex = 595;
		this.btnDoActiveSchedule.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDoActiveSchedule.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnDoActiveSchedule.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnDoActiveSchedule.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnDoActiveSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDoActiveSchedule.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.btnDoActiveSchedule.Location = new System.Drawing.Point(180, 2);
		this.btnDoActiveSchedule.Margin = new System.Windows.Forms.Padding(0);
		this.btnDoActiveSchedule.Name = "btnDoActiveSchedule";
		this.btnDoActiveSchedule.Size = new System.Drawing.Size(20, 20);
		this.btnDoActiveSchedule.TabIndex = 11;
		this.btnDoActiveSchedule.UseVisualStyleBackColor = true;
		this.btnDoActiveSchedule.Click += new System.EventHandler(btnDoActiveSchedule_Click);
		this.InActiveGrid.AllowEdit = (InheritableBoolean)2;
		InActiveGrid_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.InActiveGrid.DesignTimeLayout = InActiveGrid_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.InActiveGrid).Dock = System.Windows.Forms.DockStyle.Fill;
		this.InActiveGrid.DynamicFiltering = true;
		this.InActiveGrid.FilterMode = (FilterMode)2;
		this.InActiveGrid.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.InActiveGrid).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.InActiveGrid.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.InActiveGrid).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.InActiveGrid).Name = "InActiveGrid";
		((ScrollableBase)this.InActiveGrid).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.InActiveGrid).Size = new System.Drawing.Size(617, 529);
		((System.Windows.Forms.Control)(object)this.InActiveGrid).TabIndex = 10;
		this.InActiveGrid.TotalRow = (InheritableBoolean)1;
		this.InActiveGrid.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.InActiveGrid).VisualStyle = (VisualStyle)4;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(578, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.InActiveGrid;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 9;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Dock = System.Windows.Forms.DockStyle.Top;
		this.label23.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(0, 0);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(617, 24);
		this.label23.TabIndex = 1;
		this.label23.Text = "InActive Pricing Schedule";
		this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel13.Controls.Add(this.btnDelSchedule);
		this.panel13.Controls.Add(this.panel14);
		this.panel13.Controls.Add(this.ctrlGrdBar1);
		this.panel13.Controls.Add(this.label22);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel13.Location = new System.Drawing.Point(0, 104);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(728, 553);
		this.panel13.TabIndex = 594;
		this.btnDelSchedule.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDelSchedule.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnDelSchedule.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnDelSchedule.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnDelSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDelSchedule.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelSchedule.Location = new System.Drawing.Point(171, 2);
		this.btnDelSchedule.Margin = new System.Windows.Forms.Padding(0);
		this.btnDelSchedule.Name = "btnDelSchedule";
		this.btnDelSchedule.Size = new System.Drawing.Size(20, 20);
		this.btnDelSchedule.TabIndex = 9;
		this.btnDelSchedule.UseVisualStyleBackColor = true;
		this.btnDelSchedule.Click += new System.EventHandler(btnDelSchedule_Click);
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.ActiveGrid);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(0, 24);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(728, 529);
		this.panel14.TabIndex = 8;
		this.ActiveGrid.AllowEdit = (InheritableBoolean)2;
		ActiveGrid_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.ActiveGrid.DesignTimeLayout = ActiveGrid_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.ActiveGrid).Dock = System.Windows.Forms.DockStyle.Fill;
		this.ActiveGrid.DynamicFiltering = true;
		this.ActiveGrid.FilterMode = (FilterMode)2;
		this.ActiveGrid.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.ActiveGrid).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.ActiveGrid.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.ActiveGrid).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.ActiveGrid).Name = "ActiveGrid";
		((ScrollableBase)this.ActiveGrid).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.ActiveGrid).Size = new System.Drawing.Size(728, 529);
		((System.Windows.Forms.Control)(object)this.ActiveGrid).TabIndex = 0;
		this.ActiveGrid.TotalRow = (InheritableBoolean)1;
		this.ActiveGrid.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.ActiveGrid).VisualStyle = (VisualStyle)4;
		this.ActiveGrid.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(687, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.ActiveGrid;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 7;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label22.BackColor = System.Drawing.Color.Teal;
		this.label22.Dock = System.Windows.Forms.DockStyle.Top;
		this.label22.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(0, 0);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(728, 24);
		this.label22.TabIndex = 0;
		this.label22.Text = "Active Pricing Schedule";
		this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel11.Controls.Add(this.groupBox3);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 27);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1345, 77);
		this.panel11.TabIndex = 593;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemHistory);
		this.groupBox3.Controls.Add(this.label24);
		this.groupBox3.Controls.Add(this.btnShow);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYearHistory);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add(this.label21);
		this.groupBox3.Controls.Add(this.label17);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentCategoryHistory);
		this.groupBox3.Controls.Add(this.datEffectedDateHistory);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCategoryHistory);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(5, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1022, 69);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.CmbItemHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance96;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance102).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Location = new System.Drawing.Point(506, 37);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Name = "CmbItemHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).TabIndex = 3;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(506, 18);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(67, 15);
		this.label24.TabIndex = 599;
		this.label24.Text = "Item Name";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(963, 37);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(53, 26);
		this.btnShow.TabIndex = 6;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.CmbCropYearHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYearHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYearHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYearHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance57).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance61).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYearHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).Location = new System.Drawing.Point(697, 37);
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).Name = "CmbCropYearHistory";
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).Size = new System.Drawing.Size(115, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).TabIndex = 4;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(6, 18);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(91, 15);
		this.label20.TabIndex = 595;
		this.label20.Text = "Parent Category";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(700, 18);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(57, 15);
		this.label21.TabIndex = 597;
		this.label21.Text = "Crop Year";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(814, 18);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(78, 15);
		this.label17.TabIndex = 591;
		this.label17.Text = "Effected Date";
		this.CmbParentCategoryHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentCategoryHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentCategoryHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentCategoryHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance132;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentCategoryHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentCategoryHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentCategoryHistory).Location = new System.Drawing.Point(9, 37);
		((System.Windows.Forms.Control)(object)this.CmbParentCategoryHistory).Name = "CmbParentCategoryHistory";
		((System.Windows.Forms.Control)(object)this.CmbParentCategoryHistory).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentCategoryHistory).TabIndex = 0;
		this.datEffectedDateHistory.CustomFormat = "dd-MMM-yyyy";
		this.datEffectedDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datEffectedDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datEffectedDateHistory.Location = new System.Drawing.Point(815, 39);
		this.datEffectedDateHistory.Name = "datEffectedDateHistory";
		this.datEffectedDateHistory.ShowCheckBox = true;
		this.datEffectedDateHistory.Size = new System.Drawing.Size(147, 23);
		this.datEffectedDateHistory.TabIndex = 5;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(181, 18);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(82, 15);
		this.label16.TabIndex = 588;
		this.label16.Text = "Item Category";
		this.CmbItemCategoryHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCategoryHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCategoryHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCategoryHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCategoryHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemCategoryHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCategoryHistory).Location = new System.Drawing.Point(181, 37);
		((System.Windows.Forms.Control)(object)this.CmbItemCategoryHistory).Name = "CmbItemCategoryHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemCategoryHistory).Size = new System.Drawing.Size(167, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCategoryHistory).TabIndex = 1;
		this.CmbItemTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory).Location = new System.Drawing.Point(349, 37);
		((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory).Name = "CmbItemTypeHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory).Size = new System.Drawing.Size(155, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory).TabIndex = 2;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(349, 18);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(60, 15);
		this.label14.TabIndex = 589;
		this.label14.Text = "Item Type";
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label19);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1345, 27);
		this.panel12.TabIndex = 592;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(7, 3);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(206, 19);
		this.label19.TabIndex = 0;
		this.label19.Text = "Pricing Schedule History";
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1345, 30);
		this.panel8.TabIndex = 0;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNewHistory, this.btnRefreshHistory, this.btnPrint325, this.btnPrintForInactive });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1345, 27);
		this.toolStrip2.TabIndex = 8;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = (System.Drawing.Image)resources.GetObject("btnRefreshHistory.Image");
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnPrint325.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint325.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint325.Name = "btnPrint325";
		this.btnPrint325.Size = new System.Drawing.Size(139, 24);
		this.btnPrint325.Text = "Active Print-325";
		this.btnPrint325.Click += new System.EventHandler(btnPrint325_Click);
		this.btnPrintForInactive.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintForInactive.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintForInactive.Name = "btnPrintForInactive";
		this.btnPrintForInactive.Size = new System.Drawing.Size(152, 24);
		this.btnPrintForInactive.Text = "InActive Print-325";
		this.btnPrintForInactive.Click += new System.EventHandler(btnPrintForInactive_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1359, 719);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmItemPricingScheduleForRice";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmItemPricingSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmItemPricingSchedule_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCatagory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.InActiveGrid).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ActiveGrid).EndInit();
		this.panel11.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategoryHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategoryHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemTypeHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
