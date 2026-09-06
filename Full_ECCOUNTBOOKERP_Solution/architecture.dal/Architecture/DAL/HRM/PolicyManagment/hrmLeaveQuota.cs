using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.PolicyManagment;

namespace Architecture.DAL.HRM.PolicyManagment;

public class hrmLeaveQuota
{
	public int SetData(Architecture.Model.HRM.PolicyManagment.hrmLeaveQuota obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.LeaveQuotaId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.LeaveQuotaId;
			}
			foreach (hrmLeaveQuotaDetail item in obj.HrmLeaveQuotaDetailList)
			{
				item.LeaveQuotaId = obj.LeaveQuotaId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_hrmLeaveQuotaDetail_Insert"));
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

	public List<Architecture.Model.HRM.PolicyManagment.hrmLeaveQuota> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.PolicyManagment.hrmLeaveQuota> lst = new List<Architecture.Model.HRM.PolicyManagment.hrmLeaveQuota>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.HRM.PolicyManagment.hrmLeaveQuota>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.HRM.PolicyManagment.hrmLeaveQuota item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@LeaveQuotaDetailId", item.LeaveQuotaId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadById"));
					item.HrmLeaveQuotaDetailList = GenericProvider.Get<hrmLeaveQuotaDetail>(sqlcon, "Sp_hrmLeaveQuotaDetail_GetAllMethod", lstPrm);
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
