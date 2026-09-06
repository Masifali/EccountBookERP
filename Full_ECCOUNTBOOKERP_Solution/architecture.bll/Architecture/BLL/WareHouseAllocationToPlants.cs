using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class WareHouseAllocationToPlants
{
	public static int Save(Architecture.Model.WareHouseAllocationToPlants obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.WareHouseAllocationToPlants().SetData(obj, "[dbo].[USP_WareHouseAllocationToPlants_Insert]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int PlantId, string WarehouseIds)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@PlantId",
				Value = PlantId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@WarehouseIds",
				Value = WarehouseIds
			});
			GenericProvider.GetDataTableProc<Architecture.Model.WareHouseAllocationToPlants>("[dbo].[USP_WareHouseAllocationToPlantsDeleteById]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllocatedAndUnAllocatedWarehouse(ReportsParameters obj)
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
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PlantId",
				Value = obj.PlantId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.ActionId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.WareHouseAllocationToPlants>("[dbo].[USP_WarehouseAllocatedOrUnAllocatedToPlant]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool ValidateWareHouseAgainstPlantFromWareHouseAllocationToPlants(int OrganizationId, int CompanyId, int WarehouseId, int PlantId)
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
				ParameterName = "@WarehouseId",
				Value = WarehouseId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PlantId",
				Value = PlantId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.WareHouseAllocationToPlants>("[dbo].[USP_ValidateWareHouseAgainstPlantFromWareHouseAllocationToPlants]", p).Rows.Count > 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
