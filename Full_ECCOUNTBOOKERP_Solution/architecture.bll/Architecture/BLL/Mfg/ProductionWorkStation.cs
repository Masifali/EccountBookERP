using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Mfg;
using Architecture.Model.Mfg;

namespace Architecture.BLL.Mfg;

public class ProductionWorkStation
{
	public static int Save(Architecture.Model.Mfg.ProductionWorkStation obj)
	{
		try
		{
			if (obj.ProductionWorkStationId == 0)
			{
				return new Architecture.DAL.Mfg.ProductionWorkStation().SetData(obj, "[Mfg].[USP_ProductionWorkStation_Insert]");
			}
			return new Architecture.DAL.Mfg.ProductionWorkStation().SetData(obj, "[Mfg].[USP_ProductionWorkStation_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Mfg.ProductionWorkStation obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Mfg.ProductionWorkStation>("[Mfg].[USP_ProductionWorkStation_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllWorkStationByDepartmentId(Architecture.Model.Mfg.ProductionWorkStation obj)
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
				ParameterName = "@DepartmentId",
				Value = obj.DepartmentId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAllWorkStationByDepartmentId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Mfg.ProductionWorkStation>("[Mfg].[USP_ProductionWorkStation_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Mfg.ProductionWorkStation> FormHistoryWithList(Architecture.Model.Mfg.ProductionWorkStation obj)
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
			new List<Architecture.Model.Mfg.ProductionWorkStation>();
			return new Architecture.DAL.Mfg.ProductionWorkStation().GetAll("[Mfg].[USP_ProductionWorkStation_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Mfg.ProductionWorkStation ReadById(int Id)
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
			new List<Architecture.Model.Mfg.ProductionWorkStation>();
			return new Architecture.DAL.Mfg.ProductionWorkStation().GetAll("[Mfg].[USP_ProductionWorkStation_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
