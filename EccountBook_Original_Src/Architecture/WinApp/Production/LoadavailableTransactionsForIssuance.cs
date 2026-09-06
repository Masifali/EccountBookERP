using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Production;

public class LoadavailableTransactionsForIssuance : Form
{
	private bool DoHaveCanRateandAmount;

	private bool JobOrderCreatewithoutRates;

	public bool CanLoadOnlyPurchaseAndSingleItemRows;

	private DropDownBind DDL = new DropDownBind();

	private int[] AryColWidth;

	public DataTable dtjoborderItems = new DataTable();

	public string JobOrderItems = string.Empty;

	private UserAccount UserAccount = new UserAccount();

	public DataTable dtIssuance = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Panel panel2;

	private Button btngrnlod;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	internal GridEX grd;

	private Button btnLoadOnInvoice;

	private Label label2;

	internal UltraCombo CmbSupplier;

	private Button btnReset;

	private Label label4;

	internal UltraCombo CmbItemName;

	private Label label7;

	internal UltraCombo CmbCropYear;

	private Label label6;

	private Label label5;

	internal UltraCombo CmbWarehouse;

	private Label label3;

	internal UltraCombo CmbReferenceDocument;

	internal UltraCombo CmbJobLot;

	private Label label8;

	internal UltraCombo CmbItemParentCategory;

	private TextBox txtSelectedStock;

	private Label label9;

	private Label label10;

	private TextBox txtSelectedQty;

	private Label label12;

	internal UltraCombo CmbItemType;

	private Label label11;

	internal UltraCombo CmbItemCategory;

