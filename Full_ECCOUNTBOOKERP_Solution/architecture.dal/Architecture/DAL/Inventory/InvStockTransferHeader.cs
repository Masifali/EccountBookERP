using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.VPS;

namespace Architecture.DAL.Inventory;

public class InvStockTransferHeader
{
	public int SetData(Architecture.Model.Inventory.InvStockTransferHeader obj, string ProcName)
	{
		int returnID = 0;
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
		bool loaderflag = false;
		string ItemName = "";
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		List<Architecture.Model.Inventory.GetItemGlIdsandItemName> Itemlist = new List<Architecture.Model.Inventory.GetItemGlIdsandItemName>();
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
			int LineId = 0;
			foreach (InvStockTransferDetail item6 in obj.InvStockTransferDetail)
			{
				LineId = (item6.LineId = LineId + 1);
				if (item6.RefDocumentTypeId > 0 && item6.RefDocNoId > 0 && item6.RefDocSubIdNo > 0)
				{
					loaderflag = true;
				}
			}
			bool FIFOCgs = false;
			if (obj.DocumentTypeId != 807)
			{
				FIFOCgs = CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5);
			}
			if (FIFOCgs && !loaderflag)
			{
				Itemlist = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId);
				if (obj.TransferType != "Inward")
				{
					obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
					foreach (InvStockTransferDetail item9 in obj.InvStockTransferDetail.ToList())
					{
						ReportsParameters stock = new ReportsParameters();
						stock.OrganizationId = obj.OrganizationId;
						stock.CompanyId = obj.CompanyId;
						stock.DocDate = obj.DocDate;
						stock.ItemId = item9.ItemId;
						stock.WarehouseId = item9.FromWarehouseId;
						if (obj.DocumentTypeId != 807)
						{
							stock.stockUOM = item9.PackUomId;
							stock.JobLotId = item9.JobLotId;
							stock.CropYear = item9.CropYear;
							stock.PackingTypeId = item9.InvPackingTypeId;
						}
						if (obj.ModifyUser > 0)
						{
							stock.DocumentTypeId = obj.DocumentTypeId;
							stock.Id = obj.Id;
						}
						stock.ItemQty = item9.Qty;
						stock.NetWeight = item9.NetWeight;
						stock.RateEqvailent = item9.RateUom;
						stock.RateUOM = item9.RateUomId;
						stock.LineId = item9.LineId;
						List<Architecture.Model.Inventory.GetItemGlIdsandItemName> find2 = Itemlist.Where((Architecture.Model.Inventory.GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item9.ItemId)).ToList();
						if (find2.Count > 0)
						{
							stock.ItemName = find2[0].ItemName.ToString();
						}
						foreach (InventoryStockEvalautionDetail stockdetail in CommonServices.FIFOImplemention(stock, obj.InventoryStockEvalautionDetailslist).InventoryStockEvalautionDetailslist.ToList())
						{
							InventoryStockEvalautionDetail InTrans = new InventoryStockEvalautionDetail();
							InTrans.LineId = stockdetail.LineId;
							InTrans.ItemId = stockdetail.ItemId;
							InTrans.WarehouseId = item9.ToWarehouseId;
							InTrans.RefWarehouseId = item9.FromWarehouseId;
							InTrans.RateUom = stockdetail.RateUom;
							InTrans.JobLotId = Conversion.ToInt(item9.JobLotIdTo);
							InTrans.InvPackingTypeId = Conversion.ToInt(stockdetail.InvPackingTypeId);
							InTrans.ItemUom = Conversion.ToInt(stockdetail.ItemUom);
							InTrans.CropBatch = Conversion.ToString(stockdetail.CropBatch);
							InTrans.RefRefDocumentTypeId = 0;
							InTrans.RefRefDocIdNo = 0;
							InTrans.RefRefDocSubIdNo = 0;
							InTrans.QtyIn = Conversion.ToDouble(stockdetail.QtyOut);
							InTrans.BillWeightIn = Conversion.ToDouble(stockdetail.BillWeightOut);
							InTrans.StockWeightIn = Conversion.ToDouble(stockdetail.StockWeightOut);
							InTrans.ItemRate = Conversion.ToDouble(stockdetail.CgsRate);
							InTrans.AmountIn = stockdetail.CgsAmount;
							stockdetail.RefWarehouseId = item9.ToWarehouseId;
							if (obj.TransferType == "MoveOrder")
							{
								obj.InventoryStockEvalautionDetailslist.Add(InTrans);
								obj.InventoryStockEvalautionDetailslist.Add(stockdetail);
							}
							else if (obj.TransferType == "Outward" && obj.NoOfBranches > 1)
							{
								obj.InventoryStockEvalautionDetailslist.Add(stockdetail);
							}
						}
					}
				}
			}
			foreach (InvStockTransferDetail item8 in obj.InvStockTransferDetail)
			{
				if (FIFOCgs && !loaderflag && obj.TransferType != "Inward")
				{
					List<Architecture.Model.Inventory.GetItemGlIdsandItemName> find = Itemlist.Where((Architecture.Model.Inventory.GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item8.ItemId)).ToList();
					if (find.Count > 0)
					{
						ItemName = find[0].ItemName.ToString();
					}
					List<InventoryStockEvalautionDetail> FilterlistbyItemId = new List<InventoryStockEvalautionDetail>();
					FilterlistbyItemId = ((obj.DocumentTypeId != 807) ? obj.InventoryStockEvalautionDetailslist.Where((InventoryStockEvalautionDetail x) => x.ItemId == Conversion.ToInt(item8.ItemId) && x.WarehouseId == Conversion.ToInt(item8.FromWarehouseId) && x.JobLotId == Conversion.ToInt(item8.JobLotId) && x.InvPackingTypeId == Conversion.ToInt(item8.InvPackingTypeId) && x.ItemUom == Conversion.ToInt(item8.PackUomId) && x.CropBatch == Conversion.ToString(item8.CropYear) && x.BillWeightOut > 0.0).ToList() : obj.InventoryStockEvalautionDetailslist.Where((InventoryStockEvalautionDetail x) => x.ItemId == Conversion.ToInt(item8.ItemId) && x.WarehouseId == Conversion.ToInt(item8.FromWarehouseId) && x.BillWeightOut > 0.0).ToList());
					double CgsAmount = FilterlistbyItemId.Sum((InventoryStockEvalautionDetail row) => row.CgsAmount);
					double StockWeightOut = FilterlistbyItemId.Sum((InventoryStockEvalautionDetail row) => row.StockWeightOut);
					if (!(CgsAmount > 0.0) || !(StockWeightOut > 0.0))
					{
						throw new Exception("CGS Amount or StockWeight cannot be equal to zero.this Item " + ItemName + " against FIFO Method");
					}
					item8.ItemRate = CgsAmount / StockWeightOut;
					item8.ItemAmount = item8.ItemRate * item8.NetWeight;
					ReportsParameters UomId = new ReportsParameters();
					UomId.OrganizationId = obj.OrganizationId;
					UomId.CompanyId = obj.CompanyId;
					UomId.ItemId = item8.ItemId;
					UomId.RateEqvailent = 1.0;
					item8.RateUomId = CommonServices.GetUomScheduleIdByItemIdAndEquivalent(UomId);
					if (item8.RateUomId <= 0)
					{
						throw new Exception("1KG Rate ScheduleId Not Found this Item " + ItemName + " against FIFO Method");
					}
				}
				item8.InvStockTransferHeaderId = obj.Id;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "Sp_InvStockTransferDetail_Insert"));
				item8.Id = code;
			}
			foreach (InvStockTransferExpense item5 in obj.InvStockTransferExpense)
			{
				item5.InvStockTransferHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "Sp_InvStockTransferExpense_Insert"));
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
				foreach (DMSAttachments item4 in obj.AttachmentsList.ToList())
				{
					item4.RefAccountId = obj.WbTicketId;
					item4.RefDocumentTypeId = obj.DocumentTypeId;
					item4.RefDocumentNo = returnID;
					item4.EntryDate = DateTime.Now;
					item4.ModifyDate = DateTime.Now;
					item4.EntryUser = obj.EntryUser;
					item4.ModifyUser = obj.ModifyUser;
					item4.OrganizationId = obj.OrganizationId;
					item4.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Proc_DMSAttachments_Insert"));
				}
			}
			if (FIFOCgs && !loaderflag && obj.TransferType != "Inward")
			{
				if (obj.InventoryStockEvalautionDetailslist == null || obj.InventoryStockEvalautionDetailslist.Count <= 0)
				{
					throw new Exception("InventoryStockEvalautionDetailslist not found against FIFO....");
				}
				if (obj.ModifyUser > 0)
				{
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlcon;
					sqlCommand.Transaction = sqlTrn;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandText = "[dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId]";
					sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand.Parameters.AddWithValue("@RefDocumentTypeId", obj.DocumentTypeId);
					sqlCommand.Parameters.AddWithValue("@RefDocIdNo", obj.Id);
					sqlCommand.ExecuteNonQuery();
				}
				foreach (InventoryStockEvalautionDetail item7 in obj.InventoryStockEvalautionDetailslist)
				{
					item7.OrganizationId = obj.OrganizationId;
					item7.CompanyId = obj.CompanyId;
					item7.DocDate = obj.DocDate;
					item7.DocCodeNo = obj.DocNo;
					item7.BranchesId = obj.BranchesId;
					item7.RefDocumentTypeId = obj.DocumentTypeId;
					item7.EntryUser = obj.EntryUser;
					item7.ModifyUser = obj.ModifyUser;
					item7.CalcType = "Weight";
					if (item7.BillWeightIn > 0.0)
					{
						item7.ItemRate = item7.ItemRate;
					}
					else
					{
						item7.ItemRate = item7.CgsRate;
					}
					item7.AmountOut = item7.CgsAmount;
					InvStockTransferDetail sid = obj.InvStockTransferDetail.Find((InvStockTransferDetail x) => x.LineId == item7.LineId && x.LineId > 0 && item7.LineId > 0);
					if (sid != null)
					{
						item7.RefDocIdNo = sid.InvStockTransferHeaderId;
						item7.RefDocSubIdNo = sid.Id;
					}
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "USP_InventoryStockEvalautionDetail_Insert"));
				}
			}
			else
			{
				InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
				inventoryTransactions.OrganizationId = obj.OrganizationId;
				inventoryTransactions.CompanyId = obj.CompanyId;
				inventoryTransactions.RefDocumentTypeId = obj.DocumentTypeId;
				inventoryTransactions.RefDocIdNo = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
			}
			if (obj.InvStockTransferExpense.Count > 0 && !FIFOCgs && !Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "InventoryFinancialsEffectsInActive")))
			{
				DataTable dtvh = CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.Id);
				if (dtvh.Rows.Count > 0)
				{
					VoucherHeadIdGet = Conversion.ToInt(dtvh.Rows[0]["Id"]);
					obj.VoucherHeadInvoices.Id = VoucherHeadIdGet;
				}
				if (VoucherHeadIdGet == 0)
				{
					obj.VoucherHeadInvoices.DocumentTypeSrNo = obj.Id;
					obj.VoucherHeadInvoices.RefDocNoId = obj.Id;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_Insert"));
				}
				else
				{
					obj.VoucherHeadInvoices.DocumentTypeSrNo = obj.Id;
					obj.VoucherHeadInvoices.RefDocNoId = obj.Id;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_Update"));
				}
				if (VoucherHeadId > 0)
				{
					obj.VoucherHeadInvoices.Id = VoucherHeadId;
				}
				else
				{
					VoucherHeadId = obj.VoucherHeadInvoices.Id;
				}
				if (obj.VoucherHeadInvoices.voucherDetailList == null || obj.VoucherHeadInvoices.voucherDetailList.Count <= 0)
				{
					throw new Exception("VoucherDetail List Not Found");
				}
				foreach (VoucherDetail item3 in obj.VoucherHeadInvoices.voucherDetailList)
				{
					item3.VoucherHeadId = obj.VoucherHeadInvoices.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Sp_VoucherDetail_Insert"));
				}
				SqlCommand sqlCommand2 = new SqlCommand();
				sqlCommand2.Connection = sqlcon;
				sqlCommand2.Transaction = sqlTrn;
				sqlCommand2.CommandType = CommandType.StoredProcedure;
				sqlCommand2.CommandText = "USP_VoucherBalanceCheck";
				sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand2.Parameters.AddWithValue("@Id", obj.Id);
				sqlCommand2.ExecuteScalar();
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_H_Insert"));
				foreach (VoucherDetail item2 in obj.VoucherHeadInvoices.voucherDetailList)
				{
					item2.VoucherHeadId = obj.VoucherHeadInvoices.Id;
					item2.DocumentTypeIdRef = Code;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_H_Insert"));
				}
			}
			InventoryTransactions transactions = new InventoryTransactions();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = obj.Id;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			if (obj.DocumentTypeId == 806 && obj.DetailRowsRemoveIds != null && obj.DetailRowsRemoveIds != string.Empty)
			{
				SqlCommand sqlCommand3 = new SqlCommand();
				sqlCommand3.Connection = sqlcon;
				sqlCommand3.Transaction = sqlTrn;
				sqlCommand3.CommandType = CommandType.StoredProcedure;
				sqlCommand3.CommandText = "[dbo].[USP_InvStockTransferDetailRowsDeleteByIds]";
				sqlCommand3.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand3.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand3.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
				sqlCommand3.Parameters.AddWithValue("@Id", obj.Id);
				sqlCommand3.Parameters.AddWithValue("@DetailIds", obj.DetailRowsRemoveIds);
				sqlCommand3.ExecuteScalar();
			}
			foreach (InvStockTransferDetail item in obj.InvStockTransferDetail)
			{
				SqlCommand sqlCommand4 = new SqlCommand();
				sqlCommand4.Connection = sqlcon;
				sqlCommand4.Transaction = sqlTrn;
				sqlCommand4.CommandType = CommandType.StoredProcedure;
				sqlCommand4.CommandText = "USP_InventoryValidation";
				sqlCommand4.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand4.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand4.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
				sqlCommand4.Parameters.AddWithValue("@DocDate", obj.DocDate);
				sqlCommand4.Parameters.AddWithValue("@ItemId", item.ItemId);
				sqlCommand4.Parameters.AddWithValue("@WarehouseId", item.FromWarehouseId);
				sqlCommand4.Parameters.AddWithValue("@JobLotId", item.JobLotId);
				sqlCommand4.Parameters.AddWithValue("@CropYear", item.CropYear);
				sqlCommand4.Parameters.AddWithValue("@InvPackingTypeId", item.InvPackingTypeId);
				sqlCommand4.Parameters.AddWithValue("@PackUomId", item.PackUomId);
				sqlCommand4.Parameters.AddWithValue("@RefDocumentTypeId", item.RefDocumentTypeId);
				sqlCommand4.Parameters.AddWithValue("@RefDocNoId", item.RefDocNoId);
				sqlCommand4.Parameters.AddWithValue("@RefDocSubIdNo", item.RefDocSubIdNo);
				sqlCommand4.Parameters.AddWithValue("@NetWeight", item.NetWeight);
				sqlCommand4.Parameters.AddWithValue("@ItemConditionId", item.ItemConditionId);
				sqlCommand4.ExecuteScalar();
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

	public List<Architecture.Model.Inventory.InvStockTransferHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.InvStockTransferHeader> lst = new List<Architecture.Model.Inventory.InvStockTransferHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetAllDetail<Architecture.Model.Inventory.InvStockTransferHeader>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.InvStockTransferHeader item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@HeaderId", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
						item2.InvStockTransferDetail = GenericProvider.GetAllDetail<InvStockTransferDetail>(sqlConnection2, "Sp_InvStockTransferHeader_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Inventory.InvStockTransferHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@HeaderId", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadExpenseDetailByHeaderId"));
					item.InvStockTransferExpense = GenericProvider.GetAllDetail<InvStockTransferExpense>(sqlcon, "Sp_InvStockTransferHeader_GetAllMethod", lstPrm);
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
