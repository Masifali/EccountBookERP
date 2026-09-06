using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class InvCommAgentOrder
{
	public int SetData(Architecture.Model.Inventory.InvCommAgentOrder obj, string ProcName)
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
			foreach (InvCommAgentOrderDetail item in obj.InvCommAgentOrderDetailslist)
			{
				item.InvCommAgentOrderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_InvCommAgentOrderDetail_Insert"));
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

	public List<Architecture.Model.Inventory.InvCommAgentOrder> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.InvCommAgentOrder> lst = new List<Architecture.Model.Inventory.InvCommAgentOrder>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Inventory.InvCommAgentOrder>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Inventory.InvCommAgentOrder item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.InvCommAgentOrderDetailslist = GenericProvider.GetAllDetail<InvCommAgentOrderDetail>(sqlcon, "Sp_InvCommAgentOrder_ReadAll", lstPrm);
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
