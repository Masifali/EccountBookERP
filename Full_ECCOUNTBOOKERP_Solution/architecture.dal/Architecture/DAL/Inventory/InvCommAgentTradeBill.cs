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
using Architecture.Model.Inventory;
using Architecture.Model.VPS;

namespace Architecture.DAL.Inventory;

public class InvCommAgentTradeBill
{
	public int SetData(Architecture.Model.Inventory.InvCommAgentTradeBill obj, string ProcName)
	{
		int returnID = 0;
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
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
			foreach (InvCommAgentTradeBillDetail item12 in obj.InvCommAgentTradeBillDetailslist)
			{
				item12.InvCommAgentTradeBillId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item12, "Sp_InvCommAgentTradeBillDetail_Insert"));
			}
			foreach (CommisionAgentBillPurchaseExpense item11 in obj.InvCommAgentTradePurchaseExpList)
			{
				item11.InvCommAgentBillTradeId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item11, "Sp_InvCommisionAgentBillPurchaseExpense_Insert"));
			}
			foreach (CommisionAgentBillSaleExpense item10 in obj.InvCommAgentTradeSaleExpList)
			{
				item10.InvCommAgentBillTradeId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item10, "Sp_InvCommisionAgentBillSaleExpense_Insert"));
			}
			foreach (CommisionAgentBillSaleExpenseCreditToReleventAc item9 in obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList)
			{
				item9.InvCommAgentBillTradeId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item9, "Sp_CommisionAgentBillSaleExpenseCreditToReleventAc_Insert"));
			}
			foreach (CommisionAgentBillPurchaseFreightExpense item8 in obj.InvCommAgentTradeFreightExpList)
			{
				item8.InvCommAgentBillTradeId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "USP_CommisionAgentBillPurchaseFreightExpense_Insert"));
			}
			if (obj.CommisionAgentBillPaymentDetailList != null)
			{
				foreach (CommisionAgentBillPaymentDetail item7 in obj.CommisionAgentBillPaymentDetailList)
				{
					item7.InvCommAgentBillTradeId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "USP_CommisionAgentBillPaymentDetail_Insert"));
				}
			}
			if (obj.CommisionAgentBillCommissionDetailList != null)
			{
				foreach (CommisionAgentBillCommissionDetail item6 in obj.CommisionAgentBillCommissionDetailList)
				{
					item6.InvCommAgentBillTradeId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "USP_CommisionAgentBillCommissionDetail_Insert"));
				}
			}
			if (obj.CommisionAgentBillFreightDetailList != null)
			{
				foreach (CommisionAgentBillFreightDetail item5 in obj.CommisionAgentBillFreightDetailList)
				{
					item5.InvCommAgentBillTradeId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "USP_CommisionAgentBillFreightDetail_Insert"));
				}
			}
			if (obj.CommisionAgentBillTaxDetailList != null)
			{
				foreach (CommisionAgentBillTaxDetail item4 in obj.CommisionAgentBillTaxDetailList)
				{
					item4.InvCommAgentBillTradeId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "USP_CommisionAgentBillTaxDetail_Insert"));
				}
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
					Value = obj.Id
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "DeleteById"
				});
				new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
				foreach (DMSAttachments item3 in obj.AttachmentsList.ToList())
				{
					item3.RefAccountId = obj.SupplierId;
					item3.RefDocumentTypeId = obj.DocumentTypeId;
					item3.RefDocumentNo = obj.Id;
					item3.OrganizationId = obj.OrganizationId;
					item3.CompanyId = obj.CompanyId;
					item3.EntryUser = obj.EnteryUserId;
					item3.ModifyUser = obj.EnteryUserId;
					item3.EntryDate = obj.EnteryDate;
					item3.ModifyDate = obj.ModifyDate;
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
					item2.BranchesId = obj.BranchesId;
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
					item.BranchesId = obj.BranchesId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_H_Insert"));
				}
				if (obj.DocumentTypeId == 1056)
				{
					SqlCommand sqlCommand2 = new SqlCommand();
					sqlCommand2.Connection = sqlcon;
					sqlCommand2.Transaction = sqlTrn;
					sqlCommand2.CommandType = CommandType.StoredProcedure;
					sqlCommand2.CommandText = "[dbo].[USP_InvCommAgentTradeBill_GenerateSpecialApprovalLog]";
					sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand2.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
					sqlCommand2.Parameters.AddWithValue("@DocId", obj.Id);
					sqlCommand2.Parameters.AddWithValue("@UserId", (obj.EnteryUserId > 0) ? obj.EnteryUserId : obj.ModifyUserId);
					sqlCommand2.ExecuteNonQuery();
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

	public List<Architecture.Model.Inventory.InvCommAgentTradeBill> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		try
		{
			List<Architecture.Model.Inventory.InvCommAgentTradeBill> lst;
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.Get<Architecture.Model.Inventory.InvCommAgentTradeBill>(sqlConnection, ProcName, p);
			}
			if (lst == null || lst.Count == 0)
			{
				return lst;
			}
			foreach (Architecture.Model.Inventory.InvCommAgentTradeBill item in lst)
			{
				using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
				{
					sqlConnection2.Open();
					item.InvCommAgentTradeBillDetailslist = GetDetail<InvCommAgentTradeBillDetail>(sqlConnection2, item.Id, "ReadByHeaderId_InvCommAgentTradeBillDetail");
				}
				using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
				{
					sqlConnection3.Open();
					item.InvCommAgentTradePurchaseExpList = GetDetail<CommisionAgentBillPurchaseExpense>(sqlConnection3, item.Id, "ReadByHeaderId_CommisionAgentBillPurchaseExpense");
				}
				using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
				{
					sqlConnection4.Open();
					item.InvCommAgentTradeFreightExpList = GetDetail<CommisionAgentBillPurchaseFreightExpense>(sqlConnection4, item.Id, "ReadByHeaderId_CommisionAgentBillPurchaseFreightExpense");
				}
				using (SqlConnection sqlConnection5 = ConnectionObject.SQL())
				{
					sqlConnection5.Open();
					item.InvCommAgentTradeSaleExpList = GetDetail<CommisionAgentBillSaleExpense>(sqlConnection5, item.Id, "ReadByHeaderId_CommisionAgentBillSaleExpense");
				}
				using (SqlConnection sqlConnection6 = ConnectionObject.SQL())
				{
					sqlConnection6.Open();
					item.CommisionAgentBillSaleExpenseCreditToReleventAcsList = GetDetail<CommisionAgentBillSaleExpenseCreditToReleventAc>(sqlConnection6, item.Id, "ReadByHeaderId_CommisionAgentBillSaleExpenseCreditToReleventAc");
				}
				using (SqlConnection sqlConnection7 = ConnectionObject.SQL())
				{
					sqlConnection7.Open();
					item.CommisionAgentBillCommissionDetailList = GetDetail<CommisionAgentBillCommissionDetail>(sqlConnection7, item.Id, "ReadByHeaderId_CommisionAgentBillCommissionDetail");
				}
				using (SqlConnection sqlConnection8 = ConnectionObject.SQL())
				{
					sqlConnection8.Open();
					item.CommisionAgentBillPaymentDetailList = GetDetail<CommisionAgentBillPaymentDetail>(sqlConnection8, item.Id, "ReadByHeaderId_CommisionAgentBillPaymentDetail");
				}
				using (SqlConnection sqlConnection9 = ConnectionObject.SQL())
				{
					sqlConnection9.Open();
					item.CommisionAgentBillFreightDetailList = GetDetail<CommisionAgentBillFreightDetail>(sqlConnection9, item.Id, "ReadByHeaderId_CommisionAgentBillFreightDetail");
				}
				using SqlConnection sqlcon = ConnectionObject.SQL();
				sqlcon.Open();
				item.CommisionAgentBillTaxDetailList = GetDetail<CommisionAgentBillTaxDetail>(sqlcon, item.Id, "ReadByHeaderId_CommisionAgentBillTaxDetail");
			}
			return lst;
		}
		catch
		{
			throw;
		}
	}

	private List<T> GetDetail<T>(SqlConnection sqlcon, int id, string activity)
	{
		List<SqlParameter> prm = new List<SqlParameter>
		{
			new SqlParameter("@Id", id),
			new SqlParameter("@Activity", activity)
		};
		return GenericProvider.GetAllDetail<T>(sqlcon, "USP_InvCommAgentTradeBill_GetAllMethods", prm);
	}
}
