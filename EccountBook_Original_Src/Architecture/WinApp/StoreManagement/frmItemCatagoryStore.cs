using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.AssetSchema;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.AssetSchema;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.StoreManagement;

public class frmItemCatagoryStore : Form
{
	private int RecId;

	private int FixedAssetCategoryId;

	public int FormTypeId;

	private string ParentCategoryIds = "7";

	private string ClassGroupIds = "4";

	private bool IfConfigAllAccountAllowOn;

	private bool AttributesForItemFeature;

	private bool MultiLanguageFeature;

	private DataTable dtAccounts = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	public UserAccount UserAccount = new UserAccount();

	private List<COAAllocation> Accountslst = new List<COAAllocation>();

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private Panel panel2;

	private Panel panel3;

	private CheckBox chkstatus;

	private TextBox txtItemCategorySerialTo;

	private TextBox txtItemCategorySerialFrom;

	private TextBox txtItemCategoryDescription;

	private TextBox txtItemCategoryCode;

	private Label lblDepresitionExpenses;

	private Label lblWipCapital;

	private Label lblAccumlatedDepricition;

	private Label label38;

	private Label label37;

	private Label label36;

	private Panel panel4;

	internal GridEX grdfrm;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo cmbItemCategoryRevenueAccount;

	internal UltraCombo cmbItemCategoryInventoryAccount;

	internal UltraCombo cmbItemCategoryCGSAccount;

	private Label lblParentCategory;

	internal UltraCombo CmbItemParentCategory;

	private Label label8;

	internal UltraCombo CmbItemClassGroup;

	private Panel panel5;

	private Label label1;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnMultiLingo;

	private GroupBox groupBox1;

	private Panel PanelAttributes;

	internal GridEX grdAttributes;

	private Panel panel7;

	private Label label2;

	public Label lblCategoryDescription;

	internal UltraCombo CmbExpenseMaintenanceAccount;

	private Label lblExpenseMaintenanceAc;

	private GroupBox groupBoxDepreciationMethod;

	internal UltraCombo CmbDepreciationMethodName;

	private Label label3;

	private TextBox txtDepreciationRatePercent;

	private Label label5;

	private TextBox txtUseFullLifeMonth;

	private Label label4;

