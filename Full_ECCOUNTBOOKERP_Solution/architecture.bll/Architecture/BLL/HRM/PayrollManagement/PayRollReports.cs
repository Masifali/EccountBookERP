using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL.HRM.PayrollManagement;

public class PayRollReports
{
	public static DataSet GetEmployeePayrollSalary(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (obj.EmployeeId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeId",
					Value = obj.EmployeeId
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
			if (obj.DepartmentId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentId",
					Value = obj.DepartmentId
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
			if (obj.DebitAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DebitAccountId",
					Value = obj.DebitAccountId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Month",
				Value = obj.Month
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Year",
				Value = obj.Year
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@MonthValue",
				Value = obj.MonthValue
			});
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
			return GenericProvider.GetDataSetProc<ReportsParameters>("Sp_GetEmployeePayrollSalary", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable EmployeeSalarySlip_Rpt(ReportsParameters obj)
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
			if (obj.DepartmentId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentId",
					Value = obj.DepartmentId
				});
			}
			if (obj.DebitAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DebitAccountId",
					Value = obj.DebitAccountId
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_EmployeeSalarySlip_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataSet GetEmployeePostedSalary(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (obj.EmployeeId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeId",
					Value = obj.EmployeeId
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
			if (obj.DepartmentId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentId",
					Value = obj.DepartmentId
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
			if (obj.DebitAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DebitAccountId",
					Value = obj.DebitAccountId
				});
			}
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PayrollId",
					Value = obj.Id
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Month",
				Value = obj.Month
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Year",
				Value = obj.Year
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@MonthValue",
				Value = obj.MonthValue
			});
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
			return GenericProvider.GetDataSetProc<ReportsParameters>("Sp_GetEmployeePostedSalary", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
