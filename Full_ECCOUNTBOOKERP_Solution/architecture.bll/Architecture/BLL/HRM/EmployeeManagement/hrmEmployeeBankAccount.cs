using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.EmployeeManagement;
using Architecture.Model.HRM.EmployeeManagement;

namespace Architecture.BLL.HRM.EmployeeManagement;

public class hrmEmployeeBankAccount
{
	public static int Save(Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBankAccount obj)
	{
		try
		{
			obj.ActionTypeId = 1;
			obj.AlteredOn = DateTime.Now;
			obj.CreatedOn = DateTime.Now;
			if (obj.EmployeeBankAccountId == 0)
			{
				return new Architecture.DAL.HRM.EmployeeManagement.hrmEmployeeBankAccount().SetData(obj, "Sp_hrmEmployeeBankAccount_Insert");
			}
			return new Architecture.DAL.HRM.EmployeeManagement.hrmEmployeeBankAccount().SetData(obj, "Sp_hrmEmployeeBankAccount_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBankAccount GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeBankAccountId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBankAccount>();
			return new Architecture.DAL.HRM.EmployeeManagement.hrmEmployeeBankAccount().GetAll("Sp_hrmEmployeeBankAccount_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBankAccount obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBankAccount>("Sp_hrmEmployeeBankAccount_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
