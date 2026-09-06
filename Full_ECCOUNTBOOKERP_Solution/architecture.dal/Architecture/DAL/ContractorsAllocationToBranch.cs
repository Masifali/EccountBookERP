using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class ContractorsAllocationToBranch
{
	public int SetData(Architecture.Model.ContractorsAllocationToBranch obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.ContractorsAllocationToBranch item in obj.ContractorsAllocationToBranchList)
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

	public List<Architecture.Model.ContractorsAllocationToBranch> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ContractorsAllocationToBranch> lst = new List<Architecture.Model.ContractorsAllocationToBranch>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.ContractorsAllocationToBranch>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
