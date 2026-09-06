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

public class InvStockTransferPartyProcessing
{
	public int SetData(InvStockTransferHeaderPartyProcessing obj, string ProcName)
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
			if (obj.InvStockTransferDetailPartyProcessingList.Count <= 0 || obj.InvStockTransferDetailPartyProcessingList == null)
			{
				throw new Exception("InvStockTransferDetailPartyProcessingList not found");
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
			foreach (InvStockTransferDetailPartyProcessing item3 in obj.InvStockTransferDetailPartyProcessingList)
			{
				if (item3.RefDocumentTypeId > 0 && item3.RefDocNoId > 0 && item3.RefDocSubIdNo > 0)
				{
					loaderflag = true;
				}
				item3.InvStockTransferHeaderId = obj.Id;
				item3.LineId = LineId;
				int CodeDetail = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Sp_InvStockTransferDetailPartyProcessing_Insert"));
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
					item2.RefAccountId = obj.WbTicketId;
					item2.RefDocumentTypeId = obj.DocumentTypeId;
					item2.RefDocumentNo = returnID;
					item2.OrganizationId = obj.OrganizationId;
					item2.CompanyId = obj.CompanyId;
					item2.ModifyDate = DateTime.Now;
					item2.ModifyUser = obj.ModifyUser;
					item2.EntryUser = obj.EntryUser;
					item2.EntryDate = DateTime.Now;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Proc_DMSAttachments_Insert"));
				}
			}
			if (!loaderflag)
			{
				List<GetItemGlIdsandItemName> dtitem = (dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId));
				obj.InventoryTransactionsPartyProcessingForFIFOlist = new List<InventoryTransactionsPartyProcessingForFifo>();
				foreach (InvStockTransferDetailPartyProcessing item5 in obj.InvStockTransferDetailPartyProcessingList.ToList())
				{
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item5.ItemId)).ToList();
					if (find.Count <= 0)
					{
						continue;
					}
					ReportsParameters stock = new ReportsParameters();
					stock.OrganizationId = obj.OrganizationId;
					stock.CompanyId = obj.CompanyId;
					stock.ItemId = item5.ItemId;
					stock.StockPartyId = item5.FromStockPartyId;
					stock.WarehouseId = item5.FromWarehouseId;
					stock.DocDate = obj.DocDate;
					stock.stockUOM = item5.PackUomId;
					stock.JobLotId = item5.JobLotId;
					stock.CropYearId = item5.CropYearId;
					stock.PackingTypeId = item5.InvPackingTypeId;
					if (obj.ModifyUser > 0)
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
						stockdetail.TranRemarks = "Stock Transfer Party Processing";
						stockdetail.DetailRemarks = item5.RemarksDetail;
						stockdetail.GrossWeight = item5.GrossWeight;
						stockdetail.EbUnit = item5.EbUnit;
						stockdetail.EbTotal = item5.EbTotal;
						stockdetail.AddLess = item5.AdLsWeight;
						stockdetail.StockPartyId = item5.FromStockPartyId;
						stockdetail.RefWarehouseId = item5.ToWarehouseId;
						InventoryTransactionsPartyProcessingForFifo Intrans = new InventoryTransactionsPartyProcessingForFifo();
						Intrans.LineId = stockdetail.LineId;
						Intrans.ItemId = stockdetail.ItemId;
						Intrans.WarehouseId = item5.ToWarehouseId;
						Intrans.JobLotId = item5.JobLotIdTo;
						Intrans.CropYearId = stockdetail.CropYearId;
						Intrans.InvPackingTypeId = stockdetail.InvPackingTypeId;
						Intrans.ItemUom = stockdetail.ItemUom;
						Intrans.RefWarehouseId = item5.FromWarehouseId;
						Intrans.StockPartyId = item5.ToStockPartyId;
						Intrans.GrossWeight = item5.GrossWeight;
						Intrans.EbUnit = item5.EbUnit;
						Intrans.EbTotal = item5.EbTotal;
						Intrans.AddLess = item5.AdLsWeight;
						Intrans.QtyIn = stockdetail.QtyOut;
						Intrans.BillWeightIn = stockdetail.BillWeightOut;
						Intrans.StockWeightIn = stockdetail.StockWeightOut;
						Intrans.TranRemarks = stockdetail.TranRemarks;
						Intrans.RefRefDocumentTypeId = 0;
						Intrans.RefRefDocIdNo = 0;
						Intrans.RefRefDocSubIdNo = 0;
						obj.InventoryTransactionsPartyProcessingForFIFOlist.Add(Intrans);
						obj.InventoryTransactionsPartyProcessingForFIFOlist.Add(stockdetail);
					}
				}
				if (obj.InventoryTransactionsPartyProcessingForFIFOlist != null && obj.InventoryTransactionsPartyProcessingForFIFOlist.Count > 0)
				{
					if (obj.ModifyUser > 0)
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
						item4.BranchesId = obj.BranchesId;
						item4.RefDocumentTypeId = obj.DocumentTypeId;
						item4.CalcType = "Weight";
						item4.EntryUserId = obj.EntryUser;
						item4.ModifyUserId = obj.ModifyUser;
						item4.EntryDate = DateTime.Now;
						item4.ModifyDate = DateTime.Now;
						InvStockTransferDetailPartyProcessing sid = obj.InvStockTransferDetailPartyProcessingList.Find((InvStockTransferDetailPartyProcessing x) => x.LineId == item4.LineId && x.LineId > 0 && item4.LineId > 0);
						if (sid != null)
						{
							item4.RefDocIdNo = sid.InvStockTransferHeaderId;
							item4.RefDocSubIdNo = sid.Id;
						}
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "USP_InventoryTransactionsPartyProcessing_Insert"));
					}
				}
			}
			else
			{
				InventoryTransactionsPartyProcessing transactions = new InventoryTransactionsPartyProcessing();
				transactions.OrganizationId = obj.OrganizationId;
				transactions.CompanyId = obj.CompanyId;
				transactions.RefDocumentTypeId = obj.DocumentTypeId;
				transactions.RefDocIdNo = returnID;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactionsPartyProcessing_Insert"));
			}
			foreach (InvStockTransferDetailPartyProcessing item in obj.InvStockTransferDetailPartyProcessingList)
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
				sqlCommand2.Parameters.AddWithValue("@StockPartyId", item.FromStockPartyId);
				sqlCommand2.Parameters.AddWithValue("@ItemId", item.ItemId);
				sqlCommand2.Parameters.AddWithValue("@WarehouseId", item.FromWarehouseId);
				sqlCommand2.Parameters.AddWithValue("@JobLotId", item.JobLotId);
				sqlCommand2.Parameters.AddWithValue("@CropYearId", item.CropYearId);
				sqlCommand2.Parameters.AddWithValue("@InvPackingTypeId", item.InvPackingTypeId);
				sqlCommand2.Parameters.AddWithValue("@PackUomId", item.PackUomId);
				sqlCommand2.Parameters.AddWithValue("@NetWeight", item.NetWeight);
				sqlCommand2.Parameters.AddWithValue("@RefDocumentTypeId", item.RefDocumentTypeId);
				sqlCommand2.Parameters.AddWithValue("@RefDocNoId", item.RefDocNoId);
				sqlCommand2.Parameters.AddWithValue("@RefDocSubIdNo", item.RefDocSubIdNo);
				sqlCommand2.ExecuteNonQuery();
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

	public List<InvStockTransferHeaderPartyProcessing> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<InvStockTransferHeaderPartyProcessing> lst = new List<InvStockTransferHeaderPartyProcessing>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<InvStockTransferHeaderPartyProcessing>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (InvStockTransferHeaderPartyProcessing item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@HeaderId", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.InvStockTransferDetailPartyProcessingList = GenericProvider.GetAllDetail<InvStockTransferDetailPartyProcessing>(sqlcon, "Sp_InvStockTransferHeaderPartyProcessing_GetAllMethod", lstPrm);
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