	public frmItemCatagoryStore(UserAccount pUserAccount)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void InvDeffrmItemCatagory_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1000, 750);
			PanelAttributes.Visible = false;
			panel3.Height = 100;
			if (FormTypeId == 0 || FormTypeId == 1)
			{
				ParentCategoryIds = "7";
				ClassGroupIds = "4";
				lblCategoryDescription.Text = "Item Category PM";
			}
			else if (FormTypeId == 2)
			{
				ParentCategoryIds = "8";
				ClassGroupIds = "5";
				lblCategoryDescription.Text = "Item Category Store";
			}
			else if (FormTypeId == 3)
			{
				ParentCategoryIds = "5";
				ClassGroupIds = "6";
				lblCategoryDescription.Text = "Item Category (Fix Assets)";
				lblAccumlatedDepricition.Text = "Accumulated A/c";
				lblWipCapital.Text = "Assets GL A/c";
				lblDepresitionExpenses.Text = "Depreciation A/c";
				lblExpenseMaintenanceAc.Visible = true;
				((Control)(object)CmbExpenseMaintenanceAccount).Visible = true;
				groupBoxDepreciationMethod.Visible = true;
				panel3.Height = 145;
			}
			MultiLanguageFeature = CommonServices.GetERPFeatureById(7);
			btnMultiLingo.Visible = MultiLanguageFeature;
			AttributesForItemFeature = CommonServices.GetERPFeatureById(13);
			if (AttributesForItemFeature)
			{
				PanelAttributes.Visible = AttributesForItemFeature;
				AttributeGridFill(0);
			}
			IfConfigAllAccountAllowOn = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("All Account Allow on Item and Item Category"));
			((Control)(object)CmbItemParentCategory).Enabled = true;
			((Control)(object)CmbItemClassGroup).Enabled = true;
			ItemParentCategoryFill();
			GetItemClassGroup();
			gridfill();
			AccountTitleFill();
			if (FormTypeId != 3)
			{
				AccountsBind();
			}
			else if (FormTypeId == 3)
			{
				AssetAccountBind();
				ExpenseAccountFill();
				DepreciatonMethodNameBind();
			}
			btnupdate.Visible = false;
			txtItemCategoryCode.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AttributeGridFill(int CategoryId)
	{
		try
		{
			DataTable dt = AttributeVariant.Attribute_GetForItemCategory(UserAccount.OrganizationId, UserAccount.CompanyId, CategoryId);
			if (dt.Rows.Count > 0)
			{
				DataTable AttributeVariant = new DataTable();
				AttributeVariant.Columns.Add("CategoryAttributeId", typeof(int));
				AttributeVariant.Columns.Add("attributeId", typeof(int));
				AttributeVariant.Columns.Add("Name", typeof(string));
				AttributeVariant.Columns.Add("Description", typeof(string));
				AttributeVariant.Columns.Add("Status", typeof(string));
				AttributeVariant.Columns.Add("StatusValue", typeof(int));
				AttributeVariant.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					AttributeVariant.Rows.Add(Conversion.ToInt(dt.Rows[i]["CategoryAttributeId"]), Conversion.ToInt(dt.Rows[i]["attributeId"]), Conversion.ToString(dt.Rows[i]["attributename"]), Conversion.ToString(dt.Rows[i]["attributedescription"]), Conversion.ToString(dt.Rows[i]["AllocationStatus"]), Conversion.ToString(dt.Rows[i]["StatusValue"]));
				}
				grdAttributes.DataSource = AttributeVariant;
				grdAttributes.RetrieveStructure();
				GrdSetting();
			}
			else
			{
				grdAttributes.ClearStructure();
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
			grdAttributes.GroupByBoxVisible = false;
			grdAttributes.RootTable.Columns["CategoryAttributeId"].Visible = false;
			grdAttributes.RootTable.Columns["attributeId"].Visible = false;
			grdAttributes.RootTable.Columns["StatusValue"].Visible = false;
			grdAttributes.RootTable.Columns["Name"].Width = 85;
			grdAttributes.RootTable.Columns["Description"].Width = 135;
			grdAttributes.RootTable.Columns["Status"].Width = 85;
			grdAttributes.RootTable.Columns.Add("Select");
			grdAttributes.RootTable.Columns["Select"].ActAsSelector = true;
			grdAttributes.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdAttributes.RootTable.Columns["Select"].Width = 30;
			grdAttributes.RootTable.Columns["Select"].Position = 0;
			GridEXRow[] rows = grdAttributes.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["StatusValue"].Value) == 1)
				{
					r.IsChecked = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountTitleFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			COAAllocation coa = new COAAllocation();
			coa.OrganizationId = UserAccount.OrganizationId;
			coa.CompanyId = UserAccount.CompanyId;
			Accountslst = COAAllocation.GetLst(coa);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemParentCategoryFill()
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
			if (dt.Rows.Count > 0)
			{
				DataTable dtFiltered = new DataTable();
				List<DataRow> filteredRows = (from row in dt.AsEnumerable()
					where ParentCategoryIds.Contains(row["Id"].ToString())
					select row).ToList();
				if (filteredRows.Any())
				{
					dtFiltered = filteredRows.CopyToDataTable();
					DDL.BindDDL(dtFiltered, CmbItemParentCategory, "Id", "InvParentCateDescription", "ParentCategory", ZeroIndex: false);
					((UltraGridBase)CmbItemParentCategory).Rows[0].Activate();
					((UltraGridBase)CmbItemParentCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetItemClassGroup()
	{
		try
		{
			DataTable dt = ItemCategory.GetItemClassGroup();
			if (dt.Rows.Count > 0)
			{
				DataTable dtFiltered = new DataTable();
				List<DataRow> filteredRows = (from row in dt.AsEnumerable()
					where ClassGroupIds.Contains(row["Id"].ToString())
					select row).ToList();
				if (filteredRows.Any())
				{
					dtFiltered = filteredRows.CopyToDataTable();
					DDL.BindDDL(dtFiltered, CmbItemClassGroup, "Id", "ClassGroupName", "Item Class Group", ZeroIndex: false);
					((UltraGridBase)CmbItemClassGroup).Rows[0].Activate();
					((UltraGridBase)CmbItemClassGroup).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsBind()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[1] { 4 });
			DataTable dtsale = DatatableHelper.GetAccountsFromGlobalByTypeIds((!IfConfigAllAccountAllowOn) ? new int[1] { 10 } : new int[2] { 10, 14 });
			DataTable dtcgs = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[1] { 12 });
			InfragisticsHelper.BindAndRetainSelection(cmbItemCategoryInventoryAccount, dt, "Id", "AccountTitle", "Accumulative Account", AllColumns: true, Conversion.ToInt(cmbItemCategoryInventoryAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbItemCategoryRevenueAccount, dtsale, "Id", "AccountTitle", "Assets GL Account", AllColumns: true, Conversion.ToInt(cmbItemCategoryRevenueAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbItemCategoryCGSAccount, dtcgs, "Id", "AccountTitle", "Assets GL Account", AllColumns: true, Conversion.ToInt(cmbItemCategoryCGSAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)cmbItemCategoryInventoryAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
			if (dtsale.Rows.Count > 0)
			{
				((UltraGridBase)cmbItemCategoryRevenueAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
			if (dtcgs.Rows.Count > 0)
			{
				((UltraGridBase)cmbItemCategoryCGSAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AssetAccountBind()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[1] { 1 });
			InfragisticsHelper.BindAndRetainSelection(cmbItemCategoryInventoryAccount, dt, "Id", "AccountTitle", "Accumulative Account", AllColumns: true, Conversion.ToInt(cmbItemCategoryInventoryAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbItemCategoryRevenueAccount, dt, "Id", "AccountTitle", "Assets GL Account", AllColumns: true, Conversion.ToInt(cmbItemCategoryRevenueAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)cmbItemCategoryInventoryAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)cmbItemCategoryRevenueAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpenseAccountFill()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[6] { 11, 12, 14, 20, 21, 23 });
			InfragisticsHelper.BindAndRetainSelection(CmbExpenseMaintenanceAccount, dt, "Id", "AccountTitle", "Expense Maintenance Account", AllColumns: true, Conversion.ToInt(CmbExpenseMaintenanceAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbItemCategoryCGSAccount, dt, "Id", "AccountTitle", "Depreciation Account", AllColumns: true, Conversion.ToInt(cmbItemCategoryCGSAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbExpenseMaintenanceAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)cmbItemCategoryCGSAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DepreciatonMethodNameBind()
	{
		try
		{
			DataTable dt = DepreciationMethodSchedule.GetActiveDepreciationMethodSchedule(UserAccount.OrganizationId, UserAccount.CompanyId);
			InfragisticsHelper.BindAndRetainSelection(CmbDepreciationMethodName, dt, "depreciationMethodScheduleId", "DepreciatonMethodName", "Depreciaton Method", AllColumns: true, Conversion.ToInt(CmbDepreciationMethodName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (txtItemCategoryCode.Text.Trim() == "" || txtItemCategoryCode.Text == "0")
		{
			MessageBox.Show("Please Insert Code");
			txtItemCategoryCode.Focus();
			return false;
		}
		if (txtItemCategoryDescription.Text.Trim() == "" || txtItemCategoryDescription.Text.Trim() == "0")
		{
			MessageBox.Show("Please Insert Description");
			txtItemCategoryDescription.Focus();
			return false;
		}
		if (txtItemCategorySerialFrom.Text.Trim() == "" || txtItemCategorySerialFrom.Text.Trim() == "0")
		{
			MessageBox.Show("Please Insert Serial From");
			txtItemCategorySerialFrom.Focus();
			return false;
		}
		if (txtItemCategorySerialTo.Text.Trim() == "" || txtItemCategorySerialTo.Text.Trim() == "0")
		{
			MessageBox.Show("Please Insert Serial To");
			txtItemCategorySerialTo.Focus();
			return false;
		}
		if (((Control)(object)cmbItemCategoryRevenueAccount).Text.Trim() == "" || Conversion.ToInt(cmbItemCategoryRevenueAccount.Value) == 0)
		{
			string Ac = ((FormTypeId == 3) ? "Accumulated" : "Revenue");
			MessageBox.Show("Please Select " + Ac + " Account");
			cmbItemCategoryRevenueAccount.Focus();
			return false;
		}
		if (((Control)(object)cmbItemCategoryInventoryAccount).Text.Trim() == "" || Conversion.ToInt(cmbItemCategoryInventoryAccount.Value) == 0)
		{
			string Ac2 = ((FormTypeId == 3) ? "Assets" : "Inventory");
			MessageBox.Show("Please Select " + Ac2 + " Account");
			cmbItemCategoryInventoryAccount.Focus();
			return false;
		}
		if (((Control)(object)cmbItemCategoryCGSAccount).Text.Trim() == "" || Conversion.ToInt(cmbItemCategoryCGSAccount.Value) == 0)
		{
			string Ac3 = ((FormTypeId == 3) ? "Depreciation" : "CGS");
			MessageBox.Show("Please Select " + Ac3 + " Account");
			cmbItemCategoryCGSAccount.Focus();
			return false;
		}
		if (((Control)(object)CmbItemParentCategory).Text.Trim() == "" || Conversion.ToInt(CmbItemParentCategory.Value) == 0)
		{
			MessageBox.Show("Please Select Parent Category");
			CmbItemParentCategory.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			ItemCategory cat = new ItemCategory();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				cat.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			cat.CategoryCode = Conversion.ToString((object)txtItemCategoryCode.Text);
			cat.CategoryDescription = Conversion.ToString((object)txtItemCategoryDescription.Text);
			cat.SerialFrom = Conversion.ToInt((object)txtItemCategorySerialFrom.Text);
			cat.SerialTo = Conversion.ToInt((object)txtItemCategorySerialTo.Text);
			cat.RevenueAccountId = Conversion.ToInt(cmbItemCategoryRevenueAccount.Value);
			cat.CGSAccountId = Conversion.ToInt(cmbItemCategoryCGSAccount.Value);
			cat.InventoryAccountId = Conversion.ToInt(cmbItemCategoryInventoryAccount.Value);
			cat.ItemClassGroupId = Conversion.ToInt(CmbItemClassGroup.Value);
			if (ParentCategoryIds.Contains("5") && FormTypeId == 3)
			{
				cat.accumulatedDepreciationAcId = Conversion.ToInt(cmbItemCategoryRevenueAccount.Value);
				cat.CapitalWipAcId = Conversion.ToInt(cmbItemCategoryInventoryAccount.Value);
				cat.DepreciationExpenseAcId = Conversion.ToInt(cmbItemCategoryCGSAccount.Value);
				cat.ExpenseMaintenanceAccountId = Conversion.ToInt(CmbExpenseMaintenanceAccount.Value);
				cat.depreciationMethodScheduleId = Conversion.ToInt(CmbDepreciationMethodName.Value);
				cat.usefullLifeInMonths = Conversion.ToInt((object)txtUseFullLifeMonth.Text);
				cat.depriciationrate = Conversion.ToDecimal((object)txtDepreciationRatePercent.Text);
				CategoryDepreciationSchedule detail2 = new CategoryDepreciationSchedule();
				detail2.rateofDepreciation = cat.depriciationrate;
				detail2.usefullLifeInMonths = cat.usefullLifeInMonths;
				detail2.depreciationMethodScheduleId = cat.depreciationMethodScheduleId;
				detail2.isActive = true;
				cat.CategoryDepreciationScheduleList.Add(detail2);
			}
			cat.InventoryParentCategoriesId = Conversion.ToInt(CmbItemParentCategory.Value);
			cat.CategoryStatus = Conversion.ToBool((object)chkstatus.Checked);
			cat.AssetCategoryId = FixedAssetCategoryId;
			cat.OrganizationId = UserAccount.OrganizationId;
			cat.CompanyId = UserAccount.CompanyId;
			cat.EntryUser = UserAccount.ID;
			cat.ModifyUser = UserAccount.ID;
			cat.CategoryAttributeList = new List<CategoryAttribute>();
			if (AttributesForItemFeature)
			{
				GridEXRow[] checkedRows = grdAttributes.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					if (Conversion.ToInt(r.Cells["StatusValue"].Value) == 0)
					{
						CategoryAttribute detail = new CategoryAttribute();
						detail.CategoryAttributeId = Conversion.ToInt(r.Cells["CategoryAttributeId"].Value);
						detail.attributeId = Conversion.ToInt(r.Cells["attributeId"].Value);
						detail.itemCategoryId = RecId;
						detail.organizationId = UserAccount.OrganizationId;
						detail.companyId = UserAccount.CompanyId;
						detail.EntryUserId = UserAccount.ID;
						detail.ModifyUserId = UserAccount.ID;
						detail.EntryDate = DateTime.Now;
						detail.ModifyDate = DateTime.Now;
						cat.CategoryAttributeList.Add(detail);
					}
				}
			}
			ItemCategory.Save(cat);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully.");
			}
			else
			{
				MessageBox.Show("Record Save Successfully.");
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
			RecId = Id;
			ItemCategory cat = ItemCategory.GetByID(RecId);
			if (cat == null)
			{
				return;
			}
			btnsave.Visible = false;
			btnupdate.Visible = true;
			txtItemCategoryCode.Text = Conversion.ToString((object)cat.CategoryCode);
			txtItemCategoryDescription.Text = Conversion.ToString((object)cat.CategoryDescription);
			txtItemCategorySerialFrom.Text = Conversion.ToString((object)cat.SerialFrom);
			txtItemCategorySerialTo.Text = Conversion.ToString((object)cat.SerialTo);
			cmbItemCategoryRevenueAccount.Value = Conversion.ToInt((object)cat.RevenueAccountId);
			cmbItemCategoryCGSAccount.Value = Conversion.ToInt((object)cat.CGSAccountId);
			cmbItemCategoryInventoryAccount.Value = Conversion.ToInt((object)cat.InventoryAccountId);
			if (ParentCategoryIds.Contains("5") && FormTypeId == 3)
			{
				cmbItemCategoryRevenueAccount.Value = cat.accumulatedDepreciationAcId;
				cmbItemCategoryInventoryAccount.Value = cat.CapitalWipAcId;
				cmbItemCategoryCGSAccount.Value = cat.DepreciationExpenseAcId;
				if (cat.ExpenseMaintenanceAccountId > 0)
				{
					CmbExpenseMaintenanceAccount.Value = cat.ExpenseMaintenanceAccountId;
				}
				if (cat.depreciationMethodScheduleId > 0)
				{
					CmbDepreciationMethodName.Value = cat.depreciationMethodScheduleId;
				}
				txtUseFullLifeMonth.Text = Conversion.ToString((object)cat.usefullLifeInMonths);
				txtDepreciationRatePercent.Text = Conversion.ToString((object)cat.depriciationrate);
			}
			CmbItemParentCategory.Value = Conversion.ToInt((object)cat.InventoryParentCategoriesId);
			CmbItemClassGroup.Value = Conversion.ToInt((object)cat.ItemClassGroupId);
			chkstatus.Checked = Conversion.ToBool((object)cat.CategoryStatus);
			FixedAssetCategoryId = cat.AssetCategoryId;
			gridfill();
			((Control)(object)CmbItemParentCategory).Enabled = true;
			if (AttributesForItemFeature)
			{
				PanelAttributes.Visible = true;
				AttributeGridFill(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			DataTable dt = ItemCategory.FormHistory(new ItemCategory
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				CategoryCode = ParentCategoryIds
			});
			if (dt.Rows.Count > 0)
			{
				DataTable table = new DataTable();
				table.Columns.Add("Id", typeof(int));
				table.Columns.Add("CategoryCode", typeof(string));
				table.Columns.Add("CategoryDescription", typeof(string));
				table.Columns.Add("SerialFrom", typeof(int));
				table.Columns.Add("SerialTo", typeof(int));
				table.Columns.Add("InventoryParentCategoriesId", typeof(int));
				table.Columns.Add("CategoryStatus", typeof(string));
				table.Columns.Add("ParentCategory", typeof(string));
				table.Columns.Add("CGSAccountId", typeof(int));
				table.Columns.Add("CGSAccountTitle", typeof(string));
				table.Columns.Add("RevenueAccountId", typeof(int));
				table.Columns.Add("RevenueAccountTitle", typeof(string));
				table.Columns.Add("InventoryAccountId", typeof(int));
				table.Columns.Add("InventoryAccountTitle", typeof(string));
				table.Columns.Add("AccumulatedDepreciationAccount", typeof(string));
				table.Columns.Add("AssetGlAccount", typeof(string));
				table.Columns.Add("DepreciationExpenseAccount", typeof(string));
				table.Columns.Add("ExpenseMaintenanceAccount", typeof(string));
				table.Columns.Add("ClassGroupName", typeof(string));
				table.Columns.Add("DepreciationMethodName", typeof(string));
				table.Columns.Add("UsefullLifeInMonths", typeof(int));
				table.Columns.Add("DepreciationRate(%)", typeof(decimal));
				table.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					table.Rows.Add(Conversion.ToInt(dt.Rows[i]["Id"]), Conversion.ToString(dt.Rows[i]["categoryCode"]), Conversion.ToString(dt.Rows[i]["CategoryDescription"]), Conversion.ToInt(dt.Rows[i]["SerialFrom"]), Conversion.ToInt(dt.Rows[i]["SerialTo"]), Conversion.ToInt(dt.Rows[i]["InventoryParentCategoriesId"]), Conversion.ToString(dt.Rows[i]["CategoryStatus"]), Conversion.ToString(dt.Rows[i]["InvParentCateDescription"]), Conversion.ToInt(dt.Rows[i]["CGSAccountId"]), Conversion.ToString(dt.Rows[i]["CGSAccountTitle"]), Conversion.ToInt(dt.Rows[i]["RevenueAccountId"]), Conversion.ToString(dt.Rows[i]["RevenueAccountTitle"]), Conversion.ToInt(dt.Rows[i]["InventoryAccountId"]), Conversion.ToString(dt.Rows[i]["InventoryAccountTitle"]), Conversion.ToString(dt.Rows[i]["AccumulatedDepreciationAccount"]), Conversion.ToString(dt.Rows[i]["CapitalWipAccount"]), Conversion.ToString(dt.Rows[i]["DepreciationExpenseAccount"]), Conversion.ToString(dt.Rows[i]["ExpenseMaintenanceAccount"]), Conversion.ToString(dt.Rows[i]["ClassGroupName"]), Conversion.ToString(dt.Rows[i]["depreciationMethodName"]), Conversion.ToInt(dt.Rows[i]["usefullLifeInMonths"]), Conversion.ToDouble(dt.Rows[i]["depriciationrate"]));
				}
				grdfrm.DataSource = table;
				grdfrm.RetrieveStructure();
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
			List<string> hiddenColumns = new List<string> { "Id", "RevenueAccountId", "InventoryAccountId", "CGSAccountId", "InventoryParentCategoriesId" };
			if (ParentCategoryIds.Contains("5") && FormTypeId == 3)
			{
				hiddenColumns.AddRange(new string[3] { "CGSAccountTitle", "RevenueAccountTitle", "InventoryAccountTitle" });
			}
			else
			{
				hiddenColumns.AddRange(new string[7] { "AccumulatedDepreciationAccount", "DepreciationExpenseAccount", "CapitalWipAccount", "ExpenseMaintenanceAccount", "DepreciationMethodName", "DepreciationRate(%)", "UsefullLifeInMonths" });
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grdfrm, 3, 3, hiddenColumns);
			grdfrm.RootTable.Columns["CategoryCode"].Width = 100;
			grdfrm.RootTable.Columns["CategoryDescription"].Width = 150;
			grdfrm.RootTable.Columns["SerialFrom"].Width = 100;
			grdfrm.RootTable.Columns["SerialTo"].Width = 100;
			grdfrm.RootTable.Columns["CategoryStatus"].Width = 100;
			grdfrm.RootTable.Columns["ParentCategory"].Width = 120;
			grdfrm.RootTable.Columns["CGSAccountTitle"].Width = 200;
			grdfrm.RootTable.Columns["RevenueAccountTitle"].Width = 200;
			grdfrm.RootTable.Columns["InventoryAccountTitle"].Width = 200;
			grdfrm.RootTable.Columns["ExpenseMaintenanceAccount"].Width = 200;
			grdfrm.RootTable.Columns["ClassGroupName"].Width = 120;
			grdfrm.RootTable.Columns["DepreciationMethodName"].Width = 120;
			grdfrm.RootTable.Columns["UsefullLifeInMonths"].Width = 120;
			grdfrm.RootTable.Columns["DepreciationRate(%)"].Width = 100;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				ReadById(CurrId);
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
			RecId = 0;
			FixedAssetCategoryId = 0;
			PanelAttributes.Visible = AttributesForItemFeature;
			txtItemCategoryCode.Clear();
			txtItemCategoryDescription.Clear();
			txtItemCategorySerialFrom.Clear();
			txtItemCategorySerialTo.Clear();
			((Control)(object)cmbItemCategoryCGSAccount).Text = string.Empty;
			((Control)(object)cmbItemCategoryInventoryAccount).Text = string.Empty;
			((Control)(object)cmbItemCategoryRevenueAccount).Text = string.Empty;
			((Control)(object)CmbExpenseMaintenanceAccount).Text = string.Empty;
			((Control)(object)CmbItemClassGroup).Text = string.Empty;
			((Control)(object)CmbDepreciationMethodName).Text = string.Empty;
			txtDepreciationRatePercent.Text = string.Empty;
			txtUseFullLifeMonth.Text = string.Empty;
			gridfill();
			((Control)(object)CmbItemParentCategory).Enabled = true;
			((Control)(object)CmbItemParentCategory).Text = string.Empty;
			if (AttributesForItemFeature)
			{
				AttributeGridFill(0);
			}
			btnsave.Visible = true;
			btnupdate.Visible = false;
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			if (AttributesForItemFeature)
			{
				PanelAttributes.Visible = AttributesForItemFeature;
				AttributeGridFill(0);
			}
			ItemParentCategoryFill();
			GetItemClassGroup();
			AccountTitleFill();
			if (FormTypeId != 3)
			{
				AccountsBind();
			}
			else if (FormTypeId == 3)
			{
				AssetAccountBind();
				ExpenseAccountFill();
				DepreciatonMethodNameBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvDeffrmItemCatagory_FormClosing(object sender, FormClosingEventArgs e)
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdfrm).Name);
			if (fs != null)
			{
				grdfrm.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void InvDeffrmItemCatagory_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnnew_Click(null, null);
		}
		if (btnsave.Visible && e.Control && e.KeyCode == Keys.S)
		{
			btnsave_Click(null, null);
		}
		if (btnupdate.Visible && e.Control && e.KeyCode == Keys.U)
		{
			btnUpdate_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
	}

	private void txtItemCategorySerialFrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtDepreciationRatePercent_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnMultiLingo_Click(object sender, EventArgs e)
	{
		try
		{
			new ItemCategoryMultiLanguage(UserAccount).Show();
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
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
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
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
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Expected O, but got Unknown
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Expected O, but got Unknown
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Expected O, but got Unknown
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.StoreManagement.frmItemCatagoryStore));
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance14 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		this.lblCategoryDescription = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnMultiLingo = new System.Windows.Forms.ToolStripButton();
		this.grdfrm = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.chkstatus = new System.Windows.Forms.CheckBox();
		this.CmbExpenseMaintenanceAccount = new UltraCombo();
		this.lblExpenseMaintenanceAc = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBoxDepreciationMethod = new System.Windows.Forms.GroupBox();
		this.CmbDepreciationMethodName = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.txtDepreciationRatePercent = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtUseFullLifeMonth = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbItemClassGroup = new UltraCombo();
		this.CmbItemParentCategory = new UltraCombo();
		this.txtItemCategoryCode = new System.Windows.Forms.TextBox();
		this.lblDepresitionExpenses = new System.Windows.Forms.Label();
		this.cmbItemCategoryCGSAccount = new UltraCombo();
		this.cmbItemCategoryRevenueAccount = new UltraCombo();
		this.cmbItemCategoryInventoryAccount = new UltraCombo();
		this.lblWipCapital = new System.Windows.Forms.Label();
		this.lblAccumlatedDepricition = new System.Windows.Forms.Label();
		this.txtItemCategorySerialTo = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.txtItemCategoryDescription = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.txtItemCategorySerialFrom = new System.Windows.Forms.TextBox();
		this.lblParentCategory = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.PanelAttributes = new System.Windows.Forms.Panel();
		this.grdAttributes = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExpenseMaintenanceAccount).BeginInit();
		this.panel3.SuspendLayout();
		this.groupBoxDepreciationMethod.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDepreciationMethodName).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemClassGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemParentCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategoryCGSAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategoryRevenueAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategoryInventoryAccount).BeginInit();
		this.PanelAttributes.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAttributes).BeginInit();
		this.panel7.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		base.SuspendLayout();
		this.lblCategoryDescription.AutoSize = true;
		this.lblCategoryDescription.BackColor = System.Drawing.Color.Teal;
		this.lblCategoryDescription.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCategoryDescription.ForeColor = System.Drawing.Color.White;
		this.lblCategoryDescription.Location = new System.Drawing.Point(5, 2);
		this.lblCategoryDescription.Name = "lblCategoryDescription";
		this.lblCategoryDescription.Size = new System.Drawing.Size(157, 19);
		this.lblCategoryDescription.TabIndex = 5;
		this.lblCategoryDescription.Text = "Item Category PM";
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 29);
		this.panel1.TabIndex = 66;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnRefresh, this.btnupdate, this.btnsave, this.btnMultiLingo });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(964, 27);
		this.toolStrip1.TabIndex = 67;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnMultiLingo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnMultiLingo.Image = (System.Drawing.Image)resources.GetObject("btnMultiLingo.Image");
		this.btnMultiLingo.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnMultiLingo.Name = "btnMultiLingo";
		this.btnMultiLingo.Size = new System.Drawing.Size(101, 24);
		this.btnMultiLingo.Text = "Multi Lingo";
		this.btnMultiLingo.Click += new System.EventHandler(btnMultiLingo_Click);
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 23);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(614, 344);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.chkstatus);
		this.panel2.Controls.Add(this.lblCategoryDescription);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 29);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(964, 25);
		this.panel2.TabIndex = 67;
		this.chkstatus.AutoSize = true;
		this.chkstatus.Checked = true;
		this.chkstatus.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkstatus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkstatus.ForeColor = System.Drawing.Color.White;
		this.chkstatus.Location = new System.Drawing.Point(870, 2);
		this.chkstatus.Name = "chkstatus";
		this.chkstatus.Size = new System.Drawing.Size(90, 20);
		this.chkstatus.TabIndex = 9;
		this.chkstatus.Text = "Is Active";
		this.chkstatus.UseVisualStyleBackColor = true;
		this.CmbExpenseMaintenanceAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbExpenseMaintenanceAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbExpenseMaintenanceAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbExpenseMaintenanceAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbExpenseMaintenanceAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).Location = new System.Drawing.Point(779, 66);
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).Name = "CmbExpenseMaintenanceAccount";
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).Visible = false;
		this.lblExpenseMaintenanceAc.AutoSize = true;
		this.lblExpenseMaintenanceAc.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblExpenseMaintenanceAc.ForeColor = System.Drawing.Color.Black;
		this.lblExpenseMaintenanceAc.Location = new System.Drawing.Point(685, 64);
		this.lblExpenseMaintenanceAc.Name = "lblExpenseMaintenanceAc";
		this.lblExpenseMaintenanceAc.Size = new System.Drawing.Size(98, 30);
		this.lblExpenseMaintenanceAc.TabIndex = 608;
		this.lblExpenseMaintenanceAc.Text = "Expense \r\nMaintenance A/c";
		this.lblExpenseMaintenanceAc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblExpenseMaintenanceAc.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBoxDepreciationMethod);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 54);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(964, 145);
		this.panel3.TabIndex = 68;
		this.groupBoxDepreciationMethod.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDepreciationMethodName);
		this.groupBoxDepreciationMethod.Controls.Add(this.label3);
		this.groupBoxDepreciationMethod.Controls.Add(this.txtDepreciationRatePercent);
		this.groupBoxDepreciationMethod.Controls.Add(this.label5);
		this.groupBoxDepreciationMethod.Controls.Add(this.txtUseFullLifeMonth);
		this.groupBoxDepreciationMethod.Controls.Add(this.label4);
		this.groupBoxDepreciationMethod.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBoxDepreciationMethod.Location = new System.Drawing.Point(0, 98);
		this.groupBoxDepreciationMethod.Name = "groupBoxDepreciationMethod";
		this.groupBoxDepreciationMethod.Size = new System.Drawing.Size(960, 46);
		this.groupBoxDepreciationMethod.TabIndex = 1;
		this.groupBoxDepreciationMethod.TabStop = false;
		this.groupBoxDepreciationMethod.Text = "Depreciation Method";
		this.groupBoxDepreciationMethod.Visible = false;
		this.CmbDepreciationMethodName.AllowNull = (DefaultableBoolean)1;
		this.CmbDepreciationMethodName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDepreciationMethodName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDepreciationMethodName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance51).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDepreciationMethodName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDepreciationMethodName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDepreciationMethodName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDepreciationMethodName).Location = new System.Drawing.Point(99, 15);
		((System.Windows.Forms.Control)(object)this.CmbDepreciationMethodName).Name = "CmbDepreciationMethodName";
		((System.Windows.Forms.Control)(object)this.CmbDepreciationMethodName).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.CmbDepreciationMethodName).TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(3, 21);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(84, 15);
		this.label3.TabIndex = 6;
		this.label3.Text = "Method Name";
		this.txtDepreciationRatePercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDepreciationRatePercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDepreciationRatePercent.Location = new System.Drawing.Point(668, 17);
		this.txtDepreciationRatePercent.Name = "txtDepreciationRatePercent";
		this.txtDepreciationRatePercent.Size = new System.Drawing.Size(89, 23);
		this.txtDepreciationRatePercent.TabIndex = 2;
		this.txtDepreciationRatePercent.Tag = "double";
		this.txtDepreciationRatePercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDepreciationRatePercent_KeyPress);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(547, 21);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(114, 15);
		this.label5.TabIndex = 2;
		this.label5.Text = "Depreciation Rate %";
		this.txtUseFullLifeMonth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtUseFullLifeMonth.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtUseFullLifeMonth.Location = new System.Drawing.Point(452, 17);
		this.txtUseFullLifeMonth.Name = "txtUseFullLifeMonth";
		this.txtUseFullLifeMonth.Size = new System.Drawing.Size(89, 23);
		this.txtUseFullLifeMonth.TabIndex = 1;
		this.txtUseFullLifeMonth.Tag = "int";
		this.txtUseFullLifeMonth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemCategorySerialFrom_KeyPress);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(329, 21);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(119, 15);
		this.label4.TabIndex = 2;
		this.label4.Text = "UseFull Life (Months)";
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemClassGroup);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemParentCategory);
		this.groupBox1.Controls.Add(this.lblExpenseMaintenanceAc);
		this.groupBox1.Controls.Add(this.txtItemCategoryCode);
		this.groupBox1.Controls.Add(this.lblDepresitionExpenses);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCategoryCGSAccount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCategoryRevenueAccount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCategoryInventoryAccount);
		this.groupBox1.Controls.Add(this.lblWipCapital);
		this.groupBox1.Controls.Add(this.lblAccumlatedDepricition);
		this.groupBox1.Controls.Add(this.txtItemCategorySerialTo);
		this.groupBox1.Controls.Add(this.label37);
		this.groupBox1.Controls.Add(this.txtItemCategoryDescription);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.txtItemCategorySerialFrom);
		this.groupBox1.Controls.Add(this.lblParentCategory);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(960, 97);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.CmbItemClassGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbItemClassGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemClassGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemClassGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance72;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance77).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance80).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemClassGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemClassGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemClassGroup).Location = new System.Drawing.Point(779, 39);
		((System.Windows.Forms.Control)(object)this.CmbItemClassGroup).Name = "CmbItemClassGroup";
		((System.Windows.Forms.Control)(object)this.CmbItemClassGroup).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemClassGroup).TabIndex = 8;
		this.CmbItemParentCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance59;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance64).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).Location = new System.Drawing.Point(779, 12);
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).Name = "CmbItemParentCategory";
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).TabIndex = 7;
		this.txtItemCategoryCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemCategoryCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemCategoryCode.Location = new System.Drawing.Point(99, 14);
		this.txtItemCategoryCode.Name = "txtItemCategoryCode";
		this.txtItemCategoryCode.Size = new System.Drawing.Size(190, 23);
		this.txtItemCategoryCode.TabIndex = 0;
		this.lblDepresitionExpenses.AutoSize = true;
		this.lblDepresitionExpenses.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDepresitionExpenses.Location = new System.Drawing.Point(293, 72);
		this.lblDepresitionExpenses.Name = "lblDepresitionExpenses";
		this.lblDepresitionExpenses.Size = new System.Drawing.Size(77, 15);
		this.lblDepresitionExpenses.TabIndex = 6;
		this.lblDepresitionExpenses.Text = "CGS Account";
		this.cmbItemCategoryCGSAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCategoryCGSAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCategoryCGSAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCategoryCGSAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryCGSAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemCategoryCGSAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryCGSAccount).Location = new System.Drawing.Point(453, 66);
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryCGSAccount).Name = "cmbItemCategoryCGSAccount";
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryCGSAccount).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryCGSAccount).TabIndex = 6;
		this.cmbItemCategoryRevenueAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCategoryRevenueAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCategoryRevenueAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCategoryRevenueAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance83).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryRevenueAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemCategoryRevenueAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryRevenueAccount).Location = new System.Drawing.Point(453, 12);
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryRevenueAccount).Name = "cmbItemCategoryRevenueAccount";
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryRevenueAccount).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryRevenueAccount).TabIndex = 4;
		this.cmbItemCategoryInventoryAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCategoryInventoryAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCategoryInventoryAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCategoryInventoryAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance38).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryInventoryAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemCategoryInventoryAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryInventoryAccount).Location = new System.Drawing.Point(453, 39);
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryInventoryAccount).Name = "cmbItemCategoryInventoryAccount";
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryInventoryAccount).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryInventoryAccount).TabIndex = 5;
		this.lblWipCapital.AutoSize = true;
		this.lblWipCapital.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblWipCapital.Location = new System.Drawing.Point(293, 45);
		this.lblWipCapital.Name = "lblWipCapital";
		this.lblWipCapital.Size = new System.Drawing.Size(106, 15);
		this.lblWipCapital.TabIndex = 5;
		this.lblWipCapital.Text = "Inventory Account";
		this.lblAccumlatedDepricition.AutoSize = true;
		this.lblAccumlatedDepricition.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAccumlatedDepricition.Location = new System.Drawing.Point(293, 18);
		this.lblAccumlatedDepricition.Name = "lblAccumlatedDepricition";
		this.lblAccumlatedDepricition.Size = new System.Drawing.Size(100, 15);
		this.lblAccumlatedDepricition.TabIndex = 4;
		this.lblAccumlatedDepricition.Text = "Revenue Account";
		this.txtItemCategorySerialTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemCategorySerialTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemCategorySerialTo.Location = new System.Drawing.Point(190, 68);
		this.txtItemCategorySerialTo.Name = "txtItemCategorySerialTo";
		this.txtItemCategorySerialTo.Size = new System.Drawing.Size(99, 23);
		this.txtItemCategorySerialTo.TabIndex = 3;
		this.txtItemCategorySerialTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemCategorySerialFrom_KeyPress);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(3, 45);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(68, 15);
		this.label37.TabIndex = 1;
		this.label37.Text = "Description";
		this.txtItemCategoryDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemCategoryDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemCategoryDescription.Location = new System.Drawing.Point(99, 41);
		this.txtItemCategoryDescription.Name = "txtItemCategoryDescription";
		this.txtItemCategoryDescription.Size = new System.Drawing.Size(190, 23);
		this.txtItemCategoryDescription.TabIndex = 1;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(3, 18);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(34, 15);
		this.label36.TabIndex = 0;
		this.label36.Text = "Code";
		this.txtItemCategorySerialFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemCategorySerialFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemCategorySerialFrom.Location = new System.Drawing.Point(99, 68);
		this.txtItemCategorySerialFrom.Name = "txtItemCategorySerialFrom";
		this.txtItemCategorySerialFrom.Size = new System.Drawing.Size(89, 23);
		this.txtItemCategorySerialFrom.TabIndex = 2;
		this.txtItemCategorySerialFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemCategorySerialFrom_KeyPress);
		this.lblParentCategory.AutoSize = true;
		this.lblParentCategory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblParentCategory.Location = new System.Drawing.Point(685, 18);
		this.lblParentCategory.Name = "lblParentCategory";
		this.lblParentCategory.Size = new System.Drawing.Size(91, 15);
		this.lblParentCategory.TabIndex = 40;
		this.lblParentCategory.Text = "Parent Category";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(685, 45);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(69, 15);
		this.label8.TabIndex = 40;
		this.label8.Text = "Class Group";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(3, 72);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(95, 15);
		this.label38.TabIndex = 2;
		this.label38.Text = "Serial From && To";
		this.PanelAttributes.Controls.Add((System.Windows.Forms.Control)(object)this.grdAttributes);
		this.PanelAttributes.Controls.Add(this.panel7);
		this.PanelAttributes.Dock = System.Windows.Forms.DockStyle.Left;
		this.PanelAttributes.Location = new System.Drawing.Point(0, 199);
		this.PanelAttributes.Name = "PanelAttributes";
		this.PanelAttributes.Size = new System.Drawing.Size(350, 367);
		this.PanelAttributes.TabIndex = 42;
		this.grdAttributes.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdAttributes).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdAttributes.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdAttributes).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.grdAttributes.GroupTotals = (GroupTotals)2;
		this.grdAttributes.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdAttributes).Location = new System.Drawing.Point(0, 23);
		((System.Windows.Forms.Control)(object)this.grdAttributes).Name = "grdAttributes";
		((ScrollableBase)this.grdAttributes).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdAttributes).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdAttributes).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAttributes).Size = new System.Drawing.Size(350, 344);
		((System.Windows.Forms.Control)(object)this.grdAttributes).TabIndex = 69;
		this.grdAttributes.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAttributes).VisualStyle = (VisualStyle)4;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(350, 23);
		this.panel7.TabIndex = 68;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(3, 2);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(71, 14);
		this.label2.TabIndex = 5;
		this.label2.Text = "Attributes";
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(350, 199);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(614, 367);
		this.panel4.TabIndex = 69;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(614, 23);
		this.panel5.TabIndex = 68;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(569, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(40, 19);
		this.ctrlGrdBar1.TabIndex = 68;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(4, 2);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(67, 19);
		this.label1.TabIndex = 5;
		this.label1.Text = "History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 566);
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.PanelAttributes);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmItemCatagoryStore";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvDeffrmItemCatagory_FormClosing);
		base.Load += new System.EventHandler(InvDeffrmItemCatagory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvDeffrmItemCatagory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExpenseMaintenanceAccount).EndInit();
		this.panel3.ResumeLayout(false);
		this.groupBoxDepreciationMethod.ResumeLayout(false);
		this.groupBoxDepreciationMethod.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDepreciationMethodName).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemClassGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemParentCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategoryCGSAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategoryRevenueAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategoryInventoryAccount).EndInit();
		this.PanelAttributes.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAttributes).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		base.ResumeLayout(false);
	}
}
