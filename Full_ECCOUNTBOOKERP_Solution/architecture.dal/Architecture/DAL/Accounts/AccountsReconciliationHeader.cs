using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.DAL.Accounts;

public class AccountsReconciliationHeader : DALBase
{
	public int SetDate(Architecture.Model.Accounts.AccountsReconciliationHeader obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			obj.Id = Convert.ToInt32(returnID);
			foreach (AccountsReconciliationDetail item in obj.AccountsReconiliationDetailList)
			{
				item.AccountsReconiliationId = obj.Id;
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_AccountsReconciliationDetail_Insert"));
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

	public List<Architecture.Model.Accounts.AccountsReconciliationHeader> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Accounts.AccountsReconciliationHeader> lst = new List<Architecture.Model.Accounts.AccountsReconciliationHeader>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Accounts.AccountsReconciliationHeader>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
