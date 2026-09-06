using System;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.pcc;

namespace Architecture.DAL.pcc;

public class ItemPricingSchedule
{
	public int SetData(Architecture.Model.pcc.ItemPricingSchedule obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.pcc.ItemPricingSchedule item in obj.ItemPricingScheduleslist)
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
