using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Production;
using Architecture.Model.Production;

namespace Architecture.BLL.Production;

public class ProductionType
{
	public static int Save(Architecture.Model.Production.ProductionType obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Production.ProductionType().SetDate(obj, "Sp_ProductionType_Insert");
			}
			return new Architecture.DAL.Production.ProductionType().SetDate(obj, "Sp_ProductionType_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Production.ProductionType GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			return new Architecture.DAL.Production.ProductionType().GetDate("Sp_ProductionType_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Production.ProductionType obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.ProductionType>("Sp_ProductionType_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
