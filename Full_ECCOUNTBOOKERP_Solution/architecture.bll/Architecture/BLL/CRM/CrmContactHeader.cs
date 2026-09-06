using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.CRM;
using Architecture.DAL.Common;
using Architecture.Model.CRM;

namespace Architecture.BLL.CRM;

public class CrmContactHeader
{
	public static int Save(Architecture.Model.CRM.CrmContactHeader obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.CRM.CrmContactHeader().SetData(obj, "Sp_CrmContactHeader_Insert");
			}
			return new Architecture.DAL.CRM.CrmContactHeader().SetData(obj, "Sp_CrmContactHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.CRM.CrmContactHeader GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.CRM.CrmContactHeader>();
			return new Architecture.DAL.CRM.CrmContactHeader().GetData("SP_CrmContactHeader_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.CRM.CrmContactHeader obj)
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
			new List<Architecture.Model.CRM.CrmContactHeader>();
			return GenericProvider.GetDataTableProc<Architecture.Model.CRM.CrmContactHeader>("Sp_CrmContactHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetBySearch(Architecture.Model.CRM.CrmContactHeader obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@Activity", "GetByCompanyName"));
			return GenericProvider.GetDataTableProc<Architecture.Model.CRM.CrmContactHeader>("SP_CrmContactHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GenrateCode(Architecture.Model.CRM.CrmContactHeader obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@crmContactGroupId", obj.crmContactGroupId));
			p.Add(new SqlParameter("@Activity", "GenerateContactCode"));
			return GenericProvider.GetDataTableProc<Architecture.Model.CRM.CrmContactHeader>("SP_CrmContactHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
