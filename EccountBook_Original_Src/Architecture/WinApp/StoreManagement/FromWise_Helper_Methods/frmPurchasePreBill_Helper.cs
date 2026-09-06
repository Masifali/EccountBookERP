using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Architecture.Model.StoreManagement;
using Architecture.WinApp.Common;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.StoreManagement.FromWise_Helper_Methods;

public class frmPurchasePreBill_Helper
{
	public static void InitializeDetailtable(DataTable dtDetailGrid)
	{
		if (dtDetailGrid.Columns.Count == 0)
		{
			dtDetailGrid.Columns.Add("Id", typeof(int));
			dtDetailGrid.Columns.Add("PurchaseDemandHeaderId", typeof(int));
			dtDetailGrid.Columns.Add("PurchaseDemandDetailId", typeof(int));
			dtDetailGrid.Columns.Add("PurchaseDemandNo", typeof(int));
			dtDetailGrid.Columns.Add("ItemId", typeof(int));
			dtDetailGrid.Columns.Add("ItemCode");
			dtDetailGrid.Columns.Add("ItemName");
			dtDetailGrid.Columns.Add("UomId", typeof(int));
			dtDetailGrid.Columns.Add("Uom", typeof(string));
			dtDetailGrid.Columns.Add("UomEquivalent", typeof(decimal));
			dtDetailGrid.Columns.Add("ItemConditionId", typeof(int));
			dtDetailGrid.Columns.Add("DemandQty", typeof(decimal));
			dtDetailGrid.Columns.Add("PurchasedQty", typeof(decimal));
			dtDetailGrid.Columns.Add("BalanceQty", typeof(decimal));
			dtDetailGrid.Columns.Add("ThisQty", typeof(decimal));
			dtDetailGrid.Columns.Add("Rate", typeof(decimal));
			dtDetailGrid.Columns.Add("ItemAmountWithoutDiscount", typeof(decimal));
			dtDetailGrid.Columns.Add("DiscountAmount", typeof(decimal));
			dtDetailGrid.Columns.Add("ItemAmount", typeof(decimal));
			dtDetailGrid.Columns.Add("ExpenseAmount", typeof(decimal));
			dtDetailGrid.Columns.Add("ItemNetAmount", typeof(decimal));
			dtDetailGrid.Columns.Add("Remarks");
		}
	}

	public static void FillDetailFromListCommonForReadById(PurchasePreBillHeader invGrn, DataTable dt, bool ExtraColumns = false)
	{
		if (((invGrn != null) ? invGrn.PurchasePreBillDetailList : null) == null || invGrn.PurchasePreBillDetailList.Count == 0)
		{
			return;
		}
		foreach (PurchasePreBillDetail detail in invGrn.PurchasePreBillDetailList)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.PurchasePreBillDetailId;
			dr["PurchaseDemandHeaderId"] = detail.PurchaseDemandHeaderId;
			dr["PurchaseDemandDetailId"] = detail.PurchaseDemandDetailId;
			dr["PurchaseDemandNo"] = detail.PurchaseDemandDocNo;
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
			dr["DemandQty"] = detail.PurchaseDemandQty;
			dr["PurchasedQty"] = detail.TotalPurchasedForDemand;
			dr["BalanceQty"] = detail.PurchaseDemandQty - detail.TotalPurchasedForDemand;
			dr["ThisQty"] = detail.Qty;
			dr["Rate"] = detail.Rate;
			dr["ItemAmountWithoutDiscount"] = detail.ItemAmountWithoutDiscount;
			dr["DiscountAmount"] = detail.DiscountAmount;
			dr["ItemAmount"] = detail.ItemAmount;
			dr["ExpenseAmount"] = detail.ExpenseAmount;
			dr["ItemNetAmount"] = detail.ItemNetAmount;
			dr["Remarks"] = detail.RemarksDetail;
			dt.Rows.Add(dr);
		}
	}

	public static void DetailGridCommonSetting(GridEX gridEX, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "PurchaseDemandHeaderId", "PurchaseDemandDetailId", "ItemId", "UomId", "UomEquivalent" };
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 2, 3, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["ItemAmountWithoutDiscount"].Caption = "Gross Amount";
			gridEX.RootTable.Columns["DemandQty"].Caption = "Approved Demand Qty";
			gridEX.RootTable.Columns["PurchasedQty"].Caption = "Already Purchased Qty";
			gridEX.RootTable.Columns["BalanceQty"].Caption = "Balance Demand Qty";
			gridEX.RootTable.Columns["ThisQty"].Caption = "Purchased Qty";
			gridEX.RootTable.Columns["PurchaseDemandNo"].Width = 90;
			gridEX.RootTable.Columns["ItemCode"].Width = 100;
			gridEX.RootTable.Columns["ItemName"].Width = 200;
			gridEX.RootTable.Columns["Uom"].Width = 70;
			gridEX.RootTable.Columns["UomEquivalent"].Width = 80;
			gridEX.RootTable.Columns["DemandQty"].Width = 110;
			gridEX.RootTable.Columns["PurchasedQty"].Width = 110;
			gridEX.RootTable.Columns["BalanceQty"].Width = 90;
			gridEX.RootTable.Columns["ThisQty"].Width = 100;
			gridEX.RootTable.Columns["Rate"].Width = 80;
			gridEX.RootTable.Columns["ItemAmountWithoutDiscount"].Width = 110;
			gridEX.RootTable.Columns["DiscountAmount"].Width = 110;
			gridEX.RootTable.Columns["ItemAmount"].Width = 100;
			gridEX.RootTable.Columns["ExpenseAmount"].Width = 100;
			gridEX.RootTable.Columns["ItemNetAmount"].Width = 120;
			gridEX.RootTable.Columns["Remarks"].Width = 220;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}
}
