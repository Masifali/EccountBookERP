using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.VPS;
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

namespace Architecture.WinApp.StoreManagement;

public class AddItemPM : Form
{
	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formrights = new Rightsobjects();

	private ServerCredentials Sc = new ServerCredentials();

	public UserAccount UserAccount = new UserAccount();

	private Country country = new Country();

	private int RecId;

	private bool flag;

	public bool UpdateMode;

	private bool IsPartyProcessingFeatureOn;

	private bool AttributesForItemFeature;

	private bool IsVpsAttachmentsServiceOn;

	private bool AutoCoaDefineByItemNameOnInsert;

	private bool ItemCodingEnable;

	private string saveDirectory = "";

	private string fileSavePath = "";

	private string ofd = "";

	private string fileSavePath2 = "";

	private string ofd2 = "";

	private DataTable dtMasterItems = new DataTable();

	private DataTable dtCatergory = new DataTable();

	private IContainer components;

	private TabControl tabCAddItem;

	private TabPage tpItemDefinitionAddItemHistory;

	private Panel panel3;

	private Panel panel4;

	internal GridEX grdhistory;

	private Panel panel10;

	private CtrlGrdBar ctrlGrdBar1;

	private TabPage tpItemDefinitionAddItem;

	private Panel panel1;

	private Panel panel7;

	internal UltraCombo cmbTaxType;

	internal UltraCombo cmbItemType;

	internal UltraCombo cmbBaseUnit;

	internal UltraCombo cmbItemCategory;

	private CheckBox chkstatus;

	private Label label33;

	private Button btnimg1;

	private PictureBox picbox2;

	private PictureBox picbox1;

	internal UltraCombo cmbCGSGL;

	internal UltraCombo cmbSaleGL;

	internal UltraCombo cmbPurchaseGL;

	private CheckBox chbGST;

	private TextBox txtItemCode;

	private TextBox txtItemName;

	private Label label12;

	private Label label11;

	private Label label10;

	private Label label5;

	private Label label4;

	private Label label3;

	private Label label2;

	private Label label1;

	private Panel panel2;

	private Label label13;

	private Panel panel6;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnattachment;

	private ToolStripButton BtnRefresh;

	private ToolStrip miniToolStrip;

	private GroupBox groupBox1;

	internal GridEX grdAllocation;

	private Label label40;

	private TextBox txtBarcodeNo;

	private Label label21;

	private Label label15;

	private Label label14;

	private Label label8;

	private Label label7;

	private Label label24;

	private Label label23;

	private Label label22;

	private Label label20;

	private Label label6;

	private TextBox txtItemCodeNew;

	private CheckBox ChkIsThirdParty;

	private CheckBox ChkIsCompany;

	private TextBox txtReOrderLevel;

	private Label label17;

	private TextBox txtMaxStock;

	private Label label9;

	private TextBox txtMinStock;

	private TextBox txtReOrderQty;

	private Label label25;

	private GroupBox BrandImages;

	private GroupBox groupBox2;

	private Label label26;

	private TextBox txtTaxPercent;

	private Label lblTaxPercent;

	private RadioButton rdImport;

	private RadioButton rdLocal;

	private TextBox txtLeadTimeDay;

	private Label label16;

	private Button btnimg2;

	private ToolStripButton BtnDefineCategory;

	private ToolStripButton BtnItemType;

	private ToolStripButton BtnPrintBarCode;

	private ToolStripButton BtnUom;

	private ToolStripButton BtnItemUomSchedule;

	private ToolStripButton BtnAttributesAllocation;

	internal UltraCombo CmbPackSizeId;

	private TextBox txtPackSize;

	private Label label18;

	private CheckBox chkAllowMultiUom;

	private Button btnPic2Reset;

	private Button btnPic1Reset;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private Panel panel9;

	private GroupBox groupBox5;

	private Button btnsearch;

	internal UltraCombo CmbItemTypeHistory;

	private Label label39;

	internal UltraCombo cmbitemcathistory;

	private Label label32;

	private Panel panel5;

	private Label label19;

	internal UltraCombo CmbMasterItem;

	private Label label27;

	private Label label28;

	private Label label30;

	private TextBox txtEbWeight;

	private ToolStripButton btnDefileMasterItem;

	private ToolStripButton btnMasterItemsUpdate;

	internal UltraCombo CmbMasterItemHistory;

	private Label label29;

	private ToolStripButton btnRefreshHistory;

	private ToolStripButton btnItemAllocationToFlow;

	internal UltraCombo CmbRackName;

	private Label label31;

	private Label label34;

	private ToolStripButton btnDefineRack;

