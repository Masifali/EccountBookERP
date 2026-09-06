using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.ContractorWages;

namespace Architecture.DAL.ContractorWages;

public class InvConractorWagesAccounts : DALBase
{
	public int SetDate(Architecture.Model.ContractorWages.InvConractorWagesAccounts obj, string ProcName)
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
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.ContractorWages.InvConractorWagesAccounts> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ContractorWages.InvConractorWagesAccounts> lst = new List<Architecture.Model.ContractorWages.InvConractorWagesAccounts>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.ContractorWages.InvConractorWagesAccounts>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
