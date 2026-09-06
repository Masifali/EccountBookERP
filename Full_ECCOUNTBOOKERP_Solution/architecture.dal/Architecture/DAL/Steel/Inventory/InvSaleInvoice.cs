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
using Architecture.Model.Steel.Inventory;
using Architecture.Model.VPS;

namespace Architecture.DAL.Steel.Inventory;

public class InvSaleInvoice : DALBase
{
	public int SetData(Architecture.Model.Steel.Inventory.InvSaleInvoice obj, string ProcName)
	{
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		using SqlConnection sqlcon = ConnectionObject.SQL();
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
			foreach (Architecture.Model.Steel.Inventory.InvSaleInvoiceDetail item6 in obj.InvSaleInvoiceDetaillist)
			{
				item6.InvSaleInvoiceId = obj.Id;
				item6.ItemNetAmount = item6.ItemAmount + item6.ExpenseAmount - item6.CommissionAmount;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "ST.USP_InvSaleInvoiceDetail_Insert"));
				item6.Id = code;
			}
			foreach (Architecture.Model.Steel.Inventory.InvSaleInvoiceFreight item5 in obj.InvSaleInvoiceFreightList)
			{
				item5.InvSaleInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "[ST].[USP_InvSaleInvoiceFreight_Insert]"));
			}
			foreach (Architecture.Model.Steel.Inventory.InvSaleInvoiceJournal item4 in obj.InvSaleInvoiceJournalList)
			{
				item4.InvSaleInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[ST].[USP_InvSaleInvoiceJournal_Insert]"));
			}
			foreach (Architecture.Model.Steel.Inventory.InvSaleInvoiceExpense item3 in obj.InvSaleInvoiceExpenseList)
			{
				item3.InvSaleInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "[ST].[USP_InvSaleInvoiceExpense_Insert]"));
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
					item2.RefAccountId = obj.SupplierCustomerId;
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
			foreach (VoucherDetail item7 in obj.VoucherHeadInvoices.voucherDetailList)
			{
				Architecture.Model.Steel.Inventory.InvSaleInvoiceDetail sid = obj.InvSaleInvoiceDetaillist.Find((Architecture.Model.Steel.Inventory.InvSaleInvoiceDetail x) => x.LineId == item7.LineId && x.LineId > 0 && item7.LineId > 0);
				item7.VoucherHeadId = obj.VoucherHeadInvoices.Id;
				if (sid != null)
				{
					item7.RefDocSubIdNo = sid.Id;
				}
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "Sp_VoucherDetail_Insert"));
			}
			if (obj.VoucherHeadInvoices.voucherDetailList == null || obj.VoucherHeadInvoices.voucherDetailList.Count == 0)
			{
				throw new Exception("Voucher Detail list Not Found");
			}
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "USP_VoucherBalanceCheck";
			sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
			sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
			sqlCommand.Parameters.AddWithValue("@Id", obj.VoucherHeadInvoices.Id);
			sqlCommand.ExecuteScalar();
			int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_H_Insert"));
			foreach (VoucherDetail item in obj.VoucherHeadInvoices.voucherDetailList)
			{
				item.VoucherHeadId = obj.VoucherHeadInvoices.Id;
				item.DocumentTypeIdRef = Code;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_H_Insert"));
			}
			InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
			inventoryTransactions.OrganizationId = obj.OrganizationId;
			inventoryTransactions.CompanyId = obj.CompanyId;
			inventoryTransactions.RefDocumentTypeId = obj.DocumentTypeId;
			inventoryTransactions.RefDocIdNo = obj.Id;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
			if (obj.DocumentTypeId == 1510)
			{
				InventoryTransactions transactions = new InventoryTransactions();
				transactions.OrganizationId = obj.OrganizationId;
				transactions.CompanyId = obj.CompanyId;
				transactions.RefDocumentTypeId = obj.DocumentTypeId;
				transactions.RefDocIdNo = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
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

	private void MakeCGSVoucher(Architecture.Model.Steel.Inventory.InvSaleInvoice obj)
	{
		try
		{
			List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
			dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId);
			if (obj.InvSaleInvoiceDetaillist.Count <= 0)
			{
				return;
			}
			foreach (Architecture.Model.Steel.Inventory.InvSaleInvoiceDetail item in obj.InvSaleInvoiceDetaillist.ToList())
			{
				_ = string.Empty;
				if (dtitem.Count > 0)
				{
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.ItemId)).ToList();
					if (find.Count > 0)
					{
						if (obj.DocumentTypeId == 60)
						{
							continue;
						}
						bool CgsAllow = false;
						string ConfigKey = CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CGSEntryAllow");
						if (ConfigKey != string.Empty)
						{
							CgsAllow = Conversion.ToBool(ConfigKey);
						}
						if (!CgsAllow)
						{
							continue;
						}
						ReportsParameters parameters = new ReportsParameters();
						parameters.OrganizationId = obj.OrganizationId;
						parameters.CompanyId = obj.CompanyId;
						parameters.DocDate = obj.DocDate;
						parameters.ItemId = item.ItemId;
						parameters.JobLotId = item.JobLotId;
						parameters.WarehouseId = item.WarehouseId;
						item.ItemCgsRate = Conversion.ToDecimal(CommonServices.AvgRateOnlyForCGS(parameters));
						if (item.ItemCgsRate <= 0m)
						{
							throw new Exception("CGS Rate not found");
						}
						decimal CgsAmount = default(decimal);
						string RemarksCgs = "";
						decimal RateUom = Conversion.ToDecimal(CommonServices.GetEqvilentByItemIdAndUomScheduleId(new ReportsParameters
						{
							OrganizationId = obj.OrganizationId,
							CompanyId = obj.CompanyId,
							ItemId = item.ItemId,
							Id = item.RateUomScheduleId
						}));
						if (obj.DocumentTypeId == 128)
						{
							RemarksCgs = "NoOfBags: " + item.ItemQty + "   " + Conversion.ToString(find[0].ItemName) + "  Net Weight: " + item.NetBillWeight + "  Rate:" + item.ItemCgsRate;
							if (item.ExpenseAmount > 0m)
							{
								RemarksCgs = RemarksCgs + "   Expense Amount: " + item.ExpenseAmount;
							}
							if (item.CommissionAmount > 0m)
							{
								RemarksCgs = RemarksCgs + "    Commission Amount: " + item.CommissionAmount;
							}
							CgsAmount = item.ItemCgsRate * item.ItemQty;
						}
						else if (obj.DocumentTypeId == 126 || obj.DocumentTypeId == 133)
						{
							RemarksCgs = "ItemQty: " + item.ItemQty + "   " + Conversion.ToString(find[0].ItemName) + "  Net Weight: " + item.NetBillWeight + "  Rate:" + item.ItemCgsRate;
							if (item.FreightAmount > 0m)
							{
								RemarksCgs = RemarksCgs + "   Credit Amount: " + item.FreightAmount;
							}
							if (item.CommissionAmount > 0m)
							{
								RemarksCgs = RemarksCgs + "    Commission Amount: " + item.CommissionAmount;
							}
							CgsAmount = item.ItemCgsRate * item.ItemQty;
						}
						else if (obj.DocumentTypeId != 128 && obj.DocumentTypeId != 126 && obj.DocumentTypeId != 133)
						{
							if (item.ItemCgsRate > 0m && item.NetBillWeight > 0m && RateUom > 0m)
							{
								item.ItemCgsRate = RateUom * item.ItemCgsRate;
								CgsAmount = item.NetBillWeight / RateUom * item.ItemCgsRate;
							}
							else
							{
								CgsAmount = item.NetBillWeight * item.ItemCgsRate;
							}
							RemarksCgs = "NoOfBags: " + item.ItemQty + "   " + Conversion.ToString(find[0].ItemName) + "  GrossWeight: " + item.GrossWeight + "  Net Weight: " + item.NetBillWeight + "  Rate:" + item.ItemCgsRate;
							if (item.ExpenseAmount > 0m)
							{
								RemarksCgs = RemarksCgs + "   Expense Amount: " + item.ExpenseAmount;
							}
							if (item.CommissionAmount > 0m)
							{
								RemarksCgs = RemarksCgs + "    Commission Amount: " + item.CommissionAmount;
							}
							if (item.NetBillWeight == 0m)
							{
								throw new Exception("NetBillWeight Not Found Please Check");
							}
						}
						VoucherDetail vd6 = new VoucherDetail();
						vd6.LineId = item.LineId;
						vd6.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd6.AgainstAccountId = Conversion.ToInt(find[0].COGSGLAC);
						vd6.Comments = Conversion.ToString(RemarksCgs + "  " + obj.CommonIdsForFinancials.CompanyName);
						vd6.CreditAmount = Conversion.ToDouble(CgsAmount);
						vd6.DocumentTypeIdRef = Conversion.ToInt(item.RefRefDocumentTypeId);
						vd6.InvoiceNoRefId = Conversion.ToInt(item.RefRefDocIdNo);
						vd6.RefInvoiceNo = Conversion.ToString(item.RefDocSubId);
						vd6.ItemId = Conversion.ToInt(item.ItemId);
						vd6.QtyOut = Conversion.ToDouble(item.ItemQty);
						vd6.WeightOut = Conversion.ToDouble(item.NetBillWeight);
						vd6.ItemCgsRate = Conversion.ToDouble(item.ItemCgsRate);
						vd6.RateCut = Conversion.ToDouble(item.RateCut);
						vd6.RateCutAmount = Conversion.ToDouble(item.RateCutAmount);
						vd6.ItemAmount = Conversion.ToDouble(CgsAmount);
						vd6.Expenses = Conversion.ToDouble(item.ExpenseAmount);
						vd6.Commission = Conversion.ToDouble(item.CommissionAmount);
						vd6.Freight = Conversion.ToDouble(item.FreightAmount);
						vd6.GpNo = Conversion.ToInt(item.GpNo);
						vd6.VehicleNo = item.VehicleNo;
						vd6.JobLotId = Conversion.ToInt(item.JobLotId);
						vd6.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						obj.VoucherHeadInvoices.voucherDetailList.Add(vd6);
						VoucherDetail vd7 = new VoucherDetail();
						vd7.LineId = item.LineId;
						vd7.AccountId = Conversion.ToInt(find[0].COGSGLAC);
						vd7.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd7.Comments = Conversion.ToString(RemarksCgs + "  " + obj.CommonIdsForFinancials.CompanyName);
						vd7.DebitAmount = Conversion.ToDouble(CgsAmount);
						vd7.DocumentTypeIdRef = Conversion.ToInt(item.RefRefDocumentTypeId);
						vd7.InvoiceNoRefId = Conversion.ToInt(item.RefRefDocIdNo);
						vd7.RefInvoiceNo = Conversion.ToString(item.RefDocSubId);
						vd7.ItemId = Conversion.ToInt(item.ItemId);
						vd7.QtyOut = Conversion.ToDouble(item.ItemQty);
						vd7.WeightOut = Conversion.ToDouble(item.NetBillWeight);
						vd7.ItemCgsRate = Conversion.ToDouble(item.ItemCgsRate);
						vd7.RateCut = Conversion.ToDouble(item.RateCut);
						vd7.RateCutAmount = Conversion.ToDouble(item.RateCutAmount);
						vd7.ItemAmount = Conversion.ToDouble(CgsAmount);
						vd7.Expenses = Conversion.ToDouble(item.ExpenseAmount);
						vd7.Commission = Conversion.ToDouble(item.CommissionAmount);
						vd7.Freight = Conversion.ToDouble(item.FreightAmount);
						vd7.GpNo = Conversion.ToInt(item.GpNo);
						vd7.VehicleNo = item.VehicleNo;
						vd7.JobLotId = Conversion.ToInt(item.JobLotId);
						vd7.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						obj.VoucherHeadInvoices.voucherDetailList.Add(vd7);
						continue;
					}
					throw new Exception("Item Record Not found");
				}
				throw new Exception("Item Record Not found");
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public List<Architecture.Model.Steel.Inventory.InvSaleInvoice> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Steel.Inventory.InvSaleInvoice> lst = new List<Architecture.Model.Steel.Inventory.InvSaleInvoice>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Steel.Inventory.InvSaleInvoice>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.Steel.Inventory.InvSaleInvoice item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
						item4.InvSaleInvoiceDetaillist = GenericProvider.Get<Architecture.Model.Steel.Inventory.InvSaleInvoiceDetail>(sqlConnection2, "ST.USP_SaleInvoice_GetAllMethod", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.Steel.Inventory.InvSaleInvoice item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "InvSaleInvoiceExpense_ReadBySaleInvoiceID"));
						item3.InvSaleInvoiceExpenseList = GenericProvider.GetAllDetail<Architecture.Model.Steel.Inventory.InvSaleInvoiceExpense>(sqlConnection3, "ST.USP_SaleInvoice_GetAllMethod", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.Steel.Inventory.InvSaleInvoice item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "InvSaleInvoiceFreight_ReadBySaleInvoiceID"));
						item2.InvSaleInvoiceFreightList = GenericProvider.Get<Architecture.Model.Steel.Inventory.InvSaleInvoiceFreight>(sqlConnection4, "ST.USP_SaleInvoice_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null && lst.Count > 0)
			{
				foreach (Architecture.Model.Steel.Inventory.InvSaleInvoice item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "InvSaleInvoiceJournal_ReadBySaleInvoiceID"));
					item.InvSaleInvoiceJournalList = GenericProvider.Get<Architecture.Model.Steel.Inventory.InvSaleInvoiceJournal>(sqlcon, "ST.USP_SaleInvoice_GetAllMethod", lstPrm);
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
