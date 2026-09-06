using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL.Inventory;

public class SupplierCustomerAllocateCostCenter
{
	public static long Save(Architecture.Model.SupplierCustomerAllocateCostCenter obj)
	{
		try
		{
			if (obj.SupplierCustomerAllocateCostCenterId == 0L)
			{
				return new Architecture.DAL.SupplierCustomerAllocateCostCenter().SetData(obj, "[dbo].[USP_SupplierCustomerAllocateCostCenter_InsertAndUpdate]");
			}
			return 0L;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SupplierCustomerAllocateCostCenter_AllocatedData(int OrganizationId, int CompanyId, int CostCenterId = 0, int SupplierCustomerId = 0)
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
			if (CostCenterId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CostCenterId",
					Value = CostCenterId
				});
			}
			if (SupplierCustomerId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = SupplierCustomerId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.SupplierCustomerAllocateCostCenter>("[dbo].[USP_SupplierCustomerAllocateCostCenter_AllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SupplierCustomerAllocateCostCenter_UnAllocatedData(int OrganizationId, int CompanyId, int CostCenterId = 0, int SupplierCustomerId = 0)
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
			if (CostCenterId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CostCenterId",
					Value = CostCenterId
				});
			}
			if (SupplierCustomerId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = SupplierCustomerId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.SupplierCustomerAllocateCostCenter>("[dbo].[USP_SupplierCustomerAllocateCostCenter_UnAllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
