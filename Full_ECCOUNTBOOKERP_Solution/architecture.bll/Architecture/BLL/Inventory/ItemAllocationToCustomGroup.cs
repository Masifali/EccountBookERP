using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class ItemAllocationToCustomGroup
{
	public static int Save(Architecture.Model.Inventory.ItemAllocationToCustomGroup obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.ItemAllocationToCustomGroup().SetData(obj, "[dbo].[USP_ItemAllocationToCustomGroup_Insert]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int CustomGroupId, string ItemIds)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemCustomGroupId",
				Value = CustomGroupId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemIds",
				Value = ItemIds
			});
			GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemAllocationToCustomGroup>("[dbo].[USp_ItemAllocationToCustomGroupDeleteById]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllocatedAndUnAllocatedItems(Architecture.Model.Inventory.ItemAllocationToCustomGroup obj)
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
				ParameterName = "@ItemCustomGroupId",
				Value = obj.ItemCustomGroupId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemAllocationToCustomGroup>("[dbo].[USP_ItemAllocatedOrUnAllocatedToItemCustomGroup]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
