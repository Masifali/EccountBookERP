using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class StockMonthlyClosingHeader
{
	public static int Save(Architecture.Model.Inventory.StockMonthlyClosingHeader obj)
	{
		try
		{
			if (obj.Id == 0 && obj.StockMonthlyClosingList != null)
			{
				return new Architecture.DAL.Inventory.StockMonthlyClosingHeader().SetData(obj, "USP_StockMonthlyClosingHeader_Insert");
			}
			return new Architecture.DAL.Inventory.StockMonthlyClosingHeader().SetData(obj, "USP_StockMonthlyClosingHeader_Update");
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

	public static DataTable GetAvailableStockForMonthlyClosing(int OrganizationId, int CompanyId, DateTime AsOnDate)
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
				ParameterName = "@DocDate",
				Value = AsOnDate
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.SupplierCustomerAllocateToDiscounType>("usp_getAvailableStockForMonthlyClosing", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByClosingDateFromStockClosing(DateTime ClosingDate)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ClosingDate",
				Value = ClosingDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByClosingDate"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.SupplierCustomerAllocateToDiscounType>("[dbo].[USP_StockMonthlyClosing_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable MonthlyClosingStock_Slip(ReportsParameters obj)
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
				ParameterName = "@ClosingDate",
				Value = obj.DocDate
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.SupplierCustomerAllocateToDiscounType>("usp_MonthlyClosingStock_Slip", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
