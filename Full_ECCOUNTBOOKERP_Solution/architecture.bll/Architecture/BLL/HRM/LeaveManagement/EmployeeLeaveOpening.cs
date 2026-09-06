using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.LeaveManagement;
using Architecture.Model;
using Architecture.Model.HRM.LeaveManagement;

namespace Architecture.BLL.HRM.LeaveManagement;

public class EmployeeLeaveOpening
{
	public static long Save(Architecture.Model.HRM.LeaveManagement.EmployeeLeaveOpening obj)
	{
		try
		{
			if (obj.EmployeeLeaveOpeningId == 0L)
			{
				obj.ActionTypeId = 1;
				return new Architecture.DAL.HRM.LeaveManagement.EmployeeLeaveOpening().SetData(obj, "hrm.Sp_EmployeeLeaveOpening_Insert");
			}
			obj.ActionTypeId = 2;
			return new Architecture.DAL.HRM.LeaveManagement.EmployeeLeaveOpening().SetData(obj, "hrm.Sp_EmployeeLeaveOpening_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.LeaveManagement.EmployeeLeaveOpening GetByID(long Id)
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
				ParameterName = "@EmployeeLeaveOpeningId",
				Value = Id
			});
			new List<Architecture.Model.HRM.LeaveManagement.EmployeeLeaveOpening>();
			return new Architecture.DAL.HRM.LeaveManagement.EmployeeLeaveOpening().GetData("hrm.Sp_EmployeeLeaveOpening_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAll(Architecture.Model.HRM.LeaveManagement.EmployeeLeaveOpening obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.LeaveManagement.EmployeeLeaveOpening>("hrm.Sp_EmployeeLeaveOpening_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static decimal GetLeavesBalanceByEmployeeId(ReportsParameters obj)
	{
		decimal Balance = default(decimal);
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
				ParameterName = "@EmployeeId",
				Value = obj.EmployeeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@LeaveQuotaId",
				Value = obj.LeaveQuotaId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@LeaveTypeProfileId",
				Value = obj.LeaveTypeProfileId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetLeavesBalanceByEmployeeId"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.HRM.LeaveManagement.EmployeeLeaveOpening>("hrm.Sp_EmployeeLeaveOpening_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				return Conversion.ToDecimal(dt.Rows[0][0]);
			}
			return Balance;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
