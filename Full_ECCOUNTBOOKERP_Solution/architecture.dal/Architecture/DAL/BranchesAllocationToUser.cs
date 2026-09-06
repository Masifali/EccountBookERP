using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class BranchesAllocationToUser
{
	public int SetData(Architecture.Model.BranchesAllocationToUser obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.BranchesAllocationToUser item in obj.BranchesAllocationToUserList)
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

	public List<Architecture.Model.BranchesAllocationToUser> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.BranchesAllocationToUser> lst = new List<Architecture.Model.BranchesAllocationToUser>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.BranchesAllocationToUser>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
