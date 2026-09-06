using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class CompanyReport
{
	public int SetData(Architecture.Model.CompanyReport obj, string ProcName)
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
				obj.CompanyReportId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.CompanyReportId;
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

	public List<Architecture.Model.CompanyReport> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.CompanyReport> lst = new List<Architecture.Model.CompanyReport>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.CompanyReport>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
