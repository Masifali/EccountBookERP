using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.SystemUtilities;

namespace Architecture.DAL.SystemUtilities;

public class WeighbridgeScales
{
	public int Save(Architecture.Model.SystemUtilities.WeighbridgeScales obj, string ProcName)
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
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public Architecture.Model.SystemUtilities.WeighbridgeScales ReadByCompanyId(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.SystemUtilities.WeighbridgeScales> lst = new List<Architecture.Model.SystemUtilities.WeighbridgeScales>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			lst = GenericProvider.GetProc<Architecture.Model.SystemUtilities.WeighbridgeScales>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
		if (lst.Count > 0)
		{
			return lst[0];
		}
		return new Architecture.Model.SystemUtilities.WeighbridgeScales();
	}
}
