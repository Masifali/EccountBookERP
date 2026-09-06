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

public class saleOrderMaster
{
	public int SetData(Architecture.Model.CommissionAgent.saleOrderMaster obj, string procName)
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
				List<saleOrderDetail> saleOrderDetailList = obj.saleOrderDetailList;
				if (saleOrderDetailList != null && saleOrderDetailList.Count == 0)
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
				obj.saleOrderMasterId = ((returnID > 0) ? returnID : obj.saleOrderMasterId);
				foreach (saleOrderDetail detail5 in obj.saleOrderDetailList)
				{
					detail5.saleOrderMasterId = obj.saleOrderMasterId;
					GenericProvider.SetProc(sqlTrn, detail5, "[cmagt].[USP_saleOrderDetail_Insert]");
				}
				foreach (saleOrderPaymentDetail detail4 in obj.saleOrderPaymentDetailList)
				{
					detail4.saleOrderMasterId = obj.saleOrderMasterId;
					GenericProvider.SetProc(sqlTrn, detail4, "[cmagt].[USP_saleOrderPaymentDetail_Insert]");
				}
				foreach (saleOrderEmptyBagDetail detail3 in obj.saleOrderEmptyBagDetailList)
				{
					detail3.saleOrderMasterId = obj.saleOrderMasterId;
					GenericProvider.SetProc(sqlTrn, detail3, "[cmagt].[USP_saleOrderEmptyBagDetail_Insert]");
				}
				foreach (saleOrderbuyerExpenseDetail detail2 in obj.saleOrderBuyerExpenseDetailList)
				{
					detail2.saleOrderMasterId = obj.saleOrderMasterId;
					GenericProvider.SetProc(sqlTrn, detail2, "[cmagt].[USP_saleOrderBuyerExpenseDetail_Insert]");
				}
				foreach (saleOrderCommissionDetail detail in obj.saleOrderCommissionDetailList)
				{
					detail.saleOrderMasterId = obj.saleOrderMasterId;
					GenericProvider.SetProc(sqlTrn, detail, "[cmagt].[USP_saleOrderCommissionDetail_Insert]");
				}
				List<DMSAttachments> attachmentsList3 = obj.AttachmentsList;
				if (attachmentsList3 != null && attachmentsList3.Count > 0)
				{
					List<SqlParameter> p = new List<SqlParameter>
					{
						new SqlParameter("@ScreenName", obj.AttachmentsList[0].ScreenName),
						new SqlParameter("@Id", obj.saleOrderMasterId),
						new SqlParameter("@Activity", "DeleteById")
					};
					new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
					foreach (DMSAttachments att in obj.AttachmentsList)
					{
						att.RefAccountId = obj.entryUserId;
						att.RefDocumentTypeId = obj.documentTypeId;
						att.RefDocumentNo = obj.saleOrderMasterId;
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
		return obj.saleOrderMasterId;
	}

	public List<Architecture.Model.CommissionAgent.saleOrderMaster> GetData(string procName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.CommissionAgent.saleOrderMaster> list;
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				list = GenericProvider.GetAllDetail<Architecture.Model.CommissionAgent.saleOrderMaster>(sqlConnection, procName, p);
			}
			if (list != null && list.Count > 0)
			{
				using SqlConnection sqlCon = ConnectionObject.SQL();
				sqlCon.Open();
				foreach (Architecture.Model.CommissionAgent.saleOrderMaster item in list)
				{
					List<SqlParameter> prm = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.saleOrderMasterId),
						new SqlParameter("@Activity", "ReadByHeaderId_saleOrderDetail")
					};
					item.saleOrderDetailList = GenericProvider.GetAllDetail<saleOrderDetail>(sqlCon, "[cmagt].[USP_saleOrderMaster_GetAllMethod]", prm);
					List<SqlParameter> prmForExpenseLis = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.saleOrderMasterId),
						new SqlParameter("@Activity", "ReadByHeaderId_saleOrderBuyerExpenseDetail")
					};
					item.saleOrderBuyerExpenseDetailList = GenericProvider.GetAllDetail<saleOrderbuyerExpenseDetail>(sqlCon, "[cmagt].[USP_saleOrderMaster_GetAllMethod]", prmForExpenseLis);
					List<SqlParameter> prmForEbList = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.saleOrderMasterId),
						new SqlParameter("@Activity", "ReadByHeaderId_saleOrderEmptyBagDetail")
					};
					item.saleOrderEmptyBagDetailList = GenericProvider.GetAllDetail<saleOrderEmptyBagDetail>(sqlCon, "[cmagt].[USP_saleOrderMaster_GetAllMethod]", prmForEbList);
					List<SqlParameter> prmForCommList = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.saleOrderMasterId),
						new SqlParameter("@Activity", "ReadByHeaderId_saleOrderCommissionDetailId")
					};
					item.saleOrderCommissionDetailList = GenericProvider.GetAllDetail<saleOrderCommissionDetail>(sqlCon, "[cmagt].[USP_saleOrderMaster_GetAllMethod]", prmForCommList);
					List<SqlParameter> prmForSchudlueList = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.saleOrderMasterId),
						new SqlParameter("@Activity", "ReadByHeaderId_saleOrderPaymentDetail")
					};
					item.saleOrderPaymentDetailList = GenericProvider.GetAllDetail<saleOrderPaymentDetail>(sqlCon, "[cmagt].[USP_saleOrderMaster_GetAllMethod]", prmForSchudlueList);
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
