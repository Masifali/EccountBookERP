// using Architecture.Reports;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Production;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.Production
{

public class BillOfMaterialWiseProduction : Form
{
	private UserAccount UserAccount = new UserAccount();

	private int RecId;

	private int updateDetailIndex;

	private bool UpdateMode;

	private DataTable dtitem = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtUom = new DataTable();

	private DataTable table = new DataTable();

	private DataTable tableByProduct = new DataTable();

	private DataTable dtPackingMaterial = new DataTable();

	private DataTable dtExpense = new DataTable();

	private DataTable dtUomlst = new DataTable();

	private DataTable dtChargeTo = new DataTable();

	private DataTable dtMoistureSlab = new DataTable();

	private DataTable dtAccounts = new DataTable();

	private int VoucherHeadId;

	private bool DetailEditMood;

	private bool CheckExpenseAmountTotal;

	private bool checkPackingMaterialAmountTotal;

	private LoadavailableTransactionsForIssuance LoadTransactions;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton Print;

	private Panel panel4;

	private Panel panel10;

	private Panel panel13;

	private Panel panel11;

	private Panel panel16;

	private Panel panel15;

	private ToolStrip toolStrip2;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton toolStripButton4;

	private Button btnSearch;

	private Label label50;

	private TextBox txtDocNumberTo;

	private DateTimePicker txtToDate;

	private Label label52;

	private DateTimePicker txtfromDate;

	private Label label48;

	private Label label49;

	private TextBox txtDocNoFrom;

	internal GridEX DatagridHistory;

	private Panel panel17;

	internal GridEX grdDetail;

	private Panel panel7;

	private GroupBox groupBox2;

	private TextBox txtPackingMaterialAmount;

	private TextBox txtPackingMaterialRate;

	private TextBox txtPackingMaterialWeight;

	private TextBox txtPackingMaterialQty;

	private Label label42;

	private TextBox txtOverHeadAmount;

	private TextBox txtFinishGoodsAmount;

	private TextBox txtOutputByProductAmount;

	private TextBox txtInputAmount;

	private Label label35;

	private TextBox txtOverHeadRate;

	private TextBox txtOverHeadWeight;

	private TextBox txtOverHeadQty;

	private TextBox txtFinishGoodsRate;

	private TextBox txtFinishGoodsWeight;

	private TextBox txtFinishGoodsQty;

	private TextBox txtOutputByProductRate;

	private TextBox txtOutputByProductWeight;

	private TextBox txtOutputByProductQty;

	private TextBox txtInputRate;

	private TextBox txtInputWeight;

	private Label label36;

	private Label label41;

	private Label label40;

	private Label label39;

	private Label label38;

	private Label label37;

	private Label label34;

	private Label label33;

	private TextBox txtInputQuantity;

	private ToolStripButton btnHNew;

	private Panel panel19;

	internal GridEX grdDetailExpense;

	private Panel panel20;

	internal GridEX grdDetailPackingMaterial;

	private Panel panel18;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton2;

	private Label label22;

	private TextBox txtDifference;

	private TextBox txtAvgRate;

	private ToolStripButton btnRefresh;

	private Panel panel21;

	private Panel panel14;

	internal UltraCombo cmbEntryType;

	private CheckBox chkPrint;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private TextBox txtdeailRemarks;

	private Label label21;

	internal UltraCombo cmbRateUom;

	private Label label47;

	internal UltraCombo cmbUOM;

	private Label label32;

	private Label label23;

	private Label label30;

	private TextBox txtAverageRate;

	private Label label28;

	private TextBox txtStock;

	internal UltraCombo cmbGodown;

	internal UltraCombo cmbLot;

	internal UltraCombo cmbItem;

	internal UltraCombo CmbCropyr;

	private Button Add;

	private DateTimePicker txtDocdate;

	private Label label2;

	private Label label3;

	private TextBox txtAmount;

	private Label label1;

	private Label label26;

	private Label label6;

	private TextBox txtRate;

	private Label label24;

	private Label label44;

	private Label label7;

	private TextBox txtUnitWeight;

	private TextBox txtdocnumber;

	private TextBox txtProductionNo;

	private TextBox txtQty;

	private TextBox txtRemarks;

	private Label label18;

	private Label label5;

	private Label label12;

	internal UltraCombo cmbBagType;

	private Label label10;

	private Label label8;

	private GroupBox groupBox1;

	private Label label27;

	internal UltraCombo cmbParentCategory;

	private Label label4;

	private TextBox txtMoisture;

	internal UltraCombo cmbMoistureSlab;

	private Label label9;

	private Panel panel5;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private Panel panel22;

	internal GridEX grdStockConversionProductionDetail;

	private TabPage tabPage4;

	private Panel panel23;

	internal GridEX grdByProduct;

	private TabPage tabPage5;

	private TabPage tabPage6;

	private Panel panel6;

	private Panel panel12;

	internal GridEX grdOverHead;

	private Label label11;

	private Label label13;

	private Label label14;

	private Label label15;

	internal GridEX grdPackingMaterial;

	private ToolStripButton btnGenerate;

	private CtrlGrdBar ctrlGrdBar2;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnIssuanceLoad;

	private Button btnLoadBOM;

	public BillOfMaterialWiseProduction(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void invfrmStockConversionProduction_Load(object sender, EventArgs e)
	{
		try
		{
			AccountTitleFll();
			dtitem.Columns.Add("Id");
			dtitem.Columns.Add("ItemName");
			dtUom.Columns.Add("Id");
			dtUom.Columns.Add("ItemUOM");
			table.Columns.Add("RefDocumentTypeId");
			table.Columns.Add("RefDocNoId");
			table.Columns.Add("RefDocSubId");
			table.Columns.Add("EntryType");
			table.Columns.Add("WareHouseId");
			table.Columns.Add("WareHouse");
			table.Columns.Add("ItemId");
			table.Columns.Add("Item");
			table.Columns.Add("ItemUOMId");
			table.Columns.Add("UOM");
			table.Columns.Add("CropYear");
			table.Columns.Add("JobLotId");
			table.Columns.Add("JobLot");
			table.Columns.Add("PackingTypeId");
			table.Columns.Add("PackingType");
			table.Columns.Add("BalQty", typeof(double));
			table.Columns.Add("BalWeight", typeof(double));
			table.Columns.Add("Quantity", typeof(double));
			table.Columns.Add("Weight", typeof(double));
			table.Columns.Add("Rate", typeof(double));
			table.Columns.Add("RateUOMId");
			table.Columns.Add("RateUOM");
			table.Columns.Add("Amount", typeof(double));
			table.Columns.Add("Moisture");
			table.Columns.Add("MoistureSlabId");
			table.Columns.Add("Remarks");
			grdStockConversionProductionDetail.DataSource = table;
			grdStockConversionProductionDetail.RetrieveStructure();
			gridsettings();
			tableByProduct.Columns.Add("EntryType");
			tableByProduct.Columns.Add("WareHouseId");
			tableByProduct.Columns.Add("WareHouse");
			tableByProduct.Columns.Add("ItemId");
			tableByProduct.Columns.Add("Item");
			tableByProduct.Columns.Add("ItemUOMId");
			tableByProduct.Columns.Add("UOM");
			tableByProduct.Columns.Add("CropYear");
			tableByProduct.Columns.Add("JobLotId");
			tableByProduct.Columns.Add("JobLot");
			tableByProduct.Columns.Add("PackingTypeId");
			tableByProduct.Columns.Add("PackingType");
			tableByProduct.Columns.Add("Quantity", typeof(double));
			tableByProduct.Columns.Add("Weight", typeof(double));
			tableByProduct.Columns.Add("Rate", typeof(double));
			tableByProduct.Columns.Add("RateUOMId");
			tableByProduct.Columns.Add("RateUOM");
			tableByProduct.Columns.Add("AmountWithoutExpenses", typeof(double));
			tableByProduct.Columns.Add("Moisture");
			tableByProduct.Columns.Add("MoistureSlabId");
			tableByProduct.Columns.Add("MoistureSlabDescription");
			tableByProduct.Columns.Add("PMAmount", typeof(double));
			tableByProduct.Columns.Add("ExpAmount", typeof(double));
			tableByProduct.Columns.Add("Amount", typeof(double));
			tableByProduct.Columns.Add("Remarks");
			grdByProduct.DataSource = tableByProduct;
			grdByProduct.RetrieveStructure();
			gridsettingsByProduct();
			dtPackingMaterial.Columns.Add("ItemId");
			dtPackingMaterial.Columns.Add("ItemQTY", typeof(double));
			dtPackingMaterial.Columns.Add("ItemRate", typeof(double));
			dtPackingMaterial.Columns.Add("Amount", typeof(double));
			dtPackingMaterial.Columns.Add("ChargeTo");
			dtExpense.Columns.Add("ChartofAccount");
			dtExpense.Columns.Add("LedgerRemarks");
			dtExpense.Columns.Add("ExpAmount", typeof(double));
			dtExpense.Columns.Add("ChargeTo");
			txtProductionNo.Focus();
			CmbEntryTypeFill();
			bagType();
			UOMFill();
			MoistureSlabFill();
			ParentCategoryFill();
			cmbChargeTo();
			GenerateDocNumber();
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			AddRowInPackingMaterialGrid();
			grdPackingMaterial.RetrieveStructure();
			grdPackingMaterialSettings();
			AddRowInExpenseGrid();
			grdOverHead.RetrieveStructure();
			grdConversionExpenseSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		try
		{
			InvStockConversion val = new InvStockConversion
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocTypeId = 79
			};
			DataTable dth = new DataTable();
			dth = InvStockConversion.GetHistory(val);
			DataTable dtHistory = new DataTable();
			dtHistory.Columns.Add("Id");
			dtHistory.Columns.Add("DocSrNo");
			dtHistory.Columns.Add("DocDate");
			dtHistory.Columns.Add("ProductionNo");
			dtHistory.Columns.Add("Remarks");
			for (int i = 0; i < dth.Rows.Count; i++)
			{
				dtHistory.Rows.Add(dth.Rows[i]["Id"], dth.Rows[i]["DocSrNo"], Conversion.ToDateTime(dth.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dth.Rows[i]["ProductionNo"], dth.Rows[i]["Remarks"]);
			}
			DatagridHistory.DataSource = dtHistory;
			DatagridHistory.RetrieveStructure();
			DatagridHistory.RootTable.Columns["Id"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGridHeaderistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		try
		{
			InvStockConversion val = new InvStockConversion
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocTypeId = 79
			};
			DataTable dth = new DataTable();
			dth = InvStockConversion.GetHistory(val);
			DataTable dtHistory = new DataTable();
			dtHistory.Columns.Add("Id");
			dtHistory.Columns.Add("DocSrNo");
			dtHistory.Columns.Add("DocDate");
			dtHistory.Columns.Add("PNo");
			dtHistory.Columns.Add("Remarks");
			for (int i = 0; i < dth.Rows.Count; i++)
			{
				dtHistory.Rows.Add(dth.Rows[i]["Id"], dth.Rows[i]["DocSrNo"], Conversion.ToDateTime(dth.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dth.Rows[i]["ProductionNo"], dth.Rows[i]["Remarks"]);
			}
			DatagridHistory.DataSource = dtHistory;
			DatagridHistory.RetrieveStructure();
			HeaderGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RefreshForm()
	{
		try
		{
			VoucherHeadId = 0;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			UpdateMode = false;
			txtProductionNo.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			txtQty.Text = string.Empty;
			txtUnitWeight.Text = string.Empty;
			table.Clear();
			GenerateDocNumber();
			grdStockConversionProductionDetail.ClearStructure();
			grdByProduct.ClearStructure();
			tableByProduct.Rows.Clear();
			grdOverHead.ClearStructure();
			grdPackingMaterial.ClearStructure();
			dtExpense.Clear();
			dtPackingMaterial.Clear();
			AddRowInPackingMaterialGrid();
			grdPackingMaterial.RetrieveStructure();
			grdPackingMaterialSettings();
			AddRowInExpenseGrid();
			grdOverHead.RetrieveStructure();
			grdConversionExpenseSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void resetDetail()
	{
		cmbItem.Value = string.Empty;
		cmbUOM.Value = string.Empty;
		cmbGodown.Value = string.Empty;
		CmbCropyr.Value = string.Empty;
		cmbLot.Value = string.Empty;
		cmbBagType.Value = string.Empty;
		((Control)(object)cmbRateUom).Text = string.Empty;
		((Control)(object)cmbMoistureSlab).Text = string.Empty;
		txtUnitWeight.Text = string.Empty;
		txtAverageRate.Text = string.Empty;
		txtMoisture.Text = string.Empty;
		txtQty.Text = string.Empty;
		txtStock.Text = 0.ToString();
		txtRate.Text = string.Empty;
		txtAmount.Text = string.Empty;
		txtdeailRemarks.Text = string.Empty;
		CheckExpenseAmountTotal = false;
		checkPackingMaterialAmountTotal = false;
	}

	private void summeryreset()
	{
		txtInputAmount.Text = string.Empty;
		txtInputQuantity.Text = string.Empty;
		txtInputWeight.Text = string.Empty;
		txtInputRate.Text = string.Empty;
		txtOutputByProductAmount.Text = string.Empty;
		txtOutputByProductQty.Text = string.Empty;
		txtOutputByProductRate.Text = string.Empty;
		txtOutputByProductWeight.Text = string.Empty;
		txtFinishGoodsAmount.Text = string.Empty;
		txtFinishGoodsQty.Text = string.Empty;
		txtFinishGoodsRate.Text = string.Empty;
		txtFinishGoodsWeight.Text = string.Empty;
		txtOverHeadAmount.Text = string.Empty;
		txtOverHeadQty.Text = string.Empty;
		txtOverHeadRate.Text = string.Empty;
		txtOverHeadWeight.Text = string.Empty;
		txtPackingMaterialAmount.Text = string.Empty;
		txtPackingMaterialQty.Text = string.Empty;
		txtPackingMaterialRate.Text = string.Empty;
		txtPackingMaterialWeight.Text = string.Empty;
	}

	private void CmbEntryTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("EntryType");
			dt.Rows.Add(3, "Recovery Head Rice");
			DDL.BindDDL(dt, cmbEntryType, "Id", "EntryType", "EntryType", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbChargeTo()
	{
		try
		{
			dtChargeTo.Columns.Add("Id");
			dtChargeTo.Columns.Add("ChargeTo");
			dtChargeTo.Rows.Add(1, "Recovery By Product");
			dtChargeTo.Rows.Add(2, "Recovery Head Rice");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Warehouse()
	{
		try
		{
			DataTable dtwh = CommonServices.getActiveWareHouse();
			if (dtwh.Rows.Count > 0)
			{
				DDL.BindDDL(dtwh, cmbGodown, "Id", "WareHouseName", "Issue From ", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			Item obj = new Item();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtitem = Item.ReadAllItems(obj);
			if (dtitem.Rows.Count > 0)
			{
				DDL.BindDDL(dtitem, cmbItem, "Id", "ItemName", "Item Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combojoblotfill()
	{
		try
		{
			DataTable dtJob = CommonServices.JobLotGetAllService();
			if (dtJob.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtJob, cmbLot, "Id", "JobLotDescription", "JobLot Description", false);
			}
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
			DataTable dtCrop = CommonServices.CropYearGetAllService();
			if (dtCrop.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCrop, CmbCropyr, "Id", "CropYear", "Crop Year", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bagType()
	{
		try
		{
			DataTable dtpt = new DataTable();
			dtpt = InvPackingType.Getall();
			if (dtpt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtpt, cmbBagType, "Id", "PackTypeDesc", "Packing Type", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountTitleFll()
	{
		try
		{
			dtAccounts = CommonServices.CoaAllocationGetForComboServiceBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UOMFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		UOMSchedule obj = new UOMSchedule();
		obj.OrganizationId = UserAccount.OrganizationId;
		obj.CompanyId = UserAccount.CompanyId;
		dtUomlst = UOMSchedule.Getall(obj);
	}

	private void bindUom()
	{
		try
		{
			if (dtUomlst.Rows.Count <= 0)
			{
				return;
			}
			dtUom.Rows.Clear();
			for (int i = 0; i < dtUomlst.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtUomlst.Rows[i]["ItemId"]) == Conversion.ToInt(cmbItem.Value))
				{
					dtUom.Rows.Add(dtUomlst.Rows[i]["Id"], dtUomlst.Rows[i]["Equivalent"]);
				}
				if (dtUom.Rows.Count > 0)
				{
					DDL.BindDDL(dtUom, cmbUOM, "Id", "ItemUOM", "UOM", ZeroIndex: false);
					DDL.BindDDL(dtUom, cmbRateUom, "Id", "ItemUOM", "UOM", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbEntryType).Text == "Issue")
			{
				GetAllLotsByItemByCropByWarehouseByInventoryParentCategory(Conversion.ToInt(cmbParentCategory.Value), Conversion.ToInt(cmbGodown.Value), Conversion.ToString((object)((Control)(object)CmbCropyr).Text), Conversion.ToInt(cmbItem.Value));
				bindUom();
			}
			else
			{
				bindUom();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAvgRate()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemId = Conversion.ToInt(cmbItem.Value),
				CropYear = ((Control)(object)CmbCropyr).Text.ToString(),
				JobLotId = Conversion.ToInt(cmbLot.Value),
				ToDate = txtDocdate.Value
			};
			DataTable dtang = new DataTable();
			dtang = InvPurchaseInvoice.GetInventoryStockEvalautionDetail_GetAvgRates(val);
			if (dtang.Rows.Count > 0)
			{
				txtAverageRate.Text = Math.Round(Conversion.ToDouble(dtang.Rows[0]["AvgRate"]), 2).ToString();
				if (!DetailEditMood)
				{
					txtRate.Text = Math.Round(Conversion.ToDouble(dtang.Rows[0]["AvgRate"]), 2).ToString();
				}
			}
			else
			{
				txtAverageRate.Text = "0";
				txtRate.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherHeadIdGet()
	{
		try
		{
			VoucherHeadId = CommonServices.VoucherHeadIdGet(RecId, 79);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParentCategoryFill()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			DataTable dtParentCategory = InvStockConversion.ItemStockFilterFromInventoryTransaction(new ReportsParameters
			{
				Activity = "GetPaddyRiceParentCategories",
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtParentCategory.Rows.Count > 0)
			{
				for (int i = 0; i < dtParentCategory.Rows.Count; i++)
				{
					dt.Rows.Add(dtParentCategory.Rows[i]["Id"], dtParentCategory.Rows[i]["InvParentCateCode"]);
				}
				DDL.BindDDL(dt, cmbParentCategory, "Id", "Name", "Parent Category", ZeroIndex: false);
				((UltraGridBase)cmbParentCategory).DisplayLayout.Bands[0].Columns[1].Width = 167;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WareHouseFillByParentCategoryId()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		try
		{
			((Control)(object)cmbGodown).Text = string.Empty;
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			DataTable dtWarehouse = InvStockConversion.ItemStockFilterFromInventoryTransaction(new ReportsParameters
			{
				Activity = "GetAllWarehousesByParentCategory",
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemCategoryId = Conversion.ToInt(cmbParentCategory.Value)
			});
			if (dtWarehouse.Rows.Count > 0)
			{
				dt.Rows.Clear();
				for (int i = 0; i < dtWarehouse.Rows.Count; i++)
				{
					dt.Rows.Add(dtWarehouse.Rows[i]["Id"], dtWarehouse.Rows[i]["WareHouseName"]);
				}
				DDL.BindDDL(dt, cmbGodown, "Id", "Name", "Warehouse", ZeroIndex: false);
				((UltraGridBase)cmbGodown).DisplayLayout.Bands[0].Columns[1].Width = 151;
				((UltraGridBase)cmbGodown).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYearbyWarehouseIdandParentCategoryId(int WarehouseId, int ParentCategoryId)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		try
		{
			((Control)(object)CmbCropyr).Text = string.Empty;
			ReportsParameters val = new ReportsParameters
			{
				Activity = "GetAllCropYearsByByParentCategoryWarehouses",
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WarehouseId = WarehouseId,
				ItemCategoryId = ParentCategoryId
			};
			DataTable dt = new DataTable
			{
				Columns = { "Id", "Name" }
			};
			DataTable dtCropYear = InvStockConversion.ItemStockFilterFromInventoryTransaction(val);
			if (dtCropYear.Rows.Count > 0)
			{
				dt.Rows.Clear();
				for (int i = 0; i < dtCropYear.Rows.Count; i++)
				{
					dt.Rows.Add(dtCropYear.Rows[i]["CropYear"], dtCropYear.Rows[i]["CropYear"]);
				}
				DDL.BindDDL(dt, CmbCropyr, "Id", "Name", "CropYear", ZeroIndex: false);
				((UltraGridBase)CmbCropyr).DisplayLayout.Bands[0].Columns[1].Width = 84;
				GetCurrentItemStockByWarehouseByJobLotAndCropYear();
				((UltraGridBase)CmbCropyr).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAllItemsByCropByWarehouseByInventoryParentCategory(int ParentCategoryId, int WarehouseId, string CropYear)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		try
		{
			((Control)(object)cmbItem).Text = string.Empty;
			ReportsParameters val = new ReportsParameters
			{
				Activity = "GetAllItemsByCropByWarehouseByInventoryParentCategory",
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WarehouseId = WarehouseId,
				ItemCategoryId = ParentCategoryId,
				CropYear = CropYear
			};
			DataTable dt = new DataTable
			{
				Columns = { "Id", "Name" }
			};
			DataTable dtItem = InvStockConversion.ItemStockFilterFromInventoryTransaction(val);
			if (dtItem.Rows.Count > 0)
			{
				dt.Rows.Clear();
				for (int i = 0; i < dtItem.Rows.Count; i++)
				{
					dt.Rows.Add(dtItem.Rows[i]["Id"], dtItem.Rows[i]["ItemName"]);
				}
				DDL.BindDDL(dt, cmbItem, "Id", "Name", "Item Name", ZeroIndex: false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 200;
				GetCurrentItemStockByWarehouseByJobLotAndCropYear();
				((UltraGridBase)cmbItem).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAllLotsByItemByCropByWarehouseByInventoryParentCategory(int ParentCategoryId, int WarehouseId, string CropYear, int ItemId)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		try
		{
			((Control)(object)cmbLot).Text = string.Empty;
			ReportsParameters val = new ReportsParameters
			{
				Activity = "GetAllLotsByItemByCropByWarehouseByInventoryParentCategory",
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WarehouseId = WarehouseId,
				ItemCategoryId = ParentCategoryId,
				CropYear = CropYear,
				ItemId = ItemId
			};
			DataTable dt = new DataTable
			{
				Columns = { "Id", "Name" }
			};
			DataTable dtLot = InvStockConversion.ItemStockFilterFromInventoryTransaction(val);
			if (dtLot.Rows.Count > 0)
			{
				dt.Rows.Clear();
				for (int i = 0; i < dtLot.Rows.Count; i++)
				{
					dt.Rows.Add(dtLot.Rows[i]["Id"], dtLot.Rows[i]["JobLotDescription"]);
				}
				DDL.BindDDL(dt, cmbLot, "Id", "Name", "Job Lot", ZeroIndex: false);
				((UltraGridBase)cmbLot).DisplayLayout.Bands[0].Columns[1].Width = 200;
				GetCurrentItemStockByWarehouseByJobLotAndCropYear();
				((UltraGridBase)cmbLot).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetCurrentItemStockByWarehouseByJobLotAndCropYear()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		try
		{
			DataTable dtGetCurrentItemStock = InvStockConversion.ItemStockFilterFromInventoryTransaction(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemCategoryId = Conversion.ToInt(cmbParentCategory.Value),
				WarehouseId = Conversion.ToInt(cmbGodown.Value),
				ItemId = Conversion.ToInt(cmbItem.Value),
				JobLotId = Conversion.ToInt(cmbLot.Value),
				ToDate = DateTime.Now,
				CropYear = Conversion.ToString((object)((Control)(object)CmbCropyr).Text),
				Activity = "GetCurrentItemStockByWarehouseByJobLotAndCropYear"
			});
			if (dtGetCurrentItemStock.Rows.Count > 0)
			{
				txtStock.Text = Conversion.ToString(dtGetCurrentItemStock.Rows[0]["AvailableItemStock"]);
			}
			else
			{
				txtStock.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MoistureSlabFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtMoistureSlab = MoistureSlab.MoistureSlabGetAll(obj);
			if (dtMoistureSlab.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtMoistureSlab, cmbMoistureSlab, "Id", "MoistureSlabDescription", "Moisture Slab", false);
				((UltraGridBase)cmbMoistureSlab).DisplayLayout.Bands[0].Columns[1].Width = 130;
			}
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

	private void invfrmStockConversionProduction_KeyDown(object sender, KeyEventArgs e)
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
		if (e.Control && e.KeyCode == Keys.U && UpdateMode)
		{
			btnUpdate_Click(null, null);
		}
	}

	private void GenerateDocNumber()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		try
		{
			int code = InvStockConversion.GetGenerateCode(new InvStockConversion
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				DocTypeId = 79
			});
			if (code > 0)
			{
				txtdocnumber.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Expected O, but got Unknown
		//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Expected O, but got Unknown
		//IL_0886: Unknown result type (might be due to invalid IL or missing references)
		//IL_088d: Expected O, but got Unknown
		//IL_0a98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9f: Expected O, but got Unknown
		//IL_0c50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c93: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!FormValidation())
			{
				return;
			}
			GridEXRow[] rows = grdPackingMaterial.GetRows();
			foreach (GridEXRow r6 in rows)
			{
				if (Conversion.ToDouble(r6.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r6.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Item Against Expense First");
					return;
				}
			}
			rows = grdOverHead.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				if (Conversion.ToDouble(r5.Cells["ExpAmount"].Value) > 0.0 && Conversion.ToInt(r5.Cells["ChartofAccount"].Value) == 0)
				{
					MessageBox.Show("Please Select Account Title First");
					return;
				}
			}
			bool falag = false;
			rows = grdStockConversionProductionDetail.GetRows();
			for (int i = 0; i < rows.Length; i++)
			{
				if (rows[i].Cells["EntryType"].Text == "Issue")
				{
					falag = true;
				}
			}
			if (!falag)
			{
				MessageBox.Show("Issue is Required in Detail Grid");
				return;
			}
			InvStockConversion stockConversion = new InvStockConversion();
			stockConversion.DocSrNo = Conversion.ToInt((object)txtdocnumber.Text);
			stockConversion.DocDate = Conversion.ToDateTime((object)txtDocdate.Value);
			stockConversion.DocTypeId = 79;
			stockConversion.ProductionNo = Conversion.ToString((object)txtProductionNo.Text);
			stockConversion.DocManualRef = string.Empty;
			stockConversion.Remarks = Conversion.ToString((object)txtRemarks.Text);
			stockConversion.OrganizationId = UserAccount.OrganizationId;
			stockConversion.CompanyId = UserAccount.CompanyId;
			stockConversion.EntryUser = UserAccount.ID;
			stockConversion.EntryDate = DateTime.Now;
			stockConversion.ModifyDate = DateTime.Now;
			stockConversion.parentCategoryId = Conversion.ToInt(cmbParentCategory.Value);
			stockConversion.invStockConversionDetails = new List<InvStockConversionDetail>();
			stockConversion.invStockConversionAddExpenses = new List<InvStockConversionAddExpense>();
			stockConversion.invStockConversionPackings = new List<InvStockConversionPackingMaterial>();
			rows = grdStockConversionProductionDetail.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				if (Conversion.ToInt((object)r4.Cells["ItemId"].Value.ToString()) != 0)
				{
					InvStockConversionDetail Conversiondetail3 = new InvStockConversionDetail();
					Conversiondetail3.RefDocumentTypeId = Conversion.ToInt(r4.Cells["RefDocumentTypeId"].Value);
					Conversiondetail3.RefDocNoId = Conversion.ToInt(r4.Cells["RefDocNoId"].Value);
					Conversiondetail3.RefDocSubId = Conversion.ToInt(r4.Cells["RefDocSubId"].Value);
					Conversiondetail3.EntryType = Conversion.ToString((object)r4.Cells["EntryType"].Text);
					Conversiondetail3.WarehouseId = Conversion.ToInt(r4.Cells["WareHouseId"].Value);
					Conversiondetail3.ItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
					Conversiondetail3.ItemUomId = Conversion.ToInt(r4.Cells["ItemUOMId"].Value);
					Conversiondetail3.CropBatch = Conversion.ToString((object)r4.Cells["CropYear"].Text);
					Conversiondetail3.JobLotId = Conversion.ToInt(r4.Cells["JobLotId"].Value);
					Conversiondetail3.PackingtypeId = Conversion.ToInt(r4.Cells["PackingTypeId"].Value);
					Conversiondetail3.Qty = Conversion.ToDouble(r4.Cells["Quantity"].Value);
					if (Conversiondetail3.Qty == 0.0)
					{
						MessageBox.Show("Qty field Required");
					}
					Conversiondetail3.Weight = Conversion.ToSingle(r4.Cells["Weight"].Value);
					if (Conversiondetail3.Weight == 0.0)
					{
						MessageBox.Show("Weight field Required");
					}
					Conversiondetail3.Rate = Conversion.ToSingle(r4.Cells["Rate"].Value);
					if (Conversiondetail3.Rate == 0.0)
					{
						MessageBox.Show("ItemRate field Required");
					}
					Conversiondetail3.RateUOMId = Conversion.ToInt(r4.Cells["RateUOMId"].Value);
					Conversiondetail3.Amount = Conversion.ToSingle(r4.Cells["Amount"].Value);
					Conversiondetail3.Moisture = Conversion.ToSingle(r4.Cells["Moisture"].Value);
					Conversiondetail3.MoistureSlabId = Conversion.ToInt(r4.Cells["MoistureSlabId"].Value);
					Conversiondetail3.ProjectId = 0;
					Conversiondetail3.VoucherHeadId = 0;
					Conversiondetail3.PackUnit = 0;
					Conversiondetail3.Remarks = Conversion.ToString((object)r4.Cells["Remarks"].Text);
					stockConversion.invStockConversionDetails.Add(Conversiondetail3);
				}
			}
			rows = grdByProduct.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToInt((object)r3.Cells["ItemId"].Value.ToString()) != 0)
				{
					InvStockConversionDetail Conversiondetail2 = new InvStockConversionDetail();
					Conversiondetail2.EntryType = Conversion.ToString((object)r3.Cells["EntryType"].Text);
					Conversiondetail2.WarehouseId = Conversion.ToInt(r3.Cells["WareHouseId"].Value);
					Conversiondetail2.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
					Conversiondetail2.ItemUomId = Conversion.ToInt(r3.Cells["ItemUOMId"].Value);
					Conversiondetail2.CropBatch = Conversion.ToString((object)r3.Cells["CropYear"].Text);
					Conversiondetail2.JobLotId = Conversion.ToInt(r3.Cells["JobLotId"].Value);
					Conversiondetail2.PackingtypeId = Conversion.ToInt(r3.Cells["PackingTypeId"].Value);
					Conversiondetail2.Qty = Conversion.ToDouble(r3.Cells["Quantity"].Value);
					Conversiondetail2.Weight = Conversion.ToSingle(r3.Cells["Weight"].Value);
					Conversiondetail2.Rate = Conversion.ToSingle(r3.Cells["Rate"].Value);
					Conversiondetail2.RateUOMId = Conversion.ToInt(r3.Cells["RateUOMId"].Value);
					Conversiondetail2.Moisture = Conversion.ToInt((object)r3.Cells["Moisture"].Text);
					Conversiondetail2.MoistureSlabId = Conversion.ToInt((object)r3.Cells["MoistureSlabId"].Text);
					Conversiondetail2.Amount = Conversion.ToSingle(r3.Cells["Amount"].Value);
					Conversiondetail2.ExpenseAmount = Conversion.ToSingle(r3.Cells["ExpAmount"].Value);
					Conversiondetail2.PackingMaterialAmount = Conversion.ToSingle(r3.Cells["PMAmount"].Value);
					Conversiondetail2.ProjectId = 0;
					Conversiondetail2.VoucherHeadId = 0;
					Conversiondetail2.PackUnit = 0;
					Conversiondetail2.Remarks = Conversion.ToString((object)r3.Cells["Remarks"].Text);
					stockConversion.invStockConversionDetails.Add(Conversiondetail2);
				}
			}
			rows = grdPackingMaterial.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt((object)r2.Cells["ItemId"].Value.ToString()) != 0)
				{
					InvStockConversionPackingMaterial Conversiondetail = new InvStockConversionPackingMaterial();
					if (!(r2.Cells["ItemId"].Text != string.Empty))
					{
						MessageBox.Show("Item Name Field Require");
						return;
					}
					Conversiondetail.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					if (!(r2.Cells["ItemQTY"].Text != string.Empty))
					{
						MessageBox.Show("Item Qty Field Require");
						return;
					}
					Conversiondetail.ItemQty = Conversion.ToInt(r2.Cells["ItemQTY"].Value);
					if (!(r2.Cells["ItemRate"].Text != string.Empty))
					{
						MessageBox.Show("Item Rate Field Require");
						return;
					}
					Conversiondetail.ItemRate = Conversion.ToDouble((object)r2.Cells["ItemRate"].Text);
					if (!(r2.Cells["Amount"].Text != string.Empty))
					{
						MessageBox.Show("Amount Field Require ");
						return;
					}
					Conversiondetail.ItemAmount = Conversion.ToDouble((object)r2.Cells["Amount"].Text);
					if (!(r2.Cells["ChargeTo"].Text != string.Empty))
					{
						MessageBox.Show("Charge To is Required");
						return;
					}
					Conversiondetail.ChargeTo = Conversion.ToString((object)r2.Cells["ChargeTo"].Text);
					stockConversion.invStockConversionPackings.Add(Conversiondetail);
				}
			}
			rows = grdOverHead.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt((object)r.Cells["ChartofAccount"].Value.ToString()) != 0)
				{
					InvStockConversionAddExpense Expensedetail = new InvStockConversionAddExpense();
					if (!(r.Cells["ChartofAccount"].Text != string.Empty))
					{
						MessageBox.Show("ChartofAccount Field Require ");
						return;
					}
					Expensedetail.ChartOfAccountId = Conversion.ToInt(r.Cells["ChartofAccount"].Value);
					Expensedetail.LedgerRemarks = Conversion.ToString((object)r.Cells["LedgerRemarks"].Text);
					if (!(r.Cells["ExpAmount"].Text != string.Empty))
					{
						MessageBox.Show("ExpAmount Field Require ");
						return;
					}
					Expensedetail.ExpAmount = Conversion.ToDouble((object)r.Cells["ExpAmount"].Text);
					if (!(r.Cells["ChargeTo"].Text != string.Empty))
					{
						MessageBox.Show("Charge To is Required");
						return;
					}
					Expensedetail.ChargeTo = Conversion.ToString((object)r.Cells["ChargeTo"].Text);
					stockConversion.invStockConversionAddExpenses.Add(Expensedetail);
				}
			}
			CheckExpenseandPackingMaterialAmountTotal();
			if (checkPackingMaterialAmountTotal)
			{
				MessageBox.Show("Packing Material Amount not Equal By Product PMAmount");
				return;
			}
			if (CheckExpenseAmountTotal)
			{
				MessageBox.Show("Expenses Amount not Equal ByProduct Expenese Amount");
				return;
			}
			int code = InvStockConversion.Save(stockConversion);
			if (code > 0)
			{
				MessageBox.Show("Save Successfully");
				RefreshForm();
				grdStockConversionProductionDetail.ClearStructure();
				summeryreset();
			}
			if (chkPrint.Checked)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
				val.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
				val.RptPerameter("@hId", code.ToString());
				val.ShowReport("RptInvProductionJobOrderSlipA.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Expected O, but got Unknown
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Expected O, but got Unknown
		//IL_0782: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Expected O, but got Unknown
		//IL_098f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0996: Expected O, but got Unknown
		//IL_0b39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!FormValidation())
			{
				return;
			}
			GridEXRow[] rows = grdPackingMaterial.GetRows();
			foreach (GridEXRow r6 in rows)
			{
				if (Conversion.ToDouble(r6.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r6.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Item Against Expense First");
					return;
				}
			}
			rows = grdOverHead.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				if (Conversion.ToDouble(r5.Cells["ExpAmount"].Value) > 0.0 && Conversion.ToInt(r5.Cells["ChartofAccount"].Value) == 0)
				{
					MessageBox.Show("Please Select Account Title First");
					return;
				}
			}
			InvStockConversion stockConversion = new InvStockConversion();
			stockConversion.Id = RecId;
			stockConversion.CompanyId = UserAccount.CompanyId;
			stockConversion.OrganizationId = UserAccount.OrganizationId;
			stockConversion.DocSrNo = Conversion.ToInt((object)txtdocnumber.Text);
			stockConversion.DocDate = Conversion.ToDateTime((object)txtDocdate.Value);
			stockConversion.DocTypeId = 79;
			stockConversion.ProductionNo = Conversion.ToString((object)txtProductionNo.Text);
			stockConversion.DocManualRef = string.Empty;
			stockConversion.Remarks = Conversion.ToString((object)txtRemarks.Text);
			stockConversion.ModifyDate = DateTime.Now;
			stockConversion.EntryDate = DateTime.Now;
			stockConversion.EntryUser = UserAccount.ID;
			stockConversion.parentCategoryId = Conversion.ToInt(cmbParentCategory.Value);
			stockConversion.invStockConversionDetails = new List<InvStockConversionDetail>();
			stockConversion.invStockConversionPackings = new List<InvStockConversionPackingMaterial>();
			stockConversion.invStockConversionAddExpenses = new List<InvStockConversionAddExpense>();
			rows = grdStockConversionProductionDetail.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				InvStockConversionDetail Conversiondetail3 = new InvStockConversionDetail();
				Conversiondetail3.RefDocumentTypeId = Conversion.ToInt(r4.Cells["RefDocumentTypeId"].Value);
				Conversiondetail3.RefDocNoId = Conversion.ToInt(r4.Cells["RefDocNoId"].Value);
				Conversiondetail3.RefDocSubId = Conversion.ToInt(r4.Cells["RefDocSubId"].Value);
				Conversiondetail3.EntryType = Conversion.ToString((object)r4.Cells["EntryType"].Text);
				Conversiondetail3.WarehouseId = Conversion.ToInt(r4.Cells["WareHouseId"].Value);
				Conversiondetail3.ItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
				Conversiondetail3.ItemUomId = Conversion.ToInt(r4.Cells["ItemUOMId"].Value);
				Conversiondetail3.CropBatch = Conversion.ToString((object)r4.Cells["CropYear"].Text);
				Conversiondetail3.JobLotId = Conversion.ToInt(r4.Cells["JobLotId"].Value);
				Conversiondetail3.PackingtypeId = Conversion.ToInt(r4.Cells["PackingTypeId"].Value);
				Conversiondetail3.Qty = Conversion.ToDouble(r4.Cells["Quantity"].Value);
				Conversiondetail3.Weight = Conversion.ToSingle(r4.Cells["Weight"].Value);
				Conversiondetail3.Rate = Conversion.ToSingle(r4.Cells["Rate"].Value);
				Conversiondetail3.RateUOMId = Conversion.ToInt(r4.Cells["RateUOMId"].Value);
				Conversiondetail3.Amount = Conversion.ToSingle(r4.Cells["Amount"].Value);
				Conversiondetail3.Moisture = Conversion.ToSingle(r4.Cells["Moisture"].Value);
				Conversiondetail3.MoistureSlabId = Conversion.ToInt(r4.Cells["MoistureSlabId"].Value);
				Conversiondetail3.ProjectId = 0;
				Conversiondetail3.VoucherHeadId = 0;
				Conversiondetail3.PackUnit = 0;
				Conversiondetail3.Remarks = Conversion.ToString((object)r4.Cells["Remarks"].Text);
				stockConversion.invStockConversionDetails.Add(Conversiondetail3);
			}
			rows = grdByProduct.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				InvStockConversionDetail Conversiondetail2 = new InvStockConversionDetail();
				Conversiondetail2.EntryType = Conversion.ToString((object)r3.Cells["EntryType"].Text);
				Conversiondetail2.WarehouseId = Conversion.ToInt(r3.Cells["WareHouseId"].Value);
				Conversiondetail2.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
				Conversiondetail2.ItemUomId = Conversion.ToInt(r3.Cells["ItemUOMId"].Value);
				Conversiondetail2.CropBatch = Conversion.ToString((object)r3.Cells["CropYear"].Text);
				Conversiondetail2.JobLotId = Conversion.ToInt(r3.Cells["JobLotId"].Value);
				Conversiondetail2.PackingtypeId = Conversion.ToInt(r3.Cells["PackingTypeId"].Value);
				Conversiondetail2.Qty = Conversion.ToDouble(r3.Cells["Quantity"].Value);
				Conversiondetail2.Weight = Conversion.ToSingle(r3.Cells["Weight"].Value);
				Conversiondetail2.Rate = Conversion.ToSingle(r3.Cells["Rate"].Value);
				Conversiondetail2.RateUOMId = Conversion.ToInt(r3.Cells["RateUOMId"].Value);
				Conversiondetail2.Moisture = Conversion.ToInt((object)r3.Cells["Moisture"].Text);
				Conversiondetail2.MoistureSlabId = Conversion.ToInt((object)r3.Cells["MoistureSlabId"].Text);
				Conversiondetail2.Amount = Conversion.ToSingle(r3.Cells["Amount"].Value);
				Conversiondetail2.ExpenseAmount = Conversion.ToSingle(r3.Cells["ExpAmount"].Value);
				Conversiondetail2.PackingMaterialAmount = Conversion.ToSingle(r3.Cells["PMAmount"].Value);
				Conversiondetail2.ProjectId = 0;
				Conversiondetail2.VoucherHeadId = 0;
				Conversiondetail2.PackUnit = 0;
				Conversiondetail2.Remarks = Conversion.ToString((object)r3.Cells["Remarks"].Text);
				stockConversion.invStockConversionDetails.Add(Conversiondetail2);
			}
			rows = grdPackingMaterial.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt((object)r2.Cells["ItemId"].Value.ToString()) != 0)
				{
					InvStockConversionPackingMaterial Conversiondetail = new InvStockConversionPackingMaterial();
					if (!(r2.Cells["ItemId"].Text != string.Empty))
					{
						MessageBox.Show("Item Name Field Require");
						return;
					}
					Conversiondetail.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					if (!(r2.Cells["ItemQTY"].Text != string.Empty))
					{
						MessageBox.Show("Item Qty Field Require");
						return;
					}
					Conversiondetail.ItemQty = Conversion.ToInt((object)r2.Cells["ItemQTY"].Text);
					if (!(r2.Cells["ItemRate"].Text != string.Empty))
					{
						MessageBox.Show("Item Rate Field Require");
						return;
					}
					Conversiondetail.ItemRate = Conversion.ToDouble((object)r2.Cells["ItemRate"].Text);
					if (!(r2.Cells["Amount"].Text != string.Empty))
					{
						MessageBox.Show("Amount Field Require ");
						return;
					}
					Conversiondetail.ItemAmount = Conversion.ToDouble((object)r2.Cells["Amount"].Text);
					if (!(r2.Cells["ChargeTo"].Text != string.Empty))
					{
						MessageBox.Show("Charge To is Required");
						return;
					}
					Conversiondetail.ChargeTo = Conversion.ToString((object)r2.Cells["ChargeTo"].Text);
					stockConversion.invStockConversionPackings.Add(Conversiondetail);
				}
			}
			rows = grdOverHead.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt((object)r.Cells["ChartofAccount"].Value.ToString()) != 0)
				{
					InvStockConversionAddExpense Expensedetail = new InvStockConversionAddExpense();
					if (!(r.Cells["ChartofAccount"].Text != string.Empty))
					{
						MessageBox.Show("ChartofAccount Field Require ");
						return;
					}
					Expensedetail.ChartOfAccountId = Conversion.ToInt(r.Cells["ChartofAccount"].Value);
					Expensedetail.LedgerRemarks = Conversion.ToString((object)r.Cells["LedgerRemarks"].Text);
					if (!(r.Cells["ExpAmount"].Text != string.Empty))
					{
						MessageBox.Show("ExpAmount Field Require ");
						return;
					}
					Expensedetail.ExpAmount = Conversion.ToDouble((object)r.Cells["ExpAmount"].Text);
					if (!(r.Cells["ChargeTo"].Text != string.Empty))
					{
						MessageBox.Show("Charge To is Required");
						return;
					}
					Expensedetail.ChargeTo = Conversion.ToString((object)r.Cells["ChargeTo"].Text);
					stockConversion.invStockConversionAddExpenses.Add(Expensedetail);
				}
			}
			CheckExpenseandPackingMaterialAmountTotal();
			if (checkPackingMaterialAmountTotal)
			{
				MessageBox.Show("Packing Material Amount not Equal By Product PMAmount");
				return;
			}
			if (CheckExpenseAmountTotal)
			{
				MessageBox.Show("Expenses Amount not Equal ByProduct Expenese Amount");
				return;
			}
			int code = InvStockConversion.Save(stockConversion);
			if (code > 0)
			{
				MessageBox.Show("Update Successfully");
			}
			RefreshForm();
			summeryreset();
			if (chkPrint.Checked)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
				val.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
				val.RptPerameter("@hId", code.ToString());
				val.ShowReport("RptInvProductionJobOrderSlipA.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdStockConversionProductionDetail_DoubleClick(object sender, EventArgs e)
	{
	}

	private void DatagridHistory_DoubleClick(object sender, EventArgs e)
	{
		ReadById(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells[0].Value.ToString()));
		GenerateSummaryForUser();
	}

	public void ReadById(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvStockConversion SC = new InvStockConversion();
			RecId = Id;
			SC = InvStockConversion.GetById(RecId);
			tabControl1.SelectedIndex = 0;
			if (SC != null)
			{
				txtdocnumber.Text = Conversion.ToString((object)SC.DocSrNo);
				txtProductionNo.Text = Conversion.ToString((object)SC.ProductionNo);
				txtDocdate.Value = Conversion.ToDateTime((object)SC.DocDate);
				txtRemarks.Text = Conversion.ToString((object)SC.Remarks);
				cmbParentCategory.Value = Conversion.ToInt((object)SC.parentCategoryId);
				dtPackingMaterial.Clear();
				if (SC.invStockConversionPackings.Count > 0)
				{
					for (int k = 0; k < SC.invStockConversionPackings.Count; k++)
					{
						dtPackingMaterial.Rows.Add(SC.invStockConversionPackings[k].ItemId, SC.invStockConversionPackings[k].ItemQty, SC.invStockConversionPackings[k].ItemRate, SC.invStockConversionPackings[k].ItemAmount, SC.invStockConversionPackings[k].ChargeTo);
					}
				}
				else
				{
					AddRowInPackingMaterialGrid();
				}
				grdPackingMaterial.DataSource = dtPackingMaterial;
				grdPackingMaterial.RetrieveStructure();
				grdStockConversionProductionDetail.DataSource = table;
				grdStockConversionProductionDetail.RetrieveStructure();
				grdByProduct.DataSource = tableByProduct;
				grdByProduct.RetrieveStructure();
				grdPackingMaterialSettings();
				btnUpdate.Visible = true;
				btnsave.Visible = false;
				gridsettings();
				gridsettingsByProduct();
				dtExpense.Clear();
				if (SC.invStockConversionAddExpenses.Count > 0)
				{
					for (int j = 0; j < SC.invStockConversionAddExpenses.Count; j++)
					{
						dtExpense.Rows.Add(SC.invStockConversionAddExpenses[j].ChartOfAccountId, SC.invStockConversionAddExpenses[j].LedgerRemarks, SC.invStockConversionAddExpenses[j].ExpAmount, SC.invStockConversionAddExpenses[j].ChargeTo);
					}
				}
				else
				{
					AddRowInExpenseGrid();
				}
				grdOverHead.DataSource = dtExpense;
				grdOverHead.RetrieveStructure();
				grdConversionExpenseSettings();
				proportionatedPackingMaterialAmountInOutputGrid();
				proportionateOverheadGrid();
				btnGenerate_Click(null, null);
				table.Clear();
				tableByProduct.Clear();
				double AmountWithoutExpense = 0.0;
				for (int i = 0; i < SC.invStockConversionDetails.Count; i++)
				{
					if (SC.invStockConversionDetails[i].EntryType == "Issue")
					{
						table.Rows.Add(SC.invStockConversionDetails[i].RefDocumentTypeId, SC.invStockConversionDetails[i].RefDocNoId, SC.invStockConversionDetails[i].RefDocSubId, SC.invStockConversionDetails[i].EntryType, SC.invStockConversionDetails[i].WarehouseId, SC.invStockConversionDetails[i].WareHouseName, SC.invStockConversionDetails[i].ItemId, SC.invStockConversionDetails[i].ItemName, SC.invStockConversionDetails[i].ItemUomId, SC.invStockConversionDetails[i].UOMDescription, SC.invStockConversionDetails[i].CropBatch, SC.invStockConversionDetails[i].JobLotId, SC.invStockConversionDetails[i].JobLotDescription, SC.invStockConversionDetails[i].PackingtypeId, SC.invStockConversionDetails[i].PackTypeDesc, 0, 0, SC.invStockConversionDetails[i].Qty, SC.invStockConversionDetails[i].Weight, SC.invStockConversionDetails[i].Rate, SC.invStockConversionDetails[i].RateUOMId, SC.invStockConversionDetails[i].Equivalent, SC.invStockConversionDetails[i].Amount, SC.invStockConversionDetails[i].Moisture, SC.invStockConversionDetails[i].MoistureSlabId, SC.invStockConversionDetails[i].Remarks);
					}
					else if (SC.invStockConversionDetails[i].EntryType != "Issue")
					{
						AmountWithoutExpense = SC.invStockConversionDetails[i].Amount - (SC.invStockConversionDetails[i].PackingMaterialAmount + SC.invStockConversionDetails[i].ExpenseAmount);
						tableByProduct.Rows.Add(SC.invStockConversionDetails[i].EntryType, SC.invStockConversionDetails[i].WarehouseId, SC.invStockConversionDetails[i].WareHouseName, SC.invStockConversionDetails[i].ItemId, SC.invStockConversionDetails[i].ItemName, SC.invStockConversionDetails[i].ItemUomId, SC.invStockConversionDetails[i].UOMDescription, SC.invStockConversionDetails[i].CropBatch, SC.invStockConversionDetails[i].JobLotId, SC.invStockConversionDetails[i].JobLotDescription, SC.invStockConversionDetails[i].PackingtypeId, SC.invStockConversionDetails[i].PackTypeDesc, SC.invStockConversionDetails[i].Qty, SC.invStockConversionDetails[i].Weight, SC.invStockConversionDetails[i].Rate, SC.invStockConversionDetails[i].RateUOMId, SC.invStockConversionDetails[i].Equivalent, AmountWithoutExpense, SC.invStockConversionDetails[i].Moisture, SC.invStockConversionDetails[i].MoistureSlabId, SC.invStockConversionDetails[i].MoistureSlabDescription, SC.invStockConversionDetails[i].PackingMaterialAmount, SC.invStockConversionDetails[i].ExpenseAmount, SC.invStockConversionDetails[i].Amount, SC.invStockConversionDetails[i].Remarks);
					}
				}
			}
			else
			{
				dtExpense.Clear();
				grdOverHead.ClearStructure();
				grdDetail.ClearStructure();
				table.Clear();
				grdPackingMaterial.ClearStructure();
				dtPackingMaterial.Clear();
				summeryreset();
			}
			VoucherHeadIdGet();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		RefreshForm();
		resetDetail();
		summeryreset();
	}

	private bool FormValidation()
	{
		if (txtdocnumber.Text.Trim() == string.Empty || txtdocnumber.Text == "0" || txtdocnumber.Text == null)
		{
			MessageBox.Show("document Number Field Required");
			txtdocnumber.Focus();
			return false;
		}
		if (txtProductionNo.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Product No. Field Required");
			txtProductionNo.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationOfDetailPortion()
	{
		if (((Control)(object)cmbEntryType).Text == "Issue" && Conversion.ToDouble((object)txtStock.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("Stock Should Not b Less then or Equal to 0");
			txtStock.Focus();
			return false;
		}
		if (int.Parse(((UltraGridBase)cmbMoistureSlab).ActiveRow.Cells[0].Value.ToString()) == 0)
		{
			MessageBox.Show("Moisture Slab Field Required");
			cmbMoistureSlab.Focus();
			return false;
		}
		if (txtMoisture.Text == string.Empty)
		{
			MessageBox.Show("Moisture Field Required");
			txtMoisture.Focus();
			return false;
		}
		if (((Control)(object)cmbEntryType).Text == string.Empty)
		{
			MessageBox.Show("Entry Type Field Required");
			cmbEntryType.Focus();
			return false;
		}
		if (int.Parse(((UltraGridBase)cmbGodown).ActiveRow.Cells[0].Value.ToString()) == 0)
		{
			MessageBox.Show("Ware house Field Required");
			cmbGodown.Focus();
			return false;
		}
		if (int.Parse(((UltraGridBase)cmbItem).ActiveRow.Cells[0].Value.ToString()) == 0)
		{
			MessageBox.Show("Item Field Field Required");
			cmbItem.Focus();
			return false;
		}
		if (int.Parse(((UltraGridBase)cmbUOM).ActiveRow.Cells[0].Value.ToString()) == 0)
		{
			MessageBox.Show("UOM Field Required");
			cmbUOM.Focus();
			return false;
		}
		if (((Control)(object)CmbCropyr).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Crop Year Field Required");
			CmbCropyr.Focus();
			return false;
		}
		if (int.Parse(((UltraGridBase)cmbLot).ActiveRow.Cells[0].Value.ToString()) == 0)
		{
			MessageBox.Show("Lot Field Required");
			cmbLot.Focus();
			return false;
		}
		if (int.Parse(((UltraGridBase)cmbBagType).ActiveRow.Cells[0].Value.ToString()) == 0)
		{
			MessageBox.Show("Bag Type Field Required");
			cmbBagType.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == string.Empty || txtQty.Text.Trim() == "0")
		{
			MessageBox.Show("Bag Quantity Field Required");
			txtQty.Focus();
			return false;
		}
		if (txtUnitWeight.Text.Trim() == string.Empty || txtUnitWeight.Text.Trim() == "0")
		{
			MessageBox.Show("Unit Weight Field Required");
			txtUnitWeight.Focus();
			return false;
		}
		if (((Control)(object)cmbEntryType).Text != "Recovery Head Rice" && txtRate.Text == string.Empty)
		{
			MessageBox.Show("Rate  Field Required");
			txtRate.Focus();
			return false;
		}
		if (int.Parse(((UltraGridBase)cmbRateUom).ActiveRow.Cells[0].Value.ToString()) == 0)
		{
			MessageBox.Show("Rate UOM Field Required");
			cmbRateUom.Focus();
			return false;
		}
		if (txtAmount.Text == string.Empty)
		{
			MessageBox.Show("Amount  Field Required");
			txtAmount.Focus();
			return false;
		}
		return true;
	}

	private void AddInGrid_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortion())
			{
				if (((Control)(object)cmbEntryType).Text == "Issue")
				{
					table.Rows.Add(0, 0, 0, ((Control)(object)cmbEntryType).Text, cmbGodown.Value, ((Control)(object)cmbGodown).Text, cmbItem.Value, ((Control)(object)cmbItem).Text, cmbUOM.Value, ((Control)(object)cmbUOM).Text, ((Control)(object)CmbCropyr).Text, cmbLot.Value, ((Control)(object)cmbLot).Text, cmbBagType.Value, ((Control)(object)cmbBagType).Text, txtQty.Text, txtUnitWeight.Text, txtRate.Text, cmbRateUom.Value, ((Control)(object)cmbRateUom).Text, txtAmount.Text, txtMoisture.Text.Trim(), cmbMoistureSlab.Value, txtdeailRemarks.Text);
					grdStockConversionProductionDetail.DataSource = table;
					grdStockConversionProductionDetail.RetrieveStructure();
					gridsettings();
					resetDetail();
					DetailEditMood = false;
				}
				else
				{
					tableByProduct.Rows.Add(((Control)(object)cmbEntryType).Text, cmbGodown.Value, ((Control)(object)cmbGodown).Text, cmbItem.Value, ((Control)(object)cmbItem).Text, cmbUOM.Value, ((Control)(object)cmbUOM).Text, ((Control)(object)CmbCropyr).Text, cmbLot.Value, ((Control)(object)cmbLot).Text, cmbBagType.Value, ((Control)(object)cmbBagType).Text, txtQty.Text, txtUnitWeight.Text, txtRate.Text, cmbRateUom.Value, ((Control)(object)cmbRateUom).Text, txtAmount.Text, txtMoisture.Text.Trim(), cmbMoistureSlab.Value, ((Control)(object)cmbMoistureSlab).Text, 0, 0, txtAmount.Text, txtdeailRemarks.Text);
					grdByProduct.DataSource = tableByProduct;
					grdByProduct.RetrieveStructure();
					gridsettingsByProduct();
					resetDetail();
				}
				proportionateOverheadGrid();
				proportionatedPackingMaterialAmountInOutputGrid();
				GenerateSummaryForUser();
				cmbEntryType.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsettings()
	{
		try
		{
			grdStockConversionProductionDetail.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocNoId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocSubId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["WareHouseId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["ItemId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["ItemUOMId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["JobLotId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["PackingTypeId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["RateUOMId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["CropYear"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["JobLot"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["PackingType"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocumentTypeId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocNoId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocSubId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["EntryType"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["WareHouseId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["Item"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["ItemUOMId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["UOM"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["BalQty"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["BalWeight"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["Quantity"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["Weight"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["RateUOMId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["MoistureSlabId"].ColumnType = (ColumnType)0;
			grdStockConversionProductionDetail.RootTable.Columns["MoistureSlabId"].HasValueList = true;
			grdStockConversionProductionDetail.RootTable.Columns["MoistureSlabId"].LimitToList = true;
			grdStockConversionProductionDetail.RootTable.Columns["MoistureSlabId"].EditType = (EditType)4;
			grdStockConversionProductionDetail.RootTable.Columns["MoistureSlabId"].Caption = "MoistureSlabDescription";
			grdStockConversionProductionDetail.RootTable.Columns["MoistureSlabId"].ValueList.PopulateValueList((IEnumerable)dtMoistureSlab.DefaultView, "Id", "MoistureSlabDescription");
			grdStockConversionProductionDetail.RootTable.Columns["MoistureSlabId"].Width = 150;
			grdStockConversionProductionDetail.RootTable.Columns["EntryType"].Width = 150;
			grdStockConversionProductionDetail.RootTable.Columns["WareHouse"].Width = 120;
			grdStockConversionProductionDetail.RootTable.Columns["Item"].Width = 300;
			grdStockConversionProductionDetail.RootTable.Columns["UOM"].Width = 50;
			grdStockConversionProductionDetail.RootTable.Columns["CropYear"].Width = 80;
			grdStockConversionProductionDetail.RootTable.Columns["JobLot"].Width = 80;
			grdStockConversionProductionDetail.RootTable.Columns["PackingType"].Width = 120;
			grdStockConversionProductionDetail.RootTable.Columns["Quantity"].Width = 80;
			grdStockConversionProductionDetail.RootTable.Columns["Weight"].Width = 80;
			grdStockConversionProductionDetail.RootTable.Columns["Rate"].Width = 80;
			grdStockConversionProductionDetail.RootTable.Columns["RateUOM"].Width = 50;
			grdStockConversionProductionDetail.RootTable.Columns["Amount"].Width = 120;
			grdStockConversionProductionDetail.RootTable.Columns["Remarks"].Width = 200;
			grdStockConversionProductionDetail.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdStockConversionProductionDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdStockConversionProductionDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdStockConversionProductionDetail.RootTable.Columns["Quantity"].FormatString = "#,##";
			grdStockConversionProductionDetail.RootTable.Columns["Weight"].FormatString = "#,##";
			grdStockConversionProductionDetail.RootTable.Columns["Rate"].FormatString = "#.##";
			grdStockConversionProductionDetail.RootTable.Columns["Amount"].FormatString = "#,##";
			grdStockConversionProductionDetail.RootTable.Columns["Quantity"].TotalFormatString = "#,##";
			grdStockConversionProductionDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##";
			grdStockConversionProductionDetail.RootTable.Columns["Rate"].TotalFormatString = "#.##";
			grdStockConversionProductionDetail.RootTable.Columns["Amount"].TotalFormatString = "#,##";
			grdStockConversionProductionDetail.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdStockConversionProductionDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdStockConversionProductionDetail.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdStockConversionProductionDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsettingsByProduct()
	{
		try
		{
			grdByProduct.RootTable.Columns["MoistureSlabId"].Visible = false;
			grdByProduct.RootTable.Columns["WareHouseId"].Visible = false;
			grdByProduct.RootTable.Columns["ItemId"].Visible = false;
			grdByProduct.RootTable.Columns["ItemUOMId"].Visible = false;
			grdByProduct.RootTable.Columns["JobLotId"].Visible = false;
			grdByProduct.RootTable.Columns["PackingTypeId"].Visible = false;
			grdByProduct.RootTable.Columns["RateUOMId"].Visible = false;
			grdByProduct.RootTable.Columns["CropYear"].Visible = false;
			grdByProduct.RootTable.Columns["JobLot"].Visible = false;
			grdByProduct.RootTable.Columns["PackingType"].Visible = false;
			grdByProduct.RootTable.Columns["Moisture"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["MoistureSlabDescription"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["MoistureSlabId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["WareHouseId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ItemUOMId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["RateUOMId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["EntryType"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Item"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["UOM"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Quantity"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Weight"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ExpAmount"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["PMAmount"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["AmountWithoutExpenses"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Moisture"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["MoistureSlabDescription"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["EntryType"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["WareHouse"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["UOM"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["CropYear"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["JobLot"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["PackingType"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Quantity"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Rate"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["RateUOM"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["ExpAmount"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["PMAmount"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["AmountWithoutExpenses"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["EntryType"].Width = 150;
			grdByProduct.RootTable.Columns["WareHouse"].Width = 120;
			grdByProduct.RootTable.Columns["Item"].Width = 300;
			grdByProduct.RootTable.Columns["UOM"].Width = 50;
			grdByProduct.RootTable.Columns["CropYear"].Width = 80;
			grdByProduct.RootTable.Columns["JobLot"].Width = 80;
			grdByProduct.RootTable.Columns["PackingType"].Width = 120;
			grdByProduct.RootTable.Columns["Quantity"].Width = 80;
			grdByProduct.RootTable.Columns["Weight"].Width = 80;
			grdByProduct.RootTable.Columns["Rate"].Width = 80;
			grdByProduct.RootTable.Columns["RateUOM"].Width = 50;
			grdByProduct.RootTable.Columns["Amount"].Width = 120;
			grdByProduct.RootTable.Columns["Remarks"].Width = 200;
			grdByProduct.RootTable.Columns["ExpAmount"].Width = 80;
			grdByProduct.RootTable.Columns["PMAmount"].Width = 80;
			grdByProduct.RootTable.Columns["AmountWithoutExpenses"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["PMAmount"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["ExpAmount"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["AmountWithoutExpenses"].FormatString = "#,##";
			grdByProduct.RootTable.Columns["Quantity"].FormatString = "#,##";
			grdByProduct.RootTable.Columns["Weight"].FormatString = "#0,##";
			grdByProduct.RootTable.Columns["Rate"].FormatString = "#0.##";
			grdByProduct.RootTable.Columns["Amount"].FormatString = "#0,##";
			grdByProduct.RootTable.Columns["PMAmount"].FormatString = "#0,##";
			grdByProduct.RootTable.Columns["ExpAmount"].FormatString = "#0,##";
			grdByProduct.RootTable.Columns["AmountWithoutExpenses"].TotalFormatString = "#,##";
			grdByProduct.RootTable.Columns["Quantity"].TotalFormatString = "#,##";
			grdByProduct.RootTable.Columns["Weight"].TotalFormatString = "#0,##";
			grdByProduct.RootTable.Columns["Rate"].TotalFormatString = "#0,##";
			grdByProduct.RootTable.Columns["Amount"].TotalFormatString = "#0,##";
			grdByProduct.RootTable.Columns["PMAmount"].TotalFormatString = "#0,##";
			grdByProduct.RootTable.Columns["ExpAmount"].TotalFormatString = "#0,##";
			grdByProduct.RootTable.Columns["AmountWithoutExpenses"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["PMAmount"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["ExpAmount"].TextAlignment = (TextAlignment)3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSearch_Click(object sender, EventArgs e)
	{
		GridHistory();
	}

	private void handleWeightAndAmountCalculation()
	{
		try
		{
			double RateUom = Conversion.ToDouble((object)((Control)(object)cmbRateUom).Text);
			double rate = Conversion.ToDouble((object)txtRate.Text);
			if (rate > 0.0 && RateUom > 0.0 && Conversion.ToDouble((object)txtUnitWeight.Text) > 0.0)
			{
				double NetAmount = Conversion.ToDouble((object)txtUnitWeight.Text) / RateUom * rate;
				txtAmount.Text = Conversion.ToDouble((object)NetAmount).ToString("0,0");
			}
			else
			{
				txtAmount.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbUOM_TextChanged(object sender, EventArgs e)
	{
		try
		{
			handleWeightAndAmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			double ItemUom = Conversion.ToDouble((object)((Control)(object)cmbUOM).Text);
			double qty = Conversion.ToDouble((object)txtQty.Text);
			if (ItemUom > 0.0 && qty > 0.0)
			{
				double NetWeight = ItemUom * qty;
				txtUnitWeight.Text = Conversion.ToDouble((object)NetWeight).ToString("0,0");
			}
			else
			{
				txtUnitWeight.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			handleWeightAndAmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbRateUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			handleWeightAndAmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtUnitWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			handleWeightAndAmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterialSettings()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			DataTable dt = Item.GetItemByItemTypeId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Ids = "14"
			});
			grdPackingMaterial.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdPackingMaterial.RootTable.Columns["ItemId"].HasValueList = true;
			grdPackingMaterial.RootTable.Columns["ItemId"].LimitToList = true;
			grdPackingMaterial.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdPackingMaterial.RootTable.Columns["ItemId"].Caption = "Item";
			grdPackingMaterial.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dt.DefaultView, "Id", "ItemName");
			grdPackingMaterial.RootTable.Columns["ItemId"].Width = 200;
			grdPackingMaterial.RootTable.Columns["ChargeTo"].ColumnType = (ColumnType)0;
			grdPackingMaterial.RootTable.Columns["ChargeTo"].HasValueList = true;
			grdPackingMaterial.RootTable.Columns["ChargeTo"].LimitToList = true;
			grdPackingMaterial.RootTable.Columns["ChargeTo"].EditType = (EditType)4;
			grdPackingMaterial.RootTable.Columns["ChargeTo"].Caption = "ChargeTo";
			grdPackingMaterial.RootTable.Columns["ChargeTo"].ValueList.PopulateValueList((IEnumerable)dtChargeTo.DefaultView, "Id", "ChargeTo");
			grdPackingMaterial.RootTable.Columns["ChargeTo"].Width = 200;
			grdPackingMaterial.RootTable.Columns.Add("Delete");
			grdPackingMaterial.RootTable.Columns["Delete"].Key = "Delete";
			grdPackingMaterial.RootTable.Columns["Delete"].Caption = "X";
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonText = "X";
			grdPackingMaterial.RootTable.Columns["Delete"].Width = 30;
			grdPackingMaterial.RootTable.Columns.Add("Add");
			grdPackingMaterial.RootTable.Columns["Add"].Key = "Add";
			grdPackingMaterial.RootTable.Columns["Add"].Caption = "+";
			grdPackingMaterial.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterial.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterial.RootTable.Columns["Add"].ButtonText = "+";
			grdPackingMaterial.RootTable.Columns["Add"].Width = 30;
			grdPackingMaterial.RootTable.Columns["ItemQTY"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterial.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterial.RootTable.Columns["ItemQTY"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["ItemQTY"].FormatString = "#,##";
			grdPackingMaterial.RootTable.Columns["ItemRate"].FormatString = "#.##";
			grdPackingMaterial.RootTable.Columns["Amount"].FormatString = "#,##";
			grdPackingMaterial.RootTable.Columns["ItemQTY"].TotalFormatString = "#,##";
			grdPackingMaterial.RootTable.Columns["ItemRate"].TotalFormatString = "#.##";
			grdPackingMaterial.RootTable.Columns["Amount"].TotalFormatString = "#,##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdConversionExpenseSettings()
	{
		try
		{
			grdOverHead.RootTable.Columns["ChartofAccount"].ColumnType = (ColumnType)0;
			grdOverHead.RootTable.Columns["ChartofAccount"].HasValueList = true;
			grdOverHead.RootTable.Columns["ChartofAccount"].LimitToList = true;
			grdOverHead.RootTable.Columns["ChartofAccount"].EditType = (EditType)4;
			grdOverHead.RootTable.Columns["ChartofAccount"].Caption = "Account Title";
			grdOverHead.RootTable.Columns["ChartofAccount"].ValueList.PopulateValueList((IEnumerable)dtAccounts.DefaultView, "Id", "AccountTitle");
			grdOverHead.RootTable.Columns["ChartofAccount"].Width = 200;
			grdOverHead.RootTable.Columns["ChargeTo"].ColumnType = (ColumnType)0;
			grdOverHead.RootTable.Columns["ChargeTo"].HasValueList = true;
			grdOverHead.RootTable.Columns["ChargeTo"].LimitToList = true;
			grdOverHead.RootTable.Columns["ChargeTo"].EditType = (EditType)4;
			grdOverHead.RootTable.Columns["ChargeTo"].Caption = "ChargeTo";
			grdOverHead.RootTable.Columns["ChargeTo"].ValueList.PopulateValueList((IEnumerable)dtChargeTo.DefaultView, "Id", "ChargeTo");
			grdOverHead.RootTable.Columns["ChargeTo"].Width = 200;
			grdOverHead.RootTable.Columns.Add("Delete");
			grdOverHead.RootTable.Columns["Delete"].Key = "Delete";
			grdOverHead.RootTable.Columns["Delete"].Caption = "X";
			grdOverHead.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHead.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOverHead.RootTable.Columns["Delete"].ButtonText = "X";
			grdOverHead.RootTable.Columns["Delete"].Width = 30;
			grdOverHead.RootTable.Columns.Add("Add");
			grdOverHead.RootTable.Columns["Add"].Key = "Add";
			grdOverHead.RootTable.Columns["Add"].Caption = "+";
			grdOverHead.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHead.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdOverHead.RootTable.Columns["Add"].ButtonText = "+";
			grdOverHead.RootTable.Columns["Add"].Width = 30;
			grdOverHead.RootTable.Columns["ExpAmount"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["ExpAmount"].AggregateFunction = (AggregateFunction)2;
			grdOverHead.RootTable.Columns["ExpAmount"].FormatString = "#,##";
			grdOverHead.RootTable.Columns["ExpAmount"].TotalFormatString = "#,##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInPackingMaterialGrid()
	{
		dtPackingMaterial.Rows.Add(0, 0, 0, 0);
		grdPackingMaterial.DataSource = dtPackingMaterial;
	}

	private void AddRowInExpenseGrid()
	{
		dtExpense.Rows.Add(0, 0, 0);
		grdOverHead.DataSource = dtExpense;
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
				}
				if (dtPackingMaterial.Rows.Count == 0)
				{
					AddRowInPackingMaterialGrid();
				}
				proportionatedPackingMaterialAmountInOutputGrid();
			}
			if (e.Column.Key == "Add")
			{
				AddRowInPackingMaterialGrid();
				proportionatedPackingMaterialAmountInOutputGrid();
			}
			grdPackingMaterial.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPackingMaterial.UpdateData();
			GridEXRow item = grdPackingMaterial.CurrentRow;
			if ((e.Column.Key == "ItemQTY" || e.Column.Key == "ItemRate") && item != null && (int)item.RowType == 0 && item.Cells["ItemQTY"].Value.ToString() != string.Empty && item.Cells["ItemRate"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)item.Cells["ItemQTY"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["ItemRate"].Value.ToString());
				double Amount = num * Rate;
				item.Cells["Amount"].Value = Amount;
			}
			proportionatedPackingMaterialAmountInOutputGrid();
			grdPackingMaterial.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdOverHead.CurrentRow;
				dtExpense.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtExpense.Rows.Count == 0)
				{
					AddRowInExpenseGrid();
				}
				proportionateOverheadGrid();
			}
			if (e.Column.Key == "Add")
			{
				AddRowInExpenseGrid();
				proportionateOverheadGrid();
			}
			grdOverHead.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortion())
			{
				if (((Control)(object)cmbEntryType).Text == "Issue")
				{
					table.Rows[updateDetailIndex]["EntryType"] = ((Control)(object)cmbEntryType).Text;
					table.Rows[updateDetailIndex]["WareHouseId"] = cmbGodown.Value;
					table.Rows[updateDetailIndex]["WareHouse"] = ((Control)(object)cmbGodown).Text;
					table.Rows[updateDetailIndex]["JobLotId"] = cmbLot.Value;
					table.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)cmbLot).Text;
					table.Rows[updateDetailIndex]["PackingTypeId"] = cmbBagType.Value;
					table.Rows[updateDetailIndex]["PackingType"] = ((Control)(object)cmbBagType).Text;
					table.Rows[updateDetailIndex]["ItemId"] = cmbItem.Value;
					table.Rows[updateDetailIndex]["Item"] = ((Control)(object)cmbItem).Text;
					table.Rows[updateDetailIndex]["ItemUOMId"] = cmbUOM.Value;
					table.Rows[updateDetailIndex]["UOM"] = ((Control)(object)cmbUOM).Text;
					table.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)CmbCropyr).Text;
					table.Rows[updateDetailIndex]["Quantity"] = txtQty.Text;
					table.Rows[updateDetailIndex]["Weight"] = txtUnitWeight.Text;
					table.Rows[updateDetailIndex]["Rate"] = txtRate.Text;
					table.Rows[updateDetailIndex]["RateUOMId"] = cmbRateUom.Value;
					table.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)cmbRateUom).Text;
					table.Rows[updateDetailIndex]["Amount"] = txtAmount.Text;
					table.Rows[updateDetailIndex]["Moisture"] = txtMoisture.Text;
					table.Rows[updateDetailIndex]["MoistureSlabId"] = cmbMoistureSlab.Value;
					table.Rows[updateDetailIndex]["MoistureSlab"] = ((Control)(object)cmbMoistureSlab).Text;
					table.Rows[updateDetailIndex]["Remarks"] = txtdeailRemarks.Text;
					btnUpdateDetail.Visible = false;
					btnCancelDetail.Visible = false;
					Add.Visible = true;
				}
				else
				{
					tableByProduct.Rows[updateDetailIndex]["EntryType"] = ((Control)(object)cmbEntryType).Text;
					tableByProduct.Rows[updateDetailIndex]["WareHouseId"] = cmbGodown.Value;
					tableByProduct.Rows[updateDetailIndex]["WareHouse"] = ((Control)(object)cmbGodown).Text;
					tableByProduct.Rows[updateDetailIndex]["JobLotId"] = cmbLot.Value;
					tableByProduct.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)cmbLot).Text;
					tableByProduct.Rows[updateDetailIndex]["PackingTypeId"] = cmbBagType.Value;
					tableByProduct.Rows[updateDetailIndex]["PackingType"] = ((Control)(object)cmbBagType).Text;
					tableByProduct.Rows[updateDetailIndex]["ItemId"] = cmbItem.Value;
					tableByProduct.Rows[updateDetailIndex]["Item"] = ((Control)(object)cmbItem).Text;
					tableByProduct.Rows[updateDetailIndex]["ItemUOMId"] = cmbUOM.Value;
					tableByProduct.Rows[updateDetailIndex]["UOM"] = ((Control)(object)cmbUOM).Text;
					tableByProduct.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)CmbCropyr).Text;
					tableByProduct.Rows[updateDetailIndex]["Quantity"] = txtQty.Text;
					tableByProduct.Rows[updateDetailIndex]["Weight"] = txtUnitWeight.Text;
					tableByProduct.Rows[updateDetailIndex]["Rate"] = txtRate.Text;
					tableByProduct.Rows[updateDetailIndex]["RateUOMId"] = cmbRateUom.Value;
					tableByProduct.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)cmbRateUom).Text;
					tableByProduct.Rows[updateDetailIndex]["Amount"] = txtAmount.Text;
					tableByProduct.Rows[updateDetailIndex]["Remarks"] = txtdeailRemarks.Text;
					btnUpdateDetail.Visible = false;
					btnCancelDetail.Visible = false;
					Add.Visible = true;
				}
				resetDetail();
				proportionateOverheadGrid();
				proportionatedPackingMaterialAmountInOutputGrid();
				GenerateSummaryForUser();
				cmbEntryType.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		Add.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelDetail.Visible = false;
		resetDetail();
	}

	private void HeaderGridSetting()
	{
		try
		{
			DatagridHistory.RootTable.Columns["Id"].Visible = false;
			DatagridHistory.RootTable.Columns["DocSrNo"].Width = 60;
			DatagridHistory.RootTable.Columns["DocDate"].Width = 100;
			DatagridHistory.RootTable.Columns["DocDate"].EditType = (EditType)2;
			DatagridHistory.RootTable.Columns["PNo"].Width = 60;
			DatagridHistory.RootTable.Columns["Remarks"].Width = 280;
			DatagridHistory.RootTable.Columns.Add("Detail");
			DatagridHistory.RootTable.Columns["Detail"].Key = "Detail";
			DatagridHistory.RootTable.Columns["Detail"].Caption = "Detail";
			DatagridHistory.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Detail"].ButtonText = "Detail";
			DatagridHistory.RootTable.Columns["Detail"].Width = 60;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvStockConversion SC = new InvStockConversion();
			int num = Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells[0].Value.ToString());
			new List<InvStockConversion>();
			SC = InvStockConversion.GetById(num);
			DataTable dt = new DataTable();
			DataTable dtpack = new DataTable();
			DataTable dtExp = new DataTable();
			dt.Columns.Add("EntryType");
			dt.Columns.Add("WareHouse");
			dt.Columns.Add("Item");
			dt.Columns.Add("UOM");
			dt.Columns.Add("JobLot");
			dt.Columns.Add("CropBatch");
			dt.Columns.Add("PackType");
			dt.Columns.Add("Qty", typeof(double));
			dt.Columns.Add("Weight", typeof(double));
			dt.Columns.Add("Rate");
			dt.Columns.Add("Equivalent");
			dt.Columns.Add("Amount", typeof(double));
			dt.Columns.Add("Remarks");
			dtpack.Columns.Add("Item");
			dtpack.Columns.Add("UOM");
			dtpack.Columns.Add("ItemQTY", typeof(double));
			dtpack.Columns.Add("Rate");
			dtpack.Columns.Add("Amount", typeof(double));
			dtExp.Columns.Add("ChartofAccount");
			dtExp.Columns.Add("ExpAmount", typeof(double));
			dtExp.Columns.Add("LedgerRemarks");
			for (int k = 0; k < SC.invStockConversionDetails.Count; k++)
			{
				dt.Rows.Add(SC.invStockConversionDetails[k].EntryType, SC.invStockConversionDetails[k].WareHouseName, SC.invStockConversionDetails[k].ItemName, SC.invStockConversionDetails[k].UOMDescription, SC.invStockConversionDetails[k].JobLotDescription, SC.invStockConversionDetails[k].CropBatch, SC.invStockConversionDetails[k].PackTypeDesc, SC.invStockConversionDetails[k].Qty, SC.invStockConversionDetails[k].Weight, SC.invStockConversionDetails[k].Rate, SC.invStockConversionDetails[k].Equivalent, SC.invStockConversionDetails[k].Amount, SC.invStockConversionDetails[k].Remarks);
			}
			for (int j = 0; j < SC.invStockConversionPackings.Count; j++)
			{
				dtpack.Rows.Add(SC.invStockConversionPackings[j].ItemName, SC.invStockConversionPackings[j].UOMDescription, SC.invStockConversionPackings[j].ItemQty, SC.invStockConversionPackings[j].ItemRate, SC.invStockConversionPackings[j].ItemAmount);
			}
			for (int i = 0; i < SC.invStockConversionAddExpenses.Count; i++)
			{
				dtExp.Rows.Add(SC.invStockConversionAddExpenses[i].AccountTitle, SC.invStockConversionAddExpenses[i].ExpAmount, SC.invStockConversionAddExpenses[i].LedgerRemarks);
			}
			grdDetailExpense.DataSource = dtExp;
			grdDetailExpense.RetrieveStructure();
			grdDetail.DataSource = dt;
			grdDetail.RetrieveStructure();
			grdDetailPackingMaterial.DataSource = dtpack;
			grdDetailPackingMaterial.RetrieveStructure();
			GriddetailexpSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GriddetailexpSetting()
	{
		try
		{
			grdDetailPackingMaterial.RootTable.Columns["Item"].Width = 280;
			grdDetailExpense.RootTable.Columns["ChartofAccount"].Width = 300;
			grdDetailExpense.RootTable.Columns["ExpAmount"].Width = 80;
			grdDetailExpense.RootTable.Columns["LedgerRemarks"].Width = 300;
			grdDetailPackingMaterial.RootTable.Columns["ItemQTY"].AggregateFunction = (AggregateFunction)2;
			grdDetailPackingMaterial.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdDetailExpense.RootTable.Columns["ExpAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdDetailPackingMaterial.RootTable.Columns["ItemQTY"].TextAlignment = (TextAlignment)3;
			grdDetailPackingMaterial.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdDetailPackingMaterial.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdDetailExpense.RootTable.Columns["ExpAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdDetailPackingMaterial.RootTable.Columns["ItemQTY"].FormatString = "#,##";
			grdDetailPackingMaterial.RootTable.Columns["Amount"].FormatString = "#,##";
			grdDetailExpense.RootTable.Columns["ExpAmount"].FormatString = "#,##";
			grdDetail.RootTable.Columns["Amount"].FormatString = "#,##";
			grdDetail.RootTable.Columns["Qty"].FormatString = "#,##";
			grdDetail.RootTable.Columns["Weight"].FormatString = "#,##";
			grdDetailPackingMaterial.RootTable.Columns["ItemQTY"].TotalFormatString = "#,##";
			grdDetailPackingMaterial.RootTable.Columns["Amount"].TotalFormatString = "#,##";
			grdDetailExpense.RootTable.Columns["ExpAmount"].TotalFormatString = "#,##";
			grdDetail.RootTable.Columns["Amount"].TotalFormatString = "#,##";
			grdDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##";
			grdDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##";
			grdDetail.RootTable.Columns["EntryType"].Width = 150;
			grdDetail.RootTable.Columns["WareHouse"].Width = 150;
			grdDetail.RootTable.Columns["Item"].Width = 350;
			grdDetail.RootTable.Columns["UOM"].Width = 50;
			grdDetail.RootTable.Columns["CropBatch"].Width = 80;
			grdDetail.RootTable.Columns["JobLot"].Width = 80;
			grdDetail.RootTable.Columns["PackType"].Width = 130;
			grdDetail.RootTable.Columns["Qty"].Width = 60;
			grdDetail.RootTable.Columns["Weight"].Width = 70;
			grdDetail.RootTable.Columns["Rate"].Width = 50;
			grdDetail.RootTable.Columns["Equivalent"].Width = 80;
			grdDetail.RootTable.Columns["Amount"].Width = 120;
			grdDetail.RootTable.Columns["Remarks"].Width = 200;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbEntryType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbParentCategory).Text == string.Empty)
			{
				MessageBox.Show("Please Select Parent Category First!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cmbParentCategory.Focus();
				return;
			}
			if (((Control)(object)cmbEntryType).Text == string.Empty)
			{
				MessageBox.Show("Please Select Entry Type First!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cmbEntryType.Focus();
				return;
			}
			if (((Control)(object)cmbEntryType).Text == "Recovery Head Rice")
			{
				txtRate.ReadOnly = true;
			}
			else
			{
				txtRate.ReadOnly = false;
			}
			if (((Control)(object)cmbEntryType).Text == "Issue")
			{
				WareHouseFillByParentCategoryId();
				tabControl2.SelectedIndex = 0;
				cmbGodown.Focus();
			}
			else if (((Control)(object)cmbEntryType).Text != "Issue")
			{
				ItemFill();
				combojoblotfill();
				CropYear();
				Warehouse();
				txtStock.Text = 0.ToString();
				txtQty.Text = 0.ToString();
				txtUnitWeight.Text = 0.ToString();
				txtRate.Text = 0.ToString();
				txtAverageRate.Text = 0.ToString();
				txtAmount.Text = 0.ToString();
				tabControl2.SelectedIndex = 1;
				cmbGodown.Focus();
			}
			handleWeightAndAmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocnumber_TextChanged(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			int Code = InvStockConversion.GetIdByDocNo(new InvStockConversion
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocSrNo = Conversion.ToInt((object)txtdocnumber.Text)
			});
			if (Code > 0)
			{
				RecId = Code;
				ReadById(Conversion.ToInt((object)RecId));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnHNew_Click(object sender, EventArgs e)
	{
		txtDocNoFrom.Text = string.Empty;
		txtDocNumberTo.Text = string.Empty;
		txtfromDate.Value = DateTime.Now;
		txtToDate.Value = DateTime.Now;
		grdDetailExpense.ClearStructure();
		grdDetailPackingMaterial.ClearStructure();
		grdDetail.ClearStructure();
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ProductionJobOrderSlipA(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOverHead.UpdateData();
			GridEXRow item = grdOverHead.CurrentRow;
			double Amount = 0.0;
			if (item != null && (int)item.RowType == 0 && Conversion.ToDouble(item.Cells["ExpAmount"].Value) != 0.0)
			{
				Amount = Conversion.ToDouble((object)item.Cells["ExpAmount"].Value.ToString());
				item.Cells["ExpAmount"].Value = Amount;
			}
			proportionateOverheadGrid();
			GenerateSummaryForUser();
			grdOverHead.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProduct_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdByProduct.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				((Control)(object)cmbEntryType).Text = item.Cells["EntryType"].Text.ToString();
				cmbGodown.Value = item.Cells["WareHouseId"].Value.ToString();
				cmbLot.Value = item.Cells["JobLotId"].Value.ToString();
				cmbBagType.Value = item.Cells["PackingTypeId"].Value.ToString();
				cmbItem.Value = item.Cells["ItemId"].Value.ToString();
				bindUom();
				cmbUOM.Value = item.Cells["ItemUOMId"].Value.ToString();
				((Control)(object)CmbCropyr).Text = item.Cells["CropYear"].Text.ToString();
				cmbRateUom.Value = item.Cells["RateUOMId"].Value.ToString();
				txtQty.Text = item.Cells["Quantity"].Text.ToString();
				txtUnitWeight.Text = item.Cells["Weight"].Text.ToString();
				txtRate.Text = item.Cells["Rate"].Text.ToString();
				txtAmount.Text = item.Cells["Amount"].Text.ToString();
				txtMoisture.Text = item.Cells["Moisture"].Text.ToString();
				cmbMoistureSlab.Value = item.Cells["MoistureSlabId"].Value.ToString();
				txtRemarks.Text = item.Cells["Remarks"].Text.ToString();
				Add.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGenerate_Click(object sender, EventArgs e)
	{
		try
		{
			handleAverageRateCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabPage1_Click(object sender, EventArgs e)
	{
	}

	private void CmbCropyr_Leave(object sender, EventArgs e)
	{
		if (((Control)(object)cmbEntryType).Text == "Issue")
		{
			GetAllItemsByCropByWarehouseByInventoryParentCategory(Conversion.ToInt(cmbParentCategory.Value), Conversion.ToInt(cmbGodown.Value), Conversion.ToString((object)((Control)(object)CmbCropyr).Text));
		}
	}

	private void cmbLot_Leave(object sender, EventArgs e)
	{
		if (((Control)(object)cmbEntryType).Text == "Issue")
		{
			GetCurrentItemStockByWarehouseByJobLotAndCropYear();
			GetAvgRate();
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ItemFill();
			combojoblotfill();
			CropYear();
			Warehouse();
			bagType();
			UOMFill();
			GenerateDocNumber();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				BindGridHeaderistory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbGodown_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbEntryType).Text == "Issue")
			{
				CropYearbyWarehouseIdandParentCategoryId(Conversion.ToInt(cmbGodown.Value), Conversion.ToInt(cmbParentCategory.Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void proportionatedPackingMaterialAmountInOutputGrid()
	{
		try
		{
			grdByProduct.UpdateData();
			double totalPackingMaterialAmountAgainstHeadRice = 0.0;
			double totalPackingMaterialAmountAgainstByProduct = 0.0;
			double totalHeadRiceWeightInOutput = 0.0;
			double totalByProductWeightInOutput = 0.0;
			if (grdByProduct.GetRows().Count() > 0 && grdPackingMaterial.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdByProduct.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					r3.BeginEdit();
					if (Conversion.ToString((object)r3.Cells["EntryType"].Text) == "Recovery By Product")
					{
						totalByProductWeightInOutput += Conversion.ToDouble(r3.Cells["Weight"].Value);
					}
					else if (Conversion.ToString((object)r3.Cells["EntryType"].Text) == "Recovery Head Rice")
					{
						totalHeadRiceWeightInOutput += Conversion.ToDouble(r3.Cells["Weight"].Value);
					}
				}
				rows = grdPackingMaterial.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					r2.BeginEdit();
					if (Conversion.ToString((object)r2.Cells["ChargeTo"].Text) == "Recovery Head Rice")
					{
						totalPackingMaterialAmountAgainstHeadRice += Conversion.ToDouble(r2.Cells["Amount"].Value);
					}
					else if (Conversion.ToString((object)r2.Cells["ChargeTo"].Text) == "Recovery By Product")
					{
						totalPackingMaterialAmountAgainstByProduct += Conversion.ToDouble(r2.Cells["Amount"].Value);
					}
				}
				rows = grdByProduct.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					if (Conversion.ToString((object)r.Cells["EntryType"].Text) == "Recovery Head Rice")
					{
						double overheadCharges = totalPackingMaterialAmountAgainstHeadRice / totalHeadRiceWeightInOutput * Conversion.ToDouble(r.Cells["Weight"].Value);
						r.Cells["PMAmount"].Value = Conversion.ToString((object)overheadCharges);
					}
					else if (Conversion.ToString((object)r.Cells["EntryType"].Text) == "Recovery By Product")
					{
						double overheadCharges2 = totalPackingMaterialAmountAgainstByProduct / totalByProductWeightInOutput * Conversion.ToDouble(r.Cells["Weight"].Value);
						r.Cells["PMAmount"].Value = Conversion.ToString((object)overheadCharges2);
					}
				}
			}
			else if (grdByProduct.GetRows().Count() > 0 && grdPackingMaterial.GetRows().Count() == 0)
			{
				GridEXRow[] rows = grdByProduct.GetRows();
				for (int i = 0; i < rows.Length; i++)
				{
					rows[i].Cells["PMAmount"].Value = 0.ToString();
				}
			}
			grdByProduct.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void proportionateOverheadGrid()
	{
		try
		{
			grdByProduct.UpdateData();
			double totalOverheadAmountAgainstHeadRice = 0.0;
			double totalOverheadAmountAgainstByProduct = 0.0;
			double totalHeadRiceWeightInOutput = 0.0;
			double totalByProductWeightInOutput = 0.0;
			if (grdByProduct.GetRows().Count() > 0 && grdOverHead.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdByProduct.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					r3.BeginEdit();
					if (r3.Cells["EntryType"].Text == "Recovery By Product")
					{
						totalByProductWeightInOutput += Conversion.ToDouble((object)r3.Cells["Weight"].Text);
					}
					else if (r3.Cells["EntryType"].Text == "Recovery Head Rice")
					{
						totalHeadRiceWeightInOutput += Conversion.ToDouble((object)r3.Cells["Weight"].Text);
					}
				}
				rows = grdOverHead.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					r2.BeginEdit();
					if (r2.Cells["ChargeTo"].Text == "Recovery Head Rice")
					{
						totalOverheadAmountAgainstHeadRice += Conversion.ToDouble((object)r2.Cells["ExpAmount"].Text);
					}
					else if (r2.Cells["ChargeTo"].Text == "Recovery By Product")
					{
						totalOverheadAmountAgainstByProduct += Conversion.ToDouble((object)r2.Cells["ExpAmount"].Text);
					}
				}
				rows = grdByProduct.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					if (r.Cells["EntryType"].Text == "Recovery Head Rice")
					{
						double overheadCharges = totalOverheadAmountAgainstHeadRice / totalHeadRiceWeightInOutput * Conversion.ToDouble((object)r.Cells["Weight"].Text);
						r.Cells["ExpAmount"].Value = Conversion.ToString((object)overheadCharges);
					}
					else if (r.Cells["EntryType"].Text == "Recovery By Product")
					{
						double overheadCharges2 = totalOverheadAmountAgainstByProduct / totalByProductWeightInOutput * Conversion.ToDouble((object)r.Cells["Weight"].Text);
						r.Cells["ExpAmount"].Value = Conversion.ToString((object)overheadCharges2);
					}
				}
			}
			else if (grdByProduct.GetRows().Count() > 0 && grdOverHead.GetRows().Count() == 0)
			{
				GridEXRow[] rows = grdByProduct.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["ExpAmount"].Text = 0.ToString();
				}
			}
			grdByProduct.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_CellEdited(object sender, ColumnActionEventArgs e)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPackingMaterial.CurrentRow;
			if ((e.Column.Key == "ItemQTY" || e.Column.Key == "ItemRate") && item != null && (int)item.RowType == 0 && item.Cells["ItemQTY"].Value.ToString() != string.Empty && item.Cells["ItemRate"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)item.Cells["ItemQTY"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["ItemRate"].Value.ToString());
				double Amount = num * Rate;
				item.Cells["Amount"].Value = Amount;
			}
			proportionatedPackingMaterialAmountInOutputGrid();
			GenerateSummaryForUser();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void handleAverageRateCalculation()
	{
		try
		{
			double totalAmountInInput = 0.0;
			double totalByProductAmountInOutput = 0.0;
			double totalPmAmount = 0.0;
			double totalExpAmount = 0.0;
			double totalHeadRiceWeightinOutput = 0.0;
			GridEXRow[] rows = grdStockConversionProductionDetail.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				r3.BeginEdit();
				totalAmountInInput += Conversion.ToDouble(r3.Cells["Amount"].Value);
			}
			rows = grdByProduct.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				r2.BeginEdit();
				if (r2.Cells["EntryType"].Text == "Recovery By Product")
				{
					totalByProductAmountInOutput += Conversion.ToDouble(r2.Cells["AmountWithoutExpenses"].Value);
				}
				else if (r2.Cells["EntryType"].Text == "Recovery Head Rice")
				{
					totalPmAmount += Conversion.ToDouble(r2.Cells["PMAmount"].Value);
					totalExpAmount += Conversion.ToDouble(r2.Cells["ExpAmount"].Value);
					totalHeadRiceWeightinOutput += Conversion.ToDouble(r2.Cells["Weight"].Value);
				}
			}
			double headRiceRateFor1KgWithExpese = Conversion.ToDouble((object)(Conversion.ToDouble((object)totalAmountInInput) + totalPmAmount + totalExpAmount - Conversion.ToDouble((object)totalByProductAmountInOutput))) / Conversion.ToDouble((object)totalHeadRiceWeightinOutput);
			double headRiceRateFor1KgWithoutExpese = Conversion.ToDouble((object)(Conversion.ToDouble((object)totalAmountInInput) - Conversion.ToDouble((object)totalByProductAmountInOutput))) / Conversion.ToDouble((object)totalHeadRiceWeightinOutput);
			rows = grdByProduct.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				if (r.Cells["EntryType"].Text == "Recovery By Product")
				{
					double totalAmount = Conversion.ToDouble((object)r.Cells["AmountWithoutExpenses"].Text) + Conversion.ToDouble((object)r.Cells["PMAmount"].Text) + Conversion.ToDouble((object)r.Cells["ExpAmount"].Text);
					r.Cells["Amount"].Value = Conversion.ToString((object)totalAmount);
				}
				else if (r.Cells["EntryType"].Text == "Recovery Head Rice")
				{
					double rateWiehtWithExpeseAgainstUom = Conversion.ToDouble((object)headRiceRateFor1KgWithExpese) * Conversion.ToDouble((object)r.Cells["RateUOM"].Text);
					double rateWiehtoutExpeseAgainstUom = Conversion.ToDouble((object)headRiceRateFor1KgWithoutExpese) * Conversion.ToDouble((object)r.Cells["RateUOM"].Text);
					r.Cells["Rate"].Value = Conversion.ToString((object)rateWiehtWithExpeseAgainstUom);
					double amountWithoutExpense = Conversion.ToDouble((object)r.Cells["Weight"].Text) / Conversion.ToDouble((object)r.Cells["RateUOM"].Text) * Conversion.ToDouble((object)rateWiehtoutExpeseAgainstUom);
					r.Cells["AmountWithoutExpenses"].Value = Conversion.ToString((object)amountWithoutExpense);
					double amountWithExpenses = Conversion.ToDouble((object)amountWithoutExpense) + Conversion.ToDouble((object)r.Cells["PMAmount"].Text) + Conversion.ToDouble((object)r.Cells["ExpAmount"].Text);
					r.Cells["Amount"].Value = Conversion.ToString((object)amountWithExpenses);
				}
			}
			grdByProduct.UpdateData();
			GenerateSummaryForUser();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSummaryForUser()
	{
		try
		{
			summeryreset();
			string QtyInput = grdStockConversionProductionDetail.GetTotal(grdStockConversionProductionDetail.RootTable.Columns["Quantity"], (AggregateFunction)2).ToString();
			txtInputQuantity.Text = QtyInput;
			string WeightInput = grdStockConversionProductionDetail.GetTotal(grdStockConversionProductionDetail.RootTable.Columns["Weight"], (AggregateFunction)2).ToString();
			txtInputWeight.Text = WeightInput;
			string InputRate = grdStockConversionProductionDetail.GetTotal(grdStockConversionProductionDetail.RootTable.Columns["Rate"], (AggregateFunction)2).ToString();
			txtInputRate.Text = InputRate;
			string InputAmount = grdStockConversionProductionDetail.GetTotal(grdStockConversionProductionDetail.RootTable.Columns["Amount"], (AggregateFunction)2).ToString();
			txtInputAmount.Text = Math.Round(Conversion.ToDouble((object)InputAmount), 2).ToString();
			GridEXRow[] rows = grdByProduct.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				if (grdByProduct.GetRows().Count() == 1)
				{
					if (r.Cells["EntryType"].Text == "Recovery By Product")
					{
						txtOutputByProductQty.Text = Conversion.ToString((object)r.Cells["Quantity"].Text);
						txtOutputByProductWeight.Text = Conversion.ToString((object)r.Cells["Weight"].Text);
						txtOutputByProductRate.Text = Conversion.ToString((object)r.Cells["Rate"].Text);
						txtOutputByProductAmount.Text = Conversion.ToString((object)r.Cells["Amount"].Text);
					}
					else if (r.Cells["EntryType"].Text == "Recovery Head Rice")
					{
						txtFinishGoodsQty.Text = Conversion.ToString((object)r.Cells["Quantity"].Text);
						txtFinishGoodsWeight.Text = Conversion.ToString((object)r.Cells["Weight"].Text);
						txtFinishGoodsRate.Text = Conversion.ToString((object)r.Cells["Rate"].Text);
						txtFinishGoodsAmount.Text = Conversion.ToString((object)r.Cells["Amount"].Text);
					}
				}
				else if (grdByProduct.GetRows().Count() > 1)
				{
					if (r.Cells["EntryType"].Text == "Recovery By Product")
					{
						double OutputByProductQty = Conversion.ToDouble((object)txtOutputByProductQty.Text) + Conversion.ToDouble(r.Cells["Quantity"].Value);
						txtOutputByProductQty.Text = OutputByProductQty.ToString();
						double OutputByProductWeight = Conversion.ToDouble((object)txtOutputByProductWeight.Text) + Conversion.ToDouble(r.Cells["Weight"].Value);
						txtOutputByProductWeight.Text = OutputByProductWeight.ToString();
						double OutputByProductRate = Conversion.ToDouble((object)txtOutputByProductRate.Text) + Conversion.ToDouble(r.Cells["Rate"].Value);
						txtOutputByProductRate.Text = OutputByProductRate.ToString();
						double OutputByProductAmount = Conversion.ToDouble((object)txtOutputByProductAmount.Text) + Conversion.ToDouble(r.Cells["Amount"].Value);
						txtOutputByProductAmount.Text = OutputByProductAmount.ToString();
					}
					else if (r.Cells["EntryType"].Text == "Recovery Head Rice")
					{
						double FinishGoodsQty = Conversion.ToDouble((object)txtFinishGoodsQty.Text) + Conversion.ToDouble(r.Cells["Quantity"].Value);
						txtFinishGoodsQty.Text = FinishGoodsQty.ToString();
						double FinishGoodsWeight = Conversion.ToDouble((object)txtFinishGoodsWeight.Text) + Conversion.ToDouble(r.Cells["Weight"].Value);
						txtFinishGoodsWeight.Text = FinishGoodsWeight.ToString();
						double FinishGoodsRate = Conversion.ToDouble((object)txtFinishGoodsRate.Text) + Conversion.ToDouble(r.Cells["Rate"].Value);
						txtFinishGoodsRate.Text = FinishGoodsRate.ToString();
						double FinishGoodsAmount = Conversion.ToDouble((object)txtFinishGoodsAmount.Text) + Conversion.ToDouble(r.Cells["Amount"].Value);
						txtFinishGoodsAmount.Text = Math.Round(FinishGoodsAmount, 2).ToString();
					}
				}
			}
			string QtyPackingMaterial = grdPackingMaterial.GetTotal(grdPackingMaterial.RootTable.Columns["ItemQTY"], (AggregateFunction)2).ToString();
			txtPackingMaterialQty.Text = QtyPackingMaterial;
			string RatePackingMaterial = grdPackingMaterial.GetTotal(grdPackingMaterial.RootTable.Columns["ItemRate"], (AggregateFunction)2).ToString();
			txtPackingMaterialRate.Text = RatePackingMaterial;
			string AmountPackingMaterial = grdPackingMaterial.GetTotal(grdPackingMaterial.RootTable.Columns["Amount"], (AggregateFunction)2).ToString();
			txtPackingMaterialAmount.Text = AmountPackingMaterial;
			string AmountOverheads = grdOverHead.GetTotal(grdOverHead.RootTable.Columns["ExpAmount"], (AggregateFunction)2).ToString();
			txtOverHeadAmount.Text = AmountOverheads;
			if (Conversion.ToDouble((object)txtInputAmount.Text) > 0.0 && Conversion.ToDouble((object)txtInputWeight.Text) > 0.0)
			{
				double AverageRateInput = Conversion.ToDouble((object)txtInputAmount.Text) / Conversion.ToDouble((object)txtInputWeight.Text) * 40.0;
				txtInputRate.Text = Math.Round(AverageRateInput, 2).ToString();
			}
			if (Conversion.ToDouble((object)txtOutputByProductAmount.Text) > 0.0 && Conversion.ToDouble((object)txtOutputByProductWeight.Text) > 0.0)
			{
				double AverageRateOutput = Conversion.ToDouble((object)txtOutputByProductAmount.Text) / Conversion.ToDouble((object)txtOutputByProductWeight.Text) * 40.0;
				txtOutputByProductRate.Text = AverageRateOutput.ToString("0,0");
			}
			if (Conversion.ToDouble((object)txtFinishGoodsAmount.Text) > 0.0 && Conversion.ToDouble((object)txtFinishGoodsWeight.Text) > 0.0)
			{
				double AverageRateFinishGoods = Conversion.ToDouble((object)txtFinishGoodsAmount.Text) / Conversion.ToDouble((object)txtFinishGoodsWeight.Text) * 40.0;
				txtFinishGoodsRate.Text = AverageRateFinishGoods.ToString("0,0.00");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CheckExpenseandPackingMaterialAmountTotal()
	{
		try
		{
			double PackingMaterialAmount = 0.0;
			double ExpenseAmount = 0.0;
			double ExpAmountBP = 0.0;
			double PMAmount = 0.0;
			PackingMaterialAmount = Conversion.ToDouble((object)grdPackingMaterial.GetTotal(grdPackingMaterial.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			ExpenseAmount = Conversion.ToDouble((object)grdOverHead.GetTotal(grdOverHead.RootTable.Columns["ExpAmount"], (AggregateFunction)2).ToString());
			ExpAmountBP = Conversion.ToDouble((object)grdByProduct.GetTotal(grdByProduct.RootTable.Columns["ExpAmount"], (AggregateFunction)2).ToString());
			PMAmount = Conversion.ToDouble((object)grdByProduct.GetTotal(grdByProduct.RootTable.Columns["PMAmount"], (AggregateFunction)2).ToString());
			if (PackingMaterialAmount > 0.0 && PMAmount > 0.0 && PackingMaterialAmount != PMAmount)
			{
				if (PackingMaterialAmount > PMAmount)
				{
					if (PackingMaterialAmount - PMAmount > 5.0)
					{
						checkPackingMaterialAmountTotal = true;
					}
				}
				else if (PMAmount > PackingMaterialAmount && PMAmount - PackingMaterialAmount > 5.0)
				{
					checkPackingMaterialAmountTotal = true;
				}
			}
			if (!(ExpenseAmount > 0.0) || !(ExpAmountBP > 0.0) || ExpenseAmount == ExpAmountBP)
			{
				return;
			}
			if (ExpenseAmount > ExpAmountBP)
			{
				if (ExpenseAmount - ExpAmountBP > 5.0)
				{
					CheckExpenseAmountTotal = true;
				}
			}
			else if (ExpAmountBP > ExpenseAmount && ExpAmountBP - ExpenseAmount > 5.0)
			{
				CheckExpenseAmountTotal = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdStockConversionProductionDetail).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdStockConversionProductionDetail).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdStockConversionProductionDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdByProduct).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdByProduct).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdByProduct.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnIssuanceLoad_Click(object sender, EventArgs e)
	{
		try
		{
			LoadTransactions = new LoadavailableTransactionsForIssuance(UserAccount);
			LoadTransactions.ShowDialog();
			LoadTransactions.BringToFront();
			LoadDataDetailfromPurchaseInvoivce();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailfromPurchaseInvoivce()
	{
		try
		{
			DataTable dtpurchaseinvoice = LoadTransactions.dtIssuance;
			if (dtpurchaseinvoice.Rows.Count > 0)
			{
				for (int i = 0; i < dtpurchaseinvoice.Rows.Count; i++)
				{
					table.Rows.Add(dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"], dtpurchaseinvoice.Rows[i]["RefDocIdNo"], dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"], "Issue", dtpurchaseinvoice.Rows[i]["WareHouseId"], dtpurchaseinvoice.Rows[i]["WareHouse"], dtpurchaseinvoice.Rows[i]["ItemId"], dtpurchaseinvoice.Rows[i]["ItemName"], dtpurchaseinvoice.Rows[i]["ItemUOMId"], dtpurchaseinvoice.Rows[i]["ItemUOM"], dtpurchaseinvoice.Rows[i]["CropYear"], dtpurchaseinvoice.Rows[i]["JobLotId"], dtpurchaseinvoice.Rows[i]["JobLotCode"], dtpurchaseinvoice.Rows[i]["PackingTypeId"], dtpurchaseinvoice.Rows[i]["PackTypeCode"], dtpurchaseinvoice.Rows[i]["ItemQty"], dtpurchaseinvoice.Rows[i]["Weight"], dtpurchaseinvoice.Rows[i]["ItemQty"], dtpurchaseinvoice.Rows[i]["Weight"], dtpurchaseinvoice.Rows[i]["ItemRate"], dtpurchaseinvoice.Rows[i]["RateUOMId"], dtpurchaseinvoice.Rows[i]["RateUOM"], dtpurchaseinvoice.Rows[i]["ItemAmount"], "", 0, "");
				}
				grdStockConversionProductionDetail.DataSource = table;
				grdStockConversionProductionDetail.RetrieveStructure();
				gridsettings();
				proportionateOverheadGrid();
				proportionatedPackingMaterialAmountInOutputGrid();
				GenerateSummaryForUser();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdStockConversionProductionDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdStockConversionProductionDetail.UpdateData();
			GridEXRow item = grdStockConversionProductionDetail.CurrentRow;
			if (e.Column.Key == "Quantity")
			{
				double Qty = Conversion.ToDouble((object)item.Cells["Quantity"].Value.ToString());
				double Uom = Conversion.ToDouble((object)item.Cells["UOM"].Value.ToString());
				item.Cells["Weight"].Value = Qty * Uom;
				double num = Qty * Uom;
				if (Conversion.ToDouble((object)item.Cells["BalQty"].Value.ToString()) < Qty)
				{
					item.Cells["Quantity"].Value = 0;
					throw new Exception("Qty cannot greater than BalanceQty Please check!");
				}
				double Rate2 = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double RateUOM2 = Conversion.ToDouble((object)item.Cells["RateUOM"].Value.ToString());
				double Amount2 = num / RateUOM2 * Rate2;
				item.Cells["Amount"].Value = Amount2;
			}
			if (e.Column.Key == "Weight")
			{
				double Weight = Conversion.ToDouble((object)item.Cells["Weight"].Value.ToString());
				if (Conversion.ToDouble((object)item.Cells["BalWeight"].Value.ToString()) < Weight)
				{
					item.Cells["Weight"].Value = 0;
					throw new Exception("Weight cannot greater than BalanceWeight Please check!");
				}
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double RateUOM = Conversion.ToDouble((object)item.Cells["RateUOM"].Value.ToString());
				double Amount = Weight / RateUOM * Rate;
				item.Cells["Amount"].Value = Amount;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadBOM_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			string OverHeadItemIds = "";
			string ItemIds = "";
			table.Rows.Clear();
			dtExpense.Rows.Clear();
			GridEXRow[] rows = grdByProduct.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToString((object)r.Cells["EntryType"].Text) == "Recovery By Product")
				{
					GridEXRow item2 = r.Cells["ItemId"].Row;
					tableByProduct.Rows[item2.RowIndex].Delete();
					if (item2 != null && (int)item2.RowType == 0)
					{
						item2.Delete();
					}
				}
			}
			rows = grdByProduct.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (!(Conversion.ToString((object)item.Cells["EntryType"].Text) == "Recovery Head Rice"))
				{
					continue;
				}
				OverHeadItemIds = OverHeadItemIds + "," + Conversion.ToString(item.Cells["ItemId"].Value);
				obj.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				obj.NetWeight = Conversion.ToDouble(item.Cells["Weight"].Value);
				DataTable dtItemIds = InvManufacturingBOMHeader.GetInPutItemsIdbyBomItemId(obj);
				if (dtItemIds.Rows.Count > 0)
				{
					for (int k = 0; k < dtItemIds.Rows.Count; k++)
					{
						ItemIds = ItemIds + "," + Conversion.ToInt(dtItemIds.Rows[k]["ItemId"]);
					}
				}
				obj.Ids = ItemIds;
				DataTable dtOutPut = InvManufacturingBOMHeader.GetOutPutItemsIdbyBomItemId(obj);
				if (dtOutPut.Rows.Count > 0)
				{
					for (int j = 0; j < dtOutPut.Rows.Count; j++)
					{
						tableByProduct.Rows.Add(dtOutPut.Rows[j][0], dtOutPut.Rows[j][1], dtOutPut.Rows[j][2], dtOutPut.Rows[j][3], dtOutPut.Rows[j][4], dtOutPut.Rows[j][5], dtOutPut.Rows[j][6], dtOutPut.Rows[j][7], dtOutPut.Rows[j][8], dtOutPut.Rows[j][9], dtOutPut.Rows[j][10], dtOutPut.Rows[j][11], dtOutPut.Rows[j][12], dtOutPut.Rows[j][13], dtOutPut.Rows[j][14], dtOutPut.Rows[j][15], dtOutPut.Rows[j][16], dtOutPut.Rows[j][17], dtOutPut.Rows[j][18], dtOutPut.Rows[j][19], dtOutPut.Rows[j][20], dtOutPut.Rows[j][21], dtOutPut.Rows[j][22], dtOutPut.Rows[j][23], dtOutPut.Rows[j][24]);
					}
				}
				string WarehouseIds = ItemAllocateToWareHouse.GetWarehouseIdsbyItemId(obj);
				if (WarehouseIds != string.Empty)
				{
					DataTable dtpurchaseinvoice = InventoryStockEvalautionDetail.GetAvailableTransactionsForIssuance(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						WarehouseIds = WarehouseIds,
						ItemIds = ItemIds
					});
					if (dtpurchaseinvoice.Rows.Count > 0)
					{
						double RequiredWt = 0.0;
						for (int x = 0; x < dtItemIds.Rows.Count; x++)
						{
							double Receivable = 0.0;
							for (int i = 0; i < dtpurchaseinvoice.Rows.Count; i++)
							{
								if (Conversion.ToInt(dtItemIds.Rows[x]["ItemId"]) == Conversion.ToInt(dtpurchaseinvoice.Rows[i]["ItemId"]))
								{
									double Received = 0.0;
									double LoaderWeight = Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["WeightBalance"]);
									RequiredWt = Conversion.ToDouble(dtItemIds.Rows[x]["RequiredWeight"]) - Receivable;
									if (RequiredWt > LoaderWeight)
									{
										Received = Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["WeightBalance"]);
										Receivable = Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["WeightBalance"]);
									}
									else
									{
										Received = RequiredWt;
										Receivable = Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["WeightBalance"]);
									}
									if (RequiredWt <= 0.0)
									{
										RequiredWt = 0.0;
										break;
									}
									table.Rows.Add(dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"], dtpurchaseinvoice.Rows[i]["RefDocIdNo"], dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"], "Issue", dtpurchaseinvoice.Rows[i]["WareHouseId"], dtpurchaseinvoice.Rows[i]["WareHouseCode"], dtpurchaseinvoice.Rows[i]["ItemId"], dtpurchaseinvoice.Rows[i]["ItemName"], dtpurchaseinvoice.Rows[i]["ItemUom"], dtpurchaseinvoice.Rows[i]["PackSize"], dtpurchaseinvoice.Rows[i]["CropBatch"], dtpurchaseinvoice.Rows[i]["JobLotId"], dtpurchaseinvoice.Rows[i]["JobLotCode"], dtpurchaseinvoice.Rows[i]["InvPackingTypeId"], dtpurchaseinvoice.Rows[i]["PackingType"], dtpurchaseinvoice.Rows[i]["QtyBalance"], dtpurchaseinvoice.Rows[i]["WeightBalance"], dtpurchaseinvoice.Rows[i]["QtyBalance"], Received, dtpurchaseinvoice.Rows[i]["AVgRate"], dtpurchaseinvoice.Rows[i]["RateUomId"], dtpurchaseinvoice.Rows[i]["Equivalent"], dtpurchaseinvoice.Rows[i]["ItemAmount"], "", 0, "");
								}
							}
						}
					}
					ItemIds = "";
				}
				DataTable dtOverHead = InvManufacturingBOMHeader.GetOverHeads(obj);
				if (dtOverHead.Rows.Count > 0)
				{
					for (int y = 0; y < dtOverHead.Rows.Count; y++)
					{
						dtExpense.Rows.Add(dtOverHead.Rows[y][0], dtOverHead.Rows[y][1], dtOverHead.Rows[y][2], dtOverHead.Rows[y][3]);
					}
				}
			}
			grdStockConversionProductionDetail.DataSource = table;
			grdStockConversionProductionDetail.RetrieveStructure();
			gridsettings();
			grdByProduct.DataSource = tableByProduct;
			grdByProduct.RetrieveStructure();
			gridsettingsByProduct();
			grdDetailExpense.DataSource = dtExpense;
			grdDetailExpense.RetrieveStructure();
			grdConversionExpenseSettings();
			proportionateOverheadGrid();
			proportionatedPackingMaterialAmountInOutputGrid();
			GenerateSummaryForUser();
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
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Expected O, but got Unknown
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Expected O, but got Unknown
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Expected O, but got Unknown
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Expected O, but got Unknown
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Expected O, but got Unknown
		//IL_0643: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Expected O, but got Unknown
		//IL_06a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b0: Expected O, but got Unknown
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bb: Expected O, but got Unknown
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Expected O, but got Unknown
		//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fd: Expected O, but got Unknown
		//IL_0782: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Expected O, but got Unknown
		//IL_0798: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Expected O, but got Unknown
		//IL_07a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Expected O, but got Unknown
		//IL_07ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Expected O, but got Unknown
		//IL_08cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Expected O, but got Unknown
		//IL_0903: Unknown result type (might be due to invalid IL or missing references)
		//IL_090d: Expected O, but got Unknown
		//IL_0919: Unknown result type (might be due to invalid IL or missing references)
		//IL_0923: Expected O, but got Unknown
		//IL_092f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0939: Expected O, but got Unknown
		//IL_2847: Unknown result type (might be due to invalid IL or missing references)
		//IL_2851: Expected O, but got Unknown
		//IL_2c52: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c5c: Expected O, but got Unknown
		//IL_2c69: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c73: Expected O, but got Unknown
		//IL_2c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c8a: Expected O, but got Unknown
		//IL_2e5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e64: Expected O, but got Unknown
		//IL_2e71: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e7b: Expected O, but got Unknown
		//IL_7f28: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f32: Expected O, but got Unknown
		GridEXLayout grdStockConversionProductionDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdByProduct_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPackingMaterial_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOverHead_DesignTimeLayout = new GridEXLayout();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
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
		Appearance appearance33 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production.BillOfMaterialWiseProduction));
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetailExpense_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetailPackingMaterial_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.txtAvgRate = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.txtDifference = new System.Windows.Forms.TextBox();
		this.txtPackingMaterialAmount = new System.Windows.Forms.TextBox();
		this.txtPackingMaterialRate = new System.Windows.Forms.TextBox();
		this.txtPackingMaterialWeight = new System.Windows.Forms.TextBox();
		this.txtPackingMaterialQty = new System.Windows.Forms.TextBox();
		this.label42 = new System.Windows.Forms.Label();
		this.txtOverHeadAmount = new System.Windows.Forms.TextBox();
		this.txtFinishGoodsAmount = new System.Windows.Forms.TextBox();
		this.txtOutputByProductAmount = new System.Windows.Forms.TextBox();
		this.txtInputAmount = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.txtOverHeadRate = new System.Windows.Forms.TextBox();
		this.txtOverHeadWeight = new System.Windows.Forms.TextBox();
		this.txtOverHeadQty = new System.Windows.Forms.TextBox();
		this.txtFinishGoodsRate = new System.Windows.Forms.TextBox();
		this.txtFinishGoodsWeight = new System.Windows.Forms.TextBox();
		this.txtFinishGoodsQty = new System.Windows.Forms.TextBox();
		this.txtOutputByProductRate = new System.Windows.Forms.TextBox();
		this.txtOutputByProductWeight = new System.Windows.Forms.TextBox();
		this.txtOutputByProductQty = new System.Windows.Forms.TextBox();
		this.txtInputRate = new System.Windows.Forms.TextBox();
		this.txtInputWeight = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtInputQuantity = new System.Windows.Forms.TextBox();
		this.panel5 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel22 = new System.Windows.Forms.Panel();
		this.grdStockConversionProductionDetail = new GridEX();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel23 = new System.Windows.Forms.Panel();
		this.grdByProduct = new GridEX();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdPackingMaterial = new GridEX();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdOverHead = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbMoistureSlab = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtMoisture = new System.Windows.Forms.TextBox();
		this.cmbEntryType = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtAverageRate = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtStock = new System.Windows.Forms.TextBox();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.cmbBagType = new UltraCombo();
		this.cmbUOM = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.txtdeailRemarks = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbCropyr = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbRateUom = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.txtUnitWeight = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.cmbGodown = new UltraCombo();
		this.Add = new System.Windows.Forms.Button();
		this.cmbLot = new UltraCombo();
		this.cmbItem = new UltraCombo();
		this.cmbParentCategory = new UltraCombo();
		this.chkPrint = new System.Windows.Forms.CheckBox();
		this.txtDocdate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtdocnumber = new System.Windows.Forms.TextBox();
		this.txtProductionNo = new System.Windows.Forms.TextBox();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnGenerate = new System.Windows.Forms.ToolStripButton();
		this.btnIssuanceLoad = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.panel20 = new System.Windows.Forms.Panel();
		this.panel21 = new System.Windows.Forms.Panel();
		this.grdDetailExpense = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grdDetailPackingMaterial = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.DatagridHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.btnSearch = new System.Windows.Forms.Button();
		this.label50 = new System.Windows.Forms.Label();
		this.txtDocNumberTo = new System.Windows.Forms.TextBox();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.label52 = new System.Windows.Forms.Label();
		this.txtfromDate = new System.Windows.Forms.DateTimePicker();
		this.label48 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.txtDocNoFrom = new System.Windows.Forms.TextBox();
		this.panel11 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.btnHNew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.btnLoadBOM = new System.Windows.Forms.Button();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel22.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdStockConversionProductionDetail).BeginInit();
		this.tabPage4.SuspendLayout();
		this.panel23.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdByProduct).BeginInit();
		this.tabPage5.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterial).BeginInit();
		this.tabPage6.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbMoistureSlab).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEntryType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBagType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropyr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGodown).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentCategory).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel16.SuspendLayout();
		this.panel19.SuspendLayout();
		this.panel20.SuspendLayout();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetailExpense).BeginInit();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetailPackingMaterial).BeginInit();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel11.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1184, 627);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1184, 627);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add(this.panel7);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1176, 601);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.Click += new System.EventHandler(tabPage1_Click);
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.groupBox2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(3, 454);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1170, 144);
		this.panel7.TabIndex = 0;
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add(this.txtAvgRate);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.txtDifference);
		this.groupBox2.Controls.Add(this.txtPackingMaterialAmount);
		this.groupBox2.Controls.Add(this.txtPackingMaterialRate);
		this.groupBox2.Controls.Add(this.txtPackingMaterialWeight);
		this.groupBox2.Controls.Add(this.txtPackingMaterialQty);
		this.groupBox2.Controls.Add(this.label42);
		this.groupBox2.Controls.Add(this.txtOverHeadAmount);
		this.groupBox2.Controls.Add(this.txtFinishGoodsAmount);
		this.groupBox2.Controls.Add(this.txtOutputByProductAmount);
		this.groupBox2.Controls.Add(this.txtInputAmount);
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add(this.txtOverHeadRate);
		this.groupBox2.Controls.Add(this.txtOverHeadWeight);
		this.groupBox2.Controls.Add(this.txtOverHeadQty);
		this.groupBox2.Controls.Add(this.txtFinishGoodsRate);
		this.groupBox2.Controls.Add(this.txtFinishGoodsWeight);
		this.groupBox2.Controls.Add(this.txtFinishGoodsQty);
		this.groupBox2.Controls.Add(this.txtOutputByProductRate);
		this.groupBox2.Controls.Add(this.txtOutputByProductWeight);
		this.groupBox2.Controls.Add(this.txtOutputByProductQty);
		this.groupBox2.Controls.Add(this.txtInputRate);
		this.groupBox2.Controls.Add(this.txtInputWeight);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add(this.label41);
		this.groupBox2.Controls.Add(this.label40);
		this.groupBox2.Controls.Add(this.label39);
		this.groupBox2.Controls.Add(this.label38);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.label34);
		this.groupBox2.Controls.Add(this.label33);
		this.groupBox2.Controls.Add(this.txtInputQuantity);
		this.groupBox2.Location = new System.Drawing.Point(3, 6);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1161, 131);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Summery";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(971, 24);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(49, 13);
		this.label11.TabIndex = 122;
		this.label11.Text = "Amount";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(866, 24);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(34, 13);
		this.label13.TabIndex = 119;
		this.label13.Text = "Rate";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(762, 24);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(47, 13);
		this.label14.TabIndex = 120;
		this.label14.Text = "Weight";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(656, 24);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(54, 13);
		this.label15.TabIndex = 121;
		this.label15.Text = "Quantity";
		this.txtAvgRate.Enabled = false;
		this.txtAvgRate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAvgRate.Location = new System.Drawing.Point(765, 97);
		this.txtAvgRate.Name = "txtAvgRate";
		this.txtAvgRate.Size = new System.Drawing.Size(100, 23);
		this.txtAvgRate.TabIndex = 118;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(545, 102);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(66, 13);
		this.label22.TabIndex = 117;
		this.label22.Text = "Difference";
		this.txtDifference.Enabled = false;
		this.txtDifference.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDifference.Location = new System.Drawing.Point(659, 97);
		this.txtDifference.Name = "txtDifference";
		this.txtDifference.Size = new System.Drawing.Size(98, 23);
		this.txtDifference.TabIndex = 116;
		this.txtPackingMaterialAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingMaterialAmount.Enabled = false;
		this.txtPackingMaterialAmount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingMaterialAmount.Location = new System.Drawing.Point(974, 70);
		this.txtPackingMaterialAmount.Name = "txtPackingMaterialAmount";
		this.txtPackingMaterialAmount.Size = new System.Drawing.Size(98, 23);
		this.txtPackingMaterialAmount.TabIndex = 19;
		this.txtPackingMaterialAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPackingMaterialRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingMaterialRate.Enabled = false;
		this.txtPackingMaterialRate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingMaterialRate.Location = new System.Drawing.Point(869, 70);
		this.txtPackingMaterialRate.Name = "txtPackingMaterialRate";
		this.txtPackingMaterialRate.Size = new System.Drawing.Size(98, 23);
		this.txtPackingMaterialRate.TabIndex = 18;
		this.txtPackingMaterialRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPackingMaterialWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingMaterialWeight.Enabled = false;
		this.txtPackingMaterialWeight.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingMaterialWeight.Location = new System.Drawing.Point(765, 70);
		this.txtPackingMaterialWeight.Name = "txtPackingMaterialWeight";
		this.txtPackingMaterialWeight.Size = new System.Drawing.Size(98, 23);
		this.txtPackingMaterialWeight.TabIndex = 17;
		this.txtPackingMaterialWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPackingMaterialQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingMaterialQty.Enabled = false;
		this.txtPackingMaterialQty.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingMaterialQty.Location = new System.Drawing.Point(659, 70);
		this.txtPackingMaterialQty.Name = "txtPackingMaterialQty";
		this.txtPackingMaterialQty.Size = new System.Drawing.Size(98, 23);
		this.txtPackingMaterialQty.TabIndex = 16;
		this.txtPackingMaterialQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(543, 75);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(102, 13);
		this.label42.TabIndex = 82;
		this.label42.Text = "Packing Material";
		this.txtOverHeadAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOverHeadAmount.Enabled = false;
		this.txtOverHeadAmount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOverHeadAmount.Location = new System.Drawing.Point(974, 42);
		this.txtOverHeadAmount.Name = "txtOverHeadAmount";
		this.txtOverHeadAmount.Size = new System.Drawing.Size(98, 23);
		this.txtOverHeadAmount.TabIndex = 15;
		this.txtOverHeadAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFinishGoodsAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFinishGoodsAmount.Enabled = false;
		this.txtFinishGoodsAmount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFinishGoodsAmount.Location = new System.Drawing.Point(438, 97);
		this.txtFinishGoodsAmount.Name = "txtFinishGoodsAmount";
		this.txtFinishGoodsAmount.Size = new System.Drawing.Size(98, 23);
		this.txtFinishGoodsAmount.TabIndex = 11;
		this.txtFinishGoodsAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOutputByProductAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOutputByProductAmount.Enabled = false;
		this.txtOutputByProductAmount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutputByProductAmount.Location = new System.Drawing.Point(438, 70);
		this.txtOutputByProductAmount.Name = "txtOutputByProductAmount";
		this.txtOutputByProductAmount.Size = new System.Drawing.Size(98, 23);
		this.txtOutputByProductAmount.TabIndex = 7;
		this.txtOutputByProductAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtInputAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInputAmount.Enabled = false;
		this.txtInputAmount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputAmount.Location = new System.Drawing.Point(438, 42);
		this.txtInputAmount.Name = "txtInputAmount";
		this.txtInputAmount.Size = new System.Drawing.Size(98, 23);
		this.txtInputAmount.TabIndex = 3;
		this.txtInputAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(435, 24);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(49, 13);
		this.label35.TabIndex = 77;
		this.label35.Text = "Amount";
		this.txtOverHeadRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOverHeadRate.Enabled = false;
		this.txtOverHeadRate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOverHeadRate.Location = new System.Drawing.Point(869, 42);
		this.txtOverHeadRate.Name = "txtOverHeadRate";
		this.txtOverHeadRate.Size = new System.Drawing.Size(98, 23);
		this.txtOverHeadRate.TabIndex = 14;
		this.txtOverHeadRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOverHeadWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOverHeadWeight.Enabled = false;
		this.txtOverHeadWeight.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOverHeadWeight.Location = new System.Drawing.Point(765, 42);
		this.txtOverHeadWeight.Name = "txtOverHeadWeight";
		this.txtOverHeadWeight.Size = new System.Drawing.Size(98, 23);
		this.txtOverHeadWeight.TabIndex = 13;
		this.txtOverHeadWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOverHeadQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOverHeadQty.Enabled = false;
		this.txtOverHeadQty.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOverHeadQty.Location = new System.Drawing.Point(659, 42);
		this.txtOverHeadQty.Name = "txtOverHeadQty";
		this.txtOverHeadQty.Size = new System.Drawing.Size(98, 23);
		this.txtOverHeadQty.TabIndex = 12;
		this.txtOverHeadQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFinishGoodsRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFinishGoodsRate.Enabled = false;
		this.txtFinishGoodsRate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFinishGoodsRate.Location = new System.Drawing.Point(332, 97);
		this.txtFinishGoodsRate.Name = "txtFinishGoodsRate";
		this.txtFinishGoodsRate.Size = new System.Drawing.Size(98, 23);
		this.txtFinishGoodsRate.TabIndex = 10;
		this.txtFinishGoodsRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFinishGoodsWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFinishGoodsWeight.Enabled = false;
		this.txtFinishGoodsWeight.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFinishGoodsWeight.Location = new System.Drawing.Point(229, 97);
		this.txtFinishGoodsWeight.Name = "txtFinishGoodsWeight";
		this.txtFinishGoodsWeight.Size = new System.Drawing.Size(98, 23);
		this.txtFinishGoodsWeight.TabIndex = 9;
		this.txtFinishGoodsWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFinishGoodsQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFinishGoodsQty.Enabled = false;
		this.txtFinishGoodsQty.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFinishGoodsQty.Location = new System.Drawing.Point(123, 97);
		this.txtFinishGoodsQty.Name = "txtFinishGoodsQty";
		this.txtFinishGoodsQty.Size = new System.Drawing.Size(98, 23);
		this.txtFinishGoodsQty.TabIndex = 8;
		this.txtFinishGoodsQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOutputByProductRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOutputByProductRate.Enabled = false;
		this.txtOutputByProductRate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutputByProductRate.Location = new System.Drawing.Point(333, 70);
		this.txtOutputByProductRate.Name = "txtOutputByProductRate";
		this.txtOutputByProductRate.Size = new System.Drawing.Size(98, 23);
		this.txtOutputByProductRate.TabIndex = 6;
		this.txtOutputByProductRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOutputByProductWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOutputByProductWeight.Enabled = false;
		this.txtOutputByProductWeight.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutputByProductWeight.Location = new System.Drawing.Point(229, 70);
		this.txtOutputByProductWeight.Name = "txtOutputByProductWeight";
		this.txtOutputByProductWeight.Size = new System.Drawing.Size(98, 23);
		this.txtOutputByProductWeight.TabIndex = 5;
		this.txtOutputByProductWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOutputByProductQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOutputByProductQty.Enabled = false;
		this.txtOutputByProductQty.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutputByProductQty.Location = new System.Drawing.Point(123, 70);
		this.txtOutputByProductQty.Name = "txtOutputByProductQty";
		this.txtOutputByProductQty.Size = new System.Drawing.Size(98, 23);
		this.txtOutputByProductQty.TabIndex = 4;
		this.txtOutputByProductQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtInputRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInputRate.Enabled = false;
		this.txtInputRate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputRate.Location = new System.Drawing.Point(333, 42);
		this.txtInputRate.Name = "txtInputRate";
		this.txtInputRate.Size = new System.Drawing.Size(98, 23);
		this.txtInputRate.TabIndex = 2;
		this.txtInputRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtInputWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInputWeight.Enabled = false;
		this.txtInputWeight.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputWeight.Location = new System.Drawing.Point(229, 42);
		this.txtInputWeight.Name = "txtInputWeight";
		this.txtInputWeight.Size = new System.Drawing.Size(98, 23);
		this.txtInputWeight.TabIndex = 1;
		this.txtInputWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(330, 24);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(34, 13);
		this.label36.TabIndex = 61;
		this.label36.Text = "Rate";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(543, 47);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(68, 13);
		this.label41.TabIndex = 61;
		this.label41.Text = "Over Head";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(7, 102);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(80, 13);
		this.label40.TabIndex = 61;
		this.label40.Text = "Finish Goods";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(7, 75);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(111, 13);
		this.label39.TabIndex = 61;
		this.label39.Text = "Output By Product";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(7, 47);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(36, 13);
		this.label38.TabIndex = 61;
		this.label38.Text = "Input";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(7, 24);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(71, 13);
		this.label37.TabIndex = 61;
		this.label37.Text = "Description";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(226, 24);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(47, 13);
		this.label34.TabIndex = 61;
		this.label34.Text = "Weight";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(120, 24);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(54, 13);
		this.label33.TabIndex = 61;
		this.label33.Text = "Quantity";
		this.txtInputQuantity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInputQuantity.Enabled = false;
		this.txtInputQuantity.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputQuantity.Location = new System.Drawing.Point(123, 42);
		this.txtInputQuantity.Name = "txtInputQuantity";
		this.txtInputQuantity.Size = new System.Drawing.Size(98, 23);
		this.txtInputQuantity.TabIndex = 0;
		this.txtInputQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.panel5.Controls.Add(this.tabControl2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 226);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1170, 228);
		this.panel5.TabIndex = 3;
		this.tabControl2.Appearance = System.Windows.Forms.TabAppearance.Buttons;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Controls.Add(this.tabPage5);
		this.tabControl2.Controls.Add(this.tabPage6);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1170, 228);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.BackColor = System.Drawing.Color.White;
		this.tabPage3.Controls.Add(this.panel22);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1162, 199);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Input";
		this.panel22.Controls.Add((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel22.Location = new System.Drawing.Point(3, 3);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1156, 193);
		this.panel22.TabIndex = 0;
		this.grdStockConversionProductionDetail.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdStockConversionProductionDetail).BackColor = System.Drawing.Color.White;
		grdStockConversionProductionDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdStockConversionProductionDetail.DesignTimeLayout = grdStockConversionProductionDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdStockConversionProductionDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).Name = "grdStockConversionProductionDetail";
		((ScrollableBase)this.grdStockConversionProductionDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).Size = new System.Drawing.Size(1156, 193);
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).TabIndex = 0;
		this.grdStockConversionProductionDetail.TotalRow = (InheritableBoolean)1;
		this.grdStockConversionProductionDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdStockConversionProductionDetail).VisualStyle = (VisualStyle)4;
		this.grdStockConversionProductionDetail.CellUpdated += new ColumnActionEventHandler(grdStockConversionProductionDetail_CellUpdated);
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).DoubleClick += new System.EventHandler(grdStockConversionProductionDetail_DoubleClick);
		this.tabPage4.Controls.Add(this.panel23);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1162, 199);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Output";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel23.Controls.Add((System.Windows.Forms.Control)(object)this.grdByProduct);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel23.Location = new System.Drawing.Point(3, 3);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1156, 193);
		this.panel23.TabIndex = 0;
		this.grdByProduct.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdByProduct).BackColor = System.Drawing.Color.White;
		grdByProduct_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdByProduct.DesignTimeLayout = grdByProduct_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdByProduct.FilterMode = (FilterMode)2;
		this.grdByProduct.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdByProduct.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdByProduct).Name = "grdByProduct";
		((ScrollableBase)this.grdByProduct).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Size = new System.Drawing.Size(1156, 193);
		((System.Windows.Forms.Control)(object)this.grdByProduct).TabIndex = 0;
		this.grdByProduct.TotalRow = (InheritableBoolean)1;
		this.grdByProduct.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdByProduct).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdByProduct).DoubleClick += new System.EventHandler(grdByProduct_DoubleClick);
		this.tabPage5.Controls.Add(this.panel6);
		this.tabPage5.Location = new System.Drawing.Point(4, 25);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Size = new System.Drawing.Size(1162, 199);
		this.tabPage5.TabIndex = 2;
		this.tabPage5.Text = "Packing Material";
		this.tabPage5.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackingMaterial);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1162, 199);
		this.panel6.TabIndex = 0;
		this.grdPackingMaterial.AutomaticSort = false;
		((ScrollableBase)this.grdPackingMaterial).BackColor = System.Drawing.Color.White;
		this.grdPackingMaterial.ColumnAutoResize = true;
		grdPackingMaterial_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackingMaterial.DesignTimeLayout = grdPackingMaterial_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPackingMaterial.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackingMaterial.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Name = "grdPackingMaterial";
		((ScrollableBase)this.grdPackingMaterial).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Size = new System.Drawing.Size(1162, 199);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).TabIndex = 13;
		this.grdPackingMaterial.TotalRow = (InheritableBoolean)1;
		this.grdPackingMaterial.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingMaterial).VisualStyle = (VisualStyle)4;
		this.grdPackingMaterial.CellEdited += new ColumnActionEventHandler(grdPackingMaterial_CellEdited);
		this.grdPackingMaterial.CellUpdated += new ColumnActionEventHandler(grdPackingMaterial_CellUpdated);
		this.grdPackingMaterial.ColumnButtonClick += new ColumnActionEventHandler(grdPackingMaterial_ColumnButtonClick);
		this.tabPage6.Controls.Add(this.panel12);
		this.tabPage6.Location = new System.Drawing.Point(4, 25);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Size = new System.Drawing.Size(1162, 199);
		this.tabPage6.TabIndex = 3;
		this.tabPage6.Text = "Overheads";
		this.tabPage6.UseVisualStyleBackColor = true;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdOverHead);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1162, 199);
		this.panel12.TabIndex = 0;
		this.grdOverHead.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdOverHead).BackColor = System.Drawing.Color.White;
		grdOverHead_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOverHead.DesignTimeLayout = grdOverHead_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOverHead.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdOverHead).Name = "grdOverHead";
		((ScrollableBase)this.grdOverHead).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Size = new System.Drawing.Size(1162, 199);
		((System.Windows.Forms.Control)(object)this.grdOverHead).TabIndex = 15;
		this.grdOverHead.TotalRow = (InheritableBoolean)1;
		this.grdOverHead.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOverHead).VisualStyle = (VisualStyle)4;
		this.grdOverHead.CellUpdated += new ColumnActionEventHandler(grdOverHead_CellUpdated);
		this.grdOverHead.ColumnButtonClick += new ColumnActionEventHandler(grdOverHead_ColumnButtonClick);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.btnLoadBOM);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbParentCategory);
		this.panel4.Controls.Add(this.chkPrint);
		this.panel4.Controls.Add(this.txtDocdate);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.txtdocnumber);
		this.panel4.Controls.Add(this.txtProductionNo);
		this.panel4.Controls.Add(this.txtRemarks);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 33);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1170, 193);
		this.panel4.TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(416, 5);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(94, 15);
		this.label27.TabIndex = 176;
		this.label27.Text = "Parent Category";
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbMoistureSlab);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtMoisture);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEntryType);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.txtAverageRate);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.txtStock);
		this.groupBox1.Controls.Add(this.btnCancelDetail);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBagType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUOM);
		this.groupBox1.Controls.Add(this.label44);
		this.groupBox1.Controls.Add(this.txtdeailRemarks);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropyr);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbRateUom);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.label47);
		this.groupBox1.Controls.Add(this.txtQty);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.txtUnitWeight);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.txtRate);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.txtAmount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGodown);
		this.groupBox1.Controls.Add(this.Add);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLot);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Location = new System.Drawing.Point(6, 60);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1164, 129);
		this.groupBox1.TabIndex = 175;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.cmbMoistureSlab.AllowNull = (DefaultableBoolean)1;
		this.cmbMoistureSlab.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbMoistureSlab.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbMoistureSlab.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance34).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbMoistureSlab).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbMoistureSlab.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbMoistureSlab).Location = new System.Drawing.Point(581, 95);
		((System.Windows.Forms.Control)(object)this.cmbMoistureSlab).Name = "cmbMoistureSlab";
		((System.Windows.Forms.Control)(object)this.cmbMoistureSlab).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.cmbMoistureSlab).TabIndex = 15;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(581, 76);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(84, 15);
		this.label9.TabIndex = 178;
		this.label9.Text = "Moisture Slab";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(486, 76);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(57, 15);
		this.label4.TabIndex = 176;
		this.label4.Text = "Moisture";
		this.txtMoisture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMoisture.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMoisture.Location = new System.Drawing.Point(486, 95);
		this.txtMoisture.Name = "txtMoisture";
		this.txtMoisture.Size = new System.Drawing.Size(92, 23);
		this.txtMoisture.TabIndex = 14;
		this.cmbEntryType.AllowNull = (DefaultableBoolean)1;
		this.cmbEntryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEntryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEntryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEntryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance110;
		((SpecialBoxBase)((UltraGridBase)this.cmbEntryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance2).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance2).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance2).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance43).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEntryType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Location = new System.Drawing.Point(6, 40);
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Name = "cmbEntryType";
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Size = new System.Drawing.Size(163, 26);
		((System.Windows.Forms.Control)(object)this.cmbEntryType).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Leave += new System.EventHandler(cmbEntryType_Leave);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(179, 76);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(74, 15);
		this.label30.TabIndex = 165;
		this.label30.Text = "AverageRate";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(324, 21);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(59, 15);
		this.label8.TabIndex = 153;
		this.label8.Text = "Crop Year";
		this.txtAverageRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAverageRate.Enabled = false;
		this.txtAverageRate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAverageRate.Location = new System.Drawing.Point(179, 95);
		this.txtAverageRate.Multiline = true;
		this.txtAverageRate.Name = "txtAverageRate";
		this.txtAverageRate.Size = new System.Drawing.Size(113, 26);
		this.txtAverageRate.TabIndex = 11;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(704, 21);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(36, 15);
		this.label28.TabIndex = 163;
		this.label28.Text = "Stock";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(612, 21);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(23, 15);
		this.label10.TabIndex = 155;
		this.label10.Text = "Lot";
		this.txtStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtStock.Enabled = false;
		this.txtStock.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStock.Location = new System.Drawing.Point(704, 40);
		this.txtStock.Multiline = true;
		this.txtStock.Name = "txtStock";
		this.txtStock.Size = new System.Drawing.Size(113, 26);
		this.txtStock.TabIndex = 5;
		this.btnCancelDetail.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.Location = new System.Drawing.Point(1106, 96);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(56, 23);
		this.btnCancelDetail.TabIndex = 19;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = true;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnUpdateDetail.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.Location = new System.Drawing.Point(1046, 96);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(58, 23);
		this.btnUpdateDetail.TabIndex = 18;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = true;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.cmbBagType.AllowNull = (DefaultableBoolean)1;
		this.cmbBagType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBagType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBagType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbBagType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBagType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBagType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBagType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbBagType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBagType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbBagType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBagType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbBagType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBagType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBagType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBagType).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBagType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBagType).Location = new System.Drawing.Point(912, 40);
		((System.Windows.Forms.Control)(object)this.cmbBagType).Name = "cmbBagType";
		((System.Windows.Forms.Control)(object)this.cmbBagType).Size = new System.Drawing.Size(115, 26);
		((System.Windows.Forms.Control)(object)this.cmbBagType).TabIndex = 7;
		this.cmbUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance88;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Location = new System.Drawing.Point(819, 40);
		((System.Windows.Forms.Control)(object)this.cmbUOM).Name = "cmbUOM";
		((System.Windows.Forms.Control)(object)this.cmbUOM).Size = new System.Drawing.Size(90, 26);
		((System.Windows.Forms.Control)(object)this.cmbUOM).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbUOM).TextChanged += new System.EventHandler(cmbUOM_TextChanged);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(819, 21);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(35, 15);
		this.label44.TabIndex = 142;
		this.label44.Text = "UOM";
		this.txtdeailRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeailRemarks.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdeailRemarks.Location = new System.Drawing.Point(715, 97);
		this.txtdeailRemarks.Name = "txtdeailRemarks";
		this.txtdeailRemarks.Size = new System.Drawing.Size(286, 23);
		this.txtdeailRemarks.TabIndex = 16;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(715, 80);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(54, 15);
		this.label21.TabIndex = 172;
		this.label21.Text = "Remarks";
		this.CmbCropyr.AllowNull = (DefaultableBoolean)1;
		this.CmbCropyr.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropyr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropyr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance51;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance92).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropyr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Location = new System.Drawing.Point(324, 40);
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Name = "CmbCropyr";
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Size = new System.Drawing.Size(84, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropyr).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Leave += new System.EventHandler(CmbCropyr_Leave);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(912, 21);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(54, 15);
		this.label12.TabIndex = 157;
		this.label12.Text = "Bag Type";
		this.cmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.cmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance97;
		((SpecialBoxBase)((UltraGridBase)this.cmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance103).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance115).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbRateUom).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbRateUom).Location = new System.Drawing.Point(295, 95);
		((System.Windows.Forms.Control)(object)this.cmbRateUom).Name = "cmbRateUom";
		((System.Windows.Forms.Control)(object)this.cmbRateUom).Size = new System.Drawing.Size(95, 26);
		((System.Windows.Forms.Control)(object)this.cmbRateUom).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.cmbRateUom).TextChanged += new System.EventHandler(cmbRateUom_TextChanged);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(171, 21);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(71, 15);
		this.label5.TabIndex = 151;
		this.label5.Text = "WareHouse";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(1029, 19);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(77, 15);
		this.label18.TabIndex = 159;
		this.label18.Text = "Bag Quantity";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(295, 76);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(62, 15);
		this.label47.TabIndex = 170;
		this.label47.Text = "Rate UOM";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(1030, 41);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(82, 23);
		this.txtQty.TabIndex = 8;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(9, 76);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(67, 15);
		this.label32.TabIndex = 169;
		this.label32.Text = "Net Weight";
		this.txtUnitWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtUnitWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtUnitWeight.Location = new System.Drawing.Point(9, 95);
		this.txtUnitWeight.Name = "txtUnitWeight";
		this.txtUnitWeight.Size = new System.Drawing.Size(91, 23);
		this.txtUnitWeight.TabIndex = 9;
		this.txtUnitWeight.TextChanged += new System.EventHandler(txtUnitWeight_TextChanged);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(6, 21);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(62, 15);
		this.label23.TabIndex = 167;
		this.label23.Text = "Entry Type";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(410, 21);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(31, 15);
		this.label7.TabIndex = 141;
		this.label7.Text = "Item";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(103, 76);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(31, 15);
		this.label24.TabIndex = 161;
		this.label24.Text = "Rate";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRate.Location = new System.Drawing.Point(102, 95);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(75, 23);
		this.txtRate.TabIndex = 10;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(392, 76);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(49, 15);
		this.label26.TabIndex = 162;
		this.label26.Text = "Amount";
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(392, 95);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.Size = new System.Drawing.Size(92, 23);
		this.txtAmount.TabIndex = 13;
		this.cmbGodown.AllowNull = (DefaultableBoolean)1;
		this.cmbGodown.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGodown.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGodown.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Appearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbGodown).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGodown).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGodown).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGodown).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance71;
		((SpecialBoxBase)((UltraGridBase)this.cmbGodown).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGodown).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbGodown).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGodown).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbGodown).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGodown).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGodown).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbGodown).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbGodown.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGodown).Location = new System.Drawing.Point(171, 40);
		((System.Windows.Forms.Control)(object)this.cmbGodown).Name = "cmbGodown";
		((System.Windows.Forms.Control)(object)this.cmbGodown).Size = new System.Drawing.Size(151, 26);
		((System.Windows.Forms.Control)(object)this.cmbGodown).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbGodown).Leave += new System.EventHandler(cmbGodown_Leave);
		this.Add.BackColor = System.Drawing.Color.White;
		this.Add.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.Location = new System.Drawing.Point(1004, 97);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(38, 22);
		this.Add.TabIndex = 17;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(AddInGrid_Click);
		this.cmbLot.AllowNull = (DefaultableBoolean)1;
		this.cmbLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance91;
		((SpecialBoxBase)((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance109).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance111).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance112).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLot).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLot).Location = new System.Drawing.Point(612, 40);
		((System.Windows.Forms.Control)(object)this.cmbLot).Name = "cmbLot";
		((System.Windows.Forms.Control)(object)this.cmbLot).Size = new System.Drawing.Size(90, 26);
		((System.Windows.Forms.Control)(object)this.cmbLot).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbLot).Leave += new System.EventHandler(cmbLot_Leave);
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance74;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance83).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(410, 40);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.cmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance66;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance63).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Location = new System.Drawing.Point(409, 28);
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Name = "cmbParentCategory";
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Size = new System.Drawing.Size(167, 26);
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).TabIndex = 3;
		this.chkPrint.AutoSize = true;
		this.chkPrint.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrint.Location = new System.Drawing.Point(923, 36);
		this.chkPrint.Name = "chkPrint";
		this.chkPrint.Size = new System.Drawing.Size(15, 14);
		this.chkPrint.TabIndex = 5;
		this.chkPrint.UseVisualStyleBackColor = true;
		this.txtDocdate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDocdate.Location = new System.Drawing.Point(134, 28);
		this.txtDocdate.Name = "txtDocdate";
		this.txtDocdate.Size = new System.Drawing.Size(153, 23);
		this.txtDocdate.TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(131, 11);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(90, 15);
		this.label2.TabIndex = 145;
		this.label2.Text = "Document Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(294, 9);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(85, 15);
		this.label3.TabIndex = 147;
		this.label3.Text = "Production No";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(3, 11);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(80, 15);
		this.label1.TabIndex = 140;
		this.label1.Text = "Document No";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(578, 14);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(54, 15);
		this.label6.TabIndex = 149;
		this.label6.Text = "Remarks";
		this.txtdocnumber.BackColor = System.Drawing.Color.White;
		this.txtdocnumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocnumber.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocnumber.Location = new System.Drawing.Point(6, 28);
		this.txtdocnumber.Name = "txtdocnumber";
		this.txtdocnumber.ReadOnly = true;
		this.txtdocnumber.Size = new System.Drawing.Size(122, 23);
		this.txtdocnumber.TabIndex = 0;
		this.txtProductionNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProductionNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionNo.Location = new System.Drawing.Point(293, 28);
		this.txtProductionNo.Name = "txtProductionNo";
		this.txtProductionNo.Size = new System.Drawing.Size(113, 23);
		this.txtProductionNo.TabIndex = 2;
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(581, 30);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(334, 23);
		this.txtRemarks.TabIndex = 4;
		this.panel2.Controls.Add(this.ctrlGrdBar2);
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1170, 30);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1064, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdStockConversionProductionDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1118, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdByProduct;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.toolStripButton1, this.btnnew, this.btnsave, this.btnUpdate, this.Print, this.btnRefresh, this.btnGenerate, this.btnIssuanceLoad });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1170, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripButton1.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(105, 24);
		this.toolStripButton1.Text = "Production    ";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
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
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(56, 24);
		this.Print.Text = "&Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(50, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnGenerate.Image = Architecture.WinApp.Properties.Resources.icons8_copy_16;
		this.btnGenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerate.Name = "btnGenerate";
		this.btnGenerate.Size = new System.Drawing.Size(78, 24);
		this.btnGenerate.Text = "Generate";
		this.btnGenerate.Click += new System.EventHandler(btnGenerate_Click);
		this.btnIssuanceLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnIssuanceLoad.Name = "btnIssuanceLoad";
		this.btnIssuanceLoad.Size = new System.Drawing.Size(85, 24);
		this.btnIssuanceLoad.Text = "Issuance Load";
		this.btnIssuanceLoad.Click += new System.EventHandler(btnIssuanceLoad_Click);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1176, 601);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add(this.panel13);
		this.panel10.Controls.Add(this.panel11);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1170, 595);
		this.panel10.TabIndex = 0;
		this.panel13.Controls.Add(this.panel17);
		this.panel13.Controls.Add(this.panel16);
		this.panel13.Controls.Add(this.panel15);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 26);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1170, 569);
		this.panel13.TabIndex = 2;
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(0, 351);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1170, 218);
		this.panel17.TabIndex = 2;
		this.grdDetail.AllowDelete = (InheritableBoolean)1;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1170, 218);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 45;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel16.Controls.Add(this.panel19);
		this.panel16.Controls.Add(this.panel18);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 10);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1170, 341);
		this.panel16.TabIndex = 1;
		this.panel19.Controls.Add(this.panel20);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel19.Location = new System.Drawing.Point(577, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(693, 341);
		this.panel19.TabIndex = 46;
		this.panel20.Controls.Add(this.panel21);
		this.panel20.Controls.Add(this.panel14);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(691, 341);
		this.panel20.TabIndex = 0;
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetailExpense);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(0, 174);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(691, 170);
		this.panel21.TabIndex = 47;
		this.grdDetailExpense.AllowDelete = (InheritableBoolean)1;
		grdDetailExpense_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetailExpense.DesignTimeLayout = grdDetailExpense_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetailExpense).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdDetailExpense.DynamicFiltering = true;
		this.grdDetailExpense.FilterMode = (FilterMode)2;
		this.grdDetailExpense.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetailExpense).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetailExpense.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetailExpense).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetailExpense).Name = "grdDetailExpense";
		((ScrollableBase)this.grdDetailExpense).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetailExpense).Size = new System.Drawing.Size(691, 167);
		((System.Windows.Forms.Control)(object)this.grdDetailExpense).TabIndex = 45;
		this.grdDetailExpense.TotalRow = (InheritableBoolean)1;
		this.grdDetailExpense.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetailExpense).VisualStyle = (VisualStyle)4;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetailPackingMaterial);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(691, 174);
		this.panel14.TabIndex = 46;
		this.grdDetailPackingMaterial.AllowDelete = (InheritableBoolean)1;
		grdDetailPackingMaterial_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetailPackingMaterial.DesignTimeLayout = grdDetailPackingMaterial_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetailPackingMaterial).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdDetailPackingMaterial.DynamicFiltering = true;
		this.grdDetailPackingMaterial.FilterMode = (FilterMode)2;
		this.grdDetailPackingMaterial.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetailPackingMaterial).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetailPackingMaterial.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetailPackingMaterial).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetailPackingMaterial).Name = "grdDetailPackingMaterial";
		((ScrollableBase)this.grdDetailPackingMaterial).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetailPackingMaterial).Size = new System.Drawing.Size(691, 174);
		((System.Windows.Forms.Control)(object)this.grdDetailPackingMaterial).TabIndex = 45;
		this.grdDetailPackingMaterial.TotalRow = (InheritableBoolean)1;
		this.grdDetailPackingMaterial.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetailPackingMaterial).VisualStyle = (VisualStyle)4;
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(577, 341);
		this.panel18.TabIndex = 45;
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
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(577, 341);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 44;
		this.DatagridHistory.TotalRow = (InheritableBoolean)1;
		this.DatagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistory).VisualStyle = (VisualStyle)4;
		this.DatagridHistory.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).DoubleClick += new System.EventHandler(DatagridHistory_DoubleClick);
		this.panel15.Controls.Add(this.btnSearch);
		this.panel15.Controls.Add(this.label50);
		this.panel15.Controls.Add(this.txtDocNumberTo);
		this.panel15.Controls.Add(this.txtToDate);
		this.panel15.Controls.Add(this.label52);
		this.panel15.Controls.Add(this.txtfromDate);
		this.panel15.Controls.Add(this.label48);
		this.panel15.Controls.Add(this.label49);
		this.panel15.Controls.Add(this.txtDocNoFrom);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1170, 10);
		this.panel15.TabIndex = 0;
		this.btnSearch.Location = new System.Drawing.Point(864, 16);
		this.btnSearch.Name = "btnSearch";
		this.btnSearch.Size = new System.Drawing.Size(75, 23);
		this.btnSearch.TabIndex = 4;
		this.btnSearch.Text = "Search";
		this.btnSearch.UseVisualStyleBackColor = true;
		this.btnSearch.Visible = false;
		this.btnSearch.Click += new System.EventHandler(btnSearch_Click);
		this.label50.AutoSize = true;
		this.label50.Location = new System.Drawing.Point(660, 21);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(60, 13);
		this.label50.TabIndex = 54;
		this.label50.Text = "Doc No To";
		this.label50.Visible = false;
		this.txtDocNumberTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNumberTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNumberTo.Location = new System.Drawing.Point(726, 16);
		this.txtDocNumberTo.Name = "txtDocNumberTo";
		this.txtDocNumberTo.Size = new System.Drawing.Size(113, 22);
		this.txtDocNumberTo.TabIndex = 3;
		this.txtDocNumberTo.Visible = false;
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtToDate.Location = new System.Drawing.Point(298, 17);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(153, 20);
		this.txtToDate.TabIndex = 1;
		this.txtToDate.Visible = false;
		this.label52.AutoSize = true;
		this.label52.Location = new System.Drawing.Point(246, 21);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(46, 13);
		this.label52.TabIndex = 52;
		this.label52.Text = "Date To";
		this.label52.Visible = false;
		this.txtfromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtfromDate.Location = new System.Drawing.Point(87, 17);
		this.txtfromDate.Name = "txtfromDate";
		this.txtfromDate.Size = new System.Drawing.Size(153, 20);
		this.txtfromDate.TabIndex = 0;
		this.txtfromDate.Visible = false;
		this.label48.AutoSize = true;
		this.label48.Location = new System.Drawing.Point(25, 21);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(56, 13);
		this.label48.TabIndex = 47;
		this.label48.Text = "Date From";
		this.label48.Visible = false;
		this.label49.AutoSize = true;
		this.label49.Location = new System.Drawing.Point(462, 21);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(70, 13);
		this.label49.TabIndex = 49;
		this.label49.Text = "Doc No From";
		this.label49.Visible = false;
		this.txtDocNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFrom.Location = new System.Drawing.Point(538, 16);
		this.txtDocNoFrom.Name = "txtDocNoFrom";
		this.txtDocNoFrom.Size = new System.Drawing.Size(113, 22);
		this.txtDocNoFrom.TabIndex = 2;
		this.txtDocNoFrom.Visible = false;
		this.panel11.Controls.Add(this.toolStrip2);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1170, 26);
		this.panel11.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.toolStripSeparator1, this.toolStripButton2, this.btnHNew, this.toolStripButton4 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1170, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.toolStripButton2.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(90, 24);
		this.toolStripButton2.Text = "History       ";
		this.btnHNew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnHNew.Image = (System.Drawing.Image)resources.GetObject("btnHNew.Image");
		this.btnHNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnHNew.Name = "btnHNew";
		this.btnHNew.Size = new System.Drawing.Size(55, 24);
		this.btnHNew.Text = "&New";
		this.btnHNew.Click += new System.EventHandler(btnHNew_Click);
		this.toolStripButton4.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(56, 24);
		this.toolStripButton4.Text = "&Print";
		this.btnLoadBOM.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadBOM.Location = new System.Drawing.Point(1081, 5);
		this.btnLoadBOM.Name = "btnLoadBOM";
		this.btnLoadBOM.Size = new System.Drawing.Size(81, 23);
		this.btnLoadBOM.TabIndex = 179;
		this.btnLoadBOM.Text = "LoadBOM";
		this.btnLoadBOM.UseVisualStyleBackColor = true;
		this.btnLoadBOM.Click += new System.EventHandler(btnLoadBOM_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 627);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "BillOfMaterialWiseProduction";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(invfrmStockConversionProduction_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(invfrmStockConversionProduction_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel22.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdStockConversionProductionDetail).EndInit();
		this.tabPage4.ResumeLayout(false);
		this.panel23.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdByProduct).EndInit();
		this.tabPage5.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterial).EndInit();
		this.tabPage6.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbMoistureSlab).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEntryType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBagType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropyr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGodown).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentCategory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		this.panel20.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetailExpense).EndInit();
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetailPackingMaterial).EndInit();
		this.panel18.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
