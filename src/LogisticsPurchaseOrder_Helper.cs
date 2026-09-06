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

public class LogisticsPurchaseOrder_Helper
{
	public static void InitializeDetailtable(DataTable dtCustomerDetail)
	{
		if (dtCustomerDetail.Columns.Count == 0)
		{
			dtCustomerDetail.Columns.Add("Id", typeof(int));
			dtCustomerDetail.Columns.Add("AgreementHeaderId", typeof(int));
			dtCustomerDetail.Columns.Add("AgreementDetailId", typeof(int));
			dtCustomerDetail.Columns.Add("AgreementNo", typeof(int));
			dtCustomerDetail.Columns.Add("PartyRefDocNo", typeof(string));
			dtCustomerDetail.Columns.Add("PartyRefDocDate", typeof(DateTime));
			dtCustomerDetail.Columns.Add("ItemId", typeof(int));
			dtCustomerDetail.Columns.Add("ServiceItemCode");
			dtCustomerDetail.Columns.Add("ServiceItemName");
			dtCustomerDetail.Columns.Add("TransactionCurrencyId", typeof(int));
			dtCustomerDetail.Columns.Add("TransactionCurrency", typeof(string));
			dtCustomerDetail.Columns.Add("TransactionExchangeRate", typeof(decimal));
			dtCustomerDetail.Columns.Add("ServiceRate", typeof(decimal));
			dtCustomerDetail.Columns.Add("RateBaseId", typeof(int));
			dtCustomerDetail.Columns.Add("RateBase", typeof(string));
			dtCustomerDetail.Columns.Add("RateBaseEquivalent", typeof(decimal));
			dtCustomerDetail.Columns.Add("Qty", typeof(decimal));
			dtCustomerDetail.Columns.Add("NetWeight", typeof(decimal));
			dtCustomerDetail.Columns.Add("TcyAmount", typeof(decimal));
			dtCustomerDetail.Columns.Add("LcyAmount", typeof(decimal));
			dtCustomerDetail.Columns.Add("TaxNameId", typeof(int));
			dtCustomerDetail.Columns.Add("TaxName", typeof(string));
			dtCustomerDetail.Columns.Add("Tax%", typeof(decimal));
			dtCustomerDetail.Columns.Add("TaxAmount", typeof(decimal));
			dtCustomerDetail.Columns.Add("TotalTcyAmount", typeof(decimal));
			dtCustomerDetail.Columns.Add("LocationTypeId", typeof(int));
			dtCustomerDetail.Columns.Add("LocationType", typeof(string));
			dtCustomerDetail.Columns.Add("LocationFromId", typeof(int));
			dtCustomerDetail.Columns.Add("LocationFrom", typeof(string));
			dtCustomerDetail.Columns.Add("LocationToId", typeof(int));
			dtCustomerDetail.Columns.Add("LocationTo", typeof(string));
			dtCustomerDetail.Columns.Add("Remarks");
		}
	}

	public static void FillDetailFromListCommonForReadById(PurchaseOrderHeader invGrn, DataTable dt)
	{
		if (((invGrn != null) ? invGrn.PurchaseOrderDetailList : null) == null || invGrn.PurchaseOrderDetailList.Count == 0)
		{
			return;
		}
		foreach (PurchaseOrderDetail detail in invGrn.PurchaseOrderDetailList)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.PurchaseOrderDetailId;
			dr["AgreementHeaderId"] = detail.AgreementHeaderId;
			dr["AgreementDetailId"] = detail.AgreementDetailId;
			dr["AgreementNo"] = detail.AgreementDocNo;
			dr["PartyRefDocNo"] = detail.partyRefDocNo;
			dr["PartyRefDocDate"] = detail.partyRefDocDate;
			dr["ItemId"] = detail.serviceItemId;
			dr["ServiceItemCode"] = detail.ItemCode;
			dr["ServiceItemName"] = detail.ItemName;
			dr["TransactionCurrencyId"] = detail.transactionCurrencyId;
			dr["TransactionCurrency"] = detail.TransactionCurrencyCode;
			dr["TransactionExchangeRate"] = detail.transactionExchangeRate;
			dr["ServiceRate"] = detail.OrderRate;
			dr["RateBaseId"] = detail.RateBaseUomId;
			dr["RateBase"] = detail.RateBaseUom;
			dr["RateBaseEquivalent"] = detail.RateBaseUomEquivalent;
			dr["Qty"] = detail.Qty;
			dr["NetWeight"] = detail.NetWeight;
			dr["TcyAmount"] = detail.tcyAmount;
			dr["LcyAmount"] = detail.lcyAmount;
			dr["TaxNameId"] = detail.TaxNameId;
			dr["TaxName"] = detail.TaxName;
			dr["Tax%"] = detail.TaxPercent;
			dr["TaxAmount"] = detail.TaxAmount;
			dr["TotalTcyAmount"] = detail.TotalAmount;
			dr["LocationTypeId"] = detail.LocationTypeId;
			dr["LocationType"] = detail.LocationType;
			dr["LocationFromId"] = ((detail.LocationTypeId == 16) ? detail.LoadingPortId : detail.FromCityId);
			dr["LocationFrom"] = ((detail.LocationTypeId == 16) ? detail.LoadingPort : detail.FromCity);
			dr["LocationToId"] = ((detail.LocationTypeId == 16) ? detail.DestinationPortId : detail.ToCityId);
			dr["LocationTo"] = ((detail.LocationTypeId == 16) ? detail.DestinationPort : detail.ToCity);
			dr["Remarks"] = detail.Remarks;
			dt.Rows.Add(dr);
		}
	}

	public static void DetailGridCommonSetting(GridEX gridEX, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "ItemId", "TransactionCurrencyId", "RateBaseId", "RateBaseEquivalent", "AgreementHeaderId", "AgreementDetailId", "TaxNameId", "LocationTypeId", "LocationFromId",
				"LocationToId"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 3, 3, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["AgreementNo"].Width = 80;
			gridEX.RootTable.Columns["PartyRefDocNo"].Width = 100;
			gridEX.RootTable.Columns["PartyRefDocDate"].Width = 80;
			gridEX.RootTable.Columns["ServiceItemName"].Width = 160;
			gridEX.RootTable.Columns["ServiceItemCode"].Width = 100;
			gridEX.RootTable.Columns["TransactionCurrency"].Width = 80;
			gridEX.RootTable.Columns["TransactionExchangeRate"].Width = 80;
			gridEX.RootTable.Columns["ServiceRate"].Width = 80;
			gridEX.RootTable.Columns["RateBase"].Width = 80;
			gridEX.RootTable.Columns["Qty"].Width = 70;
			gridEX.RootTable.Columns["NetWeight"].Width = 90;
			gridEX.RootTable.Columns["TcyAmount"].Width = 90;
			gridEX.RootTable.Columns["LcyAmount"].Width = 90;
			gridEX.RootTable.Columns["TaxName"].Width = 90;
			gridEX.RootTable.Columns["Tax%"].Width = 65;
			gridEX.RootTable.Columns["TaxAmount"].Width = 90;
			gridEX.RootTable.Columns["TotalTcyAmount"].Width = 100;
			gridEX.RootTable.Columns["LocationType"].Width = 80;
			gridEX.RootTable.Columns["LocationFrom"].Width = 100;
			gridEX.RootTable.Columns["LocationTo"].Width = 100;
			gridEX.RootTable.Columns["Remarks"].Width = 150;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}
}

}
