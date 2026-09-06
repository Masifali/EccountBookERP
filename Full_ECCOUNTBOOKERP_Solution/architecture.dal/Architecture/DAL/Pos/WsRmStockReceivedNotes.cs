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

public class WsRmStockReceivedNotes
{
	public int SetData(Architecture.Model.POS.WsRmStockReceivedNotes obj, string ProcName)
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
			int LineId = 0;
			foreach (WsRmStockReceivedNotesDetail item3 in obj.WsRmStockReceivedNotesDetailList)
			{
				LineId++;
				item3.WsRmStockReceivedNotesId = obj.Id;
				item3.LineId = LineId;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Sp_WsRmStockReceivedNotesDetail_Insert"));
				item3.Id = code;
			}
			foreach (WsRmStockReceivedNotesExpenses item2 in obj.WsRmStockReceivedNotesExpensesList)
			{
				item2.WsRmStockReceivedNotesId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_WsRmStockReceivedNotesExpenses_Insert"));
			}
			List<SqlParameter> s = new List<SqlParameter>();
			s.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId"
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeSrNo",
				Value = obj.Id
			});
			DataTable dtvh = GenericProvider.GetDataTableProc<VoucherHead>("Sp_Vouchers_GetMethods", s);
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
			foreach (VoucherDetail item4 in obj.VoucherHeadInvoices.voucherDetailList)
			{
				WsRmStockReceivedNotesDetail d = obj.WsRmStockReceivedNotesDetailList.Find((WsRmStockReceivedNotesDetail x) => x.LineId == item4.LineId);
				if (d != null)
				{
					item4.RefDocSubIdNo = d.Id;
				}
				item4.VoucherHeadId = obj.VoucherHeadInvoices.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Sp_VoucherDetail_Insert"));
			}
			if (obj.VoucherHeadInvoices.voucherDetailList == null || obj.VoucherHeadInvoices.voucherDetailList.Count == 0)
			{
				throw new Exception("Voucher Detail List Not Found");
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

	public List<Architecture.Model.POS.WsRmStockReceivedNotes> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.POS.WsRmStockReceivedNotes> lst = new List<Architecture.Model.POS.WsRmStockReceivedNotes>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.POS.WsRmStockReceivedNotes>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.POS.WsRmStockReceivedNotes item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
						item2.WsRmStockReceivedNotesDetailList = GenericProvider.GetAllDetail<WsRmStockReceivedNotesDetail>(sqlConnection2, "Sp_WsRmStockReceivedNotes_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.POS.WsRmStockReceivedNotes item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadWsRmStockReceivedNotesExpensesByHeaderId"));
					item.WsRmStockReceivedNotesExpensesList = GenericProvider.GetAllDetail<WsRmStockReceivedNotesExpenses>(sqlcon, "Sp_WsRmStockReceivedNotes_GetAllMethod", lstPrm);
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
