using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL.Inventory;

public class ChartOfAccountAllocateCostCenter
{
	public static long Save(Architecture.Model.ChartOfAccountAllocateCostCenter obj)
	{
		try
		{
			if (obj.ChartOfAccountAllocateCostCenterId == 0L)
			{
				return new Architecture.DAL.ChartOfAccountAllocateCostCenter().SetData(obj, "[dbo].[USP_ChartOfAccountAllocateCostCenter_InsertAndUpdate]");
			}
			return 0L;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ChartOfAccountAllocateCostCenter_AllocatedData(int OrganizationId, int CompanyId, int CostCenterId = 0, int ChartOfAccountId = 0, int ParentId = 0, int CustomGroupId = 0)
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
			if (ChartOfAccountId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ChartOfAccountId",
					Value = ChartOfAccountId
				});
			}
			if (ParentId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentId",
					Value = ParentId
				});
			}
			if (CustomGroupId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomGroupId",
					Value = CustomGroupId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.ChartOfAccountAllocateCostCenter>("[dbo].[USP_ChartOfAccountAllocateCostCenter_AllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ChartOfAccountAllocateCostCenter_UnAllocatedData(int OrganizationId, int CompanyId, int CostCenterId = 0, int ChartOfAccountId = 0, int ParentId = 0, int CustomGroupId = 0)
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
			if (ChartOfAccountId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ChartOfAccountId",
					Value = ChartOfAccountId
				});
			}
			if (ParentId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentId",
					Value = ParentId
				});
			}
			if (CustomGroupId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomGroupId",
					Value = CustomGroupId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.ChartOfAccountAllocateCostCenter>("[dbo].[USP_ChartOfAccountAllocateCostCenter_UnAllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
