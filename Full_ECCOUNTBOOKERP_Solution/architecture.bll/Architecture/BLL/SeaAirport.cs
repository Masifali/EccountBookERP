using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Import;
using Architecture.Model.Import;

namespace Architecture.BLL;

public class SeaAirport
{
	public static int Save(Architecture.Model.Import.SeaAirport obj)
	{
		try
		{
			if (obj.seaAirPortId == 0)
			{
				return new Architecture.DAL.Import.SeaAirport().SetData(obj, "USP_SeaAirport_Insert");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllocatedAndUnAllocatedItems(Architecture.Model.Import.SeaAirport obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "WarehouseAllocatedOrUnAllocatedToUser"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.SeaAirport>("USp_SeaAirport_GetAllMethod", p);
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
			GenericProvider.GetDataTableProc<Architecture.Model.Import.SeaAirport>("USp_SeaAirport_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
