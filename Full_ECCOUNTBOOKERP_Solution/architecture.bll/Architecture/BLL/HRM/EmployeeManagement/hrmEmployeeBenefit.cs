using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.EmployeeManagement;
using Architecture.Model.HRM.EmployeeManagement;

namespace Architecture.BLL.HRM.EmployeeManagement;

public class hrmEmployeeBenefit
{
	public static int Save(Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBenefit obj)
	{
		try
		{
			if (obj.EmployeeBenefitsList[0].EmployeeBenefitId == 0)
			{
				return new Architecture.DAL.HRM.EmployeeManagement.hrmEmployeeBenefit().SetData(obj, "Sp_hrmEmployeeBenefit_Insert");
			}
			return new Architecture.DAL.HRM.EmployeeManagement.hrmEmployeeBenefit().SetData(obj, "Sp_hrmEmployeeBenefit_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBenefit> GetByID(Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBenefit obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (obj.EmployeeBenefitId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.EmployeeBenefitId
				});
			}
			if (obj.EmployeeId != 0)
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
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBenefit>();
			return new Architecture.DAL.HRM.EmployeeManagement.hrmEmployeeBenefit().GetAll("Sp_hrmEmployeeBenefit_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBenefit obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBenefit>("Sp_hrmEmployeeBenefit_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByEmployeeId(Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBenefit obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBenefit>("Sp_hrmEmployeeBenefit_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
