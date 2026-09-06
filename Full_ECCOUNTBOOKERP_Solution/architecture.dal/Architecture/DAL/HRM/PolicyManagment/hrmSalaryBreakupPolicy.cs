using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.PolicyManagment;

namespace Architecture.DAL.HRM.PolicyManagment;

public class hrmSalaryBreakupPolicy
{
	public int SetData(Architecture.Model.HRM.PolicyManagment.hrmSalaryBreakupPolicy obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.HRM.PolicyManagment.hrmSalaryBreakupPolicy item in obj.SalaryBreakupPolicieslist)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
			}
			if (returnID > 0)
			{
				obj.SalaryBreakupPolicyId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.SalaryBreakupPolicyId;
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

	public List<Architecture.Model.HRM.PolicyManagment.hrmSalaryBreakupPolicy> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.PolicyManagment.hrmSalaryBreakupPolicy> lst = new List<Architecture.Model.HRM.PolicyManagment.hrmSalaryBreakupPolicy>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<Architecture.Model.HRM.PolicyManagment.hrmSalaryBreakupPolicy>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
