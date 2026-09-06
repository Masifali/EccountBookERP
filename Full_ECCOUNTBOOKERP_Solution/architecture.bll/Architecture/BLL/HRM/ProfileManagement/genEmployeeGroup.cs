using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.ProfileManagement;
using Architecture.Model.HRM.ProfileManagement;

namespace Architecture.BLL.HRM.ProfileManagement;

public class genEmployeeGroup
{
	public static int Save(Architecture.Model.HRM.ProfileManagement.genEmployeeGroup obj)
	{
		try
		{
			if (obj.EmployeeGroupId == 0)
			{
				return new Architecture.DAL.HRM.ProfileManagement.genEmployeeGroup().SetData(obj, "Sp_genEmployeeGroup_Insert");
			}
			return new Architecture.DAL.HRM.ProfileManagement.genEmployeeGroup().SetData(obj, "Sp_genEmployeeGroup_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.ProfileManagement.genEmployeeGroup GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeGroupId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.ProfileManagement.genEmployeeGroup>();
			return new Architecture.DAL.HRM.ProfileManagement.genEmployeeGroup().GetAll("Sp_genEmployeeGroup_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.ProfileManagement.genEmployeeGroup obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrginizationId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.ProfileManagement.genEmployeeGroup>("Sp_genEmployeeGroup_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
