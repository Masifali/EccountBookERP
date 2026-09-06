using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.PayrollManagement;

namespace Architecture.DAL.HRM.PayrollManagement;

public class EmployeeAllowance
{
	public long SetData(Architecture.Model.HRM.PayrollManagement.EmployeeAllowance obj, string ProcName)
	{
		long returnID = 0L;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.HRM.PayrollManagement.EmployeeAllowance item in obj.EmployeeAllowanceList)
			{
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public List<Architecture.Model.HRM.PayrollManagement.EmployeeAllowance> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.PayrollManagement.EmployeeAllowance> lst = new List<Architecture.Model.HRM.PayrollManagement.EmployeeAllowance>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<Architecture.Model.HRM.PayrollManagement.EmployeeAllowance>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
