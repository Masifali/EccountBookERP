using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.LeaveManagement;

namespace Architecture.DAL.HRM.LeaveManagement;

public class genEmployeeLeave
{
	public long SetData(Architecture.Model.HRM.LeaveManagement.genEmployeeLeave obj, string ProcName)
	{
		long returnID = 0L;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.EmployeeLeaveId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.EmployeeLeaveId;
			}
			foreach (genEmployeeLeaveDetail item2 in obj.genEmployeeLeaveDetailList)
			{
				item2.EmployeeLeaveId = obj.EmployeeLeaveId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_genEmployeeLeaveDetail_Insert"));
			}
			foreach (CPLEmployeeLeaveDetail item in obj.CPLEmployeeLeaveDetailslist)
			{
				item.EmployeeLeaveId = obj.EmployeeLeaveId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_CPLEmployeeLeaveDetail_Insert"));
			}
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
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
		CommonProvider.ExecuteProcedure("[dbo].[Sp_SysUpdateAttendanceFromPull]", p);
		CommonProvider.ExecuteProcedure("[dbo].[Sp_SysUpdateAttendanceStatus]", p);
		return returnID;
	}

	public List<Architecture.Model.HRM.LeaveManagement.genEmployeeLeave> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.LeaveManagement.genEmployeeLeave> lst = new List<Architecture.Model.HRM.LeaveManagement.genEmployeeLeave>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.HRM.LeaveManagement.genEmployeeLeave>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.HRM.LeaveManagement.genEmployeeLeave item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@EmployeeLeaveId", item2.EmployeeLeaveId));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
						item2.genEmployeeLeaveDetailList = GenericProvider.GetAllDetail<genEmployeeLeaveDetail>(sqlConnection2, "Sp_genEmployeeLeave_ReadAll", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.HRM.LeaveManagement.genEmployeeLeave item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@EmployeeLeaveId", item.EmployeeLeaveId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadCPLLeaveDetailByHeaderId"));
					item.CPLEmployeeLeaveDetailslist = GenericProvider.GetAllDetail<CPLEmployeeLeaveDetail>(sqlcon, "Sp_genEmployeeLeave_ReadAll", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}
}
