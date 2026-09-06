using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class DefineVehicleWeight
{
	public static int Save(Architecture.Model.Inventory.DefineVehicleWeight obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.DefineVehicleWeight().SetData(obj, "USp_DefineVehicleWeight_Insert");
			}
			return new Architecture.DAL.Inventory.DefineVehicleWeight().SetData(obj, "USp_DefineVehicleWeight_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.DefineVehicleWeight GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			new List<Architecture.Model.Inventory.DefineVehicleWeight>();
			return new Architecture.DAL.Inventory.DefineVehicleWeight().GetAll("USp_DefineVehicleWeight_ReadById", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAll(Architecture.Model.Inventory.DefineVehicleWeight obj)
	{
		try
		{
			new DataTable();
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.DefineVehicleWeight>("USp_DefineVehicleWeight_FormHistory", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
