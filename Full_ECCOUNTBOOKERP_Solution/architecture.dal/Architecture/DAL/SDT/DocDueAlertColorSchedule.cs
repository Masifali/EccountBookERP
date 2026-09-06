using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.SDT;

namespace Architecture.DAL.SDT;

public class DocDueAlertColorSchedule
{
	public int SetData(Architecture.Model.SDT.DocDueAlertColorSchedule obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.SDT.DocDueAlertColorSchedule item in obj.DocDueAlertColorScheduleList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public List<Architecture.Model.SDT.DocDueAlertColorSchedule> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.SDT.DocDueAlertColorSchedule> lst = new List<Architecture.Model.SDT.DocDueAlertColorSchedule>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.SDT.DocDueAlertColorSchedule>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
