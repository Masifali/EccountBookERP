using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.PayrollManagement;
using Architecture.Model.HRM.PayrollManagement;

namespace Architecture.BLL.HRM.PayrollManagement;

public class EmployeeShortAdjustment
{
	public static long Save(Architecture.Model.HRM.PayrollManagement.EmployeeShortAdjustment obj)
	{
		try
		{
			if (obj.EmployeeShortAdjustmentId == 0L)
			{
				obj.ActionTypeId = 1;
				return new Architecture.DAL.HRM.PayrollManagement.EmployeeShortAdjustment().SetData(obj, "hrm.Sp_EmployeeShortAdjustment_Insert");
			}
			obj.ActionTypeId = 2;
			return new Architecture.DAL.HRM.PayrollManagement.EmployeeShortAdjustment().SetData(obj, "hrm.Sp_EmployeeShortAdjustment_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.PayrollManagement.EmployeeShortAdjustment GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeShortAdjustmentId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.PayrollManagement.EmployeeShortAdjustment>();
			return new Architecture.DAL.HRM.PayrollManagement.EmployeeShortAdjustment().GetAll("hrm.Sp_EmployeeShortAdjustment_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.HRM.PayrollManagement.EmployeeShortAdjustment obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.PayrollManagement.EmployeeShortAdjustment>("hrm.Sp_EmployeeShortAdjustment_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
