using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class City
{
	public static int save(Architecture.Model.City cty)
	{
		try
		{
			cty.EntryDate = DateTime.Now;
			cty.ModifyDate = DateTime.Now;
			if (cty.Id == 0)
			{
				return new Architecture.DAL.City().SetData(cty, "Sp_City_Insert");
			}
			return new Architecture.DAL.City().SetData(cty, "Sp_City_Update");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAll(Architecture.Model.City obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@MethodType", "GetAll"));
			return GenericProvider.GetDataTableProc<Architecture.Model.City>("SP_City_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.City GetById(int id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@MethodType",
				Value = "ReadById"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = id
			});
			new List<Architecture.Model.City>();
			return new Architecture.DAL.City().GetDate("SP_City_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAllByCountryId(Architecture.Model.City obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@CountryId", obj.CountryId));
			p.Add(new SqlParameter("@MethodType", "ReadByOrganizationCompanyId"));
			return GenericProvider.GetDataTableProc<Architecture.Model.City>("SP_City_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDistrictAndTehsilByCityId(Architecture.Model.City obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@Id", obj.Id));
			p.Add(new SqlParameter("@MethodType", "GetDistrictAndTehsilByCityId"));
			return GenericProvider.GetDataTableProc<Architecture.Model.City>("SP_City_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
