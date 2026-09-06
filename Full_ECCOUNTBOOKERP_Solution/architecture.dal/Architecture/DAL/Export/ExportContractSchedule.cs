using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.VPS;

namespace Architecture.DAL.Export;

public class ExportContractSchedule : DALBase
{
	public int SetData(Architecture.Model.Export.ExportContractSchedule obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Export.ExportContractSchedule detail in obj.ExportContractScheduleList)
			{
				if (IsVpsAttachmentsServiceOn)
				{
					Sc = CommonServices.GetServerCredentials();
				}
				else
				{
					AttachmentFolderPath = CommonServices.GetConfigurationFromAllocation(detail.OrganizationId, detail.CompanyId, "Attachment Folder Path");
					if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0 && !Directory.Exists(AttachmentFolderPath))
					{
						throw new Exception("Attachment Folder Path Not Configure Properly");
					}
				}
				if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
				{
					obj.AttachmentsList = CommonServices.CommonMethodForAttachmentsToPhysicallySave(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.AttachmentsList, copiedFiles);
					detail.AttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.Attachment));
					detail.CustomAttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.UploadedFileCustomName));
				}
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, detail, "USP_ExportContractSchedule_InsertAndUpdate"));
			}
			if (obj.ExportContractScheduleList.Any((Architecture.Model.Export.ExportContractSchedule r) => r.Id > 0) && obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "Sp_DMSAttachments_GetAllMethod";
				sqlCommand.Parameters.AddWithValue("@Id", obj.ExportContractScheduleList[0].RecordIdentityId);
				sqlCommand.Parameters.AddWithValue("@ScreenName", obj.AttachmentsList[0].ScreenName);
				sqlCommand.Parameters.AddWithValue("@Activity", "DeleteById");
				sqlCommand.ExecuteNonQuery();
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				foreach (DMSAttachments item2 in obj.AttachmentsList.ToList())
				{
					item2.RefAccountId = obj.ExportContractScheduleList[0].ContractId;
					item2.RefDocumentTypeId = obj.ExportContractScheduleList[0].DocumentTypeId;
					item2.RefDocumentNo = obj.ExportContractScheduleList[0].RecordIdentityId;
					item2.EntryDate = DateTime.Now;
					item2.ModifyDate = DateTime.Now;
					item2.EntryUser = obj.ExportContractScheduleList[0].EntryUserId;
					item2.ModifyUser = obj.ExportContractScheduleList[0].ModifyUserId;
					item2.OrganizationId = obj.ExportContractScheduleList[0].OrganizationId;
					item2.CompanyId = obj.ExportContractScheduleList[0].CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Proc_DMSAttachments_Insert"));
				}
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

	public List<Architecture.Model.Export.ExportContractSchedule> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.ExportContractSchedule> lst = new List<Architecture.Model.Export.ExportContractSchedule>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.Get<Architecture.Model.Export.ExportContractSchedule>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
