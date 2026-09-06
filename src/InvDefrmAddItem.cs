using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.VPS;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Taxation;
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

public class InvDefrmAddItem : Form
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

	private bool IsPartyProcessingFeatureOn;

	private bool IsVpsAttachmentsServiceOn;

	private bool AutoCoaDefineByItemNameOnInsert;

	private bool SameAccountForStockAndRevenue;

	private bool ItemCodingEnable;

	private ServerCredentials Sc = new ServerCredentials();

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

	private CheckBox ChkIsThirdParty;

	private CheckBox ChkIsCompany;

	private Label label38;

	internal UltraCombo CmbMotherItem;

	private Button btnDefineBrands;

	internal UltraCombo CmbItemTypeHistory;

	private Label label39;

	internal UltraCombo CmbParentCategoryHistory;

	private Label label40;

	private RadioButton radBtnTypeAndCategory;

	private RadioButton radBtnCategoryAndType;

	private Label label41;

	public InvDefrmAddItem(UserAccount pUserAccount)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
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
			AutoCoaDefineByItemNameOnInsert = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("AutoCoaDefineByItemNameOnInsert"));
			SameAccountForStockAndRevenue = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SameAccountForStockAndRevenue"));
			IsVpsAttachmentsServiceOn = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("IsVpsAttachmentsServiceOn"));
			ItemCodingEnable = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemCodingEnable"));
			if (IsVpsAttachmentsServiceOn)
			{
				Sc = CommonServices.GetServerCredentials();
			}
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnprint.Enabled = formrights.DoHavePrintRights;
			btnupdate.Enabled = formrights.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			IsPartyProcessingFeatureOn = CommonServices.GetERPFeatureById(8);
			ChkIsCompany.Visible = IsPartyProcessingFeatureOn;
			ChkIsThirdParty.Visible = IsPartyProcessingFeatureOn;
			IfConfigAllAccountAllowOn = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("All Account Allow on Item and Item Category"));
			ParentCategoryBind();
			ItemTypeFill();
			getMotherItems();
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
			if (ItemCodingEnable)
			{
				label41.Visible = true;
				radBtnCategoryAndType.Visible = true;
				radBtnTypeAndCategory.Visible = true;
			}
			else
			{
				label41.Visible = false;
				radBtnCategoryAndType.Visible = false;
				radBtnTypeAndCategory.Visible = false;
			}
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
			cat.CategoryCode = "1,2,3,4,6,10,11";
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
			DataTable dtParent = new DataTable();
			dtParent.Columns.Add("Id", typeof(int));
			dtParent.Columns.Add("ParentCategory");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToInt(dt.Rows[i]["Id"]) == 1 || Conversion.ToInt(dt.Rows[i]["Id"]) == 2 || Conversion.ToInt(dt.Rows[i]["Id"]) == 3 || Conversion.ToInt(dt.Rows[i]["Id"]) == 4 || Conversion.ToInt(dt.Rows[i]["Id"]) == 6 || Conversion.ToInt(dt.Rows[i]["Id"]) == 10 || Conversion.ToInt(dt.Rows[i]["Id"]) == 11)
				{
					dtParent.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["InvParentCateDescription"]);
				}
			}
			DDL.BindDDL(dtParent, CmbParentCategoryHistory, "Id", "ParentCategory", "Parent Category", ZeroIndex: false);
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
			DataTable dt = CommonServices.GetItemTypeForComboServiceBind(0, "1,2,3,4,6,10,11");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbItemType, "Id", "TypeDescription", "Item Type", false);
				DDL.BindDDLNew(dt, CmbItemTypeHistory, "Id", "TypeDescription", "Item Type", false);
				((UltraGridBase)cmbItemType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbItemTypeHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void getMotherItems()
	{
		try
		{
			DataTable dt = ItemType.getMotherItems(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbMotherItem, "Id", "MotherItem", "Mother Items", ZeroIndex: false);
				((UltraGridBase)CmbMotherItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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

	private void PurchaseGLAcFill1()
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
				for (int j = 0; j < Accountslst.Count; j++)
				{
					if (Accountslst[j].AccountTypeId == 4)
					{
						dt.Rows.Add(Accountslst[j].Id, Accountslst[j].AccountTitle);
					}
					if (Accountslst[j].AccountTypeId == 10)
					{
						dtsale.Rows.Add(Accountslst[j].Id, Accountslst[j].AccountTitle);
					}
				}
				if (AutoCoaDefineByItemNameOnInsert && SameAccountForStockAndRevenue)
				{
					DDL.BindDDL(dt, cmbPurchaseGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: false);
					DDL.BindDDL(dt, cmbSaleGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: false);
				}
				else
				{
					if (dt.Rows.Count > 0)
					{
						DDL.BindDDL(dt, cmbPurchaseGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: false);
					}
					if (dtsale.Rows.Count > 0)
					{
						DDL.BindDDL(dtsale, cmbSaleGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: false);
					}
				}
			}
			else
			{
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("Id");
				dt2.Columns.Add("AccountTitle");
				for (int k = 0; k < Accountslst.Count; k++)
				{
					if (AutoCoaDefineByItemNameOnInsert && SameAccountForStockAndRevenue)
					{
						if (Accountslst[k].AccountTypeId == 4)
						{
							dt2.Rows.Add(Accountslst[k].Id, Accountslst[k].AccountTitle);
						}
					}
					else if (Accountslst[k].AccountTypeId == 4 || Accountslst[k].AccountTypeId == 10)
					{
						dt2.Rows.Add(Accountslst[k].Id, Accountslst[k].AccountTitle);
					}
				}
				DDL.BindDDL(dt2, cmbPurchaseGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: false);
				DDL.BindDDL(dt2, cmbSaleGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: false);
			}
			DataTable dtcgs = new DataTable();
			dtcgs.Columns.Add("Id");
			dtcgs.Columns.Add("AccountTitle");
			for (int i = 0; i < Accountslst.Count; i++)
			{
				if (Accountslst[i].AccountTypeId == 12)
				{
					dtcgs.Rows.Add(Accountslst[i].Id, Accountslst[i].AccountTitle);
				}
			}
			if (dtcgs.Rows.Count > 0)
			{
				DDL.BindDDL(dtcgs, cmbCGSGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: false);
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
			DataTable dtPurchaseSale = CreateAccountDataTable();
			DataTable dtPurchase = CreateAccountDataTable();
			DataTable dtSale = CreateAccountDataTable();
			DataTable dtCGS = CreateAccountDataTable();
			foreach (COAAllocation account in Accountslst)
			{
				switch (account.AccountTypeId)
				{
				case 4:
					dtPurchase.Rows.Add(account.Id, account.AccountTitle, account.AccountTypeId);
					dtPurchaseSale.Rows.Add(account.Id, account.AccountTitle, account.AccountTypeId);
					break;
				case 10:
					dtSale.Rows.Add(account.Id, account.AccountTitle, account.AccountTypeId);
					dtPurchaseSale.Rows.Add(account.Id, account.AccountTitle, account.AccountTypeId);
					break;
				case 12:
					dtCGS.Rows.Add(account.Id, account.AccountTitle, account.AccountTypeId);
					break;
				}
			}
			if (!IfConfigAllAccountAllowOn)
			{
				if (AutoCoaDefineByItemNameOnInsert && SameAccountForStockAndRevenue)
				{
					DDL.BindDDL(dtPurchase, cmbPurchaseGL, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
					DDL.BindDDL(dtPurchase, cmbSaleGL, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
					((UltraGridBase)cmbPurchaseGL).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)cmbSaleGL).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
				else
				{
					if (dtPurchase.Rows.Count > 0)
					{
						DDL.BindDDL(dtPurchase, cmbPurchaseGL, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
					}
					((UltraGridBase)cmbPurchaseGL).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					if (dtSale.Rows.Count > 0)
					{
						DDL.BindDDL(dtSale, cmbSaleGL, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
					}
					((UltraGridBase)cmbSaleGL).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
			}
			else if (AutoCoaDefineByItemNameOnInsert && SameAccountForStockAndRevenue)
			{
				if (dtPurchase.Rows.Count > 0)
				{
					DDL.BindDDL(dtPurchase, cmbPurchaseGL, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				}
				DDL.BindDDL(dtPurchase, cmbSaleGL, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				((UltraGridBase)cmbPurchaseGL).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbSaleGL).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
			else
			{
				if (dtPurchase.Rows.Count > 0)
				{
					DDL.BindDDL(dtPurchaseSale, cmbPurchaseGL, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
					((UltraGridBase)cmbPurchaseGL).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
				if (dtPurchaseSale.Rows.Count > 0)
				{
					DDL.BindDDL(dtPurchaseSale, cmbSaleGL, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
					((UltraGridBase)cmbSaleGL).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
			}
			if (dtCGS.Rows.Count > 0)
			{
				DDL.BindDDL(dtCGS, cmbCGSGL, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
			}
			((UltraGridBase)cmbCGSGL).DisplayLayout.Bands[0].Columns[2].Hidden = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable CreateAccountDataTable()
	{
		return new DataTable
		{
			Columns = { "Id", "AccountTitle", "AccountTypeId" }
		};
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
			DataTable dt = global::Architecture.BLL.Inventory.TaxesTypes.Getall(new global::Architecture.Model.Inventory.TaxesTypes
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
		try
		{
			if (IsSaveButtonActive())
			{
				Item item = GetItemFromUI();
				GenerateItemCode(item);
				SetGLAccounts(item);
				txtPackQuantity.Text = "0";
				((UltraGridBase)cmbItemClass).Rows[0].Activate();
				CreateItemName();
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private void cmbItemType_Leave(object sender, EventArgs e)
	{
		try
		{
			Item item = GetItemFromUI();
			GenerateItemCode(item);
			if (IsSaveButtonActive())
			{
				CreateItemName();
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
				txtItemCodeNew.Text = lst.Rows[0]["ItemCodeNew"].ToString();
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
				if (!AutoCoaDefineByItemNameOnInsert)
				{
					cmbPurchaseGL.Value = row["InventoryAccountId"].ToString();
					cmbSaleGL.Value = row["RevenueAccountId"].ToString();
				}
				cmbCGSGL.Value = row["CGSAccountId"].ToString();
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private void CreateItemName()
	{
		try
		{
			if (!IsSaveButtonActive() || !ItemCodingEnable)
			{
				return;
			}
			string categoryName = Conversion.ToString((object)((Control)(object)cmbItemCategory).Text);
			string typeName = Conversion.ToString((object)((Control)(object)cmbItemType).Text);
			if (!string.IsNullOrWhiteSpace(categoryName) && !string.IsNullOrWhiteSpace(typeName))
			{
				if (radBtnCategoryAndType.Checked)
				{
					txtItemName.Text = categoryName + " " + typeName;
				}
				else if (radBtnTypeAndCategory.Checked)
				{
					txtItemName.Text = typeName + " " + categoryName;
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
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
		if (IsPartyProcessingFeatureOn && !ChkIsCompany.Checked && !ChkIsThirdParty.Checked)
		{
			MessageBox.Show("Please Check of the Check box ('Is Company' Or 'Is third Party') ");
			ChkIsCompany.Focus();
			return false;
		}
		if (!btnsave.Visible || !btnsave.Enabled || !AutoCoaDefineByItemNameOnInsert)
		{
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
		}
		if (((UltraGridBase)cmbCGSGL).ActiveRow == null)
		{
			MessageBox.Show("COGS GL A/c Filed is Required");
			cmbCGSGL.Focus();
			return false;
		}
		if (((UltraGridBase)CmbMotherItem).ActiveRow == null || Conversion.ToInt(CmbMotherItem.Value) == 0)
		{
			MessageBox.Show("Mother Item Filed is Required");
			CmbMotherItem.Focus();
			return false;
		}
		return true;
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
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Expected O, but got Unknown
		//IL_0625: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Expected O, but got Unknown
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Expected O, but got Unknown
		if (!FormValidationForAddItem())
		{
			return;
		}
		try
		{
			List<(string, int)> ImgList = new List<(string, int)>();
			if (fileSavePath != null && fileSavePath != string.Empty)
			{
				ImgList.Add((fileSavePath, 1));
			}
			if (fileSavePath2 != null && fileSavePath2 != string.Empty)
			{
				ImgList.Add((fileSavePath2, 2));
			}
			if (fileSavePath3 != null && fileSavePath3 != string.Empty)
			{
				ImgList.Add((fileSavePath3, 3));
			}
			if (fileSavePath4 != null && fileSavePath4 != string.Empty)
			{
				ImgList.Add((fileSavePath4, 4));
			}
			if (((UltraGridBase)cmbPurchaseGL).ActiveRow != null && Conversion.ToInt(cmbPurchaseGL.Value) > 0 && ((UltraGridBase)cmbSaleGL).ActiveRow != null && Conversion.ToInt(cmbSaleGL.Value) > 0)
			{
				int num = Conversion.ToInt(((UltraDropDownBase)cmbPurchaseGL).SelectedRow.Cells["AccountTypeId"].Value);
				int InventorytypeId = Conversion.ToInt(((UltraDropDownBase)cmbSaleGL).SelectedRow.Cells["AccountTypeId"].Value);
				if (num == 4 && InventorytypeId == 4 && Conversion.ToInt(cmbPurchaseGL.Value) != Conversion.ToInt(cmbSaleGL.Value))
				{
					cmbSaleGL.Focus();
					throw new Exception("Sale Account Should be Same as Purchase Account or Select an Account Of Type Sale In Sale Account");
				}
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
			items.IsTaxable = false;
			items.Pic1 = Conversion.ToString((object)Path.GetFileName(fileSavePath));
			items.Pic2 = Conversion.ToString((object)Path.GetFileName(fileSavePath2));
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
			items.MotherItemId = Conversion.ToInt(CmbMotherItem.Value);
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
			items.CommOnSale = chkCommissionOnSale.Checked;
			items.CommOnPurchase = chkCommissionOnPurchase.Checked;
			items.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
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
				img.EffetedDate = EffectedDate.Value;
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
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Expected O, but got Unknown
		//IL_0874: Unknown result type (might be due to invalid IL or missing references)
		//IL_087b: Expected O, but got Unknown
		//IL_094c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0953: Expected O, but got Unknown
		if (!FormValidationForAddItem())
		{
			return;
		}
		try
		{
			if (((UltraGridBase)cmbPurchaseGL).ActiveRow != null && Conversion.ToInt(cmbPurchaseGL.Value) > 0 && ((UltraGridBase)cmbSaleGL).ActiveRow != null && Conversion.ToInt(cmbSaleGL.Value) > 0)
			{
				int num = Conversion.ToInt(((UltraDropDownBase)cmbPurchaseGL).SelectedRow.Cells["AccountTypeId"].Value);
				int InventorytypeId = Conversion.ToInt(((UltraDropDownBase)cmbSaleGL).SelectedRow.Cells["AccountTypeId"].Value);
				if (num == 4 && InventorytypeId == 4 && Conversion.ToInt(cmbPurchaseGL.Value) != Conversion.ToInt(cmbSaleGL.Value))
				{
					cmbSaleGL.Focus();
					throw new Exception("Sale Account Should be Same as Purchase Account or Select an Account Of Type Sale In Sale Account");
				}
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
			if (fileSavePath3 != null && fileSavePath3 != string.Empty)
			{
				ImgList.Add((fileSavePath3, Conversion.ToString(picbox3.Tag), 3));
			}
			if (fileSavePath4 != null && fileSavePath4 != string.Empty)
			{
				ImgList.Add((fileSavePath4, Conversion.ToString(picbox4.Tag), 4));
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
			if (Conversion.ToString(picbox3.Tag) != null && Conversion.ToString(picbox3.Tag) != string.Empty && picbox3.Image == null)
			{
				DeleteImgList.Add(Conversion.ToString(picbox3.Tag));
			}
			if (Conversion.ToString(picbox4.Tag) != null && Conversion.ToString(picbox4.Tag) != string.Empty && picbox4.Image == null)
			{
				DeleteImgList.Add(Conversion.ToString(picbox4.Tag));
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
			items.IsTaxable = false;
			items.Pic1 = ((fileSavePath != null && fileSavePath != string.Empty && Conversion.ToString(picbox1.Tag) != string.Empty) ? picbox1.AccessibleName : Conversion.ToString((object)Path.GetFileName(fileSavePath)));
			items.Pic2 = ((fileSavePath2 != null && fileSavePath2 != string.Empty && Conversion.ToString(picbox2.Tag) != string.Empty) ? picbox2.AccessibleName : Conversion.ToString((object)Path.GetFileName(fileSavePath2)));
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
			items.MotherItemId = Conversion.ToInt(CmbMotherItem.Value);
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
			items.ItemImageslist = new List<ItemImage>();
			items.ItemImagesDeletelist = new List<ItemImage>();
			items.ItemAllocationlist = new List<ItemAllocation>();
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
				img2.EffetedDate = EffectedDate.Value;
				items.ItemImageslist.Add(img2);
			}
			items.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			items.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<Item>(items, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			if (DeleteImgList != null && DeleteImgList.Count > 0)
			{
				foreach (string item in DeleteImgList)
				{
					ItemImage img = new ItemImage();
					img.ImagePath = item;
					img.EffetedDate = EffectedDate.Value;
					items.ItemImagesDeletelist.Add(img);
				}
			}
			if (Item.Save(items, true) >= 1)
			{
				MessageBox.Show("Update Successfully");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 0, base.Name);
			}
			grdfrmfill();
			refresh();
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
			Reset();
			RecId = Id;
			Item items = Item.GetByID(Id);
			if (items == null)
			{
				MessageBox.Show("Record Not Found");
			}
			else
			{
				EditRecord(items);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadByCode(string ItemCode)
	{
		try
		{
			Item items = Item.GetByItemCode(ItemCode);
			if (items != null)
			{
				Reset();
				RecId = items.Id;
				EditRecord(items);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EditRecord(Item items)
	{
		try
		{
			panelDetails.Visible = true;
			btnsave.Visible = false;
			btnupdate.Visible = true;
			if (((IEnumerable<UltraGridRow>)((UltraGridBase)CmbItemGroup).Rows).Count() > 0)
			{
				((UltraGridBase)CmbItemGroup).Rows[0].Activate();
			}
			tabCAddItem.SelectedIndex = 0;
			if (IsPartyProcessingFeatureOn)
			{
				ChkIsThirdParty.Checked = items.IsThirdParty;
				ChkIsCompany.Checked = items.IsCompany;
			}
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
			CmbMotherItem.Value = Conversion.ToInt((object)items.MotherItemId);
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
				else if (items.ItemImageslist[i].SortNo == 2)
				{
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
				else if (items.ItemImageslist[i].SortNo == 3)
				{
					fileSavePath3 = items.ItemImageslist[i].FileName;
					if (!IsVpsAttachmentsServiceOn)
					{
						string path3 = Path.Combine(clsGlobalVariables.AttachmentFolderPath, items.ItemImageslist[i].ImagePath);
						if (File.Exists(path3))
						{
							using MemoryStream memoryStream2 = new MemoryStream();
							Image image3 = Image.FromFile(path3);
							Bitmap bmp3 = new Bitmap(image3);
							bmp3.Save(memoryStream2, image3.RawFormat);
							picbox3.Image = bmp3;
							picbox3.Tag = fileSavePath3;
							picbox3.AccessibleName = items.ItemImageslist[i].FileName;
							image3.Dispose();
							memoryStream2.Position = 0L;
							memoryStream2.Dispose();
						}
					}
					else
					{
						DownloadItemImageFromVpsInUpdateCase(items.ItemImageslist[i].ImagePath, items.ItemImageslist[i].FileName, picbox3);
					}
				}
				else
				{
					if (items.ItemImageslist[i].SortNo != 4)
					{
						continue;
					}
					fileSavePath4 = items.ItemImageslist[i].FileName;
					if (!IsVpsAttachmentsServiceOn)
					{
						string path4 = Path.Combine(clsGlobalVariables.AttachmentFolderPath, items.ItemImageslist[i].ImagePath);
						if (File.Exists(path4))
						{
							using MemoryStream memoryStream3 = new MemoryStream();
							Image image4 = Image.FromFile(path4);
							Bitmap bmp4 = new Bitmap(image4);
							bmp4.Save(memoryStream3, image4.RawFormat);
							picbox4.Image = bmp4;
							picbox4.Tag = fileSavePath4;
							picbox4.AccessibleName = items.ItemImageslist[i].FileName;
							image4.Dispose();
							memoryStream3.Position = 0L;
							memoryStream3.Dispose();
						}
					}
					else
					{
						DownloadItemImageFromVpsInUpdateCase(items.ItemImageslist[i].ImagePath, items.ItemImageslist[i].FileName, picbox4);
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtItemCodeNew_TextChanged(object sender, EventArgs e)
	{
		ReadByCode(txtItemCodeNew.Text);
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

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
		refresh();
		cmbItemCategory_Leave(null, null);
	}

	private void Reset()
	{
		try
		{
			((Control)(object)cmbBaseUnit).Text = string.Empty;
			((Control)(object)cmbItemType).Text = string.Empty;
			chkCommissionOnPurchase.Checked = true;
			chkCommissionOnPurchase.Visible = true;
			chkCommissionOnSale.Checked = true;
			chkCommissionOnSale.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
			panelDetails.Hide();
			BtnShowDetails.Text = "+";
			fileSavePath = "";
			fileSavePath2 = "";
			fileSavePath3 = "";
			fileSavePath4 = "";
			picbox1.Image = null;
			picbox1.Tag = "";
			picbox2.Image = null;
			picbox2.Tag = "";
			picbox3.Image = null;
			picbox3.Tag = "";
			picbox4.Image = null;
			picbox4.Tag = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsearch_Click(object sender, EventArgs e)
	{
		grdfrmfill();
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdhistory.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				int CurrId = Conversion.ToInt((object)row.Cells["Id"].Value.ToString());
				ReadById(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrmfill(int NoOfRecords = 0)
	{
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
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
			dt.Columns.Add("MotherItem", typeof(string));
			dt.Columns.Add("IsCompany", typeof(string));
			dt.Columns.Add("IsThirdParty", typeof(string));
			dt.Columns.Add("EntryDate", typeof(DateTime));
			dt.Columns.Add("EntryUserName", typeof(string));
			dt.Columns.Add("NoOfAttachments", typeof(int));
			ReportsParameters reports = new ReportsParameters();
			reports.OrganizationId = UserAccount.OrganizationId;
			reports.CompanyId = UserAccount.CompanyId;
			reports.NoOfRecords = NoOfRecords;
			if (Conversion.ToInt(cmbitemcathistory.Value) != 0)
			{
				reports.ItemCategoryId = Convert.ToInt32(cmbitemcathistory.Value);
			}
			reports.ScreenName = base.Name;
			reports.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!reports.CanViewAllRecord)
			{
				reports.EntryUser = UserAccount.ID;
			}
			reports.IsTaxable = false;
			if (Conversion.ToInt(CmbParentCategoryHistory.Value) > 0)
			{
				reports.InventoryParentCategories = Conversion.ToInt(CmbParentCategoryHistory.Value);
			}
			else
			{
				reports.ParentCategoryIds = "1,2,3,4,6,10,11";
			}
			reports.ItemTypeId = Conversion.ToInt(CmbItemTypeHistory.Value);
			DataTable grdlst = Item.FormHistory(reports);
			if (grdlst.Rows.Count > 0)
			{
				for (int i = 0; i < grdlst.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(grdlst.Rows[i]["Id"]), Conversion.ToString(grdlst.Rows[i]["ItemCode"]), Conversion.ToString(grdlst.Rows[i]["ItemCodeNew"]), Conversion.ToString(grdlst.Rows[i]["ItemName"]), Conversion.ToString(grdlst.Rows[i]["ItemNameOtherLingo"]), Conversion.ToString(grdlst.Rows[i]["TypeDescription"]), Conversion.ToString(grdlst.Rows[i]["CategoryDescription"]), Conversion.ToString(grdlst.Rows[i]["InvParentCateDescription"]), Conversion.ToString(grdlst.Rows[i]["ClassGroupName"]), Conversion.ToString(grdlst.Rows[i]["ItemStatus"]), Conversion.ToString(grdlst.Rows[i]["GlStockAccountTitle"]), Conversion.ToString(grdlst.Rows[i]["GlSaleAccountTitle"]), Conversion.ToString(grdlst.Rows[i]["GlCgsAccountTitle"]), Conversion.ToString(grdlst.Rows[i]["ItemWithVarient"]), Conversion.ToString(grdlst.Rows[i]["MotherItem"]), Conversion.ToString(grdlst.Rows[i]["IsCompany"]), Conversion.ToString(grdlst.Rows[i]["IsThirdParty"]), grdlst.Rows[i]["EntryDate"], Conversion.ToString(grdlst.Rows[i]["EntryUserName"]), Conversion.ToInt(grdlst.Rows[i]["NoOfAttachments"]));
				}
				grdhistory.DataSource = dt;
				grdhistory.RetrieveStructure();
				gridsetting();
			}
			else
			{
				grdhistory.DataSource = null;
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
			grdhistory.RootTable.Columns["IsThirdParty"].Visible = IsPartyProcessingFeatureOn;
			grdhistory.RootTable.Columns["IsCompany"].Visible = IsPartyProcessingFeatureOn;
			grdhistory.AllowEdit = (InheritableBoolean)1;
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["ItemCodeNew"].Caption = "Item Code ";
			grdhistory.RootTable.Columns["ItemCode"].Visible = false;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
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
			GridEX_Helper.GridWrappingAndColumnSettings(grdhistory, 2);
			grdhistory.RootTable.Columns["ItemCodeNew"].Width = Constants.InventoryConstants.ItemCode;
			grdhistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grdhistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdhistory.RootTable.Columns["ItemNameOtherLingo"].Width = Constants.InventoryConstants.OtherDescription;
			grdhistory.RootTable.Columns["TypeDescription"].Width = Constants.InventoryConstants.ItemType;
			grdhistory.RootTable.Columns["CategoryDescription"].Width = Constants.InventoryConstants.CategoryDescription;
			grdhistory.RootTable.Columns["ParentCategory"].Width = 100;
			grdhistory.RootTable.Columns["ClassGroupName"].Width = Constants.InventoryConstants.ItemClass;
			grdhistory.RootTable.Columns["ItemStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdhistory.RootTable.Columns["StockAc"].Width = Constants.InventoryConstants.AccountTitle;
			grdhistory.RootTable.Columns["SaleAc"].Width = Constants.InventoryConstants.AccountTitle;
			grdhistory.RootTable.Columns["CgsAc"].Width = Constants.InventoryConstants.AccountTitle;
			grdhistory.RootTable.Columns["ItemWithVarient"].Width = Constants.InventoryConstants.Variant;
			grdhistory.RootTable.Columns["MotherItem"].Width = Constants.InventoryConstants.MotherItem;
			grdhistory.RootTable.Columns["IsCompany"].Width = 65;
			grdhistory.RootTable.Columns["IsThirdParty"].Width = 65;
			grdhistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["EntryUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grdhistory.RootTable.Columns["Select"].Width = 30;
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
		if (e.Control && e.KeyCode == Keys.Up && tabCAddItem.SelectedIndex == 1)
		{
			CmbParentCategoryHistory.Focus();
		}
		if (e.Control && e.KeyCode == Keys.Down && tabCAddItem.SelectedIndex == 1)
		{
			((Control)(object)grdhistory).Focus();
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

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			IsVpsAttachmentsServiceOn = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("IsVpsAttachmentsServiceOn"));
			AutoCoaDefineByItemNameOnInsert = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("AutoCoaDefineByItemNameOnInsert"));
			SameAccountForStockAndRevenue = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("SameAccountForStockAndRevenue"));
			ItemTypeFill();
			getMotherItems();
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

	private void tabCAddItem_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabCAddItem.SelectedIndex == 1)
		{
			int num = grdhistory.GetRows().Count();
			CmbParentCategoryHistory.Focus();
			if (num == 0)
			{
				grdfrmfill(50);
			}
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
		try
		{
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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

	private void btnDefineBrands_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineBrand(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void radBtnCategoryAndType_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			CreateItemName();
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
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Expected O, but got Unknown
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Expected O, but got Unknown
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Expected O, but got Unknown
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Expected O, but got Unknown
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Expected O, but got Unknown
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Expected O, but got Unknown
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Expected O, but got Unknown
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Expected O, but got Unknown
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Expected O, but got Unknown
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Expected O, but got Unknown
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Expected O, but got Unknown
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Expected O, but got Unknown
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Expected O, but got Unknown
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Expected O, but got Unknown
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Expected O, but got Unknown
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Expected O, but got Unknown
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Expected O, but got Unknown
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Expected O, but got Unknown
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Expected O, but got Unknown
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Expected O, but got Unknown
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Expected O, but got Unknown
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Expected O, but got Unknown
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Expected O, but got Unknown
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Expected O, but got Unknown
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Expected O, but got Unknown
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Expected O, but got Unknown
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Expected O, but got Unknown
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Expected O, but got Unknown
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Expected O, but got Unknown
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Expected O, but got Unknown
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Expected O, but got Unknown
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Expected O, but got Unknown
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Expected O, but got Unknown
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Expected O, but got Unknown
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Expected O, but got Unknown
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Expected O, but got Unknown
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Expected O, but got Unknown
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Expected O, but got Unknown
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Expected O, but got Unknown
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Expected O, but got Unknown
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Expected O, but got Unknown
		//IL_0760: Unknown result type (might be due to invalid IL or missing references)
		//IL_076a: Expected O, but got Unknown
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0846: Expected O, but got Unknown
		//IL_0852: Unknown result type (might be due to invalid IL or missing references)
		//IL_085c: Expected O, but got Unknown
		//IL_0873: Unknown result type (might be due to invalid IL or missing references)
		//IL_087d: Expected O, but got Unknown
		//IL_089f: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a9: Expected O, but got Unknown
		//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d5: Expected O, but got Unknown
		//IL_08e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08eb: Expected O, but got Unknown
		//IL_0a15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1f: Expected O, but got Unknown
		//IL_0a2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a35: Expected O, but got Unknown
		//IL_0a36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a40: Expected O, but got Unknown
		//IL_0a41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4b: Expected O, but got Unknown
		//IL_0a4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a56: Expected O, but got Unknown
		//IL_0aaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab9: Expected O, but got Unknown
		//IL_0b12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1c: Expected O, but got Unknown
		//IL_0b49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b53: Expected O, but got Unknown
		//IL_0b6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b74: Expected O, but got Unknown
		//IL_0b8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b95: Expected O, but got Unknown
		//IL_0b96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba0: Expected O, but got Unknown
		//IL_0bee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf8: Expected O, but got Unknown
		//IL_0c72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7c: Expected O, but got Unknown
		//IL_0cd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdf: Expected O, but got Unknown
		//IL_0cf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d00: Expected O, but got Unknown
		//IL_0d0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d16: Expected O, but got Unknown
		//IL_a3be: Unknown result type (might be due to invalid IL or missing references)
		//IL_a3c8: Expected O, but got Unknown
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
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance210 = new Appearance();
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
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
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
		Appearance appearance168 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Definition.InvDefrmAddItem));
		Appearance appearance145 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
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
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.btnDefineBrands = new System.Windows.Forms.Button();
		this.btnItemAllocateToManufacturer = new System.Windows.Forms.Button();
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
		this.txtPackQuantity = new System.Windows.Forms.TextBox();
		this.combcropyear = new UltraCombo();
		this.txtHSCode = new System.Windows.Forms.TextBox();
		this.cmbPurchaseGL = new UltraCombo();
		this.cmbTaxType = new UltraCombo();
		this.cmbSaleGL = new UltraCombo();
		this.cmbCGSGL = new UltraCombo();
		this.rdWithoutVariant = new System.Windows.Forms.RadioButton();
		this.rdWithVariant = new System.Windows.Forms.RadioButton();
		this.ChkIsThirdParty = new System.Windows.Forms.CheckBox();
		this.ChkIsCompany = new System.Windows.Forms.CheckBox();
		this.chkCommissionOnPurchase = new System.Windows.Forms.CheckBox();
		this.chkCommissionOnSale = new System.Windows.Forms.CheckBox();
		this.txtItemCodeNew = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.cmbItemCategory = new UltraCombo();
		this.BtnShowDetails = new System.Windows.Forms.Button();
		this.txtOtherLingo = new System.Windows.Forms.TextBox();
		this.lblCropYear = new System.Windows.Forms.Label();
		this.chbGST = new System.Windows.Forms.CheckBox();
		this.txtItemCode = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbItemClass = new UltraCombo();
		this.txtItemName = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbItemType = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbBaseUnit = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbMotherItem = new UltraCombo();
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
		this.CmbParentCategoryHistory = new UltraCombo();
		this.label40 = new System.Windows.Forms.Label();
		this.btnsearch = new System.Windows.Forms.Button();
		this.CmbItemTypeHistory = new UltraCombo();
		this.label39 = new System.Windows.Forms.Label();
		this.cmbitemcathistory = new UltraCombo();
		this.label32 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.label31 = new System.Windows.Forms.Label();
		this.radBtnCategoryAndType = new System.Windows.Forms.RadioButton();
		this.radBtnTypeAndCategory = new System.Windows.Forms.RadioButton();
		this.label41 = new System.Windows.Forms.Label();
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
		((System.ComponentModel.ISupportInitialize)this.combcropyear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPurchaseGL).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTaxType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSaleGL).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCGSGL).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemClass).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBaseUnit).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbMotherItem).BeginInit();
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
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategoryHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemTypeHistory).BeginInit();
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
		this.tabCAddItem.Size = new System.Drawing.Size(1311, 665);
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
		this.tpItemDefinitionAddItem.Size = new System.Drawing.Size(1303, 636);
		this.tpItemDefinitionAddItem.TabIndex = 0;
		this.tpItemDefinitionAddItem.Text = "Add Item";
		this.panelDetails.Controls.Add(this.groupBox4);
		this.panelDetails.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelDetails.Location = new System.Drawing.Point(3, 393);
		this.panelDetails.Name = "panelDetails";
		this.panelDetails.Size = new System.Drawing.Size(1297, 240);
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
		this.groupBox4.Location = new System.Drawing.Point(5, 5);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1110, 266);
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
		this.panel1.Controls.Add(this.groupBox3);
		this.panel1.Controls.Add(this.groupBox2);
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 59);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1297, 334);
		this.panel1.TabIndex = 0;
		this.groupBox3.Controls.Add(this.btnDefineBrands);
		this.groupBox3.Controls.Add(this.btnItemAllocateToManufacturer);
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
		this.groupBox3.Size = new System.Drawing.Size(263, 327);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Related Defination";
		this.btnDefineBrands.BackColor = System.Drawing.Color.Teal;
		this.btnDefineBrands.FlatAppearance.BorderSize = 0;
		this.btnDefineBrands.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDefineBrands.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDefineBrands.ForeColor = System.Drawing.Color.White;
		this.btnDefineBrands.Location = new System.Drawing.Point(6, 296);
		this.btnDefineBrands.Name = "btnDefineBrands";
		this.btnDefineBrands.Size = new System.Drawing.Size(253, 27);
		this.btnDefineBrands.TabIndex = 16;
		this.btnDefineBrands.Text = "Define Brands";
		this.btnDefineBrands.UseVisualStyleBackColor = false;
		this.btnDefineBrands.Click += new System.EventHandler(btnDefineBrands_Click);
		this.btnItemAllocateToManufacturer.BackColor = System.Drawing.Color.Teal;
		this.btnItemAllocateToManufacturer.FlatAppearance.BorderSize = 0;
		this.btnItemAllocateToManufacturer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnItemAllocateToManufacturer.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnItemAllocateToManufacturer.ForeColor = System.Drawing.Color.White;
		this.btnItemAllocateToManufacturer.Location = new System.Drawing.Point(6, 267);
		this.btnItemAllocateToManufacturer.Name = "btnItemAllocateToManufacturer";
		this.btnItemAllocateToManufacturer.Size = new System.Drawing.Size(253, 27);
		this.btnItemAllocateToManufacturer.TabIndex = 16;
		this.btnItemAllocateToManufacturer.Text = "Item Allocate To Manufacturer";
		this.btnItemAllocateToManufacturer.UseVisualStyleBackColor = false;
		this.btnItemAllocateToManufacturer.Click += new System.EventHandler(btnItemAllocateToManufacturer_Click);
		this.btnItemMultiLanguage.BackColor = System.Drawing.Color.Teal;
		this.btnItemMultiLanguage.FlatAppearance.BorderSize = 0;
		this.btnItemMultiLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnItemMultiLanguage.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnItemMultiLanguage.ForeColor = System.Drawing.Color.White;
		this.btnItemMultiLanguage.Location = new System.Drawing.Point(6, 237);
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
		this.btnItemAllocation.Location = new System.Drawing.Point(6, 207);
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
		this.button1.Location = new System.Drawing.Point(98, 163);
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
		this.btnitmcat.Location = new System.Drawing.Point(6, 24);
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
		this.btnUomGroupAssignToItem.Location = new System.Drawing.Point(98, 116);
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
		this.btnitmtype.Location = new System.Drawing.Point(98, 24);
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
		this.btnItemGroupSchedule.Location = new System.Drawing.Point(98, 70);
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
		this.btnitmtaxsch.Location = new System.Drawing.Point(6, 163);
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
		this.btnitmuom.Location = new System.Drawing.Point(153, 24);
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
		this.btnitmuomtaxsch.Location = new System.Drawing.Point(6, 116);
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
		this.btnItemGroup.Location = new System.Drawing.Point(6, 70);
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
		this.btnCropYear.Location = new System.Drawing.Point(208, 24);
		this.btnCropYear.Name = "btnCropYear";
		this.btnCropYear.Size = new System.Drawing.Size(51, 41);
		this.btnCropYear.TabIndex = 3;
		this.btnCropYear.Text = "Crop Year";
		this.btnCropYear.UseVisualStyleBackColor = false;
		this.btnCropYear.Click += new System.EventHandler(btnCropYear_Click);
		this.groupBox2.Controls.Add(this.txtPackQuantity);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combcropyear);
		this.groupBox2.Controls.Add(this.txtHSCode);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPurchaseGL);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTaxType);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSaleGL);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCGSGL);
		this.groupBox2.Controls.Add(this.rdWithoutVariant);
		this.groupBox2.Controls.Add(this.rdWithVariant);
		this.groupBox2.Controls.Add(this.ChkIsThirdParty);
		this.groupBox2.Controls.Add(this.ChkIsCompany);
		this.groupBox2.Controls.Add(this.chkCommissionOnPurchase);
		this.groupBox2.Controls.Add(this.chkCommissionOnSale);
		this.groupBox2.Controls.Add(this.txtItemCodeNew);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCategory);
		this.groupBox2.Controls.Add(this.BtnShowDetails);
		this.groupBox2.Controls.Add(this.txtOtherLingo);
		this.groupBox2.Controls.Add(this.lblCropYear);
		this.groupBox2.Controls.Add(this.chbGST);
		this.groupBox2.Controls.Add(this.txtItemCode);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.label38);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemClass);
		this.groupBox2.Controls.Add(this.txtItemName);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemType);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBaseUnit);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMotherItem);
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
		this.groupBox2.Size = new System.Drawing.Size(593, 324);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.txtPackQuantity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackQuantity.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPackQuantity.Location = new System.Drawing.Point(361, 135);
		this.txtPackQuantity.Name = "txtPackQuantity";
		this.txtPackQuantity.Size = new System.Drawing.Size(220, 23);
		this.txtPackQuantity.TabIndex = 13;
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
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcropyear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance176;
		((SpecialBoxBase)((UltraGridBase)this.combcropyear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcropyear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.combcropyear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcropyear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance231).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance231).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance242).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance242;
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
		((System.Windows.Forms.Control)(object)this.combcropyear).Location = new System.Drawing.Point(361, 191);
		((System.Windows.Forms.Control)(object)this.combcropyear).Name = "combcropyear";
		((System.Windows.Forms.Control)(object)this.combcropyear).Size = new System.Drawing.Size(124, 26);
		((System.Windows.Forms.Control)(object)this.combcropyear).TabIndex = 16;
		this.txtHSCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtHSCode.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtHSCode.Location = new System.Drawing.Point(361, 107);
		this.txtHSCode.Name = "txtHSCode";
		this.txtHSCode.Size = new System.Drawing.Size(220, 23);
		this.txtHSCode.TabIndex = 12;
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
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPurchaseGL.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Location = new System.Drawing.Point(361, 20);
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Name = "cmbPurchaseGL";
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).TabIndex = 9;
		this.cmbTaxType.AllowNull = (DefaultableBoolean)1;
		this.cmbTaxType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTaxType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTaxType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance204;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance205).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance207).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbTaxType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Location = new System.Drawing.Point(361, 162);
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Name = "cmbTaxType";
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.cmbTaxType).TabIndex = 14;
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
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).Location = new System.Drawing.Point(361, 49);
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).Name = "cmbSaleGL";
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).TabIndex = 10;
		this.cmbCGSGL.AllowNull = (DefaultableBoolean)1;
		this.cmbCGSGL.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCGSGL.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCGSGL.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Appearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance185).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance185).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance186;
		((SpecialBoxBase)((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance192).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance193).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCGSGL.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Location = new System.Drawing.Point(361, 78);
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Name = "cmbCGSGL";
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).TabIndex = 11;
		this.rdWithoutVariant.AutoSize = true;
		this.rdWithoutVariant.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdWithoutVariant.Location = new System.Drawing.Point(490, 224);
		this.rdWithoutVariant.Name = "rdWithoutVariant";
		this.rdWithoutVariant.Size = new System.Drawing.Size(109, 19);
		this.rdWithoutVariant.TabIndex = 21;
		this.rdWithoutVariant.TabStop = true;
		this.rdWithoutVariant.Text = "Without Variant";
		this.rdWithoutVariant.UseVisualStyleBackColor = true;
		this.rdWithVariant.AutoSize = true;
		this.rdWithVariant.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdWithVariant.Location = new System.Drawing.Point(490, 205);
		this.rdWithVariant.Name = "rdWithVariant";
		this.rdWithVariant.Size = new System.Drawing.Size(91, 19);
		this.rdWithVariant.TabIndex = 20;
		this.rdWithVariant.TabStop = true;
		this.rdWithVariant.Text = "With Variant";
		this.rdWithVariant.UseVisualStyleBackColor = true;
		this.ChkIsThirdParty.AutoSize = true;
		this.ChkIsThirdParty.Checked = true;
		this.ChkIsThirdParty.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkIsThirdParty.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.ChkIsThirdParty.Location = new System.Drawing.Point(490, 271);
		this.ChkIsThirdParty.Name = "ChkIsThirdParty";
		this.ChkIsThirdParty.Size = new System.Drawing.Size(96, 19);
		this.ChkIsThirdParty.TabIndex = 23;
		this.ChkIsThirdParty.Text = "Is Third Party";
		this.ChkIsThirdParty.UseVisualStyleBackColor = true;
		this.ChkIsThirdParty.Visible = false;
		this.ChkIsCompany.AutoSize = true;
		this.ChkIsCompany.Checked = true;
		this.ChkIsCompany.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkIsCompany.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.ChkIsCompany.Location = new System.Drawing.Point(490, 252);
		this.ChkIsCompany.Name = "ChkIsCompany";
		this.ChkIsCompany.Size = new System.Drawing.Size(89, 19);
		this.ChkIsCompany.TabIndex = 22;
		this.ChkIsCompany.Text = "Is Company";
		this.ChkIsCompany.UseVisualStyleBackColor = true;
		this.ChkIsCompany.Visible = false;
		this.chkCommissionOnPurchase.AutoSize = true;
		this.chkCommissionOnPurchase.Checked = true;
		this.chkCommissionOnPurchase.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkCommissionOnPurchase.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chkCommissionOnPurchase.Location = new System.Drawing.Point(286, 247);
		this.chkCommissionOnPurchase.Name = "chkCommissionOnPurchase";
		this.chkCommissionOnPurchase.Size = new System.Drawing.Size(160, 19);
		this.chkCommissionOnPurchase.TabIndex = 18;
		this.chkCommissionOnPurchase.Text = "Commission on Purchase";
		this.chkCommissionOnPurchase.UseVisualStyleBackColor = true;
		this.chkCommissionOnSale.AutoSize = true;
		this.chkCommissionOnSale.Checked = true;
		this.chkCommissionOnSale.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkCommissionOnSale.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chkCommissionOnSale.Location = new System.Drawing.Point(286, 228);
		this.chkCommissionOnSale.Name = "chkCommissionOnSale";
		this.chkCommissionOnSale.Size = new System.Drawing.Size(131, 19);
		this.chkCommissionOnSale.TabIndex = 17;
		this.chkCommissionOnSale.Text = "Commision On Sale";
		this.chkCommissionOnSale.UseVisualStyleBackColor = true;
		this.txtItemCodeNew.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemCodeNew.Location = new System.Drawing.Point(91, 80);
		this.txtItemCodeNew.Name = "txtItemCodeNew";
		this.txtItemCodeNew.ReadOnly = true;
		this.txtItemCodeNew.Size = new System.Drawing.Size(192, 23);
		this.txtItemCodeNew.TabIndex = 2;
		this.txtItemCodeNew.TabStop = false;
		this.txtItemCodeNew.Leave += new System.EventHandler(txtItemCodeNew_TextChanged);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(6, 291);
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
		this.BtnShowDetails.AutoSize = true;
		this.BtnShowDetails.BackColor = System.Drawing.Color.Teal;
		this.BtnShowDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowDetails.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowDetails.ForeColor = System.Drawing.Color.White;
		this.BtnShowDetails.Location = new System.Drawing.Point(103, 286);
		this.BtnShowDetails.Name = "BtnShowDetails";
		this.BtnShowDetails.Size = new System.Drawing.Size(41, 28);
		this.BtnShowDetails.TabIndex = 24;
		this.BtnShowDetails.Text = "+";
		this.BtnShowDetails.UseVisualStyleBackColor = false;
		this.BtnShowDetails.Click += new System.EventHandler(BtnShowDetails_Click);
		this.txtOtherLingo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherLingo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOtherLingo.Location = new System.Drawing.Point(91, 135);
		this.txtOtherLingo.Name = "txtOtherLingo";
		this.txtOtherLingo.Size = new System.Drawing.Size(192, 23);
		this.txtOtherLingo.TabIndex = 4;
		this.lblCropYear.AutoSize = true;
		this.lblCropYear.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblCropYear.Location = new System.Drawing.Point(286, 197);
		this.lblCropYear.Name = "lblCropYear";
		this.lblCropYear.Size = new System.Drawing.Size(57, 15);
		this.lblCropYear.TabIndex = 43;
		this.lblCropYear.Text = "Crop Year";
		this.chbGST.AutoSize = true;
		this.chbGST.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chbGST.Location = new System.Drawing.Point(533, 164);
		this.chbGST.Name = "chbGST";
		this.chbGST.Size = new System.Drawing.Size(48, 19);
		this.chbGST.TabIndex = 15;
		this.chbGST.Text = "GST";
		this.chbGST.UseVisualStyleBackColor = true;
		this.txtItemCode.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemCode.Location = new System.Drawing.Point(176, 289);
		this.txtItemCode.Name = "txtItemCode";
		this.txtItemCode.ReadOnly = true;
		this.txtItemCode.Size = new System.Drawing.Size(107, 23);
		this.txtItemCode.TabIndex = 1;
		this.txtItemCode.TabStop = false;
		this.txtItemCode.Visible = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(6, 84);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(62, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Item Code";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(6, 255);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(74, 15);
		this.label38.TabIndex = 11;
		this.label38.Text = "Mother Item";
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
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance173;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance179).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance180).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance183;
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
		this.txtItemName.Location = new System.Drawing.Point(91, 107);
		this.txtItemName.Name = "txtItemName";
		this.txtItemName.Size = new System.Drawing.Size(192, 23);
		this.txtItemName.TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(6, 111);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 1;
		this.label2.Text = "Item Name";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(286, 84);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(68, 15);
		this.label12.TabIndex = 11;
		this.label12.Text = "CGS GL A/c";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(6, 168);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(56, 15);
		this.label3.TabIndex = 2;
		this.label3.Text = "Base Unit";
		this.cmbItemType.AllowNull = (DefaultableBoolean)1;
		this.cmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance212).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance212).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance213;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance214).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance214).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance218).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance219).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance219).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance219).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance219;
		((AppearanceBase)appearance221).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Location = new System.Drawing.Point(91, 49);
		((System.Windows.Forms.Control)(object)this.cmbItemType).Name = "cmbItemType";
		((System.Windows.Forms.Control)(object)this.cmbItemType).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemType).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Leave += new System.EventHandler(cmbItemType_Leave);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(286, 55);
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
		this.cmbBaseUnit.AllowNull = (DefaultableBoolean)1;
		this.cmbBaseUnit.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBaseUnit.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBaseUnit.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Appearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance225).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance225).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance226;
		((SpecialBoxBase)((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance227).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance232).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance232).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance233).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance233).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance233).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance233;
		((AppearanceBase)appearance234).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance235).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbBaseUnit.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Location = new System.Drawing.Point(91, 162);
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Name = "cmbBaseUnit";
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).TabIndex = 5;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(286, 26);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(76, 15);
		this.label10.TabIndex = 9;
		this.label10.Text = "Stock GL A/c";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(6, 55);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(60, 15);
		this.label5.TabIndex = 4;
		this.label5.Text = "Item Type";
		this.CmbMotherItem.AllowNull = (DefaultableBoolean)1;
		this.CmbMotherItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMotherItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMotherItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance251).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance251).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance251).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMotherItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance251;
		((AppearanceBase)appearance252).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance252;
		((SpecialBoxBase)((UltraGridBase)this.CmbMotherItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMotherItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMotherItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbMotherItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMotherItem).Location = new System.Drawing.Point(91, 249);
		((System.Windows.Forms.Control)(object)this.CmbMotherItem).Name = "CmbMotherItem";
		((System.Windows.Forms.Control)(object)this.CmbMotherItem).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.CmbMotherItem).TabIndex = 8;
		this.CmbItemGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbItemGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
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
		this.label8.Location = new System.Drawing.Point(286, 139);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(54, 15);
		this.label8.TabIndex = 7;
		this.label8.Text = "Pack Qty";
		this.chkstatus.AutoSize = true;
		this.chkstatus.Checked = true;
		this.chkstatus.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkstatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chkstatus.Location = new System.Drawing.Point(286, 268);
		this.chkstatus.Name = "chkstatus";
		this.chkstatus.Size = new System.Drawing.Size(71, 19);
		this.chkstatus.TabIndex = 19;
		this.chkstatus.Text = "Is Active";
		this.chkstatus.UseVisualStyleBackColor = true;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(6, 139);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(72, 15);
		this.label37.TabIndex = 6;
		this.label37.Text = "Other Name";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(286, 168);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(53, 15);
		this.label33.TabIndex = 10;
		this.label33.Text = "Tax Type";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(286, 111);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(53, 15);
		this.label7.TabIndex = 6;
		this.label7.Text = "HS Code";
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.grdAllocation);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(870, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(245, 324);
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
		((System.Windows.Forms.Control)(object)this.grdAllocation).Size = new System.Drawing.Size(239, 302);
		((System.Windows.Forms.Control)(object)this.grdAllocation).TabIndex = 0;
		this.grdAllocation.TotalRow = (InheritableBoolean)1;
		this.grdAllocation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAllocation).VisualStyle = (VisualStyle)4;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label41);
		this.panel2.Controls.Add(this.radBtnTypeAndCategory);
		this.panel2.Controls.Add(this.radBtnCategoryAndType);
		this.panel2.Controls.Add(this.label13);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 35);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1297, 24);
		this.panel2.TabIndex = 6;
		this.panel6.Controls.Add(this.toolStrip1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1297, 32);
		this.panel6.TabIndex = 5;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnnew, this.toolStripButton1, this.btnupdate, this.btnsave, this.btnprint, this.btnattachment });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1297, 27);
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
		this.tpItemDefinitionAddItemHistory.Size = new System.Drawing.Size(1303, 636);
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
		this.panel3.Size = new System.Drawing.Size(1303, 636);
		this.panel3.TabIndex = 0;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		this.grdhistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupTotals = (GroupTotals)2;
		this.grdhistory.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 149);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdhistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1303, 487);
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
		this.panel4.Location = new System.Drawing.Point(0, 121);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1303, 28);
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
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1256, 1);
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
		this.panel9.Size = new System.Drawing.Size(1303, 69);
		this.panel9.TabIndex = 68;
		this.grpUpdateItemName.Controls.Add(this.label34);
		this.grpUpdateItemName.Controls.Add(this.btnUomUpdate);
		this.grpUpdateItemName.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.grpUpdateItemName.Location = new System.Drawing.Point(569, 2);
		this.grpUpdateItemName.Name = "grpUpdateItemName";
		this.grpUpdateItemName.Size = new System.Drawing.Size(657, 63);
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
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentCategoryHistory);
		this.groupBox5.Controls.Add(this.label40);
		this.groupBox5.Controls.Add(this.btnsearch);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory);
		this.groupBox5.Controls.Add(this.label39);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitemcathistory);
		this.groupBox5.Controls.Add(this.label32);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(5, 2);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(561, 63);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Filters";
		this.CmbParentCategoryHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentCategoryHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentCategoryHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentCategoryHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentCategoryHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentCategoryHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentCategoryHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentCategoryHistory).Location = new System.Drawing.Point(3, 32);
		((System.Windows.Forms.Control)(object)this.CmbParentCategoryHistory).Name = "CmbParentCategoryHistory";
		((System.Windows.Forms.Control)(object)this.CmbParentCategoryHistory).Size = new System.Drawing.Size(127, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentCategoryHistory).TabIndex = 0;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(3, 14);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(91, 15);
		this.label40.TabIndex = 4;
		this.label40.Text = "Parent Category";
		this.btnsearch.BackColor = System.Drawing.Color.Teal;
		this.btnsearch.FlatAppearance.BorderSize = 0;
		this.btnsearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnsearch.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsearch.ForeColor = System.Drawing.Color.White;
		this.btnsearch.Location = new System.Drawing.Point(487, 32);
		this.btnsearch.Name = "btnsearch";
		this.btnsearch.Size = new System.Drawing.Size(70, 26);
		this.btnsearch.TabIndex = 3;
		this.btnsearch.Text = "Search";
		this.btnsearch.UseVisualStyleBackColor = false;
		this.btnsearch.Click += new System.EventHandler(btnsearch_Click);
		this.CmbItemTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance238).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance238).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance238).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance238;
		((AppearanceBase)appearance239).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance239;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance241).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance241;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance244;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance245).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance246).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance246).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance246).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance246;
		((AppearanceBase)appearance247).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory).Location = new System.Drawing.Point(323, 32);
		((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory).Name = "CmbItemTypeHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory).Size = new System.Drawing.Size(162, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemTypeHistory).TabIndex = 2;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(323, 14);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(60, 15);
		this.label39.TabIndex = 2;
		this.label39.Text = "Item Type";
		this.cmbitemcathistory.AllowNull = (DefaultableBoolean)1;
		this.cmbitemcathistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitemcathistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitemcathistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitemcathistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Location = new System.Drawing.Point(131, 32);
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Name = "cmbitemcathistory";
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).TabIndex = 1;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(131, 14);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(82, 15);
		this.label32.TabIndex = 0;
		this.label32.Text = "Item Category";
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.toolStrip2);
		this.panel10.Controls.Add(this.label31);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1303, 52);
		this.panel10.TabIndex = 65;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1303, 27);
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
		this.radBtnCategoryAndType.AutoSize = true;
		this.radBtnCategoryAndType.Checked = true;
		this.radBtnCategoryAndType.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radBtnCategoryAndType.ForeColor = System.Drawing.Color.Yellow;
		this.radBtnCategoryAndType.Location = new System.Drawing.Point(241, 2);
		this.radBtnCategoryAndType.Name = "radBtnCategoryAndType";
		this.radBtnCategoryAndType.Size = new System.Drawing.Size(143, 21);
		this.radBtnCategoryAndType.TabIndex = 1;
		this.radBtnCategoryAndType.TabStop = true;
		this.radBtnCategoryAndType.Text = "Category And Type";
		this.radBtnCategoryAndType.UseVisualStyleBackColor = true;
		this.radBtnCategoryAndType.CheckedChanged += new System.EventHandler(radBtnCategoryAndType_CheckedChanged);
		this.radBtnTypeAndCategory.AutoSize = true;
		this.radBtnTypeAndCategory.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radBtnTypeAndCategory.ForeColor = System.Drawing.Color.Yellow;
		this.radBtnTypeAndCategory.Location = new System.Drawing.Point(384, 2);
		this.radBtnTypeAndCategory.Name = "radBtnTypeAndCategory";
		this.radBtnTypeAndCategory.Size = new System.Drawing.Size(143, 21);
		this.radBtnTypeAndCategory.TabIndex = 2;
		this.radBtnTypeAndCategory.Text = "Type And Category";
		this.radBtnTypeAndCategory.UseVisualStyleBackColor = true;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Yellow;
		this.label41.Location = new System.Drawing.Point(146, 4);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(95, 17);
		this.label41.TabIndex = 192;
		this.label41.Text = "Item Name By";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1311, 665);
		base.Controls.Add(this.tabCAddItem);
		base.KeyPreview = true;
		base.Name = "InvDefrmAddItem";
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
		((System.ComponentModel.ISupportInitialize)this.combcropyear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPurchaseGL).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTaxType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSaleGL).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCGSGL).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemClass).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBaseUnit).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbMotherItem).EndInit();
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
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategoryHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemTypeHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitemcathistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
