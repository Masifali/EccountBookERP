using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.BLL.PurchaseTrading;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.PurchaseTrading;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory_Stocks_Report;
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
namespace Architecture.WinApp.Export
{

public class PackingMaterialIssuanceForShipment : Form
{
	private int RECID;

	private int VoucherHeadId;

	private int updateDetailIndex = -1;

	private DataTable table = new DataTable();

	private DataTable dtItem = new DataTable();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControlGsIssuance;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel6;

	private Panel panel5;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton Print;

	private Label label15;

	private DateTimePicker txtDocdate;

	private Label label1;

	private Label label5;

	private TextBox txtDocNo;

	internal GridEX grdDetail;

	internal GridEX DataGridHistory;

	private ToolStripButton btnRefresh;

	internal UltraCombo cmbItem;

	private Label label9;

	private TextBox txtItemQty;

	private Label label18;

	private Label label6;

	internal UltraCombo CmbWarehouse;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private Button Add;

	private TextBox txtBalanceQty;

	private Label label8;

	private Label label13;

	private TextBox txtRemarksDetail;

	private TextBox txtItemAmount;

	private Label label12;

	private TextBox txtItemRate;

	private Label label10;

	private ToolStripButton btnVoucher;

	internal UltraCombo cmbDrAc;

	private Label label16;

	private ToolStripButton btnShortcutkeys;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	private Panel panel8;

	private Label label17;

	private Panel panel9;

	private Label label19;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grddetailofmain;

	private Panel panel12;

	private Label label20;

	private CtrlGrdBar ctrlGrdBar2;

	internal UltraCombo CmbInvoiceNo;

	private Label label3;

	private ToolStripButton btnAttachment;

	private GroupBox groupBox3;

	internal UltraCombo CmbContractNo;

	internal UltraCombo CmbPortTo;

	private Label label2;

	internal UltraCombo CmbPortFrom;

	private Label label4;

	private Label label14;

	private TextBox txtnoofcontainer;

	private Label label7;

	private Label label21;

	private TextBox txtnetweight;

	internal UltraCombo CmbCustomer;

	internal UltraCombo CmbScheduleNo;

	private Label label22;

	private Label label11;

	private Button BtnStockReport;

	internal UltraCombo CmbItemCondition;

	private Label label29;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private Panel panel16;

	private GroupBox groupBox4;

	private Button btnShowHistory;

	private Label label54;

	private Label label55;

	private Label label56;

	private Label label57;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel7;

	private Label label23;

	private TextBox txtRemarks;

	internal UltraCombo CmbRackName;

	private Label label24;

