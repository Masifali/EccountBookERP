using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class ItemAllocateToWareHouse
{
	public static int Save(Architecture.Model.Inventory.ItemAllocateToWareHouse obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.ItemAllocateToWareHouse().SetData(obj, "Sp_ItemAllocateToWareHouse_Insert");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.ItemAllocateToWareHouse GetByID(int Id)
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
			new List<Architecture.Model.Inventory.ItemAllocateToWareHouse>();
			return new Architecture.DAL.Inventory.ItemAllocateToWareHouse().GetData("Sp_InvWareHouse_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string GetWarehouseIdsbyItemId(ReportsParameters obj)
	{
		try
		{
			string Code = string.Empty;
			DataTable dt = null;
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
				ParameterName = "@ItemIds",
				Value = obj.Ids
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetWarehouseIdsbyItemId"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemAllocateToWareHouse>("Sp_ItemAllocateToWareHouse_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					Code = Code + "," + Conversion.ToString(dt.Rows[i]["WarehouseId"]);
				}
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemsFromWarehouseAllocationByWareHouseId(Architecture.Model.Inventory.ItemAllocateToWareHouse obj)
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
				ParameterName = "@WarehouseId",
				Value = obj.WarehouseId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetItemsFromWarehouseAllocationByWareHouseId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemAllocateToWareHouse>("Sp_ItemAllocateToWareHouse_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
