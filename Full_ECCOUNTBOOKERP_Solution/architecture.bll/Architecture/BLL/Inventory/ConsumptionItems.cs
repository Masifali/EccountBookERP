using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class ConsumptionItems
{
	public static int Save(Architecture.Model.Inventory.ConsumptionItems obj)
	{
		try
		{
			return new Architecture.DAL.Inventory.ConsumptionItems().SetData(obj, "USP_ConsumptionItems_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@organizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.CustomerGroup>("USP_ConsumptionItems_FormHistory", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemsAllocationForConsumption(int OrganizationId, int CompanyId, int ItemCategoryId = 0, int ItemTypeId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@organizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			if (ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = ItemCategoryId
				});
			}
			if (ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = ItemTypeId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.CustomerGroup>("USP_GetItemsAllocationForConsumption", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