	public LoadavailableTransactionsForIssuance(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void LoadInvoices_Load(object sender, EventArgs e)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1310, 830);
			JobOrderCreatewithoutRates = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("JobOrderCreatewithoutRates"));
			List<tblUserRights> lstRights = tblUserRights.GetByUserId(new tblUserRights
			{
				UserId = UserAccount.ID,
				ScreenName = "LoadavailableTransactionsForIssuance",
				RightName = UserAccount.RoleName
			});
			if (lstRights.Count > 0)
			{
				List<tblUserRights> newList = lstRights.Where((tblUserRights x) => x.RightName == "Rate").ToList();
				DoHaveCanRateandAmount = Conversion.ToBool((object)newList[0].Value.ToString());
				DoHaveCanRateandAmount = !JobOrderCreatewithoutRates && DoHaveCanRateandAmount;
			}
			FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			CmbItemParentCategory.Focus();
			dtIssuance.Columns.Add("RefDocumentTypeId");
			dtIssuance.Columns.Add("RefDocumentType");
			dtIssuance.Columns.Add("RefDocIdNo");
			dtIssuance.Columns.Add("RefDocSubIdNo");
			dtIssuance.Columns.Add("SupplierCustomerId", typeof(int));
			dtIssuance.Columns.Add("PartyName");
			dtIssuance.Columns.Add("DocDate", typeof(DateTime));
			dtIssuance.Columns.Add("DocNo", typeof(int));
			dtIssuance.Columns.Add("WareHouseId");
			dtIssuance.Columns.Add("WareHouse");
			dtIssuance.Columns.Add("ItemId");
			dtIssuance.Columns.Add("ItemName");
			dtIssuance.Columns.Add("ItemCode");
			dtIssuance.Columns.Add("ItemUOMId");
			dtIssuance.Columns.Add("PackUom");
			dtIssuance.Columns.Add("ItemUOM");
			dtIssuance.Columns.Add("CropYearId");
			dtIssuance.Columns.Add("CropYear");
			dtIssuance.Columns.Add("JobLotId");
			dtIssuance.Columns.Add("JobLotCode");
			dtIssuance.Columns.Add("PackingTypeId");
			dtIssuance.Columns.Add("PackTypeCode");
			dtIssuance.Columns.Add("BalQty", typeof(double));
			dtIssuance.Columns.Add("BalWeight", typeof(double));
			dtIssuance.Columns.Add("ItemQty", typeof(double));
			dtIssuance.Columns.Add("Weight", typeof(double));
			dtIssuance.Columns.Add("ItemRate");
			dtIssuance.Columns.Add("RateUOMId");
			dtIssuance.Columns.Add("RateUOM");
			dtIssuance.Columns.Add("Equivalent");
			dtIssuance.Columns.Add("ItemAmount", typeof(double));
			dtIssuance.Columns.Add("Remarks");
			StockComboFill();
			PendingInventoryTransactionsForIssuanceLoad();
			FromDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StockComboFill()
	{
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		try
		{
			DataTable dtMainType = new DataTable();
			dtMainType.Columns.Add("Id");
			dtMainType.Columns.Add("name");
			DataTable dtItemCategory = dtMainType.Clone();
			DataTable dtItemType = dtMainType.Clone();
			DataTable dtJobLot = new DataTable();
			dtJobLot.Columns.Add("Id");
			dtJobLot.Columns.Add("name");
			DataTable dtCropYear = new DataTable();
			dtCropYear.Columns.Add("Id");
			dtCropYear.Columns.Add("name");
			DataTable dtwr = new DataTable();
			dtwr.Columns.Add("Id");
			dtwr.Columns.Add("name");
			DataTable dtDocType = new DataTable();
			dtDocType.Columns.Add("Id");
			dtDocType.Columns.Add("name");
			DataTable dtParty = new DataTable();
			dtParty.Columns.Add("Id");
			dtParty.Columns.Add("name");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("name");
			DataTable dtAc = new DataTable();
			dtAc.Columns.Add("Id");
			dtAc.Columns.Add("name");
			DataTable dtBindCombo = StocksReport.Inventory_StockEvalautionDetail_DropDownAndLists(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesIds = Conversion.ToString((object)UserAccount.BranchesId)
			});
			if (dtBindCombo.Rows.Count > 0)
			{
				for (int i = 0; i < dtBindCombo.Rows.Count; i++)
				{
					if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "ParentCategories")
					{
						dtMainType.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					}
					else if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "ItemCategories")
					{
						dtItemCategory.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					}
					else if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "ItemTypes")
					{
						dtItemType.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					}
					else if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "JobLot")
					{
						dtJobLot.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					}
					else if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "CropYear")
					{
						dtCropYear.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					}
					else if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "Warehouse")
					{
						dtwr.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					}
					else if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "DocumentType")
					{
						dtDocType.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					}
					else if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "Supplier_Customer")
					{
						dtParty.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					}
					else if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "Items" && dtjoborderItems.Rows.Count <= 0)
					{
						dtItem.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					}
					else if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "Stock_Account")
					{
						dtAc.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					}
				}
				if (dtjoborderItems != null && dtjoborderItems.Rows.Count > 0)
				{
					foreach (DataRow row in dtjoborderItems.Rows)
					{
						dtItem.Rows.Add(row.Field<int>("ItemId"), row.Field<string>("ItemName"));
					}
				}
			}
			DDL.BindDDL(dtMainType, CmbItemParentCategory, "Id", "name", "Parent Categories", ZeroIndex: false);
			DDL.BindDDL(dtItemCategory, CmbItemCategory, "Id", "name", "Item Category", ZeroIndex: false);
			DDL.BindDDL(dtItemType, CmbItemType, "Id", "name", "Item Type", ZeroIndex: false);
			DDL.BindDDL(dtJobLot, CmbJobLot, "Id", "name", "JobLot", ZeroIndex: false);
			DDL.BindDDL(dtCropYear, CmbCropYear, "Id", "name", "CropYear", ZeroIndex: false);
			DDL.BindDDL(dtwr, CmbWarehouse, "Id", "name", "WarehouseName", ZeroIndex: false);
			DDL.BindDDL(dtDocType, CmbReferenceDocument, "Id", "name", "Document Type", ZeroIndex: false);
			DDL.BindDDL(dtParty, CmbSupplier, "Id", "name", "Party Name", ZeroIndex: false);
			DDL.BindDDL(dtItem, CmbItemName, "Id", "name", "Item Name", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btngrnlod_Click(object sender, EventArgs e)
	{
		try
		{
			PendingInventoryTransactionsForIssuanceLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingInventoryTransactionsForIssuanceLoad()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FromDate = Convert.ToDateTime(FromDate.Value);
			obj.ToDate = Convert.ToDateTime(Todate.Value);
			obj.InventoryParentCategories = Conversion.ToInt(CmbItemParentCategory.Value);
			obj.ItemCategoryId = Conversion.ToInt(CmbItemCategory.Value);
			obj.ItemTypeId = Conversion.ToInt(CmbItemType.Value);
			obj.JobLotId = Conversion.ToInt(CmbJobLot.Value);
			obj.CropYear = Conversion.ToString((object)((Control)(object)CmbCropYear).Text);
			obj.WarehouseId = Conversion.ToInt(CmbWarehouse.Value);
			obj.RefDocumentTypeId = Conversion.ToInt(CmbReferenceDocument.Value);
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			if (!string.IsNullOrEmpty(JobOrderItems))
			{
				obj.ItemIds = JobOrderItems;
				obj.ItemId = Conversion.ToInt(CmbItemName.Value);
			}
			else
			{
				obj.ItemId = Conversion.ToInt(CmbItemName.Value);
			}
			DataTable dtlst = new DataTable();
			dtlst = InventoryStockEvalautionDetail.GetAvailableTransactionsForIssuance(obj);
			if (dtlst.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RefDocumentTypeId", typeof(int));
				dtcol.Columns.Add("RefDocIdNo", typeof(int));
				dtcol.Columns.Add("RefDocSubIdNo", typeof(int));
				dtcol.Columns.Add("RefDocumentType", typeof(string));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("DocCodeNo", typeof(int));
				dtcol.Columns.Add("ManualNo", typeof(string));
				dtcol.Columns.Add("GrnNo", typeof(int));
				dtcol.Columns.Add("SupplierCustomerId", typeof(int));
				dtcol.Columns.Add("SupplierCustomerName", typeof(string));
				dtcol.Columns.Add("VehicleNo", typeof(string));
				dtcol.Columns.Add("GpNo", typeof(string));
				dtcol.Columns.Add("WarehouseId", typeof(int));
				dtcol.Columns.Add("WareHouseCode", typeof(string));
				dtcol.Columns.Add("RefWarehouse", typeof(string));
				dtcol.Columns.Add("ItemId", typeof(int));
				dtcol.Columns.Add("ItemName", typeof(string));
				dtcol.Columns.Add("ItemCode", typeof(string));
				dtcol.Columns.Add("CropYearId", typeof(int));
				dtcol.Columns.Add("CropBatch", typeof(string));
				dtcol.Columns.Add("JobLotId", typeof(int));
				dtcol.Columns.Add("JobLotCode", typeof(string));
				dtcol.Columns.Add("InvPackingTypeId", typeof(int));
				dtcol.Columns.Add("PackingType", typeof(string));
				dtcol.Columns.Add("ItemUom", typeof(string));
				dtcol.Columns.Add("PackUom", typeof(string));
				dtcol.Columns.Add("PackSize", typeof(decimal));
				dtcol.Columns.Add("QtyIn", typeof(decimal));
				dtcol.Columns.Add("QtyOut", typeof(decimal));
				dtcol.Columns.Add("QtyBalance", typeof(decimal));
				dtcol.Columns.Add("WeightIn", typeof(decimal));
				dtcol.Columns.Add("WeightOut", typeof(decimal));
				dtcol.Columns.Add("WeightBalance", typeof(decimal));
				dtcol.Columns.Add("ReserveWeight", typeof(decimal));
				dtcol.Columns.Add("StockWeightOut", typeof(decimal));
				dtcol.Columns.Add("AVgRate", typeof(decimal));
				dtcol.Columns.Add("RateUom", typeof(string));
				dtcol.Columns.Add("Equivalent", typeof(decimal));
				dtcol.Columns.Add("RateUomId", typeof(int));
				dtcol.Columns.Add("ItemAmount", typeof(decimal));
				dtcol.Columns.Add("BiltyNo", typeof(string));
				dtcol.Columns.Add("Remarks", typeof(string));
				dtcol.Rows.Clear();
				foreach (DataRow row in dtlst.Rows)
				{
					dtcol.Rows.Add(row["RefDocumentTypeId"], row["RefDocIdNo"], row["RefDocSubIdNo"], row["RefDocumentType"], row["DocDate"], row["DocCodeNo"], row["ManualNo"], (Conversion.ToInt(row["RefDocumentTypeId"]) == 112) ? ((object)0) : ((Conversion.ToInt(row["RefDocumentTypeId"]) == 80) ? ((object)0) : row["GrnNo"]), row["SupplierCustomerId"], row["SupplierCustomerName"], row["VehicleNo"], row["GpNo"], row["WarehouseId"], row["WareHouseCode"], row["RefWarehouse"], row["ItemId"], row["ItemName"], row["ItemCode"], row["CropYearId"], row["CropBatch"], row["JobLotId"], row["JobLotCode"], row["InvPackingTypeId"], row["PackingType"], row["ItemUom"], row["PackUom"], row["PackSize"], row["QtyIn"], row["QtyOut"], row["QtyBalance"], row["WeightIn"], row["WeightOut"], row["WeightBalance"], row["ReserveWeight"], row["StockWeightOut"], row["AVgRate"], row["RateUom"], row["Equivalent"], row["RateUomId"], row["ItemAmount"], row["BiltyNo"], row["TranRemarks"]);
				}
				grd.DataSource = dtcol;
				grd.RetrieveStructure();
				grdSettings();
			}
			else
			{
				grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		try
		{
			grd.FilterMode = (FilterMode)2;
			grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
			if (!FoodProductionWithValues.ValuesShowRights)
			{
				grd.RootTable.Columns["ItemAmount"].Visible = false;
				grd.RootTable.Columns["AVgRate"].Visible = false;
			}
			grd.RootTable.Columns["ItemName"].Width = 250;
			grd.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["CropYearId"].Visible = false;
			grd.RootTable.Columns["RefDocIdNo"].Visible = false;
			grd.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["ItemUom"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["InvPackingTypeId"].Visible = false;
			grd.RootTable.Columns["WareHouseId"].Visible = false;
			grd.RootTable.Columns["RateUomId"].Visible = false;
			grd.RootTable.Columns["ReserveWeight"].Visible = false;
			grd.RootTable.Columns["PackSize"].Visible = false;
			grd.RootTable.Columns["Equivalent"].Visible = false;
			grd.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grd.RootTable.Columns["StockWeightOut"].Visible = false;
			grd.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					ConfigureNumericalColumn(column);
				}
			}
			grd.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["DocCodeNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["RefDocumentType"].Width = Constants.InventoryConstants.DocumentTypeCode;
			grd.RootTable.Columns["SupplierCustomerName"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["GrnNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["ManualNo"].Width = Constants.InventoryConstants.RecordNo;
			grd.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grd.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grd.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grd.RootTable.Columns["WareHouseCode"].Width = Constants.InventoryConstants.WareHouseName - 40;
			grd.RootTable.Columns["RefWarehouse"].Width = Constants.InventoryConstants.WareHouseName - 40;
			grd.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode - 20;
			grd.RootTable.Columns["CropBatch"].Width = Constants.InventoryConstants.CropYearConstant;
			grd.RootTable.Columns["JobLotCode"].Width = Constants.InventoryConstants.JobLotConstant;
			grd.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType - 35;
			grd.RootTable.Columns["ItemUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["PackSize"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["QtyIn"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["QtyOut"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["QtyBalance"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["WeightIn"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["WeightOut"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["WeightBalance"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["ReserveWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["StockWeightOut"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["AVgRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["Equivalent"].Width = Constants.InventoryConstants.VariantEquivalent;
			grd.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["BiltyNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			grd.RootTable.Columns["BiltyNo"].Position = grd.RootTable.Columns["VehicleNo"].Position + 1;
			grd.RootTable.Columns.Add("Select");
			grd.RootTable.Columns["Select"].ActAsSelector = true;
			grd.RootTable.Columns["Select"].UseHeaderSelector = true;
			grd.RootTable.Columns["Select"].Width = 40;
			grd.RootTable.Columns["Select"].Position = 0;
			grd.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ConfigureNumericalColumn(GridEXColumn column)
	{
		if (column.Key.Contains("Amount"))
		{
			column.AggregateFunction = (AggregateFunction)2;
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = clsGlobalVariables.stringFormatsingle;
			column.TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		else if (column.Key.Contains("Rate"))
		{
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = clsGlobalVariables.DecimalRateFormate;
		}
		else
		{
			column.AggregateFunction = (AggregateFunction)2;
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = "#,##0.##";
			column.TotalFormatString = "#,##0.##";
		}
	}

	private void grd_ColumnHeaderClick(object sender, ColumnActionEventArgs e)
	{
		SelectedWeightCalculation();
	}

	private void grd_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
		SelectedWeightCalculation();
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		CmbItemParentCategory.Focus();
		((Control)(object)CmbSupplier).Text = string.Empty;
		((Control)(object)CmbItemName).Text = string.Empty;
		grd.ClearStructure();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		dtIssuance = null;
		Hide();
	}

	private void btnLoadOnInvoice_Click_1(object sender, EventArgs e)
	{
		try
		{
			List<GridEXRow> getCount = grd.GetCheckedRows().ToList();
			if (getCount.Count == 0)
			{
				MessageBox.Show("Please Select Row first");
				return;
			}
			if (CanLoadOnlyPurchaseAndSingleItemRows)
			{
				int firstItemId = Conversion.ToInt(getCount[0].Cells["ItemId"].Value);
				if (!getCount.All((GridEXRow r) => Conversion.ToInt(r.Cells["ItemId"].Value) == firstItemId))
				{
					MessageBox.Show("Please select rows with the same ItemId.");
					return;
				}
			}
			foreach (GridEXRow r2 in getCount)
			{
				dtIssuance.Rows.Add(r2.Cells["RefDocumentTypeId"].Value.ToString(), r2.Cells["RefDocumentType"].Value.ToString(), r2.Cells["RefDocIdNo"].Value.ToString(), r2.Cells["RefDocSubIdNo"].Value.ToString(), r2.Cells["SupplierCustomerId"].Value.ToString(), r2.Cells["SupplierCustomerName"].Value.ToString(), r2.Cells["DocDate"].Value.ToString(), r2.Cells["DocCodeNo"].Value.ToString(), Conversion.ToInt(r2.Cells["WarehouseId"].Value), r2.Cells["WareHouseCode"].Value.ToString(), Conversion.ToInt(r2.Cells["ItemId"].Value), r2.Cells["ItemName"].Value.ToString(), r2.Cells["ItemCode"].Value.ToString(), Conversion.ToInt(r2.Cells["ItemUom"].Value), r2.Cells["PackUom"].Value, Conversion.ToDouble(r2.Cells["PackSize"].Value), Conversion.ToInt(r2.Cells["CropYearId"].Value), r2.Cells["CropBatch"].Value.ToString(), Conversion.ToInt(r2.Cells["JobLotId"].Value), r2.Cells["JobLotCode"].Value.ToString(), Conversion.ToInt(r2.Cells["InvPackingTypeId"].Value), r2.Cells["PackingType"].Value.ToString(), Conversion.ToDouble(r2.Cells["QtyBalance"].Value), Conversion.ToDouble(r2.Cells["WeightBalance"].Value), Conversion.ToDouble(r2.Cells["QtyBalance"].Value), Conversion.ToDouble(r2.Cells["WeightBalance"].Value), Conversion.ToDouble(r2.Cells["AVgRate"].Value), Conversion.ToInt(r2.Cells["RateUomId"].Value), r2.Cells["RateUom"].Value, Conversion.ToDouble(r2.Cells["Equivalent"].Value), Conversion.ToDouble(r2.Cells["ItemAmount"].Value), "");
			}
			Hide();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadavailableTransactionsForIssuance_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				CmbItemParentCategory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.L)
			{
				btnLoadOnInvoice_Click_1(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btngrnlod_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SelectedWeightCalculation()
	{
		try
		{
			double SelectedWeight = 0.0;
			double SelectedQty = 0.0;
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			foreach (GridEXRow item in checkedRows)
			{
				SelectedWeight += Conversion.ToDouble(item.Cells["WeightBalance"].Value);
				SelectedQty += Conversion.ToDouble(item.Cells["QtyBalance"].Value);
			}
			txtSelectedStock.Text = Math.Round(SelectedWeight).ToString("0,0");
			txtSelectedQty.Text = Math.Round(SelectedQty).ToString("0,0");
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
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Expected O, but got Unknown
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Expected O, but got Unknown
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Expected O, but got Unknown
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Expected O, but got Unknown
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Expected O, but got Unknown
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Expected O, but got Unknown
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_072a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Expected O, but got Unknown
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_074b: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
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
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
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
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
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
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.txtSelectedQty = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtSelectedStock = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbItemParentCategory = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbCropYear = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbWarehouse = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbReferenceDocument = new UltraCombo();
		this.CmbJobLot = new UltraCombo();
		this.btnReset = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbItemName = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.btnLoadOnInvoice = new System.Windows.Forms.Button();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.CmbItemType = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbItemCategory = new UltraCombo();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemParentCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceDocument).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.ctrlGrdBar1);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1117, 32);
		this.panel1.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1068, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1117, 501);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellValueChanged += new ColumnActionEventHandler(grd_CellValueChanged);
		this.grd.ColumnHeaderClick += new ColumnActionEventHandler(grd_ColumnHeaderClick);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 7);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(301, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Available Transactions For Issuance";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.label12);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemType);
		this.panel2.Controls.Add(this.label11);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCategory);
		this.panel2.Controls.Add(this.label10);
		this.panel2.Controls.Add(this.txtSelectedQty);
		this.panel2.Controls.Add(this.label9);
		this.panel2.Controls.Add(this.txtSelectedStock);
		this.panel2.Controls.Add(this.label8);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemParentCategory);
		this.panel2.Controls.Add(this.label7);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouse);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReferenceDocument);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.panel2.Controls.Add(this.btnReset);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.panel2.Controls.Add(this.btnLoadOnInvoice);
		this.panel2.Controls.Add(this.btngrnlod);
		this.panel2.Controls.Add(this.Todate);
		this.panel2.Controls.Add(this.FromDate);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 32);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1117, 141);
		this.panel2.TabIndex = 30;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(675, 36);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(103, 15);
		this.label10.TabIndex = 42;
		this.label10.Text = "Total Selected Qty";
		this.txtSelectedQty.BackColor = System.Drawing.Color.White;
		this.txtSelectedQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSelectedQty.Location = new System.Drawing.Point(804, 32);
		this.txtSelectedQty.Name = "txtSelectedQty";
		this.txtSelectedQty.ReadOnly = true;
		this.txtSelectedQty.Size = new System.Drawing.Size(120, 23);
		this.txtSelectedQty.TabIndex = 13;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(675, 9);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(123, 15);
		this.label9.TabIndex = 40;
		this.label9.Text = "Total Selected Weight";
		this.txtSelectedStock.BackColor = System.Drawing.Color.White;
		this.txtSelectedStock.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSelectedStock.Location = new System.Drawing.Point(804, 5);
		this.txtSelectedStock.Name = "txtSelectedStock";
		this.txtSelectedStock.ReadOnly = true;
		this.txtSelectedStock.Size = new System.Drawing.Size(120, 23);
		this.txtSelectedStock.TabIndex = 12;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(8, 36);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(91, 15);
		this.label8.TabIndex = 38;
		this.label8.Text = "Parent Category";
		this.CmbItemParentCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance95;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance101).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).Location = new System.Drawing.Point(103, 30);
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).Name = "CmbItemParentCategory";
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemParentCategory).TabIndex = 2;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(347, 9);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(57, 15);
		this.label7.TabIndex = 36;
		this.label7.Text = "Crop Year";
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(443, 3);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 6;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(347, 117);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 34;
		this.label6.Text = "Item Name";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(347, 36);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 15);
		this.label5.TabIndex = 33;
		this.label5.Text = "Warehouse";
		this.CmbWarehouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Location = new System.Drawing.Point(443, 30);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Name = "CmbWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).TabIndex = 7;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(347, 63);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(84, 15);
		this.label3.TabIndex = 31;
		this.label3.Text = "Ref Document";
		this.CmbReferenceDocument.AllowNull = (DefaultableBoolean)1;
		this.CmbReferenceDocument.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReferenceDocument.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReferenceDocument.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Appearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance82;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance88).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReferenceDocument).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbReferenceDocument.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbReferenceDocument).Location = new System.Drawing.Point(443, 57);
		((System.Windows.Forms.Control)(object)this.CmbReferenceDocument).Name = "CmbReferenceDocument";
		((System.Windows.Forms.Control)(object)this.CmbReferenceDocument).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.CmbReferenceDocument).TabIndex = 8;
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance29;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(103, 111);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 5;
		this.btnReset.BackColor = System.Drawing.Color.Teal;
		this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnReset.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.ForeColor = System.Drawing.Color.White;
		this.btnReset.Location = new System.Drawing.Point(672, 112);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(70, 25);
		this.btnReset.TabIndex = 13;
		this.btnReset.Text = "Reset";
		this.btnReset.UseVisualStyleBackColor = false;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(8, 118);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(46, 15);
		this.label4.TabIndex = 28;
		this.label4.Text = "Job Lot";
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance69;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance77).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(443, 111);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 10;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(347, 90);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(69, 15);
		this.label2.TabIndex = 24;
		this.label2.Text = "Party Name";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance87).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(443, 84);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 9;
		this.btnLoadOnInvoice.BackColor = System.Drawing.Color.Teal;
		this.btnLoadOnInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.ForeColor = System.Drawing.Color.White;
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(672, 86);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(70, 25);
		this.btnLoadOnInvoice.TabIndex = 12;
		this.btnLoadOnInvoice.Text = "Load";
		this.btnLoadOnInvoice.UseVisualStyleBackColor = false;
		this.btnLoadOnInvoice.Click += new System.EventHandler(btnLoadOnInvoice_Click_1);
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btngrnlod.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(672, 58);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(70, 26);
		this.btngrnlod.TabIndex = 11;
		this.btngrnlod.Text = "Search";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.Todate.CustomFormat = "dd-MMM-yy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(223, 5);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(119, 23);
		this.Todate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(103, 5);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(119, 23);
		this.FromDate.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(8, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(91, 15);
		this.label1.TabIndex = 12;
		this.label1.Text = "From && To Date";
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 173);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1117, 501);
		this.panel3.TabIndex = 31;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(8, 90);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(60, 15);
		this.label12.TabIndex = 50;
		this.label12.Text = "Item Type";
		this.CmbItemType.AllowNull = (DefaultableBoolean)1;
		this.CmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance22).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Location = new System.Drawing.Point(103, 84);
		((System.Windows.Forms.Control)(object)this.CmbItemType).Name = "CmbItemType";
		((System.Windows.Forms.Control)(object)this.CmbItemType).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemType).TabIndex = 4;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(8, 63);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(82, 15);
		this.label11.TabIndex = 48;
		this.label11.Text = "Item Category";
		this.CmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance108;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Location = new System.Drawing.Point(103, 57);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Name = "CmbItemCategory";
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).TabIndex = 3;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1117, 674);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "LoadavailableTransactionsForIssuance";
		this.Text = "Available Transactions For Issuance";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(LoadInvoices_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LoadavailableTransactionsForIssuance_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemParentCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceDocument).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).EndInit();
		base.ResumeLayout(false);
	}
}
