using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.DMS;

namespace Architecture.DAL.DMS;

public class DocFilePathAllocation
{
	public long SetData(Architecture.Model.DMS.DocFilePathAllocation obj, string ProcName)
	{
		long returnID = 0L;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.DMS.DocFilePathAllocation item in obj.DocFilePathAllocationslist)
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

	public List<Architecture.Model.DMS.DocFilePathAllocation> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.DMS.DocFilePathAllocation> lst = new List<Architecture.Model.DMS.DocFilePathAllocation>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.DMS.DocFilePathAllocation>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
