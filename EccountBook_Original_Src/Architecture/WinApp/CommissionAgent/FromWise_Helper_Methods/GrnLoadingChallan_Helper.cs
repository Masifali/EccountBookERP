using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Architecture.Model.CommissionAgent;
using Architecture.WinApp.Common;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.CommissionAgent.FromWise_Helper_Methods;

public class GrnLoadingChallan_Helper
{
	public static void InitializeDetailtable(DataTable dtDetail)
	{
		if (dtDetail != null && dtDetail.Columns.Count == 0)
		{
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("PurchaseOrderId", typeof(int));
			dtDetail.Columns.Add("PurchaseOrderDetailId", typeof(int));
			dtDetail.Columns.Add("PurchaseOrderNo", typeof(int));
			dtDetail.Columns.Add("PODeliveryStartDate", typeof(DateTime));
			dtDetail.Columns.Add("POExpiryDate", typeof(DateTime));
			dtDetail.Columns.Add("ParentItemId", typeof(int));
			dtDetail.Columns.Add("ParentItem", typeof(string));
			dtDetail.Columns.Add("ItemId", typeof(int));
			dtDetail.Columns.Add("ItemCode", typeof(string));
			dtDetail.Columns.Add("ItemName", typeof(string));
			dtDetail.Columns.Add("CropYearId", typeof(int));
			dtDetail.Columns.Add("CropYear", typeof(string));
			dtDetail.Columns.Add("PackingTypeId", typeof(int));
			dtDetail.Columns.Add("PackingType", typeof(string));
			dtDetail.Columns.Add("PackUomId", typeof(int));
			dtDetail.Columns.Add("PackUom", typeof(string));
			dtDetail.Columns.Add("PackUomEquivalent", typeof(double));
			dtDetail.Columns.Add("Qty", typeof(decimal));
			dtDetail.Columns.Add("GrossWeight", typeof(decimal));
			dtDetail.Columns.Add("EbUnit", typeof(decimal));
			dtDetail.Columns.Add("EbTotal", typeof(decimal));
			dtDetail.Columns.Add("AddLessWeight", typeof(decimal));
			dtDetail.Columns.Add("NetBillWeight", typeof(decimal));
			dtDetail.Columns.Add("EmptyBagTermId", typeof(int));
			dtDetail.Columns.Add("EmptyBagTerm", typeof(string));
			dtDetail.Columns.Add("MappingId", typeof(int));
			dtDetail.Columns.Add("SaleOrderId", typeof(int));
			dtDetail.Columns.Add("SaleOrderDetailId", typeof(int));
			dtDetail.Columns.Add("SaleOrderNo", typeof(int));
			dtDetail.Columns.Add("BuyerId", typeof(int));
			dtDetail.Columns.Add("BuyerName", typeof(string));
			dtDetail.Columns.Add("DeliveryToPartyId", typeof(int));
			dtDetail.Columns.Add("DeliveryToParty", typeof(string));
			dtDetail.Columns.Add("ShipToAddressId", typeof(int));
			dtDetail.Columns.Add("ShipToAddress", typeof(string));
			dtDetail.Columns.Add("Remarks", typeof(string));
			dtDetail.Columns.Add("WarningRemarks", typeof(string));
		}
	}

