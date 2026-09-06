using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.AssetSchema;

namespace Architecture.BLL.AssetSchema;

public class Category
{
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
			return GenericProvider.GetDataTableProc<Architecture.Model.AssetSchema.Category>("[Asset].[USP_Category_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetCategoryForComboBind(int OrganizationId, int CompanyId)
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
				Value = "GetCategoryForComboBind"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.AssetSchema.Category>("[asset].[USP_Category_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAssetItemForComboBind(int OrganizationId, int CompanyId, int AssetCategoryId)
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
			if (AssetCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = AssetCategoryId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAssetItemForComboBind"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.AssetSchema.Category>("[asset].[USP_Category_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAssetCondition()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAssetCondition"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.AssetSchema.Category>("Asset.USP_Category_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
