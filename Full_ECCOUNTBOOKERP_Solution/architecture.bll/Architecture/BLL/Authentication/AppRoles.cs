using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Authentication;
using Architecture.DAL.Common;
using Architecture.Model.Authentication;

namespace Architecture.BLL.Authentication;

public class AppRoles
{
	public static int Save(Architecture.Model.Authentication.AppRoles obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Authentication.AppRoles().SetData(obj, "Proc_AppRoles_Insert");
			}
			return new Architecture.DAL.Authentication.AppRoles().SetData(obj, "Proc_AppRoles_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Authentication.AppRoles obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Authentication.AppRoles>("[Sp_AppRoles_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
