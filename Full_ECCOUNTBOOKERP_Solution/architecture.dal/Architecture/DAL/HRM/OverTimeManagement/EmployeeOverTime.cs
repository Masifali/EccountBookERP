using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.OverTimeManagement;

namespace Architecture.DAL.HRM.OverTimeManagement;

public class EmployeeOverTime
{
	public long SetData(Architecture.Model.HRM.OverTimeManagement.EmployeeOverTime obj, string ProcName)
	{
		long returnID = 0L;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.HRM.OverTimeManagement.EmployeeOverTime item in obj.EmployeeOverTimeList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public List<Architecture.Model.HRM.OverTimeManagement.EmployeeOverTime> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.OverTimeManagement.EmployeeOverTime> lst = new List<Architecture.Model.HRM.OverTimeManagement.EmployeeOverTime>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.HRM.OverTimeManagement.EmployeeOverTime>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
