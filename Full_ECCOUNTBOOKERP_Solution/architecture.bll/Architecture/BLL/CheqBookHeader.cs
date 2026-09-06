using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class CheqBookHeader
{
	public static int Save(Architecture.Model.CheqBookHeader obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.CheqBookHeader().SetDate(obj, "SP_CheqBookHeader_Insert");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetData(Architecture.Model.CheqBookHeader obj)
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
				ParameterName = "@MethodType",
				Value = "GetAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CheqBookHeader>("SP_CheqBookHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
