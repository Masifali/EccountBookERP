using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.OverTimeManagement;

namespace Architecture.DAL.HRM.OverTimeManagement;

public class OverTimeRequest
{
	public long SetData(Architecture.Model.HRM.OverTimeManagement.OverTimeRequest obj, string ProcName)
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
				obj.OverTimeRequestId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.OverTimeRequestId;
			}
			foreach (OverTimeRequestDetail item in obj.OverTimeRequestDetailList)
			{
				item.OverTimeRequestId = obj.OverTimeRequestId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "hrm.Sp_OverTimeRequestDetail_Insert"));
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

	public List<Architecture.Model.HRM.OverTimeManagement.OverTimeRequest> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.OverTimeManagement.OverTimeRequest> lst = new List<Architecture.Model.HRM.OverTimeManagement.OverTimeRequest>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.HRM.OverTimeManagement.OverTimeRequest>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.HRM.OverTimeManagement.OverTimeRequest item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@OverTimeRequestId", item.OverTimeRequestId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.OverTimeRequestDetailList = GenericProvider.GetAllDetail<OverTimeRequestDetail>(sqlcon, "hrm.Sp_OverTimeRequest_GetAllMethod", lstPrm);
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
