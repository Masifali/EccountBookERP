using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Production;

namespace Architecture.DAL.Production;

public class DailyPlantConsumedHours
{
	public int SetData(Architecture.Model.Production.DailyPlantConsumedHours obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Conversion.ToInt(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Production.DailyPlantConsumedHours> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Production.DailyPlantConsumedHours> lst = new List<Architecture.Model.Production.DailyPlantConsumedHours>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Production.DailyPlantConsumedHours>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
