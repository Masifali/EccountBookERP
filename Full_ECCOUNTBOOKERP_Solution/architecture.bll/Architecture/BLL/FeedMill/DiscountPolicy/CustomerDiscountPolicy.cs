using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.FeedMill.DiscountPolicy;
using Architecture.Model;
using Architecture.Model.FeedMill.DiscountPolicy;
using Architecture.Model.Inventory;

namespace Architecture.BLL.FeedMill.DiscountPolicy;

public class CustomerDiscountPolicy
{
	public static int Save(Architecture.Model.FeedMill.DiscountPolicy.CustomerDiscountPolicy obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
				return new Architecture.DAL.FeedMill.DiscountPolicy.CustomerDiscountPolicy().SetData(obj, "[fed].[USP_CustomerDiscountPolicy_InsertAndUpdate]");
			}
			obj.ActionId = 2;
			return new Architecture.DAL.FeedMill.DiscountPolicy.CustomerDiscountPolicy().SetData(obj, "[fed].[USP_CustomerDiscountPolicy_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.FeedMill.DiscountPolicy.CustomerDiscountPolicy GetByID(int Id)
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
			new List<Architecture.Model.FeedMill.DiscountPolicy.CustomerDiscountPolicy>();
			return new Architecture.DAL.FeedMill.DiscountPolicy.CustomerDiscountPolicy().GetData("[fed].[USP_CustomerDiscountPolicy_GetAllMethod]", p)[0];
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
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
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
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DiscountDebitAccountId",
					Value = obj.AccountId
				});
			}
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUserId",
					Value = obj.EntryUser
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.DiscountPolicy.CustomerDiscountPolicy>("[fed].[USP_CustomerDiscountPolicy_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int Id, int EntryUserId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@Id", Id));
			p.Add(new SqlParameter("@EntryUserId", EntryUserId));
			p.Add(new SqlParameter("@Activity", "DeleteById"));
			GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.DiscountPolicy.CustomerDiscountPolicy>("[fed].[USP_CustomerDiscountPolicy_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DropDownFillFromCustomerDiscountPolicy(ReportsParameters obj)
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
			if (obj.Activity != string.Empty || obj.Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[fed].[USP_DropDownFillFromCustomerDiscountPolicy]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CustomerDiscountPolicySlip(ReportsParameters obj)
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[fed].[USP_CustomerDiscountPolicySlip]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet getCustomerPolicy_DataSet(int OrganizationId, int CompanyId, int CustomerId, DateTime EffectedDate)
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
				ParameterName = "@CustomerId",
				Value = CustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EffectedDate",
				Value = EffectedDate
			});
			return GenericProvider.GetDataSetProc<SupplierCustomer>("[fed].[usp_getCustomerPolicy_DataSet]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
