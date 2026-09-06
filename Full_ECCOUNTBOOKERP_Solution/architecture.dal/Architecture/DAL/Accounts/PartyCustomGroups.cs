using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.DAL.Accounts;

public class PartyCustomGroups
{
	public int SetData(Architecture.Model.Accounts.PartyCustomGroups obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Accounts.PartyCustomGroups item in obj.GetList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public List<Architecture.Model.Accounts.PartyCustomGroups> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Accounts.PartyCustomGroups> lst = new List<Architecture.Model.Accounts.PartyCustomGroups>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Accounts.PartyCustomGroups>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
