using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Main;
using Architecture.Model.Production;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
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

public class StoreStockConversion : Form
{
	private int RecId;

	private int updateDetailIndex = -1;

	private int updateDetailIndexOutput = -1;

	private int VoucherHeadId;

	private bool UpdateMode;

	private bool WagesStatus;

	private bool FifoFeature;

	private bool CGSEntryAllow;

	private bool DetailEditMood;

	private bool FormInitialized;

	private DataTable table = new DataTable();

	private DataTable tableOutput = new DataTable();

	private DataTable dtInPutItems = new DataTable();

	private DataTable dtItem = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

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

	private ToolStrip toolStrip2;

	private ToolStripButton toolStripButton4;

	internal GridEX DatagridHistory;

	internal GridEX grdDetail;

	private TextBox txtOutputByProductAmount;

	private TextBox txtInputAmount;

	private Label label35;

	private TextBox txtOutputByProductRate;

	private TextBox txtOutputByProductQty;

	private TextBox txtInputRate;

	private Label label36;

	private Label label33;

	private TextBox txtInputQuantity;

	private ToolStripButton btnHNew;

	private ToolStripButton btnRefresh;

	internal UltraCombo cmbEntryType;

	private CheckBox chkPrint;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private TextBox txtdeailRemarks;

	private Label label21;

	private Label label23;

	private Label label30;

	private TextBox txtAverageRate;

	private Label label28;

	private TextBox txtStock;

	internal UltraCombo cmbGodown;

	internal UltraCombo cmbLot;

	internal UltraCombo cmbItem;

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

	private Label label7;

	private TextBox txtdocnumber;

	private TextBox txtProductionNo;

	private TextBox txtQty;

	private Label label18;

	private Label label5;

	private Label label10;

	private GroupBox groupBox1;

	private Label label27;

	internal UltraCombo cmbParentCategory;

	private Panel panel5;

	internal GridEX grdInput;

	internal GridEX grdOutput;

	private ToolStripButton btnGenerate;

	private CtrlGrdBar ctrlGrdBar2;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnIssuanceLoad;

	private GroupBox groupBox3;

	private Panel panel3;

	private Label label16;

	private Label label4;

	private Panel panel12;

	private Label label11;

	private Label label12;

	private Label label13;

	private Panel panel24;

	private Label label15;

	private Label label9;

	private Panel panel6;

	private Label label14;

	private Label label8;

	private GroupBox groupBox4;

	private GroupBox groupBox2;

	private Label label17;

	internal UltraCombo cmbItemCondition;

	private TextBox txtRemarks;

	internal UltraCombo CmbPackUom;

	private Panel panel9;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label19;

	private Panel panel10;

	private DateTimePicker ToDateHistory;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label42;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private Label label45;

	private Button btnshowHistory;

	private DateTimePicker FromDateHistory;

	private Label label46;

	private Label label48;

	private Panel panel7;

	private Label label20;

	private Panel panel18;

	private Label label41;

	private Label label49;

	private Button BtnForwardRowsFromInputToOutPut;

	private Label label22;

	private Button btnUpdateItemCondition;

	internal UltraCombo CmbRackName;

	private Label label25;

