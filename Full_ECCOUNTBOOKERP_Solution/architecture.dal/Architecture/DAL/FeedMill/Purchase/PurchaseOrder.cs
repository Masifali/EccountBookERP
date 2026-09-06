using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.FeedMill.Purchase;

namespace Architecture.DAL.FeedMill.Purchase;

public class PurchaseOrder
{
	public int SetData(Architecture.Model.FeedMill.Purchase.PurchaseOrder obj, string ProcName)
	{
		int ActionId = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.purchaseOrderDetailList == null || obj.purchaseOrderDetailList.Count <= 0)
			{
				throw new Exception("OrderDetail list not found");
			}
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				ActionId = 2;
				returnID = obj.Id;
			}
			decimal LimitAmount = default(decimal);
			foreach (PurchaseOrderDetail item6 in obj.purchaseOrderDetailList)
			{
				item6.PurchaseOrderId = obj.Id;
				LimitAmount += Convert.ToDecimal(item6.Amount);
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "fed.USP_PurchaseOrderDetail_Insert"));
			}
			if (obj.PurchaseorderEmptyBagsList != null && obj.PurchaseorderEmptyBagsList.Count > 0)
			{
				foreach (PurchaseorderEmptyBags item5 in obj.PurchaseorderEmptyBagsList)
				{
					item5.PurchaseOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "fed.USP_PurchaseOrderEmptyBags_Insert"));
				}
			}
			if (obj.PurchaseOrderExpensesChargeToProductsList != null && obj.PurchaseOrderExpensesChargeToProductsList.Count > 0)
			{
				foreach (PurchaseOrderExpensesChargeToProduct item4 in obj.PurchaseOrderExpensesChargeToProductsList)
				{
					item4.PurchaseOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "fed.USP_PurchaseOrderExpensesChargeToProduct_Insert"));
				}
			}
			if (obj.PurchaseOrderLabDeductionList != null && obj.PurchaseOrderLabDeductionList.Count > 0)
			{
				foreach (PurchaseOrderLabDeduction item3 in obj.PurchaseOrderLabDeductionList)
				{
					item3.PurchaseOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "fed.USP_PurchaseOrderLabDeduction_Insert"));
				}
			}
			if (obj.PurchaseOrderSupplierExpList != null && obj.PurchaseOrderSupplierExpList.Count > 0)
			{
				foreach (PurchaseOrderSupplierExpense item2 in obj.PurchaseOrderSupplierExpList)
				{
					item2.PurchaseOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "fed.USP_PurchaseOrderSupplierExpense_Insert"));
				}
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenName",
					Value = obj.AttachmentsList[0].ScreenName
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = returnID
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "DeleteById"
				});
				new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
				foreach (DMSAttachments item in obj.AttachmentsList.ToList())
				{
					item.RefAccountId = obj.OrderSupCustId;
					item.RefDocumentTypeId = obj.DocumentTypeId;
					item.RefDocumentNo = returnID;
					item.EntryDate = DateTime.Now;
					item.ModifyDate = DateTime.Now;
					item.EntryUser = obj.EntryUserId;
					item.ModifyUser = obj.EntryUserId;
					item.OrganizationId = obj.OrganizationId;
					item.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Proc_DMSAttachments_Insert"));
				}
			}
			if (ActionId != 2)
			{
				DocumentApprovalDetail AP = new DocumentApprovalDetail();
				AP.OrganizationId = obj.OrganizationId;
				AP.CompanyId = obj.CompanyId;
				AP.DocumentTypeId = obj.DocumentTypeId;
				AP.Id = obj.Id;
				AP.LimitAmount = LimitAmount;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, AP, "[DAW].[USp_DocumentApprovalDetail_Insert]"));
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

	public int SetDataDetail(Architecture.Model.FeedMill.Purchase.PurchaseOrder obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (PurchaseOrderDetail item in obj.purchaseOrderDetailList)
			{
				item.PurchaseOrderId = obj.Id;
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

	public List<Architecture.Model.FeedMill.Purchase.PurchaseOrder> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.FeedMill.Purchase.PurchaseOrder> lst = new List<Architecture.Model.FeedMill.Purchase.PurchaseOrder>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.FeedMill.Purchase.PurchaseOrder>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.FeedMill.Purchase.PurchaseOrder item5 in lst)
					{
						List<SqlParameter> lstPrm5 = new List<SqlParameter>();
						lstPrm5.Add(new SqlParameter("@Id", item5.Id));
						lstPrm5.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
						item5.purchaseOrderDetailList = GenericProvider.GetAllDetail<PurchaseOrderDetail>(sqlConnection2, "[fed].[USP_PurchaseOrder_GetAllMethod]", lstPrm5);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.FeedMill.Purchase.PurchaseOrder item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "ReadPurchaseOrderEmptyBagsDetailByHeaderId"));
						item4.PurchaseorderEmptyBagsList = GenericProvider.GetAllDetail<PurchaseorderEmptyBags>(sqlConnection3, "[fed].[USP_PurchaseOrder_GetAllMethod]", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.FeedMill.Purchase.PurchaseOrder item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadPurchaseOrderExpensesChargeToProductDetailByHeaderId"));
						item3.PurchaseOrderExpensesChargeToProductsList = GenericProvider.GetAllDetail<PurchaseOrderExpensesChargeToProduct>(sqlConnection4, "[fed].[USP_PurchaseOrder_GetAllMethod]", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection5 = ConnectionObject.SQL())
			{
				sqlConnection5.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.FeedMill.Purchase.PurchaseOrder item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadPurchaseOrderLabDeductionByHeaderId"));
						item2.PurchaseOrderLabDeductionList = GenericProvider.GetAllDetail<PurchaseOrderLabDeduction>(sqlConnection5, "[fed].[USP_PurchaseOrder_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.FeedMill.Purchase.PurchaseOrder item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadPurchaseOrderSupplierExpenseByHeaderId"));
					item.PurchaseOrderSupplierExpList = GenericProvider.GetAllDetail<PurchaseOrderSupplierExpense>(sqlcon, "[fed].[USP_PurchaseOrder_GetAllMethod]", lstPrm);
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
