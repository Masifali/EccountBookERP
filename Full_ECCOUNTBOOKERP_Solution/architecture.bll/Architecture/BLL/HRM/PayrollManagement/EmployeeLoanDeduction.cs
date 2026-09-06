using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.PayrollManagement;
using Architecture.Model.HRM.PayrollManagement;

namespace Architecture.BLL.HRM.PayrollManagement;

public class EmployeeLoanDeduction
{
	public static long Save(Architecture.Model.HRM.PayrollManagement.EmployeeLoanDeduction obj)
	{
		try
		{
			return new Architecture.DAL.HRM.PayrollManagement.EmployeeLoanDeduction().SetData(obj, "hrm.Sp_EmployeeLoanDeduction_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.PayrollManagement.EmployeeLoanDeduction GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeLoanDeductionId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.PayrollManagement.EmployeeLoanDeduction>();
			return new Architecture.DAL.HRM.PayrollManagement.EmployeeLoanDeduction().GetAll("hrm.Sp_EmployeeLoanDeduction_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.HRM.PayrollManagement.EmployeeLoanDeduction obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.PayrollManagement.EmployeeLoanDeduction>("hrm.Sp_EmployeeLoanDeduction_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLoanInstallmentsForLoanDeductions(Architecture.Model.HRM.PayrollManagement.EmployeeLoanDeduction obj)
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
				Value = "GetLoanInstallmentsForLoanDeductions"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.PayrollManagement.EmployeeLoanDeduction>("[hrm].[Sp_EmployeeLoanDeduction_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.HRM.PayrollManagement.EmployeeLoanDeduction> GetByYearAndMonth(Architecture.Model.HRM.PayrollManagement.EmployeeLoanDeduction obj)
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
				ParameterName = "@PayrollMonth",
				Value = obj.PayrollMonth
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PayrollYear",
				Value = obj.PayrollYear
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetByYearAndMonth"
			});
			new List<Architecture.Model.HRM.PayrollManagement.EmployeeLoanDeduction>();
			return new Architecture.DAL.HRM.PayrollManagement.EmployeeLoanDeduction().GetAll("[hrm].[Sp_EmployeeLoanDeduction_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
