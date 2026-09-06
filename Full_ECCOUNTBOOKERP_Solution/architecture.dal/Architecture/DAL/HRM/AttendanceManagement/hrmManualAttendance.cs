using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.AttendanceManagement;

namespace Architecture.DAL.HRM.AttendanceManagement;

public class hrmManualAttendance
{
	public int SetData(Architecture.Model.HRM.AttendanceManagement.hrmManualAttendance obj, string ProcName)
	{
		int returnID = 0;
		using (SqlConnection sqlcon = ConnectionObject.SQL())
		{
			sqlcon.Open();
			SqlTransaction sqlTrn = sqlcon.BeginTransaction();
			try
			{
				foreach (Architecture.Model.HRM.AttendanceManagement.hrmManualAttendance item in obj.listHrmManualAttendances)
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
		}
		List<SqlParameter> p = new List<SqlParameter>();
		p.Add(new SqlParameter
		{
			ParameterName = "@OrganizationId",
			Value = obj.OrganizationId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@CompanyId",
			Value = obj.CompanyId
		});
		CommonProvider.ExecuteProcedure("[dbo].[Sp_SysUpdateAttendanceFromPull]", p);
		CommonProvider.ExecuteProcedure("[dbo].[Sp_SysUpdateAttendanceStatus]", p);
		return returnID;
	}

	public List<Architecture.Model.HRM.AttendanceManagement.hrmManualAttendance> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.AttendanceManagement.hrmManualAttendance> lst = new List<Architecture.Model.HRM.AttendanceManagement.hrmManualAttendance>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.HRM.AttendanceManagement.hrmManualAttendance>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
