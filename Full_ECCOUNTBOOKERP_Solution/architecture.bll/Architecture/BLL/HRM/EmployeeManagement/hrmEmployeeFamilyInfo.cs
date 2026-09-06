using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.EmployeeManagement;
using Architecture.Model.HRM.EmployeeManagement;

namespace Architecture.BLL.HRM.EmployeeManagement;

public class hrmEmployeeFamilyInfo
{
	public static int Save(Architecture.Model.HRM.EmployeeManagement.hrmEmployeeFamilyInfo obj)
	{
		try
		{
			if (obj.EmployeeFamilyInfoId == 0)
			{
				return new Architecture.DAL.HRM.EmployeeManagement.hrmEmployeeFamilyInfo().SetData(obj, "Sp_hrmEmployeeFamilyInfo_Insert");
			}
			return new Architecture.DAL.HRM.EmployeeManagement.hrmEmployeeFamilyInfo().SetData(obj, "Sp_hrmEmployeeFamilyInfo_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.EmployeeManagement.hrmEmployeeFamilyInfo GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeFamilyInfoId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeFamilyInfo>();
			return new Architecture.DAL.HRM.EmployeeManagement.hrmEmployeeFamilyInfo().GetAll("Sp_hrmEmployeeFamilyInfo_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.EmployeeManagement.hrmEmployeeFamilyInfo obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeFamilyInfo>("Sp_hrmEmployeeFamilyInfo_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
