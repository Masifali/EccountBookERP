using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class ItemAllocationToSupplierCustomer
{
	public static long Save(Architecture.Model.Inventory.ItemAllocationToSupplierCustomer obj)
	{
		try
		{
			if (obj.Id == 0L)
			{
				return new Architecture.DAL.Inventory.ItemAllocationToSupplierCustomer().SetData(obj, "[dbo].[USP_ItemAllocationToSupplierCustomer_InsertAndUpdate]");
			}
			return 0L;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int OrganizationId, int CompanyId, string Ids, int SupplierCustomerId, int ItemId)
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
				ParameterName = "@Ids",
				Value = Ids
			});
			if (SupplierCustomerId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = SupplierCustomerId
				});
			}
			if (ItemId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = ItemId
				});
			}
			GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemAllocationToSupplierCustomer>("[dbo].[USp_ItemAllocationToSupplierCustomerDeleteById]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ItemAllocationToSupplierCustomer_AllocatedData(int OrganizationId, int CompanyId, int SupplierCustomerId = 0, int ItemId = 0)
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
			if (SupplierCustomerId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = SupplierCustomerId
				});
			}
			if (ItemId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = ItemId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemAllocationToSupplierCustomer>("[dbo].[USP_ItemAllocationToSupplierCustomer_AllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ItemAllocationToSupplierCustomer_UnAllocatedData(int OrganizationId, int CompanyId, int SupplierCustomerId = 0, int ItemId = 0)
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
			if (SupplierCustomerId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = SupplierCustomerId
				});
			}
			if (ItemId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = ItemId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemAllocationToSupplierCustomer>("[dbo].[USP_ItemAllocationToSupplierCustomer_UnAllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
