using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.Steel.Inventory;
using Architecture.Model.VPS;

namespace Architecture.DAL.Steel.Inventory;

public class InvPurchaseInvoice : DALBase
{
	public int SetData(Architecture.Model.Steel.Inventory.InvPurchaseInvoice obj, string ProcName)
	{
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		_ = DateTime.Now;
		if (obj.ActionId == 2)
		{
			GetRecordsById(obj.Id);
		}
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (IsVpsAttachmentsServiceOn)
			{
				Sc = CommonServices.GetServerCredentials();
			}
			else
			{
				AttachmentFolderPath = CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Attachment Folder Path");
				if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0 && !Directory.Exists(AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				obj.AttachmentsList = CommonServices.CommonMethodForAttachmentsToPhysicallySave(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.AttachmentsList, copiedFiles);
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
			foreach (Architecture.Model.Steel.Inventory.InvPurchaseInvoiceDetail item7 in obj.invPurchaseInvoiceDetailList)
			{
				item7.ItemNetAmount = item7.ItemAmount + item7.FreightAmount + item7.ExpenseAmount + item7.CommissionAmount;
				item7.InvPurchaseInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "[ST].[USP_PurchaseInvoiceDetail_Insert]"));
			}
			foreach (Architecture.Model.Steel.Inventory.InvPurchaseInvoiceFreight item6 in obj.InvPurchaseInvoiceFreightList)
			{
				item6.InvPurchaseInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "[ST].[USP_InvPurchaseInvoiceFreight_Insert]"));
			}
			foreach (Architecture.Model.Steel.Inventory.InvPurchaseInvoiceJournal item5 in obj.InvPurchaseInvoicejournalList)
			{
				item5.InvPurchaseInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "[ST].[USP_InvPurchaseInvoiceJournal_Insert]"));
			}
			foreach (Architecture.Model.Steel.Inventory.InvPurchaseInvoiceExpense item4 in obj.InvPurchaseInvoiceExpList)
			{
				item4.InvPurchaseInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[ST].[USP_InvPurchaseInvoiceExpense_Insert]"));
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenName",
					Value = obj.AttachmentsList[0].ScreenName
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = returnID
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "DeleteById"
				});
				new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
				foreach (DMSAttachments item3 in obj.AttachmentsList.ToList())
				{
					item3.RefAccountId = obj.SupplierCustomerId;
					item3.RefDocumentTypeId = obj.DocumentTypeId;
					item3.RefDocumentNo = returnID;
					item3.EntryDate = DateTime.Now;
					item3.ModifyDate = DateTime.Now;
					item3.EntryUser = obj.EntryUserId;
					item3.ModifyUser = obj.EntryUserId;
					item3.OrganizationId = obj.OrganizationId;
					item3.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Proc_DMSAttachments_Insert"));
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
				InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
				inventoryTransactions.OrganizationId = obj.OrganizationId;
				inventoryTransactions.CompanyId = obj.CompanyId;
				inventoryTransactions.RefDocumentTypeId = obj.DocumentTypeId;
				inventoryTransactions.RefDocIdNo = returnID;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
				if (obj.DocumentTypeId == 1503)
				{
					InventoryTransactions transactions = new InventoryTransactions();
					transactions.OrganizationId = obj.OrganizationId;
					transactions.CompanyId = obj.CompanyId;
					transactions.RefDocumentTypeId = obj.DocumentTypeId;
					transactions.RefDocIdNo = returnID;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
				}
				if (obj.DeleteAttachmentsList != null && obj.DeleteAttachmentsList.Count > 0)
				{
					CommonServices.DeleteAttachmentsPhysically(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.DeleteAttachmentsList);
				}
				sqlTrn.Commit();
				return returnID;
			}
			throw new Exception("VoucherDetail list Not Found");
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			CommonServices.DeleteAttachmentsPhysicallyInException(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, copiedFiles);
			throw ex;
		}
	}

	public List<Architecture.Model.Steel.Inventory.InvPurchaseInvoice> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Steel.Inventory.InvPurchaseInvoice> lst = new List<Architecture.Model.Steel.Inventory.InvPurchaseInvoice>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Steel.Inventory.InvPurchaseInvoice>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Steel.Inventory.InvPurchaseInvoice item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
						item4.invPurchaseInvoiceDetailList = GenericProvider.Get<Architecture.Model.Steel.Inventory.InvPurchaseInvoiceDetail>(sqlConnection2, "[ST].[USP_PurchaseInvoice_GetAllMethod]", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Steel.Inventory.InvPurchaseInvoice item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "InvPurchaseInvoiceExpense_ReadByPurchaseInvoiceID"));
						item3.InvPurchaseInvoiceExpList = GenericProvider.GetAllDetail<Architecture.Model.Steel.Inventory.InvPurchaseInvoiceExpense>(sqlConnection3, "[ST].[USP_PurchaseInvoice_GetAllMethod]", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Steel.Inventory.InvPurchaseInvoice item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "InvPurchaseInvoiceFreight_ReadByPurchaseInvoiceID"));
						item2.InvPurchaseInvoiceFreightList = GenericProvider.Get<Architecture.Model.Steel.Inventory.InvPurchaseInvoiceFreight>(sqlConnection4, "[ST].[USP_PurchaseInvoice_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Steel.Inventory.InvPurchaseInvoice item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "InvPurchaseInvoiceJournal_ReadByPurchaseInvoiceID"));
					item.InvPurchaseInvoicejournalList = GenericProvider.Get<Architecture.Model.Steel.Inventory.InvPurchaseInvoiceJournal>(sqlcon, "[ST].[USP_PurchaseInvoice_GetAllMethod]", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public void AccountandInventoryRemoveById(Architecture.Model.Inventory.InvPurchaseInvoice obj)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Sp_InvoicesVouchersandStocksDelete");
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.BigInt, obj.OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.BigInt, obj.CompanyId);
			sqlDataBase.AddInParameter(cmd, "Id", SqlDbType.BigInt, obj.Id);
			sqlDataBase.AddInParameter(cmd, "DocumentTypeId", SqlDbType.BigInt, obj.DocumentTypeId);
			sqlDataBase.ExecuteNonQuery(cmd);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DateTime GetRecordsById(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetRecordId"
			});
			return Conversion.ToDateTime(GenericProvider.GetDataTableProc<Architecture.Model.Steel.Inventory.InvPurchaseInvoice>("[ST].[USP_PurchaseInvoice_GetAllMethod]", p).Rows[0][0]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
