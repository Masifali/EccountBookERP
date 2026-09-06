using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class Organization
{
	public int SetData(Architecture.Model.Organization obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
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

	public List<Architecture.Model.Organization> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Organization> lst = new List<Architecture.Model.Organization>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Organization>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
