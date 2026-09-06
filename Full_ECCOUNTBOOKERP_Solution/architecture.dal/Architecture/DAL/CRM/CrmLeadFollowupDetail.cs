using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.CRM;

namespace Architecture.DAL.CRM;

public class CrmLeadFollowupDetail : DALBase
{
	public int SetData(Architecture.Model.CRM.CrmLeadFollowupDetail obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.LeadFollowupId = Convert.ToInt32(returnID);
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

	public List<Architecture.Model.CRM.CrmLeadFollowupDetail> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.CRM.CrmLeadFollowupDetail> lst = new List<Architecture.Model.CRM.CrmLeadFollowupDetail>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.CRM.CrmLeadFollowupDetail>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
