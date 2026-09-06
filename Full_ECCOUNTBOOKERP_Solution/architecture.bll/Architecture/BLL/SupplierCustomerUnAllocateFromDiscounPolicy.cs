using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class SupplierCustomerUnAllocateFromDiscounPolicy
{
	public static int Save(Architecture.Model.SupplierCustomerUnAllocateFromDiscounPolicy obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.SupplierCustomerUnAllocateFromDiscounPolicy().SetData(obj, "[dbo].[USP_SupplierCustomerUnAllocateFromDiscounPolicy_Insert]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int DiscountPolicyId, string SupplierCustomerIds)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@DiscountPolicyId",
				Value = DiscountPolicyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SupplierCustomerIds",
				Value = SupplierCustomerIds
			});
			GenericProvider.GetDataTableProc<Architecture.Model.SupplierCustomerUnAllocateFromDiscounPolicy>("[dbo].[USp_SupplierCustomerUnAllocateFromDiscounPolicyDeleteById]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllocatedAndUnAllocatedSupplierCustomerIds(ReportsParameters obj)
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
				ParameterName = "@DiscountPolicyId",
				Value = obj.DiscountPolicyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.ActionId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.SupplierCustomerUnAllocateFromDiscounPolicy>("[dbo].[USP_GetSupplierCustomerUnAllocateFromDiscounPolicy]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
