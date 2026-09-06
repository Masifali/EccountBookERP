using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model.CommissionAgent;
using Architecture.WinApp.Common;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.CommissionAgent.FromWise_Helper_Methods;

public class PurchaseOrderCmagt_Helper
{
	public static void InitializeDetailtable(DataTable dtDetail)
	{
		if (dtDetail != null && dtDetail.Columns.Count == 0)
		{
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("ParentItemId", typeof(int));
			dtDetail.Columns.Add("ParentItem", typeof(string));
			dtDetail.Columns.Add("ItemId", typeof(int));
			dtDetail.Columns.Add("ItemName", typeof(string));
			dtDetail.Columns.Add("CropYearId", typeof(int));
			dtDetail.Columns.Add("CropYear", typeof(string));
			dtDetail.Columns.Add("PackingTypeId", typeof(int));
			dtDetail.Columns.Add("PackingType", typeof(string));
			dtDetail.Columns.Add("PackUomId", typeof(int));
			dtDetail.Columns.Add("PackUom", typeof(string));
			dtDetail.Columns.Add("Qty", typeof(decimal));
			dtDetail.Columns.Add("Weight", typeof(decimal));
			dtDetail.Columns.Add("Rate", typeof(decimal));
			dtDetail.Columns.Add("RateUomId", typeof(int));
			dtDetail.Columns.Add("RateUom", typeof(string));
			dtDetail.Columns.Add("Amount", typeof(decimal));
			dtDetail.Columns.Add("TaxNameId", typeof(int));
			dtDetail.Columns.Add("TaxName", typeof(string));
			dtDetail.Columns.Add("Tax%", typeof(decimal));
			dtDetail.Columns.Add("TaxAmount", typeof(decimal));
			dtDetail.Columns.Add("TotalAmount", typeof(decimal));
			dtDetail.Columns.Add("Remarks", typeof(string));
		}
	}

