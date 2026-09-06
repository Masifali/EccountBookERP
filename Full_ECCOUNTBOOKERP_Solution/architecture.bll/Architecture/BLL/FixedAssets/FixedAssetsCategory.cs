using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.FixedAssets;
using Architecture.Model.FixedAssets;

namespace Architecture.BLL.FixedAssets;

public class FixedAssetsCategory
{
	public static int Save(Architecture.Model.FixedAssets.FixedAssetsCategory obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.FixedAssets.FixedAssetsCategory().SetData(obj, "Sp_FixedAssetsCategory_Insert");
			}
			return new Architecture.DAL.FixedAssets.FixedAssetsCategory().SetData(obj, "Sp_FixedAssetsCategory_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.FixedAssets.FixedAssetsCategory>("Sp_FixedAssetsCategory_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static Architecture.Model.FixedAssets.FixedAssetsCategory GetById(int Id)
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
			return new Architecture.DAL.FixedAssets.FixedAssetsCategory().GetAll("Sp_FixedAssetsCategory_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
