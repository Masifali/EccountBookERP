using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.AssetSchema;

namespace Architecture.DAL.AssetSchema;

public class AssetRegister
{
	public int SetData(Architecture.Model.AssetSchema.AssetRegister obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public List<Architecture.Model.AssetSchema.AssetRegister> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.AssetSchema.AssetRegister> lst = new List<Architecture.Model.AssetSchema.AssetRegister>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<Architecture.Model.AssetSchema.AssetRegister>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
