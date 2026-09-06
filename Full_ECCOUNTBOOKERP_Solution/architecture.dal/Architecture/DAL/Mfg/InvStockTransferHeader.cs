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
using Architecture.Model.Mfg;
using Architecture.Model.VPS;

namespace Architecture.DAL.Mfg;

public class InvStockTransferHeader
{
	public int SetData(Architecture.Model.Mfg.InvStockTransferHeader obj, string ProcName)
	{
		int returnID = 0;
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		new List<GetItemGlIdsandItemName>();
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
			int LineId = 1;
			foreach (Architecture.Model.Mfg.InvStockTransferDetail item4 in obj.InvStockTransferDetailList)
			{
				item4.LineId = LineId;
				item4.InvStockTransferHeaderId = obj.Id;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[Mfg].[USP_InvStockTransferDetail_Insert]"));
				item4.Id = code;
				LineId++;
			}
			InventoryTransactions transactions = new InventoryTransactions();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = obj.Id;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
			inventoryTransactions.OrganizationId = obj.OrganizationId;
			inventoryTransactions.CompanyId = obj.CompanyId;
			inventoryTransactions.RefDocumentTypeId = obj.DocumentTypeId;
			inventoryTransactions.RefDocIdNo = obj.Id;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
			if (obj.DocumentTypeId == 1663 || obj.DocumentTypeId == 1665)
			{
				foreach (Architecture.Model.Mfg.InvStockTransferDetail item3 in obj.InvStockTransferDetailList)
				{
					SqlCommand sqlCommandInv = new SqlCommand
					{
						Connection = sqlcon,
						Transaction = sqlTrn,
						CommandType = CommandType.StoredProcedure,
						CommandText = "USP_InventoryValidation"
					};
					sqlCommandInv.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommandInv.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommandInv.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
					sqlCommandInv.Parameters.AddWithValue("@DocDate", obj.DocDate);
					sqlCommandInv.Parameters.AddWithValue("@ItemId", item3.ItemId);
					sqlCommandInv.Parameters.AddWithValue("@WarehouseId", item3.WarehouseFromId);
					if (item3.ItemVariantId > 0)
					{
						sqlCommandInv.Parameters.AddWithValue("@VarientId", item3.ItemVariantId);
					}
					if (item3.ProductionStageFromId > 0)
					{
						sqlCommandInv.Parameters.AddWithValue("@ProductionStageId", item3.ProductionStageFromId);
					}
					if (item3.WorkOrderId > 0)
					{
						sqlCommandInv.Parameters.AddWithValue("@WorkOrderId", item3.WorkOrderId);
					}
					if (item3.WorkStationFromId > 0)
					{
						sqlCommandInv.Parameters.AddWithValue("@WorkStationId", item3.WorkStationFromId);
					}
					if (!string.IsNullOrEmpty(item3.BatchDescription))
					{
						sqlCommandInv.Parameters.AddWithValue("@BatchNo", item3.BatchDescription);
					}
					sqlCommandInv.Parameters.AddWithValue("@NetWeight", Conversion.ToDouble(item3.ItemQty));
					sqlCommandInv.ExecuteNonQuery();
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

	public List<Architecture.Model.Mfg.InvStockTransferHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Mfg.InvStockTransferHeader> lst = new List<Architecture.Model.Mfg.InvStockTransferHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Mfg.InvStockTransferHeader>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Mfg.InvStockTransferHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId_InvStockTransferDetail"));
					item.InvStockTransferDetailList = GenericProvider.GetAllDetail<Architecture.Model.Mfg.InvStockTransferDetail>(sqlcon, "[Mfg].[USP_InvStockTransferHeader_GetAllMethod]", lstPrm);
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
