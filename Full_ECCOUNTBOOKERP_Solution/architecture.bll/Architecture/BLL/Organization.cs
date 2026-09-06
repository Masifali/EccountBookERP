using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class Organization
{
	public static int Save(Architecture.Model.Organization obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Organization().SetData(obj, "Sp_Organization_Insert");
			}
			return new Architecture.DAL.Organization().SetData(obj, "Sp_Organization_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Organization GetByID(int Id)
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
			new List<Architecture.Model.Organization>();
			return new Architecture.DAL.Organization().GetData("Sp_Organization_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Organization>("Sp_Organization_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetRemainingCompaniesCount(int Id)
	{
		DataTable dt = null;
		try
		{
			int RemainingCompanies = 0;
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetRemainingCompaniesCount"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Organization>("Sp_Organization_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				RemainingCompanies = Conversion.ToInt(dt.Rows[0]["RemainingCompanies"]);
			}
			return RemainingCompanies;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
