using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class AccountsPayRecFollowUpHistory
{
	public static int Save(Architecture.Model.Accounts.AccountsPayRecFollowUpHistory obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Accounts.AccountsPayRecFollowUpHistory().SetData(obj, "Sp_AccountsPayRecFollowUpHistory_Insert");
			}
			return new Architecture.DAL.Accounts.AccountsPayRecFollowUpHistory().SetData(obj, "Sp_AccountsPayRecFollowUpHistory_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Accounts.AccountsPayRecFollowUpHistory obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AccountsPayRecFollowUpHistory>("Sp_AccountsPayRecFollowUpHistory_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
