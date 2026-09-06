using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.FeedMill.Purchase;
using Architecture.Model.Inventory;

namespace Architecture.DAL.FeedMill.Purchase;

public class InvPurchaseInvoice : DALBase
{
	public int SetData(Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice obj, string ProcName)
	{
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.invPurchaseInvoiceDetailList == null || obj.invPurchaseInvoiceDetailList.Count == 0)
			{
				throw new Exception("Detail List not found");
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
			bool WagesChargeToProduct = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "ContractWagesChargetoProduct"));
			foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail item11 in obj.invPurchaseInvoiceDetailList)
			{
				LineIdForPDI = (item11.LineId = LineIdForPDI + 1);
				if (obj.DocumentTypeId == 1116)
				{
					if (WagesChargeToProduct)
					{
						item11.ItemNetAmount = item11.ItemAmount + item11.FreightAmount + item11.ExpenseAmount + item11.WagesAmount - item11.CommissionAmount;
					}
					else
					{
						item11.ItemNetAmount = item11.ItemAmount + item11.FreightAmount + item11.ExpenseAmount - item11.CommissionAmount;
					}
				}
				else if (WagesChargeToProduct)
				{
					item11.ItemNetAmount = item11.ItemAmount + item11.FreightAmount + item11.ExpenseAmount + item11.CommissionAmount + item11.BrokeryAmount + item11.WagesAmount + item11.TaxAmount - item11.FreightDeduction;
				}
				else
				{
					item11.ItemNetAmount = item11.ItemAmount + item11.FreightAmount + item11.ExpenseAmount + item11.CommissionAmount + item11.BrokeryAmount + item11.TaxAmount - item11.FreightDeduction;
				}
				item11.InvPurchaseInvoiceId = obj.Id;
				ItemIds = ItemIds + item11.ItemId + ",";
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item11, "fed.USP_InvPurchaseInvoiceDetail_Insert"));
				item11.Id = code;
			}
			foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceFreight item10 in obj.InvPurchaseInvoiceFreightList)
			{
				item10.InvPurchaseInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item10, "fed.USP_InvPurchaseInvoiceFreight_Insert"));
			}
			foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceJournal item9 in obj.InvPurchaseInvoicejournalList)
			{
				item9.InvPurchaseInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item9, "fed.USP_InvPurchaseInvoiceJournal_Insert"));
			}
			foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceExpense item8 in obj.InvPurchaseInvoiceExpList)
			{
				item8.InvPurchaseInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "fed.USP_InvPurchaseInvoiceExpense_Insert"));
			}
			foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceEmptyBags item7 in obj.InvPurchaseInvoiceEmptyBagslist)
			{
				item7.InvPurchaseInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "fed.USP_InvPurchaseInvoiceEmptyBags_Insert"));
			}
			if (obj.PaymentDuesSchedules != null && obj.PaymentDuesSchedules.Count > 0)
			{
				foreach (PaymentDueSchedule item6 in obj.PaymentDuesSchedules)
				{
					item6.RefdocNoId = obj.Id;
					item6.refDocumentTypeId = obj.DocumentTypeId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "Sp_PaymentDueSchedule_Insert"));
				}
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				foreach (DMSAttachments item5 in obj.AttachmentsList.ToList())
				{
					item5.RefAccountId = obj.SupplierCustomerId;
					item5.RefDocumentTypeId = obj.DocumentTypeId;
					item5.RefDocumentNo = returnID;
					item5.EntryDate = DateTime.Now;
					item5.ModifyDate = DateTime.Now;
					item5.EntryUser = obj.EntryUserId;
					item5.ModifyUser = obj.ModifyUserId;
					item5.OrganizationId = obj.OrganizationId;
					item5.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "Proc_DMSAttachments_Insert"));
				}
			}
			if (CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5) && obj.DocumentTypeId == 1105)
			{
				string RemarksCgs = "";
				List<GetItemGlIdsandItemName> dtitem = (dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId));
				obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
				foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail item4 in obj.invPurchaseInvoiceDetailList.ToList())
				{
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item4.ItemId)).ToList();
					if (find.Count <= 0)
					{
						continue;
					}
					ReportsParameters stock = new ReportsParameters();
					stock.OrganizationId = obj.OrganizationId;
					stock.CompanyId = obj.CompanyId;
					stock.ItemId = item4.ItemId;
					stock.WarehouseId = item4.WarehouseId;
					stock.stockUOM = item4.ItemUOMId;
					stock.DocDate = obj.DocDate;
					stock.JobLotId = item4.JobLotId;
					stock.CropYearId = item4.CropYearId;
					stock.PackingTypeId = item4.PackingTypeId;
					if (obj.ModifyUserId > 0)
					{
						stock.DocumentTypeId = obj.DocumentTypeId;
						stock.Id = obj.Id;
					}
					stock.ItemQty = Conversion.ToDouble(item4.ItemQty);
					stock.NetWeight = Conversion.ToDouble(item4.NetStockWeight);
					stock.RateUOM = item4.RateUomId;
					stock.LineId = item4.LineId;
					stock.ItemName = Conversion.ToString(find[0].ItemName);
					foreach (InventoryStockEvalautionDetail stockdetail in CommonServices.FIFOImplemention(stock, obj.InventoryStockEvalautionDetailslist).InventoryStockEvalautionDetailslist.ToList())
					{
						RemarksCgs = "ItemQty: " + stockdetail.QtyOut + "   " + Conversion.ToString(find[0].ItemName) + "  Net Weight: " + stockdetail.StockWeightOut + "  CGS Rate:" + stockdetail.CgsRate;
						VoucherDetail vd7 = new VoucherDetail();
						vd7.LineId = item4.LineId;
						vd7.AccountId = Conversion.ToInt(find[0].COGSGLAC);
						vd7.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd7.Comments = Conversion.ToString(RemarksCgs + "  " + obj.CommonIdsForFinancials.CompanyName);
						vd7.DebitAmount = Conversion.ToDouble(stockdetail.CgsAmount);
						vd7.ItemId = Conversion.ToInt(item4.ItemId);
						vd7.QtyOut = Conversion.ToDouble(stockdetail.QtyOut);
						vd7.WeightOut = Conversion.ToDouble(stockdetail.StockWeightOut);
						vd7.ItemCgsRate = Conversion.ToDouble(stockdetail.CgsRate);
						vd7.RateCut = Conversion.ToDouble(item4.RateCut);
						vd7.RateCutAmount = Conversion.ToDouble(item4.RateCutAmount);
						vd7.ItemAmount = Conversion.ToDouble(stockdetail.AmountOut);
						vd7.Expenses = Conversion.ToDouble(item4.ExpenseAmount);
						vd7.Commission = Conversion.ToDouble(item4.CommissionAmount);
						vd7.Freight = Conversion.ToDouble(item4.FreightAmount);
						vd7.GpNo = Conversion.ToInt(item4.GpNo);
						vd7.VehicleNo = item4.VehicleNo;
						vd7.JobLotId = Conversion.ToInt(item4.JobLotId);
						vd7.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						obj.VoucherHeadInvoices.voucherDetailList.Add(vd7);
						VoucherDetail vd6 = new VoucherDetail();
						vd6.LineId = item4.LineId;
						vd6.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd6.AgainstAccountId = Conversion.ToInt(find[0].COGSGLAC);
						vd6.Comments = Conversion.ToString(RemarksCgs + "  " + obj.CommonIdsForFinancials.CompanyName);
						vd6.CreditAmount = Conversion.ToDouble(stockdetail.CgsAmount);
						vd6.ItemId = Conversion.ToInt(item4.ItemId);
						vd6.QtyOut = Conversion.ToDouble(stockdetail.QtyOut);
						vd6.WeightOut = Conversion.ToDouble(stockdetail.StockWeightOut);
						vd6.ItemCgsRate = Conversion.ToDouble(stockdetail.CgsRate);
						vd6.RateCut = Conversion.ToDouble(item4.RateCut);
						vd6.RateCutAmount = Conversion.ToDouble(item4.RateCutAmount);
						vd6.ItemAmount = Conversion.ToDouble(stockdetail.AmountOut);
						vd6.Expenses = Conversion.ToDouble(item4.ExpenseAmount);
						vd6.Commission = Conversion.ToDouble(item4.CommissionAmount);
						vd6.Freight = Conversion.ToDouble(item4.FreightAmount);
						vd6.GpNo = Conversion.ToInt(item4.GpNo);
						vd6.VehicleNo = item4.VehicleNo;
						vd6.JobLotId = Conversion.ToInt(item4.JobLotId);
						vd6.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						obj.VoucherHeadInvoices.voucherDetailList.Add(vd6);
						obj.InventoryStockEvalautionDetailslist.Add(stockdetail);
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
					foreach (InventoryStockEvalautionDetail item3 in obj.InventoryStockEvalautionDetailslist)
					{
						item3.OrganizationId = obj.OrganizationId;
						item3.CompanyId = obj.CompanyId;
						item3.DocDate = obj.DocDate;
						item3.DocCodeNo = obj.DocNo;
						item3.SupplierCustomerId = obj.SupplierCustomerId;
						item3.BranchesId = obj.BranchesId;
						item3.RefDocumentTypeId = obj.DocumentTypeId;
						item3.EntryUser = obj.EntryUserId;
						item3.ModifyUser = obj.ModifyUserId;
						item3.CalcType = "Weight";
						Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail pid = obj.invPurchaseInvoiceDetailList.Find((Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail x) => x.LineId == item3.LineId && x.LineId > 0 && item3.LineId > 0);
						if (pid != null)
						{
							double RateUom = CommonServices.GetEqvilentByItemIdAndUomScheduleId(new ReportsParameters
							{
								OrganizationId = obj.OrganizationId,
								CompanyId = obj.CompanyId,
								ItemId = item3.ItemId,
								Id = pid.RateUomId
							});
							if (item3.BillWeightOut > 0.0 && pid.ItemRate > 0m && RateUom > 0.0)
							{
								item3.AmountOut = item3.BillWeightOut / RateUom * Conversion.ToDouble(pid.ItemRate);
							}
							item3.ItemRate = Conversion.ToDouble(pid.ItemRate);
							item3.RefDocIdNo = pid.InvPurchaseInvoiceId;
							item3.RefDocSubIdNo = pid.Id;
						}
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "USP_InventoryStockEvalautionDetail_Insert"));
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
				foreach (VoucherDetail item2 in obj.VoucherHeadInvoices.voucherDetailList)
				{
					item2.VoucherHeadId = obj.VoucherHeadInvoices.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_Insert"));
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
				if (obj.DocumentTypeId == 1104 || obj.DocumentTypeId == 1105)
				{
					InventoryTransactions transactions = new InventoryTransactions();
					transactions.OrganizationId = obj.OrganizationId;
					transactions.CompanyId = obj.CompanyId;
					transactions.RefDocumentTypeId = obj.DocumentTypeId;
					transactions.RefDocIdNo = returnID;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
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
			throw new Exception("VoucherDetail list Not Found");
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice> lst = new List<Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice item6 in lst)
					{
						List<SqlParameter> lstPrm6 = new List<SqlParameter>();
						lstPrm6.Add(new SqlParameter("@Id", item6.Id));
						lstPrm6.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
						item6.invPurchaseInvoiceDetailList = GenericProvider.Get<Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail>(sqlConnection2, "[fed].[USP_InvPurchaseInvoice_GetAllMethod]", lstPrm6);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice item5 in lst)
					{
						List<SqlParameter> lstPrm5 = new List<SqlParameter>();
						lstPrm5.Add(new SqlParameter("@Id", item5.Id));
						lstPrm5.Add(new SqlParameter("@Activity", "InvPurchaseInvoiceExpense_ReadByPurchaseInvoiceID"));
						item5.InvPurchaseInvoiceExpList = GenericProvider.GetAllDetail<Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceExpense>(sqlConnection3, "[fed].[USP_InvPurchaseInvoice_GetAllMethod]", lstPrm5);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "InvPurchaseInvoiceFreight_ReadByPurchaseInvoiceID"));
						item4.InvPurchaseInvoiceFreightList = GenericProvider.Get<Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceFreight>(sqlConnection4, "[fed].[USP_InvPurchaseInvoice_GetAllMethod]", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection5 = ConnectionObject.SQL())
			{
				sqlConnection5.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "InvPurchaseInvoiceJournal_ReadByPurchaseInvoiceID"));
						item3.InvPurchaseInvoicejournalList = GenericProvider.Get<Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceJournal>(sqlConnection5, "[fed].[USP_InvPurchaseInvoice_GetAllMethod]", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection6 = ConnectionObject.SQL())
			{
				sqlConnection6.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "InvPurchaseInvoiceEmptyBags_ReadByPurchaseInvoiceID"));
						item2.InvPurchaseInvoiceEmptyBagslist = GenericProvider.Get<Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceEmptyBags>(sqlConnection6, "[fed].[USP_InvPurchaseInvoice_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@DocumentTypeId", item.DocumentTypeId));
					lstPrm.Add(new SqlParameter("@RefDocNoId", item.Id));
					item.PaymentDuesSchedules = GenericProvider.Get<PaymentDueSchedule>(sqlcon, "Sp_PaymentDueSchedule_ReadAll", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public void AccountandInventoryRemoveById(Architecture.Model.Inventory.InvPurchaseInvoice obj)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Sp_InvoicesVouchersandStocksDelete");
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.BigInt, obj.OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.BigInt, obj.CompanyId);
			sqlDataBase.AddInParameter(cmd, "Id", SqlDbType.BigInt, obj.Id);
			sqlDataBase.AddInParameter(cmd, "DocumentTypeId", SqlDbType.BigInt, obj.DocumentTypeId);
			sqlDataBase.AddInParameter(cmd, "UserId", SqlDbType.Int, obj.EntryUser);
			sqlDataBase.ExecuteNonQuery(cmd);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
