using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Inventory_Definition;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Taxation;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Tax_Definition;

public class DefineTaxItem : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	public UserAccount UserAccount = new UserAccount();

	public bool UpdateMode;

	private Country country = new Country();

	private bool flag;

	private int RecId;

	private string fileSavePath = "";

	private string fileSavePath2 = "";

	private string fileSavePath3 = "";

	private string fileSavePath4 = "";

	private DataTable dtCatergory = new DataTable();

	private bool IfConfigAllAccountAllowOn;

	private IContainer components;

	private Label label13;

	private TabControl tabCAddItem;

	private TabPage tpItemDefinitionAddItem;

	private Button btnitmuomtaxsch;

	private Button btnitmuom;

	private Button btnitmtype;

	private Button btnitmtaxsch;

	private Button btnitmcat;

	private Panel panel2;

	private Panel panel6;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnprint;

	private ToolStripButton btnattachment;

	private Panel panel1;

	private Label label12;

	private Button BtnShowDetails;

	private CheckBox chbOther;

	private CheckBox chbExcise;

	private CheckBox chbVST;

	private CheckBox chbGST;

	private TextBox txtSaleRate;

	private Label label29;

	private Label label28;

	private Label label27;

	private TextBox txtPurchaseRate;

	private Label label23;

	private Label label22;

	private Label label21;

	private Label label20;

	private Label label19;

	private Label label18;

	private TextBox txtCostPrice;

	private TextBox txtReorderLevel;

	private Label label17;

	private TextBox txtMaxStockLevel;

	private TextBox txtProductNumber;

	private Label label16;

	private TextBox txtBuyerPartNo;

	private Label label15;

	private TextBox txtManufacturerPartNo;

	private Label label24;

	private Label label25;

	private Label label26;

	private TabPage tpItemDefinitionAddItemHistory;

	private Panel panel3;

	private Panel panel9;

	private Button btnsearch;

	private Label label32;

	private Panel panel10;

	private Label label31;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel pnlimage;

	private Button btnimg1;

	private Button btnimg2;

	private PictureBox picbox2;

	private PictureBox picbox1;

	internal UltraCombo cmbCGSGL;

	internal UltraCombo cmbBuyerOrSupplier;

	private ToolStripButton toolStripButton1;

	private Button btnCropYear;

	private Button btnItemGroup;

	private Button btnItemGroupSchedule;

	private CheckBox chkstatus;

	private Label label33;

	internal UltraCombo CmbItemGroup;

	internal UltraCombo cmbSaleGL;

	internal UltraCombo cmbPurchaseGL;

	private Label label9;

	private TextBox txtPackQuantity;

	private TextBox txtHSCode;

	private TextBox txtItemCode;

	private TextBox txtItemName;

	private Label label11;

	private Label label10;

	private Label label8;

	private Label label7;

	private Label label6;

	private Label label5;

	private Label label4;

	private Label label3;

	private Label label2;

	private Label label1;

	internal UltraCombo cmbItemClass;

	internal UltraCombo cmbTaxType;

	internal UltraCombo cmbItemType;

	internal UltraCombo cmbBaseUnit;

	internal UltraCombo cmbItemCategory;

	internal UltraCombo cmbitemcathistory;

	internal UltraCombo cmbManufacturerID;

	internal UltraCombo cmbCostUOM;

	internal UltraCombo cmbPurchaseUOM;

	internal UltraCombo cmbAmountCalcType;

	internal UltraCombo cmbcountry;

	internal UltraCombo cmbSaleUOM;

	private Button btnUomUpdate;

	private Button btnUomGroupAssignToItem;

	private Button button1;

	private GroupBox groupBox1;

	internal GridEX grdAllocation;

	private Button btnItemAllocation;

	private Panel panelDetails;

	internal UltraCombo combcropyear;

	private Label lblCropYear;

	private GroupBox groupBox3;

	private GroupBox groupBox2;

	private Label label14;

	private GroupBox groupBox4;

	internal GridEX grdhistory;

	private Panel panel4;

	private Label label30;

	private GroupBox grpUpdateItemName;

	private GroupBox groupBox5;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private Label label34;

	private TextBox txtItemCodeNew;

	private CheckBox chkCommissionOnPurchase;

	private CheckBox chkCommissionOnSale;

	private Button btnItemMultiLanguage;

	private PictureBox picbox3;

	private PictureBox picbox4;

	private Label label35;

	private TextBox txtModelName;

	private Button btnImage3;

	private Button btnImage4;

	private Label label36;

	private DateTimePicker EffectedDate;

	private Button btnItemAllocateToManufacturer;

	private Button btnPic1Reset;

	private Button btnPic4Reset;

	private Button btnPic3Reset;

	private Button btnPic2Reset;

	private TextBox txtOtherLingo;

	private Label label37;

	private RadioButton rdWithoutVariant;

	private RadioButton rdWithVariant;

	public DefineTaxItem(UserAccount pUserAccount)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void InvDefrmAddItem_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1150, 750);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnprint.Enabled = formrights.DoHavePrintRights;
			btnupdate.Enabled = formrights.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			IfConfigAllAccountAllowOn = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("All Account Allow on Item and Item Category"));
			ItemTypeFill();
			ItemClassFill();
			PurchaseGLAcFill();
			SupplierBuyerfill();
			cmbcountryfill();
			ItemCatagoryHistoryFill();
			ItemGroupNameFill();
			CmbTaxTypeFill();
			BaseUnitFill();
			CompaniesBindInGrid();
			AmountCaluculation();
			panelDetails.Hide();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			cmbItemCategory.Focus();
			CropYear();
			lblCropYear.Hide();
			((Control)(object)combcropyear).Hide();
			grpUpdateItemName.Visible = false;
			rdWithoutVariant.Checked = true;
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
					DDL.BindDDL(dtcath, cmbitemcathistory, "Id", "CategoryDescription", "Item Category", ZeroIndex: false);
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
		try
		{
			DataTable dt = CommonServices.GetItemTypeForComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbItemType, "Id", "TypeDescription", "Item Type", false);
				((UltraGridBase)cmbItemType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtType = new DataTable();
			dtType.Columns.Add("ClassId");
			dtType.Columns.Add("ClassDescription");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToInt(dt.Rows[i]["ClassId"]) == 1 || Conversion.ToInt(dt.Rows[i]["ClassId"]) == 6 || Conversion.ToInt(dt.Rows[i]["ClassId"]) == 7 || Conversion.ToInt(dt.Rows[i]["ClassId"]) == 8)
				{
					dtType.Rows.Add(dt.Rows[i]["ClassId"], dt.Rows[i]["ClassDescription"]);
				}
			}
			if (dtType.Rows.Count > 0)
			{
				DDL.BindDDL(dtType, cmbItemClass, "ClassId", "ClassDescription", "Item Class", ZeroIndex: false);
				((UltraGridBase)cmbItemClass).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchaseGLAcFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			List<COAAllocation> Accountslst = COAAllocation.GetLst(new COAAllocation
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (Accountslst.Count <= 0)
			{
				return;
			}
			if (!IfConfigAllAccountAllowOn)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("AccountTitle");
				DataTable dtsale = new DataTable();
				dtsale.Columns.Add("Id");
				dtsale.Columns.Add("AccountTitle");
				DataTable dtcgs = new DataTable();
				dtcgs.Columns.Add("Id");
				dtcgs.Columns.Add("AccountTitle");
				for (int i = 0; i < Accountslst.Count; i++)
				{
					if (Accountslst[i].AccountTypeId == 4)
					{
						dt.Rows.Add(Accountslst[i].Id, Accountslst[i].AccountTitle);
					}
					if (Accountslst[i].AccountTypeId == 10)
					{
						dtsale.Rows.Add(Accountslst[i].Id, Accountslst[i].AccountTitle);
					}
					if (Accountslst[i].AccountTypeId == 12)
					{
						dtcgs.Rows.Add(Accountslst[i].Id, Accountslst[i].AccountTitle);
					}
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbPurchaseGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: false);
				}
				if (dtsale.Rows.Count > 0)
				{
					DDL.BindDDL(dtsale, cmbSaleGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: false);
				}
				if (dtcgs.Rows.Count > 0)
				{
					DDL.BindDDL(dtcgs, cmbCGSGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: false);
				}
				return;
			}
			DataTable dt2 = new DataTable();
			dt2.Columns.Add("Id");
			dt2.Columns.Add("AccountTitle");
			for (int j = 0; j < Accountslst.Count; j++)
			{
				if (Accountslst[j].AccountTypeId == 4 || Accountslst[j].AccountTypeId == 10 || Accountslst[j].AccountTypeId == 12)
				{
					dt2.Rows.Add(Accountslst[j].Id, Accountslst[j].AccountTitle);
				}
			}
			DDL.BindDDL(dt2, cmbPurchaseGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: false);
			DDL.BindDDL(dt2, cmbSaleGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: false);
			DDL.BindDDL(dt2, cmbCGSGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: false);
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
					((UltraGridBase)CmbItemGroup).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
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
				if (Convert.ToInt32(dt.Rows[i]["Type"]) != 1)
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

	private void SupplierBuyerfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			List<SupplierCustomer> lst = SupplierCustomer.GetAllList(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CompanyName");
			for (int i = 0; i < lst.Count; i++)
			{
				dt.Rows.Add(lst[i].Id, lst[i].CompanyName);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbBuyerOrSupplier, "Id", "CompanyName", "Supplier Name", ZeroIndex: true);
				DDL.BindDDL(dt, cmbManufacturerID, "Id", "CompanyName", "Supplier Name", ZeroIndex: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CostUOMFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = UOMSchedule.Getall(new UOMSchedule
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
			dtType.Columns.Add("Equivalent");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Convert.ToInt32(dt.Rows[i]["ItemId"]) == RecId)
				{
					dtType.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["Equivalent"]);
				}
			}
			if (dtType.Rows.Count > 0)
			{
				DDL.BindDDL(dtType, cmbCostUOM, "Id", "Equivalent", "TaxName", ZeroIndex: false);
				((UltraGridBase)cmbCostUOM).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtType, cmbPurchaseUOM, "Id", "Equivalent", "TaxName", ZeroIndex: false);
				((UltraGridBase)cmbPurchaseUOM).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtType, cmbSaleUOM, "Id", "Equivalent", "TaxName", ZeroIndex: false);
				((UltraGridBase)cmbSaleUOM).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbcountryfill()
	{
		try
		{
			country.OrganizationId = UserAccount.OrganizationId;
			country.CompanyId = UserAccount.CompanyId;
			DataTable dt = country.GetAll(country);
			if (dt.Rows.Count > 0)
			{
				DataTable dtType = new DataTable();
				dtType.Columns.Add("Id");
				dtType.Columns.Add("Description");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtType.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["Description"]);
				}
				if (dtType.Rows.Count > 0)
				{
					DDL.BindDDL(dtType, cmbcountry, "Id", "Description", "Country Name", ZeroIndex: false);
					((UltraGridBase)cmbcountry).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AmountCaluculation()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Code");
			dt.Rows.Add(1, "Multiply");
			dt.Rows.Add(1, "Devide");
			DDL.BindDDL(dt, cmbAmountCalcType, "Id", "Code", "Type", ZeroIndex: false);
			((UltraGridBase)cmbAmountCalcType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYear()
	{
		try
		{
			DataTable lst = CommonServices.CropYearGetAllService();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDLNew(lst, combcropyear, "Id", "CropYear", "Crop Year", false);
				((UltraGridBase)combcropyear).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
					grdAllocation.AllowEdit = (InheritableBoolean)2;
					grdAllocation.ColumnAutoResize = false;
					grdAllocation.RootTable.Columns["Id"].Visible = false;
					grdAllocation.RootTable.Columns["Location"].Width = 250;
					grdAllocation.RootTable.Columns["Value"].EditType = (EditType)8;
					grdAllocation.RootTable.Columns["Value"].FilterEditType = (FilterEditType)7;
					grdAllocation.RootTable.Columns["Value"].ColumnType = (ColumnType)3;
					grdAllocation.RootTable.Columns["Value"].CheckBoxTrueValue = "True";
					grdAllocation.RootTable.Columns["Value"].CheckBoxFalseValue = "False";
					grdAllocation.RootTable.Columns["Value"].UseHeaderSelector = true;
					grdAllocation.RootTable.Columns["Value"].Position = 0;
					grdAllocation.RootTable.Columns["Value"].Width = 40;
					grdAllocation.FrozenColumns = 1;
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
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_00c3: Expected O, but got Unknown
		try
		{
			if (btnsave.Visible && btnsave.Enabled)
			{
				Item val = new Item
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					ItemCategoryId = Convert.ToInt32(cmbItemCategory.Value)
				};
				DataTable lst = Item.GenerateCode(val);
				if (lst.Rows.Count > 0)
				{
					txtItemCode.Text = lst.Rows[0]["ItemCode"].ToString();
					txtItemCodeNew.Text = lst.Rows[0]["ItemCodeNew"].ToString();
				}
				DataTable dt = Item.GetGLAccountbyItemCategoryId(val);
				if (dt.Rows.Count > 0)
				{
					cmbPurchaseGL.Value = dt.Rows[0]["InventoryAccountId"].ToString();
					cmbSaleGL.Value = dt.Rows[0]["RevenueAccountId"].ToString();
					cmbCGSGL.Value = dt.Rows[0]["CGSAccountId"].ToString();
				}
			}
			txtPackQuantity.Text = "0";
			((UltraGridBase)cmbItemClass).Rows[0].Activate();
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private void btnsearch_Click(object sender, EventArgs e)
	{
		grdfrmfill();
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
		if (((UltraGridBase)cmbItemClass).ActiveRow == null)
		{
			MessageBox.Show("Item Class Field is Required");
			cmbItemClass.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemClass).ActiveRow != null && ((Control)(object)cmbItemClass).Text == "Brand" && ((UltraGridBase)combcropyear).ActiveRow == null)
		{
			MessageBox.Show("Crop Year Field is Required");
			combcropyear.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPurchaseGL).ActiveRow == null)
		{
			MessageBox.Show("Purchase GL A/C Field is Required");
			cmbPurchaseGL.Focus();
			return false;
		}
		if (((UltraGridBase)cmbSaleGL).ActiveRow == null)
		{
			MessageBox.Show("Sale GL A/C Field is Required");
			cmbSaleGL.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCGSGL).ActiveRow == null)
		{
			MessageBox.Show("COGS GL A/c Filed is Required");
			cmbCGSGL.Focus();
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
			cmbItemCategory.ReadOnly = false;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			((Control)(object)cmbAmountCalcType).Text = string.Empty;
			((Control)(object)cmbBuyerOrSupplier).Text = string.Empty;
			((Control)(object)cmbCGSGL).Text = string.Empty;
			((Control)(object)cmbCostUOM).Text = string.Empty;
			((Control)(object)cmbcountry).Text = string.Empty;
			((Control)(object)cmbItemClass).Text = string.Empty;
			((Control)(object)cmbManufacturerID).Text = string.Empty;
			((Control)(object)cmbPurchaseGL).Text = string.Empty;
			((Control)(object)cmbPurchaseUOM).Text = string.Empty;
			((Control)(object)cmbSaleGL).Text = string.Empty;
			((Control)(object)cmbSaleUOM).Text = string.Empty;
			txtItemCode.Clear();
			txtBuyerPartNo.Clear();
			txtCostPrice.Clear();
			txtHSCode.Clear();
			txtItemName.Clear();
			txtManufacturerPartNo.Clear();
			txtMaxStockLevel.Clear();
			txtPackQuantity.Clear();
			txtProductNumber.Clear();
			txtModelName.Clear();
			txtPurchaseRate.Clear();
			txtReorderLevel.Clear();
			txtSaleRate.Clear();
			chbExcise.Checked = false;
			chbGST.Checked = false;
			chbVST.Checked = false;
			chbOther.Checked = false;
			cmbItemCategory.Focus();
			CompaniesBindInGrid();
			cmbItemCategory_Leave(null, null);
			panelDetails.Hide();
			BtnShowDetails.Text = "+";
			fileSavePath = "";
			fileSavePath2 = "";
			fileSavePath3 = "";
			fileSavePath4 = "";
			picbox1.Image = null;
			picbox2.Image = null;
			picbox3.Image = null;
			picbox4.Image = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowDetails_Click(object sender, EventArgs e)
	{
		if (BtnShowDetails.Text == "+")
		{
			panelDetails.Show();
			BtnShowDetails.Text = "-";
		}
		else
		{
			panelDetails.Hide();
			BtnShowDetails.Text = "+";
		}
	}

	private void BtnSave_Click(object sender, EventArgs e)
	{
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Expected O, but got Unknown
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Expected O, but got Unknown
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Expected O, but got Unknown
		if (!FormValidationForAddItem())
		{
			return;
		}
		try
		{
			List<string> ImgList = new List<string>();
			if (this.fileSavePath != null && this.fileSavePath != string.Empty)
			{
				ImgList.Add(this.fileSavePath);
			}
			if (this.fileSavePath2 != null && this.fileSavePath2 != string.Empty)
			{
				ImgList.Add(this.fileSavePath2);
			}
			if (fileSavePath3 != null && fileSavePath3 != string.Empty)
			{
				ImgList.Add(fileSavePath3);
			}
			if (fileSavePath4 != null && fileSavePath4 != string.Empty)
			{
				ImgList.Add(fileSavePath4);
			}
			Item items = new Item();
			items.ItemCode = Conversion.ToString((object)txtItemCode.Text.Trim());
			items.ItemCodeNew = Conversion.ToString((object)txtItemCodeNew.Text.Trim());
			items.ItemName = Conversion.ToString((object)txtItemName.Text.Trim());
			items.ItemNameOtherLingo = Conversion.ToString((object)txtOtherLingo.Text.Trim());
			items.BaseUnitId = Conversion.ToInt(cmbBaseUnit.Value);
			items.Equivalent = Conversion.ToDouble(((UltraDropDownBase)cmbBaseUnit).SelectedRow.Cells[2].Value);
			items.ItemCategoryId = Conversion.ToInt(cmbItemCategory.Value);
			items.ItemTypeId = Conversion.ToInt(cmbItemType.Value);
			items.ItemClassId = Conversion.ToInt(cmbItemClass.Value);
			items.HSCode = Conversion.ToString((object)txtHSCode.Text.Trim());
			items.PackQty = Conversion.ToInt((object)txtPackQuantity.Text.Trim());
			items.ItemStatus = Conversion.ToBool((object)chkstatus.Checked);
			items.IsTaxable = true;
			items.Pic1 = Conversion.ToString((object)this.fileSavePath);
			items.Pic2 = Conversion.ToString((object)this.fileSavePath2);
			items.PurchaseGLAC = Conversion.ToInt(cmbPurchaseGL.Value);
			items.SaleGLAC = Conversion.ToInt(cmbSaleGL.Value);
			items.COGSGLAC = Conversion.ToInt(cmbCGSGL.Value);
			items.ManufactureId = Conversion.ToInt(cmbManufacturerID.Value);
			items.ManufacturePartNo = Conversion.ToString((object)txtManufacturerPartNo.Text.Trim());
			items.BuyerSupplierId = Conversion.ToInt(cmbBuyerOrSupplier.Value);
			items.BuyerPartNo = Conversion.ToString((object)txtBuyerPartNo.Text.Trim());
			items.ProductNo = Conversion.ToString((object)txtProductNumber.Text.Trim());
			items.ModelName = Conversion.ToString((object)txtModelName.Text.Trim());
			items.ItemGroupId = Conversion.ToInt(CmbItemGroup.Value);
			items.WholeSalePrice = Conversion.ToDouble((object)txtPurchaseRate.Text);
			items.BaseCropYear = Conversion.ToString((object)((Control)(object)combcropyear).Text);
			items.OrganizationId = UserAccount.OrganizationId;
			items.CompanyId = UserAccount.CompanyId;
			items.EntryUser = UserAccount.ID;
			items.BranchesId = UserAccount.BranchesId;
			if (chbGST.Checked)
			{
				items.ApplyGST = true;
				items.TaxTypeId = Conversion.ToInt(cmbTaxType.Value);
			}
			if (rdWithVariant.Checked)
			{
				items.ItemWithVarient = true;
			}
			else if (rdWithoutVariant.Checked)
			{
				items.ItemWithVarient = false;
			}
			items.CommOnSale = chkCommissionOnSale.Checked;
			items.CommOnPurchase = chkCommissionOnPurchase.Checked;
			items.AttachmentsList = new List<DMSAttachments>();
			items.ItemAllocationlist = new List<ItemAllocation>();
			items.ItemImageslist = new List<ItemImage>();
			for (int j = 0; j < ImgList.Count; j++)
			{
				string UniquePath = $"{Guid.NewGuid()}.Jpg";
				ItemImage img = new ItemImage();
				img.ImagePath = UniquePath;
				img.FileName = Path.GetFileName(ImgList[j]);
				img.EffetedDate = EffectedDate.Value;
				items.ItemImageslist.Add(img);
				if (!Directory.Exists(clsGlobalVariables.AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
				string fileSavePath2 = Path.Combine(clsGlobalVariables.AttachmentFolderPath, img.ImagePath);
				if (!File.Exists(fileSavePath2))
				{
					File.Copy(Attachment.dtAttachments.Rows[j][1].ToString(), fileSavePath2, overwrite: true);
				}
			}
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Path.GetFileName(Attachment.dtAttachments.Rows[i][1].ToString());
				attach.ScreenName = base.Name;
				items.AttachmentsList.Add(attach);
				if (!Directory.Exists(clsGlobalVariables.AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
				string fileSavePath = Path.Combine(clsGlobalVariables.AttachmentFolderPath, attach.Attachment);
				if (!File.Exists(fileSavePath))
				{
					File.Copy(Attachment.dtAttachments.Rows[i][1].ToString(), fileSavePath, overwrite: true);
				}
			}
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
			if (Item.Save(items, true) >= 1)
			{
				MessageBox.Show("Save Successfully");
			}
			grdfrmfill();
			refresh();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Expected O, but got Unknown
		//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Expected O, but got Unknown
		if (!FormValidationForAddItem())
		{
			return;
		}
		try
		{
			List<string> ImgList = new List<string>();
			if (this.fileSavePath != null && this.fileSavePath != string.Empty)
			{
				ImgList.Add(this.fileSavePath);
			}
			if (this.fileSavePath2 != null && this.fileSavePath2 != string.Empty)
			{
				ImgList.Add(this.fileSavePath2);
			}
			if (fileSavePath3 != null && fileSavePath3 != string.Empty)
			{
				ImgList.Add(fileSavePath3);
			}
			if (fileSavePath4 != null && fileSavePath4 != string.Empty)
			{
				ImgList.Add(fileSavePath4);
			}
			Item items = new Item();
			items.Id = RecId;
			items.ItemCode = Conversion.ToString((object)txtItemCode.Text.Trim());
			items.ItemCodeNew = Conversion.ToString((object)txtItemCodeNew.Text.Trim());
			items.ItemName = Conversion.ToString((object)txtItemName.Text.Trim());
			items.ItemNameOtherLingo = Conversion.ToString((object)txtOtherLingo.Text.Trim());
			items.BaseUnitId = Conversion.ToInt(cmbBaseUnit.Value);
			items.Equivalent = Conversion.ToDouble(((UltraDropDownBase)cmbBaseUnit).SelectedRow.Cells[2].Value);
			items.ItemCategoryId = Conversion.ToInt(cmbItemCategory.Value);
			items.ItemTypeId = Conversion.ToInt(cmbItemType.Value);
			items.ItemClassId = Conversion.ToInt(cmbItemClass.Value);
			items.HSCode = Conversion.ToString((object)txtHSCode.Text.Trim());
			items.PackQty = Conversion.ToInt((object)txtPackQuantity.Text.Trim());
			items.ItemStatus = Conversion.ToBool((object)chkstatus.Checked);
			items.IsTaxable = true;
			items.Pic1 = Conversion.ToString((object)this.fileSavePath);
			items.Pic2 = Conversion.ToString((object)this.fileSavePath2);
			items.PurchaseGLAC = Conversion.ToInt(cmbPurchaseGL.Value);
			items.SaleGLAC = Conversion.ToInt(cmbSaleGL.Value);
			items.COGSGLAC = Conversion.ToInt(cmbCGSGL.Value);
			items.ManufactureId = Conversion.ToInt(cmbManufacturerID.Value);
			items.ManufacturePartNo = Conversion.ToString((object)txtManufacturerPartNo.Text.Trim());
			items.BuyerSupplierId = Conversion.ToInt(cmbBuyerOrSupplier.Value);
			items.BuyerPartNo = Conversion.ToString((object)txtBuyerPartNo.Text.Trim());
			items.ProductNo = Conversion.ToString((object)txtProductNumber.Text.Trim());
			items.ModelName = Conversion.ToString((object)txtModelName.Text.Trim());
			items.MaxStockLevel = Conversion.ToInt((object)txtMaxStockLevel.Text.Trim());
			items.ReorderLevel = Conversion.ToInt((object)txtReorderLevel.Text.Trim());
			items.ItemOriginId = Conversion.ToInt(cmbcountry.Value);
			items.CostPrice = Conversion.ToInt((object)txtCostPrice.Text.Trim());
			items.UOMScheduleIdCostRate = Conversion.ToInt((object)((Control)(object)cmbCostUOM).Text.Trim());
			items.PurchasePrice = Conversion.ToInt((object)txtPurchaseRate.Text.Trim());
			items.UOMScheduleIdPurRate = Conversion.ToInt((object)((Control)(object)cmbPurchaseUOM).Text.Trim());
			items.WholeSalePrice = Conversion.ToInt((object)txtSaleRate.Text.Trim());
			items.UOMScheduleIdWhsRate = Conversion.ToInt((object)((Control)(object)cmbSaleUOM).Text.Trim());
			items.BaseCropYear = Conversion.ToString((object)((Control)(object)combcropyear).Text.Trim());
			items.AmountCalcType = Conversion.ToString((object)((Control)(object)cmbAmountCalcType).Text);
			items.ApplyGST = Conversion.ToBool((object)chbGST.Checked);
			items.ApplyVAT = Conversion.ToBool((object)chbVST.Checked);
			items.ApplyExciese = Conversion.ToBool((object)chbExcise.Checked);
			items.OtherTax = Conversion.ToBool((object)chbOther.Checked);
			items.OrganizationId = UserAccount.OrganizationId;
			items.CompanyId = UserAccount.CompanyId;
			items.BranchesId = UserAccount.BranchesId;
			items.ModifyUser = UserAccount.ID;
			if (rdWithVariant.Checked)
			{
				items.ItemWithVarient = true;
			}
			else if (rdWithoutVariant.Checked)
			{
				items.ItemWithVarient = false;
			}
			items.AttachmentsList = new List<DMSAttachments>();
			items.ItemImageslist = new List<ItemImage>();
			for (int j = 0; j < ImgList.Count; j++)
			{
				string UniquePath = $"{Guid.NewGuid()}.Jpg";
				ItemImage img = new ItemImage();
				img.ImagePath = UniquePath;
				img.FileName = Path.GetFileName(ImgList[j]);
				img.EffetedDate = EffectedDate.Value;
				items.ItemImageslist.Add(img);
				if (!Directory.Exists(clsGlobalVariables.AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
				string fileSavePath2 = Path.Combine(clsGlobalVariables.AttachmentFolderPath, img.ImagePath);
				if (!File.Exists(fileSavePath2))
				{
					string FullPath = ImgList[j];
					if (img.FileName == FullPath)
					{
						ImgList[j] = clsGlobalVariables.AttachmentFolderPath + "\\" + ImgList[j];
					}
					File.Copy(ImgList[j], fileSavePath2, overwrite: true);
				}
			}
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Path.GetFileName(Attachment.dtAttachments.Rows[i][1].ToString());
				attach.ScreenName = base.Name;
				items.AttachmentsList.Add(attach);
				if (!Directory.Exists(clsGlobalVariables.AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
				string fileSavePath = Path.Combine(clsGlobalVariables.AttachmentFolderPath, attach.Attachment);
				if (!File.Exists(fileSavePath))
				{
					File.Copy(Attachment.dtAttachments.Rows[i][1].ToString(), fileSavePath, overwrite: true);
				}
			}
			if (Item.Save(items, true) >= 1)
			{
				MessageBox.Show("Update Successfully");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
				{
					for (int k = 0; k < AT.RemovedAttachmentListInUpdateCase.Count; k++)
					{
						using (FileStream fs = new FileStream(AT.RemovedAttachmentListInUpdateCase[k].Path, FileMode.Open, FileAccess.ReadWrite))
						{
							fs.Flush();
							fs.Close();
							fs.Dispose();
						}
						bool fileDeleted = false;
						int attempts = 0;
						while (!fileDeleted && attempts < 5)
						{
							try
							{
								File.Delete(AT.RemovedAttachmentListInUpdateCase[k].Path);
								fileDeleted = true;
							}
							catch (IOException)
							{
								attempts++;
								Thread.Sleep(500);
							}
						}
						if (!fileDeleted)
						{
							throw new Exception("Unable to delete file. It is in use by another process.");
						}
					}
				}
			}
			grdfrmfill();
			refresh();
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			GridEXRow row = grdhistory.CurrentRow;
			if (row == null || (int)row.RowType != 0)
			{
				return;
			}
			panelDetails.Visible = true;
			btnsave.Visible = false;
			btnupdate.Visible = true;
			((UltraGridBase)CmbItemGroup).Rows[0].Activate();
			Item items = new Item();
			RecId = Conversion.ToInt((object)row.Cells["Id"].Value.ToString());
			items = Item.GetByID(RecId);
			items.Id = RecId;
			tabCAddItem.SelectedIndex = 0;
			txtItemCode.Text = Conversion.ToString((object)items.ItemCode);
			txtItemCodeNew.Text = Conversion.ToString((object)items.ItemCodeNew);
			txtItemName.Text = Conversion.ToString((object)items.ItemName);
			txtOtherLingo.Text = Conversion.ToString((object)items.ItemNameOtherLingo);
			cmbBaseUnit.Value = Conversion.ToInt((object)items.BaseUnitId);
			cmbItemCategory.Value = items.ItemCategoryId;
			cmbItemCategory.ReadOnly = true;
			cmbItemType.Value = items.ItemTypeId;
			cmbItemClass.Value = items.ItemClassId;
			txtHSCode.Text = Conversion.ToString((object)items.HSCode);
			txtPackQuantity.Text = Conversion.ToString((object)items.PackQty);
			chkstatus.Checked = Conversion.ToBool((object)items.ItemStatus);
			cmbPurchaseGL.Value = items.PurchaseGLAC;
			cmbSaleGL.Value = items.SaleGLAC;
			cmbCGSGL.Value = items.COGSGLAC;
			cmbManufacturerID.Value = items.ManufactureId;
			txtManufacturerPartNo.Text = Conversion.ToString((object)items.ManufacturePartNo);
			cmbBuyerOrSupplier.Value = items.BuyerSupplierId;
			txtBuyerPartNo.Text = Conversion.ToString((object)items.BuyerPartNo);
			txtProductNumber.Text = Conversion.ToString((object)items.ProductNo);
			txtModelName.Text = Conversion.ToString((object)items.ModelName);
			txtMaxStockLevel.Text = Conversion.ToString((object)items.MaxStockLevel);
			txtReorderLevel.Text = Conversion.ToString((object)items.ReorderLevel);
			cmbcountry.Value = items.ItemOriginId;
			txtCostPrice.Text = Conversion.ToString((object)items.CostPrice);
			((Control)(object)cmbCostUOM).Text = Conversion.ToString((object)items.UOMScheduleIdCostRate);
			txtPurchaseRate.Text = Conversion.ToString((object)items.PurchasePrice);
			((Control)(object)cmbPurchaseUOM).Text = Conversion.ToString((object)items.UOMScheduleIdPurRate);
			txtSaleRate.Text = Conversion.ToString((object)items.WholeSalePrice);
			((Control)(object)cmbSaleUOM).Text = Conversion.ToString((object)items.UOMScheduleIdWhsRate);
			((Control)(object)cmbAmountCalcType).Text = Conversion.ToString((object)items.AmountCalcType);
			chbGST.Checked = Conversion.ToBool((object)items.ApplyGST);
			chbVST.Checked = Conversion.ToBool((object)items.ApplyVAT);
			chbExcise.Checked = Conversion.ToBool((object)items.ApplyExciese);
			chbOther.Checked = Conversion.ToBool((object)items.OtherTax);
			if (items.ItemWithVarient)
			{
				rdWithVariant.Checked = true;
			}
			else if (!items.ItemWithVarient)
			{
				rdWithoutVariant.Checked = true;
			}
			combcropyear.Value = items.BaseCropYear;
			fileSavePath = items.Pic1;
			chkCommissionOnPurchase.Checked = false;
			chkCommissionOnPurchase.Visible = false;
			chkCommissionOnSale.Checked = false;
			chkCommissionOnSale.Visible = false;
			List<DMSAttachments> byID = DMSAttachments.GetByID(RecId, Conversion.ToString(base.Tag));
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
			if (items.ItemImageslist == null || items.ItemImageslist.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < items.ItemImageslist.Count; i++)
			{
				switch (i)
				{
				case 0:
				{
					fileSavePath = items.ItemImageslist[i].ImagePath;
					string path = Path.Combine(clsGlobalVariables.AttachmentFolderPath, fileSavePath);
					if (File.Exists(path))
					{
						using MemoryStream stream = new MemoryStream();
						Image image = Image.FromFile(path);
						Bitmap bmp = new Bitmap(image);
						bmp.Save(stream, image.RawFormat);
						picbox1.Image = bmp;
						image.Dispose();
						stream.Position = 0L;
						stream.Dispose();
					}
					break;
				}
				case 1:
				{
					fileSavePath2 = items.ItemImageslist[i].ImagePath;
					string path2 = Path.Combine(clsGlobalVariables.AttachmentFolderPath, fileSavePath2);
					if (File.Exists(path2))
					{
						using MemoryStream memoryStream2 = new MemoryStream();
						Image image2 = Image.FromFile(path2);
						Bitmap bmp2 = new Bitmap(image2);
						bmp2.Save(memoryStream2, image2.RawFormat);
						picbox2.Image = bmp2;
						image2.Dispose();
						memoryStream2.Position = 0L;
						memoryStream2.Dispose();
					}
					break;
				}
				case 2:
				{
					fileSavePath3 = items.ItemImageslist[i].ImagePath;
					string path3 = Path.Combine(clsGlobalVariables.AttachmentFolderPath, fileSavePath3);
					if (File.Exists(path3))
					{
						using MemoryStream memoryStream3 = new MemoryStream();
						Image image3 = Image.FromFile(path3);
						Bitmap bmp3 = new Bitmap(image3);
						bmp3.Save(memoryStream3, image3.RawFormat);
						picbox3.Image = bmp3;
						image3.Dispose();
						memoryStream3.Position = 0L;
						memoryStream3.Dispose();
					}
					break;
				}
				case 3:
				{
					fileSavePath4 = items.ItemImageslist[i].ImagePath;
					string path4 = Path.Combine(clsGlobalVariables.AttachmentFolderPath, fileSavePath4);
					if (File.Exists(path4))
					{
						using MemoryStream memoryStream = new MemoryStream();
						Image image4 = Image.FromFile(path4);
						Bitmap bmp4 = new Bitmap(image4);
						bmp4.Save(memoryStream, image4.RawFormat);
						picbox4.Image = bmp4;
						image4.Dispose();
						memoryStream.Position = 0L;
						memoryStream.Dispose();
					}
					break;
				}
				}
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
		((Control)(object)cmbBaseUnit).Text = string.Empty;
		((Control)(object)cmbItemType).Text = string.Empty;
		chkCommissionOnPurchase.Checked = true;
		chkCommissionOnPurchase.Visible = true;
		chkCommissionOnSale.Checked = true;
		chkCommissionOnSale.Visible = true;
	}

	private void grdfrmfill(int NoOfRecords = 0)
	{
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("ItemCode", typeof(string));
			dt.Columns.Add("ItemCodeNew", typeof(string));
			dt.Columns.Add("ItemName", typeof(string));
			dt.Columns.Add("ItemNameOtherLingo", typeof(string));
			dt.Columns.Add("TypeDescription", typeof(string));
			dt.Columns.Add("CategoryDescription", typeof(string));
			dt.Columns.Add("ParentCategory", typeof(string));
			dt.Columns.Add("ClassGroupName", typeof(string));
			dt.Columns.Add("ItemStatus", typeof(string));
			dt.Columns.Add("StockAc", typeof(string));
			dt.Columns.Add("SaleAc", typeof(string));
			dt.Columns.Add("CgsAc", typeof(string));
			dt.Columns.Add("ItemWithVarient", typeof(string));
			dt.Columns.Add("EntryDate", typeof(DateTime));
			dt.Columns.Add("EntryUserName", typeof(string));
			dt.Columns.Add("NoOfAttachments", typeof(int));
			ReportsParameters reports = new ReportsParameters();
			reports.OrganizationId = UserAccount.OrganizationId;
			reports.CompanyId = UserAccount.CompanyId;
			reports.NoOfRecords = NoOfRecords;
			if (Conversion.ToInt(cmbitemcathistory.Value) != 0)
			{
				reports.Id = Convert.ToInt32(cmbitemcathistory.Value);
			}
			reports.ScreenName = base.Name;
			reports.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!reports.CanViewAllRecord)
			{
				reports.EntryUser = UserAccount.ID;
			}
			reports.IsTaxable = true;
			DataTable grdlst = Item.FormHistory(reports);
			if (grdlst.Rows.Count > 0)
			{
				for (int i = 0; i < grdlst.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(grdlst.Rows[i]["Id"]), Conversion.ToString(grdlst.Rows[i]["ItemCode"]), Conversion.ToString(grdlst.Rows[i]["ItemCodeNew"]), Conversion.ToString(grdlst.Rows[i]["ItemName"]), Conversion.ToString(grdlst.Rows[i]["ItemNameOtherLingo"]), Conversion.ToString(grdlst.Rows[i]["TypeDescription"]), Conversion.ToString(grdlst.Rows[i]["CategoryDescription"]), Conversion.ToString(grdlst.Rows[i]["InvParentCateDescription"]), Conversion.ToString(grdlst.Rows[i]["ClassGroupName"]), Conversion.ToString(grdlst.Rows[i]["ItemStatus"]), Conversion.ToString(grdlst.Rows[i]["GlStockAccountTitle"]), Conversion.ToString(grdlst.Rows[i]["GlSaleAccountTitle"]), Conversion.ToString(grdlst.Rows[i]["GlCgsAccountTitle"]), Conversion.ToString(grdlst.Rows[i]["ItemWithVarient"]), Conversion.ToDateTime(grdlst.Rows[i]["EntryDate"]).ToShortDateString(), Conversion.ToString(grdlst.Rows[i]["EntryUserName"]), Conversion.ToInt(grdlst.Rows[i]["NoOfAttachments"]));
				}
				grdhistory.DataSource = dt;
				grdhistory.RetrieveStructure();
				gridsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OnItemUpdateInHistory()
	{
		try
		{
			if (grpUpdateItemName.Visible && grpUpdateItemName.Enabled)
			{
				grdhistory.RootTable.Columns["Id"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["ItemCode"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["ItemCodeNew"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["ItemName"].EditType = (EditType)1;
				grdhistory.RootTable.Columns["ItemNameOtherLingo"].EditType = (EditType)1;
				grdhistory.RootTable.Columns["TypeDescription"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["CategoryDescription"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["ParentCategory"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["ClassGroupName"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["ItemStatus"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["StockAc"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["SaleAc"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["CgsAc"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["EntryDate"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["EntryUserName"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["ItemWithVarient"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["NoOfAttachments"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["Select"].Visible = true;
				grdhistory.FrozenColumns = 1;
			}
			else
			{
				grdhistory.RootTable.Columns["Id"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["ItemCode"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["ItemCodeNew"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["ItemName"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["ItemNameOtherLingo"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["TypeDescription"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["CategoryDescription"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["ParentCategory"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["ClassGroupName"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["ItemStatus"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["StockAc"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["SaleAc"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["CgsAc"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["EntryDate"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["EntryUserName"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["ItemWithVarient"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["NoOfAttachments"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["Select"].Visible = false;
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
			grdhistory.AllowEdit = (InheritableBoolean)1;
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["ItemCodeNew"].Caption = "Item Code ";
			grdhistory.RootTable.Columns["ItemCode"].Visible = true;
			grdhistory.RootTable.Columns.Add("Select");
			grdhistory.RootTable.Columns["Select"].ActAsSelector = true;
			grdhistory.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdhistory.RootTable.Columns["Select"].Position = 0;
			grdhistory.RootTable.Columns["Select"].Width = 30;
			grdhistory.RootTable.Columns["Select"].Visible = false;
			grdhistory.FrozenColumns = 1;
			if (grpUpdateItemName.Visible && grpUpdateItemName.Enabled)
			{
				grdhistory.RootTable.Columns["Select"].Visible = true;
			}
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustmentNew(grdhistory);
			ctrlGrdBar1_Load(null, null);
			grdhistory.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["ItemNameOtherLingo"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["ItemCode"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["ItemCodeNew"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["TypeDescription"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["CategoryDescription"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["ParentCategory"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["ClassGroupName"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["ItemStatus"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["StockAc"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["SaleAc"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["CgsAc"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["EntryDate"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["EntryUserName"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["ItemWithVarient"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["NoOfAttachments"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["Id"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["ItemCodeNew"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["ItemNameOtherLingo"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["TypeDescription"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["CategoryDescription"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["ParentCategory"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["ClassGroupName"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["ItemStatus"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["StockAc"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["SaleAc"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["CgsAc"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["EntryDate"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["EntryUserName"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["ItemWithVarient"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["NoOfAttachments"].EditType = (EditType)0;
			if (grpUpdateItemName.Visible && grpUpdateItemName.Enabled)
			{
				grdhistory.RootTable.Columns["ItemName"].EditType = (EditType)1;
				grdhistory.RootTable.Columns["ItemNameOtherLingo"].EditType = (EditType)1;
				grdhistory.RootTable.Columns["Select"].Visible = true;
				grdhistory.FrozenColumns = 1;
			}
			else
			{
				grdhistory.RootTable.Columns["ItemName"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["ItemNameOtherLingo"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["Select"].Visible = false;
			}
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
		if (e.Control && e.Shift && e.KeyCode == Keys.F10 && formrights.DoHaveUpdateRights)
		{
			if (grpUpdateItemName.Visible && grpUpdateItemName.Enabled)
			{
				grpUpdateItemName.Visible = false;
				grpUpdateItemName.Enabled = false;
				OnItemUpdateInHistory();
			}
			else
			{
				grpUpdateItemName.Visible = true;
				grpUpdateItemName.Enabled = true;
				OnItemUpdateInHistory();
			}
		}
		if (btnupdate.Visible && e.Control && e.KeyCode == Keys.U && (UpdateMode = true))
		{
			btnupdate_Click(null, null);
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

	private void btnImage3_Click(object sender, EventArgs e)
	{
		try
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "(*.jpg; *.jpeg; *.gif; *.PNG; *.png)| *.jpg; *.jpeg; *.gif; *.PNG;*.png; ";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				Path.GetFileName(openFileDialog.FileName);
				picbox3.Image = new Bitmap(openFileDialog.FileName);
				fileSavePath3 = openFileDialog.FileName;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnImage4_Click(object sender, EventArgs e)
	{
		try
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "(*.jpg; *.jpeg; *.gif; *.PNG; *.png)| *.jpg; *.jpeg; *.gif; *.PNG;*.png; ";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				Path.GetFileName(openFileDialog.FileName);
				picbox4.Image = new Bitmap(openFileDialog.FileName);
				fileSavePath4 = openFileDialog.FileName;
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void btnitmcat_Click(object sender, EventArgs e)
	{
		new InvDeffrmItemCatagory(UserAccount).Show();
	}

	private void btnitmtaxsch_Click(object sender, EventArgs e)
	{
		new InvDeffrmItemTaxSchdule(UserAccount).Show();
	}

	private void btnitmtype_Click(object sender, EventArgs e)
	{
		new InvDeffrmItemType(UserAccount).Show();
	}

	private void btnitmuom_Click(object sender, EventArgs e)
	{
		new InvDeffrmItemItemUom(UserAccount).Show();
	}

	private void btnitmuomtaxsch_Click(object sender, EventArgs e)
	{
		new InvDeffrmItemUomSchedule(UserAccount).Show();
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
			ItemTypeFill();
			ItemClassFill();
			PurchaseGLAcFill();
			SupplierBuyerfill();
			cmbcountryfill();
			ItemCatagoryHistoryFill();
			ItemGroupNameFill();
			CmbTaxTypeFill();
			BaseUnitFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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

	private void panel8_Paint(object sender, PaintEventArgs e)
	{
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

	private void btnUomUpdate_Click(object sender, EventArgs e)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		try
		{
			if (grdhistory.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please check row first");
			}
			GridEXRow[] checkedRows = grdhistory.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				Item.UpdateItemName(new ReportsParameters
				{
					AccountTitle = r.Cells["ItemName"].Value.ToString(),
					OrderNo = r.Cells["ItemCode"].Value.ToString(),
					OtherCode = r.Cells["ItemNameOtherLingo"].Value.ToString(),
					Id = Convert.ToInt32(r.Cells["Id"].Value.ToString())
				});
			}
			MessageBox.Show("Record Update Successfully");
			btnLoadAll_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUomGroupAssignToItem_Click(object sender, EventArgs e)
	{
		try
		{
			new frmAssignGroupToItem(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		new ItemAllocateToWarehouse(UserAccount).Show();
	}

	private void btnItemAllocation_Click(object sender, EventArgs e)
	{
		new ItemsAllocationToCompany(UserAccount).Show();
	}

	private void cmbItemClass_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbItemClass).ActiveRow != null)
			{
				if (((Control)(object)cmbItemClass).Text == "Brand")
				{
					lblCropYear.Show();
					((Control)(object)combcropyear).Show();
				}
				else
				{
					lblCropYear.Hide();
					((Control)(object)combcropyear).Hide();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvDefrmAddItem_FormClosing(object sender, FormClosingEventArgs e)
	{
	}

	private void btnItemMultiLanguage_Click(object sender, EventArgs e)
	{
		try
		{
			new ItemMultiLanguage(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnItemAllocateToManufacturer_Click(object sender, EventArgs e)
	{
		new frmItemAllocateToManufacturer(UserAccount).Show();
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

	private void btnPic3Reset_Click(object sender, EventArgs e)
	{
		try
		{
			fileSavePath3 = null;
			picbox3.Image = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPic4Reset_Click(object sender, EventArgs e)
	{
		try
		{
			fileSavePath4 = null;
			picbox4.Image = null;
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
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Expected O, but got Unknown
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Expected O, but got Unknown
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Expected O, but got Unknown
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Expected O, but got Unknown
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Expected O, but got Unknown
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Expected O, but got Unknown
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Expected O, but got Unknown
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Expected O, but got Unknown
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Expected O, but got Unknown
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Expected O, but got Unknown
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Expected O, but got Unknown
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Expected O, but got Unknown
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Expected O, but got Unknown
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Expected O, but got Unknown
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Expected O, but got Unknown
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Expected O, but got Unknown
		//IL_0664: Unknown result type (might be due to invalid IL or missing references)
		//IL_066e: Expected O, but got Unknown
		//IL_0740: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Expected O, but got Unknown
		//IL_0756: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Expected O, but got Unknown
		//IL_0777: Unknown result type (might be due to invalid IL or missing references)
		//IL_0781: Expected O, but got Unknown
		//IL_07a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Expected O, but got Unknown
		//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Expected O, but got Unknown
		//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Expected O, but got Unknown
		//IL_0945: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Expected O, but got Unknown
		//IL_095b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0965: Expected O, but got Unknown
		//IL_09be: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c8: Expected O, but got Unknown
		//IL_09df: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e9: Expected O, but got Unknown
		//IL_09ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Expected O, but got Unknown
		//IL_0a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a15: Expected O, but got Unknown
		//IL_0a16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a20: Expected O, but got Unknown
		//IL_0a37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a41: Expected O, but got Unknown
		//IL_0a42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4c: Expected O, but got Unknown
		//IL_0a63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6d: Expected O, but got Unknown
		//IL_0abb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac5: Expected O, but got Unknown
		//IL_0b3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b49: Expected O, but got Unknown
		//IL_0ba2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bac: Expected O, but got Unknown
		//IL_9a83: Unknown result type (might be due to invalid IL or missing references)
		//IL_9a8d: Expected O, but got Unknown
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
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
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
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
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Tax_Definition.DefineTaxItem));
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		this.label13 = new System.Windows.Forms.Label();
		this.tabCAddItem = new System.Windows.Forms.TabControl();
		this.tpItemDefinitionAddItem = new System.Windows.Forms.TabPage();
		this.panelDetails = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label35 = new System.Windows.Forms.Label();
		this.txtModelName = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.cmbPurchaseUOM = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.txtMaxStockLevel = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.pnlimage = new System.Windows.Forms.Panel();
		this.btnPic4Reset = new System.Windows.Forms.Button();
		this.btnPic3Reset = new System.Windows.Forms.Button();
		this.btnPic2Reset = new System.Windows.Forms.Button();
		this.btnPic1Reset = new System.Windows.Forms.Button();
		this.label36 = new System.Windows.Forms.Label();
		this.EffectedDate = new System.Windows.Forms.DateTimePicker();
		this.btnImage3 = new System.Windows.Forms.Button();
		this.btnImage4 = new System.Windows.Forms.Button();
		this.picbox3 = new System.Windows.Forms.PictureBox();
		this.picbox4 = new System.Windows.Forms.PictureBox();
		this.btnimg1 = new System.Windows.Forms.Button();
		this.btnimg2 = new System.Windows.Forms.Button();
		this.picbox2 = new System.Windows.Forms.PictureBox();
		this.picbox1 = new System.Windows.Forms.PictureBox();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbManufacturerID = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.cmbAmountCalcType = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.txtPurchaseRate = new System.Windows.Forms.TextBox();
		this.cmbcountry = new UltraCombo();
		this.txtCostPrice = new System.Windows.Forms.TextBox();
		this.txtManufacturerPartNo = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.cmbSaleUOM = new UltraCombo();
		this.txtReorderLevel = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.cmbCostUOM = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.cmbBuyerOrSupplier = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.chbOther = new System.Windows.Forms.CheckBox();
		this.txtProductNumber = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.txtSaleRate = new System.Windows.Forms.TextBox();
		this.chbExcise = new System.Windows.Forms.CheckBox();
		this.label15 = new System.Windows.Forms.Label();
		this.txtBuyerPartNo = new System.Windows.Forms.TextBox();
		this.chbVST = new System.Windows.Forms.CheckBox();
		this.panel1 = new System.Windows.Forms.Panel();
		this.btnItemAllocateToManufacturer = new System.Windows.Forms.Button();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.btnItemMultiLanguage = new System.Windows.Forms.Button();
		this.btnItemAllocation = new System.Windows.Forms.Button();
		this.button1 = new System.Windows.Forms.Button();
		this.btnitmcat = new System.Windows.Forms.Button();
		this.btnUomGroupAssignToItem = new System.Windows.Forms.Button();
		this.btnitmtype = new System.Windows.Forms.Button();
		this.btnItemGroupSchedule = new System.Windows.Forms.Button();
		this.btnitmtaxsch = new System.Windows.Forms.Button();
		this.btnitmuom = new System.Windows.Forms.Button();
		this.btnitmuomtaxsch = new System.Windows.Forms.Button();
		this.btnItemGroup = new System.Windows.Forms.Button();
		this.btnCropYear = new System.Windows.Forms.Button();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdWithoutVariant = new System.Windows.Forms.RadioButton();
		this.rdWithVariant = new System.Windows.Forms.RadioButton();
		this.chkCommissionOnPurchase = new System.Windows.Forms.CheckBox();
		this.chkCommissionOnSale = new System.Windows.Forms.CheckBox();
		this.txtItemCodeNew = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.cmbItemCategory = new UltraCombo();
		this.txtPackQuantity = new System.Windows.Forms.TextBox();
		this.combcropyear = new UltraCombo();
		this.BtnShowDetails = new System.Windows.Forms.Button();
		this.txtOtherLingo = new System.Windows.Forms.TextBox();
		this.txtHSCode = new System.Windows.Forms.TextBox();
		this.lblCropYear = new System.Windows.Forms.Label();
		this.chbGST = new System.Windows.Forms.CheckBox();
		this.txtItemCode = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbItemClass = new UltraCombo();
		this.txtItemName = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbPurchaseGL = new UltraCombo();
		this.cmbTaxType = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbSaleGL = new UltraCombo();
		this.cmbItemType = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbCGSGL = new UltraCombo();
		this.cmbBaseUnit = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbItemGroup = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.chkstatus = new System.Windows.Forms.CheckBox();
		this.label37 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.grdAllocation = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.tpItemDefinitionAddItemHistory = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label30 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grpUpdateItemName = new System.Windows.Forms.GroupBox();
		this.label34 = new System.Windows.Forms.Label();
		this.btnUomUpdate = new System.Windows.Forms.Button();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.cmbitemcathistory = new UltraCombo();
		this.label32 = new System.Windows.Forms.Label();
		this.btnsearch = new System.Windows.Forms.Button();
		this.panel10 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.label31 = new System.Windows.Forms.Label();
		this.tabCAddItem.SuspendLayout();
		this.tpItemDefinitionAddItem.SuspendLayout();
		this.panelDetails.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPurchaseUOM).BeginInit();
		this.pnlimage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picbox3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picbox4).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picbox2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picbox1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbManufacturerID).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAmountCalcType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcountry).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSaleUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCostUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBuyerOrSupplier).BeginInit();
		this.panel1.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcropyear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemClass).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPurchaseGL).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTaxType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSaleGL).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCGSGL).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBaseUnit).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemGroup).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAllocation).BeginInit();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tpItemDefinitionAddItemHistory.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel4.SuspendLayout();
		this.panel9.SuspendLayout();
		this.grpUpdateItemName.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbitemcathistory).BeginInit();
		this.panel10.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.label13.AutoSize = true;
		this.label13.BackColor = System.Drawing.Color.Teal;
		this.label13.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(7, 2);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(85, 19);
		this.label13.TabIndex = 0;
		this.label13.Text = "Add Item";
		this.tabCAddItem.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabCAddItem.Controls.Add(this.tpItemDefinitionAddItem);
		this.tabCAddItem.Controls.Add(this.tpItemDefinitionAddItemHistory);
		this.tabCAddItem.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabCAddItem.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabCAddItem.Location = new System.Drawing.Point(0, 0);
		this.tabCAddItem.Multiline = true;
		this.tabCAddItem.Name = "tabCAddItem";
		this.tabCAddItem.SelectedIndex = 0;
		this.tabCAddItem.Size = new System.Drawing.Size(1136, 711);
		this.tabCAddItem.TabIndex = 0;
		this.tabCAddItem.SelectedIndexChanged += new System.EventHandler(tabCAddItem_SelectedIndexChanged);
		this.tpItemDefinitionAddItem.BackColor = System.Drawing.Color.White;
		this.tpItemDefinitionAddItem.Controls.Add(this.panelDetails);
		this.tpItemDefinitionAddItem.Controls.Add(this.panel1);
		this.tpItemDefinitionAddItem.Controls.Add(this.panel2);
		this.tpItemDefinitionAddItem.Controls.Add(this.panel6);
		this.tpItemDefinitionAddItem.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tpItemDefinitionAddItem.Location = new System.Drawing.Point(4, 4);
		this.tpItemDefinitionAddItem.Name = "tpItemDefinitionAddItem";
		this.tpItemDefinitionAddItem.Padding = new System.Windows.Forms.Padding(3);
		this.tpItemDefinitionAddItem.Size = new System.Drawing.Size(1128, 682);
		this.tpItemDefinitionAddItem.TabIndex = 0;
		this.tpItemDefinitionAddItem.Text = "Add Item";
		this.panelDetails.Controls.Add(this.groupBox4);
		this.panelDetails.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelDetails.Location = new System.Drawing.Point(3, 343);
		this.panelDetails.Name = "panelDetails";
		this.panelDetails.Size = new System.Drawing.Size(1122, 336);
		this.panelDetails.TabIndex = 3;
		this.groupBox4.Controls.Add(this.label35);
		this.groupBox4.Controls.Add(this.txtModelName);
		this.groupBox4.Controls.Add(this.label26);
		this.groupBox4.Controls.Add(this.label17);
		this.groupBox4.Controls.Add(this.label21);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPurchaseUOM);
		this.groupBox4.Controls.Add(this.label20);
		this.groupBox4.Controls.Add(this.txtMaxStockLevel);
		this.groupBox4.Controls.Add(this.label22);
		this.groupBox4.Controls.Add(this.pnlimage);
		this.groupBox4.Controls.Add(this.label19);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbManufacturerID);
		this.groupBox4.Controls.Add(this.label23);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAmountCalcType);
		this.groupBox4.Controls.Add(this.label18);
		this.groupBox4.Controls.Add(this.txtPurchaseRate);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcountry);
		this.groupBox4.Controls.Add(this.txtCostPrice);
		this.groupBox4.Controls.Add(this.txtManufacturerPartNo);
		this.groupBox4.Controls.Add(this.label27);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSaleUOM);
		this.groupBox4.Controls.Add(this.txtReorderLevel);
		this.groupBox4.Controls.Add(this.label25);
		this.groupBox4.Controls.Add(this.label28);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCostUOM);
		this.groupBox4.Controls.Add(this.label16);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBuyerOrSupplier);
		this.groupBox4.Controls.Add(this.label29);
		this.groupBox4.Controls.Add(this.chbOther);
		this.groupBox4.Controls.Add(this.txtProductNumber);
		this.groupBox4.Controls.Add(this.label24);
		this.groupBox4.Controls.Add(this.txtSaleRate);
		this.groupBox4.Controls.Add(this.chbExcise);
		this.groupBox4.Controls.Add(this.label15);
		this.groupBox4.Controls.Add(this.txtBuyerPartNo);
		this.groupBox4.Controls.Add(this.chbVST);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(5, 3);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1110, 268);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label35.Location = new System.Drawing.Point(5, 159);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(76, 15);
		this.label35.TabIndex = 69;
		this.label35.Text = "Model Name";
		this.txtModelName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtModelName.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtModelName.Location = new System.Drawing.Point(128, 155);
		this.txtModelName.Name = "txtModelName";
		this.txtModelName.Size = new System.Drawing.Size(207, 23);
		this.txtModelName.TabIndex = 5;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(5, 21);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(95, 15);
		this.label26.TabIndex = 24;
		this.label26.Text = "Manufacturer ID";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(5, 240);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(93, 15);
		this.label17.TabIndex = 55;
		this.label17.Text = "Max Stock Level";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(346, 49);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(61, 15);
		this.label21.TabIndex = 62;
		this.label21.Text = "Cost UOM";
		this.cmbPurchaseUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbPurchaseUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPurchaseUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPurchaseUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPurchaseUOM).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPurchaseUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPurchaseUOM).Location = new System.Drawing.Point(451, 101);
		((System.Windows.Forms.Control)(object)this.cmbPurchaseUOM).Name = "cmbPurchaseUOM";
		((System.Windows.Forms.Control)(object)this.cmbPurchaseUOM).Size = new System.Drawing.Size(141, 26);
		((System.Windows.Forms.Control)(object)this.cmbPurchaseUOM).TabIndex = 12;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(346, 21);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(59, 15);
		this.label20.TabIndex = 61;
		this.label20.Text = "Cost Price";
		this.txtMaxStockLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMaxStockLevel.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtMaxStockLevel.Location = new System.Drawing.Point(128, 236);
		this.txtMaxStockLevel.Name = "txtMaxStockLevel";
		this.txtMaxStockLevel.Size = new System.Drawing.Size(207, 23);
		this.txtMaxStockLevel.TabIndex = 8;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(346, 78);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(81, 15);
		this.label22.TabIndex = 63;
		this.label22.Text = "Purchase Rate";
		this.pnlimage.Controls.Add(this.btnPic4Reset);
		this.pnlimage.Controls.Add(this.btnPic3Reset);
		this.pnlimage.Controls.Add(this.btnPic2Reset);
		this.pnlimage.Controls.Add(this.btnPic1Reset);
		this.pnlimage.Controls.Add(this.label36);
		this.pnlimage.Controls.Add(this.EffectedDate);
		this.pnlimage.Controls.Add(this.btnImage3);
		this.pnlimage.Controls.Add(this.btnImage4);
		this.pnlimage.Controls.Add(this.picbox3);
		this.pnlimage.Controls.Add(this.picbox4);
		this.pnlimage.Controls.Add(this.btnimg1);
		this.pnlimage.Controls.Add(this.btnimg2);
		this.pnlimage.Controls.Add(this.picbox2);
		this.pnlimage.Controls.Add(this.picbox1);
		this.pnlimage.Location = new System.Drawing.Point(602, 20);
		this.pnlimage.Name = "pnlimage";
		this.pnlimage.Size = new System.Drawing.Size(505, 186);
		this.pnlimage.TabIndex = 18;
		this.btnPic4Reset.BackColor = System.Drawing.Color.Teal;
		this.btnPic4Reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPic4Reset.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.btnPic4Reset.ForeColor = System.Drawing.Color.White;
		this.btnPic4Reset.Location = new System.Drawing.Point(462, 157);
		this.btnPic4Reset.Name = "btnPic4Reset";
		this.btnPic4Reset.Size = new System.Drawing.Size(34, 26);
		this.btnPic4Reset.TabIndex = 74;
		this.btnPic4Reset.Text = "X";
		this.btnPic4Reset.UseVisualStyleBackColor = false;
		this.btnPic4Reset.Click += new System.EventHandler(btnPic4Reset_Click);
		this.btnPic3Reset.BackColor = System.Drawing.Color.Teal;
		this.btnPic3Reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPic3Reset.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.btnPic3Reset.ForeColor = System.Drawing.Color.White;
		this.btnPic3Reset.Location = new System.Drawing.Point(335, 157);
		this.btnPic3Reset.Name = "btnPic3Reset";
		this.btnPic3Reset.Size = new System.Drawing.Size(33, 26);
		this.btnPic3Reset.TabIndex = 73;
		this.btnPic3Reset.Text = "X";
		this.btnPic3Reset.UseVisualStyleBackColor = false;
		this.btnPic3Reset.Click += new System.EventHandler(btnPic3Reset_Click);
		this.btnPic2Reset.BackColor = System.Drawing.Color.Teal;
		this.btnPic2Reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPic2Reset.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.btnPic2Reset.ForeColor = System.Drawing.Color.White;
		this.btnPic2Reset.Location = new System.Drawing.Point(211, 157);
		this.btnPic2Reset.Name = "btnPic2Reset";
		this.btnPic2Reset.Size = new System.Drawing.Size(32, 26);
		this.btnPic2Reset.TabIndex = 72;
		this.btnPic2Reset.Text = "X";
		this.btnPic2Reset.UseVisualStyleBackColor = false;
		this.btnPic2Reset.Click += new System.EventHandler(btnPic2Reset_Click);
		this.btnPic1Reset.BackColor = System.Drawing.Color.Teal;
		this.btnPic1Reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPic1Reset.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.btnPic1Reset.ForeColor = System.Drawing.Color.White;
		this.btnPic1Reset.Location = new System.Drawing.Point(85, 158);
		this.btnPic1Reset.Name = "btnPic1Reset";
		this.btnPic1Reset.Size = new System.Drawing.Size(31, 25);
		this.btnPic1Reset.TabIndex = 71;
		this.btnPic1Reset.Text = "X";
		this.btnPic1Reset.UseVisualStyleBackColor = false;
		this.btnPic1Reset.Click += new System.EventHandler(btnPic1Reset_Click);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(127, 10);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(120, 19);
		this.label36.TabIndex = 70;
		this.label36.Text = "Effected Date";
		this.EffectedDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.EffectedDate.CustomFormat = "dd-MMM-yy";
		this.EffectedDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.EffectedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.EffectedDate.Location = new System.Drawing.Point(256, 8);
		this.EffectedDate.Name = "EffectedDate";
		this.EffectedDate.Size = new System.Drawing.Size(119, 23);
		this.EffectedDate.TabIndex = 70;
		this.btnImage3.BackColor = System.Drawing.Color.Teal;
		this.btnImage3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnImage3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.btnImage3.ForeColor = System.Drawing.Color.White;
		this.btnImage3.Location = new System.Drawing.Point(260, 157);
		this.btnImage3.Name = "btnImage3";
		this.btnImage3.Size = new System.Drawing.Size(76, 26);
		this.btnImage3.TabIndex = 39;
		this.btnImage3.Text = "Browse";
		this.btnImage3.UseVisualStyleBackColor = false;
		this.btnImage3.Click += new System.EventHandler(btnImage3_Click);
		this.btnImage4.BackColor = System.Drawing.Color.Teal;
		this.btnImage4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnImage4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.btnImage4.ForeColor = System.Drawing.Color.White;
		this.btnImage4.Location = new System.Drawing.Point(388, 157);
		this.btnImage4.Name = "btnImage4";
		this.btnImage4.Size = new System.Drawing.Size(76, 26);
		this.btnImage4.TabIndex = 40;
		this.btnImage4.Text = "Browse";
		this.btnImage4.UseVisualStyleBackColor = false;
		this.btnImage4.Click += new System.EventHandler(btnImage4_Click);
		this.picbox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.picbox3.Location = new System.Drawing.Point(255, 39);
		this.picbox3.Name = "picbox3";
		this.picbox3.Size = new System.Drawing.Size(118, 115);
		this.picbox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picbox3.TabIndex = 38;
		this.picbox3.TabStop = false;
		this.picbox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.picbox4.Location = new System.Drawing.Point(381, 39);
		this.picbox4.Name = "picbox4";
		this.picbox4.Size = new System.Drawing.Size(118, 115);
		this.picbox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picbox4.TabIndex = 37;
		this.picbox4.TabStop = false;
		this.btnimg1.BackColor = System.Drawing.Color.Teal;
		this.btnimg1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnimg1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.btnimg1.ForeColor = System.Drawing.Color.White;
		this.btnimg1.Location = new System.Drawing.Point(8, 158);
		this.btnimg1.Name = "btnimg1";
		this.btnimg1.Size = new System.Drawing.Size(77, 25);
		this.btnimg1.TabIndex = 0;
		this.btnimg1.Text = "Browse";
		this.btnimg1.UseVisualStyleBackColor = false;
		this.btnimg1.Click += new System.EventHandler(btnimg1_Click);
		this.btnimg2.BackColor = System.Drawing.Color.Teal;
		this.btnimg2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnimg2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.btnimg2.ForeColor = System.Drawing.Color.White;
		this.btnimg2.Location = new System.Drawing.Point(135, 157);
		this.btnimg2.Name = "btnimg2";
		this.btnimg2.Size = new System.Drawing.Size(76, 26);
		this.btnimg2.TabIndex = 1;
		this.btnimg2.Text = "Browse";
		this.btnimg2.UseVisualStyleBackColor = false;
		this.btnimg2.Click += new System.EventHandler(btnimg2_Click);
		this.picbox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.picbox2.Location = new System.Drawing.Point(129, 39);
		this.picbox2.Name = "picbox2";
		this.picbox2.Size = new System.Drawing.Size(118, 115);
		this.picbox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picbox2.TabIndex = 36;
		this.picbox2.TabStop = false;
		this.picbox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.picbox1.Location = new System.Drawing.Point(3, 39);
		this.picbox1.Name = "picbox1";
		this.picbox1.Size = new System.Drawing.Size(118, 115);
		this.picbox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picbox1.TabIndex = 35;
		this.picbox1.TabStop = false;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(5, 213);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(72, 15);
		this.label19.TabIndex = 60;
		this.label19.Text = "Item Region";
		this.cmbManufacturerID.AllowNull = (DefaultableBoolean)1;
		this.cmbManufacturerID.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbManufacturerID.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbManufacturerID.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbManufacturerID).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbManufacturerID).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbManufacturerID).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbManufacturerID.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbManufacturerID).Location = new System.Drawing.Point(128, 15);
		((System.Windows.Forms.Control)(object)this.cmbManufacturerID).Name = "cmbManufacturerID";
		((System.Windows.Forms.Control)(object)this.cmbManufacturerID).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbManufacturerID).TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label23.Location = new System.Drawing.Point(346, 107);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(86, 15);
		this.label23.TabIndex = 64;
		this.label23.Text = "Purchase UOM";
		this.cmbAmountCalcType.AllowNull = (DefaultableBoolean)1;
		this.cmbAmountCalcType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAmountCalcType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAmountCalcType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAmountCalcType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbAmountCalcType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAmountCalcType).Location = new System.Drawing.Point(451, 185);
		((System.Windows.Forms.Control)(object)this.cmbAmountCalcType).Name = "cmbAmountCalcType";
		((System.Windows.Forms.Control)(object)this.cmbAmountCalcType).Size = new System.Drawing.Size(141, 26);
		((System.Windows.Forms.Control)(object)this.cmbAmountCalcType).TabIndex = 15;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(5, 185);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(80, 15);
		this.label18.TabIndex = 59;
		this.label18.Text = "ReOrder Level";
		this.txtPurchaseRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPurchaseRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPurchaseRate.Location = new System.Drawing.Point(451, 74);
		this.txtPurchaseRate.Name = "txtPurchaseRate";
		this.txtPurchaseRate.Size = new System.Drawing.Size(141, 23);
		this.txtPurchaseRate.TabIndex = 11;
		this.cmbcountry.AllowNull = (DefaultableBoolean)1;
		this.cmbcountry.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcountry.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcountry.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbcountry).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcountry).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbcountry).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcountry).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbcountry).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcountry).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcountry).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcountry).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcountry.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcountry).Location = new System.Drawing.Point(128, 207);
		((System.Windows.Forms.Control)(object)this.cmbcountry).Name = "cmbcountry";
		((System.Windows.Forms.Control)(object)this.cmbcountry).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbcountry).TabIndex = 7;
		this.txtCostPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCostPrice.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCostPrice.Location = new System.Drawing.Point(451, 17);
		this.txtCostPrice.Name = "txtCostPrice";
		this.txtCostPrice.Size = new System.Drawing.Size(141, 23);
		this.txtCostPrice.TabIndex = 9;
		this.txtManufacturerPartNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtManufacturerPartNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtManufacturerPartNo.Location = new System.Drawing.Point(128, 45);
		this.txtManufacturerPartNo.Name = "txtManufacturerPartNo";
		this.txtManufacturerPartNo.Size = new System.Drawing.Size(207, 23);
		this.txtManufacturerPartNo.TabIndex = 1;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(346, 134);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(55, 15);
		this.label27.TabIndex = 65;
		this.label27.Text = "Sale Rate";
		this.cmbSaleUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbSaleUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSaleUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSaleUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSaleUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.cmbSaleUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSaleUOM).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbSaleUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSaleUOM).Location = new System.Drawing.Point(451, 156);
		((System.Windows.Forms.Control)(object)this.cmbSaleUOM).Name = "cmbSaleUOM";
		((System.Windows.Forms.Control)(object)this.cmbSaleUOM).Size = new System.Drawing.Size(141, 26);
		((System.Windows.Forms.Control)(object)this.cmbSaleUOM).TabIndex = 14;
		this.txtReorderLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtReorderLevel.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtReorderLevel.Location = new System.Drawing.Point(128, 181);
		this.txtReorderLevel.Name = "txtReorderLevel";
		this.txtReorderLevel.Size = new System.Drawing.Size(207, 23);
		this.txtReorderLevel.TabIndex = 6;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(5, 49);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(122, 15);
		this.label25.TabIndex = 25;
		this.label25.Text = "Manufacturer Part No";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(346, 162);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(60, 15);
		this.label28.TabIndex = 66;
		this.label28.Text = "Sale UOM";
		this.cmbCostUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbCostUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCostUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCostUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCostUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.cmbCostUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCostUOM).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCostUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCostUOM).Location = new System.Drawing.Point(451, 43);
		((System.Windows.Forms.Control)(object)this.cmbCostUOM).Name = "cmbCostUOM";
		((System.Windows.Forms.Control)(object)this.cmbCostUOM).Size = new System.Drawing.Size(141, 26);
		((System.Windows.Forms.Control)(object)this.cmbCostUOM).TabIndex = 10;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(5, 134);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(96, 15);
		this.label16.TabIndex = 52;
		this.label16.Text = "Product Number";
		this.cmbBuyerOrSupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbBuyerOrSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBuyerOrSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBuyerOrSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBuyerOrSupplier).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbBuyerOrSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBuyerOrSupplier).Location = new System.Drawing.Point(128, 72);
		((System.Windows.Forms.Control)(object)this.cmbBuyerOrSupplier).Name = "cmbBuyerOrSupplier";
		((System.Windows.Forms.Control)(object)this.cmbBuyerOrSupplier).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbBuyerOrSupplier).TabIndex = 2;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(346, 191);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(103, 15);
		this.label29.TabIndex = 67;
		this.label29.Text = "Amount calc Type";
		this.chbOther.AutoSize = true;
		this.chbOther.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chbOther.Location = new System.Drawing.Point(458, 217);
		this.chbOther.Name = "chbOther";
		this.chbOther.Size = new System.Drawing.Size(56, 19);
		this.chbOther.TabIndex = 18;
		this.chbOther.Text = "Other";
		this.chbOther.UseVisualStyleBackColor = true;
		this.txtProductNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProductNumber.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtProductNumber.Location = new System.Drawing.Point(128, 130);
		this.txtProductNumber.Name = "txtProductNumber";
		this.txtProductNumber.Size = new System.Drawing.Size(207, 23);
		this.txtProductNumber.TabIndex = 4;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(5, 78);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(86, 15);
		this.label24.TabIndex = 26;
		this.label24.Text = "Buyer/Supplier";
		this.txtSaleRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSaleRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtSaleRate.Location = new System.Drawing.Point(451, 130);
		this.txtSaleRate.Name = "txtSaleRate";
		this.txtSaleRate.Size = new System.Drawing.Size(141, 23);
		this.txtSaleRate.TabIndex = 13;
		this.chbExcise.AutoSize = true;
		this.chbExcise.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chbExcise.Location = new System.Drawing.Point(394, 217);
		this.chbExcise.Name = "chbExcise";
		this.chbExcise.Size = new System.Drawing.Size(58, 19);
		this.chbExcise.TabIndex = 17;
		this.chbExcise.Text = "Excise";
		this.chbExcise.UseVisualStyleBackColor = true;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(5, 107);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(80, 15);
		this.label15.TabIndex = 51;
		this.label15.Text = "Buyer Part No";
		this.txtBuyerPartNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBuyerPartNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBuyerPartNo.Location = new System.Drawing.Point(128, 103);
		this.txtBuyerPartNo.Name = "txtBuyerPartNo";
		this.txtBuyerPartNo.Size = new System.Drawing.Size(207, 23);
		this.txtBuyerPartNo.TabIndex = 3;
		this.chbVST.AutoSize = true;
		this.chbVST.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chbVST.Location = new System.Drawing.Point(341, 217);
		this.chbVST.Name = "chbVST";
		this.chbVST.Size = new System.Drawing.Size(47, 19);
		this.chbVST.TabIndex = 16;
		this.chbVST.Text = "VAT";
		this.chbVST.UseVisualStyleBackColor = true;
		this.panel1.Controls.Add(this.btnItemAllocateToManufacturer);
		this.panel1.Controls.Add(this.groupBox3);
		this.panel1.Controls.Add(this.groupBox2);
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 59);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1122, 284);
		this.panel1.TabIndex = 0;
		this.btnItemAllocateToManufacturer.BackColor = System.Drawing.Color.Teal;
		this.btnItemAllocateToManufacturer.FlatAppearance.BorderSize = 0;
		this.btnItemAllocateToManufacturer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnItemAllocateToManufacturer.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnItemAllocateToManufacturer.ForeColor = System.Drawing.Color.White;
		this.btnItemAllocateToManufacturer.Location = new System.Drawing.Point(872, 250);
		this.btnItemAllocateToManufacturer.Name = "btnItemAllocateToManufacturer";
		this.btnItemAllocateToManufacturer.Size = new System.Drawing.Size(243, 27);
		this.btnItemAllocateToManufacturer.TabIndex = 16;
		this.btnItemAllocateToManufacturer.Text = "Item Allocate To Manufacturer";
		this.btnItemAllocateToManufacturer.UseVisualStyleBackColor = false;
		this.btnItemAllocateToManufacturer.Click += new System.EventHandler(btnItemAllocateToManufacturer_Click);
		this.groupBox3.Controls.Add(this.btnItemMultiLanguage);
		this.groupBox3.Controls.Add(this.btnItemAllocation);
		this.groupBox3.Controls.Add(this.button1);
		this.groupBox3.Controls.Add(this.btnitmcat);
		this.groupBox3.Controls.Add(this.btnUomGroupAssignToItem);
		this.groupBox3.Controls.Add(this.btnitmtype);
		this.groupBox3.Controls.Add(this.btnItemGroupSchedule);
		this.groupBox3.Controls.Add(this.btnitmtaxsch);
		this.groupBox3.Controls.Add(this.btnitmuom);
		this.groupBox3.Controls.Add(this.btnitmuomtaxsch);
		this.groupBox3.Controls.Add(this.btnItemGroup);
		this.groupBox3.Controls.Add(this.btnCropYear);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(602, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(263, 277);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Related Defination";
		this.btnItemMultiLanguage.BackColor = System.Drawing.Color.Teal;
		this.btnItemMultiLanguage.FlatAppearance.BorderSize = 0;
		this.btnItemMultiLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnItemMultiLanguage.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnItemMultiLanguage.ForeColor = System.Drawing.Color.White;
		this.btnItemMultiLanguage.Location = new System.Drawing.Point(6, 234);
		this.btnItemMultiLanguage.Name = "btnItemMultiLanguage";
		this.btnItemMultiLanguage.Size = new System.Drawing.Size(253, 27);
		this.btnItemMultiLanguage.TabIndex = 11;
		this.btnItemMultiLanguage.Text = "Item Multi Language";
		this.btnItemMultiLanguage.UseVisualStyleBackColor = false;
		this.btnItemMultiLanguage.Click += new System.EventHandler(btnItemMultiLanguage_Click);
		this.btnItemAllocation.BackColor = System.Drawing.Color.Teal;
		this.btnItemAllocation.FlatAppearance.BorderSize = 0;
		this.btnItemAllocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnItemAllocation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnItemAllocation.ForeColor = System.Drawing.Color.White;
		this.btnItemAllocation.Location = new System.Drawing.Point(6, 205);
		this.btnItemAllocation.Name = "btnItemAllocation";
		this.btnItemAllocation.Size = new System.Drawing.Size(253, 27);
		this.btnItemAllocation.TabIndex = 10;
		this.btnItemAllocation.Text = "Item Allocation to Location";
		this.btnItemAllocation.UseVisualStyleBackColor = false;
		this.btnItemAllocation.Click += new System.EventHandler(btnItemAllocation_Click);
		this.button1.BackColor = System.Drawing.Color.Teal;
		this.button1.FlatAppearance.BorderSize = 0;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.ForeColor = System.Drawing.Color.White;
		this.button1.Location = new System.Drawing.Point(98, 161);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(161, 41);
		this.button1.TabIndex = 9;
		this.button1.Text = "Items Allocate to Warehouse";
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.btnitmcat.BackColor = System.Drawing.Color.Teal;
		this.btnitmcat.FlatAppearance.BorderSize = 0;
		this.btnitmcat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnitmcat.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnitmcat.ForeColor = System.Drawing.Color.White;
		this.btnitmcat.Location = new System.Drawing.Point(6, 27);
		this.btnitmcat.Name = "btnitmcat";
		this.btnitmcat.Size = new System.Drawing.Size(89, 41);
		this.btnitmcat.TabIndex = 0;
		this.btnitmcat.Text = "Item Category";
		this.btnitmcat.UseVisualStyleBackColor = false;
		this.btnitmcat.Click += new System.EventHandler(btnitmcat_Click);
		this.btnUomGroupAssignToItem.BackColor = System.Drawing.Color.Teal;
		this.btnUomGroupAssignToItem.FlatAppearance.BorderSize = 0;
		this.btnUomGroupAssignToItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUomGroupAssignToItem.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUomGroupAssignToItem.ForeColor = System.Drawing.Color.White;
		this.btnUomGroupAssignToItem.Location = new System.Drawing.Point(98, 117);
		this.btnUomGroupAssignToItem.Name = "btnUomGroupAssignToItem";
		this.btnUomGroupAssignToItem.Size = new System.Drawing.Size(161, 41);
		this.btnUomGroupAssignToItem.TabIndex = 7;
		this.btnUomGroupAssignToItem.Text = "Uom Group Assign To Item";
		this.btnUomGroupAssignToItem.UseVisualStyleBackColor = false;
		this.btnUomGroupAssignToItem.Click += new System.EventHandler(btnUomGroupAssignToItem_Click);
		this.btnitmtype.BackColor = System.Drawing.Color.Teal;
		this.btnitmtype.FlatAppearance.BorderSize = 0;
		this.btnitmtype.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnitmtype.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnitmtype.ForeColor = System.Drawing.Color.White;
		this.btnitmtype.Location = new System.Drawing.Point(98, 27);
		this.btnitmtype.Name = "btnitmtype";
		this.btnitmtype.Size = new System.Drawing.Size(51, 41);
		this.btnitmtype.TabIndex = 1;
		this.btnitmtype.Text = "Item Type";
		this.btnitmtype.UseVisualStyleBackColor = false;
		this.btnitmtype.Click += new System.EventHandler(btnitmtype_Click);
		this.btnItemGroupSchedule.BackColor = System.Drawing.Color.Teal;
		this.btnItemGroupSchedule.FlatAppearance.BorderSize = 0;
		this.btnItemGroupSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnItemGroupSchedule.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnItemGroupSchedule.ForeColor = System.Drawing.Color.White;
		this.btnItemGroupSchedule.Location = new System.Drawing.Point(98, 72);
		this.btnItemGroupSchedule.Name = "btnItemGroupSchedule";
		this.btnItemGroupSchedule.Size = new System.Drawing.Size(161, 41);
		this.btnItemGroupSchedule.TabIndex = 5;
		this.btnItemGroupSchedule.Text = "Item Uom Group Schedule";
		this.btnItemGroupSchedule.UseVisualStyleBackColor = false;
		this.btnItemGroupSchedule.Click += new System.EventHandler(btnItemGroupSchedule_Click);
		this.btnitmtaxsch.BackColor = System.Drawing.Color.Teal;
		this.btnitmtaxsch.FlatAppearance.BorderSize = 0;
		this.btnitmtaxsch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnitmtaxsch.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnitmtaxsch.ForeColor = System.Drawing.Color.White;
		this.btnitmtaxsch.Location = new System.Drawing.Point(6, 161);
		this.btnitmtaxsch.Name = "btnitmtaxsch";
		this.btnitmtaxsch.Size = new System.Drawing.Size(89, 41);
		this.btnitmtaxsch.TabIndex = 8;
		this.btnitmtaxsch.Text = "Item Tax Schedule";
		this.btnitmtaxsch.UseVisualStyleBackColor = false;
		this.btnitmtaxsch.Click += new System.EventHandler(btnitmtaxsch_Click);
		this.btnitmuom.BackColor = System.Drawing.Color.Teal;
		this.btnitmuom.FlatAppearance.BorderSize = 0;
		this.btnitmuom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnitmuom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnitmuom.ForeColor = System.Drawing.Color.White;
		this.btnitmuom.Location = new System.Drawing.Point(153, 27);
		this.btnitmuom.Name = "btnitmuom";
		this.btnitmuom.Size = new System.Drawing.Size(51, 41);
		this.btnitmuom.TabIndex = 2;
		this.btnitmuom.Text = "Item Uom";
		this.btnitmuom.UseVisualStyleBackColor = false;
		this.btnitmuom.Click += new System.EventHandler(btnitmuom_Click);
		this.btnitmuomtaxsch.BackColor = System.Drawing.Color.Teal;
		this.btnitmuomtaxsch.FlatAppearance.BorderSize = 0;
		this.btnitmuomtaxsch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnitmuomtaxsch.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnitmuomtaxsch.ForeColor = System.Drawing.Color.White;
		this.btnitmuomtaxsch.Location = new System.Drawing.Point(6, 117);
		this.btnitmuomtaxsch.Name = "btnitmuomtaxsch";
		this.btnitmuomtaxsch.Size = new System.Drawing.Size(89, 41);
		this.btnitmuomtaxsch.TabIndex = 6;
		this.btnitmuomtaxsch.Text = "Item Uom Schedule";
		this.btnitmuomtaxsch.UseVisualStyleBackColor = false;
		this.btnitmuomtaxsch.Click += new System.EventHandler(btnitmuomtaxsch_Click);
		this.btnItemGroup.BackColor = System.Drawing.Color.Teal;
		this.btnItemGroup.FlatAppearance.BorderSize = 0;
		this.btnItemGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnItemGroup.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnItemGroup.ForeColor = System.Drawing.Color.White;
		this.btnItemGroup.Location = new System.Drawing.Point(6, 72);
		this.btnItemGroup.Name = "btnItemGroup";
		this.btnItemGroup.Size = new System.Drawing.Size(89, 41);
		this.btnItemGroup.TabIndex = 4;
		this.btnItemGroup.Text = "Item Uom Group";
		this.btnItemGroup.UseVisualStyleBackColor = false;
		this.btnItemGroup.Click += new System.EventHandler(btnItemGroup_Click);
		this.btnCropYear.BackColor = System.Drawing.Color.Teal;
		this.btnCropYear.FlatAppearance.BorderSize = 0;
		this.btnCropYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCropYear.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCropYear.ForeColor = System.Drawing.Color.White;
		this.btnCropYear.Location = new System.Drawing.Point(208, 27);
		this.btnCropYear.Name = "btnCropYear";
		this.btnCropYear.Size = new System.Drawing.Size(51, 41);
		this.btnCropYear.TabIndex = 3;
		this.btnCropYear.Text = "Crop Year";
		this.btnCropYear.UseVisualStyleBackColor = false;
		this.btnCropYear.Click += new System.EventHandler(btnCropYear_Click);
		this.groupBox2.Controls.Add(this.rdWithoutVariant);
		this.groupBox2.Controls.Add(this.rdWithVariant);
		this.groupBox2.Controls.Add(this.chkCommissionOnPurchase);
		this.groupBox2.Controls.Add(this.chkCommissionOnSale);
		this.groupBox2.Controls.Add(this.txtItemCodeNew);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCategory);
		this.groupBox2.Controls.Add(this.txtPackQuantity);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combcropyear);
		this.groupBox2.Controls.Add(this.BtnShowDetails);
		this.groupBox2.Controls.Add(this.txtOtherLingo);
		this.groupBox2.Controls.Add(this.txtHSCode);
		this.groupBox2.Controls.Add(this.lblCropYear);
		this.groupBox2.Controls.Add(this.chbGST);
		this.groupBox2.Controls.Add(this.txtItemCode);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemClass);
		this.groupBox2.Controls.Add(this.txtItemName);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPurchaseGL);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTaxType);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSaleGL);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemType);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCGSGL);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBaseUnit);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemGroup);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.chkstatus);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.label33);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(593, 277);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.rdWithoutVariant.AutoSize = true;
		this.rdWithoutVariant.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdWithoutVariant.Location = new System.Drawing.Point(480, 247);
		this.rdWithoutVariant.Name = "rdWithoutVariant";
		this.rdWithoutVariant.Size = new System.Drawing.Size(109, 19);
		this.rdWithoutVariant.TabIndex = 193;
		this.rdWithoutVariant.TabStop = true;
		this.rdWithoutVariant.Text = "Without Variant";
		this.rdWithoutVariant.UseVisualStyleBackColor = true;
		this.rdWithVariant.AutoSize = true;
		this.rdWithVariant.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdWithVariant.Location = new System.Drawing.Point(480, 228);
		this.rdWithVariant.Name = "rdWithVariant";
		this.rdWithVariant.Size = new System.Drawing.Size(91, 19);
		this.rdWithVariant.TabIndex = 192;
		this.rdWithVariant.TabStop = true;
		this.rdWithVariant.Text = "With Variant";
		this.rdWithVariant.UseVisualStyleBackColor = true;
		this.chkCommissionOnPurchase.AutoSize = true;
		this.chkCommissionOnPurchase.Checked = true;
		this.chkCommissionOnPurchase.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkCommissionOnPurchase.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chkCommissionOnPurchase.Location = new System.Drawing.Point(289, 247);
		this.chkCommissionOnPurchase.Name = "chkCommissionOnPurchase";
		this.chkCommissionOnPurchase.Size = new System.Drawing.Size(160, 19);
		this.chkCommissionOnPurchase.TabIndex = 18;
		this.chkCommissionOnPurchase.Text = "Commission on Purchase";
		this.chkCommissionOnPurchase.UseVisualStyleBackColor = true;
		this.chkCommissionOnSale.AutoSize = true;
		this.chkCommissionOnSale.Checked = true;
		this.chkCommissionOnSale.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkCommissionOnSale.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chkCommissionOnSale.Location = new System.Drawing.Point(289, 228);
		this.chkCommissionOnSale.Name = "chkCommissionOnSale";
		this.chkCommissionOnSale.Size = new System.Drawing.Size(131, 19);
		this.chkCommissionOnSale.TabIndex = 17;
		this.chkCommissionOnSale.Text = "Commision On Sale";
		this.chkCommissionOnSale.UseVisualStyleBackColor = true;
		this.txtItemCodeNew.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemCodeNew.Location = new System.Drawing.Point(91, 51);
		this.txtItemCodeNew.Name = "txtItemCodeNew";
		this.txtItemCodeNew.ReadOnly = true;
		this.txtItemCodeNew.Size = new System.Drawing.Size(192, 23);
		this.txtItemCodeNew.TabIndex = 1;
		this.txtItemCodeNew.TabStop = false;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(6, 252);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(96, 19);
		this.label14.TabIndex = 191;
		this.label14.Text = "More Details";
		this.cmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Location = new System.Drawing.Point(91, 20);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Name = "cmbItemCategory";
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Leave += new System.EventHandler(cmbItemCategory_Leave);
		this.txtPackQuantity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackQuantity.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPackQuantity.Location = new System.Drawing.Point(369, 135);
		this.txtPackQuantity.Name = "txtPackQuantity";
		this.txtPackQuantity.Size = new System.Drawing.Size(220, 23);
		this.txtPackQuantity.TabIndex = 12;
		this.combcropyear.AllowNull = (DefaultableBoolean)1;
		this.combcropyear.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcropyear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcropyear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcropyear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.combcropyear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcropyear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcropyear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcropyear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.combcropyear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcropyear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.combcropyear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcropyear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance195).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.combcropyear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcropyear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcropyear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcropyear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combcropyear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcropyear).Location = new System.Drawing.Point(369, 191);
		((System.Windows.Forms.Control)(object)this.combcropyear).Name = "combcropyear";
		((System.Windows.Forms.Control)(object)this.combcropyear).Size = new System.Drawing.Size(143, 26);
		((System.Windows.Forms.Control)(object)this.combcropyear).TabIndex = 15;
		this.BtnShowDetails.AutoSize = true;
		this.BtnShowDetails.BackColor = System.Drawing.Color.Teal;
		this.BtnShowDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowDetails.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowDetails.ForeColor = System.Drawing.Color.White;
		this.BtnShowDetails.Location = new System.Drawing.Point(103, 247);
		this.BtnShowDetails.Name = "BtnShowDetails";
		this.BtnShowDetails.Size = new System.Drawing.Size(41, 28);
		this.BtnShowDetails.TabIndex = 18;
		this.BtnShowDetails.Text = "+";
		this.BtnShowDetails.UseVisualStyleBackColor = false;
		this.BtnShowDetails.Click += new System.EventHandler(BtnShowDetails_Click);
		this.txtOtherLingo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherLingo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOtherLingo.Location = new System.Drawing.Point(91, 107);
		this.txtOtherLingo.Name = "txtOtherLingo";
		this.txtOtherLingo.Size = new System.Drawing.Size(192, 23);
		this.txtOtherLingo.TabIndex = 3;
		this.txtHSCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtHSCode.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtHSCode.Location = new System.Drawing.Point(369, 107);
		this.txtHSCode.Name = "txtHSCode";
		this.txtHSCode.Size = new System.Drawing.Size(220, 23);
		this.txtHSCode.TabIndex = 11;
		this.lblCropYear.AutoSize = true;
		this.lblCropYear.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblCropYear.Location = new System.Drawing.Point(292, 197);
		this.lblCropYear.Name = "lblCropYear";
		this.lblCropYear.Size = new System.Drawing.Size(57, 15);
		this.lblCropYear.TabIndex = 43;
		this.lblCropYear.Text = "Crop Year";
		this.chbGST.AutoSize = true;
		this.chbGST.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chbGST.Location = new System.Drawing.Point(518, 163);
		this.chbGST.Name = "chbGST";
		this.chbGST.Size = new System.Drawing.Size(48, 19);
		this.chbGST.TabIndex = 14;
		this.chbGST.Text = "GST";
		this.chbGST.UseVisualStyleBackColor = true;
		this.txtItemCode.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemCode.Location = new System.Drawing.Point(176, 250);
		this.txtItemCode.Name = "txtItemCode";
		this.txtItemCode.ReadOnly = true;
		this.txtItemCode.Size = new System.Drawing.Size(107, 23);
		this.txtItemCode.TabIndex = 1;
		this.txtItemCode.TabStop = false;
		this.txtItemCode.Visible = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(6, 55);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(62, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Item Code";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(6, 226);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(69, 15);
		this.label9.TabIndex = 11;
		this.label9.Text = "Uom Group";
		this.cmbItemClass.AllowNull = (DefaultableBoolean)1;
		this.cmbItemClass.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemClass.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemClass.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Appearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance137;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance143).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemClass.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Location = new System.Drawing.Point(91, 191);
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Name = "cmbItemClass";
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemClass).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbItemClass).TextChanged += new System.EventHandler(cmbItemClass_TextChanged);
		this.txtItemName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemName.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemName.Location = new System.Drawing.Point(91, 80);
		this.txtItemName.Name = "txtItemName";
		this.txtItemName.Size = new System.Drawing.Size(192, 23);
		this.txtItemName.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(6, 84);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 1;
		this.label2.Text = "Item Name";
		this.cmbPurchaseGL.AllowNull = (DefaultableBoolean)1;
		this.cmbPurchaseGL.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPurchaseGL.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPurchaseGL.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance132).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPurchaseGL.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Location = new System.Drawing.Point(369, 20);
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Name = "cmbPurchaseGL";
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).TabIndex = 8;
		this.cmbTaxType.AllowNull = (DefaultableBoolean)1;
		this.cmbTaxType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTaxType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTaxType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance164;
		((SpecialBoxBase)((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance169).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbTaxType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Location = new System.Drawing.Point(369, 162);
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Name = "cmbTaxType";
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Size = new System.Drawing.Size(143, 26);
		((System.Windows.Forms.Control)(object)this.cmbTaxType).TabIndex = 13;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(292, 84);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(68, 15);
		this.label12.TabIndex = 11;
		this.label12.Text = "CGS GL A/c";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(6, 139);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(56, 15);
		this.label3.TabIndex = 2;
		this.label3.Text = "Base Unit";
		this.cmbSaleGL.AllowNull = (DefaultableBoolean)1;
		this.cmbSaleGL.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSaleGL.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSaleGL.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSaleGL).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.cmbSaleGL).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbSaleGL.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).Location = new System.Drawing.Point(369, 49);
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).Name = "cmbSaleGL";
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).TabIndex = 9;
		this.cmbItemType.AllowNull = (DefaultableBoolean)1;
		this.cmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance177;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance182).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance183).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance183).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance183).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance185).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Location = new System.Drawing.Point(91, 162);
		((System.Windows.Forms.Control)(object)this.cmbItemType).Name = "cmbItemType";
		((System.Windows.Forms.Control)(object)this.cmbItemType).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemType).TabIndex = 5;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(292, 55);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(68, 15);
		this.label11.TabIndex = 10;
		this.label11.Text = "Sale GL A/c";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(6, 26);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(82, 15);
		this.label4.TabIndex = 3;
		this.label4.Text = "Item Category";
		this.cmbCGSGL.AllowNull = (DefaultableBoolean)1;
		this.cmbCGSGL.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCGSGL.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCGSGL.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Appearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance150;
		((SpecialBoxBase)((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance156).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCGSGL.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Location = new System.Drawing.Point(369, 78);
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Name = "cmbCGSGL";
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).TabIndex = 10;
		this.cmbBaseUnit.AllowNull = (DefaultableBoolean)1;
		this.cmbBaseUnit.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBaseUnit.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBaseUnit.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Appearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance196).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance197).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbBaseUnit.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Location = new System.Drawing.Point(91, 133);
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Name = "cmbBaseUnit";
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).TabIndex = 4;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(292, 26);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(76, 15);
		this.label10.TabIndex = 9;
		this.label10.Text = "Stock GL A/c";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(6, 168);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(60, 15);
		this.label5.TabIndex = 4;
		this.label5.Text = "Item Type";
		this.CmbItemGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbItemGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance216;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemGroup).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemGroup).Location = new System.Drawing.Point(91, 220);
		((System.Windows.Forms.Control)(object)this.CmbItemGroup).Name = "CmbItemGroup";
		((System.Windows.Forms.Control)(object)this.CmbItemGroup).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemGroup).TabIndex = 7;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(6, 197);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(61, 15);
		this.label6.TabIndex = 5;
		this.label6.Text = "Item Class";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(292, 139);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(54, 15);
		this.label8.TabIndex = 7;
		this.label8.Text = "Pack Qty";
		this.chkstatus.AutoSize = true;
		this.chkstatus.Checked = true;
		this.chkstatus.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkstatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chkstatus.Location = new System.Drawing.Point(518, 183);
		this.chkstatus.Name = "chkstatus";
		this.chkstatus.Size = new System.Drawing.Size(71, 19);
		this.chkstatus.TabIndex = 16;
		this.chkstatus.Text = "Is Active";
		this.chkstatus.UseVisualStyleBackColor = true;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(6, 111);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(72, 15);
		this.label37.TabIndex = 6;
		this.label37.Text = "Other Name";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(292, 168);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(53, 15);
		this.label33.TabIndex = 10;
		this.label33.Text = "Tax Type";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(292, 111);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(53, 15);
		this.label7.TabIndex = 6;
		this.label7.Text = "HS Code";
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.grdAllocation);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(870, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(245, 244);
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
		((System.Windows.Forms.Control)(object)this.grdAllocation).Size = new System.Drawing.Size(239, 222);
		((System.Windows.Forms.Control)(object)this.grdAllocation).TabIndex = 0;
		this.grdAllocation.TotalRow = (InheritableBoolean)1;
		this.grdAllocation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAllocation).VisualStyle = (VisualStyle)4;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label13);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 35);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1122, 24);
		this.panel2.TabIndex = 6;
		this.panel6.Controls.Add(this.toolStrip1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1122, 32);
		this.panel6.TabIndex = 5;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnnew, this.toolStripButton1, this.btnupdate, this.btnsave, this.btnprint, this.btnattachment });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1122, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.toolStripButton1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(80, 24);
		this.toolStripButton1.Text = "Refresh";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(BtnSave_Click);
		this.btnprint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnprint.Image = (System.Drawing.Image)resources.GetObject("btnprint.Image");
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(61, 24);
		this.btnprint.Text = "&Print";
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = (System.Drawing.Image)resources.GetObject("btnattachment.Image");
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.tpItemDefinitionAddItemHistory.Controls.Add(this.panel3);
		this.tpItemDefinitionAddItemHistory.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.tpItemDefinitionAddItemHistory.Location = new System.Drawing.Point(4, 4);
		this.tpItemDefinitionAddItemHistory.Name = "tpItemDefinitionAddItemHistory";
		this.tpItemDefinitionAddItemHistory.Size = new System.Drawing.Size(1128, 682);
		this.tpItemDefinitionAddItemHistory.TabIndex = 6;
		this.tpItemDefinitionAddItemHistory.Text = "History";
		this.tpItemDefinitionAddItemHistory.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Controls.Add(this.panel9);
		this.panel3.Controls.Add(this.panel10);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1128, 682);
		this.panel3.TabIndex = 0;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		this.grdhistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupTotals = (GroupTotals)2;
		this.grdhistory.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 148);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdhistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1128, 534);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 0;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label30);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 120);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1128, 28);
		this.panel4.TabIndex = 71;
		this.label30.AutoSize = true;
		this.label30.BackColor = System.Drawing.Color.Teal;
		this.label30.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.White;
		this.label30.Location = new System.Drawing.Point(12, 3);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(148, 19);
		this.label30.TabIndex = 0;
		this.label30.Text = "Add Item History";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1081, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdhistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(43, 26);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel9.BackColor = System.Drawing.Color.White;
		this.panel9.Controls.Add(this.grpUpdateItemName);
		this.panel9.Controls.Add(this.groupBox5);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 52);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1128, 68);
		this.panel9.TabIndex = 68;
		this.grpUpdateItemName.Controls.Add(this.label34);
		this.grpUpdateItemName.Controls.Add(this.btnUomUpdate);
		this.grpUpdateItemName.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.grpUpdateItemName.Location = new System.Drawing.Point(465, 3);
		this.grpUpdateItemName.Name = "grpUpdateItemName";
		this.grpUpdateItemName.Size = new System.Drawing.Size(657, 60);
		this.grpUpdateItemName.TabIndex = 1;
		this.grpUpdateItemName.TabStop = false;
		this.grpUpdateItemName.Text = "Update ItemName";
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label34.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label34.Location = new System.Drawing.Point(7, 18);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(541, 34);
		this.label34.TabIndex = 17;
		this.label34.Text = "When you change the name of an item in the grid, you should then check the corresponding row and click the 'Update Item Name' button. This action will update the item name in that specific row.";
		this.btnUomUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnUomUpdate.FlatAppearance.BorderSize = 0;
		this.btnUomUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUomUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUomUpdate.ForeColor = System.Drawing.Color.White;
		this.btnUomUpdate.Location = new System.Drawing.Point(554, 13);
		this.btnUomUpdate.Name = "btnUomUpdate";
		this.btnUomUpdate.Size = new System.Drawing.Size(97, 40);
		this.btnUomUpdate.TabIndex = 0;
		this.btnUomUpdate.TabStop = false;
		this.btnUomUpdate.Text = "Update Item Name";
		this.btnUomUpdate.UseVisualStyleBackColor = false;
		this.btnUomUpdate.Click += new System.EventHandler(btnUomUpdate_Click);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitemcathistory);
		this.groupBox5.Controls.Add(this.label32);
		this.groupBox5.Controls.Add(this.btnsearch);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(5, 3);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(456, 60);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Filter By Category";
		this.cmbitemcathistory.AllowNull = (DefaultableBoolean)1;
		this.cmbitemcathistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitemcathistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitemcathistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance203;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance209).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance210).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance210).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance210).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance211).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitemcathistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Location = new System.Drawing.Point(108, 24);
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Name = "cmbitemcathistory";
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Size = new System.Drawing.Size(268, 26);
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).TabIndex = 0;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(9, 28);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(82, 15);
		this.label32.TabIndex = 0;
		this.label32.Text = "Item Category";
		this.btnsearch.BackColor = System.Drawing.Color.Teal;
		this.btnsearch.FlatAppearance.BorderSize = 0;
		this.btnsearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnsearch.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsearch.ForeColor = System.Drawing.Color.White;
		this.btnsearch.Location = new System.Drawing.Point(381, 23);
		this.btnsearch.Name = "btnsearch";
		this.btnsearch.Size = new System.Drawing.Size(70, 26);
		this.btnsearch.TabIndex = 1;
		this.btnsearch.Text = "Search";
		this.btnsearch.UseVisualStyleBackColor = false;
		this.btnsearch.Click += new System.EventHandler(btnsearch_Click);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.toolStrip2);
		this.panel10.Controls.Add(this.label31);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1128, 52);
		this.panel10.TabIndex = 65;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1128, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnLoadAll.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(77, 24);
		this.btnLoadAll.Text = "&LoadAll";
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.White;
		this.label31.Location = new System.Drawing.Point(11, 28);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(111, 19);
		this.label31.TabIndex = 0;
		this.label31.Text = "Item History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1136, 711);
		base.Controls.Add(this.tabCAddItem);
		base.KeyPreview = true;
		base.Name = "DefineTaxItem";
		this.Text = "Add Item ";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvDefrmAddItem_FormClosing);
		base.Load += new System.EventHandler(InvDefrmAddItem_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvDefrmAddItem_KeyDown);
		this.tabCAddItem.ResumeLayout(false);
		this.tpItemDefinitionAddItem.ResumeLayout(false);
		this.panelDetails.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPurchaseUOM).EndInit();
		this.pnlimage.ResumeLayout(false);
		this.pnlimage.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picbox3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picbox4).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picbox2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picbox1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbManufacturerID).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAmountCalcType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcountry).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSaleUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCostUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBuyerOrSupplier).EndInit();
		this.panel1.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcropyear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemClass).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPurchaseGL).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTaxType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSaleGL).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCGSGL).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBaseUnit).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemGroup).EndInit();
		this.groupBox1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAllocation).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tpItemDefinitionAddItemHistory.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.grpUpdateItemName.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbitemcathistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
