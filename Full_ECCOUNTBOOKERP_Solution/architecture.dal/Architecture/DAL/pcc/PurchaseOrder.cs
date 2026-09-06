using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.pcc;

namespace Architecture.DAL.pcc;

public class PurchaseOrder
{
	public int SetData(Architecture.Model.pcc.PurchaseOrder obj, string ProcName)
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
			foreach (PurchaseOrderDetail item2 in obj.PurchaseOrderDetaillist)
			{
				item2.PurchaseOrderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "[pcc].[USP_PurchaseOrderDetail_Insert]"));
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				foreach (DMSAttachments item in obj.AttachmentsList.ToList())
				{
					item.RefAccountId = obj.SupplierCustomerId;
					item.RefDocumentTypeId = obj.DocumentTypeId;
					item.RefDocumentNo = returnID;
					item.EntryDate = DateTime.Now;
					item.ModifyDate = DateTime.Now;
					item.EntryUser = obj.EntryUserId;
					item.ModifyUser = obj.ModifyUserId;
					item.OrganizationId = obj.OrganizationId;
					item.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Proc_DMSAttachments_Insert"));
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

	public List<Architecture.Model.pcc.PurchaseOrder> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.pcc.PurchaseOrder> lst = new List<Architecture.Model.pcc.PurchaseOrder>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.pcc.PurchaseOrder>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.pcc.PurchaseOrder item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
					item.PurchaseOrderDetaillist = GenericProvider.GetAllDetail<PurchaseOrderDetail>(sqlcon, "[pcc].[USP_PurchaseOrder_GetAllMethod]", lstPrm);
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
