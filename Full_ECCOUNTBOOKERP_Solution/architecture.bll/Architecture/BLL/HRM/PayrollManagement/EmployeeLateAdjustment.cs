using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.PayrollManagement;
using Architecture.Model.HRM.PayrollManagement;

namespace Architecture.BLL.HRM.PayrollManagement;

public class EmployeeLateAdjustment
{
	public static long Save(Architecture.Model.HRM.PayrollManagement.EmployeeLateAdjustment obj)
	{
		try
		{
			if (obj.EmployeeLateAdjustmentId == 0L)
			{
				obj.ActionTypeId = 1L;
				return new Architecture.DAL.HRM.PayrollManagement.EmployeeLateAdjustment().SetData(obj, "hrm.Sp_EmployeeLateAdjustment_Insert");
			}
			obj.ActionTypeId = 2L;
			return new Architecture.DAL.HRM.PayrollManagement.EmployeeLateAdjustment().SetData(obj, "hrm.Sp_EmployeeLateAdjustment_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.PayrollManagement.EmployeeLateAdjustment GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeLateAdjustmentId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.PayrollManagement.EmployeeLateAdjustment>();
			return new Architecture.DAL.HRM.PayrollManagement.EmployeeLateAdjustment().GetAll("hrm.Sp_EmployeeLateAdjustment_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.HRM.PayrollManagement.EmployeeLateAdjustment obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.PayrollManagement.EmployeeLateAdjustment>("hrm.Sp_EmployeeLateAdjustment_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
