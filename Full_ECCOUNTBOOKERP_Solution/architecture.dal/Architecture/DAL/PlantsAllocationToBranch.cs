using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class PlantsAllocationToBranch
{
	public int SetData(Architecture.Model.PlantsAllocationToBranch obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.PlantsAllocationToBranch item in obj.PlantsAllocationToBranchList)
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

	public List<Architecture.Model.PlantsAllocationToBranch> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.PlantsAllocationToBranch> lst = new List<Architecture.Model.PlantsAllocationToBranch>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.PlantsAllocationToBranch>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
