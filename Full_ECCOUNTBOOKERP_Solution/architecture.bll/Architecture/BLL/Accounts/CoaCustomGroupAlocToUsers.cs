using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class CoaCustomGroupAlocToUsers
{
	public static int Save(Architecture.Model.Accounts.CoaCustomGroupAlocToUsers obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Accounts.CoaCustomGroupAlocToUsers().SetData(obj, "[dbo].[USP_CoaCustomGroupAlocToUsers_Insert]");
			}
			return new Architecture.DAL.Accounts.CoaCustomGroupAlocToUsers().SetData(obj, "");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(string Ids, int UserId, int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CustomGroupIds",
				Value = Ids
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = UserId
			});
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
			GenericProvider.GetDataTableProc<Architecture.Model.Accounts.CoaCustomGroupAlocToUsers>("[dbo].[USp_CoaCustomGroupAlocToUserDeleteById]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllocatedCutomGroupsToUser(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.CoaCustomGroupAlocToUsers>("[dbo].[USP-GetAllCoaCustomGroupAlocToUser]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetUnAllocatedCutomGroupsToUser(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.CoaCustomGroupAlocToUsers>("[dbo].[USP-GetAllCoaCustomGroupNotAlocToUser]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
