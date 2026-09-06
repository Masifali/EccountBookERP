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
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Production;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Lookups;

public class frmItemPricingSchedule : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtCategory = new DataTable();

	private DataTable dtItemType = new DataTable();

	private DataTable dt = new DataTable();

	private Rightsobjects formrights = new Rightsobjects();

	private int RecId;

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

	internal GridEX DataGridHistory;

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

	private ToolStripButton btnrefresh;

	private CheckBox CbOnlyShowRatedItems;

	private Label label18;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel10;

	private Panel panel11;

	private Label label19;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label11;

	private GroupBox groupBox3;

	internal UltraCombo CmbItemCategoryHistory;

	private Button btnPrintHistory;

	private Label label16;

	private Label label14;

	private Label label17;

	internal UltraCombo CmbItemTypeHistory;

	private DateTimePicker datEffectedDateHistory;

	public frmItemPricingSchedule(UserAccount userAccount)
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
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			dt.Columns.Add("ItemId");
			dt.Columns.Add("RateUomId");
			dt.Columns.Add("BaseUomId");
			dt.Columns.Add("ItemCategory");
			dt.Columns.Add("ItemName");
			dt.Columns.Add("BaseUom");
			dt.Columns.Add("PreviousDate", typeof(DateTime));
			dt.Columns.Add("PreviousRate", typeof(double));
			dt.Columns.Add("RateUom");
			dt.Columns.Add("NewPrice", typeof(double));
			ParentCategoryBind();
			ItemCategoryAndTypeFill();
			ItemFill(Conversion.ToInt(CmbParentCatagory.Value));
			CmbParentCatagory_Leave(null, null);
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
				DDL.BindDDLNew(dt, CmbItemCategoryHistory, "Id", "CategoryDescription", "Item Category", false);
				((UltraGridBase)cmbItemCategory).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
			else
			{
				DDL.BindDDLNew(dtCategory, cmbItemCategory, "Id", "CategoryDescription", "Item Category", false);
				DDL.BindDDLNew(dtCategory, CmbItemCategoryHistory, "Id", "CategoryDescription", "Item Category", false);
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
				DDL.BindDDLNew(dt, CmbItemTypeHistory, "Id", "TypeDescription", "ItemType", false);
				((UltraGridBase)cmbItemType).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
			else
			{
				DDL.BindDDLNew(dtItemType, cmbItemType, "Id", "TypeDescription", "ItemType", false);
				DDL.BindDDLNew(dtItemType, CmbItemTypeHistory, "Id", "TypeDescription", "ItemType", false);
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
				objdetail.DocumentTypeId = 183;
				objdetail.EntryDate = DateTime.Now;
				objdetail.ModifyDate = DateTime.Now;
				objdetail.ApprovedDate = DateTime.Now;
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
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			DataTable dtHistory = ItemPricingScheduleForRice.FormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 183
			});
			if (dtHistory.Rows.Count > 0)
			{
				DataGridHistory.DataSource = dtHistory;
				DataGridHistory.RetrieveStructure();
				DataGridHistorySetting();
			}
			else
			{
				DataGridHistory.ClearStructure();
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
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["ItemId"].Visible = false;
			DataGridHistory.RootTable.Columns["CropYearId"].Visible = false;
			DataGridHistory.RootTable.Columns.Add("Select");
			DataGridHistory.RootTable.Columns["Select"].ActAsSelector = true;
			DataGridHistory.RootTable.Columns["Select"].UseHeaderSelector = true;
			DataGridHistory.RootTable.Columns["Select"].Width = 40;
			DataGridHistory.RootTable.Columns["Select"].Position = 0;
			DataGridHistory.FrozenColumns = 1;
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			ctrlGrdBar1_Load(null, null);
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
			GridEXRow[] dataRows = grd.GetDataRows();
			for (int i = 0; i < dataRows.Length; i++)
			{
				if (Conversion.ToInt(dataRows[i].Cells["ItemId"].Value) == Conversion.ToInt(cmbItem.Value))
				{
					cmbItem.Focus();
					throw new Exception("You cannot add same item With Same CropYear in grid");
				}
			}
			dt.Rows.Add(Conversion.ToInt(cmbItem.Value), Conversion.ToInt(CmbRateUom.Value), 0, ((UltraDropDownBase)cmbItem).SelectedRow.Cells[2].Value, Conversion.ToString((object)((Control)(object)cmbItem).Text), 0, Conversion.ToDateTime((object)datPreviousDate.Value), Conversion.ToDouble((object)txtLastRate.Text), Conversion.ToInt(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtNewPrice.Text));
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
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Caption = "Delete";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].Image = Resources.icons8_delete_bin_16;
			grd.RootTable.Columns["Delete"].Width = 35;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.FrozenColumns = 1;
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
				FormHistory();
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			DataTable dt = ItemPricingScheduleForRice.GetLastItemRateByItemIdAndUomId(new ItemPricingScheduleForRice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemId = Conversion.ToInt(cmbItem.Value),
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
					dt.Rows.Add(dtData.Rows[i]["ItemId"], dtData.Rows[i]["RateUomId"], dtData.Rows[i]["BaseUomId"], dtData.Rows[i]["CategoryDescription"], dtData.Rows[i]["ItemName"], dtData.Rows[i]["BaseUom"], dtData.Rows[i]["PreviousDate"], dtData.Rows[i]["PreviousRate"], applyRateUom, 0);
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

	private void btnPrintHistory_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.ItemCategoryId = Conversion.ToInt(CmbItemCategoryHistory.Value);
			obj.ItemTypeId = Conversion.ToInt(CmbItemTypeHistory.Value);
			if (DataGridHistory.GetCheckedRows().Count() > 0)
			{
				GridEXRow[] checkedRows = DataGridHistory.GetCheckedRows();
				foreach (GridEXRow item in checkedRows)
				{
					obj.Ids = obj.Ids + item.Cells["Id"].Value?.ToString() + ",";
				}
			}
			else
			{
				obj.EffectedDate = Conversion.ToDateTime((object)datEffectedDateHistory.Value);
			}
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
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
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Expected O, but got Unknown
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Expected O, but got Unknown
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Expected O, but got Unknown
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Expected O, but got Unknown
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Expected O, but got Unknown
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0ae8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af2: Expected O, but got Unknown
		//IL_38c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_38cd: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance31 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Lookups.frmItemPricingSchedule));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
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
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnrefresh = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.DataGridHistory = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.CmbItemCategoryHistory = new UltraCombo();
		this.btnPrintHistory = new System.Windows.Forms.Button();
		this.label16 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.CmbItemTypeHistory = new UltraCombo();
		this.datEffectedDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		this.groupBox2.SuspendLayout();
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
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel11.SuspendLayout();
		this.panel8.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategoryHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemTypeHistory).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1084, 661);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1084, 661);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1076, 635);
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
		this.panel2.Size = new System.Drawing.Size(1070, 629);
		this.panel2.TabIndex = 0;
		this.panel7.Controls.Add(this.panel10);
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label18);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 198);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1070, 431);
		this.panel7.TabIndex = 3;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 30);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1070, 401);
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
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1070, 401);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1022, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grd;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar2.TabIndex = 6;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label18.BackColor = System.Drawing.Color.Teal;
		this.label18.Dock = System.Windows.Forms.DockStyle.Top;
		this.label18.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(0, 0);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(1070, 30);
		this.label18.TabIndex = 1;
		this.label18.Text = "Detail Grid";
		this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel5.Controls.Add(this.groupBox2);
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 61);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1070, 137);
		this.panel5.TabIndex = 2;
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
		this.groupBox2.Location = new System.Drawing.Point(563, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(504, 134);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "By One by One Item";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(224, 22);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(80, 15);
		this.label6.TabIndex = 578;
		this.label6.Text = "Effected Time";
		this.datEffectedTime.CustomFormat = "";
		this.datEffectedTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datEffectedTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
		this.datEffectedTime.Location = new System.Drawing.Point(304, 18);
		this.datEffectedTime.Name = "datEffectedTime";
		this.datEffectedTime.Size = new System.Drawing.Size(130, 23);
		this.datEffectedTime.TabIndex = 1;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(6, 51);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(67, 15);
		this.label7.TabIndex = 573;
		this.label7.Text = "Item Name";
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(436, 101);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(65, 26);
		this.btnAdd.TabIndex = 8;
		this.btnAdd.Text = "Add";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance37;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance42).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(85, 74);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(136, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Leave += new System.EventHandler(CmbRateUom_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 80);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(59, 15);
		this.label2.TabIndex = 573;
		this.label2.Text = "Rate Uom";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(224, 107);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(80, 15);
		this.label10.TabIndex = 576;
		this.label10.Text = "Previous Date";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(6, 22);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(78, 15);
		this.label8.TabIndex = 576;
		this.label8.Text = "Effected Date";
		this.txtLastRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLastRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLastRate.Location = new System.Drawing.Point(304, 76);
		this.txtLastRate.Name = "txtLastRate";
		this.txtLastRate.ReadOnly = true;
		this.txtLastRate.Size = new System.Drawing.Size(130, 23);
		this.txtLastRate.TabIndex = 2;
		this.txtNewPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNewPrice.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtNewPrice.Location = new System.Drawing.Point(85, 103);
		this.txtNewPrice.Name = "txtNewPrice";
		this.txtNewPrice.Size = new System.Drawing.Size(136, 23);
		this.txtNewPrice.TabIndex = 7;
		this.datPreviousDate.CustomFormat = "dd-MMM-yyyy";
		this.datPreviousDate.Enabled = false;
		this.datPreviousDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datPreviousDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datPreviousDate.Location = new System.Drawing.Point(304, 103);
		this.datPreviousDate.Name = "datPreviousDate";
		this.datPreviousDate.Size = new System.Drawing.Size(130, 23);
		this.datPreviousDate.TabIndex = 3;
		this.txtEffectedDate.CustomFormat = "dd-MMM-yyyy";
		this.txtEffectedDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEffectedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEffectedDate.Location = new System.Drawing.Point(85, 18);
		this.txtEffectedDate.Name = "txtEffectedDate";
		this.txtEffectedDate.Size = new System.Drawing.Size(136, 23);
		this.txtEffectedDate.TabIndex = 0;
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance14;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance19).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(85, 45);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(349, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_ValueChanged);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(6, 107);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(57, 15);
		this.label9.TabIndex = 574;
		this.label9.Text = "New Rate";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(224, 80);
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
		this.groupBox1.Location = new System.Drawing.Point(4, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(559, 134);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "By Filter Item's";
		this.CbOnlyShowRatedItems.AutoSize = true;
		this.CbOnlyShowRatedItems.Checked = true;
		this.CbOnlyShowRatedItems.CheckState = System.Windows.Forms.CheckState.Checked;
		this.CbOnlyShowRatedItems.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CbOnlyShowRatedItems.Location = new System.Drawing.Point(368, 78);
		this.CbOnlyShowRatedItems.Name = "CbOnlyShowRatedItems";
		this.CbOnlyShowRatedItems.Size = new System.Drawing.Size(150, 19);
		this.CbOnlyShowRatedItems.TabIndex = 586;
		this.CbOnlyShowRatedItems.Text = "Show Only Rated Items";
		this.CbOnlyShowRatedItems.UseVisualStyleBackColor = true;
		this.chkApplyDate.AutoSize = true;
		this.chkApplyDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkApplyDate.Location = new System.Drawing.Point(451, 20);
		this.chkApplyDate.Name = "chkApplyDate";
		this.chkApplyDate.Size = new System.Drawing.Size(85, 19);
		this.chkApplyDate.TabIndex = 4;
		this.chkApplyDate.Text = "Apply Date";
		this.chkApplyDate.UseVisualStyleBackColor = true;
		this.chkApplyDate.CheckedChanged += new System.EventHandler(checkBox1_CheckedChanged);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(368, 22);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(78, 15);
		this.label3.TabIndex = 580;
		this.label3.Text = "Effected Date";
		this.txtApplyRateUom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtApplyRateUom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtApplyRateUom.Location = new System.Drawing.Point(109, 103);
		this.txtApplyRateUom.Name = "txtApplyRateUom";
		this.txtApplyRateUom.Size = new System.Drawing.Size(252, 23);
		this.txtApplyRateUom.TabIndex = 3;
		this.datEffectedForFilter.CustomFormat = "dd-MMM-yyyy";
		this.datEffectedForFilter.Enabled = false;
		this.datEffectedForFilter.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datEffectedForFilter.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datEffectedForFilter.Location = new System.Drawing.Point(368, 47);
		this.datEffectedForFilter.Name = "datEffectedForFilter";
		this.datEffectedForFilter.Size = new System.Drawing.Size(185, 23);
		this.datEffectedForFilter.TabIndex = 5;
		this.btnSearch.BackColor = System.Drawing.Color.Teal;
		this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSearch.ForeColor = System.Drawing.Color.White;
		this.btnSearch.Location = new System.Drawing.Point(487, 102);
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
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance67).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Location = new System.Drawing.Point(109, 74);
		((System.Windows.Forms.Control)(object)this.cmbItemType).Name = "cmbItemType";
		((System.Windows.Forms.Control)(object)this.cmbItemType).Size = new System.Drawing.Size(252, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemType).TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(12, 107);
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
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCatagory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCatagory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance66).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentCatagory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentCatagory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentCatagory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentCatagory).Location = new System.Drawing.Point(109, 16);
		((System.Windows.Forms.Control)(object)this.CmbParentCatagory).Name = "CmbParentCatagory";
		((System.Windows.Forms.Control)(object)this.CmbParentCatagory).Size = new System.Drawing.Size(252, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentCatagory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbParentCatagory).Leave += new System.EventHandler(CmbParentCatagory_Leave);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(12, 80);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(60, 15);
		this.label12.TabIndex = 585;
		this.label12.Text = "Item Type";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(12, 22);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(91, 15);
		this.label4.TabIndex = 573;
		this.label4.Text = "Parent Catagory";
		this.cmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance74;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Location = new System.Drawing.Point(109, 45);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Name = "cmbItemCategory";
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Size = new System.Drawing.Size(252, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Leave += new System.EventHandler(cmbItemCategory_Leave);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(12, 51);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(82, 15);
		this.label13.TabIndex = 583;
		this.label13.Text = "Item Category";
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 28);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1070, 33);
		this.panel4.TabIndex = 1;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(7, 7);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(187, 19);
		this.label15.TabIndex = 26;
		this.label15.Text = "Item Pricing Schedule";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1070, 28);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnsave, this.btnUpdate, this.toolStripSeparator, this.btnrefresh });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1070, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
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
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnrefresh.Image = (System.Drawing.Image)resources.GetObject("btnrefresh.Image");
		this.btnrefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnrefresh.Name = "btnrefresh";
		this.btnrefresh.Size = new System.Drawing.Size(80, 24);
		this.btnrefresh.Text = "Refresh";
		this.btnrefresh.Click += new System.EventHandler(btnrefresh_Click);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1076, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel9);
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1070, 626);
		this.panel6.TabIndex = 0;
		this.panel9.Controls.Add(this.ctrlGrdBar1);
		this.panel9.Controls.Add(this.panel11);
		this.panel9.Controls.Add(this.label19);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 91);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1070, 535);
		this.panel9.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1022, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar1.TabIndex = 7;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1070, 509);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 26);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1070, 509);
		this.panel11.TabIndex = 2;
		this.label19.BackColor = System.Drawing.Color.Teal;
		this.label19.Dock = System.Windows.Forms.DockStyle.Top;
		this.label19.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(0, 0);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(1070, 26);
		this.label19.TabIndex = 1;
		this.label19.Text = "History Grid";
		this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel8.Controls.Add(this.label11);
		this.panel8.Controls.Add(this.groupBox3);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1070, 91);
		this.panel8.TabIndex = 0;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCategoryHistory);
		this.groupBox3.Controls.Add(this.btnPrintHistory);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Controls.Add(this.label17);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory);
		this.groupBox3.Controls.Add(this.datEffectedDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(5, 29);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(940, 58);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.CmbItemCategoryHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCategoryHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCategoryHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCategoryHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance24).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCategoryHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCategoryHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemCategoryHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCategoryHistory).Location = new System.Drawing.Point(92, 21);
		((System.Windows.Forms.Control)(object)this.CmbItemCategoryHistory).Name = "CmbItemCategoryHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemCategoryHistory).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCategoryHistory).TabIndex = 0;
		this.btnPrintHistory.BackColor = System.Drawing.Color.Teal;
		this.btnPrintHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPrintHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintHistory.ForeColor = System.Drawing.Color.White;
		this.btnPrintHistory.Location = new System.Drawing.Point(799, 20);
		this.btnPrintHistory.Name = "btnPrintHistory";
		this.btnPrintHistory.Size = new System.Drawing.Size(70, 28);
		this.btnPrintHistory.TabIndex = 3;
		this.btnPrintHistory.Text = "Print";
		this.btnPrintHistory.UseVisualStyleBackColor = false;
		this.btnPrintHistory.Click += new System.EventHandler(btnPrintHistory_Click);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(4, 27);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(82, 15);
		this.label16.TabIndex = 588;
		this.label16.Text = "Item Category";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(315, 27);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(60, 15);
		this.label14.TabIndex = 589;
		this.label14.Text = "Item Type";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(572, 27);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(78, 15);
		this.label17.TabIndex = 591;
		this.label17.Text = "Effected Date";
		this.CmbItemTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance51;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance57).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory).Location = new System.Drawing.Point(382, 21);
		((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory).Name = "CmbItemTypeHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory).TabIndex = 1;
		this.datEffectedDateHistory.CustomFormat = "dd-MMM-yyyy";
		this.datEffectedDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datEffectedDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datEffectedDateHistory.Location = new System.Drawing.Point(655, 23);
		this.datEffectedDateHistory.Name = "datEffectedDateHistory";
		this.datEffectedDateHistory.Size = new System.Drawing.Size(138, 23);
		this.datEffectedDateHistory.TabIndex = 2;
		this.label11.BackColor = System.Drawing.Color.Teal;
		this.label11.Dock = System.Windows.Forms.DockStyle.Top;
		this.label11.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(0, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(1070, 27);
		this.label11.TabIndex = 3;
		this.label11.Text = "Item Pricing Schedule History";
		this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1084, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmItemPricingSchedule";
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
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategoryHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemTypeHistory).EndInit();
		base.ResumeLayout(false);
	}
}
