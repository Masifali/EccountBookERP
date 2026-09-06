using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class WarehousesAllocationToBranch
{
	public static int Save(Architecture.Model.WarehousesAllocationToBranch obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.WarehousesAllocationToBranch().SetData(obj, "[dbo].[USP_WarehousesAllocationToBranch_Insert]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int BranchId, string WarehouseIds)
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
				ParameterName = "@WarehouseIds",
				Value = WarehouseIds
			});
			GenericProvider.GetDataTableProc<Architecture.Model.WarehousesAllocationToBranch>("[dbo].[USP_WarehousesAllocationToBranchDeleteById]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable WarehousesAllocatedOrUnAllocatedToBranch(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.WarehousesAllocationToBranch>("[dbo].[USP_WarehousesAllocatedOrUnAllocatedToBranch]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetWarehousesAllocatedToBranchByBranchId(ReportsParameters obj)
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
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchId",
					Value = obj.BranchesId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.WarehousesAllocationToBranch>("[dbo].[USP_GetWarehousesAllocatedToBranch]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetWarehousesFromTransactionAndAllocatedToBranchPM(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.WarehousesAllocationToBranch>("USP_GetWarehousesFromTransactionAndAllocatedToBranchPM", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
