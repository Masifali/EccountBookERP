using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.DeviceManagement;
using Architecture.Model.HRM.DeviceManagement;
using Architecture.Model.HRM.ProfileManagement;

namespace Architecture.BLL.HRM.DeviceManagement;

public class hrmPullAttendance
{
	public static int Save(Architecture.Model.HRM.DeviceManagement.hrmPullAttendance obj)
	{
		try
		{
			return new Architecture.DAL.HRM.DeviceManagement.hrmPullAttendance().SetData(obj, "Sp_hrmSetPullAttendance");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			return GenericProvider.GetDataTableProc<genDepartment>("Sp_AttendancePullFromMachine", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
