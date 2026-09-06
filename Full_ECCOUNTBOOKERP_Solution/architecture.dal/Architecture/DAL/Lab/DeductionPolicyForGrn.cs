using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Lab;

namespace Architecture.DAL.Lab;

public class DeductionPolicyForGrn
{
	public int SetDate(Architecture.Model.Lab.DeductionPolicyForGrn obj, string ProcName)
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
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Lab.DeductionPolicyForGrn> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Lab.DeductionPolicyForGrn> lst = new List<Architecture.Model.Lab.DeductionPolicyForGrn>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.Get<Architecture.Model.Lab.DeductionPolicyForGrn>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
