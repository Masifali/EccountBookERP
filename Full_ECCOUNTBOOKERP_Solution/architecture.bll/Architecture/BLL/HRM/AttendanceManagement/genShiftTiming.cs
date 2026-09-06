using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.AttendanceManagement;
using Architecture.Model.HRM.AttendanceManagement;

namespace Architecture.BLL.HRM.AttendanceManagement;

public class genShiftTiming
{
	public static int Save(Architecture.Model.HRM.AttendanceManagement.genShiftTiming obj)
	{
		try
		{
			if (obj.ShiftTimingId == 0)
			{
				obj.ActionTypeId = 1;
				return new Architecture.DAL.HRM.AttendanceManagement.genShiftTiming().SetData(obj, "Sp_genShiftTiming_Insert");
			}
			obj.ActionTypeId = 2;
			return new Architecture.DAL.HRM.AttendanceManagement.genShiftTiming().SetData(obj, "Sp_genShiftTiming_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.AttendanceManagement.genShiftTiming GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ShiftTimingId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.AttendanceManagement.genShiftTiming>();
			return new Architecture.DAL.HRM.AttendanceManagement.genShiftTiming().GetAll("Sp_genShiftTiming_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.AttendanceManagement.genShiftTiming obj)
	{
		try
		{
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.AttendanceManagement.genShiftTiming>("Sp_genShiftTiming_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
