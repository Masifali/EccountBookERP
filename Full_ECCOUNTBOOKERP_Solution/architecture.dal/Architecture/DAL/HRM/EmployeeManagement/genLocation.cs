using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.EmployeeManagement;

namespace Architecture.DAL.HRM.EmployeeManagement;

public class genLocation
{
	public int SetData(Architecture.Model.HRM.EmployeeManagement.genLocation obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.LocationId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.LocationId;
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

	public List<Architecture.Model.HRM.EmployeeManagement.genLocation> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.EmployeeManagement.genLocation> lst = new List<Architecture.Model.HRM.EmployeeManagement.genLocation>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.HRM.EmployeeManagement.genLocation>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
