using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class WarehousesAllocationToBranch
{
	public int SetData(Architecture.Model.WarehousesAllocationToBranch obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.WarehousesAllocationToBranch item in obj.WarehousesAllocationToBranchList)
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

	public List<Architecture.Model.WarehousesAllocationToBranch> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.WarehousesAllocationToBranch> lst = new List<Architecture.Model.WarehousesAllocationToBranch>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.WarehousesAllocationToBranch>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
