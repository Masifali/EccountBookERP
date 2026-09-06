using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Templates;

namespace Architecture.DAL.Templates;

public class InActiveScreenOfTemplatesModule
{
	public long SetData(Architecture.Model.Templates.InActiveScreenOfTemplatesModule obj, string ProcName)
	{
		long returnID = 0L;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Templates.InActiveScreenOfTemplatesModule item in obj.InActiveScreenOfTemplatesModuleList)
			{
				returnID = Conversion.ToInt64(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public List<Architecture.Model.Templates.InActiveScreenOfTemplatesModule> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Templates.InActiveScreenOfTemplatesModule> lst = new List<Architecture.Model.Templates.InActiveScreenOfTemplatesModule>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Templates.InActiveScreenOfTemplatesModule>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
