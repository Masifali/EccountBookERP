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
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
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

public class InvDeffrmItemCatagory : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	public string ParentCategoryIds = "1,2,3,4,6,10,11";

	public string ClassGroupIds = "1,2,3";

	public int ParentCategoryId;

	private List<COAAllocation> Accountslst = new List<COAAllocation>();

	private DropDownBind DDL = new DropDownBind();

	private bool IfConfigAllAccountAllowOn;

	private bool AttributesForItemFeature;

	private bool MultiLanguageFeature;

	private bool AutoCoaDefineByItemNameOnInsert;

	private bool ItemCodingEnable;

	private bool SameAccountForStockAndRevenue;

	private DataTable dt3rdLevelAccounts = new DataTable();

	private IContainer components;

	private Label lblCategoryDescription;

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

	internal UltraCombo CmbVarityNature;

	internal UltraCombo CmbProductionStag;

	private Label label4;

	private Label label3;

	public InvDeffrmItemCatagory(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
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
			if (ParentCategoryId == 9)
			{
				ParentCategoryIds = "9";
				ClassGroupIds = "7";
			}
			PanelAttributes.Visible = false;
			ItemParentCategoryFill();
			GetItemClassGroup();
			MultiLanguageFeature = CommonServices.GetERPFeatureById(7);
			btnMultiLingo.Visible = MultiLanguageFeature;
			GetConfigurationsFromGlobal();
			if (ParentCategoryId == 5)
			{
				lblCategoryDescription.Text = "Fixed Assets Category";
				lblParentCategory.Text = "Assets Parent Category";
				lblAccumlatedDepricition.Text = "Accumulated Depreciation A/c";
				lblWipCapital.Text = "Assets GL A/c";
				lblDepresitionExpenses.Text = "Depreciation Expense A/c";
				AccumulatedDepreciationAccountFill();
				DepreciationExpenseAccountFill();
				FixedAssetsGridFill();
				CmbItemParentCategory.Value = ParentCategoryId;
				CmbItemClassGroup.Value = 6;
				((Control)(object)CmbItemParentCategory).Enabled = false;
				((Control)(object)CmbItemClassGroup).Enabled = false;
				ParentCategoryIds = "5";
				ClassGroupIds = "6";
			}
			else
			{
				ItemProducitonStageBind();
				ItemVarietyNatureBind();
				gridfill();
				AccountTitleFill();
				RevenueAcFill();
				((Control)(object)CmbItemParentCategory).Enabled = true;
				((Control)(object)CmbItemClassGroup).Enabled = true;
				if (AttributesForItemFeature)
				{
					PanelAttributes.Visible = AttributesForItemFeature;
					AttributeGridFill(0);
				}
			}
			btnupdate.Visible = false;
			if (ItemCodingEnable)
			{
				txtItemCategoryCode.Text = GenerateCode(Conversion.ToInt(CmbItemParentCategory.Value));
				txtItemCategoryDescription.Focus();
			}
			else
			{
				txtItemCategoryCode.Focus();
			}
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

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			ItemCodingEnable = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemCodingEnable"));
			IfConfigAllAccountAllowOn = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("All Account Allow on Item and Item Category"));
			SameAccountForStockAndRevenue = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SameAccountForStockAndRevenue"));
			AutoCoaDefineByItemNameOnInsert = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("AutoCoaDefineByItemNameOnInsert"));
			txtItemCategoryCode.Enabled = !ItemCodingEnable;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string GenerateCode(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			return ItemCategory.GenerateCode(new ItemCategory
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				InventoryParentCategoriesId = Id
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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

	private void ItemProducitonStageBind()
	{
		try
		{
			DataTable dt = ItemCategory.getItemProductionStage();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbProductionStag, "Id", "productionStageName", "Production Stage", ZeroIndex: false);
				((UltraGridBase)CmbProductionStag).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemVarietyNatureBind()
	{
		try
		{
			DataTable dt = ItemCategory.getItemVarietyNature();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbVarityNature, "Id", "VarietyNatureName", "Variety Nature", ZeroIndex: false);
				((UltraGridBase)CmbVarityNature).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccumulatedDepreciationAccountFill()
	{
		try
		{
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("1");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbItemCategoryInventoryAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				DDL.BindDDL(dt, cmbItemCategoryRevenueAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DepreciationExpenseAccountFill()
	{
		try
		{
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("14");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbItemCategoryCGSAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RevenueAcFill()
	{
		try
		{
			if (Accountslst.Count <= 0)
			{
				return;
			}
			if (AutoCoaDefineByItemNameOnInsert)
			{
				LoadThirdLevelAccounts();
				if (dt3rdLevelAccounts.Rows.Count <= 0)
				{
					return;
				}
				if (IfConfigAllAccountAllowOn)
				{
					DataTable dtInventory = CreateAccountDataTable();
					DataTable dtRevenue = CreateAccountDataTable();
					foreach (DataRow r in dt3rdLevelAccounts.Rows)
					{
						int accountTypeId = Conversion.ToInt(r["AccountTypeId"]);
						if (accountTypeId == 4)
						{
							dtInventory.Rows.Add(r["Id"], r["AccountTitle"], accountTypeId);
						}
						if (SameAccountForStockAndRevenue && accountTypeId == 4)
						{
							dtRevenue.Rows.Add(r["Id"], r["AccountTitle"], accountTypeId);
						}
						else if (!SameAccountForStockAndRevenue && (accountTypeId == 4 || accountTypeId == 10))
						{
							dtRevenue.Rows.Add(r["Id"], r["AccountTitle"], accountTypeId);
						}
					}
					DDL.BindDDL(dtInventory, cmbItemCategoryInventoryAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
					DDL.BindDDL(dtRevenue, cmbItemCategoryRevenueAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
					((UltraGridBase)cmbItemCategoryInventoryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)cmbItemCategoryRevenueAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
				else
				{
					DataTable dtInventory2 = CreateAccountDataTable();
					DataTable dtRevenue2 = CreateAccountDataTable();
					foreach (DataRow r2 in dt3rdLevelAccounts.Rows)
					{
						int accountTypeId2 = Conversion.ToInt(r2["AccountTypeId"]);
						switch (accountTypeId2)
						{
						case 4:
							dtInventory2.Rows.Add(r2["Id"], r2["AccountTitle"], accountTypeId2);
							break;
						case 10:
							dtRevenue2.Rows.Add(r2["Id"], r2["AccountTitle"], accountTypeId2);
							break;
						}
					}
					if (SameAccountForStockAndRevenue)
					{
						DDL.BindDDL(dtInventory2, cmbItemCategoryInventoryAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
						DDL.BindDDL(dtInventory2, cmbItemCategoryRevenueAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
						((UltraGridBase)cmbItemCategoryInventoryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
						((UltraGridBase)cmbItemCategoryRevenueAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					}
					else
					{
						if (dtInventory2.Rows.Count > 0)
						{
							DDL.BindDDL(dtInventory2, cmbItemCategoryInventoryAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
							((UltraGridBase)cmbItemCategoryInventoryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
						}
						if (dtRevenue2.Rows.Count > 0)
						{
							DDL.BindDDL(dtRevenue2, cmbItemCategoryRevenueAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
							((UltraGridBase)cmbItemCategoryInventoryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
						}
					}
				}
				BindCGSAccounts();
			}
			else
			{
				HandleDefaultCase();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadThirdLevelAccounts()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		if (dt3rdLevelAccounts == null || dt3rdLevelAccounts.Rows.Count == 0)
		{
			ReportsParameters coa = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Account_Level = 3
			};
			dt3rdLevelAccounts = ChartofAccount.ReadAllAccountgroup(coa);
		}
	}

	private void BindCGSAccounts()
	{
		DataTable dtCGS = new DataTable();
		dtCGS.Columns.Add("Id");
		dtCGS.Columns.Add("AccountTitle");
		dtCGS.Columns.Add("AccountTypeId");
		foreach (COAAllocation account in Accountslst)
		{
			if (account.AccountTypeId == 12)
			{
				dtCGS.Rows.Add(account.Id, account.AccountTitle, account.AccountTypeId);
			}
		}
		if (dtCGS.Rows.Count > 0)
		{
			DDL.BindDDL(dtCGS, cmbItemCategoryCGSAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
			((UltraGridBase)cmbItemCategoryCGSAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
		}
	}

	private void HandleDefaultCase()
	{
		DataTable dt = CreateAccountDataTable();
		DataTable dtSale = CreateAccountDataTable();
		foreach (COAAllocation account in Accountslst)
		{
			if (IfConfigAllAccountAllowOn)
			{
				if (account.AccountTypeId == 4 || account.AccountTypeId == 10)
				{
					dt.Rows.Add(account.Id, account.AccountTitle, account.AccountTypeId);
				}
			}
			else if (account.AccountTypeId == 4)
			{
				dt.Rows.Add(account.Id, account.AccountTitle, account.AccountTypeId);
			}
			else if (account.AccountTypeId == 10)
			{
				dtSale.Rows.Add(account.Id, account.AccountTitle, account.AccountTypeId);
			}
		}
		BindDefaultCaseAccounts(dt, dtSale);
		BindCGSAccounts();
	}

	private void BindDefaultCaseAccounts(DataTable dt, DataTable dtSale)
	{
		if (IfConfigAllAccountAllowOn)
		{
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbItemCategoryInventoryAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				((UltraGridBase)cmbItemCategoryInventoryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				DDL.BindDDL(dt, cmbItemCategoryRevenueAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				((UltraGridBase)cmbItemCategoryRevenueAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
			return;
		}
		if (dt.Rows.Count > 0)
		{
			DDL.BindDDL(dt, cmbItemCategoryInventoryAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
			((UltraGridBase)cmbItemCategoryInventoryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
		}
		if (dtSale.Rows.Count > 0)
		{
			DDL.BindDDL(dtSale, cmbItemCategoryRevenueAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
			((UltraGridBase)cmbItemCategoryRevenueAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
		}
	}

	private DataTable CreateAccountDataTable()
	{
		return new DataTable
		{
			Columns = { "Id", "AccountTitle", "AccountTypeId" }
		};
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
			MessageBox.Show("Please Select Revenue Account");
			cmbItemCategoryRevenueAccount.Focus();
			return false;
		}
		if (((Control)(object)cmbItemCategoryInventoryAccount).Text.Trim() == "" || Conversion.ToInt(cmbItemCategoryInventoryAccount.Value) == 0)
		{
			MessageBox.Show("Please Select Inventory Account");
			cmbItemCategoryInventoryAccount.Focus();
			return false;
		}
		if (((Control)(object)cmbItemCategoryCGSAccount).Text.Trim() == "" || Conversion.ToInt(cmbItemCategoryCGSAccount.Value) == 0)
		{
			MessageBox.Show("Please Select Revenue Account");
			cmbItemCategoryCGSAccount.Focus();
			return false;
		}
		if (((Control)(object)CmbItemParentCategory).Text.Trim() == "" || Conversion.ToInt(CmbItemParentCategory.Value) == 0)
		{
			MessageBox.Show("Please Select Parent Category");
			CmbItemParentCategory.Focus();
			return false;
		}
		if ((ParentCategoryId != 5 || ParentCategoryId != 9) && (((UltraGridBase)CmbProductionStag).ActiveRow == null || Conversion.ToInt(CmbProductionStag.Value) == 0))
		{
			MessageBox.Show("Please Select Production Stage");
			CmbProductionStag.Focus();
			return false;
		}
		if ((ParentCategoryId != 5 || ParentCategoryId != 9) && (((UltraGridBase)CmbVarityNature).ActiveRow == null || Conversion.ToInt(CmbVarityNature.Value) == 0))
		{
			MessageBox.Show("Please Select Variety Nature");
			CmbVarityNature.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
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
			if (AutoCoaDefineByItemNameOnInsert && SameAccountForStockAndRevenue && Conversion.ToInt(cmbItemCategoryInventoryAccount.Value) != Conversion.ToInt(cmbItemCategoryRevenueAccount.Value))
			{
				cmbItemCategoryRevenueAccount.Focus();
				throw new Exception("Revenue Account Should be Same as Inventory Account");
			}
			int num = Conversion.ToInt(((UltraDropDownBase)cmbItemCategoryRevenueAccount).SelectedRow.Cells["AccountTypeId"].Value);
			int InventorytypeId = Conversion.ToInt(((UltraDropDownBase)cmbItemCategoryInventoryAccount).SelectedRow.Cells["AccountTypeId"].Value);
			if (num == 4 && InventorytypeId == 4 && Conversion.ToInt(cmbItemCategoryInventoryAccount.Value) != Conversion.ToInt(cmbItemCategoryRevenueAccount.Value))
			{
				cmbItemCategoryRevenueAccount.Focus();
				throw new Exception("Revenue Account Should be Same as Inventory Account or Select an Account Of Type Sale In Revenue Account");
			}
			cat.CategoryCode = Conversion.ToString((object)txtItemCategoryCode.Text);
			cat.CategoryDescription = Conversion.ToString((object)txtItemCategoryDescription.Text);
			cat.SerialFrom = Conversion.ToInt((object)txtItemCategorySerialFrom.Text);
			cat.SerialTo = Conversion.ToInt((object)txtItemCategorySerialTo.Text);
			cat.RevenueAccountId = Conversion.ToInt(cmbItemCategoryRevenueAccount.Value);
			cat.CGSAccountId = Conversion.ToInt(cmbItemCategoryCGSAccount.Value);
			cat.InventoryAccountId = Conversion.ToInt(cmbItemCategoryInventoryAccount.Value);
			cat.ItemClassGroupId = Conversion.ToInt(CmbItemClassGroup.Value);
			cat.ItemProductionStageId = Conversion.ToInt(CmbProductionStag.Value);
			cat.ItemVarietyNatureId = Conversion.ToInt(CmbVarityNature.Value);
			if (ParentCategoryId == 5)
			{
				cat.accumulatedDepreciationAcId = Conversion.ToInt(cmbItemCategoryRevenueAccount.Value);
				cat.CapitalWipAcId = Conversion.ToInt(cmbItemCategoryInventoryAccount.Value);
				cat.DepreciationExpenseAcId = Conversion.ToInt(cmbItemCategoryCGSAccount.Value);
			}
			cat.InventoryParentCategoriesId = Conversion.ToInt(CmbItemParentCategory.Value);
			cat.CategoryStatus = Conversion.ToBool((object)chkstatus.Checked);
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
			if (cat != null)
			{
				btnsave.Visible = false;
				btnupdate.Visible = true;
				txtItemCategoryCode.Text = Conversion.ToString((object)cat.CategoryCode);
				txtItemCategoryDescription.Text = Conversion.ToString((object)cat.CategoryDescription);
				txtItemCategorySerialFrom.Text = Conversion.ToString((object)cat.SerialFrom);
				txtItemCategorySerialTo.Text = Conversion.ToString((object)cat.SerialTo);
				cmbItemCategoryRevenueAccount.Value = Conversion.ToInt((object)cat.RevenueAccountId);
				cmbItemCategoryCGSAccount.Value = Conversion.ToInt((object)cat.CGSAccountId);
				cmbItemCategoryInventoryAccount.Value = Conversion.ToInt((object)cat.InventoryAccountId);
				CmbItemParentCategory.Value = Conversion.ToInt((object)cat.InventoryParentCategoriesId);
				CmbItemClassGroup.Value = Conversion.ToInt((object)cat.ItemClassGroupId);
				CmbProductionStag.Value = Conversion.ToInt((object)cat.ItemProductionStageId);
				CmbVarityNature.Value = Conversion.ToInt((object)cat.ItemVarietyNatureId);
				chkstatus.Checked = Conversion.ToBool((object)cat.CategoryStatus);
				if (ParentCategoryId == 5)
				{
					FixedAssetsGridFill();
					CmbItemParentCategory.Value = ParentCategoryId;
					((Control)(object)CmbItemParentCategory).Enabled = false;
				}
				else
				{
					gridfill();
					((Control)(object)CmbItemParentCategory).Enabled = true;
				}
				if (AttributesForItemFeature)
				{
					PanelAttributes.Visible = true;
					AttributeGridFill(RecId);
				}
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
				table.Columns.Add("ClassGroupName", typeof(string));
				table.Columns.Add("ProductionStage", typeof(string));
				table.Columns.Add("VarietyNature", typeof(string));
				table.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					table.Rows.Add(Conversion.ToInt(dt.Rows[i]["Id"]), Conversion.ToString(dt.Rows[i]["categoryCode"]), Conversion.ToString(dt.Rows[i]["CategoryDescription"]), Conversion.ToInt(dt.Rows[i]["SerialFrom"]), Conversion.ToInt(dt.Rows[i]["SerialTo"]), Conversion.ToInt(dt.Rows[i]["InventoryParentCategoriesId"]), Conversion.ToString(dt.Rows[i]["CategoryStatus"]), Conversion.ToString(dt.Rows[i]["InvParentCateDescription"]), Conversion.ToInt(dt.Rows[i]["CGSAccountId"]), Conversion.ToString(dt.Rows[i]["CGSAccountTitle"]), Conversion.ToInt(dt.Rows[i]["RevenueAccountId"]), Conversion.ToString(dt.Rows[i]["RevenueAccountTitle"]), Conversion.ToInt(dt.Rows[i]["InventoryAccountId"]), Conversion.ToString(dt.Rows[i]["InventoryAccountTitle"]), Conversion.ToString(dt.Rows[i]["ClassGroupName"]), Conversion.ToString(dt.Rows[i]["productionStageName"]), Conversion.ToString(dt.Rows[i]["VarietyNatureName"]));
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

	private void FixedAssetsGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = ItemCategory.FormHistoryForAssetsCategory(new ItemCategory
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				grdfrm.DataSource = dt;
				grdfrm.RetrieveStructure();
				FixedAssetGridsetting();
			}
			else
			{
				grdfrm.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsetting()
	{
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["RevenueAccountId"].Visible = false;
			grdfrm.RootTable.Columns["InventoryAccountId"].Visible = false;
			grdfrm.RootTable.Columns["CGSAccountId"].Visible = false;
			grdfrm.RootTable.Columns["InventoryParentCategoriesId"].Visible = false;
			grdfrm.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdfrm.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdfrm.RootTable.Columns["CategoryCode"].Width = 100;
			grdfrm.RootTable.Columns["CategoryDescription"].Width = 150;
			grdfrm.RootTable.Columns["SerialFrom"].Width = 100;
			grdfrm.RootTable.Columns["SerialTo"].Width = 100;
			grdfrm.RootTable.Columns["CategoryStatus"].Width = 100;
			grdfrm.RootTable.Columns["ParentCategory"].Width = 120;
			grdfrm.RootTable.Columns["CGSAccountTitle"].Width = 150;
			grdfrm.RootTable.Columns["RevenueAccountTitle"].Width = 150;
			grdfrm.RootTable.Columns["InventoryAccountTitle"].Width = 150;
			grdfrm.RootTable.Columns["ClassGroupName"].Width = 120;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FixedAssetGridsetting()
	{
		try
		{
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["RevenueAccountId"].Visible = false;
			grdfrm.RootTable.Columns["InventoryAccountId"].Visible = false;
			grdfrm.RootTable.Columns["InventoryParentCategoriesId"].Visible = false;
			grdfrm.RootTable.Columns["accumulatedDepreciationAcId"].Visible = false;
			grdfrm.RootTable.Columns["DepreciationExpenseAcId"].Visible = false;
			grdfrm.RootTable.Columns["CapitalWipAcId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdfrm);
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
			PanelAttributes.Visible = AttributesForItemFeature;
			txtItemCategoryCode.Clear();
			txtItemCategoryDescription.Clear();
			txtItemCategorySerialFrom.Clear();
			txtItemCategorySerialTo.Clear();
			((Control)(object)cmbItemCategoryCGSAccount).Text = string.Empty;
			((Control)(object)cmbItemCategoryInventoryAccount).Text = string.Empty;
			((Control)(object)cmbItemCategoryRevenueAccount).Text = string.Empty;
			((Control)(object)CmbItemClassGroup).Text = string.Empty;
			if (ParentCategoryId == 5)
			{
				FixedAssetsGridFill();
				CmbItemParentCategory.Value = ParentCategoryId;
				((Control)(object)CmbItemParentCategory).Enabled = false;
			}
			else
			{
				gridfill();
				((Control)(object)CmbItemParentCategory).Enabled = true;
				((Control)(object)CmbItemParentCategory).Text = string.Empty;
				if (AttributesForItemFeature)
				{
					AttributeGridFill(0);
				}
			}
			if (ItemCodingEnable)
			{
				txtItemCategoryCode.Text = GenerateCode(Conversion.ToInt(CmbItemParentCategory.Value));
				txtItemCategoryDescription.Focus();
			}
			else
			{
				txtItemCategoryCode.Focus();
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
			GetConfigurationsFromGlobal();
			ItemParentCategoryFill();
			GetItemClassGroup();
			AccountTitleFill();
			RevenueAcFill();
			ItemProducitonStageBind();
			ItemVarietyNatureBind();
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

	private void txtItemCategorySerialTo_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
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

	private void CmbItemParentCategory_Leave(object sender, EventArgs e)
	{
		try
		{
			txtItemCategoryCode.Text = GenerateCode(Conversion.ToInt(CmbItemParentCategory.Value));
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
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Expected O, but got Unknown
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Definition.InvDeffrmItemCatagory));
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
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
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
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
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
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtItemCategoryCode = new System.Windows.Forms.TextBox();
		this.CmbVarityNature = new UltraCombo();
		this.CmbProductionStag = new UltraCombo();
		this.CmbItemClassGroup = new UltraCombo();
		this.chkstatus = new System.Windows.Forms.CheckBox();
		this.CmbItemParentCategory = new UltraCombo();
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
		this.label4 = new System.Windows.Forms.Label();
		this.txtItemCategorySerialFrom = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
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
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbVarityNature).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionStag).BeginInit();
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
		this.lblCategoryDescription.Size = new System.Drawing.Size(127, 19);
		this.lblCategoryDescription.TabIndex = 5;
		this.lblCategoryDescription.Text = "Item Category";
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
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(614, 354);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.lblCategoryDescription);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 29);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(964, 25);
		this.panel2.TabIndex = 67;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 54);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(964, 135);
		this.panel3.TabIndex = 68;
		this.groupBox1.Controls.Add(this.txtItemCategoryCode);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVarityNature);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionStag);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemClassGroup);
		this.groupBox1.Controls.Add(this.chkstatus);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemParentCategory);
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
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtItemCategorySerialFrom);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.lblParentCategory);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(960, 132);
		this.groupBox1.TabIndex = 41;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.txtItemCategoryCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemCategoryCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemCategoryCode.Location = new System.Drawing.Point(105, 14);
		this.txtItemCategoryCode.Name = "txtItemCategoryCode";
		this.txtItemCategoryCode.Size = new System.Drawing.Size(190, 23);
		this.txtItemCategoryCode.TabIndex = 0;
		this.CmbVarityNature.AllowNull = (DefaultableBoolean)1;
		this.CmbVarityNature.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVarityNature.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVarityNature.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVarityNature).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.CmbVarityNature).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVarityNature).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVarityNature).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbVarityNature.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVarityNature).Location = new System.Drawing.Point(739, 41);
		((System.Windows.Forms.Control)(object)this.CmbVarityNature).Name = "CmbVarityNature";
		((System.Windows.Forms.Control)(object)this.CmbVarityNature).Size = new System.Drawing.Size(215, 26);
		((System.Windows.Forms.Control)(object)this.CmbVarityNature).TabIndex = 10;
		this.CmbProductionStag.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionStag.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionStag.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionStag.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionStag).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionStag).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionStag).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionStag).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProductionStag.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionStag).Location = new System.Drawing.Point(739, 11);
		((System.Windows.Forms.Control)(object)this.CmbProductionStag).Name = "CmbProductionStag";
		((System.Windows.Forms.Control)(object)this.CmbProductionStag).Size = new System.Drawing.Size(215, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionStag).TabIndex = 9;
		this.CmbItemClassGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbItemClassGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemClassGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemClassGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemClassGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemClassGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemClassGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemClassGroup).Location = new System.Drawing.Point(404, 100);
		((System.Windows.Forms.Control)(object)this.CmbItemClassGroup).Name = "CmbItemClassGroup";
		((System.Windows.Forms.Control)(object)this.CmbItemClassGroup).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemClassGroup).TabIndex = 8;
		this.chkstatus.AutoSize = true;
		this.chkstatus.Checked = true;
		this.chkstatus.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkstatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkstatus.Location = new System.Drawing.Point(883, 71);
		this.chkstatus.Name = "chkstatus";
		this.chkstatus.Size = new System.Drawing.Size(71, 19);
		this.chkstatus.TabIndex = 11;
		this.chkstatus.Text = "Is Active";
		this.chkstatus.UseVisualStyleBackColor = true;
		this.CmbItemParentCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).Location = new System.Drawing.Point(404, 71);
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).Name = "CmbItemParentCategory";
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).Leave += new System.EventHandler(CmbItemParentCategory_Leave);
		this.lblDepresitionExpenses.AutoSize = true;
		this.lblDepresitionExpenses.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDepresitionExpenses.Location = new System.Drawing.Point(300, 47);
		this.lblDepresitionExpenses.Name = "lblDepresitionExpenses";
		this.lblDepresitionExpenses.Size = new System.Drawing.Size(77, 15);
		this.lblDepresitionExpenses.TabIndex = 6;
		this.lblDepresitionExpenses.Text = "CGS Account";
		this.cmbItemCategoryCGSAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCategoryCGSAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCategoryCGSAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCategoryCGSAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCategoryCGSAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryCGSAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemCategoryCGSAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryCGSAccount).Location = new System.Drawing.Point(404, 41);
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
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategoryRevenueAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
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
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryRevenueAccount).Location = new System.Drawing.Point(105, 96);
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryRevenueAccount).Name = "cmbItemCategoryRevenueAccount";
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryRevenueAccount).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryRevenueAccount).TabIndex = 4;
		this.cmbItemCategoryInventoryAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCategoryInventoryAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCategoryInventoryAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCategoryInventoryAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryInventoryAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemCategoryInventoryAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryInventoryAccount).Location = new System.Drawing.Point(404, 13);
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryInventoryAccount).Name = "cmbItemCategoryInventoryAccount";
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryInventoryAccount).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryInventoryAccount).TabIndex = 5;
		this.lblWipCapital.AutoSize = true;
		this.lblWipCapital.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblWipCapital.Location = new System.Drawing.Point(300, 19);
		this.lblWipCapital.Name = "lblWipCapital";
		this.lblWipCapital.Size = new System.Drawing.Size(106, 15);
		this.lblWipCapital.TabIndex = 5;
		this.lblWipCapital.Text = "Inventory Account";
		this.lblAccumlatedDepricition.AutoSize = true;
		this.lblAccumlatedDepricition.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAccumlatedDepricition.Location = new System.Drawing.Point(3, 102);
		this.lblAccumlatedDepricition.Name = "lblAccumlatedDepricition";
		this.lblAccumlatedDepricition.Size = new System.Drawing.Size(100, 15);
		this.lblAccumlatedDepricition.TabIndex = 4;
		this.lblAccumlatedDepricition.Text = "Revenue Account";
		this.txtItemCategorySerialTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemCategorySerialTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemCategorySerialTo.Location = new System.Drawing.Point(195, 71);
		this.txtItemCategorySerialTo.Name = "txtItemCategorySerialTo";
		this.txtItemCategorySerialTo.Size = new System.Drawing.Size(99, 23);
		this.txtItemCategorySerialTo.TabIndex = 3;
		this.txtItemCategorySerialTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemCategorySerialTo_KeyPress);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(3, 47);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(68, 15);
		this.label37.TabIndex = 1;
		this.label37.Text = "Description";
		this.txtItemCategoryDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemCategoryDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemCategoryDescription.Location = new System.Drawing.Point(105, 43);
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
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(634, 47);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(82, 15);
		this.label4.TabIndex = 40;
		this.label4.Text = "Variety Nature";
		this.txtItemCategorySerialFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemCategorySerialFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemCategorySerialFrom.Location = new System.Drawing.Point(105, 71);
		this.txtItemCategorySerialFrom.Name = "txtItemCategorySerialFrom";
		this.txtItemCategorySerialFrom.Size = new System.Drawing.Size(89, 23);
		this.txtItemCategorySerialFrom.TabIndex = 2;
		this.txtItemCategorySerialFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemCategorySerialFrom_KeyPress);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(634, 17);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(99, 15);
		this.label3.TabIndex = 40;
		this.label3.Text = "Production Stage";
		this.lblParentCategory.AutoSize = true;
		this.lblParentCategory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblParentCategory.Location = new System.Drawing.Point(300, 77);
		this.lblParentCategory.Name = "lblParentCategory";
		this.lblParentCategory.Size = new System.Drawing.Size(91, 15);
		this.lblParentCategory.TabIndex = 40;
		this.lblParentCategory.Text = "Parent Category";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(300, 106);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(69, 15);
		this.label8.TabIndex = 40;
		this.label8.Text = "Class Group";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(3, 75);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(95, 15);
		this.label38.TabIndex = 2;
		this.label38.Text = "Serial From && To";
		this.PanelAttributes.Controls.Add((System.Windows.Forms.Control)(object)this.grdAttributes);
		this.PanelAttributes.Controls.Add(this.panel7);
		this.PanelAttributes.Dock = System.Windows.Forms.DockStyle.Left;
		this.PanelAttributes.Location = new System.Drawing.Point(0, 189);
		this.PanelAttributes.Name = "PanelAttributes";
		this.PanelAttributes.Size = new System.Drawing.Size(350, 377);
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
		((System.Windows.Forms.Control)(object)this.grdAttributes).Size = new System.Drawing.Size(350, 354);
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
		this.panel4.Location = new System.Drawing.Point(350, 189);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(614, 377);
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
		base.Name = "InvDeffrmItemCatagory";
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
		this.panel3.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbVarityNature).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionStag).EndInit();
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

}
