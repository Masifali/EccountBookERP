using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class DeliverySchedule : DALBase
{
	public int SetData(Architecture.Model.Inventory.DeliverySchedule obj, string ProcName)
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
			else
			{
				returnID = obj.Id;
			}
			foreach (DeliveryScheduleDetail item2 in obj.DeliveryScheduleDetail)
			{
				item2.DeliveryScheduleId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_DeliveryScheduleDetail_Insert"));
			}
			foreach (DeliveryScheduleDetailDates item in obj.DeliveryScheduleDetaildate)
			{
				item.DeliveryScheduleId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_DeliveryScheduleDetailDates_Insert"));
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

	public List<Architecture.Model.Inventory.DeliverySchedule> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.DeliverySchedule> lst = new List<Architecture.Model.Inventory.DeliverySchedule>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Inventory.DeliverySchedule>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Inventory.DeliverySchedule item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByDeliveryScheduleID"));
					item.DeliveryScheduleDetail = GenericProvider.Get<DeliveryScheduleDetail>(sqlcon, "Sp_DeliverySchedule_GetAllMethod", lstPrm);
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
