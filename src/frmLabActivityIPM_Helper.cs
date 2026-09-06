using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model.IPM;
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
namespace Architecture.WinApp.IPM
{

public class frmLabActivityIPM_Helper
{
	public static void InitializeFumigationTable(DataTable dtFumigationDetail)
	{
		if (dtFumigationDetail != null && dtFumigationDetail.Columns.Count == 0)
		{
			dtFumigationDetail.Columns.Add("MasterId", typeof(int));
			dtFumigationDetail.Columns.Add("FumigationLotNo", typeof(string));
			dtFumigationDetail.Columns.Add("AnalysisStepId", typeof(int));
			dtFumigationDetail.Columns.Add("FumigatedDate", typeof(DateTime));
			dtFumigationDetail.Columns.Add("FumigatedDays", typeof(int));
			dtFumigationDetail.Columns.Add("CompletionDueDate", typeof(DateTime));
			dtFumigationDetail.Columns.Add("StatusAfterCompletionId", typeof(int));
			dtFumigationDetail.Columns.Add("NextFumigationPlanDate", typeof(DateTime));
			dtFumigationDetail.Columns.Add("FumigatedBy", typeof(string));
			dtFumigationDetail.Columns.Add("SupervisorCheckBy", typeof(string));
			dtFumigationDetail.Columns.Add("VerifiedBy", typeof(string));
			dtFumigationDetail.Columns.Add("StockReleaseHoldStatusId", typeof(int));
			dtFumigationDetail.Columns.Add("FumigationActivity", typeof(string));
		}
	}

	public static void FillFumigationDetailFromListByMasterId(List<labIPmActivityLog> obj, DataTable dt, bool addExtra = false)
	{
		if (obj == null || obj.Count == 0)
		{
			return;
		}
		foreach (labIPmActivityLog detail in from r in obj
			group r by r.FumigationLotNo into g
			select g.OrderByDescending((labIPmActivityLog r) => r.labIPmActivityLogId).FirstOrDefault())
		{
			DataRow dr = dt.NewRow();
			dr["MasterId"] = detail.labIPmActivityLogMasterId;
			dr["FumigationLotNo"] = detail.FumigationLotNo;
			dr["AnalysisStepId"] = detail.qcActivityStepId;
			dr["FumigatedDate"] = detail.qcActivityDate;
			dr["FumigatedDays"] = detail.activityForDays;
			dr["CompletionDueDate"] = detail.completionDueDate;
			dr["StatusAfterCompletionId"] = detail.qcActivityStatusAfterCompletionId;
			dr["NextFumigationPlanDate"] = detail.nextActivityPlanDate;
			dr["FumigatedBy"] = detail.fumigatedBy ?? string.Empty;
			dr["SupervisorCheckBy"] = detail.checkBy ?? string.Empty;
			dr["VerifiedBy"] = detail.verifiedBy ?? string.Empty;
			dr["StockReleaseHoldStatusId"] = detail.stockReleaseHoldStatusId;
			dr["FumigationActivity"] = detail.qcActivityDescription ?? string.Empty;
			dt.Rows.Add(dr);
		}
	}

