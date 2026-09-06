using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.VPS;

namespace Architecture.DAL.Inventory;

public class PurchaseOrder
{
	public int SetData(Architecture.Model.Inventory.PurchaseOrder obj, string ProcName)
	{
		int ActionId = 0;
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.purchaseOrderDetailList == null || obj.purchaseOrderDetailList.Count <= 0)
			{
				throw new Exception("OrderDetail list not found");
			}
			if (IsVpsAttachmentsServiceOn)
			{
				Sc = CommonServices.GetServerCredentials();
			}
			else
			{
				AttachmentFolderPath = CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Attachment Folder Path");
				if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0 && !Directory.Exists(AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				obj.AttachmentsList = CommonServices.CommonMethodForAttachmentsToPhysicallySave(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.AttachmentsList, copiedFiles);
				obj.AttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.Attachment));
				obj.CustomAttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.UploadedFileCustomName));
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
			foreach (PurchaseOrderDetail item9 in obj.purchaseOrderDetailList)
			{
				item9.PurchaseOrderId = obj.Id;
				LimitAmount += Convert.ToDecimal(item9.Amount);
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item9, "Sp_PurchaseOrderDetail_Insert"));
			}
			if (obj.PurchaseorderEmptyBagsList != null && obj.PurchaseorderEmptyBagsList.Count > 0)
			{
				foreach (PurchaseorderEmptyBags item8 in obj.PurchaseorderEmptyBagsList)
				{
					item8.PurchaseOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "Sp_PurchaseOrderEmptyBags_Insert"));
				}
			}
			if (obj.PurchaseOrderExpensesChargeToProductsList != null && obj.PurchaseOrderExpensesChargeToProductsList.Count > 0)
			{
				foreach (PurchaseOrderExpensesChargeToProduct item7 in obj.PurchaseOrderExpensesChargeToProductsList)
				{
					item7.PurchaseOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "Sp_PurchaseOrderExpensesChargeToProduct_Insert"));
				}
			}
			if (obj.PurchaseOrderLabDeductionList != null && obj.PurchaseOrderLabDeductionList.Count > 0)
			{
				foreach (PurchaseOrderLabDeduction item6 in obj.PurchaseOrderLabDeductionList)
				{
					item6.PurchaseOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "Sp_PurchaseOrderLabDeduction_Insert"));
				}
			}
			if (obj.PurchaseOrderSupplierExpList != null && obj.PurchaseOrderSupplierExpList.Count > 0)
			{
				foreach (PurchaseOrderSupplierExpense item5 in obj.PurchaseOrderSupplierExpList)
				{
					item5.PurchaseOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "Sp_PurchaseOrderSupplierExpense_Insert"));
				}
			}
			if (obj.SupplierDispatchDetailList != null && obj.SupplierDispatchDetailList.Count > 0)
			{
				foreach (PurchaseOrderSupplierDispatchDetail item4 in obj.SupplierDispatchDetailList)
				{
					item4.PurchaseOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[dbo].[USP_PurchaseOrderSupplierDispatchDetail_Insert]"));
				}
			}
			if (obj.PurchaseOrderPaymentTermsDetailList != null && obj.PurchaseOrderPaymentTermsDetailList.Count > 0)
			{
				foreach (PurchaseOrderPaymentTermsDetail item3 in obj.PurchaseOrderPaymentTermsDetailList)
				{
					item3.PurchaseOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "[dbo].[USP_PurchaseOrderPaymentTermsDetail_Insert]"));
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
				foreach (DMSAttachments item2 in obj.AttachmentsList.ToList())
				{
					item2.RefAccountId = obj.OrderSupCustId;
					item2.RefDocumentTypeId = obj.DocumentTypeId;
					item2.RefDocumentNo = returnID;
					item2.EntryDate = DateTime.Now;
					item2.ModifyDate = DateTime.Now;
					item2.EntryUser = obj.EntryUser;
					item2.ModifyUser = obj.EntryUser;
					item2.OrganizationId = obj.OrganizationId;
					item2.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Proc_DMSAttachments_Insert"));
				}
			}
			if (obj.DeleteAttachmentsList != null && obj.DeleteAttachmentsList.Count > 0)
			{
				CommonServices.DeleteAttachmentsPhysically(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.DeleteAttachmentsList);
			}
			if (ActionId == 2)
			{
				if (!string.IsNullOrEmpty(obj.OrderDetailRemoveIds))
				{
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlcon;
					sqlCommand.Transaction = sqlTrn;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandText = "USP_DeletePurchaseOrderDetailIfNotExistInGrn";
					sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand.Parameters.AddWithValue("@OrderId", obj.Id);
					sqlCommand.Parameters.AddWithValue("@UserId", obj.ModifyUser);
					sqlCommand.Parameters.AddWithValue("@OrderDetailIds", obj.OrderDetailRemoveIds);
					sqlCommand.ExecuteNonQuery();
				}
				SqlCommand sqlCommand2 = new SqlCommand();
				sqlCommand2.Connection = sqlcon;
				sqlCommand2.Transaction = sqlTrn;
				sqlCommand2.CommandType = CommandType.StoredProcedure;
				sqlCommand2.CommandText = "Sp_PurchaseOrder_GetAllMethod";
				sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand2.Parameters.AddWithValue("@Id", obj.Id);
				sqlCommand2.Parameters.AddWithValue("@Activity", "PoWeightAndGpWeightValidation");
				sqlCommand2.ExecuteNonQuery();
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
			CommonServices.DeleteAttachmentsPhysicallyInException(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, copiedFiles);
			throw ex;
		}
	}

	public int SetDataDetail(Architecture.Model.Inventory.PurchaseOrder obj, string ProcName)
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

	public List<Architecture.Model.Inventory.PurchaseOrder> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.PurchaseOrder> lst = new List<Architecture.Model.Inventory.PurchaseOrder>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Inventory.PurchaseOrder>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.PurchaseOrder item7 in lst)
					{
						List<SqlParameter> lstPrm7 = new List<SqlParameter>();
						lstPrm7.Add(new SqlParameter("@Id", item7.Id));
						lstPrm7.Add(new SqlParameter("@Activity", "ReadByPurchaseOrderHeaderId"));
						item7.purchaseOrderDetailList = GenericProvider.GetAllDetail<PurchaseOrderDetail>(sqlConnection2, "Sp_PurchaseOrder_GetAllMethod", lstPrm7);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.PurchaseOrder item6 in lst)
					{
						List<SqlParameter> lstPrm6 = new List<SqlParameter>();
						lstPrm6.Add(new SqlParameter("@Id", item6.Id));
						lstPrm6.Add(new SqlParameter("@Activity", "ReadPurchaseOrderEmptyBagsDetailByHeaderId"));
						item6.PurchaseorderEmptyBagsList = GenericProvider.GetAllDetail<PurchaseorderEmptyBags>(sqlConnection3, "Sp_PurchaseOrder_GetAllMethod", lstPrm6);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.PurchaseOrder item5 in lst)
					{
						List<SqlParameter> lstPrm5 = new List<SqlParameter>();
						lstPrm5.Add(new SqlParameter("@Id", item5.Id));
						lstPrm5.Add(new SqlParameter("@Activity", "ReadPurchaseOrderExpensesChargeToProductDetailByHeaderId"));
						item5.PurchaseOrderExpensesChargeToProductsList = GenericProvider.GetAllDetail<PurchaseOrderExpensesChargeToProduct>(sqlConnection4, "Sp_PurchaseOrder_GetAllMethod", lstPrm5);
					}
				}
			}
			using (SqlConnection sqlConnection5 = ConnectionObject.SQL())
			{
				sqlConnection5.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.PurchaseOrder item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "ReadPurchaseOrderLabDeductionByHeaderId"));
						item4.PurchaseOrderLabDeductionList = GenericProvider.GetAllDetail<PurchaseOrderLabDeduction>(sqlConnection5, "Sp_PurchaseOrder_GetAllMethod", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection6 = ConnectionObject.SQL())
			{
				sqlConnection6.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.PurchaseOrder item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadPurchaseOrderSupplierExpenseByHeaderId"));
						item3.PurchaseOrderSupplierExpList = GenericProvider.GetAllDetail<PurchaseOrderSupplierExpense>(sqlConnection6, "Sp_PurchaseOrder_GetAllMethod", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection7 = ConnectionObject.SQL())
			{
				sqlConnection7.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.PurchaseOrder item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadByHeaderId_PurchaseOrderSupplierDispatchDetail"));
						item2.SupplierDispatchDetailList = GenericProvider.GetAllDetail<PurchaseOrderSupplierDispatchDetail>(sqlConnection7, "Sp_PurchaseOrder_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Inventory.PurchaseOrder item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "PurchaseOrderPaymentTermDetailByHeaderId"));
					item.PurchaseOrderPaymentTermsDetailList = GenericProvider.GetAllDetail<PurchaseOrderPaymentTermsDetail>(sqlcon, "Sp_PurchaseOrder_GetAllMethod", lstPrm);
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
