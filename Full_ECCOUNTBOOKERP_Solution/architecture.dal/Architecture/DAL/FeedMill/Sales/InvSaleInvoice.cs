using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.FeedMill.Sales;
using Architecture.Model.Inventory;

namespace Architecture.DAL.FeedMill.Sales;

public class InvSaleInvoice : DALBase
{
	public int SetData(Architecture.Model.FeedMill.Sales.InvSaleInvoice obj, string ProcName)
	{
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
		new List<SupplierCustomer>();
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.InvSaleInvoiceDetaillist == null || obj.InvSaleInvoiceDetaillist.Count == 0)
			{
				throw new Exception("Detail List not found");
			}
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			int LineIdForPDI = 0;
			foreach (Architecture.Model.FeedMill.Sales.InvSaleInvoiceDetail item9 in obj.InvSaleInvoiceDetaillist)
			{
				LineIdForPDI = (item9.LineId = LineIdForPDI + 1);
				item9.InvSaleInvoiceId = obj.Id;
				item9.BillAmount = item9.ItemAmount + item9.ExpenseAmount - item9.CommissionAmount;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item9, "fed.USP_InvSaleInvoiceDetail_Insert"));
				item9.Id = code;
			}
			if (obj.InvSaleInvoiceFreightList != null && obj.InvSaleInvoiceFreightList.Count > 0)
			{
				foreach (Architecture.Model.FeedMill.Sales.InvSaleInvoiceFreight item8 in obj.InvSaleInvoiceFreightList)
				{
					item8.InvSaleInvoiceId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "fed.usp_InvSaleInvoiceFreight_Insert"));
				}
			}
			if (obj.InvSaleInvoiceJournalList != null && obj.InvSaleInvoiceJournalList.Count > 0)
			{
				foreach (Architecture.Model.FeedMill.Sales.InvSaleInvoiceJournal item7 in obj.InvSaleInvoiceJournalList)
				{
					item7.InvSaleInvoiceId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "fed.usp_InvSaleInvoiceJournal_Insert"));
				}
			}
			if (obj.InvSaleInvoiceExpenseList != null && obj.InvSaleInvoiceExpenseList.Count > 0)
			{
				foreach (Architecture.Model.FeedMill.Sales.InvSaleInvoiceExpense item6 in obj.InvSaleInvoiceExpenseList)
				{
					item6.InvSaleInvoiceId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "fed.usp_InvSaleInvoiceExpense_Insert"));
				}
			}
			if (obj.InvSaleInvoiceCommission != null && obj.InvSaleInvoiceCommission.Count > 0)
			{
				foreach (Architecture.Model.FeedMill.Sales.InvSaleInvoiceCommission item5 in obj.InvSaleInvoiceCommission)
				{
					item5.InvSaleInvoiceId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "fed.usp_InvSaleInvoiceCommission_Insert"));
				}
			}
			if (obj.SaleInvoicePaymentTermsDetailslist != null && obj.SaleInvoicePaymentTermsDetailslist.Count > 0)
			{
				int sortno = 1;
				foreach (Architecture.Model.FeedMill.Sales.SaleInvoicePaymentTermsDetail item4 in obj.SaleInvoicePaymentTermsDetailslist)
				{
					item4.InvSaleInvoiceId = obj.Id;
					item4.SortNo = sortno;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[fed].[usp_SaleInvoicePaymentTermsDetail_Insert]"));
					sortno++;
				}
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				foreach (DMSAttachments item3 in obj.AttachmentsList.ToList())
				{
					item3.RefAccountId = obj.SupplierCustomerId;
					item3.RefDocumentTypeId = obj.DocumentTypeId;
					item3.RefDocumentNo = returnID;
					item3.EntryDate = DateTime.Now;
					item3.ModifyDate = DateTime.Now;
					item3.EntryUser = obj.EntryUserId;
					item3.ModifyUser = obj.ModifyUserId;
					item3.OrganizationId = obj.OrganizationId;
					item3.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Proc_DMSAttachments_Insert"));
				}
			}
			InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
			inventoryTransactions.OrganizationId = obj.OrganizationId;
			inventoryTransactions.CompanyId = obj.CompanyId;
			inventoryTransactions.RefDocumentTypeId = obj.DocumentTypeId;
			inventoryTransactions.RefDocIdNo = obj.Id;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
			if (obj.DocumentTypeId == 1115)
			{
				InventoryTransactions transactions = new InventoryTransactions();
				transactions.OrganizationId = obj.OrganizationId;
				transactions.CompanyId = obj.CompanyId;
				transactions.RefDocumentTypeId = obj.DocumentTypeId;
				transactions.RefDocIdNo = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			}
			if (obj.DocumentTypeId != 1114)
			{
				foreach (Architecture.Model.FeedMill.Sales.InvSaleInvoiceDetail item2 in obj.InvSaleInvoiceDetaillist)
				{
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlcon;
					sqlCommand.Transaction = sqlTrn;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandText = "USP_InventoryValidation";
					sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
					sqlCommand.Parameters.AddWithValue("@DocDate", obj.DocDate);
					sqlCommand.Parameters.AddWithValue("@ItemId", item2.ItemId);
					sqlCommand.Parameters.AddWithValue("@WarehouseId", item2.WarehouseId);
					sqlCommand.Parameters.AddWithValue("@JobLotId", item2.JobLotId);
					sqlCommand.Parameters.AddWithValue("@CropYear", item2.CropYearId);
					sqlCommand.Parameters.AddWithValue("@InvPackingTypeId", item2.PackingTypeId);
					sqlCommand.Parameters.AddWithValue("@PackUomId", item2.ItemUOMId);
					sqlCommand.Parameters.AddWithValue("@NetWeight", item2.NetStockWeight);
					sqlCommand.ExecuteNonQuery();
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
			foreach (VoucherDetail item10 in obj.VoucherHeadInvoices.voucherDetailList)
			{
				item10.VoucherHeadId = obj.VoucherHeadInvoices.Id;
				Architecture.Model.FeedMill.Sales.InvSaleInvoiceDetail sid = obj.InvSaleInvoiceDetaillist.Find((Architecture.Model.FeedMill.Sales.InvSaleInvoiceDetail x) => x.LineId == item10.LineId && x.LineId > 0 && item10.LineId > 0);
				if (sid != null)
				{
					item10.RefDocSubIdNo = sid.Id;
				}
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item10, "Sp_VoucherDetail_Insert"));
			}
			if (obj.VoucherHeadInvoices.voucherDetailList == null || obj.VoucherHeadInvoices.voucherDetailList.Count == 0)
			{
				throw new Exception("Voucher Detail list Not Found");
			}
			SqlCommand sqlCommand2 = new SqlCommand();
			sqlCommand2.Connection = sqlcon;
			sqlCommand2.Transaction = sqlTrn;
			sqlCommand2.CommandType = CommandType.StoredProcedure;
			sqlCommand2.CommandText = "USP_VoucherBalanceCheck";
			sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
			sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
			sqlCommand2.Parameters.AddWithValue("@Id", obj.VoucherHeadInvoices.Id);
			sqlCommand2.ExecuteScalar();
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

	public List<Architecture.Model.FeedMill.Sales.InvSaleInvoice> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.FeedMill.Sales.InvSaleInvoice> lst = new List<Architecture.Model.FeedMill.Sales.InvSaleInvoice>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.FeedMill.Sales.InvSaleInvoice>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.FeedMill.Sales.InvSaleInvoice item6 in lst)
					{
						List<SqlParameter> lstPrm6 = new List<SqlParameter>();
						lstPrm6.Add(new SqlParameter("@Id", item6.Id));
						lstPrm6.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
						item6.InvSaleInvoiceDetaillist = GenericProvider.Get<Architecture.Model.FeedMill.Sales.InvSaleInvoiceDetail>(sqlConnection2, "[fed].[usp_InvSaleInvoice_GetAllMethod]", lstPrm6);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.FeedMill.Sales.InvSaleInvoice item5 in lst)
					{
						List<SqlParameter> lstPrm5 = new List<SqlParameter>();
						lstPrm5.Add(new SqlParameter("@Id", item5.Id));
						lstPrm5.Add(new SqlParameter("@Activity", "InvSaleInvoiceExpense_ReadBySaleInvoiceID"));
						item5.InvSaleInvoiceExpenseList = GenericProvider.GetAllDetail<Architecture.Model.FeedMill.Sales.InvSaleInvoiceExpense>(sqlConnection3, "[fed].[usp_InvSaleInvoice_GetAllMethod]", lstPrm5);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.FeedMill.Sales.InvSaleInvoice item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "InvSaleInvoiceFreight_ReadBySaleInvoiceID"));
						item4.InvSaleInvoiceFreightList = GenericProvider.Get<Architecture.Model.FeedMill.Sales.InvSaleInvoiceFreight>(sqlConnection4, "[fed].[usp_InvSaleInvoice_GetAllMethod]", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection5 = ConnectionObject.SQL())
			{
				sqlConnection5.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.FeedMill.Sales.InvSaleInvoice item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "InvSaleInvoiceJournal_ReadBySaleInvoiceID"));
						item3.InvSaleInvoiceJournalList = GenericProvider.Get<Architecture.Model.FeedMill.Sales.InvSaleInvoiceJournal>(sqlConnection5, "[fed].[usp_InvSaleInvoice_GetAllMethod]", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection6 = ConnectionObject.SQL())
			{
				sqlConnection6.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.FeedMill.Sales.InvSaleInvoice item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "InvSaleInvoiceCommission_ReadBySaleInvoiceID"));
						item2.InvSaleInvoiceCommission = GenericProvider.Get<Architecture.Model.FeedMill.Sales.InvSaleInvoiceCommission>(sqlConnection6, "[fed].[usp_InvSaleInvoice_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null && lst.Count > 0)
			{
				foreach (Architecture.Model.FeedMill.Sales.InvSaleInvoice item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "InvSaleInvoicePaymentTerm_ReadBySaleInvoiceID"));
					item.SaleInvoicePaymentTermsDetailslist = GenericProvider.Get<Architecture.Model.FeedMill.Sales.SaleInvoicePaymentTermsDetail>(sqlcon, "[fed].[usp_InvSaleInvoice_GetAllMethod]", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public void AccountandInventoryRemoveById(Architecture.Model.FeedMill.Sales.InvSaleInvoice obj)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Sp_InvoicesVouchersandStocksDelete");
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.BigInt, obj.OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.BigInt, obj.CompanyId);
			sqlDataBase.AddInParameter(cmd, "Id", SqlDbType.BigInt, obj.Id);
			sqlDataBase.AddInParameter(cmd, "DocumentTypeId", SqlDbType.BigInt, obj.DocumentTypeId);
			sqlDataBase.AddInParameter(cmd, "UserId", SqlDbType.Int, obj.EntryUserId);
			sqlDataBase.ExecuteNonQuery(cmd);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
