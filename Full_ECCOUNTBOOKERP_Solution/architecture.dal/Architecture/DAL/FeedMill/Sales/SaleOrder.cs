using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.FeedMill.Sales;
using Architecture.Model.Inventory;

namespace Architecture.DAL.FeedMill.Sales;

public class SaleOrder
{
	public int SetData(Architecture.Model.FeedMill.Sales.SaleOrder obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.SaleOrderDetailList == null || obj.SaleOrderDetailList.Count == 0)
			{
				throw new Exception("SaleOrderDetailList not found");
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
			decimal LimitAmount = default(decimal);
			foreach (Architecture.Model.FeedMill.Sales.SaleOrderDetail item4 in obj.SaleOrderDetailList)
			{
				item4.SaleOrderId = obj.Id;
				if (item4.ActionTypeId != 3)
				{
					LimitAmount += Conversion.ToDecimal(item4.Amount);
				}
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[fed].[USP_SaleOrderDetail_Insert]"));
			}
			if (obj.SaleOrderPaymentTermsDetailslist != null && obj.SaleOrderPaymentTermsDetailslist.Count > 0)
			{
				int lineId2 = 1;
				foreach (Architecture.Model.FeedMill.Sales.SaleOrderPaymentTermsDetail item3 in obj?.SaleOrderPaymentTermsDetailslist)
				{
					item3.SaleOrderId = obj.Id;
					item3.SortNo = lineId2;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "[fed].[USP_SaleOrderPaymentTermsDetail_Insert]"));
					lineId2++;
				}
			}
			if (obj.SaleOrderCustomerExpenseslist != null && obj.SaleOrderCustomerExpenseslist.Count > 0)
			{
				int lineId = 1;
				foreach (Architecture.Model.FeedMill.Sales.SaleOrderCustomerExpenses item2 in obj?.SaleOrderCustomerExpenseslist)
				{
					item2.SaleOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "[fed].[USP_SaleOrderCustomerExpenses_Insert]"));
					lineId++;
				}
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				foreach (DMSAttachments item in obj.AttachmentsList.ToList())
				{
					item.RefAccountId = obj.OrderSupCustId;
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
			DocumentApprovalDetail AP = new DocumentApprovalDetail();
			AP.OrganizationId = obj.OrganizationId;
			AP.CompanyId = obj.CompanyId;
			AP.DocumentTypeId = obj.DocumentTypeId;
			AP.Id = obj.Id;
			AP.LimitAmount = LimitAmount;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, AP, "[DAW].[USp_DocumentApprovalDetail_Insert]"));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public int SetDataDetail(Architecture.Model.Inventory.SaleOrder obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Inventory.SaleOrderDetail item in obj.SaleOrderDetailList)
			{
				item.SaleOrderId = obj.Id;
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

	public List<Architecture.Model.FeedMill.Sales.SaleOrder> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.FeedMill.Sales.SaleOrder> lst = new List<Architecture.Model.FeedMill.Sales.SaleOrder>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.FeedMill.Sales.SaleOrder>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.FeedMill.Sales.SaleOrder item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
						item3.SaleOrderDetailList = GenericProvider.GetAllDetail<Architecture.Model.FeedMill.Sales.SaleOrderDetail>(sqlConnection2, "[fed].[usp_SaleOrder_GetAllMethod]", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.FeedMill.Sales.SaleOrder item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "SaleOrderPaymentTermDetailByHeaderId"));
						item2.SaleOrderPaymentTermsDetailslist = GenericProvider.GetAllDetail<Architecture.Model.FeedMill.Sales.SaleOrderPaymentTermsDetail>(sqlConnection3, "[fed].[usp_SaleOrder_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.FeedMill.Sales.SaleOrder item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "SaleOrderCustomerExpensesByHeaderId"));
					item.SaleOrderCustomerExpenseslist = GenericProvider.GetAllDetail<Architecture.Model.FeedMill.Sales.SaleOrderCustomerExpenses>(sqlcon, "[fed].[usp_SaleOrder_GetAllMethod]", lstPrm);
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
