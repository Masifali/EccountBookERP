using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Architecture.Model.CommissionAgent;
using Architecture.WinApp.Common;
using Janus.Windows.GridEX;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.CommissionAgent.FromWise_Helper_Methods
{

public class GdnBuyerDispatch_Helper
{
	public static void InitializeDetailtable(DataTable dtDetail)
	{
		if (dtDetail != null && dtDetail.Columns.Count == 0)
		{
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("GrnId", typeof(int));
			dtDetail.Columns.Add("GrnDetailId", typeof(int));
			dtDetail.Columns.Add("GrnNo", typeof(int));
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
			dtDetail.Columns.Add("PurchaseOrderId", typeof(int));
			dtDetail.Columns.Add("PurchaseOrderDetailId", typeof(int));
			dtDetail.Columns.Add("PurchaseOrderNo", typeof(int));
			dtDetail.Columns.Add("POExpiryDate", typeof(DateTime));
			dtDetail.Columns.Add("SaleOrderId", typeof(int));
			dtDetail.Columns.Add("SaleOrderDetailId", typeof(int));
			dtDetail.Columns.Add("SaleOrderNo", typeof(int));
			dtDetail.Columns.Add("SOExpiryDate", typeof(DateTime));
			dtDetail.Columns.Add("SupplierId", typeof(int));
			dtDetail.Columns.Add("SupplierName", typeof(string));
			dtDetail.Columns.Add("Remarks", typeof(string));
			dtDetail.Columns.Add("WarningRemarks", typeof(string));
		}
	}

	public static void FillDetailtableFromListCommonForReadById(gdnBuyerDispatchMaster header, DataTable dt, bool AddExtraColumns = false)
	{
		if (((header != null) ? header.GdnBuyerDispatchDetailList : null) == null || header.GdnBuyerDispatchDetailList.Count == 0)
		{
			return;
		}
		foreach (gdnBuyerDispatchDetail detail in header.GdnBuyerDispatchDetailList)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.gdnBuyerDispatchDetailId;
			dr["GrnId"] = detail.grnSupplierLoadingMasterId;
			dr["GrnDetailId"] = detail.grnSupplierLoadingDetailId;
			dr["GrnNo"] = detail.GrnNo;
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
			dr["PurchaseOrderId"] = detail.purchaseOrderMasterId;
			dr["PurchaseOrderDetailId"] = detail.purchaseOrderDetailId;
			dr["PurchaseOrderNo"] = detail.PurchaseOrderNo;
			dr["POExpiryDate"] = detail.POExpiryDate;
			dr["SaleOrderId"] = detail.saleOrderMasterId;
			dr["SaleOrderDetailId"] = detail.saleOrderDetailId;
			dr["SaleOrderNo"] = detail.SaleOrderNo;
			dr["SOExpiryDate"] = detail.SOExpiryDate;
			dr["SupplierId"] = detail.supplierId;
			dr["SupplierName"] = detail.SupplierName;
			dr["Remarks"] = detail.remarks;
			dr["WarningRemarks"] = detail.warningRemarks;
			dt.Rows.Add(dr);
		}
	}

	public static void FillExpenseTableFromListCommonForReadById(gdnBuyerDispatchMaster header, DataTable dt, bool AddExtraColumns = false)
	{
		if (((header != null) ? header.gdnBuyerDispatchExpenseDetailList : null) == null || header.gdnBuyerDispatchExpenseDetailList.Count == 0)
		{
			return;
		}
		foreach (gdnBuyerDispatchExpenseDetail detail in header.gdnBuyerDispatchExpenseDetailList)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.gdnBuyerDispatchExpenseDetailId;
			dr["SoId"] = detail.saleOrderMasterId;
			dr["SaleOrderNo"] = detail.SaleOrderNo;
			dr["SoExpenseId"] = detail.saleOrderExpenseDetailId;
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

	public static void FillEmptyBagTableFromListCommonForReadById(gdnBuyerDispatchMaster header, DataTable dt, bool AddExtraColumns = false)
	{
		if (((header != null) ? header.gdnBuyerDispatchEmptyBagDetailList : null) == null || header.gdnBuyerDispatchEmptyBagDetailList.Count == 0)
		{
			return;
		}
		foreach (gdnBuyerDispatchEmptyBagDetail detail in header.gdnBuyerDispatchEmptyBagDetailList)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.gdnBuyerDispatchEmptyBagDetailId;
			dr["SoId"] = detail.saleOrderMasterId;
			dr["SaleOrderNo"] = detail.SaleOrderNo;
			dr["SoEbId"] = detail.saleOrderEmptyBagDetailId;
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
				"Id", "GrnId", "GrnDetailId", "PurchaseOrderId", "PurchaseOrderDetailId", "SupplierId", "ParentItemId", "ItemId", "CropYearId", "PackingTypeId",
				"PackUomId", "PackUomEquivalent", "EmptyBagTermId", "SaleOrderId", "SaleOrderDetailId"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 3, 3, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["POExpiryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			gridEX.RootTable.Columns["SOExpiryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			gridEX.RootTable.Columns["GrnNo"].Width = Constants.InventoryConstants.DocNoConstant;
			gridEX.RootTable.Columns["PurchaseOrderNo"].Width = Constants.InventoryConstants.DocNoConstant;
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
			gridEX.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName;
			gridEX.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			gridEX.RootTable.Columns["WarningRemarks"].Width = Constants.InventoryConstants.RemarksConstants;
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
			List<string> hiddenColumns = new List<string> { "Id", "SoId", "SoExpenseId" };
			if (((Control)(object)gridEx).Name == "grdInvExp")
			{
				GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns, editableColumns);
			}
			else
			{
				GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns);
			}
			gridEx.RootTable.Columns["SaleOrderNo"].Width = Constants.InventoryConstants.DocNoConstant + 20;
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
			List<string> hiddenColumns = new List<string> { "Id", "SoId", "SoEbId" };
			if (((Control)(object)gridEx).Name == "grdEmptyBags")
			{
				GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns, editableColumns);
			}
			else
			{
				GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns);
			}
			gridEx.RootTable.Columns["SaleOrderNo"].Width = Constants.InventoryConstants.DocNoConstant + 20;
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
			dtGrid.Columns.Add("SoId", typeof(int));
			dtGrid.Columns.Add("SoExpenseId", typeof(int));
			dtGrid.Columns.Add("SaleOrderNo", typeof(int));
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
			dtGrid.Columns.Add("SoId", typeof(int));
			dtGrid.Columns.Add("SoEbId", typeof(int));
			dtGrid.Columns.Add("SaleOrderNo", typeof(int));
			dtGrid.Columns.Add("PackingType", typeof(int));
			dtGrid.Columns.Add("PurchaseRate", typeof(decimal));
			dtGrid.Columns.Add("EmptyBagItem", typeof(int));
		}
	}
}

}
