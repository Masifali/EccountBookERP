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
using Architecture.Model.PartyProcessing;
using Architecture.Model.VPS;

namespace Architecture.DAL.PartyProcessing;

public class InvStockAdjustmentPartyProcessing
{
	public int SetData(Architecture.Model.PartyProcessing.InvStockAdjustmentPartyProcessing obj, string ProcName)
	{
		int returnID = 0;
		string AttachmentFolderPath = "";
		List<string> copiedFiles = new List<string>();
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.InvStockAdjustmentDetailPartyProcessingList.Count <= 0 || obj.InvStockAdjustmentDetailPartyProcessingList == null)
			{
				throw new Exception("InvStockAdjustmentDetailPartyProcessingList not found");
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
			int LineId = 1;
			bool loaderflag = false;
			foreach (InvStockAdjustmentDetailPartyProcessing item3 in obj.InvStockAdjustmentDetailPartyProcessingList)
			{
				if (obj.AdjustmentTypeId == 2 && item3.RefDocumentTypeId > 0 && item3.RefDocNoId > 0 && item3.RefDocSubIdNo > 0)
				{
					loaderflag = true;
				}
				item3.InvStockAdjustmentId = obj.Id;
				item3.LineId = LineId;
				int CodeDetail = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "usp_InvStockAdjustmentDetailPartyProcessing_Insert"));
				item3.Id = CodeDetail;
				LineId++;
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
					item2.RefAccountId = obj.AdjustmentTypeId;
					item2.RefDocumentTypeId = obj.DocumentTypeId;
					item2.RefDocumentNo = returnID;
					item2.OrganizationId = obj.OrganizationId;
					item2.CompanyId = obj.CompanyId;
					item2.ModifyDate = DateTime.Now;
					item2.ModifyUser = obj.ModifyUserId;
					item2.EntryUser = obj.EntryUserId;
					item2.EntryDate = DateTime.Now;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Proc_DMSAttachments_Insert"));
				}
			}
			if (obj.AdjustmentTypeId == 2 && !loaderflag)
			{
				List<GetItemGlIdsandItemName> dtitem = (dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId));
				obj.InventoryTransactionsPartyProcessingForFIFOlist = new List<InventoryTransactionsPartyProcessingForFifo>();
				foreach (InvStockAdjustmentDetailPartyProcessing item5 in obj.InvStockAdjustmentDetailPartyProcessingList.ToList())
				{
					if (item5.ActionTypeId == 3)
					{
						continue;
					}
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item5.ItemId)).ToList();
					if (find.Count <= 0)
					{
						continue;
					}
					ReportsParameters stock = new ReportsParameters();
					stock.OrganizationId = obj.OrganizationId;
					stock.CompanyId = obj.CompanyId;
					stock.ItemId = item5.ItemId;
					stock.StockPartyId = obj.StockPartyId;
					stock.WarehouseId = item5.WarehouseId;
					stock.DocDate = obj.DocDate;
					stock.stockUOM = item5.PackUomId;
					stock.JobLotId = item5.JobLotId;
					stock.CropYearId = item5.CropYearId;
					stock.PackingTypeId = item5.PackingTypeId;
					if (obj.ModifyUserId > 0)
					{
						stock.DocumentTypeId = obj.DocumentTypeId;
						stock.Id = obj.Id;
					}
					stock.ItemQty = item5.Qty;
					stock.NetWeight = item5.NetWeight;
					stock.LineId = item5.LineId;
					stock.ItemName = Conversion.ToString(find[0].ItemName);
					foreach (InventoryTransactionsPartyProcessingForFifo stockdetail in CommonServices.FIFOImplementionForPartyProcessing(stock, obj.InventoryTransactionsPartyProcessingForFIFOlist).InventoryTransactionsPartyProcessingForFIFOlist.ToList())
					{
						stockdetail.TranRemarks = ((obj.AdjustmentTypeId == 1) ? "Gain Stock Adjustment Party Processing" : "Loss Stock Adjustment Party Processing");
						stockdetail.DetailRemarks = item5.RemarksDetail;
						obj.InventoryTransactionsPartyProcessingForFIFOlist.Add(stockdetail);
					}
				}
				if (obj.InventoryTransactionsPartyProcessingForFIFOlist != null && obj.InventoryTransactionsPartyProcessingForFIFOlist.Count > 0)
				{
					if (obj.ModifyUserId > 0)
					{
						SqlCommand sqlCommand = new SqlCommand();
						sqlCommand.Connection = sqlcon;
						sqlCommand.Transaction = sqlTrn;
						sqlCommand.CommandType = CommandType.StoredProcedure;
						sqlCommand.CommandText = "[dbo].[USP_InventoryPartyProcessingQtyReverseAndDeleteByReferenceId]";
						sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
						sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
						sqlCommand.Parameters.AddWithValue("@RefDocumentTypeId", obj.DocumentTypeId);
						sqlCommand.Parameters.AddWithValue("@RefDocIdNo", obj.Id);
						sqlCommand.ExecuteNonQuery();
					}
					foreach (InventoryTransactionsPartyProcessingForFifo item4 in obj.InventoryTransactionsPartyProcessingForFIFOlist)
					{
						item4.OrganizationId = obj.OrganizationId;
						item4.CompanyId = obj.CompanyId;
						item4.DocDate = obj.DocDate;
						item4.DocCodeNo = obj.DocNo;
						item4.StockPartyId = obj.StockPartyId;
						item4.BranchesId = obj.BranchesId;
						item4.RefDocumentTypeId = obj.DocumentTypeId;
						item4.CalcType = "Weight";
						item4.EntryUserId = obj.EntryUserId;
						item4.ModifyUserId = obj.ModifyUserId;
						item4.EntryDate = DateTime.Now;
						item4.ModifyDate = DateTime.Now;
						InvStockAdjustmentDetailPartyProcessing sid = obj.InvStockAdjustmentDetailPartyProcessingList.Find((InvStockAdjustmentDetailPartyProcessing x) => x.LineId == item4.LineId && x.LineId > 0 && item4.LineId > 0);
						if (sid != null)
						{
							item4.RefDocIdNo = sid.InvStockAdjustmentId;
							item4.RefDocSubIdNo = sid.Id;
						}
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "USP_InventoryTransactionsPartyProcessing_Insert"));
					}
				}
			}
			if (obj.AdjustmentTypeId == 2 && loaderflag)
			{
				InventoryTransactionsPartyProcessing transactions = new InventoryTransactionsPartyProcessing();
				transactions.OrganizationId = obj.OrganizationId;
				transactions.CompanyId = obj.CompanyId;
				transactions.RefDocumentTypeId = obj.DocumentTypeId;
				transactions.RefDocIdNo = returnID;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactionsPartyProcessing_Insert"));
			}
			else if (obj.AdjustmentTypeId == 1)
			{
				InventoryTransactionsPartyProcessing transactions2 = new InventoryTransactionsPartyProcessing();
				transactions2.OrganizationId = obj.OrganizationId;
				transactions2.CompanyId = obj.CompanyId;
				transactions2.RefDocumentTypeId = obj.DocumentTypeId;
				transactions2.RefDocIdNo = returnID;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions2, "Sp_InventoryTransactionsPartyProcessing_Insert"));
			}
			if (obj.AdjustmentTypeId == 2)
			{
				foreach (InvStockAdjustmentDetailPartyProcessing item in obj.InvStockAdjustmentDetailPartyProcessingList)
				{
					if (item.ActionTypeId != 3)
					{
						SqlCommand sqlCommand2 = new SqlCommand();
						sqlCommand2.Connection = sqlcon;
						sqlCommand2.Transaction = sqlTrn;
						sqlCommand2.CommandType = CommandType.StoredProcedure;
						sqlCommand2.CommandText = "USP_InventoryValidationPartyProcessing";
						sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
						sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
						sqlCommand2.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
						sqlCommand2.Parameters.AddWithValue("@DocDate", obj.DocDate);
						sqlCommand2.Parameters.AddWithValue("@StockPartyId", obj.StockPartyId);
						sqlCommand2.Parameters.AddWithValue("@ItemId", item.ItemId);
						sqlCommand2.Parameters.AddWithValue("@WarehouseId", item.WarehouseId);
						sqlCommand2.Parameters.AddWithValue("@JobLotId", item.JobLotId);
						sqlCommand2.Parameters.AddWithValue("@CropYearId", item.CropYearId);
						sqlCommand2.Parameters.AddWithValue("@InvPackingTypeId", item.PackingTypeId);
						sqlCommand2.Parameters.AddWithValue("@PackUomId", item.PackUomId);
						sqlCommand2.Parameters.AddWithValue("@NetWeight", item.NetWeight);
						sqlCommand2.Parameters.AddWithValue("@RefDocumentTypeId", item.RefDocumentTypeId);
						sqlCommand2.Parameters.AddWithValue("@RefDocNoId", item.RefDocNoId);
						sqlCommand2.Parameters.AddWithValue("@RefDocSubIdNo", item.RefDocSubIdNo);
						sqlCommand2.ExecuteNonQuery();
					}
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

	public List<Architecture.Model.PartyProcessing.InvStockAdjustmentPartyProcessing> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.PartyProcessing.InvStockAdjustmentPartyProcessing> lst = new List<Architecture.Model.PartyProcessing.InvStockAdjustmentPartyProcessing>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.PartyProcessing.InvStockAdjustmentPartyProcessing>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.PartyProcessing.InvStockAdjustmentPartyProcessing item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.InvStockAdjustmentDetailPartyProcessingList = GenericProvider.GetAllDetail<InvStockAdjustmentDetailPartyProcessing>(sqlcon, "usp_InvStockAdjustmentPartyProcessing_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public void DeleteById(int OrganizationId, int CompanyId, int DocumentTypeId, int Id, int EntryUserId)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "usp_InvStockAdjustmentPartyProcessing_GetAllMethod";
			sqlCommand.Parameters.AddWithValue("@OrganizationId", OrganizationId);
			sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
			sqlCommand.Parameters.AddWithValue("@DocumentTypeId", DocumentTypeId);
			sqlCommand.Parameters.AddWithValue("@Id", Id);
			sqlCommand.Parameters.AddWithValue("@EntryUser", EntryUserId);
			sqlCommand.Parameters.AddWithValue("@Activity", "DeleteById");
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}
}
