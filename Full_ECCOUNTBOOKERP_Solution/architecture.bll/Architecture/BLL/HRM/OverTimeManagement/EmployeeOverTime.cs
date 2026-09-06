using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.OverTimeManagement;
using Architecture.Model;
using Architecture.Model.HRM.OverTimeManagement;

namespace Architecture.BLL.HRM.OverTimeManagement;

public class EmployeeOverTime
{
	public static long Save(Architecture.Model.HRM.OverTimeManagement.EmployeeOverTime obj)
	{
		try
		{
			if (obj.EmployeeOverTimeId == 0L)
			{
				obj.ActionTypeId = 1;
				return new Architecture.DAL.HRM.OverTimeManagement.EmployeeOverTime().SetData(obj, "hrm.Sp_EmployeeOverTime_Insert");
			}
			obj.ActionTypeId = 2;
			return new Architecture.DAL.HRM.OverTimeManagement.EmployeeOverTime().SetData(obj, "hrm.Sp_EmployeeOverTime_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.OverTimeManagement.EmployeeOverTime GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeOverTimeId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.OverTimeManagement.EmployeeOverTime>();
			return new Architecture.DAL.HRM.OverTimeManagement.EmployeeOverTime().GetAll("hrm.Sp_EmployeeOverTime_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.HRM.OverTimeManagement.EmployeeOverTime obj)
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
			if (obj.OverTimeMonth != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Month",
					Value = obj.OverTimeMonth
				});
			}
			if (obj.OverTimeYear != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Year",
					Value = obj.OverTimeYear
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.OverTimeManagement.EmployeeOverTime>("hrm.Sp_EmployeeOverTime_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDown(int OrganizationId, int CompanyId, string Activity = null)
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
			if (Activity != null && Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = Activity
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.OverTimeManagement.EmployeeOverTime>("USP_GetDataForDropDownFromEmployeeOverTime", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getOTEmployees(int OrganizationId, int CompanyId, int Month, int Year)
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
				ParameterName = "@Month",
				Value = Month
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Year",
				Value = Year
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.OverTimeManagement.OverTimeRequest>("hrm.usp_getOTEmployees", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetEmployeeAttendanceMonthWise(ReportsParameters obj)
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
			if (obj.EmployeeId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeId",
					Value = obj.EmployeeId
				});
			}
			if (obj.MonthValue != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Month",
					Value = obj.MonthValue
				});
			}
			if (obj.Year != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Year",
					Value = obj.Year
				});
			}
			if (obj.LocationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LocationId",
					Value = obj.LocationId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.OverTimeManagement.OverTimeRequest>("[hrm].[usp_GetEmployeeAttendanceMonthWise]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ActualOverTimeLoaderForRequest(ReportsParameters obj)
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
				ParameterName = "@FromDate",
				Value = obj.FromDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = obj.ToDate
			});
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
			return GenericProvider.GetDataTableProc<ReportsParameters>("[dbo].[usp_ActualOverTimeLoaderForRequest]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable EmployeeOverTime_SlipAndRegister(ReportsParameters obj)
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
				ParameterName = "@Month",
				Value = obj.MonthValue
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Year",
				Value = obj.Year
			});
			if (obj.EmployeeId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeId",
					Value = obj.EmployeeId
				});
			}
			if (obj.DutyRoasterId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.DutyRoasterId
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[hrm].[USP_EmployeeOverTime_SlipAndRegister]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
