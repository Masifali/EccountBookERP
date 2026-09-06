using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class UsersWithCostCenter
{
	public static long Save(Architecture.Model.UsersWithCostCenter obj)
	{
		try
		{
			if (obj.UsersWithCostCenterId == 0L)
			{
				return new Architecture.DAL.UsersWithCostCenter().SetData(obj, "[dbo].[USP_UsersWithCostCenter_InsertAndUpdate]");
			}
			return 0L;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int OrganizationId, int CompanyId, string Ids, int CostCenterId, int UserId)
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
				ParameterName = "@Ids",
				Value = Ids
			});
			if (CostCenterId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CostCenterId",
					Value = CostCenterId
				});
			}
			if (UserId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = UserId
				});
			}
			GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemAllocationToSupplierCustomer>("[dbo].[USp_UsersWithCostCenterDeleteById]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable UsersWithCostCenter_AllocatedData(ReportsParameters obj)
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
			if (obj.CostCenterId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CostCenterId",
					Value = obj.CostCenterId
				});
			}
			if (obj.UserId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = obj.UserId
				});
			}
			if (obj.AppId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AppId",
					Value = obj.AppId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.UsersWithCostCenter>("[dbo].[USP_UsersWithCostCenter_AllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable UsersWithCostCenter_UnAllocatedData(int OrganizationId, int CompanyId, int CostCenterId = 0, int UserId = 0)
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
			if (UserId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = UserId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.UsersWithCostCenter>("[dbo].[USP_UsersWithCostCenter_UnAllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
