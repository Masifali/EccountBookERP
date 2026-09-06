using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.CommissionAgent;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Cmagt;

public class frmPendingGdnLoader : Form
{
	private UserAccount UserAccount = new UserAccount();

	public DataTable dtLoader = new DataTable();

	public DataTable dtGrid = new DataTable();

	public DataSet ds = new DataSet();

	private DataTable dtHistory = new DataTable();

	private bool FormInitialized;

	private int DocumentTypeId = 1055;

	public int SupplierId;

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Panel panel2;

	private Button btngrnlod;

	private Panel panel3;

	internal GridEX grd;

	private DateTimePicker ToDate;

	private DateTimePicker FromDate;

	private Label label2;

	private Label label3;

	private TextBox txtDocNoTo;

	private TextBox txtDocNoFrom;

	private ToolStrip toolStrip1;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnShortcutKeys;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton BtnLoadSelectedRows;

	internal GridEX grdDetail;

	private Panel panel4;

	private Label label5;

	internal UltraCombo CmbCommissionAgent;

	private Label label6;

	internal UltraCombo CmbItemName;

	internal UltraCombo CmbSupplier;

	private Label label8;

	internal UltraCombo CmbBuyerName;

	private Label label9;

	private Label label1;

	private Label label35;

	internal UltraCombo CmbDeliveryToParty;

	private Label label60;

	internal UltraCombo CmbShipToAddress;

	private CtrlGrdBar ctrlGrdBar6;

	public frmPendingGdnLoader(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentMethod();
	}

	private async void InitializeComponentMethod()
	{
		try
		{
			CombosFill((await Task.Run(() => new
			{
				ComboData = ComboDbCall()
			})).ComboData);
			if (SupplierId > 0)
			{
				CmbBuyerName.Value = SupplierId;
			}
			PendingDataDbCall();
			GrdDataBind();
			FromDate.Value = DateTime.Now.AddDays(-7.0);
			FromDate.Focus();
			FormInitialized = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error occurred during database call: " + ex.Message);
		}
	}

	private void frmPendingGdnLoader_Load(object sender, EventArgs e)
	{
		AutoScroll = true;
		base.AutoScrollMargin = new Size(20, 20);
		base.AutoScrollMinSize = new Size(1000, 650);
	}

