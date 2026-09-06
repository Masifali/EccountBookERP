using System;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.SystemUtilities;

namespace Architecture.DAL.SystemUtilities;

public class MultiLanguages
{
	public int SetData(Architecture.Model.SystemUtilities.MultiLanguages obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}
}
