using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Main.Commission;
using Architecture.Model;
using Architecture.Model.Main.Commission;

namespace Architecture.BLL.Main.Commission;

public class AllocateCommissionItemToCommissionType
{
	public static int Save(Architecture.Model.Main.Commission.AllocateCommissionItemToCommissionType obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Main.Commission.AllocateCommissionItemToCommissionType().SetData(obj, "[dbo].[USP_AllocateCommissionItemToCommissionType_Insert]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int CommissionTypeId, string CommissionItemIds)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CommissionTypeId",
				Value = CommissionTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CommissionItemIds",
				Value = CommissionItemIds
			});
			GenericProvider.GetDataTableProc<Architecture.Model.Main.Commission.AllocateCommissionItemToCommissionType>("[dbo].[USP_AllocateCommissionItemToCommissionType_DeleteById]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllocateAndUnallocatedCommissionItemToCommissionType(ReportsParameters obj)
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
				ParameterName = "@CommissionTypeId",
				Value = obj.TypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.ActionId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.SupplierCustomerAllocateToDiscounType>("[USP_GetAllocateAndUnallocatedCommissionItemToCommissionType]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CommissionItemsFromAllocationByCommissionTypeId(int OrganizationId, int CompanyId, int CommissionTypeId)
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
				ParameterName = "@CommissionTypeId",
				Value = CommissionTypeId
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Main.Commission.AllocateCommissionItemToCommissionType>("[dbo].[USP_CommissionItemsFromAllocationByCommissionTypeId]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
