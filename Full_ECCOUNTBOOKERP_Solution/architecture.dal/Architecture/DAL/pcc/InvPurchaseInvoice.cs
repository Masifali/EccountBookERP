using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.pcc;

namespace Architecture.DAL.pcc;

public class InvPurchaseInvoice : DALBase
{
	public int SetData(Architecture.Model.pcc.InvPurchaseInvoice obj, string ProcName)
	{
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
		new List<SupplierCustomer>();
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			int LineId = 1;
			foreach (Architecture.Model.pcc.InvPurchaseInvoiceDetail item5 in obj.InvPurchaseInvoiceDetailList)
			{
				item5.LineId = LineId;
				item5.InvPurchaseInvoiceId = obj.Id;
				item5.ItemNetAmount = item5.ItemAmount + item5.ExpenseAmount + item5.CommissionAmount + item5.FreightAmount;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "[pcc].[USP_InvPurchaseInvoiceDetail_Insert]"));
				item5.Id = code;
				LineId++;
			}
			foreach (Architecture.Model.pcc.InvPurchaseInvoiceJournal item4 in obj.InvPurchaseInvoiceJournalList)
			{
				item4.InvPurchaseInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[pcc].[USP_InvPurchaseInvoiceJournal_Insert]"));
			}
			foreach (Architecture.Model.pcc.InvPurchaseInvoiceExpense item3 in obj.InvPurchaseInvoiceExpenseList)
			{
				item3.InvPurchaseInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "[pcc].[USP_InvPurchaseInvoiceExpense_Insert]"));
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				foreach (DMSAttachments item2 in obj.AttachmentsList.ToList())
				{
					item2.RefAccountId = obj.SupplierCustomerId;
					item2.RefDocumentTypeId = obj.DocumentTypeId;
					item2.RefDocumentNo = returnID;
					item2.EntryDate = DateTime.Now;
					item2.ModifyDate = DateTime.Now;
					item2.EntryUser = obj.EntryUserId;
					item2.ModifyUser = obj.ModifyUserId;
					item2.OrganizationId = obj.OrganizationId;
					item2.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Proc_DMSAttachments_Insert"));
				}
			}
			if (obj.DocumentTypeId == 1859 || obj.DocumentTypeId == 1865)
			{
				InventoryTransactions transactions = new InventoryTransactions();
				transactions.OrganizationId = obj.OrganizationId;
				transactions.CompanyId = obj.CompanyId;
				transactions.RefDocumentTypeId = obj.DocumentTypeId;
				transactions.RefDocIdNo = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			}
			InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
			inventoryTransactions.OrganizationId = obj.OrganizationId;
			inventoryTransactions.CompanyId = obj.CompanyId;
			inventoryTransactions.RefDocumentTypeId = obj.DocumentTypeId;
			inventoryTransactions.RefDocIdNo = obj.Id;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
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
			foreach (VoucherDetail item6 in obj.VoucherHeadInvoices.voucherDetailList)
			{
				item6.VoucherHeadId = obj.VoucherHeadInvoices.Id;
				Architecture.Model.pcc.InvPurchaseInvoiceDetail sid = obj.InvPurchaseInvoiceDetailList.Find((Architecture.Model.pcc.InvPurchaseInvoiceDetail x) => x.LineId == item6.LineId && x.LineId > 0 && item6.LineId > 0);
				if (sid != null)
				{
					item6.RefDocSubIdNo = sid.Id;
				}
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "Sp_VoucherDetail_Insert"));
			}
			if (obj.VoucherHeadInvoices.voucherDetailList == null || obj.VoucherHeadInvoices.voucherDetailList.Count == 0)
			{
				throw new Exception("Voucher Detail list Not Found");
			}
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "USP_VoucherBalanceCheck";
			sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
			sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
			sqlCommand.Parameters.AddWithValue("@Id", obj.VoucherHeadInvoices.Id);
			sqlCommand.ExecuteScalar();
			int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_H_Insert"));
			foreach (VoucherDetail item in obj.VoucherHeadInvoices.voucherDetailList)
			{
				item.VoucherHeadId = obj.VoucherHeadInvoices.Id;
				item.DocumentTypeIdRef = Code;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_H_Insert"));
			}
			DocumentApprovalDetail AP = new DocumentApprovalDetail();
			AP.OrganizationId = obj.OrganizationId;
			AP.CompanyId = obj.CompanyId;
			AP.DocumentTypeId = obj.DocumentTypeId;
			AP.Id = obj.Id;
			AP.LimitAmount = Conversion.ToDecimal(obj.BillAmount);
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, AP, "[DAW].[USp_DocumentApprovalDetail_Insert]"));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.pcc.InvPurchaseInvoice> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.pcc.InvPurchaseInvoice> lst = new List<Architecture.Model.pcc.InvPurchaseInvoice>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.pcc.InvPurchaseInvoice>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.pcc.InvPurchaseInvoice item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
						item3.InvPurchaseInvoiceDetailList = GenericProvider.Get<Architecture.Model.pcc.InvPurchaseInvoiceDetail>(sqlConnection2, "[pcc].[USP_InvPurchaseInvoice_GetAllMethod]", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.pcc.InvPurchaseInvoice item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "InvPurchaseInvoiceExpense_ReadByPurchaseInvoiceID"));
						item2.InvPurchaseInvoiceExpenseList = GenericProvider.GetAllDetail<Architecture.Model.pcc.InvPurchaseInvoiceExpense>(sqlConnection3, "[pcc].[USP_InvPurchaseInvoice_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null && lst.Count > 0)
			{
				foreach (Architecture.Model.pcc.InvPurchaseInvoice item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "InvPurchaseInvoiceJournal_ReadByPurchaseInvoiceID"));
					item.InvPurchaseInvoiceJournalList = GenericProvider.Get<Architecture.Model.pcc.InvPurchaseInvoiceJournal>(sqlcon, "[pcc].[USP_InvPurchaseInvoice_GetAllMethod]", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public static List<InventoryStockEvalautionDetail> GetInventoryStockEvalautionDetailByOtherIds(ReportsParameters obj)
	{
		try
		{
			new List<InventoryStockEvalautionDetail>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OtherDocumentTypeId",
				Value = obj.OtherDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OtherDocNoId",
				Value = obj.OtherDocNoId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OtherSubDocNoId",
				Value = obj.OtherSubDocNoId
			});
			return Conversion.ConvertDataTableToList<InventoryStockEvalautionDetail>(GenericProvider.GetDataTableProc<Architecture.Model.Company>("USP_GetInventoryStockEvalautionDetailByOtherIds", p));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
