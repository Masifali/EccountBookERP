using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class UserAccountAllocation
{
	public static DataTable GetAllCompaniesByUserId(Architecture.Model.UserAccountAllocation obj)
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
				ParameterName = "@UserAccountId",
				Value = obj.UserAccountId
			});
			if (obj.IsActive != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsActive",
					Value = obj.IsActive
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetCompaniesByUserId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.UserAccount>("sp_UserAccountAllocation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
