using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.AttendanceManagement;

namespace Architecture.DAL.HRM.AttendanceManagement;

public class CPLAttendance
{
	public int SetData(Architecture.Model.HRM.AttendanceManagement.CPLAttendance obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.HRM.AttendanceManagement.CPLAttendance item in obj.CPLAttendancesList)
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

	public List<Architecture.Model.HRM.AttendanceManagement.CPLAttendance> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.AttendanceManagement.CPLAttendance> lst = new List<Architecture.Model.HRM.AttendanceManagement.CPLAttendance>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<Architecture.Model.HRM.AttendanceManagement.CPLAttendance>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
