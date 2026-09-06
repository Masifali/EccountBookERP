using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.FixedAssets;

namespace Architecture.DAL.FixedAssets;

public class FixedAssetsRegister
{
	public int SetData(Architecture.Model.FixedAssets.FixedAssetsRegister obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			obj.Id = Convert.ToInt32(returnID);
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public List<Architecture.Model.FixedAssets.FixedAssetsRegister> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.FixedAssets.FixedAssetsRegister> lst = new List<Architecture.Model.FixedAssets.FixedAssetsRegister>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.FixedAssets.FixedAssetsRegister>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
