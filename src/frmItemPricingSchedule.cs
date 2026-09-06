using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Pos;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.POS;
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
namespace Architecture.WinApp.WholeSale
{

public class frmItemPricingSchedule : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrid = new DataTable();

	private Rightsobjects formrights = new Rightsobjects();

	private int priceTypeId;

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

	private TextBox txtDescription;

	private Label label9;

	internal UltraCombo cmbItem;

	private Label label3;

	private Label label10;

	private DateTimePicker txtEffectedDate;

	private GroupBox groupBox1;

	private Label label4;

	internal UltraCombo cmbItemType;

	private Label label5;

	internal UltraCombo cmbItemCategory;

	private Button btnAdd;

	private Button btnSearch;

	internal GridEX datagrid;

	private Panel panel9;

	internal GridEX DataGridHistory;

	private Panel panel8;

	private GroupBox groupBox2;

	private Label label1;

	private DateTimePicker datEffectedTime;

	private Label label2;

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
			if (Conversion.ToString(base.Tag).Trim() == "frmPosRetailPricing")
			{
				priceTypeId = 7;
			}
			else
			{
				priceTypeId = 6;
			}
			ItemFill();
			ItemTypeFill();
			ItemCategoryFill();
			dtGrid.Columns.Add("ItemId");
			dtGrid.Columns.Add("ItemName");
			dtGrid.Columns.Add("EffectedDate");
			dtGrid.Columns.Add("BranchId");
			dtGrid.Columns.Add("ScheduleDesc");
			dtGrid.Columns.Add("ItemPrice");
			dtGrid.Columns.Add("NewPrice");
			datagrid.DataSource = dtGrid;
			datagrid.RetrieveStructure();
			dataGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void dataGridSetting()
	{
		try
		{
			datagrid.RootTable.Columns["ItemId"].Visible = false;
			datagrid.RootTable.Columns["ScheduleDesc"].Visible = false;
			datagrid.RootTable.Columns["EffectedDate"].Visible = false;
			datagrid.RootTable.Columns["BranchId"].Visible = false;
			datagrid.RootTable.Columns["ItemName"].Width = 400;
			datagrid.RootTable.Columns["ItemName"].EditType = (EditType)0;
			datagrid.RootTable.Columns["ItemPrice"].EditType = (EditType)0;
			datagrid.RootTable.Columns["ItemPrice"].FormatString = "#,#.##";
			datagrid.RootTable.Columns["ItemPrice"].TextAlignment = (TextAlignment)3;
			datagrid.RootTable.Columns.Add("Delete");
			datagrid.RootTable.Columns["Delete"].Caption = "Delete";
			datagrid.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			datagrid.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			datagrid.RootTable.Columns["Delete"].Image = Resources.icons8_delete_bin_16;
			datagrid.RootTable.Columns["Delete"].Width = 35;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFill()
	{
		try
		{
			DataTable dtItem = CommonServices.ItemBindFromPricingSchedule(priceTypeId);
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItem, cmbItem, "ItemId", "ItemName", "Item Name", false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemCategoryFill()
	{
		try
		{
			DataTable dtItemCategory = CommonServices.GetItemCategoryFromPricingShedule(priceTypeId);
			if (dtItemCategory.Rows.Count > 0)
			{
				DDL.BindDDL(dtItemCategory, cmbItemCategory, "ItemCategoryId", "CategoryDescription", "Item Category", ZeroIndex: false);
				((UltraGridBase)cmbItemCategory).DisplayLayout.Bands[0].Columns[1].Width = 350;
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
			DataTable dtItemType = CommonServices.GetItemTypesFromPricingShedule(priceTypeId);
			if (dtItemType.Rows.Count > 0)
			{
				DDL.BindDDL(dtItemType, cmbItemType, "ItemTypeId", "TypeDescription", "Item Type", ZeroIndex: false);
				((UltraGridBase)cmbItemType).DisplayLayout.Bands[0].Columns[1].Width = 350;
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

	private void GetPreviousPricesByItemId(string Ids, int ItemCategoryId = 0, int ItemTypeId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			if (Ids != null && Ids != string.Empty)
			{
				obj.ItemIds = "," + Ids;
			}
			obj.Id = priceTypeId;
			obj.ItemCategoryId = ItemCategoryId;
			obj.ItemTypeId = ItemTypeId;
			DataTable dtGetPreviousPrice = ItemPricingSchedule.ReadByItemIdAndPriceLookUpId(obj);
			if (dtGetPreviousPrice.Rows.Count > 0)
			{
				dtGrid.Rows.Clear();
				if (datagrid.GetRows().Count() == 0)
				{
					for (int i = 0; i < dtGetPreviousPrice.Rows.Count; i++)
					{
						dtGrid.Rows.Add(dtGetPreviousPrice.Rows[i]["ItemId"], dtGetPreviousPrice.Rows[i]["ItemName"], dtGetPreviousPrice.Rows[i]["EffectedDate"], 0, txtDescription.Text, dtGetPreviousPrice.Rows[i]["ItemPrice"], 0);
						datagrid.DataSource = dtGrid;
						datagrid.RetrieveStructure();
						dataGridSetting();
					}
				}
			}
			else
			{
				datagrid.DataSource = null;
				dtGrid.Rows.Clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetFilteredItems()
	{
		try
		{
			if (Conversion.ToInt(cmbItemCategory.Value) > 0 || Conversion.ToInt(cmbItemType.Value) > 0)
			{
				GetPreviousPricesByItemId("", Conversion.ToInt(cmbItemCategory.Value), Conversion.ToInt(cmbItemType.Value));
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
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		try
		{
			if (datagrid.GetRows().Length == 0)
			{
				throw new Exception("Grid record not found");
			}
			ItemPricingSchedule obj = new ItemPricingSchedule();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			obj.ItemPricingScheduleslist = new List<ItemPricingSchedule>();
			GridEXRow[] rows = datagrid.GetRows();
			foreach (GridEXRow r in rows)
			{
				ItemPricingSchedule obj2 = new ItemPricingSchedule();
				obj2.Id = RecId;
				obj2.CompanyId = UserAccount.CompanyId;
				obj2.OrganizationId = UserAccount.OrganizationId;
				obj2.EntryUser = UserAccount.ID;
				obj2.ModifyUser = UserAccount.ID;
				obj2.EntryDate = DateTime.Now;
				obj2.ModifyDate = DateTime.Now;
				obj2.EffectedDate = Conversion.ToDateTime((object)txtEffectedDate.Value);
				string EffDate = Conversion.ToString((object)obj2.EffectedDate.ToString("dd/MMM/yyyy"));
				obj2.EffectedDate = Convert.ToDateTime(EffDate + " " + datEffectedTime.Value.TimeOfDay.ToString());
				obj2.BranchesId = Conversion.ToInt(r.Cells["BranchId"].Value);
				if (Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Item Required");
					return;
				}
				obj2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				obj2.ScheduleDesc = Conversion.ToString((object)txtDescription.Text);
				if (Conversion.ToDouble(r.Cells["NewPrice"].Value) == 0.0)
				{
					MessageBox.Show("Check Item Price This Should be Not Zero");
					return;
				}
				obj2.ItemPrice = Conversion.ToDouble(r.Cells["NewPrice"].Value);
				obj2.ChangedPriceLookUpId = priceTypeId;
				obj2.PriceTypeId = priceTypeId;
				obj.ItemPricingScheduleslist.Add(obj2);
			}
			ItemPricingSchedule.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Update Successfully");
			}
			else
			{
				MessageBox.Show("Save Successfully");
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FormHistory()
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("ItemId");
			dt.Columns.Add("ItemName");
			dt.Columns.Add("EffectedDate");
			dt.Columns.Add("ItemPrice");
			dt.Columns.Add("EntryUserName");
			DataTable dtHistory = ItemPricingSchedule.ItemPricingScheduleFormHistory(new ItemPricingSchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				PriceTypeId = priceTypeId
			});
			if (dtHistory.Rows.Count > 0)
			{
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["ItemId"], dtHistory.Rows[i]["ItemName"], dtHistory.Rows[i]["EffectedDate"], dtHistory.Rows[i]["ItemPrice"], dtHistory.Rows[i]["EntryUserName"]);
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				DataGridHistorySetting();
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
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 50;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["ItemId"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetByItemId()
	{
		try
		{
			if (datagrid.GetRows().Count() > 0)
			{
				MessageBox.Show("You cannot update price of more than 1 items at time!");
				return;
			}
			DataTable dtGetByItemId = CommonServices.ItemBindFromPricingSchedule(priceTypeId, Conversion.ToString(DataGridHistory.CurrentRow.Cells["ItemId"].Value));
			if (dtGetByItemId.Rows.Count > 0)
			{
				dtGrid.Rows.Clear();
				RecId = Conversion.ToInt(dtGetByItemId.Rows[0]["Id"]);
				txtDescription.Text = Conversion.ToString(dtGetByItemId.Rows[0]["ScheduleDesc"]);
				txtEffectedDate.Value = Conversion.ToDateTime(dtGetByItemId.Rows[0]["EffectedDate"]);
				string EffDate = Conversion.ToString((object)txtEffectedDate.Value.ToString("dd/MMM/yyyy"));
				datEffectedTime.Value = Convert.ToDateTime(EffDate + " " + txtEffectedDate.Value.TimeOfDay.ToString());
				cmbItem.Value = Conversion.ToInt(dtGetByItemId.Rows[0]["ItemId"]);
				dtGrid.Rows.Add(Conversion.ToInt(dtGetByItemId.Rows[0]["ItemId"]), Conversion.ToString(dtGetByItemId.Rows[0]["ItemName"]), 0, 0, 0, Conversion.ToDouble(dtGetByItemId.Rows[0]["ItemPrice"]), 0);
				datagrid.DataSource = dtGrid;
				datagrid.RetrieveStructure();
				dataGridSetting();
				btnUpdate.Visible = true;
				btnsave.Visible = false;
				btnAdd.Enabled = false;
				btnSearch.Enabled = false;
				tabControl1.SelectedIndex = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItem_ValueChanged(object sender, EventArgs e)
	{
	}

	private void btnSearch_Click(object sender, EventArgs e)
	{
		try
		{
			GetFilteredItems();
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
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbItemCategory).Text = string.Empty;
			((Control)(object)cmbItemType).Text = string.Empty;
			txtDescription.Text = string.Empty;
			txtEffectedDate.Value = DateTime.Now;
			dtGrid.Rows.Clear();
			datagrid.ClearStructure();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			RecId = 0;
			btnAdd.Enabled = true;
			btnSearch.Enabled = true;
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
			string Ids = string.Empty;
			Ids = Conversion.ToString(cmbItem.Value);
			if (Ids != string.Empty)
			{
				GetPreviousPricesByItemId(Ids);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
		try
		{
			if (e.Column.Key == "Edit")
			{
				GetByItemId();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void datagrid_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (btnsave.Visible && btnsave.Enabled)
			{
				if (e.Column.Key == "Delete")
				{
					GridEXRow item = datagrid.CurrentRow;
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

	private void frmItemPricingSchedule_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0740: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Expected O, but got Unknown
		//IL_227d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2287: Expected O, but got Unknown
		GridEXLayout datagrid_DesignTimeLayout = new GridEXLayout();
		Appearance appearance37 = new Appearance();
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
		Appearance appearance13 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.WholeSale.frmItemPricingSchedule));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.datagrid = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label1 = new System.Windows.Forms.Label();
		this.datEffectedTime = new System.Windows.Forms.DateTimePicker();
		this.cmbItem = new UltraCombo();
		this.btnAdd = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.txtEffectedDate = new System.Windows.Forms.DateTimePicker();
		this.label9 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnSearch = new System.Windows.Forms.Button();
		this.cmbItemType = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbItemCategory = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.datagrid).BeginInit();
		this.panel5.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel8.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1015, 719);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1015, 719);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1007, 693);
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
		this.panel2.Size = new System.Drawing.Size(1001, 687);
		this.panel2.TabIndex = 0;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.datagrid);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 208);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1001, 479);
		this.panel7.TabIndex = 3;
		this.datagrid.ColumnAutoResize = true;
		datagrid_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.datagrid.DesignTimeLayout = datagrid_DesignTimeLayout;
		this.datagrid.DynamicFiltering = true;
		this.datagrid.FilterMode = (FilterMode)2;
		this.datagrid.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.datagrid).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.datagrid.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.datagrid).Location = new System.Drawing.Point(0, 1);
		((System.Windows.Forms.Control)(object)this.datagrid).Name = "datagrid";
		((ScrollableBase)this.datagrid).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.datagrid).Size = new System.Drawing.Size(1001, 475);
		((System.Windows.Forms.Control)(object)this.datagrid).TabIndex = 0;
		this.datagrid.TotalRow = (InheritableBoolean)1;
		this.datagrid.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.datagrid).VisualStyle = (VisualStyle)4;
		this.datagrid.ColumnButtonClick += new ColumnActionEventHandler(datagrid_ColumnButtonClick);
		this.panel5.Controls.Add(this.groupBox2);
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 59);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1001, 149);
		this.panel5.TabIndex = 2;
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.datEffectedTime);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox2.Controls.Add(this.btnAdd);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.txtDescription);
		this.groupBox2.Controls.Add(this.txtEffectedDate);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(12, 6);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(540, 138);
		this.groupBox2.TabIndex = 582;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "By One by One Item";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(303, 51);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(89, 17);
		this.label1.TabIndex = 578;
		this.label1.Text = "Effected Time";
		this.datEffectedTime.CustomFormat = "";
		this.datEffectedTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datEffectedTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
		this.datEffectedTime.Location = new System.Drawing.Point(395, 48);
		this.datEffectedTime.Name = "datEffectedTime";
		this.datEffectedTime.Size = new System.Drawing.Size(139, 23);
		this.datEffectedTime.TabIndex = 2;
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(144, 74);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(390, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 3;
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(144, 103);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(69, 25);
		this.btnAdd.TabIndex = 4;
		this.btnAdd.Text = "Add";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 79);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(76, 17);
		this.label3.TabIndex = 573;
		this.label3.Text = "Item Name";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(6, 51);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(88, 17);
		this.label10.TabIndex = 576;
		this.label10.Text = "Effected Date";
		this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDescription.Location = new System.Drawing.Point(144, 20);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(390, 23);
		this.txtDescription.TabIndex = 0;
		this.txtEffectedDate.CustomFormat = "dd-MMM-yyyy";
		this.txtEffectedDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEffectedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEffectedDate.Location = new System.Drawing.Point(144, 48);
		this.txtEffectedDate.Name = "txtEffectedDate";
		this.txtEffectedDate.Size = new System.Drawing.Size(127, 23);
		this.txtEffectedDate.TabIndex = 1;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(6, 23);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(134, 17);
		this.label9.TabIndex = 574;
		this.label9.Text = "Schedule Description";
		this.groupBox1.Controls.Add(this.btnSearch);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemType);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCategory);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(558, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(438, 138);
		this.groupBox1.TabIndex = 581;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "By Filter Item's";
		this.btnSearch.BackColor = System.Drawing.Color.Teal;
		this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSearch.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSearch.ForeColor = System.Drawing.Color.White;
		this.btnSearch.Location = new System.Drawing.Point(116, 75);
		this.btnSearch.Name = "btnSearch";
		this.btnSearch.Size = new System.Drawing.Size(69, 25);
		this.btnSearch.TabIndex = 2;
		this.btnSearch.Text = "Search";
		this.btnSearch.UseVisualStyleBackColor = false;
		this.btnSearch.Click += new System.EventHandler(btnSearch_Click);
		this.cmbItemType.AllowNull = (DefaultableBoolean)1;
		this.cmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Location = new System.Drawing.Point(116, 46);
		((System.Windows.Forms.Control)(object)this.cmbItemType).Name = "cmbItemType";
		((System.Windows.Forms.Control)(object)this.cmbItemType).Size = new System.Drawing.Size(271, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemType).TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 51);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(68, 17);
		this.label5.TabIndex = 585;
		this.label5.Text = "Item Type";
		this.cmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance30).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Location = new System.Drawing.Point(116, 18);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Name = "cmbItemCategory";
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Size = new System.Drawing.Size(271, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(6, 23);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(96, 17);
		this.label4.TabIndex = 583;
		this.label4.Text = "Item Category";
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label15);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 28);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1001, 31);
		this.panel4.TabIndex = 1;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(6, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(187, 19);
		this.label15.TabIndex = 26;
		this.label15.Text = "Item Pricing Schedule";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1001, 28);
		this.panel3.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnsave, this.btnUpdate, this.toolStripSeparator });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1001, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1007, 693);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "history";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel9);
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1001, 687);
		this.panel6.TabIndex = 0;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 31);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1001, 656);
		this.panel9.TabIndex = 1;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.ColumnAutoResize = true;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1001, 656);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1001, 31);
		this.panel8.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(4, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(250, 19);
		this.label2.TabIndex = 27;
		this.label2.Text = "Item Pricing Schedule History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1015, 719);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmItemPricingSchedule";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmItemPricingSchedule_FormClosing);
		base.Load += new System.EventHandler(frmItemPricingSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmItemPricingSchedule_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.datagrid).EndInit();
		this.panel5.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).EndInit();
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
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
