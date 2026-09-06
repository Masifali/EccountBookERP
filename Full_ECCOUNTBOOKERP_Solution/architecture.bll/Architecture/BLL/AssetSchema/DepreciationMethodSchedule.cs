using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.AssetSchema;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.AssetSchema;

namespace Architecture.BLL.AssetSchema;

public class DepreciationMethodSchedule
{
	public static int Save(Architecture.Model.AssetSchema.DepreciationMethodSchedule obj)
	{
		try
		{
			_ = obj.depreciationMethodScheduleId;
			return new Architecture.DAL.AssetSchema.DepreciationMethodSchedule().SetData(obj, "");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.AssetSchema.DepreciationMethodSchedule GetById(int Id)
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
			return new Architecture.DAL.AssetSchema.DepreciationMethodSchedule().GetAll("", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(ReportsParameters obj)
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.AssetSchema.DepreciationMethodSchedule>("Sp_FixedAssetsCategory_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetActiveDepreciationMethodSchedule(int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
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
				Value = "GetActiveDepreciationMethodSchedule"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.AssetSchema.DepreciationMethodSchedule>("Asset.USP_DepreciationMethodSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
