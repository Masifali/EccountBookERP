using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.pcc;

namespace Architecture.DAL.pcc;

public class InvStockTransferHeader
{
	public int SetData(Architecture.Model.pcc.InvStockTransferHeader obj, string ProcName)
	{
		int returnID = 0;
		List<GetItemGlIdsandItemName> Itemlist = new List<GetItemGlIdsandItemName>();
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
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
			foreach (Architecture.Model.pcc.InvStockTransferDetail invStockTransferDetail in obj.InvStockTransferDetailList)
			{
				invStockTransferDetail.LineId = LineId;
				LineId++;
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
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
			InventoryTransactions transactions = new InventoryTransactions();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = obj.Id;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			bool FIFOCgs = CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5);
			if (FIFOCgs)
			{
				Itemlist = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId);
				obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
				foreach (Architecture.Model.pcc.InvStockTransferDetail item5 in obj.InvStockTransferDetailList.ToList())
				{
					if (item5.ActionTypeId == 3)
					{
						continue;
					}
					ReportsParameters stock = new ReportsParameters();
					stock.OrganizationId = obj.OrganizationId;
					stock.CompanyId = obj.CompanyId;
					stock.DocDate = obj.DocDate;
					stock.ItemId = item5.ItemId;
					stock.WarehouseId = item5.WarehouseFromId;
					stock.stockUOM = item5.AttributeVareintId;
					if (obj.ModifyUserId > 0)
					{
						stock.DocumentTypeId = obj.DocumentTypeId;
						stock.Id = obj.Id;
					}
					stock.ItemQty = Conversion.ToDouble(item5.ItemQty);
					stock.NetWeight = Conversion.ToDouble(item5.NetWeight);
					stock.LineId = item5.LineId;
					List<GetItemGlIdsandItemName> find2 = Itemlist.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item5.ItemId)).ToList();
					if (find2.Count > 0)
					{
						stock.ItemName = find2[0].ItemName.ToString();
					}
					foreach (InventoryStockEvalautionDetail stockdetail in CommonServices.FIFOImplementionForConcrete(stock, obj.InventoryStockEvalautionDetailslist).InventoryStockEvalautionDetailslist.ToList())
					{
						if (item5.AttributeVareintId == 0)
						{
							stockdetail.ItemUom = item5.ItemUomId;
						}
						else
						{
							stockdetail.VarientId = item5.AttributeVareintId;
						}
						InventoryStockEvalautionDetail InTrans = new InventoryStockEvalautionDetail();
						InTrans.LineId = stockdetail.LineId;
						InTrans.ItemId = stockdetail.ItemId;
						InTrans.WarehouseId = item5.WarehouseToId;
						InTrans.RateUom = stockdetail.RateUom;
						InTrans.JobLotId = Conversion.ToInt(stockdetail.JobLotId);
						InTrans.InvPackingTypeId = Conversion.ToInt(stockdetail.InvPackingTypeId);
						InTrans.VarientId = Conversion.ToInt(stockdetail.VarientId);
						if (InTrans.VarientId == 0)
						{
							InTrans.ItemUom = Conversion.ToInt(stockdetail.ItemUom);
						}
						else
						{
							stockdetail.ItemUom = 0;
						}
						InTrans.CropBatch = Conversion.ToString(stockdetail.CropBatch);
						InTrans.RefRefDocumentTypeId = 0;
						InTrans.RefRefDocIdNo = 0;
						InTrans.RefRefDocSubIdNo = 0;
						InTrans.QtyIn = Conversion.ToDouble(stockdetail.QtyOut);
						InTrans.BillWeightIn = Conversion.ToDouble(stockdetail.BillWeightOut);
						InTrans.StockWeightIn = Conversion.ToDouble(stockdetail.StockWeightOut);
						InTrans.ItemRate = Conversion.ToDouble(stockdetail.CgsRate);
						InTrans.AmountIn = stockdetail.CgsAmount;
						obj.InventoryStockEvalautionDetailslist.Add(InTrans);
						obj.InventoryStockEvalautionDetailslist.Add(stockdetail);
					}
				}
				foreach (Architecture.Model.pcc.InvStockTransferDetail item4 in obj.InvStockTransferDetailList)
				{
					if (FIFOCgs && item4.ActionTypeId != 3)
					{
						string ItemName = "";
						List<GetItemGlIdsandItemName> find = Itemlist.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item4.ItemId)).ToList();
						if (find.Count > 0)
						{
							ItemName = find[0].ItemName.ToString();
						}
						new List<InventoryStockEvalautionDetail>();
						List<InventoryStockEvalautionDetail> source = obj.InventoryStockEvalautionDetailslist.Where((InventoryStockEvalautionDetail x) => x.ItemId == Conversion.ToInt(item4.ItemId) && x.WarehouseId == Conversion.ToInt(item4.WarehouseFromId) && x.VarientId == Conversion.ToInt(item4.AttributeVareintId) && x.QtyOut > 0.0).ToList();
						double CgsAmount = source.Sum((InventoryStockEvalautionDetail row) => row.CgsAmount);
						double QtyOut = source.Sum((InventoryStockEvalautionDetail row) => row.QtyOut);
						if (!(CgsAmount > 0.0) || !(QtyOut > 0.0))
						{
							throw new Exception("CGS Amount or Qty cannot be equal to zero.this Item " + ItemName + " against FIFO Method");
						}
						item4.ItemRate = Conversion.ToDecimal(CgsAmount / QtyOut);
						item4.ItemAmount = item4.ItemRate * item4.ItemQty;
					}
					item4.InvStockTransferHeaderId = obj.Id;
					int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[pcc].[USP_InvStockTransferDetail_Insert]"));
					item4.Id = code;
				}
				if (obj.InventoryStockEvalautionDetailslist == null || obj.InventoryStockEvalautionDetailslist.Count <= 0)
				{
					throw new Exception("InventoryStockEvalautionDetailslist not found against FIFO....");
				}
				if (obj.ModifyUserId > 0)
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
				foreach (InventoryStockEvalautionDetail item3 in obj.InventoryStockEvalautionDetailslist)
				{
					item3.OrganizationId = obj.OrganizationId;
					item3.CompanyId = obj.CompanyId;
					item3.DocDate = obj.DocDate;
					item3.DocCodeNo = obj.DocNo;
					item3.BranchesId = obj.BranchesId;
					item3.RefDocumentTypeId = obj.DocumentTypeId;
					item3.EntryUser = obj.EntryUserId;
					item3.ModifyUser = obj.ModifyUserId;
					item3.CalcType = "Qty";
					if (item3.BillWeightIn > 0.0)
					{
						item3.ItemRate = item3.ItemRate;
					}
					else
					{
						item3.ItemRate = item3.CgsRate;
					}
					item3.AmountOut = item3.CgsAmount;
					Architecture.Model.pcc.InvStockTransferDetail sid = obj.InvStockTransferDetailList.Find((Architecture.Model.pcc.InvStockTransferDetail x) => x.LineId == item3.LineId && x.LineId > 0 && item3.LineId > 0);
					if (sid != null)
					{
						item3.RefDocIdNo = sid.InvStockTransferHeaderId;
						item3.RefDocSubIdNo = sid.Id;
					}
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "USP_InventoryStockEvalautionDetail_Insert"));
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
			if (obj.DocumentTypeId == 1860)
			{
				foreach (Architecture.Model.pcc.InvStockTransferDetail item in obj.InvStockTransferDetailList)
				{
					SqlCommand sqlCommandInv = new SqlCommand
					{
						Connection = sqlcon,
						Transaction = sqlTrn,
						CommandType = CommandType.StoredProcedure,
						CommandText = "USP_StockValidationConcrete"
					};
					sqlCommandInv.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommandInv.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommandInv.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
					sqlCommandInv.Parameters.AddWithValue("@DocDate", obj.DocDate);
					sqlCommandInv.Parameters.AddWithValue("@ItemId", item.ItemId);
					sqlCommandInv.Parameters.AddWithValue("@WarehouseId", item.WarehouseFromId);
					if (item.AttributeVareintId > 0)
					{
						sqlCommandInv.Parameters.AddWithValue("@VarientId", item.AttributeVareintId);
					}
					sqlCommandInv.Parameters.AddWithValue("@Qty", item.ItemQty);
					sqlCommandInv.ExecuteNonQuery();
				}
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

	public List<Architecture.Model.pcc.InvStockTransferHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.pcc.InvStockTransferHeader> lst = new List<Architecture.Model.pcc.InvStockTransferHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.pcc.InvStockTransferHeader>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.pcc.InvStockTransferHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId_InvStockTransferDetail"));
					item.InvStockTransferDetailList = GenericProvider.GetAllDetail<Architecture.Model.pcc.InvStockTransferDetail>(sqlcon, "[pcc].[USP_InvStockTransferHeader_GetAllMethod]", lstPrm);
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
