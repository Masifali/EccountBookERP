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
using Architecture.Model.pcc;

namespace Architecture.DAL.pcc;

public class InvSaleInvoice
{
	public int SetData(Architecture.Model.pcc.InvSaleInvoice obj, string ProcName)
	{
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
		new List<SupplierCustomer>();
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			int LineId = 1;
			int getGdnId = 0;
			int OtherDocumentTypeId = 0;
			foreach (Architecture.Model.pcc.InvSaleInvoiceDetail item15 in obj.InvSaleInvoiceDetaillist)
			{
				item15.LineId = LineId;
				item15.InvSaleInvoiceId = obj.Id;
				getGdnId = item15.InvGdnId;
				item15.ItemNetAmount = item15.ItemAmountWithDisc + item15.ExpenseAmount + item15.WagesAmount - item15.CommissionAmount;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item15, "pcc.USP_InvSaleInvoiceDetail_Insert"));
				item15.Id = code;
				LineId++;
			}
			foreach (Architecture.Model.pcc.InvSaleInvoiceFreight item14 in obj.InvSaleInvoiceFreightList)
			{
				item14.InvSaleInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item14, "pcc.USP_InvSaleInvoiceFreight_Insert"));
			}
			foreach (Architecture.Model.pcc.InvSaleInvoiceJournal item13 in obj.InvSaleInvoiceJournalList)
			{
				item13.InvSaleInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item13, "pcc.USP_InvSaleInvoiceJournal_Insert"));
			}
			foreach (Architecture.Model.pcc.InvSaleInvoiceExpense item12 in obj.InvSaleInvoiceExpenseList)
			{
				item12.InvSaleInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item12, "pcc.USP_InvSaleInvoiceExpense_Insert"));
			}
			foreach (InvSaleInvoiceWagesDetail item11 in obj.InvSaleInvoiceWagesDetailList)
			{
				item11.InvSaleInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item11, "pcc.USP_InvSaleInvoiceWagesDetail_Insert"));
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				foreach (DMSAttachments item10 in obj.AttachmentsList.ToList())
				{
					item10.RefAccountId = obj.SupplierCustomerId;
					item10.RefDocumentTypeId = obj.DocumentTypeId;
					item10.RefDocumentNo = returnID;
					item10.EntryDate = DateTime.Now;
					item10.ModifyDate = DateTime.Now;
					item10.EntryUser = obj.EntryUserId;
					item10.ModifyUser = obj.ModifyUserId;
					item10.OrganizationId = obj.OrganizationId;
					item10.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item10, "Proc_DMSAttachments_Insert"));
				}
			}
			List<GetItemGlIdsandItemName> dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId);
			if (CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5) && obj.DocumentTypeId != 1862)
			{
				if (getGdnId > 0)
				{
					if (obj.DocumentTypeId == 1861)
					{
						OtherDocumentTypeId = 1855;
					}
					else
					{
						OtherDocumentTypeId = 1866;
					}
					obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
					foreach (Architecture.Model.pcc.InvSaleInvoiceDetail item7 in obj.InvSaleInvoiceDetaillist)
					{
						List<InventoryStockEvalautionDetail> lstOtherRef = new List<InventoryStockEvalautionDetail>();
						lstOtherRef = GetInventoryStockEvalautionDetailByOtherIds(new ReportsParameters
						{
							OrganizationId = obj.OrganizationId,
							CompanyId = obj.CompanyId,
							OtherDocumentTypeId = OtherDocumentTypeId,
							OtherDocNoId = item7.InvGdnId,
							OtherSubDocNoId = item7.InvGdnDetailId,
							ActionId = 1
						});
						if (lstOtherRef != null && lstOtherRef.Count > 0)
						{
							string RemarksCgs = "";
							foreach (InventoryStockEvalautionDetail stockdetail2 in lstOtherRef.ToList())
							{
								List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item7.ItemId)).ToList();
								if (find.Count == 0)
								{
									throw new Exception("ItemId Not Found Against CGS Transaction");
								}
								RemarksCgs = Conversion.ToString(find[0].ItemName) + " Qty: " + stockdetail2.QtyOut + " Attribute Varient  " + item7.VarientEquivalent + " CGS Rate: " + stockdetail2.CgsRate;
								VoucherDetail vd8 = new VoucherDetail();
								vd8.LineId = item7.LineId;
								vd8.IsCGS = 1;
								vd8.AccountId = Conversion.ToInt(find[0].COGSGLAC);
								vd8.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
								vd8.Comments = Conversion.ToString(RemarksCgs + "  " + obj.CommonIdsForFinancials.CompanyName);
								vd8.DebitAmount = Conversion.ToDouble(stockdetail2.CgsAmount);
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
								vd8.GpNo = Conversion.ToInt(item7.GpNo);
								vd8.VehicleNo = item7.VehicleNo;
								vd8.JobLotId = Conversion.ToInt(item7.JobLotId);
								vd8.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								obj.VoucherHeadInvoices.voucherDetailList.Add(vd8);
								VoucherDetail vd6 = new VoucherDetail();
								vd6.LineId = item7.LineId;
								vd6.IsCGS = 1;
								vd6.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
								vd6.AgainstAccountId = Conversion.ToInt(find[0].COGSGLAC);
								vd6.Comments = Conversion.ToString(RemarksCgs + "  " + obj.CommonIdsForFinancials.CompanyName);
								vd6.CreditAmount = Conversion.ToDouble(stockdetail2.CgsAmount);
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
								vd6.GpNo = Conversion.ToInt(item7.GpNo);
								vd6.VehicleNo = item7.VehicleNo;
								vd6.JobLotId = Conversion.ToInt(item7.JobLotId);
								vd6.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								obj.VoucherHeadInvoices.voucherDetailList.Add(vd6);
								stockdetail2.OrganizationId = obj.OrganizationId;
								stockdetail2.CompanyId = obj.CompanyId;
								stockdetail2.DocDate = obj.DocDate;
								stockdetail2.DocCodeNo = obj.DocNo;
								stockdetail2.SupplierCustomerId = obj.SupplierCustomerId;
								stockdetail2.BranchesId = obj.BranchesId;
								stockdetail2.RefDocumentTypeId = obj.DocumentTypeId;
								stockdetail2.EntryUser = obj.EntryUserId;
								stockdetail2.ModifyUser = obj.ModifyUserId;
								stockdetail2.CalcType = "Qty";
								Architecture.Model.pcc.InvSaleInvoiceDetail sid2 = obj.InvSaleInvoiceDetaillist.Find((Architecture.Model.pcc.InvSaleInvoiceDetail x) => x.InvGdnId == stockdetail2.OtherDocNoId && x.InvGdnDetailId == stockdetail2.OtherSubDocNoId && stockdetail2.OtherDocumentTypeId == OtherDocumentTypeId);
								if (sid2 != null)
								{
									if (stockdetail2.QtyOut > 0.0 && sid2.ItemRate > 0m)
									{
										stockdetail2.AmountOut = stockdetail2.QtyOut * Conversion.ToDouble(sid2.ItemRate);
									}
									stockdetail2.ItemRate = Conversion.ToDouble(sid2.ItemRate);
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
				else
				{
					string RemarksCgs2 = "";
					obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
					foreach (Architecture.Model.pcc.InvSaleInvoiceDetail item6 in obj.InvSaleInvoiceDetaillist.ToList())
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
						stock.DocDate = obj.DocDate;
						if (item6.ItemAttributeVarientId > 0)
						{
							stock.stockUOM = item6.ItemAttributeVarientId;
						}
						if (obj.ModifyUserId > 0)
						{
							stock.DocumentTypeId = obj.DocumentTypeId;
							stock.Id = obj.Id;
						}
						stock.ItemQty = Conversion.ToDouble(item6.ItemQty);
						stock.NetWeight = Conversion.ToDouble(item6.ItemNetWeight);
						stock.LineId = item6.LineId;
						stock.ItemName = Conversion.ToString(find2[0].ItemName);
						foreach (InventoryStockEvalautionDetail stockdetail3 in CommonServices.FIFOImplementionForConcrete(stock, obj.InventoryStockEvalautionDetailslist).InventoryStockEvalautionDetailslist.ToList())
						{
							RemarksCgs2 = Conversion.ToString(find2[0].ItemName) + " Qty: " + stockdetail3.QtyOut + " Attribute Varient  " + item6.VarientEquivalent + " CGS Rate: " + stockdetail3.CgsRate;
							VoucherDetail vd9 = new VoucherDetail();
							vd9.LineId = item6.LineId;
							vd9.IsCGS = 1;
							vd9.AccountId = Conversion.ToInt(find2[0].COGSGLAC);
							vd9.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
							vd9.Comments = Conversion.ToString(RemarksCgs2 + "  " + obj.CommonIdsForFinancials.CompanyName);
							vd9.DebitAmount = Conversion.ToDouble(stockdetail3.CgsAmount);
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
							vd9.GpNo = Conversion.ToInt(item6.GpNo);
							vd9.VehicleNo = item6.VehicleNo;
							vd9.JobLotId = Conversion.ToInt(item6.JobLotId);
							vd9.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							obj.VoucherHeadInvoices.voucherDetailList.Add(vd9);
							VoucherDetail vd7 = new VoucherDetail();
							vd7.LineId = item6.LineId;
							vd7.IsCGS = 1;
							vd7.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
							vd7.AgainstAccountId = Conversion.ToInt(find2[0].COGSGLAC);
							vd7.Comments = Conversion.ToString(RemarksCgs2 + "  " + obj.CommonIdsForFinancials.CompanyName);
							vd7.CreditAmount = Conversion.ToDouble(stockdetail3.CgsAmount);
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
							vd7.GpNo = Conversion.ToInt(item6.GpNo);
							vd7.VehicleNo = item6.VehicleNo;
							vd7.JobLotId = Conversion.ToInt(item6.JobLotId);
							vd7.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							obj.VoucherHeadInvoices.voucherDetailList.Add(vd7);
							stockdetail3.VarientId = item6.ItemAttributeVarientId;
							stockdetail3.ItemUom = item6.ItemUomId;
							stockdetail3.JobLotId = item6.JobLotId;
							obj.InventoryStockEvalautionDetailslist.Add(stockdetail3);
						}
					}
					if (obj.InventoryStockEvalautionDetailslist != null && obj.InventoryStockEvalautionDetailslist.Count > 0)
					{
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
						foreach (InventoryStockEvalautionDetail item5 in obj.InventoryStockEvalautionDetailslist)
						{
							item5.OrganizationId = obj.OrganizationId;
							item5.CompanyId = obj.CompanyId;
							item5.DocDate = obj.DocDate;
							item5.DocCodeNo = obj.DocNo;
							item5.SupplierCustomerId = obj.SupplierCustomerId;
							item5.BranchesId = obj.BranchesId;
							item5.RefDocumentTypeId = obj.DocumentTypeId;
							item5.EntryUser = obj.EntryUserId;
							item5.ModifyUser = obj.ModifyUserId;
							item5.CalcType = "Qty";
							Architecture.Model.pcc.InvSaleInvoiceDetail sid3 = obj.InvSaleInvoiceDetaillist.Find((Architecture.Model.pcc.InvSaleInvoiceDetail x) => x.LineId == item5.LineId && x.LineId > 0 && item5.LineId > 0);
							if (sid3 != null)
							{
								if (item5.QtyOut > 0.0 && sid3.ItemRate > 0m)
								{
									item5.AmountOut = item5.QtyOut * Conversion.ToDouble(sid3.ItemRate);
								}
								item5.ItemRate = Conversion.ToDouble(sid3.ItemRate);
								item5.RefDocIdNo = sid3.InvSaleInvoiceId;
								item5.RefDocSubIdNo = sid3.Id;
							}
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "USP_InventoryStockEvalautionDetail_Insert"));
						}
					}
				}
			}
			else if (getGdnId > 0)
			{
				obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
				foreach (Architecture.Model.pcc.InvSaleInvoiceDetail item9 in obj.InvSaleInvoiceDetaillist)
				{
					List<InventoryStockEvalautionDetail> lstOtherRef2 = new List<InventoryStockEvalautionDetail>();
					lstOtherRef2 = GetInventoryStockEvalautionDetailByOtherIds(new ReportsParameters
					{
						OrganizationId = obj.OrganizationId,
						CompanyId = obj.CompanyId,
						OtherDocumentTypeId = OtherDocumentTypeId,
						OtherDocNoId = item9.InvGdnId,
						OtherSubDocNoId = item9.InvGdnDetailId,
						ActionId = 1
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
							stockdetail.EntryUser = obj.EntryUserId;
							stockdetail.ModifyUser = obj.ModifyUserId;
							stockdetail.CalcType = "Qty";
							Architecture.Model.pcc.InvSaleInvoiceDetail sid4 = obj.InvSaleInvoiceDetaillist.Find((Architecture.Model.pcc.InvSaleInvoiceDetail x) => x.InvGdnId == stockdetail.OtherDocNoId && x.InvGdnDetailId == stockdetail.OtherSubDocNoId && stockdetail.OtherDocumentTypeId == 1855);
							if (sid4 != null)
							{
								if (stockdetail.QtyOut > 0.0 && sid4.ItemRate > 0m)
								{
									stockdetail.AmountOut = stockdetail.QtyOut * Conversion.ToDouble(sid4.ItemRate);
									stockdetail.CgsAmount = stockdetail.AmountOut;
								}
								stockdetail.ItemRate = Conversion.ToDouble(sid4.ItemRate);
								stockdetail.CgsRate = stockdetail.ItemRate;
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
			else
			{
				InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
				inventoryTransactions.OrganizationId = obj.OrganizationId;
				inventoryTransactions.CompanyId = obj.CompanyId;
				inventoryTransactions.RefDocumentTypeId = obj.DocumentTypeId;
				inventoryTransactions.RefDocIdNo = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
			}
			if (obj.DocumentTypeId == 1856 || obj.DocumentTypeId == 1862)
			{
				InventoryTransactions transactions = new InventoryTransactions();
				transactions.OrganizationId = obj.OrganizationId;
				transactions.CompanyId = obj.CompanyId;
				transactions.RefDocumentTypeId = obj.DocumentTypeId;
				transactions.RefDocIdNo = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			}
			if (obj.DocumentTypeId == 1856 && getGdnId == 0)
			{
				foreach (Architecture.Model.pcc.InvSaleInvoiceDetail item2 in obj.InvSaleInvoiceDetaillist)
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
					sqlCommandInv.Parameters.AddWithValue("@ItemId", item2.ItemId);
					sqlCommandInv.Parameters.AddWithValue("@WarehouseId", item2.WarehouseId);
					if (item2.ItemAttributeVarientId > 0)
					{
						sqlCommandInv.Parameters.AddWithValue("@VarientId", item2.ItemAttributeVarientId);
					}
					sqlCommandInv.Parameters.AddWithValue("@Qty", item2.ItemQty);
					sqlCommandInv.ExecuteNonQuery();
				}
			}
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
			foreach (VoucherDetail item4 in obj.VoucherHeadInvoices.voucherDetailList)
			{
				item4.VoucherHeadId = obj.VoucherHeadInvoices.Id;
				Architecture.Model.pcc.InvSaleInvoiceDetail sid = obj.InvSaleInvoiceDetaillist.Find((Architecture.Model.pcc.InvSaleInvoiceDetail x) => x.LineId == item4.LineId && x.LineId > 0 && item4.LineId > 0);
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
			foreach (VoucherDetail item in obj.VoucherHeadInvoices.voucherDetailList)
			{
				item.VoucherHeadId = obj.VoucherHeadInvoices.Id;
				item.DocumentTypeIdRef = Code;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_H_Insert"));
			}
			DocumentApprovalDetail AP = new DocumentApprovalDetail();
			AP.OrganizationId = obj.OrganizationId;
			AP.CompanyId = obj.CompanyId;
			AP.DocumentTypeId = obj.DocumentTypeId;
			AP.Id = obj.Id;
			AP.LimitAmount = Conversion.ToDecimal(obj.BillAmount);
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, AP, "[DAW].[USp_DocumentApprovalDetail_Insert]"));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.pcc.InvSaleInvoice> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.pcc.InvSaleInvoice> lst = new List<Architecture.Model.pcc.InvSaleInvoice>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.pcc.InvSaleInvoice>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.pcc.InvSaleInvoice item5 in lst)
					{
						List<SqlParameter> lstPrm5 = new List<SqlParameter>();
						lstPrm5.Add(new SqlParameter("@Id", item5.Id));
						lstPrm5.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
						item5.InvSaleInvoiceDetaillist = GenericProvider.Get<Architecture.Model.pcc.InvSaleInvoiceDetail>(sqlConnection2, "[pcc].[USP_InvSaleInvoice_GetAllMethod]", lstPrm5);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.pcc.InvSaleInvoice item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "InvSaleInvoiceExpense_ReadBySaleInvoiceID"));
						item4.InvSaleInvoiceExpenseList = GenericProvider.GetAllDetail<Architecture.Model.pcc.InvSaleInvoiceExpense>(sqlConnection3, "[pcc].[USP_InvSaleInvoice_GetAllMethod]", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.pcc.InvSaleInvoice item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "InvSaleInvoiceFreight_ReadBySaleInvoiceID"));
						item3.InvSaleInvoiceFreightList = GenericProvider.Get<Architecture.Model.pcc.InvSaleInvoiceFreight>(sqlConnection4, "[pcc].[USP_InvSaleInvoice_GetAllMethod]", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection5 = ConnectionObject.SQL())
			{
				sqlConnection5.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.pcc.InvSaleInvoice item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "InvSaleInvoiceJournal_ReadBySaleInvoiceID"));
						item2.InvSaleInvoiceJournalList = GenericProvider.Get<Architecture.Model.pcc.InvSaleInvoiceJournal>(sqlConnection5, "[pcc].[USP_InvSaleInvoice_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null && lst.Count > 0)
			{
				foreach (Architecture.Model.pcc.InvSaleInvoice item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "InvSaleInvoiceWagesDetail_ReadBySaleInvoiceID"));
					item.InvSaleInvoiceWagesDetailList = GenericProvider.Get<InvSaleInvoiceWagesDetail>(sqlcon, "[pcc].[USP_InvSaleInvoice_GetAllMethod]", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public static List<InventoryStockEvalautionDetail> GetInventoryStockEvalautionDetailByOtherIds(ReportsParameters obj)
	{
		try
		{
			new List<InventoryStockEvalautionDetail>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OtherDocumentTypeId",
				Value = obj.OtherDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OtherDocNoId",
				Value = obj.OtherDocNoId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OtherSubDocNoId",
				Value = obj.OtherSubDocNoId
			});
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			return Conversion.ConvertDataTableToList<InventoryStockEvalautionDetail>(GenericProvider.GetDataTableProc<Architecture.Model.Company>("USP_GetInventoryStockEvalautionDetailByOtherIds", p));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
