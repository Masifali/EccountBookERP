using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.DAL.Accounts;

public class CoaCustomGroupAlocToUsers
{
	public int SetData(Architecture.Model.Accounts.CoaCustomGroupAlocToUsers obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Accounts.CoaCustomGroupAlocToUsers item in obj.CoaCustomGroupAlocToUsersList)
			{
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

	public List<Architecture.Model.Accounts.CoaCustomGroupAlocToUsers> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Accounts.CoaCustomGroupAlocToUsers> lst = new List<Architecture.Model.Accounts.CoaCustomGroupAlocToUsers>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Accounts.CoaCustomGroupAlocToUsers>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
