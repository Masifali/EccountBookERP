using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Templates;
using Architecture.Model.Templates;

namespace Architecture.BLL.Templates;

public class UserModuleAdmin
{
	public static long Save(Architecture.Model.Templates.UserModuleAdmin obj)
	{
		try
		{
			if (obj.UserModuleAdminId == 0L)
			{
				return new Architecture.DAL.Templates.UserModuleAdmin().SetData(obj, "[dbo].[USP_UserModuleAdmin_InsertAndUpdate]");
			}
			return 0L;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable UserModuleAdmin_AllocatedData(int OrganizationId, int CompanyId, int UserId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = UserId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Templates.UserModuleAdmin>("[dbo].[USP_UserModuleAdmin_AllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable UserModuleAdmin_UnAllocatedData(int OrganizationId, int CompanyId, int UserId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = UserId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Templates.UserModuleAdmin>("[dbo].[USP_UserModuleAdmin_UnAllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
