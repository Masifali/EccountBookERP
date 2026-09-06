using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.POS;

namespace Architecture.DAL.Pos;

public class WsRmSalesOrder
{
	public int SetData(Architecture.Model.POS.WsRmSalesOrder obj, string ProcName)
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
			foreach (WsRmSalesOrderDetail item in obj.WsRmSalesOrderDetailsList)
			{
				item.WsRmSalesOrderId = obj.Id;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_WsRmSalesOrderDetail_Insert"));
				foreach (WsRmSalesOrderDetailDiscount Sub in item.WsRmSalesOrderDetailDiscountslist)
				{
					Sub.WsRmSaleOrderId = returnID;
					Sub.WsRmSaleOrderDetailId = code;
					Convert.ToInt64(GenericProvider.SetProc(sqlTrn, Sub, "Sp_WsRmSalesOrderDetailDiscount_Insert"));
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

	public List<Architecture.Model.POS.WsRmSalesOrder> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.POS.WsRmSalesOrder> lst = new List<Architecture.Model.POS.WsRmSalesOrder>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.POS.WsRmSalesOrder>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.POS.WsRmSalesOrder item in lst)
					{
						List<SqlParameter> lstPrm = new List<SqlParameter>();
						lstPrm.Add(new SqlParameter("@Id", item.Id));
						lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
						item.WsRmSalesOrderDetailsList = GenericProvider.GetAllDetail<WsRmSalesOrderDetail>(sqlConnection2, "Sp_WsRmSalesOrder_GetAllMethod", lstPrm);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.POS.WsRmSalesOrder item2 in lst)
				{
					foreach (WsRmSalesOrderDetail Sub in item2.WsRmSalesOrderDetailsList)
					{
						List<SqlParameter> lstPrmSub = new List<SqlParameter>();
						lstPrmSub.Add(new SqlParameter("@Id", Sub.Id));
						lstPrmSub.Add(new SqlParameter("@Activity", "ReadWsRmSalesOrderDetailDiscountByDetailId"));
						Sub.WsRmSalesOrderDetailDiscountslist = GenericProvider.GetProc<WsRmSalesOrderDetailDiscount>(sqlcon, "Sp_WsRmSalesOrder_GetAllMethod", lstPrmSub);
					}
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
