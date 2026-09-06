using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.DAL.Accounts;

public class PartyCustomGroupsAllocateToDocumentType
{
	public int SetData(Architecture.Model.Accounts.PartyCustomGroupsAllocateToDocumentType obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Accounts.PartyCustomGroupsAllocateToDocumentType item in obj.PartyCustomGroupsAllocateToDocumentTypeList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
			}
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

	public List<Architecture.Model.Accounts.PartyCustomGroupsAllocateToDocumentType> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Accounts.PartyCustomGroupsAllocateToDocumentType> lst = new List<Architecture.Model.Accounts.PartyCustomGroupsAllocateToDocumentType>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Accounts.PartyCustomGroupsAllocateToDocumentType>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
