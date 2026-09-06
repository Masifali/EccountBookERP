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

public class grnSupplierLoadingMaster
{
	public int SetData(Architecture.Model.CommissionAgent.grnSupplierLoadingMaster obj, string procName)
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
				List<grnSupplierLoadingDetail> grnSupplierLoadingDetailList = obj.grnSupplierLoadingDetailList;
				if (grnSupplierLoadingDetailList != null && grnSupplierLoadingDetailList.Count == 0)
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
				obj.grnSupplierLoadingMasterId = ((returnID > 0) ? returnID : obj.grnSupplierLoadingMasterId);
				foreach (grnSupplierLoadingDetail detail3 in obj.grnSupplierLoadingDetailList)
				{
					detail3.grnSupplierLoadingMasterId = obj.grnSupplierLoadingMasterId;
					GenericProvider.SetProc(sqlTrn, detail3, "[cmagt].[USP_grnSupplierLoadingDetail_Insert]");
				}
				foreach (grnSupplierLoadingEmptyBagDetail detail2 in obj.grnSupplierLoadingEmptyBagDetailList)
				{
					detail2.grnSupplierLoadingMasterId = obj.grnSupplierLoadingMasterId;
					GenericProvider.SetProc(sqlTrn, detail2, "[cmagt].[USP_grnSupplierLoadingEmptyBagDetail_Insert]");
				}
				foreach (grnSupplierLoadingExpenseDetail detail in obj.grnSupplierLoadingExpenseDetailList)
				{
					detail.grnSupplierLoadingMasterId = obj.grnSupplierLoadingMasterId;
					GenericProvider.SetProc(sqlTrn, detail, "[cmagt].[USP_grnSupplierLoadingExpenseDetail_Insert]");
				}
				List<DMSAttachments> attachmentsList3 = obj.AttachmentsList;
				if (attachmentsList3 != null && attachmentsList3.Count > 0)
				{
					List<SqlParameter> p = new List<SqlParameter>
					{
						new SqlParameter("@ScreenName", obj.AttachmentsList[0].ScreenName),
						new SqlParameter("@Id", obj.grnSupplierLoadingMasterId),
						new SqlParameter("@Activity", "DeleteById")
					};
					new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
					foreach (DMSAttachments att in obj.AttachmentsList)
					{
						att.RefAccountId = obj.entryUserId;
						att.RefDocumentTypeId = obj.documentTypeId;
						att.RefDocumentNo = obj.grnSupplierLoadingMasterId;
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
		return obj.grnSupplierLoadingMasterId;
	}

	public List<Architecture.Model.CommissionAgent.grnSupplierLoadingMaster> GetData(string procName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.CommissionAgent.grnSupplierLoadingMaster> list;
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				list = GenericProvider.GetProc<Architecture.Model.CommissionAgent.grnSupplierLoadingMaster>(sqlConnection, procName, p);
			}
			if (list != null && list.Count > 0)
			{
				using SqlConnection sqlCon = ConnectionObject.SQL();
				sqlCon.Open();
				foreach (Architecture.Model.CommissionAgent.grnSupplierLoadingMaster item in list)
				{
					List<SqlParameter> detailist = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.grnSupplierLoadingMasterId),
						new SqlParameter("@Activity", "ReadDetailByHeaderId")
					};
					item.grnSupplierLoadingDetailList = GenericProvider.GetAllDetail<grnSupplierLoadingDetail>(sqlCon, "[cmagt].[usp_grnSupplierLoadingMaster_GetAllMethod]", detailist);
					List<SqlParameter> ExpenseLis = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.grnSupplierLoadingMasterId),
						new SqlParameter("@Activity", "grnSupplierLoadingExpensesDetailByHeaderId")
					};
					item.grnSupplierLoadingExpenseDetailList = GenericProvider.GetAllDetail<grnSupplierLoadingExpenseDetail>(sqlCon, "[cmagt].[usp_grnSupplierLoadingMaster_GetAllMethod]", ExpenseLis);
					List<SqlParameter> EmptybagsList = new List<SqlParameter>
					{
						new SqlParameter("@Id", item.grnSupplierLoadingMasterId),
						new SqlParameter("@Activity", "grnSupplierLoadingEmptyBagDetailByHeaderId")
					};
					item.grnSupplierLoadingEmptyBagDetailList = GenericProvider.GetAllDetail<grnSupplierLoadingEmptyBagDetail>(sqlCon, "[cmagt].[usp_grnSupplierLoadingMaster_GetAllMethod]", EmptybagsList);
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
