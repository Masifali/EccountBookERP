using System;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.DeviceManagement;

namespace Architecture.DAL.HRM.DeviceManagement;

public class hrmPullAttendance
{
	public int SetData(Architecture.Model.HRM.DeviceManagement.hrmPullAttendance obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.HRM.DeviceManagement.hrmPullAttendance item in obj.HrmPullAttendancesList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
			}
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
		CommonProvider.ExecuteProcedure("[dbo].[Sp_SysUpdateAttendanceFromPull]");
		CommonProvider.ExecuteProcedure("[dbo].[Sp_SysUpdateAttendanceStatus]");
		return returnID;
	}
}
