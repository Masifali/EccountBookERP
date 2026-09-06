using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class UserWarehouse
{
	public static int Save(Architecture.Model.UserWarehouse obj)
	{
		try
		{
			if (obj.UserWarehuseId == 0)
			{
				return new Architecture.DAL.UserWarehouse().SetData(obj, "USP_UserWarehouse_Insert");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllocatedAndUnAllocatedItems(Architecture.Model.UserWarehouse obj)
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
				ParameterName = "@UserId",
				Value = obj.UserAccountId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.UserWarehuseId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "WarehouseAllocatedOrUnAllocatedToUser"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.UserWarehouse>("USp_UserWarehouse_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int OrganizationId, int CompanyId, int UserId, string WarehouseIds)
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
				ParameterName = "@UserId",
				Value = UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@WareHouseIds",
				Value = WarehouseIds
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "DeleteAllocatedWarehouse"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.UserWarehouse>("USp_UserWarehouse_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
