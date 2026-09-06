using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.PurchaseTrading;

namespace Architecture.DAL.PurchaseTrading;

public class SupplySchedule
{
	public int SetData(Architecture.Model.PurchaseTrading.SupplySchedule obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			foreach (SupplyScheduleDetail item2 in obj.supplyScheduleDetail)
			{
				item2.SupplyScheduleId = obj.Id;
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_SupplyScheduleDetail_Insert"));
			}
			foreach (SupplyScheduleDetailDates item in obj.supplyScheduleDetaildate)
			{
				item.SupplyScheduleId = obj.Id;
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_SupplyScheduleDetailDates_Insert"));
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

	public List<Architecture.Model.PurchaseTrading.SupplySchedule> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.PurchaseTrading.SupplySchedule> lst = new List<Architecture.Model.PurchaseTrading.SupplySchedule>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.PurchaseTrading.SupplySchedule>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.PurchaseTrading.SupplySchedule item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailBySupplyScheduleID"));
					item.supplyScheduleDetail = GenericProvider.Get<SupplyScheduleDetail>(sqlcon, "Sp_SupplySchedule_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}
}
