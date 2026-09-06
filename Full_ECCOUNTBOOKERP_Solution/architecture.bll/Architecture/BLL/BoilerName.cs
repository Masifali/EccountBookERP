using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class BoilerName
{
	public static int save(Architecture.Model.InvBoilerName obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.InvBoilerName().SetData(obj, "Sp_Boiler_Insert");
			}
			return new Architecture.DAL.InvBoilerName().SetData(obj, "Sp_Boiler_Update");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAll(Architecture.Model.InvBoilerName obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.InvBoilerName>("[Sp_Boiler_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.InvBoilerName obj)
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
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.InvBoilerName>("[Sp_Boiler_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.InvBoilerName GetById(int id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = id
			});
			new List<Architecture.Model.InvBoilerName>();
			return new Architecture.DAL.InvBoilerName().GetData("Sp_Boiler_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
