using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.FeedMill.DiscountPolicy;
using Architecture.Model;
using Architecture.Model.FeedMill.DiscountPolicy;

namespace Architecture.BLL.FeedMill.DiscountPolicy;

public class CustomerCommissionAgentPolicy
{
	public static int Save(Architecture.Model.FeedMill.DiscountPolicy.CustomerCommissionAgentPolicy obj)
	{
		try
		{
			return new Architecture.DAL.FeedMill.DiscountPolicy.CustomerCommissionAgentPolicy().SetData(obj, "[fed].[USP_CustomerCommissionAgentPolicy_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.FeedMill.DiscountPolicy.CustomerCommissionAgentPolicy GetByID(int Id)
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
			new List<Architecture.Model.FeedMill.DiscountPolicy.CustomerCommissionAgentPolicy>();
			return new Architecture.DAL.FeedMill.DiscountPolicy.CustomerCommissionAgentPolicy().GetData("[fed].[USP_CustomerCommissionAgentPolicy_GetAllMethod]", p)[0];
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
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			if (obj.CustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomerId",
					Value = obj.CustomerId
				});
			}
			if (obj.CommissionAgentId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CommissionAgentId",
					Value = obj.CommissionAgentId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<ReportsParameters>("[fed].[USP_CustomerCommissionAgentPolicy_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromCustomerCommissionAgentPolicy(int OrganizationId, int CompanyId, string Activity = "")
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
			if (Activity != null && Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = Activity
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[fed].[USP_GetDataForDropDownFromCustomerCommissionAgentPolicy]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetCustomerCommissionAgentCommissionRateByTypAndEffectiveDate(ReportsParameters obj)
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
				Value = obj.CustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CommissionAgentId",
				Value = obj.CommissionAgentId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CommissionTypeId",
				Value = obj.TypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EffectiveDate",
				Value = obj.EffectedDate
			});
			if (obj.DebitAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CommissionDebitAccountId",
					Value = obj.DebitAccountId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetCustomerCommissionAgentCommissionRateByTypAndEffectiveDate"
			});
			DataTable dt = GenericProvider.GetDataTableProc<ReportsParameters>("[fed].[USP_CustomerCommissionAgentPolicy_GetAllMethod]", p);
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
