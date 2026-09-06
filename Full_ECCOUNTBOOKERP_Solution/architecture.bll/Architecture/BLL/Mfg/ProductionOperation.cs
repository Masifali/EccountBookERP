using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Mfg;
using Architecture.Model.Mfg;

namespace Architecture.BLL.Mfg;

public class ProductionOperation
{
	public static int Save(Architecture.Model.Mfg.ProductionOperation obj)
	{
		try
		{
			if (obj.ProductionOperationId == 0)
			{
				return new Architecture.DAL.Mfg.ProductionOperation().SetData(obj, "[Mfg].[USP_ProductionOperation_Insert]");
			}
			return new Architecture.DAL.Mfg.ProductionOperation().SetData(obj, "[Mfg].[USP_ProductionOperation_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Mfg.ProductionOperation obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Mfg.ProductionOperation>("[Mfg].[USP_ProductionOperation_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Mfg.ProductionOperation> FormHistoryWithList(Architecture.Model.Mfg.ProductionOperation obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			new List<Architecture.Model.Mfg.ProductionOperation>();
			return new Architecture.DAL.Mfg.ProductionOperation().GetAll("[Mfg].[USP_ProductionOperation_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Mfg.ProductionOperation ReadById(int Id)
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
			new List<Architecture.Model.Mfg.ProductionOperation>();
			return new Architecture.DAL.Mfg.ProductionOperation().GetAll("[Mfg].[USP_ProductionOperation_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
