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
using Architecture.Model.Tax.Purchase;

namespace Architecture.DAL.Tax;

public class PurchaseInvoice : DALBase
{
	public int SetData(Architecture.Model.Tax.Purchase.PurchaseInvoice obj, string ProcName)
	{
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
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
			string ItemIds = "";
			int LineIdForPDI = 0;
			bool WagesChargeToProduct = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "ContractWagesChargetoProduct"));
			foreach (PurchaseInvoiceDetail item8 in obj.PurchaseInvoiceDetailList)
			{
				LineIdForPDI = (item8.LineId = LineIdForPDI + 1);
				if (obj.DocumentTypeId == 98)
				{
					if (WagesChargeToProduct)
					{
						item8.BillAmount = item8.ItemAmount + item8.FreightAmount + item8.ExpenseAmount + item8.WagesAmount - item8.CommissionAmount;
					}
					else
					{
						item8.BillAmount = item8.ItemAmount + item8.FreightAmount + item8.ExpenseAmount - item8.CommissionAmount;
					}
				}
				else if (WagesChargeToProduct)
				{
					item8.BillAmount = item8.ItemAmount + item8.FreightAmount + item8.ExpenseAmount + item8.CommissionAmount + item8.Brokery + item8.WagesAmount;
				}
				else
				{
					item8.BillAmount = item8.ItemAmount + item8.FreightAmount + item8.ExpenseAmount + item8.CommissionAmount + item8.Brokery;
				}
				item8.PurchaseInvoiceId = obj.Id;
				ItemIds = ItemIds + item8.ItemId + ",";
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "[Tax].[USP_PurchaseInvoiceDetail_Insert]"));
				item8.Id = code;
			}
			foreach (PurchaseInvoiceFreight item7 in obj.PurchaseInvoiceFreightList)
			{
				item7.PurchaseInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "[Tax].[USP_PurchaseInvoiceFreight_Insert]"));
			}
			foreach (PurchaseInvoiceJournal item6 in obj.PurchaseInvoicejournalList)
			{
				item6.PurchaseInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "[Tax].[USP_PurchaseInvoiceJournal_Insert]"));
			}
			foreach (PurchaseInvoiceExpense item5 in obj.PurchaseInvoiceExpList)
			{
				item5.PurchaseInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "[Tax].[USP_PurchaseInvoiceExpense_Insert]"));
			}
			foreach (PurchaseInvoiceEmptyBags item4 in obj.PurchaseInvoiceEmptyBagsList)
			{
				item4.PurchaseInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[Tax].[USP_PurchaseInvoiceEmptyBags_Insert]"));
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
					item3.EntryUser = obj.EntryUser;
					item3.ModifyUser = obj.EntryUser;
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
			DataTable dtvh = CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.Id);
			if (dtvh.Rows.Count > 0)
			{
				VoucherHeadIdGet = Conversion.ToInt(dtvh.Rows[0]["Id"]);
				obj.VoucherHeadInvoices.Id = VoucherHeadIdGet;
			}
			if (VoucherHeadIdGet == 0)
			{
				obj.VoucherHeadInvoices.DocumentTypeSrNo = obj.Id;
				VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_Insert"));
			}
			else
			{
				obj.VoucherHeadInvoices.DocumentTypeSrNo = obj.Id;
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
			if (obj.VoucherHeadInvoices.voucherDetailList.Count > 0 && obj.VoucherHeadInvoices.voucherDetailList != null)
			{
				foreach (VoucherDetail item2 in obj.VoucherHeadInvoices.voucherDetailList)
				{
					item2.VoucherHeadId = obj.VoucherHeadInvoices.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_Insert"));
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
				if (obj.DocumentTypeId == 59 || obj.DocumentTypeId == 1604 || obj.DocumentTypeId == 1804)
				{
					InventoryTransactions transactions = new InventoryTransactions();
					transactions.OrganizationId = obj.OrganizationId;
					transactions.CompanyId = obj.CompanyId;
					transactions.RefDocumentTypeId = obj.DocumentTypeId;
					transactions.RefDocIdNo = returnID;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
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
			throw new Exception("VoucherDetail list Not Found");
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Tax.Purchase.PurchaseInvoice> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Tax.Purchase.PurchaseInvoice> lst = new List<Architecture.Model.Tax.Purchase.PurchaseInvoice>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Tax.Purchase.PurchaseInvoice>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Tax.Purchase.PurchaseInvoice item5 in lst)
					{
						List<SqlParameter> lstPrm5 = new List<SqlParameter>();
						lstPrm5.Add(new SqlParameter("@Id", item5.Id));
						lstPrm5.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
						item5.PurchaseInvoiceDetailList = GenericProvider.Get<PurchaseInvoiceDetail>(sqlConnection2, "[Tax].[USP_PurchaseInvoice_GetAllMethod]", lstPrm5);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Tax.Purchase.PurchaseInvoice item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "PurchaseInvoiceExpense_ReadByPurchaseInvoiceID"));
						item4.PurchaseInvoiceExpList = GenericProvider.GetAllDetail<PurchaseInvoiceExpense>(sqlConnection3, "[Tax].[USP_PurchaseInvoice_GetAllMethod]", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Tax.Purchase.PurchaseInvoice item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "PurchaseInvoiceFreight_ReadByPurchaseInvoiceID"));
						item3.PurchaseInvoiceFreightList = GenericProvider.Get<PurchaseInvoiceFreight>(sqlConnection4, "[Tax].[USP_PurchaseInvoice_GetAllMethod]", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection5 = ConnectionObject.SQL())
			{
				sqlConnection5.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Tax.Purchase.PurchaseInvoice item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "PurchaseInvoiceJournal_ReadByPurchaseInvoiceID"));
						item2.PurchaseInvoicejournalList = GenericProvider.Get<PurchaseInvoiceJournal>(sqlConnection5, "[Tax].[USP_PurchaseInvoice_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Tax.Purchase.PurchaseInvoice item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "PurchaseInvoiceEmptyBags_ReadByPurchaseInvoiceID"));
					item.PurchaseInvoiceEmptyBagsList = GenericProvider.Get<PurchaseInvoiceEmptyBags>(sqlcon, "[Tax].[USP_PurchaseInvoice_GetAllMethod]", lstPrm);
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
