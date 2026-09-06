using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.DeviceManagement;
using Architecture.Model.HRM.DeviceManagement;

namespace Architecture.BLL.HRM.DeviceManagement;

public class mmProductDeviceJob
{
	public static int Save(Architecture.Model.HRM.DeviceManagement.mmProductDeviceJob obj)
	{
		try
		{
			if (obj.ProductDeviceId == 0)
			{
				return new Architecture.DAL.HRM.DeviceManagement.mmProductDeviceJob().SetData(obj, "Sp_mmProductDeviceJob_Insert");
			}
			return new Architecture.DAL.HRM.DeviceManagement.mmProductDeviceJob().SetData(obj, "Sp_mmProductDeviceJob_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.DeviceManagement.mmProductDeviceJob GetByID(long Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ProductDeviceJobId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.DeviceManagement.mmProductDeviceJob>();
			return new Architecture.DAL.HRM.DeviceManagement.mmProductDeviceJob().GetAll("Sp_mmProductDeviceJob_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.HRM.DeviceManagement.mmProductDeviceJob> Getall(Architecture.Model.HRM.DeviceManagement.mmProductDeviceJob obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			new List<Architecture.Model.HRM.DeviceManagement.mmProductDeviceJob>();
			return new Architecture.DAL.HRM.DeviceManagement.mmProductDeviceJob().GetAll("Sp_mmProductDeviceJob_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.HRM.DeviceManagement.mmProductDeviceJob> PendingDeviceJob(Architecture.Model.HRM.DeviceManagement.mmProductDeviceJob obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "PendingDeviceJob"
			});
			new List<Architecture.Model.HRM.DeviceManagement.mmProductDeviceJob>();
			return new Architecture.DAL.HRM.DeviceManagement.mmProductDeviceJob().GetAll("Sp_mmProductDeviceJob_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void AutoAttendancePull()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			GenericProvider.GetDataTableProc<Architecture.Model.HRM.DeviceManagement.mmProductDeviceJob>("USP_AUTOATTENDANCEPULLREQUESTGENERATE", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
