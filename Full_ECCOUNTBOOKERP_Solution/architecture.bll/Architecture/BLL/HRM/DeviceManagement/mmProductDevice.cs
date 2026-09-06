using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.DeviceManagement;
using Architecture.Model.HRM.DeviceManagement;

namespace Architecture.BLL.HRM.DeviceManagement;

public class mmProductDevice
{
	public static int Save(Architecture.Model.HRM.DeviceManagement.mmProductDevice obj)
	{
		try
		{
			if (obj.ProductDeviceId == 0)
			{
				return new Architecture.DAL.HRM.DeviceManagement.mmProductDevice().SetData(obj, "Sp_mmProductDevice_Insert");
			}
			return new Architecture.DAL.HRM.DeviceManagement.mmProductDevice().SetData(obj, "Sp_mmProductDevice_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.DeviceManagement.mmProductDevice GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ProductDeviceId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.DeviceManagement.mmProductDevice>();
			return new Architecture.DAL.HRM.DeviceManagement.mmProductDevice().GetAll("Sp_mmProductDevice_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.DeviceManagement.mmProductDevice obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.DeviceManagement.mmProductDevice>("Sp_mmProductDevice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
