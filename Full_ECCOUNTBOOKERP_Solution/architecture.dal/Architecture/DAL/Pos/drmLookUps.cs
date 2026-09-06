using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.POS;

namespace Architecture.DAL.Pos;

public class drmLookUps
{
	public int SetData(Architecture.Model.POS.drmLookUps obj, string ProcName)
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

	public List<Architecture.Model.POS.drmLookUps> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.POS.drmLookUps> lst = new List<Architecture.Model.POS.drmLookUps>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.POS.drmLookUps>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
