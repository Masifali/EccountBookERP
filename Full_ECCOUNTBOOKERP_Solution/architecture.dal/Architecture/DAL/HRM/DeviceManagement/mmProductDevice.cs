using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.DeviceManagement;

namespace Architecture.DAL.HRM.DeviceManagement;

public class mmProductDevice
{
	public int SetData(Architecture.Model.HRM.DeviceManagement.mmProductDevice obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.ProductDeviceId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.ProductDeviceId;
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

	public List<Architecture.Model.HRM.DeviceManagement.mmProductDevice> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.DeviceManagement.mmProductDevice> lst = new List<Architecture.Model.HRM.DeviceManagement.mmProductDevice>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.HRM.DeviceManagement.mmProductDevice>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
