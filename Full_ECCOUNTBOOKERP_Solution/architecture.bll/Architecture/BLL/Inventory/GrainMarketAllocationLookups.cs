using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class GrainMarketAllocationLookups
{
	public static int Save(Architecture.Model.Inventory.GrainMarketAllocationLookups obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.GrainMarketAllocationLookups().SetData(obj, "Sp_GrainMarketAllocationLookups_Insert");
			}
			return new Architecture.DAL.Inventory.GrainMarketAllocationLookups().SetData(obj, "Sp_GrainMarketAllocationLookups_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Inventory.GrainMarketAllocationLookups obj)
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
				ParameterName = "@AllocationTypeId",
				Value = obj.LookupTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByAllocationTypeId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.GrainMarketAllocationLookups>("Sp_GrainMarketAllocationLookups_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateStatus(Architecture.Model.Inventory.GrainMarketAllocationLookups obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Status",
				Value = obj.Active
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "UpdateStatus"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.Inventory.GrainMarketAllocationLookups>("Sp_GrainMarketAllocationLookups_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
