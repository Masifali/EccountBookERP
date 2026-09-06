using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class SupplierCustomerAllocateToDiscounType
{
	public static int Save(Architecture.Model.SupplierCustomerAllocateToDiscounType obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.SupplierCustomerAllocateToDiscounType().SetData(obj, "[dbo].[USP_SupplierCustomerAllocateToDiscounType_Insert]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int DiscountTypeId, string SupplierCustomerIds)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@DiscountTypeId",
				Value = DiscountTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SupplierCustomerIds",
				Value = SupplierCustomerIds
			});
			GenericProvider.GetDataTableProc<Architecture.Model.SupplierCustomerAllocateToDiscounType>("[dbo].[USp_SupplierCustomerAllocateToDiscounTypeDeleteById]", p);
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
				ParameterName = "@DiscountTypeId",
				Value = obj.DiscountTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.ActionId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.SupplierCustomerAllocateToDiscounType>("[dbo].[USP_GetSupplierCustomerAllocateToDiscounType]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
