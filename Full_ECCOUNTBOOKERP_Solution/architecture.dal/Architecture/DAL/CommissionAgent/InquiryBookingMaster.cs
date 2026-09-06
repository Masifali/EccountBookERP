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

public class InquiryBookingMaster
{
	public int SetData(Architecture.Model.CommissionAgent.InquiryBookingMaster obj, string procName)
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
				List<InquiryBookingDetail> inquiryBookingDetailList = obj.InquiryBookingDetailList;
				if (inquiryBookingDetailList != null && inquiryBookingDetailList.Count == 0)
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
				obj.inquiryBookingMasterId = ((returnID > 0) ? returnID : obj.inquiryBookingMasterId);
				foreach (InquiryBookingDetail detail4 in obj.InquiryBookingDetailList)
				{
					detail4.inquiryBookingMasterId = obj.inquiryBookingMasterId;
					GenericProvider.SetProc(sqlTrn, detail4, "[cmagt].[USP_inquiryBookingDetail_Insert]");
				}
				foreach (InquiryBookingPaymentSchedule detail3 in obj.InquiryBookingPaymentScheduleList)
				{
					detail3.inquiryBookingMasterId = obj.inquiryBookingMasterId;
					GenericProvider.SetProc(sqlTrn, detail3, "[cmagt].[USP_inquiryBookingPaymentSchedule_Insert]");
				}
				foreach (InquiryBookingQualitySpecification detail2 in obj.InquiryBookingQualitySpecificationList)
				{
					detail2.inquiryBookingMasterId = obj.inquiryBookingMasterId;
					GenericProvider.SetProc(sqlTrn, detail2, "[cmagt].[USP_inquiryBookingQualitySpecification_Insert]");
				}
				foreach (InquiryBookingPartyDetail detail in obj.InquiryBookingPartyDetailList)
				{
					detail.inquiryBookingMasterId = obj.inquiryBookingMasterId;
					GenericProvider.SetProc(sqlTrn, detail, "[cmagt].[USP_InquiryBookingPartyDetail_Insert]");
				}
				List<DMSAttachments> attachmentsList3 = obj.AttachmentsList;
				if (attachmentsList3 != null && attachmentsList3.Count > 0)
				{
					List<SqlParameter> p = new List<SqlParameter>
					{
						new SqlParameter("@ScreenName", obj.AttachmentsList[0].ScreenName),
						new SqlParameter("@Id", obj.inquiryBookingMasterId),
						new SqlParameter("@Activity", "DeleteById")
					};
					new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
					foreach (DMSAttachments att in obj.AttachmentsList)
					{
						att.RefAccountId = obj.entryUserId;
						att.RefDocumentTypeId = obj.documentTypeId;
						att.RefDocumentNo = obj.inquiryBookingMasterId;
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
		return obj.inquiryBookingMasterId;
	}

	public List<Architecture.Model.CommissionAgent.InquiryBookingMaster> GetData(string procName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.CommissionAgent.InquiryBookingMaster> list;
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				list = GenericProvider.GetProc<Architecture.Model.CommissionAgent.InquiryBookingMaster>(sqlConnection, procName, p);
			}
			if (list != null && list.Count > 0)
			{
				using SqlConnection sqlCon = ConnectionObject.SQL();
				sqlCon.Open();
				foreach (Architecture.Model.CommissionAgent.InquiryBookingMaster item in list)
				{
					List<SqlParameter> prm = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.inquiryBookingMasterId),
						new SqlParameter("@Activity", "ReadByHeaderId_inquiryBookingDetail")
					};
					item.InquiryBookingDetailList = GenericProvider.GetAllDetail<InquiryBookingDetail>(sqlCon, "[cmagt].[USP_inquiryBookingMaster_GetAllMethod]", prm);
					List<SqlParameter> prmForSchudlueList = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.inquiryBookingMasterId),
						new SqlParameter("@Activity", "ReadByHeaderId_inquiryBookingPaymentSchedule")
					};
					item.InquiryBookingPaymentScheduleList = GenericProvider.GetAllDetail<InquiryBookingPaymentSchedule>(sqlCon, "[cmagt].[USP_inquiryBookingMaster_GetAllMethod]", prmForSchudlueList);
					List<SqlParameter> prmForQualitySpecificationList = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.inquiryBookingMasterId),
						new SqlParameter("@Activity", "ReadByHeaderId_inquiryBookingQualitySpecification")
					};
					item.InquiryBookingQualitySpecificationList = GenericProvider.GetAllDetail<InquiryBookingQualitySpecification>(sqlCon, "[cmagt].[USP_inquiryBookingMaster_GetAllMethod]", prmForQualitySpecificationList);
					List<SqlParameter> InquiryBookingPartyDetailList = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.inquiryBookingMasterId),
						new SqlParameter("@Activity", "ReadByHeaderId_InquiryBookingPartyDetail")
					};
					item.InquiryBookingPartyDetailList = GenericProvider.GetAllDetail<InquiryBookingPartyDetail>(sqlCon, "[cmagt].[USP_inquiryBookingMaster_GetAllMethod]", InquiryBookingPartyDetailList);
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
