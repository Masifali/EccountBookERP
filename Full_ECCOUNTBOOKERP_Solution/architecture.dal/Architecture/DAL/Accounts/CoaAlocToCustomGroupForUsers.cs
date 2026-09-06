using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.DAL.Accounts;

public class CoaAlocToCustomGroupForUsers
{
	public int SetData(Architecture.Model.Accounts.CoaAlocToCustomGroupForUsers obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Accounts.CoaAlocToCustomGroupForUsers item in obj.CoaAlocToCustomGroupForUsersList)
			{
				obj.OrganizationId = item.OrganizationId;
				obj.CompanyId = item.CompanyId;
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
			}
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "USP_AccountsStatusUserWiseInsertIntoTmpTable";
			sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
			sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Accounts.CoaAlocToCustomGroupForUsers> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Accounts.CoaAlocToCustomGroupForUsers> lst = new List<Architecture.Model.Accounts.CoaAlocToCustomGroupForUsers>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Accounts.CoaAlocToCustomGroupForUsers>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public int AccountsStatusShowHideSave(AccountsShowAndHideStatusUserWise obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (AccountsShowAndHideStatusUserWise item in obj.AccountsShowAndHideStatusUserWiseslist)
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
}
