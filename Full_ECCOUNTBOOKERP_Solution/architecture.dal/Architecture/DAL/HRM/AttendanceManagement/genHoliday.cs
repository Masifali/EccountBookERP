using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.AttendanceManagement;

namespace Architecture.DAL.HRM.AttendanceManagement;

public class genHoliday
{
	public int SetData(Architecture.Model.HRM.AttendanceManagement.genHoliday obj, string ProcName)
	{
		int returnID = 0;
		using (SqlConnection sqlcon = ConnectionObject.SQL())
		{
			sqlcon.Open();
			SqlTransaction sqlTrn = sqlcon.BeginTransaction();
			try
			{
				foreach (Architecture.Model.HRM.AttendanceManagement.genHoliday item in obj.genHolidaysList)
				{
					if (item.HolidayId == 0)
					{
						item.ActionTypeId = 1;
					}
					else
					{
						item.ActionTypeId = 2;
					}
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
			Value = obj.OrginizationId
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

	public List<Architecture.Model.HRM.AttendanceManagement.genHoliday> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.AttendanceManagement.genHoliday> lst = new List<Architecture.Model.HRM.AttendanceManagement.genHoliday>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.HRM.AttendanceManagement.genHoliday>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
