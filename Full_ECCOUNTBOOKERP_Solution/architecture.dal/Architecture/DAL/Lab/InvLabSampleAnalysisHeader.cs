using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Lab;
using Architecture.Model.VPS;

namespace Architecture.DAL.Lab;

public class InvLabSampleAnalysisHeader
{
	public int SetData(Architecture.Model.Lab.InvLabSampleAnalysisHeader obj, string ProcName)
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
				if (obj.dmsAttachment != null && obj.dmsAttachment.Count > 0 && !Directory.Exists(AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
			}
			if (obj.dmsAttachment != null && obj.dmsAttachment.Count > 0)
			{
				obj.dmsAttachment = CommonServices.CommonMethodForAttachmentsToPhysicallySave(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.dmsAttachment, copiedFiles);
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
			foreach (InvLabSampleAnalysisDetail item2 in obj.LabSampleDetail)
			{
				item2.InvLabSampleAnalysisHeaderId = obj.Id;
				int Code = (item2.Id = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_InvLabSampleAnalysisDetail_Insert")));
				List<InvLabSampleAnalysiSubParamsDetail> invLabSampleAnalysiSubParamsDetailsList = item2.InvLabSampleAnalysiSubParamsDetailsList;
				if (invLabSampleAnalysiSubParamsDetailsList == null || invLabSampleAnalysiSubParamsDetailsList.Count <= 0)
				{
					continue;
				}
				foreach (InvLabSampleAnalysiSubParamsDetail sub in item2.InvLabSampleAnalysiSubParamsDetailsList)
				{
					sub.InvLabSampleAnalysisHeaderId = obj.Id;
					sub.InvLabSampleAnalysisDetailId = Code;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, sub, "Sp_InvLabSampleAnalysiSubParamsDetail_Insert"));
				}
			}
			foreach (DMSAttachments item in obj.dmsAttachment)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenName",
					Value = obj.dmsAttachment[0].ScreenName
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
				item.RefDocumentNo = obj.Id;
				item.RefAccountId = obj.SupplierCustomerId;
				item.RefDocumentTypeId = obj.DocumentTypeId;
				item.EntryDate = DateTime.Now;
				item.EntryUser = obj.EntryUser;
				item.ModifyDate = DateTime.Now;
				item.ModifyUser = obj.ModifyUser;
				item.OrganizationId = obj.OrganizationId;
				item.CompanyId = obj.CompanyId;
				item.DMSFoldersLabelsId = 0;
				item.BranchId = 0;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Proc_DMSAttachments_Insert"));
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

	public List<Architecture.Model.Lab.InvLabSampleAnalysisHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Lab.InvLabSampleAnalysisHeader> lst = new List<Architecture.Model.Lab.InvLabSampleAnalysisHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Lab.InvLabSampleAnalysisHeader>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst.Count > 0)
				{
					foreach (Architecture.Model.Lab.InvLabSampleAnalysisHeader item in lst)
					{
						List<SqlParameter> lstPrm = new List<SqlParameter>();
						lstPrm.Add(new SqlParameter("@Id", item.Id));
						lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
						item.LabSampleDetail = GenericProvider.Get<InvLabSampleAnalysisDetail>(sqlConnection2, "Sp_InvLabSampleAnalysisHeader_GetAllMethod", lstPrm);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst.Count > 0)
			{
				List<SqlParameter> lstsubParm = new List<SqlParameter>();
				lstsubParm.Add(new SqlParameter("@Id", lst[0].Id));
				lstsubParm.Add(new SqlParameter("@Activity", "ReadLabSampleSubParamsDetailByHeaderId"));
				List<InvLabSampleAnalysiSubParamsDetail> sublst = GenericProvider.Get<InvLabSampleAnalysiSubParamsDetail>(sqlcon, "Sp_InvLabSampleAnalysisHeader_GetAllMethod", lstsubParm);
				foreach (InvLabSampleAnalysisDetail item2 in lst[0].LabSampleDetail)
				{
					item2.InvLabSampleAnalysiSubParamsDetailsList = sublst.FindAll((InvLabSampleAnalysiSubParamsDetail x) => x.InvParentParameterId == item2.InvLabAnalysisItemsId);
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
