using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.DAL.Accounts;

public class AccountsAllocateToCustomAccountGroupByDocumentType
{
	public int SetData(Architecture.Model.Accounts.AccountsAllocateToCustomAccountGroupByDocumentType obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Accounts.AccountsAllocateToCustomAccountGroupByDocumentType item in obj.AccountsAllocateToCustomAccountGroupByDocumentTypeList)
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

	public List<Architecture.Model.Accounts.AccountsAllocateToCustomAccountGroupByDocumentType> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Accounts.AccountsAllocateToCustomAccountGroupByDocumentType> lst = new List<Architecture.Model.Accounts.AccountsAllocateToCustomAccountGroupByDocumentType>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Accounts.AccountsAllocateToCustomAccountGroupByDocumentType>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
