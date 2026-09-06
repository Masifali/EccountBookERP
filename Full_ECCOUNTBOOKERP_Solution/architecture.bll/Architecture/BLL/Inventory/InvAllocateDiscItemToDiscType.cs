using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class InvAllocateDiscItemToDiscType
{
	public static int Save(Architecture.Model.Inventory.InvAllocateDiscItemToDiscType obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.InvAllocateDiscItemToDiscType().SetData(obj, "[USP_InvAllocateDiscItemToDiscType_Insert]");
			}
			return new Architecture.DAL.Inventory.InvAllocateDiscItemToDiscType().SetData(obj, "[USP_InvAllocateDiscItemToDiscType_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.InvAllocateDiscItemToDiscType GetById(int Id)
	{
		try
		{
			new List<Architecture.Model.Inventory.InvAllocateDiscItemToDiscType>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			return new Architecture.DAL.Inventory.InvAllocateDiscItemToDiscType().GetData("[USP_InvAllocateDiscItemToDiscType_ReadById]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Inventory.InvAllocateDiscItemToDiscType obj)
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvAllocateDiscItemToDiscType>("[USP_InvAllocateDiscItemToDiscType_FormHistory]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDiscountItems(Architecture.Model.Inventory.InvAllocateDiscItemToDiscType obj)
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
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvAllocateDiscItemToDiscType>("[Usp_GetDiscountItems]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDiscountItemsFromAllocation(Architecture.Model.Inventory.InvAllocateDiscItemToDiscType obj)
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
				ParameterName = "@DiscountTypeId",
				Value = obj.DiscountTypeId
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvAllocateDiscItemToDiscType>("[Usp_GetDiscountItemsFromAllocation]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDiscountTypes(Architecture.Model.Inventory.InvAllocateDiscItemToDiscType obj)
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
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvAllocateDiscItemToDiscType>("[Usp_GetDiscountTypes]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
