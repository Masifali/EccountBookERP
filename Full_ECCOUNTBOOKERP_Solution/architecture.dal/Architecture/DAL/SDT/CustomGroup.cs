using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.SDT;

namespace Architecture.DAL.SDT;

public class CustomGroup
{
	public int SetData(Architecture.Model.SDT.CustomGroup obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			obj.customGroupId = Convert.ToInt32(returnID);
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public List<Architecture.Model.SDT.CustomGroup> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.SDT.CustomGroup> lst = new List<Architecture.Model.SDT.CustomGroup>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.SDT.CustomGroup>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
