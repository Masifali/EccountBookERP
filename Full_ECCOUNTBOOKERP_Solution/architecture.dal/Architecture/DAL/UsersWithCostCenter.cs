using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class UsersWithCostCenter
{
	public long SetData(Architecture.Model.UsersWithCostCenter obj, string ProcName)
	{
		long returnID = 0L;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.UsersWithCostCenter item in obj.UsersWithCostCenterList)
			{
				returnID = Conversion.ToInt64(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public List<Architecture.Model.UsersWithCostCenter> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.UsersWithCostCenter> lst = new List<Architecture.Model.UsersWithCostCenter>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.UsersWithCostCenter>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
