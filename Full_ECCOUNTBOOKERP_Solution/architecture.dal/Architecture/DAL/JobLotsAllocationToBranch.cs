using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class JobLotsAllocationToBranch
{
	public int SetData(Architecture.Model.JobLotsAllocationToBranch obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.JobLotsAllocationToBranch item in obj.JobLotsAllocationToBranchList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public List<Architecture.Model.JobLotsAllocationToBranch> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.JobLotsAllocationToBranch> lst = new List<Architecture.Model.JobLotsAllocationToBranch>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.JobLotsAllocationToBranch>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
