using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model.Inventory;
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

public class TradeBill_Helper
{
	public static void InitializeDetailtable(DataTable dtDetail)
	{
		if (dtDetail != null && dtDetail.Columns.Count == 0)
		{
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("GdnId", typeof(int));
			dtDetail.Columns.Add("GdnDetailId", typeof(int));
			dtDetail.Columns.Add("GdnNo", typeof(int));
			dtDetail.Columns.Add("GdnDate", typeof(DateTime));
			dtDetail.Columns.Add("GrnId", typeof(int));
			dtDetail.Columns.Add("GrnDetailId", typeof(int));
			dtDetail.Columns.Add("GrnNo", typeof(int));
			dtDetail.Columns.Add("GrnDate", typeof(DateTime));
			dtDetail.Columns.Add("PoId", typeof(int));
			dtDetail.Columns.Add("PoDetailId", typeof(int));
			dtDetail.Columns.Add("PoNo", typeof(int));
			dtDetail.Columns.Add("PoDate", typeof(DateTime));
			dtDetail.Columns.Add("SoId", typeof(int));
			dtDetail.Columns.Add("SoDetailId", typeof(int));
			dtDetail.Columns.Add("SoNo", typeof(int));
			dtDetail.Columns.Add("SoDate", typeof(DateTime));
			dtDetail.Columns.Add("ItemId", typeof(int));
			dtDetail.Columns.Add("ItemCode");
			dtDetail.Columns.Add("ItemName");
			dtDetail.Columns.Add("CropYearId", typeof(int));
			dtDetail.Columns.Add("CropYear");
			dtDetail.Columns.Add("PackTypeId", typeof(int));
			dtDetail.Columns.Add("PackTypeDesc");
			dtDetail.Columns.Add("UomId", typeof(int));
			dtDetail.Columns.Add("UomCode");
			dtDetail.Columns.Add("PackEquivalent", typeof(double));
			dtDetail.Columns.Add("ItemQty", typeof(double));
			dtDetail.Columns.Add("RefDocNo");
			dtDetail.Columns.Add("VehicleNo");
			dtDetail.Columns.Add("BiltyNo");
			dtDetail.Columns.Add("PurchaseGrossWeight", typeof(double));
			dtDetail.Columns.Add("PurchaseEbCut", typeof(double));
			dtDetail.Columns.Add("PurchaseEbCutTotal", typeof(double));
			dtDetail.Columns.Add("PurchaseAddLessWeight", typeof(double));
			dtDetail.Columns.Add("PurchaseBillWeight", typeof(double));
			dtDetail.Columns.Add("PurchaseRateWithoutAddLess", typeof(double));
			dtDetail.Columns.Add("PurchaseRateAddLess", typeof(double));
			dtDetail.Columns.Add("PurchaseNetRate", typeof(double));
			dtDetail.Columns.Add("PurchaseRateUomId", typeof(int));
			dtDetail.Columns.Add("PurchaseRateUom");
			dtDetail.Columns.Add("PurchaseRateEquivalent", typeof(double));
			dtDetail.Columns.Add("PurchaseAmount", typeof(double));
			dtDetail.Columns.Add("PurchaseTaxNameId", typeof(int));
			dtDetail.Columns.Add("PurchaseTaxName", typeof(string));
			dtDetail.Columns.Add("PurchaseTax%", typeof(decimal));
			dtDetail.Columns.Add("PurchaseTaxAmount", typeof(decimal));
			dtDetail.Columns.Add("PurchaseFreight", typeof(double));
			dtDetail.Columns.Add("PurchaseFreightLess", typeof(double));
			dtDetail.Columns.Add("PurchaseExpenses", typeof(double));
			dtDetail.Columns.Add("PurchaseCommission", typeof(double));
			dtDetail.Columns.Add("PurchaseBrokery", typeof(double));
			dtDetail.Columns.Add("PurchaseNetAmount", typeof(double));
			dtDetail.Columns.Add("SaleGrossWeight", typeof(double));
			dtDetail.Columns.Add("SaleEbCut", typeof(double));
			dtDetail.Columns.Add("SaleEbCutTotal", typeof(double));
			dtDetail.Columns.Add("SaleAddLess", typeof(double));
			dtDetail.Columns.Add("SaleBillWeight", typeof(double));
			dtDetail.Columns.Add("SaleRateWithoutAddLess", typeof(double));
			dtDetail.Columns.Add("SaleRateAddLess", typeof(double));
			dtDetail.Columns.Add("SaleNetRate", typeof(double));
			dtDetail.Columns.Add("SaleRateUomId", typeof(int));
			dtDetail.Columns.Add("SaleRateUom");
			dtDetail.Columns.Add("SaleRateEquivalent", typeof(double));
			dtDetail.Columns.Add("SaleAmount", typeof(double));
			dtDetail.Columns.Add("SaleTaxNameId", typeof(int));
			dtDetail.Columns.Add("SaleTaxName", typeof(string));
			dtDetail.Columns.Add("SaleTax%", typeof(decimal));
			dtDetail.Columns.Add("SaleTaxAmount", typeof(decimal));
			dtDetail.Columns.Add("SaleExpenses", typeof(double));
			dtDetail.Columns.Add("SaleCommission", typeof(double));
			dtDetail.Columns.Add("SaleBrokery", typeof(double));
			dtDetail.Columns.Add("SaleNetAmount", typeof(double));
			dtDetail.Columns.Add("PLAmount", typeof(double));
			dtDetail.Columns.Add("RemarksDetail");
		}
	}

	public static void InitializePurchaseExpenseDetailtable(DataTable dtGrid)
	{
		if (dtGrid.Columns.Count == 0)
		{
			dtGrid.Columns.Add("GrnId", typeof(int));
			dtGrid.Columns.Add("GrnNo", typeof(int));
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("ItemName");
			dtGrid.Columns.Add("%OfTotal", typeof(double));
			dtGrid.Columns.Add("Qty", typeof(double));
			dtGrid.Columns.Add("Rate", typeof(double));
			dtGrid.Columns.Add("Amount", typeof(double));
			dtGrid.Columns.Add("Remarks", typeof(string));
		}
	}

