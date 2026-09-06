using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.POS;

namespace Architecture.DAL.Pos;

public class WsRmSaleInvoice
{
	public int SetData(Architecture.Model.POS.WsRmSaleInvoice obj, string ProcName)
	{
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (WsRmSaleInvoiceDetail item3 in obj.WsRmSaleInvoiceDetailsList)
			{
				item3.InvSaleInvoiceId = obj.Id;
				int code2 = (item3.Id = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Sp_WsRmSaleInvoiceDetail_Insert")));
				foreach (WsRmSalesInvoiceDetailDiscount Sub in item3.WsRmSalesInvoiceDetailDiscountsList)
				{
					Sub.WsRmSaleInvoiceId = returnID;
					Sub.WsRmSaleInvoiceDetailId = code2;
					Convert.ToInt64(GenericProvider.SetProc(sqlTrn, Sub, "Sp_WsRmSalesInvoiceDetailDiscount_Insert"));
				}
			}
			if (obj.WsRmSaleInvoiceDetailXGetYFreeslist != null && obj.WsRmSaleInvoiceDetailXGetYFreeslist.Count > 0)
			{
				int LineId = obj.WsRmSaleInvoiceDetailsList.Count;
				foreach (WsRmSaleInvoiceDetailXGetYFree item2 in obj.WsRmSaleInvoiceDetailXGetYFreeslist)
				{
					LineId++;
					item2.WsRmSaleInvoiceId = obj.Id;
					item2.LineId = LineId;
					int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_WsRmSaleInvoiceDetailXGetYFree_Insert"));
					item2.Id = code;
				}
			}
			DataTable dtvh = CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.Id);
			if (dtvh.Rows.Count > 0)
			{
				VoucherHeadIdGet = Conversion.ToInt(dtvh.Rows[0]["Id"]);
				obj.VoucherHeadInvoices.Id = VoucherHeadIdGet;
			}
			if (VoucherHeadIdGet == 0)
			{
				obj.VoucherHeadInvoices.DocumentTypeSrNo = obj.Id;
				obj.VoucherHeadInvoices.RefDocNoId = obj.Id;
				VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_Insert"));
			}
			else
			{
				obj.VoucherHeadInvoices.DocumentTypeSrNo = obj.Id;
				obj.VoucherHeadInvoices.RefDocNoId = obj.Id;
				VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_Update"));
			}
			if (VoucherHeadId > 0)
			{
				obj.VoucherHeadInvoices.Id = VoucherHeadId;
			}
			else
			{
				VoucherHeadId = obj.VoucherHeadInvoices.Id;
			}
			if (obj.VoucherHeadInvoices.voucherDetailList != null && obj.VoucherHeadInvoices.voucherDetailList.Count > 0)
			{
				foreach (VoucherDetail item4 in obj.VoucherHeadInvoices.voucherDetailList)
				{
					WsRmSaleInvoiceDetail sid = obj.WsRmSaleInvoiceDetailsList.Find((WsRmSaleInvoiceDetail x) => x.LineId == item4.LineId);
					if (sid != null)
					{
						item4.RefDocSubIdNo = sid.Id;
					}
					WsRmSaleInvoiceDetailXGetYFree sif = obj.WsRmSaleInvoiceDetailXGetYFreeslist.Find((WsRmSaleInvoiceDetailXGetYFree x) => x.LineId == item4.LineId);
					if (sif != null)
					{
						item4.RefDocSubIdNo = sif.Id;
					}
					item4.VoucherHeadId = obj.VoucherHeadInvoices.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Sp_VoucherDetail_Insert"));
				}
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_H_Insert"));
				foreach (VoucherDetail item in obj.VoucherHeadInvoices.voucherDetailList)
				{
					item.VoucherHeadId = obj.VoucherHeadInvoices.Id;
					item.DocumentTypeIdRef = Code;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_H_Insert"));
				}
				InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
				inventoryTransactions.OrganizationId = obj.OrganizationId;
				inventoryTransactions.CompanyId = obj.CompanyId;
				inventoryTransactions.RefDocumentTypeId = obj.DocumentTypeId;
				inventoryTransactions.RefDocIdNo = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
				InventoryTransactions transactions = new InventoryTransactions();
				transactions.OrganizationId = obj.OrganizationId;
				transactions.CompanyId = obj.CompanyId;
				transactions.RefDocumentTypeId = obj.DocumentTypeId;
				transactions.RefDocIdNo = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
				sqlTrn.Commit();
				return returnID;
			}
			throw new Exception("VoucherDetail List Not Found");
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.POS.WsRmSaleInvoice> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.POS.WsRmSaleInvoice> lst = new List<Architecture.Model.POS.WsRmSaleInvoice>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.POS.WsRmSaleInvoice>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.POS.WsRmSaleInvoice item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
						item2.WsRmSaleInvoiceDetailsList = GenericProvider.GetAllDetail<WsRmSaleInvoiceDetail>(sqlConnection2, "Sp_WsRmSaleInvoice_GetAllMethod", lstPrm2);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					string Ids = string.Empty;
					foreach (Architecture.Model.POS.WsRmSaleInvoice item3 in lst)
					{
						foreach (WsRmSaleInvoiceDetail Sub in item3.WsRmSaleInvoiceDetailsList)
						{
							Ids = Ids + Conversion.ToString(Sub.Id) + ",";
						}
					}
					List<SqlParameter> lstPrmSub = new List<SqlParameter>();
					lstPrmSub.Add(new SqlParameter("@Ids", Ids));
					lstPrmSub.Add(new SqlParameter("@Activity", "ReadWsRmSalesOrderDetailDiscountByDetailId"));
					List<WsRmSalesInvoiceDetailDiscount> lstDiscount = GenericProvider.GetProc<WsRmSalesInvoiceDetailDiscount>(sqlConnection3, "Sp_WsRmSaleInvoice_GetAllMethod", lstPrmSub);
					if (lstDiscount.Count > 0 && lstDiscount != null)
					{
						for (int i = 0; i < lst[0].WsRmSaleInvoiceDetailsList.Count; i++)
						{
							lst[0].WsRmSaleInvoiceDetailsList[i].WsRmSalesInvoiceDetailDiscountsList = new List<WsRmSalesInvoiceDetailDiscount>();
							for (int j = 0; j < lstDiscount.Count; j++)
							{
								if (lstDiscount[j].WsRmSaleInvoiceDetailId == lst[0].WsRmSaleInvoiceDetailsList[i].Id)
								{
									lst[0].WsRmSaleInvoiceDetailsList[i].WsRmSalesInvoiceDetailDiscountsList.Add(lstDiscount[j]);
								}
							}
						}
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.POS.WsRmSaleInvoice item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadSalesInvoiceDetailXGetYFreeByHeaderId"));
					item.WsRmSaleInvoiceDetailXGetYFreeslist = GenericProvider.GetAllDetail<WsRmSaleInvoiceDetailXGetYFree>(sqlcon, "Sp_WsRmSaleInvoice_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}
}
