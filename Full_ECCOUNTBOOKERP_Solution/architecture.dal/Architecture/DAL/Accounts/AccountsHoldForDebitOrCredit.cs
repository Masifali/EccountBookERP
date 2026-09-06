using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.DAL.Accounts;

public class AccountsHoldForDebitOrCredit
{
	public int SetData(Architecture.Model.Accounts.AccountsHoldForDebitOrCredit obj, string ProcName)
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
			throw ex;
		}
	}

	public List<Architecture.Model.Accounts.AccountsHoldForDebitOrCredit> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Accounts.AccountsHoldForDebitOrCredit> lst = new List<Architecture.Model.Accounts.AccountsHoldForDebitOrCredit>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Accounts.AccountsHoldForDebitOrCredit>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
