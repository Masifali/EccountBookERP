using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Architecture.Model.lgstcm;
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
namespace Architecture.WinApp.LogisticManagement.FromWise_Helper_Methods
{

public class FreightVoucherOutward_Helper
{
	public static void InitializeExpenseDetailtable(DataTable dtGrid)
	{
		if (dtGrid.Columns.Count == 0)
		{
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("Qty", typeof(double));
			dtGrid.Columns.Add("Rate", typeof(double));
			dtGrid.Columns.Add("AddAmount", typeof(double));
			dtGrid.Columns.Add("LessAmount", typeof(double));
			dtGrid.Columns.Add("Remarks");
		}
	}

	public static void InitializePaymentDetailtable(DataTable dtGrid)
	{
		if (dtGrid.Columns.Count == 0)
		{
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("TransactionTypeId", typeof(int));
			dtGrid.Columns.Add("TransactionType");
			dtGrid.Columns.Add("InstrumentTypeId", typeof(int));
			dtGrid.Columns.Add("InstrumentType");
			dtGrid.Columns.Add("AccountTitleId", typeof(int));
			dtGrid.Columns.Add("AccountTitle");
			dtGrid.Columns.Add("ChequeId", typeof(int));
			dtGrid.Columns.Add("ChequeNo", typeof(string));
			dtGrid.Columns.Add("ChequeDate", typeof(DateTime));
			dtGrid.Columns.Add("Amount", typeof(double));
			dtGrid.Columns.Add("PayeeTitle", typeof(string));
			dtGrid.Columns.Add("Remarks", typeof(string));
		}
	}

	public static void FillPaymentDetailFromList(FreightVoucherOutward invGrn, DataTable dt, DataTable dtDebitRow = null)
	{
		if (((invGrn != null) ? invGrn.FreightVoucherOutwardPaymentDetailList : null) == null || invGrn.FreightVoucherOutwardPaymentDetailList.Count == 0)
		{
			return;
		}
		foreach (FreightVoucherOutwardPaymentDetail detail in invGrn.FreightVoucherOutwardPaymentDetailList)
		{
			if (detail.CreditAmount > 0m)
			{
				DataRow dr = dt.NewRow();
				FillCommonPaymentColumns(dr, detail);
				dr["Amount"] = detail.CreditAmount;
				dt.Rows.Add(dr);
			}
			if (dtDebitRow != null && dtDebitRow.Columns.Count > 0 && dtDebitRow.Rows.Count < 1 && detail.DebitAmount > 0m)
			{
				DataRow drDebit = dtDebitRow.NewRow();
				FillCommonPaymentColumns(drDebit, detail);
				drDebit["Amount"] = detail.DebitAmount;
				dtDebitRow.Rows.Add(drDebit);
			}
		}
	}

	private static void FillCommonPaymentColumns(DataRow dr, FreightVoucherOutwardPaymentDetail detail)
	{
		dr["Id"] = detail.FreightVoucherOutwardPaymentDetailId;
		dr["TransactionTypeId"] = detail.TransTypeId;
		dr["TransactionType"] = detail.TransactionType;
		dr["InstrumentTypeId"] = detail.InstrumentTypeId;
		dr["InstrumentType"] = detail.InstrumentType;
		dr["AccountTitleId"] = detail.AccountId;
		dr["AccountTitle"] = detail.AccountTitle;
		dr["ChequeId"] = detail.CheqId;
		dr["ChequeNo"] = detail.CheqNo;
		dr["ChequeDate"] = detail.CheqDate;
		dr["PayeeTitle"] = detail.PayeeTitle;
		dr["Remarks"] = detail.Remarks;
	}

	public static void InitializeMasterDataDetailtable(DataTable dtGrid)
	{
		if (dtGrid.Columns.Count == 0)
		{
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("DocNo", typeof(int));
			dtGrid.Columns.Add("DocDate", typeof(DateTime));
			dtGrid.Columns.Add("GPId", typeof(int));
			dtGrid.Columns.Add("GpNo", typeof(int));
			dtGrid.Columns.Add("GpDate", typeof(DateTime));
			dtGrid.Columns.Add("DoId", typeof(int));
			dtGrid.Columns.Add("DoNo", typeof(int));
			dtGrid.Columns.Add("DoDate", typeof(DateTime));
			dtGrid.Columns.Add("LoadingCityId", typeof(int));
			dtGrid.Columns.Add("UnLoadingCityId", typeof(int));
			dtGrid.Columns.Add("VehicleNo", typeof(string));
			dtGrid.Columns.Add("BiltyNo", typeof(string));
			dtGrid.Columns.Add("BiltyDate", typeof(DateTime));
			dtGrid.Columns.Add("PoId", typeof(int));
			dtGrid.Columns.Add("PoNo", typeof(int));
			dtGrid.Columns.Add("NoOfBages", typeof(decimal));
			dtGrid.Columns.Add("NetWeightDo", typeof(decimal));
			dtGrid.Columns.Add("NetWeightWB", typeof(decimal));
			dtGrid.Columns.Add("BillWeightBaseId", typeof(int));
			dtGrid.Columns.Add("BillWeight", typeof(decimal));
			dtGrid.Columns.Add("FreightRate", typeof(decimal));
			dtGrid.Columns.Add("RateUomId", typeof(int));
			dtGrid.Columns.Add("QtyForRate", typeof(decimal));
			dtGrid.Columns.Add("BiltyFreight", typeof(decimal));
			dtGrid.Columns.Add("OtherCharges", typeof(decimal));
			dtGrid.Columns.Add("TotalBiltyFreight", typeof(decimal));
			dtGrid.Columns.Add("AdvanceOrCashFreight", typeof(decimal));
			dtGrid.Columns.Add("NetCreditToTransporter", typeof(decimal));
			dtGrid.Columns.Add("Remarks", typeof(string));
		}
	}

