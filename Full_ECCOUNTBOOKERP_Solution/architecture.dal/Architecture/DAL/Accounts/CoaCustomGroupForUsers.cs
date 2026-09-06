using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.DAL.Accounts;

public class CoaCustomGroupForUsers
{
	public int SetData(Architecture.Model.Accounts.CoaCustomGroupForUsers obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = returnID;
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

	public List<Architecture.Model.Accounts.CoaCustomGroupForUsers> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Accounts.CoaCustomGroupForUsers> lst = new List<Architecture.Model.Accounts.CoaCustomGroupForUsers>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Accounts.CoaCustomGroupForUsers>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
