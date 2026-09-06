using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class ItemAllocationToStoreRack
{
	public static int Save(Architecture.Model.Inventory.ItemAllocationToStoreRack obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.ItemAllocationToStoreRack().SetData(obj, "[dbo].[USP_ItemAllocationToStoreRack_InsertAndUpdate]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemsByRackAllocation(ReportsParameters obj)
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
				ParameterName = "@ActionId",
				Value = obj.ActionId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@StoreRackId",
				Value = obj.StoreRackId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemAllocationToStoreRack>("[usp_GetItemsByRackAllocation]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
