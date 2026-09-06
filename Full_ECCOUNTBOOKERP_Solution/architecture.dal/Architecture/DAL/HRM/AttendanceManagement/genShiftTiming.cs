using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.AttendanceManagement;

namespace Architecture.DAL.HRM.AttendanceManagement;

public class genShiftTiming
{
	public int SetData(Architecture.Model.HRM.AttendanceManagement.genShiftTiming obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.ShiftTimingId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.ShiftTimingId;
			}
			foreach (genShiftTimingWeekDay item in obj.GenShiftTimingWeekDaysDetailList)
			{
				item.ShiftTimingId = obj.ShiftTimingId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "sP_genShiftTimingWeekDay_Insert"));
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

	public List<Architecture.Model.HRM.AttendanceManagement.genShiftTiming> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.AttendanceManagement.genShiftTiming> lst = new List<Architecture.Model.HRM.AttendanceManagement.genShiftTiming>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.HRM.AttendanceManagement.genShiftTiming>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.HRM.AttendanceManagement.genShiftTiming item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@ShiftTimingId", item.ShiftTimingId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByShiftTimingIdWeekofDays"));
					item.GenShiftTimingWeekDaysDetailList = GenericProvider.GetAllDetail<genShiftTimingWeekDay>(sqlcon, "Sp_genShiftTiming_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}
}
