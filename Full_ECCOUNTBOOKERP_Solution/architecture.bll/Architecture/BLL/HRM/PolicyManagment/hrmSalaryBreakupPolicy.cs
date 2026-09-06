using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.PolicyManagment;
using Architecture.Model.HRM.PolicyManagment;

namespace Architecture.BLL.HRM.PolicyManagment;

public class hrmSalaryBreakupPolicy
{
	public static int Save(Architecture.Model.HRM.PolicyManagment.hrmSalaryBreakupPolicy obj)
	{
		try
		{
			if (obj.SalaryBreakupPolicyId == 0)
			{
				return new Architecture.DAL.HRM.PolicyManagment.hrmSalaryBreakupPolicy().SetData(obj, "Sp_hrmSalaryBreakupPolicy_Insert");
			}
			return new Architecture.DAL.HRM.PolicyManagment.hrmSalaryBreakupPolicy().SetData(obj, "Sp_hrmSalaryBreakupPolicy_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.PolicyManagment.hrmSalaryBreakupPolicy GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@SalaryBreakupPolicyId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.PolicyManagment.hrmSalaryBreakupPolicy>();
			return new Architecture.DAL.HRM.PolicyManagment.hrmSalaryBreakupPolicy().GetAll("Sp_hrmSalaryBreakupPolicy_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.PolicyManagment.hrmSalaryBreakupPolicy obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.PolicyManagment.hrmSalaryBreakupPolicy>("Sp_hrmSalaryBreakupPolicy_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.HRM.PolicyManagment.hrmSalaryBreakupPolicy> GetByLocationId(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@LocationId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByLocationId"
			});
			new List<Architecture.Model.HRM.PolicyManagment.hrmSalaryBreakupPolicy>();
			return new Architecture.DAL.HRM.PolicyManagment.hrmSalaryBreakupPolicy().GetAll("Sp_hrmSalaryBreakupPolicy_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
