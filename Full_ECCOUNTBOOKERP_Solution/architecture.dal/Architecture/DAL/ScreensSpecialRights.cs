using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class ScreensSpecialRights
{
	public int SetData(Architecture.Model.ScreensSpecialRights obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.ScreensSpecialRights item in obj.ScreensSpecialRightsList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
				obj.Id = Convert.ToInt32(returnID);
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public List<Architecture.Model.ScreensSpecialRights> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ScreensSpecialRights> lst = new List<Architecture.Model.ScreensSpecialRights>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.ScreensSpecialRights>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
