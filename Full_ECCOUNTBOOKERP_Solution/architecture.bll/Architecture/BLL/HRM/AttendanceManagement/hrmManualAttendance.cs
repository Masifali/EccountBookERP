using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.AttendanceManagement;
using Architecture.Model;
using Architecture.Model.HRM.AttendanceManagement;

namespace Architecture.BLL.HRM.AttendanceManagement;

public class hrmManualAttendance
{
	public static int Save(Architecture.Model.HRM.AttendanceManagement.hrmManualAttendance obj)
	{
		try
		{
			obj.AlteredOn = DateTime.Now;
			obj.CreatedOn = DateTime.Now;
			return new Architecture.DAL.HRM.AttendanceManagement.hrmManualAttendance().SetData(obj, "Sp_hrmManualAttendance_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.AttendanceManagement.hrmManualAttendance GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ManualAttendanceId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.AttendanceManagement.hrmManualAttendance>();
			return new Architecture.DAL.HRM.AttendanceManagement.hrmManualAttendance().GetAll("Sp_hrmManualAttendance_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.AttendanceManagement.hrmManualAttendance obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.AttendanceManagement.hrmManualAttendance>("Sp_hrmManualAttendance_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable genEmployeeAttendanceLoad(ReportsParameters obj)
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
			if (obj.ShiftId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShiftId",
					Value = obj.ShiftId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.DocDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DutyDate",
					Value = obj.DocDate
				});
			}
			if (obj.EmployeeId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeId",
					Value = obj.EmployeeId
				});
			}
			if (obj.DepartmentId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentId",
					Value = obj.DepartmentId
				});
			}
			if (obj.DesignationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DesignationId",
					Value = obj.DesignationId
				});
			}
			if (obj.SectionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SectionId",
					Value = obj.SectionId
				});
			}
			if (obj.PartyLocationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LocationId",
					Value = obj.PartyLocationId
				});
			}
			if (obj.EmployeeCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeCategoryId",
					Value = obj.EmployeeCategoryId
				});
			}
			if (obj.EmployeeGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeGroupId",
					Value = obj.EmployeeGroupId
				});
			}
			if (obj.GetMonth != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Month",
					Value = obj.GetMonth
				});
			}
			if (obj.GetYear != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Year",
					Value = obj.GetYear
				});
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsRestDay",
					Value = obj.IsRestDay
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.AttendanceManagement.hrmManualAttendance>("Sp_genEmployeeAttendanceWithActivities", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable EmployeeAttendenceStatusByDate(int OrganizationId, int CompanyId, DateTime Date, string Activity = null)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Date",
				Value = Date
			});
			if (Activity != null && Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = Activity
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.AttendanceManagement.hrmManualAttendance>("USP_EmployeeStatusByDate", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable EmployeeAttandanceSummaryStatus(int OrganizationId, int CompanyId, DateTime Date, int DepartmentId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Date",
				Value = Date
			});
			if (DepartmentId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentId",
					Value = DepartmentId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.AttendanceManagement.hrmManualAttendance>("USP_EmployeeAttandanceSummaryStatus", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable EmployeeAttandanceStatusMonthly(int OrganizationId, int CompanyId, long EmployeeId = 0L, int DepartmentId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			if (EmployeeId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeId",
					Value = EmployeeId
				});
			}
			if (DepartmentId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentId",
					Value = DepartmentId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.AttendanceManagement.hrmManualAttendance>("USP_EmployeeAttandanceStatusMonthly", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
