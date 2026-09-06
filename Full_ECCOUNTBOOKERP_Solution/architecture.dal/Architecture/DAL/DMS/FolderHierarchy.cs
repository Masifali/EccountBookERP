using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.DMS;

namespace Architecture.DAL.DMS;

public class FolderHierarchy
{
	public long SetData(Architecture.Model.DMS.FolderHierarchy obj, string ProcName)
	{
		long returnID = 0L;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.FolderHierarchyId = returnID;
			}
			else
			{
				returnID = obj.FolderHierarchyId;
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

	public List<Architecture.Model.DMS.FolderHierarchy> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.DMS.FolderHierarchy> lst = new List<Architecture.Model.DMS.FolderHierarchy>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.DMS.FolderHierarchy>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
