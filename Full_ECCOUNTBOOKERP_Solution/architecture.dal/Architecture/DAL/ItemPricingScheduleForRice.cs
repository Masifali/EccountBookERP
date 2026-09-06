using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class ItemPricingScheduleForRice
{
	public int SetData(Architecture.Model.ItemPricingScheduleForRice obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.ItemPricingScheduleForRice item in obj.ItemPricingScheduleForRicelist)
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

	public List<Architecture.Model.ItemPricingScheduleForRice> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ItemPricingScheduleForRice> lst = new List<Architecture.Model.ItemPricingScheduleForRice>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.ItemPricingScheduleForRice>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
