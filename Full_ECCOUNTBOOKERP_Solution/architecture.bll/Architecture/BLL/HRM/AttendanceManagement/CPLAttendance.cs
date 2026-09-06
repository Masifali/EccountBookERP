using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.AttendanceManagement;
using Architecture.Model;
using Architecture.Model.HRM.AttendanceManagement;

namespace Architecture.BLL.HRM.AttendanceManagement;

public class CPLAttendance
{
	public static int Save(Architecture.Model.HRM.AttendanceManagement.CPLAttendance obj)
	{
		try
		{
			obj.AlteredOn = DateTime.Now;
			obj.CreatedOn = DateTime.Now;
			return new Architecture.DAL.HRM.AttendanceManagement.CPLAttendance().SetData(obj, "Sp_CPLAttendance_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.AttendanceManagement.CPLAttendance ReadById(long Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.AttendanceManagement.CPLAttendance>();
			return new Architecture.DAL.HRM.AttendanceManagement.CPLAttendance().GetAll("Sp_CPLAttendance_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAll(Architecture.Model.HRM.AttendanceManagement.CPLAttendance obj)
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
			if (obj.ReqTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReqTypeId",
					Value = obj.ReqTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.AttendanceManagement.CPLAttendance>("Sp_CPLAttendance_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetEmployeeAttendanceRecordForCPLAttendance(ReportsParameters obj)
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
				ParameterName = "@EmployeeId",
				Value = obj.EmployeeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Month",
				Value = obj.MonthValue
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Year",
				Value = obj.Year
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetEmployeeAttendanceRecordForCPLAttendance"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.AttendanceManagement.CPLAttendance>("Sp_CPLAttendance_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetCPLRecordForEmployeeLeaveRequest(ReportsParameters obj)
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
				ParameterName = "@EmployeeId",
				Value = obj.EmployeeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetCPLRecordForEmployeeLeaveRequest"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.AttendanceManagement.CPLAttendance>("Sp_CPLAttendance_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
