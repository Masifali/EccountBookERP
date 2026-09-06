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

public class PreBookingOrder
{
	public int SetData(Architecture.Model.Inventory.PreBookingOrder obj, string ProcName)
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
			if (obj.PreBookingOrderDetailList == null || obj.PreBookingOrderDetailList.Count == 0)
			{
				throw new Exception("PreBookingOrderDetailList not found");
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
			foreach (PreBookingOrderDetail item2 in obj.PreBookingOrderDetailList)
			{
				item2.PreBookingOrderId = obj.Id;
				if (item2.OrderItemId <= 0)
				{
					throw new Exception("ItemId not found in detail");
				}
				if (item2.OrderItemUOMId <= 0)
				{
					throw new Exception("PackUomId not found in detail");
				}
				if (item2.OrderItemRateUOMId <= 0)
				{
					throw new Exception("RateUomId not found in detail");
				}
				if (!(item2.OrderItemRate > 0.0))
				{
					throw new Exception("Rate not found in detail");
				}
				if (!(item2.Amount > 0.0))
				{
					throw new Exception("Amount not found in detail");
				}
				if (item2.ActionTypeId != 3)
				{
					LimitAmount += Conversion.ToDecimal(item2.Amount);
				}
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "USP_PreBookingOrderDetail_Insert"));
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
					item.EntryUser = clsGlobalVariables.UserAccount.ID;
					item.ModifyUser = clsGlobalVariables.UserAccount.ID;
					item.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
					item.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Proc_DMSAttachments_Insert"));
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

	public int SetDataDetail(Architecture.Model.Inventory.PreBookingOrder obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (PreBookingOrderDetail item in obj.PreBookingOrderDetailList)
			{
				item.PreBookingOrderId = obj.Id;
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

	public List<Architecture.Model.Inventory.PreBookingOrder> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.PreBookingOrder> lst = new List<Architecture.Model.Inventory.PreBookingOrder>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Inventory.PreBookingOrder>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Inventory.PreBookingOrder item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByPreBookingOrderId"));
					item.PreBookingOrderDetailList = GenericProvider.GetAllDetail<PreBookingOrderDetail>(sqlcon, "USP_PreBookingOrder_GetAllMethod", lstPrm);
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
