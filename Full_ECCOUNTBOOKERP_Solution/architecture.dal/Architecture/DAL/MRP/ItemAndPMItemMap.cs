using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.MRP;
using Architecture.Model.VPS;

namespace Architecture.DAL.MRP;

public class ItemAndPMItemMap
{
	public int SetData(Architecture.Model.MRP.ItemAndPMItemMap obj, string ProcName)
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
			}
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.ItemAndPMItemMapId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.ItemAndPMItemMapId;
			}
			foreach (ItemAndPMItemMapDetail item2 in obj.ItemAndPMItemMapDetailList)
			{
				item2.ItemAndPMItemMapId = obj.ItemAndPMItemMapId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "[mrp].[USP_ItemAndPMItemMapDetail_Insert]"));
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
				foreach (DMSAttachments item in obj.AttachmentsList.ToList())
				{
					item.RefAccountId = obj.ItemId;
					item.RefDocumentTypeId = 0;
					item.RefDocumentNo = returnID;
					item.EntryDate = DateTime.Now;
					item.ModifyDate = DateTime.Now;
					item.EntryUser = obj.EntryUserId;
					item.ModifyUser = obj.ModifyUserId;
					item.OrganizationId = obj.OrganizationId;
					item.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Proc_DMSAttachments_Insert"));
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

	public List<Architecture.Model.MRP.ItemAndPMItemMap> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.MRP.ItemAndPMItemMap> lst = new List<Architecture.Model.MRP.ItemAndPMItemMap>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.MRP.ItemAndPMItemMap>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.MRP.ItemAndPMItemMap item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.ItemAndPMItemMapId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.ItemAndPMItemMapDetailList = GenericProvider.GetAllDetail<ItemAndPMItemMapDetail>(sqlcon, "[MRP].[usp_ItemAndPMItemMap_GetAllMethod]", lstPrm);
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
