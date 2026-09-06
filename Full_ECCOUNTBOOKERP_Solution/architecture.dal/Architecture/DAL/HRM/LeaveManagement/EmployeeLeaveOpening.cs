using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.LeaveManagement;

namespace Architecture.DAL.HRM.LeaveManagement;

public class EmployeeLeaveOpening
{
	public long SetData(Architecture.Model.HRM.LeaveManagement.EmployeeLeaveOpening obj, string ProcName)
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
				obj.EmployeeLeaveOpeningId = Convert.ToInt64(returnID);
			}
			else
			{
				returnID = obj.EmployeeLeaveOpeningId;
			}
			foreach (EmployeeLeaveQuota item in obj.EmployeeLeaveQuotasList)
			{
				item.EmployeeLeaveOpeningId = obj.EmployeeLeaveOpeningId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "hrm.Sp_EmployeeLeaveQuota_Insert"));
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.HRM.LeaveManagement.EmployeeLeaveOpening> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.LeaveManagement.EmployeeLeaveOpening> lst = new List<Architecture.Model.HRM.LeaveManagement.EmployeeLeaveOpening>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.HRM.LeaveManagement.EmployeeLeaveOpening>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.HRM.LeaveManagement.EmployeeLeaveOpening item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@EmployeeLeaveOpeningId", item.EmployeeLeaveOpeningId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.EmployeeLeaveQuotasList = GenericProvider.GetAllDetail<EmployeeLeaveQuota>(sqlcon, "hrm.Sp_EmployeeLeaveOpening_GetAllMethod", lstPrm);
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
