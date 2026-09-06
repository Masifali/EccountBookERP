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
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Taxation;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Inventory_Definition;

public class frmAddItemConcrete : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

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

	private CheckBox chkstatus;

	private Label label33;

	internal UltraCombo cmbSaleGL;

	internal UltraCombo cmbPurchaseGL;

	private TextBox txtItemCode;

	private TextBox txtItemName;

	private Label label11;

	private Label label10;

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

	private Button button1;

	private GroupBox groupBox1;

	internal GridEX grdAllocation;

	private Button btnItemAllocation;

	private Panel panelDetails;

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

	private Label label8;

	internal UltraCombo CmbQualityGrade;

	private Label label9;

	private TextBox txtMaxMeaurement;

	internal UltraCombo CmbMaxMrmtUnit;

	private Label label7;

	private Label label35;

	private TextBox txtWeightKgs;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private Button btnQualityGrate;

	private Button BtnAttributeVarientAllocationtoItem;

	private RadioButton rdWithoutVariant;

	private RadioButton rdWithVariant;

	public frmAddItemConcrete(UserAccount pUserAccount)
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
			pnlimage.Visible = false;
			dtViewAttachments.Columns.Add("AttchmentPath");
			IfConfigAllAccountAllowOn = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("All Account Allow on Item and Item Category"));
			ItemTypeFill();
			ItemClassFill();
			PurchaseGLAcFill();
			SupplierBuyerfill();
			cmbcountryfill();
			ItemCatagoryHistoryFill();
			ItemQcGradeFill();
			CmbTaxTypeFill();
			BaseUnitFill();
			CompaniesBindInGrid();
			AmountCaluculation();
			panelDetails.Hide();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			cmbItemCategory.Focus();
			grpUpdateItemName.Visible = false;
			rdWithVariant.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemCatagoryHistoryFill()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(cmbitemcathistory.Value);
			int Idf = Conversion.ToInt(cmbItemCategory.Value);
			ItemCategory cat = new ItemCategory();
			cat.OrganizationId = UserAccount.OrganizationId;
			cat.CompanyId = UserAccount.CompanyId;
			dtCatergory = ItemCategory.Getall(cat);
			if (dtCatergory.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtcath = new DataTable();
			dtcath.Columns.Add("Id", typeof(int));
			dtcath.Columns.Add("CategoryDescription");
			for (int i = 0; i < dtCatergory.Rows.Count; i++)
			{
				dtcath.Rows.Add(dtCatergory.Rows[i]["Id"], dtCatergory.Rows[i]["CategoryDescription"]);
			}
			if (dtcath.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtcath, cmbitemcathistory, "Id", "CategoryDescription", "Item Category", ZeroIndex: true);
			DDL.BindDDL(dtcath, cmbItemCategory, "Id", "CategoryDescription", "Item Category", ZeroIndex: true);
			if (Id > 0)
			{
				if (dtcath.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					cmbitemcathistory.Value = Id;
				}
				else
				{
					((Control)(object)cmbitemcathistory).Text = string.Empty;
				}
			}
			if (Idf > 0)
			{
				if (dtcath.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idf))
				{
					cmbItemCategory.Value = Idf;
				}
				else
				{
					((Control)(object)cmbItemCategory).Text = string.Empty;
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
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(cmbBaseUnit.Value);
			int Idf = Conversion.ToInt(CmbMaxMrmtUnit.Value);
			DataTable dtuom = UOM.Getall(new UOM
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtuom.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("UomCode");
			dt.Columns.Add("Equivalent", typeof(double));
			for (int i = 0; i < dtuom.Rows.Count; i++)
			{
				dt.Rows.Add(dtuom.Rows[i]["Id"], dtuom.Rows[i]["UOMCode"], dtuom.Rows[i]["Equivalent"]);
			}
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dt, cmbBaseUnit, "Id", "UOMCode", "UOM Code", ZeroIndex: true);
			DDL.BindDDL(dt, CmbMaxMrmtUnit, "Id", "UOMCode", "UOM Code", ZeroIndex: true);
			((UltraGridBase)cmbBaseUnit).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbMaxMrmtUnit).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					cmbBaseUnit.Value = Id;
				}
				else
				{
					((Control)(object)cmbBaseUnit).Text = string.Empty;
				}
			}
			if (Idf > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idf))
				{
					CmbMaxMrmtUnit.Value = Idf;
				}
				else
				{
					((Control)(object)CmbMaxMrmtUnit).Text = string.Empty;
				}
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
			int Id = Conversion.ToInt(cmbItemType.Value);
			DataTable dt = CommonServices.GetItemTypeForComboServiceBind();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, cmbItemType, "Id", "TypeDescription", "Item Type", true);
			((UltraGridBase)cmbItemType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					cmbItemType.Value = Id;
				}
				else
				{
					((Control)(object)cmbItemType).Text = string.Empty;
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int Id = Conversion.ToInt(cmbItemClass.Value);
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
			dtType.Columns.Add("ClassId", typeof(int));
			dtType.Columns.Add("ClassDescription");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToInt(dt.Rows[i]["ClassId"]) == 1 || Conversion.ToInt(dt.Rows[i]["ClassId"]) == 6 || Conversion.ToInt(dt.Rows[i]["ClassId"]) == 7 || Conversion.ToInt(dt.Rows[i]["ClassId"]) == 8)
				{
					dtType.Rows.Add(dt.Rows[i]["ClassId"], dt.Rows[i]["ClassDescription"]);
				}
			}
			if (dtType.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtType, cmbItemClass, "ClassId", "ClassDescription", "Item Class", ZeroIndex: true);
			((UltraGridBase)cmbItemClass).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Id > 0)
			{
				if (dtType.AsEnumerable().Any((DataRow row) => row.Field<int>("ClassId") == Id))
				{
					cmbItemClass.Value = Id;
				}
				else
				{
					((Control)(object)cmbItemClass).Text = string.Empty;
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
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		try
		{
			int Id1 = Conversion.ToInt(cmbPurchaseGL.Value);
			int Id2 = Conversion.ToInt(cmbSaleGL.Value);
			int Id3 = Conversion.ToInt(cmbCGSGL.Value);
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
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("AccountTitle");
				DataTable dtsale = new DataTable();
				dtsale.Columns.Add("Id", typeof(int));
				dtsale.Columns.Add("AccountTitle");
				DataTable dtcgs = new DataTable();
				dtcgs.Columns.Add("Id", typeof(int));
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
					DDL.BindDDL(dt, cmbPurchaseGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: true);
					if (Id1 > 0)
					{
						if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id1))
						{
							cmbPurchaseGL.Value = Id1;
						}
						else
						{
							((Control)(object)cmbPurchaseGL).Text = string.Empty;
						}
					}
				}
				if (dtsale.Rows.Count > 0)
				{
					DDL.BindDDL(dtsale, cmbSaleGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: true);
					if (Id2 > 0)
					{
						if (dtsale.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id2))
						{
							cmbSaleGL.Value = Id2;
						}
						else
						{
							((Control)(object)cmbSaleGL).Text = string.Empty;
						}
					}
				}
				if (dtcgs.Rows.Count <= 0)
				{
					return;
				}
				DDL.BindDDL(dtcgs, cmbCGSGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: true);
				if (Id3 > 0)
				{
					if (dtsale.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id3))
					{
						cmbCGSGL.Value = Id3;
					}
					else
					{
						((Control)(object)cmbCGSGL).Text = string.Empty;
					}
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
			DDL.BindDDL(dt2, cmbPurchaseGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: true);
			DDL.BindDDL(dt2, cmbSaleGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: true);
			DDL.BindDDL(dt2, cmbCGSGL, "Id", "AccountTitle", "Account Tile", ZeroIndex: true);
			if (Id1 > 0)
			{
				if (dt2.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id1))
				{
					cmbPurchaseGL.Value = Id1;
				}
				else
				{
					((Control)(object)cmbPurchaseGL).Text = string.Empty;
				}
			}
			if (Id2 > 0)
			{
				if (dt2.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id2))
				{
					cmbSaleGL.Value = Id2;
				}
				else
				{
					((Control)(object)cmbSaleGL).Text = string.Empty;
				}
			}
			if (Id3 > 0)
			{
				if (dt2.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id3))
				{
					cmbCGSGL.Value = Id3;
				}
				else
				{
					((Control)(object)cmbCGSGL).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemQcGradeFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int Id1 = Conversion.ToInt(CmbQualityGrade.Value);
			DataTable lst = ItemQcGrade.FormHistory(new ItemQcGrade
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("QcGradeDesc");
			dt.Rows.Clear();
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["QcGradeDesc"]);
			}
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dt, CmbQualityGrade, "Id", "QcGradeDesc", "Quality Grade", ZeroIndex: true);
			((UltraGridBase)CmbQualityGrade).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Id1 > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id1))
				{
					CmbQualityGrade.Value = Id1;
				}
				else
				{
					((Control)(object)CmbQualityGrade).Text = string.Empty;
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int Id1 = Conversion.ToInt(cmbTaxType.Value);
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
			dtType.Columns.Add("Id", typeof(int));
			dtType.Columns.Add("TaxName");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Convert.ToInt32(dt.Rows[i]["Type"]) != 1)
				{
					dtType.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["TaxName"]);
				}
			}
			if (dtType.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtType, cmbTaxType, "Id", "TaxName", "TaxName", ZeroIndex: false);
			((UltraGridBase)cmbTaxType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Id1 > 0)
			{
				if (dtType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id1))
				{
					cmbTaxType.Value = Id1;
				}
				else
				{
					((Control)(object)cmbTaxType).Text = string.Empty;
				}
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
				dtType.Columns.Add("Id", typeof(int));
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
			dt.Columns.Add("Id", typeof(int));
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
			((UltraGridBase)cmbItemClass).Rows[1].Activate();
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
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

	private bool FormValidationForAddItem()
	{
		if (((UltraGridBase)cmbItemCategory).ActiveRow == null || Conversion.ToInt(cmbItemCategory.Value) == 0)
		{
			MessageBox.Show("Item Category Field is Required");
			cmbItemCategory.Focus();
			return false;
		}
		if (txtItemName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Item Name Field is Required");
			txtItemName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbQualityGrade).ActiveRow == null || Conversion.ToInt(CmbQualityGrade.Value) == 0)
		{
			MessageBox.Show("Quality Grade Field is Required");
			CmbQualityGrade.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemType).ActiveRow == null || Conversion.ToInt(cmbItemType.Value) == 0)
		{
			MessageBox.Show("Item Type Field is Required");
			cmbItemType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemClass).ActiveRow == null || Conversion.ToInt(cmbItemClass.Value) == 0)
		{
			MessageBox.Show("Item Class Field is Required");
			cmbItemClass.Focus();
			return false;
		}
		if (((UltraGridBase)cmbBaseUnit).ActiveRow == null || Conversion.ToInt(cmbBaseUnit.Value) == 0)
		{
			MessageBox.Show("Base Unit Field is Required");
			cmbBaseUnit.Focus();
			return false;
		}
		if (Conversion.ToDecimal((object)txtWeightKgs.Text) == 0m || txtWeightKgs.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Weight (Kgs) Field is Required");
			txtWeightKgs.Focus();
			return false;
		}
		if (Conversion.ToDecimal((object)txtMaxMeaurement.Text) == 0m || txtMaxMeaurement.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Max Meaurement Field is Required");
			txtMaxMeaurement.Focus();
			return false;
		}
		if (((UltraGridBase)CmbMaxMrmtUnit).ActiveRow == null || Conversion.ToInt(CmbMaxMrmtUnit.Value) == 0)
		{
			MessageBox.Show("Max Meaurement Unit Field is Required");
			CmbMaxMrmtUnit.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPurchaseGL).ActiveRow == null || Conversion.ToInt(cmbPurchaseGL.Value) == 0)
		{
			MessageBox.Show("Purchase GL A/C Field is Required");
			cmbPurchaseGL.Focus();
			return false;
		}
		if (((UltraGridBase)cmbSaleGL).ActiveRow == null || Conversion.ToInt(cmbSaleGL.Value) == 0)
		{
			MessageBox.Show("Sale GL A/C Field is Required");
			cmbSaleGL.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCGSGL).ActiveRow == null || Conversion.ToInt(cmbCGSGL.Value) == 0)
		{
			MessageBox.Show("COGS GL A/c Filed is Required");
			cmbCGSGL.Focus();
			return false;
		}
		if (!rdWithVariant.Checked && !rdWithoutVariant.Checked)
		{
			MessageBox.Show("Please Select One Option With Or Without Variant");
			rdWithVariant.Focus();
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
			pnlimage.Visible = false;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			((Control)(object)cmbAmountCalcType).Text = string.Empty;
			((Control)(object)cmbBuyerOrSupplier).Text = string.Empty;
			((Control)(object)cmbCGSGL).Text = string.Empty;
			((Control)(object)cmbCostUOM).Text = string.Empty;
			((Control)(object)cmbcountry).Text = string.Empty;
			((Control)(object)cmbManufacturerID).Text = string.Empty;
			((Control)(object)cmbPurchaseGL).Text = string.Empty;
			((Control)(object)cmbPurchaseUOM).Text = string.Empty;
			((Control)(object)cmbSaleGL).Text = string.Empty;
			((Control)(object)cmbSaleUOM).Text = string.Empty;
			txtItemCode.Clear();
			txtBuyerPartNo.Clear();
			txtCostPrice.Clear();
			txtItemName.Clear();
			txtManufacturerPartNo.Clear();
			txtMaxStockLevel.Clear();
			txtProductNumber.Clear();
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
			rdWithoutVariant.Checked = false;
			rdWithVariant.Checked = false;
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
		((Control)(object)cmbItemClass).Text = string.Empty;
		((Control)(object)CmbQualityGrade).Text = string.Empty;
		txtMaxMeaurement.Text = string.Empty;
		txtWeightKgs.Text = string.Empty;
		((Control)(object)CmbMaxMrmtUnit).Text = string.Empty;
		chkCommissionOnPurchase.Checked = true;
		chkCommissionOnPurchase.Visible = true;
		chkCommissionOnSale.Checked = true;
		chkCommissionOnSale.Visible = true;
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
			ItemQcGradeFill();
			CmbTaxTypeFill();
			BaseUnitFill();
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

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_0598: Expected O, but got Unknown
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Expected O, but got Unknown
		//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Expected O, but got Unknown
		try
		{
			if (!FormValidationForAddItem())
			{
				return;
			}
			Item items = new Item();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				items.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			items.OrganizationId = UserAccount.OrganizationId;
			items.CompanyId = UserAccount.CompanyId;
			items.BranchesId = UserAccount.BranchesId;
			items.EntryUser = UserAccount.ID;
			items.ModifyUser = UserAccount.ID;
			items.ItemCategoryId = Conversion.ToInt(cmbItemCategory.Value);
			items.ItemName = Conversion.ToString((object)txtItemName.Text.Trim());
			items.ItemCode = Conversion.ToString((object)txtItemCode.Text.Trim());
			items.ItemCodeNew = Conversion.ToString((object)txtItemCodeNew.Text.Trim());
			items.ItemQcGradeId = Conversion.ToInt(CmbQualityGrade.Value);
			items.ItemTypeId = Conversion.ToInt(cmbItemType.Value);
			items.ItemClassId = Conversion.ToInt(cmbItemClass.Value);
			items.BaseUnitId = Conversion.ToInt(cmbBaseUnit.Value);
			items.Equivalent = Conversion.ToDouble(((UltraDropDownBase)cmbBaseUnit).SelectedRow.Cells[2].Value);
			items.WeightKgs = Conversion.ToDecimal((object)txtWeightKgs.Text.Trim());
			items.MaxMeaurement = Conversion.ToDecimal((object)txtMaxMeaurement.Text.Trim());
			items.MaxMeaurementUnitId = Conversion.ToInt(CmbMaxMrmtUnit.Value);
			items.PurchaseGLAC = Conversion.ToInt(cmbPurchaseGL.Value);
			items.SaleGLAC = Conversion.ToInt(cmbSaleGL.Value);
			items.COGSGLAC = Conversion.ToInt(cmbCGSGL.Value);
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
			items.ItemStatus = Conversion.ToBool((object)chkstatus.Checked);
			items.Pic1 = Conversion.ToString((object)fileSavePath);
			items.Pic2 = Conversion.ToString((object)fileSavePath2);
			items.ManufactureId = Conversion.ToInt(cmbManufacturerID.Value);
			items.ManufacturePartNo = Conversion.ToString((object)txtManufacturerPartNo.Text.Trim());
			items.BuyerSupplierId = Conversion.ToInt(cmbBuyerOrSupplier.Value);
			items.BuyerPartNo = Conversion.ToString((object)txtBuyerPartNo.Text.Trim());
			items.ProductNo = Conversion.ToString((object)txtProductNumber.Text.Trim());
			items.ItemGroupId = 0;
			items.WholeSalePrice = Conversion.ToDouble((object)txtPurchaseRate.Text);
			items.BaseCropYear = "";
			int countfalsevalue = 0;
			items.MaxStockLevel = Conversion.ToInt((object)txtMaxStockLevel.Text.Trim());
			items.ReorderLevel = Conversion.ToInt((object)txtReorderLevel.Text.Trim());
			items.ItemOriginId = Conversion.ToInt(cmbcountry.Value);
			items.CostPrice = Conversion.ToInt((object)txtCostPrice.Text.Trim());
			items.UOMScheduleIdCostRate = Conversion.ToInt((object)((Control)(object)cmbCostUOM).Text.Trim());
			items.PurchasePrice = Conversion.ToInt((object)txtPurchaseRate.Text.Trim());
			items.UOMScheduleIdPurRate = Conversion.ToInt((object)((Control)(object)cmbPurchaseUOM).Text.Trim());
			items.UOMScheduleIdWhsRate = Conversion.ToInt((object)((Control)(object)cmbSaleUOM).Text.Trim());
			items.AmountCalcType = Conversion.ToString((object)((Control)(object)cmbAmountCalcType).Text);
			items.ApplyGST = Conversion.ToBool((object)chbGST.Checked);
			items.ApplyVAT = Conversion.ToBool((object)chbVST.Checked);
			items.ApplyExciese = Conversion.ToBool((object)chbExcise.Checked);
			items.OtherTax = Conversion.ToBool((object)chbOther.Checked);
			items.AttachmentsList = new List<DMSAttachments>();
			items.ItemAllocationlist = new List<ItemAllocation>();
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.UploadedFileCustomName = Attachment.dtAttachments.Rows[i]["CustomFileName"].ToString();
				attach.ScreenName = base.Name;
				items.AttachmentsList.Add(attach);
			}
			if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
			{
				items.DeleteAttachmentsList = new List<DMSAttachments>();
				for (int j = 0; j < AT.RemovedAttachmentListInUpdateCase.Count; j++)
				{
					DMSAttachments delete = new DMSAttachments();
					delete.Attachment = Path.GetFileName(AT.RemovedAttachmentListInUpdateCase[j].Path);
					delete.ScreenName = base.Name;
					items.DeleteAttachmentsList.Add(delete);
				}
			}
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
			Item.Save(items, true);
			if (RecId > 0)
			{
				MessageBox.Show("Receord Update Successfully " + items.Id);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && Attachment.dtAttachments.Rows.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 0, 0);
				}
			}
			else
			{
				MessageBox.Show("Receord Save Successfully " + items.Id);
			}
			refresh();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById(int ID)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnupdate.Visible = true;
			Item items = new Item();
			RecId = ID;
			items = Item.GetByID(RecId);
			if (items == null)
			{
				return;
			}
			items.Id = RecId;
			tabCAddItem.SelectedIndex = 0;
			cmbItemCategory.Value = items.ItemCategoryId;
			cmbItemCategory.ReadOnly = true;
			txtItemCode.Text = Conversion.ToString((object)items.ItemCode);
			txtItemCodeNew.Text = Conversion.ToString((object)items.ItemCodeNew);
			txtItemName.Text = Conversion.ToString((object)items.ItemName);
			CmbQualityGrade.Value = items.ItemQcGradeId;
			cmbItemType.Value = items.ItemTypeId;
			cmbItemClass.Value = items.ItemClassId;
			cmbBaseUnit.Value = Conversion.ToInt((object)items.BaseUnitId);
			txtWeightKgs.Text = Conversion.ToString((object)items.WeightKgs);
			txtMaxMeaurement.Text = Conversion.ToString((object)items.MaxMeaurement);
			CmbMaxMrmtUnit.Value = items.MaxMeaurementUnitId;
			cmbPurchaseGL.Value = items.PurchaseGLAC;
			cmbSaleGL.Value = items.SaleGLAC;
			cmbCGSGL.Value = items.COGSGLAC;
			chbGST.Checked = Conversion.ToBool((object)items.ApplyGST);
			cmbTaxType.Value = items.TaxTypeId;
			chkCommissionOnPurchase.Checked = items.CommOnPurchase;
			chkCommissionOnSale.Checked = items.CommOnSale;
			chkstatus.Checked = Conversion.ToBool((object)items.ItemStatus);
			if (items.ItemWithVarient)
			{
				rdWithVariant.Checked = true;
			}
			else if (!items.ItemWithVarient)
			{
				rdWithoutVariant.Checked = true;
			}
			cmbManufacturerID.Value = items.ManufactureId;
			txtManufacturerPartNo.Text = Conversion.ToString((object)items.ManufacturePartNo);
			cmbBuyerOrSupplier.Value = items.BuyerSupplierId;
			txtBuyerPartNo.Text = Conversion.ToString((object)items.BuyerPartNo);
			txtProductNumber.Text = Conversion.ToString((object)items.ProductNo);
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
			chbVST.Checked = Conversion.ToBool((object)items.ApplyVAT);
			chbExcise.Checked = Conversion.ToBool((object)items.ApplyExciese);
			chbOther.Checked = Conversion.ToBool((object)items.OtherTax);
			fileSavePath = items.Pic1;
			List<DMSAttachments> byID = DMSAttachments.GetByID(RecId, Conversion.ToString((object)base.Name));
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				TA.ActionId = 2;
				TA.CustomFileName = Attachment.UploadedFileCustomName;
				AT.lst.Add(TA);
			}
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

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdhistory.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				ReadById(Conversion.ToInt((object)row.Cells["Id"].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void btnsearch_Click(object sender, EventArgs e)
	{
		grdfrmfill();
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
			DataTable grdlst = Item.FormHistory(reports);
			if (grdlst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("ItemCodeNew", typeof(string));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("ItemQcGrade", typeof(string));
				dt.Columns.Add("ItemType", typeof(string));
				dt.Columns.Add("ItemCategory", typeof(string));
				dt.Columns.Add("ItemParentCategory", typeof(string));
				dt.Columns.Add("ItemClass", typeof(string));
				dt.Columns.Add("BaseUnit", typeof(string));
				dt.Columns.Add("WeightKgs", typeof(decimal));
				dt.Columns.Add("MaxLength", typeof(decimal));
				dt.Columns.Add("MaxLengthUnit", typeof(string));
				dt.Columns.Add("ItemStatus", typeof(string));
				dt.Columns.Add("StockAc", typeof(string));
				dt.Columns.Add("SaleAc", typeof(string));
				dt.Columns.Add("CgsAc", typeof(string));
				dt.Columns.Add("ItemWithVarient", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < grdlst.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(grdlst.Rows[i]["Id"]), Conversion.ToString(grdlst.Rows[i]["ItemCode"]), Conversion.ToString(grdlst.Rows[i]["ItemCodeNew"]), Conversion.ToString(grdlst.Rows[i]["ItemName"]), Conversion.ToString(grdlst.Rows[i]["ItemQcGrade"]), Conversion.ToString(grdlst.Rows[i]["TypeDescription"]), Conversion.ToString(grdlst.Rows[i]["CategoryDescription"]), Conversion.ToString(grdlst.Rows[i]["InvParentCateDescription"]), Conversion.ToString(grdlst.Rows[i]["ClassGroupName"]), Conversion.ToString(grdlst.Rows[i]["BaseUnit"]), Conversion.ToDecimal(grdlst.Rows[i]["WeightKgs"]), Conversion.ToDecimal(grdlst.Rows[i]["MaxMeaurement"]), Conversion.ToString(grdlst.Rows[i]["MaxUOMCode"]), Conversion.ToString(grdlst.Rows[i]["ItemStatus"]), Conversion.ToString(grdlst.Rows[i]["GlStockAccountTitle"]), Conversion.ToString(grdlst.Rows[i]["GlSaleAccountTitle"]), Conversion.ToString(grdlst.Rows[i]["GlCgsAccountTitle"]), Conversion.ToString(grdlst.Rows[i]["ItemWithVarient"]), Conversion.ToDateTime(grdlst.Rows[i]["EntryDate"]).ToShortDateString(), Conversion.ToString(grdlst.Rows[i]["EntryUserName"]), Conversion.ToInt(grdlst.Rows[i]["NoOfAttachments"]));
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
				grdhistory.RootTable.Columns["ItemName"].EditType = (EditType)1;
				grdhistory.RootTable.Columns["Select"].Visible = true;
				grdhistory.FrozenColumns = 1;
			}
			else
			{
				grdhistory.RootTable.Columns["ItemName"].EditType = (EditType)0;
				grdhistory.RootTable.Columns["Select"].Visible = false;
			}
			grdhistory.RootTable.Columns["Id"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["ItemCodeNew"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["ItemType"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["ItemCategory"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["ItemParentCategory"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["ItemClass"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["ItemQcGrade"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["BaseUnit"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["WeightKgs"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["MaxLength"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["MaxLengthUnit"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["ItemStatus"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["StockAc"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["SaleAc"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["CgsAc"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["EntryDate"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["EntryUser"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["ItemWithVarient"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["NoOfAttachments"].EditType = (EditType)0;
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

	private void gridsetting()
	{
		try
		{
			grdhistory.AllowEdit = (InheritableBoolean)1;
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["ItemCodeNew"].Caption = "Item Code ";
			grdhistory.RootTable.Columns["ItemCode"].Visible = true;
			grdhistory.RootTable.Columns["WeightKgs"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["WeightKgs"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["WeightKgs"].FormatString = "#,##.##";
			grdhistory.RootTable.Columns["WeightKgs"].TotalFormatString = "#,##.##";
			grdhistory.RootTable.Columns["MaxLength"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["MaxLength"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["MaxLength"].FormatString = "#,##.##";
			grdhistory.RootTable.Columns["MaxLength"].TotalFormatString = "#,##.##";
			grdhistory.RootTable.Columns["MaxLengthUnit"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["MaxLengthUnit"].FormatString = "#,##.##";
			grdhistory.RootTable.Columns["MaxLengthUnit"].TotalFormatString = "#,##.##";
			grdhistory.RootTable.Columns["BaseUnit"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["BaseUnit"].FormatString = "#,##.##";
			grdhistory.RootTable.Columns["BaseUnit"].TotalFormatString = "#,##.##";
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustmentNew(grdhistory);
			grdhistory.RootTable.Columns.Add("Select");
			grdhistory.RootTable.Columns["Select"].ActAsSelector = true;
			grdhistory.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdhistory.RootTable.Columns["Select"].Position = 0;
			grdhistory.RootTable.Columns["Select"].Width = 30;
			grdhistory.RootTable.Columns["Select"].Visible = false;
			grdhistory.FrozenColumns = 1;
			OnItemUpdateInHistory();
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUomUpdate_Click(object sender, EventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
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
					Id = Convert.ToInt32(r.Cells["Id"].Value.ToString())
				});
			}
			MessageBox.Show("Record Update Successfully");
			grdfrmfill();
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

	private void btnattachment_Click(object sender, EventArgs e)
	{
		AT.Show();
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

	private void button1_Click(object sender, EventArgs e)
	{
		new ItemAllocateToWarehouse(UserAccount).Show();
	}

	private void btnItemAllocation_Click(object sender, EventArgs e)
	{
		new ItemsAllocationToCompany(UserAccount).Show();
	}

	private void txtCostPrice_KeyPress(object sender, KeyPressEventArgs e)
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

	private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtWeightKgs_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnQualityGrate_Click(object sender, EventArgs e)
	{
		try
		{
			new ItemQcGrade(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAttributeVarientAllocationtoItem_Click(object sender, EventArgs e)
	{
		try
		{
			new AttributeVarientAllocationToItems(UserAccount).Show();
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
		//IL_0659: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Expected O, but got Unknown
		//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Expected O, but got Unknown
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Expected O, but got Unknown
		//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fd: Expected O, but got Unknown
		//IL_0714: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Expected O, but got Unknown
		//IL_074b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Expected O, but got Unknown
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Expected O, but got Unknown
		//IL_088a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Expected O, but got Unknown
		//IL_08a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08aa: Expected O, but got Unknown
		//IL_08d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Expected O, but got Unknown
		//IL_08f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Expected O, but got Unknown
		//IL_0919: Unknown result type (might be due to invalid IL or missing references)
		//IL_0923: Expected O, but got Unknown
		//IL_0924: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Expected O, but got Unknown
		//IL_0966: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Expected O, but got Unknown
		//IL_0987: Unknown result type (might be due to invalid IL or missing references)
		//IL_0991: Expected O, but got Unknown
		//IL_0992: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Expected O, but got Unknown
		//IL_099d: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a7: Expected O, but got Unknown
		//IL_09df: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e9: Expected O, but got Unknown
		//IL_0aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaf: Expected O, but got Unknown
		//IL_0b08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b12: Expected O, but got Unknown
		//IL_8df4: Unknown result type (might be due to invalid IL or missing references)
		//IL_8dfe: Expected O, but got Unknown
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance302 = new Appearance();
		Appearance appearance303 = new Appearance();
		Appearance appearance304 = new Appearance();
		Appearance appearance305 = new Appearance();
		Appearance appearance306 = new Appearance();
		Appearance appearance307 = new Appearance();
		Appearance appearance308 = new Appearance();
		Appearance appearance309 = new Appearance();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance252 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Definition.frmAddItemConcrete));
		Appearance appearance313 = new Appearance();
		Appearance appearance314 = new Appearance();
		Appearance appearance315 = new Appearance();
		Appearance appearance316 = new Appearance();
		Appearance appearance317 = new Appearance();
		Appearance appearance318 = new Appearance();
		Appearance appearance319 = new Appearance();
		Appearance appearance320 = new Appearance();
		Appearance appearance321 = new Appearance();
		Appearance appearance322 = new Appearance();
		Appearance appearance323 = new Appearance();
		Appearance appearance324 = new Appearance();
		this.label13 = new System.Windows.Forms.Label();
		this.tabCAddItem = new System.Windows.Forms.TabControl();
		this.tpItemDefinitionAddItem = new System.Windows.Forms.TabPage();
		this.panelDetails = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label26 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.cmbPurchaseUOM = new UltraCombo();
		this.txtMaxStockLevel = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.pnlimage = new System.Windows.Forms.Panel();
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
		this.txtManufacturerPartNo = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.cmbSaleUOM = new UltraCombo();
		this.txtReorderLevel = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.txtCostPrice = new System.Windows.Forms.TextBox();
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
		this.btnItemAllocation = new System.Windows.Forms.Button();
		this.BtnAttributeVarientAllocationtoItem = new System.Windows.Forms.Button();
		this.button1 = new System.Windows.Forms.Button();
		this.btnitmcat = new System.Windows.Forms.Button();
		this.btnitmtype = new System.Windows.Forms.Button();
		this.btnitmtaxsch = new System.Windows.Forms.Button();
		this.btnQualityGrate = new System.Windows.Forms.Button();
		this.btnitmuom = new System.Windows.Forms.Button();
		this.btnitmuomtaxsch = new System.Windows.Forms.Button();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdWithoutVariant = new System.Windows.Forms.RadioButton();
		this.rdWithVariant = new System.Windows.Forms.RadioButton();
		this.chkCommissionOnPurchase = new System.Windows.Forms.CheckBox();
		this.chkCommissionOnSale = new System.Windows.Forms.CheckBox();
		this.txtItemCodeNew = new System.Windows.Forms.TextBox();
		this.cmbItemCategory = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbItemType = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.chbGST = new System.Windows.Forms.CheckBox();
		this.cmbItemClass = new UltraCombo();
		this.txtItemCode = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbQualityGrade = new UltraCombo();
		this.txtItemName = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbPurchaseGL = new UltraCombo();
		this.cmbTaxType = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtWeightKgs = new System.Windows.Forms.TextBox();
		this.txtMaxMeaurement = new System.Windows.Forms.TextBox();
		this.cmbSaleGL = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbCGSGL = new UltraCombo();
		this.CmbMaxMrmtUnit = new UltraCombo();
		this.cmbBaseUnit = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.chkstatus = new System.Windows.Forms.CheckBox();
		this.label33 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.grdAllocation = new GridEX();
		this.label14 = new System.Windows.Forms.Label();
		this.BtnShowDetails = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
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
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemClass).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbQualityGrade).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPurchaseGL).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTaxType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSaleGL).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCGSGL).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbMaxMrmtUnit).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBaseUnit).BeginInit();
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
		this.label13.Location = new System.Drawing.Point(9, 7);
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
		this.tabCAddItem.Size = new System.Drawing.Size(1155, 663);
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
		this.tpItemDefinitionAddItem.Size = new System.Drawing.Size(1147, 634);
		this.tpItemDefinitionAddItem.TabIndex = 0;
		this.tpItemDefinitionAddItem.Text = "Add Item";
		this.panelDetails.Controls.Add(this.groupBox4);
		this.panelDetails.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelDetails.Location = new System.Drawing.Point(3, 657);
		this.panelDetails.Name = "panelDetails";
		this.panelDetails.Size = new System.Drawing.Size(1141, 0);
		this.panelDetails.TabIndex = 3;
		this.panelDetails.Visible = false;
		this.groupBox4.Controls.Add(this.label26);
		this.groupBox4.Controls.Add(this.label17);
		this.groupBox4.Controls.Add(this.label35);
		this.groupBox4.Controls.Add(this.label21);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPurchaseUOM);
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
		this.groupBox4.Controls.Add(this.txtManufacturerPartNo);
		this.groupBox4.Controls.Add(this.label27);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSaleUOM);
		this.groupBox4.Controls.Add(this.txtReorderLevel);
		this.groupBox4.Controls.Add(this.label25);
		this.groupBox4.Controls.Add(this.txtCostPrice);
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
		this.groupBox4.Size = new System.Drawing.Size(1110, 262);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "More Detail";
		this.groupBox4.Visible = false;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(5, 21);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(95, 15);
		this.label26.TabIndex = 24;
		this.label26.Text = "Manufacturer ID";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(5, 219);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(93, 15);
		this.label17.TabIndex = 55;
		this.label17.Text = "Max Stock Level";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(346, 21);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(59, 15);
		this.label35.TabIndex = 62;
		this.label35.Text = "Cost Price";
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
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance162;
		((SpecialBoxBase)((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance164).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance164).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance168).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance169).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance170).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPurchaseUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPurchaseUOM).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPurchaseUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPurchaseUOM).Location = new System.Drawing.Point(451, 101);
		((System.Windows.Forms.Control)(object)this.cmbPurchaseUOM).Name = "cmbPurchaseUOM";
		((System.Windows.Forms.Control)(object)this.cmbPurchaseUOM).Size = new System.Drawing.Size(141, 26);
		((System.Windows.Forms.Control)(object)this.cmbPurchaseUOM).TabIndex = 11;
		this.txtMaxStockLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMaxStockLevel.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtMaxStockLevel.Location = new System.Drawing.Point(128, 215);
		this.txtMaxStockLevel.Name = "txtMaxStockLevel";
		this.txtMaxStockLevel.Size = new System.Drawing.Size(207, 23);
		this.txtMaxStockLevel.TabIndex = 7;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(346, 78);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(81, 15);
		this.label22.TabIndex = 63;
		this.label22.Text = "Purchase Rate";
		this.pnlimage.Controls.Add(this.btnimg1);
		this.pnlimage.Controls.Add(this.btnimg2);
		this.pnlimage.Controls.Add(this.picbox2);
		this.pnlimage.Controls.Add(this.picbox1);
		this.pnlimage.Location = new System.Drawing.Point(602, 20);
		this.pnlimage.Name = "pnlimage";
		this.pnlimage.Size = new System.Drawing.Size(505, 224);
		this.pnlimage.TabIndex = 18;
		this.btnimg1.BackColor = System.Drawing.Color.Teal;
		this.btnimg1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnimg1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.btnimg1.ForeColor = System.Drawing.Color.White;
		this.btnimg1.Location = new System.Drawing.Point(87, 194);
		this.btnimg1.Name = "btnimg1";
		this.btnimg1.Size = new System.Drawing.Size(75, 27);
		this.btnimg1.TabIndex = 0;
		this.btnimg1.Text = "Browse";
		this.btnimg1.UseVisualStyleBackColor = false;
		this.btnimg2.BackColor = System.Drawing.Color.Teal;
		this.btnimg2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnimg2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.btnimg2.ForeColor = System.Drawing.Color.White;
		this.btnimg2.Location = new System.Drawing.Point(341, 194);
		this.btnimg2.Name = "btnimg2";
		this.btnimg2.Size = new System.Drawing.Size(75, 27);
		this.btnimg2.TabIndex = 1;
		this.btnimg2.Text = "Browse";
		this.btnimg2.UseVisualStyleBackColor = false;
		this.picbox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.picbox2.Location = new System.Drawing.Point(257, 3);
		this.picbox2.Name = "picbox2";
		this.picbox2.Size = new System.Drawing.Size(242, 188);
		this.picbox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picbox2.TabIndex = 36;
		this.picbox2.TabStop = false;
		this.picbox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.picbox1.Location = new System.Drawing.Point(3, 4);
		this.picbox1.Name = "picbox1";
		this.picbox1.Size = new System.Drawing.Size(242, 188);
		this.picbox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picbox1.TabIndex = 35;
		this.picbox1.TabStop = false;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(5, 191);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(72, 15);
		this.label19.TabIndex = 60;
		this.label19.Text = "Item Region";
		this.cmbManufacturerID.AllowNull = (DefaultableBoolean)1;
		this.cmbManufacturerID.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbManufacturerID.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbManufacturerID.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Appearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbManufacturerID).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance149;
		((SpecialBoxBase)((UltraGridBase)this.cmbManufacturerID).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance155).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance156).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbManufacturerID).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance159;
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
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance176;
		((SpecialBoxBase)((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance181).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAmountCalcType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAmountCalcType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbAmountCalcType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAmountCalcType).Location = new System.Drawing.Point(451, 185);
		((System.Windows.Forms.Control)(object)this.cmbAmountCalcType).Name = "cmbAmountCalcType";
		((System.Windows.Forms.Control)(object)this.cmbAmountCalcType).Size = new System.Drawing.Size(141, 26);
		((System.Windows.Forms.Control)(object)this.cmbAmountCalcType).TabIndex = 14;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(5, 162);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(80, 15);
		this.label18.TabIndex = 59;
		this.label18.Text = "ReOrder Level";
		this.txtPurchaseRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPurchaseRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPurchaseRate.Location = new System.Drawing.Point(451, 74);
		this.txtPurchaseRate.Name = "txtPurchaseRate";
		this.txtPurchaseRate.Size = new System.Drawing.Size(141, 23);
		this.txtPurchaseRate.TabIndex = 10;
		this.cmbcountry.AllowNull = (DefaultableBoolean)1;
		this.cmbcountry.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcountry.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcountry.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Appearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbcountry).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcountry).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance136;
		((SpecialBoxBase)((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbcountry).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcountry).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance142).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbcountry).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcountry).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcountry).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcountry).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcountry.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcountry).Location = new System.Drawing.Point(128, 185);
		((System.Windows.Forms.Control)(object)this.cmbcountry).Name = "cmbcountry";
		((System.Windows.Forms.Control)(object)this.cmbcountry).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbcountry).TabIndex = 6;
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
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSaleUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance188;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance189;
		((SpecialBoxBase)((UltraGridBase)this.cmbSaleUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance190).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance190).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance194).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance195).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance195).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance195).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance195;
		((AppearanceBase)appearance197).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSaleUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSaleUOM).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbSaleUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSaleUOM).Location = new System.Drawing.Point(451, 156);
		((System.Windows.Forms.Control)(object)this.cmbSaleUOM).Name = "cmbSaleUOM";
		((System.Windows.Forms.Control)(object)this.cmbSaleUOM).Size = new System.Drawing.Size(141, 26);
		((System.Windows.Forms.Control)(object)this.cmbSaleUOM).TabIndex = 13;
		this.txtReorderLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtReorderLevel.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtReorderLevel.Location = new System.Drawing.Point(128, 158);
		this.txtReorderLevel.Name = "txtReorderLevel";
		this.txtReorderLevel.Size = new System.Drawing.Size(207, 23);
		this.txtReorderLevel.TabIndex = 5;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(5, 49);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(122, 15);
		this.label25.TabIndex = 25;
		this.label25.Text = "Manufacturer Part No";
		this.txtCostPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCostPrice.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCostPrice.Location = new System.Drawing.Point(451, 17);
		this.txtCostPrice.Name = "txtCostPrice";
		this.txtCostPrice.Size = new System.Drawing.Size(141, 23);
		this.txtCostPrice.TabIndex = 8;
		this.txtCostPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCostPrice_KeyPress);
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
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCostUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance202;
		((SpecialBoxBase)((UltraGridBase)this.cmbCostUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance203).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance203).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance204;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance208).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance209).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance209).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance209).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCostUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCostUOM).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCostUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCostUOM).Location = new System.Drawing.Point(451, 43);
		((System.Windows.Forms.Control)(object)this.cmbCostUOM).Name = "cmbCostUOM";
		((System.Windows.Forms.Control)(object)this.cmbCostUOM).Size = new System.Drawing.Size(141, 26);
		((System.Windows.Forms.Control)(object)this.cmbCostUOM).TabIndex = 9;
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
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance131).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBuyerOrSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance133;
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
		this.chbOther.TabIndex = 17;
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
		this.txtSaleRate.TabIndex = 12;
		this.chbExcise.AutoSize = true;
		this.chbExcise.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chbExcise.Location = new System.Drawing.Point(394, 217);
		this.chbExcise.Name = "chbExcise";
		this.chbExcise.Size = new System.Drawing.Size(58, 19);
		this.chbExcise.TabIndex = 16;
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
		this.chbVST.TabIndex = 15;
		this.chbVST.Text = "VAT";
		this.chbVST.UseVisualStyleBackColor = true;
		this.panel1.Controls.Add(this.groupBox3);
		this.panel1.Controls.Add(this.groupBox2);
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Controls.Add(this.label14);
		this.panel1.Controls.Add(this.BtnShowDetails);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 67);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1141, 590);
		this.panel1.TabIndex = 0;
		this.groupBox3.Controls.Add(this.btnItemAllocation);
		this.groupBox3.Controls.Add(this.BtnAttributeVarientAllocationtoItem);
		this.groupBox3.Controls.Add(this.button1);
		this.groupBox3.Controls.Add(this.btnitmcat);
		this.groupBox3.Controls.Add(this.btnitmtype);
		this.groupBox3.Controls.Add(this.btnitmtaxsch);
		this.groupBox3.Controls.Add(this.btnQualityGrate);
		this.groupBox3.Controls.Add(this.btnitmuom);
		this.groupBox3.Controls.Add(this.btnitmuomtaxsch);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(854, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(269, 337);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Related Defination";
		this.btnItemAllocation.BackColor = System.Drawing.Color.Teal;
		this.btnItemAllocation.FlatAppearance.BorderSize = 0;
		this.btnItemAllocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnItemAllocation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnItemAllocation.ForeColor = System.Drawing.Color.White;
		this.btnItemAllocation.Location = new System.Drawing.Point(6, 263);
		this.btnItemAllocation.Name = "btnItemAllocation";
		this.btnItemAllocation.Size = new System.Drawing.Size(225, 31);
		this.btnItemAllocation.TabIndex = 6;
		this.btnItemAllocation.Text = "Item Allocation to Location";
		this.btnItemAllocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnItemAllocation.UseVisualStyleBackColor = false;
		this.btnItemAllocation.Click += new System.EventHandler(btnItemAllocation_Click);
		this.BtnAttributeVarientAllocationtoItem.BackColor = System.Drawing.Color.Teal;
		this.BtnAttributeVarientAllocationtoItem.FlatAppearance.BorderSize = 0;
		this.BtnAttributeVarientAllocationtoItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAttributeVarientAllocationtoItem.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAttributeVarientAllocationtoItem.ForeColor = System.Drawing.Color.White;
		this.BtnAttributeVarientAllocationtoItem.Location = new System.Drawing.Point(5, 228);
		this.BtnAttributeVarientAllocationtoItem.Name = "BtnAttributeVarientAllocationtoItem";
		this.BtnAttributeVarientAllocationtoItem.Size = new System.Drawing.Size(213, 31);
		this.BtnAttributeVarientAllocationtoItem.TabIndex = 7;
		this.BtnAttributeVarientAllocationtoItem.Text = "Varient Allocation to Item";
		this.BtnAttributeVarientAllocationtoItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnAttributeVarientAllocationtoItem.UseVisualStyleBackColor = false;
		this.BtnAttributeVarientAllocationtoItem.Click += new System.EventHandler(BtnAttributeVarientAllocationtoItem_Click);
		this.button1.BackColor = System.Drawing.Color.Teal;
		this.button1.FlatAppearance.BorderSize = 0;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.ForeColor = System.Drawing.Color.White;
		this.button1.Location = new System.Drawing.Point(6, 297);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(257, 31);
		this.button1.TabIndex = 7;
		this.button1.Text = "Items Allocate to Warehouse";
		this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.btnitmcat.BackColor = System.Drawing.Color.Teal;
		this.btnitmcat.FlatAppearance.BorderSize = 0;
		this.btnitmcat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnitmcat.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnitmcat.ForeColor = System.Drawing.Color.White;
		this.btnitmcat.Location = new System.Drawing.Point(6, 90);
		this.btnitmcat.Name = "btnitmcat";
		this.btnitmcat.Size = new System.Drawing.Size(126, 31);
		this.btnitmcat.TabIndex = 2;
		this.btnitmcat.Text = "Item Category";
		this.btnitmcat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnitmcat.UseVisualStyleBackColor = false;
		this.btnitmcat.Click += new System.EventHandler(btnitmcat_Click);
		this.btnitmtype.BackColor = System.Drawing.Color.Teal;
		this.btnitmtype.FlatAppearance.BorderSize = 0;
		this.btnitmtype.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnitmtype.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnitmtype.ForeColor = System.Drawing.Color.White;
		this.btnitmtype.Location = new System.Drawing.Point(6, 56);
		this.btnitmtype.Name = "btnitmtype";
		this.btnitmtype.Size = new System.Drawing.Size(109, 31);
		this.btnitmtype.TabIndex = 1;
		this.btnitmtype.Text = "Item Type";
		this.btnitmtype.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnitmtype.UseVisualStyleBackColor = false;
		this.btnitmtype.Click += new System.EventHandler(btnitmtype_Click);
		this.btnitmtaxsch.BackColor = System.Drawing.Color.Teal;
		this.btnitmtaxsch.FlatAppearance.BorderSize = 0;
		this.btnitmtaxsch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnitmtaxsch.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnitmtaxsch.ForeColor = System.Drawing.Color.White;
		this.btnitmtaxsch.Location = new System.Drawing.Point(6, 193);
		this.btnitmtaxsch.Name = "btnitmtaxsch";
		this.btnitmtaxsch.Size = new System.Drawing.Size(186, 31);
		this.btnitmtaxsch.TabIndex = 5;
		this.btnitmtaxsch.Text = "Item Tax Schedule";
		this.btnitmtaxsch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnitmtaxsch.UseVisualStyleBackColor = false;
		this.btnitmtaxsch.Click += new System.EventHandler(btnitmtaxsch_Click);
		this.btnQualityGrate.BackColor = System.Drawing.Color.Teal;
		this.btnQualityGrate.FlatAppearance.BorderSize = 0;
		this.btnQualityGrate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnQualityGrate.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold);
		this.btnQualityGrate.ForeColor = System.Drawing.Color.White;
		this.btnQualityGrate.Location = new System.Drawing.Point(6, 124);
		this.btnQualityGrate.Name = "btnQualityGrate";
		this.btnQualityGrate.Size = new System.Drawing.Size(145, 31);
		this.btnQualityGrate.TabIndex = 3;
		this.btnQualityGrate.Text = "Quality Grade";
		this.btnQualityGrate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnQualityGrate.UseVisualStyleBackColor = false;
		this.btnQualityGrate.Click += new System.EventHandler(btnQualityGrate_Click);
		this.btnitmuom.BackColor = System.Drawing.Color.Teal;
		this.btnitmuom.FlatAppearance.BorderSize = 0;
		this.btnitmuom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnitmuom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnitmuom.ForeColor = System.Drawing.Color.White;
		this.btnitmuom.Location = new System.Drawing.Point(5, 22);
		this.btnitmuom.Name = "btnitmuom";
		this.btnitmuom.Size = new System.Drawing.Size(91, 31);
		this.btnitmuom.TabIndex = 0;
		this.btnitmuom.Text = "Item Uom";
		this.btnitmuom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnitmuom.UseVisualStyleBackColor = false;
		this.btnitmuom.Click += new System.EventHandler(btnitmuom_Click);
		this.btnitmuomtaxsch.BackColor = System.Drawing.Color.Teal;
		this.btnitmuomtaxsch.FlatAppearance.BorderSize = 0;
		this.btnitmuomtaxsch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnitmuomtaxsch.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnitmuomtaxsch.ForeColor = System.Drawing.Color.White;
		this.btnitmuomtaxsch.Location = new System.Drawing.Point(6, 159);
		this.btnitmuomtaxsch.Name = "btnitmuomtaxsch";
		this.btnitmuomtaxsch.Size = new System.Drawing.Size(164, 31);
		this.btnitmuomtaxsch.TabIndex = 4;
		this.btnitmuomtaxsch.Text = "Item Uom Schedule";
		this.btnitmuomtaxsch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnitmuomtaxsch.UseVisualStyleBackColor = false;
		this.btnitmuomtaxsch.Click += new System.EventHandler(btnitmuomtaxsch_Click);
		this.groupBox2.Controls.Add(this.rdWithoutVariant);
		this.groupBox2.Controls.Add(this.rdWithVariant);
		this.groupBox2.Controls.Add(this.chkCommissionOnPurchase);
		this.groupBox2.Controls.Add(this.chkCommissionOnSale);
		this.groupBox2.Controls.Add(this.txtItemCodeNew);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCategory);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemType);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.chbGST);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemClass);
		this.groupBox2.Controls.Add(this.txtItemCode);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbQualityGrade);
		this.groupBox2.Controls.Add(this.txtItemName);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPurchaseGL);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTaxType);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.txtWeightKgs);
		this.groupBox2.Controls.Add(this.txtMaxMeaurement);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSaleGL);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCGSGL);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMaxMrmtUnit);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBaseUnit);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.chkstatus);
		this.groupBox2.Controls.Add(this.label33);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(602, 337);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.rdWithoutVariant.AutoSize = true;
		this.rdWithoutVariant.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdWithoutVariant.Location = new System.Drawing.Point(482, 263);
		this.rdWithoutVariant.Name = "rdWithoutVariant";
		this.rdWithoutVariant.Size = new System.Drawing.Size(109, 19);
		this.rdWithoutVariant.TabIndex = 20;
		this.rdWithoutVariant.TabStop = true;
		this.rdWithoutVariant.Text = "Without Variant";
		this.rdWithoutVariant.UseVisualStyleBackColor = true;
		this.rdWithVariant.AutoSize = true;
		this.rdWithVariant.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdWithVariant.Location = new System.Drawing.Point(376, 263);
		this.rdWithVariant.Name = "rdWithVariant";
		this.rdWithVariant.Size = new System.Drawing.Size(91, 19);
		this.rdWithVariant.TabIndex = 19;
		this.rdWithVariant.TabStop = true;
		this.rdWithVariant.Text = "With Variant";
		this.rdWithVariant.UseVisualStyleBackColor = true;
		this.chkCommissionOnPurchase.AutoSize = true;
		this.chkCommissionOnPurchase.Checked = true;
		this.chkCommissionOnPurchase.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkCommissionOnPurchase.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chkCommissionOnPurchase.Location = new System.Drawing.Point(376, 222);
		this.chkCommissionOnPurchase.Name = "chkCommissionOnPurchase";
		this.chkCommissionOnPurchase.Size = new System.Drawing.Size(160, 19);
		this.chkCommissionOnPurchase.TabIndex = 17;
		this.chkCommissionOnPurchase.Text = "Commission on Purchase";
		this.chkCommissionOnPurchase.UseVisualStyleBackColor = true;
		this.chkCommissionOnSale.AutoSize = true;
		this.chkCommissionOnSale.Checked = true;
		this.chkCommissionOnSale.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkCommissionOnSale.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chkCommissionOnSale.Location = new System.Drawing.Point(376, 203);
		this.chkCommissionOnSale.Name = "chkCommissionOnSale";
		this.chkCommissionOnSale.Size = new System.Drawing.Size(134, 19);
		this.chkCommissionOnSale.TabIndex = 16;
		this.chkCommissionOnSale.Text = "Commission on Sale";
		this.chkCommissionOnSale.UseVisualStyleBackColor = true;
		this.txtItemCodeNew.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemCodeNew.Location = new System.Drawing.Point(90, 51);
		this.txtItemCodeNew.Name = "txtItemCodeNew";
		this.txtItemCodeNew.ReadOnly = true;
		this.txtItemCodeNew.Size = new System.Drawing.Size(192, 23);
		this.txtItemCodeNew.TabIndex = 1;
		this.txtItemCodeNew.TabStop = false;
		this.cmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Location = new System.Drawing.Point(90, 20);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Name = "cmbItemCategory";
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Leave += new System.EventHandler(cmbItemCategory_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(286, 26);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(70, 15);
		this.label8.TabIndex = 61;
		this.label8.Text = "Max Length";
		this.cmbItemType.AllowNull = (DefaultableBoolean)1;
		this.cmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance289).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance289;
		((UltraGridBase)this.cmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance290).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance290).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance290).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance290;
		((AppearanceBase)appearance291).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance291;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance292).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance292).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance292).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance292;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance293).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance293;
		((AppearanceBase)appearance294).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance294).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance294;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance295;
		((AppearanceBase)appearance296).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance296).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance296;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance297).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance297).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance297).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance297).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance297;
		((AppearanceBase)appearance298).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance298;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance299).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance299).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance299;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance300;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Location = new System.Drawing.Point(90, 141);
		((System.Windows.Forms.Control)(object)this.cmbItemType).Name = "cmbItemType";
		((System.Windows.Forms.Control)(object)this.cmbItemType).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemType).TabIndex = 4;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(5, 179);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(61, 15);
		this.label6.TabIndex = 5;
		this.label6.Text = "Item Class";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(291, 55);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(76, 15);
		this.label20.TabIndex = 61;
		this.label20.Text = "Weight (Kgs)";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(5, 147);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(60, 15);
		this.label5.TabIndex = 4;
		this.label5.Text = "Item Type";
		this.chbGST.AutoSize = true;
		this.chbGST.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chbGST.Location = new System.Drawing.Point(544, 177);
		this.chbGST.Name = "chbGST";
		this.chbGST.Size = new System.Drawing.Size(48, 19);
		this.chbGST.TabIndex = 15;
		this.chbGST.Text = "GST";
		this.chbGST.UseVisualStyleBackColor = true;
		this.cmbItemClass.AllowNull = (DefaultableBoolean)1;
		this.cmbItemClass.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemClass.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemClass.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Appearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance230).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance230).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance231;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance232).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance233;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance234).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance235).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance235).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance235).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance235).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance236).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemClass.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Location = new System.Drawing.Point(90, 173);
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Name = "cmbItemClass";
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemClass).TabIndex = 5;
		this.txtItemCode.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemCode.Location = new System.Drawing.Point(90, 265);
		this.txtItemCode.Name = "txtItemCode";
		this.txtItemCode.ReadOnly = true;
		this.txtItemCode.Size = new System.Drawing.Size(192, 23);
		this.txtItemCode.TabIndex = 8;
		this.txtItemCode.TabStop = false;
		this.txtItemCode.Visible = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(5, 55);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(62, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Item Code";
		this.CmbQualityGrade.AllowNull = (DefaultableBoolean)1;
		this.CmbQualityGrade.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbQualityGrade.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbQualityGrade.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.Appearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance254).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance254).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance254).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbQualityGrade).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance254;
		((AppearanceBase)appearance255).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance255;
		((SpecialBoxBase)((UltraGridBase)this.CmbQualityGrade).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance256).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance256).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance256).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance257).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance257;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance260).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance261).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance261).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance261;
		((AppearanceBase)appearance262).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance263).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbQualityGrade).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbQualityGrade).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbQualityGrade.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbQualityGrade).Location = new System.Drawing.Point(90, 109);
		((System.Windows.Forms.Control)(object)this.CmbQualityGrade).Name = "CmbQualityGrade";
		((System.Windows.Forms.Control)(object)this.CmbQualityGrade).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.CmbQualityGrade).TabIndex = 3;
		this.txtItemName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemName.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemName.Location = new System.Drawing.Point(90, 80);
		this.txtItemName.Name = "txtItemName";
		this.txtItemName.Size = new System.Drawing.Size(192, 23);
		this.txtItemName.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(5, 84);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 1;
		this.label2.Text = "Item Name";
		this.cmbPurchaseGL.AllowNull = (DefaultableBoolean)1;
		this.cmbPurchaseGL.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPurchaseGL.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPurchaseGL.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance152;
		((SpecialBoxBase)((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance207).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance218).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance218).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance218).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance229).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPurchaseGL.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Location = new System.Drawing.Point(376, 78);
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Name = "cmbPurchaseGL";
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).TabIndex = 11;
		this.cmbTaxType.AllowNull = (DefaultableBoolean)1;
		this.cmbTaxType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTaxType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTaxType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance277).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance277;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance278).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance278).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance278;
		((AppearanceBase)appearance279).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance279;
		((SpecialBoxBase)((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance280).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance280).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance280).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance280;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance281).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance281;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance282).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance283;
		((AppearanceBase)appearance284).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance284).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance285).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance285).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance285).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance285).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance285;
		((AppearanceBase)appearance286).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance286;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance287).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance287;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance288;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbTaxType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Location = new System.Drawing.Point(376, 173);
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Name = "cmbTaxType";
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.cmbTaxType).TabIndex = 14;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(286, 147);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(68, 15);
		this.label12.TabIndex = 11;
		this.label12.Text = "CGS GL A/c";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(5, 240);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(69, 15);
		this.label9.TabIndex = 2;
		this.label9.Text = "Length Unit";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(5, 211);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(56, 15);
		this.label3.TabIndex = 2;
		this.label3.Text = "Base Unit";
		this.txtWeightKgs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeightKgs.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWeightKgs.Location = new System.Drawing.Point(376, 51);
		this.txtWeightKgs.Name = "txtWeightKgs";
		this.txtWeightKgs.Size = new System.Drawing.Size(192, 23);
		this.txtWeightKgs.TabIndex = 10;
		this.txtWeightKgs.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWeightKgs_KeyPress);
		this.txtMaxMeaurement.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMaxMeaurement.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtMaxMeaurement.Location = new System.Drawing.Point(376, 22);
		this.txtMaxMeaurement.Name = "txtMaxMeaurement";
		this.txtMaxMeaurement.Size = new System.Drawing.Size(220, 23);
		this.txtMaxMeaurement.TabIndex = 9;
		this.txtMaxMeaurement.KeyPress += new System.Windows.Forms.KeyPressEventHandler(textBox1_KeyPress);
		this.cmbSaleGL.AllowNull = (DefaultableBoolean)1;
		this.cmbSaleGL.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSaleGL.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSaleGL.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Appearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance214).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance214).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSaleGL).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance214;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance215;
		((SpecialBoxBase)((UltraGridBase)this.cmbSaleGL).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance216).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance216).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance219).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance221).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance222).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance222).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance222).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance222;
		((AppearanceBase)appearance223).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSaleGL).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbSaleGL.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).Location = new System.Drawing.Point(376, 109);
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).Name = "cmbSaleGL";
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbSaleGL).TabIndex = 12;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(286, 115);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(68, 15);
		this.label11.TabIndex = 10;
		this.label11.Text = "Sale GL A/c";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(5, 26);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(82, 15);
		this.label4.TabIndex = 3;
		this.label4.Text = "Item Category";
		this.cmbCGSGL.AllowNull = (DefaultableBoolean)1;
		this.cmbCGSGL.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCGSGL.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCGSGL.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Appearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance266).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance266).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance266;
		((AppearanceBase)appearance267).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance267;
		((SpecialBoxBase)((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance268).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance268).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance268).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance269).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance269;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance270).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance271;
		((AppearanceBase)appearance272).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance272).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance273).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance273).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance273).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance273;
		((AppearanceBase)appearance274).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance274;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance275).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCGSGL).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCGSGL.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Location = new System.Drawing.Point(376, 141);
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Name = "cmbCGSGL";
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbCGSGL).TabIndex = 13;
		this.CmbMaxMrmtUnit.AllowNull = (DefaultableBoolean)1;
		this.CmbMaxMrmtUnit.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMaxMrmtUnit.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMaxMrmtUnit.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance301).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance301).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.Appearance = (AppearanceBase)(object)appearance301;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance302).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance302).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance302).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance302).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance302;
		((AppearanceBase)appearance303).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance303;
		((SpecialBoxBase)((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance304).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance304).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance304).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance304).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance304;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance305).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance305).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance305;
		((AppearanceBase)appearance306).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance306).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance306;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance307).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance307;
		((AppearanceBase)appearance308).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance308).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance308;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance309).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance309).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance309).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance309).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance309).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance309;
		((AppearanceBase)appearance310).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance310;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance311).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance311).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance311;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance312).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance312;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMaxMrmtUnit).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMaxMrmtUnit).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbMaxMrmtUnit.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMaxMrmtUnit).Location = new System.Drawing.Point(90, 234);
		((System.Windows.Forms.Control)(object)this.CmbMaxMrmtUnit).Name = "CmbMaxMrmtUnit";
		((System.Windows.Forms.Control)(object)this.CmbMaxMrmtUnit).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.CmbMaxMrmtUnit).TabIndex = 7;
		this.cmbBaseUnit.AllowNull = (DefaultableBoolean)1;
		this.cmbBaseUnit.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBaseUnit.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBaseUnit.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Appearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance241).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance241).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance241).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance241;
		((AppearanceBase)appearance242).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance242;
		((SpecialBoxBase)((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance243).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance243).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance244;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance245).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance246;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance247).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance248).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance248).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance248).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance248;
		((AppearanceBase)appearance249).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBaseUnit).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbBaseUnit.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Location = new System.Drawing.Point(90, 205);
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Name = "cmbBaseUnit";
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.cmbBaseUnit).TabIndex = 6;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(286, 84);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(76, 15);
		this.label10.TabIndex = 9;
		this.label10.Text = "Stock GL A/c";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(5, 115);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(79, 15);
		this.label7.TabIndex = 5;
		this.label7.Text = "Quality Grade";
		this.chkstatus.AutoSize = true;
		this.chkstatus.Checked = true;
		this.chkstatus.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkstatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chkstatus.Location = new System.Drawing.Point(376, 241);
		this.chkstatus.Name = "chkstatus";
		this.chkstatus.Size = new System.Drawing.Size(71, 19);
		this.chkstatus.TabIndex = 18;
		this.chkstatus.Text = "Is Active";
		this.chkstatus.UseVisualStyleBackColor = true;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(286, 179);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(53, 15);
		this.label33.TabIndex = 10;
		this.label33.Text = "Tax Type";
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.grdAllocation);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(609, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(245, 337);
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
		((System.Windows.Forms.Control)(object)this.grdAllocation).Size = new System.Drawing.Size(239, 315);
		((System.Windows.Forms.Control)(object)this.grdAllocation).TabIndex = 0;
		this.grdAllocation.TotalRow = (InheritableBoolean)1;
		this.grdAllocation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAllocation).VisualStyle = (VisualStyle)4;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(0, 581);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(96, 19);
		this.label14.TabIndex = 191;
		this.label14.Text = "More Details";
		this.label14.Visible = false;
		this.BtnShowDetails.AutoSize = true;
		this.BtnShowDetails.BackColor = System.Drawing.Color.Teal;
		this.BtnShowDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowDetails.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowDetails.ForeColor = System.Drawing.Color.White;
		this.BtnShowDetails.Location = new System.Drawing.Point(95, 576);
		this.BtnShowDetails.Name = "BtnShowDetails";
		this.BtnShowDetails.Size = new System.Drawing.Size(43, 28);
		this.BtnShowDetails.TabIndex = 18;
		this.BtnShowDetails.Text = "+";
		this.BtnShowDetails.UseVisualStyleBackColor = false;
		this.BtnShowDetails.Visible = false;
		this.BtnShowDetails.Click += new System.EventHandler(BtnShowDetails_Click);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label13);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 35);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1141, 32);
		this.panel2.TabIndex = 6;
		this.panel6.Controls.Add(this.toolStrip1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1141, 32);
		this.panel6.TabIndex = 5;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnnew, this.toolStripButton1, this.toolStripSeparator1, this.btnupdate, this.btnsave, this.toolStripSeparator2, this.btnprint, this.toolStripSeparator3, this.btnattachment, this.toolStripSeparator4 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1141, 27);
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
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
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
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnprint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnprint.Image = (System.Drawing.Image)resources.GetObject("btnprint.Image");
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(61, 24);
		this.btnprint.Text = "&Print";
		this.btnprint.Visible = false;
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = (System.Drawing.Image)resources.GetObject("btnattachment.Image");
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.tpItemDefinitionAddItemHistory.Controls.Add(this.panel3);
		this.tpItemDefinitionAddItemHistory.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.tpItemDefinitionAddItemHistory.Location = new System.Drawing.Point(4, 4);
		this.tpItemDefinitionAddItemHistory.Name = "tpItemDefinitionAddItemHistory";
		this.tpItemDefinitionAddItemHistory.Size = new System.Drawing.Size(1147, 634);
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
		this.panel3.Size = new System.Drawing.Size(1147, 634);
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
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1147, 486);
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
		this.panel4.Size = new System.Drawing.Size(1147, 28);
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
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1100, 1);
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
		this.panel9.Size = new System.Drawing.Size(1147, 68);
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
		((AppearanceBase)appearance313).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance313).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance313;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance314).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance314).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance314).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance314).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance314;
		((AppearanceBase)appearance315).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance315;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance316).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance316).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance316).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance316).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance316;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance317).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance317).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance317;
		((AppearanceBase)appearance318).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance318).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance318;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance319).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance319;
		((AppearanceBase)appearance320).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance320).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance320;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance321).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance321).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance321).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance321).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance321).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance321;
		((AppearanceBase)appearance322).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance322;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance323).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance323).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance323;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance324).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance324;
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
		this.panel10.Size = new System.Drawing.Size(1147, 52);
		this.panel10.TabIndex = 65;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1147, 27);
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
		base.ClientSize = new System.Drawing.Size(1155, 663);
		base.Controls.Add(this.tabCAddItem);
		base.KeyPreview = true;
		base.Name = "frmAddItemConcrete";
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
		((System.ComponentModel.ISupportInitialize)this.picbox2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picbox1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbManufacturerID).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAmountCalcType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcountry).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSaleUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCostUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBuyerOrSupplier).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemClass).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbQualityGrade).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPurchaseGL).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTaxType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSaleGL).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCGSGL).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbMaxMrmtUnit).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBaseUnit).EndInit();
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
