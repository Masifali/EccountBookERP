using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.FixedAssets;

namespace Architecture.DAL.FixedAssets;

public class FixedAssetsCategory
{
	public int SetData(Architecture.Model.FixedAssets.FixedAssetsCategory obj, string ProcName)
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

	public List<Architecture.Model.FixedAssets.FixedAssetsCategory> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.FixedAssets.FixedAssetsCategory> lst = new List<Architecture.Model.FixedAssets.FixedAssetsCategory>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.FixedAssets.FixedAssetsCategory>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
