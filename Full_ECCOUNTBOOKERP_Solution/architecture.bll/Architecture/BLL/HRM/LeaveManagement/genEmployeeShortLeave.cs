using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.LeaveManagement;
using Architecture.Model.HRM.LeaveManagement;

namespace Architecture.BLL.HRM.LeaveManagement;

public class genEmployeeShortLeave
{
	public static long Save(Architecture.Model.HRM.LeaveManagement.genEmployeeShortLeave obj)
	{
		try
		{
			if (obj.EmployeeShortLeaveId == 0L)
			{
				return new Architecture.DAL.HRM.LeaveManagement.genEmployeeShortLeave().SetData(obj, "Sp_genEmployeeShortLeave_Insert");
			}
			return new Architecture.DAL.HRM.LeaveManagement.genEmployeeShortLeave().SetData(obj, "Sp_genEmployeeShortLeave_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.LeaveManagement.genEmployeeShortLeave GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeShortLeaveId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.LeaveManagement.genEmployeeShortLeave>();
			return new Architecture.DAL.HRM.LeaveManagement.genEmployeeShortLeave().GetAll("Sp_genEmployeeShortLeave_ReadAll", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.LeaveManagement.genEmployeeShortLeave obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.LeaveManagement.genEmployeeShortLeave>("Sp_genEmployeeShortLeave_ReadAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAvailShortLeavesByEmployee(Architecture.Model.HRM.LeaveManagement.genEmployeeShortLeave obj)
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
				ParameterName = "@EmployeeId",
				Value = obj.EmployeeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocDate",
				Value = obj.ApplyDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAvailShortLeavesByEmployee"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.LeaveManagement.genEmployeeShortLeave>("Sp_genEmployeeShortLeave_ReadAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
