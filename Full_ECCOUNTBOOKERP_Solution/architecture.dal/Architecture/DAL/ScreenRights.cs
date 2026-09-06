using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class ScreenRights
{
	public int SetData(Architecture.Model.ScreenRights obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.ScreenRights item in obj.DetailScreenRights)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_ScreenRights_Insert"));
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

	public List<Architecture.Model.ScreenRights> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ScreenRights> lst = new List<Architecture.Model.ScreenRights>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.ScreenRights>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
