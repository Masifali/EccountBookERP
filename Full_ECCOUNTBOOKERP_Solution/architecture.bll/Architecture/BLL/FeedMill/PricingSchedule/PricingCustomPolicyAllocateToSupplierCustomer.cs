using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.FeedMill.PricingSchedule;
using Architecture.Model;
using Architecture.Model.FeedMill.PricingSchedule;

namespace Architecture.BLL.FeedMill.PricingSchedule;

public class PricingCustomPolicyAllocateToSupplierCustomer
{
	public static int Save(Architecture.Model.FeedMill.PricingSchedule.PricingCustomPolicyAllocateToSupplierCustomer obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.FeedMill.PricingSchedule.PricingCustomPolicyAllocateToSupplierCustomer().SetData(obj, "[USP_PricingCustomPolicyAllocateToSupplierCustomer_Insert]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int GroupId, string SuppCustIds)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@PricingCustomGroupId",
				Value = GroupId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SupplierCustomerIds",
				Value = SuppCustIds
			});
			GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.PricingSchedule.PricingCustomPolicyAllocateToSupplierCustomer>("[dbo].[USp_PricingCustomPolicyAllocateToSupplierCustomerDeleteById]", p);
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
				ParameterName = "@PricingCustomGroupId",
				Value = obj.GroupId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.PricingSchedule.PricingCustomPolicyAllocateToSupplierCustomer>("[dbo].[USP_SupplierCustomerAllocatedOrUnAllocatedToPolicyCustomGroups]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
