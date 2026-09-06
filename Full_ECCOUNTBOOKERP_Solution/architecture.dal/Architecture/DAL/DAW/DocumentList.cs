using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.DAW;

namespace Architecture.DAL.DAW;

public class DocumentList
{
	public int SetData(Architecture.Model.DAW.DocumentList obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID == 0)
			{
				returnID = obj.Id;
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

	public List<Architecture.Model.DAW.DocumentList> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.DAW.DocumentList> lst = new List<Architecture.Model.DAW.DocumentList>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.DAW.DocumentList>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
