using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Tax.Definitions;
using Architecture.Model;
using Architecture.Model.Tax.Definitions;

namespace Architecture.BLL.Tax.Definitions;

public class RegularSupplierCustomersAllocateToTaxSupplierCustomers
{
	public static int Save(Architecture.Model.Tax.Definitions.RegularSupplierCustomersAllocateToTaxSupplierCustomers obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Tax.Definitions.RegularSupplierCustomersAllocateToTaxSupplierCustomers().SetData(obj, "[dbo].[USP_RegularSupplierCustomersAllocateToTaxSupplierCustomers_Insert]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int TaxSupplierCustomerId, string Ids)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@TaxSupplierCustomerId",
				Value = TaxSupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SupplierCustomerIds",
				Value = Ids
			});
			GenericProvider.GetDataTableProc<Architecture.Model.ItemAllocateToProductType>("[dbo].[USp_RegularSupplierCustomersAllocateToTaxSupplierCustomers_DeleteById]", p);
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
				ParameterName = "@TaxSupplierCustomerId",
				Value = obj.SupplierCustomerId
			});
			if (obj.PartyTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PartyTypeId",
					Value = obj.PartyTypeId
				});
			}
			if (obj.CustomerGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomerGroupId",
					Value = obj.CustomerGroupId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.ItemAllocateToProductType>("[dbo].[USP_SupplierCustomersAllocatedOrUnAllocatedToTaxSupplierCustomer]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
