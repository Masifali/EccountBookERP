using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Steel.Reports.StockReports;

public class StockReportWithValues_St : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtBindCombo = new DataTable();

	public DateTime FromDate;

	public DateTime ToDate;

	private DataTable dtStockSum = new DataTable();

	public int[] AryColWidth;

	private DataTable dtItemStockSummary = new DataTable();

	private DataTable dtItemandWarehouseStockSummary = new DataTable();

	private DataTable dtWarehouseandItemStockSummary = new DataTable();

	private DataTable dtItemandCropYearStockSummary = new DataTable();

	private DataTable dtItemandCropYearandWarehouseStockSummary = new DataTable();

	private DataTable dtJobLotandItemStockSummary = new DataTable();

	private DataTable dtWarehouseandJoblotandItemStockSummary = new DataTable();

	private DataTable dtItemandPackSizeStockSummary = new DataTable();

	private DataTable dtItemandPackingTypeStockSummary = new DataTable();

	private DataTable dtItemandPackSizeandPackingTypeStockSummary = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	internal GridEX grdfrm;

	private ToolStrip toolStrip1;

	private ToolStripButton btnRefresh;

	private Label label8;

	internal UltraCombo cmbItemType;

	private Label label7;

	internal UltraCombo cmbMainType;

	private Label label6;

	private Label label3;

	private Label label2;

	private Label label1;

	internal UltraCombo cmbWareHouse;

	internal UltraCombo cmbCategory;

	private Button btnshow;

	private DateTimePicker txtDateFrom;

	private DateTimePicker txtDateTo;

	private Label label9;

	internal UltraCombo cmbItem;

	private Label label4;

	internal UltraCombo CmbClassGroup;

	private Label label5;

	internal UltraCombo cmbJobLot;

	private Label label11;

	internal UltraCombo cmbReportType;

	private CtrlGrdBar ctrlGrdBar1;

	private CheckBox ChkItemWiseStock;

	private Panel panel5;

	private Label label23;

	private Label lblStockAcItem;

	internal UltraCombo cmbStockAccounts;

	private Label label13;

	internal UltraCombo cmbPacktype;

	private Label label12;

	internal UltraCombo cmbPackUom;

	private ToolStripButton btnPrint;

	private CheckBox chkSkipZero;

	public StockReportWithValues_St(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmStockReport_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 600);
			dtItemStockSummary.Columns.Add("AccountId");
			dtItemStockSummary.Columns.Add("AccountTitle");
			dtItemStockSummary.Columns.Add("ItemId");
			dtItemStockSummary.Columns.Add("ItemName");
			dtItemStockSummary.Columns.Add("PackUom");
			dtItemStockSummary.Columns.Add("PackingType");
			dtItemStockSummary.Columns.Add("OpQty", typeof(double));
			dtItemStockSummary.Columns.Add("QtyIn", typeof(double));
			dtItemStockSummary.Columns.Add("QtyOut", typeof(double));
			dtItemStockSummary.Columns.Add("BalQty", typeof(double));
			dtItemStockSummary.Columns.Add("OpWeight", typeof(double));
			dtItemStockSummary.Columns.Add("WeightIn", typeof(double));
			dtItemStockSummary.Columns.Add("WeightOut", typeof(double));
			dtItemStockSummary.Columns.Add("BalWeight", typeof(double));
			dtItemStockSummary.Columns.Add("OpAmount", typeof(double));
			dtItemStockSummary.Columns.Add("AmountIn", typeof(double));
			dtItemStockSummary.Columns.Add("AmountOut", typeof(double));
			dtItemStockSummary.Columns.Add("BalAmount", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("AccountId");
			dtItemandWarehouseStockSummary.Columns.Add("AccountTitle");
			dtItemandWarehouseStockSummary.Columns.Add("ItemId");
			dtItemandWarehouseStockSummary.Columns.Add("ItemName");
			dtItemandWarehouseStockSummary.Columns.Add("WareHouseName");
			dtItemandWarehouseStockSummary.Columns.Add("PackUom");
			dtItemandWarehouseStockSummary.Columns.Add("PackingType");
			dtItemandWarehouseStockSummary.Columns.Add("OpQty", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("QtyIn", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("QtyOut", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("BalQty", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("OpWeight", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("WeightIn", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("WeightOut", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("BalWeight", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("OpAmount", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("AmountIn", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("AmountOut", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("BalAmount", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("AccountId");
			dtWarehouseandItemStockSummary.Columns.Add("AccountTitle");
			dtWarehouseandItemStockSummary.Columns.Add("ItemId");
			dtWarehouseandItemStockSummary.Columns.Add("ItemName");
			dtWarehouseandItemStockSummary.Columns.Add("WareHouseName");
			dtWarehouseandItemStockSummary.Columns.Add("PackUom");
			dtWarehouseandItemStockSummary.Columns.Add("PackingType");
			dtWarehouseandItemStockSummary.Columns.Add("OpQty", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("QtyIn", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("QtyOut", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("BalQty", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("OpWeight", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("WeightIn", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("WeightOut", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("BalWeight", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("OpAmount", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("AmountIn", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("AmountOut", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("BalAmount", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("AccountId");
			dtJobLotandItemStockSummary.Columns.Add("AccountTitle");
			dtJobLotandItemStockSummary.Columns.Add("ItemId");
			dtJobLotandItemStockSummary.Columns.Add("ItemName");
			dtJobLotandItemStockSummary.Columns.Add("JobLot");
			dtJobLotandItemStockSummary.Columns.Add("PackUom");
			dtJobLotandItemStockSummary.Columns.Add("PackingType");
			dtJobLotandItemStockSummary.Columns.Add("OpQty", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("QtyIn", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("QtyOut", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("BalQty", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("OpWeight", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("WeightIn", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("WeightOut", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("BalWeight", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("OpAmount", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("AmountIn", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("AmountOut", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("BalAmount", typeof(double));
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("AccountId");
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("AccountTitle");
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("ItemId");
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("ItemName");
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("WareHouseName");
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("JobLot");
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("PackUom");
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("PackingType");
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("OpQty", typeof(double));
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("QtyIn", typeof(double));
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("QtyOut", typeof(double));
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("BalQty", typeof(double));
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("OpWeight", typeof(double));
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("WeightIn", typeof(double));
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("WeightOut", typeof(double));
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("BalWeight", typeof(double));
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("OpAmount", typeof(double));
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("AmountIn", typeof(double));
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("AmountOut", typeof(double));
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("BalAmount", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("AccountId");
			dtItemandPackSizeStockSummary.Columns.Add("AccountTitle");
			dtItemandPackSizeStockSummary.Columns.Add("ItemId");
			dtItemandPackSizeStockSummary.Columns.Add("ItemName");
			dtItemandPackSizeStockSummary.Columns.Add("PackUom");
			dtItemandPackSizeStockSummary.Columns.Add("OpQty", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("QtyIn", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("QtyOut", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("BalQty", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("OpWeight", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("WeightIn", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("WeightOut", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("BalWeight", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("OpAmount", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("AmountIn", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("AmountOut", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("BalAmount", typeof(double));
			dtItemandPackingTypeStockSummary.Columns.Add("AccountId");
			dtItemandPackingTypeStockSummary.Columns.Add("AccountTitle");
			dtItemandPackingTypeStockSummary.Columns.Add("ItemId");
			dtItemandPackingTypeStockSummary.Columns.Add("ItemName");
			dtItemandPackingTypeStockSummary.Columns.Add("PackingType");
			dtItemandPackingTypeStockSummary.Columns.Add("OpQty", typeof(double));
			dtItemandPackingTypeStockSummary.Columns.Add("QtyIn", typeof(double));
			dtItemandPackingTypeStockSummary.Columns.Add("QtyOut", typeof(double));
			dtItemandPackingTypeStockSummary.Columns.Add("BalQty", typeof(double));
			dtItemandPackingTypeStockSummary.Columns.Add("OpWeight", typeof(double));
			dtItemandPackingTypeStockSummary.Columns.Add("WeightIn", typeof(double));
			dtItemandPackingTypeStockSummary.Columns.Add("WeightOut", typeof(double));
			dtItemandPackingTypeStockSummary.Columns.Add("BalWeight", typeof(double));
			dtItemandPackingTypeStockSummary.Columns.Add("OpAmount", typeof(double));
			dtItemandPackingTypeStockSummary.Columns.Add("AmountIn", typeof(double));
			dtItemandPackingTypeStockSummary.Columns.Add("AmountOut", typeof(double));
			dtItemandPackingTypeStockSummary.Columns.Add("BalAmount", typeof(double));
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("AccountId");
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("AccountTitle");
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("ItemId");
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("ItemName");
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("PackUom");
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("PackingType");
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("OpQty", typeof(double));
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("QtyIn", typeof(double));
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("QtyOut", typeof(double));
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("BalQty", typeof(double));
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("OpWeight", typeof(double));
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("WeightIn", typeof(double));
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("WeightOut", typeof(double));
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("BalWeight", typeof(double));
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("OpAmount", typeof(double));
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("AmountIn", typeof(double));
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("AmountOut", typeof(double));
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("BalAmount", typeof(double));
			txtDateFrom.Focus();
			if (!Conversion.CheckDateTimeNull(FromDate) && !Conversion.CheckDateTimeNull(ToDate))
			{
				txtDateFrom.Value = FromDate;
				txtDateTo.Value = ToDate;
			}
			else
			{
				txtDateFrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
			StockComboFill();
			ItemStockAcFill();
			ClassGroupNameFill();
			PackingTypeFill();
			UOMFill();
			ReportTypeFill();
			((UltraGridBase)cmbReportType).Rows[0].Activate();
			if (((Control)(object)cmbReportType).Text == Conversion.ToString((object)"Item Stock Summary"))
			{
				GridBind();
			}
			PrintButtonManage();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StockComboFill()
	{
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		try
		{
			DataTable dtMainType = new DataTable();
			dtMainType.Columns.Add("Id");
			dtMainType.Columns.Add("name");
			DataTable dtItemType = new DataTable();
			dtItemType.Columns.Add("Id");
			dtItemType.Columns.Add("name");
			DataTable dtClassGroup = new DataTable();
			dtClassGroup.Columns.Add("Id");
			dtClassGroup.Columns.Add("name");
			DataTable dtItemCategories = new DataTable();
			dtItemCategories.Columns.Add("Id");
			dtItemCategories.Columns.Add("name");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("name");
			DataTable dtJobLot = new DataTable();
			dtJobLot.Columns.Add("Id");
			dtJobLot.Columns.Add("name");
			DataTable dtwr = new DataTable();
			dtwr.Columns.Add("Id");
			dtwr.Columns.Add("name");
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtBindCombo = StocksReport.StockReportComboByOrganizationAndCompanyId(obj);
			if (dtBindCombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtBindCombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "ParentCategories")
				{
					dtMainType.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					DDL.BindDDLNew(dtMainType, cmbMainType, "Id", "name", "Parent Categories", false);
					((UltraGridBase)cmbMainType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "ItemTypes")
				{
					dtItemType.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					DDL.BindDDLNew(dtItemType, cmbItemType, "Id", "name", "Item Type", false);
					((UltraGridBase)cmbItemType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "ItemCategories")
				{
					dtItemCategories.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					DDL.BindDDLNew(dtItemCategories, cmbCategory, "Id", "name", "Item Categories", false);
					((UltraGridBase)cmbCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "ItemClassGroup")
				{
					dtClassGroup.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					DDL.BindDDLNew(dtClassGroup, CmbClassGroup, "Id", "name", "ItemClassGroup", false);
					((UltraGridBase)CmbClassGroup).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "Items")
				{
					dtItem.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					DDL.BindDDLNew(dtItem, cmbItem, "Id", "name", "Item Name", false);
					((UltraGridBase)cmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "JobLot")
				{
					dtJobLot.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					DDL.BindDDLNew(dtJobLot, cmbJobLot, "Id", "name", "JobLot", false);
					((UltraGridBase)cmbJobLot).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "Warehouse")
				{
					dtwr.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					DDL.BindDDLNew(dtwr, cmbWareHouse, "Id", "name", "WarehouseName", false);
					((UltraGridBase)cmbWareHouse).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ClassGroupNameFill()
	{
		try
		{
			DataTable dt = ItemCategory.GetItemClassGroup();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbClassGroup, "Id", "ClassGroupName", "ClassGroupName", ZeroIndex: false);
				((UltraGridBase)CmbClassGroup).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemStockAcFill()
	{
		try
		{
			DataTable dtStockAcItem = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("4");
			if (dtStockAcItem.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtStockAcItem, cmbStockAccounts, "Id", "AccountTitle", "AccountName", false);
				((UltraGridBase)cmbStockAccounts).DisplayLayout.Bands[0].Columns[1].Width = 275;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UOMFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("PackUom");
			dt.Rows.Add(1, "1KG");
			dt.Rows.Add(5, "5KG");
			dt.Rows.Add(10, "10KG");
			dt.Rows.Add(15, "15KG");
			dt.Rows.Add(20, "20KG");
			dt.Rows.Add(25, "25KG");
			dt.Rows.Add(40, "40KG");
			dt.Rows.Add(50, "50KG");
			dt.Rows.Add(60, "60KG");
			dt.Rows.Add(65, "65KG");
			dt.Rows.Add(80, "80KG");
			dt.Rows.Add(100, "100KG");
			DDL.BindDDL(dt, cmbPackUom, "Id", "PackUom", "PackUom", ZeroIndex: false);
			((UltraGridBase)cmbPackUom).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportTypeFill()
	{
		try
		{
			DataTable dtreporttype = new DataTable();
			dtreporttype.Columns.Add("Id");
			dtreporttype.Columns.Add("name");
			dtreporttype.Rows.Add("ItemStockSummary", "Item Stock Summary");
			dtreporttype.Rows.Add("ItemandWarehouseStockSummary", "Warehouse and Item Stock Summary");
			dtreporttype.Rows.Add("ItemandWarehouseStockSummary", "Item and WareHouse Stock Summary");
			dtreporttype.Rows.Add("JobLotandItemStockSummary", "JobLot and Item");
			dtreporttype.Rows.Add("WarehouseandJoblotandItemStockSummary", "Warehouse, Joblot and Item Stock Summary");
			dtreporttype.Rows.Add("ItemandPackSizeStockSummary", "Item and PackSize Stock Summary");
			dtreporttype.Rows.Add("ItemandPackingTypeStockSummary", "Item and PackingType Stock Summary");
			dtreporttype.Rows.Add("ItemandPackSizeandPackingTypeStockSummary", "Item and PackSize and PackingType Stock Summary");
			DDL.BindDDLNew(dtreporttype, cmbReportType, "Id", "name", "Report Type", false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingTypeFill()
	{
		try
		{
			DataTable dtPack = InvPackingType.Getall();
			if (dtPack.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtPack, cmbPacktype, "Id", "PackTypeDesc", "Description", false);
				((UltraGridBase)cmbPacktype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			GridBind();
			PrintButtonManage();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = Conversion.ToDateTime((object)txtDateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtDateTo.Value);
			obj.Activity = Conversion.ToString(cmbReportType.Value);
			obj.ItemTypeId = Conversion.ToInt(cmbItemType.Value);
			obj.ItemCategoryId = Conversion.ToInt(cmbCategory.Value);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			obj.PackUomFrom = Conversion.ToDouble(cmbPackUom.Value);
			obj.PackingTypeId = Conversion.ToInt(cmbPacktype.Value);
			obj.WarehouseId = Conversion.ToInt(cmbWareHouse.Value);
			obj.JobLotId = Conversion.ToInt(cmbJobLot.Value);
			obj.PurchaseGLAC = Conversion.ToInt(cmbStockAccounts.Value);
			obj.InventoryParentCategories = Conversion.ToInt(cmbMainType.Value);
			obj.ItemClassGroupId = Conversion.ToInt(CmbClassGroup.Value);
			if (ChkItemWiseStock.Checked)
			{
				obj.stockUOM = 0;
				obj.RateUOM = 0;
			}
			else
			{
				obj.stockUOM = 1;
				obj.RateUOM = 1;
			}
			if (chkSkipZero.Checked)
			{
				obj.ZeroBalanceType = 1;
			}
			dtStockSum = StocksReport.StockSummaryWithValuesReport(obj);
			if (dtStockSum.Rows.Count > 0)
			{
				if (Conversion.ToString(cmbReportType.Value) == "ItemStockSummary")
				{
					dtItemStockSummary.Clear();
					for (int i3 = 0; i3 < dtStockSum.Rows.Count; i3++)
					{
						dtItemStockSummary.Rows.Add(dtStockSum.Rows[i3]["AccountId"], dtStockSum.Rows[i3]["AccountTitle"], dtStockSum.Rows[i3]["ItemId"], dtStockSum.Rows[i3]["ItemName"], dtStockSum.Rows[i3]["PackUom"], dtStockSum.Rows[i3]["PackingType"], Conversion.ToDouble(dtStockSum.Rows[i3]["OpQty"]), Conversion.ToDouble(dtStockSum.Rows[i3]["QtyIn"]), Conversion.ToDouble(dtStockSum.Rows[i3]["QtyOut"]), Conversion.ToDouble(dtStockSum.Rows[i3]["BalQty"]), Conversion.ToDouble(dtStockSum.Rows[i3]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[i3]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[i3]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[i3]["BalWeight"]), Conversion.ToDouble(dtStockSum.Rows[i3]["OpAmount"]), Conversion.ToDouble(dtStockSum.Rows[i3]["AmountIn"]), Conversion.ToDouble(dtStockSum.Rows[i3]["AmountOut"]), Conversion.ToDouble(dtStockSum.Rows[i3]["BalAmount"]));
					}
					((Control)(object)grdfrm).Name = "ItemStockSummary";
					grdfrm.DataSource = dtItemStockSummary;
					grdfrm.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandWarehouseStockSummary")
				{
					dtItemandWarehouseStockSummary.Clear();
					for (int i2 = 0; i2 < dtStockSum.Rows.Count; i2++)
					{
						dtItemandWarehouseStockSummary.Rows.Add(dtStockSum.Rows[i2]["AccountId"], dtStockSum.Rows[i2]["AccountTitle"], dtStockSum.Rows[i2]["ItemId"], dtStockSum.Rows[i2]["ItemName"], dtStockSum.Rows[i2]["WareHouseName"], dtStockSum.Rows[i2]["PackUom"], dtStockSum.Rows[i2]["PackingType"], Conversion.ToDouble(dtStockSum.Rows[i2]["OpQty"]), Conversion.ToDouble(dtStockSum.Rows[i2]["QtyIn"]), Conversion.ToDouble(dtStockSum.Rows[i2]["QtyOut"]), Conversion.ToDouble(dtStockSum.Rows[i2]["BalQty"]), Conversion.ToDouble(dtStockSum.Rows[i2]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[i2]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[i2]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[i2]["BalWeight"]), Conversion.ToDouble(dtStockSum.Rows[i2]["OpAmount"]), Conversion.ToDouble(dtStockSum.Rows[i2]["AmountIn"]), Conversion.ToDouble(dtStockSum.Rows[i2]["AmountOut"]), Conversion.ToDouble(dtStockSum.Rows[i2]["BalAmount"]));
					}
					((Control)(object)grdfrm).Name = "ItemandWarehouseStockSummary";
					grdfrm.DataSource = dtItemandWarehouseStockSummary;
					grdfrm.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandWarehouseStockSummary")
				{
					dtWarehouseandItemStockSummary.Clear();
					for (int n = 0; n < dtStockSum.Rows.Count; n++)
					{
						dtWarehouseandItemStockSummary.Rows.Add(dtStockSum.Rows[n]["AccountId"], dtStockSum.Rows[n]["AccountTitle"], dtStockSum.Rows[n]["ItemId"], dtStockSum.Rows[n]["ItemName"], dtStockSum.Rows[n]["WareHouseName"], dtStockSum.Rows[n]["PackUom"], dtStockSum.Rows[n]["PackingType"], Conversion.ToDouble(dtStockSum.Rows[n]["OpQty"]), Conversion.ToDouble(dtStockSum.Rows[n]["QtyIn"]), Conversion.ToDouble(dtStockSum.Rows[n]["QtyOut"]), Conversion.ToDouble(dtStockSum.Rows[n]["BalQty"]), Conversion.ToDouble(dtStockSum.Rows[n]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[n]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[n]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[n]["BalWeight"]), Conversion.ToDouble(dtStockSum.Rows[n]["OpAmount"]), Conversion.ToDouble(dtStockSum.Rows[n]["AmountIn"]), Conversion.ToDouble(dtStockSum.Rows[n]["AmountOut"]), Conversion.ToDouble(dtStockSum.Rows[n]["BalAmount"]));
					}
					((Control)(object)grdfrm).Name = "WarehouseandItemStockSummary";
					grdfrm.DataSource = dtWarehouseandItemStockSummary;
					grdfrm.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "JobLotandItemStockSummary")
				{
					dtJobLotandItemStockSummary.Clear();
					for (int m = 0; m < dtStockSum.Rows.Count; m++)
					{
						dtJobLotandItemStockSummary.Rows.Add(dtStockSum.Rows[m]["AccountId"], dtStockSum.Rows[m]["AccountTitle"], dtStockSum.Rows[m]["ItemId"], dtStockSum.Rows[m]["ItemName"], dtStockSum.Rows[m]["JobLot"], dtStockSum.Rows[m]["PackUom"], dtStockSum.Rows[m]["PackingType"], Conversion.ToDouble(dtStockSum.Rows[m]["OpQty"]), Conversion.ToDouble(dtStockSum.Rows[m]["QtyIn"]), Conversion.ToDouble(dtStockSum.Rows[m]["QtyOut"]), Conversion.ToDouble(dtStockSum.Rows[m]["BalQty"]), Conversion.ToDouble(dtStockSum.Rows[m]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[m]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[m]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[m]["BalWeight"]), Conversion.ToDouble(dtStockSum.Rows[m]["OpAmount"]), Conversion.ToDouble(dtStockSum.Rows[m]["AmountIn"]), Conversion.ToDouble(dtStockSum.Rows[m]["AmountOut"]), Conversion.ToDouble(dtStockSum.Rows[m]["BalAmount"]));
					}
					((Control)(object)grdfrm).Name = "JobLotandItemStockSummary";
					grdfrm.DataSource = dtJobLotandItemStockSummary;
					grdfrm.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "WarehouseandJoblotandItemStockSummary")
				{
					dtWarehouseandJoblotandItemStockSummary.Clear();
					for (int l = 0; l < dtStockSum.Rows.Count; l++)
					{
						dtWarehouseandJoblotandItemStockSummary.Rows.Add(dtStockSum.Rows[l]["AccountId"], dtStockSum.Rows[l]["AccountTitle"], dtStockSum.Rows[l]["ItemId"], dtStockSum.Rows[l]["ItemName"], dtStockSum.Rows[l]["WareHouseName"], dtStockSum.Rows[l]["JobLot"], dtStockSum.Rows[l]["PackUom"], dtStockSum.Rows[l]["PackingType"], Conversion.ToDouble(dtStockSum.Rows[l]["OpQty"]), Conversion.ToDouble(dtStockSum.Rows[l]["QtyIn"]), Conversion.ToDouble(dtStockSum.Rows[l]["QtyOut"]), Conversion.ToDouble(dtStockSum.Rows[l]["BalQty"]), Conversion.ToDouble(dtStockSum.Rows[l]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[l]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[l]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[l]["BalWeight"]), Conversion.ToDouble(dtStockSum.Rows[l]["OpAmount"]), Conversion.ToDouble(dtStockSum.Rows[l]["AmountIn"]), Conversion.ToDouble(dtStockSum.Rows[l]["AmountOut"]), Conversion.ToDouble(dtStockSum.Rows[l]["BalAmount"]));
					}
					((Control)(object)grdfrm).Name = "WarehouseandJoblotandItemStockSummary";
					grdfrm.DataSource = dtWarehouseandJoblotandItemStockSummary;
					grdfrm.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandPackSizeStockSummary")
				{
					dtItemandPackSizeStockSummary.Clear();
					for (int k = 0; k < dtStockSum.Rows.Count; k++)
					{
						dtItemandPackSizeStockSummary.Rows.Add(dtStockSum.Rows[k]["AccountId"], dtStockSum.Rows[k]["AccountTitle"], dtStockSum.Rows[k]["ItemId"], dtStockSum.Rows[k]["ItemName"], dtStockSum.Rows[k]["PackUom"], Conversion.ToDouble(dtStockSum.Rows[k]["OpQty"]), Conversion.ToDouble(dtStockSum.Rows[k]["QtyIn"]), Conversion.ToDouble(dtStockSum.Rows[k]["QtyOut"]), Conversion.ToDouble(dtStockSum.Rows[k]["BalQty"]), Conversion.ToDouble(dtStockSum.Rows[k]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[k]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[k]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[k]["BalWeight"]), Conversion.ToDouble(dtStockSum.Rows[k]["OpAmount"]), Conversion.ToDouble(dtStockSum.Rows[k]["AmountIn"]), Conversion.ToDouble(dtStockSum.Rows[k]["AmountOut"]), Conversion.ToDouble(dtStockSum.Rows[k]["BalAmount"]));
					}
					((Control)(object)grdfrm).Name = "ItemandPackSizeStockSummary";
					grdfrm.DataSource = dtItemandPackSizeStockSummary;
					grdfrm.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandPackingTypeStockSummary")
				{
					dtItemandPackingTypeStockSummary.Clear();
					for (int j = 0; j < dtStockSum.Rows.Count; j++)
					{
						dtItemandPackingTypeStockSummary.Rows.Add(dtStockSum.Rows[j]["AccountId"], dtStockSum.Rows[j]["AccountTitle"], dtStockSum.Rows[j]["ItemId"], dtStockSum.Rows[j]["ItemName"], dtStockSum.Rows[j]["PackingType"], Conversion.ToDouble(dtStockSum.Rows[j]["OpQty"]), Conversion.ToDouble(dtStockSum.Rows[j]["QtyIn"]), Conversion.ToDouble(dtStockSum.Rows[j]["QtyOut"]), Conversion.ToDouble(dtStockSum.Rows[j]["BalQty"]), Conversion.ToDouble(dtStockSum.Rows[j]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[j]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[j]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[j]["BalWeight"]), Conversion.ToDouble(dtStockSum.Rows[j]["OpAmount"]), Conversion.ToDouble(dtStockSum.Rows[j]["AmountIn"]), Conversion.ToDouble(dtStockSum.Rows[j]["AmountOut"]), Conversion.ToDouble(dtStockSum.Rows[j]["BalAmount"]));
					}
					((Control)(object)grdfrm).Name = "ItemandPackingTypeStockSummary";
					grdfrm.DataSource = dtItemandPackingTypeStockSummary;
					grdfrm.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandPackSizeandPackingTypeStockSummary")
				{
					dtItemandPackSizeandPackingTypeStockSummary.Clear();
					for (int i = 0; i < dtStockSum.Rows.Count; i++)
					{
						dtItemandPackSizeandPackingTypeStockSummary.Rows.Add(dtStockSum.Rows[i]["AccountId"], dtStockSum.Rows[i]["AccountTitle"], dtStockSum.Rows[i]["ItemId"], dtStockSum.Rows[i]["ItemName"], dtStockSum.Rows[i]["PackUom"], dtStockSum.Rows[i]["PackingType"], Conversion.ToDouble(dtStockSum.Rows[i]["OpQty"]), Conversion.ToDouble(dtStockSum.Rows[i]["QtyIn"]), Conversion.ToDouble(dtStockSum.Rows[i]["QtyOut"]), Conversion.ToDouble(dtStockSum.Rows[i]["BalQty"]), Conversion.ToDouble(dtStockSum.Rows[i]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[i]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[i]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[i]["BalWeight"]), Conversion.ToDouble(dtStockSum.Rows[i]["OpAmount"]), Conversion.ToDouble(dtStockSum.Rows[i]["AmountIn"]), Conversion.ToDouble(dtStockSum.Rows[i]["AmountOut"]), Conversion.ToDouble(dtStockSum.Rows[i]["BalAmount"]));
					}
					((Control)(object)grdfrm).Name = "ItemandPackSizeandPackingTypeStockSummary";
					grdfrm.DataSource = dtItemandPackSizeandPackingTypeStockSummary;
					grdfrm.RetrieveStructure();
					GridSettings();
				}
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

	private void GridSettings()
	{
		try
		{
			grdfrm.RootTable.Columns["ItemId"].Visible = false;
			grdfrm.RootTable.Columns["AccountId"].Visible = false;
			grdfrm.RootTable.Columns["OpWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["OpWeight"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["OpWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["OpWeight"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["WeightIn"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["WeightIn"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["WeightIn"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["WeightIn"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["WeightOut"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["WeightOut"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["WeightOut"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["WeightOut"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["BalWeight"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["BalWeight"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["OpQty"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["OpQty"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["OpQty"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["OpQty"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["QtyIn"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["QtyIn"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["QtyIn"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["QtyIn"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["QtyOut"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["QtyOut"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["QtyOut"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["QtyOut"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["BalQty"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["OpAmount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["OpAmount"].FormatString = "#,#";
			grdfrm.RootTable.Columns["OpAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["OpAmount"].TotalFormatString = "#,#";
			grdfrm.RootTable.Columns["AmountIn"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["AmountIn"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["AmountIn"].FormatString = "#,#";
			grdfrm.RootTable.Columns["AmountIn"].TotalFormatString = "#,#";
			grdfrm.RootTable.Columns["AmountOut"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["AmountOut"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["AmountOut"].FormatString = "#,#";
			grdfrm.RootTable.Columns["AmountOut"].TotalFormatString = "#,#";
			grdfrm.RootTable.Columns["BalAmount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["BalAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["BalAmount"].FormatString = "#,#";
			grdfrm.RootTable.Columns["BalAmount"].TotalFormatString = "#,#";
			grdfrm.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			((JanusCollectionBase)grdfrm.RootTable.Groups).Clear();
			if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Warehouse and Item Stock Summary")
			{
				grdfrm.RootTable.Groups.Add("WareHouseName");
				grdfrm.RootTable.Columns["WareHouseName"].Visible = false;
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			}
			if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Item and WareHouse Stock Summary")
			{
				grdfrm.RootTable.Groups.Add("ItemName");
				grdfrm.RootTable.Columns["ItemName"].Visible = false;
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			}
			if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Crop Year and Item Stock Summary")
			{
				grdfrm.RootTable.Groups.Add("CropYear");
				grdfrm.RootTable.Columns["CropYear"].Visible = false;
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			}
			if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Crop Year, Warehouse and Item Stock Summary")
			{
				grdfrm.RootTable.Groups.Add("CropYear");
				grdfrm.RootTable.Columns["CropYear"].Visible = false;
				grdfrm.RootTable.Groups.Add("WareHouseName");
				grdfrm.RootTable.Columns["WareHouseName"].Visible = false;
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			}
			if (Conversion.ToString(cmbReportType.Value) == "WarehouseandJoblotandItemStockSummary")
			{
				grdfrm.RootTable.Groups.Add("WareHouseName");
				grdfrm.RootTable.Columns["WareHouseName"].Visible = false;
				grdfrm.RootTable.Groups.Add("JobLot");
				grdfrm.RootTable.Columns["JobLot"].Visible = false;
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			}
			if (Conversion.ToString(cmbReportType.Value) == "JobLotandItemStockSummary")
			{
				grdfrm.RootTable.Groups.Add("JobLot");
				grdfrm.RootTable.Columns["JobLot"].Visible = false;
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			}
			CommonServices.GridAutoAdjustment(AryColWidth, grdfrm);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
			if (e.Column.Key == "ItemName")
			{
				InventoryEvaluationItemLedger obj = new InventoryEvaluationItemLedger(UserAccount);
				obj.CmbItem.Value = ItemId;
				if (((UltraGridBase)CmbClassGroup).ActiveRow != null)
				{
					obj.cmbClassGroup.Value = CmbClassGroup.Value;
				}
				if (((UltraGridBase)cmbMainType).ActiveRow != null)
				{
					obj.cmbparentCategory.Value = cmbMainType.Value;
				}
				if (((UltraGridBase)cmbCategory).ActiveRow != null)
				{
					obj.cmbItemcategory.Value = cmbCategory.Value;
				}
				if (((UltraGridBase)cmbItemType).ActiveRow != null)
				{
					obj.cmbItemType.Value = cmbItemType.Value;
				}
				if (((UltraGridBase)cmbItem).ActiveRow != null)
				{
					obj.CmbItem.Value = cmbItem.Value;
				}
				if (((UltraGridBase)cmbWareHouse).ActiveRow != null)
				{
					obj.CmbWareHouse.Value = cmbWareHouse.Value;
				}
				if (((UltraGridBase)cmbJobLot).ActiveRow != null)
				{
					obj.CmbJobLot.Value = cmbJobLot.Value;
				}
				if (((UltraGridBase)cmbStockAccounts).ActiveRow != null)
				{
					obj.CmbItemStockAc.Value = cmbStockAccounts.Value;
				}
				obj.Show();
			}
			if (e.Column.Key == "AccountTitle")
			{
				Conversion.ToInt(r.Cells["AccountId"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(ItemId, txtDateFrom.Value, txtDateTo.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			txtDateFrom.Value = clsGlobalVariables.ActiveYr.Start_Period;
			((Control)(object)cmbCategory).Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbItemType).Text = string.Empty;
			((Control)(object)cmbJobLot).Text = string.Empty;
			((Control)(object)cmbMainType).Text = string.Empty;
			((Control)(object)cmbReportType).Text = string.Empty;
			((Control)(object)CmbClassGroup).Text = string.Empty;
			((Control)(object)cmbWareHouse).Text = string.Empty;
			txtDateFrom.Focus();
			grdfrm.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
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

	private void PrintButtonManage()
	{
		try
		{
			if (((UltraGridBase)cmbReportType).ActiveRow != null)
			{
				btnPrint.Visible = true;
				if (Conversion.ToString(cmbReportType.Value) == "ItemStockSummary")
				{
					btnPrint.Text = "1550-ItemStockSummary";
				}
				else if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Item and WareHouse Stock Summary")
				{
					btnPrint.Text = "1551-ItemandWarehouseStockSummary";
				}
				else if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Warehouse and Item Stock Summary")
				{
					btnPrint.Text = "1552-WarehouseAndItemStockSummary";
				}
				else if (Conversion.ToString(cmbReportType.Value) == "JobLotandItemStockSummary")
				{
					btnPrint.Text = "1553-JobLotandItemStockSummary";
				}
				else if (Conversion.ToString(cmbReportType.Value) == "WarehouseandJoblotandItemStockSummary")
				{
					btnPrint.Text = "1554-WarehouseandJoblotandItemStockSummary";
				}
				else if (Conversion.ToString(cmbReportType.Value) == "ItemandPackSizeStockSummary")
				{
					btnPrint.Text = "1555-ItemandPackSizeStockSummary";
				}
				else if (Conversion.ToString(cmbReportType.Value) == "ItemandPackingTypeStockSummary")
				{
					btnPrint.Text = "1556-ItemandPackingTypeStockSummary";
				}
				else if (Conversion.ToString(cmbReportType.Value) == "ItemandPackSizeandPackingTypeStockSummary")
				{
					btnPrint.Text = "1557-ItemandPackSizeandPackingTypeStockSummary";
				}
			}
			else
			{
				btnPrint.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (btnPrint.Text == "1550-ItemStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtStockSum, "1550-ItemStockSummary.rpt");
			}
			if (btnPrint.Text == "1551-ItemandWarehouseStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.ShowReportWithDataTable(dtStockSum, "1551-ItemandWarehouseStockSummary.rpt");
			}
			if (btnPrint.Text == "1552-WarehouseAndItemStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val3 = new Reporting();
				val3.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val3.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val3.ShowReportWithDataTable(dtStockSum, "1552-WarehouseAndItemStockSummary.rpt");
			}
			if (btnPrint.Text == "1553-JobLotandItemStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val4 = new Reporting();
				val4.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val4.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val4.ShowReportWithDataTable(dtStockSum, "1553-JobLotandItemStockSummary.rpt");
			}
			if (btnPrint.Text == "1554-WarehouseandJoblotandItemStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val5 = new Reporting();
				val5.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val5.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val5.ShowReportWithDataTable(dtStockSum, "1554-WarehouseandJoblotandItemStockSummary.rpt");
			}
			if (btnPrint.Text == "1555-ItemandPackSizeStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val6 = new Reporting();
				val6.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val6.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val6.ShowReportWithDataTable(dtStockSum, "1555-ItemandPackSizeStockSummary.rpt");
			}
			if (btnPrint.Text == "1556-ItemandPackingTypeStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val7 = new Reporting();
				val7.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val7.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val7.ShowReportWithDataTable(dtStockSum, "1556-ItemandPackingTypeStockSummary.rpt");
			}
			if (btnPrint.Text == "1557-ItemandPackSizeandPackingTypeStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val8 = new Reporting();
				val8.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val8.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val8.ShowReportWithDataTable(dtStockSum, "1557-ItemandPackSizeandPackingTypeStockSummary.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmStockReportWithValues_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StockReportWithValues_St_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Expected O, but got Unknown
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Expected O, but got Unknown
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Expected O, but got Unknown
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Expected O, but got Unknown
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Expected O, but got Unknown
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Expected O, but got Unknown
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Expected O, but got Unknown
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Expected O, but got Unknown
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f8: Expected O, but got Unknown
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0503: Expected O, but got Unknown
		//IL_083e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0848: Expected O, but got Unknown
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
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
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
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
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.chkSkipZero = new System.Windows.Forms.CheckBox();
		this.label13 = new System.Windows.Forms.Label();
		this.cmbPacktype = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbPackUom = new UltraCombo();
		this.lblStockAcItem = new System.Windows.Forms.Label();
		this.cmbStockAccounts = new UltraCombo();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkItemWiseStock = new System.Windows.Forms.CheckBox();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbReportType = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbJobLot = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbClassGroup = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbItemType = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbMainType = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbWareHouse = new UltraCombo();
		this.cmbCategory = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
		this.txtDateTo = new System.Windows.Forms.DateTimePicker();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPacktype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStockAccounts).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbReportType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbClassGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbMainType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCategory).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 661);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 215);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(964, 446);
		this.panel4.TabIndex = 2;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdfrm).BackColor = System.Drawing.Color.White;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(964, 446);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.LinkClicked += new ColumnActionEventHandler(grdfrm_LinkClicked);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.chkSkipZero);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPacktype);
		this.panel3.Controls.Add(this.label12);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackUom);
		this.panel3.Controls.Add(this.lblStockAcItem);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStockAccounts);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.ChkItemWiseStock);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReportType);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLot);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbClassGroup);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemType);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbMainType);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouse);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCategory);
		this.panel3.Controls.Add(this.btnshow);
		this.panel3.Controls.Add(this.txtDateFrom);
		this.panel3.Controls.Add(this.txtDateTo);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(964, 188);
		this.panel3.TabIndex = 0;
		this.chkSkipZero.AutoSize = true;
		this.chkSkipZero.Checked = true;
		this.chkSkipZero.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkSkipZero.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chkSkipZero.Location = new System.Drawing.Point(725, 129);
		this.chkSkipZero.Name = "chkSkipZero";
		this.chkSkipZero.Size = new System.Drawing.Size(76, 19);
		this.chkSkipZero.TabIndex = 116;
		this.chkSkipZero.Text = "Skip Zero";
		this.chkSkipZero.UseVisualStyleBackColor = true;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(265, 72);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(77, 15);
		this.label13.TabIndex = 115;
		this.label13.Text = "Packing Type";
		this.cmbPacktype.AllowNull = (DefaultableBoolean)1;
		this.cmbPacktype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPacktype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPacktype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPacktype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.cmbPacktype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPacktype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPacktype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPacktype).Location = new System.Drawing.Point(345, 66);
		((System.Windows.Forms.Control)(object)this.cmbPacktype).Name = "cmbPacktype";
		((System.Windows.Forms.Control)(object)this.cmbPacktype).Size = new System.Drawing.Size(169, 26);
		((System.Windows.Forms.Control)(object)this.cmbPacktype).TabIndex = 6;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(265, 102);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(61, 15);
		this.label12.TabIndex = 113;
		this.label12.Text = "Pack Uom";
		this.cmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.cmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Location = new System.Drawing.Point(345, 96);
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Name = "cmbPackUom";
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Size = new System.Drawing.Size(169, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackUom).TabIndex = 7;
		this.lblStockAcItem.AutoSize = true;
		this.lblStockAcItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblStockAcItem.Location = new System.Drawing.Point(514, 72);
		this.lblStockAcItem.Name = "lblStockAcItem";
		this.lblStockAcItem.Size = new System.Drawing.Size(85, 15);
		this.lblStockAcItem.TabIndex = 111;
		this.lblStockAcItem.Text = "Stock Account";
		this.cmbStockAccounts.AllowNull = (DefaultableBoolean)1;
		this.cmbStockAccounts.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStockAccounts.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStockAccounts.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Appearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStockAccounts).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance96;
		((SpecialBoxBase)((UltraGridBase)this.cmbStockAccounts).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance102).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStockAccounts).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbStockAccounts.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStockAccounts).Location = new System.Drawing.Point(602, 66);
		((System.Windows.Forms.Control)(object)this.cmbStockAccounts).Name = "cmbStockAccounts";
		((System.Windows.Forms.Control)(object)this.cmbStockAccounts).Size = new System.Drawing.Size(310, 26);
		((System.Windows.Forms.Control)(object)this.cmbStockAccounts).TabIndex = 12;
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label23);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(964, 31);
		this.panel5.TabIndex = 109;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(919, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 23);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(10, 8);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(209, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Stock Report With Value";
		this.ChkItemWiseStock.AutoSize = true;
		this.ChkItemWiseStock.Checked = true;
		this.ChkItemWiseStock.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkItemWiseStock.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.ChkItemWiseStock.Location = new System.Drawing.Point(602, 129);
		this.ChkItemWiseStock.Name = "ChkItemWiseStock";
		this.ChkItemWiseStock.Size = new System.Drawing.Size(113, 19);
		this.ChkItemWiseStock.TabIndex = 14;
		this.ChkItemWiseStock.Text = "Item Wise Stock";
		this.ChkItemWiseStock.UseVisualStyleBackColor = true;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(514, 101);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(70, 15);
		this.label11.TabIndex = 107;
		this.label11.Text = "Report Type";
		this.cmbReportType.AllowNull = (DefaultableBoolean)1;
		this.cmbReportType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReportType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReportType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbReportType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReportType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbReportType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReportType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance46).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReportType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbReportType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReportType).Location = new System.Drawing.Point(602, 95);
		((System.Windows.Forms.Control)(object)this.cmbReportType).Name = "cmbReportType";
		((System.Windows.Forms.Control)(object)this.cmbReportType).Size = new System.Drawing.Size(310, 26);
		((System.Windows.Forms.Control)(object)this.cmbReportType).TabIndex = 13;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(265, 131);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(43, 15);
		this.label5.TabIndex = 103;
		this.label5.Text = "JobLot";
		this.cmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Location = new System.Drawing.Point(345, 125);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Name = "cmbJobLot";
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Size = new System.Drawing.Size(169, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).TabIndex = 9;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(10, 131);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(69, 15);
		this.label4.TabIndex = 101;
		this.label4.Text = "Class Group";
		this.CmbClassGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbClassGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbClassGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbClassGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbClassGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance109;
		((SpecialBoxBase)((UltraGridBase)this.CmbClassGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance115).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance116).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbClassGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).Location = new System.Drawing.Point(105, 125);
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).Name = "CmbClassGroup";
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).Size = new System.Drawing.Size(151, 26);
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).TabIndex = 3;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(514, 42);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 15);
		this.label9.TabIndex = 99;
		this.label9.Text = "Item Name";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance53;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(602, 36);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(310, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 11;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(265, 43);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(63, 15);
		this.label8.TabIndex = 97;
		this.label8.Text = "Item Type ";
		this.cmbItemType.AllowNull = (DefaultableBoolean)1;
		this.cmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
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
		((System.Windows.Forms.Control)(object)this.cmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Location = new System.Drawing.Point(345, 37);
		((System.Windows.Forms.Control)(object)this.cmbItemType).Name = "cmbItemType";
		((System.Windows.Forms.Control)(object)this.cmbItemType).Size = new System.Drawing.Size(169, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemType).TabIndex = 5;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(10, 102);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(91, 15);
		this.label7.TabIndex = 96;
		this.label7.Text = "Parent Category";
		this.cmbMainType.AllowNull = (DefaultableBoolean)1;
		this.cmbMainType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbMainType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbMainType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbMainType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbMainType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbMainType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMainType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance66;
		((SpecialBoxBase)((UltraGridBase)this.cmbMainType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMainType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbMainType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbMainType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance72).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbMainType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbMainType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbMainType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbMainType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbMainType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbMainType).Location = new System.Drawing.Point(105, 96);
		((System.Windows.Forms.Control)(object)this.cmbMainType).Name = "cmbMainType";
		((System.Windows.Forms.Control)(object)this.cmbMainType).Size = new System.Drawing.Size(151, 26);
		((System.Windows.Forms.Control)(object)this.cmbMainType).TabIndex = 2;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(265, 160);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(69, 15);
		this.label6.TabIndex = 95;
		this.label6.Text = "WareHouse";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(10, 160);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(82, 15);
		this.label3.TabIndex = 92;
		this.label3.Text = "Item Category";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(10, 72);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 91;
		this.label2.Text = "Date To";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(10, 43);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 90;
		this.label1.Text = "Date From";
		this.cmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance85).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Location = new System.Drawing.Point(345, 154);
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Name = "cmbWareHouse";
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Size = new System.Drawing.Size(169, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).TabIndex = 10;
		this.cmbCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance89;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance91).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCategory).Location = new System.Drawing.Point(105, 154);
		((System.Windows.Forms.Control)(object)this.cmbCategory).Name = "cmbCategory";
		((System.Windows.Forms.Control)(object)this.cmbCategory).Size = new System.Drawing.Size(151, 26);
		((System.Windows.Forms.Control)(object)this.cmbCategory).TabIndex = 4;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(857, 125);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 26);
		this.btnshow.TabIndex = 15;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.txtDateFrom.CustomFormat = "dd-MMM-yy";
		this.txtDateFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateFrom.Location = new System.Drawing.Point(105, 39);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.Size = new System.Drawing.Size(151, 23);
		this.txtDateFrom.TabIndex = 0;
		this.txtDateTo.CustomFormat = "dd-MMM-yy";
		this.txtDateTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateTo.Location = new System.Drawing.Point(105, 68);
		this.txtDateTo.Name = "txtDateTo";
		this.txtDateTo.Size = new System.Drawing.Size(151, 23);
		this.txtDateTo.TabIndex = 1;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(964, 27);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnRefresh, this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(964, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(56, 24);
		this.btnPrint.Text = "Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "StockReportWithValues_St";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(StockReportWithValues_St_FormClosing);
		base.Load += new System.EventHandler(frmStockReport_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmStockReportWithValues_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPacktype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStockAccounts).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbReportType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbClassGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbMainType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCategory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
