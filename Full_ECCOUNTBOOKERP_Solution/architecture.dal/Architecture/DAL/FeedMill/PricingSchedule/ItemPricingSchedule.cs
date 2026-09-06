using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.FeedMill.PricingSchedule;

namespace Architecture.DAL.FeedMill.PricingSchedule;

public class ItemPricingSchedule : DALBase
{
	public int SetData(Architecture.Model.FeedMill.PricingSchedule.ItemPricingSchedule obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.FeedMill.PricingSchedule.ItemPricingSchedule item in obj.ScheduleList)
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

	public List<Architecture.Model.FeedMill.PricingSchedule.ItemPricingSchedule> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.FeedMill.PricingSchedule.ItemPricingSchedule> lst = new List<Architecture.Model.FeedMill.PricingSchedule.ItemPricingSchedule>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.FeedMill.PricingSchedule.ItemPricingSchedule>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
