using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class country
{
	public static int save(Architecture.Model.Country ctry)
	{
		try
		{
			if (ctry.Id == 0)
			{
				return new Architecture.DAL.Country().SetData(ctry, "Sp_Country_Insert");
			}
			return new Architecture.DAL.Country().SetData(ctry, "Sp_Country_Update");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAll(Architecture.Model.Country obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@MethodType", "GetAll"));
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Country>("SP_Country_ReadMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Country GetById(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@MethodType",
				Value = "GetById"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			new List<Architecture.Model.Country>();
			return new Architecture.DAL.Country().GetAll("SP_Country_ReadMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
