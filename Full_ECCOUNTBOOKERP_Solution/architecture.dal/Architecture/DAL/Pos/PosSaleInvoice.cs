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

public class PosSaleInvoice
{
	public int SetData(Architecture.Model.POS.PosSaleInvoice obj, string ProcName)
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
			foreach (PosSaleInvoiceDetail item4 in obj.PosSaleInvoiceDetailList)
			{
				item4.InvSaleInvoiceId = obj.Id;
				int code2 = (item4.Id = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Sp_PosSaleInvoiceDetail_Insert")));
				List<PosSaleInvoiceDetailDiscount> posalesInvoiceDetailDiscountsList = item4.PosalesInvoiceDetailDiscountsList;
				if (posalesInvoiceDetailDiscountsList == null || posalesInvoiceDetailDiscountsList.Count <= 0)
				{
					continue;
				}
				foreach (PosSaleInvoiceDetailDiscount Sub in item4.PosalesInvoiceDetailDiscountsList)
				{
					Sub.PosSaleInvoiceId = returnID;
					Sub.PosSaleInvoiceDetailId = code2;
					Convert.ToInt64(GenericProvider.SetProc(sqlTrn, Sub, "Sp_PosSaleInvoiceDetailDiscount_Insert"));
				}
			}
			List<PosSaleInvoiceDetailXGetYFree> posSaleInvoiceDetailXGetYFreeslist = obj.PosSaleInvoiceDetailXGetYFreeslist;
			if (posSaleInvoiceDetailXGetYFreeslist != null && posSaleInvoiceDetailXGetYFreeslist.Count > 0)
			{
				int LineId = obj.PosSaleInvoiceDetailList.Count;
				foreach (PosSaleInvoiceDetailXGetYFree item2 in obj.PosSaleInvoiceDetailXGetYFreeslist)
				{
					LineId++;
					item2.InvSaleInvoiceId = obj.Id;
					item2.LineId = LineId;
					int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_PosSaleInvoiceDetailXGetYFree_Insert"));
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
			if (obj.VoucherHeadInvoices.voucherDetailList == null || obj.VoucherHeadInvoices.voucherDetailList.Count == 0)
			{
				throw new Exception("Voucher Detail List Not Found");
			}
			foreach (VoucherDetail item3 in obj.VoucherHeadInvoices.voucherDetailList)
			{
				PosSaleInvoiceDetail sid = obj.PosSaleInvoiceDetailList.Find((PosSaleInvoiceDetail x) => x.LineId == item3.LineId);
				if (sid != null)
				{
					item3.RefDocSubIdNo = sid.Id;
				}
				PosSaleInvoiceDetailXGetYFree sif = obj.PosSaleInvoiceDetailXGetYFreeslist.Find((PosSaleInvoiceDetailXGetYFree x) => x.LineId == item3.LineId);
				if (sif != null)
				{
					item3.RefDocSubIdNo = sif.Id;
				}
				item3.VoucherHeadId = obj.VoucherHeadInvoices.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Sp_VoucherDetail_Insert"));
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
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.POS.PosSaleInvoice> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.POS.PosSaleInvoice> lst = new List<Architecture.Model.POS.PosSaleInvoice>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.POS.PosSaleInvoice>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.POS.PosSaleInvoice item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
						item2.PosSaleInvoiceDetailList = GenericProvider.GetAllDetail<PosSaleInvoiceDetail>(sqlConnection2, "Sp_PosSaleInvoice_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.POS.PosSaleInvoice item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadPosSaleInvoiceDetailXGetYFreeByHeaderId"));
					item.PosSaleInvoiceDetailXGetYFreeslist = GenericProvider.GetAllDetail<PosSaleInvoiceDetailXGetYFree>(sqlcon, "Sp_PosSaleInvoice_GetAllMethod", lstPrm);
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
