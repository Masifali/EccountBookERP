using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.pcc;

namespace Architecture.DAL.pcc;

public class InvDeliveryOrder
{
	public int SetData(Architecture.Model.pcc.InvDeliveryOrder obj, string ProcName)
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
			foreach (InvDeliveryOrderdetail item2 in obj.InvDeliveryOrderDetailList)
			{
				item2.InvDeliveryOrderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "[pcc].[USP_InvDeliveryOrderDetail_Insert]"));
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				foreach (DMSAttachments item in obj.AttachmentsList.ToList())
				{
					item.RefAccountId = returnID;
					item.RefDocumentTypeId = obj.DocumentTypeId;
					item.RefDocumentNo = returnID;
					item.EntryDate = DateTime.Now;
					item.ModifyDate = DateTime.Now;
					item.EntryUser = clsGlobalVariables.UserAccount.ID;
					item.ModifyUser = clsGlobalVariables.UserAccount.ID;
					item.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
					item.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
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

	public List<Architecture.Model.pcc.InvDeliveryOrder> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.pcc.InvDeliveryOrder> lst = new List<Architecture.Model.pcc.InvDeliveryOrder>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetAllDetail<Architecture.Model.pcc.InvDeliveryOrder>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.pcc.InvDeliveryOrder item in lst)
				{
					if (item.DocumentTypeId == 1853)
					{
						List<SqlParameter> lstPrm = new List<SqlParameter>();
						lstPrm.Add(new SqlParameter("@Id", item.Id));
						lstPrm.Add(new SqlParameter("@Activity", "ReadByDeliveryOrderHeaderId"));
						item.InvDeliveryOrderDetailList = GenericProvider.GetAllDetail<InvDeliveryOrderdetail>(sqlcon, "[pcc].[USP_InvDeliveryOrder_GetAllMethod]", lstPrm);
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
