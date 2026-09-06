using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class InventoryItemsOther
{
	public static int Save(Architecture.Model.Inventory.InventoryItemsOther io)
	{
		try
		{
			if (io.Id == 0)
			{
				return new Architecture.DAL.Inventory.InventoryItemsOther().SetData(io, "Sp_InventoryItemsOther_Insert");
			}
			return new Architecture.DAL.Inventory.InventoryItemsOther().SetData(io, "Sp_InventoryItemsOther_Update");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static Architecture.Model.Inventory.InventoryItemsOther GetById(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			List<Architecture.Model.Inventory.InventoryItemsOther> lst = new List<Architecture.Model.Inventory.InventoryItemsOther>();
			lst = new Architecture.DAL.Inventory.InventoryItemsOther().GetAll("Sp_InventoryItemsOther_GetAllMethod", p);
			return (lst != null && lst.Count > 0) ? lst[0] : null;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAll(Architecture.Model.Inventory.InventoryItemsOther obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@organizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InventoryItemsOther>("Sp_InventoryItemsOther_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
