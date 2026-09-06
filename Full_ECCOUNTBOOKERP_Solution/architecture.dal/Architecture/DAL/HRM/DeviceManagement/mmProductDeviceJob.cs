using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.DeviceManagement;

namespace Architecture.DAL.HRM.DeviceManagement;

public class mmProductDeviceJob
{
	public int SetData(Architecture.Model.HRM.DeviceManagement.mmProductDeviceJob obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.HRM.DeviceManagement.mmProductDeviceJob item in obj.MmProductDeviceJobsList)
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

	public List<Architecture.Model.HRM.DeviceManagement.mmProductDeviceJob> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.DeviceManagement.mmProductDeviceJob> lst = new List<Architecture.Model.HRM.DeviceManagement.mmProductDeviceJob>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.HRM.DeviceManagement.mmProductDeviceJob>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
