using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.EmployeeManagement;

namespace Architecture.DAL.HRM.EmployeeManagement;

public class genShift
{
	public int SetData(Architecture.Model.HRM.EmployeeManagement.genShift obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.ShiftId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.ShiftId;
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

	public List<Architecture.Model.HRM.EmployeeManagement.genShift> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.EmployeeManagement.genShift> lst = new List<Architecture.Model.HRM.EmployeeManagement.genShift>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.HRM.EmployeeManagement.genShift>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
