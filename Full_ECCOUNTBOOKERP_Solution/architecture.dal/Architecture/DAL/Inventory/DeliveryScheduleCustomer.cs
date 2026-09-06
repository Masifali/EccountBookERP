using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class DeliveryScheduleCustomer
{
	public int SetData(Architecture.Model.Inventory.DeliveryScheduleCustomer obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.DeliveryScheduleCustomerDetailList == null || obj.DeliveryScheduleCustomerDetailList.Count == 0)
			{
				throw new Exception("Detail List not found");
			}
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (DeliveryScheduleCustomerDetail item in obj.DeliveryScheduleCustomerDetailList)
			{
				item.DeliveryScheduleCustomerId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "USP_DeliveryScheduleCustomerDetail_Insert"));
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

	public List<Architecture.Model.Inventory.DeliveryScheduleCustomer> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.DeliveryScheduleCustomer> lst = new List<Architecture.Model.Inventory.DeliveryScheduleCustomer>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Inventory.DeliveryScheduleCustomer>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Inventory.DeliveryScheduleCustomer item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.DeliveryScheduleCustomerDetailList = GenericProvider.GetAllDetail<DeliveryScheduleCustomerDetail>(sqlcon, "usp_DeliveryScheduleCustomer_GetAllMethod", lstPrm);
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
