using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class CoaCustomGroupForUsers
{
	public static int save(Architecture.Model.Accounts.CoaCustomGroupForUsers obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Accounts.CoaCustomGroupForUsers().SetData(obj, "[dbo].[USP_CoaCustomGroupForUsers_Insert]");
			}
			return new Architecture.DAL.Accounts.CoaCustomGroupForUsers().SetData(obj, "[dbo].[USP_CoaCustomGroupForUsers_Update]");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAll(ReportsParameters obj)
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
			if (obj.UserId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = obj.UserId
				});
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsActive",
					Value = obj.IsApproved
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.CoaCustomGroupForUsers>("[dbo].[USP_CoaCustomGroupForUsers_ReadAll]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable BindCustomGroupForAlocToCoa(ReportsParameters obj)
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
			if (obj.UserId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = obj.UserId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.CoaCustomGroupForUsers>("[dbo].[USP_BindCustomGroupForAlocToCoa]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
