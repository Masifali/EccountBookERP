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

public class InvDeliveryOrder
{
	public int SetData(Architecture.Model.Inventory.InvDeliveryOrder obj, string ProcName)
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
			if (obj.InvDeliveryOrderDetailList == null || obj.InvDeliveryOrderDetailList.Count == 0)
			{
				throw new Exception("Detail List not found");
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
			foreach (InvDeliveryOrderdetail item4 in obj.InvDeliveryOrderDetailList)
			{
				item4.InvDeliveryOrderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Sp_InvDeliveryOrderDetail_Insert"));
			}
			foreach (InvDeliveryOrderExpense item3 in obj.InvDeliveryOrderExpenseslist)
			{
				item3.InvDeliveryOrderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Sp_InvDeliveryOrderExpense_Insert"));
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
					item2.RefAccountId = returnID;
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
			AP.LimitAmount = 0m;
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

	public List<Architecture.Model.Inventory.InvDeliveryOrder> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.InvDeliveryOrder> lst = new List<Architecture.Model.Inventory.InvDeliveryOrder>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetAllDetail<Architecture.Model.Inventory.InvDeliveryOrder>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.InvDeliveryOrder item2 in lst)
					{
						if (item2.DocumentTypeId == 84 && item2.DeliveryOrderType == "Export")
						{
							List<SqlParameter> lstPrm7 = new List<SqlParameter>();
							lstPrm7.Add(new SqlParameter("@Id", item2.Id));
							lstPrm7.Add(new SqlParameter("@Activity", "ReadByIdDetailIdExport"));
							item2.InvDeliveryOrderDetailList = GenericProvider.GetAllDetail<InvDeliveryOrderdetail>(sqlConnection2, "Sp_InvDeliveryOrder_GetAllMethod", lstPrm7);
						}
						if ((item2.DocumentTypeId == 84 || item2.DocumentTypeId == 910) && item2.DeliveryOrderType != "Export")
						{
							List<SqlParameter> lstPrm6 = new List<SqlParameter>();
							lstPrm6.Add(new SqlParameter("@Id", item2.Id));
							lstPrm6.Add(new SqlParameter("@Activity", "ReadByIdDetailId"));
							item2.InvDeliveryOrderDetailList = GenericProvider.GetAllDetail<InvDeliveryOrderdetail>(sqlConnection2, "Sp_InvDeliveryOrder_GetAllMethod", lstPrm6);
						}
						if (item2.DocumentTypeId == 85)
						{
							List<SqlParameter> lstPrm5 = new List<SqlParameter>();
							lstPrm5.Add(new SqlParameter("@Id", item2.Id));
							lstPrm5.Add(new SqlParameter("@Activity", "ReadByIdPackingMaterialDetail"));
							item2.InvDeliveryOrderDetailList = GenericProvider.GetAllDetail<InvDeliveryOrderdetail>(sqlConnection2, "Sp_InvDeliveryOrder_GetAllMethod", lstPrm5);
						}
						if (item2.DocumentTypeId == 1606 || item2.DocumentTypeId == 1657)
						{
							List<SqlParameter> lstPrm4 = new List<SqlParameter>();
							lstPrm4.Add(new SqlParameter("@Id", item2.Id));
							lstPrm4.Add(new SqlParameter("@Activity", "ReadByIdDetailIdForEngr"));
							item2.InvDeliveryOrderDetailList = GenericProvider.GetAllDetail<InvDeliveryOrderdetail>(sqlConnection2, "Sp_InvDeliveryOrder_GetAllMethod", lstPrm4);
						}
						if (item2.DocumentTypeId == 1200)
						{
							List<SqlParameter> lstPrm3 = new List<SqlParameter>();
							lstPrm3.Add(new SqlParameter("@Id", item2.Id));
							lstPrm3.Add(new SqlParameter("@Activity", "ReadByIdDetailIdForReturnable"));
							item2.InvDeliveryOrderDetailList = GenericProvider.GetAllDetail<InvDeliveryOrderdetail>(sqlConnection2, "Sp_InvDeliveryOrder_GetAllMethod", lstPrm3);
						}
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Inventory.InvDeliveryOrder item in lst)
				{
					if (item.DocumentTypeId == 84 && item.DeliveryOrderType == "Export")
					{
						List<SqlParameter> lstPrm = new List<SqlParameter>();
						lstPrm.Add(new SqlParameter("@Id", item.Id));
						lstPrm.Add(new SqlParameter("@Activity", "ReadInvDeliveryOrderExpensesByHeaderIdForExport"));
						item.InvDeliveryOrderExpenseslist = GenericProvider.GetAllDetail<InvDeliveryOrderExpense>(sqlcon, "Sp_InvDeliveryOrder_GetAllMethod", lstPrm);
					}
					else if (item.DocumentTypeId == 84 && item.DeliveryOrderType != "Export")
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadInvDeliveryOrderExpensesByHeaderId"));
						item.InvDeliveryOrderExpenseslist = GenericProvider.GetAllDetail<InvDeliveryOrderExpense>(sqlcon, "Sp_InvDeliveryOrder_GetAllMethod", lstPrm2);
					}
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
