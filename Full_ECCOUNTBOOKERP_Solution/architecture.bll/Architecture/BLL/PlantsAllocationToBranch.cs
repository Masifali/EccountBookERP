using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class PlantsAllocationToBranch
{
	public static int Save(Architecture.Model.PlantsAllocationToBranch obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.PlantsAllocationToBranch().SetData(obj, "[dbo].[USP_PlantsAllocationToBranch_Insert]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int BranchId, string PlantIds)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchId",
				Value = BranchId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PlantIds",
				Value = PlantIds
			});
			GenericProvider.GetDataTableProc<Architecture.Model.PlantsAllocationToBranch>("[dbo].[USP_PlantsAllocationToBranchDeleteById]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PlantsAllocatedOrUnAllocatedToBranch(ReportsParameters obj)
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
				ParameterName = "@BranchId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.ActionId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.PlantsAllocationToBranch>("[dbo].[USP_PlantsAllocatedOrUnAllocatedToBranch]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPlantsAllocatedToBranch(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.PlantsAllocationToBranch>("[dbo].[USP_GetPlantsAllocatedToBranch]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