	public static void InitializeSaleExpenseDetailtable(DataTable dtGrid)
	{
		if (dtGrid.Columns.Count == 0)
		{
			dtGrid.Columns.Add("GdnId", typeof(int));
			dtGrid.Columns.Add("GdnNo", typeof(int));
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("ItemName");
			dtGrid.Columns.Add("%OfTotal", typeof(double));
			dtGrid.Columns.Add("Qty", typeof(double));
			dtGrid.Columns.Add("Rate", typeof(double));
			dtGrid.Columns.Add("Amount", typeof(double));
			dtGrid.Columns.Add("Remarks", typeof(string));
		}
	}

	public static void InitializeExpensePackingMaterialTable(DataTable dtGrid)
	{
		if (dtGrid.Columns.Count == 0)
		{
			dtGrid.Columns.Add("GdnId", typeof(int));
			dtGrid.Columns.Add("GdnNo", typeof(int));
			dtGrid.Columns.Add("AccountId", typeof(int));
			dtGrid.Columns.Add("PackingTypeId", typeof(int));
			dtGrid.Columns.Add("PmItemId", typeof(int));
			dtGrid.Columns.Add("Qty", typeof(double));
			dtGrid.Columns.Add("Rate", typeof(double));
			dtGrid.Columns.Add("Amount", typeof(double));
			dtGrid.Columns.Add("Debit", typeof(double));
			dtGrid.Columns.Add("Remarks", typeof(string));
			dtGrid.Columns.Add("EmptyBagTermId", typeof(int));
			dtGrid.Columns.Add("EmptyBagTerm", typeof(string));
		}
	}

	public static void InitializeFreightDetailTable(DataTable dtGrid)
	{
		if (dtGrid.Columns.Count == 0)
		{
			dtGrid.Columns.Add("GrnId", typeof(int));
			dtGrid.Columns.Add("GrnNo", typeof(int));
			dtGrid.Columns.Add("AccountId", typeof(int));
			dtGrid.Columns.Add("PackingTypeId", typeof(int));
			dtGrid.Columns.Add("PmItemId", typeof(int));
			dtGrid.Columns.Add("Qty", typeof(double));
			dtGrid.Columns.Add("Rate", typeof(double));
			dtGrid.Columns.Add("Amount", typeof(double));
			dtGrid.Columns.Add("Debit", typeof(double));
			dtGrid.Columns.Add("Remarks");
			dtGrid.Columns.Add("EmptyBagTermId", typeof(int));
			dtGrid.Columns.Add("EmptyBagTerm", typeof(string));
		}
	}

	public static void InitializePurchasePaymentDetailTable(DataTable dtGrid)
	{
		if (dtGrid.Columns.Count == 0)
		{
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("PoId", typeof(int));
			dtGrid.Columns.Add("PoNo", typeof(int));
			dtGrid.Columns.Add("PaymentTermId", typeof(int));
			dtGrid.Columns.Add("DueDays", typeof(int));
			dtGrid.Columns.Add("%OfTotal", typeof(decimal));
			dtGrid.Columns.Add("Amount", typeof(decimal));
			dtGrid.Columns.Add("BaseDateTypeId", typeof(int));
			dtGrid.Columns.Add("DueDate", typeof(DateTime));
		}
	}

	public static void InitializeSalePaymentDetailTable(DataTable dtGrid)
	{
		if (dtGrid.Columns.Count == 0)
		{
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("SoId", typeof(int));
			dtGrid.Columns.Add("SoNo", typeof(int));
			dtGrid.Columns.Add("PaymentTermId", typeof(int));
			dtGrid.Columns.Add("DueDays", typeof(int));
			dtGrid.Columns.Add("%OfTotal", typeof(decimal));
			dtGrid.Columns.Add("Amount", typeof(decimal));
			dtGrid.Columns.Add("BaseDateTypeId", typeof(int));
			dtGrid.Columns.Add("DueDate", typeof(DateTime));
		}
	}

	public static void InitializePurchaseCommissionDetailTable(DataTable dtGrid)
	{
		if (dtGrid.Columns.Count == 0)
		{
			dtGrid.Columns.Add("PoId", typeof(int));
			dtGrid.Columns.Add("PoNo", typeof(int));
			dtGrid.Columns.Add("CommissionAgentId", typeof(int));
			dtGrid.Columns.Add("CommissionTypeId", typeof(int));
			dtGrid.Columns.Add("CommissionUomId", typeof(int));
			dtGrid.Columns.Add("CommissionRate", typeof(decimal));
			dtGrid.Columns.Add("CommissionAmount", typeof(decimal));
			dtGrid.Columns.Add("DebitAccountId", typeof(int));
		}
	}

	public static void InitializeSaleCommissionDetailTable(DataTable dtGrid)
	{
		if (dtGrid.Columns.Count == 0)
		{
			dtGrid.Columns.Add("SoId", typeof(int));
			dtGrid.Columns.Add("SoNo", typeof(int));
			dtGrid.Columns.Add("CommissionAgentId", typeof(int));
			dtGrid.Columns.Add("CommissionTypeId", typeof(int));
			dtGrid.Columns.Add("CommissionUomId", typeof(int));
			dtGrid.Columns.Add("CommissionRate", typeof(decimal));
			dtGrid.Columns.Add("CommissionAmount", typeof(decimal));
			dtGrid.Columns.Add("DebitAccountId", typeof(int));
		}
	}

	public static void InitializePurchaseBrokeryDetailTable(DataTable dtGrid)
	{
		if (dtGrid.Columns.Count == 0)
		{
			dtGrid.Columns.Add("PoId", typeof(int));
			dtGrid.Columns.Add("PoNo", typeof(int));
			dtGrid.Columns.Add("BrokeryAgentId", typeof(int));
			dtGrid.Columns.Add("BrokeryTypeId", typeof(int));
			dtGrid.Columns.Add("BrokeryUomId", typeof(int));
			dtGrid.Columns.Add("BrokeryRate", typeof(decimal));
			dtGrid.Columns.Add("BrokeryAmount", typeof(decimal));
			dtGrid.Columns.Add("DebitAccountId", typeof(int));
		}
	}

