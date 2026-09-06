using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class RevExpAcAllocationToProductAndDiscountType
{
	public static int Save(Architecture.Model.RevExpAcAllocationToProductAndDiscountType obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.RevExpAcAllocationToProductAndDiscountType().SetData(obj, "[dbo].[USP_RevExpAcAllocationToProductAndDiscountType_Insert]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int ProductTypeId, int DiscountTypeId, string RevExpAcIds)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ProductTypeId",
				Value = ProductTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DiscountTypeId",
				Value = DiscountTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ReveneuExpenseAccountIds",
				Value = RevExpAcIds
			});
			GenericProvider.GetDataTableProc<Architecture.Model.RevExpAcAllocationToProductAndDiscountType>("[dbo].[USp_RevExpAcAllocationToProductAndDiscountTypeDeleteById]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllocatedAndUnAllocatedRevExpenseAc(ReportsParameters obj)
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
				ParameterName = "@ProductTypeId",
				Value = obj.ProductTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DiscountTypeId",
				Value = obj.DiscountTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.ActionId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.RevExpAcAllocationToProductAndDiscountType>("[dbo].[USP_GetRevExpAcAllocationToProductAndDiscountType]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
