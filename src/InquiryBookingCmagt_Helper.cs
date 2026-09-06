using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Architecture.Common;
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

public class InquiryBookingCmagt_Helper
{
	public static void InitializeDetailTable(DataTable dtDetail)
	{
		if (dtDetail != null && dtDetail.Columns.Count == 0)
		{
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("SubPartyId", typeof(int));
			dtDetail.Columns.Add("ItemQty", typeof(decimal));
			dtDetail.Columns.Add("Rate", typeof(decimal));
			dtDetail.Columns.Add("Amount", typeof(decimal));
			dtDetail.Columns.Add("Remarks", typeof(string));
		}
	}

	public static void FillDetailFromListByMasterId(InquiryBookingMaster obj, DataTable dt, bool AddExtra = false)
	{
		if (((obj != null) ? obj.InquiryBookingPartyDetailList : null) == null || obj.InquiryBookingPartyDetailList.Count == 0)
		{
			return;
		}
		foreach (InquiryBookingPartyDetail detail in obj.InquiryBookingPartyDetailList)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.inquiryBookingPartyDetailId;
			dr["SubPartyId"] = detail.SubPartyId;
			dr["ItemQty"] = detail.itemQty;
			dr["Rate"] = detail.Rate;
			dr["Amount"] = detail.Amount;
			dr["Remarks"] = detail.remarks ?? string.Empty;
			if (AddExtra)
			{
				dr["SubPartyName"] = detail.SubPartyName ?? string.Empty;
			}
			dt.Rows.Add(dr);
		}
	}

	public static void DetailGridCommonSetting(GridEX gridEX, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "" };
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 3, 3, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["SubPartyId"].Width = 150;
			gridEX.RootTable.Columns["ItemQty"].Width = 80;
			gridEX.RootTable.Columns["Rate"].Width = 80;
			gridEX.RootTable.Columns["Amount"].Width = 100;
			gridEX.RootTable.Columns["Remarks"].Width = 200;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void FillDetailRow(InquiryBookingPartyDetail vd, GridEXRow r)
	{
		vd.inquiryBookingPartyDetailId = ((r.Cells["Id"].Value != null) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
		vd.SubPartyId = ((r.Cells["SubPartyId"].Value != null) ? Conversion.ToInt(r.Cells["SubPartyId"].Value) : 0);
		vd.itemQty = ((r.Cells["ItemQty"].Value != null) ? Conversion.ToDecimal(r.Cells["ItemQty"].Value) : 0m);
		vd.Rate = ((r.Cells["Rate"].Value != null) ? Conversion.ToDecimal(r.Cells["Rate"].Value) : 0m);
		vd.Amount = ((r.Cells["Amount"].Value != null) ? Conversion.ToDecimal(r.Cells["Amount"].Value) : 0m);
		vd.remarks = ((r.Cells["Remarks"].Value != null) ? r.Cells["Remarks"].Value.ToString() : string.Empty);
	}
}

}
