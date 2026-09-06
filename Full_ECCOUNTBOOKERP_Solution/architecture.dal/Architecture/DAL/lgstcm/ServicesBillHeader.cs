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

public class ServicesBillHeader
{
	public int SetData(Architecture.Model.lgstcm.ServicesBillHeader obj, string procName)
	{
		int returnID = 0;
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
		List<string> copiedFiles = new List<string>();
		string attachmentFolderPath = "";
		ServerCredentials serverCredentials = new ServerCredentials();
		bool isVpsAttachmentServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		using (SqlConnection sqlcon = ConnectionObject.SQL())
		{
			sqlcon.Open();
			SqlTransaction sqlTrn = sqlcon.BeginTransaction();
			try
			{
				List<ServicesBillDetail> servicesBillDetailList = obj.ServicesBillDetailList;
				if (servicesBillDetailList != null && servicesBillDetailList.Count == 0)
				{
					throw new Exception("Detail list not found");
				}
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
				obj.ServicesBillHeaderId = ((returnID > 0) ? returnID : obj.ServicesBillHeaderId);
				foreach (ServicesBillDetail detail2 in obj.ServicesBillDetailList)
				{
					detail2.ServicesBillHeaderId = obj.ServicesBillHeaderId;
					GenericProvider.SetProc(sqlTrn, detail2, "[lgstcm].[USP_ServicesBillDetail_Insert]");
				}
				foreach (ServicesBillFreightVoucherDetail detail in obj.ServicesBillFreightVoucherDetailList)
				{
					detail.ServicesBillHeaderId = obj.ServicesBillHeaderId;
					detail.ExImInvoiceId = obj.ExportInvoiceId;
					GenericProvider.SetProc(sqlTrn, detail, "[lgstcm].[USP_ServicesBillFreightVoucherDetail_Insert]");
				}
				if (obj.ExportInvoiceId > 0)
				{
					SqlCommand cmd = new SqlCommand
					{
						Connection = sqlcon,
						Transaction = sqlTrn,
						CommandType = CommandType.StoredProcedure,
						CommandText = "[lgstcm].[USP_Purchase_ValidateWithInvoice]"
					};
					cmd.Parameters.AddWithValue("@ExportInvoiceId", obj.ExportInvoiceId);
					cmd.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
					if (obj.ServicesBillHeaderId > 0)
					{
						cmd.Parameters.AddWithValue("@CurrentDocId", obj.ServicesBillHeaderId);
					}
					cmd.ExecuteNonQuery();
				}
				List<DMSAttachments> attachmentsList3 = obj.AttachmentsList;
				if (attachmentsList3 != null && attachmentsList3.Count > 0)
				{
					List<SqlParameter> p = new List<SqlParameter>
					{
						new SqlParameter("@ScreenName", obj.AttachmentsList[0].ScreenName),
						new SqlParameter("@Id", obj.ServicesBillHeaderId),
						new SqlParameter("@Activity", "DeleteById")
					};
					new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
					foreach (DMSAttachments att in obj.AttachmentsList)
					{
						att.RefAccountId = obj.EntryUserId;
						att.RefDocumentTypeId = obj.DocumentTypeId;
						att.RefDocumentNo = obj.ServicesBillHeaderId;
						att.EntryUser = obj.EntryUserId;
						att.ModifyUser = obj.ModifyUserId;
						att.EntryDate = DateTime.Now;
						att.ModifyDate = DateTime.Now;
						att.OrganizationId = obj.OrganizationId;
						att.CompanyId = obj.CompanyId;
						GenericProvider.SetProc(sqlTrn, att, "Proc_DMSAttachments_Insert");
					}
				}
				DataTable dtvh = CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.ServicesBillHeaderId);
				if (dtvh.Rows.Count > 0)
				{
					VoucherHeadIdGet = Conversion.ToInt(dtvh.Rows[0]["Id"]);
					obj.VoucherHeadInvoices.Id = VoucherHeadIdGet;
				}
				if (VoucherHeadIdGet == 0)
				{
					obj.VoucherHeadInvoices.DocumentTypeSrNo = obj.ServicesBillHeaderId;
					obj.VoucherHeadInvoices.RefDocNoId = obj.ServicesBillHeaderId;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_Insert"));
				}
				else
				{
					obj.VoucherHeadInvoices.DocumentTypeSrNo = obj.ServicesBillHeaderId;
					obj.VoucherHeadInvoices.RefDocNoId = obj.ServicesBillHeaderId;
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
				if (obj.VoucherHeadInvoices.voucherDetailList == null || obj.VoucherHeadInvoices.voucherDetailList.Count <= 0)
				{
					throw new Exception("VoucherDetail List Not Found");
				}
				foreach (VoucherDetail item2 in obj.VoucherHeadInvoices.voucherDetailList)
				{
					item2.VoucherHeadId = obj.VoucherHeadInvoices.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_Insert"));
				}
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_H_Insert"));
				foreach (VoucherDetail item in obj.VoucherHeadInvoices.voucherDetailList)
				{
					item.VoucherHeadId = obj.VoucherHeadInvoices.Id;
					item.DocumentTypeIdRef = Code;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_H_Insert"));
				}
				List<DMSAttachments> deleteAttachmentsList = obj.DeleteAttachmentsList;
				if (deleteAttachmentsList != null && deleteAttachmentsList.Count > 0)
				{
					CommonServices.DeleteAttachmentsPhysically(serverCredentials, isVpsAttachmentServiceOn, attachmentFolderPath, obj.DeleteAttachmentsList);
				}
				sqlTrn.Commit();
			}
			catch
			{
				sqlTrn.Rollback();
				CommonServices.DeleteAttachmentsPhysicallyInException(serverCredentials, isVpsAttachmentServiceOn, attachmentFolderPath, copiedFiles);
				throw;
			}
		}
		return obj.ServicesBillHeaderId;
	}

	public List<Architecture.Model.lgstcm.ServicesBillHeader> GetData(string procName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.lgstcm.ServicesBillHeader> list;
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				list = GenericProvider.GetAllDetail<Architecture.Model.lgstcm.ServicesBillHeader>(sqlConnection, procName, p);
			}
			if (list != null && list.Count > 0)
			{
				using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
				{
					sqlConnection2.Open();
					foreach (Architecture.Model.lgstcm.ServicesBillHeader item2 in list)
					{
						List<SqlParameter> prm2 = new List<SqlParameter>
						{
							new SqlParameter("@Id", item2.ServicesBillHeaderId),
							new SqlParameter("@Activity", "ReadByHeaderId_ServicesBillDetail")
						};
						item2.ServicesBillDetailList = GenericProvider.GetAllDetail<ServicesBillDetail>(sqlConnection2, "[lgstcm].[USP_ServicesBillHeader_GetAllMethod]", prm2);
					}
				}
				using SqlConnection sqlCon = ConnectionObject.SQL();
				sqlCon.Open();
				foreach (Architecture.Model.lgstcm.ServicesBillHeader item in list)
				{
					List<SqlParameter> prm = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.ServicesBillHeaderId),
						new SqlParameter("@Activity", "ReadByHeaderId_ServicesBillFreightVoucherDetail")
					};
					item.ServicesBillFreightVoucherDetailList = GenericProvider.GetAllDetail<ServicesBillFreightVoucherDetail>(sqlCon, "[lgstcm].[USP_ServicesBillHeader_GetAllMethod]", prm);
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
