using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class VehicleType
{
	public static DataTable GetAll()
	{
		try
		{
			new DataTable();
			new List<SqlParameter>().Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.VehicleType>("Sp_VehicleType_GetAllMethod");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int Save(Architecture.Model.Inventory.VehicleType obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.VehicleType().SetData(obj, "Sp_VehicleType_Insert");
			}
			return new Architecture.DAL.Inventory.VehicleType().SetData(obj, "Sp_VehicleType_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
