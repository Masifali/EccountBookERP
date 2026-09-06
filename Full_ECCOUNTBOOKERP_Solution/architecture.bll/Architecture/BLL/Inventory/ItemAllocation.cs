using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class ItemAllocation
{
	public static int Save(Architecture.Model.Inventory.ItemAllocation obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.ItemAllocation().SetData(obj, "Sp_ItemAllocation_Insert");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPendingForAllocation(Architecture.Model.Inventory.ItemAllocation obj)
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
				ParameterName = "@Activity",
				Value = "ItemPendingForAllocation"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemAllocation>("Sp_ItemAllocation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllocatedAndUnAllocatedItems(ReportsParameters obj)
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
				ParameterName = "@Activity",
				Value = "ItemsAllocatedOrUnAllocatedToCompany"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemAllocation>("[dbo].[Sp_ItemAllocation_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int OrganizationId, int CompanyId, string ItemIds)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemIds",
				Value = ItemIds
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "DeleteAllocatedItemsCompanyWise"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemAllocation>("[dbo].[Sp_ItemAllocation_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAllItemsFromAllocationWithStatusByCompanyId(ReportsParameters obj)
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
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Status",
					Value = obj.ItemStatus
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAllItemsFromAllocationWithStatusByCompanyId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.Item>("Sp_Item_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable UpdateItemsStatusInAllocation(int OrganizationId, int CompanyId, string ItemIds, bool IsApproved)
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
				ParameterName = "@ItemIds",
				Value = ItemIds
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Status",
				Value = IsApproved
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "UpdateItemStatusInAllocation"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.Item>("Sp_ItemAllocation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
