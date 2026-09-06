using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class ItemAttributVariant
{
	public static long Save(Architecture.Model.Inventory.ItemAttributVariant obj)
	{
		try
		{
			return new Architecture.DAL.Inventory.ItemAttributVariant().SetData(obj, "[item].[USP_ItemAttributVariant_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int OrganizationId, int CompanyId, int ItemId, string AttributeIds, string variantIds)
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
				ParameterName = "@AttributeIds",
				Value = AttributeIds
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@variantIds",
				Value = variantIds
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = ItemId
			});
			GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemAttributVariant>("[item].[USp_ItemAttributVariant_DeleteById]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ItemAttributVariant_AllocatedData(int OrganizationId, int CompanyId, int ItemId)
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
				ParameterName = "@ItemId",
				Value = ItemId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemAttributVariant>("[item].[USP_ItemAttributeVariant_AllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ItemAttributVariant_UnAllocatedData(int OrganizationId, int CompanyId, int ItemId)
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
				ParameterName = "@ItemId",
				Value = ItemId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemAttributVariant>("[item].[USP_ItemAttributVariant_UnAllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
