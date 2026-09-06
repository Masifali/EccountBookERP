using System;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Lab;

namespace Architecture.DAL.Lab;

public class InvLabAnalysisInProcessHeader
{
	public int SetData(Architecture.Model.Lab.InvLabAnalysisInProcessHeader obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (InvLabAnalysisInProcessDetail item2 in obj.LabSampleDetail)
			{
				item2.InvLabAnalysisInProcessHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_InvLabAnalysisInProcessDetail_Insert"));
			}
			foreach (DMSAttachments item in obj.dmsAttachment)
			{
				item.RefDocumentNo = obj.Id;
				item.RefAccountId = Conversion.ToInt(0);
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
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}
}
