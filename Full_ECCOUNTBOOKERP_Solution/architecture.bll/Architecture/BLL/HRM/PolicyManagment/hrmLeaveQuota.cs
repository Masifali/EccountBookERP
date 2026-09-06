using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.PolicyManagment;
using Architecture.Model;
using Architecture.Model.HRM.PolicyManagment;

namespace Architecture.BLL.HRM.PolicyManagment;

public class hrmLeaveQuota
{
	public static int Save(Architecture.Model.HRM.PolicyManagment.hrmLeaveQuota obj)
	{
		try
		{
			if (obj.LeaveQuotaId == 0)
			{
				obj.ActionTypeId = 1;
				return new Architecture.DAL.HRM.PolicyManagment.hrmLeaveQuota().SetData(obj, "sp_hrmLeaveQuota_Insert");
			}
			obj.ActionTypeId = 2;
			return new Architecture.DAL.HRM.PolicyManagment.hrmLeaveQuota().SetData(obj, "Sp_hrmLeaveQuota_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.PolicyManagment.hrmLeaveQuota GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@leaveQuotaId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.PolicyManagment.hrmLeaveQuota>();
			return new Architecture.DAL.HRM.PolicyManagment.hrmLeaveQuota().GetAll("Sp_hrmLeaveQuota_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.PolicyManagment.hrmLeaveQuota obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.PolicyManagment.hrmLeaveQuota>("Sp_hrmLeaveQuota_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForLeaveQuotaDropDown(Architecture.Model.HRM.PolicyManagment.hrmLeaveQuota obj)
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
				ParameterName = "@FromDate",
				Value = obj.FromDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetDataForLeaveQuotaDropDown"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.PolicyManagment.hrmLeaveQuota>("Sp_hrmLeaveQuota_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForLeaveOpening(ReportsParameters obj)
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
				ParameterName = "@LeaveQuotaId",
				Value = obj.LeaveQuotaId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@LeaveTypeProfileId",
				Value = obj.LeaveTypeProfileId
			});
			if (obj.EmployeeId != 0L)
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
				Value = "GetDataForLeaveOpening"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.PolicyManagment.hrmLeaveQuota>("Sp_hrmLeaveQuota_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
