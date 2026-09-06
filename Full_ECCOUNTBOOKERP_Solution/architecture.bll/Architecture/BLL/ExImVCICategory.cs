using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class ExImVCICategory
{
	public static int save(Architecture.Model.ExImVCICategory cty)
	{
		try
		{
			if (cty.Id == 0)
			{
				return new Architecture.DAL.ExImVCICategory().SetData(cty, "USP_ExImVCICategory_Insert");
			}
			return new Architecture.DAL.ExImVCICategory().SetData(cty, "USP_ExImVCICategory_Update");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAll(Architecture.Model.ExImVCICategory obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.ExImVCICategory>("USP_ExImVCICategory_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
