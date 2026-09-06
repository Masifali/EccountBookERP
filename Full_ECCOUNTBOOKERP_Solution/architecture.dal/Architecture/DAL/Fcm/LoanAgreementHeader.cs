using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Fcm;
using Architecture.Model.VPS;

namespace Architecture.DAL.Fcm;

public class LoanAgreementHeader
{
	public int SetData(Architecture.Model.Fcm.LoanAgreementHeader obj, string ProcName)
	{
		int returnID = 0;
		List<string> copiedFiles = new List<string>();
		string attachmentFolderPath = "";
		ServerCredentials serverCredentials = new ServerCredentials();
		bool isVpsAttachmentServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		using SqlConnection sqlcon = ConnectionObject.SQL();
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
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = Conversion.ToInt(obj.Id);
			}
			foreach (LoanAgreementDetail item in obj.LoanAgreementDetailsList)
			{
				item.LoanAgreementHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "[Fcm].[USP_LoanAgreementDetail_Insert]"));
			}
			List<DMSAttachments> attachmentsList3 = obj.AttachmentsList;
			if (attachmentsList3 != null && attachmentsList3.Count > 0)
			{
				List<SqlParameter> p = new List<SqlParameter>
				{
					new SqlParameter("@ScreenName", obj.AttachmentsList[0].ScreenName),
					new SqlParameter("@Id", returnID),
					new SqlParameter("@Activity", "DeleteById")
				};
				new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
				foreach (DMSAttachments att in obj.AttachmentsList)
				{
					att.RefAccountId = obj.EntryUserId;
					att.RefDocumentTypeId = obj.DocumentTypeId;
					att.RefDocumentNo = returnID;
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
			return returnID;
		}
		catch
		{
			sqlTrn.Rollback();
			CommonServices.DeleteAttachmentsPhysicallyInException(serverCredentials, isVpsAttachmentServiceOn, attachmentFolderPath, copiedFiles);
			throw;
		}
	}

	public List<Architecture.Model.Fcm.LoanAgreementHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Fcm.LoanAgreementHeader> lst = new List<Architecture.Model.Fcm.LoanAgreementHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Fcm.LoanAgreementHeader>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Fcm.LoanAgreementHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
					item.LoanAgreementDetailsList = GenericProvider.Get<LoanAgreementDetail>(sqlcon, "[Fcm].[USP_LoanAgreementHeader_GetAllMethod]", lstPrm);
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
