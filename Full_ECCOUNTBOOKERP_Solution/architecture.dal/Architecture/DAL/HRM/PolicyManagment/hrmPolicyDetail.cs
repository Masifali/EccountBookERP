using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.PolicyManagment;

namespace Architecture.DAL.HRM.PolicyManagment;

public class hrmPolicyDetail
{
	public int SetData(Architecture.Model.HRM.PolicyManagment.hrmPolicyDetail obj, string ProcName)
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
				obj.PolicyId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.PolicyId;
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

	public List<Architecture.Model.HRM.PolicyManagment.hrmPolicyDetail> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.PolicyManagment.hrmPolicyDetail> lst = new List<Architecture.Model.HRM.PolicyManagment.hrmPolicyDetail>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.HRM.PolicyManagment.hrmPolicyDetail>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
