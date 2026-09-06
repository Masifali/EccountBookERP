using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.PayrollManagement;
using Architecture.Model.HRM.PayrollManagement;

namespace Architecture.BLL.HRM.PayrollManagement;

public class EmployeeAllowance
{
	public static long Save(Architecture.Model.HRM.PayrollManagement.EmployeeAllowance obj)
	{
		try
		{
			_ = obj.EmployeeAllowanceList[0].EmployeeAllowanceId;
			return new Architecture.DAL.HRM.PayrollManagement.EmployeeAllowance().SetData(obj, "hrm.Sp_EmployeeAllowance_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.HRM.PayrollManagement.EmployeeAllowance> GetByID(long Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeAllowanceId",
				Value = Id
			});
			new List<Architecture.Model.HRM.PayrollManagement.EmployeeAllowance>();
			return new Architecture.DAL.HRM.PayrollManagement.EmployeeAllowance().GetData("hrm.Sp_EmployeeAllowance_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.HRM.PayrollManagement.EmployeeAllowance> ReadByEmployeeId(Architecture.Model.HRM.PayrollManagement.EmployeeAllowance obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByEmployeeId"
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
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeId",
				Value = obj.EmployeeId
			});
			new List<Architecture.Model.HRM.PayrollManagement.EmployeeAllowance>();
			return new Architecture.DAL.HRM.PayrollManagement.EmployeeAllowance().GetData("hrm.Sp_EmployeeAllowance_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAll(Architecture.Model.HRM.PayrollManagement.EmployeeAllowance obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.PayrollManagement.EmployeeAllowance>("hrm.Sp_EmployeeAllowance_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
