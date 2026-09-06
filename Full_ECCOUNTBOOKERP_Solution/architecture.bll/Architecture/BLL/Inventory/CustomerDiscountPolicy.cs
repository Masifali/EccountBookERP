using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class CustomerDiscountPolicy
{
	public static int Save(Architecture.Model.Inventory.CustomerDiscountPolicy obj)
	{
		try
		{
			return new Architecture.DAL.Inventory.CustomerDiscountPolicy().SetData(obj, "Sp_CustomerDiscountPolicy_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.CustomerDiscountPolicy GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.Inventory.CustomerDiscountPolicy>();
			return new Architecture.DAL.Inventory.CustomerDiscountPolicy().GetData("Sp_CustomerDiscountPolicy_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrgnaizationId",
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
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_CustomerDiscountPolicy_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetCustomerDiscountByEffectiveDate(ReportsParameters obj)
	{
		try
		{
			double Discount = 0.0;
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrgnaizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CustomerId",
				Value = obj.SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EffectiveDate",
				Value = obj.EffectedDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetCustomerDiscountByEffectiveDate"
			});
			DataTable dt = GenericProvider.GetDataTableProc<ReportsParameters>("Sp_CustomerDiscountPolicy_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				Discount = Convert.ToDouble(dt.Rows[0][0]);
			}
			return Discount;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
