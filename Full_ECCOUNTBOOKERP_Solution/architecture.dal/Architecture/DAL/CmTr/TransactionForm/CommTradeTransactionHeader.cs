using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.CmTr.TransactionForm;
using Architecture.Model.Inventory;
using Architecture.Model.VPS;

namespace Architecture.DAL.CmTr.TransactionForm;

public class CommTradeTransactionHeader
{
	public int SetData(Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader obj, string ProcName)
	{
		int returnID = 0;
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
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
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (CommTradeTransactionDetail item10 in obj.CommTradeTransactionDetailList)
			{
				item10.CommTradeTransactionHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item10, "[CmTr].[USP_CommTradeTransactionDetail_Insert]"));
			}
			foreach (CommTradeTransactionPmOther item9 in obj.CommTradeTransactionPmOtherList)
			{
				item9.CommTradeTransactionHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item9, "[CmTr].[USP_CommTradeTransactionPmOther_Insert]"));
			}
			foreach (CommTradeTransactionRevExp item8 in obj.CommTradeTransactionRevExpList)
			{
				item8.CommTradeTransactionHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "[CmTr].[USP_CommTradeTransactionRevExp_Insert]"));
			}
			foreach (CommTradeTransactionCommBrokery item7 in obj.CommTradeTransactionCommBrokeryList)
			{
				item7.CommTradeTransactionHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "[CmTr].[USP_CommTradeTransactionCommBrokery_Insert]"));
			}
			foreach (CommTradeTransactionSupplierSummary item6 in obj.CommTradeTransactionSupplierSummaryList)
			{
				item6.CommTradeTransactionHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "[CmTr].[USP_CommTradeTransactionSupplierSummary_Insert]"));
			}
			foreach (CommTradeTransactionPaymentTermsDetail item5 in obj.CommTradeTransactionPaymentTermsDetailList)
			{
				item5.CommTradeTransactionHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "[CmTr].[USP_CommTradeTransactionPaymentTermsDetail_Insert]"));
			}
			foreach (CommTradeTransactionVehicleFreightDetail item4 in obj.CommTradeTransactionVehicleFreightDetailList)
			{
				item4.CommTradeTransactionHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[CmTr].[USP_CommTradeTransactionVehicleFreightDetail_Insert]"));
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
					item3.RefAccountId = obj.CommTradeTransactionDetailList[0].SupplierId;
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
				if (obj.DeleteAttachmentsList != null && obj.DeleteAttachmentsList.Count > 0)
				{
					CommonServices.DeleteAttachmentsPhysically(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.DeleteAttachmentsList);
				}
				sqlTrn.Commit();
				return returnID;
			}
			throw new Exception("VoucherDetail List Not Found");
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			CommonServices.DeleteAttachmentsPhysicallyInException(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, copiedFiles);
			throw ex;
		}
	}

	public List<Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader> lst = new List<Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader item7 in lst)
					{
						List<SqlParameter> lstPrm7 = new List<SqlParameter>();
						lstPrm7.Add(new SqlParameter("@Id", item7.Id));
						lstPrm7.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
						item7.CommTradeTransactionDetailList = GenericProvider.GetAllDetail<CommTradeTransactionDetail>(sqlConnection2, "[CmTr].[USP_CommTradeTransaction_GetAllMethod]", lstPrm7);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader item6 in lst)
					{
						List<SqlParameter> lstPrm6 = new List<SqlParameter>();
						lstPrm6.Add(new SqlParameter("@Id", item6.Id));
						lstPrm6.Add(new SqlParameter("@Activity", "CommTradeTransactionPm_ReadByCommTradeTransactionHeaderID"));
						item6.CommTradeTransactionPmOtherList = GenericProvider.GetAllDetail<CommTradeTransactionPmOther>(sqlConnection3, "[CmTr].[USP_CommTradeTransaction_GetAllMethod]", lstPrm6);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader item5 in lst)
					{
						List<SqlParameter> lstPrm5 = new List<SqlParameter>();
						lstPrm5.Add(new SqlParameter("@Id", item5.Id));
						lstPrm5.Add(new SqlParameter("@Activity", "CommTradeTransactionCommBrokery_ReadByCommTradeTransactionHeaderID"));
						item5.CommTradeTransactionCommBrokeryList = GenericProvider.GetAllDetail<CommTradeTransactionCommBrokery>(sqlConnection4, "[CmTr].[USP_CommTradeTransaction_GetAllMethod]", lstPrm5);
					}
				}
			}
			using (SqlConnection sqlConnection5 = ConnectionObject.SQL())
			{
				sqlConnection5.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "CommTradeTransactionRevExp_ReadByCommTradeTransactionHeaderID"));
						item4.CommTradeTransactionRevExpList = GenericProvider.GetAllDetail<CommTradeTransactionRevExp>(sqlConnection5, "[CmTr].[USP_CommTradeTransaction_GetAllMethod]", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection6 = ConnectionObject.SQL())
			{
				sqlConnection6.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "CommTradeTransactionSupplierSummary_ReadByCommTradeTransactionHeaderID"));
						item3.CommTradeTransactionSupplierSummaryList = GenericProvider.GetAllDetail<CommTradeTransactionSupplierSummary>(sqlConnection6, "[CmTr].[USP_CommTradeTransaction_GetAllMethod]", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection7 = ConnectionObject.SQL())
			{
				sqlConnection7.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "CommTradeTransactionPaymentTermsDetail_ReadByCommTradeTransactionHeaderID"));
						item2.CommTradeTransactionPaymentTermsDetailList = GenericProvider.GetAllDetail<CommTradeTransactionPaymentTermsDetail>(sqlConnection7, "[CmTr].[USP_CommTradeTransaction_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "CommTradeTransactionVehicleFreightDetail_ReadByCommTradeTransactionHeaderID"));
					item.CommTradeTransactionVehicleFreightDetailList = GenericProvider.GetAllDetail<CommTradeTransactionVehicleFreightDetail>(sqlcon, "[CmTr].[USP_CommTradeTransaction_GetAllMethod]", lstPrm);
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
