using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class UserWarehouse
{
	public int SetData(Architecture.Model.UserWarehouse obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.UserWarehouse item in obj.UserWarehousesList)
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

	public List<Architecture.Model.UserWarehouse> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.UserWarehouse> lst = new List<Architecture.Model.UserWarehouse>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.UserWarehouse>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
