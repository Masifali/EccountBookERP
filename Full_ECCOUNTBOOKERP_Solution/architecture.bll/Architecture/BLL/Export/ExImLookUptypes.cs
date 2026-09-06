using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Export;
using Architecture.Model.Export;

namespace Architecture.BLL.Export;

public class ExImLookUptypes
{
	public static int Save(Architecture.Model.Export.ExImLookUptypes obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Export.ExImLookUptypes().SetData(obj, "Sp_ExImLookUptypes_Insert");
			}
			return new Architecture.DAL.Export.ExImLookUptypes().SetData(obj, "Sp_ExImLookUptypes_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Export.ExImLookUptypes GetByID(int Id)
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
			new List<Architecture.Model.Export.ExImLookUptypes>();
			return new Architecture.DAL.Export.ExImLookUptypes().GetAll("Sp_ExImLookUptypes_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImDeliveryTerm>("Sp_ExImLookUptypes_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
