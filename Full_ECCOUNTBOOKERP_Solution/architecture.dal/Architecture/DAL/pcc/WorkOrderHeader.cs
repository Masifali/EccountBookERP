using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.VPS;
using Architecture.Model.pcc;

namespace Architecture.DAL.pcc;

public class WorkOrderHeader
{
	public int SetData(Architecture.Model.pcc.WorkOrderHeader obj, string procName)
	{
		if (obj.WorkOrderCustomerDetailList == null || obj.WorkOrderCustomerDetailList.Count == 0)
		{
			throw new Exception("Detail list not found");
		}
		int returnID = 0;
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
				obj.WorkOrderHeaderId = ((returnID > 0) ? returnID : obj.WorkOrderHeaderId);
				int lineId = 1;
				foreach (WorkOrderCustomerDetail detail2 in obj.WorkOrderCustomerDetailList)
				{
					detail2.WorkOrderHeaderId = obj.WorkOrderHeaderId;
					detail2.LineId = lineId++;
					GenericProvider.SetProc(sqlTrn, detail2, "[pcc].[USP_WorkOrderCustomerDetail_Insert]");
				}
				lineId = 1;
				foreach (WorkOrderInputDetail detail in obj.WorkOrderInputDetailList)
				{
					detail.WorkOrderHeaderId = obj.WorkOrderHeaderId;
					detail.SortNo = lineId++;
					GenericProvider.SetProc(sqlTrn, detail, "[pcc].[USP_WorkOrderInputDetail_Insert]");
				}
				List<DMSAttachments> attachmentsList3 = obj.AttachmentsList;
				if (attachmentsList3 != null && attachmentsList3.Count > 0)
				{
					List<SqlParameter> p = new List<SqlParameter>
					{
						new SqlParameter("@ScreenName", obj.AttachmentsList[0].ScreenName),
						new SqlParameter("@Id", obj.WorkOrderHeaderId),
						new SqlParameter("@Activity", "DeleteById")
					};
					new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
					foreach (DMSAttachments att in obj.AttachmentsList)
					{
						att.RefAccountId = obj.EntryUserId;
						att.RefDocumentTypeId = obj.DocumentTypeId;
						att.RefDocumentNo = obj.WorkOrderHeaderId;
						att.EntryUser = obj.EntryUserId;
						att.ModifyUser = obj.ModifyUserId;
						att.EntryDate = DateTime.Now;
						att.ModifyDate = DateTime.Now;
						att.OrganizationId = obj.OrganizationId;
						att.CompanyId = obj.CompanyId;
						GenericProvider.SetProc(sqlTrn, att, "Proc_DMSAttachments_Insert");
					}
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
		return obj.WorkOrderHeaderId;
	}

	public List<Architecture.Model.pcc.WorkOrderHeader> GetData(string procName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.pcc.WorkOrderHeader> list;
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				list = GenericProvider.GetProc<Architecture.Model.pcc.WorkOrderHeader>(sqlConnection, procName, p);
			}
			if (list != null && list.Count > 0)
			{
				using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
				{
					sqlConnection2.Open();
					foreach (Architecture.Model.pcc.WorkOrderHeader item2 in list)
					{
						List<SqlParameter> prm2 = new List<SqlParameter>
						{
							new SqlParameter("@Id", item2.WorkOrderHeaderId),
							new SqlParameter("@Activity", "ReadByHeaderId_WorkOrderCustomerDetail")
						};
						item2.WorkOrderCustomerDetailList = GenericProvider.GetAllDetail<WorkOrderCustomerDetail>(sqlConnection2, "[pcc].[USP_WorkOrderHeader_GetAllMethod]", prm2);
					}
				}
				using SqlConnection sqlCon = ConnectionObject.SQL();
				sqlCon.Open();
				foreach (Architecture.Model.pcc.WorkOrderHeader item in list)
				{
					List<SqlParameter> prm = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.WorkOrderHeaderId),
						new SqlParameter("@Activity", "ReadByHeaderId_WorkOrderInputDetail")
					};
					item.WorkOrderInputDetailList = GenericProvider.GetAllDetail<WorkOrderInputDetail>(sqlCon, "[pcc].[USP_WorkOrderHeader_GetAllMethod]", prm);
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
