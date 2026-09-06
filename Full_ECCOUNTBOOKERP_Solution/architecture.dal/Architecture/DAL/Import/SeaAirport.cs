using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Import;

namespace Architecture.DAL.Import;

public class SeaAirport
{
	public int SetData(Architecture.Model.Import.SeaAirport obj, string ProcName)
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

	public List<Architecture.Model.Import.SeaAirport> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Import.SeaAirport> lst = new List<Architecture.Model.Import.SeaAirport>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Import.SeaAirport>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
