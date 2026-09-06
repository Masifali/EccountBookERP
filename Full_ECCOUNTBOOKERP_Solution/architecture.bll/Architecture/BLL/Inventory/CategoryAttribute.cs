using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class CategoryAttribute
{
	public static long Save(Architecture.Model.Inventory.CategoryAttribute obj)
	{
		try
		{
			return new Architecture.DAL.Inventory.CategoryAttribute().SetData(obj, "[item].[USP_CategoryAttribute_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int OrganizationId, int CompanyId, string Ids, int ItemId)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = ItemId
			});
			GenericProvider.GetDataTableProc<Architecture.Model.Inventory.CategoryAttribute>("[item].[USp_CategoryAttribute_DeleteById]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CategoryAttribute_AllocatedData(int OrganizationId, int CompanyId, int ItemId)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.CategoryAttribute>("[item].[USP_CategoryAttribute_AllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CategoryAttribute_UnAllocatedData(int OrganizationId, int CompanyId, int ItemId)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.CategoryAttribute>("[item].[USP_CategoryAttribute_UnAllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
