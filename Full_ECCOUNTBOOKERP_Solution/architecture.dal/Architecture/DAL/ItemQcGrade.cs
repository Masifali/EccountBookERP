using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class ItemQcGrade
{
	public int SetData(Architecture.Model.ItemQcGrade obj, string ProcName)
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

	public List<Architecture.Model.ItemQcGrade> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ItemQcGrade> lst = new List<Architecture.Model.ItemQcGrade>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.ItemQcGrade>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public List<Architecture.Model.ItemQcGrade> GetById(string ProcName, int Id)
	{
		List<Architecture.Model.ItemQcGrade> lst = new List<Architecture.Model.ItemQcGrade>();
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.ItemQcGrade>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
