using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.SystemUtilities;

namespace Architecture.DAL.SystemUtilities;

public class SeasonYearSchedule
{
	public int SetData(Architecture.Model.SystemUtilities.SeasonYearSchedule obj, string ProcName)
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

	public List<Architecture.Model.SystemUtilities.SeasonYearSchedule> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.SystemUtilities.SeasonYearSchedule> lst = new List<Architecture.Model.SystemUtilities.SeasonYearSchedule>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<Architecture.Model.SystemUtilities.SeasonYearSchedule>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
