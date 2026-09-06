using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Pos;
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.Inventory;
using Architecture.Model.POS;
using Architecture.Model.Production;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Architecture.WinApp.WholeSale;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Production;

public class frmBillOfMaterial_Manufacturing : Form
{
	private int RecId;

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	public DataTable dtSupply = new DataTable();

	private double HeaderWeight;

	private DataTable dtUom = new DataTable();

	private DataTable dtRateUom = new DataTable();

	private DataTable dtInput = new DataTable();

	private DataTable dtOHAccounts = new DataTable();

	private DataTable dtPMItems = new DataTable();

	private DataTable dtPMItemsUOM = new DataTable();

	private DataTable dtPackingMaterial = new DataTable();

	private DataTable dtPriceList = new DataTable();

	private DataTable dtHistoryHeader = new DataTable();

	private DataTable dtOverHead = new DataTable();

	private DataTable dtOperation = new DataTable();

	private DataTable dtuomGetAll = new DataTable();

	private DataTable dtbomHeadermanu = new DataTable();

	private DataTable dtAccounts = new DataTable();

	private bool UpdateMode;

	private int DefaultDaysToLessFromHistoryFromDate;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton toolStripButton1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton Print;

	private Panel panel5;

	private Panel panel6;

	private Panel panel10;

	private Label label1;

	internal GridEX grdInputMaterial;

	private CheckBox chkIsDefault;

	private CheckBox ChkIsActive;

	private TextBox txtQty;

	private Label label18;

	internal UltraCombo cmbUOM;

	private Label label44;

	internal UltraCombo cmbItem;

	private Label label7;

	private TextBox txtRecipeName;

	private Label label11;

	private Label label12;

	private TabPage tabPage3;

	private Label label13;

	private DateTimePicker txtEffectedDate;

	private Panel panel11;

	private Panel panel13;

	private Panel panel7;

	private Panel panel14;

	internal GridEX grdOverHead;

	private Panel panel15;

	private Label label2;

	private Panel panel8;

	internal GridEX grdPackingMaterial;

	private Panel panel12;

	private Label label3;

	internal GridEX grdPriceList;

	private Panel panel31;

	private Label label4;

	internal UltraCombo cmbtype;

	private Panel panel16;

	private Panel txtAddOverHead;

	private TextBox txtNetValue;

	private Label label10;

	private TextBox txtOverHead;

	private Label label9;

	private TextBox txtLessByProduct;

	private Label label8;

	private TextBox txtTotalInputs;

	private Label label6;

	private GroupBox groupBox1;

	private Button btnShowForPriceList;

	internal UltraCombo CmbItemCategory;

	private Label label21;

	internal UltraCombo CmbItemType;

	private Label label5;

	private Button BtnLoadForInput;

	private Label label22;

	private ToolStripButton btnrefresh;

	private Panel PanelHistoryMain;

	internal GridEX DatagridHistoryOverHead;

	internal GridEX DatagridHistoryInput;

	private Panel PanelHistoryMaingrid;

	private Panel PanelhistoryToolStrip;

	private ToolStrip toolStripHistory;

	private ToolStripButton btnHistoryRefresh;

	private ToolStripButton toolStripButton6;

	internal GridEX DatagridHistoryPackMaterial;

	private TextBox txtDocno;

	private Label label14;

	private Label label15;

	internal UltraCombo txtPriceFilterDate;

	internal UltraCombo cmbCropYearFilter;

	private Label label20;

	private Label label23;

	internal UltraCombo cmbcropYear;

	private ToolStripButton BtnPriceList;

	private ToolStripButton BtnPriceListSale;

	private Panel panel45;

	private Label label92;

	private Panel panel23;

	private GroupBox groupBox14;

	private DateTimePicker datfromDateHistory;

	private Label label164;

	private TextBox txtDocNoFrom;

	internal UltraCombo CmbItemNameHistory;

	private Label label49;

	private Label label163;

	private Label label48;

	private Button btnSearchHistory;

	private Label label52;

	internal UltraCombo CmbRecipeHistory;

	private DateTimePicker datToDateHistory;

	private Label label50;

	private TextBox txtDocNumberTo;

	private Panel panel24;

	internal GridEX DatagridHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Panel panel18;

	private Panel panel19;

	private Panel panel21;

	private Panel panel26;

	private Panel panel22;

	private Label label19;

	private Panel panel25;

	private Panel panel20;

	private Label label17;

	private Panel panel9;

	private Panel panel27;

	private Panel panel17;

	private Label label16;

	private Label label24;

	internal UltraCombo CmbCropYearHistory;