	public static void FillDetailFromListCommonForReadById(purchaseOrderMaster obj, DataTable dt)
	{
		if (((obj != null) ? obj.purchaseOrderDetailList : null) == null || obj.purchaseOrderDetailList.Count == 0)
		{
			return;
		}
		foreach (purchaseOrderDetail detail in obj.purchaseOrderDetailList)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.purchaseOrderDetailId;
			dr["ParentItemId"] = detail.inventoryParentCategoryId;
			dr["ParentItem"] = detail.inventoryParentCategory;
			int itemId = detail.itemId;
			dr["ItemId"] = itemId;
			dr["ItemName"] = ((itemId > 0) ? detail.ItemName : string.Empty);
			dr["CropYearId"] = detail.cropYearId;
			dr["CropYear"] = detail.cropYear;
			dr["PackingTypeId"] = detail.packingTypeId;
			dr["PackingType"] = detail.PackingType;
			dr["PackUomId"] = detail.packUomId;
			dr["PackUom"] = detail.PackUomCode;
			dr["Qty"] = detail.itemQty;
			dr["Weight"] = detail.itemWeight;
			dr["Rate"] = detail.itemRate;
			dr["RateUomId"] = detail.rateUomId;
			dr["RateUom"] = detail.RateUomCode;
			decimal Amount = detail.ItemAmount;
			dr["Amount"] = Amount;
			int taxId = detail.TaxNameId;
			decimal taxPercent = detail.TaxPercent;
			decimal taxAmount = detail.TaxAmount;
			decimal totalAmount = Amount + taxAmount;
			dr["TaxNameId"] = taxId;
			dr["TaxName"] = ((taxId > 0) ? Conversion.ToString((object)detail.TaxName) : string.Empty);
			dr["Tax%"] = taxPercent;
			dr["TaxAmount"] = taxAmount;
			dr["TotalAmount"] = totalAmount;
			dr["Remarks"] = detail.remarks;
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
			List<string> hiddenColumns = new List<string> { "Id", "ParentItemId", "ItemId", "CropYearId", "PackingTypeId", "PackUomId", "RateUomId", "TaxNameId" };
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 3, 3, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["ParentItem"].Width = 100;
			gridEX.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			gridEX.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			gridEX.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			gridEX.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			gridEX.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			gridEX.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
			gridEX.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEX.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			gridEX.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["TaxName"].Width = Constants.InventoryConstants.StringConstant;
			gridEX.RootTable.Columns["Tax%"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEX.RootTable.Columns["TaxAmount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["TotalAmount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			gridEX.RootTable.Columns["TotalAmount"].Caption = "Tax + Amount";
			gridEX.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			gridEX.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			gridEX.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			gridEX.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			gridEX.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			gridEX.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			gridEX.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			gridEX.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			gridEX.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			gridEX.RootTable.Columns["Rate"].FormatString = "#,##0.###";
			gridEX.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			gridEX.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			gridEX.RootTable.Columns["Amount"].FormatString = "#,##0.###";
			gridEX.RootTable.Columns["Amount"].TotalFormatString = "#,##0.###";
			gridEX.RootTable.Columns["Tax%"].TextAlignment = (TextAlignment)3;
			gridEX.RootTable.Columns["Tax%"].FormatString = "#,##0.###";
			gridEX.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			gridEX.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			gridEX.RootTable.Columns["TaxAmount"].FormatString = "#,##0.###";
			gridEX.RootTable.Columns["TaxAmount"].TotalFormatString = "#,##0.###";
			gridEX.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			gridEX.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			gridEX.RootTable.Columns["TotalAmount"].FormatString = "#,##0.###";
			gridEX.RootTable.Columns["TotalAmount"].TotalFormatString = "#,##0.###";
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

	public static void InitializeExpenseDetailtable(DataTable dtExpGrid)
	{
		if (dtExpGrid.Columns.Count == 0)
		{
			dtExpGrid.Columns.Add("Id", typeof(int));
			dtExpGrid.Columns.Add("SoId", typeof(int));
			dtExpGrid.Columns.Add("SoExpenseId", typeof(int));
			dtExpGrid.Columns.Add("SaleOrderNo", typeof(int));
			dtExpGrid.Columns.Add("ItemId", typeof(int));
			dtExpGrid.Columns.Add("Qty", typeof(double));
			dtExpGrid.Columns.Add("Rate", typeof(double));
			dtExpGrid.Columns.Add("Amount", typeof(double));
			dtExpGrid.Columns.Add("Remarks");
		}
	}

	public static void InitializeEmptyBagDetailtable(DataTable dtEmptyBags)
	{
		if (dtEmptyBags.Columns.Count == 0)
		{
			dtEmptyBags.Columns.Add("SoId", typeof(int));
			dtEmptyBags.Columns.Add("SoEbId", typeof(int));
			dtEmptyBags.Columns.Add("SaleOrderNo", typeof(int));
			dtEmptyBags.Columns.Add("PackingType", typeof(int));
			dtEmptyBags.Columns.Add("WeightCut", typeof(decimal));
		}
	}

	public static void InitializeEmptyBagPMDetailtable(DataTable dtEmptyBagsPM)
	{
		if (dtEmptyBagsPM.Columns.Count == 0)
		{
			dtEmptyBagsPM.Columns.Add("PackingType", typeof(int));
			dtEmptyBagsPM.Columns.Add("PurchaseRate", typeof(decimal));
			dtEmptyBagsPM.Columns.Add("EmptyBagItem", typeof(int));
		}
	}

	public static void InitializePaymentTermDetailtable(DataTable dtPaymentTerm)
	{
		if (dtPaymentTerm.Columns.Count == 0)
		{
			dtPaymentTerm.Columns.Add("PaymentTerm", typeof(int));
			dtPaymentTerm.Columns.Add("DueDays", typeof(int));
			dtPaymentTerm.Columns.Add("%OfTotal", typeof(decimal));
			dtPaymentTerm.Columns.Add("Amount", typeof(decimal));
			dtPaymentTerm.Columns.Add("BaseDateType", typeof(int));
			dtPaymentTerm.Columns.Add("DueDate", typeof(DateTime));
		}
	}

	public static void InitializeInquiryMapDetailtable(DataTable dtSaleOrderMap)
	{
		if (dtSaleOrderMap.Columns.Count == 0)
		{
			dtSaleOrderMap.Columns.Add("Id", typeof(int));
			dtSaleOrderMap.Columns.Add("InquiryId", typeof(int));
			dtSaleOrderMap.Columns.Add("InquiryDetailId", typeof(int));
			dtSaleOrderMap.Columns.Add("InquiryNo", typeof(int));
			dtSaleOrderMap.Columns.Add("BuyerId", typeof(int));
			dtSaleOrderMap.Columns.Add("BuyerName", typeof(string));
			dtSaleOrderMap.Columns.Add("ItemId", typeof(int));
			dtSaleOrderMap.Columns.Add("ItemName", typeof(string));
			dtSaleOrderMap.Columns.Add("ItemWeight", typeof(decimal));
			dtSaleOrderMap.Columns.Add("BookedWeight", typeof(decimal));
			dtSaleOrderMap.Columns.Add("BalanceWeight", typeof(decimal));
			dtSaleOrderMap.Columns.Add("CurrentAllocationWeight", typeof(decimal));
		}
	}

	public static void InitializeSaleOrderMapDetailtable(DataTable dtSaleOrderMap)
	{
		if (dtSaleOrderMap.Columns.Count == 0)
		{
			dtSaleOrderMap.Columns.Add("Id", typeof(int));
			dtSaleOrderMap.Columns.Add("SoId", typeof(int));
			dtSaleOrderMap.Columns.Add("SoDetailId", typeof(int));
			dtSaleOrderMap.Columns.Add("SaleOrderNo", typeof(int));
			dtSaleOrderMap.Columns.Add("BuyerId", typeof(int));
			dtSaleOrderMap.Columns.Add("BuyerName", typeof(string));
			dtSaleOrderMap.Columns.Add("ItemId", typeof(int));
			dtSaleOrderMap.Columns.Add("ItemName", typeof(string));
			dtSaleOrderMap.Columns.Add("ItemWeight", typeof(decimal));
			dtSaleOrderMap.Columns.Add("BookedWeight", typeof(decimal));
			dtSaleOrderMap.Columns.Add("BalanceWeight", typeof(decimal));
			dtSaleOrderMap.Columns.Add("CurrentAllocationWeight", typeof(decimal));
			dtSaleOrderMap.Columns.Add("ValidityDate", typeof(DateTime));
		}
	}
}
