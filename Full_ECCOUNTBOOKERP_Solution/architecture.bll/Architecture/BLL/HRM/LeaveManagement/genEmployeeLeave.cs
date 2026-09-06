using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.LeaveManagement;
using Architecture.Model;
using Architecture.Model.HRM.LeaveManagement;

namespace Architecture.BLL.HRM.LeaveManagement;

public class genEmployeeLeave
{
	public static long Save(Architecture.Model.HRM.LeaveManagement.genEmployeeLeave obj)
	{
		try
		{
			if (obj.EmployeeLeaveId == 0L)
			{
				return new Architecture.DAL.HRM.LeaveManagement.genEmployeeLeave().SetData(obj, "Sp_genEmployeeLeave_Insert");
			}
			return new Architecture.DAL.HRM.LeaveManagement.genEmployeeLeave().SetData(obj, "Sp_genEmployeeLeave_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.LeaveManagement.genEmployeeLeave GetByID(int Id)
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
				ParameterName = "@EmployeeLeaveId",
				Value = Id
			});
			new List<Architecture.Model.HRM.LeaveManagement.genEmployeeLeave>();
			return new Architecture.DAL.HRM.LeaveManagement.genEmployeeLeave().GetData("Sp_genEmployeeLeave_ReadAll", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAll(Architecture.Model.HRM.LeaveManagement.genEmployeeLeave obj)
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
			if (obj.LeaveTypeProfileId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LeaveTypeProfileId",
					Value = obj.LeaveTypeProfileId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.LeaveManagement.genEmployeeLeave>("Sp_genEmployeeLeave_ReadAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForLeaveApproval(ReportsParameters obj)
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
			if (obj.EmployeeId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeId",
					Value = obj.EmployeeId
				});
			}
			if (obj.DepartmentId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentId",
					Value = obj.DepartmentId
				});
			}
			if (obj.DesignationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DesignationId",
					Value = obj.DesignationId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetDataForLeaveApproval"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.LeaveManagement.genEmployeeLeave>("Sp_genEmployeeLeave_ReadAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLeaveDetailByEmployeeLeaveId(ReportsParameters obj)
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
				ParameterName = "@LeaveTypeProfileId",
				Value = obj.LeaveTypeProfileId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetLeaveDetailByEmployeeLeaveId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.LeaveManagement.genEmployeeLeave>("Sp_genEmployeeLeave_ReadAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetEmployeeLeaveDetailIdByHeaderId(int EmployeeLeaveId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeLeaveId",
				Value = EmployeeLeaveId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetEmployeeLeaveDetailIdByHeaderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.LeaveManagement.genEmployeeLeave>("Sp_genEmployeeLeave_ReadAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void LeaveApproved(LeaveApproval obj)
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
				ParameterName = "@EmployeeLeaveQuotaId",
				Value = obj.LeaveQuotaId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@LeaveTypeProfileId",
				Value = obj.LeaveTypeProfileId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeId",
				Value = obj.EmployeeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EntryUser",
				Value = obj.EntryUser
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeLeaveDetailId",
				Value = obj.EmployeeLeaveDetailId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.ActionId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "LeaveApproved"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.HRM.LeaveManagement.genEmployeeLeave>("Sp_genEmployeeLeave_ReadAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void LeaveApprovedlist(LeaveApproval obj)
	{
		try
		{
			foreach (LeaveApproval item in obj.LeaveApprovalslist)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = item.OrganizationId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = item.CompanyId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeLeaveQuotaId",
					Value = item.LeaveQuotaId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@LeaveTypeProfileId",
					Value = item.LeaveTypeProfileId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeId",
					Value = item.EmployeeId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = item.EntryUser
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeLeaveDetailId",
					Value = item.EmployeeLeaveDetailId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = item.ActionId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "LeaveApproved"
				});
				GenericProvider.GetDataTableProc<Architecture.Model.HRM.LeaveManagement.genEmployeeLeave>("Sp_genEmployeeLeave_ReadAll", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
