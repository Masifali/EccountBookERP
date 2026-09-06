using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class ReportConfig
{
	public int SetData(Architecture.Model.ReportConfig obj, string ProcName)
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
				obj.ReportConfigId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.ReportConfigId;
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

	public List<Architecture.Model.ReportConfig> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ReportConfig> lst = new List<Architecture.Model.ReportConfig>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.ReportConfig>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
