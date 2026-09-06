using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.TaxProject.Common;
using Architecture.Model;
using Architecture.Model.TaxProject.Accounts;
using Architecture.Model.TaxProject.Inventory;
using Architecture.Model.VPS;

namespace Architecture.DAL.TaxProject.Inventory;

public class InvSaleInvoice
{
	public int SetData(Architecture.Model.TaxProject.Inventory.InvSaleInvoice obj, string ProcName)
	{
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
		new List<SupplierCustomer>();
		List<GetJobLotGlIdsandName> lstJobLot = new List<GetJobLotGlIdsandName>();
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(Architecture.DAL.TaxProject.Common.CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		using SqlConnection sqlcon = ConnectionObject.SQLForServer();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.InvSaleInvoiceDetaillist == null || obj.InvSaleInvoiceDetaillist.Count == 0)
			{
				throw new Exception("Detail List not found");
			}
			if (IsVpsAttachmentsServiceOn)
			{
				Sc = Architecture.DAL.Common.CommonServices.GetServerCredentials();
			}
			else
			{
				AttachmentFolderPath = Architecture.DAL.TaxProject.Common.CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Attachment Folder Path");
				if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0 && !Directory.Exists(AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				obj.AttachmentsList = Architecture.DAL.Common.CommonServices.CommonMethodForAttachmentsToPhysicallySave(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.AttachmentsList, copiedFiles);
				obj.AttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.Attachment));
				obj.CustomAttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.FileCustomName));
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
			bool loaderflag = false;
			foreach (InvSaleInvoiceDetail item16 in obj.InvSaleInvoiceDetaillist)
			{
				if (item16.RefRefDocumentTypeId > 0 && item16.RefRefDocIdNo > 0 && item16.RefDocSubId > 0)
				{
					loaderflag = true;
				}
				LineIdForPDI = (item16.LineId = LineIdForPDI + 1);
				item16.InvSaleInvoiceId = obj.Id;
				item16.BillAmount = item16.ItemAmount + item16.TaxAmount + item16.ExpenseAmount - item16.CommissionAmount - item16.FreightAmount;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item16, "Sp_InvSaleInvoiceDetail_Insert"));
				item16.Id = code;
			}
			if (obj.InvSaleInvoiceFreightList != null && obj.InvSaleInvoiceFreightList.Count > 0)
			{
				foreach (InvSaleInvoiceFreight item15 in obj.InvSaleInvoiceFreightList)
				{
					item15.InvSaleInvoiceId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item15, "Sp_InvSaleInvoiceFreight_Insert"));
				}
			}
			if (obj.InvSaleInvoiceJournalList != null && obj.InvSaleInvoiceJournalList.Count > 0)
			{
				foreach (InvSaleInvoiceJournal item14 in obj.InvSaleInvoiceJournalList)
				{
					item14.InvSaleInvoiceId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item14, "Sp_InvSaleInvoiceJournal_Insert"));
				}
			}
			if (obj.InvSaleInvoiceExpenseList != null && obj.InvSaleInvoiceExpenseList.Count > 0)
			{
				foreach (InvSaleInvoiceExpense item13 in obj.InvSaleInvoiceExpenseList)
				{
					item13.InvSaleInvoiceId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item13, "Sp_InvSaleInvoiceExpense_Insert"));
				}
			}
			if (obj.InvSaleInvoiceCommission != null && obj.InvSaleInvoiceCommission.Count > 0)
			{
				foreach (InvSaleInvoiceCommission item12 in obj.InvSaleInvoiceCommission)
				{
					item12.InvSaleInvoiceId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item12, "Sp_InvSaleInvoiceCommission_Insert"));
				}
			}
			if (obj.SaleInvoicePaymentTermsDetailslist != null && obj.SaleInvoicePaymentTermsDetailslist.Count > 0)
			{
				int sortno = 1;
				foreach (SaleInvoicePaymentTermsDetail item11 in obj.SaleInvoicePaymentTermsDetailslist)
				{
					item11.InvSaleInvoiceId = obj.Id;
					item11.SortNo = sortno;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item11, "USP_SaleInvoicePaymentTermsDetail_Insert"));
					sortno++;
				}
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
				foreach (DMSAttachments item10 in obj.AttachmentsList.ToList())
				{
					item10.RefAccountId = obj.SupplierCustomerId;
					item10.RefDocumentTypeId = obj.DocumentTypeId;
					item10.RefDocumentNo = returnID;
					item10.EntryDate = DateTime.Now;
					item10.ModifyDate = DateTime.Now;
					item10.EntryUser = obj.EntryUser;
					item10.ModifyUser = obj.ModifyUser;
					item10.OrganizationId = obj.OrganizationId;
					item10.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item10, "Proc_DMSAttachments_Insert"));
				}
			}
			List<GetItemGlIdsandItemName> dtitem = (dtitem = Architecture.DAL.TaxProject.Common.CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId));
			bool eRPFeaturesByCompanyId = Architecture.DAL.TaxProject.Common.CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5);
			bool SaleMinusAllowedAgainstFifo = Architecture.DAL.TaxProject.Common.CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 14);
			bool FifoEntryAllow = true;
			FifoEntryAllow = !(obj.DocumentTypeId == 99 && SaleMinusAllowedAgainstFifo);
			if (eRPFeaturesByCompanyId && !loaderflag && obj.DocumentTypeId != 1611 && obj.DocumentTypeId != 1662)
			{
				if (obj.DocumentTypeId == 95 || obj.DocumentTypeId == 171)
				{
					int RefDocumentTypeId2 = ((obj.DocumentTypeId == 171) ? 221 : 86);
					if (obj.DocumentTypeId == 171 && obj.InvSaleInvoiceDetaillist.Exists((InvSaleInvoiceDetail p) => p.SaleOrderId > 0))
					{
						RefDocumentTypeId2 = 170;
					}
					obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
					lstJobLot = Architecture.DAL.TaxProject.Common.CommonServices.GetJobLotGlIdsandName(obj.OrganizationId, obj.CompanyId);
					foreach (InvSaleInvoiceDetail item7 in obj.InvSaleInvoiceDetaillist)
					{
						List<InventoryStockEvalautionDetail> lstOtherRef = new List<InventoryStockEvalautionDetail>();
						lstOtherRef = Architecture.DAL.TaxProject.Common.CommonServices.GetInventoryStockEvalautionDetailByOtherIds(new ReportsParameters
						{
							OrganizationId = obj.OrganizationId,
							CompanyId = obj.CompanyId,
							OtherDocumentTypeId = RefDocumentTypeId2,
							OtherDocNoId = item7.InvGdnId,
							OtherSubDocNoId = item7.InvGdnDetailId
						});
						if (lstOtherRef != null && lstOtherRef.Count > 0)
						{
							List<GetJobLotGlIdsandName> findJob = lstJobLot.Where((GetJobLotGlIdsandName x) => x.Id == Conversion.ToInt(item7.JobLotId)).ToList();
							bool JobLotFlag = false;
							if (findJob != null && findJob.Count > 0 && findJob != null && findJob[0].AccountId > 0)
							{
								JobLotFlag = true;
							}
							string RemarksCgs = "";
							foreach (InventoryStockEvalautionDetail stockdetail2 in lstOtherRef.ToList())
							{
								List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item7.ItemId)).ToList();
								if (find.Count == 0)
								{
									throw new Exception("ItemId Not Found Against CGS Transaction");
								}
								if (!JobLotFlag)
								{
									RemarksCgs = "ItemQty: " + stockdetail2.QtyOut + "   " + Conversion.ToString(find[0].ItemName) + "  Net Weight: " + stockdetail2.StockWeightOut + "  CGS Rate:" + stockdetail2.CgsRate;
									VoucherDetail vd8 = new VoucherDetail();
									vd8.LineId = item7.LineId;
									vd8.IsCGS = 1;
									vd8.AccountId = Conversion.ToInt(find[0].COGSGLAC);
									vd8.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
									vd8.Comments = Conversion.ToString(RemarksCgs + "  " + obj.CommonIdsForFinancials.CompanyName);
									vd8.DebitAmount = Conversion.ToDouble(stockdetail2.CgsAmount);
									vd8.DocumentTypeIdRef = Conversion.ToInt(item7.RefRefDocumentTypeId);
									vd8.InvoiceNoRefId = Conversion.ToInt(item7.RefRefDocIdNo);
									vd8.RefInvoiceNo = Conversion.ToString(item7.RefDocSubId);
									vd8.ItemId = Conversion.ToInt(item7.ItemId);
									vd8.QtyOut = Conversion.ToDouble(stockdetail2.QtyOut);
									vd8.WeightOut = Conversion.ToDouble(stockdetail2.StockWeightOut);
									vd8.ItemCgsRate = Conversion.ToDouble(stockdetail2.CgsRate);
									vd8.RateCut = Conversion.ToDouble(item7.RateCut);
									vd8.RateCutAmount = Conversion.ToDouble(item7.RateCutAmount);
									vd8.ItemAmount = Conversion.ToDouble(stockdetail2.AmountOut);
									vd8.Expenses = Conversion.ToDouble(item7.ExpenseAmount);
									vd8.Commission = Conversion.ToDouble(item7.CommissionAmount);
									vd8.Freight = Conversion.ToDouble(item7.FreightAmount);
									vd8.OrderNo = Conversion.ToInt(item7.SaleOrder);
									vd8.GpNo = Conversion.ToInt(item7.GpNo);
									vd8.VehicleNo = item7.VehicleNo;
									vd8.JobLotId = Conversion.ToInt(item7.JobLotId);
									vd8.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
									vd8.BranchesId = item7.BranchId;
									vd8.CostCenterId = item7.CostCenterId;
									obj.VoucherHeadInvoices.voucherDetailList.Add(vd8);
									VoucherDetail vd6 = new VoucherDetail();
									vd6.LineId = item7.LineId;
									vd6.IsCGS = 1;
									vd6.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
									vd6.AgainstAccountId = Conversion.ToInt(find[0].COGSGLAC);
									vd6.Comments = Conversion.ToString(RemarksCgs + "  " + obj.CommonIdsForFinancials.CompanyName);
									vd6.CreditAmount = Conversion.ToDouble(stockdetail2.CgsAmount);
									vd6.DocumentTypeIdRef = Conversion.ToInt(item7.RefRefDocumentTypeId);
									vd6.InvoiceNoRefId = Conversion.ToInt(item7.RefRefDocIdNo);
									vd6.RefInvoiceNo = Conversion.ToString(item7.RefDocSubId);
									vd6.ItemId = Conversion.ToInt(item7.ItemId);
									vd6.QtyOut = Conversion.ToDouble(stockdetail2.QtyOut);
									vd6.WeightOut = Conversion.ToDouble(stockdetail2.StockWeightOut);
									vd6.ItemCgsRate = Conversion.ToDouble(stockdetail2.CgsRate);
									vd6.RateCut = Conversion.ToDouble(item7.RateCut);
									vd6.RateCutAmount = Conversion.ToDouble(item7.RateCutAmount);
									vd6.ItemAmount = Conversion.ToDouble(stockdetail2.AmountOut);
									vd6.Expenses = Conversion.ToDouble(item7.ExpenseAmount);
									vd6.Commission = Conversion.ToDouble(item7.CommissionAmount);
									vd6.Freight = Conversion.ToDouble(item7.FreightAmount);
									vd6.OrderNo = Conversion.ToInt(item7.SaleOrder);
									vd6.GpNo = Conversion.ToInt(item7.GpNo);
									vd6.VehicleNo = item7.VehicleNo;
									vd6.JobLotId = Conversion.ToInt(item7.JobLotId);
									vd6.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
									vd6.BranchesId = item7.BranchId;
									vd6.CostCenterId = item7.CostCenterId;
									obj.VoucherHeadInvoices.voucherDetailList.Add(vd6);
								}
								stockdetail2.OrganizationId = obj.OrganizationId;
								stockdetail2.CompanyId = obj.CompanyId;
								stockdetail2.DocDate = obj.DocDate;
								stockdetail2.DocCodeNo = obj.DocNo;
								stockdetail2.SupplierCustomerId = obj.SupplierCustomerId;
								stockdetail2.RefDocumentTypeId = obj.DocumentTypeId;
								stockdetail2.BranchesId = obj.BranchesId;
								stockdetail2.EntryUser = obj.EntryUser;
								stockdetail2.ModifyUser = obj.ModifyUser;
								stockdetail2.CalcType = "Weight";
								InvSaleInvoiceDetail sid2 = obj.InvSaleInvoiceDetaillist.Find((InvSaleInvoiceDetail x) => x.InvGdnId == stockdetail2.OtherDocNoId && x.InvGdnDetailId == stockdetail2.OtherSubDocNoId && stockdetail2.OtherDocumentTypeId == RefDocumentTypeId2);
								if (sid2 != null)
								{
									double RateUom = Architecture.DAL.TaxProject.Common.CommonServices.GetEqvilentByItemIdAndUomScheduleId(new ReportsParameters
									{
										OrganizationId = obj.OrganizationId,
										CompanyId = obj.CompanyId,
										ItemId = item7.ItemId,
										Id = sid2.UomScheduleIdRate
									});
									if (stockdetail2.BillWeightOut > 0.0 && sid2.ItemRate > 0.0 && RateUom > 0.0)
									{
										double ExpAmount = 0.0;
										double Freight = 0.0;
										double CommAmount = 0.0;
										stockdetail2.AmountOut = stockdetail2.BillWeightOut / RateUom * sid2.ItemRate;
										if (item7.ExpenseAmount > 0.0 && item7.ItemQty > 0.0 && stockdetail2.QtyOut > 0.0)
										{
											ExpAmount = item7.ExpenseAmount / item7.ItemQty * stockdetail2.QtyOut;
										}
										if (item7.FreightAmount > 0.0 && item7.NetBillWeight > 0.0 && stockdetail2.BillWeightOut > 0.0)
										{
											Freight = item7.FreightAmount / item7.NetBillWeight * stockdetail2.BillWeightOut;
										}
										if (item7.CommissionAmount > 0.0 && item7.ItemAmount > 0.0 && stockdetail2.AmountOut > 0.0)
										{
											CommAmount = item7.CommissionAmount / item7.ItemAmount * stockdetail2.AmountOut;
										}
										stockdetail2.AmountOut += ExpAmount - Freight - CommAmount;
									}
									stockdetail2.ItemRate = sid2.ItemRate;
									stockdetail2.RefDocIdNo = sid2.InvSaleInvoiceId;
									stockdetail2.RefDocSubIdNo = sid2.Id;
								}
								obj.InventoryStockEvalautionDetailslist.Add(stockdetail2);
							}
							continue;
						}
						throw new Exception("InventoryStockEvalautionDetailslist not found against CGS other reference");
					}
					if (obj.InventoryStockEvalautionDetailslist != null && obj.InventoryStockEvalautionDetailslist.Count > 0)
					{
						foreach (InventoryStockEvalautionDetail item3 in obj.InventoryStockEvalautionDetailslist.ToList())
						{
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "USP_InventoryStockEvalautionDetailGdnReferences_Update"));
						}
					}
				}
				else if (obj.DocumentTypeId != 222)
				{
					if (FifoEntryAllow)
					{
						string RemarksCgs2 = "";
						obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
						lstJobLot = Architecture.DAL.TaxProject.Common.CommonServices.GetJobLotGlIdsandName(obj.OrganizationId, obj.CompanyId);
						foreach (InvSaleInvoiceDetail item6 in obj.InvSaleInvoiceDetaillist.ToList())
						{
							List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item6.ItemId)).ToList();
							if (find2.Count <= 0)
							{
								continue;
							}
							ReportsParameters stock = new ReportsParameters();
							stock.OrganizationId = obj.OrganizationId;
							stock.CompanyId = obj.CompanyId;
							stock.ItemId = item6.ItemId;
							stock.WarehouseId = item6.WarehouseId;
							if (obj.DocumentTypeId == 99)
							{
								stock.DocDate = obj.SupplierInvoiceDate;
							}
							else
							{
								stock.DocDate = obj.DocDate;
							}
							if (obj.DocumentTypeId != 126)
							{
								stock.stockUOM = item6.ItemUOMId;
								stock.JobLotId = item6.JobLotId;
								stock.CropYear = item6.CropYear;
								stock.PackingTypeId = item6.PackingTypeId;
							}
							if (obj.ModifyUser > 0)
							{
								stock.DocumentTypeId = obj.DocumentTypeId;
								stock.Id = obj.Id;
							}
							stock.ItemQty = item6.ItemQty;
							stock.NetWeight = item6.NetStockWeight;
							stock.RateUOM = item6.UomScheduleIdRate;
							stock.LineId = item6.LineId;
							stock.ItemName = Conversion.ToString(find2[0].ItemName);
							List<GetJobLotGlIdsandName> findJob2 = lstJobLot.Where((GetJobLotGlIdsandName x) => x.Id == Conversion.ToInt(item6.JobLotId)).ToList();
							bool JobLotFlag2 = false;
							if (findJob2 != null && findJob2.Count > 0 && findJob2 != null && findJob2[0].AccountId > 0)
							{
								JobLotFlag2 = true;
							}
							foreach (InventoryStockEvalautionDetail stockdetail3 in Architecture.DAL.TaxProject.Common.CommonServices.FIFOImplemention(stock, obj.InventoryStockEvalautionDetailslist).InventoryStockEvalautionDetailslist.ToList())
							{
								if (!JobLotFlag2)
								{
									RemarksCgs2 = "ItemQty: " + stockdetail3.QtyOut + "   " + Conversion.ToString(find2[0].ItemName) + "  Net Weight: " + stockdetail3.StockWeightOut + "  CGS Rate:" + stockdetail3.CgsRate;
									VoucherDetail vd7 = new VoucherDetail();
									vd7.LineId = item6.LineId;
									vd7.IsCGS = 1;
									vd7.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
									vd7.AgainstAccountId = Conversion.ToInt(find2[0].COGSGLAC);
									vd7.Comments = Conversion.ToString(RemarksCgs2 + "  " + obj.CommonIdsForFinancials.CompanyName);
									vd7.CreditAmount = Conversion.ToDouble(stockdetail3.CgsAmount);
									vd7.DocumentTypeIdRef = Conversion.ToInt(item6.RefRefDocumentTypeId);
									vd7.InvoiceNoRefId = Conversion.ToInt(item6.RefRefDocIdNo);
									vd7.RefInvoiceNo = Conversion.ToString(item6.RefDocSubId);
									vd7.ItemId = Conversion.ToInt(item6.ItemId);
									vd7.QtyOut = Conversion.ToDouble(stockdetail3.QtyOut);
									vd7.WeightOut = Conversion.ToDouble(stockdetail3.StockWeightOut);
									vd7.ItemCgsRate = Conversion.ToDouble(stockdetail3.CgsRate);
									vd7.RateCut = Conversion.ToDouble(item6.RateCut);
									vd7.RateCutAmount = Conversion.ToDouble(item6.RateCutAmount);
									vd7.ItemAmount = Conversion.ToDouble(stockdetail3.AmountOut);
									vd7.Expenses = Conversion.ToDouble(item6.ExpenseAmount);
									vd7.Commission = Conversion.ToDouble(item6.CommissionAmount);
									vd7.Freight = Conversion.ToDouble(item6.FreightAmount);
									vd7.OrderNo = Conversion.ToInt(item6.SaleOrder);
									vd7.GpNo = Conversion.ToInt(item6.GpNo);
									vd7.VehicleNo = item6.VehicleNo;
									vd7.JobLotId = Conversion.ToInt(item6.JobLotId);
									vd7.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
									obj.VoucherHeadInvoices.voucherDetailList.Add(vd7);
									VoucherDetail vd9 = new VoucherDetail();
									vd9.LineId = item6.LineId;
									vd9.IsCGS = 1;
									vd9.AccountId = Conversion.ToInt(find2[0].COGSGLAC);
									vd9.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
									vd9.Comments = Conversion.ToString(RemarksCgs2 + "  " + obj.CommonIdsForFinancials.CompanyName);
									vd9.DebitAmount = Conversion.ToDouble(stockdetail3.CgsAmount);
									vd9.DocumentTypeIdRef = Conversion.ToInt(item6.RefRefDocumentTypeId);
									vd9.InvoiceNoRefId = Conversion.ToInt(item6.RefRefDocIdNo);
									vd9.RefInvoiceNo = Conversion.ToString(item6.RefDocSubId);
									vd9.ItemId = Conversion.ToInt(item6.ItemId);
									vd9.QtyOut = Conversion.ToDouble(stockdetail3.QtyOut);
									vd9.WeightOut = Conversion.ToDouble(stockdetail3.StockWeightOut);
									vd9.ItemCgsRate = Conversion.ToDouble(stockdetail3.CgsRate);
									vd9.RateCut = Conversion.ToDouble(item6.RateCut);
									vd9.RateCutAmount = Conversion.ToDouble(item6.RateCutAmount);
									vd9.ItemAmount = Conversion.ToDouble(stockdetail3.AmountOut);
									vd9.Expenses = Conversion.ToDouble(item6.ExpenseAmount);
									vd9.Commission = Conversion.ToDouble(item6.CommissionAmount);
									vd9.Freight = Conversion.ToDouble(item6.FreightAmount);
									vd9.OrderNo = Conversion.ToInt(item6.SaleOrder);
									vd9.GpNo = Conversion.ToInt(item6.GpNo);
									vd9.VehicleNo = item6.VehicleNo;
									vd9.JobLotId = Conversion.ToInt(item6.JobLotId);
									vd9.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
									obj.VoucherHeadInvoices.voucherDetailList.Add(vd9);
								}
								if (obj.DocumentTypeId == 126)
								{
									stockdetail3.ItemUom = item6.ItemUOMId;
									stockdetail3.JobLotId = item6.JobLotId;
								}
								obj.InventoryStockEvalautionDetailslist.Add(stockdetail3);
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
							foreach (InventoryStockEvalautionDetail item5 in obj.InventoryStockEvalautionDetailslist)
							{
								item5.OrganizationId = obj.OrganizationId;
								item5.CompanyId = obj.CompanyId;
								item5.DocDate = obj.DocDate;
								item5.DocCodeNo = obj.DocNo;
								item5.SupplierCustomerId = obj.SupplierCustomerId;
								item5.BranchesId = obj.BranchesId;
								item5.RefDocumentTypeId = obj.DocumentTypeId;
								item5.EntryUser = obj.EntryUser;
								item5.ModifyUser = obj.ModifyUser;
								item5.CalcType = "Weight";
								InvSaleInvoiceDetail sid3 = obj.InvSaleInvoiceDetaillist.Find((InvSaleInvoiceDetail x) => x.LineId == item5.LineId && x.LineId > 0 && item5.LineId > 0);
								if (sid3 != null)
								{
									double RateUom2 = Architecture.DAL.TaxProject.Common.CommonServices.GetEqvilentByItemIdAndUomScheduleId(new ReportsParameters
									{
										OrganizationId = obj.OrganizationId,
										CompanyId = obj.CompanyId,
										ItemId = item5.ItemId,
										Id = sid3.UomScheduleIdRate
									});
									if (item5.BillWeightOut > 0.0 && sid3.ItemRate > 0.0 && RateUom2 > 0.0)
									{
										if (obj.DocumentTypeId == 184)
										{
											item5.AmountOut = item5.BillWeightOut / RateUom2 * sid3.ItemRate - sid3.ItemDiscountAmount;
										}
										else
										{
											item5.AmountOut = item5.BillWeightOut / RateUom2 * sid3.ItemRate;
										}
									}
									item5.ItemRate = sid3.ItemRate;
									item5.RefDocIdNo = sid3.InvSaleInvoiceId;
									item5.RefDocSubIdNo = sid3.Id;
								}
								Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "USP_InventoryStockEvalautionDetail_Insert"));
							}
						}
					}
					else if (obj.ModifyUser > 0)
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
				}
			}
			else if (obj.DocumentTypeId != 95 && obj.DocumentTypeId != 171 && obj.DocumentTypeId != 222 && obj.DocumentTypeId != 1660)
			{
				InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
				inventoryTransactions.OrganizationId = obj.OrganizationId;
				inventoryTransactions.CompanyId = obj.CompanyId;
				inventoryTransactions.RefDocumentTypeId = obj.DocumentTypeId;
				inventoryTransactions.RefDocIdNo = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
			}
			else if (obj.DocumentTypeId == 95 || obj.DocumentTypeId == 171 || obj.DocumentTypeId == 1660)
			{
				obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
				int RefDocumentTypeId = ((obj.DocumentTypeId == 171) ? 221 : 86);
				if (obj.DocumentTypeId == 171)
				{
					if (obj.InvSaleInvoiceDetaillist.Exists((InvSaleInvoiceDetail p) => p.SaleOrderId > 0))
					{
						RefDocumentTypeId = 170;
					}
				}
				else if (obj.DocumentTypeId == 1660)
				{
					RefDocumentTypeId = 1659;
				}
				foreach (InvSaleInvoiceDetail item9 in obj.InvSaleInvoiceDetaillist)
				{
					List<InventoryStockEvalautionDetail> lstOtherRef2 = new List<InventoryStockEvalautionDetail>();
					lstOtherRef2 = Architecture.DAL.TaxProject.Common.CommonServices.GetInventoryStockEvalautionDetailByOtherIds(new ReportsParameters
					{
						OrganizationId = obj.OrganizationId,
						CompanyId = obj.CompanyId,
						OtherDocumentTypeId = RefDocumentTypeId,
						OtherDocNoId = item9.InvGdnId,
						OtherSubDocNoId = item9.InvGdnDetailId
					});
					if (lstOtherRef2 != null && lstOtherRef2.Count > 0)
					{
						foreach (InventoryStockEvalautionDetail stockdetail in lstOtherRef2.ToList())
						{
							stockdetail.OrganizationId = obj.OrganizationId;
							stockdetail.CompanyId = obj.CompanyId;
							stockdetail.DocDate = obj.DocDate;
							stockdetail.DocCodeNo = obj.DocNo;
							stockdetail.SupplierCustomerId = obj.SupplierCustomerId;
							stockdetail.BranchesId = obj.BranchesId;
							stockdetail.RefDocumentTypeId = obj.DocumentTypeId;
							stockdetail.EntryUser = obj.EntryUser;
							stockdetail.ModifyUser = obj.ModifyUser;
							stockdetail.CalcType = "Weight";
							InvSaleInvoiceDetail sid4 = obj.InvSaleInvoiceDetaillist.Find((InvSaleInvoiceDetail x) => x.InvGdnId == stockdetail.OtherDocNoId && x.InvGdnDetailId == stockdetail.OtherSubDocNoId && stockdetail.OtherDocumentTypeId == RefDocumentTypeId);
							if (sid4 != null)
							{
								double RateUom3 = Architecture.DAL.TaxProject.Common.CommonServices.GetEqvilentByItemIdAndUomScheduleId(new ReportsParameters
								{
									OrganizationId = obj.OrganizationId,
									CompanyId = obj.CompanyId,
									ItemId = item9.ItemId,
									Id = sid4.UomScheduleIdRate
								});
								if (stockdetail.BillWeightOut > 0.0 && sid4.ItemRate > 0.0 && RateUom3 > 0.0)
								{
									stockdetail.RateUom = sid4.UomScheduleIdRate;
									stockdetail.AmountOut = stockdetail.BillWeightOut / RateUom3 * sid4.ItemRate;
									stockdetail.CgsAmount = ((sid4.ItemCgsRate > 0.0) ? (stockdetail.StockWeightOut / RateUom3 * sid4.ItemCgsRate) : stockdetail.AmountOut);
								}
								stockdetail.ItemRate = sid4.ItemRate;
								stockdetail.CgsRate = ((sid4.ItemCgsRate > 0.0) ? sid4.ItemCgsRate : sid4.ItemRate);
								stockdetail.RefDocIdNo = sid4.InvSaleInvoiceId;
								stockdetail.RefDocSubIdNo = sid4.Id;
							}
							obj.InventoryStockEvalautionDetailslist.Add(stockdetail);
						}
						continue;
					}
					throw new Exception("InventoryStockEvalautionDetailslist not found other reference");
				}
				if (obj.InventoryStockEvalautionDetailslist != null && obj.InventoryStockEvalautionDetailslist.Count > 0)
				{
					foreach (InventoryStockEvalautionDetail item8 in obj.InventoryStockEvalautionDetailslist.ToList())
					{
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "USP_InventoryStockEvalautionDetailGdnReferences_Update"));
					}
				}
			}
			if (new HashSet<int>
			{
				99, 103, 60, 184, 126, 133, 139, 1608, 1661, 1611,
				186, 1809, 222, 1662
			}.Contains(obj.DocumentTypeId))
			{
				InventoryTransactions transactions = new InventoryTransactions
				{
					OrganizationId = obj.OrganizationId,
					CompanyId = obj.CompanyId,
					RefDocumentTypeId = obj.DocumentTypeId,
					RefDocIdNo = obj.Id
				};
				GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod");
			}
			DataTable dtvh = Architecture.DAL.TaxProject.Common.CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.Id);
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
			foreach (VoucherDetail item4 in obj.VoucherHeadInvoices.voucherDetailList)
			{
				item4.VoucherHeadId = obj.VoucherHeadInvoices.Id;
				item4.BranchesId = obj.BranchesId;
				InvSaleInvoiceDetail sid = obj.InvSaleInvoiceDetaillist.Find((InvSaleInvoiceDetail x) => x.LineId == item4.LineId && x.LineId > 0 && item4.LineId > 0);
				if (sid != null)
				{
					item4.RefDocSubIdNo = sid.Id;
				}
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Sp_VoucherDetail_Insert"));
			}
			if (obj.VoucherHeadInvoices.voucherDetailList == null || obj.VoucherHeadInvoices.voucherDetailList.Count == 0)
			{
				throw new Exception("Voucher Detail list Not Found");
			}
			SqlCommand sqlCommand3 = new SqlCommand();
			sqlCommand3.Connection = sqlcon;
			sqlCommand3.Transaction = sqlTrn;
			sqlCommand3.CommandType = CommandType.StoredProcedure;
			sqlCommand3.CommandText = "USP_VoucherBalanceCheck";
			sqlCommand3.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
			sqlCommand3.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
			sqlCommand3.Parameters.AddWithValue("@Id", obj.VoucherHeadInvoices.Id);
			sqlCommand3.ExecuteNonQuery();
			int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_H_Insert"));
			foreach (VoucherDetail item2 in obj.VoucherHeadInvoices.voucherDetailList)
			{
				item2.VoucherHeadId = obj.VoucherHeadInvoices.Id;
				item2.DocumentTypeIdRef = Code;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_H_Insert"));
			}
			DocumentApprovalDetail AP = new DocumentApprovalDetail();
			AP.OrganizationId = obj.OrganizationId;
			AP.CompanyId = obj.CompanyId;
			AP.DocumentTypeId = obj.DocumentTypeId;
			AP.Id = obj.Id;
			AP.LimitAmount = Conversion.ToDecimal(obj.BillAmount);
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, AP, "[DAW].[USp_DocumentApprovalDetail_Insert]"));
			if (obj.DeleteAttachmentsList != null && obj.DeleteAttachmentsList.Count > 0)
			{
				Architecture.DAL.Common.CommonServices.DeleteAttachmentsPhysically(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.DeleteAttachmentsList);
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			Architecture.DAL.Common.CommonServices.DeleteAttachmentsPhysicallyInException(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, copiedFiles);
			throw ex;
		}
	}
}
