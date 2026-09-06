using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
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
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using Zen.Barcode;
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

public class InvAddItemsPOS : Form
{
	private string ofd = string.Empty;

	private string UniqFileName2 = string.Empty;

	private DropDownBind DDL = new DropDownBind();

	public UserAccount UserAccount = new UserAccount();

	private Rightsobjects formrights = new Rightsobjects();

	private Attachment AT = new Attachment();

	public bool UpdateMode;

	private Country country = new Country();

	private bool flag;

	private int RecId;

	private string saveDirectory = "";

	private string fileSavePath = "";

	private string fileSavePath2 = "";

	private DataTable dtCatergory = new DataTable();

	private object picbox2;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton toolStripButton2;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnprint;

	private ToolStripButton btnattachment;

	private ToolStripButton btnRefresh;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	private TextBox txtRetailRate;

	private Label label15;

	private TextBox txtWholeSaleRate;

	private Label label27;

	private TextBox txtPurchaseRate;

	private Label label22;

	private Label label20;

	private Label label18;

	private TextBox txtCostPrice;

	private TextBox txtReorderLevel;

	private Label label17;

	private TextBox txtMinStockLevel;

	internal UltraCombo cmbUnit;

	internal UltraCombo cmbItemCategory;

	internal UltraCombo cmbCGSGL;

	internal UltraCombo cmbPurchaseGL;

	private Label label12;

	private Label label10;

	private Label label4;

	private Label label3;

	internal UltraCombo cmbItemType;

	internal UltraCombo cmbSaleGL;

	private TextBox txtItemCode;

	private Label label11;

	private Label label5;

	private Label label1;

	internal UltraCombo cmbItemClass;

	private TextBox txtPackQuantity;

	private TextBox txtItemName;

	private Label label8;

	private Label label6;

	private Label label2;

	private Button btnclose;

	private CheckBox chkIsActive;

	private CheckBox chkIsTaxable;

	private GroupBox groupBox3;

	private TextBox txtEcnomicalStockLevel;

	private Label label13;

	private TextBox txtMaxStockLevel;

	private Label label14;

	private GroupBox groupBox4;

	private TextBox txtBarcodeNo;

	private Label label21;

	private GroupBox groupBox5;

	private Button btnimg1;

	private PictureBox PicProduct;

	private GroupBox groupBox6;

	private PictureBox PicBarcode;

	private ToolStripButton btnPrintBarcode;

	private ToolStripButton btnBarcodeGenerate;

	private Panel panel5;

	private Panel panel6;

	private ToolStrip toolStrip2;

	private ToolStripButton toolStripButton1;

	private ToolStripButton btnLoadAll;

	internal GridEX grdhistory;

	private Button btnAddSupplier;

	private Button btnPurchaseAcc;

	private Button btnSaleAcc;

	private Button btnCgsAcc;

	private Button btnItemClass;

	private Button btnAddItemType;

	private Button btnAddBaseUnit;

	private Button btnAddItemCategory;

	private TextBox txtsalesTaxOnSales;

	private Label txt;

	private TextBox txtsalestaxonPurchase;

	private Label label7;

	private TextBox txtitemSpec;

	private Label label26;

	private TextBox txtitemaliasname;

	private Label label9;

	internal UltraCombo cmbUomType;

	private Label label16;

	internal UltraCombo cmbParentitem;

	private Label label37;

	private TextBox txtHsCode;

	private Label label28;

	internal UltraCombo cmbSeason;

	private Label label30;

	internal UltraCombo cmbItemReigion;

	private Label label29;

	internal UltraCombo cmbPurchaseType;

	private Label label31;

	internal UltraCombo cmbManufacture;

	private Label label33;

	private TextBox txtManufacturerCode;

	private Label label34;

	internal UltraCombo cmbPrimarySupplier;

	private Label label35;

	private TextBox txtShelfLife;

	private Label label36;

	private Label label19;

	private CheckBox chkIsDiscountable;

	private CheckBox chkisExpireable;

	private Label label49;

	private Label label48;

	private Label label47;

	private Label label46;

	private Label label45;

	private Label label44;

	private Label label43;

	private Label label42;

	private Label label41;

	private Label label40;

	private Label label39;

	private Label label38;

	private Label label24;

	private Label label58;

	private Label label57;

	private Label label56;

	private Label label55;

	private Label label54;

	private Label label53;

	private Label label52;

	private Label label51;

	private Label label50;

	private Panel panel9;

	private RadioButton chktaxAtRetailPrice;

	private RadioButton chktaxAtPurchasePrice;

	private Panel panel8;

	private Panel panel7;

	internal UltraCombo cmbitemcathistory;

	private Button btnsearch;

	private Label label32;

	internal GridEX grdhistoryForHistory;

	private CtrlGrdBar ctrlGrdBar1;

	private GroupBox groupBox7;

	private FlowLayoutPanel flowLayoutPanel1;

	internal GridEX grdcompany;

