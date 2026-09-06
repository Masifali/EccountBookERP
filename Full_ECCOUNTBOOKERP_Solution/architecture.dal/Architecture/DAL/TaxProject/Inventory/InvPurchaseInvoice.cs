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

public class InvPurchaseInvoice : DALBase
{
	public int SetData(Architecture.Model.TaxProject.Inventory.InvPurchaseInvoice obj, string ProcName)
	{
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
		bool GdnId = false;
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
			if (obj.invPurchaseInvoiceDetailList == null || obj.invPurchaseInvoiceDetailList.Count == 0)
			{
				throw new Exception("Detail List not found");
			}
			GdnId = obj.DocumentTypeId == 59 && obj.invPurchaseInvoiceDetailList.Any((InvPurchaseInvoiceDetail x) => x.GdnId > 0);
			obj.InvoiceQty = Conversion.ToDecimal(obj.invPurchaseInvoiceDetailList.Sum((InvPurchaseInvoiceDetail x) => x.ItemQty));
			obj.InvoiceWeight = Conversion.ToDecimal(obj.invPurchaseInvoiceDetailList.Sum((InvPurchaseInvoiceDetail x) => x.NetBillWeight));
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
			string ItemIds = "";
			int LineIdForPDI = 0;
			bool WagesChargeToProduct = Conversion.ToBool(Architecture.DAL.TaxProject.Common.CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "ContractWagesChargetoProduct"));
			foreach (InvPurchaseInvoiceDetail item16 in obj.invPurchaseInvoiceDetailList)
			{
				LineIdForPDI = (item16.LineId = LineIdForPDI + 1);
				if (obj.DocumentTypeId == 98)
				{
					if (WagesChargeToProduct)
					{
						item16.BillAmount = item16.ItemAmount + item16.FreightAmount + item16.ExpenseAmount + item16.WagesAmount - item16.CommissionAmount;
					}
					else
					{
						item16.BillAmount = item16.ItemAmount + item16.FreightAmount + item16.ExpenseAmount - item16.CommissionAmount;
					}
				}
				else if (WagesChargeToProduct)
				{
					item16.BillAmount = item16.ItemAmount + item16.FreightAmount + item16.ExpenseAmount + item16.CommissionAmount + item16.Brokery + item16.WagesAmount;
				}
				else
				{
					item16.BillAmount = item16.ItemAmount + item16.FreightAmount + item16.ExpenseAmount + item16.CommissionAmount + item16.Brokery;
				}
				item16.InvPurchaseInvoiceId = obj.Id;
				ItemIds = ItemIds + item16.ItemId + ",";
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item16, "Sp_InvPurchaseInvoiceDetail_Insert"));
				item16.Id = code;
			}
			foreach (InvPurchaseInvoiceFreight item15 in obj.InvPurchaseInvoiceFreightList)
			{
				item15.InvPurchaseInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item15, "Sp_InvPurchaseInvoiceFreight_Insert"));
			}
			foreach (InvPurchaseInvoiceJournal item14 in obj.InvPurchaseInvoicejournalList)
			{
				item14.InvPurchaseInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item14, "Sp_InvPurchaseInvoiceJournal_Insert"));
			}
			foreach (InvPurchaseInvoiceExpense item13 in obj.InvPurchaseInvoiceExpList)
			{
				item13.InvPurchaseInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item13, "Sp_InvPurchaseInvoiceExpense_Insert"));
			}
			foreach (InvPurchaseInvoiceEmptyBags item12 in obj.InvPurchaseInvoiceEmptyBagslist)
			{
				item12.InvPurchaseInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item12, "Sp_InvPurchaseInvoiceEmptyBags_Insert"));
			}
			if (obj.PaymentDuesSchedules != null && obj.PaymentDuesSchedules.Count > 0)
			{
				foreach (PaymentDueSchedule item11 in obj.PaymentDuesSchedules)
				{
					item11.RefdocNoId = obj.Id;
					item11.refDocumentTypeId = obj.DocumentTypeId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item11, "Sp_PaymentDueSchedule_Insert"));
				}
			}
			if (obj.InvPurchaseInvoicePaymentTermsDetailList != null && obj.InvPurchaseInvoicePaymentTermsDetailList.Count > 0)
			{
				int sortno = 1;
				foreach (InvPurchaseInvoicePaymentTermsDetail item10 in obj.InvPurchaseInvoicePaymentTermsDetailList)
				{
					item10.InvPurchaseInvoiceId = obj.Id;
					item10.SortNo = sortno;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item10, "USP_InvPurchaseInvoicePaymentTermsDetail_Insert"));
					sortno++;
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
				foreach (DMSAttachments item9 in obj.AttachmentsList.ToList())
				{
					item9.RefAccountId = obj.SupplierCustomerId;
					item9.RefDocumentTypeId = obj.DocumentTypeId;
					item9.RefDocumentNo = returnID;
					item9.EntryDate = DateTime.Now;
					item9.ModifyDate = DateTime.Now;
					item9.EntryUser = obj.EntryUser;
					item9.ModifyUser = obj.EntryUser;
					item9.OrganizationId = obj.OrganizationId;
					item9.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item9, "Proc_DMSAttachments_Insert"));
				}
			}
			bool FIFOCgs = Architecture.DAL.TaxProject.Common.CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5);
			if (FIFOCgs && obj.DocumentTypeId == 59 && !GdnId)
			{
				string RemarksCgs = "";
				List<GetItemGlIdsandItemName> dtitem = (dtitem = Architecture.DAL.TaxProject.Common.CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId));
				obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
				foreach (InvPurchaseInvoiceDetail item8 in obj.invPurchaseInvoiceDetailList.ToList())
				{
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item8.ItemId)).ToList();
					if (find.Count <= 0)
					{
						continue;
					}
					ReportsParameters stock = new ReportsParameters();
					stock.OrganizationId = obj.OrganizationId;
					stock.CompanyId = obj.CompanyId;
					stock.ItemId = item8.ItemId;
					stock.WarehouseId = item8.WarehouseId;
					stock.stockUOM = item8.ItemUOMId;
					stock.DocDate = obj.DocDate;
					stock.JobLotId = item8.JobLotId;
					stock.CropYear = item8.CropYear;
					stock.PackingTypeId = item8.PackingTypeId;
					if (obj.ModifyUser > 0)
					{
						stock.DocumentTypeId = obj.DocumentTypeId;
						stock.Id = obj.Id;
					}
					stock.ItemQty = item8.ItemQty;
					stock.NetWeight = item8.NetStockWeight;
					stock.RateUOM = item8.UomScheduleIdRate;
					stock.LineId = item8.LineId;
					stock.ItemName = Conversion.ToString(find[0].ItemName);
					foreach (InventoryStockEvalautionDetail stockdetail2 in Architecture.DAL.TaxProject.Common.CommonServices.FIFOImplemention(stock, obj.InventoryStockEvalautionDetailslist).InventoryStockEvalautionDetailslist.ToList())
					{
						RemarksCgs = "ItemQty: " + stockdetail2.QtyOut + "   " + Conversion.ToString(find[0].ItemName) + "  Net Weight: " + stockdetail2.StockWeightOut + "  CGS Rate:" + stockdetail2.CgsRate;
						VoucherDetail vd8 = new VoucherDetail();
						vd8.LineId = item8.LineId;
						vd8.AccountId = Conversion.ToInt(find[0].COGSGLAC);
						vd8.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd8.Comments = Conversion.ToString(RemarksCgs + "  " + obj.CommonIdsForFinancials.CompanyName);
						vd8.DebitAmount = Conversion.ToDouble(stockdetail2.CgsAmount);
						vd8.ItemId = Conversion.ToInt(item8.ItemId);
						vd8.QtyOut = Conversion.ToDouble(stockdetail2.QtyOut);
						vd8.WeightOut = Conversion.ToDouble(stockdetail2.StockWeightOut);
						vd8.ItemCgsRate = Conversion.ToDouble(stockdetail2.CgsRate);
						vd8.RateCut = Conversion.ToDouble(item8.RateCut);
						vd8.RateCutAmount = Conversion.ToDouble(item8.RateCutAmount);
						vd8.ItemAmount = Conversion.ToDouble(stockdetail2.AmountOut);
						vd8.Expenses = Conversion.ToDouble(item8.ExpenseAmount);
						vd8.Commission = Conversion.ToDouble(item8.CommissionAmount);
						vd8.Freight = Conversion.ToDouble(item8.FreightAmount);
						vd8.GpNo = Conversion.ToInt(item8.GpNo);
						vd8.VehicleNo = item8.VehicleNo;
						vd8.JobLotId = Conversion.ToInt(item8.JobLotId);
						vd8.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						obj.VoucherHeadInvoices.voucherDetailList.Add(vd8);
						VoucherDetail vd6 = new VoucherDetail();
						vd6.LineId = item8.LineId;
						vd6.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd6.AgainstAccountId = Conversion.ToInt(find[0].COGSGLAC);
						vd6.Comments = Conversion.ToString(RemarksCgs + "  " + obj.CommonIdsForFinancials.CompanyName);
						vd6.CreditAmount = Conversion.ToDouble(stockdetail2.CgsAmount);
						vd6.ItemId = Conversion.ToInt(item8.ItemId);
						vd6.QtyOut = Conversion.ToDouble(stockdetail2.QtyOut);
						vd6.WeightOut = Conversion.ToDouble(stockdetail2.StockWeightOut);
						vd6.ItemCgsRate = Conversion.ToDouble(stockdetail2.CgsRate);
						vd6.RateCut = Conversion.ToDouble(item8.RateCut);
						vd6.RateCutAmount = Conversion.ToDouble(item8.RateCutAmount);
						vd6.ItemAmount = Conversion.ToDouble(stockdetail2.AmountOut);
						vd6.Expenses = Conversion.ToDouble(item8.ExpenseAmount);
						vd6.Commission = Conversion.ToDouble(item8.CommissionAmount);
						vd6.Freight = Conversion.ToDouble(item8.FreightAmount);
						vd6.GpNo = Conversion.ToInt(item8.GpNo);
						vd6.VehicleNo = item8.VehicleNo;
						vd6.JobLotId = Conversion.ToInt(item8.JobLotId);
						vd6.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						obj.VoucherHeadInvoices.voucherDetailList.Add(vd6);
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
						item7.DocCodeNo = obj.DocNo;
						item7.SupplierCustomerId = obj.SupplierCustomerId;
						item7.BranchesId = obj.BranchesId;
						item7.RefDocumentTypeId = obj.DocumentTypeId;
						item7.EntryUser = obj.EntryUser;
						item7.ModifyUser = obj.ModifyUser;
						item7.CalcType = "Weight";
						InvPurchaseInvoiceDetail pid = obj.invPurchaseInvoiceDetailList.Find((InvPurchaseInvoiceDetail x) => x.LineId == item7.LineId && x.LineId > 0 && item7.LineId > 0);
						if (pid != null)
						{
							double RateUom = Architecture.DAL.TaxProject.Common.CommonServices.GetEqvilentByItemIdAndUomScheduleId(new ReportsParameters
							{
								OrganizationId = obj.OrganizationId,
								CompanyId = obj.CompanyId,
								ItemId = item7.ItemId,
								Id = pid.UomScheduleIdRate
							});
							if (item7.BillWeightOut > 0.0 && pid.ItemRate > 0.0 && RateUom > 0.0)
							{
								item7.AmountOut = item7.BillWeightOut / RateUom * pid.ItemRate;
							}
							item7.ItemRate = pid.ItemRate;
							item7.RefDocIdNo = pid.InvPurchaseInvoiceId;
							item7.RefDocSubIdNo = pid.Id;
						}
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "USP_InventoryStockEvalautionDetail_Insert"));
					}
				}
			}
			else if (FIFOCgs && obj.DocumentTypeId == 59 && GdnId)
			{
				obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
				lstJobLot = Architecture.DAL.TaxProject.Common.CommonServices.GetJobLotGlIdsandName(obj.OrganizationId, obj.CompanyId);
				List<GetItemGlIdsandItemName> dtitem2 = Architecture.DAL.TaxProject.Common.CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId);
				foreach (InvPurchaseInvoiceDetail item5 in obj.invPurchaseInvoiceDetailList)
				{
					int RefDocumentTypeId = 703;
					List<InventoryStockEvalautionDetail> lstOtherRef = new List<InventoryStockEvalautionDetail>();
					lstOtherRef = Architecture.DAL.TaxProject.Common.CommonServices.GetInventoryStockEvalautionDetailByOtherIds(new ReportsParameters
					{
						OrganizationId = obj.OrganizationId,
						CompanyId = obj.CompanyId,
						OtherDocumentTypeId = RefDocumentTypeId,
						OtherDocNoId = item5.GdnId,
						OtherSubDocNoId = item5.GdnDetailId
					});
					if (lstOtherRef != null && lstOtherRef.Count > 0)
					{
						List<GetJobLotGlIdsandName> findJob = lstJobLot.Where((GetJobLotGlIdsandName x) => x.Id == Conversion.ToInt(item5.JobLotId)).ToList();
						bool JobLotFlag = false;
						if (findJob != null && findJob.Count > 0 && findJob != null && findJob[0].AccountId > 0)
						{
							JobLotFlag = true;
						}
						string RemarksCgs2 = "";
						foreach (InventoryStockEvalautionDetail stockdetail in lstOtherRef.ToList())
						{
							List<GetItemGlIdsandItemName> find2 = dtitem2.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item5.ItemId)).ToList();
							if (find2.Count == 0)
							{
								throw new Exception("ItemId Not Found Against CGS Transaction");
							}
							if (!JobLotFlag)
							{
								RemarksCgs2 = "ItemQty: " + stockdetail.QtyOut + "   " + Conversion.ToString(find2[0].ItemName) + "  Net Weight: " + stockdetail.StockWeightOut + "  CGS Rate:" + stockdetail.CgsRate;
								VoucherDetail vd9 = new VoucherDetail();
								vd9.LineId = item5.LineId;
								vd9.IsCGS = 1;
								vd9.AccountId = Conversion.ToInt(find2[0].COGSGLAC);
								vd9.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
								vd9.Comments = Conversion.ToString(RemarksCgs2 + "  " + obj.CommonIdsForFinancials.CompanyName);
								vd9.DebitAmount = Conversion.ToDouble(stockdetail.CgsAmount);
								vd9.ItemId = Conversion.ToInt(item5.ItemId);
								vd9.QtyOut = Conversion.ToDouble(stockdetail.QtyOut);
								vd9.WeightOut = Conversion.ToDouble(stockdetail.StockWeightOut);
								vd9.ItemCgsRate = Conversion.ToDouble(stockdetail.CgsRate);
								vd9.RateCut = Conversion.ToDouble(item5.RateCut);
								vd9.RateCutAmount = Conversion.ToDouble(item5.RateCutAmount);
								vd9.ItemAmount = Conversion.ToDouble(stockdetail.AmountOut);
								vd9.Expenses = Conversion.ToDouble(item5.ExpenseAmount);
								vd9.Commission = Conversion.ToDouble(item5.CommissionAmount);
								vd9.Freight = Conversion.ToDouble(item5.FreightAmount);
								vd9.GpNo = Conversion.ToInt(item5.GpNo);
								vd9.VehicleNo = item5.VehicleNo;
								vd9.JobLotId = Conversion.ToInt(item5.JobLotId);
								vd9.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vd9.BranchesId = item5.BranchId;
								obj.VoucherHeadInvoices.voucherDetailList.Add(vd9);
								VoucherDetail vd7 = new VoucherDetail();
								vd7.LineId = item5.LineId;
								vd7.IsCGS = 1;
								vd7.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
								vd7.AgainstAccountId = Conversion.ToInt(find2[0].COGSGLAC);
								vd7.Comments = Conversion.ToString(RemarksCgs2 + "  " + obj.CommonIdsForFinancials.CompanyName);
								vd7.CreditAmount = Conversion.ToDouble(stockdetail.CgsAmount);
								vd7.ItemId = Conversion.ToInt(item5.ItemId);
								vd7.QtyOut = Conversion.ToDouble(stockdetail.QtyOut);
								vd7.WeightOut = Conversion.ToDouble(stockdetail.StockWeightOut);
								vd7.ItemCgsRate = Conversion.ToDouble(stockdetail.CgsRate);
								vd7.RateCut = Conversion.ToDouble(item5.RateCut);
								vd7.RateCutAmount = Conversion.ToDouble(item5.RateCutAmount);
								vd7.ItemAmount = Conversion.ToDouble(stockdetail.AmountOut);
								vd7.Expenses = Conversion.ToDouble(item5.ExpenseAmount);
								vd7.Commission = Conversion.ToDouble(item5.CommissionAmount);
								vd7.Freight = Conversion.ToDouble(item5.FreightAmount);
								vd7.GpNo = Conversion.ToInt(item5.GpNo);
								vd7.VehicleNo = item5.VehicleNo;
								vd7.JobLotId = Conversion.ToInt(item5.JobLotId);
								vd7.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vd7.BranchesId = item5.BranchId;
								obj.VoucherHeadInvoices.voucherDetailList.Add(vd7);
							}
							stockdetail.OrganizationId = obj.OrganizationId;
							stockdetail.CompanyId = obj.CompanyId;
							stockdetail.DocDate = obj.DocDate;
							stockdetail.DocCodeNo = obj.DocNo;
							stockdetail.SupplierCustomerId = obj.SupplierCustomerId;
							stockdetail.RefDocumentTypeId = obj.DocumentTypeId;
							stockdetail.BranchesId = obj.BranchesId;
							stockdetail.EntryUser = obj.EntryUser;
							stockdetail.ModifyUser = obj.ModifyUser;
							stockdetail.CalcType = "Weight";
							InvPurchaseInvoiceDetail sid = obj.invPurchaseInvoiceDetailList.Find((InvPurchaseInvoiceDetail x) => x.GdnId == stockdetail.OtherDocNoId && x.GdnDetailId == stockdetail.OtherSubDocNoId && stockdetail.OtherDocumentTypeId == RefDocumentTypeId);
							if (sid != null)
							{
								double RateUom2 = Architecture.DAL.TaxProject.Common.CommonServices.GetEqvilentByItemIdAndUomScheduleId(new ReportsParameters
								{
									OrganizationId = obj.OrganizationId,
									CompanyId = obj.CompanyId,
									ItemId = item5.ItemId,
									Id = sid.UomScheduleIdRate
								});
								if (stockdetail.BillWeightOut > 0.0 && sid.ItemRate > 0.0 && RateUom2 > 0.0)
								{
									double ExpAmount = 0.0;
									double Freight = 0.0;
									double CommAmount = 0.0;
									stockdetail.AmountOut = stockdetail.BillWeightOut / RateUom2 * sid.ItemRate;
									if (item5.ExpenseAmount > 0.0 && item5.ItemQty > 0.0 && stockdetail.QtyOut > 0.0)
									{
										ExpAmount = item5.ExpenseAmount / item5.ItemQty * stockdetail.QtyOut;
									}
									if (item5.FreightAmount > 0.0 && item5.NetBillWeight > 0.0 && stockdetail.BillWeightOut > 0.0)
									{
										Freight = item5.FreightAmount / item5.NetBillWeight * stockdetail.BillWeightOut;
									}
									if (item5.CommissionAmount > 0.0 && item5.ItemAmount > 0.0 && stockdetail.AmountOut > 0.0)
									{
										CommAmount = item5.CommissionAmount / item5.ItemAmount * stockdetail.AmountOut;
									}
									stockdetail.AmountOut += ExpAmount - Freight - CommAmount;
								}
								stockdetail.ItemRate = sid.ItemRate;
								stockdetail.RefDocIdNo = sid.InvPurchaseInvoiceId;
								stockdetail.RefDocSubIdNo = sid.Id;
							}
							obj.InventoryStockEvalautionDetailslist.Add(stockdetail);
						}
						continue;
					}
					throw new Exception("InventoryStockEvalautionDetailslist not found against CGS other reference");
				}
				if (obj.InventoryStockEvalautionDetailslist != null && obj.InventoryStockEvalautionDetailslist.Count > 0)
				{
					foreach (InventoryStockEvalautionDetail item6 in obj.InventoryStockEvalautionDetailslist.ToList())
					{
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "USP_InventoryStockEvalautionDetailGdnReferences_Update"));
					}
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
			DataTable dtvh = Architecture.DAL.TaxProject.Common.CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.Id);
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
					item4.BranchesId = obj.BranchesId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Sp_VoucherDetail_Insert"));
				}
				SqlCommand sqlCommand2 = new SqlCommand();
				sqlCommand2.Connection = sqlcon;
				sqlCommand2.Transaction = sqlTrn;
				sqlCommand2.CommandType = CommandType.StoredProcedure;
				sqlCommand2.CommandText = "USP_VoucherBalanceCheck";
				sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand2.Parameters.AddWithValue("@Id", obj.VoucherHeadInvoices.Id);
				sqlCommand2.ExecuteScalar();
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_H_Insert"));
				foreach (VoucherDetail item3 in obj.VoucherHeadInvoices.voucherDetailList)
				{
					item3.VoucherHeadId = obj.VoucherHeadInvoices.Id;
					item3.DocumentTypeIdRef = Code;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Sp_VoucherDetail_H_Insert"));
				}
				if (new HashSet<int> { 57, 98, 61, 63, 104, 131, 59, 1604, 1804, 245 }.Contains(obj.DocumentTypeId))
				{
					InventoryTransactions transactions = new InventoryTransactions();
					transactions.OrganizationId = obj.OrganizationId;
					transactions.CompanyId = obj.CompanyId;
					transactions.RefDocumentTypeId = obj.DocumentTypeId;
					transactions.RefDocIdNo = returnID;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
				}
				if (obj.DocumentTypeId == 59 && !GdnId)
				{
					foreach (InvPurchaseInvoiceDetail item2 in obj.invPurchaseInvoiceDetailList)
					{
						SqlCommand sqlCommand3 = new SqlCommand();
						sqlCommand3.Connection = sqlcon;
						sqlCommand3.Transaction = sqlTrn;
						sqlCommand3.CommandType = CommandType.StoredProcedure;
						sqlCommand3.CommandText = "USP_InventoryValidation";
						sqlCommand3.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
						sqlCommand3.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
						sqlCommand3.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
						sqlCommand3.Parameters.AddWithValue("@DocDate", obj.DocDate);
						sqlCommand3.Parameters.AddWithValue("@ItemId", item2.ItemId);
						sqlCommand3.Parameters.AddWithValue("@WarehouseId", item2.WarehouseId);
						sqlCommand3.Parameters.AddWithValue("@JobLotId", item2.JobLotId);
						sqlCommand3.Parameters.AddWithValue("@CropYear", item2.CropYear);
						sqlCommand3.Parameters.AddWithValue("@InvPackingTypeId", item2.PackingTypeId);
						sqlCommand3.Parameters.AddWithValue("@PackUomId", item2.ItemUOMId);
						sqlCommand3.Parameters.AddWithValue("@NetWeight", item2.NetStockWeight);
						sqlCommand3.ExecuteNonQuery();
					}
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
			throw new Exception("VoucherDetail list Not Found");
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			Architecture.DAL.Common.CommonServices.DeleteAttachmentsPhysicallyInException(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, copiedFiles);
			throw ex;
		}
	}
}