	public static void InitializeSaleBrokeryDetailTable(DataTable dtGrid)
	{
		if (dtGrid.Columns.Count == 0)
		{
			dtGrid.Columns.Add("SoId", typeof(int));
			dtGrid.Columns.Add("SoNo", typeof(int));
			dtGrid.Columns.Add("BrokeryAgentId", typeof(int));
			dtGrid.Columns.Add("BrokeryTypeId", typeof(int));
			dtGrid.Columns.Add("BrokeryUomId", typeof(int));
			dtGrid.Columns.Add("BrokeryRate", typeof(decimal));
			dtGrid.Columns.Add("BrokeryAmount", typeof(decimal));
			dtGrid.Columns.Add("DebitAccountId", typeof(int));
		}
	}

	public static void DetailGridCommonSetting(GridEX gridEX, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "GdnId", "GdnDetailId", "GrnId", "GrnDetailId", "PoId", "PoDetailId", "SoId", "SoDetailId", "ItemId",
				"CropYearId", "PackTypeId", "UomId", "PackEquivalent", "PurchaseRateUomId", "PurchaseRateEquivalent", "SaleRateUomId", "SaleRateEquivalent", "PurchaseTaxNameId", "SaleTaxNameId",
				"PLAmount"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 3, 3, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["ItemName"].Width = 250;
			gridEX.RootTable.Columns["PurchaseFreight"].Caption = "Purchase Freight Add";
			gridEX.RootTable.Columns["PackTypeDesc"].Caption = "Packing Type";
			gridEX.RootTable.Columns["SoDate"].FormatString = "dd-MMM-yy";
			gridEX.RootTable.Columns["PoDate"].FormatString = "dd-MMM-yy";
			gridEX.RootTable.Columns["GdnDate"].FormatString = "dd-MMM-yy";
			gridEX.RootTable.Columns["GrnDate"].FormatString = "dd-MMM-yy";
			gridEX.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			gridEX.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			gridEX.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			gridEX.RootTable.Columns["PackTypeDesc"].Width = Constants.InventoryConstants.PackingType;
			gridEX.RootTable.Columns["UomCode"].Width = Constants.InventoryConstants.UomConstant;
			gridEX.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			gridEX.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			gridEX.RootTable.Columns["RefDocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			gridEX.RootTable.Columns["RemarksDetail"].Width = Constants.InventoryConstants.RemarksConstants;
			gridEX.RootTable.Columns["PurchaseGrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			gridEX.RootTable.Columns["PurchaseEbCut"].Width = Constants.InventoryConstants.WeightConstant;
			gridEX.RootTable.Columns["PurchaseEbCutTotal"].Width = Constants.InventoryConstants.WeightConstant;
			gridEX.RootTable.Columns["PurchaseBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			gridEX.RootTable.Columns["PurchaseRateWithoutAddLess"].Width = Constants.InventoryConstants.ExRateConstant + 15;
			gridEX.RootTable.Columns["PurchaseRateAddLess"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEX.RootTable.Columns["PurchaseNetRate"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEX.RootTable.Columns["PurchaseRateUom"].Width = Constants.InventoryConstants.UomConstant + 10;
			gridEX.RootTable.Columns["PurchaseAmount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["PurchaseFreight"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["PurchaseFreightLess"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["PurchaseExpenses"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["PurchaseNetAmount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["SaleGrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			gridEX.RootTable.Columns["SaleEbCut"].Width = Constants.InventoryConstants.EbWt;
			gridEX.RootTable.Columns["SaleEbCutTotal"].Width = Constants.InventoryConstants.WtCut;
			gridEX.RootTable.Columns["SaleAddLess"].Width = Constants.InventoryConstants.WeightConstant;
			gridEX.RootTable.Columns["SaleBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			gridEX.RootTable.Columns["SaleRateWithoutAddLess"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEX.RootTable.Columns["SaleRateAddLess"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEX.RootTable.Columns["SaleNetRate"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEX.RootTable.Columns["SaleAmount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["SaleExpenses"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["SaleNetAmount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["PLAmount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["PurchaseExpenses"].AggregateFunction = (AggregateFunction)2;
			gridEX.RootTable.Columns["PurchaseExpenses"].TextAlignment = (TextAlignment)3;
			gridEX.RootTable.Columns["PurchaseExpenses"].FormatString = clsGlobalVariables.stringFormatsingle;
			gridEX.RootTable.Columns["PurchaseExpenses"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			gridEX.RootTable.Columns["PurchaseFreight"].AggregateFunction = (AggregateFunction)2;
			gridEX.RootTable.Columns["PurchaseFreight"].TextAlignment = (TextAlignment)3;
			gridEX.RootTable.Columns["PurchaseFreight"].FormatString = clsGlobalVariables.stringFormatsingle;
			gridEX.RootTable.Columns["PurchaseFreight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			gridEX.RootTable.Columns["PurchaseFreightLess"].AggregateFunction = (AggregateFunction)2;
			gridEX.RootTable.Columns["PurchaseFreightLess"].TextAlignment = (TextAlignment)3;
			gridEX.RootTable.Columns["PurchaseFreightLess"].FormatString = clsGlobalVariables.stringFormatsingle;
			gridEX.RootTable.Columns["PurchaseFreightLess"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			gridEX.RootTable.Columns["SaleExpenses"].AggregateFunction = (AggregateFunction)2;
			gridEX.RootTable.Columns["SaleExpenses"].TextAlignment = (TextAlignment)3;
			gridEX.RootTable.Columns["SaleExpenses"].FormatString = clsGlobalVariables.stringFormatsingle;
			gridEX.RootTable.Columns["SaleExpenses"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseExpenseGridCommonSetting(GridEX gridEx, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "GrnId", "ItemId" };
			GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns, editableColumns);
			gridEx.RootTable.Columns["GrnNo"].Width = Constants.InventoryConstants.DocNoConstant;
			gridEx.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName + 20;
			gridEx.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			gridEx.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEx.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEx.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 250;
			gridEx.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			gridEx.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			gridEx.RootTable.Columns["Amount"].FormatString = "#,##0.###";
			gridEx.RootTable.Columns["Amount"].TotalFormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleExpenseGridCommonSetting(GridEX gridEx, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "GdnId", "ItemId" };
			GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns, editableColumns);
			gridEx.RootTable.Columns["GdnNo"].Width = Constants.InventoryConstants.DocNoConstant;
			gridEx.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName + 20;
			gridEx.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			gridEx.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEx.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEx.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 250;
			gridEx.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			gridEx.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			gridEx.RootTable.Columns["Amount"].FormatString = "#,##0.###";
			gridEx.RootTable.Columns["Amount"].TotalFormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void FreightGridCommonSetting(GridEX gridEx, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "GrnId", "EmptyBagTermId", "Debit" };
			GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns, editableColumns);
			gridEx.RootTable.Columns["Amount"].Caption = "Credit";
			gridEx.RootTable.Columns["AccountId"].Width = Constants.InventoryConstants.AccountTitle;
			gridEx.RootTable.Columns["PackingTypeId"].Width = Constants.InventoryConstants.PackingType;
			gridEx.RootTable.Columns["PmItemId"].Width = Constants.InventoryConstants.ItemName;
			gridEx.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			gridEx.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEx.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEx.RootTable.Columns["Debit"].Width = Constants.InventoryConstants.AmountConstant;
			gridEx.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 250;
			gridEx.RootTable.Columns["EmptyBagTerm"].Width = Constants.InventoryConstants.TermConstant;
			gridEx.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			gridEx.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			gridEx.RootTable.Columns["Amount"].FormatString = "#,##0.###";
			gridEx.RootTable.Columns["Amount"].TotalFormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PackingMaterialGridCommonSetting(GridEX gridEx, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "GdnId", "EmptyBagTermId", "Debit" };
			gridEx.RootTable.Columns["Amount"].Caption = "Credit";
			GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns, editableColumns);
			gridEx.RootTable.Columns["AccountId"].Width = Constants.InventoryConstants.AccountTitle;
			gridEx.RootTable.Columns["PackingTypeId"].Width = Constants.InventoryConstants.PackingType;
			gridEx.RootTable.Columns["PmItemId"].Width = Constants.InventoryConstants.ItemName;
			gridEx.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			gridEx.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEx.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEx.RootTable.Columns["Debit"].Width = Constants.InventoryConstants.AmountConstant;
			gridEx.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 250;
			gridEx.RootTable.Columns["EmptyBagTerm"].Width = Constants.InventoryConstants.TermConstant;
			gridEx.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			gridEx.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			gridEx.RootTable.Columns["Amount"].FormatString = "#,##0.###";
			gridEx.RootTable.Columns["Amount"].TotalFormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchasePaymentGridCommonSetting(GridEX gridEx, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "PoId" };
			if (((Control)(object)gridEx).Name == "grdPaymentDetailPurchase")
			{
				GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns, editableColumns);
			}
			else
			{
				GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns);
			}
			gridEx.RootTable.Columns["PoNo"].Width = Constants.InventoryConstants.DocNoConstant;
			gridEx.RootTable.Columns["PaymentTermId"].Width = Constants.InventoryConstants.TermConstant;
			gridEx.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			gridEx.RootTable.Columns["%OfTotal"].Width = Constants.InventoryConstants.PercentOfTotal;
			gridEx.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEx.RootTable.Columns["BaseDateTypeId"].Width = Constants.InventoryConstants.DateConstant;
			gridEx.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			gridEx.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			gridEx.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			gridEx.RootTable.Columns["Amount"].FormatString = "#,##0.###";
			gridEx.RootTable.Columns["Amount"].TotalFormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SalePaymentGridCommonSetting(GridEX gridEx, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "SoId" };
			GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns, editableColumns);
			gridEx.RootTable.Columns["SoNo"].Width = Constants.InventoryConstants.DocNoConstant;
			gridEx.RootTable.Columns["PaymentTermId"].Width = Constants.InventoryConstants.TermConstant;
			gridEx.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			gridEx.RootTable.Columns["%OfTotal"].Width = Constants.InventoryConstants.PercentOfTotal;
			gridEx.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEx.RootTable.Columns["BaseDateTypeId"].Width = Constants.InventoryConstants.DateConstant;
			gridEx.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			gridEx.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			gridEx.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			gridEx.RootTable.Columns["Amount"].FormatString = "#,##0.###";
			gridEx.RootTable.Columns["Amount"].TotalFormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleCommissionGridCommonSetting(GridEX gridEx, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "SoId" };
			if (((Control)(object)gridEx).Name == "")
			{
				GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns, editableColumns);
			}
			else
			{
				GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns);
			}
			gridEx.RootTable.Columns["SoNo"].Width = Constants.InventoryConstants.DocNoConstant;
			gridEx.RootTable.Columns["CommissionAgentId"].Width = Constants.InventoryConstants.SupplierName + 50;
			gridEx.RootTable.Columns["CommissionTypeId"].Width = Constants.InventoryConstants.DiscountType;
			gridEx.RootTable.Columns["CommissionUomId"].Width = Constants.InventoryConstants.UomConstant;
			gridEx.RootTable.Columns["CommissionRate"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEx.RootTable.Columns["CommissionAmount"].Width = Constants.InventoryConstants.AmountConstant + 20;
			gridEx.RootTable.Columns["DebitAccountId"].Width = Constants.InventoryConstants.AccountTitle;
			gridEx.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
			gridEx.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
			gridEx.RootTable.Columns["CommissionAmount"].FormatString = "#,##0.###";
			gridEx.RootTable.Columns["CommissionAmount"].TotalFormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseCommissionGridCommonSetting(GridEX gridEx, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "PoId" };
			if (((Control)(object)gridEx).Name == "")
			{
				GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns, editableColumns);
			}
			else
			{
				GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns);
			}
			gridEx.RootTable.Columns["PoNo"].Width = Constants.InventoryConstants.DocNoConstant;
			gridEx.RootTable.Columns["CommissionAgentId"].Width = Constants.InventoryConstants.SupplierName - 30;
			gridEx.RootTable.Columns["CommissionTypeId"].Width = Constants.InventoryConstants.UomConstant - 15;
			gridEx.RootTable.Columns["CommissionUomId"].Width = Constants.InventoryConstants.UomConstant - 15;
			gridEx.RootTable.Columns["CommissionAmount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEx.RootTable.Columns["DebitAccountId"].Width = Constants.InventoryConstants.AccountTitle - 30;
			gridEx.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
			gridEx.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
			gridEx.RootTable.Columns["CommissionAmount"].FormatString = "#,##0.###";
			gridEx.RootTable.Columns["CommissionAmount"].TotalFormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleBrokeryGridCommonSetting(GridEX gridEx, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "SoId" };
			if (((Control)(object)gridEx).Name == "")
			{
				GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns, editableColumns);
			}
			else
			{
				GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns);
			}
			gridEx.RootTable.Columns["SoNo"].Width = Constants.InventoryConstants.DocNoConstant;
			gridEx.RootTable.Columns["BrokeryAgentId"].Width = Constants.InventoryConstants.SupplierName + 50;
			gridEx.RootTable.Columns["BrokeryTypeId"].Width = Constants.InventoryConstants.DiscountType;
			gridEx.RootTable.Columns["BrokeryUomId"].Width = Constants.InventoryConstants.UomConstant;
			gridEx.RootTable.Columns["BrokeryRate"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEx.RootTable.Columns["BrokeryAmount"].Width = Constants.InventoryConstants.AmountConstant + 20;
			gridEx.RootTable.Columns["DebitAccountId"].Width = Constants.InventoryConstants.AccountTitle;
			gridEx.RootTable.Columns["BrokeryAmount"].AggregateFunction = (AggregateFunction)2;
			gridEx.RootTable.Columns["BrokeryAmount"].TextAlignment = (TextAlignment)3;
			gridEx.RootTable.Columns["BrokeryAmount"].FormatString = "#,##0.###";
			gridEx.RootTable.Columns["BrokeryAmount"].TotalFormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseBrokeryGridCommonSetting(GridEX gridEx, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "PoId" };
			if (((Control)(object)gridEx).Name == "")
			{
				GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns, editableColumns);
			}
			else
			{
				GridEX_Helper.GridWrappingAndColumnSettings(gridEx, 2, 3, hiddenColumns);
			}
			gridEx.RootTable.Columns["PoNo"].Width = Constants.InventoryConstants.DocNoConstant;
			gridEx.RootTable.Columns["BrokeryAgentId"].Width = Constants.InventoryConstants.SupplierName + 50;
			gridEx.RootTable.Columns["BrokeryTypeId"].Width = Constants.InventoryConstants.DiscountType;
			gridEx.RootTable.Columns["BrokeryUomId"].Width = Constants.InventoryConstants.UomConstant;
			gridEx.RootTable.Columns["BrokeryRate"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEx.RootTable.Columns["BrokeryAmount"].Width = Constants.InventoryConstants.AmountConstant + 20;
			gridEx.RootTable.Columns["DebitAccountId"].Width = Constants.InventoryConstants.AccountTitle;
			gridEx.RootTable.Columns["BrokeryAmount"].AggregateFunction = (AggregateFunction)2;
			gridEx.RootTable.Columns["BrokeryAmount"].TextAlignment = (TextAlignment)3;
			gridEx.RootTable.Columns["BrokeryAmount"].FormatString = "#,##0.###";
			gridEx.RootTable.Columns["BrokeryAmount"].TotalFormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void FillDetailtableFromListCommonForReadById(InvCommAgentTradeBill header, DataTable dt, bool AddExtraColumns = false)
	{
		if (((header != null) ? header.InvCommAgentTradeBillDetailslist : null) == null || header.InvCommAgentTradeBillDetailslist.Count == 0)
		{
			return;
		}
		foreach (InvCommAgentTradeBillDetail detail in header.InvCommAgentTradeBillDetailslist)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.Id;
			dr["GrnId"] = detail.grnSupplierLoadingMasterId;
			dr["GrnDetailId"] = detail.grnSupplierLoadingDetailId;
			dr["GrnNo"] = detail.GrnNo;
			dr["GrnDate"] = detail.GrnDate;
			dr["GdnId"] = detail.gdnBuyerDispatchMasterId;
			dr["GdnDetailId"] = detail.gdnBuyerDispatchDetailId;
			dr["GdnNo"] = detail.GdnNo;
			dr["GdnDate"] = detail.GdnDate;
			dr["PoId"] = detail.purchaseOrderMasterId;
			dr["PoDetailId"] = detail.purchaseOrderDetailId;
			dr["PoNo"] = detail.PurchaseOrderNo;
			dr["PoDate"] = detail.PurchaseOrderDate;
			dr["SoId"] = detail.saleOrderMasterId;
			dr["SoDetailId"] = detail.saleOrderDetailId;
			dr["SoNo"] = detail.SaleOrderNo;
			dr["SoDate"] = detail.SaleOrderDate;
			dr["ItemId"] = detail.ItemId;
			dr["ItemCode"] = detail.ItemCode;
			dr["ItemName"] = detail.ItemName;
			dr["CropYearId"] = detail.Crop;
			dr["CropYear"] = detail.CropYear;
			dr["PackTypeId"] = detail.PackingTypeId;
			dr["PackTypeDesc"] = detail.PackTypeDesc;
			dr["UomId"] = detail.PackUomId;
			dr["UomCode"] = detail.PackUom;
			dr["PackEquivalent"] = detail.PackUomEquivalent;
			dr["ItemQty"] = detail.Qty;
			dr["RefDocNo"] = detail.RefDocNo;
			dr["VehicleNo"] = detail.VehicleNo;
			dr["BiltyNo"] = detail.BiltyNo;
			dr["PurchaseGrossWeight"] = detail.GrossWeight;
			dr["PurchaseEbCut"] = detail.EbCut;
			dr["PurchaseEbCutTotal"] = detail.EbCutTotal;
			dr["PurchaseAddLessWeight"] = detail.AddLessPurchase;
			dr["PurchaseBillWeight"] = detail.BillWeight;
			dr["PurchaseRateWithoutAddLess"] = detail.PurchaseRateWithoutAddLess;
			dr["PurchaseRateAddLess"] = detail.PurchaseRateAddLess;
			dr["PurchaseNetRate"] = detail.RatePurchase;
			dr["PurchaseRateUomId"] = detail.RateUomId;
			dr["PurchaseRateUom"] = detail.RateUom;
			dr["PurchaseRateEquivalent"] = detail.RateUomEquivalent;
			dr["PurchaseAmount"] = detail.ItemAmount;
			dr["PurchaseTaxNameId"] = detail.TaxNameIdPurchase;
			dr["PurchaseTaxName"] = detail.TaxNamePurchase;
			dr["PurchaseTax%"] = detail.TaxPercentPurchase;
			dr["PurchaseTaxAmount"] = detail.TaxAmountPurchase;
			dr["PurchaseFreight"] = detail.PurchaseFreight;
			dr["PurchaseFreightLess"] = detail.PurchaseFreightLess;
			dr["PurchaseExpenses"] = detail.PurchaseExpenses;
			dr["PurchaseCommission"] = detail.PurchaseCommission;
			dr["PurchaseBrokery"] = detail.PurchaseBrokery;
			dr["PurchaseNetAmount"] = detail.PurchaseNetAmount;
			dr["SaleGrossWeight"] = detail.SaleGrossWeight;
			dr["SaleEbCut"] = detail.SaleEbCut;
			dr["SaleEbCutTotal"] = detail.SaleEbCutTotal;
			dr["SaleAddLess"] = detail.AddLessSale;
			dr["SaleBillWeight"] = detail.SaleBillWeight;
			dr["SaleRateWithoutAddLess"] = detail.SaleRateWithoutAddLess;
			dr["SaleRateAddLess"] = detail.SaleRateAddLess;
			dr["SaleNetRate"] = detail.RateSale;
			dr["SaleRateUomId"] = detail.SaleRateUomId;
			dr["SaleRateUom"] = detail.SaleRateUom;
			dr["SaleRateEquivalent"] = detail.SaleRateUomEquivalent;
			dr["SaleAmount"] = detail.SaleAmount;
			dr["SaleTaxNameId"] = detail.TaxNameIdSale;
			dr["SaleTaxName"] = detail.TaxNameSale;
			dr["SaleTax%"] = detail.TaxPercentSale;
			dr["SaleTaxAmount"] = detail.TaxAmountSale;
			dr["SaleExpenses"] = detail.SaleExpenses;
			dr["SaleCommission"] = detail.SaleCommission;
			dr["SaleBrokery"] = detail.SaleBrokery;
			dr["SaleNetAmount"] = detail.SaleNetAmount;
			dr["PLAmount"] = 0;
			dr["RemarksDetail"] = detail.RemarksDetail;
			dt.Rows.Add(dr);
		}
	}

	public static void FillPurchaseExpenseTableFromListCommonForReadById(InvCommAgentTradeBill header, DataTable dt, bool AddExtraColumns = false)
	{
		if (((header != null) ? header.InvCommAgentTradePurchaseExpList : null) == null || header.InvCommAgentTradePurchaseExpList.Count == 0)
		{
			return;
		}
		for (int i = 0; i < header.InvCommAgentTradePurchaseExpList.Count; i++)
		{
			CommisionAgentBillPurchaseExpense detail = header.InvCommAgentTradePurchaseExpList[i];
			DataRow[] rowToUpdate = dt.Select($"ItemId = '{detail.InvExpItemId}'");
			if (rowToUpdate != null && rowToUpdate.Length != 0)
			{
				rowToUpdate[0].SetField("GrnId", detail.grnSupplierLoadingMasterId);
				rowToUpdate[0].SetField("GrnNo", detail.GrnNo);
				rowToUpdate[0].SetField("ItemId", detail.InvExpItemId);
				rowToUpdate[0].SetField("ItemName", detail.OtherItemName);
				rowToUpdate[0].SetField("Qty", detail.Qty);
				rowToUpdate[0].SetField("Rate", detail.Rate);
				rowToUpdate[0].SetField("Amount", detail.Amount);
				rowToUpdate[0].SetField("Remarks", detail.Remarks);
			}
		}
	}

	public static void FillSaleExpenseTableFromListCommonForReadById(InvCommAgentTradeBill header, DataTable dt, bool AddExtraColumns = false)
	{
		if (((header != null) ? header.InvCommAgentTradeSaleExpList : null) == null || header.InvCommAgentTradeSaleExpList.Count == 0)
		{
			return;
		}
		for (int i = 0; i < header.InvCommAgentTradeSaleExpList.Count; i++)
		{
			CommisionAgentBillSaleExpense detail = header.InvCommAgentTradeSaleExpList[i];
			DataRow[] rowToUpdate = dt.Select($"ItemId = '{detail.InvExpItemId}'");
			if (rowToUpdate != null && rowToUpdate.Length != 0)
			{
				rowToUpdate[0].SetField("GdnId", detail.gdnBuyerDispatchMasterId);
				rowToUpdate[0].SetField("GdnNo", detail.GdnNo);
				rowToUpdate[0].SetField("ItemId", detail.InvExpItemId);
				rowToUpdate[0].SetField("ItemName", detail.OtherItemName);
				rowToUpdate[0].SetField("Qty", detail.Qty);
				rowToUpdate[0].SetField("Rate", detail.Rate);
				rowToUpdate[0].SetField("Amount", detail.Amount);
				rowToUpdate[0].SetField("Remarks", detail.Remarks);
			}
		}
	}

	public static void FillPurchaseFreightTableFromListCommonForReadById(InvCommAgentTradeBill header, DataTable dt, bool AddExtraColumns = false)
	{
		if (((header != null) ? header.InvCommAgentTradeFreightExpList : null) == null || header.InvCommAgentTradeFreightExpList.Count == 0)
		{
			return;
		}
		foreach (CommisionAgentBillPurchaseFreightExpense detail in header.InvCommAgentTradeFreightExpList)
		{
			DataRow dr = dt.NewRow();
			dr["GrnId"] = detail.grnSupplierLoadingMasterId;
			dr["GrnNo"] = detail.GrnNo;
			dr["AccountId"] = detail.AccountId;
			dr["PackingTypeId"] = detail.PackingTypeId;
			dr["PmItemId"] = detail.PmItemId;
			dr["Qty"] = detail.Qty;
			dr["Rate"] = detail.Rate;
			dr["Amount"] = detail.Amount;
			dr["Debit"] = 0;
			dr["Remarks"] = detail.Remarks;
			dr["EmptyBagTermId"] = detail.EBWeightDeductionTermId;
			dr["EmptyBagTerm"] = detail.EBWeightDeductionTerm;
			if (AddExtraColumns)
			{
				dr["AccountTitle"] = detail.AccountTitle;
				dr["PackingType"] = detail.PackingType;
				dr["PmItemName"] = detail.PmItemName;
			}
			dt.Rows.Add(dr);
		}
	}

	public static void FillSalePmTableFromListCommonForReadById(InvCommAgentTradeBill header, DataTable dt, bool AddExtraColumns = false)
	{
		if (((header != null) ? header.CommisionAgentBillSaleExpenseCreditToReleventAcsList : null) == null || header.CommisionAgentBillSaleExpenseCreditToReleventAcsList.Count == 0)
		{
			return;
		}
		foreach (CommisionAgentBillSaleExpenseCreditToReleventAc detail in header.CommisionAgentBillSaleExpenseCreditToReleventAcsList)
		{
			DataRow dr = dt.NewRow();
			dr["GdnId"] = detail.gdnBuyerDispatchMasterId;
			dr["GdnNo"] = detail.GdnNo;
			dr["AccountId"] = detail.AccountId;
			dr["PackingTypeId"] = detail.PackingTypeId;
			dr["PmItemId"] = detail.PmItemId;
			dr["Qty"] = detail.Qty;
			dr["Rate"] = detail.Rate;
			dr["Amount"] = detail.Amount;
			dr["Debit"] = 0;
			dr["Remarks"] = detail.Remarks;
			dr["EmptyBagTermId"] = detail.EBWeightDeductionTermId;
			dr["EmptyBagTerm"] = detail.EBWeightDeductionTerm;
			if (AddExtraColumns)
			{
				dr["AccountTitle"] = detail.AccountTitle;
				dr["PackingType"] = detail.PackingType;
				dr["PmItemName"] = detail.PmItemName;
			}
			dt.Rows.Add(dr);
		}
	}

	private static void FillCommissionTableCore(IEnumerable<CommisionAgentBillCommissionDetail> details, DataTable dt, bool AddExtraColumns, Action<DataRow, CommisionAgentBillCommissionDetail> fillRow, Action<DataRow, CommisionAgentBillCommissionDetail> fillExtraColumns = null)
	{
		foreach (CommisionAgentBillCommissionDetail detail in details)
		{
			DataRow dr = dt.NewRow();
			fillRow(dr, detail);
			if (AddExtraColumns)
			{
				fillExtraColumns?.Invoke(dr, detail);
			}
			dt.Rows.Add(dr);
		}
	}

	private static bool HasCommissionList(InvCommAgentTradeBill header)
	{
		if (((header != null) ? header.CommisionAgentBillCommissionDetailList : null) != null)
		{
			return header.CommisionAgentBillCommissionDetailList.Count > 0;
		}
		return false;
	}

	public static void FillPurchaseCommissionTableFromListCommonForReadById(InvCommAgentTradeBill header, DataTable dt, bool AddExtraColumns = false)
	{
		if (HasCommissionList(header))
		{
			FillCommissionTableCore(header.CommisionAgentBillCommissionDetailList.Where((CommisionAgentBillCommissionDetail r) => r.EntrySideId == 1 && r.agentTypeId == 1), dt, AddExtraColumns, delegate(DataRow dr, CommisionAgentBillCommissionDetail detail)
			{
				dr["PoId"] = detail.purchaseOrderMasterId;
				dr["PoNo"] = detail.PurchaseOrderNo;
				dr["CommissionAgentId"] = detail.commissionAgentId;
				dr["CommissionTypeId"] = detail.commissionTypeId;
				dr["CommissionUomId"] = detail.rateUomId;
				dr["CommissionRate"] = detail.commissionRate;
				dr["CommissionAmount"] = detail.commissionAmount;
				dr["DebitAccountId"] = detail.debitAccountId;
			}, delegate(DataRow dr, CommisionAgentBillCommissionDetail detail)
			{
				dr["CommissionAgent"] = detail.CommissionAgentName;
				dr["CommissionType"] = detail.CommissionType;
				dr["Uom"] = detail.RateUom;
				dr["DebitAccount"] = detail.AccountTitle;
			});
		}
	}

	public static void FillPurchaseBrokeryTableFromListCommonForReadById(InvCommAgentTradeBill header, DataTable dt, bool AddExtraColumns = false)
	{
		if (HasCommissionList(header))
		{
			FillCommissionTableCore(header.CommisionAgentBillCommissionDetailList.Where((CommisionAgentBillCommissionDetail r) => r.EntrySideId == 1 && r.agentTypeId == 2), dt, AddExtraColumns, delegate(DataRow dr, CommisionAgentBillCommissionDetail detail)
			{
				dr["PoId"] = detail.purchaseOrderMasterId;
				dr["PoNo"] = detail.PurchaseOrderNo;
				dr["BrokeryAgentId"] = detail.commissionAgentId;
				dr["BrokeryTypeId"] = detail.commissionTypeId;
				dr["BrokeryUomId"] = detail.rateUomId;
				dr["BrokeryRate"] = detail.commissionRate;
				dr["BrokeryAmount"] = detail.commissionAmount;
				dr["DebitAccountId"] = detail.debitAccountId;
			}, delegate(DataRow dr, CommisionAgentBillCommissionDetail detail)
			{
				dr["BrokeryAgent"] = detail.CommissionAgentName;
				dr["BrokeryType"] = detail.CommissionType;
				dr["Uom"] = detail.RateUom;
				dr["DebitAccount"] = detail.AccountTitle;
			});
		}
	}

	public static void FillSaleCommissionTableFromListCommonForReadById(InvCommAgentTradeBill header, DataTable dt, bool AddExtraColumns = false)
	{
		if (HasCommissionList(header))
		{
			FillCommissionTableCore(header.CommisionAgentBillCommissionDetailList.Where((CommisionAgentBillCommissionDetail r) => r.EntrySideId == 2 && r.agentTypeId == 1), dt, AddExtraColumns, delegate(DataRow dr, CommisionAgentBillCommissionDetail detail)
			{
				dr["SoId"] = detail.saleOrderMasterId;
				dr["SoNo"] = detail.SaleOrderNo;
				dr["CommissionAgentId"] = detail.commissionAgentId;
				dr["CommissionTypeId"] = detail.commissionTypeId;
				dr["CommissionUomId"] = detail.rateUomId;
				dr["CommissionRate"] = detail.commissionRate;
				dr["CommissionAmount"] = detail.commissionAmount;
				dr["DebitAccountId"] = detail.debitAccountId;
			}, delegate(DataRow dr, CommisionAgentBillCommissionDetail detail)
			{
				dr["CommissionAgent"] = detail.CommissionAgentName;
				dr["CommissionType"] = detail.CommissionType;
				dr["Uom"] = detail.RateUom;
				dr["DebitAccount"] = detail.AccountTitle;
			});
		}
	}

	public static void FillSaleBrokeryTableFromListCommonForReadById(InvCommAgentTradeBill header, DataTable dt, bool AddExtraColumns = false)
	{
		if (HasCommissionList(header))
		{
			FillCommissionTableCore(header.CommisionAgentBillCommissionDetailList.Where((CommisionAgentBillCommissionDetail r) => r.EntrySideId == 2 && r.agentTypeId == 2), dt, AddExtraColumns, delegate(DataRow dr, CommisionAgentBillCommissionDetail detail)
			{
				dr["SoId"] = detail.saleOrderMasterId;
				dr["SoNo"] = detail.SaleOrderNo;
				dr["BrokeryAgentId"] = detail.commissionAgentId;
				dr["BrokeryTypeId"] = detail.commissionTypeId;
				dr["BrokeryUomId"] = detail.rateUomId;
				dr["BrokeryRate"] = detail.commissionRate;
				dr["BrokeryAmount"] = detail.commissionAmount;
				dr["DebitAccountId"] = detail.debitAccountId;
			}, delegate(DataRow dr, CommisionAgentBillCommissionDetail detail)
			{
				dr["BrokeryAgent"] = detail.CommissionAgentName;
				dr["BrokeryType"] = detail.CommissionType;
				dr["Uom"] = detail.RateUom;
				dr["DebitAccount"] = detail.AccountTitle;
			});
		}
	}

	public static void FillPurchasePaymentTableFromListCommonForReadById(InvCommAgentTradeBill header, DataTable dt, bool AddExtraColumns = false)
	{
		if (((header != null) ? header.CommisionAgentBillPaymentDetailList : null) == null || header.CommisionAgentBillPaymentDetailList.Count == 0)
		{
			return;
		}
		foreach (CommisionAgentBillPaymentDetail detail in header.CommisionAgentBillPaymentDetailList.Where((CommisionAgentBillPaymentDetail r) => r.EntrySideId == 1))
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.Id;
			dr["PoId"] = detail.purchaseOrderMasterId;
			dr["PoNo"] = detail.PurchaseOrderNo;
			dr["PaymentTermId"] = detail.PaymentTermId;
			dr["DueDays"] = detail.DueDays;
			dr["%OfTotal"] = detail.pctOfTotal;
			dr["Amount"] = detail.dueAmount;
			dr["BaseDateTypeId"] = detail.BaseDueDateTypeId;
			dr["DueDate"] = Conversion.ToDateTime((object)detail.DueDate);
			if (AddExtraColumns)
			{
				dr["PaymentTerm"] = detail.PaymentTerm;
				dr["BaseDateType"] = detail.BaseDateType;
			}
			dt.Rows.Add(dr);
		}
	}

	public static void FillSalePaymentTableFromListCommonForReadById(InvCommAgentTradeBill header, DataTable dt, bool AddExtraColumns = false)
	{
		if (((header != null) ? header.CommisionAgentBillPaymentDetailList : null) == null || header.CommisionAgentBillPaymentDetailList.Count == 0)
		{
			return;
		}
		foreach (CommisionAgentBillPaymentDetail detail in header.CommisionAgentBillPaymentDetailList.Where((CommisionAgentBillPaymentDetail r) => r.EntrySideId == 2))
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.Id;
			dr["SoId"] = detail.saleOrderMasterId;
			dr["SoNo"] = detail.SaleOrderNo;
			dr["PaymentTermId"] = detail.PaymentTermId;
			dr["DueDays"] = detail.DueDays;
			dr["%OfTotal"] = detail.pctOfTotal;
			dr["Amount"] = detail.dueAmount;
			dr["BaseDateTypeId"] = detail.BaseDueDateTypeId;
			dr["DueDate"] = Conversion.ToDateTime((object)detail.DueDate);
			if (AddExtraColumns)
			{
				dr["PaymentTerm"] = detail.PaymentTerm;
				dr["BaseDateType"] = detail.BaseDateType;
			}
			dt.Rows.Add(dr);
		}
	}
}

}
