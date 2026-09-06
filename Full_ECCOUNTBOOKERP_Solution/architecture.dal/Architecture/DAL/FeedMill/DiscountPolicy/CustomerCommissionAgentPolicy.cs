using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.FeedMill.DiscountPolicy;

namespace Architecture.DAL.FeedMill.DiscountPolicy;

public class CustomerCommissionAgentPolicy
{
	public int SetData(Architecture.Model.FeedMill.DiscountPolicy.CustomerCommissionAgentPolicy obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.FeedMill.DiscountPolicy.CustomerCommissionAgentPolicy> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.FeedMill.DiscountPolicy.CustomerCommissionAgentPolicy> lst = new List<Architecture.Model.FeedMill.DiscountPolicy.CustomerCommissionAgentPolicy>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.FeedMill.DiscountPolicy.CustomerCommissionAgentPolicy>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
