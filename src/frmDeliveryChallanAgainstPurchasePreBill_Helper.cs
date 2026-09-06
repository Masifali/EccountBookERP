using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Architecture.Model.StoreManagement;
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
namespace Architecture.WinApp.StoreManagement.FromWise_Helper_Methods
{

public class frmDeliveryChallanAgainstPurchasePreBill_Helper
{
	public static void InitializeDetailtable(DataTable dtDetailGrid)
	{
		if (dtDetailGrid.Columns.Count == 0)
		{
			dtDetailGrid.Columns.Add("Id", typeof(int));
			dtDetailGrid.Columns.Add("PurchaseDemandHeaderId", typeof(int));
			dtDetailGrid.Columns.Add("PurchaseDemandDetailId", typeof(int));
			dtDetailGrid.Columns.Add("PurchaseDemandNo", typeof(int));
			dtDetailGrid.Columns.Add("PurchasePreBillHeaderId", typeof(int));
			dtDetailGrid.Columns.Add("PurchasePreBillDetailId", typeof(int));
			dtDetailGrid.Columns.Add("PurchasePreBillNo", typeof(int));
			dtDetailGrid.Columns.Add("ItemId", typeof(int));
			dtDetailGrid.Columns.Add("ItemCode");
			dtDetailGrid.Columns.Add("ItemName");
			dtDetailGrid.Columns.Add("UomId", typeof(int));
			dtDetailGrid.Columns.Add("Uom", typeof(string));
			dtDetailGrid.Columns.Add("UomEquivalent", typeof(decimal));
			dtDetailGrid.Columns.Add("ItemConditionId", typeof(string));
			dtDetailGrid.Columns.Add("TotalDemandQty", typeof(decimal));
			dtDetailGrid.Columns.Add("PreBillQty", typeof(decimal));
			dtDetailGrid.Columns.Add("UsedQty", typeof(decimal));
			dtDetailGrid.Columns.Add("BalanceQty", typeof(decimal));
			dtDetailGrid.Columns.Add("ThisQty", typeof(decimal));
			dtDetailGrid.Columns.Add("PartyBillNo", typeof(string));
			dtDetailGrid.Columns.Add("PartyBillDate", typeof(DateTime));
			dtDetailGrid.Columns.Add("VendorSupplierId", typeof(int));
			dtDetailGrid.Columns.Add("ReferencePartyId", typeof(int));
			dtDetailGrid.Columns.Add("VendorSupplier", typeof(string));
			dtDetailGrid.Columns.Add("Remarks");
		}
	}

	public static void FillDetailFromListCommonForReadById(DeliveryChallanHeader invGrn, DataTable dt, bool ExtraColumns = false)
	{
		if (((invGrn != null) ? invGrn.DeliveryChallanDetailList : null) == null || invGrn.DeliveryChallanDetailList.Count == 0)
		{
			return;
		}
		foreach (DeliveryChallanDetail detail in invGrn.DeliveryChallanDetailList)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.DeliveryChallanDetailId;
			dr["PurchaseDemandHeaderId"] = detail.PurchaseDemandHeaderId;
			dr["PurchaseDemandDetailId"] = detail.PurchaseDemandDetailId;
			dr["PurchaseDemandNo"] = detail.PurchaseDemandDocNo;
			dr["PurchasePreBillHeaderId"] = detail.PurchasePreBillHeaderId;
			dr["PurchasePreBillDetailId"] = detail.PurchasePreBillDetailId;
			dr["PurchasePreBillNo"] = detail.PurchasePreBillDocNo;
			dr["ItemId"] = detail.ItemId;
			dr["ItemCode"] = detail.ItemCode;
			dr["ItemName"] = detail.ItemName;
			dr["UomId"] = detail.UomId;
			dr["Uom"] = detail.Uom;
			dr["UomEquivalent"] = detail.UomEquivalent;
			dr["ItemConditionId"] = detail.ItemConditionId;
			if (ExtraColumns)
			{
				dr["ItemCondition"] = detail.ItemCondition;
			}
			dr["TotalDemandQty"] = detail.PurchaseDemandQty;
			dr["PreBillQty"] = detail.PurchasePreBillQty;
			dr["UsedQty"] = detail.AlreadyUsedPreBillQtyInDeliveryChallan;
			dr["BalanceQty"] = detail.PurchasePreBillQty - detail.AlreadyUsedPreBillQtyInDeliveryChallan;
			dr["ThisQty"] = detail.Qty;
			dr["PartyBillNo"] = detail.PartyBillNo;
			dr["PartyBillDate"] = detail.PartyBillDate;
			dr["VendorSupplierId"] = detail.VendorSupplierId;
			dr["ReferencePartyId"] = detail.ReferencePartyId;
			dr["VendorSupplier"] = ((detail.VendorSupplierId > 0) ? detail.VendorSupplierName : detail.ReferencePartyName);
			dr["Remarks"] = detail.RemarksDetail;
			dt.Rows.Add(dr);
		}
	}

	public static void DetailGridCommonSetting(GridEX gridEX, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "PurchaseDemandHeaderId", "PurchaseDemandDetailId", "PurchasePreBillHeaderId", "PurchasePreBillDetailId", "ItemId", "UomId", "UomEquivalent", "VendorSupplierId", "ReferencePartyId" };
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 2, 3, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["TotalDemandQty"].Caption = "Approved Demand Qty";
			gridEX.RootTable.Columns["UsedQty"].Caption = "Qty Delivered To Factory";
			gridEX.RootTable.Columns["ThisQty"].Caption = "Delivery Challan Qty";
			gridEX.RootTable.Columns["PurchaseDemandNo"].Width = 100;
			gridEX.RootTable.Columns["PurchasePreBillNo"].Width = 110;
			gridEX.RootTable.Columns["ItemCode"].Width = 100;
			gridEX.RootTable.Columns["ItemName"].Width = 220;
			gridEX.RootTable.Columns["Uom"].Width = 70;
			gridEX.RootTable.Columns["TotalDemandQty"].Width = 110;
			gridEX.RootTable.Columns["PreBillQty"].Width = 100;
			gridEX.RootTable.Columns["UsedQty"].Width = 120;
			gridEX.RootTable.Columns["BalanceQty"].Width = 90;
			gridEX.RootTable.Columns["ThisQty"].Width = 110;
			gridEX.RootTable.Columns["PartyBillNo"].Width = 120;
			gridEX.RootTable.Columns["PartyBillDate"].Width = 110;
			gridEX.RootTable.Columns["VendorSupplier"].Width = 180;
			gridEX.RootTable.Columns["Remarks"].Width = 220;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}
}

}
