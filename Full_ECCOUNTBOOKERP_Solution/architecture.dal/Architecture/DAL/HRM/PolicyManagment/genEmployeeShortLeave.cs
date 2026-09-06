using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.PolicyManagment;

namespace Architecture.DAL.HRM.PolicyManagment;

public class genEmployeeShortLeave
{
	public long SetData(Architecture.Model.HRM.PolicyManagment.genEmployeeShortLeave obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			long returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.EmployeeShortLeaveId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.EmployeeShortLeaveId;
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

	public List<Architecture.Model.HRM.PolicyManagment.genEmployeeShortLeave> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.PolicyManagment.genEmployeeShortLeave> lst = new List<Architecture.Model.HRM.PolicyManagment.genEmployeeShortLeave>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.HRM.PolicyManagment.genEmployeeShortLeave>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
