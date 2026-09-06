using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.CmTr.Reports;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.CmTr.Reports;

public class frmStockReportCmtr : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtBindCombo = new DataTable();

	private DataTable dtStockSum = new DataTable();

	private DataTable dtItemStockSummary = new DataTable();

	private DataTable dtItemandWarehouseStockSummary = new DataTable();

	private DataTable dtWarehouseandItemStockSummary = new DataTable();

	private DataTable dtJobLotandItemStockSummary = new DataTable();

	private DataTable dtItemWarehouseAndJoblotStockSummary = new DataTable();

	private DataTable dtItemandPackSizeStockSummary = new DataTable();

	private DataTable dtItemandItemAttributeVarientStockSummary = new DataTable();

	private DataTable dtItemandPackSizeandItemAttributeVarientStockSummary = new DataTable();

	private DataTable dtMainType = new DataTable();

	private DataTable dtItem = new DataTable();

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

	internal UltraCombo cmbMainCategory;

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

	private Label label5;

	internal UltraCombo cmbJobLot;

	private Label label11;

	internal UltraCombo cmbReportType;

	private CtrlGrdBar ctrlGrdBar1;

	private CheckBox ChkItemWiseStock;

	private Panel panel5;

	private Label label23;

	private Label label12;

	internal UltraCombo cmbPacktype;

	private ToolStripButton btnPrint;

	private CheckBox chkSikeZero;

	private CheckBox chkIncludeWipItem;

	private Label label4;

	private ToolStripButton btnShortCutKeys;

	public frmStockReportCmtr(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmStockReportCmtr_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 600);
			dtMainType.Columns.Add("Id", typeof(int));
			dtMainType.Columns.Add("name");
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("name");
			dtItemStockSummary.Columns.Add("ItemId");
			dtItemStockSummary.Columns.Add("ItemName");
			dtItemStockSummary.Columns.Add("TotalQty", typeof(double));
			dtItemStockSummary.Columns.Add("OpWeight", typeof(double));
			dtItemStockSummary.Columns.Add("WeightIn", typeof(double));
			dtItemStockSummary.Columns.Add("WeightOut", typeof(double));
			dtItemStockSummary.Columns.Add("BalWeight", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("ItemId");
			dtItemandWarehouseStockSummary.Columns.Add("ItemName");
			dtItemandWarehouseStockSummary.Columns.Add("WareHouseName");
			dtItemandWarehouseStockSummary.Columns.Add("TotalQty", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("OpWeight", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("WeightIn", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("WeightOut", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("BalWeight", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("ItemId");
			dtWarehouseandItemStockSummary.Columns.Add("ItemName");
			dtWarehouseandItemStockSummary.Columns.Add("WareHouseName");
			dtWarehouseandItemStockSummary.Columns.Add("TotalQty", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("OpWeight", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("WeightIn", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("WeightOut", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("BalWeight", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("ItemId");
			dtJobLotandItemStockSummary.Columns.Add("ItemName");
			dtJobLotandItemStockSummary.Columns.Add("JobLot");
			dtJobLotandItemStockSummary.Columns.Add("TotalQty", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("OpWeight", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("WeightIn", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("WeightOut", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("BalWeight", typeof(double));
			dtItemWarehouseAndJoblotStockSummary.Columns.Add("ItemId");
			dtItemWarehouseAndJoblotStockSummary.Columns.Add("ItemName");
			dtItemWarehouseAndJoblotStockSummary.Columns.Add("WareHouseName");
			dtItemWarehouseAndJoblotStockSummary.Columns.Add("JobLot");
			dtItemWarehouseAndJoblotStockSummary.Columns.Add("TotalQty", typeof(double));
			dtItemWarehouseAndJoblotStockSummary.Columns.Add("OpWeight", typeof(double));
			dtItemWarehouseAndJoblotStockSummary.Columns.Add("WeightIn", typeof(double));
			dtItemWarehouseAndJoblotStockSummary.Columns.Add("WeightOut", typeof(double));
			dtItemWarehouseAndJoblotStockSummary.Columns.Add("BalWeight", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("ItemId");
			dtItemandPackSizeStockSummary.Columns.Add("ItemName");
			dtItemandPackSizeStockSummary.Columns.Add("PackUom");
			dtItemandPackSizeStockSummary.Columns.Add("TotalQty", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("OpWeight", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("WeightIn", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("WeightOut", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("BalWeight", typeof(double));
			dtItemandItemAttributeVarientStockSummary.Columns.Add("ItemId");
			dtItemandItemAttributeVarientStockSummary.Columns.Add("ItemName");
			dtItemandItemAttributeVarientStockSummary.Columns.Add("ItemAttributeVarient");
			dtItemandItemAttributeVarientStockSummary.Columns.Add("TotalQty", typeof(double));
			dtItemandItemAttributeVarientStockSummary.Columns.Add("OpWeight", typeof(double));
			dtItemandItemAttributeVarientStockSummary.Columns.Add("WeightIn", typeof(double));
			dtItemandItemAttributeVarientStockSummary.Columns.Add("WeightOut", typeof(double));
			dtItemandItemAttributeVarientStockSummary.Columns.Add("BalWeight", typeof(double));
			dtItemandPackSizeandItemAttributeVarientStockSummary.Columns.Add("ItemId");
			dtItemandPackSizeandItemAttributeVarientStockSummary.Columns.Add("ItemName");
			dtItemandPackSizeandItemAttributeVarientStockSummary.Columns.Add("PackUom");
			dtItemandPackSizeandItemAttributeVarientStockSummary.Columns.Add("ItemAttributeVarient");
			dtItemandPackSizeandItemAttributeVarientStockSummary.Columns.Add("TotalQty", typeof(double));
			dtItemandPackSizeandItemAttributeVarientStockSummary.Columns.Add("OpWeight", typeof(double));
			dtItemandPackSizeandItemAttributeVarientStockSummary.Columns.Add("WeightIn", typeof(double));
			dtItemandPackSizeandItemAttributeVarientStockSummary.Columns.Add("WeightOut", typeof(double));
			dtItemandPackSizeandItemAttributeVarientStockSummary.Columns.Add("BalWeight", typeof(double));
			txtDateFrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			txtDateFrom.Focus();
			WareHouseFill();
			StockComboFill();
			ReportTypeFill();
			PackingTypeFill();
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
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		try
		{
			DataTable dtItemType = new DataTable();
			dtItemType.Columns.Add("Id");
			dtItemType.Columns.Add("name");
			DataTable dtItemCategories = new DataTable();
			dtItemCategories.Columns.Add("Id");
			dtItemCategories.Columns.Add("name");
			DataTable dtJobLot = new DataTable();
			dtJobLot.Columns.Add("Id");
			dtJobLot.Columns.Add("name");
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtBindCombo = StocksReport.StockReportComboByOrganizationAndCompanyId(obj);
			if (dtBindCombo.Rows.Count > 0)
			{
				for (int i = 0; i < dtBindCombo.Rows.Count; i++)
				{
					if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "ParentCategories")
					{
						dtMainType.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					}
					if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "ItemTypes")
					{
						dtItemType.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					}
					if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "ItemCategories")
					{
						dtItemCategories.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					}
					if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "Items")
					{
						dtItem.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					}
					if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "JobLot")
					{
						dtJobLot.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					}
				}
			}
			DDL.BindDDL(dtMainType, cmbMainCategory, "Id", "name", "Parent Category", ZeroIndex: false);
			((UltraGridBase)cmbMainCategory).DisplayLayout.Bands[0].Columns[1].Width = 161;
			UltraGridColumn obj2 = ((UltraGridBase)cmbMainCategory).DisplayLayout.Bands[0].Columns.Add();
			((KeyedSubObjectBase)obj2).Key = "Selected";
			((HeaderBase)obj2.Header).Caption = string.Empty;
			obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
			obj2.DataType = typeof(bool);
			((HeaderBase)obj2.Header).VisiblePosition = 0;
			cmbMainCategory.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)cmbMainCategory.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)cmbMainCategory.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)cmbMainCategory.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			DDL.BindDDL(dtItemType, cmbItemType, "Id", "name", "Item Type", ZeroIndex: false);
			((UltraGridBase)cmbItemType).DisplayLayout.Bands[0].Columns[1].Width = 161;
			DDL.BindDDL(dtItemCategories, cmbCategory, "Id", "name", "Item Categories", ZeroIndex: false);
			((UltraGridBase)cmbCategory).DisplayLayout.Bands[0].Columns[1].Width = 275;
			DDL.BindDDL(dtItem, cmbItem, "Id", "name", "Item Name", ZeroIndex: false);
			((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 325;
			DDL.BindDDL(dtJobLot, cmbJobLot, "Id", "name", "JobLot", ZeroIndex: false);
			((UltraGridBase)cmbJobLot).DisplayLayout.Bands[0].Columns[1].Width = 127;
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

	private void WareHouseFill()
	{
		try
		{
			DataTable dtwarehouse = CommonServices.WareHouseGetAllService();
			if (dtwarehouse.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtwarehouse, cmbWareHouse, "Id", "WareHouseName", "WareHouse", false);
				((UltraGridBase)cmbWareHouse).DisplayLayout.Bands[0].Columns[1].Width = 275;
				((UltraGridBase)cmbWareHouse).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
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
			dtreporttype.Rows.Add("ItemandWarehouseStockSummary", "Item and WareHouse Stock Summary");
			dtreporttype.Rows.Add("ItemandWarehouseStockSummary", "Warehouse and Item Stock Summary");
			dtreporttype.Rows.Add("JobLotandItemStockSummary", "JobLot and Item");
			dtreporttype.Rows.Add("ItemWarehouseAndJoblotStockSummary", "Item Warehouse And Joblot Stock Summary");
			dtreporttype.Rows.Add("ItemandPackSizeStockSummary", "Item and PackSize Stock Summary");
			dtreporttype.Rows.Add("ItemandItemAttributeVarientStockSummary", "Item and ItemAttributeVarient Stock Summary");
			dtreporttype.Rows.Add("ItemandPackSizeandItemAttributeVarientStockSummary", "Item and PackSize and ItemAttributeVarient Stock Summary");
			DDL.BindDDLNew(dtreporttype, cmbReportType, "Id", "name", "Report Type", false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItem_Leave(object sender, EventArgs e)
	{
	}

	private void cmbMainCategory_Leave(object sender, EventArgs e)
	{
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		try
		{
			if (Conversion.ToString((object)((Control)(object)cmbMainCategory).Text) != string.Empty)
			{
				string Ids = "";
				if (((Control)(object)cmbMainCategory).Text != null && ((Control)(object)cmbMainCategory).Text != string.Empty)
				{
					string[] array = (Conversion.ToString((object)((Control)(object)cmbMainCategory).Text.Trim()) + ",").Split(',');
					foreach (string author in array)
					{
						DataRow[] dr = dtMainType.Select("name='" + Conversion.ToString((object)author) + "'");
						if (dr.Length != 0)
						{
							Ids = Ids + Conversion.ToString(dr[0].ItemArray[0]) + ",";
						}
					}
				}
				DataTable item = new DataTable();
				item.Columns.Add("Id", typeof(int));
				item.Columns.Add("name", typeof(string));
				DataTable dt = StocksReport.StockReportComboByOrganizationAndCompanyId(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Activity = "Items",
					Ids = Ids
				});
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					item.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
				}
				DDL.BindDDL(item, cmbItem, "Id", "name", "Item Name", ZeroIndex: false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 325;
			}
			else
			{
				DDL.BindDDL(dtItem, cmbItem, "Id", "name", "Item Name", ZeroIndex: false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 325;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridBind()
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		try
		{
			string Ids = "";
			if (((Control)(object)cmbMainCategory).Text != null && ((Control)(object)cmbMainCategory).Text != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbMainCategory).Text.Trim()) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtMainType.Select("name='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						Ids = Ids + Conversion.ToString(dr[0].ItemArray[0]) + ",";
					}
				}
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = Conversion.ToDateTime((object)txtDateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtDateTo.Value);
			obj.Ids = Ids;
			obj.ItemCategoryId = Conversion.ToInt(cmbCategory.Value);
			obj.ItemTypeId = Conversion.ToInt(cmbItemType.Value);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			obj.WarehouseId = Conversion.ToInt(cmbWareHouse.Value);
			obj.PackingTypeId = Conversion.ToInt(cmbPacktype.Value);
			obj.JobLotId = Conversion.ToInt(cmbJobLot.Value);
			obj.Activity = Conversion.ToString(cmbReportType.Value);
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
			if (chkSikeZero.Checked)
			{
				obj.ZeroBalanceType = 1;
			}
			dtStockSum = InventoryStockReport.StockReportWithOutValues(obj);
			if (dtStockSum.Rows.Count > 0)
			{
				if (Conversion.ToString(cmbReportType.Value) == "ItemStockSummary")
				{
					dtItemStockSummary.Clear();
					for (int i3 = 0; i3 < dtStockSum.Rows.Count; i3++)
					{
						dtItemStockSummary.Rows.Add(dtStockSum.Rows[i3]["ItemId"], dtStockSum.Rows[i3]["ItemName"], Conversion.ToDouble(dtStockSum.Rows[i3]["TotalQty"]), Conversion.ToDouble(dtStockSum.Rows[i3]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[i3]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[i3]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[i3]["BalWeight"]));
					}
					grdfrm.DataSource = dtItemStockSummary;
					grdfrm.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandWarehouseStockSummary")
				{
					dtItemandWarehouseStockSummary.Clear();
					for (int i2 = 0; i2 < dtStockSum.Rows.Count; i2++)
					{
						dtItemandWarehouseStockSummary.Rows.Add(dtStockSum.Rows[i2]["ItemId"], dtStockSum.Rows[i2]["ItemName"], dtStockSum.Rows[i2]["WareHouseName"], Conversion.ToDouble(dtStockSum.Rows[i2]["TotalQty"]), Conversion.ToDouble(dtStockSum.Rows[i2]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[i2]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[i2]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[i2]["BalWeight"]));
					}
					grdfrm.DataSource = dtItemandWarehouseStockSummary;
					grdfrm.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandWarehouseStockSummary")
				{
					dtWarehouseandItemStockSummary.Clear();
					for (int n = 0; n < dtStockSum.Rows.Count; n++)
					{
						dtWarehouseandItemStockSummary.Rows.Add(dtStockSum.Rows[n]["ItemId"], dtStockSum.Rows[n]["ItemName"], dtStockSum.Rows[n]["WareHouseName"], Conversion.ToDouble(dtStockSum.Rows[n]["TotalQty"]), Conversion.ToDouble(dtStockSum.Rows[n]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[n]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[n]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[n]["BalWeight"]));
					}
					grdfrm.DataSource = dtWarehouseandItemStockSummary;
					grdfrm.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "JobLotandItemStockSummary")
				{
					dtJobLotandItemStockSummary.Clear();
					for (int m = 0; m < dtStockSum.Rows.Count; m++)
					{
						dtJobLotandItemStockSummary.Rows.Add(dtStockSum.Rows[m]["ItemId"], dtStockSum.Rows[m]["ItemName"], dtStockSum.Rows[m]["JobLot"], Conversion.ToDouble(dtStockSum.Rows[m]["TotalQty"]), Conversion.ToDouble(dtStockSum.Rows[m]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[m]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[m]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[m]["BalWeight"]));
					}
					grdfrm.DataSource = dtJobLotandItemStockSummary;
					grdfrm.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemWarehouseAndJoblotStockSummary")
				{
					dtItemWarehouseAndJoblotStockSummary.Clear();
					for (int l = 0; l < dtStockSum.Rows.Count; l++)
					{
						dtItemWarehouseAndJoblotStockSummary.Rows.Add(dtStockSum.Rows[l]["ItemId"], dtStockSum.Rows[l]["ItemName"], dtStockSum.Rows[l]["WareHouseName"], dtStockSum.Rows[l]["JobLot"], Conversion.ToDouble(dtStockSum.Rows[l]["TotalQty"]), Conversion.ToDouble(dtStockSum.Rows[l]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[l]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[l]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[l]["BalWeight"]));
					}
					grdfrm.DataSource = dtItemWarehouseAndJoblotStockSummary;
					grdfrm.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandPackSizeStockSummary")
				{
					dtItemandPackSizeStockSummary.Clear();
					for (int k = 0; k < dtStockSum.Rows.Count; k++)
					{
						dtItemandPackSizeStockSummary.Rows.Add(dtStockSum.Rows[k]["ItemId"], dtStockSum.Rows[k]["ItemName"], dtStockSum.Rows[k]["PackUom"], Conversion.ToDouble(dtStockSum.Rows[k]["TotalQty"]), Conversion.ToDouble(dtStockSum.Rows[k]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[k]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[k]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[k]["BalWeight"]));
					}
					grdfrm.DataSource = dtItemandPackSizeStockSummary;
					grdfrm.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandItemAttributeVarientStockSummary")
				{
					dtItemandItemAttributeVarientStockSummary.Clear();
					for (int j = 0; j < dtStockSum.Rows.Count; j++)
					{
						dtItemandItemAttributeVarientStockSummary.Rows.Add(dtStockSum.Rows[j]["ItemId"], dtStockSum.Rows[j]["ItemName"], dtStockSum.Rows[j]["ItemAttributeVarient"], Conversion.ToDouble(dtStockSum.Rows[j]["TotalQty"]), Conversion.ToDouble(dtStockSum.Rows[j]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[j]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[j]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[j]["BalWeight"]));
					}
					grdfrm.DataSource = dtItemandItemAttributeVarientStockSummary;
					grdfrm.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandPackSizeandItemAttributeVarientStockSummary")
				{
					dtItemandPackSizeandItemAttributeVarientStockSummary.Clear();
					for (int i = 0; i < dtStockSum.Rows.Count; i++)
					{
						dtItemandPackSizeandItemAttributeVarientStockSummary.Rows.Add(dtStockSum.Rows[i]["ItemId"], dtStockSum.Rows[i]["ItemName"], dtStockSum.Rows[i]["PackUom"], dtStockSum.Rows[i]["ItemAttributeVarient"], Conversion.ToDouble(dtStockSum.Rows[i]["TotalQty"]), Conversion.ToDouble(dtStockSum.Rows[i]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[i]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[i]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[i]["BalWeight"]));
					}
					grdfrm.DataSource = dtItemandPackSizeandItemAttributeVarientStockSummary;
					grdfrm.RetrieveStructure();
					GridSettings();
				}
				PrintButtonManage();
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
			grdfrm.RootTable.Columns["OpWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["OpWeight"].FormatString = "#,#;(#,#);00";
			grdfrm.RootTable.Columns["OpWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["OpWeight"].TotalFormatString = "#,#;(#,#);0";
			grdfrm.RootTable.Columns["WeightIn"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["WeightIn"].FormatString = "#,#;(#,#);00";
			grdfrm.RootTable.Columns["WeightIn"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["WeightIn"].TotalFormatString = "#,#;(#,#);0";
			grdfrm.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["TotalQty"].FormatString = "#,#;(#,#);00";
			grdfrm.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["TotalQty"].TotalFormatString = "#,#;(#,#);0";
			grdfrm.RootTable.Columns["WeightOut"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["WeightOut"].FormatString = "#,#;(#,#);00";
			grdfrm.RootTable.Columns["WeightOut"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["WeightOut"].TotalFormatString = "#,#;(#,#);0";
			grdfrm.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["BalWeight"].FormatString = "#,#;(#,#);00";
			grdfrm.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["BalWeight"].TotalFormatString = "#,#;(#,#);0";
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
			if (Conversion.ToString(cmbReportType.Value) == "ItemWarehouseAndJoblotStockSummary")
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
			CommonServices.GridAutoAdjustmentNew(grdfrm);
			ctrlGrdBar1_Load(null, null);
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

	private void grdfrm_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			InventoryTransactionReportCmtr obj = new InventoryTransactionReportCmtr(UserAccount);
			obj.CmbItem.Value = Conversion.ToInt(r.Cells["ItemId"].Value);
			if (((UltraGridBase)cmbWareHouse).ActiveRow != null)
			{
				obj.CmbWareHouse.Value = Conversion.ToInt(cmbWareHouse.Value);
			}
			if (((UltraGridBase)cmbJobLot).ActiveRow != null)
			{
				obj.CmbJobLot.Value = Conversion.ToInt(cmbJobLot.Value);
			}
			if (((UltraGridBase)cmbItem).ActiveRow != null)
			{
				obj.CmbItem.Value = Conversion.ToInt(cmbItem.Value);
			}
			obj.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			txtDateFrom.Value = clsGlobalVariables.ActiveYr.Start_Period;
			((Control)(object)cmbCategory).Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbItemType).Text = string.Empty;
			((Control)(object)cmbJobLot).Text = string.Empty;
			((Control)(object)cmbMainCategory).Text = string.Empty;
			((Control)(object)cmbReportType).Text = string.Empty;
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

	private void frmStockReportCmtr_FormClosing(object sender, FormClosingEventArgs e)
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

	private void PrintButtonManage()
	{
		try
		{
			if (((UltraGridBase)cmbReportType).ActiveRow != null)
			{
				if (Conversion.ToString(cmbReportType.Value) == "ItemStockSummary")
				{
					btnPrint.Text = "480_01_ItemStockSummary";
				}
				if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Item and WareHouse Stock Summary")
				{
					btnPrint.Text = "480_02_ItemandWarehouseStockSummary";
				}
				if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Warehouse and Item Stock Summary")
				{
					btnPrint.Text = "480_03_WarehouseAndItemStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "JobLotandItemStockSummary")
				{
					btnPrint.Text = "480_06_JobLotandItemStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemWarehouseAndJoblotStockSummary")
				{
					btnPrint.Text = "480_07_ItemWarehouseAndJoblotStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandPackSizeStockSummary")
				{
					btnPrint.Text = "480_08_ItemandPackSizeStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandItemAttributeVarientStockSummary")
				{
					btnPrint.Text = "480_09_ItemandItemAttributeVarientStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandPackSizeandItemAttributeVarientStockSummary")
				{
					btnPrint.Text = "480_10_ItemandPackSizeandItemAttributeVarientStockSummary";
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
			if (btnPrint.Text == "480_01_ItemStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtStockSum, "480_01_ItemStockSummary.rpt");
			}
			if (btnPrint.Text == "480_02_ItemandWarehouseStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.ShowReportWithDataTable(dtStockSum, "480_02_ItemandWarehouseStockSummary.rpt");
			}
			if (btnPrint.Text == "480_03_WarehouseAndItemStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val3 = new Reporting();
				val3.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val3.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val3.ShowReportWithDataTable(dtStockSum, "480_03_WarehouseAndItemStockSummary.rpt");
			}
			if (btnPrint.Text == "480_06_JobLotandItemStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val4 = new Reporting();
				val4.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val4.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val4.ShowReportWithDataTable(dtStockSum, "480_06_JobLotandItemStockSummary.rpt");
			}
			if (btnPrint.Text == "480_07_ItemWarehouseAndJoblotStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val5 = new Reporting();
				val5.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val5.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val5.ShowReportWithDataTable(dtStockSum, "480_07_ItemWarehouseAndJoblotStockSummary.rpt");
			}
			if (btnPrint.Text == "480_08_ItemandPackSizeStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val6 = new Reporting();
				val6.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val6.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val6.ShowReportWithDataTable(dtStockSum, "480_08_ItemandPackSizeStockSummary.rpt");
			}
			if (btnPrint.Text == "480_09_ItemandItemAttributeVarientStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val7 = new Reporting();
				val7.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val7.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val7.ShowReportWithDataTable(dtStockSum, "480_09_ItemandItemAttributeVarientStockSummary.rpt");
			}
			if (btnPrint.Text == "480_10_ItemandPackSizeandItemAttributeVarientStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val8 = new Reporting();
				val8.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val8.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val8.ShowReportWithDataTable(dtStockSum, "480_10_ItemandPackSizeandItemAttributeVarientStockSummary.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmStockReportCmtr_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdfrm).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				txtDateFrom.Focus();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKeys_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeys();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Show Record");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on from Date");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid History");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On from Date");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
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
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Expected O, but got Unknown
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Expected O, but got Unknown
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Expected O, but got Unknown
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Expected O, but got Unknown
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Expected O, but got Unknown
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Expected O, but got Unknown
		//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Expected O, but got Unknown
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel3 = new System.Windows.Forms.Panel();
		this.chkIncludeWipItem = new System.Windows.Forms.CheckBox();
		this.chkSikeZero = new System.Windows.Forms.CheckBox();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbPacktype = new UltraCombo();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkItemWiseStock = new System.Windows.Forms.CheckBox();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbReportType = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbJobLot = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbItemType = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbMainCategory = new UltraCombo();
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
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPacktype).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbReportType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbMainCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCategory).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(894, 461);
		this.panel1.TabIndex = 0;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdfrm).BackColor = System.Drawing.Color.White;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 202);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(894, 259);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 1;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.LinkClicked += new ColumnActionEventHandler(grdfrm_LinkClicked);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 179);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(894, 23);
		this.panel4.TabIndex = 2;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(4, 2);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(142, 19);
		this.label4.TabIndex = 1;
		this.label4.Text = "Filtered Records";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(849, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(43, 24);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.chkIncludeWipItem);
		this.panel3.Controls.Add(this.chkSikeZero);
		this.panel3.Controls.Add(this.label12);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPacktype);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.ChkItemWiseStock);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReportType);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLot);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemType);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbMainCategory);
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
		this.panel3.Location = new System.Drawing.Point(0, 29);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(894, 150);
		this.panel3.TabIndex = 0;
		this.chkIncludeWipItem.AutoSize = true;
		this.chkIncludeWipItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkIncludeWipItem.Location = new System.Drawing.Point(889, 42);
		this.chkIncludeWipItem.Name = "chkIncludeWipItem";
		this.chkIncludeWipItem.Size = new System.Drawing.Size(120, 19);
		this.chkIncludeWipItem.TabIndex = 12;
		this.chkIncludeWipItem.Text = "Include WIP Item";
		this.chkIncludeWipItem.UseVisualStyleBackColor = true;
		this.chkIncludeWipItem.Visible = false;
		this.chkSikeZero.AutoSize = true;
		this.chkSikeZero.Checked = true;
		this.chkSikeZero.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkSikeZero.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkSikeZero.Location = new System.Drawing.Point(743, 93);
		this.chkSikeZero.Name = "chkSikeZero";
		this.chkSikeZero.Size = new System.Drawing.Size(76, 19);
		this.chkSikeZero.TabIndex = 11;
		this.chkSikeZero.Text = "Skip Zero";
		this.chkSikeZero.UseVisualStyleBackColor = true;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(265, 66);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(77, 15);
		this.label12.TabIndex = 111;
		this.label12.Text = "Packing Type";
		this.cmbPacktype.AllowNull = (DefaultableBoolean)1;
		this.cmbPacktype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPacktype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPacktype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPacktype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance87;
		((SpecialBoxBase)((UltraGridBase)this.cmbPacktype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance92).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPacktype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPacktype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPacktype).Location = new System.Drawing.Point(343, 60);
		((System.Windows.Forms.Control)(object)this.cmbPacktype).Name = "cmbPacktype";
		((System.Windows.Forms.Control)(object)this.cmbPacktype).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.cmbPacktype).TabIndex = 5;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label23);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(894, 25);
		this.panel5.TabIndex = 109;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(115, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Stock Report";
		this.ChkItemWiseStock.AutoSize = true;
		this.ChkItemWiseStock.Checked = true;
		this.ChkItemWiseStock.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkItemWiseStock.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkItemWiseStock.Location = new System.Drawing.Point(637, 93);
		this.ChkItemWiseStock.Name = "ChkItemWiseStock";
		this.ChkItemWiseStock.Size = new System.Drawing.Size(113, 19);
		this.ChkItemWiseStock.TabIndex = 10;
		this.ChkItemWiseStock.Text = "Item Wise Stock";
		this.ChkItemWiseStock.UseVisualStyleBackColor = true;
		this.ChkItemWiseStock.Visible = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(565, 66);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(70, 15);
		this.label11.TabIndex = 107;
		this.label11.Text = "Report Type";
		this.cmbReportType.AllowNull = (DefaultableBoolean)1;
		this.cmbReportType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReportType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReportType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbReportType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReportType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbReportType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReportType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance22).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReportType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbReportType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReportType).Location = new System.Drawing.Point(637, 60);
		((System.Windows.Forms.Control)(object)this.cmbReportType).Name = "cmbReportType";
		((System.Windows.Forms.Control)(object)this.cmbReportType).Size = new System.Drawing.Size(246, 26);
		((System.Windows.Forms.Control)(object)this.cmbReportType).TabIndex = 9;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(565, 37);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(46, 15);
		this.label5.TabIndex = 103;
		this.label5.Text = "Job Lot";
		this.cmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance43;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance8).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Location = new System.Drawing.Point(637, 31);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Name = "cmbJobLot";
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Size = new System.Drawing.Size(246, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).TabIndex = 8;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(265, 95);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 15);
		this.label9.TabIndex = 99;
		this.label9.Text = "Item Name";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance29;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(343, 89);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(265, 37);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(63, 15);
		this.label8.TabIndex = 97;
		this.label8.Text = "Item Type ";
		this.cmbItemType.AllowNull = (DefaultableBoolean)1;
		this.cmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance72;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Location = new System.Drawing.Point(343, 31);
		((System.Windows.Forms.Control)(object)this.cmbItemType).Name = "cmbItemType";
		((System.Windows.Forms.Control)(object)this.cmbItemType).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemType).TabIndex = 4;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(10, 95);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(91, 15);
		this.label7.TabIndex = 96;
		this.label7.Text = "Parent Category";
		this.cmbMainCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbMainCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbMainCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbMainCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbMainCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.cmbMainCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbMainCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbMainCategory).Location = new System.Drawing.Point(107, 89);
		((System.Windows.Forms.Control)(object)this.cmbMainCategory).Name = "cmbMainCategory";
		((System.Windows.Forms.Control)(object)this.cmbMainCategory).Size = new System.Drawing.Size(152, 26);
		((System.Windows.Forms.Control)(object)this.cmbMainCategory).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbMainCategory).Leave += new System.EventHandler(cmbMainCategory_Leave);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(265, 124);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(69, 15);
		this.label6.TabIndex = 95;
		this.label6.Text = "WareHouse";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(10, 124);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(82, 15);
		this.label3.TabIndex = 92;
		this.label3.Text = "Item Category";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(10, 66);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 91;
		this.label2.Text = "Date To";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(10, 37);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 90;
		this.label1.Text = "Date From";
		this.cmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Location = new System.Drawing.Point(343, 118);
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Name = "cmbWareHouse";
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).TabIndex = 7;
		this.cmbCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance67).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCategory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCategory).Location = new System.Drawing.Point(107, 118);
		((System.Windows.Forms.Control)(object)this.cmbCategory).Name = "cmbCategory";
		((System.Windows.Forms.Control)(object)this.cmbCategory).Size = new System.Drawing.Size(152, 26);
		((System.Windows.Forms.Control)(object)this.cmbCategory).TabIndex = 3;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(823, 89);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(60, 26);
		this.btnshow.TabIndex = 12;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.txtDateFrom.CustomFormat = "dd-MMM-yy";
		this.txtDateFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateFrom.Location = new System.Drawing.Point(107, 33);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.Size = new System.Drawing.Size(152, 23);
		this.txtDateFrom.TabIndex = 0;
		this.txtDateTo.CustomFormat = "dd-MMM-yy";
		this.txtDateTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateTo.Location = new System.Drawing.Point(107, 62);
		this.txtDateTo.Name = "txtDateTo";
		this.txtDateTo.Size = new System.Drawing.Size(152, 23);
		this.txtDateTo.TabIndex = 1;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(894, 29);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnRefresh, this.btnPrint, this.btnShortCutKeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(894, 29);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(59, 26);
		this.btnRefresh.Text = "&New";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(61, 26);
		this.btnPrint.Text = "Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(121, 26);
		this.btnShortCutKeys.Text = "ShortCutKeys";
		this.btnShortCutKeys.ToolTipText = "ShortCutKeys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(894, 461);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmStockReportCmtr";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmStockReportCmtr_FormClosing);
		base.Load += new System.EventHandler(frmStockReportCmtr_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmStockReportCmtr_KeyDown);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPacktype).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbReportType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbMainCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCategory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
