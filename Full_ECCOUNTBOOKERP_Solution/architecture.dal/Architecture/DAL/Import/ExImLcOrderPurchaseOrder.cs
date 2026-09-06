using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Import;

namespace Architecture.DAL.Import;

public class ExImLcOrderPurchaseOrder : DALBase
{
	public int SetDate(Architecture.Model.Import.ExImLcOrderPurchaseOrder obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			if (obj.DocumentTypeId == 232)
			{
				foreach (ExImLcOrderPurchaseOrderDetail item in obj.EximLcorderPurchaseDetail)
				{
					item.NetWeight = item.Qty * 1000.0;
					item.ExImLcOrderPurchaseOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_ExImLcOrderPurchaseOrderDetail_Insert"));
				}
			}
			else
			{
				foreach (ImPurchaseOrderPackingDetail item2 in obj.ImPurchaseOrderPackingDetailslst)
				{
					item2.NetWeight = item2.Qty * 1000.0;
					item2.ExImLcOrderPurchaseOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_ImPurchaseOrderPackingDetail_Insert"));
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

	public List<Architecture.Model.Import.ExImLcOrderPurchaseOrder> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Import.ExImLcOrderPurchaseOrder> lst = new List<Architecture.Model.Import.ExImLcOrderPurchaseOrder>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetAllDetail<Architecture.Model.Import.ExImLcOrderPurchaseOrder>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Import.ExImLcOrderPurchaseOrder item in lst)
				{
					if (item.DocumentTypeId == 232)
					{
						List<SqlParameter> lstPrm = new List<SqlParameter>();
						lstPrm.Add(new SqlParameter("@Id", item.Id));
						lstPrm.Add(new SqlParameter("@Activity", "ReadByLcPurchaseOrderHeaderId"));
						item.EximLcorderPurchaseDetail = GenericProvider.Get<ExImLcOrderPurchaseOrderDetail>(sqlcon, "Sp_ExImLcOrderPurchaseOrder_GetAllMethod", lstPrm);
					}
					else
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadImPurchaseOrderPackingDetailByHeaderId"));
						item.ImPurchaseOrderPackingDetailslst = GenericProvider.Get<ImPurchaseOrderPackingDetail>(sqlcon, "Sp_ExImLcOrderPurchaseOrder_GetAllMethod", lstPrm2);
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
