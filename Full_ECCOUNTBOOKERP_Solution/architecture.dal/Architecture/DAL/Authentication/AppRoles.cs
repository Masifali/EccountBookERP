using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Authentication;

namespace Architecture.DAL.Authentication;

public class AppRoles : DALBase
{
	public int SetData(Architecture.Model.Authentication.AppRoles obj, string ProcName)
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

	public List<Architecture.Model.Authentication.AppRoles> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Authentication.AppRoles> lst = new List<Architecture.Model.Authentication.AppRoles>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Authentication.AppRoles>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
