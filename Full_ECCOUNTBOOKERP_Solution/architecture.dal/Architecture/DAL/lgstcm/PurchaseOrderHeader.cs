using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.VPS;
using Architecture.Model.lgstcm;

namespace Architecture.DAL.lgstcm;

public class PurchaseOrderHeader
{
	public int SetData(Architecture.Model.lgstcm.PurchaseOrderHeader obj, string procName)
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
				List<PurchaseOrderDetail> purchaseOrderDetailList = obj.PurchaseOrderDetailList;
				if (purchaseOrderDetailList != null && purchaseOrderDetailList.Count == 0)
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
				obj.PurchaseOrderHeaderId = ((returnID > 0) ? returnID : obj.PurchaseOrderHeaderId);
				foreach (PurchaseOrderDetail detail in obj.PurchaseOrderDetailList)
				{
					detail.PurchaseOrderHeaderId = obj.PurchaseOrderHeaderId;
					GenericProvider.SetProc(sqlTrn, detail, "[lgstcm].[USP_PurchaseOrderDetail_Insert]");
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
					if (obj.PurchaseOrderHeaderId > 0)
					{
						cmd.Parameters.AddWithValue("@CurrentDocId", obj.PurchaseOrderHeaderId);
					}
					cmd.ExecuteNonQuery();
				}
				List<DMSAttachments> attachmentsList3 = obj.AttachmentsList;
				if (attachmentsList3 != null && attachmentsList3.Count > 0)
				{
					List<SqlParameter> p = new List<SqlParameter>
					{
						new SqlParameter("@ScreenName", obj.AttachmentsList[0].ScreenName),
						new SqlParameter("@Id", obj.PurchaseOrderHeaderId),
						new SqlParameter("@Activity", "DeleteById")
					};
					new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
					foreach (DMSAttachments att in obj.AttachmentsList)
					{
						att.RefAccountId = obj.EntryUserId;
						att.RefDocumentTypeId = obj.DocumentTypeId;
						att.RefDocumentNo = obj.PurchaseOrderHeaderId;
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
		return obj.PurchaseOrderHeaderId;
	}

	public List<Architecture.Model.lgstcm.PurchaseOrderHeader> GetData(string procName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.lgstcm.PurchaseOrderHeader> list;
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				list = GenericProvider.GetAllDetail<Architecture.Model.lgstcm.PurchaseOrderHeader>(sqlConnection, procName, p);
			}
			if (list != null && list.Count > 0)
			{
				using SqlConnection sqlCon = ConnectionObject.SQL();
				sqlCon.Open();
				foreach (Architecture.Model.lgstcm.PurchaseOrderHeader item in list)
				{
					List<SqlParameter> prm = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.PurchaseOrderHeaderId),
						new SqlParameter("@Activity", "ReadByHeaderId_PurchaseOrderDetail")
					};
					item.PurchaseOrderDetailList = GenericProvider.GetAllDetail<PurchaseOrderDetail>(sqlCon, "[lgstcm].[USP_PurchaseOrderHeader_GetAllMethod]", prm);
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
