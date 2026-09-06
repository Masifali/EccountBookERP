using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Lab;
using Architecture.Model.VPS;

namespace Architecture.DAL.Lab;

public class InvLabAnalysisPurchaseHeader
{
	public int SetData(Architecture.Model.Lab.InvLabAnalysisPurchaseHeader obj, string ProcName)
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
			foreach (InvLabAnalysisPurchaseDetail item2 in obj.LabSampleDetail)
			{
				item2.InvLabAnalysisPurchaseHeaderId = obj.Id;
				int Code = (item2.Id = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_InvLabAnalysisPurchaseDetail_Insert")));
				List<InvLabAnalysisPurchaseSubParamsDetail> invLabAnalysisPurchaseSubParamsDetailslist = item2.InvLabAnalysisPurchaseSubParamsDetailslist;
				if (invLabAnalysisPurchaseSubParamsDetailslist == null || invLabAnalysisPurchaseSubParamsDetailslist.Count <= 0)
				{
					continue;
				}
				foreach (InvLabAnalysisPurchaseSubParamsDetail sub in item2.InvLabAnalysisPurchaseSubParamsDetailslist)
				{
					sub.InvLabAnalysisPurchaseHeaderId = obj.Id;
					sub.InvLabAnalysisPurchaseDetailId = Code;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, sub, "Sp_InvLabAnalysisPurchaseSubParamsDetail_Insert"));
				}
			}
			if (obj.dmsAttachment != null && obj.dmsAttachment.Count > 0)
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
				foreach (DMSAttachments item in obj.dmsAttachment.ToList())
				{
					item.RefAccountId = obj.SupplierCustomerId;
					item.RefDocumentTypeId = obj.DocumentTypeId;
					item.RefDocumentNo = returnID;
					item.OrganizationId = obj.OrganizationId;
					item.CompanyId = obj.CompanyId;
					item.EntryUser = obj.EntryUser;
					item.EntryDate = DateTime.Now;
					item.ModifyDate = DateTime.Now;
					item.ModifyUser = obj.ModifyUser;
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

	public List<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader> lst = new List<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.Get<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst.Count > 0)
				{
					foreach (Architecture.Model.Lab.InvLabAnalysisPurchaseHeader item in lst)
					{
						List<SqlParameter> lstPrm = new List<SqlParameter>();
						lstPrm.Add(new SqlParameter("@Id", item.Id));
						lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
						item.LabSampleDetail = GenericProvider.Get<InvLabAnalysisPurchaseDetail>(sqlConnection2, "Sp_InvLabAnalysisPurchaseHeader_GetAllMethod", lstPrm);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst.Count > 0)
			{
				List<SqlParameter> lstsubParm = new List<SqlParameter>();
				lstsubParm.Add(new SqlParameter("@Id", lst[0].Id));
				lstsubParm.Add(new SqlParameter("@Activity", "ReadSubParamsDetailByHeaderId"));
				List<InvLabAnalysisPurchaseSubParamsDetail> sublst = GenericProvider.Get<InvLabAnalysisPurchaseSubParamsDetail>(sqlcon, "Sp_InvLabAnalysisPurchaseHeader_GetAllMethod", lstsubParm);
				foreach (InvLabAnalysisPurchaseDetail item2 in lst[0].LabSampleDetail)
				{
					item2.InvLabAnalysisPurchaseSubParamsDetailslist = sublst.FindAll((InvLabAnalysisPurchaseSubParamsDetail x) => x.InvParentParameterId == item2.InvLabAnalysisItemsId);
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
