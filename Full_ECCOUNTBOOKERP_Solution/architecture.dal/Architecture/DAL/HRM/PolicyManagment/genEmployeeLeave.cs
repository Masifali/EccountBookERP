using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.PolicyManagment;

namespace Architecture.DAL.HRM.PolicyManagment;

public class genEmployeeLeave
{
	public long SetData(Architecture.Model.HRM.PolicyManagment.genEmployeeLeave obj, string ProcName)
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
			foreach (genEmployeeLeaveDetail item in obj.genEmployeeLeaveDetailList)
			{
				item.EmployeeLeaveId = obj.EmployeeLeaveId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_genEmployeeLeaveDetail_Insert"));
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

	public List<Architecture.Model.HRM.PolicyManagment.genEmployeeLeave> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.PolicyManagment.genEmployeeLeave> lst = new List<Architecture.Model.HRM.PolicyManagment.genEmployeeLeave>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.HRM.PolicyManagment.genEmployeeLeave>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.HRM.PolicyManagment.genEmployeeLeave item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@EmployeeLeaveId", item.EmployeeLeaveId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.genEmployeeLeaveDetailList = GenericProvider.GetAllDetail<genEmployeeLeaveDetail>(sqlcon, "Sp_genEmployeeLeave_ReadAll", lstPrm);
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
