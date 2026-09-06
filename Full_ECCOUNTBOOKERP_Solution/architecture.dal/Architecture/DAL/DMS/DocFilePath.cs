using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.DMS;

namespace Architecture.DAL.DMS;

public class DocFilePath
{
	public long SetData(Architecture.Model.DMS.DocFilePath obj, string ProcName)
	{
		long returnID = 0L;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.DMS.DocFilePath item in obj.docFilePathsList)
			{
				returnID = Convert.ToInt64(GenericProvider.SetProc(sqlTrn, item, ProcName));
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.DMS.DocFilePath> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.DMS.DocFilePath> lst = new List<Architecture.Model.DMS.DocFilePath>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.DMS.DocFilePath>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
