using System;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Lab;

namespace Architecture.DAL.Lab;

public class InvLabSampleLogRegister
{
	public int SetDate(Architecture.Model.Lab.InvLabSampleLogRegister obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (DMSAttachments item in obj.dmsAttachment)
			{
				item.RefDocumentNo = obj.Id;
				item.RefAccountId = Conversion.ToInt(obj.SupplierCustomerId);
				item.RefDocumentTypeId = obj.DocumentTypeId;
				item.EntryDate = obj.EntryDate;
				item.EntryUser = obj.EntryUser;
				item.ModifyDate = obj.ModifyDate;
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
