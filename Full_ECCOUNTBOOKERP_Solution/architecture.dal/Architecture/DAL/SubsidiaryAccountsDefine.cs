using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class SubsidiaryAccountsDefine
{
	public int SetData(Architecture.Model.SubsidiaryAccountsDefine obj, string ProcName)
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

	public List<Architecture.Model.SubsidiaryAccountsDefine> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.SubsidiaryAccountsDefine> lst = new List<Architecture.Model.SubsidiaryAccountsDefine>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.SubsidiaryAccountsDefine>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