	public static void FumigationDetailGridSetting(GridEX gridEX, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			if (((gridEX != null) ? gridEX.RootTable : null) != null)
			{
				List<string> hiddenColumns = new List<string> { "Id", "MasterId" };
				GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 3, 3, hiddenColumns, editableColumns);
				gridEX.RootTable.Columns["FumigationActivity"].Caption = "Fumigation Remarks";
				GridEX_Helper.SetColumnWidth(gridEX, "FumigationLotNo", 120);
				GridEX_Helper.SetColumnWidth(gridEX, "AnalysisStepId", 120);
				GridEX_Helper.SetColumnWidth(gridEX, "FumigatedDate", 120);
				GridEX_Helper.SetColumnWidth(gridEX, "FumigatedDays", 90);
				GridEX_Helper.SetColumnWidth(gridEX, "CompletionDueDate", 140);
				GridEX_Helper.SetColumnWidth(gridEX, "StatusAfterCompletionId", 160);
				GridEX_Helper.SetColumnWidth(gridEX, "NextFumigationPlanDate", 160);
				GridEX_Helper.SetColumnWidth(gridEX, "FumigatedBy", 140);
				GridEX_Helper.SetColumnWidth(gridEX, "SupervisorCheckBy", 160);
				GridEX_Helper.SetColumnWidth(gridEX, "VerifiedBy", 140);
				GridEX_Helper.SetColumnWidth(gridEX, "StockReleaseHoldStatusId", 180);
				GridEX_Helper.SetColumnWidth(gridEX, "FumigationActivity", 200);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void InitializeStockTable(DataTable dtStockDetail)
	{
		if (dtStockDetail != null && dtStockDetail.Columns.Count == 0)
		{
			dtStockDetail.Columns.Add("Id", typeof(int));
			dtStockDetail.Columns.Add("RefDocumentTypeId", typeof(int));
			dtStockDetail.Columns.Add("RefDocIdNo", typeof(int));
			dtStockDetail.Columns.Add("RefDocSubIdNo", typeof(int));
			dtStockDetail.Columns.Add("RefDocumentType", typeof(string));
			dtStockDetail.Columns.Add("RefDocDate", typeof(DateTime));
			dtStockDetail.Columns.Add("RefDocNo", typeof(int));
			dtStockDetail.Columns.Add("VehicleNo", typeof(string));
			dtStockDetail.Columns.Add("ItemId", typeof(int));
			dtStockDetail.Columns.Add("ItemCode", typeof(string));
			dtStockDetail.Columns.Add("ItemName", typeof(string));
			dtStockDetail.Columns.Add("QtyBalance", typeof(double));
			dtStockDetail.Columns.Add("WeightBalance", typeof(double));
			dtStockDetail.Columns.Add("GrnNo", typeof(int));
			dtStockDetail.Columns.Add("GpNo", typeof(int));
			dtStockDetail.Columns.Add("GPDate", typeof(DateTime));
			dtStockDetail.Columns.Add("SupplierCustomerId", typeof(int));
			dtStockDetail.Columns.Add("SupplierCustomerName", typeof(string));
			dtStockDetail.Columns.Add("WarehouseId", typeof(int));
			dtStockDetail.Columns.Add("WareHouseName", typeof(string));
			dtStockDetail.Columns.Add("RefWarehouse", typeof(string));
			dtStockDetail.Columns.Add("JobLotId", typeof(int));
			dtStockDetail.Columns.Add("JobLot", typeof(string));
			dtStockDetail.Columns.Add("ManualNo", typeof(string));
		}
	}

	public static void FillStockDetailFromListByMasterId(List<labIPmActivityLog> obj, DataTable dt, bool addExtra = false)
	{
		if (obj == null || obj.Count == 0)
		{
			return;
		}
		foreach (labIPmActivityLog detail in obj)
		{
			if (!dt.AsEnumerable().Any((DataRow r) => Conversion.ToInt(r["RefDocumentTypeId"]) == detail.refDocumentTypeId && Conversion.ToInt(r["RefDocIdNo"]) == detail.refDocumentHeaderId && Conversion.ToInt(r["RefDocSubIdNo"]) == detail.refDocumentDetialId))
			{
				DataRow dr = dt.NewRow();
				dr["Id"] = detail.labIPmActivityLogId;
				dr["RefDocumentTypeId"] = detail.refDocumentTypeId;
				dr["RefDocIdNo"] = detail.refDocumentHeaderId;
				dr["RefDocSubIdNo"] = detail.refDocumentDetialId;
				dr["RefDocumentType"] = detail.RefDocumentTypeName ?? string.Empty;
				dr["RefDocDate"] = Conversion.ToDateTime((object)detail.RefDocDate);
				dr["RefDocNo"] = detail.RefDocNo;
				dr["VehicleNo"] = detail.VehicleNo ?? string.Empty;
				dr["ItemId"] = detail.ItemId;
				dr["ItemCode"] = detail.ItemCode ?? string.Empty;
				dr["ItemName"] = detail.ItemName ?? string.Empty;
				dr["QtyBalance"] = detail.QtyBalance;
				dr["WeightBalance"] = detail.WeightBalance;
				dr["GrnNo"] = detail.GrnNo;
				dr["GpNo"] = detail.GpNo;
				dr["GPDate"] = Conversion.ToDateTime((object)detail.GPDate);
				dr["SupplierCustomerId"] = detail.SupplierCustomerId;
				dr["SupplierCustomerName"] = detail.SupplierCustomerName ?? string.Empty;
				dr["WarehouseId"] = detail.WarehouseId;
				dr["WareHouseName"] = detail.WareHouseCode ?? string.Empty;
				dr["JobLotId"] = detail.JobLotId;
				dr["JobLot"] = detail.JobLotDescription ?? string.Empty;
				dr["ManualNo"] = detail.ManualNo;
				dt.Rows.Add(dr);
			}
		}
	}
}

}
