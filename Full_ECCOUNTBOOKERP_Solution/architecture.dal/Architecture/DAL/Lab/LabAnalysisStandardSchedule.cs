using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Lab;

namespace Architecture.DAL.Lab;

public class LabAnalysisStandardSchedule
{
	public int SetData(Architecture.Model.Lab.LabAnalysisStandardSchedule obj, string procName)
	{
		int returnID = 0;
		using (SqlConnection sqlcon = ConnectionObject.SQL())
		{
			sqlcon.Open();
			SqlTransaction sqlTrn = sqlcon.BeginTransaction();
			try
			{
				foreach (Architecture.Model.Lab.LabAnalysisStandardSchedule item in obj.LabAnalysisStandardScheduleList)
				{
					returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, procName));
					item.LabAnalysisStandardScheduleId = ((returnID > 0) ? returnID : item.LabAnalysisStandardScheduleId);
				}
				sqlTrn.Commit();
			}
			catch
			{
				sqlTrn.Rollback();
				throw;
			}
		}
		return obj.LabAnalysisStandardScheduleId;
	}

	public List<Architecture.Model.Lab.LabAnalysisStandardSchedule> GetData(string procName, List<SqlParameter> p = null)
	{
		try
		{
			using SqlConnection sqlCon = ConnectionObject.SQL();
			sqlCon.Open();
			return GenericProvider.GetProc<Architecture.Model.Lab.LabAnalysisStandardSchedule>(sqlCon, procName, p);
		}
		catch
		{
			throw;
		}
	}
}
