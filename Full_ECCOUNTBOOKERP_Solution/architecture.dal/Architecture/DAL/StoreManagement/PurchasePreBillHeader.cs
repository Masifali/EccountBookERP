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

public class PurchasePreBillHeader
{
	public int SetData(Architecture.Model.StoreManagement.PurchasePreBillHeader obj, string procName)
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
				List<PurchasePreBillDetail> purchasePreBillDetailList = obj.PurchasePreBillDetailList;
				if (purchasePreBillDetailList != null && purchasePreBillDetailList.Count == 0)
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
				obj.PurchasePreBillHeaderId = ((returnID > 0) ? returnID : obj.PurchasePreBillHeaderId);
				foreach (PurchasePreBillDetail detail2 in obj.PurchasePreBillDetailList)
				{
					detail2.PurchasePreBillHeaderId = obj.PurchasePreBillHeaderId;
					GenericProvider.SetProc(sqlTrn, detail2, "[dbo].[USP_PurchasePreBillDetail_Insert]");
				}
				foreach (PurchasePreBillExpenseDetail detail in obj.PurchasePreBillExpenseDetailList)
				{
					detail.PurchasePreBillHeaderId = obj.PurchasePreBillHeaderId;
					GenericProvider.SetProc(sqlTrn, detail, "[dbo].[USP_PurchasePreBillExpenseDetail_Insert]");
				}
				List<DMSAttachments> attachmentsList3 = obj.AttachmentsList;
				if (attachmentsList3 != null && attachmentsList3.Count > 0)
				{
					List<SqlParameter> p = new List<SqlParameter>
					{
						new SqlParameter("@ScreenName", obj.AttachmentsList[0].ScreenName),
						new SqlParameter("@Id", obj.PurchasePreBillHeaderId),
						new SqlParameter("@Activity", "DeleteById")
					};
					new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
					foreach (DMSAttachments att in obj.AttachmentsList)
					{
						att.RefAccountId = obj.EntryUserId;
						att.RefDocumentTypeId = obj.DocumentTypeId;
						att.RefDocumentNo = obj.PurchasePreBillHeaderId;
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
		return obj.PurchasePreBillHeaderId;
	}

	public List<Architecture.Model.StoreManagement.PurchasePreBillHeader> GetData(string procName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.StoreManagement.PurchasePreBillHeader> list;
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				list = GenericProvider.GetAllDetail<Architecture.Model.StoreManagement.PurchasePreBillHeader>(sqlConnection, procName, p);
			}
			if (list != null && list.Count > 0)
			{
				using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
				{
					sqlConnection2.Open();
					foreach (Architecture.Model.StoreManagement.PurchasePreBillHeader item2 in list)
					{
						List<SqlParameter> prm2 = new List<SqlParameter>
						{
							new SqlParameter("@Id", item2.PurchasePreBillHeaderId),
							new SqlParameter("@Activity", "ReadByHeaderId_PurchasePreBillDetail")
						};
						item2.PurchasePreBillDetailList = GenericProvider.GetAllDetail<PurchasePreBillDetail>(sqlConnection2, "[dbo].[USP_PurchasePreBillHeader_GetAllMethod]", prm2);
					}
				}
				using SqlConnection sqlCon = ConnectionObject.SQL();
				sqlCon.Open();
				foreach (Architecture.Model.StoreManagement.PurchasePreBillHeader item in list)
				{
					List<SqlParameter> prm = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.PurchasePreBillHeaderId),
						new SqlParameter("@Activity", "ReadByHeaderId_PurchasePreBillExpenseDetail")
					};
					item.PurchasePreBillExpenseDetailList = GenericProvider.GetAllDetail<PurchasePreBillExpenseDetail>(sqlCon, "[dbo].[USP_PurchasePreBillHeader_GetAllMethod]", prm);
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
