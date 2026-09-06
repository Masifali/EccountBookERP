using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.SDT;

namespace Architecture.DAL.SDT;

public class ClientCustomGroup
{
	public int SetData(Architecture.Model.SDT.ClientCustomGroup obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.SDT.ClientCustomGroup item in obj.ClientCustomGroupList)
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

	public List<Architecture.Model.SDT.ClientCustomGroup> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.SDT.ClientCustomGroup> lst = new List<Architecture.Model.SDT.ClientCustomGroup>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.SDT.ClientCustomGroup>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