	public StoreStockConversion(UserAccount user)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void invfrmStockConversionProduction_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1320, 750);
			WagesStatus = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WagesCompulsoryOnStockConversion"));
			CGSEntryAllow = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CGSEntryAllow"));
			FifoFeature = CommonServices.GetERPFeatureById(5);
			FormInitialized = true;
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			Print.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName", typeof(string));
			dtItem.Columns.Add("ItemCode", typeof(string));
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("RefDocumentTypeId", typeof(int));
			table.Columns.Add("RefDocNoId", typeof(int));
			table.Columns.Add("RefDocSubId", typeof(int));
			table.Columns.Add("EntryType");
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("ItemName");
			table.Columns.Add("WareHouseId", typeof(int));
			table.Columns.Add("WareHouse");
			table.Columns.Add("RackId", typeof(int));
			table.Columns.Add("RackName");
			table.Columns.Add("ItemConditionId", typeof(int));
			table.Columns.Add("ItemCondition");
			table.Columns.Add("ItemUOMId", typeof(int));
			table.Columns.Add("UOM");
			table.Columns.Add("JobLotId", typeof(int));
			table.Columns.Add("JobLot");
			table.Columns.Add("Quantity", typeof(double));
			table.Columns.Add("Rate", typeof(double));
			table.Columns.Add("Amount", typeof(double));
			table.Columns.Add("Remarks");
			table.Columns.Add("BalQty", typeof(double));
			table.Columns.Add("LineId", typeof(int));
			grdInput.DataSource = table;
			grdInput.RetrieveStructure();
			gridsettings();
			tableOutput.Columns.Add("Id", typeof(int));
			tableOutput.Columns.Add("EntryType");
			tableOutput.Columns.Add("ItemId", typeof(int));
			tableOutput.Columns.Add("ItemName");
			tableOutput.Columns.Add("WareHouseId", typeof(int));
			tableOutput.Columns.Add("WareHouse");
			tableOutput.Columns.Add("RackId", typeof(int));
			tableOutput.Columns.Add("RackName");
			tableOutput.Columns.Add("ItemConditionId", typeof(int));
			tableOutput.Columns.Add("ItemCondition");
			tableOutput.Columns.Add("ItemUOMId", typeof(int));
			tableOutput.Columns.Add("UOM");
			tableOutput.Columns.Add("JobLotId", typeof(int));
			tableOutput.Columns.Add("JobLot");
			tableOutput.Columns.Add("Quantity", typeof(double));
			tableOutput.Columns.Add("Rate", typeof(double));
			tableOutput.Columns.Add("Amount", typeof(double));
			tableOutput.Columns.Add("Remarks");
			tableOutput.Columns.Add("LineId", typeof(int));
			grdOutput.DataSource = tableOutput;
			grdOutput.RetrieveStructure();
			gridsettingsByProduct();
			txtProductionNo.Focus();
			GenerateDocNumber();
			ParentCategoryFill();
			CmbEntryTypeFill();
			InputItems();
			ItemFill();
			ItemConditionBindFromGlobal();
			joblotfill();
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			int JobLotId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Job/Lot"));
			if (JobLotId > 0)
			{
				cmbLot.Value = JobLotId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGridHeaderistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 808;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ModifyDateTo = ToDateHistory.Value;
				}
			}
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			DataTable dth = InvStockConversion.FormHistoryNew(obj);
			if (dth.Rows.Count > 0)
			{
				DataTable dtHistory = new DataTable();
				if (dtHistory != null && dtHistory.Columns.Count == 0)
				{
					dtHistory.Columns.Add("Id", typeof(int));
					dtHistory.Columns.Add("DocumentTypeId", typeof(int));
					dtHistory.Columns.Add("DocNo", typeof(int));
					dtHistory.Columns.Add("DocDate", typeof(DateTime));
					dtHistory.Columns.Add("ProductionNo", typeof(string));
					dtHistory.Columns.Add("Remarks", typeof(string));
					dtHistory.Columns.Add("EntryDate", typeof(DateTime));
					dtHistory.Columns.Add("EntryUser", typeof(string));
					dtHistory.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistory.Columns.Add("ModifyUser", typeof(string));
					dtHistory.Columns.Add("ApprovedDate", typeof(DateTime));
					dtHistory.Columns.Add("ApprovedUser", typeof(string));
					dtHistory.Columns.Add("NoOfAttachments", typeof(int));
				}
				dtHistory.Clear();
				foreach (DataRow row in dth.Rows)
				{
					dtHistory.Rows.Add(row["Id"], row["DocumentTypeId"], row["DocNo"], row["DocDate"], row["ProductionNo"], row["Remarks"], row["EntryDate"], row["EntryUser"], row["ModifyDate"], row["ModifyUser"], row["PostDate"], row["ApprovedUser"], row["NoOfAttachments"]);
				}
				DatagridHistory.DataSource = dtHistory;
				DatagridHistory.RetrieveStructure();
				HeaderGridSetting();
			}
			else
			{
				DatagridHistory.ClearStructure();
				grdDetail.ClearStructure();
			}
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
			RecId = 0;
			VoucherHeadId = 0;
			UpdateMode = false;
			updateDetailIndex = -1;
			updateDetailIndexOutput = -1;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			txtProductionNo.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			txtQty.Text = string.Empty;
			GenerateDocNumber();
			table.Clear();
			tableOutput.Rows.Clear();
			grdInput.ClearStructure();
			grdOutput.ClearStructure();
			int JobLotId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Job/Lot"));
			if (JobLotId > 0)
			{
				cmbLot.Value = JobLotId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void resetDetail()
	{
		updateDetailIndex = -1;
		updateDetailIndexOutput = -1;
		cmbItem.Value = string.Empty;
		cmbGodown.Value = string.Empty;
		((UltraGridBase)cmbGodown).DataSource = null;
		((Control)(object)CmbRackName).Text = string.Empty;
		((UltraGridBase)CmbRackName).DataSource = null;
		txtAverageRate.Text = string.Empty;
		txtQty.Text = string.Empty;
		txtStock.Text = 0.ToString();
		txtRate.Text = string.Empty;
		txtAmount.Text = string.Empty;
		txtdeailRemarks.Text = string.Empty;
	}

	private void summeryreset()
	{
		txtInputAmount.Text = string.Empty;
		txtInputQuantity.Text = string.Empty;
		txtInputRate.Text = string.Empty;
		txtOutputByProductAmount.Text = string.Empty;
		txtOutputByProductQty.Text = string.Empty;
		txtOutputByProductRate.Text = string.Empty;
	}

	private void ParentCategoryFill()
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
			if (dt.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> rows = from r in dt.AsEnumerable()
					where Conversion.ToInt(r["Id"]) == 7 || Conversion.ToInt(r["Id"]) == 8
					select r;
				DataTable dtP = (rows.Any() ? rows.CopyToDataTable() : dt.Clone());
				DDL.BindDDL(dtP, cmbParentCategory, "Id", "InvParentCateDescription", "Parent Category", ZeroIndex: false);
				((UltraGridBase)cmbParentCategory).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbEntryTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("EntryType");
			dt.Rows.Add(1, "Input");
			dt.Rows.Add(2, "Output");
			DDL.BindDDL(dt, cmbEntryType, "Id", "EntryType", "Entry Type", ZeroIndex: false);
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
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalWarehousesWithBranches, (GlobalWarehousesWithBranch x) => x.WarehouseId, (GlobalWarehousesWithBranch x) => x.WarehouseName);
			int Id = Conversion.ToInt(cmbGodown.Value);
			InfragisticsHelper.BindAndRetainSelection(cmbGodown, dt, "Id", "Description", "WareHouse From", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InputItems()
	{
		try
		{
			dtInPutItems = InvStockConversion.GetStoreAndPMItemsWithStockInHand(UserAccount.OrganizationId, UserAccount.CompanyId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindInputItem()
	{
		try
		{
			int ParentCategoryId = Conversion.ToInt(cmbParentCategory.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("ItemName", typeof(string));
			dt.Columns.Add("ItemCode", typeof(string));
			dt.Columns.Add("AvailableStock", typeof(double));
			foreach (DataRow row2 in (ParentCategoryId > 0) ? (from row in dtInPutItems.AsEnumerable()
				where Conversion.ToInt(row["InventoryParentCategoriesId"]) == ParentCategoryId
				select row) : dtInPutItems.AsEnumerable())
			{
				DataRow newRow = dt.NewRow();
				newRow["Id"] = row2["ItemId"];
				newRow["ItemName"] = row2["ItemName"];
				newRow["ItemCode"] = row2["ItemCode"];
				newRow["AvailableStock"] = row2["AvailableStock"];
				dt.Rows.Add(newRow);
			}
			int Id = Conversion.ToInt(cmbItem.Value);
			InfragisticsHelper.BindAndRetainSelection(cmbItem, dt, "Id", "ItemName", "Item Name", AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemFill()
	{
		try
		{
			dtItem.Rows.Clear();
			List<getGlobalAllItems> allItems = clsGlobalVariables.getGlobalAllItems.Where((getGlobalAllItems item) => item.ItemTypeOfTypeId == 14 || item.ItemTypeOfTypeId == 17).ToList();
			if (allItems == null || !allItems.Any())
			{
				return;
			}
			foreach (getGlobalAllItems row in allItems)
			{
				dtItem.Rows.Add(row.Id, row.ItemName, row.ItemCode);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemNameBind()
	{
		try
		{
			int Id = Conversion.ToInt(cmbItem.Value);
			InfragisticsHelper.BindAndRetainSelection(cmbItem, dtItem, "Id", "ItemName", "Item Name", AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemConditionBindFromGlobal()
	{
		try
		{
			List<GlobalItemCondition> list = clsGlobalVariables.globalItemConditions.ToList();
			int Id = Conversion.ToInt(cmbItemCondition.Value);
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(list, (GlobalItemCondition x) => x.Id, (GlobalItemCondition x) => x.ConditionStatus);
			InfragisticsHelper.BindAndRetainSelection(cmbItemCondition, dt, "Id", "Description", "Item Condition", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void joblotfill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			((Control)(object)cmbLot).Text = string.Empty;
			((UltraGridBase)cmbLot).DataSource = null;
			DataTable dtJob = JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
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

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		try
		{
			bindItemPackUom();
			AvailableStockGetByItem();
			GetAvgRate();
			int ItemId = Conversion.ToInt(cmbItem.Value);
			BindWarehouseDropdown(cmbGodown, ItemId);
			RackBindFromGlobalRacksByItemId(ItemId, Conversion.ToInt(cmbGodown.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindWarehouseDropdown(UltraCombo combo, int itemId)
	{
		List<RacksWithWarehouseAndItem> list = (from x in clsGlobalVariables.racksWithWarehouseAndItems
			where x.ItemId == itemId
			group x by x.WarehouseId into g
			select g.First()).ToList();
		DataTable dt = new DataTable();
		dt.Columns.Add("Id", typeof(int));
		dt.Columns.Add("Warehouse", typeof(string));
		dt.BeginLoadData();
		foreach (RacksWithWarehouseAndItem w in list)
		{
			dt.Rows.Add(w.WarehouseId, w.WareHouseName);
		}
		dt.EndLoadData();
		InfragisticsHelper.BindAndRetainSelection(combo, dt, "Id", "Warehouse", "Warehouse", AllColumns: true, Conversion.ToInt(combo.Value), insertDefaultRow: true);
		if (Conversion.ToInt(combo.Value) != 0)
		{
			return;
		}
		if (dt.Rows.Count == 1)
		{
			combo.Value = dt.Rows[0]["Id"];
		}
		else if (dt.Rows.Count > 1)
		{
			int configWarehouseId = 0;
			getGlobalAllItems obj = clsGlobalVariables.getGlobalAllItems.FirstOrDefault((getGlobalAllItems r) => r.Id == itemId);
			switch ((obj != null) ? obj.InventoryParentCategoriesId : 0)
			{
			case 7:
				configWarehouseId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PackingMaterialDefaultWarehouse"));
				break;
			case 8:
				configWarehouseId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultWarehouseForStoreFlow"));
				break;
			}
			if (configWarehouseId != 0 && dt.AsEnumerable().Any((DataRow row) => Conversion.ToInt(row["Id"]) == configWarehouseId))
			{
				combo.Value = configWarehouseId;
			}
		}
	}

	private void RackBindFromGlobalRacksByItemId(int ItemId, int WareHouseId = 0)
	{
		try
		{
			List<RacksWithWarehouseAndItem> list = (from rr in clsGlobalVariables.racksWithWarehouseAndItems
				where rr.ItemId == ItemId && (WareHouseId == 0 || rr.WarehouseId == WareHouseId)
				select rr into row
				group row by row.Id into g
				select g.First()).ToList();
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("RackName", typeof(string));
			dt.Columns.Add("WarehouseId", typeof(int));
			dt.Columns.Add("WarehouseName", typeof(string));
			dt.Columns.Add("BaseRackId", typeof(int));
			dt.BeginLoadData();
			foreach (RacksWithWarehouseAndItem item in list)
			{
				dt.Rows.Add(item.Id, item.RackName, item.WarehouseId, item.WareHouseName, item.BaseRackId);
			}
			dt.EndLoadData();
			InfragisticsHelper.BindAndRetainSelection(CmbRackName, dt, "Id", "RackName", "Rack Name", AllColumns: true, Conversion.ToInt(CmbRackName.Value), insertDefaultRow: true);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbRackName).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
			if (Conversion.ToInt(CmbRackName.Value) != 0)
			{
				return;
			}
			if (dt.Rows.Count == 1)
			{
				CmbRackName.Value = dt.Rows[0]["Id"];
				CmbRackName_Leave(null, null);
			}
			else if (dt.Rows.Count > 1)
			{
				DataRow baseRackRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => Conversion.ToInt(row["BaseRackId"]) > 0);
				if (baseRackRow != null)
				{
					CmbRackName.Value = baseRackRow["Id"];
					CmbRackName_Leave(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading Rack: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbRackName_Leave(object sender, EventArgs e)
	{
		try
		{
			int rackId = Conversion.ToInt(CmbRackName.Value);
			int currentWarehouseId = Conversion.ToInt(cmbGodown.Value);
			RacksWithWarehouseAndItem rack = clsGlobalVariables.racksWithWarehouseAndItems.FirstOrDefault((RacksWithWarehouseAndItem r) => r.Id == rackId);
			if (rack != null)
			{
				if (currentWarehouseId == 0 || rack.WarehouseId != currentWarehouseId)
				{
					cmbGodown.Value = rack.WarehouseId;
				}
				AvailableStockGetByItem();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void bindItemPackUom()
	{
		try
		{
			string Packuom = ((Control)(object)CmbPackUom).Text;
			DataTable dtUOM = CommonServices.dtUomFromGloablUomScheduleByItemId(Conversion.ToInt(cmbItem.Value));
			InfragisticsHelper.BindAndRetainSelection(CmbPackUom, dtUOM, "Id", "UOMCode", "Pack UOM", AllColumns: true, Packuom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
			if (Conversion.ToInt(CmbPackUom.Value) != 0)
			{
				return;
			}
			int rowCount = dtUOM?.Rows.Count ?? 0;
			if (rowCount == 1)
			{
				CmbPackUom.Value = dtUOM.Rows[0]["Id"];
			}
			else
			{
				if (rowCount <= 1)
				{
					return;
				}
				DataRow eqRow = dtUOM.AsEnumerable().FirstOrDefault((DataRow r) => Conversion.ToDecimal(r["Equivalent"]) == 1m);
				if (eqRow != null)
				{
					int eqId = Conversion.ToInt(eqRow["Id"]);
					UltraGridRow obj = ((IEnumerable)((UltraGridBase)CmbPackUom).Rows).Cast<UltraGridRow>().FirstOrDefault((UltraGridRow r) => Conversion.ToInt(r.Cells["Id"].Value) == eqId);
					if (obj != null)
					{
						obj.Activate();
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbItemCondition_Leave(object sender, EventArgs e)
	{
		try
		{
			AvailableStockGetByItem();
			GetAvgRate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAvgRate()
	{
		try
		{
			DataTable dt = CommonServices.GetAvgRateQtyAndStockInHand(Conversion.ToInt(cmbItem.Value), Conversion.ToDateTime((object)txtDocdate.Value), Conversion.ToInt((object)cmbItemCondition), RecId, (RecId > 0) ? 808 : 0);
			double rate = ((dt.Rows.Count > 0) ? Math.Round(Conversion.ToDouble(dt.Rows[0]["AvgRate"]), 3) : 0.0);
			if (rate > 0.0)
			{
				txtAverageRate.Text = rate.ToString(clsGlobalVariables.DecimalRateFormate);
				if (FifoFeature && CGSEntryAllow)
				{
					txtRate.Text = rate.ToString(clsGlobalVariables.DecimalRateFormate);
				}
				else if (!DetailEditMood)
				{
					txtRate.Text = rate.ToString(clsGlobalVariables.DecimalRateFormate);
				}
			}
			else
			{
				txtAverageRate.Text = "0";
				if (FifoFeature && CGSEntryAllow)
				{
					txtRate.Text = "0";
				}
				else if (btnsave.Visible && btnsave.Enabled)
				{
					txtRate.Text = "0";
				}
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
			VoucherHeadId = CommonServices.VoucherHeadIdGet(RecId, 66);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvailableStockGetByItem()
	{
		try
		{
			DateTime DocDate = Conversion.ToDateTime((object)txtDocdate.Value);
			int WarehouseId = Conversion.ToInt(cmbGodown.Value);
			DataTable dt = CommonServices.GetAvgRateQtyAndStockInHand(Conversion.ToInt(cmbItem.Value), ItemConditionId: Conversion.ToInt(cmbItemCondition.Value), RackId: Conversion.ToInt(CmbRackName.Value), DocDate: DocDate, RecId: RecId, DocumentTypeId: (RecId > 0) ? 0 : 0, WarehouseId: WarehouseId);
			double Stock = ((dt.Rows.Count > 0) ? Math.Round(Conversion.ToDouble(dt.Rows[0]["QtyInHand"]), 2) : 0.0);
			if (Stock > 0.0)
			{
				txtStock.Text = Stock.ToString("#,##0.###");
			}
			else
			{
				txtStock.Text = "0";
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
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			int code = InvStockConversion.GetGenerateCode(new InvStockConversion
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				DocTypeId = 808,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
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

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_0625: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			InvStockConversion stockConversion = new InvStockConversion();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				stockConversion.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (grdInput.GetRows().Count() <= 0)
			{
				MessageBox.Show("At Least 1 Entry Of Input is Required in Input Grid");
				return;
			}
			if (grdOutput.GetRows().Count() <= 0)
			{
				MessageBox.Show("At Least 1 Entry Of Output is Required in Output Grid");
				return;
			}
			if (Conversion.ToDouble((object)txtInputAmount.Text) != Conversion.ToDouble((object)txtOutputByProductAmount.Text))
			{
				throw new Exception("Total Input Amount Should Be Equal To Total Output Amount");
			}
			stockConversion.DocSrNo = Conversion.ToInt((object)txtdocnumber.Text);
			stockConversion.DocDate = Conversion.ToDateTime((object)txtDocdate.Value);
			stockConversion.DocTypeId = 808;
			stockConversion.ProductionNo = Conversion.ToString((object)txtProductionNo.Text);
			stockConversion.DocManualRef = string.Empty;
			stockConversion.Remarks = Conversion.ToString((object)txtRemarks.Text);
			stockConversion.OrganizationId = UserAccount.OrganizationId;
			stockConversion.CompanyId = UserAccount.CompanyId;
			stockConversion.BranchedId = UserAccount.BranchesId;
			stockConversion.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			stockConversion.EntryUser = UserAccount.ID;
			stockConversion.ModifyUser = UserAccount.ID;
			stockConversion.EntryDate = DateTime.Now;
			stockConversion.ModifyDate = DateTime.Now;
			stockConversion.parentCategoryId = Conversion.ToInt(cmbParentCategory.Value);
			stockConversion.invStockConversionDetails = new List<InvStockConversionDetail>();
			stockConversion.invStockConversionAddExpenses = new List<InvStockConversionAddExpense>();
			stockConversion.invStockConversionPackings = new List<InvStockConversionPackingMaterial>();
			GridEXRow[] rows = grdInput.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt(r2.Cells["ItemId"].Value) != 0)
				{
					InvStockConversionDetail d2 = new InvStockConversionDetail();
					d2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					d2.RefDocumentTypeId = Conversion.ToInt(r2.Cells["RefDocumentTypeId"].Value);
					d2.RefDocNoId = Conversion.ToInt(r2.Cells["RefDocNoId"].Value);
					d2.RefDocSubId = Conversion.ToInt(r2.Cells["RefDocSubId"].Value);
					d2.EntryType = Conversion.ToString((object)r2.Cells["EntryType"].Text);
					d2.WarehouseId = Conversion.ToInt(r2.Cells["WareHouseId"].Value);
					if (d2.WarehouseId == 0)
					{
						throw new Exception($"Warehouse require in Input grid row#{r2.RowIndex + 1}");
					}
					d2.RackId = Conversion.ToInt(r2.Cells["RackId"].Value);
					if (d2.RackId == 0)
					{
						throw new Exception($"Rack Name require in Input grid row#{r2.RowIndex + 1}");
					}
					d2.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					d2.ItemUomId = Conversion.ToInt(r2.Cells["ItemUOMId"].Value);
					if (d2.ItemUomId == 0)
					{
						throw new Exception($"Uom require in Input grid row#{r2.RowIndex + 1}");
					}
					d2.ItemConditionId = Conversion.ToInt(r2.Cells["ItemConditionId"].Value);
					if (d2.ItemConditionId == 0)
					{
						throw new Exception($"Item Condition require in Input grid row#{r2.RowIndex + 1}");
					}
					d2.JobLotId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
					if (d2.JobLotId == 0)
					{
						throw new Exception($"Job Lot require in Input grid row#{r2.RowIndex + 1}");
					}
					d2.Qty = Conversion.ToDouble(r2.Cells["Quantity"].Value);
					if (d2.Qty == 0.0)
					{
						MessageBox.Show("Qty field Required");
						return;
					}
					d2.Weight = d2.Qty;
					d2.Rate = Conversion.ToDouble(r2.Cells["Rate"].Value);
					if (d2.Rate == 0.0)
					{
						MessageBox.Show("ItemRate field Required In Row#" + (r2.RowIndex + 1) + "In InPut Grid");
						return;
					}
					d2.RateUOMId = d2.ItemUomId;
					d2.Amount = Conversion.ToDouble(r2.Cells["Amount"].Value);
					if (d2.Amount == 0.0)
					{
						MessageBox.Show("Amount field Required In Row#" + (r2.RowIndex + 1) + "In InPut Grid");
						return;
					}
					d2.Remarks = Conversion.ToString((object)r2.Cells["Remarks"].Text);
					d2.SortNo = Conversion.ToInt(r2.Cells["LineId"].Value);
					stockConversion.invStockConversionDetails.Add(d2);
				}
			}
			rows = grdOutput.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt((object)r.Cells["ItemId"].Value.ToString()) != 0)
				{
					InvStockConversionDetail d = new InvStockConversionDetail();
					d.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d.EntryType = Conversion.ToString((object)r.Cells["EntryType"].Text);
					d.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					if (d.WarehouseId == 0)
					{
						throw new Exception($"Warehouse require in output grid row#{r.RowIndex + 1}");
					}
					d.RackId = Conversion.ToInt(r.Cells["RackId"].Value);
					if (d.RackId == 0)
					{
						throw new Exception($"Rack Name require in output grid row#{r.RowIndex + 1}");
					}
					d.ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
					if (d.ItemConditionId == 0)
					{
						throw new Exception($"Item Condition require in output grid row#{r.RowIndex + 1}");
					}
					d.ItemUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
					d.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					d.Qty = Conversion.ToDouble(r.Cells["Quantity"].Value);
					d.Weight = d.Qty;
					d.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
					if (d.Rate == 0.0)
					{
						MessageBox.Show("ItemRate field Required In Row#" + (r.RowIndex + 1) + "In Output Grid");
						return;
					}
					d.RateUOMId = d.ItemUomId;
					d.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
					if (d.Amount == 0.0)
					{
						MessageBox.Show("Amount field Required In Row#" + (r.RowIndex + 1) + "In Output Grid");
						return;
					}
					d.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					d.SortNo = Conversion.ToInt(r.Cells["LineId"].Value);
					stockConversion.invStockConversionDetails.Add(d);
				}
			}
			int code = InvStockConversion.Save(stockConversion);
			if (RecId == 0)
			{
				MessageBox.Show("Save Successfully");
			}
			else
			{
				MessageBox.Show("Update Successfully");
			}
			_ = WagesStatus;
			if (chkPrint.Checked)
			{
				GeneratePrint(code);
			}
			RefreshForm();
			summeryreset();
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
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInput_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdInput.CurrentRow;
			if (item != null)
			{
				DetailEditMood = true;
				updateDetailIndex = item.RowIndex;
				cmbEntryType.Value = Conversion.ToInt((object)item.Cells["EntryType"].Text);
				cmbItem.Value = Conversion.ToInt(item.Cells["ItemId"].Value);
				cmbItem_Leave(null, null);
				cmbGodown.Value = Conversion.ToInt(item.Cells["WareHouseId"].Value);
				CmbRackName.Value = Conversion.ToInt(item.Cells["RackId"].Value);
				cmbItemCondition.Value = Conversion.ToInt(item.Cells["ItemConditionId"].Value);
				cmbLot.Value = Conversion.ToInt(item.Cells["JobLotId"].Value);
				txtQty.Text = item.Cells["Quantity"].Text.ToString();
				txtRate.Text = item.Cells["Rate"].Text.ToString();
				txtAmount.Text = item.Cells["Amount"].Text.ToString();
				txtRemarks.Text = item.Cells["Remarks"].Text.ToString();
				cmbEntryType.Focus();
				Add.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
				cmbEntryType.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				ReadById(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
				btnUpdate.Visible = true;
				btnsave.Visible = false;
				table.Clear();
				tableOutput.Clear();
				for (int i = 0; i < SC.invStockConversionDetails.Count; i++)
				{
					if (SC.invStockConversionDetails[i].EntryType == "1")
					{
						table.Rows.Add(SC.invStockConversionDetails[i].Id, SC.invStockConversionDetails[i].RefDocumentTypeId, SC.invStockConversionDetails[i].RefDocNoId, SC.invStockConversionDetails[i].RefDocSubId, SC.invStockConversionDetails[i].EntryType, SC.invStockConversionDetails[i].ItemId, SC.invStockConversionDetails[i].ItemName, SC.invStockConversionDetails[i].WarehouseId, SC.invStockConversionDetails[i].WareHouseName, SC.invStockConversionDetails[i].RackId, SC.invStockConversionDetails[i].rackName, SC.invStockConversionDetails[i].ItemConditionId, SC.invStockConversionDetails[i].ItemCondition, SC.invStockConversionDetails[i].ItemUomId, SC.invStockConversionDetails[i].UomCode, SC.invStockConversionDetails[i].JobLotId, SC.invStockConversionDetails[i].JobLotDescription, SC.invStockConversionDetails[i].Qty, SC.invStockConversionDetails[i].Rate, SC.invStockConversionDetails[i].Amount, SC.invStockConversionDetails[i].Remarks, 0, SC.invStockConversionDetails[i].SortNo);
					}
					else
					{
						tableOutput.Rows.Add(SC.invStockConversionDetails[i].Id, SC.invStockConversionDetails[i].EntryType, SC.invStockConversionDetails[i].ItemId, SC.invStockConversionDetails[i].ItemName, SC.invStockConversionDetails[i].WarehouseId, SC.invStockConversionDetails[i].WareHouseName, SC.invStockConversionDetails[i].RackId, SC.invStockConversionDetails[i].rackName, SC.invStockConversionDetails[i].ItemConditionId, SC.invStockConversionDetails[i].ItemCondition, SC.invStockConversionDetails[i].ItemUomId, SC.invStockConversionDetails[i].UomCode, SC.invStockConversionDetails[i].JobLotId, SC.invStockConversionDetails[i].JobLotDescription, SC.invStockConversionDetails[i].Qty, SC.invStockConversionDetails[i].Rate, SC.invStockConversionDetails[i].Amount, SC.invStockConversionDetails[i].Remarks, SC.invStockConversionDetails[i].SortNo);
					}
				}
				grdInput.DataSource = table;
				grdInput.RetrieveStructure();
				gridsettings();
				grdOutput.DataSource = tableOutput;
				grdOutput.RetrieveStructure();
				gridsettingsByProduct();
				btnGenerate_Click(null, null);
				VoucherHeadIdGet();
			}
			else
			{
				grdDetail.ClearStructure();
				table.Clear();
				summeryreset();
			}
			GenerateSummaryForUser();
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
			MessageBox.Show("Production NO. Field Required");
			txtProductionNo.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationOfDetailPortion()
	{
		if (((UltraGridBase)cmbEntryType).ActiveRow == null || Conversion.ToInt(cmbEntryType.Value) == 0)
		{
			MessageBox.Show("Entry Type Field Required");
			cmbEntryType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItem).ActiveRow == null || Conversion.ToInt(cmbItem.Value) == 0)
		{
			MessageBox.Show("Item Field Field Required");
			cmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)cmbGodown).ActiveRow == null || Conversion.ToInt(cmbGodown.Value) == 0)
		{
			MessageBox.Show("Warehouse Field Required");
			cmbGodown.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRackName).ActiveRow == null || Conversion.ToInt(CmbRackName.Value) == 0)
		{
			MessageBox.Show("RackName Field Required");
			CmbRackName.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemCondition).ActiveRow == null || Conversion.ToInt(cmbItemCondition.Value) == 0)
		{
			MessageBox.Show("Item Condition Field Field Required");
			cmbItemCondition.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) == 0)
		{
			MessageBox.Show("Uom Field Field Required");
			CmbPackUom.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbEntryType.Value) == 1)
		{
			if (Conversion.ToDouble((object)txtRate.Text) == 0.0)
			{
				MessageBox.Show("Rate Field Required");
				txtRate.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtAmount.Text) == 0.0)
			{
				MessageBox.Show("Amount Field Required");
				txtAmount.Focus();
				return false;
			}
		}
		if (((UltraGridBase)cmbLot).ActiveRow == null || Conversion.ToInt(cmbLot.Value) == 0)
		{
			MessageBox.Show("Job Lot Field Required");
			cmbLot.Focus();
			return false;
		}
		return true;
	}

	private void AddInGrid_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationOfDetailPortion())
			{
				return;
			}
			if (((Control)(object)cmbEntryType).Text == "Input")
			{
				if (Conversion.ToDouble((object)txtStock.Text) == 0.0 && MessageBox.Show("This item has Zero Stock. Quantity validations may not work correctly.\nDo you still want to continue?", "Zero Stock Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
				{
					return;
				}
				int selectedItemId = Conversion.ToInt(cmbItem.Value);
				int selectedWarehouseId = Conversion.ToInt(cmbGodown.Value);
				int selectedRackId = Conversion.ToInt(CmbRackName.Value);
				int selectedItemConditionId = Conversion.ToInt(cmbItemCondition.Value);
				double enteredQty = Conversion.ToDouble((object)txtQty.Text);
				if (btnsave.Visible && btnsave.Enabled)
				{
					DataRow matchedRow = table.AsEnumerable().FirstOrDefault((DataRow r) => Conversion.ToInt(r["ItemId"]) == selectedItemId && Conversion.ToInt(r["WareHouseId"]) == selectedWarehouseId && Conversion.ToInt(r["RackId"]) == selectedRackId && Conversion.ToInt(r["ItemConditionId"]) == selectedItemConditionId);
					if (matchedRow != null)
					{
						double balQty = Conversion.ToDouble(matchedRow["BalQty"]);
						double sumQty = (from r in table.AsEnumerable()
							where Conversion.ToInt(r["ItemId"]) == selectedItemId && Conversion.ToInt(r["WareHouseId"]) == selectedWarehouseId && Conversion.ToInt(r["RackId"]) == selectedRackId && Conversion.ToInt(r["ItemConditionId"]) == selectedItemConditionId
							select r).Sum((DataRow r) => Conversion.ToDouble(r["Quantity"]));
						if (sumQty + enteredQty > balQty)
						{
							MessageBox.Show($"Entered Quantity ({enteredQty}) exceeds Available Balance. " + $"\nBalance Qty : {balQty}" + $"\nAlready Used : {sumQty}" + $"\nRemaining    : {balQty - sumQty}");
							return;
						}
					}
				}
				DataRow row = table.NewRow();
				row["Id"] = 0;
				row["RefDocumentTypeId"] = 0;
				row["RefDocNoId"] = 0;
				row["RefDocSubId"] = 0;
				row["EntryType"] = cmbEntryType.Value;
				row["ItemId"] = Conversion.ToInt(cmbItem.Value);
				row["ItemName"] = ((Control)(object)cmbItem).Text;
				row["WareHouseId"] = Conversion.ToInt(cmbGodown.Value);
				row["WareHouse"] = ((Control)(object)cmbGodown).Text;
				row["RackId"] = Conversion.ToInt(CmbRackName.Value);
				row["RackName"] = ((Control)(object)CmbRackName).Text;
				row["ItemConditionId"] = Conversion.ToInt(cmbItemCondition.Value);
				row["ItemCondition"] = ((Control)(object)cmbItemCondition).Text;
				row["ItemUOMId"] = Conversion.ToInt(CmbPackUom.Value);
				row["UOM"] = ((Control)(object)CmbPackUom).Text;
				row["JobLotId"] = Conversion.ToInt(cmbLot.Value);
				row["JobLot"] = ((Control)(object)cmbLot).Text;
				row["Quantity"] = Conversion.ToDouble((object)txtQty.Text);
				row["Rate"] = Conversion.ToDouble((object)txtRate.Text);
				row["Amount"] = Conversion.ToDouble((object)txtAmount.Text);
				row["Remarks"] = txtdeailRemarks.Text;
				row["BalQty"] = Conversion.ToDouble((object)txtStock.Text);
				row["LineId"] = GetMaxLineId(table) + 1;
				table.Rows.Add(row);
				grdInput.DataSource = table;
				grdInput.RetrieveStructure();
				gridsettings();
				resetDetail();
				DetailEditMood = false;
			}
			else
			{
				tableOutput.Rows.Add(0, cmbEntryType.Value, Conversion.ToInt(cmbItem.Value), ((Control)(object)cmbItem).Text, Conversion.ToInt(cmbGodown.Value), ((Control)(object)cmbGodown).Text, Conversion.ToInt(CmbRackName.Value), ((Control)(object)CmbRackName).Text, Conversion.ToInt(cmbItemCondition.Value), ((Control)(object)cmbItemCondition).Text, Conversion.ToInt(CmbPackUom.Value), ((Control)(object)CmbPackUom).Text, Conversion.ToInt(cmbLot.Value), ((Control)(object)cmbLot).Text, Conversion.ToDouble((object)txtQty.Text), Conversion.ToDouble((object)txtRate.Text), Conversion.ToDouble((object)txtAmount.Text), txtdeailRemarks.Text);
				grdOutput.DataSource = tableOutput;
				grdOutput.RetrieveStructure();
				gridsettingsByProduct();
				resetDetail();
			}
			GenerateSummaryForUser();
			cmbEntryType.Focus();
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
			List<string> hiddenColumns = new List<string>
			{
				"Id", "RefDocumentTypeId", "RefDocNoId", "RefDocSubId", "WareHouseId", "RackId", "ItemId", "ItemUOMId", "ItemConditionId", "JobLotId",
				"EntryType", "BalQty", "LineId"
			};
			Dictionary<string, EditType> editCol = new Dictionary<string, EditType> { 
			{
				"Quantity",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(grdInput, 2, 3, hiddenColumns, editCol);
			grdInput.RootTable.Columns["EntryType"].Width = Constants.InventoryConstants.StringConstant;
			grdInput.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdInput.RootTable.Columns["WareHouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdInput.RootTable.Columns["RackName"].Width = Constants.InventoryConstants.StringConstant;
			grdInput.RootTable.Columns["UOM"].Width = Constants.InventoryConstants.UomConstant;
			grdInput.RootTable.Columns["ItemCondition"].Width = Constants.InventoryConstants.StringConstant;
			grdInput.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdInput.RootTable.Columns["Quantity"].Width = Constants.InventoryConstants.QtyConstant;
			grdInput.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdInput.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdInput.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			GridEX_Helper.AddButton(grdInput, "Delete", "X", 20);
			GridEX_Helper.AddSelectButton(grdInput, "Select", 25, 0);
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
			grdOutput.AllowEdit = (InheritableBoolean)1;
			List<string> hiddenColumns = new List<string> { "Id", "WareHouseId", "RackId", "ItemId", "ItemUOMId", "ItemConditionId", "JobLotId", "EntryType", "LineId" };
			Dictionary<string, EditType> editCol = new Dictionary<string, EditType> { 
			{
				"Quantity",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(grdOutput, 2, 3, hiddenColumns, editCol);
			grdOutput.RootTable.Columns["EntryType"].Width = Constants.InventoryConstants.StringConstant;
			grdOutput.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdOutput.RootTable.Columns["WareHouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdOutput.RootTable.Columns["RackName"].Width = Constants.InventoryConstants.StringConstant;
			grdOutput.RootTable.Columns["ItemCondition"].Width = Constants.InventoryConstants.StringConstant;
			grdOutput.RootTable.Columns["UOM"].Width = Constants.InventoryConstants.UomConstant;
			grdOutput.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdOutput.RootTable.Columns["Quantity"].Width = Constants.InventoryConstants.QtyConstant;
			grdOutput.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdOutput.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdOutput.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			GridEX_Helper.AddButton(grdOutput, "Delete", "X", 20);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AmountCalculation()
	{
		try
		{
			double qty = Conversion.ToDouble((object)txtQty.Text);
			double rate = Conversion.ToDouble((object)txtRate.Text);
			if (rate > 0.0 && qty > 0.0)
			{
				double NetAmount = qty * rate;
				txtAmount.Text = Conversion.ToDouble((object)NetAmount).ToString(clsGlobalVariables.stringFormatsingle);
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
			AmountCalculation();
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
			AmountCalculation();
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
			AmountCalculation();
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
			AmountCalculation();
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
			AmountCalculation();
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
			if (!FormValidationOfDetailPortion())
			{
				return;
			}
			if (((Control)(object)cmbEntryType).Text == "Input")
			{
				if (Conversion.ToDouble((object)txtStock.Text) == 0.0 && MessageBox.Show("This item has Zero Stock. Quantity validations may not work correctly.\nDo you still want to continue?", "Zero Stock Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
				{
					return;
				}
				if (btnsave.Visible && btnsave.Enabled)
				{
					int selectedWarehouseId = Conversion.ToInt(cmbGodown.Value);
					int selectedItemId = Conversion.ToInt(cmbItem.Value);
					int selectedItemConditionId = Conversion.ToInt(cmbItemCondition.Value);
					double enteredQty = Conversion.ToDouble((object)txtQty.Text);
					DataRow matchedRow = table.AsEnumerable().FirstOrDefault((DataRow r) => Conversion.ToInt(r["WareHouseId"]) == selectedWarehouseId && Conversion.ToInt(r["ItemId"]) == selectedItemId && Conversion.ToInt(r["ItemConditionId"]) == selectedItemConditionId);
					if (matchedRow != null)
					{
						double balQty = Conversion.ToDouble(matchedRow["BalQty"]);
						double sumQty = table.AsEnumerable().Where((DataRow r, int index) => index != updateDetailIndex && Conversion.ToInt(r["WareHouseId"]) == selectedWarehouseId && Conversion.ToInt(r["ItemId"]) == selectedItemId && Conversion.ToInt(r["ItemConditionId"]) == selectedItemConditionId).Sum((DataRow r) => Conversion.ToDouble(r["Quantity"]));
						if (sumQty + enteredQty > balQty)
						{
							MessageBox.Show($"Entered Quantity ({enteredQty}) exceeds Available Balance. " + $"\nBalance Qty : {balQty}" + $"\nAlready Used : {sumQty}" + $"\nRemaining    : {balQty - sumQty}");
							return;
						}
					}
				}
				table.Rows[updateDetailIndex]["EntryType"] = cmbEntryType.Value;
				table.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(cmbItem.Value);
				table.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)cmbItem).Text;
				table.Rows[updateDetailIndex]["WareHouseId"] = Conversion.ToInt(cmbGodown.Value);
				table.Rows[updateDetailIndex]["WareHouse"] = ((Control)(object)cmbGodown).Text;
				table.Rows[updateDetailIndex]["RckId"] = Conversion.ToInt(CmbRackName.Value);
				table.Rows[updateDetailIndex]["RackName"] = ((Control)(object)CmbRackName).Text;
				table.Rows[updateDetailIndex]["ItemConditionId"] = Conversion.ToInt(cmbItemCondition.Value);
				table.Rows[updateDetailIndex]["ItemCondition"] = ((Control)(object)cmbItemCondition).Text;
				table.Rows[updateDetailIndex]["ItemUOMId"] = Conversion.ToInt(CmbPackUom.Value);
				table.Rows[updateDetailIndex]["UOM"] = ((Control)(object)CmbPackUom).Text;
				table.Rows[updateDetailIndex]["JobLotId"] = Conversion.ToInt(cmbLot.Value);
				table.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)cmbLot).Text;
				table.Rows[updateDetailIndex]["Quantity"] = Conversion.ToDouble((object)txtQty.Text);
				table.Rows[updateDetailIndex]["Rate"] = Conversion.ToDouble((object)txtRate.Text);
				table.Rows[updateDetailIndex]["Amount"] = Conversion.ToDouble((object)txtAmount.Text);
				table.Rows[updateDetailIndex]["Remarks"] = txtdeailRemarks.Text;
				table.Rows[updateDetailIndex]["BalQty"] = Conversion.ToDouble((object)txtStock.Text);
				btnUpdateDetail.Visible = false;
				btnCancelDetail.Visible = false;
				Add.Visible = true;
			}
			else
			{
				tableOutput.Rows[updateDetailIndexOutput]["EntryType"] = cmbEntryType.Value;
				tableOutput.Rows[updateDetailIndexOutput]["ItemId"] = Conversion.ToInt(cmbItem.Value);
				tableOutput.Rows[updateDetailIndexOutput]["ItemName"] = ((Control)(object)cmbItem).Text;
				tableOutput.Rows[updateDetailIndexOutput]["WareHouseId"] = Conversion.ToInt(cmbGodown.Value);
				tableOutput.Rows[updateDetailIndexOutput]["WareHouse"] = ((Control)(object)cmbGodown).Text;
				tableOutput.Rows[updateDetailIndexOutput]["RackId"] = Conversion.ToInt(CmbRackName.Value);
				tableOutput.Rows[updateDetailIndexOutput]["RackName"] = ((Control)(object)CmbRackName).Text;
				tableOutput.Rows[updateDetailIndexOutput]["ItemConditionId"] = Conversion.ToInt(cmbItemCondition.Value);
				tableOutput.Rows[updateDetailIndexOutput]["ItemCondition"] = ((Control)(object)cmbItemCondition).Text;
				tableOutput.Rows[updateDetailIndexOutput]["ItemUOMId"] = Conversion.ToInt(CmbPackUom.Value);
				tableOutput.Rows[updateDetailIndexOutput]["UOM"] = ((Control)(object)CmbPackUom).Text;
				tableOutput.Rows[updateDetailIndexOutput]["JobLotId"] = Conversion.ToInt(cmbLot.Value);
				tableOutput.Rows[updateDetailIndexOutput]["JobLot"] = ((Control)(object)cmbLot).Text;
				tableOutput.Rows[updateDetailIndexOutput]["Quantity"] = Conversion.ToDouble((object)txtQty.Text);
				tableOutput.Rows[updateDetailIndexOutput]["Rate"] = Conversion.ToDouble((object)txtRate.Text);
				tableOutput.Rows[updateDetailIndexOutput]["Amount"] = Conversion.ToDouble((object)txtAmount.Text);
				tableOutput.Rows[updateDetailIndexOutput]["Remarks"] = txtdeailRemarks.Text;
				btnUpdateDetail.Visible = false;
				btnCancelDetail.Visible = false;
				Add.Visible = true;
			}
			resetDetail();
			GenerateSummaryForUser();
			cmbEntryType.Focus();
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
			DatagridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DatagridHistory.RootTable.Columns["NoOfAttachments"].Visible = false;
			DatagridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GridEX_Helper.GridWrappingAndColumnSettings(DatagridHistory, 2, 3, null, null, null, ColumnSetting: false);
			DatagridHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DatagridHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 20;
			DatagridHistory.RootTable.Columns["ProductionNo"].Width = Constants.InventoryConstants.ProductionNo;
			DatagridHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 50;
			DatagridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DatagridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			DatagridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DatagridHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			DatagridHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DatagridHistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			DatagridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			DatagridHistory.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yyyy";
			DatagridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DatagridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DatagridHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DatagridHistory.RootTable.Columns.Add("Edit");
			DatagridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Width = 35;
			DatagridHistory.RootTable.Columns["Edit"].Position = 1;
			DatagridHistory.RootTable.Columns.Add("Print");
			DatagridHistory.RootTable.Columns["Print"].Key = "Print";
			DatagridHistory.RootTable.Columns["Print"].Caption = "Print";
			DatagridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DatagridHistory.RootTable.Columns["Print"].Width = 40;
			DatagridHistory.RootTable.Columns["Print"].Position = 0;
			DatagridHistory.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
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
				ReadById(Conversion.ToInt(DatagridHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Print")
			{
				GeneratePrint(Conversion.ToInt(DatagridHistory.CurrentRow.Cells["Id"].Value));
			}
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
			txtRate.Enabled = Conversion.ToInt(cmbEntryType.Value) != 1 || ((!FifoFeature && !CGSEntryAllow) ? true : false);
			if (Conversion.ToInt(cmbEntryType.Value) == 1)
			{
				BindInputItem();
				return;
			}
			if (Conversion.ToInt(cmbEntryType.Value) == 2)
			{
				ItemNameBind();
				return;
			}
			((Control)(object)cmbItem).Text = string.Empty;
			((UltraGridBase)cmbItem).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocnumber_TextChanged(object sender, EventArgs e)
	{
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnHNew_Click(object sender, EventArgs e)
	{
		txtToDocNoHistory.Text = string.Empty;
		txtFromDocNoHistory.Text = string.Empty;
		FromDateHistory.Value = DateTime.Now;
		ToDateHistory.Value = DateTime.Now;
		DatagridHistory.ClearStructure();
		grdDetail.ClearStructure();
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				GeneratePrint(RecId);
				return;
			}
			throw new Exception("No Record Selected");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GeneratePrint(int Id)
	{
		try
		{
			CommonServices.InvStockConversionPackingMaterial665(Id);
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
			GridEXRow item = grdOutput.CurrentRow;
			if (item != null)
			{
				updateDetailIndexOutput = item.RowIndex;
				cmbEntryType.Value = Conversion.ToInt((object)item.Cells["EntryType"].Text);
				cmbItem.Value = Conversion.ToInt(item.Cells["ItemId"].Value);
				cmbItem_Leave(null, null);
				cmbGodown.Value = Conversion.ToInt(item.Cells["WareHouseId"].Value);
				CmbRackName.Value = Conversion.ToInt(item.Cells["RackId"].Value);
				cmbItemCondition.Value = Conversion.ToInt(item.Cells["ItemConditionId"].Value);
				cmbLot.Value = Conversion.ToInt(item.Cells["JobLotId"].Value);
				txtQty.Text = item.Cells["Quantity"].Text.ToString();
				txtRate.Text = item.Cells["Rate"].Text.ToString();
				txtAmount.Text = item.Cells["Amount"].Text.ToString();
				txtRemarks.Text = item.Cells["Remarks"].Text.ToString();
				cmbEntryType.Focus();
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

	private void cmbLot_Leave(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
		GetAvgRate();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId", "AllItems", "RacksWithWarehouseAndItem", "ItemConditions" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			InputItems();
			ItemFill();
			ItemConditionBindFromGlobal();
			joblotfill();
			if (Conversion.ToInt(cmbEntryType.Value) == 1)
			{
				BindInputItem();
			}
			else if (Conversion.ToInt(cmbEntryType.Value) == 2)
			{
				ItemNameBind();
			}
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
				FromDateHistory.Focus();
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
			RackBindFromGlobalRacksByItemId(Conversion.ToInt(cmbItem.Value), Conversion.ToInt(cmbGodown.Value));
			AvailableStockGetByItem();
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
			double totalAmountInOutput = 0.0;
			double totalQtyOutput = 0.0;
			GridEXRow[] rows = grdInput.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				totalAmountInInput += Conversion.ToDouble(r2.Cells["Amount"].Value);
			}
			rows = grdOutput.GetRows();
			foreach (GridEXRow r in rows)
			{
				totalAmountInOutput += Conversion.ToDouble(r.Cells["Amount"].Value);
				totalQtyOutput += Conversion.ToDouble(r.Cells["Quantity"].Value);
			}
			double OutputRateFor1Kg = totalAmountInInput / totalQtyOutput;
			rows = grdOutput.GetRows();
			foreach (GridEXRow obj in rows)
			{
				obj.BeginEdit();
				double rateAgainstUom = Conversion.ToDouble((object)OutputRateFor1Kg);
				obj.Cells["Rate"].Value = rateAgainstUom;
				double amount = Conversion.ToDouble((object)obj.Cells["Quantity"].Text) * Conversion.ToDouble((object)rateAgainstUom);
				obj.Cells["Amount"].Value = Conversion.ToString((object)amount);
			}
			grdOutput.UpdateData();
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
			if (grdInput.GetRows().Count() > 0)
			{
				double QtyInput = Conversion.ToDouble(grdInput.GetTotal(grdInput.RootTable.Columns["Quantity"], (AggregateFunction)2));
				txtInputQuantity.Text = QtyInput.ToString("#,##0.###");
				double InputRate = Conversion.ToDouble(grdInput.GetTotal(grdInput.RootTable.Columns["Rate"], (AggregateFunction)2));
				txtInputRate.Text = InputRate.ToString(clsGlobalVariables.DecimalRateFormate);
				double InputAmount = Conversion.ToDouble(grdInput.GetTotal(grdInput.RootTable.Columns["Amount"], (AggregateFunction)2));
				txtInputAmount.Text = InputAmount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			decimal OutputByProductQty = default(decimal);
			decimal OutputByProductRate = default(decimal);
			decimal OutputByProductAmount = default(decimal);
			GridEXRow[] rows = grdOutput.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				if (grdOutput.GetRows().Count() == 1)
				{
					OutputByProductQty = Conversion.ToDecimal((object)r.Cells["Quantity"].Text);
					OutputByProductRate = Conversion.ToDecimal((object)r.Cells["Rate"].Text);
					OutputByProductAmount = Conversion.ToDecimal((object)r.Cells["Amount"].Text);
				}
				else if (grdOutput.GetRows().Count() > 1)
				{
					OutputByProductQty += Conversion.ToDecimal(r.Cells["Quantity"].Value);
					OutputByProductRate += Conversion.ToDecimal(r.Cells["Rate"].Value);
					OutputByProductAmount += Conversion.ToDecimal(r.Cells["Amount"].Value);
				}
			}
			txtOutputByProductQty.Text = OutputByProductQty.ToString("#,##0.###");
			txtOutputByProductRate.Text = OutputByProductRate.ToString(clsGlobalVariables.DecimalRateFormate);
			txtOutputByProductAmount.Text = OutputByProductAmount.ToString(clsGlobalVariables.stringFormatsingle);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdInput, table);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdOutput, tableOutput);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnIssuanceLoad_Click(object sender, EventArgs e)
	{
		try
		{
			using LoadavailableTransactionsForConversionStore loader = new LoadavailableTransactionsForConversionStore(UserAccount);
			loader.BringToFront();
			loader.ShowDialog();
			LoadDataDetailfromPurchaseInvoivce(loader.dtLoader);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private int GetMaxLineId(DataTable table)
	{
		if (table == null || table.Rows.Count == 0)
		{
			return 0;
		}
		return (from r in table.AsEnumerable()
			where r["LineId"] != DBNull.Value
			select r).Max((DataRow r) => Conversion.ToInt(r["LineId"]));
	}

	private void LoadDataDetailfromPurchaseInvoivce(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				HashSet<(int, int, int, int)> existingDetailIds = new HashSet<(int, int, int, int)>(from r in grdInput.GetRows()
					select (Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToInt(r.Cells["WareHouseId"].Value), Conversion.ToInt(r.Cells["RackId"].Value), Conversion.ToInt(r.Cells["ItemConditionId"].Value)));
				foreach (DataRow dr in dt.Rows)
				{
					(int, int, int, int) key = (Conversion.ToInt(dr["ItemId"]), Conversion.ToInt(dr["WareHouseId"]), Conversion.ToInt(dr["RackId"]), Conversion.ToInt(dr["ItemConditionId"]));
					if (existingDetailIds.Add(key))
					{
						DataTable dtJobLot = (DataTable)((UltraGridBase)cmbLot).DataSource;
						bool isGridEmpty = grdInput.GetDataRows().Count() == 0;
						int jobLotId = (isGridEmpty ? Conversion.ToInt(cmbLot.Value) : Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Job/Lot")));
						string JobLot = (isGridEmpty ? ((Control)(object)cmbLot).Text : ((dtJobLot != null && dtJobLot.Rows.Count > 0) ? Conversion.ToString(dtJobLot.AsEnumerable().FirstOrDefault((DataRow r) => Conversion.ToInt(r["Id"]) == jobLotId)?["JobLotDescription"]) : string.Empty));
						DataRow newRow = table.NewRow();
						newRow["EntryType"] = "1";
						newRow["ItemId"] = dr["ItemId"];
						newRow["ItemName"] = dr["ItemName"];
						newRow["WareHouseId"] = dr["WarehouseId"];
						newRow["WareHouse"] = dr["WareHouseName"];
						newRow["RackId"] = dr["RackId"];
						newRow["RackName"] = dr["rackName"];
						newRow["ItemConditionId"] = dr["ItemConditionId"];
						newRow["ItemCondition"] = dr["ItemCondition"];
						newRow["ItemUOMId"] = dr["PackUomId"];
						newRow["UOM"] = dr["UOMCode"];
						newRow["JobLotId"] = jobLotId;
						newRow["JobLot"] = JobLot;
						newRow["Quantity"] = dr["AvailableStock"];
						newRow["Rate"] = dr["AvgRate"];
						newRow["Amount"] = dr["BalanceAmount"];
						newRow["BalQty"] = dr["AvailableStock"];
						newRow["LineId"] = GetMaxLineId(table) + 1;
						table.Rows.Add(newRow);
					}
				}
			}
			grdInput.DataSource = table;
			grdInput.RetrieveStructure();
			gridsettings();
			GenerateSummaryForUser();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInput_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInput.UpdateData();
			GridEXRow item = grdInput.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "Quantity")
			{
				if (btnsave.Visible && btnsave.Enabled)
				{
					int selectedWarehouseId = Conversion.ToInt(item.Cells["WareHouseId"].Value);
					int selectedItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
					int selectedItemConditionId = Conversion.ToInt(item.Cells["ItemConditionId"].Value);
					double enteredQty = Conversion.ToDouble(item.Cells["Quantity"].Value);
					DataRow matchedRow = table.AsEnumerable().FirstOrDefault((DataRow r) => Conversion.ToInt(r["WareHouseId"]) == selectedWarehouseId && Conversion.ToInt(r["ItemId"]) == selectedItemId && Conversion.ToInt(r["ItemConditionId"]) == selectedItemConditionId);
					if (matchedRow != null)
					{
						double balQty = Conversion.ToDouble(matchedRow["BalQty"]);
						int currentRowIndex = grdInput.CurrentRow.Position;
						double sumQty = table.AsEnumerable().Where((DataRow r, int index) => index != currentRowIndex && Conversion.ToInt(r["WareHouseId"]) == selectedWarehouseId && Conversion.ToInt(r["ItemId"]) == selectedItemId && Conversion.ToInt(r["ItemConditionId"]) == selectedItemConditionId).Sum((DataRow r) => Conversion.ToDouble(r["Quantity"]));
						if (sumQty + enteredQty > balQty)
						{
							MessageBox.Show($"Entered Quantity ({enteredQty}) exceeds Available Balance." + $"\nBalance Qty  : {balQty}" + $"\nAlready Used : {sumQty}" + $"\nRemaining    : {balQty - sumQty}");
							item.Cells["Quantity"].Value = balQty - sumQty;
						}
					}
				}
				double num = Conversion.ToDouble(item.Cells["Quantity"].Value);
				double Rate = Conversion.ToDouble(item.Cells["Rate"].Value);
				double Amount = num * Rate;
				item.Cells["Amount"].Value = Amount;
			}
			grdInput.UpdateData();
			GenerateSummaryForUser();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdStockConversionProductionDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!btnsave.Visible || !btnsave.Enabled)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdInput.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					if (updateDetailIndex != -1)
					{
						throw new Exception("You can't delete the record because detail in edit mode");
					}
					item.Delete();
				}
			}
			GenerateSummaryForUser();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProduct_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!btnsave.Visible || !btnsave.Enabled)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdOutput.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					if (updateDetailIndexOutput != -1)
					{
						throw new Exception("You can't delete the record because detail in edit mode");
					}
					item.Delete();
				}
			}
			GenerateSummaryForUser();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProduct_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdOutput.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "Quantity")
			{
				double num = Conversion.ToDouble(item.Cells["Quantity"].Value);
				double Rate = Conversion.ToDouble(item.Cells["Rate"].Value);
				double Amount = num * Rate;
				item.Cells["Amount"].Value = Amount;
			}
			grdOutput.UpdateData();
			handleAverageRateCalculation();
			GenerateSummaryForUser();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DatagridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				BindDetailsByHeaderId(Conversion.ToInt(item.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindDetailsByHeaderId(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvStockConversion SC = new InvStockConversion();
			SC = InvStockConversion.GetById(Id);
			if (SC.invStockConversionDetails != null && SC.invStockConversionDetails.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("EntryType");
				dt.Columns.Add("WareHouse");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PackUom");
				dt.Columns.Add("ItemCondition");
				dt.Columns.Add("JobLot");
				dt.Columns.Add("Qty", typeof(double));
				dt.Columns.Add("Rate", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("Remarks");
				for (int i = 0; i < SC.invStockConversionDetails.Count; i++)
				{
					dt.Rows.Add((Conversion.ToInt((object)SC.invStockConversionDetails[i].EntryType) == 1) ? "InPut" : "OutPut", SC.invStockConversionDetails[i].WareHouseName, SC.invStockConversionDetails[i].ItemName, SC.invStockConversionDetails[i].UomCode, SC.invStockConversionDetails[i].ItemCondition, SC.invStockConversionDetails[i].JobLotDescription, SC.invStockConversionDetails[i].Qty, SC.invStockConversionDetails[i].Rate, SC.invStockConversionDetails[i].Amount, SC.invStockConversionDetails[i].Remarks);
				}
				grdDetail.DataSource = dt;
				grdDetail.RetrieveStructure();
				GriddetailexpSetting();
			}
			else
			{
				grdDetail.ClearStructure();
			}
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
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 2, 3);
			grdDetail.RootTable.Columns["EntryType"].Width = Constants.InventoryConstants.DiscountType;
			grdDetail.RootTable.Columns["WareHouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdDetail.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["ItemCondition"].Width = Constants.InventoryConstants.StringConstant;
			grdDetail.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdDetail.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StoreStockConversion_FormClosing(object sender, FormClosingEventArgs e)
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

	private void AvgRateUpdateOnDocDateChange()
	{
		try
		{
			if (grdInput.RowCount <= 0)
			{
				return;
			}
			GridEXRow[] dataRows = grdInput.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				r.BeginEdit();
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				int ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
				double Qty = Conversion.ToDouble(r.Cells["Quantity"].Value);
				double rate = 0.0;
				DataTable dt = CommonServices.GetAvgRateQtyAndStockInHand(ItemId, txtDocdate.Value, ItemConditionId, RecId, (RecId > 0) ? 808 : 0);
				if (dt.Rows.Count > 0)
				{
					rate = Math.Round(Conversion.ToDouble(dt.Rows[0]["AvgRate"]), 3);
				}
				if (rate > 0.0)
				{
					r.Cells["Rate"].Value = rate;
					double Amount = Qty * rate;
					r.Cells["Amount"].Value = Amount;
				}
				else
				{
					r.Cells["Rate"].Value = 0;
					r.Cells["Amount"].Value = 0;
				}
				r.EndEdit();
			}
			grdInput.UpdateData();
			handleAverageRateCalculation();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtDocdate_Leave(object sender, EventArgs e)
	{
		try
		{
			if (FifoFeature || CGSEntryAllow)
			{
				AvgRateUpdateOnDocDateChange();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DatagridHistory).Name);
			if (fs != null)
			{
				DatagridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			BindGridHeaderistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnForwardRowsFromInputToOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdInput.RowCount == 0)
			{
				throw new Exception("Input Grid Have no Rows..");
			}
			if (grdInput.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please Check any Row First...");
			}
			GridEXRow[] checkedRows = grdInput.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				int inputLineId = Conversion.ToInt(r.Cells["LineId"].Value);
				if (!tableOutput.AsEnumerable().Any((DataRow row) => Conversion.ToInt(row["LineId"]) == inputLineId))
				{
					DataRow newRow = tableOutput.NewRow();
					newRow["EntryType"] = "2";
					newRow["ItemId"] = Conversion.ToInt(r.Cells["ItemId"].Value);
					newRow["ItemName"] = Conversion.ToString(r.Cells["ItemName"].Value);
					newRow["WareHouseId"] = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					newRow["WareHouse"] = Conversion.ToString(r.Cells["WareHouse"].Value);
					newRow["RackId"] = Conversion.ToInt(r.Cells["RackId"].Value);
					newRow["RackName"] = Conversion.ToString(r.Cells["RackName"].Value);
					newRow["ItemUOMId"] = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
					newRow["UOM"] = Conversion.ToString(r.Cells["UOM"].Value);
					newRow["JobLotId"] = Conversion.ToInt(r.Cells["JobLotId"].Value);
					newRow["JobLot"] = Conversion.ToString(r.Cells["JobLot"].Value);
					newRow["Quantity"] = Conversion.ToDouble(r.Cells["Quantity"].Value);
					newRow["Rate"] = Conversion.ToDouble(r.Cells["Rate"].Value);
					newRow["Amount"] = Conversion.ToDouble(r.Cells["Amount"].Value);
					newRow["LineId"] = inputLineId;
					tableOutput.Rows.Add(newRow);
				}
			}
			if (tableOutput.Rows.Count > 0)
			{
				grdOutput.DataSource = tableOutput;
				grdOutput.RetrieveStructure();
				gridsettingsByProduct();
				GenerateSummaryForUser();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateItemCondition_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdOutput.RowCount > 0)
			{
				int SelectedItemConditionId = Conversion.ToInt(cmbItemCondition.Value);
				string SelectedItemCondition = Conversion.ToString((object)((Control)(object)cmbItemCondition).Text);
				GridEXRow[] rows = grdOutput.GetRows();
				foreach (GridEXRow row in rows)
				{
					row.BeginEdit();
					if (Conversion.ToInt(row.Cells["ItemConditionId"].Value) == 0)
					{
						row.Cells["ItemConditionId"].Value = SelectedItemConditionId;
						row.Cells["ItemCondition"].Value = SelectedItemCondition;
					}
					row.EndEdit();
				}
			}
			grdOutput.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbParentCategory_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbEntryType.Value) == 1)
			{
				BindInputItem();
			}
			else if (Conversion.ToInt(cmbEntryType.Value) == 2)
			{
				ItemFill();
				ItemNameBind();
			}
			else
			{
				((Control)(object)cmbItem).Text = string.Empty;
				((UltraGridBase)cmbItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutput_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOutput.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdOutput.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Delete && btnsave.Visible && btnsave.Enabled)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				r.Delete();
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
			int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
			int RackId = Conversion.ToInt(r.Cells["RackId"].Value);
			if (grdOutput.CurrentColumn.Key == "ItemConditionId" || grdOutput.CurrentColumn.Key == "ItemCondition")
			{
				GrdPopUp grdPopUp = new GrdPopUp(DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalItemConditions.ToList(), (GlobalItemCondition x) => x.Id, (GlobalItemCondition x) => x.ConditionStatus), "Id", "Description");
				grdPopUp.StartPosition = FormStartPosition.CenterScreen;
				grdPopUp.ShowDialog();
				grdPopUp.BringToFront();
				int SelectedId = Conversion.ToInt((object)grdPopUp.ReturnId);
				string SelectedName = Conversion.ToString((object)grdPopUp.ReturnName);
				r.Cells["ItemConditionId"].Value = SelectedId;
				r.Cells["ItemCondition"].Value = SelectedName;
				grdOutput.UpdateData();
			}
			else if (grdOutput.CurrentColumn.Key == "WareHouseId" || grdOutput.CurrentColumn.Key == "WareHouse")
			{
				List<RacksWithWarehouseAndItem> list = (from x in clsGlobalVariables.racksWithWarehouseAndItems
					where x.ItemId == ItemId
					group x by x.WarehouseId into g
					select g.First()).ToList();
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("WareHouseName", typeof(string));
				dt.BeginLoadData();
				foreach (RacksWithWarehouseAndItem w in list)
				{
					dt.Rows.Add(w.WarehouseId, w.WareHouseName);
				}
				dt.EndLoadData();
				using (GrdPopUp obj = new GrdPopUp(dt, "Id", "WareHouseName"))
				{
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					int selectedId2 = Conversion.ToInt((object)obj.ReturnId);
					string selectedName = Conversion.ToString((object)obj.ReturnName);
					r.Cells["WareHouseId"].Value = selectedId2;
					r.Cells["WareHouse"].Value = selectedName;
					List<RacksWithWarehouseAndItem> racks = clsGlobalVariables.racksWithWarehouseAndItems.Where((RacksWithWarehouseAndItem x) => x.ItemId == ItemId && x.WarehouseId == selectedId2).ToList();
					if (RackId > 0)
					{
						if (!racks.Any((RacksWithWarehouseAndItem x) => x.Id == RackId))
						{
							if (racks.Count > 1)
							{
								r.Cells["RackId"].Value = 0;
								r.Cells["RackName"].Value = "";
							}
							else if (racks.Count == 1)
							{
								RacksWithWarehouseAndItem singleRack = racks.First();
								r.Cells["RackId"].Value = singleRack.Id;
								r.Cells["RackName"].Value = singleRack.RackName;
							}
							else
							{
								r.Cells["RackId"].Value = 0;
								r.Cells["RackName"].Value = "";
							}
						}
					}
					else if (racks.Count == 1)
					{
						RacksWithWarehouseAndItem singleRack2 = racks.First();
						r.Cells["RackId"].Value = singleRack2.Id;
						r.Cells["RackName"].Value = singleRack2.RackName;
					}
				}
				grdOutput.UpdateData();
			}
			else
			{
				if (!(grdOutput.CurrentColumn.Key == "RackId") && !(grdOutput.CurrentColumn.Key == "RackName"))
				{
					return;
				}
				List<RacksWithWarehouseAndItem> filteredRacks = (from rr in clsGlobalVariables.racksWithWarehouseAndItems
					where rr.ItemId == ItemId && (WarehouseId == 0 || rr.WarehouseId == WarehouseId)
					select rr into row
					group row by row.Id into g
					select g.First()).ToList();
				if (filteredRacks.Count == 0)
				{
					return;
				}
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("Id", typeof(int));
				dt2.Columns.Add("RackName", typeof(string));
				dt2.Columns.Add("WarehouseId", typeof(int));
				dt2.Columns.Add("WarehouseName", typeof(string));
				dt2.BeginLoadData();
				foreach (RacksWithWarehouseAndItem item in filteredRacks)
				{
					dt2.Rows.Add(item.Id, item.RackName, item.WarehouseId, item.WareHouseName);
				}
				dt2.EndLoadData();
				using (GrdPopUp grdPopUp2 = new GrdPopUp(dt2, "Id", "RackName", "WarehouseName"))
				{
					grdPopUp2.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp2.ShowDialog();
					int selectedId = Conversion.ToInt((object)grdPopUp2.ReturnId);
					string selectedName2 = Conversion.ToString((object)grdPopUp2.ReturnName);
					r.Cells["RackId"].Value = selectedId;
					r.Cells["RackName"].Value = selectedName2;
					if (WarehouseId == 0)
					{
						RacksWithWarehouseAndItem selected = filteredRacks.FirstOrDefault((RacksWithWarehouseAndItem x) => x.Id == selectedId);
						if (selected != null)
						{
							r.Cells["WareHouseId"].Value = selected.WarehouseId;
							r.Cells["WareHouse"].Value = selected.WareHouseName;
						}
					}
				}
				grdOutput.UpdateData();
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
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Expected O, but got Unknown
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Expected O, but got Unknown
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Expected O, but got Unknown
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Expected O, but got Unknown
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Expected O, but got Unknown
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Expected O, but got Unknown
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Expected O, but got Unknown
		//IL_0692: Unknown result type (might be due to invalid IL or missing references)
		//IL_069c: Expected O, but got Unknown
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Expected O, but got Unknown
		//IL_138c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1396: Expected O, but got Unknown
		//IL_13a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ad: Expected O, but got Unknown
		//IL_1df1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dfb: Expected O, but got Unknown
		//IL_1e08: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e12: Expected O, but got Unknown
		//IL_5e63: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e6d: Expected O, but got Unknown
		GridEXLayout grdOutput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInput_DesignTimeLayout = new GridEXLayout();
		Appearance appearance32 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production.StoreStockConversion));
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel12 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.txtOutputByProductRate = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtOutputByProductAmount = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtOutputByProductQty = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.panel24 = new System.Windows.Forms.Panel();
		this.label22 = new System.Windows.Forms.Label();
		this.btnUpdateItemCondition = new System.Windows.Forms.Button();
		this.label15 = new System.Windows.Forms.Label();
		this.grdOutput = new GridEX();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label9 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtInputAmount = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.txtInputQuantity = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtInputRate = new System.Windows.Forms.TextBox();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label49 = new System.Windows.Forms.Label();
		this.BtnForwardRowsFromInputToOutPut = new System.Windows.Forms.Button();
		this.label14 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdInput = new GridEX();
		this.label8 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.chkPrint = new System.Windows.Forms.CheckBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.txtdocnumber = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.txtProductionNo = new System.Windows.Forms.TextBox();
		this.cmbParentCategory = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDocdate = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbRackName = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.CmbPackUom = new UltraCombo();
		this.label17 = new System.Windows.Forms.Label();
		this.cmbItemCondition = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbEntryType = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.txtdeailRemarks = new System.Windows.Forms.TextBox();
		this.cmbGodown = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.Add = new System.Windows.Forms.Button();
		this.cmbLot = new UltraCombo();
		this.cmbItem = new UltraCombo();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtStock = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.txtAverageRate = new System.Windows.Forms.TextBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnGenerate = new System.Windows.Forms.ToolStripButton();
		this.btnIssuanceLoad = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DatagridHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label20 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label42 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.label45 = new System.Windows.Forms.Label();
		this.btnshowHistory = new System.Windows.Forms.Button();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label46 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnHNew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label41 = new System.Windows.Forms.Label();
		this.grdDetail = new GridEX();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel12.SuspendLayout();
		this.groupBox4.SuspendLayout();
		this.panel24.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutput).BeginInit();
		this.panel5.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInput).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbParentCategory).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRackName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCondition).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEntryType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGodown).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel9.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1298, 684);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1298, 684);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add(this.panel12);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1290, 658);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.Click += new System.EventHandler(tabPage1_Click);
		this.panel12.Controls.Add(this.groupBox4);
		this.panel12.Controls.Add(this.panel24);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 466);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1284, 206);
		this.panel12.TabIndex = 4;
		this.groupBox4.Controls.Add(this.txtOutputByProductRate);
		this.groupBox4.Controls.Add(this.label11);
		this.groupBox4.Controls.Add(this.txtOutputByProductAmount);
		this.groupBox4.Controls.Add(this.label12);
		this.groupBox4.Controls.Add(this.txtOutputByProductQty);
		this.groupBox4.Controls.Add(this.label13);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(1103, 48);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(171, 115);
		this.groupBox4.TabIndex = 81;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Output Summary";
		this.txtOutputByProductRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOutputByProductRate.Enabled = false;
		this.txtOutputByProductRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutputByProductRate.Location = new System.Drawing.Point(63, 54);
		this.txtOutputByProductRate.Name = "txtOutputByProductRate";
		this.txtOutputByProductRate.Size = new System.Drawing.Size(98, 23);
		this.txtOutputByProductRate.TabIndex = 6;
		this.txtOutputByProductRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(7, 33);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(54, 13);
		this.label11.TabIndex = 78;
		this.label11.Text = "Quantity";
		this.txtOutputByProductAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOutputByProductAmount.Enabled = false;
		this.txtOutputByProductAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutputByProductAmount.Location = new System.Drawing.Point(63, 79);
		this.txtOutputByProductAmount.Name = "txtOutputByProductAmount";
		this.txtOutputByProductAmount.Size = new System.Drawing.Size(98, 23);
		this.txtOutputByProductAmount.TabIndex = 7;
		this.txtOutputByProductAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(8, 59);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(34, 13);
		this.label12.TabIndex = 79;
		this.label12.Text = "Rate";
		this.txtOutputByProductQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOutputByProductQty.Enabled = false;
		this.txtOutputByProductQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutputByProductQty.Location = new System.Drawing.Point(63, 28);
		this.txtOutputByProductQty.Name = "txtOutputByProductQty";
		this.txtOutputByProductQty.Size = new System.Drawing.Size(98, 23);
		this.txtOutputByProductQty.TabIndex = 4;
		this.txtOutputByProductQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(8, 84);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(49, 13);
		this.label13.TabIndex = 80;
		this.label13.Text = "Amount";
		this.panel24.Controls.Add(this.label22);
		this.panel24.Controls.Add(this.btnUpdateItemCondition);
		this.panel24.Controls.Add(this.label15);
		this.panel24.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutput);
		this.panel24.Controls.Add(this.ctrlGrdBar1);
		this.panel24.Controls.Add(this.label9);
		this.panel24.Location = new System.Drawing.Point(0, 0);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1097, 206);
		this.panel24.TabIndex = 0;
		this.label22.BackColor = System.Drawing.Color.Teal;
		this.label22.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.Yellow;
		this.label22.Location = new System.Drawing.Point(92, 6);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(375, 19);
		this.label22.TabIndex = 176;
		this.label22.Text = "Press Update button to Update Selected Item Condition";
		this.btnUpdateItemCondition.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnUpdateItemCondition.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateItemCondition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateItemCondition.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateItemCondition.ForeColor = System.Drawing.Color.Yellow;
		this.btnUpdateItemCondition.Location = new System.Drawing.Point(992, 1);
		this.btnUpdateItemCondition.Name = "btnUpdateItemCondition";
		this.btnUpdateItemCondition.Size = new System.Drawing.Size(62, 23);
		this.btnUpdateItemCondition.TabIndex = 175;
		this.btnUpdateItemCondition.Text = "Update";
		this.btnUpdateItemCondition.UseVisualStyleBackColor = false;
		this.btnUpdateItemCondition.Click += new System.EventHandler(btnUpdateItemCondition_Click);
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(6, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(81, 16);
		this.label15.TabIndex = 174;
		this.label15.Text = "OutPut Grid";
		this.grdOutput.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdOutput).BackColor = System.Drawing.Color.White;
		grdOutput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutput.DesignTimeLayout = grdOutput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOutput.FilterMode = (FilterMode)2;
		this.grdOutput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOutput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOutput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOutput).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdOutput).Name = "grdOutput";
		((ScrollableBase)this.grdOutput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutput).Size = new System.Drawing.Size(1097, 180);
		((System.Windows.Forms.Control)(object)this.grdOutput).TabIndex = 0;
		this.grdOutput.TotalRow = (InheritableBoolean)1;
		this.grdOutput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutput).VisualStyle = (VisualStyle)4;
		this.grdOutput.CellUpdated += new ColumnActionEventHandler(grdByProduct_CellUpdated);
		this.grdOutput.ColumnButtonClick += new ColumnActionEventHandler(grdByProduct_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOutput).DoubleClick += new System.EventHandler(grdByProduct_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdOutput).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOutput_KeyDown);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1056, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdOutput;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label9.BackColor = System.Drawing.Color.Teal;
		this.label9.Dock = System.Windows.Forms.DockStyle.Top;
		this.label9.Location = new System.Drawing.Point(0, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(1097, 26);
		this.label9.TabIndex = 2;
		this.panel5.Controls.Add(this.groupBox2);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 260);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1284, 206);
		this.panel5.TabIndex = 3;
		this.groupBox2.Controls.Add(this.txtInputAmount);
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add(this.txtInputQuantity);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add(this.label33);
		this.groupBox2.Controls.Add(this.txtInputRate);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(1102, 50);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(173, 114);
		this.groupBox2.TabIndex = 78;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Input Summary";
		this.txtInputAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInputAmount.Enabled = false;
		this.txtInputAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputAmount.Location = new System.Drawing.Point(64, 78);
		this.txtInputAmount.Name = "txtInputAmount";
		this.txtInputAmount.Size = new System.Drawing.Size(98, 23);
		this.txtInputAmount.TabIndex = 3;
		this.txtInputAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(8, 83);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(49, 13);
		this.label35.TabIndex = 77;
		this.label35.Text = "Amount";
		this.txtInputQuantity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInputQuantity.Enabled = false;
		this.txtInputQuantity.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputQuantity.Location = new System.Drawing.Point(64, 27);
		this.txtInputQuantity.Name = "txtInputQuantity";
		this.txtInputQuantity.Size = new System.Drawing.Size(98, 23);
		this.txtInputQuantity.TabIndex = 0;
		this.txtInputQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(8, 57);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(34, 13);
		this.label36.TabIndex = 61;
		this.label36.Text = "Rate";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(8, 32);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(54, 13);
		this.label33.TabIndex = 61;
		this.label33.Text = "Quantity";
		this.txtInputRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInputRate.Enabled = false;
		this.txtInputRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputRate.Location = new System.Drawing.Point(64, 52);
		this.txtInputRate.Name = "txtInputRate";
		this.txtInputRate.Size = new System.Drawing.Size(98, 23);
		this.txtInputRate.TabIndex = 2;
		this.txtInputRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.panel6.Controls.Add(this.label49);
		this.panel6.Controls.Add(this.BtnForwardRowsFromInputToOutPut);
		this.panel6.Controls.Add(this.label14);
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdInput);
		this.panel6.Controls.Add(this.label8);
		this.panel6.Location = new System.Drawing.Point(0, 2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1097, 204);
		this.panel6.TabIndex = 22;
		this.label49.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label49.AutoSize = true;
		this.label49.BackColor = System.Drawing.Color.Teal;
		this.label49.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label49.ForeColor = System.Drawing.Color.White;
		this.label49.Location = new System.Drawing.Point(482, 4);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(536, 17);
		this.label49.TabIndex = 174;
		this.label49.Text = "Click the Forward_Button to Transfer Selected Input Rows Into Output Grid";
		this.BtnForwardRowsFromInputToOutPut.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnForwardRowsFromInputToOutPut.BackColor = System.Drawing.Color.Transparent;
		this.BtnForwardRowsFromInputToOutPut.FlatAppearance.BorderSize = 0;
		this.BtnForwardRowsFromInputToOutPut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnForwardRowsFromInputToOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnForwardRowsFromInputToOutPut.ForeColor = System.Drawing.Color.Transparent;
		this.BtnForwardRowsFromInputToOutPut.Image = Architecture.WinApp.Properties.Resources.Forward;
		this.BtnForwardRowsFromInputToOutPut.Location = new System.Drawing.Point(1018, 0);
		this.BtnForwardRowsFromInputToOutPut.Margin = new System.Windows.Forms.Padding(2);
		this.BtnForwardRowsFromInputToOutPut.Name = "BtnForwardRowsFromInputToOutPut";
		this.BtnForwardRowsFromInputToOutPut.Size = new System.Drawing.Size(35, 25);
		this.BtnForwardRowsFromInputToOutPut.TabIndex = 175;
		this.BtnForwardRowsFromInputToOutPut.UseVisualStyleBackColor = false;
		this.BtnForwardRowsFromInputToOutPut.Click += new System.EventHandler(BtnForwardRowsFromInputToOutPut_Click);
		this.label14.AutoSize = true;
		this.label14.BackColor = System.Drawing.Color.Teal;
		this.label14.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(8, 5);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(71, 16);
		this.label14.TabIndex = 173;
		this.label14.Text = "Input Grid";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1055, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdInput;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		((ScrollableBase)this.grdInput).BackColor = System.Drawing.Color.White;
		grdInput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInput.DesignTimeLayout = grdInput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInput).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInput).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdInput).Name = "grdInput";
		((ScrollableBase)this.grdInput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInput).Size = new System.Drawing.Size(1097, 178);
		((System.Windows.Forms.Control)(object)this.grdInput).TabIndex = 0;
		this.grdInput.TotalRow = (InheritableBoolean)1;
		this.grdInput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInput).VisualStyle = (VisualStyle)4;
		this.grdInput.CellUpdated += new ColumnActionEventHandler(grdInput_CellUpdated);
		this.grdInput.ColumnButtonClick += new ColumnActionEventHandler(grdStockConversionProductionDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInput).DoubleClick += new System.EventHandler(grdInput_DoubleClick);
		this.label8.BackColor = System.Drawing.Color.Teal;
		this.label8.Dock = System.Windows.Forms.DockStyle.Top;
		this.label8.Location = new System.Drawing.Point(0, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(1097, 26);
		this.label8.TabIndex = 3;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.panel3);
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.txtStock);
		this.panel4.Controls.Add(this.label28);
		this.panel4.Controls.Add(this.label30);
		this.panel4.Controls.Add(this.txtAverageRate);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 33);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1284, 227);
		this.panel4.TabIndex = 0;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label16);
		this.panel3.Controls.Add(this.chkPrint);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1284, 33);
		this.panel3.TabIndex = 178;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(10, 7);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(703, 19);
		this.label16.TabIndex = 0;
		this.label16.Text = "Packing Material / General Store Items Conversion From Department To Department";
		this.chkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrint.AutoSize = true;
		this.chkPrint.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrint.ForeColor = System.Drawing.Color.White;
		this.chkPrint.Location = new System.Drawing.Point(1180, 7);
		this.chkPrint.Name = "chkPrint";
		this.chkPrint.Size = new System.Drawing.Size(102, 19);
		this.chkPrint.TabIndex = 5;
		this.chkPrint.Text = "Print Preview";
		this.chkPrint.UseVisualStyleBackColor = true;
		this.groupBox3.Controls.Add(this.txtRemarks);
		this.groupBox3.Controls.Add(this.txtdocnumber);
		this.groupBox3.Controls.Add(this.label27);
		this.groupBox3.Controls.Add(this.txtProductionNo);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbParentCategory);
		this.groupBox3.Controls.Add(this.label6);
		this.groupBox3.Controls.Add(this.label1);
		this.groupBox3.Controls.Add(this.txtDocdate);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(7, 32);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1090, 71);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Main";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(526, 16);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(553, 51);
		this.txtRemarks.TabIndex = 4;
		this.txtdocnumber.BackColor = System.Drawing.Color.White;
		this.txtdocnumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocnumber.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocnumber.Location = new System.Drawing.Point(69, 16);
		this.txtdocnumber.Name = "txtdocnumber";
		this.txtdocnumber.ReadOnly = true;
		this.txtdocnumber.Size = new System.Drawing.Size(133, 23);
		this.txtdocnumber.TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(209, 47);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(91, 15);
		this.label27.TabIndex = 176;
		this.label27.Text = "Parent Category";
		this.txtProductionNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProductionNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionNo.Location = new System.Drawing.Point(309, 16);
		this.txtProductionNo.Name = "txtProductionNo";
		this.txtProductionNo.Size = new System.Drawing.Size(143, 23);
		this.txtProductionNo.TabIndex = 2;
		this.cmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance51).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Location = new System.Drawing.Point(309, 41);
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Name = "cmbParentCategory";
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Size = new System.Drawing.Size(143, 26);
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Leave += new System.EventHandler(cmbParentCategory_Leave);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(463, 19);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(52, 15);
		this.label6.TabIndex = 149;
		this.label6.Text = "Remarks";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(8, 20);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(48, 15);
		this.label1.TabIndex = 140;
		this.label1.Text = "Doc No";
		this.txtDocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDocdate.Location = new System.Drawing.Point(69, 43);
		this.txtDocdate.Name = "txtDocdate";
		this.txtDocdate.Size = new System.Drawing.Size(133, 23);
		this.txtDocdate.TabIndex = 1;
		this.txtDocdate.Leave += new System.EventHandler(txtDocdate_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(209, 20);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(85, 15);
		this.label3.TabIndex = 147;
		this.label3.Text = "Production No";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(8, 47);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(57, 15);
		this.label2.TabIndex = 145;
		this.label2.Text = "Doc Date";
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRackName);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCondition);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEntryType);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.txtdeailRemarks);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGodown);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.txtQty);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.txtRate);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.txtAmount);
		this.groupBox1.Controls.Add(this.Add);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLot);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Controls.Add(this.btnCancelDetail);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(6, 105);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1091, 118);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.CmbRackName.AllowNull = (DefaultableBoolean)1;
		this.CmbRackName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRackName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRackName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbRackName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRackName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance36;
		((SpecialBoxBase)((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbRackName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRackName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance45).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRackName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRackName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRackName).Location = new System.Drawing.Point(436, 39);
		((System.Windows.Forms.Control)(object)this.CmbRackName).Name = "CmbRackName";
		((System.Windows.Forms.Control)(object)this.CmbRackName).Size = new System.Drawing.Size(132, 26);
		((System.Windows.Forms.Control)(object)this.CmbRackName).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbRackName).Leave += new System.EventHandler(CmbRackName_Leave);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(436, 20);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(67, 15);
		this.label25.TabIndex = 249;
		this.label25.Text = "Rack Name";
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance76;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance91).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(708, 39);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(92, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 5;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(569, 21);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(87, 15);
		this.label17.TabIndex = 228;
		this.label17.Text = "Item Condition";
		this.cmbItemCondition.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCondition.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCondition.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCondition.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Appearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCondition).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance72;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCondition).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCondition).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemCondition.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCondition).Location = new System.Drawing.Point(569, 39);
		((System.Windows.Forms.Control)(object)this.cmbItemCondition).Name = "cmbItemCondition";
		((System.Windows.Forms.Control)(object)this.cmbItemCondition).Size = new System.Drawing.Size(138, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCondition).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbItemCondition).Leave += new System.EventHandler(cmbItemCondition_Leave);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(708, 21);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(33, 15);
		this.label4.TabIndex = 174;
		this.label4.Text = "Uom";
		this.cmbEntryType.AllowNull = (DefaultableBoolean)1;
		this.cmbEntryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEntryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEntryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEntryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance92;
		((SpecialBoxBase)((UltraGridBase)this.cmbEntryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance2).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance2).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance2).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance31).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEntryType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Location = new System.Drawing.Point(8, 39);
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Name = "cmbEntryType";
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Size = new System.Drawing.Size(89, 26);
		((System.Windows.Forms.Control)(object)this.cmbEntryType).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Leave += new System.EventHandler(cmbEntryType_Leave);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(801, 21);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(46, 15);
		this.label10.TabIndex = 155;
		this.label10.Text = "Job Lot";
		this.txtdeailRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeailRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdeailRemarks.Location = new System.Drawing.Point(101, 84);
		this.txtdeailRemarks.Name = "txtdeailRemarks";
		this.txtdeailRemarks.Size = new System.Drawing.Size(840, 23);
		this.txtdeailRemarks.TabIndex = 10;
		this.cmbGodown.AllowNull = (DefaultableBoolean)1;
		this.cmbGodown.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGodown.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGodown.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Appearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbGodown).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGodown).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGodown).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGodown).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance59;
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
		((System.Windows.Forms.Control)(object)this.cmbGodown).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbGodown.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGodown).Location = new System.Drawing.Point(296, 39);
		((System.Windows.Forms.Control)(object)this.cmbGodown).Name = "cmbGodown";
		((System.Windows.Forms.Control)(object)this.cmbGodown).Size = new System.Drawing.Size(139, 26);
		((System.Windows.Forms.Control)(object)this.cmbGodown).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbGodown).Leave += new System.EventHandler(cmbGodown_Leave);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(293, 21);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(142, 15);
		this.label5.TabIndex = 151;
		this.label5.Text = "Department / Warehouse";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(101, 65);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(86, 15);
		this.label21.TabIndex = 172;
		this.label21.Text = "Remarks Detail";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(907, 21);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(103, 15);
		this.label18.TabIndex = 159;
		this.label18.Text = "Quantity / Weight";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(922, 41);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(82, 23);
		this.txtQty.TabIndex = 7;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(6, 21);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(62, 15);
		this.label23.TabIndex = 167;
		this.label23.Text = "Entry Type";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(98, 21);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(67, 15);
		this.label7.TabIndex = 141;
		this.label7.Text = "Item Name";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(1009, 21);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(30, 15);
		this.label24.TabIndex = 161;
		this.label24.Text = "Rate";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRate.Location = new System.Drawing.Point(1005, 41);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(75, 23);
		this.txtRate.TabIndex = 8;
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(7, 65);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(51, 15);
		this.label26.TabIndex = 162;
		this.label26.Text = "Amount";
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Enabled = false;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(8, 84);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.Size = new System.Drawing.Size(91, 23);
		this.txtAmount.TabIndex = 9;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.Add.BackColor = System.Drawing.Color.Teal;
		this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Add.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.ForeColor = System.Drawing.Color.White;
		this.Add.Location = new System.Drawing.Point(942, 83);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(35, 25);
		this.Add.TabIndex = 11;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(AddInGrid_Click);
		this.cmbLot.AllowNull = (DefaultableBoolean)1;
		this.cmbLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance29).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance83).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLot).Location = new System.Drawing.Point(801, 39);
		((System.Windows.Forms.Control)(object)this.cmbLot).Name = "cmbLot";
		((System.Windows.Forms.Control)(object)this.cmbLot).Size = new System.Drawing.Size(119, 26);
		((System.Windows.Forms.Control)(object)this.cmbLot).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbLot).Leave += new System.EventHandler(cmbLot_Leave);
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance62;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(98, 39);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(197, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(1013, 83);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(65, 25);
		this.btnCancelDetail.TabIndex = 13;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(943, 83);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(70, 25);
		this.btnUpdateDetail.TabIndex = 12;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtStock.Enabled = false;
		this.txtStock.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStock.Location = new System.Drawing.Point(1168, 144);
		this.txtStock.Name = "txtStock";
		this.txtStock.Size = new System.Drawing.Size(106, 23);
		this.txtStock.TabIndex = 5;
		this.txtStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(1100, 149);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(37, 15);
		this.label28.TabIndex = 163;
		this.label28.Text = "Stock";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(1100, 170);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(68, 15);
		this.label30.TabIndex = 165;
		this.label30.Text = "AvgRate Kg";
		this.txtAverageRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAverageRate.Enabled = false;
		this.txtAverageRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAverageRate.Location = new System.Drawing.Point(1168, 168);
		this.txtAverageRate.Name = "txtAverageRate";
		this.txtAverageRate.Size = new System.Drawing.Size(106, 23);
		this.txtAverageRate.TabIndex = 11;
		this.txtAverageRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1284, 30);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.Print, this.btnGenerate, this.btnIssuanceLoad });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1284, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.Print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "665-&Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnGenerate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGenerate.Image = Architecture.WinApp.Properties.Resources.icons8_copy_16;
		this.btnGenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerate.Name = "btnGenerate";
		this.btnGenerate.Size = new System.Drawing.Size(91, 24);
		this.btnGenerate.Text = "Generate";
		this.btnGenerate.Click += new System.EventHandler(btnGenerate_Click);
		this.btnIssuanceLoad.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnIssuanceLoad.Image = Architecture.WinApp.Properties.Resources.download;
		this.btnIssuanceLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnIssuanceLoad.Name = "btnIssuanceLoad";
		this.btnIssuanceLoad.Size = new System.Drawing.Size(126, 24);
		this.btnIssuanceLoad.Text = "Issuance Load";
		this.btnIssuanceLoad.Click += new System.EventHandler(btnIssuanceLoad_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.toolStrip2);
		this.tabPage2.Controls.Add(this.panel18);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1290, 658);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DatagridHistory.AllowEdit = (InheritableBoolean)2;
		DatagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistory.DesignTimeLayout = DatagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DatagridHistory.DynamicFiltering = true;
		this.DatagridHistory.FilterMode = (FilterMode)2;
		this.DatagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Location = new System.Drawing.Point(3, 112);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Name = "DatagridHistory";
		((ScrollableBase)this.DatagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(1284, 284);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 44;
		this.DatagridHistory.TotalRow = (InheritableBoolean)1;
		this.DatagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistory).VisualStyle = (VisualStyle)4;
		this.DatagridHistory.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.DatagridHistory.SelectionChanged += new System.EventHandler(DatagridHistory_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).DoubleClick += new System.EventHandler(DatagridHistory_DoubleClick);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar3);
		this.panel7.Controls.Add(this.label20);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 86);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1284, 26);
		this.panel7.TabIndex = 50;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = null;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1234, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = null;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(48, 22);
		this.ctrlGrdBar3.TabIndex = 72;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label20.ForeColor = System.Drawing.Color.White;
		this.label20.Location = new System.Drawing.Point(3, 3);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(132, 18);
		this.label20.TabIndex = 71;
		this.label20.Text = "Filtered Records";
		this.panel10.Controls.Add(this.ToDateHistory);
		this.panel10.Controls.Add(this.rdmodifydate);
		this.panel10.Controls.Add(this.drdocdate);
		this.panel10.Controls.Add(this.rdentrydate);
		this.panel10.Controls.Add(this.label42);
		this.panel10.Controls.Add(this.txtToDocNoHistory);
		this.panel10.Controls.Add(this.txtFromDocNoHistory);
		this.panel10.Controls.Add(this.label45);
		this.panel10.Controls.Add(this.btnshowHistory);
		this.panel10.Controls.Add(this.FromDateHistory);
		this.panel10.Controls.Add(this.label46);
		this.panel10.Controls.Add(this.label48);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 54);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1284, 32);
		this.panel10.TabIndex = 49;
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(251, 4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(135, 23);
		this.ToDateHistory.TabIndex = 1;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(839, 6);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 7;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(681, 6);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 5;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(756, 6);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 6;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(502, 8);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(39, 15);
		this.label42.TabIndex = 627;
		this.label42.Text = "To No";
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(541, 4);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(59, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(440, 4);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(59, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(389, 8);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(54, 15);
		this.label45.TabIndex = 626;
		this.label45.Text = "From No";
		this.btnshowHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshowHistory.FlatAppearance.BorderSize = 0;
		this.btnshowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshowHistory.ForeColor = System.Drawing.Color.White;
		this.btnshowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshowHistory.Location = new System.Drawing.Point(605, 2);
		this.btnshowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnshowHistory.Name = "btnshowHistory";
		this.btnshowHistory.Size = new System.Drawing.Size(68, 26);
		this.btnshowHistory.TabIndex = 4;
		this.btnshowHistory.Text = "Show";
		this.btnshowHistory.UseVisualStyleBackColor = false;
		this.btnshowHistory.Click += new System.EventHandler(btnshowHistory_Click);
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(68, 4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(135, 23);
		this.FromDateHistory.TabIndex = 0;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(206, 8);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(48, 15);
		this.label46.TabIndex = 624;
		this.label46.Text = "Date To";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(2, 8);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(63, 15);
		this.label48.TabIndex = 625;
		this.label48.Text = "Date From";
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label19);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 28);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1284, 26);
		this.panel9.TabIndex = 48;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(3, 3);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(151, 18);
		this.label19.TabIndex = 71;
		this.label19.Text = "Conversion History";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnHNew, this.toolStripButton4 });
		this.toolStrip2.Location = new System.Drawing.Point(3, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1284, 25);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnHNew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnHNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnHNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnHNew.Name = "btnHNew";
		this.btnHNew.Size = new System.Drawing.Size(55, 24);
		this.btnHNew.Text = "&New";
		this.btnHNew.Visible = false;
		this.btnHNew.Click += new System.EventHandler(btnHNew_Click);
		this.toolStripButton4.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(56, 24);
		this.toolStripButton4.Text = "&Print";
		this.toolStripButton4.Visible = false;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label41);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel18.Location = new System.Drawing.Point(3, 396);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1284, 25);
		this.panel18.TabIndex = 51;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label41.ForeColor = System.Drawing.Color.White;
		this.label41.Location = new System.Drawing.Point(4, 2);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(313, 17);
		this.label41.TabIndex = 71;
		this.label41.Text = "Input and Out Detail of Above Selected Row";
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 421);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1284, 234);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 45;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1298, 684);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "StoreStockConversion";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(StoreStockConversion_FormClosing);
		base.Load += new System.EventHandler(invfrmStockConversionProduction_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(invfrmStockConversionProduction_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		this.panel24.ResumeLayout(false);
		this.panel24.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutput).EndInit();
		this.panel5.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInput).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbParentCategory).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRackName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCondition).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEntryType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGodown).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		base.ResumeLayout(false);
	}
}

}
