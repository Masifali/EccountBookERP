using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.SystemUtilities;
using Architecture.Model;
using Architecture.Model.SystemUtilities;

namespace Architecture.BLL.SystemUtilities;

public class MultiLanguages
{
	public static int save(Architecture.Model.SystemUtilities.MultiLanguages obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.SystemUtilities.MultiLanguages().SetData(obj, "Sp_MultiLanguages_Insert");
			}
			return new Architecture.DAL.SystemUtilities.MultiLanguages().SetData(obj, "Sp_MultiLanguages_Update");
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
			p.Add(new SqlParameter("@MethodType", "ReadAll"));
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			return GenericProvider.GetDataTableProc<Architecture.Model.City>("Sp_MultiLanguages_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetUnAllocatedAccountsByLanguageId(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@MethodType", "GetUnAllocatedAccountsByLanguageId"));
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			return GenericProvider.GetDataTableProc<Architecture.Model.City>("Sp_MultiLanguages_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
