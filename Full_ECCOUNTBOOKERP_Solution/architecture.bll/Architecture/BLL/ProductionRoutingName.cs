using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class ProductionRoutingName
{
	public static int Save(ProductionRouting obj)
	{
		try
		{
			if (obj.ProductionRoutingNameId == 0)
			{
				obj.ActionId = 1;
				return new Architecture.DAL.ProductionRoutingName().SetData(obj, "[Mfg].[USP_ProductionRoutingName_Insert]");
			}
			obj.ActionId = 2;
			return new Architecture.DAL.ProductionRoutingName().SetData(obj, "[Mfg].[USP_ProductionRoutingName_Insert]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<ProductionRouting> FormHistoryList(ProductionRouting obj)
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
			new List<ProductionRouting>();
			return new Architecture.DAL.ProductionRoutingName().GetAll("[Mfg].[USP_ProductionRoutingName_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(ProductionRouting obj)
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
			return GenericProvider.GetDataTableProc<ProductionRouting>("[Mfg].[USP_ProductionRoutingName_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static ProductionRouting ReadById(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ProductionRoutingNameId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<ProductionRouting>();
			return new Architecture.DAL.ProductionRoutingName().GetAll("[Mfg].[USP_ProductionRoutingName_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
