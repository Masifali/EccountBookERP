using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.DeviceManagement;
using Architecture.Model.HRM.DeviceManagement;

namespace Architecture.BLL.HRM.DeviceManagement;

public class hrmEmployeeBiometric
{
	public static long Save(Architecture.Model.HRM.DeviceManagement.hrmEmployeeBiometric obj)
	{
		try
		{
			if (obj.EmployeeBiometricId == 0L)
			{
				return new Architecture.DAL.HRM.DeviceManagement.hrmEmployeeBiometric().SetData(obj, "Sp_hrmEmployeeBiometric_Insert");
			}
			return 0L;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.DeviceManagement.hrmEmployeeBiometric GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeBiometricId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.DeviceManagement.hrmEmployeeBiometric>();
			return new Architecture.DAL.HRM.DeviceManagement.hrmEmployeeBiometric().GetAll("Sp_hrmEmployeeBiometric_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.DeviceManagement.hrmEmployeeBiometric obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.DeviceManagement.hrmEmployeeBiometric>("Sp_hrmEmployeeBiometric_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.DeviceManagement.hrmEmployeeBiometric GetByEmployeeNo(string EmployeeNo)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			Architecture.Model.HRM.DeviceManagement.hrmEmployeeBiometric obj = new Architecture.Model.HRM.DeviceManagement.hrmEmployeeBiometric();
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeNo",
				Value = EmployeeNo
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetByEmployeeNo"
			});
			List<Architecture.Model.HRM.DeviceManagement.hrmEmployeeBiometric> lst = new List<Architecture.Model.HRM.DeviceManagement.hrmEmployeeBiometric>();
			lst = new Architecture.DAL.HRM.DeviceManagement.hrmEmployeeBiometric().GetAll("Sp_hrmEmployeeBiometric_GetAllMethod", p);
			if (lst.Count == 0)
			{
				return obj;
			}
			return lst[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByEmployeeNo(string EmployeeNo)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "DeleteByEmployeeNo"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeNo",
				Value = EmployeeNo
			});
			GenericProvider.GetDataTableProc<Architecture.Model.HRM.DeviceManagement.hrmEmployeeBiometric>("Sp_hrmEmployeeBiometric_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
