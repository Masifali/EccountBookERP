using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.AttendanceManagement;
using Architecture.Model.HRM.AttendanceManagement;

namespace Architecture.BLL.HRM.AttendanceManagement;

public class genHoliday
{
	public static int Save(Architecture.Model.HRM.AttendanceManagement.genHoliday obj)
	{
		try
		{
			obj.AlteredOn = DateTime.Now;
			obj.CreatedOn = DateTime.Now;
			return new Architecture.DAL.HRM.AttendanceManagement.genHoliday().SetData(obj, "Sp_genHoliday_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.HRM.AttendanceManagement.genHoliday> GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "ReadByHolidayId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.AttendanceManagement.genHoliday>();
			return new Architecture.DAL.HRM.AttendanceManagement.genHoliday().GetAll("Sp_genHoliday_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.HRM.AttendanceManagement.genHoliday> GetByMonthAndYear(int Month, int Year)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Month",
				Value = Month
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Year",
				Value = Year
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByMonthAndYear"
			});
			new List<Architecture.Model.HRM.AttendanceManagement.genHoliday>();
			return new Architecture.DAL.HRM.AttendanceManagement.genHoliday().GetAll("Sp_genHoliday_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.AttendanceManagement.genHoliday obj)
	{
		try
		{
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.AttendanceManagement.genHoliday>("Sp_genHoliday_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
