using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.POS;

namespace Architecture.DAL.Pos;

public class ItemPricingSchedule : DALBase
{
	public int SetData(Architecture.Model.POS.ItemPricingSchedule obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.POS.ItemPricingSchedule item2 in obj.ItemPricingScheduleslist)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, ProcName));
			}
			if (obj.SalesPricingScheduleBreakupsList != null && obj.SalesPricingScheduleBreakupsList.Count > 0)
			{
				foreach (SalesPricingScheduleBreakup item in obj.SalesPricingScheduleBreakupsList)
				{
					returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "[dbo].[USP_SalesPricingScheduleBreakup_Insert]"));
				}
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

	public List<Architecture.Model.POS.ItemPricingSchedule> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.POS.ItemPricingSchedule> lst = new List<Architecture.Model.POS.ItemPricingSchedule>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<Architecture.Model.POS.ItemPricingSchedule>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
