using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.LoanManagement;

namespace Architecture.DAL.HRM.LoanManagement;

public class EmployeeAdvance
{
	public int SetData(Architecture.Model.HRM.LoanManagement.EmployeeAdvance obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.EmployeeAdvanceId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.EmployeeAdvanceId;
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

	public List<Architecture.Model.HRM.LoanManagement.EmployeeAdvance> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.LoanManagement.EmployeeAdvance> lst = new List<Architecture.Model.HRM.LoanManagement.EmployeeAdvance>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.HRM.LoanManagement.EmployeeAdvance>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
