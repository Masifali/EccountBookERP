using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Main.Commission;
using Architecture.Model;
using Architecture.Model.Main.Commission;

namespace Architecture.BLL.Main.Commission;

public class CustomerCommissionPolicy
{
	public static int Save(Architecture.Model.Main.Commission.CustomerCommissionPolicy obj)
	{
		try
		{
			return new Architecture.DAL.Main.Commission.CustomerCommissionPolicy().SetData(obj, "[dbo].[USP_CustomerCommissionPolicy_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Main.Commission.CustomerCommissionPolicy GetByID(int Id)
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
			new List<Architecture.Model.Main.Commission.CustomerCommissionPolicy>();
			return new Architecture.DAL.Main.Commission.CustomerCommissionPolicy().GetData("[dbo].[USP_CustomerCommissionPolicy_GetAllMethod]", p)[0];
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
			if (obj.PolicyTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PolicyTypeId",
					Value = obj.PolicyTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<ReportsParameters>("[dbo].[USP_CustomerCommissionPolicy_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetCustomerCommissionByEffectiveDate(ReportsParameters obj)
	{
		try
		{
			double Commission = 0.0;
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
			if (obj.PolicyTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PolicyTypeId",
					Value = obj.PolicyTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetCustomerCommissionByEffectiveDate"
			});
			DataTable dt = GenericProvider.GetDataTableProc<ReportsParameters>("[dbo].[USP_CustomerCommissionPolicy_GetAllMethod]", p);
			if (dt.Rows.Count > 0)
			{
				Commission = Convert.ToDouble(dt.Rows[0][0]);
			}
			return Commission;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
