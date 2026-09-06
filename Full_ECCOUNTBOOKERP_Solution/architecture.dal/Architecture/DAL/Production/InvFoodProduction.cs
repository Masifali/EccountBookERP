using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.Production;

namespace Architecture.DAL.Production;

public class InvFoodProduction
{
	public int SetData(Architecture.Model.Production.InvFoodProduction obj, string ProcName)
	{
		int returnID = 0;
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
		List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
		List<GetJobLotGlIdsandName> lstJobLot = new List<GetJobLotGlIdsandName>();
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			bool shouldNotStockUpdate = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "JobOrderCreatewithoutRates")) && obj.DocumentTypeId == 112;
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			int LineIdForPDI = 0;
			bool loaderflag = false;
			foreach (InvFoodProductionDetail item10 in obj.invFoodProductionDetails)
			{
				if (item10.RefDocumentTypeId > 0 && item10.RefDocNoId > 0 && item10.RefDocSubIdNo > 0)
				{
					loaderflag = true;
				}
				LineIdForPDI = (item10.LineId = LineIdForPDI + 1);
				item10.InvFoodProductionId = obj.Id;
				int code2 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item10, "Sp_InvFoodProductionDetail_Insert"));
				item10.Id = code2;
			}
			int LineIdCons = 0;
			bool loaderflagConsmp = false;
			foreach (InvFoodProductionConsumptionDetail item9 in obj.InvFoodProductionConsumptionDetailslist)
			{
				if (item9.RefDocumentTypeId > 0 && item9.RefDocNoId > 0 && item9.RefDocSubIdNo > 0)
				{
					loaderflagConsmp = true;
				}
				LineIdCons = (item9.LineId = LineIdCons + 1);
				item9.InvFoodProductionId = obj.Id;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item9, "USP_InvFoodProductionConsumptionDetail_Insert"));
				item9.Id = code;
			}
			if (obj.DocumentTypeId == 80)
			{
				if (CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5) && !loaderflag)
				{
					string RemarksCgs2 = "";
					dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId);
					lstJobLot = CommonServices.GetJobLotGlIdsandName(obj.OrganizationId, obj.CompanyId);
					obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
					foreach (InvFoodProductionDetail item8 in obj.invFoodProductionDetails.ToList())
					{
						List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item8.ItemId)).ToList();
						if (find2.Count <= 0)
						{
							continue;
						}
						ReportsParameters stock2 = new ReportsParameters();
						stock2.OrganizationId = obj.OrganizationId;
						stock2.CompanyId = obj.CompanyId;
						stock2.ItemId = item8.ItemId;
						stock2.WarehouseId = item8.WarehouseId;
						stock2.stockUOM = item8.ItemUomId;
						stock2.DocDate = obj.DocDate;
						stock2.JobLotId = item8.JobLotId;
						stock2.CropYear = item8.CropBatch;
						stock2.PackingTypeId = item8.PackingtypeId;
						if (obj.ModifyUser > 0)
						{
							stock2.DocumentTypeId = obj.DocumentTypeId;
							stock2.Id = obj.Id;
						}
						stock2.ItemQty = item8.Qty;
						stock2.NetWeight = item8.Weight;
						stock2.RateEqvailent = item8.RateEquivalent;
						stock2.RateUOM = item8.RateUOMId;
						stock2.LineId = item8.LineId;
						List<GetJobLotGlIdsandName> findJob2 = lstJobLot.Where((GetJobLotGlIdsandName x) => x.Id == Conversion.ToInt(item8.JobLotId)).ToList();
						bool JobLotFlag2 = false;
						if (findJob2 != null && findJob2.Count > 0 && findJob2 != null && findJob2[0].AccountId > 0)
						{
							JobLotFlag2 = true;
						}
						foreach (InventoryStockEvalautionDetail stockdetail2 in CommonServices.FIFOImplemention(stock2, obj.InventoryStockEvalautionDetailslist).InventoryStockEvalautionDetailslist.ToList())
						{
							if (!JobLotFlag2)
							{
								RemarksCgs2 = "ItemQty: " + stockdetail2.QtyOut + "   " + Conversion.ToString(find2[0].ItemName) + "  Net Weight: " + stockdetail2.StockWeightOut + "  CGS Rate:" + stockdetail2.CgsRate;
								VoucherDetail vd9 = new VoucherDetail();
								vd9.LineId = item8.LineId;
								vd9.AccountId = Conversion.ToInt(obj.WIPAccountId);
								vd9.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
								vd9.Comments = Conversion.ToString(RemarksCgs2);
								vd9.DebitAmount = Conversion.ToDouble(stockdetail2.CgsAmount);
								vd9.DocumentTypeIdRef = Conversion.ToInt(item8.RefDocumentTypeId);
								vd9.InvoiceNoRefId = Conversion.ToInt(item8.RefDocNoId);
								vd9.RefInvoiceNo = Conversion.ToString(item8.RefDocSubIdNo);
								vd9.ItemId = Conversion.ToInt(item8.ItemId);
								vd9.QtyOut = Conversion.ToDouble(stockdetail2.QtyOut);
								vd9.WeightOut = Conversion.ToDouble(stockdetail2.StockWeightOut);
								vd9.ItemCgsRate = Conversion.ToDouble(stockdetail2.CgsRate);
								vd9.RateCut = Conversion.ToDouble(0);
								vd9.RateCutAmount = Conversion.ToDouble(0);
								vd9.ItemAmount = Conversion.ToDouble(stockdetail2.CgsAmount);
								vd9.JobLotId = Conversion.ToInt(item8.JobLotId);
								vd9.BranchesId = obj.BranchId;
								vd9.OrderNo = item8.InvProductionJobOrderId;
								obj.VoucherHeadInvoices.voucherDetailList.Add(vd9);
								VoucherDetail vd7 = new VoucherDetail();
								vd7.LineId = item8.LineId;
								vd7.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
								vd7.AgainstAccountId = Conversion.ToInt(obj.WIPAccountId);
								vd7.Comments = Conversion.ToString(RemarksCgs2);
								vd7.CreditAmount = Conversion.ToDouble(stockdetail2.CgsAmount);
								vd7.DocumentTypeIdRef = Conversion.ToInt(item8.RefDocumentTypeId);
								vd7.InvoiceNoRefId = Conversion.ToInt(item8.RefDocNoId);
								vd7.RefInvoiceNo = Conversion.ToString(item8.RefDocSubIdNo);
								vd7.ItemId = Conversion.ToInt(item8.ItemId);
								vd7.QtyOut = Conversion.ToDouble(stockdetail2.QtyOut);
								vd7.WeightOut = Conversion.ToDouble(stockdetail2.StockWeightOut);
								vd7.ItemCgsRate = Conversion.ToDouble(stockdetail2.CgsRate);
								vd7.RateCut = Conversion.ToDouble(0);
								vd7.RateCutAmount = Conversion.ToDouble(0);
								vd7.ItemAmount = Conversion.ToDouble(stockdetail2.CgsAmount);
								vd7.JobLotId = Conversion.ToInt(item8.JobLotId);
								vd7.BranchesId = obj.BranchId;
								vd7.OrderNo = item8.InvProductionJobOrderId;
								obj.VoucherHeadInvoices.voucherDetailList.Add(vd7);
							}
							obj.InventoryStockEvalautionDetailslist.Add(stockdetail2);
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
						foreach (InventoryStockEvalautionDetail item7 in obj.InventoryStockEvalautionDetailslist)
						{
							item7.OrganizationId = obj.OrganizationId;
							item7.CompanyId = obj.CompanyId;
							item7.DocDate = obj.DocDate;
							item7.DocCodeNo = obj.DocCode;
							item7.SupplierCustomerId = 0;
							item7.BranchesId = obj.BranchId;
							item7.RefDocumentTypeId = obj.DocumentTypeId;
							item7.EntryUser = obj.EntryUser;
							item7.ModifyUser = obj.ModifyUser;
							item7.CalcType = "Weight";
							InvFoodProductionDetail sid3 = obj.invFoodProductionDetails.Find((InvFoodProductionDetail x) => x.LineId == item7.LineId && x.LineId > 0 && item7.LineId > 0);
							if (sid3 != null)
							{
								double RateUom2 = CommonServices.GetEqvilentByItemIdAndUomScheduleId(new ReportsParameters
								{
									OrganizationId = obj.OrganizationId,
									CompanyId = obj.CompanyId,
									ItemId = item7.ItemId,
									Id = sid3.RateUOMId
								});
								if (item7.BillWeightOut > 0.0 && sid3.Rate > 0.0 && RateUom2 > 0.0)
								{
									item7.AmountOut = item7.BillWeightOut / RateUom2 * sid3.Rate;
								}
								item7.ItemRate = sid3.Rate;
								item7.RefDocIdNo = sid3.InvFoodProductionId;
								item7.RefDocSubIdNo = sid3.Id;
							}
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "USP_InventoryStockEvalautionDetail_Insert"));
						}
					}
				}
				else if (obj.ActionId == 1)
				{
					InventoryStockEvalautionDetail inventoryTransactions2 = new InventoryStockEvalautionDetail();
					inventoryTransactions2.OrganizationId = obj.OrganizationId;
					inventoryTransactions2.CompanyId = obj.CompanyId;
					inventoryTransactions2.RefDocumentTypeId = obj.DocumentTypeId;
					inventoryTransactions2.RefDocIdNo = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions2, "Sp_InventoryStockEvalautionDetail_Update"));
				}
			}
			if (obj.DocumentTypeId == 181)
			{
				if (CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5) && !loaderflagConsmp)
				{
					string RemarksCgs = "";
					dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId);
					lstJobLot = CommonServices.GetJobLotGlIdsandName(obj.OrganizationId, obj.CompanyId);
					obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
					foreach (InvFoodProductionConsumptionDetail item6 in obj.InvFoodProductionConsumptionDetailslist.ToList())
					{
						List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item6.ItemId)).ToList();
						if (find.Count <= 0)
						{
							continue;
						}
						ReportsParameters stock = new ReportsParameters();
						stock.OrganizationId = obj.OrganizationId;
						stock.CompanyId = obj.CompanyId;
						stock.ItemId = item6.ItemId;
						stock.WarehouseId = item6.WarehouseId;
						stock.stockUOM = item6.ItemUomId;
						stock.DocDate = obj.DocDate;
						stock.JobLotId = item6.JobLotId;
						stock.CropYear = item6.CropBatch;
						stock.PackingTypeId = item6.PackingtypeId;
						if (obj.ModifyUser > 0)
						{
							stock.DocumentTypeId = obj.DocumentTypeId;
							stock.Id = obj.Id;
						}
						stock.ItemQty = item6.Qty;
						stock.NetWeight = item6.Weight;
						stock.RateEqvailent = item6.RateEquivalent;
						stock.RateUOM = item6.RateUOMId;
						stock.LineId = item6.LineId;
						List<GetJobLotGlIdsandName> findJob = lstJobLot.Where((GetJobLotGlIdsandName x) => x.Id == Conversion.ToInt(item6.JobLotId)).ToList();
						bool JobLotFlag = false;
						if (findJob != null && findJob.Count > 0 && findJob != null && findJob[0].AccountId > 0)
						{
							JobLotFlag = true;
						}
						foreach (InventoryStockEvalautionDetail stockdetail in CommonServices.FIFOImplemention(stock, obj.InventoryStockEvalautionDetailslist).InventoryStockEvalautionDetailslist.ToList())
						{
							if (!JobLotFlag)
							{
								RemarksCgs = "ItemQty: " + stockdetail.QtyOut + "   " + Conversion.ToString(find[0].ItemName) + "  Net Weight: " + stockdetail.StockWeightOut + "  CGS Rate:" + stockdetail.CgsRate;
								VoucherDetail vd8 = new VoucherDetail();
								vd8.LineId = item6.LineId;
								vd8.AccountId = Conversion.ToInt(obj.WIPAccountId);
								vd8.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
								vd8.Comments = Conversion.ToString(RemarksCgs);
								vd8.DebitAmount = Conversion.ToDouble(stockdetail.CgsAmount);
								vd8.DocumentTypeIdRef = Conversion.ToInt(item6.RefDocumentTypeId);
								vd8.InvoiceNoRefId = Conversion.ToInt(item6.RefDocNoId);
								vd8.RefInvoiceNo = Conversion.ToString(item6.RefDocSubIdNo);
								vd8.ItemId = Conversion.ToInt(item6.ItemId);
								vd8.QtyOut = Conversion.ToDouble(stockdetail.QtyOut);
								vd8.WeightOut = Conversion.ToDouble(stockdetail.StockWeightOut);
								vd8.ItemCgsRate = Conversion.ToDouble(stockdetail.CgsRate);
								vd8.RateCut = Conversion.ToDouble(0);
								vd8.RateCutAmount = Conversion.ToDouble(0);
								vd8.ItemAmount = Conversion.ToDouble(stockdetail.CgsAmount);
								vd8.JobLotId = Conversion.ToInt(item6.JobLotId);
								vd8.BranchesId = obj.BranchId;
								vd8.OrderNo = item6.InvProductionJobOrderId;
								obj.VoucherHeadInvoices.voucherDetailList.Add(vd8);
								VoucherDetail vd6 = new VoucherDetail();
								vd6.LineId = item6.LineId;
								vd6.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
								vd6.AgainstAccountId = Conversion.ToInt(obj.WIPAccountId);
								vd6.Comments = Conversion.ToString(RemarksCgs);
								vd6.CreditAmount = Conversion.ToDouble(stockdetail.CgsAmount);
								vd6.DocumentTypeIdRef = Conversion.ToInt(item6.RefDocumentTypeId);
								vd6.InvoiceNoRefId = Conversion.ToInt(item6.RefDocNoId);
								vd6.RefInvoiceNo = Conversion.ToString(item6.RefDocSubIdNo);
								vd6.ItemId = Conversion.ToInt(item6.ItemId);
								vd6.QtyOut = Conversion.ToDouble(stockdetail.QtyOut);
								vd6.WeightOut = Conversion.ToDouble(stockdetail.StockWeightOut);
								vd6.ItemCgsRate = Conversion.ToDouble(stockdetail.CgsRate);
								vd6.RateCut = Conversion.ToDouble(0);
								vd6.RateCutAmount = Conversion.ToDouble(0);
								vd6.ItemAmount = Conversion.ToDouble(stockdetail.CgsAmount);
								vd6.JobLotId = Conversion.ToInt(item6.JobLotId);
								vd6.BranchesId = obj.BranchId;
								vd6.OrderNo = item6.InvProductionJobOrderId;
								obj.VoucherHeadInvoices.voucherDetailList.Add(vd6);
							}
							obj.InventoryStockEvalautionDetailslist.Add(stockdetail);
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
						foreach (InventoryStockEvalautionDetail item5 in obj.InventoryStockEvalautionDetailslist)
						{
							item5.OrganizationId = obj.OrganizationId;
							item5.CompanyId = obj.CompanyId;
							item5.DocDate = obj.DocDate;
							item5.DocCodeNo = obj.DocCode;
							item5.SupplierCustomerId = 0;
							item5.BranchesId = obj.BranchId;
							item5.RefDocumentTypeId = obj.DocumentTypeId;
							item5.EntryUser = obj.EntryUser;
							item5.ModifyUser = obj.ModifyUser;
							item5.CalcType = "Weight";
							InvFoodProductionConsumptionDetail sid2 = obj.InvFoodProductionConsumptionDetailslist.Find((InvFoodProductionConsumptionDetail x) => x.LineId == item5.LineId && x.LineId > 0 && item5.LineId > 0);
							if (sid2 != null)
							{
								double RateUom = CommonServices.GetEqvilentByItemIdAndUomScheduleId(new ReportsParameters
								{
									OrganizationId = obj.OrganizationId,
									CompanyId = obj.CompanyId,
									ItemId = item5.ItemId,
									Id = sid2.RateUOMId
								});
								if (item5.BillWeightOut > 0.0 && sid2.Rate > 0.0 && RateUom > 0.0)
								{
									item5.AmountOut = item5.BillWeightOut / RateUom * sid2.Rate;
								}
								item5.ItemRate = sid2.Rate;
								item5.RefDocIdNo = sid2.InvFoodProductionId;
								item5.RefDocSubIdNo = sid2.Id;
							}
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "USP_InventoryStockEvalautionDetail_Insert"));
						}
					}
				}
				else if (obj.ActionId == 1)
				{
					InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
					inventoryTransactions.OrganizationId = obj.OrganizationId;
					inventoryTransactions.CompanyId = obj.CompanyId;
					inventoryTransactions.RefDocumentTypeId = obj.DocumentTypeId;
					inventoryTransactions.RefDocIdNo = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
				}
			}
			if (!shouldNotStockUpdate)
			{
				InventoryTransactions transactions = new InventoryTransactions();
				transactions.OrganizationId = obj.OrganizationId;
				transactions.CompanyId = obj.CompanyId;
				transactions.RefDocumentTypeId = obj.DocumentTypeId;
				transactions.RefDocIdNo = returnID;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			}
			if (obj.DocumentTypeId == 80)
			{
				if (obj.InputDetailRowsRemoveIds != null && obj.InputDetailRowsRemoveIds != string.Empty)
				{
					SqlCommand sqlCommand3 = new SqlCommand();
					sqlCommand3.Connection = sqlcon;
					sqlCommand3.Transaction = sqlTrn;
					sqlCommand3.CommandType = CommandType.StoredProcedure;
					sqlCommand3.CommandText = "USP_InvFoodProductionDetailRowsDeleteByIds";
					sqlCommand3.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand3.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand3.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
					sqlCommand3.Parameters.AddWithValue("@Id", obj.Id);
					sqlCommand3.Parameters.AddWithValue("@DetailIds", obj.InputDetailRowsRemoveIds);
					sqlCommand3.ExecuteNonQuery();
				}
				foreach (InvFoodProductionDetail item3 in obj.invFoodProductionDetails)
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
					sqlCommand4.Parameters.AddWithValue("@ItemId", item3.ItemId);
					sqlCommand4.Parameters.AddWithValue("@WarehouseId", item3.WarehouseId);
					sqlCommand4.Parameters.AddWithValue("@JobLotId", item3.JobLotId);
					sqlCommand4.Parameters.AddWithValue("@CropYear", item3.CropBatch);
					sqlCommand4.Parameters.AddWithValue("@InvPackingTypeId", item3.PackingtypeId);
					sqlCommand4.Parameters.AddWithValue("@PackUomId", item3.ItemUomId);
					sqlCommand4.Parameters.AddWithValue("@RefDocumentTypeId", item3.RefDocumentTypeId);
					sqlCommand4.Parameters.AddWithValue("@RefDocNoId", item3.RefDocNoId);
					sqlCommand4.Parameters.AddWithValue("@RefDocSubIdNo", item3.RefDocSubIdNo);
					sqlCommand4.Parameters.AddWithValue("@NetWeight", item3.Weight);
					sqlCommand4.ExecuteScalar();
				}
			}
			if ((obj.DocumentTypeId == 80 && obj.ActionId == 2) || (obj.DocumentTypeId == 181 && obj.ActionId == 2) || obj.DocumentTypeId == 112)
			{
				SqlCommand sqlCommand5 = new SqlCommand();
				sqlCommand5.Connection = sqlcon;
				sqlCommand5.Transaction = sqlTrn;
				sqlCommand5.CommandType = CommandType.StoredProcedure;
				sqlCommand5.CommandText = "USP_ProductionInPutAndOutPutWeightValidation";
				sqlCommand5.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand5.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand5.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
				sqlCommand5.Parameters.AddWithValue("@InvProductionJobOrderId", obj.InvJobOrderId);
				sqlCommand5.Parameters.AddWithValue("@PlantId", obj.PlantId);
				sqlCommand5.ExecuteNonQuery();
			}
			if (obj.DocumentTypeId == 181)
			{
				foreach (InvFoodProductionConsumptionDetail item2 in obj.InvFoodProductionConsumptionDetailslist)
				{
					SqlCommand sqlCommand6 = new SqlCommand();
					sqlCommand6.Connection = sqlcon;
					sqlCommand6.Transaction = sqlTrn;
					sqlCommand6.CommandType = CommandType.StoredProcedure;
					sqlCommand6.CommandText = "USP_InventoryValidation";
					sqlCommand6.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand6.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand6.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
					sqlCommand6.Parameters.AddWithValue("@DocDate", obj.DocDate);
					sqlCommand6.Parameters.AddWithValue("@ItemId", item2.ItemId);
					sqlCommand6.Parameters.AddWithValue("@WarehouseId", item2.WarehouseId);
					sqlCommand6.Parameters.AddWithValue("@JobLotId", item2.JobLotId);
					sqlCommand6.Parameters.AddWithValue("@CropYear", item2.CropBatch);
					sqlCommand6.Parameters.AddWithValue("@InvPackingTypeId", item2.PackingtypeId);
					sqlCommand6.Parameters.AddWithValue("@PackUomId", item2.ItemUomId);
					sqlCommand6.Parameters.AddWithValue("@RefDocumentTypeId", item2.RefDocumentTypeId);
					sqlCommand6.Parameters.AddWithValue("@RefDocNoId", item2.RefDocNoId);
					sqlCommand6.Parameters.AddWithValue("@RefDocSubIdNo", item2.RefDocSubIdNo);
					sqlCommand6.Parameters.AddWithValue("@NetWeight", item2.Weight);
					sqlCommand6.ExecuteNonQuery();
				}
			}
			if ((obj.DocumentTypeId == 112 || obj.DocumentTypeId == 181) && obj.OutputDetailRowsRemoveIds != null && obj.OutputDetailRowsRemoveIds != string.Empty)
			{
				SqlCommand sqlCommand7 = new SqlCommand();
				sqlCommand7.Connection = sqlcon;
				sqlCommand7.Transaction = sqlTrn;
				sqlCommand7.CommandType = CommandType.StoredProcedure;
				sqlCommand7.CommandText = "USP_InvFoodProductionDetailRowsDeleteByIds";
				sqlCommand7.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand7.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand7.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
				sqlCommand7.Parameters.AddWithValue("@Id", obj.Id);
				sqlCommand7.Parameters.AddWithValue("@DetailIds", obj.OutputDetailRowsRemoveIds);
				sqlCommand7.ExecuteNonQuery();
			}
			if (obj.DocumentTypeId == 80 || obj.DocumentTypeId == 112)
			{
				foreach (InvFoodProductionDetail item in obj.invFoodProductionDetails)
				{
					SqlCommand sqlCommand8 = new SqlCommand();
					sqlCommand8.Connection = sqlcon;
					sqlCommand8.Transaction = sqlTrn;
					sqlCommand8.CommandType = CommandType.StoredProcedure;
					sqlCommand8.CommandText = "usp_ProductionWeightCompareFromJobOrderWeight";
					sqlCommand8.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
					sqlCommand8.Parameters.AddWithValue("@JobOrderId", obj.InvJobOrderId);
					sqlCommand8.Parameters.AddWithValue("@RefId", (obj.DocumentTypeId == 80) ? item.JobOrderInPutId : item.OutPutIdFromJobOrder);
					sqlCommand8.Parameters.AddWithValue("@NetWeight", item.Weight);
					sqlCommand8.ExecuteNonQuery();
				}
			}
			if (obj.RefDocumentTypeId != 1)
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
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_Insert"));
				}
				else
				{
					obj.VoucherHeadInvoices.DocumentTypeSrNo = obj.Id;
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
				if (obj.VoucherHeadInvoices.voucherDetailList.Count > 0 && obj.VoucherHeadInvoices.voucherDetailList != null)
				{
					foreach (VoucherDetail item4 in obj.VoucherHeadInvoices.voucherDetailList)
					{
						item4.VoucherHeadId = obj.VoucherHeadInvoices.Id;
						item4.BranchesId = obj.BranchId;
						InvFoodProductionDetail sid = obj.invFoodProductionDetails.Find((InvFoodProductionDetail x) => x.LineId == item4.LineId && x.LineId > 0 && item4.LineId > 0);
						if (sid != null)
						{
							item4.RefDocSubIdNo = sid.Id;
						}
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Sp_VoucherDetail_Insert"));
					}
					if (obj.VoucherHeadInvoices.Id > 0)
					{
						SqlCommand sqlCommand9 = new SqlCommand();
						sqlCommand9.Connection = sqlcon;
						sqlCommand9.Transaction = sqlTrn;
						sqlCommand9.CommandType = CommandType.StoredProcedure;
						sqlCommand9.CommandText = "USP_VoucherBalanceCheck";
						sqlCommand9.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
						sqlCommand9.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
						sqlCommand9.Parameters.AddWithValue("@Id", obj.VoucherHeadInvoices.Id);
						sqlCommand9.ExecuteScalar();
					}
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

	public List<Architecture.Model.Production.InvFoodProduction> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Production.InvFoodProduction> lst = new List<Architecture.Model.Production.InvFoodProduction>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetAllDetail<Architecture.Model.Production.InvFoodProduction>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Production.InvFoodProduction item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "InvFoodProductionId"));
						item2.invFoodProductionDetails = GenericProvider.Get<InvFoodProductionDetail>(sqlConnection2, "Sp_InvFoodProductionDetail_GetALlMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Production.InvFoodProduction item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "InvFoodProductionId"));
					item.InvFoodProductionConsumptionDetailslist = GenericProvider.Get<InvFoodProductionConsumptionDetail>(sqlcon, "USP_InvFoodProductionConsumptionDetail_GetALlMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public int SetDataForUpdate(Architecture.Model.Production.InvFoodProduction obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "[dbo].[InvProductionJobOrder_UpdateIsSettled]";
			sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
			sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
			sqlCommand.Parameters.AddWithValue("@InvJobOrderId", obj.InvJobOrderId);
			sqlCommand.ExecuteNonQuery();
			SqlCommand sqlCommand2 = new SqlCommand();
			sqlCommand2.Connection = sqlcon;
			sqlCommand2.Transaction = sqlTrn;
			sqlCommand2.CommandType = CommandType.StoredProcedure;
			sqlCommand2.CommandText = "[dbo].[usp_SettlementDeleteByJobOrderId]";
			sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
			sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
			sqlCommand2.Parameters.AddWithValue("@InvJobOrderId", obj.InvJobOrderId);
			sqlCommand2.ExecuteNonQuery();
			int VoucherId = 0;
			obj.invFoodProductionDetails = obj.invFoodProductionDetails.OrderBy((InvFoodProductionDetail x) => x.InvFoodProductionId).ToList();
			foreach (InvFoodProductionDetail item in obj.invFoodProductionDetails)
			{
				if (Conversion.ToInt(item.InvFoodProductionId) != VoucherId)
				{
					item.DeleteFlag = 1;
				}
				else
				{
					item.DeleteFlag = 0;
				}
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
				VoucherId = item.InvFoodProductionId;
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

	public int SetDataForINPutUpdate(Architecture.Model.Production.InvFoodProduction obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int VoucherId = 0;
			obj.invFoodProductionDetails = obj.invFoodProductionDetails.OrderBy((InvFoodProductionDetail x) => x.InvFoodProductionId).ToList();
			foreach (InvFoodProductionDetail item in obj.invFoodProductionDetails)
			{
				if (Conversion.ToInt(item.InvFoodProductionId) != VoucherId)
				{
					item.DeleteFlag = 1;
				}
				else
				{
					item.DeleteFlag = 0;
				}
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
				VoucherId = item.InvFoodProductionId;
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

	public int SetDataForSettlementFinancials(VoucherHead obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = returnID;
			}
			else
			{
				returnID = obj.Id;
			}
			if (obj.voucherDetailList.Count > 0 && obj.voucherDetailList != null)
			{
				foreach (VoucherDetail item in obj.voucherDetailList)
				{
					item.VoucherHeadId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_Insert"));
				}
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "USP_VoucherBalanceCheck";
				sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand.Parameters.AddWithValue("@Id", obj.Id);
				sqlCommand.ExecuteScalar();
				SqlCommand sqlCommand2 = new SqlCommand();
				sqlCommand2.Connection = sqlcon;
				sqlCommand2.Transaction = sqlTrn;
				sqlCommand2.CommandType = CommandType.StoredProcedure;
				sqlCommand2.CommandText = "[dbo].[InvProductionJobOrder_UpdateIsSettled]";
				sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand2.Parameters.AddWithValue("@InvJobOrderId", obj.voucherDetailList[0].OrderNo);
				sqlCommand2.ExecuteScalar();
				sqlTrn.Commit();
				return returnID;
			}
			throw new Exception("Voucher Detail Not Found");
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}
}
