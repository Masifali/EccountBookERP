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
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.Model.VPS;

namespace Architecture.DAL.Export;

public class ExImFCBankReceipts
{
	public int SetDate(Architecture.Model.Export.ExImFCBankReceipts obj, string ProcName)
	{
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
		new List<Architecture.Model.Export.ExImProceedsChargesType>();
		new List<SupplierCustomer>();
		string AttachmentFolderPath = "";
		List<string> copiedFiles = new List<string>();
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
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (ExImFcBankReceiptsDeductions item11 in obj.FcBankReceiptDetail)
			{
				item11.ExImFCBankReceiptsId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item11, "Sp_ExImFcBankReceiptsDeductions_Insert"));
			}
			foreach (Architecture.Model.Export.FcyBankReceiptBreakUp item10 in obj.FcyBankReceiptBreakUpList)
			{
				item10.FcyBankReceiptId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item10, "[dbo].[USP_FcyBankReceiptBreakUp_Insert]"));
			}
			int LineId = 1;
			foreach (FcyBankReceiptBreakUpofPartyInvoices item9 in obj?.FcyBankReceiptBreakUpofPartyInvoiceslist)
			{
				item9.SortNo = LineId++;
				item9.FcyBankReceiptId = obj.Id;
				item9.RefDocumentTypeId = 4;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item9, "[dbo].[USP_FcyBankReceiptBreakUpofPartyInvoices_Insert]"));
			}
			foreach (GainLossBreakup item8 in obj?.gainLossBreakupslist)
			{
				item8.FcyBankReceiptId = obj.Id;
				item8.OrganizationId = obj.OrganizationId;
				item8.CompanyId = obj.CompanyId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "[dbo].[Sp_GainLossBreakup_Insert]"));
			}
			foreach (FcyPartyPaymentBreackUpForFinancials item7 in obj?.FcyPartyPaymentBreackUpForFinancialslist)
			{
				item7.FcyReceiptId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "USP_FcyPartyPaymentBreackUpForFinancials_Insert"));
			}
			foreach (ExImShipmentPartyAgainstAdvancesPayment item6 in obj?.ExImShipmentPartyAgainstAdvancesPaymentslist)
			{
				item6.FcyReceiptId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "USP_ExImShipmentPartyAgainstAdvancesPayment_Insert"));
			}
			foreach (FcyBankReceiptPaymentTermDetail item5 in obj?.FcyBankReceiptPaymentTermDetails)
			{
				item5.FcyBankReceiptId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "usp_FcyBankReceiptPaymentTermDetail_Insert"));
			}
			if (obj.FcyBankReceiptBreakUpList.Count > 0)
			{
				foreach (Architecture.Model.Export.FcyBankReceiptBreakUp item4 in obj.FcyBankReceiptBreakUpList)
				{
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlcon;
					sqlCommand.Transaction = sqlTrn;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandText = "USP_ExportAdvanceFinancialInstumentUtilizeValidation";
					sqlCommand.Parameters.AddWithValue("@InvoiceId", item4.RefDocRecordId);
					sqlCommand.Parameters.AddWithValue("@GdId", item4.GDId);
					sqlCommand.Parameters.AddWithValue("@RefDocumentTypeId", item4.GdRefDocTypeId);
					sqlCommand.ExecuteNonQuery();
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
					item3.EntryUser = obj.EntryUser;
					item3.ModifyUser = obj.EntryUser;
					item3.OrganizationId = obj.OrganizationId;
					item3.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Proc_DMSAttachments_Insert"));
				}
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
				obj.FcBankVoucherHead.Id = VoucherHeadIdGet;
			}
			if (VoucherHeadIdGet == 0)
			{
				obj.FcBankVoucherHead.DocumentTypeSrNo = obj.Id;
				obj.FcBankVoucherHead.RefDocNoId = obj.Id;
				VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.FcBankVoucherHead, "Sp_VoucherHead_Insert"));
			}
			else
			{
				obj.FcBankVoucherHead.DocumentTypeSrNo = obj.Id;
				obj.FcBankVoucherHead.RefDocNoId = obj.Id;
				VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.FcBankVoucherHead, "Sp_VoucherHead_Update"));
			}
			if (VoucherHeadId > 0)
			{
				obj.FcBankVoucherHead.Id = VoucherHeadId;
			}
			else
			{
				VoucherHeadId = obj.FcBankVoucherHead.Id;
			}
			foreach (VoucherDetail item2 in obj.FcBankVoucherHead.voucherDetailList)
			{
				item2.BranchesId = obj.BranchId;
				item2.VoucherHeadId = obj.FcBankVoucherHead.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_Insert"));
			}
			SqlCommand sqlCommand2 = new SqlCommand();
			sqlCommand2.Connection = sqlcon;
			sqlCommand2.Transaction = sqlTrn;
			sqlCommand2.CommandType = CommandType.StoredProcedure;
			sqlCommand2.CommandText = "USP_VoucherBalanceCheck";
			sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
			sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
			sqlCommand2.Parameters.AddWithValue("@Id", obj.FcBankVoucherHead.Id);
			sqlCommand2.ExecuteScalar();
			obj.FcBankVoucherHead.RefDocNoId = obj.FcBankVoucherHead.Id;
			int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.FcBankVoucherHead, "Sp_VoucherHead_H_Insert"));
			foreach (VoucherDetail item in obj.FcBankVoucherHead.voucherDetailList)
			{
				item.VoucherHeadId = obj.FcBankVoucherHead.Id;
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
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			CommonServices.DeleteAttachmentsPhysicallyInException(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, copiedFiles);
			throw ex;
		}
	}

	public List<Architecture.Model.Export.ExImFCBankReceipts> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.ExImFCBankReceipts> lst = new List<Architecture.Model.Export.ExImFCBankReceipts>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.Get<Architecture.Model.Export.ExImFCBankReceipts>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExImFCBankReceipts item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
						item4.FcBankReceiptDetail = GenericProvider.Get<ExImFcBankReceiptsDeductions>(sqlConnection2, "Sp_ExImFCBankReceipts_GetAllMethod", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExImFCBankReceipts item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadBreakupByHeaderId"));
						item3.FcyBankReceiptBreakUpList = GenericProvider.Get<Architecture.Model.Export.FcyBankReceiptBreakUp>(sqlConnection3, "Sp_ExImFCBankReceipts_GetAllMethod", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExImFCBankReceipts item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadPartyBreakupByHeaderId"));
						item2.FcyPartyPaymentBreackUpForFinancialslist = GenericProvider.Get<FcyPartyPaymentBreackUpForFinancials>(sqlConnection4, "Sp_ExImFCBankReceipts_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Export.ExImFCBankReceipts item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadExImShipmentPartyAgainstAdvancesPaymentByHeaderId"));
					item.ExImShipmentPartyAgainstAdvancesPaymentslist = GenericProvider.Get<ExImShipmentPartyAgainstAdvancesPayment>(sqlcon, "Sp_ExImFCBankReceipts_GetAllMethod", lstPrm);
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