	public AddItemPM(UserAccount pUserAccount)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void InvDefrmAddItem_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnupdate.Enabled = formrights.DoHaveUpdateRights;
			BrandImages.Visible = true;
			AutoCoaDefineByItemNameOnInsert = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("AutoCoaDefineByItemNameOnInsertPm"));
			IsVpsAttachmentsServiceOn = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("IsVpsAttachmentsServiceOn"));
			ItemCodingEnable = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemCodingEnable"));
			if (IsVpsAttachmentsServiceOn)
			{
				Sc = CommonServices.GetServerCredentials();
			}
			ItemCategoryBind();
			ItemTypeFill();
			MasterItemFill();
			CompaniesBindInGrid();
			PurchaseGLAcFill();
			RackNameFillFromGlobal();
			CmbTaxTypeFill();
			BaseUnitFill();
			PackSizeIdForItem();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			IsPartyProcessingFeatureOn = CommonServices.GetERPFeatureById(8);
			ChkIsCompany.Visible = IsPartyProcessingFeatureOn;
			ChkIsThirdParty.Visible = IsPartyProcessingFeatureOn;
			AttributesForItemFeature = CommonServices.GetERPFeatureById(13);
			BtnAttributesAllocation.Visible = AttributesForItemFeature;
			HistoryComboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboBind()
	{
		try
		{
			List<string> allServices = new List<string> { "AllItems" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			IEnumerable<getGlobalAllItems> allItems = clsGlobalVariables.getGlobalAllItems.Where((getGlobalAllItems item) => item.InventoryParentCategoriesId == 7);
			if (allItems == null || !allItems.Any())
			{
				return;
			}
			DataTable dtType = new DataTable();
			dtType.Columns.Add("Id", typeof(int));
			dtType.Columns.Add("Name", typeof(string));
			DataTable dtCat = dtType.Clone();
			DataTable dtMs = dtType.Clone();
			HashSet<int> typeSet = new HashSet<int>();
			HashSet<int> catSet = new HashSet<int>();
			HashSet<int> msSet = new HashSet<int>();
			foreach (getGlobalAllItems row in allItems)
			{
				if (typeSet.Add(row.ItemTypeId) && row.ItemTypeId > 0)
				{
					dtType.Rows.Add(row.ItemTypeId, row.ItemType);
				}
				if (catSet.Add(row.ItemCategoryId) && row.ItemCategoryId > 0)
				{
					dtCat.Rows.Add(row.ItemCategoryId, row.ItemCategory);
				}
				if (msSet.Add(row.MasterItemId) && row.MasterItemId > 0)
				{
					dtMs.Rows.Add(row.MasterItemId, row.MasterItem);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(cmbitemcathistory, dtCat, "Id", "Name", "Item Category", AllColumns: true, Conversion.ToInt(cmbitemcathistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbItemType, dtType, "Id", "Name", "Item Type", AllColumns: true, Conversion.ToInt(cmbItemType.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbMasterItemHistory, dtMs, "Id", "Name", "Master Item", AllColumns: true, Conversion.ToInt(CmbMasterItemHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemCategoryBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ItemCategory cat = new ItemCategory();
			cat.OrganizationId = UserAccount.OrganizationId;
			cat.CompanyId = UserAccount.CompanyId;
			cat.CategoryCode = "7";
			dtCatergory = ItemCategory.Getall(cat);
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
					((UltraGridBase)cmbItemCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MasterItemFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtMasterItems.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			dtMasterItems = Item.getMasterItems(obj);
			DataTable dataTable = dtMasterItems;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				DDL.BindDDL(dtMasterItems, CmbMasterItem, "Id", "MasterItem", "Master Item", ZeroIndex: false);
				((UltraGridBase)CmbMasterItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
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
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("UomCode");
				dt.Columns.Add("Equivalent");
				for (int i = 0; i < dtuom.Rows.Count; i++)
				{
					dt.Rows.Add(dtuom.Rows[i]["Id"], dtuom.Rows[i]["UOMCode"], dtuom.Rows[i]["Equivalent"]);
				}
				DDL.BindDDL(dt, cmbBaseUnit, "Id", "UOMCode", "UOM Code", ZeroIndex: false);
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		try
		{
			DataTable dt = ItemType.Getall(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				TypeId = 14,
				ItemIds = "7"
			});
			if (dt.Rows.Count > 0 && dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbItemType, "Id", "TypeDescription", "Item Type", false);
				((UltraGridBase)cmbItemType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dt, CmbItemTypeHistory, "Id", "TypeDescription", "Item Type", false);
				((UltraGridBase)CmbItemTypeHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchaseGLAcFill()
	{
		try
		{
			DataTable dt = CommonServices.CoaAllocationGetForComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbPurchaseGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: false);
				DDL.BindDDL(dt, cmbCGSGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: false);
				DDL.BindDDL(dt, cmbSaleGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: false);
				((UltraGridBase)cmbPurchaseGL).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)cmbSaleGL).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)cmbCGSGL).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbTaxTypeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = TaxesTypes.Getall(new TaxesTypes
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtType = new DataTable();
			dtType.Columns.Add("Id");
			dtType.Columns.Add("TaxName");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Convert.ToInt32(dt.Rows[i]["Id"]) != 1)
				{
					dtType.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["TaxName"]);
				}
			}
			if (dtType.Rows.Count > 0)
			{
				DDL.BindDDL(dtType, cmbTaxType, "Id", "TaxName", "TaxName", ZeroIndex: false);
				((UltraGridBase)cmbTaxType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RackNameFillFromGlobal()
	{
		try
		{
			int Id = Conversion.ToInt(CmbRackName.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("RackName", typeof(string));
			dt.Columns.Add("invWarehouseId", typeof(int));
			dt.Columns.Add("WarehouseName", typeof(string));
			dt.Clear();
			List<GlobalWarehouseRack> dtAll = clsGlobalVariables.globalWarehouseRacks;
			if (dtAll.Count > 0)
			{
				for (int i = 0; i < dtAll.Count; i++)
				{
					dt.Rows.Add(dtAll[i].Id, dtAll[i].rackName, dtAll[i].invWarehouseId, dtAll[i].WareHouseName);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbRackName, dt, "Id", "RackName", "Rack Name", AllColumns: true, Id, insertDefaultRow: true);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbRackName).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbItemCategory_Leave(object sender, EventArgs e)
	{
		try
		{
			if (IsSaveButtonActive())
			{
				Item item = GetItemFromUI();
				GenerateItemCode(item);
				SetGLAccounts(item);
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private bool IsSaveButtonActive()
	{
		if (btnsave.Visible)
		{
			return btnsave.Enabled;
		}
		return false;
	}

	private Item GetItemFromUI()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		return new Item
		{
			OrganizationId = UserAccount.OrganizationId,
			CompanyId = UserAccount.CompanyId,
			ItemCategoryId = Conversion.ToInt(cmbItemCategory.Value),
			ItemTypeId = Conversion.ToInt(cmbItemType.Value)
		};
	}

	private void GenerateItemCode(Item item)
	{
		try
		{
			DataTable lst = Item.GenerateCode(item);
			if (lst.Rows.Count > 0)
			{
				txtItemCode.Text = lst.Rows[0]["ItemCode"].ToString();
				txtItemCodeNew.Text = (ItemCodingEnable ? (lst.Rows[0]["ItemCodeNew"].ToString() + "-" + Conversion.ToInt((object)txtItemCode.Text)) : lst.Rows[0]["ItemCodeNew"].ToString());
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private void SetGLAccounts(Item item)
	{
		try
		{
			DataTable dt = Item.GetGLAccountbyItemCategoryId(item);
			if (dt.Rows.Count != 0)
			{
				DataRow row = dt.Rows[0];
				cmbPurchaseGL.Value = row["InventoryAccountId"].ToString();
				cmbSaleGL.Value = row["RevenueAccountId"].ToString();
				cmbCGSGL.Value = row["CGSAccountId"].ToString();
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
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

	private void PackSizeIdForItem()
	{
		try
		{
			int ID = Conversion.ToInt(CmbPackSizeId.Value);
			DataTable dt = CommonServices.StaticColumnsService("PackSizeIdForItem");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbPackSizeId, "Id", "type", "Pack Size", ZeroIndex: false);
				((UltraGridBase)CmbPackSizeId).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbPackSizeId).Rows[0].Activate();
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbPackSizeId.Value = ID;
					}
					else
					{
						((Control)(object)CmbPackSizeId).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbPackSizeId).Text = string.Empty;
				((UltraGridBase)CmbPackSizeId).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationForAddItem()
	{
		if (txtItemName.Text.Trim() == "")
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
		if (((UltraGridBase)CmbRackName).ActiveRow == null)
		{
			MessageBox.Show("Rack Name Field is Required");
			CmbRackName.Focus();
			return false;
		}
		if (IsPartyProcessingFeatureOn && !ChkIsCompany.Checked && !ChkIsThirdParty.Checked)
		{
			MessageBox.Show("Please Check of the Check box ('Is Company' Or 'Is third Party') ");
			ChkIsCompany.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtMinStock.Text.Trim()) > Conversion.ToDouble((object)txtMaxStock.Text.Trim()))
		{
			MessageBox.Show("Min stock level cannot be greater than max stock level.");
			return false;
		}
		if (((UltraGridBase)cmbPurchaseGL).ActiveRow == null)
		{
			MessageBox.Show("Stock A/c Field is Required");
			cmbPurchaseGL.Focus();
			return false;
		}
		if (((UltraGridBase)cmbSaleGL).ActiveRow == null)
		{
			MessageBox.Show("Sale A/c Field is Required");
			cmbSaleGL.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCGSGL).ActiveRow == null)
		{
			MessageBox.Show("CGS A/c Filed is Required");
			cmbCGSGL.Focus();
			return false;
		}
		return true;
	}

	private void BtnSave_Click(object sender, EventArgs e)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Expected O, but got Unknown
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Expected O, but got Unknown
		//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Expected O, but got Unknown
		try
		{
			if (!FormValidationForAddItem())
			{
				return;
			}
			List<(string, int)> ImgList = new List<(string, int)>();
			if (fileSavePath != null && fileSavePath != string.Empty)
			{
				ImgList.Add((fileSavePath, 1));
			}
			if (fileSavePath2 != null && fileSavePath2 != string.Empty)
			{
				ImgList.Add((fileSavePath2, 2));
			}
			Item items = new Item();
			items.ItemCode = Conversion.ToString((object)txtItemCode.Text.Trim());
			items.ItemCodeNew = Conversion.ToString((object)txtItemCodeNew.Text.Trim());
			items.BarcodeNo = Conversion.ToString((object)txtBarcodeNo.Text.Trim());
			items.ItemName = Conversion.ToString((object)txtItemName.Text.Trim());
			items.BaseUnitId = Conversion.ToInt(cmbBaseUnit.Value);
			items.Equivalent = Conversion.ToDouble(((UltraDropDownBase)cmbBaseUnit).SelectedRow.Cells[2].Value);
			items.ItemCategoryId = Conversion.ToInt(cmbItemCategory.Value);
			items.ItemTypeId = Conversion.ToInt(cmbItemType.Value);
			items.MasterItemId = Conversion.ToInt(CmbMasterItem.Value);
			items.RackId = Conversion.ToInt(CmbRackName.Value);
			items.EmptyBagWeight = Conversion.ToDouble((object)txtEbWeight.Text);
			items.ItemClassId = 1;
			items.ItemStatus = true;
			items.MinStockLevel = Conversion.ToDouble((object)txtMinStock.Text);
			items.MaxStockLevel = Conversion.ToDouble((object)txtMaxStock.Text);
			items.ReorderLevel = Conversion.ToDouble((object)txtReOrderLevel.Text);
			items.ReOrderQty = Conversion.ToDouble((object)txtReOrderQty.Text);
			items.LeadTimeDay = Conversion.ToInt((object)txtLeadTimeDay.Text);
			items.PackSize = Conversion.ToDouble((object)txtPackSize.Text);
			items.PackSizeId = Conversion.ToInt(CmbPackSizeId.Value);
			items.AllowMultiUom = chkAllowMultiUom.Checked;
			items.Pic1 = Conversion.ToString((object)Path.GetFileName(fileSavePath));
			items.Pic2 = Conversion.ToString((object)Path.GetFileName(fileSavePath2));
			items.PurchaseGLAC = Conversion.ToInt(cmbPurchaseGL.Value);
			items.SaleGLAC = Conversion.ToInt(cmbSaleGL.Value);
			items.COGSGLAC = Conversion.ToInt(cmbCGSGL.Value);
			items.ItemGroupId = 0;
			items.OrganizationId = UserAccount.OrganizationId;
			items.CompanyId = UserAccount.CompanyId;
			items.EntryUser = UserAccount.ID;
			items.EntryDate = DateTime.Now;
			items.ModifyDate = DateTime.Now;
			items.IsImport = rdLocal.Checked;
			if (IsPartyProcessingFeatureOn)
			{
				items.IsThirdParty = ChkIsThirdParty.Checked;
				items.IsCompany = ChkIsCompany.Checked;
			}
			else
			{
				items.IsThirdParty = false;
				items.IsCompany = true;
			}
			if (chbGST.Checked)
			{
				items.ApplyGST = true;
				items.TaxTypeId = Conversion.ToInt(cmbTaxType.Value);
			}
			items.AttachmentsList = new List<DMSAttachments>();
			items.ItemAllocationlist = new List<ItemAllocation>();
			items.ItemImageslist = new List<ItemImage>();
			for (int i = 0; i < ImgList.Count; i++)
			{
				ItemImage img = new ItemImage();
				img.FileName = ImgList[i].Item1;
				img.SortNo = ImgList[i].Item2;
				if (img.SortNo == 1)
				{
					img.IsProfileImage = true;
				}
				img.EffetedDate = DateTime.Now;
				items.ItemImageslist.Add(img);
			}
			FormHelper.UpdateAttachmentsForObject<Item>(items, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
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
			if (Item.Save(items, AutoCoaDefineByItemNameOnInsert) >= 1)
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

	private void btnupdate_Click(object sender, EventArgs e)
	{
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Expected O, but got Unknown
		try
		{
			if (!FormValidationForAddItem())
			{
				return;
			}
			List<(string, string, int)> ImgList = new List<(string, string, int)>();
			if (fileSavePath != null && fileSavePath != string.Empty)
			{
				ImgList.Add((fileSavePath, Conversion.ToString(picbox1.Tag), 1));
			}
			if (fileSavePath2 != null && fileSavePath2 != string.Empty)
			{
				ImgList.Add((fileSavePath2, Conversion.ToString(picbox2.Tag), 2));
			}
			List<string> DeleteImgList = new List<string>();
			if (Conversion.ToString(picbox1.Tag) != null && Conversion.ToString(picbox1.Tag) != string.Empty && picbox1.Image == null)
			{
				DeleteImgList.Add(Conversion.ToString(picbox1.Tag));
			}
			if (Conversion.ToString(picbox2.Tag) != null && Conversion.ToString(picbox2.Tag) != string.Empty && picbox2.Image == null)
			{
				DeleteImgList.Add(Conversion.ToString(picbox2.Tag));
			}
			Item items = new Item();
			items.Id = RecId;
			items.ItemCode = Conversion.ToString((object)txtItemCode.Text.Trim());
			items.ItemCodeNew = Conversion.ToString((object)txtItemCodeNew.Text.Trim());
			items.BarcodeNo = Conversion.ToString((object)txtBarcodeNo.Text.Trim());
			items.ItemName = Conversion.ToString((object)txtItemName.Text.Trim());
			items.BaseUnitId = Conversion.ToInt(cmbBaseUnit.Value);
			items.Equivalent = Conversion.ToDouble(((UltraDropDownBase)cmbBaseUnit).SelectedRow.Cells[2].Value);
			items.ItemCategoryId = Conversion.ToInt(cmbItemCategory.Value);
			items.ItemTypeId = Conversion.ToInt(cmbItemType.Value);
			items.MasterItemId = Conversion.ToInt(CmbMasterItem.Value);
			items.RackId = Conversion.ToInt(CmbRackName.Value);
			items.EmptyBagWeight = Conversion.ToDouble((object)txtEbWeight.Text);
			items.ItemClassId = 1;
			items.ItemStatus = Conversion.ToBool((object)chkstatus.Checked);
			items.PurchaseGLAC = Conversion.ToInt(cmbPurchaseGL.Value);
			items.SaleGLAC = Conversion.ToInt(cmbSaleGL.Value);
			items.COGSGLAC = Conversion.ToInt(cmbCGSGL.Value);
			items.MinStockLevel = Conversion.ToDouble((object)txtMinStock.Text);
			items.MaxStockLevel = Conversion.ToDouble((object)txtMaxStock.Text);
			items.ReorderLevel = Conversion.ToDouble((object)txtReOrderLevel.Text);
			items.LeadTimeDay = Conversion.ToInt((object)txtLeadTimeDay.Text);
			items.PackSize = Conversion.ToDouble((object)txtPackSize.Text);
			items.PackSizeId = Conversion.ToInt(CmbPackSizeId.Value);
			items.AllowMultiUom = chkAllowMultiUom.Checked;
			items.ApplyGST = Conversion.ToBool((object)chbGST.Checked);
			items.OrganizationId = UserAccount.OrganizationId;
			items.CompanyId = UserAccount.CompanyId;
			items.ModifyUser = UserAccount.ID;
			items.EntryDate = DateTime.Now;
			items.ModifyDate = DateTime.Now;
			if (IsPartyProcessingFeatureOn)
			{
				items.IsThirdParty = ChkIsThirdParty.Checked;
				items.IsCompany = ChkIsCompany.Checked;
			}
			else
			{
				items.IsThirdParty = false;
				items.IsCompany = true;
			}
			items.Pic1 = ((fileSavePath != null && fileSavePath != string.Empty && Conversion.ToString(picbox1.Tag) != string.Empty) ? picbox1.AccessibleName : Conversion.ToString((object)Path.GetFileName(fileSavePath)));
			items.Pic2 = ((fileSavePath2 != null && fileSavePath2 != string.Empty && Conversion.ToString(picbox2.Tag) != string.Empty) ? picbox2.AccessibleName : Conversion.ToString((object)Path.GetFileName(fileSavePath2)));
			items.ItemImageslist = new List<ItemImage>();
			items.ItemImagesDeletelist = new List<ItemImage>();
			for (int i = 0; i < ImgList.Count; i++)
			{
				ItemImage img2 = new ItemImage();
				img2.FileName = ImgList[i].Item1;
				img2.ImagePath = ImgList[i].Item2;
				img2.SortNo = ImgList[i].Item3;
				if (img2.SortNo == 1)
				{
					img2.IsProfileImage = true;
				}
				img2.EffetedDate = DateTime.Now;
				items.ItemImageslist.Add(img2);
			}
			items.AttachmentsList = new List<DMSAttachments>();
			items.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<Item>(items, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			if (DeleteImgList != null && DeleteImgList.Count > 0)
			{
				foreach (string item in DeleteImgList)
				{
					ItemImage img = new ItemImage();
					img.ImagePath = item;
					img.EffetedDate = DateTime.Now;
					items.ItemImagesDeletelist.Add(img);
				}
			}
			if (Item.Save(items, AutoCoaDefineByItemNameOnInsert) >= 1)
			{
				MessageBox.Show("Update Successfully");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 0, base.Name);
			}
			refresh();
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
			RecId = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
			items = Item.GetByID(RecId);
			items.Id = RecId;
			tabCAddItem.SelectedIndex = 0;
			if (IsPartyProcessingFeatureOn)
			{
				ChkIsThirdParty.Checked = items.IsThirdParty;
				ChkIsCompany.Checked = items.IsCompany;
			}
			txtItemCode.Text = Conversion.ToString((object)items.ItemCode);
			txtItemCodeNew.Text = Conversion.ToString((object)items.ItemCodeNew);
			txtBarcodeNo.Text = Conversion.ToString((object)items.BarcodeNo);
			txtItemName.Text = Conversion.ToString((object)items.ItemName);
			cmbBaseUnit.Value = Conversion.ToInt((object)items.BaseUnitId);
			cmbItemCategory.Value = items.ItemCategoryId;
			cmbItemCategory.ReadOnly = true;
			cmbItemType.Value = items.ItemTypeId;
			if (items.MasterItemId > 0)
			{
				CmbMasterItem.Value = items.MasterItemId;
			}
			if (items.RackId > 0)
			{
				CmbRackName.Value = items.RackId;
			}
			chkstatus.Checked = Conversion.ToBool((object)items.ItemStatus);
			cmbPurchaseGL.Value = items.PurchaseGLAC;
			cmbSaleGL.Value = items.SaleGLAC;
			cmbCGSGL.Value = items.COGSGLAC;
			txtEbWeight.Text = Conversion.ToDouble((object)items.EmptyBagWeight).ToString("#,##0.###");
			txtMinStock.Text = Conversion.ToDouble((object)items.MinStockLevel).ToString("#,##0.###");
			txtMaxStock.Text = Conversion.ToDouble((object)items.MaxStockLevel).ToString("#,##0.###");
			txtReOrderLevel.Text = Conversion.ToDouble((object)items.ReorderLevel).ToString("#,##0.###");
			txtLeadTimeDay.Text = Conversion.ToInt((object)items.LeadTimeDay).ToString("#,##0");
			txtPackSize.Text = Conversion.ToDouble((object)items.PackSize).ToString("#,##0");
			CmbPackSizeId.Value = Conversion.ToInt((object)items.PackSizeId);
			chkAllowMultiUom.Checked = items.AllowMultiUom;
			chbGST.Checked = Conversion.ToBool((object)items.ApplyGST);
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			if (items.ItemImageslist == null || items.ItemImageslist.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < items.ItemImageslist.Count; i++)
			{
				if (items.ItemImageslist[i].SortNo == 1)
				{
					fileSavePath = items.ItemImageslist[i].FileName;
					if (!IsVpsAttachmentsServiceOn)
					{
						string path = Path.Combine(clsGlobalVariables.AttachmentFolderPath, items.ItemImageslist[i].ImagePath);
						if (File.Exists(path))
						{
							using MemoryStream stream = new MemoryStream();
							Image image = Image.FromFile(path);
							Bitmap bmp = new Bitmap(image);
							bmp.Save(stream, image.RawFormat);
							picbox1.Image = bmp;
							picbox1.Tag = fileSavePath;
							picbox1.AccessibleName = items.ItemImageslist[i].FileName;
							image.Dispose();
							stream.Position = 0L;
							stream.Dispose();
						}
					}
					else
					{
						DownloadItemImageFromVpsInUpdateCase(items.ItemImageslist[i].ImagePath, items.ItemImageslist[i].FileName, picbox1);
					}
				}
				else
				{
					if (items.ItemImageslist[i].SortNo != 2)
					{
						continue;
					}
					fileSavePath2 = items.ItemImageslist[i].FileName;
					if (!IsVpsAttachmentsServiceOn)
					{
						string path2 = Path.Combine(clsGlobalVariables.AttachmentFolderPath, items.ItemImageslist[i].ImagePath);
						if (File.Exists(path2))
						{
							using MemoryStream memoryStream = new MemoryStream();
							Image image2 = Image.FromFile(path2);
							Bitmap bmp2 = new Bitmap(image2);
							bmp2.Save(memoryStream, image2.RawFormat);
							picbox2.Image = bmp2;
							picbox2.Tag = fileSavePath2;
							picbox2.AccessibleName = items.ItemImageslist[i].FileName;
							image2.Dispose();
							memoryStream.Position = 0L;
							memoryStream.Dispose();
						}
					}
					else
					{
						DownloadItemImageFromVpsInUpdateCase(items.ItemImageslist[i].ImagePath, items.ItemImageslist[i].FileName, picbox2);
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DownloadItemImageFromVpsInUpdateCase(string Path, string fileName, PictureBox picbox)
	{
		string uri = "ftp://" + Sc.ServerIp + "/" + Path;
		try
		{
			FtpWebRequest obj = (FtpWebRequest)WebRequest.Create(uri);
			obj.Method = "RETR";
			obj.UsePassive = true;
			obj.UseBinary = true;
			obj.KeepAlive = false;
			obj.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
			using FtpWebResponse ftpWebResponse = (FtpWebResponse)obj.GetResponse();
			using Stream responseStream = ftpWebResponse.GetResponseStream();
			using MemoryStream memoryStream = new MemoryStream();
			responseStream.CopyTo(memoryStream);
			_ = (double)memoryStream.Length / 1048576.0;
			memoryStream.Position = 0L;
			using MemoryStream stream = new MemoryStream();
			Image image = Image.FromStream(memoryStream);
			Bitmap bmp = new Bitmap(image);
			bmp.Save(stream, image.RawFormat);
			picbox.Image = bmp;
			picbox.Tag = fileSavePath;
			picbox.AccessibleName = fileName;
			image.Dispose();
			stream.Position = 0L;
			stream.Dispose();
		}
		catch (WebException webEx)
		{
			if (webEx.Response is FtpWebResponse response)
			{
				throw new Exception("FTP Error: " + response.StatusDescription + " - " + webEx.Message);
			}
			throw new Exception("Error: " + webEx.Message);
		}
		catch (Exception ex)
		{
			throw new Exception("Error: " + ex.Message);
		}
	}

	private void refresh()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			cmbItemCategory.ReadOnly = false;
			RecId = 0;
			BrandImages.Visible = true;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			((Control)(object)CmbRackName).Text = string.Empty;
			((Control)(object)cmbCGSGL).Text = string.Empty;
			((Control)(object)cmbSaleGL).Text = string.Empty;
			txtMinStock.Text = string.Empty;
			txtMaxStock.Text = string.Empty;
			txtReOrderLevel.Text = string.Empty;
			txtItemCode.Clear();
			txtBarcodeNo.Clear();
			txtItemName.Clear();
			chbGST.Checked = false;
			cmbItemCategory.Focus();
			cmbItemCategory_Leave(null, null);
			ofd = string.Empty;
			fileSavePath = string.Empty;
			picbox1.Image = null;
			picbox1.Tag = "";
			ofd2 = string.Empty;
			fileSavePath2 = string.Empty;
			picbox2.Image = null;
			picbox2.Tag = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		refresh();
		((Control)(object)cmbBaseUnit).Text = string.Empty;
		((Control)(object)cmbItemType).Text = string.Empty;
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

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			AutoCoaDefineByItemNameOnInsert = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("AutoCoaDefineByItemNameOnInsertPm"));
			IsVpsAttachmentsServiceOn = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("IsVpsAttachmentsServiceOn"));
			List<string> allServices = new List<string> { "WarehouseRacks" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			MasterItemFill();
			ItemTypeFill();
			PurchaseGLAcFill();
			ItemCategoryBind();
			RackNameFillFromGlobal();
			CmbTaxTypeFill();
			PackSizeIdForItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddItemPM_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnsearch_Click(object sender, EventArgs e)
	{
		HistoryGridfill();
	}

	private void HistoryGridfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			obj.InventoryParentCategories = 7;
			obj.ScreenName = base.Name;
			obj.ItemCategoryId = Conversion.ToInt(cmbitemcathistory.Value);
			obj.ItemTypeId = Conversion.ToInt(CmbItemTypeHistory.Value);
			obj.MasterItemId = Conversion.ToInt(CmbMasterItemHistory.Value);
			DataTable grdlst = Item.FormHistory(obj);
			if (grdlst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("ItemCodeNew", typeof(string));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("MasterItem", typeof(int));
				dt.Columns.Add("EmptyBagWeight", typeof(double));
				dt.Columns.Add("TypeDescription", typeof(string));
				dt.Columns.Add("CategoryDescription", typeof(string));
				dt.Columns.Add("ParentCategory", typeof(string));
				dt.Columns.Add("ClassGroupName", typeof(string));
				dt.Columns.Add("ItemStatus", typeof(string));
				dt.Columns.Add("StockAc", typeof(string));
				dt.Columns.Add("SaleAc", typeof(string));
				dt.Columns.Add("CgsAc", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUserName", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Columns.Add("LeadTimeDay", typeof(int));
				for (int i = 0; i < grdlst.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(grdlst.Rows[i]["Id"]), Conversion.ToString(grdlst.Rows[i]["ItemCode"]), Conversion.ToString(grdlst.Rows[i]["ItemCodeNew"]), Conversion.ToString(grdlst.Rows[i]["ItemName"]), Conversion.ToInt(grdlst.Rows[i]["MasterItemId"]), Conversion.ToDouble(grdlst.Rows[i]["EmptyBagWeight"]), Conversion.ToString(grdlst.Rows[i]["TypeDescription"]), Conversion.ToString(grdlst.Rows[i]["CategoryDescription"]), Conversion.ToString(grdlst.Rows[i]["InvParentCateDescription"]), Conversion.ToString(grdlst.Rows[i]["ClassGroupName"]), Conversion.ToString(grdlst.Rows[i]["ItemStatus"]), Conversion.ToString(grdlst.Rows[i]["GlStockAccountTitle"]), Conversion.ToString(grdlst.Rows[i]["GlSaleAccountTitle"]), Conversion.ToString(grdlst.Rows[i]["GlCgsAccountTitle"]), grdlst.Rows[i]["EntryDate"], Conversion.ToString(grdlst.Rows[i]["EntryUserName"]), Conversion.ToInt(grdlst.Rows[i]["NoOfAttachments"]), Conversion.ToInt(grdlst.Rows[i]["LeadTimeDay"]));
				}
				grdhistory.DataSource = dt;
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

	private void GridComboBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grdhistory.RootTable.Columns["MasterItem"], dtMasterItems, "Id", "MasterItem", "Master Item");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void gridsetting()
	{
		try
		{
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["ParentCategory"].Visible = false;
			grdhistory.RootTable.Columns["ClassGroupName"].Visible = false;
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"MasterItem",
					(EditType)4
				},
				{
					"EmptyBagWeight",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdhistory, 2, 3, null, editableColumns);
			grdhistory.RootTable.Groups.Add("TypeDescription");
			grdhistory.HideColumnsWhenGrouped = (InheritableBoolean)1;
			GridComboBind();
			grdhistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grdhistory.RootTable.Columns["ItemCodeNew"].Width = Constants.InventoryConstants.ItemCode;
			grdhistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName + 30;
			grdhistory.RootTable.Columns["MasterItem"].Width = Constants.InventoryConstants.ItemName + 30;
			grdhistory.RootTable.Columns["TypeDescription"].Width = Constants.InventoryConstants.ItemType;
			grdhistory.RootTable.Columns["CategoryDescription"].Width = Constants.InventoryConstants.CategoryDescription;
			grdhistory.RootTable.Columns["ItemStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdhistory.RootTable.Columns["EmptyBagWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdhistory.RootTable.Columns["StockAc"].Width = Constants.InventoryConstants.AccountTitle;
			grdhistory.RootTable.Columns["SaleAc"].Width = Constants.InventoryConstants.AccountTitle;
			grdhistory.RootTable.Columns["CgsAc"].Width = Constants.InventoryConstants.AccountTitle;
			grdhistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["EntryUserName"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
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
			cmbitemcathistory.Focus();
		}
		else
		{
			txtItemName.Focus();
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboBind();
			GridComboBind();
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
			((Control)(object)cmbitemcathistory).Text = string.Empty;
			((Control)(object)CmbItemTypeHistory).Text = string.Empty;
			((Control)(object)CmbMasterItemHistory).Text = string.Empty;
			grdhistory.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "NoOfAttachments")
			{
				GridEXRow item = grdhistory.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(item.Cells["Id"].Value), base.Name);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void definition()
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
		try
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "(*.jpg; *.jpeg; *.gif; *.PNG; *.png)| *.jpg; *.jpeg; *.gif; *.PNG;*.png; ";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				Path.GetFileName(openFileDialog.FileName);
				picbox1.Image = new Bitmap(openFileDialog.FileName);
				fileSavePath = openFileDialog.FileName;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnimg2_Click(object sender, EventArgs e)
	{
		try
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "(*.jpg; *.jpeg; *.gif; *.PNG; *.png)| *.jpg; *.jpeg; *.gif; *.PNG;*.png; ";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				Path.GetFileName(openFileDialog.FileName);
				picbox2.Image = new Bitmap(openFileDialog.FileName);
				fileSavePath2 = openFileDialog.FileName;
			}
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdhistory).Name);
			if (fs != null)
			{
				grdhistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void btnitmtaxsch_Click(object sender, EventArgs e)
	{
		new InvDeffrmItemTaxSchdule(UserAccount).Show();
	}

	private void btnCropYear_Click(object sender, EventArgs e)
	{
		new DefineCropYear(UserAccount).Show();
	}

	private void btnItemGroup_Click(object sender, EventArgs e)
	{
		new DefineItemGroup(UserAccount).Show();
	}

	private void btnItemGroupSchedule_Click(object sender, EventArgs e)
	{
		new DefineItemGroupUOMSchedule(UserAccount).Show();
	}

	private void btnitmcat_Click(object sender, EventArgs e)
	{
		try
		{
			new frmItemCatagoryStore(UserAccount).Show();
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
			InvDeffrmItemType invDeffrmItemType = new InvDeffrmItemType(UserAccount);
			invDeffrmItemType.FormTypeId = 1;
			invDeffrmItemType.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnBarCodePrint_Click(object sender, EventArgs e)
	{
		new BarcodeGenerate(UserAccount).Show();
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

	private void BtnAttributesAllocation_Click(object sender, EventArgs e)
	{
		try
		{
			new frmAttributeAllocationToItem(UserAccount).Show();
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

	private void txtBarcodeNo_KeyDown(object sender, KeyEventArgs e)
	{
	}

	private void txtMinStock_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtMaxStock_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtReOrderLevel_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtReOrderQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtLeadTimeDay_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPic1Reset_Click(object sender, EventArgs e)
	{
		try
		{
			fileSavePath = null;
			picbox1.Image = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPic2Reset_Click(object sender, EventArgs e)
	{
		try
		{
			fileSavePath2 = null;
			picbox2.Image = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEbWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnDefileMasterItem_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineMasterItems(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnMasterItemsUpdate_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		try
		{
			MasterItemAllocations obj = new MasterItemAllocations();
			obj.MasterItemAllocationslist = new List<MasterItemAllocations>();
			GridEXRow[] dataRows = grdhistory.GetDataRows();
			foreach (GridEXRow item in dataRows)
			{
				double EbWeight = Conversion.ToDouble(item.Cells["EmptyBagWeight"].Value);
				if (Conversion.ToInt(item.Cells["MasterItem"].Value) > 0 || EbWeight > 0.0)
				{
					MasterItemAllocations detail = new MasterItemAllocations();
					detail.ItemId = Conversion.ToInt(item.Cells["Id"].Value);
					detail.MasterItemId = Conversion.ToInt(item.Cells["MasterItem"].Value);
					detail.EmptyBagWeight = EbWeight;
					detail.EntryUserId = UserAccount.ID;
					detail.ModifyUserId = UserAccount.ID;
					obj.MasterItemAllocationslist.Add(detail);
				}
			}
			Item.SaveMasterItemAllocations(obj);
			MessageBox.Show("Record Updated successfully...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnItemAllocationToFlow_Click(object sender, EventArgs e)
	{
		try
		{
			new frmPackingMaterialItemsAllocateToTransactionFlow(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefineRack_Click(object sender, EventArgs e)
	{
		try
		{
			new ItemAllocationToStoreRack(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItemType_Leave(object sender, EventArgs e)
	{
		try
		{
			Item item = GetItemFromUI();
			GenerateItemCode(item);
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
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
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Expected O, but got Unknown
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Expected O, but got Unknown
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Expected O, but got Unknown
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Expected O, but got Unknown
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Expected O, but got Unknown
		//IL_05de: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Expected O, but got Unknown
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Expected O, but got Unknown
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0609: Expected O, but got Unknown
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Expected O, but got Unknown
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Expected O, but got Unknown
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Expected O, but got Unknown
		//IL_0846: Unknown result type (might be due to invalid IL or missing references)
		//IL_0850: Expected O, but got Unknown
		//IL_0872: Unknown result type (might be due to invalid IL or missing references)
		//IL_087c: Expected O, but got Unknown
		//IL_0888: Unknown result type (might be due to invalid IL or missing references)
		//IL_0892: Expected O, but got Unknown
		//IL_089e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a8: Expected O, but got Unknown
		//IL_0917: Unknown result type (might be due to invalid IL or missing references)
		//IL_0921: Expected O, but got Unknown
		//IL_66d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_66dd: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.StoreManagement.AddItemPM));
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
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
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
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
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		this.tabCAddItem = new System.Windows.Forms.TabControl();
		this.tpItemDefinitionAddItem = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.grdAllocation = new GridEX();
		this.BrandImages = new System.Windows.Forms.GroupBox();
		this.btnPic2Reset = new System.Windows.Forms.Button();
		this.btnPic1Reset = new System.Windows.Forms.Button();
		this.picbox2 = new System.Windows.Forms.PictureBox();
		this.btnimg2 = new System.Windows.Forms.Button();
		this.btnimg1 = new System.Windows.Forms.Button();
		this.picbox1 = new System.Windows.Forms.PictureBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label30 = new System.Windows.Forms.Label();
		this.txtEbWeight = new System.Windows.Forms.TextBox();
		this.CmbMasterItem = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.txtItemCodeNew = new System.Windows.Forms.TextBox();
		this.rdImport = new System.Windows.Forms.RadioButton();
		this.chkAllowMultiUom = new System.Windows.Forms.CheckBox();
		this.chbGST = new System.Windows.Forms.CheckBox();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.rdLocal = new System.Windows.Forms.RadioButton();
		this.txtItemCode = new System.Windows.Forms.TextBox();
		this.CmbPackSizeId = new UltraCombo();
		this.cmbTaxType = new UltraCombo();
		this.ChkIsThirdParty = new System.Windows.Forms.CheckBox();
		this.chkstatus = new System.Windows.Forms.CheckBox();
		this.ChkIsCompany = new System.Windows.Forms.CheckBox();
		this.txtPackSize = new System.Windows.Forms.TextBox();
		this.txtLeadTimeDay = new System.Windows.Forms.TextBox();
		this.txtReOrderQty = new System.Windows.Forms.TextBox();
		this.txtItemName = new System.Windows.Forms.TextBox();
		this.txtReOrderLevel = new System.Windows.Forms.TextBox();
		this.cmbPurchaseGL = new UltraCombo();
		this.cmbSaleGL = new UltraCombo();
		this.txtMaxStock = new System.Windows.Forms.TextBox();
		this.cmbCGSGL = new UltraCombo();
		this.cmbItemCategory = new UltraCombo();
		this.txtMinStock = new System.Windows.Forms.TextBox();
		this.cmbBaseUnit = new UltraCombo();
		this.cmbItemType = new UltraCombo();
		this.txtBarcodeNo = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.lblTaxPercent = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.BtnDefineCategory = new System.Windows.Forms.ToolStripButton();
		this.BtnItemType = new System.Windows.Forms.ToolStripButton();
		this.btnDefileMasterItem = new System.Windows.Forms.ToolStripButton();
		this.BtnUom = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintBarCode = new System.Windows.Forms.ToolStripButton();
		this.BtnItemUomSchedule = new System.Windows.Forms.ToolStripButton();
		this.BtnAttributesAllocation = new System.Windows.Forms.ToolStripButton();
		this.btnItemAllocationToFlow = new System.Windows.Forms.ToolStripButton();
		this.tpItemDefinitionAddItemHistory = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.btnsearch = new System.Windows.Forms.Button();
		this.CmbMasterItemHistory = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.CmbItemTypeHistory = new UltraCombo();
		this.label39 = new System.Windows.Forms.Label();
		this.cmbitemcathistory = new UltraCombo();
		this.label32 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel10 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnMasterItemsUpdate = new System.Windows.Forms.ToolStripButton();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.CmbRackName = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.btnDefineRack = new System.Windows.Forms.ToolStripButton();
		this.tabCAddItem.SuspendLayout();
		this.tpItemDefinitionAddItem.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAllocation).BeginInit();
		this.BrandImages.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picbox2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picbox1).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackSizeId).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTaxType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPurchaseGL).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSaleGL).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCGSGL).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBaseUnit).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).BeginInit();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tpItemDefinitionAddItemHistory.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel9.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterItemHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemTypeHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitemcathistory).BeginInit();
		this.panel5.SuspendLayout();
		this.panel10.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRackName).BeginInit();
		base.SuspendLayout();
		this.tabCAddItem.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabCAddItem.Controls.Add(this.tpItemDefinitionAddItem);
		this.tabCAddItem.Controls.Add(this.tpItemDefinitionAddItemHistory);
		this.tabCAddItem.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabCAddItem.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.tabCAddItem.Location = new System.Drawing.Point(0, 0);
		this.tabCAddItem.Multiline = true;
		this.tabCAddItem.Name = "tabCAddItem";
		this.tabCAddItem.SelectedIndex = 0;
		this.tabCAddItem.Size = new System.Drawing.Size(1409, 706);
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
		this.tpItemDefinitionAddItem.Size = new System.Drawing.Size(1401, 677);
		this.tpItemDefinitionAddItem.TabIndex = 0;
		this.tpItemDefinitionAddItem.Text = "Add Item";
		this.panel1.Controls.Add(this.panel7);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 62);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1395, 612);
		this.panel1.TabIndex = 7;
		this.panel7.Controls.Add(this.groupBox1);
		this.panel7.Controls.Add(this.BrandImages);
		this.panel7.Controls.Add(this.groupBox2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1395, 612);
		this.panel7.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.grdAllocation);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(319, 321);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(432, 291);
		this.groupBox1.TabIndex = 2;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Item Allocation To Company";
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
		((System.Windows.Forms.Control)(object)this.grdAllocation).Size = new System.Drawing.Size(426, 269);
		((System.Windows.Forms.Control)(object)this.grdAllocation).TabIndex = 0;
		this.grdAllocation.TotalRow = (InheritableBoolean)1;
		this.grdAllocation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAllocation).VisualStyle = (VisualStyle)4;
		this.BrandImages.Controls.Add(this.btnPic2Reset);
		this.BrandImages.Controls.Add(this.btnPic1Reset);
		this.BrandImages.Controls.Add(this.picbox2);
		this.BrandImages.Controls.Add(this.btnimg2);
		this.BrandImages.Controls.Add(this.btnimg1);
		this.BrandImages.Controls.Add(this.picbox1);
		this.BrandImages.Dock = System.Windows.Forms.DockStyle.Top;
		this.BrandImages.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.BrandImages.Location = new System.Drawing.Point(319, 0);
		this.BrandImages.Name = "BrandImages";
		this.BrandImages.Size = new System.Drawing.Size(1076, 321);
		this.BrandImages.TabIndex = 1;
		this.BrandImages.TabStop = false;
		this.BrandImages.Text = "Brand Images";
		this.btnPic2Reset.BackColor = System.Drawing.Color.MediumTurquoise;
		this.btnPic2Reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPic2Reset.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPic2Reset.ForeColor = System.Drawing.Color.White;
		this.btnPic2Reset.Location = new System.Drawing.Point(555, 279);
		this.btnPic2Reset.Name = "btnPic2Reset";
		this.btnPic2Reset.Size = new System.Drawing.Size(42, 33);
		this.btnPic2Reset.TabIndex = 3;
		this.btnPic2Reset.Text = "X";
		this.btnPic2Reset.UseVisualStyleBackColor = false;
		this.btnPic2Reset.Click += new System.EventHandler(btnPic2Reset_Click);
		this.btnPic1Reset.BackColor = System.Drawing.Color.MediumTurquoise;
		this.btnPic1Reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPic1Reset.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPic1Reset.ForeColor = System.Drawing.Color.White;
		this.btnPic1Reset.Location = new System.Drawing.Point(248, 279);
		this.btnPic1Reset.Name = "btnPic1Reset";
		this.btnPic1Reset.Size = new System.Drawing.Size(43, 33);
		this.btnPic1Reset.TabIndex = 1;
		this.btnPic1Reset.Text = "X";
		this.btnPic1Reset.UseVisualStyleBackColor = false;
		this.btnPic1Reset.Click += new System.EventHandler(btnPic1Reset_Click);
		this.picbox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.picbox2.Location = new System.Drawing.Point(314, 22);
		this.picbox2.Name = "picbox2";
		this.picbox2.Size = new System.Drawing.Size(300, 250);
		this.picbox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picbox2.TabIndex = 36;
		this.picbox2.TabStop = false;
		this.btnimg2.BackColor = System.Drawing.Color.White;
		this.btnimg2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnimg2.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold);
		this.btnimg2.ForeColor = System.Drawing.Color.White;
		this.btnimg2.Image = (System.Drawing.Image)resources.GetObject("btnimg2.Image");
		this.btnimg2.Location = new System.Drawing.Point(349, 274);
		this.btnimg2.Name = "btnimg2";
		this.btnimg2.Size = new System.Drawing.Size(189, 43);
		this.btnimg2.TabIndex = 2;
		this.btnimg2.Text = "Browse";
		this.btnimg2.UseVisualStyleBackColor = false;
		this.btnimg2.Click += new System.EventHandler(btnimg2_Click);
		this.btnimg1.BackColor = System.Drawing.Color.White;
		this.btnimg1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Teal;
		this.btnimg1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Teal;
		this.btnimg1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnimg1.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnimg1.ForeColor = System.Drawing.Color.White;
		this.btnimg1.Image = (System.Drawing.Image)resources.GetObject("btnimg1.Image");
		this.btnimg1.Location = new System.Drawing.Point(36, 274);
		this.btnimg1.Name = "btnimg1";
		this.btnimg1.Size = new System.Drawing.Size(189, 43);
		this.btnimg1.TabIndex = 0;
		this.btnimg1.Text = "Browse";
		this.btnimg1.UseVisualStyleBackColor = false;
		this.btnimg1.Click += new System.EventHandler(btnimg1_Click);
		this.picbox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.picbox1.Location = new System.Drawing.Point(6, 22);
		this.picbox1.Name = "picbox1";
		this.picbox1.Size = new System.Drawing.Size(300, 250);
		this.picbox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picbox1.TabIndex = 35;
		this.picbox1.TabStop = false;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRackName);
		this.groupBox2.Controls.Add(this.label31);
		this.groupBox2.Controls.Add(this.label34);
		this.groupBox2.Controls.Add(this.label30);
		this.groupBox2.Controls.Add(this.txtEbWeight);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMasterItem);
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Controls.Add(this.label28);
		this.groupBox2.Controls.Add(this.txtItemCodeNew);
		this.groupBox2.Controls.Add(this.rdImport);
		this.groupBox2.Controls.Add(this.chkAllowMultiUom);
		this.groupBox2.Controls.Add(this.chbGST);
		this.groupBox2.Controls.Add(this.txtTaxPercent);
		this.groupBox2.Controls.Add(this.rdLocal);
		this.groupBox2.Controls.Add(this.txtItemCode);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackSizeId);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTaxType);
		this.groupBox2.Controls.Add(this.ChkIsThirdParty);
		this.groupBox2.Controls.Add(this.chkstatus);
		this.groupBox2.Controls.Add(this.ChkIsCompany);
		this.groupBox2.Controls.Add(this.txtPackSize);
		this.groupBox2.Controls.Add(this.txtLeadTimeDay);
		this.groupBox2.Controls.Add(this.txtReOrderQty);
		this.groupBox2.Controls.Add(this.txtItemName);
		this.groupBox2.Controls.Add(this.txtReOrderLevel);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPurchaseGL);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSaleGL);
		this.groupBox2.Controls.Add(this.txtMaxStock);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCGSGL);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCategory);
		this.groupBox2.Controls.Add(this.txtMinStock);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBaseUnit);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemType);
		this.groupBox2.Controls.Add(this.txtBarcodeNo);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label40);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add(this.lblTaxPercent);
		this.groupBox2.Controls.Add(this.label26);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.label33);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(0, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(319, 612);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(213, 155);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(23, 30);
		this.label30.TabIndex = 614;
		this.label30.Text = "EB \r\nWt";
		this.txtEbWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEbWeight.Location = new System.Drawing.Point(237, 159);
		this.txtEbWeight.Name = "txtEbWeight";
		this.txtEbWeight.Size = new System.Drawing.Size(71, 23);
		this.txtEbWeight.TabIndex = 6;
		this.txtEbWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEbWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbWeight_KeyPress);
		this.CmbMasterItem.AllowNull = (DefaultableBoolean)1;
		this.CmbMasterItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMasterItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMasterItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance117;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance122).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMasterItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Location = new System.Drawing.Point(109, 103);
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Name = "CmbMasterItem";
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).TabIndex = 3;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.Red;
		this.label27.Location = new System.Drawing.Point(93, 108);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(14, 17);
		this.label27.TabIndex = 611;
		this.label27.Text = "*";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(3, 109);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(71, 15);
		this.label28.TabIndex = 610;
		this.label28.Text = "Master Item";
		this.txtItemCodeNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemCodeNew.Location = new System.Drawing.Point(109, 79);
		this.txtItemCodeNew.Name = "txtItemCodeNew";
		this.txtItemCodeNew.ReadOnly = true;
		this.txtItemCodeNew.Size = new System.Drawing.Size(199, 23);
		this.txtItemCodeNew.TabIndex = 2;
		this.rdImport.AutoSize = true;
		this.rdImport.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdImport.Location = new System.Drawing.Point(109, 544);
		this.rdImport.Name = "rdImport";
		this.rdImport.Size = new System.Drawing.Size(68, 18);
		this.rdImport.TabIndex = 23;
		this.rdImport.Text = "Import";
		this.rdImport.UseVisualStyleBackColor = true;
		this.chkAllowMultiUom.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.chkAllowMultiUom.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkAllowMultiUom.Location = new System.Drawing.Point(37, 153);
		this.chkAllowMultiUom.Margin = new System.Windows.Forms.Padding(0);
		this.chkAllowMultiUom.Name = "chkAllowMultiUom";
		this.chkAllowMultiUom.Size = new System.Drawing.Size(60, 34);
		this.chkAllowMultiUom.TabIndex = 5;
		this.chkAllowMultiUom.Text = "Allow Multi";
		this.chkAllowMultiUom.TextAlign = System.Drawing.ContentAlignment.TopLeft;
		this.chkAllowMultiUom.UseVisualStyleBackColor = true;
		this.chkAllowMultiUom.Visible = false;
		this.chbGST.AutoSize = true;
		this.chbGST.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chbGST.Location = new System.Drawing.Point(212, 449);
		this.chbGST.Name = "chbGST";
		this.chbGST.Size = new System.Drawing.Size(66, 19);
		this.chbGST.TabIndex = 18;
		this.chbGST.Text = "Taxable";
		this.chbGST.UseVisualStyleBackColor = true;
		this.txtTaxPercent.BackColor = System.Drawing.Color.White;
		this.txtTaxPercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxPercent.Location = new System.Drawing.Point(109, 446);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.ReadOnly = true;
		this.txtTaxPercent.Size = new System.Drawing.Size(101, 23);
		this.txtTaxPercent.TabIndex = 17;
		this.txtTaxPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.rdLocal.AutoSize = true;
		this.rdLocal.Checked = true;
		this.rdLocal.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdLocal.Location = new System.Drawing.Point(109, 527);
		this.rdLocal.Name = "rdLocal";
		this.rdLocal.Size = new System.Drawing.Size(56, 18);
		this.rdLocal.TabIndex = 22;
		this.rdLocal.TabStop = true;
		this.rdLocal.Text = "Local";
		this.rdLocal.UseVisualStyleBackColor = true;
		this.txtItemCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemCode.Location = new System.Drawing.Point(233, 79);
		this.txtItemCode.Name = "txtItemCode";
		this.txtItemCode.ReadOnly = true;
		this.txtItemCode.Size = new System.Drawing.Size(76, 23);
		this.txtItemCode.TabIndex = 11;
		this.txtItemCode.TabStop = false;
		this.txtItemCode.Visible = false;
		this.CmbPackSizeId.AllowNull = (DefaultableBoolean)1;
		this.CmbPackSizeId.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackSizeId.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackSizeId.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.Appearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackSizeId).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance104;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackSizeId).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance109).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackSizeId).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackSizeId).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackSizeId.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackSizeId).Location = new System.Drawing.Point(212, 496);
		((System.Windows.Forms.Control)(object)this.CmbPackSizeId).Name = "CmbPackSizeId";
		((System.Windows.Forms.Control)(object)this.CmbPackSizeId).Size = new System.Drawing.Size(96, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackSizeId).TabIndex = 21;
		this.cmbTaxType.AllowNull = (DefaultableBoolean)1;
		this.cmbTaxType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTaxType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTaxType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance156;
		((SpecialBoxBase)((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbTaxType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Location = new System.Drawing.Point(109, 417);
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Name = "cmbTaxType";
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Size = new System.Drawing.Size(199, 26);
		((System.Windows.Forms.Control)(object)this.cmbTaxType).TabIndex = 16;
		this.ChkIsThirdParty.AutoSize = true;
		this.ChkIsThirdParty.Checked = true;
		this.ChkIsThirdParty.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkIsThirdParty.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.ChkIsThirdParty.Location = new System.Drawing.Point(196, 544);
		this.ChkIsThirdParty.Name = "ChkIsThirdParty";
		this.ChkIsThirdParty.Size = new System.Drawing.Size(112, 19);
		this.ChkIsThirdParty.TabIndex = 25;
		this.ChkIsThirdParty.Text = "Third Party Item";
		this.ChkIsThirdParty.UseVisualStyleBackColor = true;
		this.ChkIsThirdParty.Visible = false;
		this.chkstatus.AutoSize = true;
		this.chkstatus.Checked = true;
		this.chkstatus.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkstatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkstatus.Location = new System.Drawing.Point(109, 563);
		this.chkstatus.Name = "chkstatus";
		this.chkstatus.Size = new System.Drawing.Size(59, 19);
		this.chkstatus.TabIndex = 26;
		this.chkstatus.TabStop = false;
		this.chkstatus.Text = "Status";
		this.chkstatus.UseVisualStyleBackColor = true;
		this.ChkIsCompany.AutoSize = true;
		this.ChkIsCompany.Checked = true;
		this.ChkIsCompany.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkIsCompany.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.ChkIsCompany.Location = new System.Drawing.Point(196, 526);
		this.ChkIsCompany.Name = "ChkIsCompany";
		this.ChkIsCompany.Size = new System.Drawing.Size(105, 19);
		this.ChkIsCompany.TabIndex = 24;
		this.ChkIsCompany.Text = "Company Item";
		this.ChkIsCompany.UseVisualStyleBackColor = true;
		this.ChkIsCompany.Visible = false;
		this.txtPackSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackSize.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackSize.Location = new System.Drawing.Point(109, 498);
		this.txtPackSize.Name = "txtPackSize";
		this.txtPackSize.Size = new System.Drawing.Size(101, 23);
		this.txtPackSize.TabIndex = 20;
		this.txtPackSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtLeadTimeDay_KeyPress);
		this.txtLeadTimeDay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLeadTimeDay.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLeadTimeDay.Location = new System.Drawing.Point(109, 471);
		this.txtLeadTimeDay.Name = "txtLeadTimeDay";
		this.txtLeadTimeDay.Size = new System.Drawing.Size(199, 23);
		this.txtLeadTimeDay.TabIndex = 19;
		this.txtLeadTimeDay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtLeadTimeDay_KeyPress);
		this.txtReOrderQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtReOrderQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReOrderQty.Location = new System.Drawing.Point(109, 284);
		this.txtReOrderQty.Name = "txtReOrderQty";
		this.txtReOrderQty.Size = new System.Drawing.Size(199, 23);
		this.txtReOrderQty.TabIndex = 11;
		this.txtReOrderQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtReOrderQty_KeyPress);
		this.txtItemName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemName.Location = new System.Drawing.Point(109, 132);
		this.txtItemName.Name = "txtItemName";
		this.txtItemName.Size = new System.Drawing.Size(199, 23);
		this.txtItemName.TabIndex = 4;
		this.txtReOrderLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtReOrderLevel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReOrderLevel.Location = new System.Drawing.Point(109, 260);
		this.txtReOrderLevel.Name = "txtReOrderLevel";
		this.txtReOrderLevel.Size = new System.Drawing.Size(199, 23);
		this.txtReOrderLevel.TabIndex = 10;
		this.txtReOrderLevel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtReOrderLevel_KeyPress);
		this.cmbPurchaseGL.AllowNull = (DefaultableBoolean)1;
		this.cmbPurchaseGL.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPurchaseGL.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPurchaseGL.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPurchaseGL.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Location = new System.Drawing.Point(109, 336);
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Name = "cmbPurchaseGL";
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Size = new System.Drawing.Size(199, 26);
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).TabIndex = 13;
		this.cmbSaleGL.AllowNull = (DefaultableBoolean)1;
		this.cmbSaleGL.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSaleGL.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSaleGL.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSaleGL).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.cmbSaleGL).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance135).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSaleGL.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).Location = new System.Drawing.Point(109, 363);
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).Name = "cmbSaleGL";
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).Size = new System.Drawing.Size(199, 26);
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).TabIndex = 14;
		this.txtMaxStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMaxStock.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMaxStock.Location = new System.Drawing.Point(109, 236);
		this.txtMaxStock.Name = "txtMaxStock";
		this.txtMaxStock.Size = new System.Drawing.Size(199, 23);
		this.txtMaxStock.TabIndex = 9;
		this.txtMaxStock.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMaxStock_KeyPress);
		this.cmbCGSGL.AllowNull = (DefaultableBoolean)1;
		this.cmbCGSGL.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCGSGL.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCGSGL.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Appearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance96).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCGSGL.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Location = new System.Drawing.Point(109, 390);
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Name = "cmbCGSGL";
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Size = new System.Drawing.Size(199, 26);
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).TabIndex = 15;
		this.cmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance83).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Location = new System.Drawing.Point(109, 22);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Name = "cmbItemCategory";
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Leave += new System.EventHandler(cmbItemCategory_Leave);
		this.txtMinStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMinStock.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMinStock.Location = new System.Drawing.Point(109, 212);
		this.txtMinStock.Name = "txtMinStock";
		this.txtMinStock.Size = new System.Drawing.Size(199, 23);
		this.txtMinStock.TabIndex = 8;
		this.txtMinStock.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMinStock_KeyPress);
		this.cmbBaseUnit.AllowNull = (DefaultableBoolean)1;
		this.cmbBaseUnit.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBaseUnit.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBaseUnit.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Appearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance130;
		((SpecialBoxBase)((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance136).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBaseUnit.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Location = new System.Drawing.Point(109, 157);
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Name = "cmbBaseUnit";
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Size = new System.Drawing.Size(101, 26);
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).TabIndex = 5;
		this.cmbItemType.AllowNull = (DefaultableBoolean)1;
		this.cmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Location = new System.Drawing.Point(109, 50);
		((System.Windows.Forms.Control)(object)this.cmbItemType).Name = "cmbItemType";
		((System.Windows.Forms.Control)(object)this.cmbItemType).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemType).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Leave += new System.EventHandler(cmbItemType_Leave);
		this.txtBarcodeNo.BackColor = System.Drawing.Color.White;
		this.txtBarcodeNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBarcodeNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBarcodeNo.Location = new System.Drawing.Point(109, 187);
		this.txtBarcodeNo.Name = "txtBarcodeNo";
		this.txtBarcodeNo.Size = new System.Drawing.Size(199, 23);
		this.txtBarcodeNo.TabIndex = 7;
		this.txtBarcodeNo.KeyDown += new System.Windows.Forms.KeyEventHandler(txtBarcodeNo_KeyDown);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.Red;
		this.label20.Location = new System.Drawing.Point(93, 422);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(14, 17);
		this.label20.TabIndex = 586;
		this.label20.Text = "*";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Red;
		this.label6.Location = new System.Drawing.Point(93, 162);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(14, 17);
		this.label6.TabIndex = 590;
		this.label6.Text = "*";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.Red;
		this.label40.Location = new System.Drawing.Point(93, 190);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(14, 17);
		this.label40.TabIndex = 579;
		this.label40.Text = "*";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Red;
		this.label24.Location = new System.Drawing.Point(93, 341);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(14, 17);
		this.label24.TabIndex = 589;
		this.label24.Text = "*";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.Red;
		this.label7.Location = new System.Drawing.Point(93, 27);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(14, 17);
		this.label7.TabIndex = 580;
		this.label7.Text = "*";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.Red;
		this.label23.Location = new System.Drawing.Point(93, 368);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(14, 17);
		this.label23.TabIndex = 588;
		this.label23.Text = "*";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Red;
		this.label8.Location = new System.Drawing.Point(93, 82);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(14, 17);
		this.label8.TabIndex = 581;
		this.label8.Text = "*";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.Red;
		this.label22.Location = new System.Drawing.Point(93, 395);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(14, 17);
		this.label22.TabIndex = 587;
		this.label22.Text = "*";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.Red;
		this.label14.Location = new System.Drawing.Point(93, 135);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(14, 17);
		this.label14.TabIndex = 582;
		this.label14.Text = "*";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.Red;
		this.label15.Location = new System.Drawing.Point(93, 55);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(14, 17);
		this.label15.TabIndex = 583;
		this.label15.Text = "*";
		this.lblTaxPercent.AutoSize = true;
		this.lblTaxPercent.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTaxPercent.Location = new System.Drawing.Point(3, 452);
		this.lblTaxPercent.Name = "lblTaxPercent";
		this.lblTaxPercent.Size = new System.Drawing.Size(38, 15);
		this.lblTaxPercent.TabIndex = 608;
		this.lblTaxPercent.Text = "Tax %";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(3, 240);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(63, 15);
		this.label26.TabIndex = 606;
		this.label26.Text = "Max Stock";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(3, 83);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(62, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Item Code";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(3, 136);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 1;
		this.label2.Text = "Item Name";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(3, 155);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(35, 30);
		this.label3.TabIndex = 2;
		this.label3.Text = "Base \r\nUOM";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(3, 28);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(82, 15);
		this.label4.TabIndex = 3;
		this.label4.Text = "Item Category";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(3, 502);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(110, 15);
		this.label18.TabIndex = 602;
		this.label18.Text = "Pack Size(Capacity)";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(3, 423);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(53, 15);
		this.label33.TabIndex = 10;
		this.label33.Text = "Tax Type";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(3, 475);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(83, 15);
		this.label16.TabIndex = 602;
		this.label16.Text = "LeadTime Day";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(3, 56);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(60, 15);
		this.label5.TabIndex = 4;
		this.label5.Text = "Item Type";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(3, 288);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(72, 15);
		this.label25.TabIndex = 602;
		this.label25.Text = "ReOrder Qty";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(3, 342);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(60, 15);
		this.label10.TabIndex = 9;
		this.label10.Text = "Stock A/C";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(3, 369);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(52, 15);
		this.label11.TabIndex = 10;
		this.label11.Text = "Sale A/C";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(3, 396);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(52, 15);
		this.label12.TabIndex = 11;
		this.label12.Text = "CGS A/C";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(3, 264);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(80, 15);
		this.label17.TabIndex = 597;
		this.label17.Text = "ReOrder Level";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(3, 216);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(61, 15);
		this.label9.TabIndex = 594;
		this.label9.Text = "Min Stock";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(3, 191);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(69, 15);
		this.label21.TabIndex = 578;
		this.label21.Text = "Barcode No";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label13);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 35);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1395, 27);
		this.panel2.TabIndex = 6;
		this.label13.AutoSize = true;
		this.label13.BackColor = System.Drawing.Color.Teal;
		this.label13.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(3, 3);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(85, 19);
		this.label13.TabIndex = 0;
		this.label13.Text = "Add Item";
		this.panel6.Controls.Add(this.toolStrip1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1395, 32);
		this.panel6.TabIndex = 5;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[14]
		{
			this.btnnew, this.BtnRefresh, this.btnsave, this.btnupdate, this.btnattachment, this.BtnDefineCategory, this.BtnItemType, this.btnDefileMasterItem, this.BtnUom, this.BtnPrintBarCode,
			this.BtnItemUomSchedule, this.BtnAttributesAllocation, this.btnItemAllocationToFlow, this.btnDefineRack
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1395, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "Refresh";
		this.BtnRefresh.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(BtnSave_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = (System.Drawing.Image)resources.GetObject("btnattachment.Image");
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.BtnDefineCategory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnDefineCategory.Image = Architecture.WinApp.Properties.Resources.google_forms;
		this.BtnDefineCategory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDefineCategory.Name = "BtnDefineCategory";
		this.BtnDefineCategory.Size = new System.Drawing.Size(126, 24);
		this.BtnDefineCategory.Text = "Item Category";
		this.BtnDefineCategory.Click += new System.EventHandler(btnitmcat_Click);
		this.BtnItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnItemType.Image = (System.Drawing.Image)resources.GetObject("BtnItemType.Image");
		this.BtnItemType.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnItemType.Name = "BtnItemType";
		this.BtnItemType.Size = new System.Drawing.Size(98, 24);
		this.BtnItemType.Text = "Item Type";
		this.BtnItemType.Click += new System.EventHandler(btnitmtype_Click);
		this.btnDefileMasterItem.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDefileMasterItem.Image = (System.Drawing.Image)resources.GetObject("btnDefileMasterItem.Image");
		this.btnDefileMasterItem.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefileMasterItem.Name = "btnDefileMasterItem";
		this.btnDefileMasterItem.Size = new System.Drawing.Size(106, 24);
		this.btnDefileMasterItem.Text = "Master tem";
		this.btnDefileMasterItem.Click += new System.EventHandler(btnDefileMasterItem_Click);
		this.BtnUom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUom.Image = (System.Drawing.Image)resources.GetObject("BtnUom.Image");
		this.BtnUom.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnUom.Name = "BtnUom";
		this.BtnUom.Size = new System.Drawing.Size(94, 24);
		this.BtnUom.Text = "Item Uom";
		this.BtnUom.Click += new System.EventHandler(btnitmuom_Click);
		this.BtnPrintBarCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrintBarCode.Image = (System.Drawing.Image)resources.GetObject("BtnPrintBarCode.Image");
		this.BtnPrintBarCode.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintBarCode.Name = "BtnPrintBarCode";
		this.BtnPrintBarCode.Size = new System.Drawing.Size(119, 24);
		this.BtnPrintBarCode.Text = "Barcode Print";
		this.BtnPrintBarCode.Click += new System.EventHandler(btnBarCodePrint_Click);
		this.BtnItemUomSchedule.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnItemUomSchedule.Image = (System.Drawing.Image)resources.GetObject("BtnItemUomSchedule.Image");
		this.BtnItemUomSchedule.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnItemUomSchedule.Name = "BtnItemUomSchedule";
		this.BtnItemUomSchedule.Size = new System.Drawing.Size(159, 24);
		this.BtnItemUomSchedule.Text = "Item Uom Schedule";
		this.BtnItemUomSchedule.Visible = false;
		this.BtnItemUomSchedule.Click += new System.EventHandler(btnitmuomtaxsch_Click);
		this.BtnAttributesAllocation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAttributesAllocation.Image = (System.Drawing.Image)resources.GetObject("BtnAttributesAllocation.Image");
		this.BtnAttributesAllocation.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnAttributesAllocation.Name = "BtnAttributesAllocation";
		this.BtnAttributesAllocation.Size = new System.Drawing.Size(213, 24);
		this.BtnAttributesAllocation.Text = "Attribute Allocation to Item";
		this.BtnAttributesAllocation.Click += new System.EventHandler(BtnAttributesAllocation_Click);
		this.btnItemAllocationToFlow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnItemAllocationToFlow.Image = (System.Drawing.Image)resources.GetObject("btnItemAllocationToFlow.Image");
		this.btnItemAllocationToFlow.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnItemAllocationToFlow.Name = "btnItemAllocationToFlow";
		this.btnItemAllocationToFlow.Size = new System.Drawing.Size(149, 24);
		this.btnItemAllocationToFlow.Text = "Allocation to Flow";
		this.btnItemAllocationToFlow.Click += new System.EventHandler(btnItemAllocationToFlow_Click);
		this.tpItemDefinitionAddItemHistory.Controls.Add(this.panel3);
		this.tpItemDefinitionAddItemHistory.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.tpItemDefinitionAddItemHistory.Location = new System.Drawing.Point(4, 4);
		this.tpItemDefinitionAddItemHistory.Name = "tpItemDefinitionAddItemHistory";
		this.tpItemDefinitionAddItemHistory.Size = new System.Drawing.Size(1242, 677);
		this.tpItemDefinitionAddItemHistory.TabIndex = 6;
		this.tpItemDefinitionAddItemHistory.Text = "History";
		this.tpItemDefinitionAddItemHistory.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Controls.Add(this.panel10);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1242, 677);
		this.panel3.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel4.Controls.Add(this.panel9);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1242, 647);
		this.panel4.TabIndex = 69;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupByBoxVisible = false;
		this.grdhistory.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 80);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdhistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1242, 567);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 1;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		this.panel9.BackColor = System.Drawing.Color.White;
		this.panel9.Controls.Add(this.groupBox5);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 31);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1242, 49);
		this.panel9.TabIndex = 0;
		this.groupBox5.Controls.Add(this.btnsearch);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMasterItemHistory);
		this.groupBox5.Controls.Add(this.label29);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory);
		this.groupBox5.Controls.Add(this.label39);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitemcathistory);
		this.groupBox5.Controls.Add(this.label32);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(5, 2);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(899, 45);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Filters";
		this.btnsearch.BackColor = System.Drawing.Color.Teal;
		this.btnsearch.FlatAppearance.BorderSize = 0;
		this.btnsearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnsearch.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsearch.ForeColor = System.Drawing.Color.White;
		this.btnsearch.Location = new System.Drawing.Point(821, 14);
		this.btnsearch.Name = "btnsearch";
		this.btnsearch.Size = new System.Drawing.Size(70, 26);
		this.btnsearch.TabIndex = 3;
		this.btnsearch.Text = "Search";
		this.btnsearch.UseVisualStyleBackColor = false;
		this.btnsearch.Click += new System.EventHandler(btnsearch_Click);
		this.CmbMasterItemHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbMasterItemHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMasterItemHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMasterItemHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance143;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance149).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance150).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMasterItemHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMasterItemHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMasterItemHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMasterItemHistory).Location = new System.Drawing.Point(584, 14);
		((System.Windows.Forms.Control)(object)this.CmbMasterItemHistory).Name = "CmbMasterItemHistory";
		((System.Windows.Forms.Control)(object)this.CmbMasterItemHistory).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.CmbMasterItemHistory).TabIndex = 2;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(509, 20);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(71, 15);
		this.label29.TabIndex = 2;
		this.label29.Text = "Master Item";
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
		((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory).Location = new System.Drawing.Point(343, 14);
		((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory).Name = "CmbItemTypeHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory).Size = new System.Drawing.Size(162, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory).TabIndex = 1;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(280, 20);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(60, 15);
		this.label39.TabIndex = 2;
		this.label39.Text = "Item Type";
		this.cmbitemcathistory.AllowNull = (DefaultableBoolean)1;
		this.cmbitemcathistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitemcathistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitemcathistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance70).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance73).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitemcathistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Location = new System.Drawing.Point(86, 14);
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Name = "cmbitemcathistory";
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).TabIndex = 0;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(2, 20);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(82, 15);
		this.label32.TabIndex = 0;
		this.label32.Text = "Item Category";
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label19);
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1242, 31);
		this.panel5.TabIndex = 74;
		this.label19.AutoSize = true;
		this.label19.BackColor = System.Drawing.Color.Teal;
		this.label19.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(3, 6);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(261, 19);
		this.label19.TabIndex = 0;
		this.label19.Text = "Packing Material Items History";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1201, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdhistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel10.BackColor = System.Drawing.Color.White;
		this.panel10.Controls.Add(this.toolStrip2);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1242, 30);
		this.panel10.TabIndex = 65;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.btnRefreshHistory, this.btnMasterItemsUpdate });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1242, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "&Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnMasterItemsUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnMasterItemsUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnMasterItemsUpdate.Name = "btnMasterItemsUpdate";
		this.btnMasterItemsUpdate.Size = new System.Drawing.Size(255, 24);
		this.btnMasterItemsUpdate.Text = "Master Items + Eb Weight Update";
		this.btnMasterItemsUpdate.Click += new System.EventHandler(btnMasterItemsUpdate_Click);
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(388, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1408, 27);
		this.miniToolStrip.TabIndex = 1;
		this.CmbRackName.AllowNull = (DefaultableBoolean)1;
		this.CmbRackName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRackName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRackName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbRackName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRackName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance52;
		((SpecialBoxBase)((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbRackName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRackName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance58).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRackName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRackName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRackName).Location = new System.Drawing.Point(109, 309);
		((System.Windows.Forms.Control)(object)this.CmbRackName).Name = "CmbRackName";
		((System.Windows.Forms.Control)(object)this.CmbRackName).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbRackName).TabIndex = 12;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.Red;
		this.label31.Location = new System.Drawing.Point(92, 314);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(14, 17);
		this.label31.TabIndex = 617;
		this.label31.Text = "*";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(4, 315);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(67, 15);
		this.label34.TabIndex = 616;
		this.label34.Text = "Rack Name";
		this.btnDefineRack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnDefineRack.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDefineRack.Image = (System.Drawing.Image)resources.GetObject("btnDefineRack.Image");
		this.btnDefineRack.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefineRack.Name = "btnDefineRack";
		this.btnDefineRack.Size = new System.Drawing.Size(167, 24);
		this.btnDefineRack.Text = "Rack Allocation To Item";
		this.btnDefineRack.Click += new System.EventHandler(btnDefineRack_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1409, 706);
		base.Controls.Add(this.tabCAddItem);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "AddItemPM";
		this.Text = "Add Item ";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(AddItemPM_FormClosing);
		base.Load += new System.EventHandler(InvDefrmAddItem_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvDefrmAddItem_KeyDown);
		this.tabCAddItem.ResumeLayout(false);
		this.tpItemDefinitionAddItem.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAllocation).EndInit();
		this.BrandImages.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picbox2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picbox1).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackSizeId).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTaxType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPurchaseGL).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSaleGL).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCGSGL).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBaseUnit).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).EndInit();
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
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterItemHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemTypeHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitemcathistory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRackName).EndInit();
		base.ResumeLayout(false);
	}
}