	public PackingMaterialIssuanceForShipment(UserAccount user)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmGSIssuance_Load(object sender, EventArgs e)
	{
		try
		{
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnVoucher.Enabled = formright.DoHavePrintRights;
			Print.Enabled = formright.DoHavePrintRights;
			FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formright);
			FormHelper.SetCtrlGrdBarRight(ctrlGrdBar2, formright);
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("ItemName", typeof(string));
			table.Columns.Add("WareHouseId", typeof(int));
			table.Columns.Add("WareHouseName", typeof(string));
			table.Columns.Add("RackId", typeof(int));
			table.Columns.Add("RackName", typeof(string));
			table.Columns.Add("ItemConditionId", typeof(int));
			table.Columns.Add("ItemCondition", typeof(string));
			table.Columns.Add("ItemQty", typeof(double));
			table.Columns.Add("ItemRate", typeof(double));
			table.Columns.Add("ItemAmount", typeof(double));
			table.Columns.Add("Remarks", typeof(string));
			table.Columns.Add("DrAcId", typeof(int));
			table.Columns.Add("DebitAc", typeof(string));
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			dtItem.Columns.Add("ItemCode");
			dtItem.Columns.Add("ItemCategory");
			dtItem.Columns.Add("LeadTimeDay", typeof(int));
			dtItem.Columns.Add("WeightCapacity");
			dtItem.Columns.Add("ParentCategoryId", typeof(int));
			GenerateDocNo();
			InvoiceNoFill();
			ItemDtsFillFromGlobal();
			ItemNameBind();
			DebitAcDetailFill();
			ItemConditionBindFromGlobal();
			PortFrom();
			txtDocdate.Focus();
			BtnStockReport.Visible = true;
			BtnStockReport.Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateDocNo()
	{
		try
		{
			int code = CommonServices.StoreIssuanceGenerateCode(213);
			if (code > 0)
			{
				txtDocNo.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceNoFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(CmbInvoiceNo.Value);
			DataTable dtInvoice = ExImInvoice.getExportInvoicePendingAndAll(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ActionId = 1,
				RecId = RECID
			});
			DDL.BindDDLNew(dtInvoice, CmbInvoiceNo, "Id", "InvoiceNo", "Invoice No", true);
			if (id > 0)
			{
				if (dtInvoice.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbInvoiceNo.Value = id;
				}
				else
				{
					((Control)(object)CmbInvoiceNo).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDtsFillFromGlobal(int CategoryOrTypeId = 0)
	{
		try
		{
			dtItem.Clear();
			IEnumerable<getGlobalAllItems> allItems = clsGlobalVariables.getGlobalAllItems.Where((getGlobalAllItems item) => item.ItemTypeOfTypeId == 14 || item.ItemTypeOfTypeId == 17);
			if (allItems == null || !allItems.Any())
			{
				return;
			}
			foreach (getGlobalAllItems row in allItems)
			{
				dtItem.Rows.Add(row.Id, row.ItemName, row.ItemCode, row.ItemCategory, row.LeadTimeDay, row.WeightCapacity, row.InventoryParentCategoriesId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading items: " + ex.Message);
		}
	}

	private void ItemNameBind()
	{
		try
		{
			int Id = Conversion.ToInt(cmbItem.Value);
			InfragisticsHelper.BindAndRetainSelection(cmbItem, dtItem, "Id", "ItemName", "Item Name", AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			if (dtItem.Rows.Count > 0)
			{
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[6].Hidden = true;
			}
			cmbItem_Leave(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(cmbItem.Value);
			DataTable dtitem = Item.GetItemByItemTypeId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Ids = "14,17"
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("ItemName");
			if (dtitem.Rows.Count > 0)
			{
				for (int i = 0; i < dtitem.Rows.Count; i++)
				{
					dt.Rows.Add(dtitem.Rows[i]["Id"], dtitem.Rows[i]["ItemName"]);
				}
			}
			DDL.BindDDL(dt, cmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
			((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 250;
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbItem.Value = id;
				}
				else
				{
					((Control)(object)cmbItem).Text = string.Empty;
				}
			}
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
			List<GlobalItemCondition> list = clsGlobalVariables.globalItemConditions.Where((GlobalItemCondition r) => r.Id != 4).ToList();
			int Id = Conversion.ToInt(CmbItemCondition.Value);
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(list, (GlobalItemCondition x) => x.Id, (GlobalItemCondition x) => x.ConditionStatus);
			InfragisticsHelper.BindAndRetainSelection(CmbItemCondition, dt, "Id", "Description", "Item Condition", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DebitAcDetailFill()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds();
			InfragisticsHelper.BindAndRetainSelection(cmbDrAc, dt, "Id", "AccountTitle", "Debit Account", AllColumns: true, Conversion.ToInt(cmbDrAc.Value), insertDefaultRow: true);
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
			VoucherHeadId = CommonServices.VoucherHeadIdGet(RECID, 213);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PortFrom()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int LoadPortId = Conversion.ToInt(CmbPortFrom.Value);
			int DestPortId = Conversion.ToInt(CmbPortTo.Value);
			DataTable dt = SeaPorts.Getall(new SeaPorts
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId
			});
			DataTable dtPort = new DataTable();
			dtPort.Columns.Add("Id", typeof(int));
			dtPort.Columns.Add("PortName");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtPort.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["PortName"]);
				}
			}
			DDL.BindDDL(dtPort, CmbPortFrom, "Id", "PortName", "Loading Place", ZeroIndex: true);
			DDL.BindDDL(dtPort, CmbPortTo, "Id", "PortName", "Destination Place", ZeroIndex: true);
			if (LoadPortId > 0)
			{
				if (dtPort.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == LoadPortId))
				{
					CmbPortFrom.Value = LoadPortId;
				}
				else
				{
					((Control)(object)CmbPortFrom).Text = string.Empty;
				}
			}
			if (DestPortId > 0)
			{
				if (dtPort.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DestPortId))
				{
					CmbPortTo.Value = DestPortId;
				}
				else
				{
					((Control)(object)CmbPortTo).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceInfo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImInvoice.GetInvoiceInformationByInvoiceIdForServices(new ExImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(CmbInvoiceNo.Value)
			});
			if (dt.Rows.Count > 0)
			{
				CmbPortFrom.Value = Conversion.ToInt(dt.Rows[0]["LoadingPortId"]);
				CmbPortTo.Value = Conversion.ToInt(dt.Rows[0]["DestinationPortId"]);
				double MTon = Conversion.ToDouble(dt.Rows[0]["NetWeight"]) / 1000.0;
				txtnoofcontainer.Text = Conversion.ToString(dt.Rows[0]["NoOfContainers"]);
				txtnetweight.Text = MTon.ToString("#,##0.###");
				DDL.BindDDLNew(dt, CmbContractNo, "LcOrderNoId", "LcOrderNo", "ContractNo", false);
				((UltraGridBase)CmbContractNo).Rows[0].Activate();
				DDL.BindDDLNew(dt, CmbScheduleNo, "ContractScheduleId", "ContractScheduleNo", "ContractScheduleNo", false);
				((UltraGridBase)CmbScheduleNo).Rows[0].Activate();
				DDL.BindDDLNew(dt, CmbCustomer, "SupplierCustomerId", "CustomerName", "CustomerName", false);
				((UltraGridBase)CmbCustomer).Rows[0].Activate();
				if (Conversion.ToInt(dt.Rows[0]["CreditAccountId"]) > 0)
				{
					cmbDrAc.Value = Conversion.ToInt(dt.Rows[0]["CreditAccountId"]);
				}
			}
			else
			{
				((Control)(object)CmbPortFrom).Text = string.Empty;
				((Control)(object)CmbPortTo).Text = string.Empty;
				txtnoofcontainer.Text = string.Empty;
				((Control)(object)CmbContractNo).Text = string.Empty;
				((Control)(object)CmbScheduleNo).Text = string.Empty;
				((Control)(object)CmbCustomer).Text = string.Empty;
				((Control)(object)cmbDrAc).Text = string.Empty;
				txtnetweight.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDocdate_Leave(object sender, EventArgs e)
	{
		try
		{
			BalanceStockQtyandAvgRate();
			getAvgRate();
			if (grdDetail.GetRows().Count() > 0)
			{
				AvgRateUpdateOnDocDateChange();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbInvoiceNo_Leave(object sender, EventArgs e)
	{
		try
		{
			InvoiceInfo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbWarehouse_Leave(object sender, EventArgs e)
	{
		try
		{
			int ItemId = Conversion.ToInt(cmbItem.Value);
			RackBindFromGlobalRacksByItemId(ItemId, Conversion.ToInt(CmbWarehouse.Value));
			BalanceStockQtyandAvgRate();
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
			int ItemId = Conversion.ToInt(cmbItem.Value);
			BalanceStockQtyandAvgRate();
			getAvgRate();
			BindWarehouseDropdown(CmbWarehouse, ItemId);
			RackBindFromGlobalRacksByItemId(ItemId, Conversion.ToInt(CmbWarehouse.Value));
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
			int configWarehouseId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PackingMaterialDefaultWarehouse"));
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
			int currentWarehouseId = Conversion.ToInt(CmbWarehouse.Value);
			RacksWithWarehouseAndItem rack = clsGlobalVariables.racksWithWarehouseAndItems.FirstOrDefault((RacksWithWarehouseAndItem r) => r.Id == rackId);
			if (rack != null)
			{
				if (currentWarehouseId == 0 || rack.WarehouseId != currentWarehouseId)
				{
					CmbWarehouse.Value = rack.WarehouseId;
				}
				BalanceStockQtyandAvgRate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemCondition_Leave(object sender, EventArgs e)
	{
		try
		{
			BalanceStockQtyandAvgRate();
			getAvgRate();
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
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			InvGsStoreIssuanceHeader obj = new InvGsStoreIssuanceHeader();
			if (RECID > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RECID;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.DocNo = Conversion.ToInt((object)txtDocNo.Text);
			obj.DocDate = Conversion.ToDateTime((object)txtDocdate.Value);
			obj.DocumentTypeId = 213;
			obj.Remarks = txtRemarks.Text;
			obj.EntryDate = DateTime.Now;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyDate = DateTime.Now;
			obj.ModifyUser = UserAccount.ID;
			obj.ActionForVoucherId = 1;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.ExImInvoiceId = Conversion.ToInt(CmbInvoiceNo.Value);
			obj.IsApproved = false;
			obj.gsStoreIssuanceDetails = new List<InvGsStoreIssuanceDetail>();
			if (grdDetail.GetRows().Count() == 0)
			{
				throw new Exception("Grid record not found");
			}
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToDouble(r.Cells["ItemQty"].Value) > 0.0)
				{
					InvGsStoreIssuanceDetail detail = new InvGsStoreIssuanceDetail();
					detail.LineId = r.RowIndex + 1;
					detail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					detail.Warehouseid = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					detail.RackId = Conversion.ToInt(r.Cells["RackId"].Value);
					detail.ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
					detail.IssueQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
					detail.ItemRate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
					detail.ItemAmount = Conversion.ToDouble(r.Cells["ItemAmount"].Value);
					detail.ReamarksDetail = Conversion.ToString((object)r.Cells["Remarks"].Text);
					detail.DrAccountId = Conversion.ToInt(r.Cells["DrAcId"].Value);
					FormHelper.ValidateField(detail.ItemId, "Item Name", r.RowIndex);
					FormHelper.ValidateField(detail.Warehouseid, "Warehouse", r.RowIndex);
					FormHelper.ValidateField(detail.RackId, "Rack Name", r.RowIndex);
					FormHelper.ValidateField(detail.ItemConditionId, "Item Condition", r.RowIndex);
					FormHelper.ValidateField(detail.IssueQty, "Item Qty", r.RowIndex);
					FormHelper.ValidateField(detail.ItemRate, "Item Rate", r.RowIndex);
					FormHelper.ValidateField(detail.ItemAmount, "Item Amount", r.RowIndex);
					obj.gsStoreIssuanceDetails.Add(detail);
				}
			}
			obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			obj.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvGsStoreIssuanceHeader>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			InvGsStoreIssuanceHeader.Save(obj);
			if (RECID == 0)
			{
				MessageBox.Show("Record Save SuccessFully" + obj.DocNo);
			}
			else
			{
				MessageBox.Show("Record Update SuccessFully" + obj.DocNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RECID, obj.DocumentTypeId, base.Name);
			}
			formReset();
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
			RECID = 0;
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
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDocNo_TextChanged(object sender, EventArgs e)
	{
	}

	private void ReadById(int ID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvGsStoreIssuanceHeader head = new InvGsStoreIssuanceHeader();
			head = InvGsStoreIssuanceHeader.GetByID(ID);
			if (head != null)
			{
				tabControlGsIssuance.SelectedIndex = 0;
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				txtDocdate.Value = head.DocDate;
				txtDocNo.Text = head.DocNo.ToString();
				CmbInvoiceNo.Value = head.ExImInvoiceId;
				InvoiceNoFill();
				txtRemarks.Text = head.Remarks;
				table.Rows.Clear();
				VoucherHeadIdGet();
				for (int i = 0; i < head.gsStoreIssuanceDetails.Count; i++)
				{
					table.Rows.Add(head.gsStoreIssuanceDetails[i].ItemId, head.gsStoreIssuanceDetails[i].ItemName, head.gsStoreIssuanceDetails[i].Warehouseid, head.gsStoreIssuanceDetails[i].WareHouseName, head.gsStoreIssuanceDetails[i].RackId, head.gsStoreIssuanceDetails[i].rackName, head.gsStoreIssuanceDetails[i].ItemConditionId, head.gsStoreIssuanceDetails[i].ItemCondition, head.gsStoreIssuanceDetails[i].IssueQty, head.gsStoreIssuanceDetails[i].ItemRate, head.gsStoreIssuanceDetails[i].ItemAmount, head.gsStoreIssuanceDetails[i].ReamarksDetail, head.gsStoreIssuanceDetails[i].DrAccountId, head.gsStoreIssuanceDetails[i].DebitAc);
				}
				grdDetail.DataSource = table;
				grdDetail.RetrieveStructure();
				DetailGridSettings();
				FormHelper.LoadAttachmentsForObject(AT, RECID, base.Name);
				ResetDetail();
			}
			else
			{
				table.Rows.Clear();
				grdDetail.ClearStructure();
				formReset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtDocNo.Text.Trim() == string.Empty || Conversion.ToInt((object)txtDocNo.Text.Trim()) == 0)
		{
			MessageBox.Show("DocNo Field Required");
			txtDocNo.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbInvoiceNo.Value) == 0)
		{
			MessageBox.Show("Invoice No Field Required");
			CmbInvoiceNo.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)cmbItem).ActiveRow == null || Conversion.ToInt(cmbItem.Value) == 0)
		{
			MessageBox.Show("Item Field Required");
			cmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWarehouse).ActiveRow == null || Conversion.ToInt(CmbWarehouse.Value) == 0)
		{
			MessageBox.Show("Warehouse Field Required");
			CmbWarehouse.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRackName).ActiveRow == null || Conversion.ToInt(CmbRackName.Value) == 0)
		{
			MessageBox.Show("Rack Name Field Required");
			CmbRackName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemCondition).ActiveRow == null || Conversion.ToInt(CmbItemCondition.Value) == 0)
		{
			MessageBox.Show("Item Condition Field Required");
			CmbItemCondition.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtItemQty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("ItemQty Field Required");
			txtItemQty.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtItemRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("ItemRate Field Required");
			txtItemRate.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtItemAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("ItemAmount Field Required");
			txtItemAmount.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbDrAc.Value) == 0)
		{
			MessageBox.Show("Debit Account Field Required");
			cmbDrAc.Focus();
			return false;
		}
		return true;
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			if (RECID > 0)
			{
				GeneratePrint(RECID);
				return;
			}
			throw new Exception("No record found");
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
			CommonServices.StoreIssuanceHeader_Slip475(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVoucher_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.VoucherReport_118(VoucherHeadId, 213);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Add_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				table.Rows.Add(cmbItem.Value, ((Control)(object)cmbItem).Text.Trim(), CmbWarehouse.Value, ((Control)(object)CmbWarehouse).Text, CmbRackName.Value, ((Control)(object)CmbRackName).Text, CmbItemCondition.Value, ((Control)(object)CmbItemCondition).Text.Trim(), Conversion.ToDouble((object)txtItemQty.Text.Trim()), Conversion.ToDouble((object)txtItemRate.Text.Trim()), Conversion.ToDouble((object)txtItemAmount.Text.Trim()), txtRemarksDetail.Text.Trim(), cmbDrAc.Value, ((Control)(object)cmbDrAc).Text.Trim());
				grdDetail.DataSource = table;
				grdDetail.RetrieveStructure();
				DetailGridSettings();
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridSettings()
	{
		try
		{
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["WareHouseId"].Visible = false;
			grdDetail.RootTable.Columns["RackId"].Visible = false;
			grdDetail.RootTable.Columns["ItemConditionId"].Visible = false;
			grdDetail.RootTable.Columns["DrAcId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 2, 3);
			grdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdDetail.RootTable.Columns["WareHouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grdDetail.RootTable.Columns["RackName"].Width = Constants.InventoryConstants.StringConstant;
			grdDetail.RootTable.Columns["ItemCondition"].Width = Constants.InventoryConstants.StringConstant;
			grdDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdDetail.RootTable.Columns["DebitAc"].Width = Constants.InventoryConstants.AccountTitle;
			GridEX_Helper.AddButton(grdDetail, "Delete", "X", 20, 0);
			grdDetail.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				cmbItem.Value = Conversion.ToInt(item.Cells["ItemId"].Value);
				cmbItem_Leave(null, null);
				CmbWarehouse.Value = Conversion.ToInt(item.Cells["WareHouseId"].Value);
				CmbRackName.Value = Conversion.ToInt(item.Cells["RackId"].Value);
				CmbItemCondition.Value = Conversion.ToInt(item.Cells["ItemConditionId"].Value);
				txtItemQty.Text = item.Cells["ITemQty"].Text.ToString();
				txtItemRate.Text = item.Cells["ItemRate"].Text.ToString();
				txtItemAmount.Text = item.Cells["ItemAmount"].Text.ToString();
				txtRemarksDetail.Text = item.Cells["Remarks"].Text.ToString();
				cmbDrAc.Value = Conversion.ToInt(item.Cells["DrAcId"].Value);
				Add.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
				cmbItem.Focus();
			}
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
			if (FormValidationDetail())
			{
				table.Rows[updateDetailIndex]["ItemId"] = cmbItem.Value;
				table.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)cmbItem).Text;
				table.Rows[updateDetailIndex]["WareHouseId"] = CmbWarehouse.Value;
				table.Rows[updateDetailIndex]["WareHouseName"] = ((Control)(object)CmbWarehouse).Text;
				table.Rows[updateDetailIndex]["RackId"] = CmbRackName.Value;
				table.Rows[updateDetailIndex]["RackName"] = ((Control)(object)CmbRackName).Text;
				table.Rows[updateDetailIndex]["ItemConditionId"] = CmbItemCondition.Value;
				table.Rows[updateDetailIndex]["ItemCondition"] = ((Control)(object)CmbItemCondition).Text;
				table.Rows[updateDetailIndex]["ItemQty"] = Conversion.ToDouble((object)txtItemQty.Text);
				table.Rows[updateDetailIndex]["ItemRate"] = Conversion.ToDouble((object)txtItemRate.Text);
				table.Rows[updateDetailIndex]["ItemAmount"] = Conversion.ToDouble((object)txtItemAmount.Text);
				table.Rows[updateDetailIndex]["Remarks"] = txtRemarksDetail.Text;
				table.Rows[updateDetailIndex]["DrAcId"] = cmbDrAc.Value;
				table.Rows[updateDetailIndex]["DebitAc"] = ((Control)(object)cmbDrAc).Text;
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (e.Column.Key == "Delete" && r != null && (int)r.RowType == 0)
			{
				table.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			grdDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Add_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "grdDetail")
			{
				CmbWarehouse.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetail()
	{
		try
		{
			updateDetailIndex = -1;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)CmbWarehouse).Text = string.Empty;
			((UltraGridBase)CmbWarehouse).DataSource = null;
			((Control)(object)CmbRackName).Text = string.Empty;
			((UltraGridBase)CmbRackName).DataSource = null;
			txtBalanceQty.Text = string.Empty;
			txtItemQty.Text = string.Empty;
			txtItemRate.Text = string.Empty;
			txtItemAmount.Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			cmbItem.Focus();
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
			formReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void formReset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			RECID = 0;
			VoucherHeadId = 0;
			CmbInvoiceNo.Value = 0;
			txtRemarks.Text = string.Empty;
			((Control)(object)cmbDrAc).Text = string.Empty;
			((Control)(object)CmbContractNo).Text = string.Empty;
			((Control)(object)CmbScheduleNo).Text = string.Empty;
			((Control)(object)CmbCustomer).Text = string.Empty;
			((Control)(object)CmbPortFrom).Text = string.Empty;
			((Control)(object)CmbPortTo).Text = string.Empty;
			txtnoofcontainer.Text = string.Empty;
			txtnetweight.Text = string.Empty;
			table.Rows.Clear();
			grdDetail.ClearStructure();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			InvoiceNoFill();
			GenerateDocNo();
			ResetDetail();
			txtDocdate.Focus();
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId", "AllItems", "RacksWithWarehouseAndItem", "ItemConditions" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			InvoiceNoFill();
			ItemDtsFillFromGlobal();
			ItemNameBind();
			ItemConditionBindFromGlobal();
			DebitAcDetailFill();
			PortFrom();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StoreIssuanceDirect_FormClosing(object sender, FormClosingEventArgs e)
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

	private void HistoryFill()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.DocumentTypeId = 213;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			if (FromDateHistory.Checked)
			{
				obj.FromDate = FromDateHistory.Value;
			}
			if (ToDateHistory.Checked)
			{
				obj.ToDate = ToDateHistory.Value;
			}
			obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			DataTable dt = InvGsStoreIssuanceHeader.FormHistory(obj);
			if (dt.Rows.Count > 0 && dt != null)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocNo", typeof(int));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("InvoiceNo", typeof(string));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUser", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("Remarks", typeof(string));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Clear();
				int i;
				for (i = 0; i < dt.Rows.Count; i++)
				{
					if (!dtcol.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(dt.Rows[i]["Id"])))
					{
						dtcol.Rows.Add(Conversion.ToInt(dt.Rows[i]["Id"]), Conversion.ToInt(dt.Rows[i]["DocNo"]), dt.Rows[i]["DocDate"], Conversion.ToString(dt.Rows[i]["InvoiceNo"]), Conversion.ToString(dt.Rows[i]["EntryUserName"]), dt.Rows[i]["EntryDate"], Conversion.ToString(dt.Rows[i]["ModifyUserName"]), dt.Rows[i]["ModifyDate"], Conversion.ToString(dt.Rows[i]["Remarks"]), Conversion.ToInt(dt.Rows[i]["NoOfAttachments"]));
					}
				}
				DataGridHistory.DataSource = dtcol;
				DataGridHistory.RetrieveStructure();
				gridsetting();
			}
			else
			{
				DataGridHistory.ClearStructure();
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
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yyyy";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 3);
			DataGridHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			DataGridHistory.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 35;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Key = "Print";
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 40;
			DataGridHistory.RootTable.Columns["Print"].Position = 1;
			DataGridHistory.FrozenColumns = 2;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControlGsIssuance_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControlGsIssuance.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RECID = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
			ReadById(RECID);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		try
		{
			if (DataGridHistory.CurrentRow == null || (int)DataGridHistory.CurrentRow.RowType != 0 || DataGridHistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			RECID = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
			InvGsStoreIssuanceHeader po = new InvGsStoreIssuanceHeader();
			po = InvGsStoreIssuanceHeader.GetByID(RECID);
			if (po != null)
			{
				DataTable dtdetailhistory = new DataTable();
				dtdetailhistory.Columns.Add("ItemName", typeof(string));
				dtdetailhistory.Columns.Add("Warehouse", typeof(string));
				dtdetailhistory.Columns.Add("RackName", typeof(string));
				dtdetailhistory.Columns.Add("ItemCondition", typeof(string));
				dtdetailhistory.Columns.Add("ItemQty", typeof(double));
				dtdetailhistory.Columns.Add("ItemRate", typeof(double));
				dtdetailhistory.Columns.Add("ItemAmount", typeof(double));
				dtdetailhistory.Columns.Add("Remarks", typeof(string));
				dtdetailhistory.Columns.Add("DebitAc", typeof(string));
				for (int i = 0; i < po.gsStoreIssuanceDetails.Count; i++)
				{
					dtdetailhistory.Rows.Add(po.gsStoreIssuanceDetails[i].ItemName, po.gsStoreIssuanceDetails[i].WareHouseName, po.gsStoreIssuanceDetails[i].rackName, po.gsStoreIssuanceDetails[i].ItemCondition, po.gsStoreIssuanceDetails[i].IssueQty, po.gsStoreIssuanceDetails[i].ItemRate, po.gsStoreIssuanceDetails[i].ItemAmount, po.gsStoreIssuanceDetails[i].ReamarksDetail, po.gsStoreIssuanceDetails[i].DebitAc);
				}
				grddetailofmain.DataSource = dtdetailhistory;
				grddetailofmain.RetrieveStructure();
				DataGridHistoryDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void DataGridHistoryDetailSetting()
	{
		try
		{
			GridEX_Helper.GridWrappingAndColumnSettings(grddetailofmain, 2, 3);
			grddetailofmain.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grddetailofmain.RootTable.Columns["Warehouse"].Width = Constants.InventoryConstants.WareHouseName;
			grddetailofmain.RootTable.Columns["RackName"].Width = Constants.InventoryConstants.StringConstant;
			grddetailofmain.RootTable.Columns["ItemCondition"].Width = Constants.InventoryConstants.StringConstant;
			grddetailofmain.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grddetailofmain.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grddetailofmain.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grddetailofmain.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grddetailofmain.RootTable.Columns["DebitAc"].Width = Constants.InventoryConstants.AccountTitle;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && DataGridHistory.CurrentColumn != null)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 213);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Edit")
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("You don't have right to update");
				}
				RECID = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
				ReadById(RECID);
			}
			else if (e.Column.Key == "Print")
			{
				if (!formright.DoHavePrintRights)
				{
					throw new Exception("You don't have right to print");
				}
				int id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
				GeneratePrint(id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BalanceStockQtyandAvgRate()
	{
		try
		{
			DataTable dtStock = CommonServices.GetAvgRateQtyAndStockInHand(Conversion.ToInt(cmbItem.Value), txtDocdate.Value, Conversion.ToInt(CmbItemCondition.Value), RECID, (RECID > 0) ? 213 : 0, Conversion.ToInt(CmbWarehouse.Value), Conversion.ToInt(CmbRackName.Value));
			double stock = ((dtStock.Rows.Count > 0) ? Math.Round(Conversion.ToDouble(dtStock.Rows[0]["QtyInHand"]), 2) : 0.0);
			if (stock > 0.0)
			{
				double ItemQty = stock;
				txtBalanceQty.Text = ItemQty.ToString("#,##.###");
			}
			else
			{
				txtBalanceQty.Text = 0.ToString("#,##.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void getAvgRate()
	{
		try
		{
			txtItemRate.Text = "0";
			DataTable dt = CommonServices.GetAvgRateQtyAndStockInHand(Conversion.ToInt(cmbItem.Value), txtDocdate.Value, Conversion.ToInt(CmbItemCondition.Value), RECID, (RECID > 0) ? 213 : 0);
			double AvgRate = ((dt.Rows.Count > 0) ? Math.Round(Conversion.ToDouble(dt.Rows[0]["AvgRate"]), 2) : 0.0);
			if (AvgRate > 0.0)
			{
				txtItemRate.Text = AvgRate.ToString();
			}
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
			if (Conversion.ToDouble((object)txtItemQty.Text) != 0.0 && Conversion.ToDouble((object)txtItemRate.Text) != 0.0)
			{
				double num = Conversion.ToDouble((object)txtItemQty.Text);
				double ItemRate = Conversion.ToDouble((object)txtItemRate.Text);
				txtItemAmount.Text = (num * ItemRate).ToString("#,##0.###");
			}
			else
			{
				txtItemAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemQty_TextChanged(object sender, EventArgs e)
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

	private void txtItemRate_TextChanged(object sender, EventArgs e)
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

	private void AvgRateUpdateOnDocDateChange()
	{
		try
		{
			if (grdDetail.RowCount <= 0)
			{
				return;
			}
			GridEXRow[] dataRows = grdDetail.GetDataRows();
			foreach (GridEXRow obj in dataRows)
			{
				double ItemQty = Conversion.ToDouble(obj.Cells["ItemQty"].Value);
				double rate = 0.0;
				DataTable dtRate = CommonServices.GetAvgRateQtyAndStockInHand(Conversion.ToInt(obj.Cells["ItemId"].Value), ItemConditionId: Conversion.ToInt(obj.Cells["ItemConditionId"].Value), DocDate: Conversion.ToDateTime((object)txtDocdate.Value), RecId: RECID, DocumentTypeId: (RECID > 0) ? 213 : 0);
				if (dtRate.Rows.Count > 0)
				{
					rate = Math.Round(Conversion.ToDouble(dtRate.Rows[0]["AvgRate"]), 3);
				}
				obj.BeginEdit();
				obj.Cells["ItemRate"].Value = rate;
				obj.Cells["ItemAmount"].Value = ItemQty * rate;
				obj.EndEdit();
			}
			grdDetail.UpdateData();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetail).Name);
			if (fs != null)
			{
				grdDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void frmGSIssuance_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S && tabControlGsIssuance.SelectedIndex == 0)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControlGsIssuance.SelectedIndex == 0)
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
				if (tabControlGsIssuance.SelectedIndex == 1)
				{
					tabControlGsIssuance.SelectedIndex = 0;
					txtDocdate.Focus();
				}
				else
				{
					tabControlGsIssuance.SelectedIndex = 1;
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1))
			{
				btnVoucher_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2))
			{
				Print_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtDocdate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControlGsIssuance.SelectedIndex == 0)
				{
					((Control)(object)grdDetail).Focus();
				}
				else if (tabControlGsIssuance.SelectedIndex == 1)
				{
					((Control)(object)grddetailofmain).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabControlGsIssuance.SelectedIndex == 0)
				{
					CmbWarehouse.Focus();
				}
				else if (tabControlGsIssuance.SelectedIndex == 1)
				{
					((Control)(object)DataGridHistory).Focus();
				}
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

	private void btnShortcutkeys_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Alt+1", "For Vouvher Print");
			dt.Rows.Add("Alt+2", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachment");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Warehouse in Detail Box");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
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

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdDetail_DoubleClick(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Space && grdDetail.CurrentColumn != null && grdDetail.CurrentColumn.Key == "Delete")
				{
					table.Rows[item.RowIndex].Delete();
					item.Delete();
				}
			}
			grdDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			Conversion.ToInt(item.Cells["Id"].Value);
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formright.DoHaveUpdateRights)
				{
					DataGridHistory_DoubleClick(null, null);
				}
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space || DataGridHistory.CurrentColumn == null)
				{
					return;
				}
				if (DataGridHistory.CurrentColumn.Key == "Edit")
				{
					if (formright.DoHaveUpdateRights)
					{
						RECID = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
						ReadById(RECID);
					}
				}
				else if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					int id = Conversion.ToInt(item.Cells["Id"].Value);
					GeneratePrint(id);
				}
				else if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 213);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemQty_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBalanceQty_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtItemAmount_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		try
		{
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnStockReport_Click(object sender, EventArgs e)
	{
		try
		{
			if (CommonServices.SetRightsValueInRightsObject("RptGenrateStocksStore").DoHaveViewRight)
			{
				RptGenrateStocksStore rptGenrateStocksStore = new RptGenrateStocksStore(UserAccount);
				rptGenrateStocksStore.cmbWarehouse.Value = Conversion.ToInt(CmbWarehouse.Value);
				rptGenrateStocksStore.cmbItemName.Value = Conversion.ToInt(cmbItem.Value);
				rptGenrateStocksStore.Show();
			}
			else
			{
				MessageBox.Show("You Don't Have rights View Of This Form..");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Text = string.Empty;
			ToDocNoHistory.Text = string.Empty;
			DataGridHistory.ClearStructure();
			grddetailofmain.ClearStructure();
			FromDateHistory.Focus();
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
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Expected O, but got Unknown
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Expected O, but got Unknown
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Expected O, but got Unknown
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Expected O, but got Unknown
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d6: Expected O, but got Unknown
		//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f7: Expected O, but got Unknown
		//IL_0cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d02: Expected O, but got Unknown
		//IL_6114: Unknown result type (might be due to invalid IL or missing references)
		//IL_611e: Expected O, but got Unknown
		//IL_6142: Unknown result type (might be due to invalid IL or missing references)
		//IL_614c: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
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
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
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
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
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
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.PackingMaterialIssuanceForShipment));
		GridEXLayout grddetailofmain_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControlGsIssuance = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label17 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.CmbContractNo = new UltraCombo();
		this.CmbCustomer = new UltraCombo();
		this.CmbPortTo = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbScheduleNo = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.CmbPortFrom = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.txtnoofcontainer = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtnetweight = new System.Windows.Forms.TextBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbRackName = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbItemCondition = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.Add = new System.Windows.Forms.Button();
		this.CmbWarehouse = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.cmbDrAc = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.txtItemQty = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.BtnStockReport = new System.Windows.Forms.Button();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.txtItemRate = new System.Windows.Forms.TextBox();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.label8 = new System.Windows.Forms.Label();
		this.txtBalanceQty = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtItemAmount = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.CmbInvoiceNo = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDocdate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnVoucher = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grddetailofmain = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label20 = new System.Windows.Forms.Label();
		this.DataGridHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.panel16 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label54 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControlGsIssuance.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel8.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbContractNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPortTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbScheduleNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPortFrom).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRackName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCondition).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDrAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailofmain).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.panel16.SuspendLayout();
		this.groupBox4.SuspendLayout();
		this.panel9.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControlGsIssuance);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1227, 688);
		this.panel1.TabIndex = 0;
		this.tabControlGsIssuance.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControlGsIssuance.Controls.Add(this.tabPage1);
		this.tabControlGsIssuance.Controls.Add(this.tabPage2);
		this.tabControlGsIssuance.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControlGsIssuance.Location = new System.Drawing.Point(0, 0);
		this.tabControlGsIssuance.Name = "tabControlGsIssuance";
		this.tabControlGsIssuance.SelectedIndex = 0;
		this.tabControlGsIssuance.Size = new System.Drawing.Size(1227, 688);
		this.tabControlGsIssuance.TabIndex = 0;
		this.tabControlGsIssuance.SelectedIndexChanged += new System.EventHandler(tabControlGsIssuance_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1219, 662);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1213, 656);
		this.panel2.TabIndex = 0;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 272);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1213, 384);
		this.panel6.TabIndex = 3;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1213, 358);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.ctrlGrdBar1);
		this.panel8.Controls.Add(this.label17);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1213, 26);
		this.panel8.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1171, 1);
		this.ctrlGrdBar1.MyGrid = this.grdDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(39, 22);
		this.ctrlGrdBar1.TabIndex = 2;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label17.AutoSize = true;
		this.label17.BackColor = System.Drawing.Color.Teal;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label17.Location = new System.Drawing.Point(3, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(58, 19);
		this.label17.TabIndex = 1;
		this.label17.Text = "Detail";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox3);
		this.panel5.Controls.Add(this.groupBox2);
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 55);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1213, 217);
		this.panel5.TabIndex = 2;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractNo);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomer);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPortTo);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbScheduleNo);
		this.groupBox3.Controls.Add(this.label22);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPortFrom);
		this.groupBox3.Controls.Add(this.label11);
		this.groupBox3.Controls.Add(this.label4);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Controls.Add(this.txtnoofcontainer);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.label21);
		this.groupBox3.Controls.Add(this.txtnetweight);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(863, 4);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(335, 205);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Invoice Information";
		this.CmbContractNo.AllowNull = (DefaultableBoolean)1;
		this.CmbContractNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance89;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance35).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContractNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Location = new System.Drawing.Point(110, 16);
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Name = "CmbContractNo";
		this.CmbContractNo.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.CmbContractNo).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbContractNo).TabStop = false;
		this.CmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance55).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance12).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Location = new System.Drawing.Point(110, 71);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Name = "CmbCustomer";
		this.CmbCustomer.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).TabStop = false;
		this.CmbPortTo.AllowNull = (DefaultableBoolean)1;
		this.CmbPortTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPortTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPortTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPortTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance119;
		((SpecialBoxBase)((UltraGridBase)this.CmbPortTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance125).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance126).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPortTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPortTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPortTo).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbPortTo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPortTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPortTo).Location = new System.Drawing.Point(110, 127);
		((System.Windows.Forms.Control)(object)this.CmbPortTo).Name = "CmbPortTo";
		this.CmbPortTo.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbPortTo).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.CmbPortTo).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbPortTo).TabStop = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(8, 22);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(75, 15);
		this.label2.TabIndex = 45;
		this.label2.Text = "Lc / Contract";
		this.CmbScheduleNo.AllowNull = (DefaultableBoolean)1;
		this.CmbScheduleNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbScheduleNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbScheduleNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbScheduleNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbScheduleNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance74).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbScheduleNo).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbScheduleNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbScheduleNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbScheduleNo).Location = new System.Drawing.Point(110, 43);
		((System.Windows.Forms.Control)(object)this.CmbScheduleNo).Name = "CmbScheduleNo";
		this.CmbScheduleNo.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbScheduleNo).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.CmbScheduleNo).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbScheduleNo).TabStop = false;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(8, 49);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(75, 15);
		this.label22.TabIndex = 49;
		this.label22.Text = "Schedule No";
		this.CmbPortFrom.AllowNull = (DefaultableBoolean)1;
		this.CmbPortFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPortFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPortFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPortFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance132;
		((SpecialBoxBase)((UltraGridBase)this.CmbPortFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPortFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPortFrom).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbPortFrom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPortFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPortFrom).Location = new System.Drawing.Point(110, 99);
		((System.Windows.Forms.Control)(object)this.CmbPortFrom).Name = "CmbPortFrom";
		this.CmbPortFrom.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbPortFrom).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.CmbPortFrom).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbPortFrom).TabStop = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(8, 77);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(93, 15);
		this.label11.TabIndex = 53;
		this.label11.Text = "Customer Name";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(8, 105);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(81, 15);
		this.label4.TabIndex = 49;
		this.label4.Text = "Loading Place";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(8, 133);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(99, 15);
		this.label14.TabIndex = 53;
		this.label14.Text = "Destination Place";
		this.txtnoofcontainer.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtnoofcontainer.Location = new System.Drawing.Point(110, 154);
		this.txtnoofcontainer.Name = "txtnoofcontainer";
		this.txtnoofcontainer.ReadOnly = true;
		this.txtnoofcontainer.Size = new System.Drawing.Size(217, 23);
		this.txtnoofcontainer.TabIndex = 5;
		this.txtnoofcontainer.TabStop = false;
		this.txtnoofcontainer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(8, 158);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(98, 15);
		this.label7.TabIndex = 55;
		this.label7.Text = "No Of Containers";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(8, 182);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(41, 15);
		this.label21.TabIndex = 57;
		this.label21.Text = "M.Ton";
		this.txtnetweight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtnetweight.Location = new System.Drawing.Point(110, 178);
		this.txtnetweight.Name = "txtnetweight";
		this.txtnetweight.ReadOnly = true;
		this.txtnetweight.Size = new System.Drawing.Size(217, 23);
		this.txtnetweight.TabIndex = 6;
		this.txtnetweight.TabStop = false;
		this.txtnetweight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRackName);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCondition);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Controls.Add(this.Add);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouse);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.txtRemarksDetail);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDrAc);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.txtItemQty);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.BtnStockReport);
		this.groupBox2.Controls.Add(this.btnCancelDetail);
		this.groupBox2.Controls.Add(this.txtItemRate);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.txtBalanceQty);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.txtItemAmount);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 84);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(854, 125);
		this.groupBox2.TabIndex = 2;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail";
		this.CmbRackName.AllowNull = (DefaultableBoolean)1;
		this.CmbRackName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRackName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRackName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbRackName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRackName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbRackName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRackName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance69).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRackName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRackName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRackName).Location = new System.Drawing.Point(323, 36);
		((System.Windows.Forms.Control)(object)this.CmbRackName).Name = "CmbRackName";
		((System.Windows.Forms.Control)(object)this.CmbRackName).Size = new System.Drawing.Size(123, 26);
		((System.Windows.Forms.Control)(object)this.CmbRackName).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbRackName).Leave += new System.EventHandler(CmbRackName_Leave);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(323, 18);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(67, 15);
		this.label24.TabIndex = 245;
		this.label24.Text = "Rack Name";
		this.CmbItemCondition.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCondition.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCondition.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCondition.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemCondition.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Location = new System.Drawing.Point(448, 36);
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Name = "CmbItemCondition";
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Size = new System.Drawing.Size(124, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Leave += new System.EventHandler(CmbItemCondition_Leave);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(448, 18);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(87, 15);
		this.label29.TabIndex = 237;
		this.label29.Text = "Item Condition";
		this.Add.BackColor = System.Drawing.Color.Teal;
		this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Add.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.ForeColor = System.Drawing.Color.White;
		this.Add.Location = new System.Drawing.Point(708, 85);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(35, 26);
		this.Add.TabIndex = 11;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click);
		this.Add.Leave += new System.EventHandler(Add_Leave);
		this.CmbWarehouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Location = new System.Drawing.Point(191, 36);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Name = "CmbWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Size = new System.Drawing.Size(131, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Leave += new System.EventHandler(CmbWarehouse_Leave);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(191, 18);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 106;
		this.label6.Text = "Warehouse";
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(198, 87);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(325, 23);
		this.txtRemarksDetail.TabIndex = 9;
		this.cmbDrAc.AllowNull = (DefaultableBoolean)1;
		this.cmbDrAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDrAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDrAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDrAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance86;
		((SpecialBoxBase)((UltraGridBase)this.cmbDrAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance95).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDrAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDrAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDrAc).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDrAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDrAc).Location = new System.Drawing.Point(525, 85);
		((System.Windows.Forms.Control)(object)this.cmbDrAc).Name = "cmbDrAc";
		((System.Windows.Forms.Control)(object)this.cmbDrAc).Size = new System.Drawing.Size(182, 26);
		((System.Windows.Forms.Control)(object)this.cmbDrAc).TabIndex = 10;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(525, 67);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(84, 15);
		this.label16.TabIndex = 116;
		this.label16.Text = "Debit Account";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(770, 18);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(54, 15);
		this.label18.TabIndex = 110;
		this.label18.Text = "Item Qty";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(198, 67);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(52, 15);
		this.label13.TabIndex = 113;
		this.label13.Text = "Remarks";
		this.txtItemQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQty.Location = new System.Drawing.Point(770, 38);
		this.txtItemQty.Name = "txtItemQty";
		this.txtItemQty.ShortcutsEnabled = false;
		this.txtItemQty.Size = new System.Drawing.Size(81, 23);
		this.txtItemQty.TabIndex = 6;
		this.txtItemQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemQty.TextChanged += new System.EventHandler(txtItemQty_TextChanged);
		this.txtItemQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemQty_KeyPress);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(5, 67);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(58, 15);
		this.label10.TabIndex = 110;
		this.label10.Text = "Item Rate";
		this.BtnStockReport.BackColor = System.Drawing.Color.Teal;
		this.BtnStockReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnStockReport.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnStockReport.ForeColor = System.Drawing.Color.White;
		this.BtnStockReport.Location = new System.Drawing.Point(659, 36);
		this.BtnStockReport.Name = "BtnStockReport";
		this.BtnStockReport.Size = new System.Drawing.Size(110, 26);
		this.BtnStockReport.TabIndex = 5;
		this.BtnStockReport.TabStop = false;
		this.BtnStockReport.Text = "Check Stock";
		this.BtnStockReport.UseVisualStyleBackColor = false;
		this.BtnStockReport.Visible = false;
		this.BtnStockReport.Click += new System.EventHandler(BtnStockReport_Click);
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(781, 85);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(68, 26);
		this.btnCancelDetail.TabIndex = 13;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnCancelDetail.Leave += new System.EventHandler(Add_Leave);
		this.txtItemRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemRate.Location = new System.Drawing.Point(5, 87);
		this.txtItemRate.Name = "txtItemRate";
		this.txtItemRate.ReadOnly = true;
		this.txtItemRate.ShortcutsEnabled = false;
		this.txtItemRate.Size = new System.Drawing.Size(86, 23);
		this.txtItemRate.TabIndex = 7;
		this.txtItemRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemRate.TextChanged += new System.EventHandler(txtItemRate_TextChanged);
		this.txtItemRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemRate_KeyPress);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(708, 85);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(73, 26);
		this.btnUpdateDetail.TabIndex = 12;
		this.btnUpdateDetail.Text = "Update ";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(574, 18);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(59, 15);
		this.label8.TabIndex = 110;
		this.label8.Text = "Stock Qty";
		this.txtBalanceQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalanceQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceQty.Location = new System.Drawing.Point(574, 38);
		this.txtBalanceQty.Name = "txtBalanceQty";
		this.txtBalanceQty.ReadOnly = true;
		this.txtBalanceQty.ShortcutsEnabled = false;
		this.txtBalanceQty.Size = new System.Drawing.Size(84, 23);
		this.txtBalanceQty.TabIndex = 4;
		this.txtBalanceQty.TabStop = false;
		this.txtBalanceQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBalanceQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBalanceQty_KeyPress);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(93, 67);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(79, 15);
		this.label12.TabIndex = 110;
		this.label12.Text = "Item Amount";
		this.txtItemAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemAmount.Location = new System.Drawing.Point(93, 87);
		this.txtItemAmount.Name = "txtItemAmount";
		this.txtItemAmount.ReadOnly = true;
		this.txtItemAmount.Size = new System.Drawing.Size(104, 23);
		this.txtItemAmount.TabIndex = 8;
		this.txtItemAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemAmount_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(5, 18);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 15);
		this.label9.TabIndex = 108;
		this.label9.Text = "Item Name";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance102).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(5, 36);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNo);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtDocdate);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.txtDocNo);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(854, 78);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(358, 17);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(490, 52);
		this.txtRemarks.TabIndex = 3;
		this.CmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance53;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance112).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Location = new System.Drawing.Point(100, 43);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Name = "CmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Leave += new System.EventHandler(CmbInvoiceNo_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(7, 49);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(65, 15);
		this.label3.TabIndex = 118;
		this.label3.Text = "Invoice No";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 21);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(88, 15);
		this.label1.TabIndex = 79;
		this.label1.Text = "Doc No && Date";
		this.txtDocdate.CustomFormat = "dd-MMM-yy";
		this.txtDocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocdate.Location = new System.Drawing.Point(173, 17);
		this.txtDocdate.Name = "txtDocdate";
		this.txtDocdate.Size = new System.Drawing.Size(118, 23);
		this.txtDocdate.TabIndex = 1;
		this.txtDocdate.Leave += new System.EventHandler(txtDocdate_Leave);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(298, 22);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(52, 15);
		this.label5.TabIndex = 6;
		this.label5.Text = "Remarks";
		this.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(100, 17);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.ReadOnly = true;
		this.txtDocNo.Size = new System.Drawing.Size(71, 23);
		this.txtDocNo.TabIndex = 0;
		this.txtDocNo.TabStop = false;
		this.txtDocNo.Leave += new System.EventHandler(txtDocNo_TextChanged);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label15);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 29);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1213, 26);
		this.panel4.TabIndex = 1;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(6, 2);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(537, 19);
		this.label15.TabIndex = 1;
		this.label15.Text = "Packing Material / Consumable Store Consumption For Shipment";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1213, 29);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.btnAttachment, this.btnVoucher, this.Print, this.btnShortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1213, 27);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnVoucher.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnVoucher.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucher.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucher.Name = "btnVoucher";
		this.btnVoucher.Size = new System.Drawing.Size(115, 24);
		this.btnVoucher.Text = "118-Voucher";
		this.btnVoucher.Click += new System.EventHandler(btnVoucher_Click);
		this.Print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "475-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnShortcutkeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutkeys.Image = (System.Drawing.Image)resources.GetObject("btnShortcutkeys.Image");
		this.btnShortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutkeys.Name = "btnShortcutkeys";
		this.btnShortcutkeys.Size = new System.Drawing.Size(106, 24);
		this.btnShortcutkeys.Text = "ShortCut Keys";
		this.btnShortcutkeys.Click += new System.EventHandler(btnShortcutkeys_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grddetailofmain);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel16);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.toolStrip2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1219, 662);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grddetailofmain.AllowDelete = (InheritableBoolean)1;
		this.grddetailofmain.AllowEdit = (InheritableBoolean)2;
		grddetailofmain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddetailofmain.DesignTimeLayout = grddetailofmain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddetailofmain).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grddetailofmain.DynamicFiltering = true;
		this.grddetailofmain.FilterMode = (FilterMode)2;
		this.grddetailofmain.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddetailofmain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetailofmain.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grddetailofmain).Location = new System.Drawing.Point(3, 500);
		((System.Windows.Forms.Control)(object)this.grddetailofmain).Name = "grddetailofmain";
		((ScrollableBase)this.grddetailofmain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetailofmain).Size = new System.Drawing.Size(1213, 159);
		((System.Windows.Forms.Control)(object)this.grddetailofmain).TabIndex = 67;
		this.grddetailofmain.TotalRow = (InheritableBoolean)1;
		this.grddetailofmain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetailofmain).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label20);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 474);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1213, 26);
		this.panel12.TabIndex = 66;
		this.label20.AutoSize = true;
		this.label20.BackColor = System.Drawing.Color.Teal;
		this.label20.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label20.Location = new System.Drawing.Point(6, 2);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(253, 19);
		this.label20.TabIndex = 1;
		this.label20.Text = "Detail Of Above Selected Row";
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 133);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1213, 341);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 64;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label23);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 108);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1213, 25);
		this.panel7.TabIndex = 72;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1172, 1);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 2;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label23.Location = new System.Drawing.Point(3, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(142, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Filtered Records";
		this.panel16.Controls.Add(this.groupBox4);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 54);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1213, 54);
		this.panel16.TabIndex = 71;
		this.groupBox4.Controls.Add(this.btnShowHistory);
		this.groupBox4.Controls.Add(this.label54);
		this.groupBox4.Controls.Add(this.label55);
		this.groupBox4.Controls.Add(this.label56);
		this.groupBox4.Controls.Add(this.label57);
		this.groupBox4.Controls.Add(this.ToDocNoHistory);
		this.groupBox4.Controls.Add(this.FromDocNoHistory);
		this.groupBox4.Controls.Add(this.ToDateHistory);
		this.groupBox4.Controls.Add(this.FromDateHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(7, 2);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(789, 47);
		this.groupBox4.TabIndex = 4;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(718, 14);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 4;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(571, 20);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(64, 15);
		this.label54.TabIndex = 86;
		this.label54.Text = "To Doc No";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(408, 20);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(79, 15);
		this.label55.TabIndex = 85;
		this.label55.Text = "From Doc No";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(214, 20);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(48, 15);
		this.label56.TabIndex = 84;
		this.label56.Text = "To Date";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(3, 20);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(63, 15);
		this.label57.TabIndex = 83;
		this.label57.Text = "From Date";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(638, 16);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.Tag = "int";
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(489, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.Tag = "int";
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(262, 16);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(69, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(142, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label19);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 28);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1213, 26);
		this.panel9.TabIndex = 65;
		this.label19.AutoSize = true;
		this.label19.BackColor = System.Drawing.Color.Teal;
		this.label19.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label19.Location = new System.Drawing.Point(6, 2);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(67, 19);
		this.label19.TabIndex = 1;
		this.label19.Text = "History";
		this.toolStrip2.BackColor = System.Drawing.Color.White;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNewHistory });
		this.toolStrip2.Location = new System.Drawing.Point(3, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1213, 25);
		this.toolStrip2.TabIndex = 69;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(55, 22);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1227, 688);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "PackingMaterialIssuanceForShipment";
		this.Text = "frmGSIssuance";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(StoreIssuanceDirect_FormClosing);
		base.Load += new System.EventHandler(frmGSIssuance_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmGSIssuance_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControlGsIssuance.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbContractNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPortTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbScheduleNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPortFrom).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRackName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCondition).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDrAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailofmain).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel16.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
