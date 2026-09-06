using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.Model.VPS;

namespace Architecture.DAL.Export;

public class ExImForwarding
{
	public int SetDate(Architecture.Model.Export.ExImForwarding obj, string ProcName)
	{
		string AttachmentFolderPath = "";
		List<string> copiedFiles = new List<string>();
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
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
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			int LineIdForPDI = 0;
			foreach (ExImForwardingPackingDetail item5 in obj.ExImForwardingDetail)
			{
				LineIdForPDI = (item5.LineId = LineIdForPDI + 1);
				item5.ExImForwardingId = obj.Id;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "Sp_ExImForwardingPackingDetail_Insert"));
				item5.Id = code;
			}
			foreach (ExImForwardingOtherItems item4 in obj.ExImForwardingOtherItemslist)
			{
				item4.ExImForwardingId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Sp_ExImForwardingOtherItems_Insert"));
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				List<SqlParameter> p2 = new List<SqlParameter>();
				p2.Add(new SqlParameter
				{
					ParameterName = "@ScreenName",
					Value = obj.AttachmentsList[0].ScreenName
				});
				p2.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = returnID
				});
				p2.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "DeleteById"
				});
				new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p2);
				foreach (DMSAttachments item3 in obj.AttachmentsList.ToList())
				{
					item3.RefAccountId = obj.SupplierCustomerId;
					item3.RefDocumentTypeId = obj.DocumentTypeId;
					item3.RefDocumentNo = returnID;
					item3.EntryDate = DateTime.Now;
					item3.ModifyDate = DateTime.Now;
					item3.EntryUser = obj.EntryUser;
					item3.ModifyUser = obj.ModifyUser;
					item3.OrganizationId = obj.OrganizationId;
					item3.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Proc_DMSAttachments_Insert"));
				}
			}
			bool FIFOCgs = CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5);
			if (FIFOCgs && obj.DocumentTypeId == 210)
			{
				List<Architecture.Model.Inventory.GetItemGlIdsandItemName> dtitem = (dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId));
				obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
				foreach (ExImForwardingPackingDetail item9 in obj.ExImForwardingDetail.ToList())
				{
					if (item9.ActionTypeId == 3)
					{
						continue;
					}
					List<Architecture.Model.Inventory.GetItemGlIdsandItemName> find = dtitem.Where((Architecture.Model.Inventory.GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item9.ItemId)).ToList();
					if (find.Count <= 0)
					{
						continue;
					}
					ReportsParameters stock = new ReportsParameters();
					stock.OrganizationId = obj.OrganizationId;
					stock.CompanyId = obj.CompanyId;
					stock.ItemId = item9.ItemId;
					stock.WarehouseId = item9.WarehouseId;
					stock.stockUOM = item9.UOMScheduleIdOuter;
					stock.DocDate = ((obj.AutoUpdateId == 0) ? obj.DocDate : DateTime.Now);
					stock.JobLotId = item9.LotJobId;
					stock.CropYearId = item9.CropYearId;
					stock.CropYear = item9.CropYear;
					stock.PackingTypeId = item9.PackingMaterialId;
					if (obj.ModifyUser > 0)
					{
						stock.DocumentTypeId = obj.DocumentTypeId;
						stock.Id = obj.Id;
					}
					stock.ItemQty = item9.OuterQty;
					stock.NetWeight = item9.NetWeight;
					stock.LineId = item9.LineId;
					stock.ItemName = Conversion.ToString(find[0].ItemName);
					foreach (InventoryStockEvalautionDetail stockdetail in CommonServices.FIFOImplemention(stock, obj.InventoryStockEvalautionDetailslist).InventoryStockEvalautionDetailslist.ToList())
					{
						InventoryStockEvalautionDetail InTrans = new InventoryStockEvalautionDetail();
						InTrans.LineId = stockdetail.LineId;
						InTrans.ItemId = stockdetail.ItemId;
						InTrans.WarehouseId = item9.WarehouseToId;
						InTrans.RateUom = stockdetail.RateUom;
						InTrans.JobLotId = Conversion.ToInt(stockdetail.JobLotId);
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
						obj.InventoryStockEvalautionDetailslist.Add(InTrans);
						obj.InventoryStockEvalautionDetailslist.Add(stockdetail);
					}
				}
				if (obj.InventoryStockEvalautionDetailslist != null && obj.InventoryStockEvalautionDetailslist.Count > 0)
				{
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
					foreach (InventoryStockEvalautionDetail item8 in obj.InventoryStockEvalautionDetailslist)
					{
						item8.OrganizationId = obj.OrganizationId;
						item8.CompanyId = obj.CompanyId;
						item8.DocDate = obj.DocDate;
						item8.DocCodeNo = obj.DocNo;
						item8.SupplierCustomerId = obj.SupplierCustomerId;
						item8.BranchesId = obj.BranchesId;
						item8.RefDocumentTypeId = obj.DocumentTypeId;
						item8.EntryUser = obj.EntryUser;
						item8.ModifyUser = obj.ModifyUser;
						item8.ItemRate = item8.CgsRate;
						item8.AmountOut = item8.CgsAmount;
						item8.CalcType = "Weight";
						ExImForwardingPackingDetail sid = obj.ExImForwardingDetail.Find((ExImForwardingPackingDetail x) => x.LineId == item8.LineId && x.LineId > 0 && item8.LineId > 0);
						if (sid != null)
						{
							item8.RefDocIdNo = sid.ExImForwardingId;
							item8.RefDocSubIdNo = sid.Id;
						}
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "USP_InventoryStockEvalautionDetail_Insert"));
					}
				}
			}
			else if (FIFOCgs && (obj.DocumentTypeId == 205 || obj.DocumentTypeId == 206))
			{
				List<Architecture.Model.Inventory.GetItemGlIdsandItemName> dtitem2 = (dtitem2 = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId));
				obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
				foreach (ExImForwardingPackingDetail item7 in obj.ExImForwardingDetail.ToList())
				{
					if (item7.ActionTypeId == 3)
					{
						continue;
					}
					List<Architecture.Model.Inventory.GetItemGlIdsandItemName> find2 = dtitem2.Where((Architecture.Model.Inventory.GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item7.ItemId)).ToList();
					if (find2.Count <= 0)
					{
						continue;
					}
					ReportsParameters stock2 = new ReportsParameters();
					stock2.OrganizationId = obj.OrganizationId;
					stock2.CompanyId = obj.CompanyId;
					stock2.ItemId = item7.ItemId;
					stock2.WarehouseId = item7.WarehouseId;
					stock2.stockUOM = item7.UOMScheduleIdOuter;
					stock2.DocDate = ((obj.AutoUpdateId == 0) ? obj.DocDate : DateTime.Now);
					stock2.JobLotId = item7.LotJobId;
					stock2.CropYearId = item7.CropYearId;
					stock2.CropYear = item7.CropYear;
					stock2.PackingTypeId = item7.PackingMaterialId;
					if (obj.ModifyUser > 0)
					{
						stock2.DocumentTypeId = obj.DocumentTypeId;
						stock2.Id = obj.Id;
					}
					stock2.ItemQty = item7.OuterQty;
					stock2.NetWeight = item7.StockWeight;
					stock2.LineId = item7.LineId;
					stock2.ItemName = Conversion.ToString(find2[0].ItemName);
					foreach (InventoryStockEvalautionDetail stockdetail2 in CommonServices.FIFOImplemention(stock2, obj.InventoryStockEvalautionDetailslist).InventoryStockEvalautionDetailslist.ToList())
					{
						if (item7.CropYearId > 0)
						{
							List<SqlParameter> p = new List<SqlParameter>();
							p.Add(new SqlParameter
							{
								ParameterName = "@Id",
								Value = item7.CropYearId
							});
							p.Add(new SqlParameter
							{
								ParameterName = "@Activity",
								Value = "ReadById"
							});
							List<Architecture.Model.Inventory.InvCropYear> lst = new List<Architecture.Model.Inventory.InvCropYear>();
							lst = new Architecture.DAL.Inventory.InvCropYear().GetData("Sp_InvCropYear_GetAllMethod", p);
							if (lst.Count > 0)
							{
								stockdetail2.CropBatch = Conversion.ToString(lst[0].CropYear);
							}
						}
						stockdetail2.InvoiceId = obj.ExImInvoiceId;
						stockdetail2.InvoiceDetailId = item7.InvoiceDetailId;
						obj.InventoryStockEvalautionDetailslist.Add(stockdetail2);
					}
				}
				if (obj.InventoryStockEvalautionDetailslist != null && obj.InventoryStockEvalautionDetailslist.Count > 0)
				{
					if (obj.ModifyUser > 0)
					{
						SqlCommand sqlCommand2 = new SqlCommand();
						sqlCommand2.Connection = sqlcon;
						sqlCommand2.Transaction = sqlTrn;
						sqlCommand2.CommandType = CommandType.StoredProcedure;
						sqlCommand2.CommandText = "[dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId]";
						sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
						sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
						sqlCommand2.Parameters.AddWithValue("@RefDocumentTypeId", obj.DocumentTypeId);
						sqlCommand2.Parameters.AddWithValue("@RefDocIdNo", obj.Id);
						sqlCommand2.ExecuteNonQuery();
					}
					foreach (InventoryStockEvalautionDetail item6 in obj.InventoryStockEvalautionDetailslist)
					{
						item6.OrganizationId = obj.OrganizationId;
						item6.CompanyId = obj.CompanyId;
						item6.DocDate = obj.DocDate;
						item6.DocCodeNo = obj.DocNo;
						item6.SupplierCustomerId = obj.SupplierCustomerId;
						item6.BranchesId = obj.BranchesId;
						item6.OtherDocumentTypeId = obj.DocumentTypeId;
						item6.EntryUser = obj.EntryUser;
						item6.ModifyUser = obj.ModifyUser;
						item6.CalcType = "Weight";
						ExImForwardingPackingDetail sid2 = obj.ExImForwardingDetail.Find((ExImForwardingPackingDetail x) => x.LineId == item6.LineId && x.LineId > 0 && item6.LineId > 0);
						if (sid2 != null)
						{
							item6.OtherDocNoId = sid2.ExImForwardingId;
							item6.OtherSubDocNoId = sid2.Id;
						}
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "USP_InventoryStockEvalautionDetail_Insert"));
					}
				}
			}
			else
			{
				SqlCommand sqlCommand3 = new SqlCommand();
				sqlCommand3.Connection = sqlcon;
				sqlCommand3.Transaction = sqlTrn;
				sqlCommand3.CommandType = CommandType.StoredProcedure;
				sqlCommand3.CommandText = "usp_StockEvaluationUpdateForExportForwarding";
				sqlCommand3.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand3.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand3.Parameters.AddWithValue("@RefDocumentTypeId", obj.DocumentTypeId);
				sqlCommand3.Parameters.AddWithValue("@RefDocIdNo", obj.Id);
				sqlCommand3.Parameters.AddWithValue("@ActionId", 1);
				sqlCommand3.ExecuteNonQuery();
			}
			if (FIFOCgs && (obj.DocumentTypeId == 210 || obj.DocumentTypeId == 205 || obj.DocumentTypeId == 206) && (obj.InventoryStockEvalautionDetailslist == null || obj.InventoryStockEvalautionDetailslist.Count == 0))
			{
				throw new Exception("InventoryStockEvalautionDetailslist not Fill");
			}
			SqlCommand sqlCommand4 = new SqlCommand();
			sqlCommand4.Connection = sqlcon;
			sqlCommand4.Transaction = sqlTrn;
			sqlCommand4.CommandType = CommandType.StoredProcedure;
			sqlCommand4.CommandText = "usp_StockInTransitUpdate_VoucherInsertFromGdnOrForwarding";
			sqlCommand4.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
			sqlCommand4.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
			sqlCommand4.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
			sqlCommand4.Parameters.AddWithValue("@Id", obj.Id);
			sqlCommand4.ExecuteNonQuery();
			InventoryTransactions transactions = new InventoryTransactions();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = returnID;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			foreach (ExImForwardingPackingDetail item2 in obj.ExImForwardingDetail)
			{
				if (item2.ActionTypeId != 3)
				{
					SqlCommand sqlCommand5 = new SqlCommand();
					sqlCommand5.Connection = sqlcon;
					sqlCommand5.Transaction = sqlTrn;
					sqlCommand5.CommandType = CommandType.StoredProcedure;
					sqlCommand5.CommandText = "USP_InventoryValidation";
					sqlCommand5.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand5.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand5.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
					sqlCommand5.Parameters.AddWithValue("@DocDate", (obj.AutoUpdateId == 0) ? obj.DocDate : DateTime.Now);
					sqlCommand5.Parameters.AddWithValue("@ItemId", item2.ItemId);
					sqlCommand5.Parameters.AddWithValue("@WarehouseId", item2.WarehouseId);
					sqlCommand5.Parameters.AddWithValue("@JobLotId", item2.LotJobId);
					sqlCommand5.Parameters.AddWithValue("@CropYearId", item2.CropYearId);
					sqlCommand5.Parameters.AddWithValue("@InvPackingTypeId", item2.PackingMaterialId);
					sqlCommand5.Parameters.AddWithValue("@PackUomId", item2.UOMScheduleIdOuter);
					sqlCommand5.Parameters.AddWithValue("@NetWeight", (obj.DocumentTypeId == 210) ? item2.NetWeight : item2.StockWeight);
					sqlCommand5.ExecuteNonQuery();
				}
			}
			if (obj.DocumentTypeId != 210)
			{
				List<ExImForwardingPackingDetail> uniqueList = (from x in obj.ExImForwardingDetail
					group x by x.ExImLcOrderId into g
					select g.First()).ToList();
				if (uniqueList.Count > 0)
				{
					foreach (ExImForwardingPackingDetail item in uniqueList)
					{
						SqlCommand sqlCommand6 = new SqlCommand();
						sqlCommand6.Connection = sqlcon;
						sqlCommand6.Transaction = sqlTrn;
						sqlCommand6.CommandType = CommandType.StoredProcedure;
						sqlCommand6.CommandText = "usp_getBalNoOfContainersByContractId";
						sqlCommand6.Parameters.AddWithValue("@ContractId", item.ExImLcOrderId);
						sqlCommand6.ExecuteNonQuery();
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

	public List<Architecture.Model.Export.ExImForwarding> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.ExImForwarding> lst = new List<Architecture.Model.Export.ExImForwarding>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetAllDetail<Architecture.Model.Export.ExImForwarding>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExImForwarding item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadByForwardingHeaderId"));
						item2.ExImForwardingDetail = GenericProvider.Get<ExImForwardingPackingDetail>(sqlConnection2, "Sp_ExImForwarding_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Export.ExImForwarding item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadOtherItemsByHeaderId"));
					item.ExImForwardingOtherItemslist = GenericProvider.Get<ExImForwardingOtherItems>(sqlcon, "Sp_ExImForwarding_GetAllMethod", lstPrm);
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
