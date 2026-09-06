using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Production;
using Architecture.Model.Production;

namespace Architecture.BLL.Production;

public class ProductionPlanType
{
	public static int Save(Architecture.Model.Production.ProductionPlanType obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Production.ProductionPlanType().SetDate(obj, "Sp_ProductionPlanType_Insert");
			}
			return new Architecture.DAL.Production.ProductionPlanType().SetDate(obj, "Sp_ProductionPlanType_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Production.ProductionPlanType obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (obj.Id > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.ProductionPlanType>("Sp_ProductionPlanType_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