	public frmBillOfMaterial_Manufacturing(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmBillOfMaterial_Manufacturing_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 625);
			formright = CommonServices.SetRightsValueInRightsObject("frmBillOfMaterialManufacturing");
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			Print.Enabled = formright.DoHavePrintRights;
			txtRecipeName.Focus();
			dtSupply.Columns.Add("ItemId");
			dtSupply.Columns.Add("ItemName");
			dtSupply.Columns.Add("CropYearId");
			dtSupply.Columns.Add("CropYear");
			dtSupply.Columns.Add("ItemPrice");
			dtSupply.Columns.Add("RateUOMId");
			dtSupply.Columns.Add("RateUOM");
			dtInput.Columns.Add("ItemId");
			dtInput.Columns.Add("ItemName");
			dtInput.Columns.Add("CropYearId");
			dtInput.Columns.Add("CropYear");
			dtInput.Columns.Add("ItemPrice", typeof(double));
			dtInput.Columns.Add("RateUOMId", typeof(double));
			dtInput.Columns.Add("RateUOM", typeof(double));
			dtInput.Columns.Add("Percentage", typeof(double));
			dtInput.Columns.Add("PercentRate", typeof(double));
			dtInput.Columns.Add("Weight", typeof(double));
			dtInput.Columns.Add("PackingMaterialAmount", typeof(double));
			dtInput.Columns.Add("OverHeadsAmount", typeof(double));
			dtInput.Columns.Add("Amount", typeof(double));
			dtInput.Columns.Add("Remarks");
			grdInputMaterial.DataSource = dtInput;
			grdInputMaterial.RetrieveStructure();
			grdInputMaterialSetting();
			dtPackingMaterial.Columns.Add("ItemId");
			dtPackingMaterial.Columns.Add("ItemName");
			dtPackingMaterial.Columns.Add("QTY", typeof(double));
			dtPackingMaterial.Columns.Add("Rate", typeof(double));
			dtPackingMaterial.Columns.Add("Amount", typeof(double));
			dtPackingMaterial.Columns.Add("Remarks");
			AddRowInPackingMaterial1Grid();
			grdPackingMaterial.RetrieveStructure();
			grdPackingMaterialGridSettings();
			dtAccounts.Columns.Add("Id");
			dtAccounts.Columns.Add("AccountTitle");
			AccountTitleFill();
			dtOverHead.Columns.Add("Id");
			dtOverHead.Columns.Add("OverHeadAccountId");
			dtOverHead.Columns.Add("OverHeadAccountTitle");
			dtOverHead.Columns.Add("Qty");
			dtOverHead.Columns.Add("Rate", typeof(double));
			dtOverHead.Columns.Add("Amount", typeof(double));
			dtOverHead.Columns.Add("Remarks");
			AddRowInOverHeadGrid();
			grdOverHead.RetrieveStructure();
			grdOverHeadGridSettings();
			dtPriceList.Columns.Add("EffectedDateFrom");
			dtPriceList.Columns.Add("ItemId");
			dtPriceList.Columns.Add("ItemName");
			dtPriceList.Columns.Add("CropYearId");
			dtPriceList.Columns.Add("CropYear");
			dtPriceList.Columns.Add("ItemPrice");
			dtPriceList.Columns.Add("RateUOMId");
			dtPriceList.Columns.Add("RateUOM");
			AddRowInPriceListGrid();
			grdPriceList.RetrieveStructure();
			grdpPicelistSetting();
			dtHistoryHeader.Columns.Add("Id");
			dtHistoryHeader.Columns.Add("EffectedDate", typeof(DateTime));
			dtHistoryHeader.Columns.Add("DocNo", typeof(int));
			dtHistoryHeader.Columns.Add("RecipeName");
			dtHistoryHeader.Columns.Add("Type");
			dtHistoryHeader.Columns.Add("ItemId");
			dtHistoryHeader.Columns.Add("ItemName");
			dtHistoryHeader.Columns.Add("CropYear");
			dtHistoryHeader.Columns.Add("ItemUOM");
			dtHistoryHeader.Columns.Add("ItemQty", typeof(double));
			dtHistoryHeader.Columns.Add("IsActive");
			dtHistoryHeader.Columns.Add("IsDefault");
			GenerateCode();
			ItemFill();
			CropYearbind();
			HistoryGridFill();
			TypeBind();
			ItemTypeBind();
			ItemCategoryBind();
			BindDatePriceListFilter();
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			HistoryComboFill();
			datfromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			datToDateHistory.Value = DateTime.Now;
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
				DDL.BindDDLNew(dtCrop, cmbcropYear, "Id", "CropYear", "CropYear", false);
				DDL.BindDDLNew(dtCrop, cmbCropYearFilter, "Id", "CropYear", "CropYear", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TypeBind()
	{
		DataTable DtType = new DataTable();
		DtType.Columns.Add("Id");
		DtType.Columns.Add("Type");
		DtType.Rows.Add(1, "Semi-Finish Goods");
		DtType.Rows.Add(2, "Finish Goods");
		if (DtType.Rows.Count > 0)
		{
			DDL.BindDDLNew(DtType, cmbtype, "Id", "Type", "Type Title", false);
			((UltraGridBase)cmbtype).Rows[0].Activate();
		}
	}

	private void BindDatePriceListFilter()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		DataTable DtDate = ItemPricingSchedule.GetDateForPriceList(new ItemPricingSchedule
		{
			OrganizationId = UserAccount.OrganizationId,
			CompanyId = UserAccount.CompanyId
		});
		DataTable DtDatePriceList = new DataTable();
		DtDatePriceList.Columns.Add("Id");
		DtDatePriceList.Columns.Add("EffectiveDate");
		if (DtDate.Rows.Count > 0)
		{
			DtDatePriceList.Rows.Clear();
			for (int i = 0; i < DtDate.Rows.Count; i++)
			{
				DtDatePriceList.Rows.Add(DtDate.Rows[i]["EffectiveDate"], DtDate.Rows[i]["EffectiveDate"]);
			}
		}
		if (DtDatePriceList.Rows.Count > 0)
		{
			DDL.BindDDLNew(DtDatePriceList, txtPriceFilterDate, "Id", "EffectiveDate", "Effective Date", false);
			((UltraGridBase)txtPriceFilterDate).Rows[0].Activate();
		}
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			int code = InvManufacturingBOMHeader.GenerateCode(new InvManufacturingBOMHeader
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
			if (code > 0)
			{
				txtDocno.Text = Conversion.ToString((object)code);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new Item
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtitem = CommonServices.ReadAllForItemsForBrands();
			if (dtitem.Rows.Count > 0)
			{
				DDL.BindDDL(dtitem, cmbItem, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable listUOM = UOMSchedule.Getall(new UOMSchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (listUOM.Rows.Count > 0)
			{
				DataTable dtUOM = new DataTable();
				dtUOM.Columns.Add("Id");
				dtUOM.Columns.Add("UOM");
				dtUOM.Columns.Add("UOMCode");
				dtUOM.Rows.Clear();
				for (int i = 0; i < listUOM.Rows.Count; i++)
				{
					if (Conversion.ToInt(listUOM.Rows[i]["ItemId"]) == Conversion.ToInt(cmbItem.Value))
					{
						dtUOM.Rows.Add(listUOM.Rows[i]["Id"], listUOM.Rows[i]["UOMCode"], listUOM.Rows[i]["Equivalent"]);
					}
					if (dtUOM.Rows.Count > 0)
					{
						DDL.BindDDL(dtUOM, cmbUOM, "Id", "UOM", "PackUOM", ZeroIndex: false);
						((UltraGridBase)cmbUOM).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					}
				}
			}
			CalculateWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemCategoryBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		DataTable dt = ItemPricingSchedule.GetItemCategoryFromPricingShedule(new ReportsParameters
		{
			OrganizationId = UserAccount.OrganizationId,
			CompanyId = UserAccount.CompanyId,
			PriceTypeId = 6
		});
		if (dt.Rows.Count > 0)
		{
			DDL.BindDDLNew(dt, CmbItemCategory, "ItemCategoryId", "CategoryDescription", "Item Category", false);
			((UltraGridBase)CmbItemCategory).DisplayLayout.Bands[0].Columns[1].Width = 200;
		}
	}

	private void ItemTypeBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		DataTable dt = ItemPricingSchedule.GetItemTypesFromPricingShedule(new ReportsParameters
		{
			OrganizationId = UserAccount.OrganizationId,
			CompanyId = UserAccount.CompanyId,
			PriceTypeId = 6
		});
		if (dt.Rows.Count > 0)
		{
			DDL.BindDDLNew(dt, CmbItemType, "ItemTypeId", "TypeDescription", "Item Type", false);
			((UltraGridBase)CmbItemType).DisplayLayout.Bands[0].Columns[1].Width = 200;
		}
	}

	private void cmbUOM_Leave(object sender, EventArgs e)
	{
		CalculateWeight();
		CalculateTotals();
	}

	private void txtQty_Leave(object sender, EventArgs e)
	{
		CalculateWeight();
		CalculateTotals();
	}

	private void AccountTitleFill()
	{
		try
		{
			DataTable dt = CommonServices.CoaAllocationGetAllServiceBind();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			dtAccounts.Rows.Clear();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 3 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 6 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 8)
				{
					dtAccounts.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["AccountTitle"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindRateUom(int ItemId)
	{
		try
		{
			if (dtuomGetAll.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtuomGetAll.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtuomGetAll.Rows[i]["ItemId"]) == Conversion.ToInt((object)ItemId))
				{
					dtRateUom.Rows.Add(dtuomGetAll.Rows[i]["Id"], dtuomGetAll.Rows[i]["Equivalent"]);
				}
			}
			_ = dtRateUom.Rows.Count;
			_ = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindUom(int ItemId)
	{
		try
		{
			dtUom.Clear();
			for (int i = 0; i < dtuomGetAll.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtuomGetAll.Rows[i]["ItemId"]) == Conversion.ToInt((object)ItemId))
				{
					dtUom.Rows.Add(dtuomGetAll.Rows[i]["Id"], dtuomGetAll.Rows[i]["UOMDescription"]);
				}
			}
			_ = dtUom.Rows.Count;
			_ = 0;
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
		//IL_005c: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtRecipe = HelperMethods.CreateDataTable("Id", "name");
			DataTable dtItem = HelperMethods.CreateDataTable("Id", "name");
			DataTable dtCrop = HelperMethods.CreateDataTable("Id", "name");
			DataTable dt = InvManufacturingBOMHeader.GetDataForDropDownFromInvManufacturingBOMHeader(val);
			if (dt != null && dt.Rows.Count > 0)
			{
				foreach (DataRow row in dt.Rows)
				{
					switch (Conversion.ToString(row["Activity"]))
					{
					case "RecipeName":
						dtRecipe.Rows.Add(row["Id"], row["ReferenceName"]);
						break;
					case "BomItem":
						dtItem.Rows.Add(row["Id"], row["ReferenceName"]);
						break;
					case "CropYear":
						dtCrop.Rows.Add(row["Id"], row["ReferenceName"]);
						break;
					}
				}
			}
			DDL.BindDDL(dtRecipe, CmbRecipeHistory, "Id", "name", "Recipe Name", ZeroIndex: false);
			DDL.BindDDL(dtItem, CmbItemNameHistory, "Id", "name", "Item Name", ZeroIndex: false);
			DDL.BindDDL(dtCrop, CmbCropYearHistory, "Id", "name", "Crop Year", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTotals()
	{
		try
		{
			string sumoftotalInputs = grdInputMaterial.GetTotal(grdInputMaterial.RootTable.Columns["Amount"], (AggregateFunction)2).ToString();
			string sumoftotalPackingMaterial = grdPackingMaterial.GetTotal(grdPackingMaterial.RootTable.Columns["Amount"], (AggregateFunction)2).ToString();
			string sumofOverhead = grdOverHead.GetTotal(grdOverHead.RootTable.Columns["Amount"], (AggregateFunction)2).ToString();
			txtTotalInputs.Text = Math.Round(double.Parse(sumoftotalInputs), 2).ToString();
			txtOverHead.Text = Math.Round(double.Parse(sumofOverhead), 2).ToString();
			txtLessByProduct.Text = Math.Round(double.Parse(sumoftotalPackingMaterial), 2).ToString();
			txtNetValue.Text = (Conversion.ToDouble((object)txtTotalInputs.Text) + Conversion.ToDouble((object)txtOverHead.Text) - Conversion.ToDouble((object)txtLessByProduct.Text)).ToString();
			OHPMExpenses();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OHPMExpenses()
	{
		try
		{
			double TotalPMAmount = Conversion.ToDouble((object)grdPackingMaterial.GetTotal(grdPackingMaterial.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			double TotalOHAmount = Conversion.ToDouble((object)grdOverHead.GetTotal(grdOverHead.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			double TotalWeight = Conversion.ToDouble((object)grdInputMaterial.GetTotal(grdInputMaterial.RootTable.Columns["Weight"], (AggregateFunction)2).ToString());
			GridEXRow[] rows = grdInputMaterial.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				if (Conversion.ToDouble((object)r.Cells["Weight"].Value.ToString()) > 0.0)
				{
					double Weight = Conversion.ToDouble((object)r.Cells["Weight"].Value.ToString());
					r.Cells["OverHeadsAmount"].Value = TotalOHAmount / TotalWeight * Weight;
					r.Cells["PackingMaterialAmount"].Value = TotalPMAmount / TotalWeight * Weight;
					r.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInInputGrid()
	{
	}

	private void grdInputMaterialSetting()
	{
		try
		{
			grdInputMaterial.RootTable.Columns.Add("Delete");
			grdInputMaterial.RootTable.Columns["Delete"].Key = "Delete";
			grdInputMaterial.RootTable.Columns["Delete"].Caption = "X";
			grdInputMaterial.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInputMaterial.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInputMaterial.RootTable.Columns["Delete"].ButtonText = "X";
			grdInputMaterial.RootTable.Columns["Delete"].Width = 40;
			grdInputMaterial.RootTable.Columns["ItemId"].Visible = false;
			grdInputMaterial.RootTable.Columns["CropYearId"].Visible = false;
			grdInputMaterial.RootTable.Columns["RateUOMId"].Visible = false;
			grdInputMaterial.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["ItemPrice"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["PercentRate"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["Weight"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["OverHeadsAmount"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["PackingMaterialAmount"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["ItemPrice"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["Percentage"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["OverHeadsAmount"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["PackingMaterialAmount"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["ItemPrice"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["Percentage"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["OverHeadsAmount"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["PackingMaterialAmount"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["ItemPrice"].FormatString = "#,##0.##";
			grdInputMaterial.RootTable.Columns["Percentage"].FormatString = "#,##0.##";
			grdInputMaterial.RootTable.Columns["Weight"].FormatString = "#,##0.##";
			grdInputMaterial.RootTable.Columns["Amount"].FormatString = "#,##0.##";
			grdInputMaterial.RootTable.Columns["OverHeadsAmount"].FormatString = "#,##0.##";
			grdInputMaterial.RootTable.Columns["PackingMaterialAmount"].FormatString = "#,##0.##";
			grdInputMaterial.RootTable.Columns["Percentage"].TotalFormatString = "#,##0.##";
			grdInputMaterial.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			grdInputMaterial.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
			grdInputMaterial.RootTable.Columns["OverHeadsAmount"].TotalFormatString = "#,##0.##";
			grdInputMaterial.RootTable.Columns["PackingMaterialAmount"].TotalFormatString = "#,##0.##";
			grdInputMaterial.RootTable.Columns["ItemName"].Width = 120;
			grdInputMaterial.RootTable.Columns["ItemPrice"].Width = 70;
			grdInputMaterial.RootTable.Columns["Percentage"].Width = 40;
			grdInputMaterial.RootTable.Columns["PercentRate"].Width = 70;
			grdInputMaterial.RootTable.Columns["Weight"].Width = 70;
			grdInputMaterial.RootTable.Columns["Amount"].Width = 70;
			grdInputMaterial.RootTable.Columns["Remarks"].Width = 130;
			grdInputMaterial.RootTable.Columns["Percentage"].Caption = " % ";
			grdInputMaterial.RootTable.Columns["PercentRate"].Caption = " % Rate ";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateWeight()
	{
		try
		{
			if (((UltraGridBase)cmbUOM).ActiveRow != null && ((Control)(object)cmbUOM).Text != string.Empty && txtQty.Text != string.Empty)
			{
				double ItemPack = Conversion.ToDouble(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value);
				double Qty = double.Parse(txtQty.Text);
				HeaderWeight = Conversion.ToDouble((object)(ItemPack * Qty).ToString("0,0"));
				GrdInputValueUpdate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputMaterial_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdInputMaterial.UpdateData();
			if (e.Column.Key == "Percentage")
			{
				GrdInputValueUpdate();
				Percentage();
			}
			grdInputMaterial.UpdateData();
			CalculateTotals();
			cmbUOM_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdInputValueUpdate()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		GridEXRow[] rows = grdInputMaterial.GetRows();
		foreach (GridEXRow item in rows)
		{
			if (item != null && (int)item.RowType == 0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
			{
				if (Math.Abs(Conversion.ToInt(item.Cells["Percentage"].Value)) > 100)
				{
					item.Cells["Percentage"].Value = 0;
					throw new Exception("Percentage mustbe less than 100");
				}
				double ItemPrice = Conversion.ToDouble(item.Cells["ItemPrice"].Value);
				double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
				double RateUOM = Conversion.ToDouble((object)item.Cells["RateUOM"].Value.ToString());
				double PercentRate = ItemPrice / 100.0 * Percentage;
				double Weight = HeaderWeight / 100.0 * Percentage;
				double Amount = Weight / RateUOM * ItemPrice;
				if (PercentRate > 0.0)
				{
					item.BeginEdit();
					item.Cells["PercentRate"].Value = Math.Round(PercentRate);
					item.Cells["Weight"].Value = Math.Round(Weight);
					item.Cells["Amount"].Value = Math.Round(Amount);
					item.EndEdit();
				}
			}
		}
	}

	private void Percentage()
	{
		double TotalPercentage = 0.0;
		GridEXRow[] rows = grdInputMaterial.GetRows();
		foreach (GridEXRow item in rows)
		{
			TotalPercentage += Conversion.ToDouble(item.Cells["Percentage"].Value);
		}
		if (TotalPercentage > 100.0)
		{
			double Percent = Conversion.ToDouble(grdInputMaterial.CurrentRow.Cells["Percentage"].Value);
			double i = TotalPercentage - 100.0;
			grdInputMaterial.CurrentRow.Cells["Percentage"].Value = Percent - i;
			MessageBox.Show("TotalPercentage Can not be Greater than 100...");
		}
	}

	private void grdInputMaterial_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdInputMaterial.CurrentRow;
				dtInput.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtInput.Rows.Count == 0)
				{
					AddRowInInputGrid();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void AddRowInPackingMaterial1Grid()
	{
		dtPackingMaterial.Rows.Add(0, "", 0, 0, 0, "");
		grdPackingMaterial.DataSource = dtPackingMaterial;
	}

	public void grdPackingMaterialGridSettings()
	{
		try
		{
			dtPMItems = CommonServices.GetItemByItemTypeId("14");
			grdPackingMaterial.RootTable.Columns["ItemId"].Visible = false;
			grdPackingMaterial.RootTable.Columns["ItemName"].ColumnType = (ColumnType)0;
			grdPackingMaterial.RootTable.Columns["ItemName"].LimitToList = true;
			grdPackingMaterial.RootTable.Columns["ItemName"].HasValueList = true;
			grdPackingMaterial.RootTable.Columns["ItemName"].EditType = (EditType)4;
			grdPackingMaterial.RootTable.Columns["ItemName"].Caption = "ItemName";
			grdPackingMaterial.RootTable.Columns["ItemName"].ValueList.PopulateValueList((IEnumerable)dtPMItems.DefaultView, "Id", "ItemName");
			grdPackingMaterial.RootTable.Columns["ItemName"].Width = 150;
			grdPackingMaterial.RootTable.Columns.Add("Add");
			grdPackingMaterial.RootTable.Columns["Add"].Key = "Add";
			grdPackingMaterial.RootTable.Columns["Add"].Caption = "+";
			grdPackingMaterial.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterial.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterial.RootTable.Columns["Add"].ButtonText = "+";
			grdPackingMaterial.RootTable.Columns["Add"].Width = 40;
			grdPackingMaterial.RootTable.Columns.Add("Delete");
			grdPackingMaterial.RootTable.Columns["Delete"].Key = "Delete";
			grdPackingMaterial.RootTable.Columns["Delete"].Caption = "X";
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonText = "X";
			grdPackingMaterial.RootTable.Columns["Delete"].Width = 40;
			grdPackingMaterial.RootTable.Columns["ItemId"].Visible = false;
			grdPackingMaterial.RootTable.Columns["QTY"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterial.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterial.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterial.RootTable.Columns["QTY"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["QTY"].FormatString = "0,0";
			grdPackingMaterial.RootTable.Columns["Rate"].FormatString = "0,0";
			grdPackingMaterial.RootTable.Columns["Amount"].FormatString = "0,0";
			grdPackingMaterial.RootTable.Columns["QTY"].Width = 70;
			grdPackingMaterial.RootTable.Columns["Amount"].Width = 80;
			grdPackingMaterial.RootTable.Columns["Rate"].Width = 70;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPackingMaterial.UpdateData();
			GridEXRow item = grdPackingMaterial.CurrentRow;
			if (e.Column.Key == "ItemName")
			{
				item.Cells["ItemId"].Value = item.Cells["ItemName"].Value;
			}
			if ((e.Column.Key == "QTY" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["QTY"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)item.Cells["QTY"].Value.ToString());
				double rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double Amount = num * rate;
				item.Cells["Amount"].Value = Amount;
			}
			grdPackingMaterial.UpdateData();
			CalculateTotals();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdPackingMaterial.CurrentRow;
				dtPackingMaterial.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
					CalculateTotals();
				}
				if (dtPackingMaterial.Rows.Count == 0)
				{
					AddRowInPackingMaterial1Grid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInPackingMaterial1Grid();
			}
			grdPackingMaterial.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInOverHeadGrid()
	{
		dtOverHead.Rows.Add(0, 0, "", 0, 0, 0, "");
		grdOverHead.DataSource = dtOverHead;
	}

	private void grdOverHeadGridSettings()
	{
		try
		{
			dtOHAccounts = CommonServices.CoaAllocationGetAllServiceBind();
			grdOverHead.RootTable.Columns.Add("Add");
			grdOverHead.RootTable.Columns["Add"].Key = "Add";
			grdOverHead.RootTable.Columns["Add"].Caption = "+";
			grdOverHead.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHead.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdOverHead.RootTable.Columns["Add"].ButtonText = "+";
			grdOverHead.RootTable.Columns["Add"].Width = 40;
			grdOverHead.RootTable.Columns.Add("Delete");
			grdOverHead.RootTable.Columns["Delete"].Key = "Delete";
			grdOverHead.RootTable.Columns["Delete"].Caption = "X";
			grdOverHead.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHead.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOverHead.RootTable.Columns["Delete"].ButtonText = "X";
			grdOverHead.RootTable.Columns["Delete"].Width = 40;
			grdOverHead.RootTable.Columns["OverHeadAccountTitle"].ColumnType = (ColumnType)0;
			grdOverHead.RootTable.Columns["OverHeadAccountTitle"].HasValueList = true;
			grdOverHead.RootTable.Columns["OverHeadAccountTitle"].LimitToList = true;
			grdOverHead.RootTable.Columns["OverHeadAccountTitle"].EditType = (EditType)4;
			grdOverHead.RootTable.Columns["OverHeadAccountTitle"].Caption = "AccountTitle";
			grdOverHead.RootTable.Columns["OverHeadAccountTitle"].ValueList.PopulateValueList((IEnumerable)dtOHAccounts.DefaultView, "Id", "AccountTitle");
			grdOverHead.RootTable.Columns["OverHeadAccountTitle"].Width = 150;
			grdOverHead.RootTable.Columns["Id"].Visible = false;
			grdOverHead.RootTable.Columns["OverHeadAccountId"].Visible = false;
			grdOverHead.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)2;
			grdOverHead.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdOverHead.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdOverHead.RootTable.Columns["Rate"].FormatString = "0,0";
			grdOverHead.RootTable.Columns["Rate"].Width = 70;
			grdOverHead.RootTable.Columns["Qty"].Width = 70;
			grdOverHead.RootTable.Columns["Amount"].Width = 90;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_CellUpdated_1(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOverHead.UpdateData();
			GridEXRow item = grdOverHead.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double amount = num * Rate;
				item.Cells["Amount"].Value = amount;
			}
			grdOverHead.UpdateData();
			CalculateTotals();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_ColumnButtonClick_1(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdOverHead.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					dtOverHead.Rows[item.RowIndex].Delete();
					item.Delete();
					CalculateTotals();
				}
				if (dtOverHead.Rows.Count == 0)
				{
					AddRowInOverHeadGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInOverHeadGrid();
			}
			grdOverHead.UpdateData();
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
			if (RecId == 0)
			{
				throw new Exception("Record not update because Id not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_0644: Unknown result type (might be due to invalid IL or missing references)
		//IL_064b: Expected O, but got Unknown
		//IL_0815: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Expected O, but got Unknown
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (!FormValidation())
			{
				return;
			}
			InvManufacturingBOMHeader bOMHeader = new InvManufacturingBOMHeader();
			bOMHeader.CompanyId = UserAccount.CompanyId;
			bOMHeader.OrganizationId = UserAccount.OrganizationId;
			bOMHeader.BomItemId = Conversion.ToInt(cmbItem.Value);
			bOMHeader.CropYearId = Conversion.ToInt(cmbcropYear.Value);
			bOMHeader.ItemUomId = Conversion.ToInt(cmbUOM.Value);
			bOMHeader.BomName = txtRecipeName.Text;
			bOMHeader.BomType = ((Control)(object)cmbtype).Text;
			bOMHeader.Id = RecId;
			bOMHeader.DocNo = Conversion.ToInt((object)txtDocno.Text);
			bOMHeader.EffectedDate = txtEffectedDate.Value;
			bOMHeader.BomBatchQty = Conversion.ToDecimal((object)txtQty.Text);
			if (ChkIsActive.Checked)
			{
				bOMHeader.IsActive = true;
			}
			else
			{
				bOMHeader.IsActive = false;
			}
			if (chkIsDefault.Checked)
			{
				bOMHeader.IsDefault = true;
			}
			else
			{
				bOMHeader.IsDefault = false;
			}
			bOMHeader.EntryUserId = UserAccount.ID;
			bOMHeader.EntryDate = DateTime.Now;
			bOMHeader.ModifyDate = DateTime.Now;
			bOMHeader.ModifyUserId = UserAccount.ID;
			bOMHeader.ApprovedUserId = UserAccount.ID;
			bOMHeader.ApprovedDate = DateTime.Now;
			bOMHeader.IsApproved = false;
			bOMHeader.InvManufacturingBOMMaterialsDetails = new List<InvManufacturingBOMMaterialDetail>();
			bOMHeader.InvManufacturingBOMProductsDetails = new List<InvManufacturingBOMBiProductDetail>();
			bOMHeader.InvManufacturingOverHeadDetails = new List<InvManufacturingBOMOverHeadsDetail>();
			bOMHeader.ManufacturingBOMPackingMaterialDetail = new List<ManufacturingBOMPackingMaterialDetail>();
			GridEXRow[] rows = grdInputMaterial.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToInt((object)r3.Cells["ItemId"].Value.ToString()) != 0)
				{
					InvManufacturingBOMMaterialDetail bOMMaterialDetail = new InvManufacturingBOMMaterialDetail();
					if (r3.Cells["ItemId"].Text == string.Empty)
					{
						MessageBox.Show("Item Field Required In InPut Grid ");
						return;
					}
					bOMMaterialDetail.sItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
					if (r3.Cells["CropYearId"].Text == string.Empty || r3.Cells["CropYearId"].Text == "0")
					{
						MessageBox.Show("CropYear Field Required In InPut Grid ");
						return;
					}
					bOMMaterialDetail.CropYearId = Conversion.ToInt(r3.Cells["CropYearId"].Value);
					if (Conversion.ToDouble((object)r3.Cells["ItemPrice"].Text) == 0.0)
					{
						MessageBox.Show("ItemPrice Field Required In InPut Grid ");
						return;
					}
					bOMMaterialDetail.ItemPrice = Conversion.ToDouble((object)r3.Cells["ItemPrice"].Text);
					if (Conversion.ToDouble((object)r3.Cells["PercentRate"].Text) == 0.0)
					{
						MessageBox.Show("PercentRate Field Required In InPut Grid ");
						return;
					}
					bOMMaterialDetail.PercentRate = Conversion.ToDouble((object)r3.Cells["PercentRate"].Text);
					if (Conversion.ToDouble((object)r3.Cells["Percentage"].Text) == 0.0)
					{
						MessageBox.Show("Percentage Field Required In InPut Grid ");
						return;
					}
					bOMMaterialDetail.Percentage = Conversion.ToDouble((object)r3.Cells["Percentage"].Text);
					if (Conversion.ToDouble((object)r3.Cells["Weight"].Text) == 0.0)
					{
						MessageBox.Show("Weight Field Required In InPut Grid");
						return;
					}
					bOMMaterialDetail.sWeight = Conversion.ToDouble(r3.Cells["Weight"].Value);
					if (Conversion.ToDouble((object)r3.Cells["Amount"].Text) == 0.0)
					{
						MessageBox.Show("AmountField Required In InPut Grid ");
						return;
					}
					bOMMaterialDetail.sAmount = Conversion.ToDouble(r3.Cells["Amount"].Value);
					if (r3.Cells["RateUOMId"].Text == string.Empty)
					{
						MessageBox.Show("RateUOM Required In InPut Grid ");
						return;
					}
					bOMMaterialDetail.sRateUomId = Conversion.ToInt(r3.Cells["RateUOMId"].Value);
					bOMMaterialDetail.sRate = Conversion.ToInt(r3.Cells["RateUOM"].Value);
					if (r3.Cells["Remarks"].Text == string.Empty)
					{
						MessageBox.Show("Remarks Required In InPut Grid ");
						return;
					}
					bOMMaterialDetail.Remarks = Conversion.ToString(r3.Cells["Remarks"].Value);
					bOMMaterialDetail.PackingMaterialAmount = Conversion.ToDouble(r3.Cells["PackingMaterialAmount"].Value);
					bOMMaterialDetail.OverHeadsAmount = Conversion.ToDouble(r3.Cells["OverHeadsAmount"].Value);
					bOMHeader.InvManufacturingBOMMaterialsDetails.Add(bOMMaterialDetail);
				}
			}
			if (grdInputMaterial.CurrentRow.Cells["ItemId"].Text != string.Empty || Conversion.ToInt(grdInputMaterial.CurrentRow.Cells["ItemId"].Value) != 0)
			{
				rows = grdPackingMaterial.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (Conversion.ToInt((object)r2.Cells["ItemId"].Value.ToString()) != 0)
					{
						ManufacturingBOMPackingMaterialDetail packingMaterialDetail = new ManufacturingBOMPackingMaterialDetail();
						if (r2.Cells["ItemId"].Text == string.Empty)
						{
							MessageBox.Show("Item Field Required In Packing Material grid ");
							return;
						}
						packingMaterialDetail.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
						packingMaterialDetail.Qty = Conversion.ToDouble((object)r2.Cells["QTY"].Text);
						packingMaterialDetail.Rate = Conversion.ToDouble(r2.Cells["Rate"].Value);
						if (Conversion.ToDouble((object)r2.Cells["Amount"].Text) == 0.0)
						{
							MessageBox.Show("AmountField Require ");
							return;
						}
						packingMaterialDetail.Amount = Conversion.ToDouble(r2.Cells["Amount"].Value);
						if (r2.Cells["Remarks"].Text == string.Empty)
						{
							MessageBox.Show("Remarks Field Require ");
							return;
						}
						packingMaterialDetail.pmRemarks = Conversion.ToString(r2.Cells["Remarks"].Value);
						bOMHeader.ManufacturingBOMPackingMaterialDetail.Add(packingMaterialDetail);
					}
				}
			}
			if (grdOverHead.CurrentRow.Cells["OverHeadAccountTitle"].Text != string.Empty || Conversion.ToInt((object)grdOverHead.CurrentRow.Cells["OverHeadAccountTitle"].Text) != 0)
			{
				rows = grdOverHead.GetRows();
				foreach (GridEXRow r in rows)
				{
					InvManufacturingBOMOverHeadsDetail overHeadsDetail = new InvManufacturingBOMOverHeadsDetail();
					if (r.Cells["OverHeadAccountTitle"].Text == string.Empty)
					{
						MessageBox.Show("AccountTitle Field Required In OverHead Grid ");
						return;
					}
					overHeadsDetail.OverHeadTypeId = Conversion.ToInt(r.Cells["OverHeadAccountTitle"].Value);
					overHeadsDetail.Qty = Conversion.ToDouble((object)r.Cells["Qty"].Text);
					overHeadsDetail.Rate = Conversion.ToDouble((object)r.Cells["Rate"].Text);
					if (Conversion.ToDouble((object)r.Cells["Amount"].Text) == 0.0)
					{
						MessageBox.Show("Amount Field Required In OverHead Grid  ");
						return;
					}
					overHeadsDetail.ohAmount = Conversion.ToDouble((object)r.Cells["Amount"].Text);
					if (r.Cells["Remarks"].Text == string.Empty)
					{
						MessageBox.Show("Remarks Field Required In OverHead Grid  ");
						return;
					}
					overHeadsDetail.OhRemarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					bOMHeader.InvManufacturingOverHeadDetails.Add(overHeadsDetail);
				}
			}
			InvManufacturingBOMHeader.Save(bOMHeader);
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....[]");
			}
			else
			{
				MessageBox.Show("Data Save Successfully....[]");
			}
			FromReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtRecipeName.Text.Trim() == string.Empty || txtRecipeName.Text == "0")
		{
			MessageBox.Show("RecipeName Field Required");
			txtRecipeName.Focus();
			return false;
		}
		if (((Control)(object)cmbtype).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Type Field Required");
			cmbtype.Focus();
			return false;
		}
		if (((Control)(object)cmbItem).Text.Trim() == string.Empty || Conversion.ToInt(cmbItem.Value) == 0)
		{
			MessageBox.Show("Item Name Feild Required");
			cmbItem.Focus();
			return false;
		}
		if (((Control)(object)cmbcropYear).Text.Trim() == string.Empty || Conversion.ToInt(cmbcropYear.Value) == 0)
		{
			MessageBox.Show("Crop Year Feild Required");
			cmbcropYear.Focus();
			return false;
		}
		if (((Control)(object)cmbUOM).Text.Trim() == string.Empty || Conversion.ToInt(cmbUOM.Value) == 0)
		{
			MessageBox.Show("UOM Field Required");
			cmbUOM.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == string.Empty || txtQty.Text == "0")
		{
			MessageBox.Show("Qty Field Required");
			txtQty.Focus();
			return false;
		}
		return true;
	}

	private void DatagridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int Id)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			dtInput.Clear();
			dtPackingMaterial.Clear();
			dtOverHead.Clear();
			grdInputMaterial.ClearStructure();
			grdPackingMaterial.ClearStructure();
			grdOverHead.ClearStructure();
			InvManufacturingBOMHeader bOMHeader = new InvManufacturingBOMHeader();
			RecId = Id;
			bOMHeader = InvManufacturingBOMHeader.GetByID(RecId);
			tabControl1.SelectedIndex = 0;
			if (bOMHeader == null)
			{
				return;
			}
			cmbItem.Value = bOMHeader.BomItemId;
			cmbItem_Leave(null, null);
			cmbcropYear.Value = bOMHeader.CropYearId;
			cmbUOM.Value = bOMHeader.ItemUomId;
			txtDocno.Text = bOMHeader.DocNo.ToString();
			txtRecipeName.Text = bOMHeader.BomName;
			((Control)(object)cmbtype).Text = bOMHeader.BomType;
			txtEffectedDate.Value = bOMHeader.EffectedDate;
			txtQty.Text = bOMHeader.BomBatchQty.ToString();
			ChkIsActive.Checked = bOMHeader.IsActive;
			chkIsDefault.Checked = bOMHeader.IsDefault;
			if (bOMHeader.InvManufacturingBOMMaterialsDetails.Count > 0)
			{
				for (int k = 0; k < bOMHeader.InvManufacturingBOMMaterialsDetails.Count; k++)
				{
					dtInput.Rows.Add(bOMHeader.InvManufacturingBOMMaterialsDetails[k].sItemId, bOMHeader.InvManufacturingBOMMaterialsDetails[k].ItemName, bOMHeader.InvManufacturingBOMMaterialsDetails[k].CropYearId, bOMHeader.InvManufacturingBOMMaterialsDetails[k].CropYear, bOMHeader.InvManufacturingBOMMaterialsDetails[k].ItemPrice, bOMHeader.InvManufacturingBOMMaterialsDetails[k].sRateUomId, bOMHeader.InvManufacturingBOMMaterialsDetails[k].RateEquivalent, bOMHeader.InvManufacturingBOMMaterialsDetails[k].Percentage, bOMHeader.InvManufacturingBOMMaterialsDetails[k].PercentRate, bOMHeader.InvManufacturingBOMMaterialsDetails[k].sWeight, bOMHeader.InvManufacturingBOMMaterialsDetails[k].PackingMaterialAmount, bOMHeader.InvManufacturingBOMMaterialsDetails[k].OverHeadsAmount, bOMHeader.InvManufacturingBOMMaterialsDetails[k].sAmount, bOMHeader.InvManufacturingBOMMaterialsDetails[k].Remarks);
				}
			}
			else
			{
				AddRowInInputGrid();
			}
			if (bOMHeader.ManufacturingBOMPackingMaterialDetail.Count > 0)
			{
				for (int j = 0; j < bOMHeader.ManufacturingBOMPackingMaterialDetail.Count; j++)
				{
					dtPackingMaterial.Rows.Add(bOMHeader.ManufacturingBOMPackingMaterialDetail[j].ItemId, bOMHeader.ManufacturingBOMPackingMaterialDetail[j].ItemName, bOMHeader.ManufacturingBOMPackingMaterialDetail[j].Qty, bOMHeader.ManufacturingBOMPackingMaterialDetail[j].Rate, bOMHeader.ManufacturingBOMPackingMaterialDetail[j].Amount, bOMHeader.ManufacturingBOMPackingMaterialDetail[j].pmRemarks);
				}
			}
			else
			{
				AddRowInPackingMaterial1Grid();
			}
			if (bOMHeader.InvManufacturingOverHeadDetails.Count > 0)
			{
				for (int i = 0; i < bOMHeader.InvManufacturingOverHeadDetails.Count; i++)
				{
					dtOverHead.Rows.Add(bOMHeader.InvManufacturingOverHeadDetails[i].Id, bOMHeader.InvManufacturingOverHeadDetails[i].OverHeadTypeId, bOMHeader.InvManufacturingOverHeadDetails[i].OverHeadAccountTitle, bOMHeader.InvManufacturingOverHeadDetails[i].Qty, bOMHeader.InvManufacturingOverHeadDetails[i].Rate, bOMHeader.InvManufacturingOverHeadDetails[i].ohAmount, bOMHeader.InvManufacturingOverHeadDetails[i].OhRemarks);
				}
			}
			else
			{
				AddRowInOverHeadGrid();
			}
			grdInputMaterial.DataSource = dtInput;
			grdPackingMaterial.DataSource = dtPackingMaterial;
			grdOverHead.DataSource = dtOverHead;
			grdOverHead.RetrieveStructure();
			grdInputMaterial.RetrieveStructure();
			grdPackingMaterial.RetrieveStructure();
			grdInputMaterialSetting();
			grdPackingMaterialGridSettings();
			grdOverHeadGridSettings();
			UpdateMode = true;
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			CalculateTotals();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void FromReset()
	{
		try
		{
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			txtRecipeName.Text = string.Empty;
			txtEffectedDate.Value = DateTime.Now;
			((Control)(object)cmbtype).Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbUOM).Text = string.Empty;
			txtQty.Text = string.Empty;
			((Control)(object)CmbItemCategory).Text = string.Empty;
			((Control)(object)CmbItemType).Text = string.Empty;
			ItemFill();
			dtInput.Rows.Clear();
			dtPackingMaterial.Rows.Clear();
			dtPriceList.Rows.Clear();
			dtOverHead.Rows.Clear();
			grdInputMaterial.ClearStructure();
			grdPackingMaterial.ClearStructure();
			grdOverHead.ClearStructure();
			grdPriceList.ClearStructure();
			UpdateMode = false;
			AddRowInInputGrid();
			grdInputMaterial.RetrieveStructure();
			grdInputMaterialSetting();
			AddRowInPackingMaterial1Grid();
			grdPackingMaterial.RetrieveStructure();
			grdPackingMaterialGridSettings();
			AddRowInPriceListGrid();
			grdPriceList.RetrieveStructure();
			grdpPicelistSetting();
			AddRowInOverHeadGrid();
			grdOverHead.RetrieveStructure();
			grdOverHeadGridSettings();
			HistoryGridFill();
			RecId = 0;
			txtTotalInputs.Text = string.Empty;
			txtOverHead.Text = string.Empty;
			txtLessByProduct.Text = string.Empty;
			txtNetValue.Text = string.Empty;
			txtRecipeName.Focus();
			GenerateCode();
			BindDatePriceListFilter();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		FromReset();
	}

	private void btnrefresh_Click(object sender, EventArgs e)
	{
		ItemFill();
		TypeBind();
		CropYearbind();
		ItemCategoryBind();
		ItemCategoryBind();
		ItemTypeBind();
		BindDatePriceListFilter();
	}

	private void HistoryGridFill()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			DatagridHistory.ClearStructure();
			DatagridHistoryInput.ClearStructure();
			DatagridHistoryOverHead.ClearStructure();
			DatagridHistoryPackMaterial.ClearStructure();
			ReportsParameters obj = new ReportsParameters();
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			if (drdocdate.Checked)
			{
				if (datfromDateHistory.Checked)
				{
					obj.FromDate = datfromDateHistory.Value;
				}
				if (datToDateHistory.Checked)
				{
					obj.ToDate = datToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (datfromDateHistory.Checked)
				{
					obj.EntryDateFrom = datfromDateHistory.Value;
				}
				if (datToDateHistory.Checked)
				{
					obj.EntryDateTo = datToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (datfromDateHistory.Checked)
				{
					obj.ModifyDateFrom = datfromDateHistory.Value;
				}
				if (datToDateHistory.Checked)
				{
					obj.ModifyDateTo = datToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (datfromDateHistory.Checked)
				{
					obj.ApprovedDateFrom = datfromDateHistory.Value;
				}
				if (datToDateHistory.Checked)
				{
					obj.ApprovedDateTo = datToDateHistory.Value;
				}
			}
			obj.FromDocNo = Conversion.ToInt((object)txtDocNoFrom.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtDocNumberTo.Text);
			obj.Id = Conversion.ToInt(CmbRecipeHistory.Value);
			obj.ItemId = Conversion.ToInt(CmbItemNameHistory.Value);
			obj.CropYearId = Conversion.ToInt(CmbCropYearHistory.Value);
			DataTable dthis = new DataTable();
			dthis = InvManufacturingBOMHeader.GetAll(obj);
			if (dthis.Rows.Count <= 0)
			{
				return;
			}
			dtHistoryHeader.Rows.Clear();
			foreach (DataRow row in dthis.Rows)
			{
				dtHistoryHeader.Rows.Add(row["Id"], Conversion.ToDateTime(row["EffectedDate"]).ToString("dd-MMM-yyyy"), row["DocNo"], row["BomName"], row["BomType"], row["BomItemId"], row["ItemName"], row["CropYear"], row["UOMCode"], row["BomBatchQty"], row["IsActive"], row["IsDefault"]);
			}
			DatagridHistory.DataSource = dtHistoryHeader;
			DatagridHistory.RetrieveStructure();
			HistoryGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void HistoryGridSetting()
	{
		try
		{
			DatagridHistory.RootTable.Columns["Id"].Visible = false;
			DatagridHistory.RootTable.Columns["ItemId"].Visible = false;
			DatagridHistory.RootTable.Columns["RecipeName"].EditType = (EditType)0;
			DatagridHistory.RootTable.Columns["DocNo"].EditType = (EditType)0;
			DatagridHistory.RootTable.Columns["Type"].EditType = (EditType)0;
			DatagridHistory.RootTable.Columns["ItemName"].EditType = (EditType)0;
			DatagridHistory.RootTable.Columns["ItemUOM"].EditType = (EditType)0;
			DatagridHistory.RootTable.Columns["EffectedDate"].EditType = (EditType)0;
			DatagridHistory.RootTable.Columns["ItemQty"].EditType = (EditType)0;
			DatagridHistory.RootTable.Columns["IsActive"].EditType = (EditType)0;
			DatagridHistory.RootTable.Columns["IsDefault"].EditType = (EditType)0;
			DatagridHistory.RootTable.Columns["RecipeName"].FilterEditType = (FilterEditType)1;
			DatagridHistory.RootTable.Columns["DocNo"].FilterEditType = (FilterEditType)1;
			DatagridHistory.RootTable.Columns["Type"].FilterEditType = (FilterEditType)1;
			DatagridHistory.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			DatagridHistory.RootTable.Columns["ItemUOM"].FilterEditType = (FilterEditType)1;
			DatagridHistory.RootTable.Columns["EffectedDate"].FilterEditType = (FilterEditType)1;
			DatagridHistory.RootTable.Columns["ItemQty"].FilterEditType = (FilterEditType)1;
			DatagridHistory.RootTable.Columns["IsActive"].FilterEditType = (FilterEditType)1;
			DatagridHistory.RootTable.Columns["IsDefault"].FilterEditType = (FilterEditType)1;
			DatagridHistory.RootTable.Columns["RecipeName"].Width = 150;
			DatagridHistory.RootTable.Columns["Type"].Width = 120;
			DatagridHistory.RootTable.Columns["ItemName"].Width = 240;
			DatagridHistory.RootTable.Columns["ItemUOM"].Width = 80;
			DatagridHistory.RootTable.Columns["EffectedDate"].Width = 100;
			DatagridHistory.RootTable.Columns["ItemQty"].Width = 80;
			DatagridHistory.RootTable.Columns["IsActive"].Width = 80;
			DatagridHistory.RootTable.Columns["IsActive"].ColumnType = (ColumnType)3;
			DatagridHistory.RootTable.Columns["IsActive"].CheckBoxTrueValue = "True";
			DatagridHistory.RootTable.Columns["IsActive"].CheckBoxFalseValue = "False";
			DatagridHistory.RootTable.Columns["IsDefault"].Width = 80;
			DatagridHistory.RootTable.Columns["IsDefault"].ColumnType = (ColumnType)3;
			DatagridHistory.RootTable.Columns["IsDefault"].CheckBoxTrueValue = "True";
			DatagridHistory.RootTable.Columns["IsDefault"].CheckBoxFalseValue = "False";
			DatagridHistory.RootTable.Columns.Add("Edit");
			DatagridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Width = 50;
			DatagridHistory.RootTable.Columns["Edit"].Position = 0;
			DatagridHistory.RootTable.Columns.Add("Detail");
			DatagridHistory.RootTable.Columns["Detail"].Key = "Detail";
			DatagridHistory.RootTable.Columns["Detail"].Caption = "Detail";
			DatagridHistory.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Detail"].ButtonText = "Detail";
			DatagridHistory.RootTable.Columns["Detail"].Width = 50;
			DatagridHistory.RootTable.Columns["Detail"].Position = 1;
			DatagridHistory.RootTable.Columns.Add("Print");
			DatagridHistory.RootTable.Columns["Print"].Key = "Print";
			DatagridHistory.RootTable.Columns["Print"].Caption = "Print";
			DatagridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DatagridHistory.RootTable.Columns["Print"].Width = 50;
			DatagridHistory.RootTable.Columns["Print"].Position = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				ReadById(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "Print")
			{
				GenerateReport(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void InputDetailGridSetting()
	{
		try
		{
			DatagridHistoryInput.RootTable.Columns["ItemId"].Visible = false;
			DatagridHistoryInput.RootTable.Columns["CropYearId"].Visible = false;
			DatagridHistoryInput.RootTable.Columns["RateUOMId"].Visible = false;
			DatagridHistoryInput.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			DatagridHistoryInput.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			DatagridHistoryInput.RootTable.Columns["RateUOM"].TextAlignment = (TextAlignment)3;
			DatagridHistoryInput.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			DatagridHistoryInput.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			DatagridHistoryInput.RootTable.Columns["RateUOM"].FormatString = "0,0";
			DatagridHistoryInput.RootTable.Columns["Amount"].FormatString = "0,0";
			DatagridHistoryInput.RootTable.Columns["Weight"].FormatString = "0,0";
			DatagridHistoryInput.RootTable.Columns["ItemName"].Width = 140;
			DatagridHistoryInput.RootTable.Columns["RateUOM"].Width = 70;
			DatagridHistoryInput.RootTable.Columns["ItemPrice"].Width = 70;
			DatagridHistoryInput.RootTable.Columns["Percentage"].Width = 40;
			DatagridHistoryInput.RootTable.Columns["PercentRate"].Width = 70;
			DatagridHistoryInput.RootTable.Columns["Amount"].Width = 80;
			DatagridHistoryInput.RootTable.Columns["Percentage"].Caption = " % ";
			DatagridHistoryInput.RootTable.Columns["PercentRate"].Caption = " % Rate ";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void PMDetailGridSetting()
	{
		try
		{
			DatagridHistoryPackMaterial.RootTable.Columns["ItemId"].Visible = false;
			DatagridHistoryPackMaterial.RootTable.Columns["QTY"].AggregateFunction = (AggregateFunction)2;
			DatagridHistoryPackMaterial.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)2;
			DatagridHistoryPackMaterial.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			DatagridHistoryPackMaterial.RootTable.Columns["QTY"].TextAlignment = (TextAlignment)3;
			DatagridHistoryPackMaterial.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			DatagridHistoryPackMaterial.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			DatagridHistoryPackMaterial.RootTable.Columns["QTY"].FormatString = "0,0";
			DatagridHistoryPackMaterial.RootTable.Columns["Rate"].FormatString = "0,0";
			DatagridHistoryPackMaterial.RootTable.Columns["Amount"].FormatString = "0,0";
			DatagridHistoryPackMaterial.RootTable.Columns["ItemName"].Width = 120;
			DatagridHistoryPackMaterial.RootTable.Columns["QTY"].Width = 60;
			DatagridHistoryPackMaterial.RootTable.Columns["Rate"].Width = 60;
			DatagridHistoryPackMaterial.RootTable.Columns["Amount"].Width = 70;
			DatagridHistoryPackMaterial.RootTable.Columns["Remarks"].Width = 230;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void OverHeadDetailGridSetting()
	{
		try
		{
			DatagridHistoryOverHead.RootTable.Columns["Id"].Visible = false;
			DatagridHistoryOverHead.RootTable.Columns["OverHeadAccountId"].Visible = false;
			DatagridHistoryOverHead.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			DatagridHistoryOverHead.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)2;
			DatagridHistoryOverHead.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			DatagridHistoryOverHead.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			DatagridHistoryOverHead.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			DatagridHistoryOverHead.RootTable.Columns["Qty"].FormatString = "0,0";
			DatagridHistoryOverHead.RootTable.Columns["Rate"].FormatString = "0,0";
			DatagridHistoryOverHead.RootTable.Columns["Amount"].FormatString = "0,0";
			DatagridHistoryOverHead.RootTable.Columns["OverHeadAccountTitle"].Width = 180;
			DatagridHistoryOverHead.RootTable.Columns["Qty"].Width = 70;
			DatagridHistoryOverHead.RootTable.Columns["Rate"].Width = 70;
			DatagridHistoryOverHead.RootTable.Columns["Amount"].Width = 80;
			DatagridHistoryOverHead.RootTable.Columns["Remarks"].Width = 140;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnHistoryRefresh_Click(object sender, EventArgs e)
	{
		DatagridHistoryOverHead.ClearStructure();
		DatagridHistoryPackMaterial.ClearStructure();
		DatagridHistoryInput.ClearStructure();
	}

	private void frmBillOfMaterial_Manufacturing_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0 && !UpdateMode)
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
					btnHistoryRefresh_Click(null, null);
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
			if (e.Control && e.KeyCode == Keys.U)
			{
				if (tabControl1.SelectedIndex == 0 && UpdateMode)
				{
					btnUpdate_Click(null, null);
				}
				if (tabControl1.SelectedIndex == 1)
				{
					_ = UpdateMode;
				}
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 80;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			datfromDateHistory.Focus();
		}
	}

	private void btnShowForPriceList_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		grdPriceList.ClearStructure();
		dtPriceList.Rows.Clear();
		DataTable dt = ItemPricingScheduleForRice.GetItemAndRateForRecipe(new ReportsParameters
		{
			OrganizationId = UserAccount.OrganizationId,
			CompanyId = UserAccount.CompanyId,
			ItemCategoryId = Conversion.ToInt(CmbItemCategory.Value),
			ItemTypeId = Conversion.ToInt(CmbItemType.Value),
			CropYearId = Conversion.ToInt(cmbCropYearFilter.Value),
			EffectedDate = Conversion.ToDateTime(txtPriceFilterDate.Value)
		});
		if (dt.Rows.Count > 0)
		{
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dtPriceList.Rows.Add(Conversion.ToDateTime(dt.Rows[i]["EffectedDate"]).ToShortDateString(), dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["CropYearId"], dt.Rows[i]["CropYear"], dt.Rows[i]["ItemRate"], dt.Rows[i]["RateUomId"], dt.Rows[i]["RateUom"]);
			}
			grdPriceList.DataSource = dtPriceList;
			grdPriceList.RetrieveStructure();
			grdpPicelistSetting();
		}
		else
		{
			grdPriceList.ClearStructure();
		}
	}

	private void AddRowInPriceListGrid()
	{
		try
		{
			grdPriceList.DataSource = dtPriceList;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdpPicelistSetting()
	{
		try
		{
			grdPriceList.RootTable.Columns["CropYearId"].Visible = false;
			grdPriceList.RootTable.Columns["ItemId"].Visible = false;
			grdPriceList.RootTable.Columns["RateUOMId"].Visible = false;
			grdPriceList.RootTable.Columns["EffectedDateFrom"].Width = 160;
			grdPriceList.RootTable.Columns["ItemName"].Width = 200;
			grdPriceList.RootTable.Columns.Add("Select");
			grdPriceList.RootTable.Columns["Select"].ActAsSelector = true;
			grdPriceList.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdPriceList.RootTable.Columns["Select"].Width = 50;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadForInput_Click(object sender, EventArgs e)
	{
		try
		{
			if (int.Parse(grdPriceList.GetCheckedRows().Count().ToString()) > 0)
			{
				dtSupply.Rows.Clear();
				GridEXRow[] checkedRows = grdPriceList.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					dtSupply.Rows.Add(r.Cells["ItemId"].Value.ToString(), r.Cells["ItemName"].Value.ToString(), r.Cells["CropYearId"].Value.ToString(), r.Cells["CropYear"].Value.ToString(), r.Cells["ItemPrice"].Value.ToString(), r.Cells["RateUOMId"].Value.ToString(), r.Cells["RateUOM"].Value.ToString());
				}
				bool flag = false;
				for (int i = 0; i < dtSupply.Rows.Count; i++)
				{
					checkedRows = grdInputMaterial.GetRows();
					foreach (GridEXRow item in checkedRows)
					{
						if (grdInputMaterial.GetRows().Length != 0 && Conversion.ToInt(item.Cells["ItemId"].Value) == Conversion.ToInt(dtSupply.Rows[i]["ItemId"]) && Conversion.ToInt(item.Cells["CropYearId"].Value) == Conversion.ToInt(dtSupply.Rows[i]["CropYearId"]))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						dtInput.Rows.Add(dtSupply.Rows[i]["ItemId"], dtSupply.Rows[i]["ItemName"], dtSupply.Rows[i]["CropYearId"], dtSupply.Rows[i]["CropYear"], dtSupply.Rows[i]["ItemPrice"], dtSupply.Rows[i]["RateUOMId"], dtSupply.Rows[i]["RateUOM"], 0, 0, 0, 0, 0, 0, "");
					}
					flag = false;
				}
				if (dtInput.Rows.Count > 0)
				{
					grdInputMaterial.DataSource = dtInput;
					grdInputMaterial.RetrieveStructure();
					grdInputMaterialSetting();
				}
				else
				{
					MessageBox.Show("Chek the row first");
				}
			}
			else
			{
				MessageBox.Show("Chek the row first");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateReport(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dt = InvManufacturingBOMHeader.ReceipePrint(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvManufacturingBOMHeader.RecipeSubReport(obj.Id), "RecipeExpenseSubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "207-RecipeSlip.rpt", lstSubReports);
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void BtnPriceList_Click(object sender, EventArgs e)
	{
		try
		{
			new frmItemPricingScheduleForRice(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPriceListSale_Click(object sender, EventArgs e)
	{
		try
		{
			new frmItemSalesPricingSchedule(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnHistoryRefresh_Click_1(object sender, EventArgs e)
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

	private void btnSearchHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				InvManufacturingBOMHeader bOMHeader = new InvManufacturingBOMHeader();
				bOMHeader = InvManufacturingBOMHeader.GetByID(Conversion.ToInt(r.Cells["Id"].Value));
				DataTable dthistorydetailinput = new DataTable();
				dthistorydetailinput.Columns.Add("ItemId");
				dthistorydetailinput.Columns.Add("ItemName");
				dthistorydetailinput.Columns.Add("CropYearId");
				dthistorydetailinput.Columns.Add("CropYear");
				dthistorydetailinput.Columns.Add("ItemPrice", typeof(double));
				dthistorydetailinput.Columns.Add("RateUOMId", typeof(double));
				dthistorydetailinput.Columns.Add("RateUOM", typeof(double));
				dthistorydetailinput.Columns.Add("Percentage");
				dthistorydetailinput.Columns.Add("PercentRate", typeof(double));
				dthistorydetailinput.Columns.Add("PackingMaterialAmount", typeof(double));
				dthistorydetailinput.Columns.Add("OverHeadsAmount", typeof(double));
				dthistorydetailinput.Columns.Add("Amount", typeof(double));
				dthistorydetailinput.Columns.Add("Weight", typeof(double));
				dthistorydetailinput.Columns.Add("Remarks");
				DataTable dthistoryPM = new DataTable();
				dthistoryPM.Columns.Add("ItemId");
				dthistoryPM.Columns.Add("ItemName");
				dthistoryPM.Columns.Add("QTY", typeof(double));
				dthistoryPM.Columns.Add("Rate", typeof(double));
				dthistoryPM.Columns.Add("Amount", typeof(double));
				dthistoryPM.Columns.Add("Remarks");
				DataTable dthistorydetailOH = new DataTable();
				dthistorydetailOH.Columns.Add("Id");
				dthistorydetailOH.Columns.Add("OverHeadAccountId");
				dthistorydetailOH.Columns.Add("OverHeadAccountTitle");
				dthistorydetailOH.Columns.Add("Qty");
				dthistorydetailOH.Columns.Add("Rate", typeof(double));
				dthistorydetailOH.Columns.Add("Amount", typeof(double));
				dthistorydetailOH.Columns.Add("Remarks");
				for (int k = 0; k < bOMHeader.InvManufacturingBOMMaterialsDetails.Count; k++)
				{
					dthistorydetailinput.Rows.Add(bOMHeader.InvManufacturingBOMMaterialsDetails[k].sItemId, bOMHeader.InvManufacturingBOMMaterialsDetails[k].ItemName, bOMHeader.InvManufacturingBOMMaterialsDetails[k].CropYearId, bOMHeader.InvManufacturingBOMMaterialsDetails[k].CropYear, bOMHeader.InvManufacturingBOMMaterialsDetails[k].ItemPrice, bOMHeader.InvManufacturingBOMMaterialsDetails[k].sRateUomId, bOMHeader.InvManufacturingBOMMaterialsDetails[k].RateEquivalent, bOMHeader.InvManufacturingBOMMaterialsDetails[k].Percentage, bOMHeader.InvManufacturingBOMMaterialsDetails[k].PercentRate, bOMHeader.InvManufacturingBOMMaterialsDetails[k].PackingMaterialAmount, bOMHeader.InvManufacturingBOMMaterialsDetails[k].OverHeadsAmount, bOMHeader.InvManufacturingBOMMaterialsDetails[k].sAmount, bOMHeader.InvManufacturingBOMMaterialsDetails[k].sWeight, bOMHeader.InvManufacturingBOMMaterialsDetails[k].Remarks);
				}
				DatagridHistoryInput.DataSource = dthistorydetailinput;
				DatagridHistoryInput.RetrieveStructure();
				for (int j = 0; j < bOMHeader.ManufacturingBOMPackingMaterialDetail.Count; j++)
				{
					dthistoryPM.Rows.Add(bOMHeader.ManufacturingBOMPackingMaterialDetail[j].ItemId, bOMHeader.ManufacturingBOMPackingMaterialDetail[j].ItemName, bOMHeader.ManufacturingBOMPackingMaterialDetail[j].Qty, bOMHeader.ManufacturingBOMPackingMaterialDetail[j].Rate, bOMHeader.ManufacturingBOMPackingMaterialDetail[j].Amount, bOMHeader.ManufacturingBOMPackingMaterialDetail[j].pmRemarks);
				}
				DatagridHistoryPackMaterial.DataSource = dthistoryPM;
				DatagridHistoryPackMaterial.RetrieveStructure();
				for (int i = 0; i < bOMHeader.InvManufacturingOverHeadDetails.Count; i++)
				{
					dthistorydetailOH.Rows.Add(bOMHeader.InvManufacturingOverHeadDetails[i].Id, bOMHeader.InvManufacturingOverHeadDetails[i].OverHeadTypeId, bOMHeader.InvManufacturingOverHeadDetails[i].OverHeadAccountTitle, bOMHeader.InvManufacturingOverHeadDetails[i].Qty, bOMHeader.InvManufacturingOverHeadDetails[i].Rate, bOMHeader.InvManufacturingOverHeadDetails[i].ohAmount, bOMHeader.InvManufacturingOverHeadDetails[i].OhRemarks);
				}
				DatagridHistoryOverHead.DataSource = dthistorydetailOH;
				DatagridHistoryOverHead.RetrieveStructure();
				InputDetailGridSetting();
				PMDetailGridSetting();
				OverHeadDetailGridSetting();
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
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Expected O, but got Unknown
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Expected O, but got Unknown
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Expected O, but got Unknown
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Expected O, but got Unknown
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Expected O, but got Unknown
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Expected O, but got Unknown
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Expected O, but got Unknown
		//IL_0584: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Expected O, but got Unknown
		//IL_0608: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Expected O, but got Unknown
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Expected O, but got Unknown
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Expected O, but got Unknown
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_0751: Expected O, but got Unknown
		//IL_0789: Unknown result type (might be due to invalid IL or missing references)
		//IL_0793: Expected O, but got Unknown
		//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ca: Expected O, but got Unknown
		//IL_07f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0801: Expected O, but got Unknown
		//IL_0865: Unknown result type (might be due to invalid IL or missing references)
		//IL_086f: Expected O, but got Unknown
		//IL_08a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b1: Expected O, but got Unknown
		//IL_0920: Unknown result type (might be due to invalid IL or missing references)
		//IL_092a: Expected O, but got Unknown
		//IL_1539: Unknown result type (might be due to invalid IL or missing references)
		//IL_1543: Expected O, but got Unknown
		//IL_1550: Unknown result type (might be due to invalid IL or missing references)
		//IL_155a: Expected O, but got Unknown
		//IL_17df: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e9: Expected O, but got Unknown
		//IL_17f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1800: Expected O, but got Unknown
		//IL_1a79: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a83: Expected O, but got Unknown
		//IL_1a90: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a9a: Expected O, but got Unknown
		//IL_6051: Unknown result type (might be due to invalid IL or missing references)
		//IL_605b: Expected O, but got Unknown
		GridEXLayout grdPriceList_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOverHead_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPackingMaterial_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInputMaterial_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance39 = new Appearance();
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
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production.frmBillOfMaterial_Manufacturing));
		GridEXLayout DatagridHistoryInput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DatagridHistoryPackMaterial_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DatagridHistoryOverHead_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance83 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdPriceList = new GridEX();
		this.panel31 = new System.Windows.Forms.Panel();
		this.label22 = new System.Windows.Forms.Label();
		this.BtnLoadForInput = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grdOverHead = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdPackingMaterial = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdInputMaterial = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.cmbcropYear = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.txtDocno = new System.Windows.Forms.TextBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbCropYearFilter = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.txtPriceFilterDate = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.btnShowForPriceList = new System.Windows.Forms.Button();
		this.CmbItemCategory = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbItemType = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.panel16 = new System.Windows.Forms.Panel();
		this.txtAddOverHead = new System.Windows.Forms.Panel();
		this.txtNetValue = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtOverHead = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtLessByProduct = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.txtTotalInputs = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbtype = new UltraCombo();
		this.txtEffectedDate = new System.Windows.Forms.DateTimePicker();
		this.label13 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.txtRecipeName = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbUOM = new UltraCombo();
		this.chkIsDefault = new System.Windows.Forms.CheckBox();
		this.label18 = new System.Windows.Forms.Label();
		this.ChkIsActive = new System.Windows.Forms.CheckBox();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnrefresh = new System.Windows.Forms.ToolStripButton();
		this.BtnPriceList = new System.Windows.Forms.ToolStripButton();
		this.BtnPriceListSale = new System.Windows.Forms.ToolStripButton();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.PanelHistoryMain = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel27 = new System.Windows.Forms.Panel();
		this.DatagridHistoryInput = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.panel18 = new System.Windows.Forms.Panel();
		this.panel21 = new System.Windows.Forms.Panel();
		this.panel26 = new System.Windows.Forms.Panel();
		this.DatagridHistoryPackMaterial = new GridEX();
		this.panel22 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.panel19 = new System.Windows.Forms.Panel();
		this.panel25 = new System.Windows.Forms.Panel();
		this.DatagridHistoryOverHead = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.PanelHistoryMaingrid = new System.Windows.Forms.Panel();
		this.panel24 = new System.Windows.Forms.Panel();
		this.DatagridHistory = new GridEX();
		this.panel23 = new System.Windows.Forms.Panel();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.groupBox14 = new System.Windows.Forms.GroupBox();
		this.datfromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label164 = new System.Windows.Forms.Label();
		this.txtDocNoFrom = new System.Windows.Forms.TextBox();
		this.CmbItemNameHistory = new UltraCombo();
		this.label49 = new System.Windows.Forms.Label();
		this.label163 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.btnSearchHistory = new System.Windows.Forms.Button();
		this.label52 = new System.Windows.Forms.Label();
		this.CmbRecipeHistory = new UltraCombo();
		this.datToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label50 = new System.Windows.Forms.Label();
		this.txtDocNumberTo = new System.Windows.Forms.TextBox();
		this.panel45 = new System.Windows.Forms.Panel();
		this.label92 = new System.Windows.Forms.Label();
		this.PanelhistoryToolStrip = new System.Windows.Forms.Panel();
		this.toolStripHistory = new System.Windows.Forms.ToolStrip();
		this.btnHistoryRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbCropYearHistory = new UltraCombo();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPriceList).BeginInit();
		this.panel31.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).BeginInit();
		this.panel15.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterial).BeginInit();
		this.panel12.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputMaterial).BeginInit();
		this.panel10.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcropYear).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYearFilter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtPriceFilterDate).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).BeginInit();
		this.panel16.SuspendLayout();
		this.txtAddOverHead.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.PanelHistoryMain.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel27.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistoryInput).BeginInit();
		this.panel17.SuspendLayout();
		this.panel18.SuspendLayout();
		this.panel21.SuspendLayout();
		this.panel26.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistoryPackMaterial).BeginInit();
		this.panel22.SuspendLayout();
		this.panel19.SuspendLayout();
		this.panel25.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistoryOverHead).BeginInit();
		this.panel20.SuspendLayout();
		this.PanelHistoryMaingrid.SuspendLayout();
		this.panel24.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel23.SuspendLayout();
		this.groupBox14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRecipeHistory).BeginInit();
		this.panel45.SuspendLayout();
		this.PanelhistoryToolStrip.SuspendLayout();
		this.toolStripHistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearHistory).BeginInit();
		base.SuspendLayout();
		this.panel1.AutoScroll = true;
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1131, 701);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1131, 701);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1123, 675);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.AutoScroll = true;
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1117, 669);
		this.panel2.TabIndex = 0;
		this.panel5.AutoScroll = true;
		this.panel5.Controls.Add(this.panel11);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 182);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1117, 487);
		this.panel5.TabIndex = 2;
		this.panel11.AutoScroll = true;
		this.panel11.Controls.Add(this.panel13);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel11.Location = new System.Drawing.Point(565, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(465, 487);
		this.panel11.TabIndex = 4;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdPriceList);
		this.panel13.Controls.Add(this.panel31);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(465, 487);
		this.panel13.TabIndex = 0;
		this.grdPriceList.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPriceList).BackColor = System.Drawing.Color.White;
		this.grdPriceList.ColumnAutoResize = true;
		grdPriceList_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPriceList.DesignTimeLayout = grdPriceList_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPriceList).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPriceList).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPriceList.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPriceList).Location = new System.Drawing.Point(0, 32);
		((System.Windows.Forms.Control)(object)this.grdPriceList).Name = "grdPriceList";
		((ScrollableBase)this.grdPriceList).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPriceList).Size = new System.Drawing.Size(465, 455);
		((System.Windows.Forms.Control)(object)this.grdPriceList).TabIndex = 0;
		this.grdPriceList.TotalRow = (InheritableBoolean)1;
		this.grdPriceList.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPriceList).VisualStyle = (VisualStyle)4;
		this.panel31.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel31.Controls.Add(this.label22);
		this.panel31.Controls.Add(this.BtnLoadForInput);
		this.panel31.Controls.Add(this.label4);
		this.panel31.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel31.Location = new System.Drawing.Point(0, 0);
		this.panel31.Name = "panel31";
		this.panel31.Size = new System.Drawing.Size(465, 32);
		this.panel31.TabIndex = 15;
		this.label22.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(235, 2);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(170, 26);
		this.label22.TabIndex = 143;
		this.label22.Text = "Click Load To  Add  Selected Rows In Input Grid";
		this.BtnLoadForInput.BackColor = System.Drawing.Color.FromArgb(192, 192, 0);
		this.BtnLoadForInput.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.BtnLoadForInput.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadForInput.ForeColor = System.Drawing.Color.Black;
		this.BtnLoadForInput.Location = new System.Drawing.Point(415, 2);
		this.BtnLoadForInput.Name = "BtnLoadForInput";
		this.BtnLoadForInput.Size = new System.Drawing.Size(46, 22);
		this.BtnLoadForInput.TabIndex = 0;
		this.BtnLoadForInput.Text = "Load";
		this.BtnLoadForInput.UseVisualStyleBackColor = false;
		this.BtnLoadForInput.Click += new System.EventHandler(BtnLoadForInput_Click);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(12, 5);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(197, 17);
		this.label4.TabIndex = 31;
		this.label4.Text = "Item Price List (Semi-Finished)";
		this.panel7.AutoScroll = true;
		this.panel7.Controls.Add(this.panel14);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Controls.Add(this.panel6);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(565, 487);
		this.panel7.TabIndex = 3;
		this.panel14.AutoScroll = true;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdOverHead);
		this.panel14.Controls.Add(this.panel15);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 422);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(548, 222);
		this.panel14.TabIndex = 4;
		this.grdOverHead.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdOverHead).BackColor = System.Drawing.Color.White;
		this.grdOverHead.ColumnAutoResize = true;
		grdOverHead_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOverHead.DesignTimeLayout = grdOverHead_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOverHead.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdOverHead).Name = "grdOverHead";
		((ScrollableBase)this.grdOverHead).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Size = new System.Drawing.Size(548, 196);
		((System.Windows.Forms.Control)(object)this.grdOverHead).TabIndex = 0;
		this.grdOverHead.TotalRow = (InheritableBoolean)1;
		this.grdOverHead.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOverHead).VisualStyle = (VisualStyle)4;
		this.grdOverHead.CellUpdated += new ColumnActionEventHandler(grdOverHead_CellUpdated_1);
		this.grdOverHead.ColumnButtonClick += new ColumnActionEventHandler(grdOverHead_ColumnButtonClick_1);
		this.panel15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel15.Controls.Add(this.label2);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(548, 26);
		this.panel15.TabIndex = 15;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(6, 4);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(104, 17);
		this.label2.TabIndex = 32;
		this.label2.Text = "Over Head Cost";
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackingMaterial);
		this.panel8.Controls.Add(this.panel12);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 205);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(548, 217);
		this.panel8.TabIndex = 3;
		this.grdPackingMaterial.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdPackingMaterial).BackColor = System.Drawing.Color.White;
		this.grdPackingMaterial.ColumnAutoResize = true;
		grdPackingMaterial_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackingMaterial.DesignTimeLayout = grdPackingMaterial_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackingMaterial.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Name = "grdPackingMaterial";
		((ScrollableBase)this.grdPackingMaterial).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Size = new System.Drawing.Size(548, 191);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).TabIndex = 0;
		this.grdPackingMaterial.TotalRow = (InheritableBoolean)1;
		this.grdPackingMaterial.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingMaterial).VisualStyle = (VisualStyle)4;
		this.grdPackingMaterial.CellUpdated += new ColumnActionEventHandler(grdPackingMaterial_CellUpdated);
		this.grdPackingMaterial.ColumnButtonClick += new ColumnActionEventHandler(grdPackingMaterial_ColumnButtonClick);
		this.panel12.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel12.Controls.Add(this.label3);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(548, 26);
		this.panel12.TabIndex = 14;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(12, 5);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(142, 17);
		this.label3.TabIndex = 32;
		this.label3.Text = "Packing Material Cost";
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputMaterial);
		this.panel6.Controls.Add(this.panel10);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(548, 205);
		this.panel6.TabIndex = 0;
		this.grdInputMaterial.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdInputMaterial).BackColor = System.Drawing.Color.White;
		grdInputMaterial_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInputMaterial.DesignTimeLayout = grdInputMaterial_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInputMaterial.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Location = new System.Drawing.Point(0, 32);
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Name = "grdInputMaterial";
		((ScrollableBase)this.grdInputMaterial).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Size = new System.Drawing.Size(548, 173);
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).TabIndex = 0;
		this.grdInputMaterial.TotalRow = (InheritableBoolean)1;
		this.grdInputMaterial.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputMaterial).VisualStyle = (VisualStyle)4;
		this.grdInputMaterial.CellUpdated += new ColumnActionEventHandler(grdInputMaterial_CellUpdated);
		this.grdInputMaterial.ColumnButtonClick += new ColumnActionEventHandler(grdInputMaterial_ColumnButtonClick);
		this.panel10.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel10.Controls.Add(this.label1);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(548, 32);
		this.panel10.TabIndex = 14;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(12, 5);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(100, 17);
		this.label1.TabIndex = 31;
		this.label1.Text = "Input Material ";
		this.panel4.AutoScroll = true;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.label23);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcropYear);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add(this.txtDocno);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.panel16);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbtype);
		this.panel4.Controls.Add(this.txtEffectedDate);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.panel4.Controls.Add(this.txtRecipeName);
		this.panel4.Controls.Add(this.label44);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUOM);
		this.panel4.Controls.Add(this.chkIsDefault);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add(this.ChkIsActive);
		this.panel4.Controls.Add(this.txtQty);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1117, 152);
		this.panel4.TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(11, 102);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(66, 17);
		this.label23.TabIndex = 140;
		this.label23.Text = "Crop Year";
		this.cmbcropYear.AllowNull = (DefaultableBoolean)1;
		this.cmbcropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance111).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Location = new System.Drawing.Point(100, 97);
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Name = "cmbcropYear";
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Size = new System.Drawing.Size(136, 27);
		((System.Windows.Forms.Control)(object)this.cmbcropYear).TabIndex = 4;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(243, 13);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(53, 17);
		this.label14.TabIndex = 138;
		this.label14.Text = "Doc No";
		this.txtDocno.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.txtDocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocno.Enabled = false;
		this.txtDocno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocno.Location = new System.Drawing.Point(305, 10);
		this.txtDocno.Name = "txtDocno";
		this.txtDocno.Size = new System.Drawing.Size(157, 22);
		this.txtDocno.TabIndex = 137;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCropYearFilter);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.txtPriceFilterDate);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.btnShowForPriceList);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCategory);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemType);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
		this.groupBox1.Location = new System.Drawing.Point(740, 12);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(290, 134);
		this.groupBox1.TabIndex = 9;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filter For Price List";
		this.cmbCropYearFilter.AllowNull = (DefaultableBoolean)1;
		this.cmbCropYearFilter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropYearFilter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropYearFilter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance38;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance72).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropYearFilter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropYearFilter).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCropYearFilter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropYearFilter).Location = new System.Drawing.Point(79, 105);
		((System.Windows.Forms.Control)(object)this.cmbCropYearFilter).Name = "cmbCropYearFilter";
		((System.Windows.Forms.Control)(object)this.cmbCropYearFilter).Size = new System.Drawing.Size(155, 27);
		((System.Windows.Forms.Control)(object)this.cmbCropYearFilter).TabIndex = 3;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(8, 110);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(67, 17);
		this.label20.TabIndex = 143;
		this.label20.Text = "Crop Year";
		this.txtPriceFilterDate.AllowNull = (DefaultableBoolean)1;
		this.txtPriceFilterDate.AutoCompleteMode = (AutoCompleteMode)4;
		this.txtPriceFilterDate.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.txtPriceFilterDate.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance34).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance43).TextHAlignAsString = "Left";
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.txtPriceFilterDate).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.txtPriceFilterDate).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPriceFilterDate.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.txtPriceFilterDate).Location = new System.Drawing.Point(79, 16);
		((System.Windows.Forms.Control)(object)this.txtPriceFilterDate).Name = "txtPriceFilterDate";
		((System.Windows.Forms.Control)(object)this.txtPriceFilterDate).Size = new System.Drawing.Size(205, 27);
		((System.Windows.Forms.Control)(object)this.txtPriceFilterDate).TabIndex = 0;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(8, 23);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(37, 17);
		this.label15.TabIndex = 140;
		this.label15.Text = "Date";
		this.btnShowForPriceList.BackColor = System.Drawing.Color.Teal;
		this.btnShowForPriceList.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnShowForPriceList.Font = new System.Drawing.Font("Segoe UI", 8.75f, System.Drawing.FontStyle.Bold);
		this.btnShowForPriceList.ForeColor = System.Drawing.Color.White;
		this.btnShowForPriceList.Location = new System.Drawing.Point(240, 105);
		this.btnShowForPriceList.Name = "btnShowForPriceList";
		this.btnShowForPriceList.Size = new System.Drawing.Size(46, 27);
		this.btnShowForPriceList.TabIndex = 4;
		this.btnShowForPriceList.Text = "Show";
		this.btnShowForPriceList.UseVisualStyleBackColor = false;
		this.btnShowForPriceList.Click += new System.EventHandler(btnShowForPriceList_Click);
		this.CmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance51;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance57).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Location = new System.Drawing.Point(79, 46);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Name = "CmbItemCategory";
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Size = new System.Drawing.Size(205, 27);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).TabIndex = 1;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(8, 50);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(64, 17);
		this.label21.TabIndex = 139;
		this.label21.Text = "Category";
		this.CmbItemType.AllowNull = (DefaultableBoolean)1;
		this.CmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance79;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance98).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Location = new System.Drawing.Point(79, 76);
		((System.Windows.Forms.Control)(object)this.CmbItemType).Name = "CmbItemType";
		((System.Windows.Forms.Control)(object)this.CmbItemType).Size = new System.Drawing.Size(205, 27);
		((System.Windows.Forms.Control)(object)this.CmbItemType).TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(8, 81);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(69, 17);
		this.label5.TabIndex = 137;
		this.label5.Text = "Item Type";
		this.panel16.Controls.Add(this.txtAddOverHead);
		this.panel16.Location = new System.Drawing.Point(482, 10);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(252, 134);
		this.panel16.TabIndex = 135;
		this.txtAddOverHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddOverHead.Controls.Add(this.txtNetValue);
		this.txtAddOverHead.Controls.Add(this.label10);
		this.txtAddOverHead.Controls.Add(this.txtOverHead);
		this.txtAddOverHead.Controls.Add(this.label9);
		this.txtAddOverHead.Controls.Add(this.txtLessByProduct);
		this.txtAddOverHead.Controls.Add(this.label8);
		this.txtAddOverHead.Controls.Add(this.txtTotalInputs);
		this.txtAddOverHead.Controls.Add(this.label6);
		this.txtAddOverHead.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtAddOverHead.Location = new System.Drawing.Point(0, 0);
		this.txtAddOverHead.Name = "txtAddOverHead";
		this.txtAddOverHead.Size = new System.Drawing.Size(252, 134);
		this.txtAddOverHead.TabIndex = 3;
		this.txtNetValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetValue.Location = new System.Drawing.Point(117, 82);
		this.txtNetValue.Name = "txtNetValue";
		this.txtNetValue.ReadOnly = true;
		this.txtNetValue.Size = new System.Drawing.Size(128, 22);
		this.txtNetValue.TabIndex = 3;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(6, 85);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(68, 17);
		this.label10.TabIndex = 97;
		this.label10.Text = "Net Value";
		this.txtOverHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOverHead.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOverHead.Location = new System.Drawing.Point(117, 28);
		this.txtOverHead.Name = "txtOverHead";
		this.txtOverHead.ReadOnly = true;
		this.txtOverHead.Size = new System.Drawing.Size(128, 22);
		this.txtOverHead.TabIndex = 1;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(6, 31);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(73, 17);
		this.label9.TabIndex = 95;
		this.label9.Text = "Over Head";
		this.txtLessByProduct.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLessByProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLessByProduct.Location = new System.Drawing.Point(117, 53);
		this.txtLessByProduct.Name = "txtLessByProduct";
		this.txtLessByProduct.ReadOnly = true;
		this.txtLessByProduct.Size = new System.Drawing.Size(128, 22);
		this.txtLessByProduct.TabIndex = 2;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(6, 56);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(111, 17);
		this.label8.TabIndex = 93;
		this.label8.Text = "Packing Material";
		this.txtTotalInputs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalInputs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalInputs.Location = new System.Drawing.Point(117, 3);
		this.txtTotalInputs.Name = "txtTotalInputs";
		this.txtTotalInputs.ReadOnly = true;
		this.txtTotalInputs.Size = new System.Drawing.Size(128, 22);
		this.txtTotalInputs.TabIndex = 0;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(6, 6);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(82, 17);
		this.label6.TabIndex = 91;
		this.label6.Text = "Total Inputs";
		this.cmbtype.AllowNull = (DefaultableBoolean)1;
		this.cmbtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance103;
		((SpecialBoxBase)((UltraGridBase)this.cmbtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance108).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbtype).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbtype).Location = new System.Drawing.Point(305, 35);
		((System.Windows.Forms.Control)(object)this.cmbtype).Name = "cmbtype";
		((System.Windows.Forms.Control)(object)this.cmbtype).Size = new System.Drawing.Size(157, 27);
		((System.Windows.Forms.Control)(object)this.cmbtype).TabIndex = 2;
		this.txtEffectedDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtEffectedDate.Location = new System.Drawing.Point(100, 11);
		this.txtEffectedDate.Name = "txtEffectedDate";
		this.txtEffectedDate.Size = new System.Drawing.Size(136, 20);
		this.txtEffectedDate.TabIndex = 0;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(11, 13);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(86, 17);
		this.label13.TabIndex = 133;
		this.label13.Text = "Effected Date";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(11, 71);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(72, 17);
		this.label7.TabIndex = 85;
		this.label7.Text = "Item Name";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(243, 40);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(35, 17);
		this.label12.TabIndex = 131;
		this.label12.Text = "Type";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance132;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(100, 66);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(362, 27);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.txtRecipeName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRecipeName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRecipeName.Location = new System.Drawing.Point(100, 37);
		this.txtRecipeName.Name = "txtRecipeName";
		this.txtRecipeName.Size = new System.Drawing.Size(136, 22);
		this.txtRecipeName.TabIndex = 1;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(239, 102);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(39, 17);
		this.label44.TabIndex = 87;
		this.label44.Text = "UOM";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(11, 40);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(86, 17);
		this.label11.TabIndex = 130;
		this.label11.Text = "Recipe Name";
		this.cmbUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance119;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance125).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance126).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Location = new System.Drawing.Point(305, 97);
		((System.Windows.Forms.Control)(object)this.cmbUOM).Name = "cmbUOM";
		((System.Windows.Forms.Control)(object)this.cmbUOM).Size = new System.Drawing.Size(157, 27);
		((System.Windows.Forms.Control)(object)this.cmbUOM).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Leave += new System.EventHandler(cmbUOM_Leave);
		this.chkIsDefault.AutoSize = true;
		this.chkIsDefault.Checked = true;
		this.chkIsDefault.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkIsDefault.Location = new System.Drawing.Point(394, 129);
		this.chkIsDefault.Name = "chkIsDefault";
		this.chkIsDefault.Size = new System.Drawing.Size(68, 17);
		this.chkIsDefault.TabIndex = 8;
		this.chkIsDefault.Text = "IsDefault";
		this.chkIsDefault.UseVisualStyleBackColor = true;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(11, 129);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(56, 17);
		this.label18.TabIndex = 89;
		this.label18.Text = "Quantity";
		this.ChkIsActive.AutoSize = true;
		this.ChkIsActive.Checked = true;
		this.ChkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkIsActive.Location = new System.Drawing.Point(305, 129);
		this.ChkIsActive.Name = "ChkIsActive";
		this.ChkIsActive.Size = new System.Drawing.Size(64, 17);
		this.ChkIsActive.TabIndex = 7;
		this.ChkIsActive.Text = "IsActive";
		this.ChkIsActive.UseVisualStyleBackColor = true;
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(100, 126);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(136, 22);
		this.txtQty.TabIndex = 6;
		this.txtQty.Leave += new System.EventHandler(txtQty_Leave);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1117, 30);
		this.panel3.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.toolStripButton1, this.btnnew, this.btnsave, this.btnUpdate, this.toolStripSeparator, this.Print, this.btnrefresh, this.BtnPriceList, this.BtnPriceListSale });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1117, 27);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripButton1.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(106, 24);
		this.toolStripButton1.Text = "Recipe Define";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
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
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(79, 24);
		this.Print.Text = "207-&Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnrefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnrefresh.Image = (System.Drawing.Image)resources.GetObject("btnrefresh.Image");
		this.btnrefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnrefresh.Name = "btnrefresh";
		this.btnrefresh.Size = new System.Drawing.Size(50, 24);
		this.btnrefresh.Text = "Refresh";
		this.btnrefresh.Click += new System.EventHandler(btnrefresh_Click);
		this.BtnPriceList.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPriceList.Name = "BtnPriceList";
		this.BtnPriceList.Size = new System.Drawing.Size(129, 24);
		this.BtnPriceList.Text = "Price List (Semi Finish)";
		this.BtnPriceList.Click += new System.EventHandler(BtnPriceList_Click);
		this.BtnPriceListSale.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPriceListSale.Name = "BtnPriceListSale";
		this.BtnPriceListSale.Size = new System.Drawing.Size(90, 24);
		this.BtnPriceListSale.Text = "Price List (Sale)";
		this.BtnPriceListSale.Click += new System.EventHandler(BtnPriceListSale_Click);
		this.tabPage3.Controls.Add(this.PanelHistoryMain);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1123, 675);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.PanelHistoryMain.AutoScroll = true;
		this.PanelHistoryMain.Controls.Add(this.panel9);
		this.PanelHistoryMain.Controls.Add(this.panel18);
		this.PanelHistoryMain.Controls.Add(this.PanelHistoryMaingrid);
		this.PanelHistoryMain.Controls.Add(this.PanelhistoryToolStrip);
		this.PanelHistoryMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelHistoryMain.Location = new System.Drawing.Point(0, 0);
		this.PanelHistoryMain.Name = "PanelHistoryMain";
		this.PanelHistoryMain.Size = new System.Drawing.Size(1123, 675);
		this.PanelHistoryMain.TabIndex = 1;
		this.panel9.Controls.Add(this.panel27);
		this.panel9.Controls.Add(this.panel17);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 306);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1123, 199);
		this.panel9.TabIndex = 53;
		this.panel27.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistoryInput);
		this.panel27.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel27.Location = new System.Drawing.Point(0, 19);
		this.panel27.Name = "panel27";
		this.panel27.Size = new System.Drawing.Size(1123, 180);
		this.panel27.TabIndex = 52;
		this.DatagridHistoryInput.AllowEdit = (InheritableBoolean)2;
		DatagridHistoryInput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistoryInput.DesignTimeLayout = DatagridHistoryInput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryInput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DatagridHistoryInput.DynamicFiltering = true;
		this.DatagridHistoryInput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryInput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistoryInput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryInput).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DatagridHistoryInput).Name = "DatagridHistoryInput";
		((ScrollableBase)this.DatagridHistoryInput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryInput).Size = new System.Drawing.Size(1123, 180);
		((System.Windows.Forms.Control)(object)this.DatagridHistoryInput).TabIndex = 49;
		this.DatagridHistoryInput.TotalRow = (InheritableBoolean)1;
		this.DatagridHistoryInput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistoryInput).VisualStyle = (VisualStyle)4;
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label16);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1123, 19);
		this.panel17.TabIndex = 51;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(3, 1);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(83, 16);
		this.label16.TabIndex = 177;
		this.label16.Text = "Input Detail";
		this.panel18.Controls.Add(this.panel21);
		this.panel18.Controls.Add(this.panel19);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel18.Location = new System.Drawing.Point(0, 505);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1123, 170);
		this.panel18.TabIndex = 52;
		this.panel21.Controls.Add(this.panel26);
		this.panel21.Controls.Add(this.panel22);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel21.Location = new System.Drawing.Point(599, 0);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(524, 170);
		this.panel21.TabIndex = 1;
		this.panel26.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistoryPackMaterial);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel26.Location = new System.Drawing.Point(0, 22);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(524, 148);
		this.panel26.TabIndex = 52;
		this.DatagridHistoryPackMaterial.AllowEdit = (InheritableBoolean)2;
		DatagridHistoryPackMaterial_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistoryPackMaterial.DesignTimeLayout = DatagridHistoryPackMaterial_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryPackMaterial).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DatagridHistoryPackMaterial.DynamicFiltering = true;
		this.DatagridHistoryPackMaterial.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryPackMaterial).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistoryPackMaterial.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryPackMaterial).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DatagridHistoryPackMaterial).Name = "DatagridHistoryPackMaterial";
		((ScrollableBase)this.DatagridHistoryPackMaterial).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryPackMaterial).Size = new System.Drawing.Size(524, 148);
		((System.Windows.Forms.Control)(object)this.DatagridHistoryPackMaterial).TabIndex = 50;
		this.DatagridHistoryPackMaterial.TotalRow = (InheritableBoolean)1;
		this.DatagridHistoryPackMaterial.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistoryPackMaterial).VisualStyle = (VisualStyle)4;
		this.panel22.BackColor = System.Drawing.Color.Teal;
		this.panel22.Controls.Add(this.label19);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 0);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(524, 22);
		this.panel22.TabIndex = 50;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(3, 4);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(154, 16);
		this.label19.TabIndex = 177;
		this.label19.Text = "Packing Material Detail";
		this.panel19.Controls.Add(this.panel25);
		this.panel19.Controls.Add(this.panel20);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(599, 170);
		this.panel19.TabIndex = 0;
		this.panel25.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistoryOverHead);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel25.Location = new System.Drawing.Point(0, 22);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(599, 148);
		this.panel25.TabIndex = 51;
		this.DatagridHistoryOverHead.AllowEdit = (InheritableBoolean)2;
		DatagridHistoryOverHead_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistoryOverHead.DesignTimeLayout = DatagridHistoryOverHead_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryOverHead).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DatagridHistoryOverHead.DynamicFiltering = true;
		this.DatagridHistoryOverHead.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryOverHead).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistoryOverHead.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryOverHead).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DatagridHistoryOverHead).Name = "DatagridHistoryOverHead";
		((ScrollableBase)this.DatagridHistoryOverHead).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryOverHead).Size = new System.Drawing.Size(599, 148);
		((System.Windows.Forms.Control)(object)this.DatagridHistoryOverHead).TabIndex = 48;
		this.DatagridHistoryOverHead.TotalRow = (InheritableBoolean)1;
		this.DatagridHistoryOverHead.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistoryOverHead).VisualStyle = (VisualStyle)4;
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.label17);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(599, 22);
		this.panel20.TabIndex = 50;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(2, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(111, 16);
		this.label17.TabIndex = 177;
		this.label17.Text = "Overhead Detail";
		this.PanelHistoryMaingrid.AutoScroll = true;
		this.PanelHistoryMaingrid.Controls.Add(this.panel24);
		this.PanelHistoryMaingrid.Controls.Add(this.panel23);
		this.PanelHistoryMaingrid.Controls.Add(this.panel45);
		this.PanelHistoryMaingrid.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelHistoryMaingrid.Location = new System.Drawing.Point(0, 29);
		this.PanelHistoryMaingrid.Name = "PanelHistoryMaingrid";
		this.PanelHistoryMaingrid.Size = new System.Drawing.Size(1123, 277);
		this.PanelHistoryMaingrid.TabIndex = 2;
		this.panel24.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel24.Location = new System.Drawing.Point(0, 79);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1123, 198);
		this.panel24.TabIndex = 50;
		this.DatagridHistory.AllowEdit = (InheritableBoolean)2;
		this.DatagridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DatagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistory.DesignTimeLayout = DatagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DatagridHistory.DynamicFiltering = true;
		this.DatagridHistory.FilterMode = (FilterMode)2;
		this.DatagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Name = "DatagridHistory";
		((ScrollableBase)this.DatagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(1123, 198);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 49;
		this.DatagridHistory.TotalRow = (InheritableBoolean)1;
		this.DatagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistory).VisualStyle = (VisualStyle)4;
		this.DatagridHistory.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.DatagridHistory.SelectionChanged += new System.EventHandler(DatagridHistory_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).DoubleClick += new System.EventHandler(DatagridHistory_DoubleClick);
		this.panel23.Controls.Add(this.groupBox14);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(0, 19);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1123, 60);
		this.panel23.TabIndex = 49;
		this.rdapproveddate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.ForeColor = System.Drawing.Color.White;
		this.rdapproveddate.Location = new System.Drawing.Point(1014, 1);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 10;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.ForeColor = System.Drawing.Color.White;
		this.rdmodifydate.Location = new System.Drawing.Point(915, 1);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 9;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.ForeColor = System.Drawing.Color.White;
		this.drdocdate.Location = new System.Drawing.Point(736, 1);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 7;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.ForeColor = System.Drawing.Color.White;
		this.rdentrydate.Location = new System.Drawing.Point(821, 1);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 8;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.groupBox14.Controls.Add(this.label24);
		this.groupBox14.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYearHistory);
		this.groupBox14.Controls.Add(this.datfromDateHistory);
		this.groupBox14.Controls.Add(this.label164);
		this.groupBox14.Controls.Add(this.txtDocNoFrom);
		this.groupBox14.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemNameHistory);
		this.groupBox14.Controls.Add(this.label49);
		this.groupBox14.Controls.Add(this.label163);
		this.groupBox14.Controls.Add(this.label48);
		this.groupBox14.Controls.Add(this.btnSearchHistory);
		this.groupBox14.Controls.Add(this.label52);
		this.groupBox14.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRecipeHistory);
		this.groupBox14.Controls.Add(this.datToDateHistory);
		this.groupBox14.Controls.Add(this.label50);
		this.groupBox14.Controls.Add(this.txtDocNumberTo);
		this.groupBox14.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox14.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox14.Location = new System.Drawing.Point(0, 0);
		this.groupBox14.Name = "groupBox14";
		this.groupBox14.Size = new System.Drawing.Size(1119, 60);
		this.groupBox14.TabIndex = 58;
		this.groupBox14.TabStop = false;
		this.groupBox14.Text = "Filters";
		this.datfromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datfromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.datfromDateHistory.Location = new System.Drawing.Point(6, 32);
		this.datfromDateHistory.Name = "datfromDateHistory";
		this.datfromDateHistory.ShowCheckBox = true;
		this.datfromDateHistory.Size = new System.Drawing.Size(133, 23);
		this.datfromDateHistory.TabIndex = 0;
		this.label164.AutoSize = true;
		this.label164.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label164.Location = new System.Drawing.Point(621, 13);
		this.label164.Name = "label164";
		this.label164.Size = new System.Drawing.Size(67, 15);
		this.label164.TabIndex = 56;
		this.label164.Text = "Item Name";
		this.txtDocNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFrom.Location = new System.Drawing.Point(274, 32);
		this.txtDocNoFrom.Name = "txtDocNoFrom";
		this.txtDocNoFrom.Size = new System.Drawing.Size(79, 23);
		this.txtDocNoFrom.TabIndex = 2;
		this.CmbItemNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Location = new System.Drawing.Point(621, 30);
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Name = "CmbItemNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).TabIndex = 5;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(274, 13);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(79, 15);
		this.label49.TabIndex = 49;
		this.label49.Text = "Doc No From";
		this.label163.AutoSize = true;
		this.label163.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label163.Location = new System.Drawing.Point(436, 13);
		this.label163.Name = "label163";
		this.label163.Size = new System.Drawing.Size(42, 15);
		this.label163.TabIndex = 45;
		this.label163.Text = "Recipe";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(6, 13);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(63, 15);
		this.label48.TabIndex = 47;
		this.label48.Text = "Date From";
		this.btnSearchHistory.BackColor = System.Drawing.Color.Teal;
		this.btnSearchHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSearchHistory.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSearchHistory.ForeColor = System.Drawing.Color.White;
		this.btnSearchHistory.Location = new System.Drawing.Point(986, 29);
		this.btnSearchHistory.Name = "btnSearchHistory";
		this.btnSearchHistory.Size = new System.Drawing.Size(65, 28);
		this.btnSearchHistory.TabIndex = 7;
		this.btnSearchHistory.Text = "Search";
		this.btnSearchHistory.UseVisualStyleBackColor = false;
		this.btnSearchHistory.Click += new System.EventHandler(btnSearchHistory_Click);
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(140, 13);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(48, 15);
		this.label52.TabIndex = 52;
		this.label52.Text = "Date To";
		this.CmbRecipeHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbRecipeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRecipeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRecipeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance86;
		((SpecialBoxBase)((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance91).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRecipeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRecipeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRecipeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRecipeHistory).Location = new System.Drawing.Point(436, 30);
		((System.Windows.Forms.Control)(object)this.CmbRecipeHistory).Name = "CmbRecipeHistory";
		((System.Windows.Forms.Control)(object)this.CmbRecipeHistory).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.CmbRecipeHistory).TabIndex = 4;
		this.datToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.datToDateHistory.Location = new System.Drawing.Point(140, 32);
		this.datToDateHistory.Name = "datToDateHistory";
		this.datToDateHistory.ShowCheckBox = true;
		this.datToDateHistory.Size = new System.Drawing.Size(133, 23);
		this.datToDateHistory.TabIndex = 1;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(355, 13);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(64, 15);
		this.label50.TabIndex = 54;
		this.label50.Text = "Doc No To";
		this.txtDocNumberTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNumberTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNumberTo.Location = new System.Drawing.Point(355, 32);
		this.txtDocNumberTo.Name = "txtDocNumberTo";
		this.txtDocNumberTo.Size = new System.Drawing.Size(79, 23);
		this.txtDocNumberTo.TabIndex = 3;
		this.panel45.BackColor = System.Drawing.Color.Teal;
		this.panel45.Controls.Add(this.rdapproveddate);
		this.panel45.Controls.Add(this.label92);
		this.panel45.Controls.Add(this.rdmodifydate);
		this.panel45.Controls.Add(this.drdocdate);
		this.panel45.Controls.Add(this.rdentrydate);
		this.panel45.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel45.Location = new System.Drawing.Point(0, 0);
		this.panel45.Name = "panel45";
		this.panel45.Size = new System.Drawing.Size(1123, 19);
		this.panel45.TabIndex = 48;
		this.label92.AutoSize = true;
		this.label92.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.label92.ForeColor = System.Drawing.Color.White;
		this.label92.Location = new System.Drawing.Point(3, 1);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(54, 16);
		this.label92.TabIndex = 177;
		this.label92.Text = "History";
		this.PanelhistoryToolStrip.Controls.Add(this.toolStripHistory);
		this.PanelhistoryToolStrip.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelhistoryToolStrip.Location = new System.Drawing.Point(0, 0);
		this.PanelhistoryToolStrip.Name = "PanelhistoryToolStrip";
		this.PanelhistoryToolStrip.Size = new System.Drawing.Size(1123, 29);
		this.PanelhistoryToolStrip.TabIndex = 1;
		this.toolStripHistory.BackColor = System.Drawing.Color.White;
		this.toolStripHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripHistory.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStripHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnHistoryRefresh, this.toolStripButton6 });
		this.toolStripHistory.Location = new System.Drawing.Point(0, 0);
		this.toolStripHistory.Name = "toolStripHistory";
		this.toolStripHistory.Size = new System.Drawing.Size(1123, 27);
		this.toolStripHistory.TabIndex = 4;
		this.toolStripHistory.Text = "toolStrip2";
		this.btnHistoryRefresh.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnHistoryRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnHistoryRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnHistoryRefresh.Name = "btnHistoryRefresh";
		this.btnHistoryRefresh.Size = new System.Drawing.Size(70, 24);
		this.btnHistoryRefresh.Text = "&Refresh";
		this.btnHistoryRefresh.Click += new System.EventHandler(btnHistoryRefresh_Click_1);
		this.toolStripButton6.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton6.Name = "toolStripButton6";
		this.toolStripButton6.Size = new System.Drawing.Size(61, 24);
		this.toolStripButton6.Text = "&Print";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(858, 12);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(57, 15);
		this.label24.TabIndex = 58;
		this.label24.Text = "Crop Year";
		this.CmbCropYearHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYearHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYearHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYearHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance64;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance70).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYearHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).Location = new System.Drawing.Point(858, 30);
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).Name = "CmbCropYearHistory";
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).Size = new System.Drawing.Size(125, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).TabIndex = 6;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1131, 701);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmBillOfMaterial_Manufacturing";
		this.Text = "Recipe Define";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmBillOfMaterial_Manufacturing_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmBillOfMaterial_Manufacturing_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPriceList).EndInit();
		this.panel31.ResumeLayout(false);
		this.panel31.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterial).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInputMaterial).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcropYear).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYearFilter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtPriceFilterDate).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).EndInit();
		this.panel16.ResumeLayout(false);
		this.txtAddOverHead.ResumeLayout(false);
		this.txtAddOverHead.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		this.PanelHistoryMain.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel27.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DatagridHistoryInput).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.panel18.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		this.panel26.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DatagridHistoryPackMaterial).EndInit();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		this.panel19.ResumeLayout(false);
		this.panel25.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DatagridHistoryOverHead).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		this.PanelHistoryMaingrid.ResumeLayout(false);
		this.panel24.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel23.ResumeLayout(false);
		this.groupBox14.ResumeLayout(false);
		this.groupBox14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRecipeHistory).EndInit();
		this.panel45.ResumeLayout(false);
		this.panel45.PerformLayout();
		this.PanelhistoryToolStrip.ResumeLayout(false);
		this.PanelhistoryToolStrip.PerformLayout();
		this.toolStripHistory.ResumeLayout(false);
		this.toolStripHistory.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearHistory).EndInit();
		base.ResumeLayout(false);
	}
}
