using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Tax.Definitions;
using Architecture.Model;
using Architecture.Model.Tax.Definitions;

namespace Architecture.BLL.Tax.Definitions;

public class RegularItemsAllocateToTaxItem
{
	public static int Save(Architecture.Model.Tax.Definitions.RegularItemsAllocateToTaxItem obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Tax.Definitions.RegularItemsAllocateToTaxItem().SetData(obj, "[dbo].[USP_RegularItemsAllocateToTaxItem_Insert]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int TaxItemId, string ItemIds)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@TaxItemId",
				Value = TaxItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemIds",
				Value = ItemIds
			});
			GenericProvider.GetDataTableProc<Architecture.Model.ItemAllocateToProductType>("[dbo].[USp_RegularItemsAllocateToTaxItem_DeleteById]", p);
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
				ParameterName = "@TaxItemId",
				Value = obj.TaxItemId
			});
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = obj.ItemCategoryId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.ItemAllocateToProductType>("[dbo].[USP_ItemsAllocatedOrUnAllocatedToTaxItem]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
