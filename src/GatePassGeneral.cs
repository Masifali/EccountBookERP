using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.StoreManagement;
using Architecture.BLL.WeightBridge;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.WeightBridge;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.CommissionAgent.Helper_Methods;
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
namespace Architecture.WinApp.Inventory_Definition
{

public class GatePassGeneral : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private Attachment AT = new Attachment();

	public UserAccount UserAccount = new UserAccount();

	private DataTable dtGrid = new DataTable();

	private DataTable table = new DataTable();

	private DataTable tablegrd = new DataTable();

	private DataTable DtHistory = new DataTable();

	private DataTable dtParty = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtRefDocNo = new DataTable();

	private DataTable dtRefDataFromDb = new DataTable();

	private DataTable dtJobLot = new DataTable();

	private DataTable dtware = new DataTable();

	public int POId;

	private bool UpdateMode;

	private int updateDetailIndex = -1;

	private int Id;

	private int GatePassTypeId;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel3;

	private Panel panel2;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnPrint;

	private Panel panel6;

	private Label label15;

	private Panel PanelHeaderInfo;

	private DateTimePicker txtgpdate;

	private TextBox txtvehicleno;

	private TextBox txtgpno;

	private Label label14;

	private Label label1;

	private Label label8;

	private Panel panel5;

	private Panel panel8;

	internal GridEX grdOpenGp;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label17;

	private TextBox txtqty;

	private Label label25;

	private Label label32;

	private CheckBox ChkBox;

	internal UltraCombo CmbWarehouse;

	internal UltraCombo cmbvehicletype;

	private Label label7;

	private Label label6;

	private Label labelGpTypeCaption;

	internal UltraCombo CmbGatePassType;

	private Label label10;

	internal UltraCombo CmbWeighBridge;

	private Label label12;

	internal UltraCombo CmbStatus;

	private TextBox txtFactoryWeight;

	private Label lblFactoryWt;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnLookup;

	private TextBox txtManualNo;

	private Label txtManualNoCaption;

	private TextBox txtSupplierWeight;

	private Label label9;

	private ToolStripButton btnRefresh;

	private TextBox txtremarks;

	private Panel PanelDetailInfo;

	private Panel panel11;

	private Label label11;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStripButton btnshortcut;

	internal UltraCombo CmbItemName;

	internal UltraCombo txtPartyName;

	internal GridEX grdhistory;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel7;

	private Panel panel10;

	private Label label13;

	private GroupBox groupBox3;

	private RadioButton rdapproved;

	private RadioButton rdpqtymodify;

	private RadioButton rdpqtydoc;

	private RadioButton rdpqtyentry;

	private Button btnshow;

	private Label label60;

	private Label label61;

	private Label label62;

	private Label label63;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel13;

	internal UltraCombo cmbPartyHistory;

	private Label label16;

	internal UltraCombo CmbDeliveryChallanNo;

	private Label label18;

	private Label lblDeliveryChallanNo;

	private Label label5;

	internal GridEX grd;

	private Panel panel14;

	private CtrlGrdBar ctrlGrdBar4;

	private Label label19;

	private GroupBox groupBox2;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	internal UltraCombo cmbJobLot;

	private Label label33;

	private TextBox txtRemarksDetail;

	private Label label23;

	private Label label24;

	private Label label26;

	private Button btnCancelDetail;

	private Button Add;

	internal UltraCombo cmbUOM;

	private Label label28;

	private Label label44;

	private Button btnUpdateDetail;

	private Label label20;

	private Panel panel9;

	private Label label52;

	internal GridEX grdHistoryDetail;

	internal UltraCombo CmbWareHouseToUpdate;

	private Button BtnUpdateSameValuesInGrid;

	private Label label66;

	private Label label65;

	private Label label49;

	internal UltraCombo CmbJobLotToUpdate;

	public RadioButton RadAll;

	public RadioButton RadNotReffered;

