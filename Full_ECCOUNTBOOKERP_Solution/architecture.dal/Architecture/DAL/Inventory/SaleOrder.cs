using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.VPS;

namespace Architecture.DAL.Inventory;

public class SaleOrder
{
	public int SetData(Architecture.Model.Inventory.SaleOrder obj, string ProcName)
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
			if (obj.SaleOrderDetailList == null || obj.SaleOrderDetailList.Count == 0)
			{
				throw new Exception("SaleOrderDetailList not found");
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
			foreach (SaleOrderDetail item6 in obj.SaleOrderDetailList)
			{
				item6.SaleOrderId = obj.Id;
				if (item6.ActionTypeId != 3)
				{
					LimitAmount += Conversion.ToDecimal(item6.Amount);
				}
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "Sp_SaleOrderDetail_Insert"));
			}
			if (obj.SaleOrderPaymentTermsDetailslist != null && obj.SaleOrderPaymentTermsDetailslist.Count > 0)
			{
				int lineId3 = 1;
				foreach (SaleOrderPaymentTermsDetail item5 in obj?.SaleOrderPaymentTermsDetailslist)
				{
					item5.SaleOrderId = obj.Id;
					item5.SortNo = lineId3;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "USP_SaleOrderPaymentTermsDetail_Insert"));
					lineId3++;
				}
			}
			if (obj.SaleOrderExtraItemsDetaillist != null && obj.SaleOrderExtraItemsDetaillist.Count > 0)
			{
				int lineId2 = 1;
				foreach (SaleOrderExtraItemsDetail item4 in obj?.SaleOrderExtraItemsDetaillist)
				{
					item4.SaleOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "USP_SaleOrderExtraItemsDetail_Insert"));
					lineId2++;
				}
			}
			if (obj.SaleOrderCustomerExpenseslist != null && obj.SaleOrderCustomerExpenseslist.Count > 0)
			{
				int lineId = 1;
				foreach (SaleOrderCustomerExpenses item3 in obj?.SaleOrderCustomerExpenseslist)
				{
					item3.SaleOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "USP_SaleOrderCustomerExpenses_Insert"));
					lineId++;
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
					item2.ModifyUser = obj.ModifyUser;
					item2.OrganizationId = obj.OrganizationId;
					item2.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Proc_DMSAttachments_Insert"));
				}
			}
			if (obj.DeleteAttachmentsList != null && obj.DeleteAttachmentsList.Count > 0)
			{
				CommonServices.DeleteAttachmentsPhysically(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.DeleteAttachmentsList);
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
			CommonServices.DeleteAttachmentsPhysicallyInException(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, copiedFiles);
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
			foreach (SaleOrderDetail item in obj.SaleOrderDetailList)
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

	public List<Architecture.Model.Inventory.SaleOrder> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.SaleOrder> lst = new List<Architecture.Model.Inventory.SaleOrder>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Inventory.SaleOrder>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.SaleOrder item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "ReadBySaleOrderHeaderId"));
						item4.SaleOrderDetailList = GenericProvider.GetAllDetail<SaleOrderDetail>(sqlConnection2, "Sp_SaleOrder_GetAllMethod", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.SaleOrder item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "SaleOrderPaymentTermDetailByHeaderId"));
						item3.SaleOrderPaymentTermsDetailslist = GenericProvider.GetAllDetail<SaleOrderPaymentTermsDetail>(sqlConnection3, "Sp_SaleOrder_GetAllMethod", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.SaleOrder item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadBySaleOrderId_ExtraItemDetail"));
						item2.SaleOrderExtraItemsDetaillist = GenericProvider.GetAllDetail<SaleOrderExtraItemsDetail>(sqlConnection4, "Sp_SaleOrder_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Inventory.SaleOrder item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "SaleOrderCustomerExpensesByHeaderId"));
					item.SaleOrderCustomerExpenseslist = GenericProvider.GetAllDetail<SaleOrderCustomerExpenses>(sqlcon, "Sp_SaleOrder_GetAllMethod", lstPrm);
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