	public static void FillDetailtableFromListCommonForReadById(grnSupplierLoadingMaster header, DataTable dt, bool AddExtraColumns = false)
	{
		if (((header != null) ? header.grnSupplierLoadingDetailList : null) == null || header.grnSupplierLoadingDetailList.Count == 0)
		{
			return;
		}
		foreach (grnSupplierLoadingDetail detail in header.grnSupplierLoadingDetailList)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.grnSupplierLoadingDetailId;
			dr["PurchaseOrderId"] = detail.purchaseOrderMasterId;
			dr["PurchaseOrderDetailId"] = detail.purchaseOrderDetailId;
			dr["PurchaseOrderNo"] = detail.PurchaseOrderNo;
			dr["PODeliveryStartDate"] = detail.PODeliveryStartDate;
			dr["POExpiryDate"] = detail.POExpiryDate;
			dr["ParentItemId"] = detail.inventoryParentCategoryId;
			dr["ParentItem"] = detail.InvParentCateDescription;
			dr["ItemId"] = detail.itemId;
			dr["ItemCode"] = detail.ItemCode;
			dr["ItemName"] = detail.ItemName;
			dr["CropYearId"] = detail.cropYearId;
			dr["CropYear"] = detail.cropYear;
			dr["PackingTypeId"] = detail.packingTypeId;
			dr["PackingType"] = detail.PackingType;
			dr["PackUomId"] = detail.packUomId;
			dr["PackUom"] = detail.PackSize;
			dr["PackUomEquivalent"] = detail.PEquivalent;
			dr["Qty"] = detail.loadingQty;
			dr["GrossWeight"] = detail.wbGrossWeight;
			dr["EbUnit"] = detail.ebwPerUnit;
			dr["EbTotal"] = detail.ebwTotal;
			dr["AddLessWeight"] = detail.addLessWeight;
			dr["NetBillWeight"] = detail.netBillWeight;
			dr["EmptyBagTermId"] = detail.EBWeightDeductionTermId;
			dr["EmptyBagTerm"] = detail.EBWeightDeductionTerm;
			dr["MappingId"] = detail.purchaseOrderSaleOrderMappingId;
			dr["SaleOrderId"] = detail.saleOrderMasterId;
			dr["SaleOrderDetailId"] = detail.saleOrderDetailId;
			dr["SaleOrderNo"] = detail.SaleOrderNo;
			dr["BuyerId"] = detail.buyerId;
			dr["BuyerName"] = detail.BuyerName;
			dr["DeliveryToPartyId"] = detail.DeliverToPartyId;
			dr["DeliveryToParty"] = detail.DeliverToPartyName;
			dr["ShipToAddressId"] = detail.DeliverToAddressId;
			dr["ShipToAddress"] = detail.DeliverToAddress;
			dr["Remarks"] = detail.remarks;
			dr["WarningRemarks"] = detail.warningRemarks;
			dt.Rows.Add(dr);
		}
	}

	public static void FillExpenseTableFromListCommonForReadById(grnSupplierLoadingMaster header, DataTable dt, bool AddExtraColumns = false)
	{
		if (((header != null) ? header.grnSupplierLoadingExpenseDetailList : null) == null || header.grnSupplierLoadingExpenseDetailList.Count == 0)
		{
			return;
		}
		foreach (grnSupplierLoadingExpenseDetail detail in header.grnSupplierLoadingExpenseDetailList)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.grnSupplierLoadingExpenseDetailId;
			dr["PoId"] = detail.purchaseOrderMasterId;
			dr["PurchaseOrderNo"] = detail.PurchaseOrderNo;
			dr["PoExpenseId"] = detail.purchaseOrderExpenseDetailId;
			dr["ItemId"] = detail.ItemId;
			dr["Qty"] = detail.Qty;
			dr["Rate"] = detail.rate;
			dr["Amount"] = detail.amount;
			dr["Remarks"] = detail.remarks;
			if (AddExtraColumns)
			{
				dr["OtherItemName"] = detail.OtherItemName;
			}
			dt.Rows.Add(dr);
		}
	}

	public static void FillEmptyBagTableFromListCommonForReadById(grnSupplierLoadingMaster header, DataTable dt, bool AddExtraColumns = false)
	{
		if (((header != null) ? header.grnSupplierLoadingEmptyBagDetailList : null) == null || header.grnSupplierLoadingEmptyBagDetailList.Count == 0)
		{
			return;
		}
		foreach (grnSupplierLoadingEmptyBagDetail detail in header.grnSupplierLoadingEmptyBagDetailList)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.grnSupplierLoadingEmptyBagDetailId;
			dr["PoId"] = detail.purchaseOrderMasterId;
			dr["PurchaseOrderNo"] = detail.PurchaseOrderNo;
			dr["PoEbId"] = detail.purchaseOrderEmptyBagDetailId;
			dr["PackingType"] = detail.PackingTypeId;
			dr["PurchaseRate"] = detail.Rate;
			dr["EmptyBagItem"] = detail.emptyBagPackingMaterialItemId;
			if (AddExtraColumns)
			{
				dr["PackType"] = detail.PackingType;
				dr["EmptyBagItemName"] = detail.EmptyBagItem;
			}
			dt.Rows.Add(dr);
		}
	}

	public static void DetailGridCommonSetting(GridEX gridEX, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "PurchaseOrderId", "PurchaseOrderDetailId", "BuyerId", "ParentItemId", "ItemId", "CropYearId", "PackingTypeId", "PackUomId", "PackUomEquivalent",
				"EmptyBagTermId", "MappingId", "SaleOrderId", "SaleOrderDetailId", "ShipToAddressId", "DeliveryToPartyId"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 3, 3, hiddenColumns);
			gridEX.RootTable.Columns["POExpiryDate"].FormatString = "dd-MMM-yy";
			gridEX.RootTable.Columns["PODeliveryStartDate"].FormatString = "dd-MMM-yy";
			gridEX.RootTable.Columns["PurchaseOrderNo"].Width = Constants.InventoryConstants.DocNoConstant + 5;
			gridEX.RootTable.Columns["SaleOrderNo"].Width = Constants.InventoryConstants.DocNoConstant;
			gridEX.RootTable.Columns["ParentItem"].Width = Constants.InventoryConstants.ItemName;
			gridEX.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			gridEX.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			gridEX.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			gridEX.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			gridEX.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			gridEX.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			gridEX.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			gridEX.RootTable.Columns["EbUnit"].Width = Constants.InventoryConstants.WeightConstant;
			gridEX.RootTable.Columns["EbTotal"].Width = Constants.InventoryConstants.WeightConstant;
			gridEX.RootTable.Columns["AddLessWeight"].Width = Constants.InventoryConstants.WeightConstant;
			gridEX.RootTable.Columns["NetBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			gridEX.RootTable.Columns["EmptyBagTerm"].Width = Constants.InventoryConstants.TermConstant + 50;
			gridEX.RootTable.Columns["NetBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			gridEX.RootTable.Columns["BuyerName"].Width = Constants.InventoryConstants.SupplierName;
			gridEX.RootTable.Columns["ShipToAddress"].Width = Constants.InventoryConstants.Address;
			gridEX.RootTable.Columns["DeliveryToParty"].Width = Constants.InventoryConstants.SupplierName;
			gridEX.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ExpenseGridCommonSetting(GridEX gridEx, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "PoId", "PoExpenseId" };
			if (((Control)(object)gridEx).Name == "grdInvExp")
			{
				GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns, editableColumns);
			}
			else
			{
				GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns);
			}
			gridEx.RootTable.Columns["PurchaseOrderNo"].Width = Constants.InventoryConstants.DocNoConstant + 20;
			gridEx.RootTable.Columns["ItemId"].Width = Constants.InventoryConstants.ItemName + 20;
			gridEx.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			gridEx.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEx.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEx.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 250;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void EmptyBagGridCommonSetting(GridEX gridEx, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "PoId", "PoEbId" };
			if (((Control)(object)gridEx).Name == "grdEmptyBags")
			{
				GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns, editableColumns);
			}
			else
			{
				GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns);
			}
			gridEx.RootTable.Columns["PurchaseOrderNo"].Width = Constants.InventoryConstants.DocNoConstant + 20;
			gridEx.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			gridEx.RootTable.Columns["PurchaseRate"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEx.RootTable.Columns["EmptyBagItem"].Width = Constants.InventoryConstants.ItemName;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void InitializeExpenseDetailtable(DataTable dtGrid)
	{
		if (dtGrid.Columns.Count == 0)
		{
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("PoId", typeof(int));
			dtGrid.Columns.Add("PoExpenseId", typeof(int));
			dtGrid.Columns.Add("PurchaseOrderNo", typeof(int));
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("Qty", typeof(double));
			dtGrid.Columns.Add("Rate", typeof(double));
			dtGrid.Columns.Add("Amount", typeof(double));
			dtGrid.Columns.Add("Remarks", typeof(string));
		}
	}

	public static void InitializeEmptyBagDetailtable(DataTable dtGrid)
	{
		if (dtGrid.Columns.Count == 0)
		{
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("PoId", typeof(int));
			dtGrid.Columns.Add("PoEbId", typeof(int));
			dtGrid.Columns.Add("PurchaseOrderNo", typeof(int));
			dtGrid.Columns.Add("PackingType", typeof(int));
			dtGrid.Columns.Add("PurchaseRate", typeof(decimal));
			dtGrid.Columns.Add("EmptyBagItem", typeof(int));
		}
	}
}
