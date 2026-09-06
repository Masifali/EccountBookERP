using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class MultiCurrency
{
	public int SetData(Architecture.Model.MultiCurrency obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.MultiCurrency> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.MultiCurrency> lst = new List<Architecture.Model.MultiCurrency>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.MultiCurrency>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
