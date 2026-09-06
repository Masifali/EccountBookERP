using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Main.Commission;

namespace Architecture.DAL.Main.Commission;

public class CustomerCommissionPolicy
{
	public int SetData(Architecture.Model.Main.Commission.CustomerCommissionPolicy obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Main.Commission.CustomerCommissionPolicy item in obj.CustomerCommissionPolicyList)
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

	public List<Architecture.Model.Main.Commission.CustomerCommissionPolicy> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Main.Commission.CustomerCommissionPolicy> lst = new List<Architecture.Model.Main.Commission.CustomerCommissionPolicy>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Main.Commission.CustomerCommissionPolicy>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
