using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.PayrollManagement;

namespace Architecture.DAL.HRM.PayrollManagement;

public class EmployeeShortAdjustment
{
	public long SetData(Architecture.Model.HRM.PayrollManagement.EmployeeShortAdjustment obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			long returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.EmployeeShortAdjustmentId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.EmployeeShortAdjustmentId;
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

	public List<Architecture.Model.HRM.PayrollManagement.EmployeeShortAdjustment> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.PayrollManagement.EmployeeShortAdjustment> lst = new List<Architecture.Model.HRM.PayrollManagement.EmployeeShortAdjustment>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.HRM.PayrollManagement.EmployeeShortAdjustment>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
