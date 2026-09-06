using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Production;
using Architecture.Model.Accounts;
using Architecture.Model.Production;

namespace Architecture.BLL.Production;

public class InvProductionPlant
{
	public static int Save(Architecture.Model.Production.InvProductionPlant obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Production.InvProductionPlant().SetDate(obj, "Sp_InvProductionPlant_Insert");
			}
			return new Architecture.DAL.Production.InvProductionPlant().SetDate(obj, "Sp_InvProductionPlant_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Production.InvProductionPlant obj)
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
				ParameterName = "@Activity",
				Value = "GetALL"
			});
			return GenericProvider.GetDataTableProc<PdcInventory>("Sp_InvProductionPlant_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetById(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetById"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvProductionPlant>("Sp_InvProductionPlant_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
