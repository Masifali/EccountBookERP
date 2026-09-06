using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class AppMenu
{
	public int SetData(Architecture.Model.AppMenu obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (Architecture.Model.ScreenRights item in obj.DetailScreenRights)
			{
				item.Id = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_ScreenRights_Insert"));
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

	public List<Architecture.Model.AppMenu> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.AppMenu> lst = new List<Architecture.Model.AppMenu>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.AppMenu>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
