using System;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.DAL.Accounts;

public class AccountsPayRecFollowUpHistory
{
	public int SetData(Architecture.Model.Accounts.AccountsPayRecFollowUpHistory obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
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
}
