using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Account;

namespace Architecture.DAL.Account;

public class lookUpType
{
	public int SetData(Architecture.Model.Account.lookUpType obj, string ProcName)
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

	public List<Architecture.Model.Account.lookUpType> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Account.lookUpType> lst = new List<Architecture.Model.Account.lookUpType>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Account.lookUpType>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public List<Architecture.Model.Account.lookUpType> GetById(string ProcName, int Id)
	{
		List<Architecture.Model.Account.lookUpType> lst = new List<Architecture.Model.Account.lookUpType>();
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@LookUpTypeId",
				Value = Id
			});
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Account.lookUpType>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