	private DataTable ComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		try
		{
			return gdnBuyerDispatchMaster.GetDataForDropDown(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchesIds = Conversion.ToString((object)UserAccount.BranchesId),
				DocumentTypeId = DocumentTypeId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CombosFill(DataTable dtcombo)
	{
		try
		{
			DataTable dtCA = CreateTable();
			DataTable dtSC = CreateTable();
			DataTable dtBA = CreateTable();
			DataTable dtItem = CreateTable();
			DataTable dtDeliveryToPartyName = CreateTable();
			DataTable dtShipToAddress = CreateTable();
			HashSet<int> caIds = new HashSet<int>();
			HashSet<int> scIds = new HashSet<int>();
			HashSet<int> baIds = new HashSet<int>();
			HashSet<int> itemIds = new HashSet<int>();
			HashSet<int> deliveryIds = new HashSet<int>();
			HashSet<string> shipToSet = new HashSet<string>();
			foreach (DataRow row in dtcombo.Rows)
			{
				string activity = Conversion.ToString(row["Activity"]);
				int id = Conversion.ToInt(row["Id"]);
				string name = Conversion.ToString(row["ReferenceName"]);
				switch (activity)
				{
				case "CommissionAgent":
					if (caIds.Add(id))
					{
						dtCA.Rows.Add(id, name);
					}
					break;
				case "SupplierName":
					if (scIds.Add(id))
					{
						dtSC.Rows.Add(id, name);
					}
					break;
				case "BuyerName":
					if (baIds.Add(id))
					{
						dtBA.Rows.Add(id, name);
					}
					break;
				case "Item":
					if (itemIds.Add(id))
					{
						dtItem.Rows.Add(id, name);
					}
					break;
				case "DeliveryToParty":
					if (deliveryIds.Add(id))
					{
						dtDeliveryToPartyName.Rows.Add(id, name);
					}
					break;
				case "DeliverToAddress":
				{
					string key = $"{id}|{name}";
					if (shipToSet.Add(key))
					{
						dtShipToAddress.Rows.Add(id, name);
					}
					break;
				}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbCommissionAgent, dtCA, "Id", "Name", "Commission Agent", AllColumns: true, Conversion.ToInt(CmbCommissionAgent.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbSupplier, dtSC, "Id", "Name", "Supplier Name", AllColumns: true, Conversion.ToInt(CmbSupplier.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbBuyerName, dtBA, "Id", "Name", "Buyer Name", AllColumns: true, Conversion.ToInt(CmbBuyerName.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbItemName, dtItem, "Id", "Name", "Item Name", AllColumns: true, Conversion.ToInt(CmbItemName.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbDeliveryToParty, dtDeliveryToPartyName, "Id", "Name", "Deliver To Party", AllColumns: true, Conversion.ToInt(CmbDeliveryToParty.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbShipToAddress, dtShipToAddress, "Id", "Name", "Deliver To Address", AllColumns: true, Conversion.ToInt(CmbShipToAddress.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		static DataTable CreateTable()
		{
			return new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					"Name"
				}
			};
		}
	}

	private void btngrnlod_Click(object sender, EventArgs e)
	{
		try
		{
			PendingDataDbCall();
			GrdDataBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingDataDbCall(bool Intilization = false)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			ReportsParameters Obj = new ReportsParameters();
			Obj.OrganizationId = UserAccount.OrganizationId;
			Obj.CompanyId = UserAccount.CompanyId;
			Obj.BranchesId = UserAccount.BranchesId;
			Obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Obj.DocumentTypeId = DocumentTypeId;
			Obj.FromDate = (Intilization ? DateTime.Now.AddDays(-7.0) : Conversion.ToDateTime((object)FromDate.Value));
			Obj.ToDate = Conversion.ToDateTime((object)ToDate.Value);
			Obj.FromDocNo = Conversion.ToInt((object)txtDocNoFrom.Text);
			Obj.ToDocNo = Conversion.ToInt((object)txtDocNoTo.Text);
			Obj.CommissionAgentId = Conversion.ToInt(CmbCommissionAgent.Value);
			Obj.SupplierId = Conversion.ToInt(CmbSupplier.Value);
			Obj.ItemId = Conversion.ToInt(CmbItemName.Value);
			Obj.BuyerId = Conversion.ToInt(CmbBuyerName.Value);
			Obj.DeliverToPartyId = Conversion.ToInt(CmbDeliveryToParty.Value);
			Obj.ShipToAddress = Conversion.ToString((object)((Control)(object)CmbShipToAddress).Text);
			ds = gdnBuyerDispatchMaster.PendingDataLoaderForBill(Obj);
			dtHistory = ds?.Tables[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GrdDataBind()
	{
		try
		{
			if (dtHistory != null && dtHistory.Rows.Count > 0)
			{
				DataTable dataTable = dtGrid;
				if (dataTable != null && dataTable.Columns.Count == 0)
				{
					dtGrid.Columns.Add("Id", typeof(int));
					dtGrid.Columns.Add("DocNo", typeof(int));
					dtGrid.Columns.Add("DocDate", typeof(DateTime));
					dtGrid.Columns.Add("DocumentTypeId", typeof(int));
					dtGrid.Columns.Add("commissionAgentId", typeof(int));
					dtGrid.Columns.Add("CommissionAgentName", typeof(string));
					dtGrid.Columns.Add("buyerId", typeof(int));
					dtGrid.Columns.Add("BuyerName", typeof(string));
					dtGrid.Columns.Add("DeliveryToPartyId", typeof(int));
					dtGrid.Columns.Add("DeliveryToPartyName", typeof(string));
					dtGrid.Columns.Add("DeliverToAddress", typeof(string));
					dtGrid.Columns.Add("LoadingCityName", typeof(string));
					dtGrid.Columns.Add("UnloadingCityName", typeof(string));
					dtGrid.Columns.Add("DeliveryTerm", typeof(string));
					dtGrid.Columns.Add("TransporterName", typeof(string));
					dtGrid.Columns.Add("TotalFreight", typeof(decimal));
					dtGrid.Columns.Add("VehicleNo", typeof(string));
					dtGrid.Columns.Add("BiltyNo", typeof(string));
					dtGrid.Columns.Add("BiltyDate", typeof(DateTime));
					dtGrid.Columns.Add("BiltyQty", typeof(decimal));
					dtGrid.Columns.Add("BuyerLoadWeight", typeof(decimal));
					dtGrid.Columns.Add("BuyerTareWeight", typeof(decimal));
					dtGrid.Columns.Add("BuyerScaleNetWeight", typeof(decimal));
					dtGrid.Columns.Add("GrnNo", typeof(int));
					dtGrid.Columns.Add("GrnDate", typeof(string));
					dtGrid.Columns.Add("SupplierName", typeof(string));
					dtGrid.Columns.Add("GrnDeliveryTerm", typeof(string));
					dtGrid.Columns.Add("GrnTransporterName", typeof(string));
					dtGrid.Columns.Add("GrnTotalFreight", typeof(decimal));
					dtGrid.Columns.Add("GrnVehicleNo", typeof(string));
					dtGrid.Columns.Add("GrnBiltyNo", typeof(string));
					dtGrid.Columns.Add("GrnBiltyDate", typeof(DateTime));
					dtGrid.Columns.Add("GrnBiltyQty", typeof(decimal));
					dtGrid.Columns.Add("SupplierLoadWeight", typeof(decimal));
					dtGrid.Columns.Add("SupplierTareWeight", typeof(decimal));
					dtGrid.Columns.Add("SupplierScaleNetWeight", typeof(decimal));
					dtGrid.Columns.Add("WarningRemarks", typeof(string));
					dtGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtGrid.Columns.Add("EntryUserName", typeof(string));
					dtGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dtGrid.Columns.Add("ModifyUserName", typeof(string));
					dtGrid.Columns.Add("NoOfAttachments", typeof(int));
				}
				dtGrid.Clear();
				HashSet<int> addedIds = new HashSet<int>();
				foreach (DataRow r in dtHistory.Rows)
				{
					int id = Conversion.ToInt(r["gdnBuyerDispatchMasterId"]);
					if (addedIds.Add(id))
					{
						dtGrid.Rows.Add(r["gdnBuyerDispatchMasterId"], r["docNo"], r["docDate"], r["documentTypeId"], r["commissionAgentId"], r["CommissionAgentName"], r["buyerId"], r["BuyerName"], r["DeliveryToPartyId"], r["DeliveryToPartyName"], r["DeliverToAddress"], r["LoadingCityName"], r["UnloadingCityName"], r["DeliveryTerm"], r["transporterName"], r["totalFreight"], r["vehicleNo"], r["biltyNo"], r["biltyDate"], r["biltyQty"], r["BuyerLoadWeight"], r["BuyerTareWeight"], r["BuyerScaleNetWeight"], r["GrnNo"], Conversion.CheckDateTimeNull(Conversion.ToDateTime(r["GrnDate"])) ? "" : Conversion.ToDateTime(r["GrnDate"]).ToString("d"), r["SupplierName"], r["GrnDeliveryTerm"], r["GrnTransporterName"], r["GrnTotalFreight"], r["GrnVehicleNo"], r["GrnBiltyNo"], r["GrnBiltyDate"], r["GrnBiltyQty"], r["SupplierLoadWeight"], r["SupplierTareWeight"], r["SupplierScaleNetWeight"], r["warningRemarksHeader"], r["entryDate"], r["EntryUserName"], r["modifyDate"], r["ModifyUserName"], r["NoOfAttachments"]);
					}
				}
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings();
			}
			else
			{
				grd.DataSource = null;
				grdDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public void grdSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "commissionAgentId", "buyerId", "supplierId", "transporterId", "vehicleTypeId", "deliveryTermId", "DeliveryToPartyId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 3, hiddenColumns);
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GridEX_Helper.SetColumnWidth(grd, "DocNo", Constants.InventoryConstants.DocNoConstant);
			GridEX_Helper.SetColumnWidth(grd, "DocDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grd, "CommissionAgentName", 140);
			GridEX_Helper.SetColumnWidth(grd, "BuyerName", 140);
			GridEX_Helper.SetColumnWidth(grd, "SupplierName", 140);
			GridEX_Helper.SetColumnWidth(grd, "LoadingCityName", 100);
			GridEX_Helper.SetColumnWidth(grd, "UnloadingCityName", 100);
			GridEX_Helper.SetColumnWidth(grd, "SupplierRefDocNo", 120);
			GridEX_Helper.SetColumnWidth(grd, "DeliveryTerm", 80);
			GridEX_Helper.SetColumnWidth(grd, "GrnDeliveryTerm", 80);
			GridEX_Helper.SetColumnWidth(grd, "TransporterName", 140);
			GridEX_Helper.SetColumnWidth(grd, "GrnTransporterName", 140);
			GridEX_Helper.SetColumnWidth(grd, "TotalFreight", 90);
			GridEX_Helper.SetColumnWidth(grd, "GrnTotalFreight", 90);
			GridEX_Helper.SetColumnWidth(grd, "VehicleNo", 90);
			GridEX_Helper.SetColumnWidth(grd, "GrnVehicleNo", 90);
			GridEX_Helper.SetColumnWidth(grd, "BiltyNo", 80);
			GridEX_Helper.SetColumnWidth(grd, "BiltyDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grd, "BiltyQty", 70);
			GridEX_Helper.SetColumnWidth(grd, "GrnBiltyNo", 80);
			GridEX_Helper.SetColumnWidth(grd, "GrnBiltyDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grd, "GrnBiltyQty", 70);
			GridEX_Helper.SetColumnWidth(grd, "BuyerLoadWeight", 80);
			GridEX_Helper.SetColumnWidth(grd, "BuyerTareWeight", 80);
			GridEX_Helper.SetColumnWidth(grd, "BuyerScaleNetWeight", 80);
			GridEX_Helper.SetColumnWidth(grd, "SupplierLoadWeight", 80);
			GridEX_Helper.SetColumnWidth(grd, "SupplierTareWeight", 80);
			GridEX_Helper.SetColumnWidth(grd, "SupplierScaleNetWeight", 80);
			GridEX_Helper.SetColumnWidth(grd, "WarningRemarks", Constants.InventoryConstants.RemarksConstants);
			GridEX_Helper.SetColumnWidth(grd, "EntryUserName", Constants.InventoryConstants.EntryUserConstants);
			GridEX_Helper.SetColumnWidth(grd, "EntryDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grd, "ModifyUserName", Constants.InventoryConstants.ModifyUserConstants);
			GridEX_Helper.SetColumnWidth(grd, "ModifyDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grd, "NoOfAttachments", 80);
			GridEX_Helper.AddButton(grd, "Load", "Load This", 75, 0);
			grd.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), Conversion.ToInt(r.Cells["DocumentTypeId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				grdDetail.DataSource = null;
			}
			else
			{
				DetailGridBind(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridBind(int selectedId)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("DetailId", typeof(int));
			dt.Columns.Add("SaleOrderNo", typeof(int));
			dt.Columns.Add("SaleOrderDate", typeof(string));
			dt.Columns.Add("PurchaseOrderNo", typeof(int));
			dt.Columns.Add("PurchaseOrderDate", typeof(string));
			dt.Columns.Add("InventoryParentCategory", typeof(string));
			dt.Columns.Add("ItemName", typeof(string));
			dt.Columns.Add("PackUom", typeof(string));
			dt.Columns.Add("CropYear", typeof(string));
			dt.Columns.Add("PackingType", typeof(string));
			dt.Columns.Add("LoadingQty", typeof(decimal));
			dt.Columns.Add("GrossWeight", typeof(decimal));
			dt.Columns.Add("EbUnit", typeof(decimal));
			dt.Columns.Add("EbTotal", typeof(decimal));
			dt.Columns.Add("AddLessWeight", typeof(decimal));
			dt.Columns.Add("NetBillWeight", typeof(decimal));
			dt.Columns.Add("GrnGrossWeight", typeof(decimal));
			dt.Columns.Add("GrnEbUnit", typeof(decimal));
			dt.Columns.Add("GrnEbTotal", typeof(decimal));
			dt.Columns.Add("GrnAddLessWeight", typeof(decimal));
			dt.Columns.Add("GrnNetBillWeight", typeof(decimal));
			dt.Columns.Add("SaleRate", typeof(decimal));
			dt.Columns.Add("SaleRateUom", typeof(string));
			dt.Columns.Add("SaleAmount", typeof(decimal));
			dt.Columns.Add("TaxNameSale", typeof(string));
			dt.Columns.Add("TaxPercentSale", typeof(decimal));
			dt.Columns.Add("TotalAmountSale", typeof(decimal));
			dt.Columns.Add("PurchaseRate", typeof(decimal));
			dt.Columns.Add("PurchaseRateUom", typeof(string));
			dt.Columns.Add("PurchaseAmount", typeof(decimal));
			dt.Columns.Add("TaxNamePurchase", typeof(string));
			dt.Columns.Add("TaxPercentPurchase", typeof(decimal));
			dt.Columns.Add("TotalAmountPurchase", typeof(decimal));
			dt.Columns.Add("WarningRemarks", typeof(string));
			foreach (DataRow r2 in from r in dtHistory.AsEnumerable()
				where Conversion.ToInt(r["gdnBuyerDispatchMasterId"]) == selectedId
				select r)
			{
				DataRow newRow = dt.NewRow();
				newRow["Id"] = r2["gdnBuyerDispatchMasterId"];
				newRow["DetailId"] = r2["DetailId"];
				newRow["SaleOrderNo"] = r2["SaleOrderNo"];
				newRow["SaleOrderDate"] = (Conversion.CheckDateTimeNull(Conversion.ToDateTime(r2["SaleOrderDate"])) ? "" : Conversion.ToDateTime(r2["SaleOrderDate"]).ToString("d"));
				newRow["PurchaseOrderNo"] = r2["PurchaseOrderNo"];
				newRow["PurchaseOrderDate"] = (Conversion.CheckDateTimeNull(Conversion.ToDateTime(r2["PurchaseOrderDate"])) ? "" : Conversion.ToDateTime(r2["PurchaseOrderDate"]).ToString("d"));
				newRow["InventoryParentCategory"] = r2["inventoryParentCategory"];
				newRow["ItemName"] = r2["ItemName"];
				newRow["PackUom"] = r2["PackUomCode"];
				newRow["CropYear"] = r2["cropYear"];
				newRow["PackingType"] = r2["PackingType"];
				newRow["LoadingQty"] = r2["loadingQty"];
				newRow["GrossWeight"] = r2["wbGrossWeight"];
				newRow["EbUnit"] = r2["ebwPerUnit"];
				newRow["EbTotal"] = r2["ebwTotal"];
				newRow["AddLessWeight"] = r2["addLessWeight"];
				newRow["NetBillWeight"] = r2["netBillWeight"];
				newRow["GrnGrossWeight"] = r2["GrnGrossWeight"];
				newRow["GrnEbUnit"] = r2["GrnEbUnit"];
				newRow["GrnEbTotal"] = r2["GrnEbTotal"];
				newRow["GrnAddLessWeight"] = r2["GrnAddLessWeight"];
				newRow["GrnNetBillWeight"] = r2["GrnNetBillWeight"];
				newRow["SaleRate"] = r2["SaleRate"];
				newRow["SaleRateUom"] = r2["SaleRateUomCode"];
				newRow["SaleAmount"] = r2["SaleAmount"];
				newRow["TaxNameSale"] = r2["TaxNameSale"];
				newRow["TaxPercentSale"] = r2["TaxPercentSale"];
				newRow["TotalAmountSale"] = r2["TotalAmountSale"];
				newRow["PurchaseRate"] = r2["PurchaseRate"];
				newRow["PurchaseRateUom"] = r2["PurchaseRateUomCode"];
				newRow["PurchaseAmount"] = r2["PurchaseAmount"];
				newRow["TaxNamePurchase"] = r2["TaxNamePurchase"];
				newRow["TaxPercentPurchase"] = r2["TaxPercentPurchase"];
				newRow["TotalAmountPurchase"] = r2["TotalAmountPurchase"];
				newRow["WarningRemarks"] = r2["warningRemarks"];
				dt.Rows.Add(newRow);
			}
			grdDetail.DataSource = dt;
			grdDetail.RetrieveStructure();
			GridDetailSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "DetailId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 3, 3, hiddenColumns);
			GridEX_Helper.SetColumnWidth(grdDetail, "SaleOrderNo", 80);
			GridEX_Helper.SetColumnWidth(grdDetail, "SaleOrderDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchaseOrderNo", 80);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchaseOrderDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "InventoryParentCategory", 110);
			GridEX_Helper.SetColumnWidth(grdDetail, "ItemName", Constants.InventoryConstants.ItemName);
			GridEX_Helper.SetColumnWidth(grdDetail, "PackUom", Constants.InventoryConstants.UomConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "CropYear", 80);
			GridEX_Helper.SetColumnWidth(grdDetail, "PackingType", 90);
			GridEX_Helper.SetColumnWidth(grdDetail, "LoadingQty", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "GrossWeight", 80);
			GridEX_Helper.SetColumnWidth(grdDetail, "EbUnit", 50);
			GridEX_Helper.SetColumnWidth(grdDetail, "EbTotal", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "AddLessWeight", 50);
			GridEX_Helper.SetColumnWidth(grdDetail, "NetBillWeight", 80);
			GridEX_Helper.SetColumnWidth(grdDetail, "GrnGrossWeight", 80);
			GridEX_Helper.SetColumnWidth(grdDetail, "GrnEbUnit", 55);
			GridEX_Helper.SetColumnWidth(grdDetail, "GrnEbTotal", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "GrnAddLessWeight", 60);
			GridEX_Helper.SetColumnWidth(grdDetail, "GrnNetBillWeight", 80);
			GridEX_Helper.SetColumnWidth(grdDetail, "SaleRate", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "SaleRateUom", 60);
			GridEX_Helper.SetColumnWidth(grdDetail, "SaleAmount", 90);
			GridEX_Helper.SetColumnWidth(grdDetail, "TaxNameSale", 80);
			GridEX_Helper.SetColumnWidth(grdDetail, "TaxPercentSale", 55);
			GridEX_Helper.SetColumnWidth(grdDetail, "TotalAmountSale", 100);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchaseRate", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchaseRateUom", 65);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchaseAmount", 90);
			GridEX_Helper.SetColumnWidth(grdDetail, "TaxNamePurchase", 80);
			GridEX_Helper.SetColumnWidth(grdDetail, "TaxPercentPurchase", 60);
			GridEX_Helper.SetColumnWidth(grdDetail, "TotalAmountPurchase", 100);
			GridEX_Helper.SetColumnWidth(grdDetail, "WarningRemarks", 100);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		try
		{
			FromDate.Focus();
			FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			txtDocNoFrom.Clear();
			txtDocNoTo.Clear();
			((Control)(object)CmbItemName).Text = string.Empty;
			PendingDataDbCall();
			GrdDataBind();
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
			CombosFill(ComboDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadOnInvoice_Click_1(object sender, EventArgs e)
	{
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+L", "To Press Load Button");
			dt.Rows.Add("Ctrl+S", "For Search");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
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

	private void btnShortcutKeys_Click(object sender, EventArgs e)
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

	private void frmPendingGdnLoader_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				dtLoader = null;
				Hide();
			}
			else if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				btngrnlod_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.L)
			{
				btnLoadOnInvoice_Click_1(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.N)
			{
				btnReset_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDate.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDocNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtDocNoTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0 || !(e.Column.Key == "Load"))
			{
				return;
			}
			int grnId = Conversion.ToInt(r.Cells["Id"].Value);
			if (grnId <= 0)
			{
				MessageBox.Show("No valid row selected.");
				return;
			}
			EnumerableRowCollection<DataRow> rows = from rr in dtHistory.AsEnumerable()
				where rr.Field<int>("gdnBuyerDispatchMasterId") == grnId
				select rr;
			if (!rows.Any())
			{
				MessageBox.Show("No matching records found.");
				return;
			}
			dtLoader = rows.CopyToDataTable();
			Hide();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dtGrid);
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
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Expected O, but got Unknown
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Expected O, but got Unknown
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Expected O, but got Unknown
		//IL_098c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0996: Expected O, but got Unknown
		//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c4: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Cmagt.frmPendingGdnLoader));
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnLoadSelectedRows = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.label23 = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label35 = new System.Windows.Forms.Label();
		this.CmbDeliveryToParty = new UltraCombo();
		this.label60 = new System.Windows.Forms.Label();
		this.CmbShipToAddress = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbBuyerName = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbCommissionAgent = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbItemName = new UltraCombo();
		this.CmbSupplier = new UltraCombo();
		this.txtDocNoTo = new System.Windows.Forms.TextBox();
		this.txtDocNoFrom = new System.Windows.Forms.TextBox();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryToParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbShipToAddress).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(984, 50);
		this.panel1.TabIndex = 0;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripSeparator6, this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.BtnLoadSelectedRows, this.btnShortcutKeys, this.toolStripSeparator1 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(984, 25);
		this.toolStrip1.TabIndex = 28;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnReset_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.BtnLoadSelectedRows.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadSelectedRows.Image = Architecture.WinApp.Properties.Resources.download;
		this.BtnLoadSelectedRows.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadSelectedRows.Name = "BtnLoadSelectedRows";
		this.BtnLoadSelectedRows.Size = new System.Drawing.Size(58, 22);
		this.BtnLoadSelectedRows.Text = "Load";
		this.BtnLoadSelectedRows.Visible = false;
		this.BtnLoadSelectedRows.Click += new System.EventHandler(btnLoadOnInvoice_Click_1);
		this.btnShortcutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeys.Name = "btnShortcutKeys";
		this.btnShortcutKeys.Size = new System.Drawing.Size(122, 22);
		this.btnShortcutKeys.Text = "ShortCut Keys";
		this.btnShortcutKeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 27);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(207, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Pending Gdn Dispatched";
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 167);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(984, 164);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).Click += new System.EventHandler(grd_SelectionChanged);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.label35);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty);
		this.panel2.Controls.Add(this.label60);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbShipToAddress);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add(this.label9);
		this.panel2.Controls.Add(this.label8);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBuyerName);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionAgent);
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.panel2.Controls.Add(this.txtDocNoTo);
		this.panel2.Controls.Add(this.txtDocNoFrom);
		this.panel2.Controls.Add(this.ToDate);
		this.panel2.Controls.Add(this.FromDate);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.btngrnlod);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 50);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(984, 96);
		this.panel2.TabIndex = 0;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(530, 32);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(88, 30);
		this.label35.TabIndex = 202;
		this.label35.Text = "Delivery / Ship \r\nTo Address";
		this.CmbDeliveryToParty.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryToParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryToParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryToParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance30;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance36).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance23).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryToParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Location = new System.Drawing.Point(655, 5);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Name = "CmbDeliveryToParty";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Size = new System.Drawing.Size(279, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).TabIndex = 7;
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(530, 11);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(125, 15);
		this.label60.TabIndex = 201;
		this.label60.Text = "Deliver / Ship To Party";
		this.CmbShipToAddress.AllowNull = (DefaultableBoolean)1;
		this.CmbShipToAddress.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbShipToAddress.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbShipToAddress.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Appearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbShipToAddress).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.CmbShipToAddress).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance21).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbShipToAddress.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).Location = new System.Drawing.Point(655, 34);
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).Name = "CmbShipToAddress";
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).Size = new System.Drawing.Size(279, 26);
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).TabIndex = 8;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(216, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(84, 30);
		this.label1.TabIndex = 198;
		this.label1.Text = "Commission \r\nAgent / Broker";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(6, 61);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(63, 30);
		this.label9.TabIndex = 197;
		this.label9.Text = "Doc No \r\nFrom && To";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(216, 69);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(37, 15);
		this.label8.TabIndex = 196;
		this.label8.Text = "Buyer";
		this.CmbBuyerName.AllowNull = (DefaultableBoolean)1;
		this.CmbBuyerName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBuyerName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBuyerName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance45).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBuyerName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).Location = new System.Drawing.Point(306, 63);
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).Name = "CmbBuyerName";
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).TabIndex = 6;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(216, 40);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(51, 15);
		this.label5.TabIndex = 193;
		this.label5.Text = "Supplier";
		this.CmbCommissionAgent.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance71;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance64).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommissionAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Location = new System.Drawing.Point(306, 5);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Name = "CmbCommissionAgent";
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).TabIndex = 4;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(530, 69);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 192;
		this.label6.Text = "Item Name";
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance17;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(655, 63);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 9;
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance50;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance68).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(306, 34);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 5;
		this.txtDocNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoTo.Location = new System.Drawing.Point(143, 65);
		this.txtDocNoTo.Name = "txtDocNoTo";
		this.txtDocNoTo.Size = new System.Drawing.Size(70, 23);
		this.txtDocNoTo.TabIndex = 3;
		this.txtDocNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDocNoTo_KeyPress);
		this.txtDocNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFrom.Location = new System.Drawing.Point(72, 65);
		this.txtDocNoFrom.Name = "txtDocNoFrom";
		this.txtDocNoFrom.Size = new System.Drawing.Size(70, 23);
		this.txtDocNoFrom.TabIndex = 2;
		this.txtDocNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDocNoFrom_KeyPress);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(72, 36);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(141, 22);
		this.ToDate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(72, 7);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(141, 22);
		this.FromDate.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 40);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "To Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 11);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(63, 15);
		this.label3.TabIndex = 16;
		this.label3.Text = "From Date";
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btngrnlod.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(877, 63);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(57, 26);
		this.btngrnlod.TabIndex = 10;
		this.btngrnlod.Text = "Show";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 146);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(984, 21);
		this.panel3.TabIndex = 31;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 354);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(984, 257);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 2;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel4.Location = new System.Drawing.Point(0, 331);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(984, 23);
		this.panel4.TabIndex = 35;
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = null;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(943, 0);
		this.ctrlGrdBar6.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar6.MyGrid = this.grd;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(38, 20);
		this.ctrlGrdBar6.TabIndex = 131;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(984, 611);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel4);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmPendingGdnLoader";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmPendingGdnLoader_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmPendingGdnLoader_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryToParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbShipToAddress).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		base.ResumeLayout(false);
	}
}
