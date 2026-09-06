using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.PolicyManagment;

namespace Architecture.DAL.HRM.PolicyManagment;

public class hrmBenefit
{
	public int SetData(Architecture.Model.HRM.PolicyManagment.hrmBenefit obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.BenefitId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.BenefitId;
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

	public List<Architecture.Model.HRM.PolicyManagment.hrmBenefit> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.PolicyManagment.hrmBenefit> lst = new List<Architecture.Model.HRM.PolicyManagment.hrmBenefit>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.HRM.PolicyManagment.hrmBenefit>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
