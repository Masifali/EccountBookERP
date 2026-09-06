using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.CmTr.TransactionForm;

namespace Architecture.DAL.CmTr.TransactionForm;

public class LookUps
{
	public int SetData(Architecture.Model.CmTr.TransactionForm.LookUps obj, string ProcName)
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

	public List<Architecture.Model.CmTr.TransactionForm.LookUps> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.CmTr.TransactionForm.LookUps> lst = new List<Architecture.Model.CmTr.TransactionForm.LookUps>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.CmTr.TransactionForm.LookUps>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public List<Architecture.Model.CmTr.TransactionForm.LookUps> GetById(string ProcName, int Id)
	{
		List<Architecture.Model.CmTr.TransactionForm.LookUps> lst = new List<Architecture.Model.CmTr.TransactionForm.LookUps>();
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
			return GenericProvider.GetProc<Architecture.Model.CmTr.TransactionForm.LookUps>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
