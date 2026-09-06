using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class ContractorsAllocationToBranch
{
	public static int Save(Architecture.Model.ContractorsAllocationToBranch obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.ContractorsAllocationToBranch().SetData(obj, "[dbo].[USP_ContractorsAllocationToBranch_Insert]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int BranchId, string Contractors)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchId",
				Value = BranchId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ContractorIds",
				Value = Contractors
			});
			GenericProvider.GetDataTableProc<Architecture.Model.ContractorsAllocationToBranch>("[dbo].[USP_ContractorsAllocationToBranchDeleteById]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ContractorsAllocatedOrUnAllocatedToBranch(ReportsParameters obj)
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
				ParameterName = "@BranchId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.ActionId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorsAllocationToBranch>("[dbo].[USP_ContractorsAllocatedOrUnAllocatedToBranch]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetContractorsAllocatedToBranch(ReportsParameters obj)
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
				ParameterName = "@BranchId",
				Value = obj.BranchesId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorsAllocationToBranch>("[dbo].[USP_GetContractorsAllocatedToBranch]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
