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
using Architecture.Model.VPS;
using Architecture.Model.lgstcm;

namespace Architecture.DAL.lgstcm;

public class FreightVoucherOutward
{
	public int SetData(Architecture.Model.lgstcm.FreightVoucherOutward obj, string procName, SqlTransaction defaultSqlTran = null, SqlConnection defaultSqlCon = null)
	{
		int returnID = 0;
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
		List<string> copiedFiles = new List<string>();
		string attachmentFolderPath = "";
		ServerCredentials serverCredentials = new ServerCredentials();
		bool isVpsAttachmentServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		bool hasExternalTran = defaultSqlTran != null;
		SqlConnection sqlcon = (hasExternalTran ? defaultSqlCon : ConnectionObject.SQL());
		if (!hasExternalTran)
		{
			sqlcon.Open();
		}
		SqlTransaction sqlTrn = (hasExternalTran ? defaultSqlTran : sqlcon.BeginTransaction());
		try
		{
			if (isVpsAttachmentServiceOn)
			{
				serverCredentials = CommonServices.GetServerCredentials();
			}
			else
			{
				attachmentFolderPath = CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Attachment Folder Path");
				List<DMSAttachments> attachmentsList = obj.AttachmentsList;
				if (attachmentsList != null && attachmentsList.Count > 0 && !Directory.Exists(attachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
			}
			List<DMSAttachments> attachmentsList2 = obj.AttachmentsList;
			if (attachmentsList2 != null && attachmentsList2.Count > 0)
			{
				obj.AttachmentsList = CommonServices.CommonMethodForAttachmentsToPhysicallySave(serverCredentials, isVpsAttachmentServiceOn, attachmentFolderPath, obj.AttachmentsList, copiedFiles);
				obj.AttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments x) => x.Attachment));
				obj.CustomAttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments x) => x.UploadedFileCustomName));
			}
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, procName));
			obj.FreightVoucherOutwardId = ((returnID > 0) ? returnID : obj.FreightVoucherOutwardId);
			int LineId = 1;
			List<FreightVoucherOutwardPaymentDetail> freightVoucherOutwardPaymentDetailList = obj.FreightVoucherOutwardPaymentDetailList;
			if (freightVoucherOutwardPaymentDetailList != null && freightVoucherOutwardPaymentDetailList.Count > 0)
			{
				foreach (FreightVoucherOutwardPaymentDetail detail2 in obj.FreightVoucherOutwardPaymentDetailList)
				{
					detail2.LineId = LineId;
					detail2.FreightVoucherOutwardId = obj.FreightVoucherOutwardId;
					if (Conversion.CheckDateTimeNull(detail2.CheqDate))
					{
						detail2.CheqDate = DateTime.Now;
					}
					GenericProvider.SetProc(sqlTrn, detail2, "[lgstcm].[USP_FreightVoucherOutwardPaymentDetail_Insert]");
					LineId++;
				}
			}
			List<FreightVoucherOutwardExpenseDetail> freightVoucherOutwardExpenseDetailList = obj.FreightVoucherOutwardExpenseDetailList;
			if (freightVoucherOutwardExpenseDetailList != null && freightVoucherOutwardExpenseDetailList.Count > 0)
			{
				foreach (FreightVoucherOutwardExpenseDetail detail in obj.FreightVoucherOutwardExpenseDetailList)
				{
					detail.FreightVoucherOutwardId = obj.FreightVoucherOutwardId;
					GenericProvider.SetProc(sqlTrn, detail, "[lgstcm].[USP_FreightVoucherOutwardExpenseDetail_Insert]");
				}
			}
			List<DMSAttachments> attachmentsList3 = obj.AttachmentsList;
			if (attachmentsList3 != null && attachmentsList3.Count > 0)
			{
				List<SqlParameter> p = new List<SqlParameter>
				{
					new SqlParameter("@ScreenName", obj.AttachmentsList[0].ScreenName),
					new SqlParameter("@Id", obj.FreightVoucherOutwardId),
					new SqlParameter("@Activity", "DeleteById")
				};
				new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
				foreach (DMSAttachments att in obj.AttachmentsList)
				{
					att.RefAccountId = obj.EntryUserId;
					att.RefDocumentTypeId = obj.documentTypeId;
					att.RefDocumentNo = obj.FreightVoucherOutwardId;
					att.EntryUser = obj.EntryUserId;
					att.ModifyUser = obj.ModifyUserId;
					att.EntryDate = DateTime.Now;
					att.ModifyDate = DateTime.Now;
					att.OrganizationId = obj.OrganizationId;
					att.CompanyId = obj.CompanyId;
					GenericProvider.SetProc(sqlTrn, att, "Proc_DMSAttachments_Insert");
				}
			}
			using (SqlCommand cmd = new SqlCommand("Sp_Vouchers_GetMethods", sqlcon, sqlTrn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				cmd.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				cmd.Parameters.AddWithValue("@Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId");
				cmd.Parameters.AddWithValue("@DocumentTypeId", obj.documentTypeId);
				cmd.Parameters.AddWithValue("@DocumentTypeSrNo", obj.FreightVoucherOutwardId);
				object result = cmd.ExecuteScalar();
				if (result != null && result != DBNull.Value)
				{
					VoucherHeadIdGet = Conversion.ToInt(result);
					obj.VoucherHeadList.Id = VoucherHeadIdGet;
				}
			}
			if (VoucherHeadIdGet == 0)
			{
				obj.VoucherHeadList.DocumentTypeSrNo = obj.FreightVoucherOutwardId;
				obj.VoucherHeadList.RefDocNoId = obj.FreightVoucherOutwardId;
				VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadList, "Sp_VoucherHead_Insert"));
			}
			else
			{
				obj.VoucherHeadList.DocumentTypeSrNo = obj.FreightVoucherOutwardId;
				obj.VoucherHeadList.RefDocNoId = obj.FreightVoucherOutwardId;
				VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadList, "Sp_VoucherHead_Update"));
			}
			if (VoucherHeadId > 0)
			{
				obj.VoucherHeadList.Id = VoucherHeadId;
			}
			else
			{
				VoucherHeadId = obj.VoucherHeadList.Id;
			}
			if (obj.VoucherHeadList.voucherDetailList == null || obj.VoucherHeadList.voucherDetailList.Count <= 0)
			{
				throw new Exception("VoucherDetail List Not Found");
			}
			foreach (VoucherDetail item in obj.VoucherHeadList.voucherDetailList)
			{
				item.VoucherHeadId = obj.VoucherHeadList.Id;
				FreightVoucherOutwardPaymentDetail detailid = obj.FreightVoucherOutwardPaymentDetailList?.Find((FreightVoucherOutwardPaymentDetail x) => x.LineId == item.LineId && x.LineId > 0 && item.LineId > 0);
				if (detailid != null)
				{
					item.RefDocSubIdNo = detailid.FreightVoucherOutwardPaymentDetailId;
				}
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_Insert"));
			}
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "USP_VoucherBalanceCheck";
			sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
			sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
			sqlCommand.Parameters.AddWithValue("@Id", obj.VoucherHeadList.Id);
			sqlCommand.ExecuteNonQuery();
			int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadList, "Sp_VoucherHead_H_Insert"));
			foreach (VoucherDetail item2 in obj.VoucherHeadList.voucherDetailList)
			{
				item2.VoucherHeadId = obj.VoucherHeadList.Id;
				item2.DocumentTypeIdRef = Code;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_H_Insert"));
			}
			List<DMSAttachments> deleteAttachmentsList = obj.DeleteAttachmentsList;
			if (deleteAttachmentsList != null && deleteAttachmentsList.Count > 0)
			{
				CommonServices.DeleteAttachmentsPhysically(serverCredentials, isVpsAttachmentServiceOn, attachmentFolderPath, obj.DeleteAttachmentsList);
			}
			if (!hasExternalTran)
			{
				sqlTrn.Commit();
			}
		}
		catch
		{
			if (!hasExternalTran)
			{
				sqlTrn.Rollback();
			}
			CommonServices.DeleteAttachmentsPhysicallyInException(serverCredentials, isVpsAttachmentServiceOn, attachmentFolderPath, copiedFiles);
			throw;
		}
		finally
		{
			if (!hasExternalTran && sqlcon.State == ConnectionState.Open)
			{
				sqlcon.Close();
			}
		}
		return obj.FreightVoucherOutwardId;
	}

	public List<Architecture.Model.lgstcm.FreightVoucherOutward> GetData(string procName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.lgstcm.FreightVoucherOutward> list;
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				list = GenericProvider.GetAllDetail<Architecture.Model.lgstcm.FreightVoucherOutward>(sqlConnection, procName, p);
			}
			if (list != null && list.Count == 1)
			{
				using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
				{
					sqlConnection2.Open();
					foreach (Architecture.Model.lgstcm.FreightVoucherOutward item2 in list)
					{
						List<SqlParameter> prm2 = new List<SqlParameter>
						{
							new SqlParameter("@Id", item2.FreightVoucherOutwardId),
							new SqlParameter("@Activity", "ReadByHeaderId_FreightVoucherOutwardPaymentDetail")
						};
						item2.FreightVoucherOutwardPaymentDetailList = GenericProvider.GetAllDetail<FreightVoucherOutwardPaymentDetail>(sqlConnection2, "[lgstcm].[USP_FreightVoucherOutward_GetAllMethod]", prm2);
					}
				}
				using SqlConnection sqlCon = ConnectionObject.SQL();
				sqlCon.Open();
				foreach (Architecture.Model.lgstcm.FreightVoucherOutward item in list)
				{
					List<SqlParameter> prm = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.FreightVoucherOutwardId),
						new SqlParameter("@Activity", "ReadByHeaderId_FreightVoucherOutwardExpenseDetail")
					};
					item.FreightVoucherOutwardExpenseDetailList = GenericProvider.GetAllDetail<FreightVoucherOutwardExpenseDetail>(sqlCon, "[lgstcm].[USP_FreightVoucherOutward_GetAllMethod]", prm);
				}
			}
		}
		catch
		{
			throw;
		}
		return list;
	}
}
