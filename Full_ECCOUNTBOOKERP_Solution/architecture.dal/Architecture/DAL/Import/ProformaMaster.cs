using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Import;
using Architecture.Model.VPS;

namespace Architecture.DAL.Import;

public class ProformaMaster : DALBase
{
	public long SetDate(Architecture.Model.Import.ProformaMaster obj, string ProcName, string Activity)
	{
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
				obj.AttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.Attachment));
				obj.CustomAttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.UploadedFileCustomName));
			}
			long returnID = Convert.ToInt64(GenericProvider.SetProc(sqlTrn, obj, ProcName, Activity));
			if (returnID > 0)
			{
				obj.proformaMasterId = returnID;
			}
			else
			{
				returnID = obj.proformaMasterId;
			}
			foreach (ProformaDetail item3 in obj.ProformaDetailList)
			{
				item3.proformaMasterId = obj.proformaMasterId;
				string DetailActivity = ((item3.appActionId == 2) ? "UPDATE" : ((item3.appActionId == 1) ? "INSERT" : ((item3.appActionId == 3) ? "DELETE" : "")));
				Convert.ToInt64(GenericProvider.SetProc(sqlTrn, item3, "[ImEx].[usp_Set_ProformaDetail]", DetailActivity));
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
				foreach (DMSAttachments item2 in obj.AttachmentsList.ToList())
				{
					item2.RefAccountId = Conversion.ToInt(obj.exporterId);
					item2.RefDocumentTypeId = obj.documentTypeId;
					item2.RefDocumentNo = Conversion.ToInt(returnID);
					item2.EntryDate = DateTime.Now;
					item2.ModifyDate = DateTime.Now;
					item2.EntryUser = Conversion.ToInt(obj.createdUserId);
					item2.ModifyUser = Conversion.ToInt(obj.lastModifiedUserId);
					item2.OrganizationId = obj.OrganizationId;
					item2.CompanyId = obj.CompanyId;
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

	public List<Architecture.Model.Import.ProformaMaster> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Import.ProformaMaster> lst = new List<Architecture.Model.Import.ProformaMaster>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Import.ProformaMaster>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Import.ProformaMaster item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@proformaMasterId", item.proformaMasterId));
					lstPrm.Add(new SqlParameter("@Activity", "SEARCH"));
					item.ProformaDetailList = GenericProvider.Get<ProformaDetail>(sqlcon, "[ImEx].[usp_Get_ProformaDetail]", lstPrm);
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
