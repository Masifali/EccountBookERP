using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Main.Commission;
using Architecture.Model;
using Architecture.Model.Main.Commission;

namespace Architecture.BLL.Main.Commission;

public class AllocateBrandItemToCommissionType
{
	public static int Save(Architecture.Model.Main.Commission.AllocateBrandItemToCommissionType obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Main.Commission.AllocateBrandItemToCommissionType().SetData(obj, "[dbo].[USP_AllocateBrandItemToCommissionType_Insert]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int CommissionTypeId, string BrandItemIds)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CommissionTypeId",
				Value = CommissionTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BrandItemIds",
				Value = BrandItemIds
			});
			GenericProvider.GetDataTableProc<Architecture.Model.Main.Commission.AllocateBrandItemToCommissionType>("[dbo].[USP_AllocateBrandItemToCommissionType_DeleteById]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllocateAndUnallocatedBrandItemToCommissionType(ReportsParameters obj)
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
				ParameterName = "@CommissionTypeId",
				Value = obj.TypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.ActionId
			});
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = obj.ItemCategoryId
				});
			}
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			if (obj.DiscountTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DiscountTypeId",
					Value = obj.DiscountTypeId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.SupplierCustomerAllocateToDiscounType>("[dbo].[USP_GetAllocateAndUnallocatedBrandItemToCommissionType]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
