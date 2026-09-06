using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.SDT;

namespace Architecture.DAL.SDT;

public class DocumentOfGroup
{
	public int SetData(Architecture.Model.SDT.DocumentOfGroup obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.SDT.DocumentOfGroup item in obj.DocumentOfGroupList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public List<Architecture.Model.SDT.DocumentOfGroup> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.SDT.DocumentOfGroup> lst = new List<Architecture.Model.SDT.DocumentOfGroup>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.SDT.DocumentOfGroup>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
