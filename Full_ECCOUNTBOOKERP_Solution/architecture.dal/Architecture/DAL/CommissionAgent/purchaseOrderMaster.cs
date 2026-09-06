using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.CommissionAgent;
using Architecture.Model.VPS;

namespace Architecture.DAL.CommissionAgent;

public class purchaseOrderMaster
{
	public int SetData(Architecture.Model.CommissionAgent.purchaseOrderMaster obj, string procName)
	{
		int returnID = 0;
		List<string> copiedFiles = new List<string>();
		string attachmentFolderPath = "";
		ServerCredentials serverCredentials = new ServerCredentials();
		bool isVpsAttachmentServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.organizationId, obj.companyId, "IsVpsAttachmentsServiceOn"));
		using (SqlConnection sqlcon = ConnectionObject.SQL())
		{
			sqlcon.Open();
			SqlTransaction sqlTrn = sqlcon.BeginTransaction();
			try
			{
				List<purchaseOrderDetail> purchaseOrderDetailList = obj.purchaseOrderDetailList;
				if (purchaseOrderDetailList != null && purchaseOrderDetailList.Count == 0)
				{
					throw new Exception("Detail list not found");
				}
				if (isVpsAttachmentServiceOn)
				{
					serverCredentials = CommonServices.GetServerCredentials();
				}
				else
				{
					attachmentFolderPath = CommonServices.GetConfigurationFromAllocation(obj.organizationId, obj.companyId, "Attachment Folder Path");
					List<DMSAttachments> attachmentsList = obj.AttachmentsList;
					if (attachmentsList != null && attachmentsList.Count > 0 && !Directory.Exists(attachmentFolderPath))
					{
						throw new Exception("Attachment Folder Path Not Configure Properly");
					}
				}
				List<DMSAttachments> attachmentsList2 = obj.AttachmentsList;
				if (attachmentsList2 != null && attachmentsList2.Count > 0)
				{
					obj.AttachmentsList = CommonServices.CommonMethodForAttachmentsToPhysicallySave(serverCredentials, isVpsAttachmentServiceOn, attachmentFolderPath, obj.AttachmentsList, copiedFiles);
					obj.attachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments x) => x.Attachment));
					obj.customAttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments x) => x.UploadedFileCustomName));
				}
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, procName));
				obj.purchaseOrderMasterId = ((returnID > 0) ? returnID : obj.purchaseOrderMasterId);
				foreach (purchaseOrderDetail detail7 in obj.purchaseOrderDetailList)
				{
					detail7.purchaseOrderMasterId = obj.purchaseOrderMasterId;
					int detailId = Conversion.ToInt(GenericProvider.SetProc(sqlTrn, detail7, "[cmagt].[USP_purchaseOrderDetail_Insert]"));
					detail7.purchaseOrderDetailId = ((detailId > 0) ? detailId : detail7.purchaseOrderDetailId);
				}
				foreach (purchaseOrderPaymentDetail detail6 in obj.purchaseOrderPaymentDetailList)
				{
					detail6.purchaseOrderMasterId = obj.purchaseOrderMasterId;
					GenericProvider.SetProc(sqlTrn, detail6, "[cmagt].[USP_purchaseOrderPaymentDetail_Insert]");
				}
				foreach (purchaseOrderEmptyBagDetail detail5 in obj.PurchaseOrderEmptyBagDetailList)
				{
					detail5.purchaseOrderMasterId = obj.purchaseOrderMasterId;
					GenericProvider.SetProc(sqlTrn, detail5, "[cmagt].[USP_purchaseOrderEmptyBagDetail_Insert]");
				}
				foreach (purchaseOrderSupplierExpenseDetail detail4 in obj.purchaseOrderSupplierExpenseDetailList)
				{
					detail4.purchaseOrderMasterId = obj.purchaseOrderMasterId;
					GenericProvider.SetProc(sqlTrn, detail4, "[cmagt].[USP_purchaseOrderSupplierExpenseDetail_Insert]");
				}
				foreach (purchaseOrderCommissionDetail detail3 in obj.PurchaseOrderCommissionDetailList)
				{
					detail3.purchaseOrderMasterId = obj.purchaseOrderMasterId;
					GenericProvider.SetProc(sqlTrn, detail3, "[cmagt].[USP_purchaseOrderCommissionDetail_Insert]");
				}
				foreach (purchaseOrderSaleOrderMapping detail2 in obj.purchaseOrderSaleOrderMappingList)
				{
					detail2.purchaseOrderMasterId = obj.purchaseOrderMasterId;
					purchaseOrderDetail matchedDetail2 = obj.purchaseOrderDetailList.FirstOrDefault((purchaseOrderDetail r) => r.itemId == detail2.itemId);
					if (matchedDetail2 != null)
					{
						detail2.purchaseOrderDetailId = matchedDetail2.purchaseOrderDetailId;
					}
					GenericProvider.SetProc(sqlTrn, detail2, "[cmagt].[USP_purchaseOrderSaleOrderMapping_Insert]");
				}
				List<SupplierOfferBuyerInquiryMappingDetail> supplierOfferBuyerInquiryMappingDetailList = obj.SupplierOfferBuyerInquiryMappingDetailList;
				if (supplierOfferBuyerInquiryMappingDetailList != null && supplierOfferBuyerInquiryMappingDetailList.Count > 0)
				{
					foreach (SupplierOfferBuyerInquiryMappingDetail detail in obj.SupplierOfferBuyerInquiryMappingDetailList)
					{
						detail.purchaseOrderMasterId = obj.purchaseOrderMasterId;
						purchaseOrderDetail matchedDetail = obj.purchaseOrderDetailList.FirstOrDefault((purchaseOrderDetail r) => r.itemId == detail.itemId);
						if (matchedDetail != null)
						{
							detail.purchaseOrderDetailId = matchedDetail.purchaseOrderDetailId;
						}
						GenericProvider.SetProc(sqlTrn, detail, "[cmagt].[USP_SupplierOfferBuyerInquiryMappingDetail_Insert]");
					}
				}
				List<DMSAttachments> attachmentsList3 = obj.AttachmentsList;
				if (attachmentsList3 != null && attachmentsList3.Count > 0)
				{
					List<SqlParameter> p = new List<SqlParameter>
					{
						new SqlParameter("@ScreenName", obj.AttachmentsList[0].ScreenName),
						new SqlParameter("@Id", obj.purchaseOrderMasterId),
						new SqlParameter("@Activity", "DeleteById")
					};
					new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
					foreach (DMSAttachments att in obj.AttachmentsList)
					{
						att.RefAccountId = obj.entryUserId;
						att.RefDocumentTypeId = obj.documentTypeId;
						att.RefDocumentNo = obj.purchaseOrderMasterId;
						att.EntryUser = obj.entryUserId;
						att.ModifyUser = obj.modifyUserId;
						att.EntryDate = DateTime.Now;
						att.ModifyDate = DateTime.Now;
						att.OrganizationId = obj.organizationId;
						att.CompanyId = obj.companyId;
						GenericProvider.SetProc(sqlTrn, att, "Proc_DMSAttachments_Insert");
					}
				}
				List<DMSAttachments> deleteAttachmentsList = obj.DeleteAttachmentsList;
				if (deleteAttachmentsList != null && deleteAttachmentsList.Count > 0)
				{
					CommonServices.DeleteAttachmentsPhysically(serverCredentials, isVpsAttachmentServiceOn, attachmentFolderPath, obj.DeleteAttachmentsList);
				}
				sqlTrn.Commit();
			}
			catch
			{
				sqlTrn.Rollback();
				CommonServices.DeleteAttachmentsPhysicallyInException(serverCredentials, isVpsAttachmentServiceOn, attachmentFolderPath, copiedFiles);
				throw;
			}
		}
		return obj.purchaseOrderMasterId;
	}

	public List<Architecture.Model.CommissionAgent.purchaseOrderMaster> GetData(string procName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.CommissionAgent.purchaseOrderMaster> list;
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				list = GenericProvider.GetAllDetail<Architecture.Model.CommissionAgent.purchaseOrderMaster>(sqlConnection, procName, p);
			}
			if (list != null && list.Count > 0)
			{
				using SqlConnection sqlCon = ConnectionObject.SQL();
				sqlCon.Open();
				foreach (Architecture.Model.CommissionAgent.purchaseOrderMaster item in list)
				{
					List<SqlParameter> prm = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.purchaseOrderMasterId),
						new SqlParameter("@Activity", "ReadByHeaderId_purchaseOrderDetail")
					};
					item.purchaseOrderDetailList = GenericProvider.GetAllDetail<purchaseOrderDetail>(sqlCon, "[cmagt].[USP_purchaseOrderMaster_GetAllMethod]", prm);
					List<SqlParameter> prmForExpenseLis = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.purchaseOrderMasterId),
						new SqlParameter("@Activity", "ReadByHeaderId_purchaseOrderSupplierExpenseDetail")
					};
					item.purchaseOrderSupplierExpenseDetailList = GenericProvider.GetAllDetail<purchaseOrderSupplierExpenseDetail>(sqlCon, "[cmagt].[USP_purchaseOrderMaster_GetAllMethod]", prmForExpenseLis);
					List<SqlParameter> prmForEbList = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.purchaseOrderMasterId),
						new SqlParameter("@Activity", "ReadByHeaderId_purchaseOrderEmptyBagDetail")
					};
					item.PurchaseOrderEmptyBagDetailList = GenericProvider.GetAllDetail<purchaseOrderEmptyBagDetail>(sqlCon, "[cmagt].[USP_purchaseOrderMaster_GetAllMethod]", prmForEbList);
					List<SqlParameter> prmForCommList = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.purchaseOrderMasterId),
						new SqlParameter("@Activity", "ReadByHeaderId_purchaseOrderCommissionDetailId")
					};
					item.PurchaseOrderCommissionDetailList = GenericProvider.GetAllDetail<purchaseOrderCommissionDetail>(sqlCon, "[cmagt].[USP_purchaseOrderMaster_GetAllMethod]", prmForCommList);
					List<SqlParameter> prmForSchudlueList = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.purchaseOrderMasterId),
						new SqlParameter("@Activity", "ReadByHeaderId_purchaseOrderPaymentDetail")
					};
					item.purchaseOrderPaymentDetailList = GenericProvider.GetAllDetail<purchaseOrderPaymentDetail>(sqlCon, "[cmagt].[USP_purchaseOrderMaster_GetAllMethod]", prmForSchudlueList);
					List<SqlParameter> prmForMappingList = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.purchaseOrderMasterId),
						new SqlParameter("@Activity", "ReadByHeaderId_purchaseOrderSaleOrderMapping")
					};
					item.purchaseOrderSaleOrderMappingList = GenericProvider.GetAllDetail<purchaseOrderSaleOrderMapping>(sqlCon, "[cmagt].[USP_purchaseOrderMaster_GetAllMethod]", prmForMappingList);
					List<SqlParameter> OfferForMappingList = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.purchaseOrderMasterId),
						new SqlParameter("@DocumentTypeId", item.documentTypeId)
					};
					item.SupplierOfferBuyerInquiryMappingDetailList = GenericProvider.GetAllDetail<SupplierOfferBuyerInquiryMappingDetail>(sqlCon, "[cmagt].[USP_SupplierOfferBuyerInquiryMappingDetail_ReadById]", OfferForMappingList);
				}
			}
		}
		catch
		{
			throw;
		}
		return list;
	}
}
