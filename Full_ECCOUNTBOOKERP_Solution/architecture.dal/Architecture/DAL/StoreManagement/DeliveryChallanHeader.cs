using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.StoreManagement;
using Architecture.Model.VPS;

namespace Architecture.DAL.StoreManagement;

public class DeliveryChallanHeader
{
	public int SetData(Architecture.Model.StoreManagement.DeliveryChallanHeader obj, string procName)
	{
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
				List<DeliveryChallanDetail> deliveryChallanDetailList = obj.DeliveryChallanDetailList;
				if (deliveryChallanDetailList != null && deliveryChallanDetailList.Count == 0)
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
				obj.DeliveryChallanHeaderId = ((returnID > 0) ? returnID : obj.DeliveryChallanHeaderId);
				foreach (DeliveryChallanDetail detail2 in obj.DeliveryChallanDetailList)
				{
					detail2.DeliveryChallanHeaderId = obj.DeliveryChallanHeaderId;
					GenericProvider.SetProc(sqlTrn, detail2, "[dbo].[USP_DeliveryChallanDetail_Insert]");
				}
				foreach (DeliveryChallanExpenseDetail detail in obj.DeliveryChallanExpenseDetailList)
				{
					detail.DeliveryChallanHeaderId = obj.DeliveryChallanHeaderId;
					GenericProvider.SetProc(sqlTrn, detail, "[dbo].[USP_DeliveryChallanExpenseDetail_Insert]");
				}
				List<DMSAttachments> attachmentsList3 = obj.AttachmentsList;
				if (attachmentsList3 != null && attachmentsList3.Count > 0)
				{
					List<SqlParameter> p = new List<SqlParameter>
					{
						new SqlParameter("@ScreenName", obj.AttachmentsList[0].ScreenName),
						new SqlParameter("@Id", obj.DeliveryChallanHeaderId),
						new SqlParameter("@Activity", "DeleteById")
					};
					new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
					foreach (DMSAttachments att in obj.AttachmentsList)
					{
						att.RefAccountId = obj.EntryUserId;
						att.RefDocumentTypeId = obj.DocumentTypeId;
						att.RefDocumentNo = obj.DeliveryChallanHeaderId;
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
		return obj.DeliveryChallanHeaderId;
	}

	public List<Architecture.Model.StoreManagement.DeliveryChallanHeader> GetData(string procName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.StoreManagement.DeliveryChallanHeader> list;
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				list = GenericProvider.GetAllDetail<Architecture.Model.StoreManagement.DeliveryChallanHeader>(sqlConnection, procName, p);
			}
			if (list != null && list.Count > 0)
			{
				using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
				{
					sqlConnection2.Open();
					foreach (Architecture.Model.StoreManagement.DeliveryChallanHeader item2 in list)
					{
						List<SqlParameter> prm2 = new List<SqlParameter>
						{
							new SqlParameter("@Id", item2.DeliveryChallanHeaderId),
							new SqlParameter("@Activity", "ReadByHeaderId_DeliveryChallanDetail")
						};
						item2.DeliveryChallanDetailList = GenericProvider.GetAllDetail<DeliveryChallanDetail>(sqlConnection2, "[dbo].[USP_DeliveryChallanHeader_GetAllMethod]", prm2);
					}
				}
				using SqlConnection sqlCon = ConnectionObject.SQL();
				sqlCon.Open();
				foreach (Architecture.Model.StoreManagement.DeliveryChallanHeader item in list)
				{
					List<SqlParameter> prm = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.DeliveryChallanHeaderId),
						new SqlParameter("@Activity", "ReadByHeaderId_DeliveryChallanExpenseDetail")
					};
					item.DeliveryChallanExpenseDetailList = GenericProvider.GetAllDetail<DeliveryChallanExpenseDetail>(sqlCon, "[dbo].[USP_DeliveryChallanHeader_GetAllMethod]", prm);
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
