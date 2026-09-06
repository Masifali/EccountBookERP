using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class ExImVCIParameter
{
	public static int save(Architecture.Model.ExImVCIParameter obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.ExImVCIParameter().SetData(obj, "USP_ExImVCIParameter_Insert");
			}
			return new Architecture.DAL.ExImVCIParameter().SetData(obj, "USP_ExImVCIParameter_Update");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAll(Architecture.Model.ExImVCIParameter obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.ExImVCICartegoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExImVCICartegoryId",
					Value = obj.ExImVCICartegoryId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ExImVCIParameter>("USP_ExImVCIParameter_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
