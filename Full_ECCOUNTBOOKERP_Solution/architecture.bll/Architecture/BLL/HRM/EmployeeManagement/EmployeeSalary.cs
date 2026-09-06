using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.EmployeeManagement;
using Architecture.Model.HRM.EmployeeManagement;

namespace Architecture.BLL.HRM.EmployeeManagement;

public class EmployeeSalary
{
	public static long Save(Architecture.Model.HRM.EmployeeManagement.EmployeeSalary obj)
	{
		try
		{
			if (obj.EmployeeSalaryId == 0L)
			{
				obj.ActionTypeId = 1;
				return new Architecture.DAL.HRM.EmployeeManagement.EmployeeSalary().SetData(obj, "hrm.Sp_EmployeeSalary_Insert");
			}
			obj.ActionTypeId = 2;
			return new Architecture.DAL.HRM.EmployeeManagement.EmployeeSalary().SetData(obj, "hrm.Sp_EmployeeSalary_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.HRM.EmployeeManagement.EmployeeSalary> GetByID(long EmployeeSalaryId = 0L, long EmployeeId = 0L)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (EmployeeSalaryId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeSalaryId",
					Value = EmployeeSalaryId
				});
			}
			if (EmployeeId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeId",
					Value = EmployeeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.EmployeeManagement.EmployeeSalary>();
			return new Architecture.DAL.HRM.EmployeeManagement.EmployeeSalary().GetAll("hrm.Sp_EmployeeSalary_ReadAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.HRM.EmployeeManagement.EmployeeSalary obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.EmployeeSalary>("hrm.Sp_EmployeeSalary_ReadAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
