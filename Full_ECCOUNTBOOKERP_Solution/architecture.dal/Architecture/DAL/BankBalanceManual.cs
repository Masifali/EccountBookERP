using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class BankBalanceManual
{
	public int SetData(Architecture.Model.BankBalanceManual obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.BankBalanceManual item in obj.BankBalanceManualList)
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

	public List<Architecture.Model.BankBalanceManual> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.BankBalanceManual> lst = new List<Architecture.Model.BankBalanceManual>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.BankBalanceManual>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public List<Architecture.Model.BankBalanceManual> GetById(string ProcName, int Id)
	{
		List<Architecture.Model.BankBalanceManual> lst = new List<Architecture.Model.BankBalanceManual>();
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
			return GenericProvider.GetProc<Architecture.Model.BankBalanceManual>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