	public GatePassGeneral(UserAccount pUserAccount)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void InwardGatePass_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1069, 696);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnPrint.Enabled = formrights.DoHavePrintRights;
			btnupdate.Enabled = formrights.DoHaveUpdateRights;
			FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formrights);
			FormHelper.SetCtrlGrdBarRight(ctrlGrdBar2, formrights);
			dtParty.Columns.Add("Id", typeof(int));
			dtParty.Columns.Add("PartyName");
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			dtItem.Columns.Add("ItemCode");
			DataTable dataTable = dtRefDocNo;
			if (dataTable != null && dataTable.Columns.Count == 0)
			{
				dtRefDocNo.Columns.Add("Id", typeof(int));
				dtRefDocNo.Columns.Add("DocNo", typeof(int));
				dtRefDocNo.Columns.Add("CityId", typeof(int));
				dtRefDocNo.Columns.Add("CityName", typeof(string));
				dtRefDocNo.Columns.Add("VehicleNo", typeof(string));
				dtRefDocNo.Columns.Add("BiltyNo", typeof(string));
				dtRefDocNo.Columns.Add("PartyId", typeof(int));
				dtRefDocNo.Columns.Add("PartyName", typeof(string));
				dtRefDocNo.Columns.Add("ItemId", typeof(int));
				dtRefDocNo.Columns.Add("ItemName", typeof(string));
				dtRefDocNo.Columns.Add("ItemCode", typeof(string));
				dtRefDocNo.Columns.Add("ItemQty", typeof(double));
			}
			DataTable dataTable2 = dtGrid;
			if (dataTable2 != null && dataTable2.Columns.Count == 0)
			{
				dtGrid.Columns.Add("Id", typeof(int));
				dtGrid.Columns.Add("RefDocumentTypeId", typeof(int));
				dtGrid.Columns.Add("RefDocId", typeof(int));
				dtGrid.Columns.Add("RefDocDetailId", typeof(int));
				dtGrid.Columns.Add("RefDocNo", typeof(int));
				dtGrid.Columns.Add("ItemId", typeof(int));
				dtGrid.Columns.Add("ItemCode", typeof(string));
				dtGrid.Columns.Add("ItemName", typeof(string));
				dtGrid.Columns.Add("WareHouseId", typeof(int));
				dtGrid.Columns.Add("JobLotId", typeof(int));
				dtGrid.Columns.Add("PackUomId", typeof(int));
				dtGrid.Columns.Add("PackUom", typeof(string));
				dtGrid.Columns.Add("PackUomEquivalent", typeof(double));
				dtGrid.Columns.Add("ItemQty", typeof(double));
				dtGrid.Columns.Add("Remarks", typeof(string));
			}
			DocumentNoFill();
			GatePassType();
			WarehouseFillFromGlobal();
			vehicleTypefill();
			PartiesAndItemdtFillFromLookUp(null);
			BindPartyName();
			ItemNameBind();
			Status();
			WeighBridgeRoute();
			FactoryWeight();
			BindGrids();
			Formgrdfill();
			HistoryComboBind(HistoryComboDbCall());
			txtgpdate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PartiesAndItemdtFillFromLookUp(HashSet<int> ActionIds)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			if (ActionIds == null)
			{
				dtParty.Clear();
				dtItem.Clear();
			}
			else
			{
				if (ActionIds.Contains(1))
				{
					dtParty.Clear();
				}
				if (ActionIds.Contains(2))
				{
					dtItem.Clear();
				}
			}
			DataTable dt = WbPartyandItemsDefine.GetAll(new WbPartyandItemsDefine
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				return;
			}
			HashSet<int> partyIds = new HashSet<int>();
			HashSet<int> itemIds = new HashSet<int>();
			foreach (DataRow row in dt.Rows)
			{
				string type = row["WbType"]?.ToString();
				int id = Conversion.ToInt(row["Id"]);
				if (type == "PartyName" && (ActionIds == null || ActionIds.Contains(1)) && partyIds.Add(id))
				{
					dtParty.Rows.Add(id, row["PartyName"]?.ToString());
				}
				if (type == "Item" && (ActionIds == null || ActionIds.Contains(2)) && itemIds.Add(id))
				{
					dtItem.Rows.Add(id, row["ItemName"]?.ToString());
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PartiesAndItemdtFillByRefdoc(int id, int GpTypeId)
	{
		try
		{
			if (GpTypeId == 8)
			{
				dtItem.Clear();
			}
			else
			{
				dtParty.Clear();
				dtItem.Clear();
			}
			if (dtRefDataFromDb == null || dtRefDataFromDb.Rows.Count == 0)
			{
				return;
			}
			bool HasDocumentTypeId = dtRefDataFromDb.Columns.Contains("DocumentTypeId");
			int firstDocTypeId = (HasDocumentTypeId ? (from r in dtRefDataFromDb.AsEnumerable()
				select r.Field<int?>("DocumentTypeId").GetValueOrDefault()).FirstOrDefault() : 0);
			foreach (DataRow row2 in (from r in dtGrid.AsEnumerable()
				where r.Field<int?>("RefDocumentTypeId").GetValueOrDefault() != firstDocTypeId
				select r).ToList())
			{
				dtGrid.Rows.Remove(row2);
			}
			HashSet<int> partyIds = new HashSet<int>();
			HashSet<int> itemIds = new HashSet<int>();
			HashSet<int> existingDetailIds = new HashSet<int>(from r in grd.GetRows()
				select Conversion.ToInt(r.Cells["RefDocDetailId"].Value));
			bool hasWarehouse = dtRefDataFromDb.Columns.Contains("WareHouseId");
			bool hasJobLot = dtRefDataFromDb.Columns.Contains("JobLotId");
			foreach (DataRow row in dtRefDataFromDb.Rows)
			{
				int itemId = row.Field<int?>("ItemId").GetValueOrDefault();
				int detailId = row.Field<int?>("DetailId").GetValueOrDefault();
				bool num = dtRefDataFromDb.Columns.Contains("BillToPartyId");
				int vendorSupplierId = 0;
				if (num)
				{
					vendorSupplierId = row.Field<int?>("BillToPartyId").GetValueOrDefault();
				}
				if (num && partyIds.Add(vendorSupplierId))
				{
					dtParty.Rows.Add(vendorSupplierId, row["BillToPartyName"]?.ToString());
				}
				if (itemIds.Add(itemId))
				{
					dtItem.Rows.Add(itemId, row["ItemName"]?.ToString(), row["ItemCode"]?.ToString());
				}
				int RefDocId = ((GpTypeId == 6) ? Conversion.ToInt(row["DeliveryChallanHeaderId"]) : Conversion.ToInt(row["Id"]));
				if (existingDetailIds.Add(detailId) && RefDocId == id)
				{
					DataRow dr = dtGrid.NewRow();
					dr["Id"] = 0;
					if (HasDocumentTypeId)
					{
						dr["RefDocumentTypeId"] = row["DocumentTypeId"];
					}
					dr["RefDocId"] = RefDocId;
					dr["RefDocNo"] = ((GpTypeId == 6) ? row["ChallanNo"] : row["DocNo"]);
					dr["RefDocDetailId"] = detailId;
					dr["ItemId"] = itemId;
					dr["ItemCode"] = row["ItemCode"];
					dr["ItemName"] = row["ItemName"];
					if (hasWarehouse && row["WareHouseId"] != DBNull.Value)
					{
						dr["WareHouseId"] = row["WareHouseId"];
					}
					if (hasJobLot && row["JobLotId"] != DBNull.Value)
					{
						dr["JobLotId"] = row["JobLotId"];
					}
					dr["PackUomId"] = row["UomId"];
					dr["PackUom"] = row["Uom"];
					dr["PackUomEquivalent"] = row["UomEquivalent"];
					dr["ItemQty"] = GpTypeId switch
					{
						7 => row["ItemQty"], 
						6 => row["DeliveryChallanQty"], 
						_ => row["BalanceQty"], 
					};
					dr["Remarks"] = row["RemarksDetail"];
					dtGrid.Rows.Add(dr);
				}
			}
			BindGrids();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindPartyName()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(txtPartyName, dtParty, "Id", "PartyName", "Party Name", AllColumns: true, Conversion.ToInt(txtPartyName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemNameBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbItemName.Value);
			string ValueMember = (rdbtnItemName.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (rdbtnItemName.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(CmbItemName, dtItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DocumentNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			int GpTypeCode = GatePassGeneral.GenerateCode(new GatePassGeneral
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 52,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (GpTypeCode > 0)
			{
				txtgpno.Text = GpTypeCode.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void vehicleTypefill()
	{
		try
		{
			DataTable dt = VehicleType.GetAll();
			InfragisticsHelper.BindAndRetainSelection(cmbvehicletype, dt, "Id", "VehicleDescription", "VehicleDescription", AllColumns: true, Conversion.ToInt(cmbvehicletype.Value), insertDefaultRow: true, null, null, ActivateRow: true, 1);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)cmbvehicletype).Rows[1].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatePassType()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Type", typeof(string));
			dt.Rows.Add(1, "General");
			dt.Rows.Add(2, "Store");
			dt.Rows.Add(3, "SupplierStock");
			dt.Rows.Add(4, "StockTransfer");
			dt.Rows.Add(5, "PackingMaterial");
			dt.Rows.Add(6, "Delivery_Challan");
			dt.Rows.Add(7, "Purchase_Order");
			dt.Rows.Add(8, "Purchase_Demand");
			InfragisticsHelper.BindAndRetainSelection(CmbGatePassType, dt, "Id", "Type", "GatePassType", AllColumns: true, Conversion.ToInt(CmbGatePassType.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			if (dt != null && dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbGatePassType).Rows[1].Activate();
				((UltraGridBase)CmbGatePassType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FactoryWeight()
	{
		try
		{
			if (((Control)(object)CmbWeighBridge).Text == "Non-Weighable")
			{
				txtFactoryWeight.Visible = false;
				lblFactoryWt.Visible = false;
			}
			else
			{
				txtFactoryWeight.Visible = true;
				lblFactoryWt.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WeighBridgeRoute()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Type", typeof(string));
			dt.Rows.Add(1, "Non-Weighable");
			dt.Rows.Add(2, "Weighable");
			InfragisticsHelper.BindAndRetainSelection(CmbWeighBridge, dt, "Id", "Type", "WeighBridge", AllColumns: true, Conversion.ToInt(CmbWeighBridge.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			if (dt != null && dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbWeighBridge).Rows[2].Activate();
				((UltraGridBase)CmbWeighBridge).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WarehouseFillFromGlobal()
	{
		try
		{
			List<GlobalWarehousesWithBranch> lst = clsGlobalVariables.globalWarehousesWithBranches.Where((GlobalWarehousesWithBranch r) => r.IsActive).ToList();
			dtware = DatatableHelper.PopulateDataTableAndReturn(lst, (GlobalWarehousesWithBranch x) => x.WarehouseId, (GlobalWarehousesWithBranch x) => x.WarehouseName);
			InfragisticsHelper.BindAndRetainSelection(CmbWarehouse, dtware, "Id", "Description", "Warehouse", AllColumns: true, Conversion.ToInt(CmbWarehouse.Value), insertDefaultRow: true);
			InfragisticsHelper.BindAndRetainSelection(CmbWareHouseToUpdate, dtware, "Id", "Description", "Warehouse", AllColumns: true, Conversion.ToInt(CmbWareHouseToUpdate.Value), insertDefaultRow: true);
			JobFillFromGlobal();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobFillFromGlobal()
	{
		try
		{
			List<GetJobLotGlIdsandName> lst = clsGlobalVariables.globalJobLot;
			dtJobLot = DatatableHelper.PopulateDataTableAndReturn(lst, (GetJobLotGlIdsandName x) => x.Id, (GetJobLotGlIdsandName x) => x.JobLotDescription);
			InfragisticsHelper.BindAndRetainSelection(cmbJobLot, dtJobLot, "Id", "Description", "Job Lot", AllColumns: true, Conversion.ToInt(cmbJobLot.Value), insertDefaultRow: true);
			InfragisticsHelper.BindAndRetainSelection(CmbJobLotToUpdate, dtJobLot, "Id", "Description", "Job Lot", AllColumns: true, Conversion.ToInt(CmbJobLotToUpdate.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Status()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Status", typeof(string));
			dt.Rows.Add(1, "Open");
			dt.Rows.Add(2, "Accepted");
			dt.Rows.Add(3, "Rejected");
			InfragisticsHelper.BindAndRetainSelection(CmbStatus, dt, "Id", "Status", "Status", AllColumns: true, Conversion.ToInt(CmbStatus.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			if (dt != null && dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbStatus).Rows[1].Activate();
				((UltraGridBase)CmbStatus).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbWeighBridge_Leave(object sender, EventArgs e)
	{
		FactoryWeight();
	}

	private void CmbGatePassType_Leave(object sender, EventArgs e)
	{
		int value = Conversion.ToInt(CmbGatePassType.Value);
		((Control)(object)CmbGatePassType).Width = 314;
		lblDeliveryChallanNo.Visible = false;
		((Control)(object)CmbDeliveryChallanNo).Visible = false;
		((Control)(object)CmbDeliveryChallanNo).Text = "";
		if ((uint)(value - 6) <= 2u)
		{
			RefDocNodtFillDbCall();
			RefDocNoBind();
			lblDeliveryChallanNo.Visible = true;
			((Control)(object)CmbDeliveryChallanNo).Visible = true;
			((Control)(object)CmbGatePassType).Width = 130;
			lblDeliveryChallanNo.Text = value switch
			{
				8 => "Demand #", 
				7 => "Order No", 
				6 => "D.C No", 
				_ => "", 
			};
			PanelDetailInfo.Visible = false;
		}
		else
		{
			PanelDetailInfo.Visible = true;
			PartiesAndItemdtFillFromLookUp(null);
			BindPartyName();
			ItemNameBind();
		}
		if (grd.RowCount > 0 && value != GatePassTypeId)
		{
			if (MessageBox.Show("You are changing the GP Type. Grid data will be cleared. Are you sure?", "Confirm Change", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
			{
				CmbGatePassType.Value = GatePassTypeId;
				return;
			}
			grd.DataSource = null;
			dtGrid?.Clear();
		}
		GatePassTypeId = value;
	}

	private void RefDocNodtFillDbCall()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		try
		{
			dtRefDocNo.Clear();
			dtRefDataFromDb.Clear();
			int gatePassTypeId = Conversion.ToInt(CmbGatePassType.Value);
			ReportsParameters param = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				RecId = Id
			};
			switch (gatePassTypeId)
			{
			case 6:
				param.DocumentTypeId = 148;
				dtRefDataFromDb = DeliveryChallanHeader.DeliveryChallanHeader_GetNoForGp(param);
				FillData(MapDeliveryChallan);
				break;
			case 7:
				param.DocumentTypeId = 42;
				dtRefDataFromDb = PurchaseOrder.PurchaseOrder_GetNoForGeneralInward(param);
				FillData(MapPurchaseOrder);
				break;
			case 8:
				param.DocumentTypeId = 42;
				dtRefDataFromDb = PurchasePreBillHeader.InvPurchaseDemand_GetNoForGeneralInward(param);
				FillData(MapPurchaseDemand);
				break;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MapDeliveryChallan(DataRow dr, DataRow row)
	{
		row["Id"] = dr["DeliveryChallanHeaderId"];
		row["DocNo"] = dr["ChallanNo"];
		row["CityId"] = dr["CityId"];
		row["CityName"] = dr["CityName"];
		row["VehicleNo"] = dr["VehicleNo"];
		row["BiltyNo"] = dr["BiltyNo"];
		row["PartyId"] = dr["BillToPartyId"];
		row["PartyName"] = dr["BillToPartyName"];
		row["ItemId"] = dr["ItemId"];
		row["ItemName"] = dr["ItemName"];
		row["ItemCode"] = dr["ItemCode"];
		row["ItemQty"] = dr["DeliveryChallanQty"];
	}

	private void MapPurchaseOrder(DataRow dr, DataRow row)
	{
		row["Id"] = dr["Id"];
		row["DocNo"] = dr["DocNo"];
		row["CityId"] = dr["CityId"];
		row["CityName"] = dr["CityName"];
		row["PartyId"] = dr["BillToPartyId"];
		row["PartyName"] = dr["BillToPartyName"];
		row["ItemId"] = dr["ItemId"];
		row["ItemName"] = dr["ItemName"];
		row["ItemCode"] = dr["ItemCode"];
		row["ItemQty"] = dr["ItemQty"];
	}

	private void MapPurchaseDemand(DataRow dr, DataRow row)
	{
		row["Id"] = dr["Id"];
		row["DocNo"] = dr["DocNo"];
		row["ItemId"] = dr["ItemId"];
		row["ItemName"] = dr["ItemName"];
		row["ItemCode"] = dr["ItemCode"];
		row["ItemQty"] = dr["BalanceQty"];
	}

	private void FillData(Action<DataRow, DataRow> mapAction)
	{
		if (dtRefDataFromDb == null || dtRefDataFromDb.Rows.Count == 0)
		{
			return;
		}
		HashSet<int> ids = new HashSet<int>();
		foreach (DataRow dr in dtRefDataFromDb.Rows)
		{
			int id = ((Conversion.ToInt(CmbGatePassType.Value) == 6) ? Conversion.ToInt(dr["DeliveryChallanHeaderId"]) : Conversion.ToInt(dr["Id"]));
			if (ids.Add(id))
			{
				DataRow row = dtRefDocNo.NewRow();
				mapAction(dr, row);
				dtRefDocNo.Rows.Add(row);
			}
		}
	}

	private void RefDocNoBind()
	{
		try
		{
			int GatePassTypeId = Conversion.ToInt(CmbGatePassType.Value);
			InfragisticsHelper.BindAndRetainSelection(headerCaption: GatePassTypeId switch
			{
				6 => "Challan No", 
				7 => "Order No", 
				8 => "Demand No", 
				_ => string.Empty, 
			}, combo: CmbDeliveryChallanNo, source: dtRefDocNo, valueMember: "Id", displayMember: "DocNo", AllColumns: true, previousValue: Conversion.ToInt(CmbDeliveryChallanNo.Value), insertDefaultRow: false, defaultText: null, defaultValue: null, ActivateRow: false);
			DataTable dataTable = dtRefDocNo;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				UltraGridBand band = ((UltraGridBase)CmbDeliveryChallanNo).DisplayLayout.Bands[0];
				band.Columns["CityId"].Hidden = true;
				band.Columns["ItemId"].Hidden = true;
				band.Columns["PartyId"].Hidden = true;
				if (GatePassTypeId == 7 || GatePassTypeId == 8)
				{
					band.Columns["VehicleNo"].Hidden = true;
					band.Columns["BiltyNo"].Hidden = true;
				}
				if (GatePassTypeId == 8)
				{
					band.Columns["PartyName"].Hidden = true;
				}
				CmbDeliveryChallanNo_Leave(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDeliveryChallanNo_Leave(object sender, EventArgs e)
	{
		if (((UltraDropDownBase)CmbDeliveryChallanNo).SelectedRow == null)
		{
			return;
		}
		int id = Conversion.ToInt(CmbDeliveryChallanNo.Value);
		if (id <= 0)
		{
			return;
		}
		CellsCollection cells = ((UltraDropDownBase)CmbDeliveryChallanNo).SelectedRow.Cells;
		if (cells != null)
		{
			int GatePassTypeId = Conversion.ToInt(CmbGatePassType.Value);
			switch (GatePassTypeId)
			{
			case 6:
			case 7:
				PartiesAndItemdtFillByRefdoc(id, GatePassTypeId);
				break;
			case 8:
				PartiesAndItemdtFillByRefdoc(id, GatePassTypeId);
				PartiesAndItemdtFillFromLookUp(new HashSet<int> { 1 });
				break;
			}
			BindPartyName();
			ItemNameBind();
			UltraCombo obj = txtPartyName;
			UltraGridCell obj2 = cells["PartyName"];
			((Control)(object)obj).Text = Conversion.ToString((obj2 != null) ? obj2.Value : null);
			UltraCombo cmbItemName = CmbItemName;
			UltraGridCell obj3 = cells["ItemName"];
			((Control)(object)cmbItemName).Text = Conversion.ToString((obj3 != null) ? obj3.Value : null);
			TextBox textBox = txtqty;
			UltraGridCell obj4 = cells["ItemQty"];
			textBox.Text = Conversion.ToString((obj4 != null) ? obj4.Value : null);
			TextBox textBox2 = txtvehicleno;
			UltraGridCell obj5 = cells["VehicleNo"];
			textBox2.Text = Conversion.ToString((obj5 != null) ? obj5.Value : null);
		}
	}

	private void CmbItemName_Leave(object sender, EventArgs e)
	{
		CommonBindings.ItemUomFromGlobalBind(Conversion.ToInt(CmbItemName.Value), cmbUOM);
	}

	private void rdbtnItemName_CheckedChanged(object sender, EventArgs e)
	{
		ItemNameBind();
	}

	private bool formvalidation()
	{
		if (((Control)(object)CmbGatePassType).Text.Trim() == "" || Conversion.ToInt(CmbGatePassType.Value) == 0)
		{
			MessageBox.Show("Please Select CmbGatePassType");
			CmbGatePassType.Focus();
			return false;
		}
		if (txtgpno.Text.Trim() == "" || txtgpno.Text.Trim() == "0")
		{
			MessageBox.Show("Gp No Filed Required");
			txtgpno.Focus();
			return false;
		}
		if (txtvehicleno.Text.Trim() == "")
		{
			MessageBox.Show("Please Select Vehicle No");
			txtvehicleno.Focus();
			return false;
		}
		if (((Control)(object)cmbvehicletype).Text.Trim() == "" || Conversion.ToInt(cmbvehicletype.Value) == 0)
		{
			MessageBox.Show("Please Select Vehicle Type");
			cmbvehicletype.Focus();
			return false;
		}
		if (((Control)(object)CmbWeighBridge).Text.Trim() == "" || Conversion.ToInt(CmbWeighBridge.Value) == 0)
		{
			MessageBox.Show("Please Select WeighBridge");
			CmbWeighBridge.Focus();
			return false;
		}
		if (((Control)(object)CmbStatus).Text.Trim() == string.Empty || Conversion.ToInt(CmbStatus.Value) == 0)
		{
			MessageBox.Show("Please Select Status");
			CmbStatus.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbItemName).ActiveRow == null || Conversion.ToInt(CmbItemName.Value) == 0)
		{
			MessageBox.Show("Item Field Required");
			CmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWarehouse).ActiveRow == null || Conversion.ToInt(CmbWarehouse.Value) == 0)
		{
			MessageBox.Show("Warehouse Field Required");
			CmbWarehouse.Focus();
			return false;
		}
		if (((UltraGridBase)cmbJobLot).ActiveRow == null || Conversion.ToInt(cmbJobLot.Value) == 0)
		{
			MessageBox.Show("Joblot Field Required");
			cmbJobLot.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtqty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("ItemQty Field Required");
			txtqty.Focus();
			return false;
		}
		return true;
	}

	private void BindGrids()
	{
		try
		{
			DataTable dataTable = dtGrid;
			if (dataTable != null && dataTable.Columns.Count > 0)
			{
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				DetailGridSettings(grd);
			}
			((Control)(object)CmbGatePassType).Enabled = grd.RowCount == 0;
			((Control)(object)CmbDeliveryChallanNo).Enabled = grd.RowCount == 0;
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
				DataRow newRow = dtGrid.NewRow();
				newRow["Id"] = 0;
				FillDetailRow(newRow);
				dtGrid.Rows.Add(newRow);
				BindGrids();
				ResetDetail();
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
				DataRow row = dtGrid.Rows[updateDetailIndex];
				FillDetailRow(row);
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailRow(DataRow row)
	{
		row["ItemId"] = Conversion.ToInt(CmbItemName.Value);
		row["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value);
		row["ItemName"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value);
		row["WareHouseId"] = Conversion.ToInt(CmbWarehouse.Value);
		row["JobLotId"] = Conversion.ToInt(cmbJobLot.Value);
		int UomId = Conversion.ToInt(cmbUOM.Value);
		row["PackUomId"] = UomId;
		row["PackUom"] = ((Control)(object)cmbUOM).Text.Trim();
		double num;
		if (UomId <= 0)
		{
			num = 0.0;
		}
		else
		{
			UltraCombo obj = cmbUOM;
			object obj2;
			if (obj == null)
			{
				obj2 = null;
			}
			else
			{
				UltraGridRow selectedRow = ((UltraDropDownBase)obj).SelectedRow;
				obj2 = ((selectedRow != null) ? selectedRow.Cells["Equivalent"].Value : null);
			}
			num = Conversion.ToDouble(obj2);
		}
		row["PackUomEquivalent"] = num;
		row["ItemQty"] = Conversion.ToDouble((object)txtqty.Text.Trim());
		row["Remarks"] = txtRemarksDetail.Text.Trim();
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

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) == 0)
			{
				updateDetailIndex = item.RowIndex;
				CmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				CmbWarehouse.Value = item.Cells["WareHouseId"].Value.ToString();
				cmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
				cmbUOM.Value = item.Cells["PackUomId"].Text.ToString();
				txtqty.Text = item.Cells["ItemQty"].Text.ToString();
				txtRemarksDetail.Text = item.Cells["Remarks"].Text.ToString();
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

	private void GridComboBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grd.RootTable.Columns["JobLotId"], dtJobLot, "Id", "Description", "Job Lot", Constants.InventoryConstants.JobLotConstant);
			GridEX_Helper.ComboBind(grd.RootTable.Columns["WareHouseId"], dtware, "Id", "Description", "WareHouse", Constants.InventoryConstants.WareHouseName);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridSettings(GridEX gridEX)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "RefDocumentTypeId", "RefDocId", "RefDocDetailId", "ItemId", "PackUomId", "PackUomEquivalent" };
			Dictionary<string, EditType> editableColumns = null;
			if (((Control)(object)gridEX).Name == "grd")
			{
				editableColumns = new Dictionary<string, EditType>
				{
					{
						"ItemQty",
						(EditType)(GatePassTypeId != 6)
					},
					{
						"Remarks",
						(EditType)1
					}
				};
			}
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 2, 2, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["PackUom"].Caption = "UOM";
			if (((Control)(object)gridEX).Name == "grd")
			{
				GridComboBind();
				if (GatePassTypeId != 6)
				{
					GridEX_Helper.AddButton(grd, "Delete", "X", 20, 0);
				}
				grd.FrozenColumns = 2;
				ctrlGrdBar1_Load(null, null);
			}
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
			if (base.ActiveControl.Name == "grd")
			{
				CmbWarehouse.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "Add")
				{
					AddDetailRow(item);
				}
				else if (e.Column.Key == "Delete")
				{
					DeleteDetailRow(item);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddDetailRow(GridEXRow r)
	{
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				dtGrid.Rows[r.RowIndex].Delete();
				r.Delete();
				((Control)(object)CmbGatePassType).Enabled = grd.RowCount == 0;
				((Control)(object)CmbDeliveryChallanNo).Enabled = grd.RowCount == 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			GridEXRow row = grd.CurrentRow;
			if (!GridEX_Helper.IsValidRow(row))
			{
				return;
			}
			if (e.Control)
			{
				switch (e.KeyCode)
				{
				case Keys.Space:
					HandleSpaceAction(row);
					break;
				case Keys.Return:
					grd_DoubleClick(null, null);
					break;
				}
			}
			if (e.KeyCode == Keys.F1)
			{
				HandleF1Actions(row);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HandleSpaceAction(GridEXRow row)
	{
		switch (grd.CurrentColumn.Key)
		{
		case "Edit":
			grd_DoubleClick(null, null);
			break;
		case "Delete":
			DeleteDetailRow(row);
			break;
		case "Add":
			AddDetailRow(row);
			break;
		}
	}

	private void HandleF1Actions(GridEXRow row)
	{
		string columnKey = grd.CurrentColumn.Key;
		Conversion.ToInt(row.Cells["ItemId"].Value);
		Conversion.ToInt(row.Cells["WarehouseId"].Value);
		if (!(columnKey == "JobLotId"))
		{
			if (columnKey == "WarehouseId")
			{
				GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow(dtware, "Id", "WarehouseNAme", row, "WarehouseId");
			}
		}
		else
		{
			GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow(dtJobLot, "Id", "Description", row, "JobLotId");
		}
		grd.UpdateData();
	}

	private void BtnUpdateSameValuesInGrid_Click(object sender, EventArgs e)
	{
		try
		{
			GridEXRow[] rows = grd.GetDataRows();
			if (rows == null || rows.Length == 0)
			{
				throw new Exception("Item Detail has no records.");
			}
			int warehouseId = Conversion.ToInt(CmbWareHouseToUpdate.Value);
			int jobLotId = Conversion.ToInt(CmbJobLotToUpdate.Value);
			bool num = ((UltraGridBase)CmbWareHouseToUpdate).ActiveRow != null && warehouseId != 0;
			bool hasJobLot = ((UltraGridBase)CmbJobLotToUpdate).ActiveRow != null && jobLotId != 0;
			if (!num && !hasJobLot)
			{
				GridEXRow obj = rows[0];
				warehouseId = Conversion.ToInt(obj.Cells["WareHouseId"].Value);
				jobLotId = Conversion.ToInt(obj.Cells["JobLotId"].Value);
				if (warehouseId == 0 && jobLotId == 0)
				{
					throw new Exception("Warehouse and JobLot is empty in row 1.");
				}
			}
			GridEXRow[] array = rows;
			foreach (GridEXRow r in array)
			{
				int currentWarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
				int currentJobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				bool shouldUpdateWarehouse = warehouseId != 0 && (RadAll.Checked || currentWarehouseId == 0);
				bool shouldUpdateJobLot = jobLotId != 0 && (RadAll.Checked || currentJobLotId == 0);
				if (shouldUpdateWarehouse || shouldUpdateJobLot)
				{
					r.BeginEdit();
					if (shouldUpdateWarehouse)
					{
						r.Cells["WareHouseId"].Value = warehouseId;
					}
					if (shouldUpdateJobLot)
					{
						r.Cells["JobLotId"].Value = jobLotId;
					}
					r.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Expected O, but got Unknown
		try
		{
			Reset();
			Id = ID;
			GatePassGeneral gatePass = GatePassGeneral.GetByID(ID);
			if (gatePass == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			btnupdate.Visible = true;
			btnsave.Visible = false;
			UpdateMode = true;
			txtgpdate.Value = gatePass.GpDate;
			txtgpno.Text = Conversion.ToString((object)gatePass.GpSrNo);
			((Control)(object)CmbGatePassType).Text = Conversion.ToString((object)gatePass.GatepassType);
			CmbGatePassType_Leave(null, null);
			GatePassTypeId = Conversion.ToInt(CmbGatePassType.Value);
			if (GatePassTypeId == 6)
			{
				CmbDeliveryChallanNo.Value = gatePass.DeliveryChallanHeaderId.ToString();
			}
			if (GatePassTypeId == 7)
			{
				CmbDeliveryChallanNo.Value = gatePass.PurchaseOrderId.ToString();
			}
			if (GatePassTypeId == 8)
			{
				CmbDeliveryChallanNo.Value = gatePass.PurchaseDemandId.ToString();
			}
			CmbDeliveryChallanNo_Leave(null, null);
			txtremarks.Text = Conversion.ToString((object)gatePass.OtherRemarks);
			txtvehicleno.Text = Conversion.ToString((object)gatePass.VehicleNo);
			((Control)(object)cmbvehicletype).Text = Conversion.ToString((object)gatePass.VehicleType);
			((Control)(object)txtPartyName).Text = Conversion.ToString((object)gatePass.OtherSupCust);
			((Control)(object)CmbStatus).Text = Conversion.ToString((object)gatePass.DocAttachment);
			((Control)(object)CmbWeighBridge).Text = Conversion.ToString((object)gatePass.Status);
			txtFactoryWeight.Text = Conversion.ToString((object)gatePass.FactoryWeight);
			txtSupplierWeight.Text = Conversion.ToString((object)gatePass.SupplierWeight);
			txtManualNo.Text = Conversion.ToString((object)gatePass.ManualNo);
			dtGrid.Rows.Clear();
			FillDetailFromListCommonForReadById(gatePass, dtGrid);
			string varietyName = Conversion.ToString((object)GridEX_Helper.GetFirstOrDefaultValueOfColumn(grd, "ItemName"));
			((Control)(object)CmbItemName).Text = (string.IsNullOrEmpty(varietyName) ? Conversion.ToString((object)gatePass.VarietyName) : varietyName);
			int noOfPackages = Conversion.ToInt((object)GridEX_Helper.GetColumnSum(grd, "ItemQty"));
			txtqty.Text = ((noOfPackages == 0) ? Conversion.ToString((object)gatePass.NoOfPackages) : noOfPackages.ToString());
			int warehouseId = Conversion.ToInt((object)GridEX_Helper.GetFirstOrDefaultValueOfColumn(grd, "WareHouseId"));
			CmbWarehouse.Value = ((warehouseId == 0) ? gatePass.WarehouseId : warehouseId);
			FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
			DataTable lstWb = WbTransactions.GetNetWeightFromWbTransactions(new WbTransactions
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ReferenceDocTypeId = 52,
				ReferenceDocNoId = Id
			});
			if (lstWb.Rows.Count > 0)
			{
				double NetWeight = 0.0;
				for (int i = 0; i < lstWb.Rows.Count; i++)
				{
					NetWeight += Convert.ToDouble(lstWb.Rows[i]["NetWbWeight"]);
				}
				txtFactoryWeight.Text = NetWeight.ToString();
			}
			else
			{
				txtFactoryWeight.Text = "0";
			}
			FactoryWeight();
			txtManualNo.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void FillDetailFromListCommonForReadById(GatePassGeneral invGrn, DataTable dt, bool ExtraColumns = false)
	{
		if (((invGrn != null) ? invGrn.GatePassGeneralDetailList : null) == null || invGrn.GatePassGeneralDetailList.Count == 0)
		{
			return;
		}
		foreach (GatePassGeneralDetail detail in invGrn.GatePassGeneralDetailList)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.Id;
			dr["RefDocumentTypeId"] = detail.RefDocumentTypeId;
			dr["RefDocId"] = detail.RefDocId;
			dr["RefDocDetailId"] = detail.RefDocDetailId;
			dr["RefDocNo"] = detail.RefDocNo;
			dr["WareHouseId"] = detail.WareHouseId;
			if (ExtraColumns)
			{
				dr["WareHouseName"] = detail.WareHouseName;
			}
			dr["JobLotId"] = detail.JobLotId;
			if (ExtraColumns)
			{
				dr["JobLot"] = detail.JobLotDescription;
			}
			dr["ItemId"] = detail.ItemId;
			dr["ItemCode"] = detail.ItemCode;
			dr["ItemName"] = detail.ItemName;
			dr["PackUomId"] = detail.ItemUomId;
			dr["PackUom"] = detail.ItemUomCode;
			dr["PackUomEquivalent"] = detail.ItemUomEquivalent;
			dr["ItemQty"] = detail.ItemQty;
			dr["Remarks"] = detail.RemarksDetail;
			dt.Rows.Add(dr);
		}
	}

	private void Insert()
	{
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			int gpTypeId = Conversion.ToInt(CmbGatePassType.Value);
			int docId = Conversion.ToInt(CmbDeliveryChallanNo.Value);
			if ((gpTypeId == 6 || gpTypeId == 7 || gpTypeId == 8) && docId == 0)
			{
				string message = "";
				MessageBox.Show(gpTypeId switch
				{
					6 => "Please Select Delivery Challan No", 
					7 => "Please Select Order No", 
					8 => "Please Select Demand No", 
					_ => "Please Select Document No", 
				});
				CmbDeliveryChallanNo.Focus();
			}
			else
			{
				if (!FormHelper.ConfirmAction((Id == 0) ? "Are you sure to Save" : "Are you sure to Update"))
				{
					return;
				}
				GatePassGeneral gatePass = new GatePassGeneral();
				gatePass.CompanyId = UserAccount.CompanyId;
				gatePass.OrganizationId = UserAccount.OrganizationId;
				gatePass.BranchesId = UserAccount.BranchesId;
				gatePass.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				gatePass.RefDocumentTypeId = 52;
				gatePass.DocumentTypeId = 52;
				gatePass.Id = Id;
				gatePass.EntryDate = DateTime.Today;
				gatePass.EntryUser = UserAccount.ID;
				gatePass.ModifyDate = DateTime.Today;
				gatePass.ModifyUser = UserAccount.ID;
				gatePass.GpSrNo = int.Parse(txtgpno.Text);
				gatePass.GpDate = txtgpdate.Value;
				gatePass.GatepassType = ((Control)(object)CmbGatePassType).Text.Trim();
				GatePassTypeId = Conversion.ToInt(CmbGatePassType.Value);
				if (GatePassTypeId == 6)
				{
					gatePass.DeliveryChallanHeaderId = Conversion.ToInt(CmbDeliveryChallanNo.Value);
				}
				if (GatePassTypeId == 7)
				{
					gatePass.PurchaseOrderId = Conversion.ToInt(CmbDeliveryChallanNo.Value);
				}
				if (GatePassTypeId == 8)
				{
					gatePass.PurchaseDemandId = Conversion.ToInt(CmbDeliveryChallanNo.Value);
				}
				gatePass.OtherSupCust = ((Control)(object)txtPartyName).Text.Trim();
				gatePass.ManualNo = txtManualNo.Text.Trim();
				gatePass.VehicleType = ((Control)(object)cmbvehicletype).Text.Trim();
				gatePass.VehicleNo = txtvehicleno.Text.Trim();
				gatePass.DocAttachment = ((Control)(object)CmbStatus).Text.Trim().ToString();
				gatePass.Status = Conversion.ToString((object)((Control)(object)CmbWeighBridge).Text.Trim());
				gatePass.InDateTimeStamp = DateTime.Now;
				gatePass.OutDateTimeStamp = DateTime.Now;
				if (txtFactoryWeight.Text.Trim() != string.Empty && txtFactoryWeight.Text.Trim() != null && txtFactoryWeight.Text.Trim() != "0")
				{
					gatePass.FactoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text.Trim());
				}
				else
				{
					gatePass.FactoryWeight = 0.0;
				}
				gatePass.SupplierWeight = Conversion.ToDouble((object)txtSupplierWeight.Text.Trim());
				gatePass.OtherRemarks = txtremarks.Text.Trim();
				string VarietyName = Conversion.ToString((object)GridEX_Helper.GetFirstOrDefaultValueOfColumn(grd, "ItemName"));
				gatePass.VarietyName = (string.IsNullOrEmpty(VarietyName) ? Conversion.ToString((object)((Control)(object)CmbItemName).Text) : VarietyName);
				int NoOfPackages = Conversion.ToInt((object)GridEX_Helper.GetColumnSum(grd, "ItemQty"));
				gatePass.NoOfPackages = ((NoOfPackages == 0) ? Conversion.ToInt((object)txtqty.Text) : NoOfPackages);
				int WarehouseId = Conversion.ToInt((object)GridEX_Helper.GetFirstOrDefaultValueOfColumn(grd, "WareHouseId"));
				gatePass.WarehouseId = ((WarehouseId == 0) ? Conversion.ToInt(CmbWarehouse.Value) : WarehouseId);
				gatePass.GatePassGeneralDetailList = new List<GatePassGeneralDetail>();
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt(r.Cells["ItemQty"].Value) > 0)
					{
						GatePassGeneralDetail d = new GatePassGeneralDetail();
						d.Id = Conversion.ToInt(r.Cells["Id"].Value);
						FillDetailListCommonForInsertAndDelete(d, r);
						FormHelper.ValidateField(d.ItemId, "Item", r.RowIndex);
						FormHelper.ValidateField(d.WareHouseId, "Warehouse", r.RowIndex);
						FormHelper.ValidateField(d.JobLotId, "JobLot", r.RowIndex);
						FormHelper.ValidateField(d.ItemQty, "Qty", r.RowIndex);
						gatePass.GatePassGeneralDetailList.Add(d);
					}
				}
				gatePass.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				gatePass.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<GatePassGeneral>(gatePass, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				int success = GatePassGeneral.Save(gatePass);
				if (Id > 0)
				{
					MessageBox.Show("Record Update Successfully[" + gatePass.GpSrNo + "]");
					FormHelper.DeleteAllAttachmentsFromDb(AT, Id, 52, base.Name);
				}
				else
				{
					MessageBox.Show("Record Save Successfully[" + gatePass.GpSrNo + "]");
				}
				Reset();
				if (ChkBox.Checked)
				{
					CommonServices.GatePassGeneralInward254(success, 52);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(GatePassGeneralDetail pd, GridEXRow r)
	{
		pd.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
		pd.RefDocId = Conversion.ToInt(r.Cells["RefDocId"].Value);
		pd.RefDocDetailId = Conversion.ToInt(r.Cells["RefDocDetailId"].Value);
		pd.RefDocNo = Conversion.ToInt(r.Cells["RefDocNo"].Value);
		pd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		pd.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		pd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		pd.WareHouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
		pd.WareHouseName = Conversion.ToString((object)r.Cells["WareHouseId"].Text);
		pd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
		pd.JobLotDescription = Conversion.ToString((object)r.Cells["JobLotId"].Text);
		pd.ItemUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
		pd.ItemUomCode = Conversion.ToString(r.Cells["PackUom"].Value);
		pd.ItemUomEquivalent = Conversion.ToDouble(r.Cells["PackUomEquivalent"].Value);
		decimal qty = (pd.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value));
		pd.Weight = qty;
		pd.RemarksDetail = Conversion.ToString((object)r.Cells["Remarks"].Text);
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			Id = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtgpno_TextChanged(object sender, EventArgs e)
	{
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

	private DataTable HistoryComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return GatePassGeneral.GetDataForDropDownFromGatePassGeneral(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryComboBind(DataTable dtcombo)
	{
		try
		{
			DataTable dtST = new DataTable();
			dtST.Columns.Add("Id", typeof(int));
			dtST.Columns.Add("Name");
			dtST.Clone();
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Supplier")
				{
					dtST.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(cmbPartyHistory, dtST, "Id", "Name", "Party Name", AllColumns: true, Conversion.ToInt(cmbPartyHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable grdfrmfill(string Status = null)
	{
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Expected O, but got Unknown
		DataTable dt = new DataTable();
		try
		{
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("GpType", typeof(string));
			dt.Columns.Add("GpDate", typeof(DateTime));
			dt.Columns.Add("GpNo", typeof(int));
			dt.Columns.Add("DeliveryChallanNo", typeof(int));
			dt.Columns.Add("ManualNo", typeof(string));
			dt.Columns.Add("VehicleType", typeof(string));
			dt.Columns.Add("VehicleNo", typeof(string));
			dt.Columns.Add("PartyName", typeof(string));
			dt.Columns.Add("ItemName", typeof(string));
			dt.Columns.Add("ItemQty", typeof(double));
			dt.Columns.Add("Warehouse", typeof(string));
			dt.Columns.Add("GpStatus", typeof(string));
			dt.Columns.Add("WBStatus", typeof(string));
			dt.Columns.Add("FactoryWeight", typeof(double));
			dt.Columns.Add("PartyWeight", typeof(double));
			dt.Columns.Add("EntryUser", typeof(string));
			dt.Columns.Add("EntryDate", typeof(DateTime));
			dt.Columns.Add("ModifyUser", typeof(string));
			dt.Columns.Add("ModifyDate", typeof(DateTime));
			dt.Columns.Add("ApprovedUser", typeof(string));
			dt.Columns.Add("ApprovedDate", typeof(DateTime));
			dt.Columns.Add("NoOfAttachments", typeof(int));
			dt.Columns.Add("Remarks", typeof(string));
			DtHistory.Rows.Clear();
			ReportsParameters gatePassGeneral = new ReportsParameters();
			gatePassGeneral.OrganizationId = UserAccount.OrganizationId;
			gatePassGeneral.CompanyId = UserAccount.CompanyId;
			gatePassGeneral.BranchesId = UserAccount.BranchesId;
			gatePassGeneral.DocumentTypeId = 52;
			gatePassGeneral.Status = Status;
			gatePassGeneral.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (Status == null || Status == string.Empty)
			{
				if (rdpqtydoc.Checked)
				{
					if (FromDateHistory.Checked)
					{
						gatePassGeneral.FromDate = FromDateHistory.Value;
					}
					if (ToDateHistory.Checked)
					{
						gatePassGeneral.ToDate = ToDateHistory.Value;
					}
				}
				else if (rdpqtyentry.Checked)
				{
					if (FromDateHistory.Checked)
					{
						gatePassGeneral.EntryDateFrom = FromDateHistory.Value;
					}
					if (ToDateHistory.Checked)
					{
						gatePassGeneral.EntryDateTo = ToDateHistory.Value;
					}
				}
				else if (rdpqtymodify.Checked)
				{
					if (FromDateHistory.Checked)
					{
						gatePassGeneral.ModifyDateFrom = FromDateHistory.Value;
					}
					if (ToDateHistory.Checked)
					{
						gatePassGeneral.ModifyDateTo = ToDateHistory.Value;
					}
				}
				else if (rdapproved.Checked)
				{
					if (FromDateHistory.Checked)
					{
						gatePassGeneral.ApprovedDateFrom = FromDateHistory.Value;
					}
					if (ToDateHistory.Checked)
					{
						gatePassGeneral.ApprovedDateTo = ToDateHistory.Value;
					}
				}
			}
			gatePassGeneral.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			gatePassGeneral.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			gatePassGeneral.SupplierCustomerId = Conversion.ToInt(cmbPartyHistory.Value);
			DtHistory = GatePassGeneral.ReadAll(gatePassGeneral);
			if (DtHistory.Rows.Count > 0 && DtHistory != null)
			{
				for (int i = 0; i < DtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(DtHistory.Rows[i]["Id"], DtHistory.Rows[i]["gatePassType"], Conversion.ToDateTime(DtHistory.Rows[i]["GpDate"]), DtHistory.Rows[i]["GpSrNo"], DtHistory.Rows[i]["DeliveryChallanNo"], DtHistory.Rows[i]["ManualNo"], DtHistory.Rows[i]["VehicleType"], DtHistory.Rows[i]["VehicleNo"], DtHistory.Rows[i]["PartyName"], DtHistory.Rows[i]["VarietyName"], DtHistory.Rows[i]["ItemQty"], DtHistory.Rows[i]["WarehouseName"], DtHistory.Rows[i]["DocAttachment"], DtHistory.Rows[i]["Status"], DtHistory.Rows[i]["FactoryWeight"], DtHistory.Rows[i]["SupplierWeight"], DtHistory.Rows[i]["EntryUser"], DtHistory.Rows[i]["EntryDate"], DtHistory.Rows[i]["ModifyUser"], DtHistory.Rows[i]["ModifyDate"], DtHistory.Rows[i]["ApprovedUser"], DtHistory.Rows[i]["PostDate"], DtHistory.Rows[i]["NoOfAttachments"], DtHistory.Rows[i]["OtherRemarks"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return dt;
	}

	private void Formgrdfill()
	{
		DataTable dt = grdfrmfill("Open").Copy();
		if (dt.Rows.Count > 0)
		{
			grdOpenGp.DataSource = dt;
			grdOpenGp.RetrieveStructure();
			grdfrmsetting();
		}
		else
		{
			grdOpenGp.ClearStructure();
		}
	}

	private void Historygrdfill()
	{
		DataTable dt = grdfrmfill().Copy();
		if (dt.Rows.Count > 0)
		{
			grdhistory.DataSource = dt;
			grdhistory.RetrieveStructure();
			grdHistorySetting();
		}
		else
		{
			grdhistory.ClearStructure();
		}
	}

	private void grdfrmsetting()
	{
		try
		{
			grdOpenGp.RootTable.Columns["Id"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdOpenGp, 2, 3, null, null, null, ColumnSetting: false);
			grdOpenGp.RootTable.Columns["GpType"].Width = Constants.InventoryConstants.GpType;
			grdOpenGp.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			grdOpenGp.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdOpenGp.RootTable.Columns["DeliveryChallanNo"].Width = Constants.InventoryConstants.DocNoConstant + 20;
			grdOpenGp.RootTable.Columns["ManualNo"].Width = Constants.InventoryConstants.ManualBillNo;
			grdOpenGp.RootTable.Columns["VehicleType"].Width = Constants.InventoryConstants.VehicleTypeConstant + 20;
			grdOpenGp.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdOpenGp.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdOpenGp.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName + 30;
			grdOpenGp.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdOpenGp.RootTable.Columns["Warehouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdOpenGp.RootTable.Columns["GpStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdOpenGp.RootTable.Columns["WBStatus"].Width = Constants.InventoryConstants.StatusConstant + 20;
			grdOpenGp.RootTable.Columns["FactoryWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdOpenGp.RootTable.Columns["PartyWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdOpenGp.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdOpenGp.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdOpenGp.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdOpenGp.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdOpenGp.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdOpenGp.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdOpenGp.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			grdOpenGp.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdOpenGp.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdOpenGp.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdOpenGp.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdOpenGp.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdOpenGp.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdOpenGp.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdOpenGp.RootTable.Columns["ItemQty"].FormatString = "#,##0.00";
			grdOpenGp.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.00";
			grdOpenGp.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			grdOpenGp.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			grdOpenGp.RootTable.Columns["FactoryWeight"].FormatString = "#,##0.00";
			grdOpenGp.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,##0.00";
			grdOpenGp.RootTable.Columns["PartyWeight"].AggregateFunction = (AggregateFunction)2;
			grdOpenGp.RootTable.Columns["PartyWeight"].TextAlignment = (TextAlignment)3;
			grdOpenGp.RootTable.Columns["PartyWeight"].FormatString = "#,##0.00";
			grdOpenGp.RootTable.Columns["PartyWeight"].TotalFormatString = "#,##0.00";
			grdOpenGp.RootTable.Columns.Add("View");
			grdOpenGp.RootTable.Columns["View"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOpenGp.RootTable.Columns["View"].ButtonStyle = (ButtonStyle)5;
			grdOpenGp.RootTable.Columns["View"].ButtonText = "View";
			grdOpenGp.RootTable.Columns["View"].Width = 40;
			grdOpenGp.RootTable.Columns["View"].Position = 0;
			grdOpenGp.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdHistorySetting()
	{
		try
		{
			grdhistory.RootTable.Columns["Id"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdhistory, 2, 3, null, null, null, ColumnSetting: false);
			grdhistory.RootTable.Columns["GpType"].Width = Constants.InventoryConstants.GpType;
			grdhistory.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["ManualNo"].Width = Constants.InventoryConstants.ManualBillNo;
			grdhistory.RootTable.Columns["VehicleType"].Width = Constants.InventoryConstants.VehicleTypeConstant + 20;
			grdhistory.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdhistory.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdhistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName + 30;
			grdhistory.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdhistory.RootTable.Columns["Warehouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdhistory.RootTable.Columns["GpStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdhistory.RootTable.Columns["WBStatus"].Width = Constants.InventoryConstants.StatusConstant + 20;
			grdhistory.RootTable.Columns["FactoryWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdhistory.RootTable.Columns["PartyWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdhistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdhistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdhistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdhistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 40;
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 50;
			grdhistory.RootTable.Columns["Print"].Position = 1;
			grdhistory.FrozenColumns = 2;
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["ItemQty"].FormatString = "#,##0.00";
			grdhistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.00";
			grdhistory.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["FactoryWeight"].FormatString = "#,##0.00";
			grdhistory.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,##0.00";
			grdhistory.RootTable.Columns["PartyWeight"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["PartyWeight"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["PartyWeight"].FormatString = "#,##0.00";
			grdhistory.RootTable.Columns["PartyWeight"].TotalFormatString = "#,##0.00";
			ctrlGrdBar2_Load_1(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOpenGp_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdOpenGp.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				Id = Conversion.ToInt((object)grdOpenGp.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(Id);
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
			Historygrdfill();
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
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Clear();
			txtToDocNoHistory.Clear();
			grdhistory.DataSource = null;
			rdpqtydoc.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboBind(HistoryComboDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name != null && base.ActiveControl.Name == "grdhistory")
			{
				FromDateHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOpenGp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdOpenGp.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "View")
			{
				CommonServices.GatePassGeneralInward254(Conversion.ToInt(item.Cells["Id"].Value), 52);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 0)
		{
			Formgrdfill();
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "Edit")
				{
					Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(Id);
				}
				if (e.Column.Key == "Print")
				{
					CommonServices.GatePassGeneralInward254(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value), 52);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value), 52);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOpenGp_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdOpenGp.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Control && e.KeyCode == Keys.Return)
				{
					Id = Conversion.ToInt((object)grdOpenGp.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(Id);
				}
				else if (e.Control && e.KeyCode == Keys.Space && grdOpenGp.CurrentColumn.Key == "View")
				{
					CommonServices.GatePassGeneralInward254(Conversion.ToInt(item.Cells["Id"].Value), 52);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		GridEXRow r = grdhistory.CurrentRow;
		if (r == null || (int)r.RowType != 0 || grdhistory.CurrentColumn == null)
		{
			return;
		}
		Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
		if (e.Control && e.KeyCode == Keys.Space)
		{
			if (grdhistory.CurrentColumn.Key == "Edit")
			{
				Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(Id);
			}
			if (grdhistory.CurrentColumn.Key == "Print")
			{
				CommonServices.GatePassGeneralInward254(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value), 52);
			}
		}
		if (e.Control && e.KeyCode == Keys.Return)
		{
			grdhistory_DoubleClick(null, null);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdhistory.CurrentRow;
			if (row != null && (int)row.RowType == 0 && row.Cells.Count > 0)
			{
				GetDetailGrdByHeadId(Conversion.ToInt(row.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(int Id)
	{
		try
		{
			GatePassGeneral Obj = GatePassGeneral.GetByID(Id);
			if (Obj != null && Obj.GatePassGeneralDetailList.Count == 0)
			{
				grdHistoryDetail.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = dtGrid.Clone();
			dtDetailHistory.Columns.Add("WareHouseName");
			dtDetailHistory.Columns.Add("JobLot");
			FillDetailFromListCommonForReadById(Obj, dtDetailHistory, ExtraColumns: true);
			grdHistoryDetail.DataSource = dtDetailHistory;
			grdHistoryDetail.RetrieveStructure();
			HistoryDetailGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryDetailGridSetting()
	{
		try
		{
			DetailGridSettings(grdHistoryDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			POId = 0;
			Id = 0;
			UpdateMode = false;
			btnupdate.Visible = false;
			btnsave.Visible = true;
			GatePassTypeId = Conversion.ToInt(CmbGatePassType.Value);
			((Control)(object)txtPartyName).Text = string.Empty;
			txtManualNo.Text = string.Empty;
			((Control)(object)cmbvehicletype).Text = string.Empty;
			txtvehicleno.Text = string.Empty;
			txtFactoryWeight.Text = "0";
			txtremarks.Text = string.Empty;
			table.Clear();
			grdOpenGp.DataSource = table;
			dtGrid.Rows.Clear();
			ResetDetail();
			BindGrids();
			txtgpdate.Focus();
			DocumentNoFill();
			Formgrdfill();
			CmbGatePassType_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChkBox_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "ctrlGrdBar")
			{
				CmbGatePassType.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "WarehousesWithBranches" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			vehicleTypefill();
			WarehouseFillFromGlobal();
			CmbGatePassType_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatePassGeneral_FormClosing(object sender, FormClosingEventArgs e)
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

	private void ResetDetail()
	{
		try
		{
			updateDetailIndex = -1;
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbWarehouse).Text = string.Empty;
			((Control)(object)cmbUOM).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			CmbWarehouse.Focus();
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			Add.Visible = true;
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdOpenGp).Name);
			if (fs != null)
			{
				grdOpenGp.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load_1(object sender, EventArgs e)
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

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.GatePassGeneralInward254(Id, 52);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click_1(object sender, EventArgs e)
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

	private void btnLookup_Click(object sender, EventArgs e)
	{
		new WeighBridgeGeneralLookups(UserAccount).Show();
	}

	private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtSupplierWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFactoryWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void InwardGatePass_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.KeyCode == Keys.Delete && tabControl1.SelectedIndex == 1)
			{
				GridEXRow[] checkedRows = grdOpenGp.GetCheckedRows();
				for (int i = 0; i < checkedRows.Length; i++)
				{
					int.Parse(checkedRows[i].Cells["Id"].Value.ToString());
				}
			}
			if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0 && btnsave.Visible && btnsave.Enabled)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					CmbGatePassType.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (btnupdate.Visible && btnupdate.Enabled && e.Control && e.KeyCode == Keys.U && tabControl1.SelectedIndex == 0)
			{
				btnupdate_Click(null, null);
			}
			if (btnPrint.Visible && btnPrint.Enabled && e.Control && e.KeyCode == Keys.P && tabControl1.SelectedIndex == 0)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				CmbGatePassType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				CmbGatePassType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdOpenGp).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Return && tabControl1.SelectedIndex == 1)
			{
				Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(Id);
			}
			if (e.Control && e.KeyCode == Keys.Up && tabControl1.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down && tabControl1.SelectedIndex == 1)
			{
				((Control)(object)grdhistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.R && tabControl1.SelectedIndex == 1)
			{
				btnRefreshHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 1)
			{
				btnNewHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 1)
			{
				btnshow_Click(null, null);
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
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on GatePass Type");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item Combo in Detail Box");
			dt.Rows.Add("Ctrl+ArrowRight", "To change Grid Tab Control,");
			dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid");
			dt.Rows.Add("Ctrl+Space", "To Call Function's On Button Or Link When Focus On Any Grid ");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshortcut_Click(object sender, EventArgs e)
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
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Expected O, but got Unknown
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Expected O, but got Unknown
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Expected O, but got Unknown
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Expected O, but got Unknown
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Expected O, but got Unknown
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Expected O, but got Unknown
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Expected O, but got Unknown
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Expected O, but got Unknown
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Expected O, but got Unknown
		//IL_0610: Unknown result type (might be due to invalid IL or missing references)
		//IL_061a: Expected O, but got Unknown
		//IL_063c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Expected O, but got Unknown
		//IL_065d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Expected O, but got Unknown
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bf: Expected O, but got Unknown
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0701: Expected O, but got Unknown
		//IL_0702: Unknown result type (might be due to invalid IL or missing references)
		//IL_070c: Expected O, but got Unknown
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		//IL_082a: Expected O, but got Unknown
		//IL_0841: Unknown result type (might be due to invalid IL or missing references)
		//IL_084b: Expected O, but got Unknown
		//IL_0883: Unknown result type (might be due to invalid IL or missing references)
		//IL_088d: Expected O, but got Unknown
		//IL_0ebe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec8: Expected O, but got Unknown
		//IL_12fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1305: Expected O, but got Unknown
		//IL_7050: Unknown result type (might be due to invalid IL or missing references)
		//IL_705a: Expected O, but got Unknown
		//IL_707e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7088: Expected O, but got Unknown
		GridEXLayout grdOpenGp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
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
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance220 = new Appearance();
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
		Appearance appearance231 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
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
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
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
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Definition.GatePassGeneral));
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdOpenGp = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label20 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label11 = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.RadAll = new System.Windows.Forms.RadioButton();
		this.RadNotReffered = new System.Windows.Forms.RadioButton();
		this.CmbWareHouseToUpdate = new UltraCombo();
		this.BtnUpdateSameValuesInGrid = new System.Windows.Forms.Button();
		this.label66 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.CmbJobLotToUpdate = new UltraCombo();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.label19 = new System.Windows.Forms.Label();
		this.PanelDetailInfo = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.cmbJobLot = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.CmbItemName = new UltraCombo();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.Add = new System.Windows.Forms.Button();
		this.cmbUOM = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.CmbWarehouse = new UltraCombo();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.PanelHeaderInfo = new System.Windows.Forms.Panel();
		this.txtManualNo = new System.Windows.Forms.TextBox();
		this.CmbGatePassType = new UltraCombo();
		this.txtgpdate = new System.Windows.Forms.DateTimePicker();
		this.label18 = new System.Windows.Forms.Label();
		this.txtPartyName = new UltraCombo();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.txtSupplierWeight = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtManualNoCaption = new System.Windows.Forms.Label();
		this.txtFactoryWeight = new System.Windows.Forms.TextBox();
		this.lblFactoryWt = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.CmbStatus = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbWeighBridge = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.labelGpTypeCaption = new System.Windows.Forms.Label();
		this.CmbDeliveryChallanNo = new UltraCombo();
		this.cmbvehicletype = new UltraCombo();
		this.label32 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.txtgpno = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.lblDeliveryChallanNo = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnLookup = new System.Windows.Forms.ToolStripButton();
		this.btnshortcut = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdhistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label52 = new System.Windows.Forms.Label();
		this.grdHistoryDetail = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel10 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label16 = new System.Windows.Forms.Label();
		this.cmbPartyHistory = new UltraCombo();
		this.rdapproved = new System.Windows.Forms.RadioButton();
		this.rdpqtymodify = new System.Windows.Forms.RadioButton();
		this.rdpqtydoc = new System.Windows.Forms.RadioButton();
		this.rdpqtyentry = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label60 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOpenGp).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseToUpdate).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotToUpdate).BeginInit();
		this.PanelDetailInfo.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).BeginInit();
		this.PanelHeaderInfo.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbGatePassType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtPartyName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWeighBridge).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryChallanNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicletype).BeginInit();
		this.panel3.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel13.SuspendLayout();
		this.panel10.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPartyHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1004, 661);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1004, 661);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdOpenGp);
		this.tabPage1.Controls.Add(this.panel11);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage1.Controls.Add(this.panel14);
		this.tabPage1.Controls.Add(this.PanelDetailInfo);
		this.tabPage1.Controls.Add(this.PanelHeaderInfo);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(996, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grdOpenGp.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdOpenGp).BackColor = System.Drawing.Color.White;
		grdOpenGp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOpenGp.DesignTimeLayout = grdOpenGp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOpenGp).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOpenGp.DynamicFiltering = true;
		this.grdOpenGp.FilterMode = (FilterMode)2;
		this.grdOpenGp.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOpenGp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOpenGp.FrozenColumns = 1;
		this.grdOpenGp.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdOpenGp).Location = new System.Drawing.Point(2, 467);
		((System.Windows.Forms.Control)(object)this.grdOpenGp).Name = "grdOpenGp";
		((ScrollableBase)this.grdOpenGp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOpenGp).Size = new System.Drawing.Size(992, 163);
		((System.Windows.Forms.Control)(object)this.grdOpenGp).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.grdOpenGp).TabStop = false;
		this.grdOpenGp.TotalRow = (InheritableBoolean)1;
		this.grdOpenGp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOpenGp).VisualStyle = (VisualStyle)4;
		this.grdOpenGp.ColumnButtonClick += new ColumnActionEventHandler(grdOpenGp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOpenGp).DoubleClick += new System.EventHandler(grdOpenGp_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdOpenGp).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOpenGp_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.ctrlGrdBar3);
		this.panel11.Controls.Add(this.label20);
		this.panel11.Controls.Add(this.ctrlGrdBar1);
		this.panel11.Controls.Add(this.label11);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(2, 446);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(992, 21);
		this.panel11.TabIndex = 47;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(949, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdOpenGp;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(41, 20);
		this.ctrlGrdBar3.TabIndex = 61;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label20.AutoSize = true;
		this.label20.BackColor = System.Drawing.Color.Teal;
		this.label20.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label20.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label20.Location = new System.Drawing.Point(0, 2);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(201, 17);
		this.label20.TabIndex = 1;
		this.label20.Text = "Gate Pass With Open Status";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1274, -2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grdOpenGp;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(60, 31);
		this.ctrlGrdBar1.TabIndex = 59;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label11.AutoSize = true;
		this.label11.BackColor = System.Drawing.Color.Teal;
		this.label11.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label11.Location = new System.Drawing.Point(4, 2);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(0, 19);
		this.label11.TabIndex = 0;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(2, 260);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(992, 186);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 3;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.RadAll);
		this.panel14.Controls.Add(this.RadNotReffered);
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouseToUpdate);
		this.panel14.Controls.Add(this.BtnUpdateSameValuesInGrid);
		this.panel14.Controls.Add(this.label66);
		this.panel14.Controls.Add(this.label65);
		this.panel14.Controls.Add(this.label49);
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLotToUpdate);
		this.panel14.Controls.Add(this.ctrlGrdBar4);
		this.panel14.Controls.Add(this.label19);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(2, 233);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(992, 27);
		this.panel14.TabIndex = 2;
		this.RadAll.AutoSize = true;
		this.RadAll.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold);
		this.RadAll.ForeColor = System.Drawing.Color.FromArgb(192, 255, 255);
		this.RadAll.Location = new System.Drawing.Point(777, 5);
		this.RadAll.Name = "RadAll";
		this.RadAll.Size = new System.Drawing.Size(39, 17);
		this.RadAll.TabIndex = 2;
		this.RadAll.Text = "All";
		this.RadAll.UseVisualStyleBackColor = true;
		this.RadNotReffered.AutoSize = true;
		this.RadNotReffered.Checked = true;
		this.RadNotReffered.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold);
		this.RadNotReffered.ForeColor = System.Drawing.Color.FromArgb(192, 255, 255);
		this.RadNotReffered.Location = new System.Drawing.Point(817, 5);
		this.RadNotReffered.Name = "RadNotReffered";
		this.RadNotReffered.Size = new System.Drawing.Size(61, 17);
		this.RadNotReffered.TabIndex = 3;
		this.RadNotReffered.TabStop = true;
		this.RadNotReffered.Text = "Empty";
		this.RadNotReffered.UseVisualStyleBackColor = true;
		this.CmbWareHouseToUpdate.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouseToUpdate.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouseToUpdate.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouseToUpdate.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.Appearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance195;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance196).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance196).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance200).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance201).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouseToUpdate).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseToUpdate).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWareHouseToUpdate.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseToUpdate).Location = new System.Drawing.Point(426, 0);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseToUpdate).Name = "CmbWareHouseToUpdate";
		((System.Windows.Forms.Control)(object)this.CmbWareHouseToUpdate).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseToUpdate).TabIndex = 0;
		this.BtnUpdateSameValuesInGrid.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateSameValuesInGrid.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnUpdateSameValuesInGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateSameValuesInGrid.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateSameValuesInGrid.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateSameValuesInGrid.Location = new System.Drawing.Point(880, 1);
		this.BtnUpdateSameValuesInGrid.Name = "BtnUpdateSameValuesInGrid";
		this.BtnUpdateSameValuesInGrid.Size = new System.Drawing.Size(65, 25);
		this.BtnUpdateSameValuesInGrid.TabIndex = 4;
		this.BtnUpdateSameValuesInGrid.Text = "Update";
		this.BtnUpdateSameValuesInGrid.UseVisualStyleBackColor = false;
		this.BtnUpdateSameValuesInGrid.Click += new System.EventHandler(BtnUpdateSameValuesInGrid_Click);
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.ForeColor = System.Drawing.Color.FromArgb(192, 255, 255);
		this.label66.Location = new System.Drawing.Point(353, 7);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(72, 13);
		this.label66.TabIndex = 160;
		this.label66.Text = "WareHouse";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.ForeColor = System.Drawing.Color.FromArgb(192, 255, 255);
		this.label65.Location = new System.Drawing.Point(576, 7);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(45, 13);
		this.label65.TabIndex = 161;
		this.label65.Text = "JobLot";
		this.label49.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.label49.ForeColor = System.Drawing.Color.White;
		this.label49.Location = new System.Drawing.Point(86, -2);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(271, 28);
		this.label49.TabIndex = 159;
		this.label49.Text = "Click to update JobLot and Warehouse, or apply first-row values to all rows if both fields are empty.";
		this.CmbJobLotToUpdate.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLotToUpdate.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLotToUpdate.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLotToUpdate.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.Appearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance207;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance208).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance208).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance208).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance211;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance212).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLotToUpdate).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLotToUpdate).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLotToUpdate.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLotToUpdate).Location = new System.Drawing.Point(621, 0);
		((System.Windows.Forms.Control)(object)this.CmbJobLotToUpdate).Name = "CmbJobLotToUpdate";
		((System.Windows.Forms.Control)(object)this.CmbJobLotToUpdate).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLotToUpdate).TabIndex = 1;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ctrlGrdBar4.FormName = null;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(953, 2);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.ctrlGrdBar4.MyGrid = this.grd;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar4.TabIndex = 2;
		this.label19.AutoSize = true;
		this.label19.BackColor = System.Drawing.Color.Teal;
		this.label19.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label19.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label19.Location = new System.Drawing.Point(0, 3);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(84, 17);
		this.label19.TabIndex = 1;
		this.label19.Text = "Item Detail";
		this.PanelDetailInfo.Controls.Add(this.groupBox2);
		this.PanelDetailInfo.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelDetailInfo.Location = new System.Drawing.Point(2, 171);
		this.PanelDetailInfo.Name = "PanelDetailInfo";
		this.PanelDetailInfo.Size = new System.Drawing.Size(992, 62);
		this.PanelDetailInfo.TabIndex = 1;
		this.groupBox2.Controls.Add(this.rdbtnItemCode);
		this.groupBox2.Controls.Add(this.rdbtnItemName);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLot);
		this.groupBox2.Controls.Add(this.label33);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox2.Controls.Add(this.txtRemarksDetail);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label26);
		this.groupBox2.Controls.Add(this.btnCancelDetail);
		this.groupBox2.Controls.Add(this.Add);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUOM);
		this.groupBox2.Controls.Add(this.label28);
		this.groupBox2.Controls.Add(this.label44);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouse);
		this.groupBox2.Controls.Add(this.txtqty);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(1, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(964, 60);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(88, 11);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 248;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(34, 11);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 247;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.rdbtnItemName.CheckedChanged += new System.EventHandler(rdbtnItemName_CheckedChanged);
		this.cmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance221).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance221).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance222;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance223).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance223).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance224;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance225).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance226;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance227).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance228).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance228).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance228).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance229).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Location = new System.Drawing.Point(378, 30);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Name = "cmbJobLot";
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).TabIndex = 2;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(378, 13);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(46, 15);
		this.label33.TabIndex = 245;
		this.label33.Text = "Job Lot";
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance126;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance131).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance132).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance217).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(4, 30);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(CmbItemName_Leave);
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(702, 32);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(197, 23);
		this.txtRemarksDetail.TabIndex = 5;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(206, 13);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(67, 15);
		this.label23.TabIndex = 106;
		this.label23.Text = "Warehouse";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(631, 13);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(26, 15);
		this.label24.TabIndex = 110;
		this.label24.Text = "Qty";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(702, 13);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(52, 15);
		this.label26.TabIndex = 113;
		this.label26.Text = "Remarks";
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(900, 12);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(67, 22);
		this.btnCancelDetail.TabIndex = 8;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.Add.BackColor = System.Drawing.Color.Teal;
		this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Add.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.ForeColor = System.Drawing.Color.White;
		this.Add.Location = new System.Drawing.Point(900, 33);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(40, 22);
		this.Add.TabIndex = 7;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click);
		this.cmbUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance244).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.cmbUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance245).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance245).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance246;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance247).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance247).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance247).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance248).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance248;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance249).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance260).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance261).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance261).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance261;
		((AppearanceBase)appearance262).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance263).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Location = new System.Drawing.Point(539, 30);
		((System.Windows.Forms.Control)(object)this.cmbUOM).Name = "cmbUOM";
		((System.Windows.Forms.Control)(object)this.cmbUOM).Size = new System.Drawing.Size(90, 26);
		((System.Windows.Forms.Control)(object)this.cmbUOM).TabIndex = 3;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(3, 13);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(32, 15);
		this.label28.TabIndex = 108;
		this.label28.Text = "Item";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(539, 13);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(35, 15);
		this.label44.TabIndex = 107;
		this.label44.Text = "UOM";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(900, 33);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(67, 22);
		this.btnUpdateDetail.TabIndex = 10;
		this.btnUpdateDetail.Text = "Update ";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.CmbWarehouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance232).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance233).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance233).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance233;
		((AppearanceBase)appearance234).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance234;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance235).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance235).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance235).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance236).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance236;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance237).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance238;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance239).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance240).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Location = new System.Drawing.Point(206, 30);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Name = "CmbWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).TabIndex = 1;
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqty.Location = new System.Drawing.Point(631, 32);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(70, 23);
		this.txtqty.TabIndex = 4;
		this.txtqty.Text = "0";
		this.txtqty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.PanelHeaderInfo.BackColor = System.Drawing.Color.White;
		this.PanelHeaderInfo.Controls.Add(this.txtManualNo);
		this.PanelHeaderInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGatePassType);
		this.PanelHeaderInfo.Controls.Add(this.txtgpdate);
		this.PanelHeaderInfo.Controls.Add(this.label18);
		this.PanelHeaderInfo.Controls.Add((System.Windows.Forms.Control)(object)this.txtPartyName);
		this.PanelHeaderInfo.Controls.Add(this.txtremarks);
		this.PanelHeaderInfo.Controls.Add(this.txtSupplierWeight);
		this.PanelHeaderInfo.Controls.Add(this.label9);
		this.PanelHeaderInfo.Controls.Add(this.txtManualNoCaption);
		this.PanelHeaderInfo.Controls.Add(this.txtFactoryWeight);
		this.PanelHeaderInfo.Controls.Add(this.lblFactoryWt);
		this.PanelHeaderInfo.Controls.Add(this.label12);
		this.PanelHeaderInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStatus);
		this.PanelHeaderInfo.Controls.Add(this.label10);
		this.PanelHeaderInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWeighBridge);
		this.PanelHeaderInfo.Controls.Add(this.label7);
		this.PanelHeaderInfo.Controls.Add(this.label6);
		this.PanelHeaderInfo.Controls.Add(this.labelGpTypeCaption);
		this.PanelHeaderInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryChallanNo);
		this.PanelHeaderInfo.Controls.Add((System.Windows.Forms.Control)(object)this.cmbvehicletype);
		this.PanelHeaderInfo.Controls.Add(this.label32);
		this.PanelHeaderInfo.Controls.Add(this.label17);
		this.PanelHeaderInfo.Controls.Add(this.txtvehicleno);
		this.PanelHeaderInfo.Controls.Add(this.txtgpno);
		this.PanelHeaderInfo.Controls.Add(this.label14);
		this.PanelHeaderInfo.Controls.Add(this.label25);
		this.PanelHeaderInfo.Controls.Add(this.label1);
		this.PanelHeaderInfo.Controls.Add(this.lblDeliveryChallanNo);
		this.PanelHeaderInfo.Controls.Add(this.label5);
		this.PanelHeaderInfo.Controls.Add(this.label8);
		this.PanelHeaderInfo.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelHeaderInfo.Location = new System.Drawing.Point(2, 55);
		this.PanelHeaderInfo.Margin = new System.Windows.Forms.Padding(2);
		this.PanelHeaderInfo.Name = "PanelHeaderInfo";
		this.PanelHeaderInfo.Size = new System.Drawing.Size(992, 116);
		this.PanelHeaderInfo.TabIndex = 0;
		this.txtManualNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtManualNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtManualNo.Location = new System.Drawing.Point(501, 7);
		this.txtManualNo.Name = "txtManualNo";
		this.txtManualNo.Size = new System.Drawing.Size(201, 23);
		this.txtManualNo.TabIndex = 6;
		this.CmbGatePassType.AllowNull = (DefaultableBoolean)1;
		this.CmbGatePassType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGatePassType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGatePassType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGatePassType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.CmbGatePassType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGatePassType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGatePassType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGatePassType).Location = new System.Drawing.Point(93, 33);
		((System.Windows.Forms.Control)(object)this.CmbGatePassType).Name = "CmbGatePassType";
		((System.Windows.Forms.Control)(object)this.CmbGatePassType).Size = new System.Drawing.Size(314, 26);
		((System.Windows.Forms.Control)(object)this.CmbGatePassType).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbGatePassType).Leave += new System.EventHandler(CmbGatePassType_Leave);
		this.txtgpdate.CustomFormat = "dd-MMM-yyyy";
		this.txtgpdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtgpdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtgpdate.Location = new System.Drawing.Point(93, 7);
		this.txtgpdate.Name = "txtgpdate";
		this.txtgpdate.Size = new System.Drawing.Size(130, 22);
		this.txtgpdate.TabIndex = 0;
		this.label18.AutoSize = true;
		this.label18.BackColor = System.Drawing.Color.White;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(228, 10);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(52, 17);
		this.label18.TabIndex = 93;
		this.label18.Text = "Gp Doc";
		this.txtPartyName.AllowNull = (DefaultableBoolean)1;
		this.txtPartyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.txtPartyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.txtPartyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.txtPartyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.txtPartyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance251).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance251).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance251).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.txtPartyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance251;
		((AppearanceBase)appearance252).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtPartyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance252;
		((SpecialBoxBase)((UltraGridBase)this.txtPartyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance253).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance253).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance253).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtPartyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.txtPartyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.txtPartyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance254).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance254;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance255).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance256;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance257).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance258).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance258).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance258).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance258).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance258;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.txtPartyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.txtPartyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.txtPartyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.txtPartyName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.txtPartyName).Location = new System.Drawing.Point(93, 61);
		((System.Windows.Forms.Control)(object)this.txtPartyName).Name = "txtPartyName";
		((System.Windows.Forms.Control)(object)this.txtPartyName).Size = new System.Drawing.Size(314, 26);
		((System.Windows.Forms.Control)(object)this.txtPartyName).TabIndex = 4;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(93, 89);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(609, 23);
		this.txtremarks.TabIndex = 5;
		this.txtSupplierWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierWeight.Location = new System.Drawing.Point(811, 89);
		this.txtSupplierWeight.Name = "txtSupplierWeight";
		this.txtSupplierWeight.Size = new System.Drawing.Size(130, 23);
		this.txtSupplierWeight.TabIndex = 12;
		this.txtSupplierWeight.Text = "0";
		this.txtSupplierWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSupplierWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSupplierWeight_KeyPress);
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.White;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(714, 92);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(88, 17);
		this.label9.TabIndex = 92;
		this.label9.Text = "Party Weight";
		this.txtManualNoCaption.AutoSize = true;
		this.txtManualNoCaption.BackColor = System.Drawing.Color.White;
		this.txtManualNoCaption.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtManualNoCaption.Location = new System.Drawing.Point(411, 10);
		this.txtManualNoCaption.Name = "txtManualNoCaption";
		this.txtManualNoCaption.Size = new System.Drawing.Size(75, 17);
		this.txtManualNoCaption.TabIndex = 90;
		this.txtManualNoCaption.Text = "Manual No";
		this.txtFactoryWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFactoryWeight.Enabled = false;
		this.txtFactoryWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFactoryWeight.Location = new System.Drawing.Point(811, 63);
		this.txtFactoryWeight.Name = "txtFactoryWeight";
		this.txtFactoryWeight.ReadOnly = true;
		this.txtFactoryWeight.Size = new System.Drawing.Size(130, 23);
		this.txtFactoryWeight.TabIndex = 11;
		this.txtFactoryWeight.TabStop = false;
		this.txtFactoryWeight.Text = "0";
		this.txtFactoryWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFactoryWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFactoryWeight_KeyPress);
		this.lblFactoryWt.AutoSize = true;
		this.lblFactoryWt.BackColor = System.Drawing.Color.White;
		this.lblFactoryWt.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold);
		this.lblFactoryWt.Location = new System.Drawing.Point(714, 66);
		this.lblFactoryWt.Name = "lblFactoryWt";
		this.lblFactoryWt.Size = new System.Drawing.Size(98, 17);
		this.lblFactoryWt.TabIndex = 88;
		this.lblFactoryWt.Text = "FactoryWeight";
		this.label12.AutoSize = true;
		this.label12.BackColor = System.Drawing.Color.White;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(714, 10);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(67, 17);
		this.label12.TabIndex = 86;
		this.label12.Text = "Gp Status";
		this.CmbStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Location = new System.Drawing.Point(811, 5);
		((System.Windows.Forms.Control)(object)this.CmbStatus).Name = "CmbStatus";
		((System.Windows.Forms.Control)(object)this.CmbStatus).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.CmbStatus).TabIndex = 9;
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.White;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(714, 38);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(71, 17);
		this.label10.TabIndex = 76;
		this.label10.Text = "WB Status";
		this.CmbWeighBridge.AllowNull = (DefaultableBoolean)1;
		this.CmbWeighBridge.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWeighBridge.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWeighBridge.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWeighBridge).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance147;
		((SpecialBoxBase)((UltraGridBase)this.CmbWeighBridge).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance152).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWeighBridge).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWeighBridge.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWeighBridge).Location = new System.Drawing.Point(811, 33);
		((System.Windows.Forms.Control)(object)this.CmbWeighBridge).Name = "CmbWeighBridge";
		((System.Windows.Forms.Control)(object)this.CmbWeighBridge).Size = new System.Drawing.Size(131, 26);
		((System.Windows.Forms.Control)(object)this.CmbWeighBridge).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.CmbWeighBridge).Leave += new System.EventHandler(CmbWeighBridge_Leave);
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.White;
		this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.Red;
		this.label7.Location = new System.Drawing.Point(82, 8);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(15, 20);
		this.label7.TabIndex = 2;
		this.label7.Text = "*";
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Red;
		this.label6.Location = new System.Drawing.Point(82, 36);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(15, 20);
		this.label6.TabIndex = 0;
		this.label6.Text = "*";
		this.labelGpTypeCaption.AutoSize = true;
		this.labelGpTypeCaption.BackColor = System.Drawing.Color.White;
		this.labelGpTypeCaption.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelGpTypeCaption.Location = new System.Drawing.Point(5, 38);
		this.labelGpTypeCaption.Name = "labelGpTypeCaption";
		this.labelGpTypeCaption.Size = new System.Drawing.Size(60, 17);
		this.labelGpTypeCaption.TabIndex = 70;
		this.labelGpTypeCaption.Text = "G.P Type";
		this.CmbDeliveryChallanNo.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryChallanNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryChallanNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryChallanNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance159;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance164).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryChallanNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryChallanNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryChallanNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryChallanNo).Location = new System.Drawing.Point(295, 33);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryChallanNo).Name = "CmbDeliveryChallanNo";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryChallanNo).Size = new System.Drawing.Size(112, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryChallanNo).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryChallanNo).Visible = false;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryChallanNo).Leave += new System.EventHandler(CmbDeliveryChallanNo_Leave);
		this.cmbvehicletype.AllowNull = (DefaultableBoolean)1;
		this.cmbvehicletype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbvehicletype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbvehicletype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance171;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance176).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbvehicletype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Location = new System.Drawing.Point(501, 33);
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Name = "cmbvehicletype";
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).TabIndex = 7;
		this.label32.AutoSize = true;
		this.label32.BackColor = System.Drawing.Color.White;
		this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.Red;
		this.label32.Location = new System.Drawing.Point(486, 36);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(15, 20);
		this.label32.TabIndex = 68;
		this.label32.Text = "*";
		this.label17.AutoSize = true;
		this.label17.BackColor = System.Drawing.Color.White;
		this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.Red;
		this.label17.Location = new System.Drawing.Point(791, 8);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(15, 20);
		this.label17.TabIndex = 4;
		this.label17.Text = "*";
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehicleno.Location = new System.Drawing.Point(501, 63);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(201, 23);
		this.txtvehicleno.TabIndex = 8;
		this.txtgpno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgpno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgpno.Location = new System.Drawing.Point(295, 7);
		this.txtgpno.Name = "txtgpno";
		this.txtgpno.ReadOnly = true;
		this.txtgpno.Size = new System.Drawing.Size(112, 23);
		this.txtgpno.TabIndex = 1;
		this.txtgpno.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtgpno.Leave += new System.EventHandler(txtgpno_TextChanged);
		this.label14.AutoSize = true;
		this.label14.BackColor = System.Drawing.Color.White;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(5, 92);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(60, 17);
		this.label14.TabIndex = 53;
		this.label14.Text = "Remarks";
		this.label25.AutoSize = true;
		this.label25.BackColor = System.Drawing.Color.White;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(5, 66);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(80, 17);
		this.label25.TabIndex = 50;
		this.label25.Text = "Party Name";
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 10);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(57, 17);
		this.label1.TabIndex = 60;
		this.label1.Text = "Gp Date";
		this.lblDeliveryChallanNo.AutoSize = true;
		this.lblDeliveryChallanNo.BackColor = System.Drawing.Color.White;
		this.lblDeliveryChallanNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDeliveryChallanNo.Location = new System.Drawing.Point(228, 38);
		this.lblDeliveryChallanNo.Name = "lblDeliveryChallanNo";
		this.lblDeliveryChallanNo.Size = new System.Drawing.Size(49, 17);
		this.lblDeliveryChallanNo.TabIndex = 48;
		this.lblDeliveryChallanNo.Text = "D.C No";
		this.lblDeliveryChallanNo.Visible = false;
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.White;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(411, 66);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(71, 17);
		this.label5.TabIndex = 48;
		this.label5.Text = "Vehicle No";
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.White;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(411, 38);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(81, 17);
		this.label8.TabIndex = 48;
		this.label8.Text = "Vehicle Type";
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 31);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(992, 24);
		this.panel3.TabIndex = 1;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label15);
		this.panel6.Controls.Add(this.ChkBox);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(992, 24);
		this.panel6.TabIndex = 46;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(4, 2);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(236, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Gate Pass Inward (General)";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Checked = true;
		this.ChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(868, 2);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(122, 20);
		this.ChkBox.TabIndex = 15;
		this.ChkBox.Text = "Print Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.ChkBox.Leave += new System.EventHandler(ChkBox_Leave);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(992, 29);
		this.panel2.TabIndex = 0;
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnnew, this.btnRefresh, this.btnupdate, this.btnsave, this.btnPrint, this.btnAttachment, this.btnLookup, this.btnshortcut });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(992, 27);
		this.toolStrip1.TabIndex = 58;
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
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = (System.Drawing.Image)resources.GetObject("btnPrint.Image");
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "254-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click_1);
		this.btnLookup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnLookup.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLookup.Image = (System.Drawing.Image)resources.GetObject("btnLookup.Image");
		this.btnLookup.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLookup.Name = "btnLookup";
		this.btnLookup.Size = new System.Drawing.Size(110, 24);
		this.btnLookup.Text = "Look-Up Define";
		this.btnLookup.Click += new System.EventHandler(btnLookup_Click);
		this.btnshortcut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcut.Name = "btnshortcut";
		this.btnshortcut.Size = new System.Drawing.Size(126, 24);
		this.btnshortcut.Text = "ShortCut Keys";
		this.btnshortcut.Click += new System.EventHandler(btnshortcut_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(996, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.FrozenColumns = 2;
		this.grdhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(2, 136);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(992, 277);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 45;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label52);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel9.Location = new System.Drawing.Point(2, 413);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(992, 24);
		this.panel9.TabIndex = 65;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.White;
		this.label52.Location = new System.Drawing.Point(4, 5);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(225, 16);
		this.label52.TabIndex = 129;
		this.label52.Text = "Detail of the Above Selected Row";
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdHistoryDetail.DynamicFiltering = true;
		this.grdHistoryDetail.FilterMode = (FilterMode)2;
		this.grdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(2, 437);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(992, 193);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 64;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.ctrlGrdBar2);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(2, 114);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(992, 22);
		this.panel13.TabIndex = 63;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(945, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(44, 20);
		this.ctrlGrdBar2.TabIndex = 60;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load_1);
		this.panel10.Controls.Add(this.groupBox3);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(2, 50);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(992, 64);
		this.panel10.TabIndex = 62;
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPartyHistory);
		this.groupBox3.Controls.Add(this.rdapproved);
		this.groupBox3.Controls.Add(this.rdpqtymodify);
		this.groupBox3.Controls.Add(this.rdpqtydoc);
		this.groupBox3.Controls.Add(this.rdpqtyentry);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label60);
		this.groupBox3.Controls.Add(this.label61);
		this.groupBox3.Controls.Add(this.label62);
		this.groupBox3.Controls.Add(this.label63);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(0, 1);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(946, 62);
		this.groupBox3.TabIndex = 26;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.label16.AutoSize = true;
		this.label16.BackColor = System.Drawing.Color.White;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(452, 17);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(69, 15);
		this.label16.TabIndex = 144;
		this.label16.Text = "Party Name";
		this.cmbPartyHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbPartyHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPartyHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPartyHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPartyHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance183;
		((SpecialBoxBase)((UltraGridBase)this.cmbPartyHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance188).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPartyHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPartyHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPartyHistory).Location = new System.Drawing.Point(452, 32);
		((System.Windows.Forms.Control)(object)this.cmbPartyHistory).Name = "cmbPartyHistory";
		((System.Windows.Forms.Control)(object)this.cmbPartyHistory).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbPartyHistory).TabIndex = 4;
		this.rdapproved.AutoSize = true;
		this.rdapproved.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproved.Location = new System.Drawing.Point(815, 31);
		this.rdapproved.Name = "rdapproved";
		this.rdapproved.Size = new System.Drawing.Size(105, 19);
		this.rdapproved.TabIndex = 8;
		this.rdapproved.Text = "Approved Date";
		this.rdapproved.UseVisualStyleBackColor = true;
		this.rdpqtymodify.AutoSize = true;
		this.rdpqtymodify.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdpqtymodify.Location = new System.Drawing.Point(815, 13);
		this.rdpqtymodify.Name = "rdpqtymodify";
		this.rdpqtymodify.Size = new System.Drawing.Size(91, 19);
		this.rdpqtymodify.TabIndex = 7;
		this.rdpqtymodify.Text = "Modify Date";
		this.rdpqtymodify.UseVisualStyleBackColor = true;
		this.rdpqtydoc.AutoSize = true;
		this.rdpqtydoc.Checked = true;
		this.rdpqtydoc.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdpqtydoc.Location = new System.Drawing.Point(734, 13);
		this.rdpqtydoc.Name = "rdpqtydoc";
		this.rdpqtydoc.Size = new System.Drawing.Size(75, 19);
		this.rdpqtydoc.TabIndex = 5;
		this.rdpqtydoc.TabStop = true;
		this.rdpqtydoc.Text = "Doc Date";
		this.rdpqtydoc.UseVisualStyleBackColor = true;
		this.rdpqtyentry.AutoSize = true;
		this.rdpqtyentry.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdpqtyentry.Location = new System.Drawing.Point(734, 31);
		this.rdpqtyentry.Name = "rdpqtyentry";
		this.rdpqtyentry.Size = new System.Drawing.Size(80, 19);
		this.rdpqtyentry.TabIndex = 6;
		this.rdpqtyentry.Text = "Entry Date";
		this.rdpqtyentry.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(675, 34);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(53, 23);
		this.btnshow.TabIndex = 9;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(370, 17);
		this.label60.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(64, 15);
		this.label60.TabIndex = 141;
		this.label60.Text = "To Doc No";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.Location = new System.Drawing.Point(288, 17);
		this.label61.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(79, 15);
		this.label61.TabIndex = 140;
		this.label61.Text = "From Doc No";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(146, 17);
		this.label62.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(48, 15);
		this.label62.TabIndex = 139;
		this.label62.Text = "To Date";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(5, 17);
		this.label63.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(63, 15);
		this.label63.TabIndex = 13;
		this.label63.Text = "From Date";
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(370, 34);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(80, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(288, 34);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(80, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(146, 34);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(140, 22);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(5, 34);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(140, 22);
		this.FromDateHistory.TabIndex = 0;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label13);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 26);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(992, 24);
		this.panel7.TabIndex = 61;
		this.label13.AutoSize = true;
		this.label13.BackColor = System.Drawing.Color.Teal;
		this.label13.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label13.Location = new System.Drawing.Point(1, 1);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(299, 19);
		this.label13.TabIndex = 61;
		this.label13.Text = "Inward Gate Pass (General) History";
		this.panel5.Controls.Add(this.toolStrip2);
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 2);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(992, 24);
		this.panel5.TabIndex = 0;
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(992, 25);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(64, 22);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(76, 22);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.panel8.Location = new System.Drawing.Point(0, 306);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(828, 318);
		this.panel8.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1004, 661);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "GatePassGeneral";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(GatePassGeneral_FormClosing);
		base.Load += new System.EventHandler(InwardGatePass_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InwardGatePass_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOpenGp).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseToUpdate).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotToUpdate).EndInit();
		this.PanelDetailInfo.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).EndInit();
		this.PanelHeaderInfo.ResumeLayout(false);
		this.PanelHeaderInfo.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbGatePassType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtPartyName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWeighBridge).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryChallanNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicletype).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPartyHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
