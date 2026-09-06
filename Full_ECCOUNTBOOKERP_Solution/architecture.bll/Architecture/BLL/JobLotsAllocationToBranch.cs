using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class JobLotsAllocationToBranch
{
	public static int Save(Architecture.Model.JobLotsAllocationToBranch obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.JobLotsAllocationToBranch().SetData(obj, "[dbo].[USP_JobLotsAllocationToBranch_Insert]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int BranchId, string JobLotIds)
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
				ParameterName = "@JobLotIds",
				Value = JobLotIds
			});
			GenericProvider.GetDataTableProc<Architecture.Model.JobLotsAllocationToBranch>("[dbo].[USP_JobLotsAllocationToBranchDeleteById]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable JobLotsAllocatedOrUnAllocatedToBranch(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.JobLotsAllocationToBranch>("[dbo].[USP_JobLotsAllocatedOrUnAllocatedToBranch]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetJobLotsAllocatedToBranchByBranchId(ReportsParameters obj)
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
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchId",
					Value = obj.BranchesId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.WarehousesAllocationToBranch>("[dbo].[USP_GetJobLotsAllocatedToBranch]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
