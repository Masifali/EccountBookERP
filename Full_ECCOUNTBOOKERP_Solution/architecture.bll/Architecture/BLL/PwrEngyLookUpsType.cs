using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class PwrEngyLookUpsType
{
	public static DataTable GetPwrLookUpsType()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetPwrLookUpsType"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Company>("SP_PwrEngyLookUpsType_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPwrLookUpsByTypeId(int OrganizationId, int CompanyId, int LookupTypeId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (LookupTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = LookupTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetPwrLookUpsByTypeId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Company>("SP_PwrEngyLookUpsType_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
