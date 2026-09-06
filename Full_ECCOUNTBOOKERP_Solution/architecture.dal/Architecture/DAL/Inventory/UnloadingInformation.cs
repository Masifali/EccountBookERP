using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.VPS;

namespace Architecture.DAL.Inventory;

public class UnloadingInformation
{
	public int SetData(Architecture.Model.Inventory.UnloadingInformation obj, string ProcName)
	{
		int returnID = 0;
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.UnloadingInformationDetailList == null || obj.UnloadingInformationDetailList.Count <= 0)
			{
				throw new Exception("Detail list not found");
			}
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
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (UnloadingInformationDetail item5 in obj.UnloadingInformationDetailList)
			{
				item5.UnloadingInformationId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "[dbo].[USP_UnloadingInformationDetail_Insert]"));
			}
			foreach (UnloadingInformationWagesDetail item4 in obj.UnloadingInformationWagesDetailList)
			{
				item4.UnloadingInformationId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "USP_UnloadingInformationWagesDetail_Insert"));
			}
			foreach (UnloadingInformationEmptyBagDetail item3 in obj.UnloadingInformationEmptyBagDetailList)
			{
				item3.UnloadingInformationId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "USP_UnloadingInformationEmptyBagDetail_Insert"));
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
					item2.RefAccountId = returnID;
					item2.RefDocumentTypeId = obj.DocumentTypeId;
					item2.RefDocumentNo = returnID;
					item2.EntryDate = DateTime.Now;
					item2.ModifyDate = DateTime.Now;
					item2.EntryUser = obj.EntryUserId;
					item2.ModifyUser = obj.ModifyUserId;
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

	public List<Architecture.Model.Inventory.UnloadingInformation> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.UnloadingInformation> lst = new List<Architecture.Model.Inventory.UnloadingInformation>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Inventory.UnloadingInformation>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.UnloadingInformation item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
						item3.UnloadingInformationDetailList = GenericProvider.GetAllDetail<UnloadingInformationDetail>(sqlConnection2, "[dbo].[USP_UnloadingInformation_GetAllMethod]", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.UnloadingInformation item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadByHeaderId_UnloadingInformationWagesDetail"));
						item2.UnloadingInformationWagesDetailList = GenericProvider.GetAllDetail<UnloadingInformationWagesDetail>(sqlConnection3, "USP_UnloadingInformation_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Inventory.UnloadingInformation item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId_UnloadingInformationEmptyBagDetail"));
					item.UnloadingInformationEmptyBagDetailList = GenericProvider.Get<UnloadingInformationEmptyBagDetail>(sqlcon, "USP_UnloadingInformation_GetAllMethod", lstPrm);
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
