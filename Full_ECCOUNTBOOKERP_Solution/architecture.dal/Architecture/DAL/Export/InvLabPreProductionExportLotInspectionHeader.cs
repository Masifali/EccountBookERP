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

public class InvLabPreProductionExportLotInspectionHeader
{
	public int SetData(Architecture.Model.Export.InvLabPreProductionExportLotInspectionHeader obj, string ProcName, string RemoveDetailIds = null)
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
			if (!string.IsNullOrEmpty(RemoveDetailIds))
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "usp_InvLabPreProductionExportLotInspectionDetailDeletebyIds";
				sqlCommand.Parameters.AddWithValue("@Id", obj.Id);
				sqlCommand.Parameters.AddWithValue("@DetailIds", RemoveDetailIds);
				sqlCommand.ExecuteNonQuery();
			}
			foreach (InvLabPreProductionExportLotInspectionDetail item5 in obj.LotInspectionDetails)
			{
				item5.InvLabPreProductionExportLotInspectionHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "Sp_InvLabPreProductionExportLotInspectionDetail_Insert"));
			}
			if (obj.InvLabPreProductionExportLotInspectionParametersDetailList != null && obj.InvLabPreProductionExportLotInspectionParametersDetailList.Count > 0)
			{
				foreach (InvLabPreProductionExportLotInspectionParametersDetail item4 in obj.InvLabPreProductionExportLotInspectionParametersDetailList)
				{
					item4.InvLabPreProductionExportLotInspectionHeaderId = obj.Id;
					item4.RequiredAnalysisGroupId = obj.RequiredAnalysisId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "USP_InvLabPreProductionExportLotInspectionParametersDetail_Insert"));
				}
			}
			if (obj.ThirdPartyInspectionLotSamplingDetailList != null && obj.ThirdPartyInspectionLotSamplingDetailList.Count > 0)
			{
				foreach (ThirdPartyInspectionLotSamplingDetail item3 in obj.ThirdPartyInspectionLotSamplingDetailList)
				{
					item3.InvLabPreProductionExportLotInspectionHeaderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "[dbo].[USP_ThirdPartyInspectionLotSamplingDetail_Insert]"));
				}
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
					item2.RefAccountId = obj.EntryUserId;
					item2.RefDocumentTypeId = 0;
					item2.ScreenName = obj.ScreenName;
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

	public List<Architecture.Model.Export.InvLabPreProductionExportLotInspectionHeader> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.InvLabPreProductionExportLotInspectionHeader> lst = new List<Architecture.Model.Export.InvLabPreProductionExportLotInspectionHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetAllDetail<Architecture.Model.Export.InvLabPreProductionExportLotInspectionHeader>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.InvLabPreProductionExportLotInspectionHeader item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@InvLabPreProductionExportLotInspectionHeaderId", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
						item3.LotInspectionDetails = GenericProvider.GetAllDetail<InvLabPreProductionExportLotInspectionDetail>(sqlConnection2, "Sp_InvLabPreProductionExportLotInspectionHeader_GetAllMethod", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.InvLabPreProductionExportLotInspectionHeader item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@InvLabPreProductionExportLotInspectionHeaderId", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadByHeaderId_ParameterDetail"));
						item2.InvLabPreProductionExportLotInspectionParametersDetailList = GenericProvider.GetAllDetail<InvLabPreProductionExportLotInspectionParametersDetail>(sqlConnection3, "Sp_InvLabPreProductionExportLotInspectionHeader_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Export.InvLabPreProductionExportLotInspectionHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@InvLabPreProductionExportLotInspectionHeaderId", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId_ThirdPartyInspectionLotSamplingDetail"));
					item.ThirdPartyInspectionLotSamplingDetailList = GenericProvider.GetAllDetail<ThirdPartyInspectionLotSamplingDetail>(sqlcon, "Sp_InvLabPreProductionExportLotInspectionHeader_GetAllMethod", lstPrm);
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
