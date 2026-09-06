using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.PartyProcessing;
using Architecture.Model.VPS;

namespace Architecture.DAL.Inventory;

public class InvGrn : DALBase
{
	public int InsertInventoryTransaction(int OrganizationId, int CompanyId, int RefDocIdNo, int RefDocumentTypeId)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Sp_InventoryTransactions_GetALLMethod");
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, CompanyId);
			sqlDataBase.AddInParameter(cmd, "RefDocIdNo", SqlDbType.Int, RefDocIdNo);
			sqlDataBase.AddInParameter(cmd, "RefDocumentTypeId", SqlDbType.Int, RefDocumentTypeId);
			return Conversion.ToInt(sqlDataBase.ExecuteNonQuery(cmd));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public int InsertInventoryTransactionEvaluation(int OrganizationId, int CompanyId, int RefDocIdNo, int RefDocumentTypeId)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Sp_InventoryStockEvalautionDetail_Update");
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, CompanyId);
			sqlDataBase.AddInParameter(cmd, "hId", SqlDbType.Int, RefDocIdNo);
			sqlDataBase.AddInParameter(cmd, "RefDocumentTypeId", SqlDbType.Int, RefDocumentTypeId);
			return Conversion.ToInt(sqlDataBase.ExecuteNonQuery(cmd));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public int SetData(Architecture.Model.Inventory.InvGrn obj, string ProcName)
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
			if (obj.invGrnDetail == null || obj.invGrnDetail.Count <= 0)
			{
				throw new Exception("Detail list not found");
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
			int LineId = 1;
			bool loaderflag = false;
			foreach (InvGrnDetail item6 in obj.invGrnDetail)
			{
				if (obj.DocumentTypeId == 217 && item6.RefDocumentTypeId > 0 && item6.RefDocNoId > 0 && item6.RefDocSubIdNo > 0)
				{
					loaderflag = true;
				}
				item6.LineId = LineId;
				item6.InvGrnId = obj.Id;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "Sp_InvGrnDetail_Insert"));
				item6.Id = code;
				LineId++;
			}
			if (obj.InvgrnDetailEmptyBagslist != null && obj.InvgrnDetailEmptyBagslist.Count > 0)
			{
				foreach (InvgrnDetailEmptyBags item5 in obj.InvgrnDetailEmptyBagslist)
				{
					item5.InvGrnId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "Sp_InvGrnDetailEmptyBags_Insert"));
				}
			}
			if (obj.InvGrnPurchaseBreakUplist != null && obj.InvGrnPurchaseBreakUplist.Count > 0)
			{
				foreach (InvGrnPurchaseBreakUp item4 in obj.InvGrnPurchaseBreakUplist)
				{
					item4.InvGrnId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[dbo].[USP_InvGrnPurchaseBreakUp_Insert]"));
				}
			}
			if (obj.DocumentTypeId == 46)
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "USP_GrnBillWeightAndStockWeightValidations";
				sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand.Parameters.AddWithValue("@Id", obj.Id);
				sqlCommand.ExecuteNonQuery();
				SqlCommand sqlCommand2 = new SqlCommand();
				sqlCommand2.Connection = sqlcon;
				sqlCommand2.Transaction = sqlTrn;
				sqlCommand2.CommandType = CommandType.StoredProcedure;
				sqlCommand2.CommandText = "USP_Validation_MarketGRNGrossWeightCompareToScaleWeight";
				sqlCommand2.Parameters.AddWithValue("@GrnId", obj.Id);
				sqlCommand2.Parameters.AddWithValue("@GpId", obj.InwardGatePassId);
				sqlCommand2.ExecuteNonQuery();
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
					Value = obj.Id
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "DeleteById"
				});
				new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
				foreach (DMSAttachments item3 in obj.AttachmentsList.ToList())
				{
					item3.RefAccountId = obj.SupplierCustomerId;
					item3.RefDocumentTypeId = obj.DocumentTypeId;
					item3.RefDocumentNo = obj.Id;
					item3.OrganizationId = obj.OrganizationId;
					item3.CompanyId = obj.CompanyId;
					item3.EntryUser = obj.EntryUser;
					item3.ModifyUser = obj.EntryUser;
					item3.EntryDate = obj.EntryDate;
					item3.ModifyDate = obj.ModifyDate;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Proc_DMSAttachments_Insert"));
				}
			}
			InventoryTransactions transactions = new InventoryTransactions();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = returnID;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			if (obj.DocumentTypeId == 48 && obj.InwardGatePassId > 0)
			{
				InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
				inventoryTransactions.OrganizationId = obj.OrganizationId;
				inventoryTransactions.CompanyId = obj.CompanyId;
				inventoryTransactions.RefDocumentTypeId = obj.DocumentTypeId;
				inventoryTransactions.RefDocIdNo = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
			}
			if (obj.DocumentTypeId == 217)
			{
				if (!loaderflag)
				{
					List<Architecture.Model.Inventory.GetItemGlIdsandItemName> dtitem = (dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId));
					List<Architecture.Model.Inventory.InvCropYear> lstcrop = CommonServices.getCropYearlist(obj.OrganizationId, obj.CompanyId);
					obj.InventoryTransactionsPartyProcessingForFIFOlist = new List<InventoryTransactionsPartyProcessingForFifo>();
					foreach (InvGrnDetail item8 in obj.invGrnDetail.ToList())
					{
						List<Architecture.Model.Inventory.GetItemGlIdsandItemName> find = dtitem.Where((Architecture.Model.Inventory.GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item8.ItemId)).ToList();
						if (find.Count <= 0)
						{
							continue;
						}
						ReportsParameters stock = new ReportsParameters();
						stock.OrganizationId = obj.OrganizationId;
						stock.CompanyId = obj.CompanyId;
						stock.ItemId = item8.ItemId;
						stock.StockPartyId = obj.SupplierCustomerId;
						stock.WarehouseId = item8.WareHouseFromId;
						stock.DocDate = obj.DocDate;
						stock.stockUOM = item8.ItemUomId;
						stock.JobLotId = item8.JobLotId;
						List<Architecture.Model.Inventory.InvCropYear> findCrop = lstcrop.Where((Architecture.Model.Inventory.InvCropYear x) => x.CropYear == Conversion.ToString(item8.CropYear)).ToList();
						if (findCrop.Count > 0)
						{
							stock.CropYearId = Conversion.ToInt(findCrop[0].Id);
						}
						stock.PackingTypeId = item8.PackingTypeId;
						if (obj.ModifyUser > 0)
						{
							stock.DocumentTypeId = obj.DocumentTypeId;
							stock.Id = obj.Id;
						}
						stock.ItemQty = item8.ItemQty;
						stock.NetWeight = item8.StockWeight;
						stock.LineId = item8.LineId;
						stock.ItemName = Conversion.ToString(find[0].ItemName);
						foreach (InventoryTransactionsPartyProcessingForFifo stockdetail in CommonServices.FIFOImplementionForPartyProcessing(stock, obj.InventoryTransactionsPartyProcessingForFIFOlist).InventoryTransactionsPartyProcessingForFIFOlist.ToList())
						{
							stockdetail.RefWarehouseId = item8.WarehouseId;
							stockdetail.CityId = item8.CityId;
							stockdetail.GpNo = obj.GpNo;
							stockdetail.VehicleNo = obj.VehicleNo;
							stockdetail.TranRemarks = "Grn For Purchase Party Processing";
							stockdetail.GrossWeight = item8.GrossWeight;
							stockdetail.EbUnit = item8.EBWPerUnit;
							stockdetail.EbTotal = item8.EBWTotal;
							stockdetail.AddLess = item8.AdLsWeight;
							stockdetail.TransporterId = obj.TransporterId;
							stockdetail.FreightAmount = obj.CarriageAmount;
							stockdetail.DetailRemarks = item8.CommentsDetail;
							obj.InventoryTransactionsPartyProcessingForFIFOlist.Add(stockdetail);
						}
					}
					if (obj.InventoryTransactionsPartyProcessingForFIFOlist != null && obj.InventoryTransactionsPartyProcessingForFIFOlist.Count > 0)
					{
						if (obj.ModifyUser > 0)
						{
							SqlCommand sqlCommand3 = new SqlCommand();
							sqlCommand3.Connection = sqlcon;
							sqlCommand3.Transaction = sqlTrn;
							sqlCommand3.CommandType = CommandType.StoredProcedure;
							sqlCommand3.CommandText = "[dbo].[USP_InventoryPartyProcessingQtyReverseAndDeleteByReferenceId]";
							sqlCommand3.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
							sqlCommand3.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
							sqlCommand3.Parameters.AddWithValue("@RefDocumentTypeId", obj.DocumentTypeId);
							sqlCommand3.Parameters.AddWithValue("@RefDocIdNo", obj.Id);
							sqlCommand3.ExecuteNonQuery();
						}
						foreach (InventoryTransactionsPartyProcessingForFifo item7 in obj.InventoryTransactionsPartyProcessingForFIFOlist)
						{
							item7.OrganizationId = obj.OrganizationId;
							item7.CompanyId = obj.CompanyId;
							item7.DocDate = obj.DocDate;
							item7.DocCodeNo = obj.DocNo;
							item7.StockPartyId = obj.SupplierCustomerId;
							item7.BranchesId = obj.BranchesId;
							item7.RefDocumentTypeId = obj.DocumentTypeId;
							item7.CalcType = "Weight";
							item7.EntryUserId = obj.EntryUser;
							item7.ModifyUserId = obj.ModifyUser;
							item7.EntryDate = DateTime.Now;
							item7.ModifyDate = DateTime.Now;
							InvGrnDetail sid = obj.invGrnDetail.Find((InvGrnDetail x) => x.LineId == item7.LineId && x.LineId > 0 && item7.LineId > 0);
							if (sid != null)
							{
								item7.RefDocIdNo = sid.InvGrnId;
								item7.RefDocSubIdNo = sid.Id;
							}
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "USP_InventoryTransactionsPartyProcessing_Insert"));
						}
					}
				}
				else
				{
					InventoryTransactionsPartyProcessing objPP = new InventoryTransactionsPartyProcessing();
					objPP.OrganizationId = obj.OrganizationId;
					objPP.CompanyId = obj.CompanyId;
					objPP.RefDocumentTypeId = obj.DocumentTypeId;
					objPP.RefDocIdNo = returnID;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, objPP, "Sp_InventoryTransactionsPartyProcessing_Insert"));
				}
				foreach (InvGrnDetail item2 in obj.invGrnDetail)
				{
					SqlCommand sqlCommand4 = new SqlCommand();
					sqlCommand4.Connection = sqlcon;
					sqlCommand4.Transaction = sqlTrn;
					sqlCommand4.CommandType = CommandType.StoredProcedure;
					sqlCommand4.CommandText = "USP_InventoryValidationPartyProcessing";
					sqlCommand4.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand4.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand4.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
					sqlCommand4.Parameters.AddWithValue("@DocDate", obj.DocDate);
					sqlCommand4.Parameters.AddWithValue("@StockPartyId", obj.SupplierCustomerId);
					sqlCommand4.Parameters.AddWithValue("@ItemId", item2.ItemId);
					sqlCommand4.Parameters.AddWithValue("@WarehouseId", item2.WareHouseFromId);
					sqlCommand4.Parameters.AddWithValue("@JobLotId", item2.JobLotId);
					sqlCommand4.Parameters.AddWithValue("@CropYear", item2.CropYear);
					sqlCommand4.Parameters.AddWithValue("@InvPackingTypeId", item2.PackingTypeId);
					sqlCommand4.Parameters.AddWithValue("@PackUomId", item2.ItemUomId);
					sqlCommand4.Parameters.AddWithValue("@NetWeight", item2.NetBillWeight);
					sqlCommand4.ExecuteNonQuery();
				}
			}
			else
			{
				SqlCommand sqlCommand5 = new SqlCommand();
				sqlCommand5.Connection = sqlcon;
				sqlCommand5.Transaction = sqlTrn;
				sqlCommand5.CommandType = CommandType.StoredProcedure;
				sqlCommand5.CommandText = "usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn";
				sqlCommand5.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand5.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand5.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
				sqlCommand5.Parameters.AddWithValue("@GrnId", obj.Id);
				sqlCommand5.ExecuteNonQuery();
			}
			if (obj.DocumentTypeId == 46)
			{
				SqlCommand sqlCommand6 = new SqlCommand();
				sqlCommand6.Connection = sqlcon;
				sqlCommand6.Transaction = sqlTrn;
				sqlCommand6.CommandType = CommandType.StoredProcedure;
				sqlCommand6.CommandText = "USP_InvGrn_GenerateSpecialApprovalLog";
				sqlCommand6.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand6.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand6.Parameters.AddWithValue("@GrnDocumentTypeId", obj.DocumentTypeId);
				sqlCommand6.Parameters.AddWithValue("@GrnDocId", obj.Id);
				sqlCommand6.Parameters.AddWithValue("@UserId", (obj.EntryUser > 0) ? obj.EntryUser : obj.ModifyUser);
				sqlCommand6.ExecuteNonQuery();
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

	public List<Architecture.Model.Inventory.InvGrn> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.InvGrn> lst = new List<Architecture.Model.Inventory.InvGrn>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetAllDetail<Architecture.Model.Inventory.InvGrn>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.InvGrn item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						if (item3.DocumentTypeId == 46 || item3.DocumentTypeId == 165 || item3.DocumentTypeId == 167 || item3.DocumentTypeId == 169 || item3.DocumentTypeId == 143)
						{
							lstPrm3.Add(new SqlParameter("@Activity", "ReadByInvGrnID"));
							item3.invGrnDetail = GenericProvider.GetAllDetail<InvGrnDetail>(sqlConnection2, "Sp_InvGrnDetail_GetAllMethod", lstPrm3);
						}
						if (item3.DocumentTypeId == 47)
						{
							lstPrm3.Add(new SqlParameter("@Activity", "ReadByInvGrnIDTrading"));
							item3.invGrnDetail = GenericProvider.GetAllDetail<InvGrnDetail>(sqlConnection2, "Sp_InvGrnDetail_GetAllMethod", lstPrm3);
						}
						if (item3.DocumentTypeId == 48 || item3.DocumentTypeId == 701)
						{
							lstPrm3.Add(new SqlParameter("@Activity", "ReadByInvGrnIDStore"));
							item3.invGrnDetail = GenericProvider.GetAllDetail<InvGrnDetail>(sqlConnection2, "Sp_InvGrnDetail_GetAllMethod", lstPrm3);
						}
						if (item3.DocumentTypeId == 137 || item3.DocumentTypeId == 217)
						{
							lstPrm3.Add(new SqlParameter("@Activity", "ReadByInvGrnIdDirect"));
							item3.invGrnDetail = GenericProvider.GetAllDetail<InvGrnDetail>(sqlConnection2, "Sp_InvGrnDetail_GetAllMethod", lstPrm3);
						}
						if (item3.DocumentTypeId == 1602 || item3.DocumentTypeId == 1618 || item3.DocumentTypeId == 1858)
						{
							lstPrm3.Add(new SqlParameter("@Activity", "ReadByInvGrnID_Engr"));
							item3.invGrnDetail = GenericProvider.GetAllDetail<InvGrnDetail>(sqlConnection2, "Sp_InvGrnDetail_GetAllMethod", lstPrm3);
						}
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.InvGrn item2 in lst)
					{
						if (item2.DocumentTypeId == 46 || ((item2.DocumentTypeId == 137) | (item2.DocumentTypeId == 217)) || item2.DocumentTypeId == 165 || item2.DocumentTypeId == 167 || item2.DocumentTypeId == 169 || item2.DocumentTypeId == 143)
						{
							List<SqlParameter> lstPrm2 = new List<SqlParameter>();
							lstPrm2.Add(new SqlParameter("@Id", item2.Id));
							lstPrm2.Add(new SqlParameter("@Activity", "ReadByInvGrnIdEmptyBagsDetail"));
							item2.InvgrnDetailEmptyBagslist = GenericProvider.GetAllDetail<InvgrnDetailEmptyBags>(sqlConnection3, "Sp_InvGrnDetail_GetAllMethod", lstPrm2);
						}
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Inventory.InvGrn item in lst)
				{
					if (item.DocumentTypeId == 46)
					{
						List<SqlParameter> lstPrm = new List<SqlParameter>();
						lstPrm.Add(new SqlParameter("@Id", item.Id));
						lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId_PurchaseBreakup"));
						item.InvGrnPurchaseBreakUplist = GenericProvider.GetAllDetail<InvGrnPurchaseBreakUp>(sqlcon, "Sp_InvGRN_GetAllMethod", lstPrm);
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
