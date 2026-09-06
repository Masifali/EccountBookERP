using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Mfg;
using Architecture.Model.VPS;

namespace Architecture.DAL.Mfg;

public class WorkOrder
{
	public int SetData(Architecture.Model.Mfg.WorkOrder obj, string ProcName)
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
			if (obj.DocumentTypeId != 1621)
			{
				if (obj.WorkOrderOperationList == null || obj.WorkOrderOperationList.Count <= 0)
				{
					throw new Exception("WorkOrderOperation Detail list not found");
				}
				if (obj.WorkOrderMaterialList == null || obj.WorkOrderMaterialList.Count <= 0)
				{
					throw new Exception("WorkOrderMaterial Detail list not found");
				}
			}
			else if (obj.workOrderItemDetailList == null || obj.workOrderItemDetailList.Count <= 0)
			{
				throw new Exception("workOrderItemDetail Detail list not found");
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
				obj.WorkOrderId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.WorkOrderId;
			}
			foreach (WorkOrderCustomer item6 in obj.WorkOrderCustomerList)
			{
				item6.WorkOrderId = obj.WorkOrderId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "[Mfg].[USP_WorkOrderCustomer_Insert]"));
			}
			foreach (workOrderMaterial item5 in obj.WorkOrderMaterialList)
			{
				item5.WorkOrderId = obj.WorkOrderId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "[Mfg].[USP_workOrderMaterial_Insert]"));
			}
			foreach (WorkOrderOperation item4 in obj.WorkOrderOperationList)
			{
				item4.WorkOrderId = obj.WorkOrderId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[Mfg].[USP_WorkOrderOperation_Insert]"));
			}
			foreach (workOrderItemDetail item3 in obj.workOrderItemDetailList)
			{
				item3.WorkOrderId = obj.WorkOrderId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "[Mfg].[USP_workOrderItemDetail_Insert]"));
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
					item2.RefAccountId = obj.EntryUserId;
					item2.RefDocumentTypeId = obj.DocumentTypeId;
					item2.RefDocumentNo = returnID;
					item2.EntryDate = DateTime.Now;
					item2.ModifyDate = DateTime.Now;
					item2.EntryUser = obj.EntryUserId;
					item2.ModifyUser = obj.ModifyUserId;
					item2.OrganizationId = obj.OrganizationId;
					item2.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Proc_DMSAttachments_Insert"));
				}
			}
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

	public List<Architecture.Model.Mfg.WorkOrder> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Mfg.WorkOrder> lst = new List<Architecture.Model.Mfg.WorkOrder>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Mfg.WorkOrder>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Mfg.WorkOrder item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.WorkOrderId));
						lstPrm4.Add(new SqlParameter("@Activity", "ReadByHeaderId_WorkOrderOperationDetail"));
						item4.WorkOrderOperationList = GenericProvider.GetAllDetail<WorkOrderOperation>(sqlConnection2, "[Mfg].[USP_WorkOrder_GetAllMethod]", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Mfg.WorkOrder item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.WorkOrderId));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadByHeaderId_WorkOrderCustomerDetail"));
						item3.WorkOrderCustomerList = GenericProvider.GetAllDetail<WorkOrderCustomer>(sqlConnection3, "[Mfg].[USP_WorkOrder_GetAllMethod]", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Mfg.WorkOrder item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.WorkOrderId));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadByHeaderId_workOrderMaterial"));
						item2.WorkOrderMaterialList = GenericProvider.GetAllDetail<workOrderMaterial>(sqlConnection4, "[Mfg].[USP_WorkOrder_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Mfg.WorkOrder item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.WorkOrderId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId_workOrderItemDetail"));
					item.workOrderItemDetailList = GenericProvider.GetAllDetail<workOrderItemDetail>(sqlcon, "[Mfg].[USP_WorkOrder_GetAllMethod]", lstPrm);
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
