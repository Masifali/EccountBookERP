using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Account;

namespace Architecture.DAL.Account;

public class lookUp
{
	public int SetData(Architecture.Model.Account.lookUp obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Account.lookUp> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Account.lookUp> lst = new List<Architecture.Model.Account.lookUp>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Account.lookUp>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public List<Architecture.Model.Account.lookUp> GetById(string ProcName, int Id)
	{
		List<Architecture.Model.Account.lookUp> lst = new List<Architecture.Model.Account.lookUp>();
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@LookUpId",
				Value = Id
			});
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Account.lookUp>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
