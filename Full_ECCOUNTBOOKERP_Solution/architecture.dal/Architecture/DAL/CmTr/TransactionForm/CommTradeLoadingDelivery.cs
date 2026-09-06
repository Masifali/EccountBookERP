using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.CmTr.TransactionForm;
using Architecture.Model.Inventory;
using Architecture.Model.VPS;

namespace Architecture.DAL.CmTr.TransactionForm;

public class CommTradeLoadingDelivery
{
	public int SetData(Architecture.Model.CmTr.TransactionForm.CommTradeLoadingDelivery obj, string ProcName)
	{
		int returnID = 0;
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
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
			foreach (CommTradeLoadingDeliveryDetail item6 in obj.CommTradeLoadingDeliveryDetailList)
			{
				item6.CommTradeLoadingDeliveryId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "[CmTr].[USP_CommTradeLoadingDeliveryDetail_Insert]"));
			}
			foreach (CommTradeLoadingDeliveryRevExp item5 in obj.CommTradeLoadingDeliveryRevExpList)
			{
				item5.CommTradeLoadingDeliveryId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "[CmTr].[USP_CommTradeLoadingDeliveryRevExp_Insert]"));
			}
			foreach (CommTradeLoadingDeliverySupplierExp item4 in obj.CommTradeLoadingDeliverySupplierExpList)
			{
				item4.CommTradeLoadingDeliveryId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[CmTr].[USP_CommTradeLoadingDeliverySupplierExp_Insert]"));
			}
			foreach (CommTradeLoadingDeliveryPaymentTermsDetail item3 in obj.CommTradeLoadingDeliveryPaymentTermsDetailList)
			{
				item3.CommTradeLoadingDeliveryId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "[CmTr].[USP_CommTradeLoadingDeliveryPaymentTermsDetail_Insert]"));
			}
			foreach (CommTradeLoadingDeliveryPmOther item2 in obj.CommTradeLoadingDeliveryPmOtherList)
			{
				item2.CommTradeLoadingDeliveryId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "[CmTr].[USP_CommTradeLoadingDeliveryPmOther_Insert]"));
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
					item.RefAccountId = obj.CommTradeLoadingDeliveryDetailList[0].SupplierId;
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
			InventoryTransactions transactions = new InventoryTransactions();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = obj.Id;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			if (obj.DeleteAttachmentsList != null && obj.DeleteAttachmentsList.Count > 0)
			{
				CommonServices.DeleteAttachmentsPhysically(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.DeleteAttachmentsList);
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

	public int SetDataSupplierCustomerExpense(Architecture.Model.CmTr.TransactionForm.CommTradeLoadingDelivery obj, string ProcName)
	{
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		using (SqlConnection sqlcon = ConnectionObject.SQL())
		{
			sqlcon.Open();
			SqlTransaction sqlTrn = sqlcon.BeginTransaction();
			try
			{
				if (obj.CommTradeLoadingDeliveryRevExpList.Count <= 0)
				{
					throw new Exception("CommTradeLoadingDeliveryRevExpList not found");
				}
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "[CmTr].[USP_CommTradeLoadingDelivery_GetAllMethod]";
				sqlCommand.Parameters.AddWithValue("@Id", obj.CommTradeLoadingDeliveryRevExpList[0].CommTradeLoadingDeliveryId);
				sqlCommand.Parameters.AddWithValue("@Activity", "DeleteExpensesDetail");
				sqlCommand.ExecuteNonQuery();
				foreach (CommTradeLoadingDeliveryRevExp item3 in obj.CommTradeLoadingDeliveryRevExpList)
				{
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "[CmTr].[USP_CommTradeLoadingDeliveryRevExp_Insert]"));
				}
				if (obj.CommTradeLoadingDeliverySupplierExpList.Count <= 0)
				{
					throw new Exception("CommTradeLoadingDeliverySupplierExpList not found");
				}
				foreach (CommTradeLoadingDeliverySupplierExp item2 in obj.CommTradeLoadingDeliverySupplierExpList)
				{
					item2.Remarks = "";
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "[CmTr].[USP_CommTradeLoadingDeliverySupplierExp_Insert]"));
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
						Value = obj.CommTradeLoadingDeliveryRevExpList[0].CommTradeLoadingDeliveryId
					});
					p.Add(new SqlParameter
					{
						ParameterName = "@Activity",
						Value = "DeleteById"
					});
					new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
					foreach (DMSAttachments item in obj.AttachmentsList.ToList())
					{
						item.RefAccountId = obj.CommTradeLoadingDeliveryDetailList[0].SupplierId;
						item.RefDocumentTypeId = obj.DocumentTypeId;
						item.RefDocumentNo = obj.CommTradeLoadingDeliveryRevExpList[0].CommTradeLoadingDeliveryId;
						item.EntryDate = DateTime.Now;
						item.ModifyDate = DateTime.Now;
						item.EntryUser = obj.EntryUserId;
						item.ModifyUser = obj.ModifyUserId;
						item.OrganizationId = obj.OrganizationId;
						item.CompanyId = obj.CompanyId;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Proc_DMSAttachments_Insert"));
					}
				}
				if (obj.DeleteAttachmentsList != null && obj.DeleteAttachmentsList.Count > 0)
				{
					CommonServices.DeleteAttachmentsPhysically(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.DeleteAttachmentsList);
				}
				sqlTrn.Commit();
			}
			catch (Exception ex)
			{
				sqlTrn.Rollback();
				CommonServices.DeleteAttachmentsPhysicallyInException(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, copiedFiles);
				throw ex;
			}
		}
		return obj.CommTradeLoadingDeliveryRevExpList[0].CommTradeLoadingDeliveryId;
	}

	public List<Architecture.Model.CmTr.TransactionForm.CommTradeLoadingDelivery> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.CmTr.TransactionForm.CommTradeLoadingDelivery> lst = new List<Architecture.Model.CmTr.TransactionForm.CommTradeLoadingDelivery>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.CmTr.TransactionForm.CommTradeLoadingDelivery>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.CmTr.TransactionForm.CommTradeLoadingDelivery item5 in lst)
					{
						List<SqlParameter> lstPrm5 = new List<SqlParameter>();
						lstPrm5.Add(new SqlParameter("@Id", item5.Id));
						lstPrm5.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
						item5.CommTradeLoadingDeliveryDetailList = GenericProvider.GetAllDetail<CommTradeLoadingDeliveryDetail>(sqlConnection2, "[CmTr].[USP_CommTradeLoadingDelivery_GetAllMethod]", lstPrm5);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.CmTr.TransactionForm.CommTradeLoadingDelivery item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "CommTradeLoadingDeliveryRevExp_ReadByCommTradeLoadingDeliveryID"));
						item4.CommTradeLoadingDeliveryRevExpList = GenericProvider.GetAllDetail<CommTradeLoadingDeliveryRevExp>(sqlConnection3, "[CmTr].[USP_CommTradeLoadingDelivery_GetAllMethod]", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.CmTr.TransactionForm.CommTradeLoadingDelivery item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "CommTradeLoadingDeliveryPmOther_ReadByCommTradeLoadingDeliveryID"));
						item3.CommTradeLoadingDeliveryPmOtherList = GenericProvider.GetAllDetail<CommTradeLoadingDeliveryPmOther>(sqlConnection4, "[CmTr].[USP_CommTradeLoadingDelivery_GetAllMethod]", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection5 = ConnectionObject.SQL())
			{
				sqlConnection5.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.CmTr.TransactionForm.CommTradeLoadingDelivery item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "CommTradeLoadingDeliveryPaymentTermsDetail_ReadByHeaderID"));
						item2.CommTradeLoadingDeliveryPaymentTermsDetailList = GenericProvider.GetAllDetail<CommTradeLoadingDeliveryPaymentTermsDetail>(sqlConnection5, "[CmTr].[USP_CommTradeLoadingDelivery_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.CmTr.TransactionForm.CommTradeLoadingDelivery item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "CommTradeLoadingDeliverySupplierExp_ReadByCommTradeLoadingDeliveryID"));
					item.CommTradeLoadingDeliverySupplierExpList = GenericProvider.GetAllDetail<CommTradeLoadingDeliverySupplierExp>(sqlcon, "[CmTr].[USP_CommTradeLoadingDelivery_GetAllMethod]", lstPrm);
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
