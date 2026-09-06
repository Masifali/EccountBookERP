using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.DeviceManagement;

namespace Architecture.DAL.HRM.DeviceManagement;

public class hrmEmployeeBiometric
{
	public long SetData(Architecture.Model.HRM.DeviceManagement.hrmEmployeeBiometric obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			long returnID = Convert.ToInt64(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.EmployeeBiometricId = Convert.ToInt64(returnID);
			}
			else
			{
				returnID = obj.EmployeeBiometricId;
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

	public List<Architecture.Model.HRM.DeviceManagement.hrmEmployeeBiometric> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.DeviceManagement.hrmEmployeeBiometric> lst = new List<Architecture.Model.HRM.DeviceManagement.hrmEmployeeBiometric>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.HRM.DeviceManagement.hrmEmployeeBiometric>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
