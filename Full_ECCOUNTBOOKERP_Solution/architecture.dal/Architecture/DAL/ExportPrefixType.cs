using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class ExportPrefixType
{
	public int SetData(Architecture.Model.ExportPrefixType obj, string ProcName)
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

	public List<Architecture.Model.ExportPrefixType> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ExportPrefixType> lst = new List<Architecture.Model.ExportPrefixType>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.ExportPrefixType>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