	public InvAddItemsPOS(UserAccount pUserAccount)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void InvAddItemsPOS_Load(object sender, EventArgs e)
	{
		try
		{
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			cmbItemCategory.Focus();
			lookUpUOMTypeFill();
			lookUpPurchaseTypeFill();
			CountryFill();
			MasterItemFill();
			SupplierNameFill();
			Season();
			ItemTypeFill();
			ItemClassFill();
			PurchaseGLAcFill();
			BaseUnitFill();
			CompanyFill();
			ItemCatagoryHistoryFill();
			btnsave.Visible = true;
			btnupdate.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CompanyFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		try
		{
			Company val = new Company
			{
				OrgCompanyTypeId = UserAccount.OrganizationId
			};
			DataTable dtdata = new DataTable();
			dtdata = Company.GetAlldt(val);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CopmanyName");
			if (dtdata.Rows.Count > 0)
			{
				for (int i = 0; i < dtdata.Rows.Count; i++)
				{
					dt.Rows.Add(dtdata.Rows[i]["Id"], dtdata.Rows[i]["CompName"]);
				}
				grdcompany.DataSource = dt;
				grdcompany.RetrieveStructure();
				grdcompanySetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdcompanySetting()
	{
		try
		{
			grdcompany.RootTable.Columns["Id"].Visible = false;
			grdcompany.RootTable.Columns["CopmanyName"].Width = 350;
			grdcompany.RootTable.Columns["CopmanyName"].EditType = (EditType)0;
			grdcompany.RootTable.Columns["CopmanyName"].EditType = (EditType)1;
			grdcompany.RootTable.Columns.Add("Select");
			grdcompany.RootTable.Columns["Select"].ActAsSelector = true;
			grdcompany.RootTable.Columns["Select"].UseHeaderSelector = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CountryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = country.GetAll(new Country
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbItemReigion, "Id", "Description", "Reigion Name", true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lookUpUOMTypeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			DataTable dt = InvLookUp.GetLookupsByTypeIdDt(new InvLookUp
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				InvLookupTypeId = 7
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbUomType, "Id", "LookupName", "Reigion Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lookUpPurchaseTypeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			DataTable dt = InvLookUp.GetLookupsByTypeIdDt(new InvLookUp
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				InvLookupTypeId = 8
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbPurchaseType, "Id", "LookupName", "Purchase Type", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MasterItemFill()
	{
		try
		{
			DataTable dt = CommonServices.ItemGetForComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbParentitem, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)cmbParentitem).DisplayLayout.Bands[0].Columns[1].Width = 500;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Season()
	{
		try
		{
			DataTable dtCrop = CommonServices.CropYearGetAllService();
			if (dtCrop.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCrop, cmbSeason, "Id", "CropYear", "Season", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierNameFill()
	{
		try
		{
			DataTable Supplierlst = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (Supplierlst.Rows.Count > 0)
			{
				DDL.BindDDLNew(Supplierlst, cmbPrimarySupplier, "Id", "CompanyName", "Primary Supplier", false);
				DDL.BindDDLNew(Supplierlst, cmbManufacture, "Id", "CompanyName", "Manufacturer", false);
				((UltraGridBase)cmbPrimarySupplier).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)cmbManufacture).DisplayLayout.Bands[0].Columns[1].Width = 300;
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

	private void chkIsTaxable_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (!chkIsTaxable.Checked)
			{
				chktaxAtPurchasePrice.Checked = false;
				chktaxAtRetailPrice.Checked = false;
				txtsalesTaxOnSales.Enabled = false;
				txtsalestaxonPurchase.Enabled = false;
			}
			else
			{
				txtsalesTaxOnSales.Enabled = true;
				txtsalestaxonPurchase.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBarcodeNo_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtBarcodeNo.Text != null || txtBarcodeNo.Text != "")
			{
				btnBarcodeGenerate_Click(null, null);
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
			if (dtuom.Rows.Count > 0 && dtuom.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtuom, cmbUnit, "Id", "UOMCode", "UOM Code", false);
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
			DataTable dt = ItemType.Getall(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtType = new DataTable();
				dtType.Columns.Add("Id");
				dtType.Columns.Add("TypeDescription");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtType.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["TypeDescription"]);
				}
				if (dtType.Rows.Count > 0)
				{
					DDL.BindDDL(dtType, cmbItemType, "Id", "TypeDescription", "Item Type", ZeroIndex: false);
				}
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
		try
		{
			new ItemClass();
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
					DDL.BindDDL(dtType, cmbItemClass, "ClassId", "ClassDescription", "Item Class", ZeroIndex: false);
				}
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
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationForAddItem()
	{
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
		if (txtItemCode.Text.Trim() == "" || txtItemCode.Text == "0")
		{
			MessageBox.Show("Item Code Field is Required");
			txtItemCode.Focus();
			return false;
		}
		if (txtItemName.Text.Trim() == "")
		{
			MessageBox.Show("Item Name Field is Required");
			txtItemName.Focus();
			return false;
		}
		if (((UltraGridBase)cmbUnit).ActiveRow == null)
		{
			MessageBox.Show("Unit Field is Required");
			cmbUnit.Focus();
			return false;
		}
		if (((UltraGridBase)cmbUomType).ActiveRow == null)
		{
			MessageBox.Show("Unit Type Field is Required");
			cmbUomType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbUomType).ActiveRow == null)
		{
			MessageBox.Show("Unit Type Field is Required");
			cmbUomType.Focus();
			return false;
		}
		if (txtPackQuantity.Text.Trim() == "" || txtPackQuantity.Text == "0")
		{
			MessageBox.Show("Pack Qty Field is Required");
			txtPackQuantity.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemReigion).ActiveRow == null)
		{
			MessageBox.Show("Item Reigion Field is Required");
			cmbItemReigion.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPurchaseType).ActiveRow == null)
		{
			MessageBox.Show("Purchase Type Field is Required");
			cmbPurchaseType.Focus();
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
		if (txtPurchaseRate.Text.Trim() == "" || txtPurchaseRate.Text == "0")
		{
			MessageBox.Show("Purchase Rate Must be Greater Than 0");
			txtPurchaseRate.Focus();
			return false;
		}
		if (txtCostPrice.Text.Trim() == "" || txtCostPrice.Text == "0")
		{
			MessageBox.Show("Cost Rate Must be Greater Than 0");
			txtCostPrice.Focus();
			return false;
		}
		if (txtRetailRate.Text.Trim() == "" || txtRetailRate.Text == "0")
		{
			MessageBox.Show("Retial Rate Must be Greater Than 0");
			txtRetailRate.Focus();
			return false;
		}
		if (txtWholeSaleRate.Text.Trim() == "" || txtWholeSaleRate.Text == "0")
		{
			MessageBox.Show("Whole Sale Rate Must be Greater Than 0");
			txtWholeSaleRate.Focus();
			return false;
		}
		if (txtReorderLevel.Text.Trim() == "" || txtReorderLevel.Text == "0")
		{
			MessageBox.Show("ReOrder Level Must be Greater Than 0");
			txtReorderLevel.Focus();
			return false;
		}
		if (txtEcnomicalStockLevel.Text.Trim() == "" || txtEcnomicalStockLevel.Text == "0")
		{
			MessageBox.Show("Ecnomical Stock Level Must be Greater Than 0");
			txtEcnomicalStockLevel.Focus();
			return false;
		}
		if (txtMinStockLevel.Text.Trim() == "" || txtMinStockLevel.Text == "0")
		{
			MessageBox.Show("Min Stock Level Must be Greater Than 0");
			txtMinStockLevel.Focus();
			return false;
		}
		if (txtMaxStockLevel.Text.Trim() == "" || txtMaxStockLevel.Text == "0")
		{
			MessageBox.Show("Max Stock Level Must be Greater Than 0");
			txtMaxStockLevel.Focus();
			return false;
		}
		return true;
	}

	private void refresh()
	{
		try
		{
			btnsave.Visible = true;
			btnupdate.Visible = false;
			RecId = 0;
			((Control)(object)cmbItemCategory).Text = string.Empty;
			cmbItemType.Value = string.Empty;
			cmbItemClass.Value = string.Empty;
			txtItemCode.Text = string.Empty;
			txtBarcodeNo.Text = string.Empty;
			txtItemName.Text = string.Empty;
			txtitemaliasname.Text = string.Empty;
			cmbUnit.Value = string.Empty;
			cmbUomType.Value = string.Empty;
			cmbPurchaseType.Value = string.Empty;
			txtPackQuantity.Text = 0.ToString();
			cmbParentitem.Value = string.Empty;
			cmbItemReigion.Value = string.Empty;
			cmbSeason.Value = string.Empty;
			txtHsCode.Text = 0.ToString();
			cmbManufacture.Value = string.Empty;
			txtManufacturerCode.Text = string.Empty;
			txtManufacturerCode.Text = string.Empty;
			cmbPrimarySupplier.Value = string.Empty;
			txtShelfLife.Text = 0.ToString();
			txtitemSpec.Text = string.Empty;
			chkIsDiscountable.Checked = false;
			chkisExpireable.Checked = false;
			chkIsActive.Checked = true;
			((Control)(object)cmbPurchaseGL).Text = string.Empty;
			((Control)(object)cmbSaleGL).Text = string.Empty;
			((Control)(object)cmbCGSGL).Text = string.Empty;
			txtPurchaseRate.Text = 0.ToString();
			txtWholeSaleRate.Text = 0.ToString();
			txtCostPrice.Text = 0.ToString();
			txtRetailRate.Text = 0.ToString();
			txtReorderLevel.Text = 0.ToString();
			txtMaxStockLevel.Text = 0.ToString();
			txtMinStockLevel.Text = 0.ToString();
			txtEcnomicalStockLevel.Text = 0.ToString();
			chkIsTaxable.Checked = false;
			txtsalestaxonPurchase.Text = 0.ToString();
			chktaxAtRetailPrice.Checked = false;
			chktaxAtPurchasePrice.Checked = false;
			txtsalesTaxOnSales.Text = 0.ToString();
			cmbItemCategory.Focus();
			PicBarcode.Image = null;
			PicBarcode.Invalidate();
			PicProduct.Image = null;
			PicProduct.Invalidate();
			AT = new Attachment();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItemCategory_Leave(object sender, EventArgs e)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		//IL_00d8: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbItemCategory.Value) > 0)
			{
				((UltraGridBase)cmbUnit).Rows[0].Activate();
				((UltraGridBase)cmbItemClass).Rows[0].Activate();
				((UltraGridBase)cmbItemType).Rows[0].Activate();
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
				}
				DataTable dt = Item.GetGLAccountbyItemCategoryId(val);
				if (dt.Rows.Count > 0)
				{
					cmbPurchaseGL.Value = dt.Rows[0]["InventoryAccountId"].ToString();
					cmbSaleGL.Value = dt.Rows[0]["RevenueAccountId"].ToString();
					cmbCGSGL.Value = dt.Rows[0]["CGSAccountId"].ToString();
					txtPackQuantity.Text = "1";
				}
				else
				{
					((Control)(object)cmbPurchaseGL).Text = string.Empty;
					((Control)(object)cmbSaleGL).Text = string.Empty;
					((Control)(object)cmbCGSGL).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Expected O, but got Unknown
		try
		{
			if (grdcompany.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please Check At least One Company For Allocate Item");
			}
			if (!FormValidationForAddItem())
			{
				return;
			}
			Item items = new Item();
			items.ItemCategoryId = Conversion.ToInt(cmbItemCategory.Value);
			items.ItemTypeId = Conversion.ToInt(cmbItemType.Value);
			items.ItemClassId = Conversion.ToInt(cmbItemClass.Value);
			items.ItemCode = Conversion.ToString((object)txtItemCode.Text.Trim());
			items.BarcodeNo = Conversion.ToString((object)txtBarcodeNo.Text);
			items.ItemName = Conversion.ToString((object)txtItemName.Text.Trim());
			items.ItemAliasName = Conversion.ToString((object)txtitemaliasname.Text.Trim());
			items.BaseUnitId = Conversion.ToInt(cmbUnit.Value);
			items.UomLookUpId = Conversion.ToInt(cmbUomType.Value);
			items.PurchaseTypeLookUpId = Conversion.ToInt(cmbPurchaseType.Value);
			items.PackQty = Conversion.ToDouble((object)txtPackQuantity.Text.Trim());
			items.MasterItemId = Conversion.ToInt(cmbParentitem.Value);
			items.ItemOriginId = Conversion.ToInt(cmbItemReigion.Value);
			items.BaseCropYear = Conversion.ToString((object)((Control)(object)cmbSeason).Text);
			items.HSCode = Conversion.ToString((object)txtHsCode.Text.Trim());
			items.ManufactureId = Conversion.ToInt(cmbManufacture.Value);
			items.ManufacturePartNo = Conversion.ToString((object)txtManufacturerCode.Text);
			items.BuyerPartNo = Conversion.ToString((object)txtManufacturerCode.Text);
			items.BuyerSupplierId = Conversion.ToInt(cmbPrimarySupplier.Value);
			items.ShelfLife = Conversion.ToInt((object)txtShelfLife.Text);
			items.ItemSpecification = Conversion.ToString((object)txtitemSpec.Text);
			items.IsDiscountable = Conversion.ToBool((object)chkIsDiscountable.Checked);
			items.IsExpirable = Conversion.ToBool((object)chkisExpireable.Checked);
			items.ItemStatus = Conversion.ToBool((object)chkIsActive.Checked);
			items.PurchaseGLAC = Conversion.ToInt(cmbPurchaseGL.Value);
			items.SaleGLAC = Conversion.ToInt(cmbSaleGL.Value);
			items.COGSGLAC = Conversion.ToInt(cmbCGSGL.Value);
			items.PurchasePrice = Conversion.ToDouble((object)txtPurchaseRate.Text.Trim());
			items.WholeSalePrice = Conversion.ToDouble((object)txtWholeSaleRate.Text.Trim());
			items.CostPrice = Conversion.ToDouble((object)txtCostPrice.Text.Trim());
			items.RetailPrice = Conversion.ToDouble((object)txtRetailRate.Text.Trim());
			items.ReorderLevel = Conversion.ToDouble((object)txtReorderLevel.Text);
			items.MaxStockLevel = Conversion.ToDouble((object)txtMaxStockLevel.Text);
			items.MinStockLevel = Conversion.ToDouble((object)txtMinStockLevel.Text);
			items.OptimalStockLevel = Conversion.ToDouble((object)txtEcnomicalStockLevel.Text);
			items.IsTaxable = Conversion.ToBool((object)chkIsTaxable.Checked);
			items.SaleTaxPurPercent = Conversion.ToDouble((object)txtsalestaxonPurchase.Text.Trim());
			items.AtRetailPriceTax = Conversion.ToBool((object)chktaxAtRetailPrice.Checked);
			items.AtPurcahsePriceTax = Conversion.ToBool((object)chktaxAtPurchasePrice.Checked);
			items.SaleTaxSalesPercent = Conversion.ToDouble((object)txtsalesTaxOnSales.Text.Trim());
			items.Pic1 = Conversion.ToString((object)fileSavePath);
			items.Pic2 = "";
			items.BarcodeImage = Conversion.ToString((object)fileSavePath2);
			items.OrganizationId = UserAccount.OrganizationId;
			items.CompanyId = UserAccount.CompanyId;
			items.EntryUser = UserAccount.ID;
			if (PicBarcode.Image != null)
			{
				items.BarcodeImage = UniqFileName2;
			}
			items.ItemAllocationlist = new List<ItemAllocation>();
			GridEXRow[] rows = grdcompany.GetRows();
			foreach (GridEXRow r in rows)
			{
				ItemAllocation coaa = new ItemAllocation();
				coaa.OrganizationId = UserAccount.OrganizationId;
				coaa.BranchId = UserAccount.BranchesId;
				coaa.CompanyId = int.Parse(r.Cells["Id"].Value.ToString());
				coaa.IsActive = Conversion.ToBool((object)chkIsActive.Checked);
				items.ItemAllocationlist.Add(coaa);
			}
			items.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.ScreenName = base.Name;
				items.AttachmentsList.Add(attach);
			}
			if (Item.Save(items, true) >= 1)
			{
				MessageBox.Show("Save Successfully");
				if (ofd != null && ofd != string.Empty)
				{
					File.Copy(ofd, fileSavePath, overwrite: true);
					ofd = string.Empty;
					fileSavePath = string.Empty;
					PicProduct.Image = null;
					PicProduct.Invalidate();
				}
			}
			refresh();
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
			refresh();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Expected O, but got Unknown
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Expected O, but got Unknown
		try
		{
			if (grdcompany.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please Check At least One Company For Allocate Item");
			}
			if (!FormValidationForAddItem())
			{
				return;
			}
			Item items = new Item();
			items.Id = RecId;
			items.ItemCategoryId = Conversion.ToInt(cmbItemCategory.Value);
			items.ItemTypeId = Conversion.ToInt(cmbItemType.Value);
			items.ItemClassId = Conversion.ToInt(cmbItemClass.Value);
			items.ItemCode = Conversion.ToString((object)txtItemCode.Text.Trim());
			items.BarcodeNo = Conversion.ToString((object)txtBarcodeNo.Text);
			items.ItemName = Conversion.ToString((object)txtItemName.Text.Trim());
			items.ItemAliasName = Conversion.ToString((object)txtitemaliasname.Text.Trim());
			items.BaseUnitId = Conversion.ToInt(cmbUnit.Value);
			items.UomLookUpId = Conversion.ToInt(cmbUomType.Value);
			items.PurchaseTypeLookUpId = Conversion.ToInt(cmbPurchaseType.Value);
			items.PackQty = Conversion.ToDouble((object)txtPackQuantity.Text.Trim());
			items.MasterItemId = Conversion.ToInt(cmbParentitem.Value);
			items.ItemOriginId = Conversion.ToInt(cmbItemReigion.Value);
			items.BaseCropYear = Conversion.ToString((object)((Control)(object)cmbSeason).Text);
			items.HSCode = Conversion.ToString((object)txtHsCode.Text.Trim());
			items.ManufactureId = Conversion.ToInt(cmbManufacture.Value);
			items.ManufacturePartNo = Conversion.ToString((object)txtManufacturerCode.Text);
			items.BuyerPartNo = Conversion.ToString((object)txtManufacturerCode.Text);
			items.BuyerSupplierId = Conversion.ToInt(cmbPrimarySupplier.Value);
			items.ShelfLife = Conversion.ToInt((object)txtShelfLife.Text);
			items.ItemSpecification = Conversion.ToString((object)txtitemSpec.Text);
			items.IsDiscountable = Conversion.ToBool((object)chkIsDiscountable.Checked);
			items.IsExpirable = Conversion.ToBool((object)chkisExpireable.Checked);
			items.ItemStatus = Conversion.ToBool((object)chkIsActive.Checked);
			items.PurchaseGLAC = Conversion.ToInt(cmbPurchaseGL.Value);
			items.SaleGLAC = Conversion.ToInt(cmbSaleGL.Value);
			items.COGSGLAC = Conversion.ToInt(cmbCGSGL.Value);
			items.PurchasePrice = Conversion.ToDouble((object)txtPurchaseRate.Text.Trim());
			items.WholeSalePrice = Conversion.ToDouble((object)txtWholeSaleRate.Text.Trim());
			items.CostPrice = Conversion.ToDouble((object)txtCostPrice.Text.Trim());
			items.RetailPrice = Conversion.ToDouble((object)txtRetailRate.Text.Trim());
			items.ReorderLevel = Conversion.ToDouble((object)txtReorderLevel.Text);
			items.MaxStockLevel = Conversion.ToDouble((object)txtMaxStockLevel.Text);
			items.MinStockLevel = Conversion.ToDouble((object)txtMinStockLevel.Text);
			items.OptimalStockLevel = Conversion.ToDouble((object)txtEcnomicalStockLevel.Text);
			items.IsTaxable = Conversion.ToBool((object)chkIsTaxable.Checked);
			items.SaleTaxPurPercent = Conversion.ToDouble((object)txtsalestaxonPurchase.Text.Trim());
			items.AtRetailPriceTax = Conversion.ToBool((object)chktaxAtRetailPrice.Checked);
			items.AtPurcahsePriceTax = Conversion.ToBool((object)chktaxAtPurchasePrice.Checked);
			items.SaleTaxSalesPercent = Conversion.ToDouble((object)txtsalesTaxOnSales.Text.Trim());
			items.ModifyUser = UserAccount.ID;
			items.OrganizationId = UserAccount.OrganizationId;
			items.CompanyId = UserAccount.CompanyId;
			if (PicBarcode.Image != null)
			{
				items.BarcodeImage = UniqFileName2;
			}
			int Success = Item.Save(items, true);
			if (Success >= 1)
			{
				try
				{
					ItemAllocation coaah = new ItemAllocation();
					coaah.ItemAllocationlist = new List<ItemAllocation>();
					GridEXRow[] rows = grdcompany.GetRows();
					foreach (GridEXRow r in rows)
					{
						ItemAllocation coaa = new ItemAllocation();
						coaa.OrganizationId = UserAccount.OrganizationId;
						coaa.BranchId = UserAccount.BranchesId;
						coaa.CompanyId = int.Parse(r.Cells["Id"].Value.ToString());
						coaa.ItemId = Success;
						coaa.IsActive = Conversion.ToBool((object)chkIsActive.Checked);
						coaah.ItemAllocationlist.Add(coaa);
					}
					ItemAllocation.Save(coaah);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
				MessageBox.Show("Update Successfully");
				if (ofd != null && ofd != string.Empty)
				{
					File.Copy(ofd, fileSavePath, overwrite: true);
					ofd = string.Empty;
					fileSavePath = string.Empty;
					PicProduct.Image = null;
					PicProduct.Invalidate();
				}
			}
			refresh();
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	private void btnBarcodeGenerate_Click(object sender, EventArgs e)
	{
		try
		{
			if (txtBarcodeNo.Text != "")
			{
				List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Attachment Folder Path").ToList();
				if (list.Count > 0)
				{
					Code128BarcodeDraw barcode = BarcodeDrawFactory.Code128WithChecksum;
					PicBarcode.Image = ((BarcodeDraw)barcode).Draw(txtBarcodeNo.Text, 50);
					string FingerPPicPath = list[0].ConfigKey.ToString() + "\\";
					string UniqFileName = $"{Guid.NewGuid()}.Bmp";
					fileSavePath2 = FingerPPicPath + UniqFileName;
					Image image = Image.FromFile(fileSavePath2);
					MemoryStream ms = new MemoryStream();
					image.Save(ms, ImageFormat.Bmp);
					SaveFileDialog saveFile = new SaveFileDialog();
					saveFile.DefaultExt = "jpg";
					saveFile.Filter = "jpeg|*.jpg";
					if (saveFile.ShowDialog() == DialogResult.OK)
					{
						fileSavePath2 = saveFile.FileName;
					}
				}
			}
			else
			{
				MessageBox.Show("Enter Barcode Number to Generate Barcode");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintBarcode_Click(object sender, EventArgs e)
	{
		try
		{
			PrintDialog printDialog = new PrintDialog();
			PrintDocument doc = new PrintDocument();
			doc.PrintPage += Doc_PrintPage;
			printDialog.Document = doc;
			if (printDialog.ShowDialog() == DialogResult.OK)
			{
				doc.Print();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Doc_PrintPage(object sender, PrintPageEventArgs e)
	{
		try
		{
			Bitmap bm = new Bitmap(PicBarcode.Width, PicBarcode.Height);
			PicBarcode.DrawToBitmap(bm, new Rectangle(0, 0, PicBarcode.Width, PicBarcode.Height));
			e.Graphics.DrawImage(bm, 0, 0);
			bm.Dispose();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrmfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters reports = new ReportsParameters();
			reports.OrganizationId = UserAccount.OrganizationId;
			reports.CompanyId = UserAccount.CompanyId;
			reports.NoOfRecords = 50;
			if (Convert.ToInt32(cmbitemcathistory.Value) != 0)
			{
				reports.Id = Convert.ToInt32(cmbitemcathistory.Value);
			}
			reports.ScreenName = base.Name;
			reports.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			DataTable grdlst = Item.FormHistory(reports);
			if (grdlst.Rows.Count > 0)
			{
				grdhistoryForHistory.DataSource = grdlst;
				grdhistoryForHistory.RetrieveStructure();
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
			grdhistoryForHistory.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grdhistoryForHistory.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdhistoryForHistory.RootTable.Columns["ItemCategory"].EditType = (EditType)0;
			grdhistoryForHistory.RootTable.Columns["ItemType"].EditType = (EditType)0;
			grdhistoryForHistory.RootTable.Columns["StockAccount"].EditType = (EditType)0;
			grdhistoryForHistory.RootTable.Columns["SaleAccount"].EditType = (EditType)0;
			grdhistoryForHistory.RootTable.Columns["CgsAccount"].EditType = (EditType)0;
			grdhistoryForHistory.RootTable.Columns["UserName"].EditType = (EditType)0;
			grdhistoryForHistory.RootTable.Columns["NoOfAttachments"].EditType = (EditType)0;
			grdhistoryForHistory.RootTable.Columns["ItemCode"].FilterEditType = (FilterEditType)1;
			grdhistoryForHistory.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			grdhistoryForHistory.RootTable.Columns["ItemCategory"].FilterEditType = (FilterEditType)1;
			grdhistoryForHistory.RootTable.Columns["ItemType"].FilterEditType = (FilterEditType)1;
			grdhistoryForHistory.RootTable.Columns["StockAccount"].FilterEditType = (FilterEditType)1;
			grdhistoryForHistory.RootTable.Columns["SaleAccount"].FilterEditType = (FilterEditType)1;
			grdhistoryForHistory.RootTable.Columns["CgsAccount"].FilterEditType = (FilterEditType)1;
			grdhistoryForHistory.RootTable.Columns["UserName"].FilterEditType = (FilterEditType)1;
			grdhistoryForHistory.RootTable.Columns["NoOfAttachments"].FilterEditType = (FilterEditType)1;
			grdhistoryForHistory.RootTable.Columns["ItemCode"].Width = 50;
			grdhistoryForHistory.RootTable.Columns["ItemName"].Width = 250;
			grdhistoryForHistory.RootTable.Columns["ItemCategory"].Width = 250;
			grdhistoryForHistory.RootTable.Columns["ItemType"].Width = 150;
			grdhistoryForHistory.RootTable.Columns["StockAccount"].Width = 250;
			grdhistoryForHistory.RootTable.Columns["SaleAccount"].Width = 250;
			grdhistoryForHistory.RootTable.Columns["CgsAccount"].Width = 250;
			grdhistoryForHistory.RootTable.Columns["UserName"].Width = 150;
			grdhistoryForHistory.RootTable.Columns["NoOfAttachments"].Width = 100;
			grdhistoryForHistory.RootTable.Columns["Id"].Visible = false;
			grdhistoryForHistory.RootTable.Columns["RecordNo"].Visible = false;
			grdhistoryForHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvDefrmAddItem_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnsave.Visible && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
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
			if (btnupdate.Visible && e.Control && e.KeyCode == Keys.U && (UpdateMode = true))
			{
				btnupdate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void definition()
	{
		try
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
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnimg1_Click(object sender, EventArgs e)
	{
		try
		{
			definition();
			if (flag)
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "(*.jpg; *.jpeg; *.gif; *.PNG; *.png)| *.jpg; *.jpeg; *.gif; *.PNG;*.png; ";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					ofd = openFileDialog.FileName;
					string fileName = Path.GetFileNameWithoutExtension(ofd);
					string exten = Path.GetExtension(ofd);
					string date = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
					fileName = fileName + date + exten;
					fileSavePath = Path.Combine(saveDirectory, fileName);
					PicProduct.Image = new Bitmap(openFileDialog.FileName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters reports = new ReportsParameters();
			reports.OrganizationId = UserAccount.OrganizationId;
			reports.CompanyId = UserAccount.CompanyId;
			if (Convert.ToInt32(cmbitemcathistory.Value) != 0)
			{
				reports.Id = Convert.ToInt32(cmbitemcathistory.Value);
			}
			reports.ScreenName = base.Name;
			reports.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			DataTable grdlst = Item.FormHistory(reports);
			if (grdlst.Rows.Count > 0)
			{
				grdhistoryForHistory.DataSource = grdlst;
				grdhistoryForHistory.RetrieveStructure();
				gridsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void grdhistoryForHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnupdate.Visible = true;
			Item items = new Item();
			RecId = Conversion.ToInt((object)grdhistoryForHistory.CurrentRow.Cells[1].Value.ToString());
			items = Item.GetByID(RecId);
			items.Id = RecId;
			tabControl1.SelectedIndex = 0;
			cmbItemCategory.Value = Conversion.ToInt((object)items.ItemCategoryId);
			cmbItemType.Value = Conversion.ToInt((object)items.ItemTypeId);
			cmbItemClass.Value = Conversion.ToInt((object)items.ItemClassId);
			txtItemCode.Text = Conversion.ToString((object)items.ItemCode);
			txtBarcodeNo.Text = Conversion.ToString((object)items.BarcodeNo);
			txtItemName.Text = Conversion.ToString((object)items.ItemName);
			txtitemaliasname.Text = Conversion.ToString((object)items.ItemAliasName);
			cmbUnit.Value = Conversion.ToInt((object)items.BaseUnitId);
			cmbUomType.Value = Conversion.ToInt((object)items.UomLookUpId);
			cmbPurchaseType.Value = Conversion.ToInt((object)items.PurchaseTypeLookUpId);
			txtPackQuantity.Text = Conversion.ToString((object)items.PackQty);
			cmbParentitem.Value = Conversion.ToInt((object)items.MasterItemId);
			cmbItemReigion.Value = Conversion.ToInt((object)items.ItemOriginId);
			((Control)(object)cmbSeason).Text = Conversion.ToString((object)items.BaseCropYear);
			txtHsCode.Text = Conversion.ToString((object)items.HSCode);
			cmbManufacture.Value = Conversion.ToInt((object)items.ManufactureId);
			txtManufacturerCode.Text = Conversion.ToString((object)items.ManufacturePartNo);
			txtManufacturerCode.Text = Conversion.ToString((object)items.BuyerPartNo);
			cmbPrimarySupplier.Value = Conversion.ToInt((object)items.BuyerSupplierId);
			txtShelfLife.Text = Conversion.ToString((object)items.ShelfLife);
			txtitemSpec.Text = Conversion.ToString((object)items.ItemSpecification);
			chkIsDiscountable.Checked = Conversion.ToBool((object)items.IsDiscountable);
			chkisExpireable.Checked = Conversion.ToBool((object)items.IsExpirable);
			chkIsActive.Checked = Conversion.ToBool((object)items.ItemStatus);
			cmbPurchaseGL.Value = Conversion.ToInt((object)items.PurchaseGLAC);
			cmbSaleGL.Value = Conversion.ToInt((object)items.SaleGLAC);
			cmbCGSGL.Value = Conversion.ToInt((object)items.COGSGLAC);
			txtPurchaseRate.Text = Conversion.ToString((object)items.PurchasePrice);
			txtWholeSaleRate.Text = Conversion.ToString((object)items.WholeSalePrice);
			txtCostPrice.Text = Conversion.ToString((object)items.CostPrice);
			txtRetailRate.Text = Conversion.ToString((object)items.RetailPrice);
			txtReorderLevel.Text = Conversion.ToString((object)items.ReorderLevel);
			txtMaxStockLevel.Text = Conversion.ToString((object)items.MaxStockLevel);
			txtMinStockLevel.Text = Conversion.ToString((object)items.MinStockLevel);
			txtEcnomicalStockLevel.Text = Conversion.ToString((object)items.OptimalStockLevel);
			chkIsTaxable.Checked = Conversion.ToBool((object)items.IsTaxable);
			txtsalestaxonPurchase.Text = Conversion.ToString((object)items.SaleTaxPurPercent);
			chktaxAtRetailPrice.Checked = Conversion.ToBool((object)items.AtRetailPriceTax);
			chktaxAtPurchasePrice.Checked = Conversion.ToBool((object)items.AtPurcahsePriceTax);
			txtsalesTaxOnSales.Text = Conversion.ToString((object)items.SaleTaxSalesPercent);
			fileSavePath = items.Pic1;
			List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(RecId, base.Name);
			AT = new Attachment();
			foreach (global::Architecture.Model.DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
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

	private void btnprint_Click(object sender, EventArgs e)
	{
	}

	private void btnAddItemCategory_Click(object sender, EventArgs e)
	{
		new InvDeffrmItemCatagory(UserAccount).Show();
	}

	private void btnAddBaseUnit_Click(object sender, EventArgs e)
	{
		new InvDeffrmItemItemUom(UserAccount).Show();
	}

	private void btnAddItemType_Click(object sender, EventArgs e)
	{
		new InvDeffrmItemType(UserAccount).Show();
	}

	private void btnItemClass_Click(object sender, EventArgs e)
	{
	}

	private void btnItemGroup_Click(object sender, EventArgs e)
	{
		new DefineItemGroup(UserAccount).Show();
	}

	private void btnAddSupplier_Click(object sender, EventArgs e)
	{
	}

	private void btnPurchaseAcc_Click(object sender, EventArgs e)
	{
		new AcfrmDefCoa(UserAccount).Show();
	}

	private void btnSaleAcc_Click(object sender, EventArgs e)
	{
		new AcfrmDefCoa(UserAccount).Show();
	}

	private void btnCgsAcc_Click(object sender, EventArgs e)
	{
		new AcfrmDefCoa(UserAccount).Show();
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				grdfrmfill();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdhistoryForHistory).Name);
			if (fs != null)
			{
				grdhistoryForHistory.LoadLayoutFile((Stream)fs);
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
		try
		{
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdhistoryForHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(grdhistoryForHistory.CurrentRow.Cells["Id"].Value), base.Name);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvAddItemsPOS_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Expected O, but got Unknown
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Expected O, but got Unknown
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Expected O, but got Unknown
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Expected O, but got Unknown
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Expected O, but got Unknown
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Expected O, but got Unknown
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Expected O, but got Unknown
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Expected O, but got Unknown
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f7: Expected O, but got Unknown
		//IL_080e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0818: Expected O, but got Unknown
		//IL_082f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0839: Expected O, but got Unknown
		//IL_0845: Unknown result type (might be due to invalid IL or missing references)
		//IL_084f: Expected O, but got Unknown
		//IL_0866: Unknown result type (might be due to invalid IL or missing references)
		//IL_0870: Expected O, but got Unknown
		//IL_0871: Unknown result type (might be due to invalid IL or missing references)
		//IL_087b: Expected O, but got Unknown
		//IL_08b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bd: Expected O, but got Unknown
		//IL_08c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d3: Expected O, but got Unknown
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ff: Expected O, but got Unknown
		//IL_0937: Unknown result type (might be due to invalid IL or missing references)
		//IL_0941: Expected O, but got Unknown
		//IL_099a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a4: Expected O, but got Unknown
		//IL_09dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e6: Expected O, but got Unknown
		//IL_0a08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a12: Expected O, but got Unknown
		//IL_0a13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1d: Expected O, but got Unknown
		//IL_0ad9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae3: Expected O, but got Unknown
		//IL_0aef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af9: Expected O, but got Unknown
		//IL_9171: Unknown result type (might be due to invalid IL or missing references)
		//IL_917b: Expected O, but got Unknown
		GridEXLayout grdcompany_DesignTimeLayout = new GridEXLayout();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance125 = new Appearance();
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
		Appearance appearance138 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Definition.InvAddItemsPOS));
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox7 = new System.Windows.Forms.GroupBox();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.grdcompany = new GridEX();
		this.groupBox6 = new System.Windows.Forms.GroupBox();
		this.label58 = new System.Windows.Forms.Label();
		this.PicBarcode = new System.Windows.Forms.PictureBox();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.btnimg1 = new System.Windows.Forms.Button();
		this.PicProduct = new System.Windows.Forms.PictureBox();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.panel9 = new System.Windows.Forms.Panel();
		this.chktaxAtRetailPrice = new System.Windows.Forms.RadioButton();
		this.chktaxAtPurchasePrice = new System.Windows.Forms.RadioButton();
		this.txtsalesTaxOnSales = new System.Windows.Forms.TextBox();
		this.txt = new System.Windows.Forms.Label();
		this.txtsalestaxonPurchase = new System.Windows.Forms.TextBox();
		this.chkIsTaxable = new System.Windows.Forms.CheckBox();
		this.label7 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label57 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.txtEcnomicalStockLevel = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtMaxStockLevel = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.txtReorderLevel = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.txtMinStockLevel = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label53 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.txtRetailRate = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.txtWholeSaleRate = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.txtPurchaseRate = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.txtCostPrice = new System.Windows.Forms.TextBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label49 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.chkIsDiscountable = new System.Windows.Forms.CheckBox();
		this.chkisExpireable = new System.Windows.Forms.CheckBox();
		this.txtShelfLife = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.cmbPrimarySupplier = new UltraCombo();
		this.label35 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbManufacture = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.cmbPurchaseGL = new UltraCombo();
		this.txtManufacturerCode = new System.Windows.Forms.TextBox();
		this.cmbCGSGL = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.cmbSaleGL = new UltraCombo();
		this.cmbPurchaseType = new UltraCombo();
		this.btnPurchaseAcc = new System.Windows.Forms.Button();
		this.btnAddSupplier = new System.Windows.Forms.Button();
		this.btnSaleAcc = new System.Windows.Forms.Button();
		this.label31 = new System.Windows.Forms.Label();
		this.btnCgsAcc = new System.Windows.Forms.Button();
		this.cmbSeason = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.cmbItemReigion = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.txtHsCode = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.cmbParentitem = new UltraCombo();
		this.label37 = new System.Windows.Forms.Label();
		this.txtitemSpec = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.txtitemaliasname = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbUomType = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.btnItemClass = new System.Windows.Forms.Button();
		this.btnAddItemType = new System.Windows.Forms.Button();
		this.btnAddBaseUnit = new System.Windows.Forms.Button();
		this.btnAddItemCategory = new System.Windows.Forms.Button();
		this.txtBarcodeNo = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.chkIsActive = new System.Windows.Forms.CheckBox();
		this.cmbItemClass = new UltraCombo();
		this.txtPackQuantity = new System.Windows.Forms.TextBox();
		this.txtItemName = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbItemType = new UltraCombo();
		this.txtItemCode = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbUnit = new UltraCombo();
		this.cmbItemCategory = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnclose = new System.Windows.Forms.Button();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnBarcodeGenerate = new System.Windows.Forms.ToolStripButton();
		this.btnPrintBarcode = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdhistoryForHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.cmbitemcathistory = new UltraCombo();
		this.btnsearch = new System.Windows.Forms.Button();
		this.label32 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox7.SuspendLayout();
		this.flowLayoutPanel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdcompany).BeginInit();
		this.groupBox6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.PicBarcode).BeginInit();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.PicProduct).BeginInit();
		this.groupBox4.SuspendLayout();
		this.panel9.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPrimarySupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbManufacture).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPurchaseGL).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCGSGL).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSaleGL).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPurchaseType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSeason).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemReigion).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUomType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemClass).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUnit).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistoryForHistory).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbitemcathistory).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1361, 585);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1361, 585);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1353, 559);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1347, 553);
		this.panel2.TabIndex = 0;
		this.panel4.Controls.Add(this.groupBox7);
		this.panel4.Controls.Add(this.groupBox6);
		this.panel4.Controls.Add(this.groupBox5);
		this.panel4.Controls.Add(this.groupBox4);
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 31);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1347, 522);
		this.panel4.TabIndex = 1;
		this.groupBox7.Controls.Add(this.flowLayoutPanel1);
		this.groupBox7.Font = new System.Drawing.Font("Segoe UI", 10.2f);
		this.groupBox7.Location = new System.Drawing.Point(720, 295);
		this.groupBox7.Name = "groupBox7";
		this.groupBox7.Size = new System.Drawing.Size(400, 224);
		this.groupBox7.TabIndex = 130;
		this.groupBox7.TabStop = false;
		this.groupBox7.Text = "Item Allocate To Company";
		this.flowLayoutPanel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdcompany);
		this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 22);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(394, 199);
		this.flowLayoutPanel1.TabIndex = 129;
		this.grdcompany.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdcompany).BackColor = System.Drawing.Color.White;
		this.grdcompany.ColumnAutoResize = true;
		grdcompany_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdcompany.DesignTimeLayout = grdcompany_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdcompany).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdcompany.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdcompany).Location = new System.Drawing.Point(3, 3);
		((System.Windows.Forms.Control)(object)this.grdcompany).Name = "grdcompany";
		((ScrollableBase)this.grdcompany).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdcompany).Size = new System.Drawing.Size(388, 193);
		((System.Windows.Forms.Control)(object)this.grdcompany).TabIndex = 5;
		this.grdcompany.TotalRow = (InheritableBoolean)1;
		this.grdcompany.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdcompany).VisualStyle = (VisualStyle)4;
		this.groupBox6.Controls.Add(this.label58);
		this.groupBox6.Controls.Add(this.PicBarcode);
		this.groupBox6.Font = new System.Drawing.Font("Segoe UI", 10.2f);
		this.groupBox6.Location = new System.Drawing.Point(1167, 184);
		this.groupBox6.Name = "groupBox6";
		this.groupBox6.Size = new System.Drawing.Size(178, 79);
		this.groupBox6.TabIndex = 0;
		this.groupBox6.TabStop = false;
		this.groupBox6.Text = "Barcode Image";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Black", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.Red;
		this.label58.Location = new System.Drawing.Point(105, -5);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(18, 21);
		this.label58.TabIndex = 583;
		this.label58.Text = "*";
		this.PicBarcode.Location = new System.Drawing.Point(6, 20);
		this.PicBarcode.Name = "PicBarcode";
		this.PicBarcode.Size = new System.Drawing.Size(160, 53);
		this.PicBarcode.TabIndex = 6;
		this.PicBarcode.TabStop = false;
		this.groupBox5.Controls.Add(this.btnimg1);
		this.groupBox5.Controls.Add(this.PicProduct);
		this.groupBox5.Font = new System.Drawing.Font("Segoe UI", 10.2f);
		this.groupBox5.Location = new System.Drawing.Point(1167, 26);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(178, 154);
		this.groupBox5.TabIndex = 5;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Product Image ";
		this.btnimg1.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnimg1.Location = new System.Drawing.Point(61, 124);
		this.btnimg1.Name = "btnimg1";
		this.btnimg1.Size = new System.Drawing.Size(75, 27);
		this.btnimg1.TabIndex = 0;
		this.btnimg1.Text = "Browse";
		this.btnimg1.UseVisualStyleBackColor = true;
		this.btnimg1.Click += new System.EventHandler(btnimg1_Click);
		this.PicProduct.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PicProduct.Location = new System.Drawing.Point(41, 21);
		this.PicProduct.Name = "PicProduct";
		this.PicProduct.Size = new System.Drawing.Size(125, 100);
		this.PicProduct.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.PicProduct.TabIndex = 39;
		this.PicProduct.TabStop = false;
		this.groupBox4.Controls.Add(this.panel9);
		this.groupBox4.Controls.Add(this.txtsalesTaxOnSales);
		this.groupBox4.Controls.Add(this.txt);
		this.groupBox4.Controls.Add(this.txtsalestaxonPurchase);
		this.groupBox4.Controls.Add(this.chkIsTaxable);
		this.groupBox4.Controls.Add(this.label7);
		this.groupBox4.Font = new System.Drawing.Font("Segoe UI", 10.2f);
		this.groupBox4.Location = new System.Drawing.Point(720, 152);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(441, 142);
		this.groupBox4.TabIndex = 3;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Taxation";
		this.panel9.Controls.Add(this.chktaxAtRetailPrice);
		this.panel9.Controls.Add(this.chktaxAtPurchasePrice);
		this.panel9.Location = new System.Drawing.Point(109, 71);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(217, 66);
		this.panel9.TabIndex = 128;
		this.chktaxAtRetailPrice.AutoSize = true;
		this.chktaxAtRetailPrice.Font = new System.Drawing.Font("Segoe UI", 10.2f);
		this.chktaxAtRetailPrice.Location = new System.Drawing.Point(3, 6);
		this.chktaxAtRetailPrice.Name = "chktaxAtRetailPrice";
		this.chktaxAtRetailPrice.Size = new System.Drawing.Size(133, 23);
		this.chktaxAtRetailPrice.TabIndex = 0;
		this.chktaxAtRetailPrice.Text = "Tax At Retail Price";
		this.chktaxAtRetailPrice.UseVisualStyleBackColor = true;
		this.chktaxAtPurchasePrice.AutoSize = true;
		this.chktaxAtPurchasePrice.Font = new System.Drawing.Font("Segoe UI", 10.2f);
		this.chktaxAtPurchasePrice.Location = new System.Drawing.Point(3, 29);
		this.chktaxAtPurchasePrice.Name = "chktaxAtPurchasePrice";
		this.chktaxAtPurchasePrice.Size = new System.Drawing.Size(121, 23);
		this.chktaxAtPurchasePrice.TabIndex = 1;
		this.chktaxAtPurchasePrice.Text = "Tax At Pur Price";
		this.chktaxAtPurchasePrice.UseVisualStyleBackColor = true;
		this.txtsalesTaxOnSales.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsalesTaxOnSales.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsalesTaxOnSales.Location = new System.Drawing.Point(214, 43);
		this.txtsalesTaxOnSales.Name = "txtsalesTaxOnSales";
		this.txtsalesTaxOnSales.Size = new System.Drawing.Size(111, 22);
		this.txtsalesTaxOnSales.TabIndex = 4;
		this.txtsalesTaxOnSales.Text = "0";
		this.txtsalesTaxOnSales.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txt.AutoSize = true;
		this.txt.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txt.Location = new System.Drawing.Point(214, 21);
		this.txt.Name = "txt";
		this.txt.Size = new System.Drawing.Size(114, 19);
		this.txt.TabIndex = 92;
		this.txt.Text = "Sales Tax (Sales)%";
		this.txtsalestaxonPurchase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsalestaxonPurchase.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsalestaxonPurchase.Location = new System.Drawing.Point(111, 43);
		this.txtsalestaxonPurchase.Name = "txtsalestaxonPurchase";
		this.txtsalestaxonPurchase.Size = new System.Drawing.Size(101, 22);
		this.txtsalestaxonPurchase.TabIndex = 1;
		this.txtsalestaxonPurchase.Text = "0";
		this.txtsalestaxonPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.chkIsTaxable.AutoSize = true;
		this.chkIsTaxable.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.chkIsTaxable.Location = new System.Drawing.Point(18, 24);
		this.chkIsTaxable.Name = "chkIsTaxable";
		this.chkIsTaxable.Size = new System.Drawing.Size(85, 23);
		this.chkIsTaxable.TabIndex = 0;
		this.chkIsTaxable.Text = "Is Taxable";
		this.chkIsTaxable.UseVisualStyleBackColor = true;
		this.chkIsTaxable.CheckedChanged += new System.EventHandler(chkIsTaxable_CheckedChanged);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(107, 21);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(105, 19);
		this.label7.TabIndex = 86;
		this.label7.Text = "Sales Tax (Pur)%";
		this.groupBox3.Controls.Add(this.label57);
		this.groupBox3.Controls.Add(this.label56);
		this.groupBox3.Controls.Add(this.label55);
		this.groupBox3.Controls.Add(this.label54);
		this.groupBox3.Controls.Add(this.txtEcnomicalStockLevel);
		this.groupBox3.Controls.Add(this.label13);
		this.groupBox3.Controls.Add(this.txtMaxStockLevel);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Controls.Add(this.txtReorderLevel);
		this.groupBox3.Controls.Add(this.label18);
		this.groupBox3.Controls.Add(this.txtMinStockLevel);
		this.groupBox3.Controls.Add(this.label17);
		this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 10.2f);
		this.groupBox3.Location = new System.Drawing.Point(720, 79);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(441, 78);
		this.groupBox3.TabIndex = 2;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Reorder ";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.ForeColor = System.Drawing.Color.Red;
		this.label57.Location = new System.Drawing.Point(339, 49);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(14, 17);
		this.label57.TabIndex = 586;
		this.label57.Text = "*";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.ForeColor = System.Drawing.Color.Red;
		this.label56.Location = new System.Drawing.Point(338, 19);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(14, 17);
		this.label56.TabIndex = 585;
		this.label56.Text = "*";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.ForeColor = System.Drawing.Color.Red;
		this.label55.Location = new System.Drawing.Point(140, 46);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(14, 17);
		this.label55.TabIndex = 584;
		this.label55.Text = "*";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.Red;
		this.label54.Location = new System.Drawing.Point(100, 20);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(14, 17);
		this.label54.TabIndex = 583;
		this.label54.Text = "*";
		this.txtEcnomicalStockLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEcnomicalStockLevel.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEcnomicalStockLevel.Location = new System.Drawing.Point(155, 47);
		this.txtEcnomicalStockLevel.Name = "txtEcnomicalStockLevel";
		this.txtEcnomicalStockLevel.Size = new System.Drawing.Size(75, 22);
		this.txtEcnomicalStockLevel.TabIndex = 2;
		this.txtEcnomicalStockLevel.Text = "0";
		this.txtEcnomicalStockLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(-1, 49);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(144, 19);
		this.label13.TabIndex = 84;
		this.label13.Text = "Ecnnomical Order Qty";
		this.txtMaxStockLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMaxStockLevel.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMaxStockLevel.Location = new System.Drawing.Point(354, 47);
		this.txtMaxStockLevel.Name = "txtMaxStockLevel";
		this.txtMaxStockLevel.Size = new System.Drawing.Size(82, 22);
		this.txtMaxStockLevel.TabIndex = 3;
		this.txtMaxStockLevel.Text = "0";
		this.txtMaxStockLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(234, 49);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(107, 19);
		this.label14.TabIndex = 83;
		this.label14.Text = "Max Stock Level";
		this.txtReorderLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtReorderLevel.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReorderLevel.Location = new System.Drawing.Point(155, 19);
		this.txtReorderLevel.Name = "txtReorderLevel";
		this.txtReorderLevel.Size = new System.Drawing.Size(75, 22);
		this.txtReorderLevel.TabIndex = 0;
		this.txtReorderLevel.Text = "0";
		this.txtReorderLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(5, 21);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(95, 19);
		this.label18.TabIndex = 80;
		this.label18.Text = "ReOrder Level";
		this.txtMinStockLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMinStockLevel.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMinStockLevel.Location = new System.Drawing.Point(354, 19);
		this.txtMinStockLevel.Name = "txtMinStockLevel";
		this.txtMinStockLevel.Size = new System.Drawing.Size(82, 22);
		this.txtMinStockLevel.TabIndex = 1;
		this.txtMinStockLevel.Text = "0";
		this.txtMinStockLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(235, 21);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(105, 19);
		this.label17.TabIndex = 79;
		this.label17.Text = "Min Stock Level";
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label52);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.label50);
		this.groupBox2.Controls.Add(this.txtRetailRate);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add(this.txtWholeSaleRate);
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Controls.Add(this.txtPurchaseRate);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.txtCostPrice);
		this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 10.2f);
		this.groupBox2.Location = new System.Drawing.Point(721, 1);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(441, 79);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Pricing ";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.Red;
		this.label53.Location = new System.Drawing.Point(82, 48);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(14, 17);
		this.label53.TabIndex = 589;
		this.label53.Text = "*";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.Red;
		this.label52.Location = new System.Drawing.Point(333, 48);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(14, 17);
		this.label52.TabIndex = 588;
		this.label52.Text = "*";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.ForeColor = System.Drawing.Color.Red;
		this.label51.Location = new System.Drawing.Point(311, 17);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(14, 17);
		this.label51.TabIndex = 587;
		this.label51.Text = "*";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.ForeColor = System.Drawing.Color.Red;
		this.label50.Location = new System.Drawing.Point(108, 17);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(14, 17);
		this.label50.TabIndex = 586;
		this.label50.Text = "*";
		this.txtRetailRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRetailRate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRetailRate.Location = new System.Drawing.Point(125, 45);
		this.txtRetailRate.Name = "txtRetailRate";
		this.txtRetailRate.Size = new System.Drawing.Size(78, 23);
		this.txtRetailRate.TabIndex = 2;
		this.txtRetailRate.Text = "0";
		this.txtRetailRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(10, 47);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(73, 19);
		this.label15.TabIndex = 87;
		this.label15.Text = "Retail Rate";
		this.txtWholeSaleRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWholeSaleRate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWholeSaleRate.Location = new System.Drawing.Point(348, 45);
		this.txtWholeSaleRate.Name = "txtWholeSaleRate";
		this.txtWholeSaleRate.Size = new System.Drawing.Size(82, 23);
		this.txtWholeSaleRate.TabIndex = 3;
		this.txtWholeSaleRate.Text = "0";
		this.txtWholeSaleRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(229, 47);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(107, 19);
		this.label27.TabIndex = 85;
		this.label27.Text = "Whole Sale Rate";
		this.txtPurchaseRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPurchaseRate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPurchaseRate.Location = new System.Drawing.Point(125, 17);
		this.txtPurchaseRate.Name = "txtPurchaseRate";
		this.txtPurchaseRate.Size = new System.Drawing.Size(78, 23);
		this.txtPurchaseRate.TabIndex = 0;
		this.txtPurchaseRate.Text = "0";
		this.txtPurchaseRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(10, 19);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(95, 19);
		this.label22.TabIndex = 84;
		this.label22.Text = "Purchase Rate";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(237, 19);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(68, 19);
		this.label20.TabIndex = 82;
		this.label20.Text = "Cost Rate";
		this.txtCostPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCostPrice.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCostPrice.Location = new System.Drawing.Point(348, 17);
		this.txtCostPrice.Name = "txtCostPrice";
		this.txtCostPrice.Size = new System.Drawing.Size(82, 23);
		this.txtCostPrice.TabIndex = 1;
		this.txtCostPrice.Text = "0";
		this.txtCostPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.label47);
		this.groupBox1.Controls.Add(this.label46);
		this.groupBox1.Controls.Add(this.label45);
		this.groupBox1.Controls.Add(this.label44);
		this.groupBox1.Controls.Add(this.label43);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.label41);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.label39);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.chkIsDiscountable);
		this.groupBox1.Controls.Add(this.chkisExpireable);
		this.groupBox1.Controls.Add(this.txtShelfLife);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPrimarySupplier);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbManufacture);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPurchaseGL);
		this.groupBox1.Controls.Add(this.txtManufacturerCode);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCGSGL);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSaleGL);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPurchaseType);
		this.groupBox1.Controls.Add(this.btnPurchaseAcc);
		this.groupBox1.Controls.Add(this.btnAddSupplier);
		this.groupBox1.Controls.Add(this.btnSaleAcc);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.btnCgsAcc);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSeason);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemReigion);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.txtHsCode);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbParentitem);
		this.groupBox1.Controls.Add(this.label37);
		this.groupBox1.Controls.Add(this.txtitemSpec);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.txtitemaliasname);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUomType);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.btnItemClass);
		this.groupBox1.Controls.Add(this.btnAddItemType);
		this.groupBox1.Controls.Add(this.btnAddBaseUnit);
		this.groupBox1.Controls.Add(this.btnAddItemCategory);
		this.groupBox1.Controls.Add(this.txtBarcodeNo);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.chkIsActive);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemClass);
		this.groupBox1.Controls.Add(this.txtPackQuantity);
		this.groupBox1.Controls.Add(this.txtItemName);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemType);
		this.groupBox1.Controls.Add(this.txtItemCode);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUnit);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCategory);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Location = new System.Drawing.Point(5, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(709, 401);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Item Define";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.Red;
		this.label49.Location = new System.Drawing.Point(394, 310);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(14, 17);
		this.label49.TabIndex = 585;
		this.label49.Text = "*";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.ForeColor = System.Drawing.Color.Red;
		this.label48.Location = new System.Drawing.Point(394, 283);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(14, 17);
		this.label48.TabIndex = 584;
		this.label48.Text = "*";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.ForeColor = System.Drawing.Color.Red;
		this.label47.Location = new System.Drawing.Point(420, 254);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(14, 17);
		this.label47.TabIndex = 583;
		this.label47.Text = "*";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.ForeColor = System.Drawing.Color.Red;
		this.label46.Location = new System.Drawing.Point(94, 359);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(14, 17);
		this.label46.TabIndex = 582;
		this.label46.Text = "*";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.Red;
		this.label45.Location = new System.Drawing.Point(83, 298);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(14, 17);
		this.label45.TabIndex = 581;
		this.label45.Text = "*";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.ForeColor = System.Drawing.Color.Red;
		this.label44.Location = new System.Drawing.Point(88, 244);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(14, 17);
		this.label44.TabIndex = 580;
		this.label44.Text = "*";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.Red;
		this.label43.Location = new System.Drawing.Point(72, 222);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(14, 17);
		this.label43.TabIndex = 579;
		this.label43.Text = "*";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.Red;
		this.label42.Location = new System.Drawing.Point(41, 196);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(14, 17);
		this.label42.TabIndex = 578;
		this.label42.Text = "*";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(76, 143);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(14, 17);
		this.label41.TabIndex = 577;
		this.label41.Text = "*";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.Red;
		this.label40.Location = new System.Drawing.Point(76, 117);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(14, 17);
		this.label40.TabIndex = 576;
		this.label40.Text = "*";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.ForeColor = System.Drawing.Color.Red;
		this.label39.Location = new System.Drawing.Point(71, 95);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(14, 17);
		this.label39.TabIndex = 575;
		this.label39.Text = "*";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.ForeColor = System.Drawing.Color.Red;
		this.label38.Location = new System.Drawing.Point(71, 69);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(14, 17);
		this.label38.TabIndex = 574;
		this.label38.Text = "*";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Red;
		this.label24.Location = new System.Drawing.Point(95, 15);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(14, 17);
		this.label24.TabIndex = 573;
		this.label24.Text = "*";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(65, 42);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(14, 17);
		this.label19.TabIndex = 572;
		this.label19.Text = "*";
		this.chkIsDiscountable.AutoSize = true;
		this.chkIsDiscountable.Font = new System.Drawing.Font("Segoe UI", 10.2f);
		this.chkIsDiscountable.Location = new System.Drawing.Point(357, 351);
		this.chkIsDiscountable.Name = "chkIsDiscountable";
		this.chkIsDiscountable.Size = new System.Drawing.Size(121, 23);
		this.chkIsDiscountable.TabIndex = 20;
		this.chkIsDiscountable.Text = "Is Discountable";
		this.chkIsDiscountable.UseVisualStyleBackColor = true;
		this.chkisExpireable.AutoSize = true;
		this.chkisExpireable.Font = new System.Drawing.Font("Segoe UI", 10.2f);
		this.chkisExpireable.Location = new System.Drawing.Point(484, 351);
		this.chkisExpireable.Name = "chkisExpireable";
		this.chkisExpireable.Size = new System.Drawing.Size(103, 23);
		this.chkisExpireable.TabIndex = 21;
		this.chkisExpireable.Text = "Is Expireable";
		this.chkisExpireable.UseVisualStyleBackColor = true;
		this.txtShelfLife.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtShelfLife.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtShelfLife.Location = new System.Drawing.Point(457, 120);
		this.txtShelfLife.Name = "txtShelfLife";
		this.txtShelfLife.Size = new System.Drawing.Size(204, 20);
		this.txtShelfLife.TabIndex = 18;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(335, 122);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(55, 15);
		this.label36.TabIndex = 109;
		this.label36.Text = "Shelf Life";
		this.cmbPrimarySupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbPrimarySupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPrimarySupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPrimarySupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance101;
		((SpecialBoxBase)((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance107).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPrimarySupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPrimarySupplier).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPrimarySupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPrimarySupplier).Location = new System.Drawing.Point(457, 93);
		((System.Windows.Forms.Control)(object)this.cmbPrimarySupplier).Name = "cmbPrimarySupplier";
		((System.Windows.Forms.Control)(object)this.cmbPrimarySupplier).Size = new System.Drawing.Size(204, 24);
		((System.Windows.Forms.Control)(object)this.cmbPrimarySupplier).TabIndex = 17;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(335, 97);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(98, 15);
		this.label35.TabIndex = 106;
		this.label35.Text = "Primary Supplier";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(326, 259);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(96, 15);
		this.label10.TabIndex = 19;
		this.label10.Text = "Purchase GL A/C";
		this.cmbManufacture.AllowNull = (DefaultableBoolean)1;
		this.cmbManufacture.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbManufacture.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbManufacture.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.Appearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbManufacture).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance83;
		((SpecialBoxBase)((UltraGridBase)this.cmbManufacture).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance90).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbManufacture).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbManufacture).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbManufacture).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbManufacture.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbManufacture).Location = new System.Drawing.Point(458, 42);
		((System.Windows.Forms.Control)(object)this.cmbManufacture).Name = "cmbManufacture";
		((System.Windows.Forms.Control)(object)this.cmbManufacture).Size = new System.Drawing.Size(204, 24);
		((System.Windows.Forms.Control)(object)this.cmbManufacture).TabIndex = 15;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(326, 315);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(70, 15);
		this.label12.TabIndex = 21;
		this.label12.Text = "CGS GL A/C";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(335, 46);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(80, 15);
		this.label33.TabIndex = 102;
		this.label33.Text = "Manufatcurer";
		this.cmbPurchaseGL.AllowNull = (DefaultableBoolean)1;
		this.cmbPurchaseGL.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPurchaseGL.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPurchaseGL.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Appearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance30;
		((SpecialBoxBase)((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPurchaseGL.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Location = new System.Drawing.Point(440, 254);
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Name = "cmbPurchaseGL";
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Size = new System.Drawing.Size(219, 25);
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).TabIndex = 23;
		this.txtManufacturerCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtManufacturerCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtManufacturerCode.Location = new System.Drawing.Point(457, 70);
		this.txtManufacturerCode.Name = "txtManufacturerCode";
		this.txtManufacturerCode.Size = new System.Drawing.Size(204, 21);
		this.txtManufacturerCode.TabIndex = 16;
		this.cmbCGSGL.AllowNull = (DefaultableBoolean)1;
		this.cmbCGSGL.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCGSGL.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCGSGL.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Appearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance114;
		((SpecialBoxBase)((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance120).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCGSGL.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Location = new System.Drawing.Point(440, 310);
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Name = "cmbCGSGL";
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Size = new System.Drawing.Size(219, 25);
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).TabIndex = 25;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(326, 287);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(69, 15);
		this.label11.TabIndex = 28;
		this.label11.Text = "Sale GL A/C";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(335, 71);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(112, 15);
		this.label34.TabIndex = 103;
		this.label34.Text = "Manufacturer Code";
		this.cmbSaleGL.AllowNull = (DefaultableBoolean)1;
		this.cmbSaleGL.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSaleGL.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSaleGL.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSaleGL).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance104;
		((SpecialBoxBase)((UltraGridBase)this.cmbSaleGL).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance159).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSaleGL.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).Location = new System.Drawing.Point(440, 282);
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).Name = "cmbSaleGL";
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).Size = new System.Drawing.Size(219, 25);
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).TabIndex = 24;
		this.cmbPurchaseType.AllowNull = (DefaultableBoolean)1;
		this.cmbPurchaseType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPurchaseType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPurchaseType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPurchaseType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance177;
		((SpecialBoxBase)((UltraGridBase)this.cmbPurchaseType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance4).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPurchaseType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPurchaseType).Font = new System.Drawing.Font("Segoe UI", 9f);
		this.cmbPurchaseType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPurchaseType).Location = new System.Drawing.Point(115, 354);
		((System.Windows.Forms.Control)(object)this.cmbPurchaseType).Name = "cmbPurchaseType";
		((System.Windows.Forms.Control)(object)this.cmbPurchaseType).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.cmbPurchaseType).TabIndex = 13;
		this.btnPurchaseAcc.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPurchaseAcc.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnPurchaseAcc.Location = new System.Drawing.Point(666, 256);
		this.btnPurchaseAcc.Name = "btnPurchaseAcc";
		this.btnPurchaseAcc.Size = new System.Drawing.Size(24, 22);
		this.btnPurchaseAcc.TabIndex = 85;
		this.btnPurchaseAcc.UseVisualStyleBackColor = true;
		this.btnPurchaseAcc.Click += new System.EventHandler(btnPurchaseAcc_Click);
		this.btnAddSupplier.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddSupplier.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnAddSupplier.Location = new System.Drawing.Point(668, 42);
		this.btnAddSupplier.Name = "btnAddSupplier";
		this.btnAddSupplier.Size = new System.Drawing.Size(24, 22);
		this.btnAddSupplier.TabIndex = 85;
		this.btnAddSupplier.UseVisualStyleBackColor = true;
		this.btnAddSupplier.Click += new System.EventHandler(btnAddSupplier_Click);
		this.btnSaleAcc.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaleAcc.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnSaleAcc.Location = new System.Drawing.Point(666, 285);
		this.btnSaleAcc.Name = "btnSaleAcc";
		this.btnSaleAcc.Size = new System.Drawing.Size(24, 22);
		this.btnSaleAcc.TabIndex = 85;
		this.btnSaleAcc.UseVisualStyleBackColor = true;
		this.btnSaleAcc.Click += new System.EventHandler(btnSaleAcc_Click);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.label31.Location = new System.Drawing.Point(7, 359);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(87, 15);
		this.label31.TabIndex = 101;
		this.label31.Text = "Purchases Type";
		this.btnCgsAcc.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCgsAcc.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnCgsAcc.Location = new System.Drawing.Point(666, 313);
		this.btnCgsAcc.Name = "btnCgsAcc";
		this.btnCgsAcc.Size = new System.Drawing.Size(24, 22);
		this.btnCgsAcc.TabIndex = 85;
		this.btnCgsAcc.UseVisualStyleBackColor = true;
		this.btnCgsAcc.Click += new System.EventHandler(btnCgsAcc_Click);
		this.cmbSeason.AllowNull = (DefaultableBoolean)1;
		this.cmbSeason.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSeason.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSeason.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSeason).DisplayLayout.Appearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbSeason).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSeason).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSeason).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSeason).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbSeason).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSeason).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbSeason).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSeason).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSeason).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSeason).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbSeason).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSeason).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSeason).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance62).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSeason).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbSeason).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSeason).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSeason).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSeason).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbSeason).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSeason).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSeason).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbSeason).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSeason).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbSeason).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSeason).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSeason).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSeason).Font = new System.Drawing.Font("Segoe UI", 9f);
		this.cmbSeason.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSeason).Location = new System.Drawing.Point(115, 324);
		((System.Windows.Forms.Control)(object)this.cmbSeason).Name = "cmbSeason";
		((System.Windows.Forms.Control)(object)this.cmbSeason).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.cmbSeason).TabIndex = 12;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.label30.Location = new System.Drawing.Point(15, 329);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(44, 15);
		this.label30.TabIndex = 99;
		this.label30.Text = "Season";
		this.cmbItemReigion.AllowNull = (DefaultableBoolean)1;
		this.cmbItemReigion.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemReigion.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemReigion.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemReigion).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemReigion).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemReigion).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemReigion).Font = new System.Drawing.Font("Segoe UI", 9f);
		this.cmbItemReigion.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemReigion).Location = new System.Drawing.Point(115, 295);
		((System.Windows.Forms.Control)(object)this.cmbItemReigion).Name = "cmbItemReigion";
		((System.Windows.Forms.Control)(object)this.cmbItemReigion).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemReigion).TabIndex = 11;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.label29.Location = new System.Drawing.Point(11, 300);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(74, 15);
		this.label29.TabIndex = 97;
		this.label29.Text = "Item Reigion";
		this.txtHsCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtHsCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtHsCode.Location = new System.Drawing.Point(458, 16);
		this.txtHsCode.Name = "txtHsCode";
		this.txtHsCode.Size = new System.Drawing.Size(204, 20);
		this.txtHsCode.TabIndex = 14;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(335, 17);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(53, 15);
		this.label28.TabIndex = 95;
		this.label28.Text = "HS Code";
		this.cmbParentitem.AllowNull = (DefaultableBoolean)1;
		this.cmbParentitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbParentitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbParentitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance49).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbParentitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbParentitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbParentitem).Font = new System.Drawing.Font("Segoe UI", 9f);
		this.cmbParentitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbParentitem).Location = new System.Drawing.Point(115, 266);
		((System.Windows.Forms.Control)(object)this.cmbParentitem).Name = "cmbParentitem";
		((System.Windows.Forms.Control)(object)this.cmbParentitem).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.cmbParentitem).TabIndex = 10;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.label37.Location = new System.Drawing.Point(11, 271);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(68, 15);
		this.label37.TabIndex = 93;
		this.label37.Text = "Parent Item";
		this.txtitemSpec.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtitemSpec.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtitemSpec.Location = new System.Drawing.Point(440, 194);
		this.txtitemSpec.Multiline = true;
		this.txtitemSpec.Name = "txtitemSpec";
		this.txtitemSpec.Size = new System.Drawing.Size(221, 57);
		this.txtitemSpec.TabIndex = 19;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(338, 197);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(102, 15);
		this.label26.TabIndex = 91;
		this.label26.Text = "Item Specification";
		this.txtitemaliasname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtitemaliasname.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtitemaliasname.Location = new System.Drawing.Point(115, 165);
		this.txtitemaliasname.Name = "txtitemaliasname";
		this.txtitemaliasname.Size = new System.Drawing.Size(546, 21);
		this.txtitemaliasname.TabIndex = 6;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(-1, 168);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(99, 15);
		this.label9.TabIndex = 89;
		this.label9.Text = "Item Other Name";
		this.cmbUomType.AllowNull = (DefaultableBoolean)1;
		this.cmbUomType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUomType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUomType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUomType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbUomType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUomType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUomType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUomType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance154;
		((SpecialBoxBase)((UltraGridBase)this.cmbUomType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUomType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbUomType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUomType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUomType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUomType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbUomType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUomType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUomType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance160).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUomType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbUomType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUomType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUomType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUomType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbUomType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUomType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUomType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbUomType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUomType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbUomType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUomType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUomType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUomType).Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUomType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUomType).Location = new System.Drawing.Point(116, 216);
		((System.Windows.Forms.Control)(object)this.cmbUomType).Name = "cmbUomType";
		((System.Windows.Forms.Control)(object)this.cmbUomType).Size = new System.Drawing.Size(189, 22);
		((System.Windows.Forms.Control)(object)this.cmbUomType).TabIndex = 8;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(11, 222);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(62, 15);
		this.label16.TabIndex = 87;
		this.label16.Text = "UOM Type";
		this.btnItemClass.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnItemClass.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnItemClass.Location = new System.Drawing.Point(310, 70);
		this.btnItemClass.Name = "btnItemClass";
		this.btnItemClass.Size = new System.Drawing.Size(24, 22);
		this.btnItemClass.TabIndex = 85;
		this.btnItemClass.UseVisualStyleBackColor = true;
		this.btnItemClass.Click += new System.EventHandler(btnItemClass_Click);
		this.btnAddItemType.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddItemType.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnAddItemType.Location = new System.Drawing.Point(310, 46);
		this.btnAddItemType.Name = "btnAddItemType";
		this.btnAddItemType.Size = new System.Drawing.Size(24, 22);
		this.btnAddItemType.TabIndex = 85;
		this.btnAddItemType.UseVisualStyleBackColor = true;
		this.btnAddItemType.Click += new System.EventHandler(btnAddItemType_Click);
		this.btnAddBaseUnit.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddBaseUnit.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnAddBaseUnit.Location = new System.Drawing.Point(311, 194);
		this.btnAddBaseUnit.Name = "btnAddBaseUnit";
		this.btnAddBaseUnit.Size = new System.Drawing.Size(24, 22);
		this.btnAddBaseUnit.TabIndex = 85;
		this.btnAddBaseUnit.UseVisualStyleBackColor = true;
		this.btnAddBaseUnit.Click += new System.EventHandler(btnAddBaseUnit_Click);
		this.btnAddItemCategory.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddItemCategory.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnAddItemCategory.Location = new System.Drawing.Point(310, 18);
		this.btnAddItemCategory.Name = "btnAddItemCategory";
		this.btnAddItemCategory.Size = new System.Drawing.Size(24, 22);
		this.btnAddItemCategory.TabIndex = 39;
		this.btnAddItemCategory.UseVisualStyleBackColor = true;
		this.btnAddItemCategory.Click += new System.EventHandler(btnAddItemCategory_Click);
		this.txtBarcodeNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBarcodeNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBarcodeNo.Location = new System.Drawing.Point(116, 117);
		this.txtBarcodeNo.Name = "txtBarcodeNo";
		this.txtBarcodeNo.Size = new System.Drawing.Size(188, 21);
		this.txtBarcodeNo.TabIndex = 4;
		this.txtBarcodeNo.TextChanged += new System.EventHandler(txtBarcodeNo_TextChanged);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(10, 120);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(69, 15);
		this.label21.TabIndex = 84;
		this.label21.Text = "Barcode No";
		this.chkIsActive.AutoSize = true;
		this.chkIsActive.Checked = true;
		this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkIsActive.Font = new System.Drawing.Font("Segoe UI", 10.2f);
		this.chkIsActive.Location = new System.Drawing.Point(593, 351);
		this.chkIsActive.Name = "chkIsActive";
		this.chkIsActive.Size = new System.Drawing.Size(79, 23);
		this.chkIsActive.TabIndex = 22;
		this.chkIsActive.Text = "Is Active";
		this.chkIsActive.UseVisualStyleBackColor = true;
		this.cmbItemClass.AllowNull = (DefaultableBoolean)1;
		this.cmbItemClass.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemClass.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemClass.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Appearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance70;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance76).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemClass.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Location = new System.Drawing.Point(115, 68);
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Name = "cmbItemClass";
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Size = new System.Drawing.Size(189, 22);
		((System.Windows.Forms.Control)(object)this.cmbItemClass).TabIndex = 2;
		this.txtPackQuantity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackQuantity.Location = new System.Drawing.Point(115, 243);
		this.txtPackQuantity.Name = "txtPackQuantity";
		this.txtPackQuantity.Size = new System.Drawing.Size(190, 20);
		this.txtPackQuantity.TabIndex = 9;
		this.txtPackQuantity.Text = "0";
		this.txtItemName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemName.Location = new System.Drawing.Point(115, 142);
		this.txtItemName.Name = "txtItemName";
		this.txtItemName.Size = new System.Drawing.Size(546, 21);
		this.txtItemName.TabIndex = 5;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(10, 247);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(81, 15);
		this.label8.TabIndex = 33;
		this.label8.Text = "Pack Quantity";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(8, 72);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(61, 15);
		this.label6.TabIndex = 32;
		this.label6.Text = "Item Class";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(8, 145);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(66, 15);
		this.label2.TabIndex = 29;
		this.label2.Text = "Item Name";
		this.cmbItemType.AllowNull = (DefaultableBoolean)1;
		this.cmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance141;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance146).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance149).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Location = new System.Drawing.Point(115, 44);
		((System.Windows.Forms.Control)(object)this.cmbItemType).Name = "cmbItemType";
		((System.Windows.Forms.Control)(object)this.cmbItemType).Size = new System.Drawing.Size(189, 22);
		((System.Windows.Forms.Control)(object)this.cmbItemType).TabIndex = 1;
		this.txtItemCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemCode.Location = new System.Drawing.Point(115, 93);
		this.txtItemCode.Name = "txtItemCode";
		this.txtItemCode.ReadOnly = true;
		this.txtItemCode.Size = new System.Drawing.Size(189, 21);
		this.txtItemCode.TabIndex = 3;
		this.txtItemCode.TabStop = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(8, 48);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(58, 15);
		this.label5.TabIndex = 26;
		this.label5.Text = "Item Type";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(8, 98);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(62, 15);
		this.label1.TabIndex = 23;
		this.label1.Text = "Item Code";
		this.cmbUnit.AllowNull = (DefaultableBoolean)1;
		this.cmbUnit.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUnit.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUnit.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUnit).DisplayLayout.Appearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbUnit).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUnit).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUnit).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUnit).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance128;
		((SpecialBoxBase)((UltraGridBase)this.cmbUnit).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUnit).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbUnit).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUnit).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUnit).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUnit).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbUnit).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUnit).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUnit).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance133).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUnit).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbUnit).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUnit).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUnit).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUnit).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbUnit).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUnit).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUnit).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbUnit).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUnit).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbUnit).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUnit).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUnit).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUnit).Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUnit.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUnit).Location = new System.Drawing.Point(116, 192);
		((System.Windows.Forms.Control)(object)this.cmbUnit).Name = "cmbUnit";
		((System.Windows.Forms.Control)(object)this.cmbUnit).Size = new System.Drawing.Size(189, 22);
		((System.Windows.Forms.Control)(object)this.cmbUnit).TabIndex = 7;
		this.cmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Location = new System.Drawing.Point(115, 18);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Name = "cmbItemCategory";
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Size = new System.Drawing.Size(189, 22);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Leave += new System.EventHandler(cmbItemCategory_Leave);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(8, 22);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(82, 15);
		this.label4.TabIndex = 16;
		this.label4.Text = "Item Category";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(11, 198);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(29, 15);
		this.label3.TabIndex = 14;
		this.label3.Text = "Unit";
		this.panel3.Controls.Add(this.btnclose);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1347, 31);
		this.panel3.TabIndex = 0;
		this.btnclose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnclose.BackColor = System.Drawing.Color.WhiteSmoke;
		this.btnclose.FlatAppearance.BorderSize = 0;
		this.btnclose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnclose.Image = (System.Drawing.Image)resources.GetObject("btnclose.Image");
		this.btnclose.Location = new System.Drawing.Point(1310, 1);
		this.btnclose.Margin = new System.Windows.Forms.Padding(2);
		this.btnclose.Name = "btnclose";
		this.btnclose.Size = new System.Drawing.Size(35, 25);
		this.btnclose.TabIndex = 37;
		this.btnclose.UseVisualStyleBackColor = false;
		this.btnclose.Click += new System.EventHandler(btnclose_Click);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.toolStripButton2, this.btnnew, this.btnupdate, this.btnsave, this.btnprint, this.btnattachment, this.btnRefresh, this.btnBarcodeGenerate, this.btnPrintBarcode });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1347, 28);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripButton2.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(108, 25);
		this.toolStripButton2.Text = "Add Item      ";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 25);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(69, 25);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 25);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnprint.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnprint.Image = (System.Drawing.Image)resources.GetObject("btnprint.Image");
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(56, 25);
		this.btnprint.Text = "&Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.btnattachment.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = (System.Drawing.Image)resources.GetObject("btnattachment.Image");
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(94, 25);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(50, 25);
		this.btnRefresh.Text = "Refresh";
		this.btnBarcodeGenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnBarcodeGenerate.Name = "btnBarcodeGenerate";
		this.btnBarcodeGenerate.Size = new System.Drawing.Size(104, 25);
		this.btnBarcodeGenerate.Text = "Generate Barcode";
		this.btnBarcodeGenerate.Click += new System.EventHandler(btnBarcodeGenerate_Click);
		this.btnPrintBarcode.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintBarcode.Name = "btnPrintBarcode";
		this.btnPrintBarcode.Size = new System.Drawing.Size(82, 25);
		this.btnPrintBarcode.Text = "Print Barcode";
		this.btnPrintBarcode.Click += new System.EventHandler(btnPrintBarcode_Click);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1353, 559);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1347, 553);
		this.panel5.TabIndex = 0;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistoryForHistory);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 80);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1347, 473);
		this.panel8.TabIndex = 2;
		this.grdhistoryForHistory.AllowDelete = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdhistoryForHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistoryForHistory.FilterMode = (FilterMode)2;
		this.grdhistoryForHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistoryForHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistoryForHistory.GroupByBoxVisible = false;
		this.grdhistoryForHistory.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdhistoryForHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistoryForHistory).Name = "grdhistoryForHistory";
		((ScrollableBase)this.grdhistoryForHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdhistoryForHistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdhistoryForHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistoryForHistory).Size = new System.Drawing.Size(1347, 473);
		((System.Windows.Forms.Control)(object)this.grdhistoryForHistory).TabIndex = 78;
		this.grdhistoryForHistory.TotalRow = (InheritableBoolean)1;
		this.grdhistoryForHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistoryForHistory).VisualStyle = (VisualStyle)4;
		this.grdhistoryForHistory.LinkClicked += new ColumnActionEventHandler(grdhistoryForHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistoryForHistory).DoubleClick += new System.EventHandler(grdhistoryForHistory_DoubleClick);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitemcathistory);
		this.panel7.Controls.Add(this.btnsearch);
		this.panel7.Controls.Add(this.label32);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 31);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1347, 49);
		this.panel7.TabIndex = 1;
		this.cmbitemcathistory.AllowNull = (DefaultableBoolean)1;
		this.cmbitemcathistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitemcathistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitemcathistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance167;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance173).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitemcathistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Location = new System.Drawing.Point(108, 11);
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Name = "cmbitemcathistory";
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).TabIndex = 19;
		this.btnsearch.FlatAppearance.BorderSize = 0;
		this.btnsearch.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.btnsearch.Location = new System.Drawing.Point(340, 11);
		this.btnsearch.Name = "btnsearch";
		this.btnsearch.Size = new System.Drawing.Size(90, 26);
		this.btnsearch.TabIndex = 18;
		this.btnsearch.Text = "Search";
		this.btnsearch.UseVisualStyleBackColor = true;
		this.btnsearch.Click += new System.EventHandler(btnsearch_Click);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label32.Location = new System.Drawing.Point(9, 16);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(90, 17);
		this.label32.TabIndex = 17;
		this.label32.Text = "Item Category";
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1347, 31);
		this.panel6.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1299, -2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdhistoryForHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(43, 28);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripButton1, this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1347, 28);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripButton1.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(141, 25);
		this.toolStripButton1.Text = "Item History        ";
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(54, 25);
		this.btnLoadAll.Text = "Load All";
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1361, 585);
		base.Controls.Add(this.panel1);
		base.Name = "InvAddItemsPOS";
		this.Text = "InvAddItemsPOS";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvAddItemsPOS_FormClosing);
		base.Load += new System.EventHandler(InvAddItemsPOS_Load);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.groupBox7.ResumeLayout(false);
		this.flowLayoutPanel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdcompany).EndInit();
		this.groupBox6.ResumeLayout(false);
		this.groupBox6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.PicBarcode).EndInit();
		this.groupBox5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.PicProduct).EndInit();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPrimarySupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbManufacture).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPurchaseGL).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCGSGL).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSaleGL).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPurchaseType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSeason).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemReigion).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUomType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemClass).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUnit).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistoryForHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbitemcathistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