	public static void FillMasterVoucherDetailFromList(List<FreightVoucherOutward> invGrn, DataTable dtGrid, bool ExtraColumns = false)
	{
		if (invGrn != null && invGrn.Count == 0)
		{
			return;
		}
		foreach (FreightVoucherOutward detail in invGrn)
		{
			DataRow dr = dtGrid.NewRow();
			dr["Id"] = detail.FreightVoucherOutwardId;
			dr["DocNo"] = detail.DocNo;
			_ = detail.DocDate;
			dr["DocDate"] = detail.DocDate;
			dr["GPId"] = detail.GatePassOutwardId;
			dr["GpNo"] = detail.GpSrNo;
			dr["GpDate"] = detail.GpDate;
			dr["DoId"] = detail.InvDeliveryOrderId;
			dr["DoNo"] = detail.DeliveryOrderNo;
			dr["DoDate"] = detail.DeliveryOrderDate;
			dr["LoadingCityId"] = detail.loadingFromCityId;
			dr["UnLoadingCityId"] = detail.unloadingToCityId;
			dr["VehicleNo"] = detail.vehicleNo;
			dr["BiltyNo"] = detail.biltyNo;
			dr["BiltyDate"] = detail.biltyDate;
			dr["PoId"] = detail.PurchaseOrderHeaderId;
			dr["PoNo"] = detail.PurchaseOrderNo;
			dr["NoOfBages"] = detail.NoOfBags;
			dr["NetWeightDo"] = detail.NetWeightDo;
			dr["NetWeightWB"] = detail.NetWeightWB;
			dr["BillWeightBaseId"] = detail.BillWeightBaseId;
			dr["BillWeight"] = detail.netWeight;
			dr["FreightRate"] = detail.freightRate;
			dr["RateUomId"] = detail.rateUomId;
			dr["QtyForRate"] = detail.QtyforRate;
			dr["BiltyFreight"] = detail.biltyFreight;
			dr["OtherCharges"] = detail.otherCharges;
			dr["TotalBiltyFreight"] = detail.totalbiltyfreight;
			dr["AdvanceOrCashFreight"] = detail.AdvanceOrCashFreight;
			dr["NetCreditToTransporter"] = detail.totalFreight;
			dr["Remarks"] = detail.RemarksHeader;
			if (ExtraColumns)
			{
				dr["LoadingCity"] = detail.LoadingFromCityName;
				dr["UnLoadingCity"] = detail.UnloadingToCityName;
				dr["BillWeightBase"] = detail.BillWeightBaseId;
				dr["RateUom"] = detail.RateUomCode;
			}
			dtGrid.Rows.Add(dr);
		}
	}

	public static void VoucherDetailGridCommonSetting(GridEX gridEX, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "DocNo", "GPId", "DoId", "PoId", "AdvanceOrCashFreight" };
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 2, 3, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["DocDate"].Width = 90;
			gridEX.RootTable.Columns["GpNo"].Width = 65;
			gridEX.RootTable.Columns["GpDate"].Width = 80;
			gridEX.RootTable.Columns["DoNo"].Width = 65;
			gridEX.RootTable.Columns["DoDate"].Width = 80;
			gridEX.RootTable.Columns["VehicleNo"].Width = 90;
			gridEX.RootTable.Columns["BiltyNo"].Width = 90;
			gridEX.RootTable.Columns["BiltyDate"].Width = 80;
			gridEX.RootTable.Columns["PoNo"].Width = 65;
			gridEX.RootTable.Columns["NoOfBages"].Width = 80;
			gridEX.RootTable.Columns["NetWeightDo"].Width = 90;
			gridEX.RootTable.Columns["NetWeightWB"].Width = 90;
			gridEX.RootTable.Columns["BillWeight"].Width = 90;
			gridEX.RootTable.Columns["FreightRate"].Width = 80;
			gridEX.RootTable.Columns["QtyForRate"].Width = 80;
			gridEX.RootTable.Columns["BiltyFreight"].Width = 80;
			gridEX.RootTable.Columns["OtherCharges"].Width = 80;
			gridEX.RootTable.Columns["TotalBiltyFreight"].Width = 90;
			gridEX.RootTable.Columns["Remarks"].Width = 120;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PaymentDetailGridCommonSetting(GridEX gridEX, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "TransactionTypeId", "InstrumentTypeId", "AccountTitleId", "ChequeId" };
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 2, 3, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["TransactionType"].Width = 120;
			gridEX.RootTable.Columns["InstrumentType"].Width = 120;
			gridEX.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle + 50;
			gridEX.RootTable.Columns["ChequeNo"].Width = Constants.InventoryConstants.CellNo;
			gridEX.RootTable.Columns["ChequeDate"].Width = Constants.InventoryConstants.DateConstant;
			gridEX.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["PayeeTitle"].Width = Constants.InventoryConstants.AccountTitle;
			gridEX.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 50;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}
}

}
