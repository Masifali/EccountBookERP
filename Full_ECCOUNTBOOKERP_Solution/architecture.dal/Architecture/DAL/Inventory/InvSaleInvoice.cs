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
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.VPS;

namespace Architecture.DAL.Inventory;

public class InvSaleInvoice : DALBase
{
	public int SetData(Architecture.Model.Inventory.InvSaleInvoice obj, string ProcName)
	{
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
		new List<Architecture.Model.Inventory.SupplierCustomer>();
		List<GetJobLotGlIdsandName> lstJobLot = new List<GetJobLotGlIdsandName>();
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
				obj.AttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.Attachment));
				obj.CustomAttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.UploadedFileCustomName));
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
			int LineId = 1;
			bool loaderflag = false;
			foreach (InvSaleInvoiceDetail item15 in obj.InvSaleInvoiceDetaillist)
			{
				if (item15.RefRefDocumentTypeId > 0 && item15.RefRefDocIdNo > 0 && item15.RefDocSubId > 0)
				{
					loaderflag = true;
				}
				item15.LineId = LineId;
				item15.InvSaleInvoiceId = obj.Id;
				item15.BillAmount = item15.ItemAmount + item15.ExpenseAmount - item15.CommissionAmount - item15.FreightAmount;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item15, "Sp_InvSaleInvoiceDetail_Insert"));
				item15.Id = code;
				LineId++;
			}
			if (obj.InvSaleInvoiceFreightList != null && obj.InvSaleInvoiceFreightList.Count > 0)
			{
				foreach (InvSaleInvoiceFreight item14 in obj.InvSaleInvoiceFreightList)
				{
					item14.InvSaleInvoiceId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item14, "Sp_InvSaleInvoiceFreight_Insert"));
				}
			}
			if (obj.InvSaleInvoiceJournalList != null && obj.InvSaleInvoiceJournalList.Count > 0)
			{
				foreach (InvSaleInvoiceJournal item13 in obj.InvSaleInvoiceJournalList)
				{
					item13.InvSaleInvoiceId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item13, "Sp_InvSaleInvoiceJournal_Insert"));
				}
			}
			if (obj.InvSaleInvoiceExpenseList != null && obj.InvSaleInvoiceExpenseList.Count > 0)
			{
				foreach (InvSaleInvoiceExpense item12 in obj.InvSaleInvoiceExpenseList)
				{
					item12.InvSaleInvoiceId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item12, "Sp_InvSaleInvoiceExpense_Insert"));
				}
			}
			if (obj.InvSaleInvoiceCommission != null && obj.InvSaleInvoiceCommission.Count > 0)
			{
				foreach (InvSaleInvoiceCommission item11 in obj.InvSaleInvoiceCommission)
				{
					item11.InvSaleInvoiceId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item11, "Sp_InvSaleInvoiceCommission_Insert"));
				}
			}
			if (obj.SaleInvoicePaymentTermsDetailslist != null && obj.SaleInvoicePaymentTermsDetailslist.Count > 0)
			{
				int sortno = 1;
				foreach (SaleInvoicePaymentTermsDetail item10 in obj.SaleInvoicePaymentTermsDetailslist)
				{
					item10.InvSaleInvoiceId = obj.Id;
					item10.SortNo = sortno;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item10, "USP_SaleInvoicePaymentTermsDetail_Insert"));
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
				foreach (DMSAttachments item9 in obj.AttachmentsList.ToList())
				{
					item9.RefAccountId = obj.SupplierCustomerId;
					item9.RefDocumentTypeId = obj.DocumentTypeId;
					item9.RefDocumentNo = returnID;
					item9.EntryDate = DateTime.Now;
					item9.ModifyDate = DateTime.Now;
					item9.EntryUser = obj.EntryUser;
					item9.ModifyUser = obj.ModifyUser;
					item9.OrganizationId = obj.OrganizationId;
					item9.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item9, "Proc_DMSAttachments_Insert"));
				}
			}
			List<Architecture.Model.Inventory.GetItemGlIdsandItemName> dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId);
			lstJobLot = CommonServices.GetJobLotGlIdsandName(obj.OrganizationId, obj.CompanyId);
			bool num = !new HashSet<int> { 103, 126, 133, 145 }.Contains(obj.DocumentTypeId) && CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5);
			bool SaleMinusAllowedAgainstFifo = CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 14);
			bool InventoryFinancialsEffectsInActive = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "InventoryFinancialsEffectsInActive"));
			bool FifoEntryAllow = !(obj.DocumentTypeId == 99 && SaleMinusAllowedAgainstFifo);
			bool num2 = num && !loaderflag && obj.DocumentTypeId != 1611 && obj.DocumentTypeId != 1662;
			bool isSpecialDocument = false;
			if (num2 || isSpecialDocument)
			{
				bool gdnexists = obj.DocumentTypeId == 99 && obj.InvSaleInvoiceDetaillist.Exists((InvSaleInvoiceDetail x) => x.InvGdnId > 0);
				if (obj.DocumentTypeId == 95 || obj.DocumentTypeId == 171 || gdnexists)
				{
					int refDocumentTypeId = obj.DocumentTypeId switch
					{
						171 => obj.InvSaleInvoiceDetaillist.Exists((InvSaleInvoiceDetail x) => x.SaleOrderId > 0) ? 170 : 221, 
						95 => 86, 
						_ => 124, 
					};
					obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
					foreach (InvSaleInvoiceDetail item16 in obj.InvSaleInvoiceDetaillist)
					{
						List<InventoryStockEvalautionDetail> lstOtherRef = GetOtherReferences(obj, item16, refDocumentTypeId);
						if (lstOtherRef == null || lstOtherRef.Count == 0)
						{
							throw new Exception("InventoryStockEvalautionDetailslist not found other reference.");
						}
						if (lstOtherRef != null && lstOtherRef.Count > 0)
						{
							bool jobLotFlag = HasJobLotAccount(lstJobLot, Conversion.ToInt(item16.JobLotId));
							foreach (InventoryStockEvalautionDetail stockdetail in lstOtherRef.ToList())
							{
								List<Architecture.Model.Inventory.GetItemGlIdsandItemName> find = dtitem.Where((Architecture.Model.Inventory.GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item16.ItemId)).ToList();
								if (find.Count == 0)
								{
									throw new Exception("ItemId Not Found Against CGS Transaction");
								}
								if (!jobLotFlag && !InventoryFinancialsEffectsInActive)
								{
									string remarksCgs = $"ItemQty: {stockdetail.QtyOut} {find[0].ItemName} Net Weight: {stockdetail.StockWeightOut} CGS Rate: {stockdetail.CgsRate} {obj.CommonIdsForFinancials.CompanyName}";
									obj.VoucherHeadInvoices.voucherDetailList.Add(CreateCgsVoucherDetail(item16, stockdetail, remarksCgs, Conversion.ToInt(find[0].COGSGLAC), Conversion.ToInt(find[0].PurchaseGLAC), Conversion.ToDouble(stockdetail.CgsAmount), 0.0, Conversion.ToInt(obj.SupplierCustomerId)));
									obj.VoucherHeadInvoices.voucherDetailList.Add(CreateCgsVoucherDetail(item16, stockdetail, remarksCgs, Conversion.ToInt(find[0].PurchaseGLAC), Conversion.ToInt(find[0].COGSGLAC), 0.0, Conversion.ToDouble(stockdetail.CgsAmount), Conversion.ToInt(obj.SupplierCustomerId)));
								}
								UpdateStockDetail(stockdetail, item16, obj, refDocumentTypeId);
								obj.InventoryStockEvalautionDetailslist.Add(stockdetail);
							}
							continue;
						}
						throw new Exception("InventoryStockEvalautionDetailslist not found against CGS other reference");
					}
					if (obj.InventoryStockEvalautionDetailslist != null && obj.InventoryStockEvalautionDetailslist.Count > 0)
					{
						foreach (InventoryStockEvalautionDetail item4 in obj.InventoryStockEvalautionDetailslist.ToList())
						{
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "USP_InventoryStockEvalautionDetailGdnReferences_Update"));
						}
					}
				}
				else if (obj.DocumentTypeId != 222)
				{
					if (FifoEntryAllow)
					{
						obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
						foreach (InvSaleInvoiceDetail item6 in obj.InvSaleInvoiceDetaillist.ToList())
						{
							List<Architecture.Model.Inventory.GetItemGlIdsandItemName> find2 = dtitem.Where((Architecture.Model.Inventory.GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item6.ItemId)).ToList();
							if (find2.Count <= 0)
							{
								continue;
							}
							ReportsParameters obj2 = CreateStockReportParameter(obj, item6, Conversion.ToString(find2[0].ItemName));
							bool jobLotFlag2 = HasJobLotAccount(lstJobLot, Conversion.ToInt(item6.JobLotId));
							foreach (InventoryStockEvalautionDetail stockdetail2 in CommonServices.FIFOImplemention(obj2, obj.InventoryStockEvalautionDetailslist).InventoryStockEvalautionDetailslist.ToList())
							{
								if (!jobLotFlag2 && !InventoryFinancialsEffectsInActive)
								{
									string remarksCgs2 = $"ItemQty: {stockdetail2.QtyOut} {find2[0].ItemName} Net Weight: {stockdetail2.StockWeightOut} CGS Rate: {stockdetail2.CgsRate} {obj.CommonIdsForFinancials.CompanyName}";
									obj.VoucherHeadInvoices.voucherDetailList.Add(CreateCgsVoucherDetail(item6, stockdetail2, remarksCgs2, Conversion.ToInt(find2[0].COGSGLAC), Conversion.ToInt(find2[0].PurchaseGLAC), Conversion.ToDouble(stockdetail2.CgsAmount), 0.0, Conversion.ToInt(obj.SupplierCustomerId)));
									obj.VoucherHeadInvoices.voucherDetailList.Add(CreateCgsVoucherDetail(item6, stockdetail2, remarksCgs2, Conversion.ToInt(find2[0].PurchaseGLAC), Conversion.ToInt(find2[0].COGSGLAC), 0.0, Conversion.ToDouble(stockdetail2.CgsAmount), Conversion.ToInt(obj.SupplierCustomerId)));
								}
								if (obj.DocumentTypeId == 126)
								{
									stockdetail2.ItemUom = item6.ItemUOMId;
									stockdetail2.JobLotId = item6.JobLotId;
								}
								obj.InventoryStockEvalautionDetailslist.Add(stockdetail2);
							}
						}
						if (obj.InventoryStockEvalautionDetailslist != null && obj.InventoryStockEvalautionDetailslist.Count > 0)
						{
							if (obj.ModifyUser > 0)
							{
								ReverseInventoryQty(sqlcon, sqlTrn, obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.Id);
							}
							foreach (InventoryStockEvalautionDetail item7 in obj.InventoryStockEvalautionDetailslist)
							{
								PrepareStockEvaluationDetail(item7, obj);
								Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "USP_InventoryStockEvalautionDetail_Insert"));
							}
						}
					}
					else if (obj.ModifyUser > 0)
					{
						ReverseInventoryQty(sqlcon, sqlTrn, obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.Id);
					}
				}
			}
			else
			{
				bool gdnExists = (obj.DocumentTypeId == 99 || obj.DocumentTypeId == 145 || obj.DocumentTypeId == 126) && obj.InvSaleInvoiceDetaillist.Exists((InvSaleInvoiceDetail x) => x.InvGdnId > 0);
				if (obj.DocumentTypeId != 95 && obj.DocumentTypeId != 171 && obj.DocumentTypeId != 222 && obj.DocumentTypeId != 1660 && !gdnExists)
				{
					UpdateInventoryReference(sqlTrn, obj);
				}
				else
				{
					obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
					int refDocumentTypeId2 = GetReferenceDocumentTypeId(obj, gdnExists);
					foreach (InvSaleInvoiceDetail item8 in obj.InvSaleInvoiceDetaillist)
					{
						List<InventoryStockEvalautionDetail> details = GetOtherReferences(obj, item8, refDocumentTypeId2);
						if (details == null || details.Count == 0)
						{
							throw new Exception("InventoryStockEvalautionDetailslist not found other reference.");
						}
						foreach (InventoryStockEvalautionDetail stockDetail in details)
						{
							PrepareGdnReferenceStockDetail(stockDetail, item8, obj, refDocumentTypeId2);
							obj.InventoryStockEvalautionDetailslist.Add(stockDetail);
						}
					}
					if (obj.InventoryStockEvalautionDetailslist != null && obj.InventoryStockEvalautionDetailslist.Count > 0)
					{
						SaveInventoryStockEvaluationDetails(sqlTrn, obj.InventoryStockEvalautionDetailslist);
					}
				}
			}
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "usp_StockInTransit_VoucherDelete_ByGdnId";
			sqlCommand.Parameters.AddWithValue("@Id", obj.Id);
			sqlCommand.ExecuteNonQuery();
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
			bool exists = obj.DocumentTypeId == 126 && obj.InvSaleInvoiceDetaillist.Exists((InvSaleInvoiceDetail p) => p.InvGdnId > 0);
			if (obj.DocumentTypeId != 95 && obj.DocumentTypeId != 1611 && obj.DocumentTypeId != 1662 && obj.DocumentTypeId != 1660 && !exists && FifoEntryAllow)
			{
				foreach (InvSaleInvoiceDetail item3 in obj.InvSaleInvoiceDetaillist)
				{
					SqlCommand sqlCommand2 = new SqlCommand();
					sqlCommand2.Connection = sqlcon;
					sqlCommand2.Transaction = sqlTrn;
					sqlCommand2.CommandType = CommandType.StoredProcedure;
					sqlCommand2.CommandText = "USP_InventoryValidation";
					sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand2.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
					sqlCommand2.Parameters.AddWithValue("@DocDate", obj.DocDate);
					sqlCommand2.Parameters.AddWithValue("@ItemId", item3.ItemId);
					sqlCommand2.Parameters.AddWithValue("@WarehouseId", item3.WarehouseId);
					sqlCommand2.Parameters.AddWithValue("@JobLotId", item3.JobLotId);
					sqlCommand2.Parameters.AddWithValue("@CropYear", item3.CropYear);
					sqlCommand2.Parameters.AddWithValue("@InvPackingTypeId", item3.PackingTypeId);
					sqlCommand2.Parameters.AddWithValue("@PackUomId", item3.ItemUOMId);
					sqlCommand2.Parameters.AddWithValue("@NetWeight", item3.NetStockWeight);
					sqlCommand2.Parameters.AddWithValue("@RefDocumentTypeId", item3.RefRefDocumentTypeId);
					sqlCommand2.Parameters.AddWithValue("@RefDocNoId", item3.RefRefDocIdNo);
					sqlCommand2.Parameters.AddWithValue("@RefDocSubIdNo", item3.RefDocSubId);
					sqlCommand2.Parameters.AddWithValue("@ItemConditionId", item3.ItemConditionId);
					sqlCommand2.ExecuteNonQuery();
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
			foreach (VoucherDetail item5 in obj.VoucherHeadInvoices.voucherDetailList)
			{
				item5.VoucherHeadId = obj.VoucherHeadInvoices.Id;
				item5.BranchesId = obj.BranchesId;
				InvSaleInvoiceDetail sid = obj.InvSaleInvoiceDetaillist.Find((InvSaleInvoiceDetail x) => x.LineId == item5.LineId && x.LineId > 0 && item5.LineId > 0);
				if (sid != null)
				{
					item5.RefDocSubIdNo = sid.Id;
				}
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "Sp_VoucherDetail_Insert"));
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
			sqlCommand3.ExecuteScalar();
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

	public List<Architecture.Model.Inventory.InvSaleInvoice> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.InvSaleInvoice> lst = new List<Architecture.Model.Inventory.InvSaleInvoice>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Inventory.InvSaleInvoice>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.Inventory.InvSaleInvoice item6 in lst)
					{
						if (item6.DocumentTypeId == 96)
						{
							List<SqlParameter> lstPrm6 = new List<SqlParameter>();
							lstPrm6.Add(new SqlParameter("@Id", item6.Id));
							lstPrm6.Add(new SqlParameter("@Activity", "SaleDetailTradingReadByInvSaleInvoiceId"));
							item6.InvSaleInvoiceDetaillist = GenericProvider.Get<InvSaleInvoiceDetail>(sqlConnection2, "Sp_InvSaleInvoice_GetAllMethod", lstPrm6);
						}
						else
						{
							List<SqlParameter> lstPrm7 = new List<SqlParameter>();
							lstPrm7.Add(new SqlParameter("@Id", item6.Id));
							lstPrm7.Add(new SqlParameter("@Activity", "SaleDetailReadByInvSaleInvoiceId"));
							item6.InvSaleInvoiceDetaillist = GenericProvider.Get<InvSaleInvoiceDetail>(sqlConnection2, "Sp_InvSaleInvoice_GetAllMethod", lstPrm7);
						}
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.Inventory.InvSaleInvoice item5 in lst)
					{
						List<SqlParameter> lstPrm5 = new List<SqlParameter>();
						lstPrm5.Add(new SqlParameter("@Id", item5.Id));
						lstPrm5.Add(new SqlParameter("@Activity", "InvSaleInvoiceExpense_ReadBySaleInvoiceID"));
						item5.InvSaleInvoiceExpenseList = GenericProvider.GetAllDetail<InvSaleInvoiceExpense>(sqlConnection3, "Sp_InvSaleInvoice_GetAllMethod", lstPrm5);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.Inventory.InvSaleInvoice item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "InvSaleInvoiceFreight_ReadBySaleInvoiceID"));
						item4.InvSaleInvoiceFreightList = GenericProvider.Get<InvSaleInvoiceFreight>(sqlConnection4, "Sp_InvSaleInvoice_GetAllMethod", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection5 = ConnectionObject.SQL())
			{
				sqlConnection5.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.Inventory.InvSaleInvoice item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "InvSaleInvoiceJournal_ReadBySaleInvoiceID"));
						item3.InvSaleInvoiceJournalList = GenericProvider.Get<InvSaleInvoiceJournal>(sqlConnection5, "Sp_InvSaleInvoice_GetAllMethod", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection6 = ConnectionObject.SQL())
			{
				sqlConnection6.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.Inventory.InvSaleInvoice item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "InvSaleInvoiceCommission_ReadBySaleInvoiceID"));
						item2.InvSaleInvoiceCommission = GenericProvider.Get<InvSaleInvoiceCommission>(sqlConnection6, "Sp_InvSaleInvoice_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null && lst.Count > 0)
			{
				foreach (Architecture.Model.Inventory.InvSaleInvoice item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "InvSaleInvoicePaymentTerm_ReadBySaleInvoiceID"));
					item.SaleInvoicePaymentTermsDetailslist = GenericProvider.Get<SaleInvoicePaymentTermsDetail>(sqlcon, "Sp_InvSaleInvoice_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public void AccountandInventoryRemoveById(Architecture.Model.Inventory.InvSaleInvoice obj)
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

	private bool HasJobLotAccount(List<GetJobLotGlIdsandName> jobLots, int jobLotId)
	{
		GetJobLotGlIdsandName getJobLotGlIdsandName = jobLots.FirstOrDefault((GetJobLotGlIdsandName x) => x.Id == jobLotId);
		if (getJobLotGlIdsandName == null)
		{
			return false;
		}
		return getJobLotGlIdsandName.AccountId > 0;
	}

	private VoucherDetail CreateCgsVoucherDetail(InvSaleInvoiceDetail item, InventoryStockEvalautionDetail stockDetail, string remarks, int accountId, int againstAccountId, double debitAmount, double creditAmount, int supplierCustomerId)
	{
		return new VoucherDetail
		{
			LineId = item.LineId,
			IsCGS = 1,
			AccountId = accountId,
			AgainstAccountId = againstAccountId,
			Comments = remarks,
			DebitAmount = debitAmount,
			CreditAmount = creditAmount,
			RefDocumentTypeId = Conversion.ToInt(stockDetail.RefRefDocumentTypeId),
			RefDocNoId = Conversion.ToInt(stockDetail.RefRefDocIdNo),
			RefDocNoDetailId = Conversion.ToInt(stockDetail.RefRefDocSubIdNo),
			ItemId = Conversion.ToInt(item.ItemId),
			QtyOut = Conversion.ToDouble(stockDetail.QtyOut),
			WeightOut = Conversion.ToDouble(stockDetail.StockWeightOut),
			ItemCgsRate = Conversion.ToDouble(stockDetail.CgsRate),
			RateCut = Conversion.ToDouble(item.RateCut),
			RateCutAmount = Conversion.ToDouble(item.RateCutAmount),
			ItemAmount = Conversion.ToDouble(stockDetail.AmountOut),
			Expenses = Conversion.ToDouble(item.ExpenseAmount),
			Commission = Conversion.ToDouble(item.CommissionAmount),
			Freight = Conversion.ToDouble(item.FreightAmount),
			OrderNo = Conversion.ToInt(item.SaleOrder),
			GpNo = Conversion.ToInt(item.GpNo),
			VehicleNo = item.VehicleNo,
			JobLotId = Conversion.ToInt(item.JobLotId),
			SupplierCustomerId = supplierCustomerId,
			BranchesId = item.BranchId,
			CostCenterId = item.CostCenterId
		};
	}

	private void UpdateStockDetail(InventoryStockEvalautionDetail stockDetail, InvSaleInvoiceDetail item, Architecture.Model.Inventory.InvSaleInvoice obj, int refDocumentTypeId)
	{
		stockDetail.OrganizationId = obj.OrganizationId;
		stockDetail.CompanyId = obj.CompanyId;
		stockDetail.DocDate = obj.DocDate;
		stockDetail.DocCodeNo = obj.DocNo;
		stockDetail.SupplierCustomerId = obj.SupplierCustomerId;
		stockDetail.RefDocumentTypeId = obj.DocumentTypeId;
		stockDetail.BranchesId = obj.BranchesId;
		stockDetail.EntryUser = obj.EntryUser;
		stockDetail.ModifyUser = obj.ModifyUser;
		stockDetail.CalcType = "Weight";
		InvSaleInvoiceDetail sid = obj.InvSaleInvoiceDetaillist.FirstOrDefault((InvSaleInvoiceDetail x) => x.InvGdnId == stockDetail.OtherDocNoId && x.InvGdnDetailId == stockDetail.OtherSubDocNoId && stockDetail.OtherDocumentTypeId == refDocumentTypeId);
		if (sid != null)
		{
			double rateUom = CommonServices.GetEqvilentByItemIdAndUomScheduleId(new ReportsParameters
			{
				OrganizationId = obj.OrganizationId,
				CompanyId = obj.CompanyId,
				ItemId = item.ItemId,
				Id = sid.UomScheduleIdRate
			});
			if (stockDetail.BillWeightOut > 0.0 && sid.ItemRate > 0.0 && rateUom > 0.0)
			{
				stockDetail.AmountOut = stockDetail.BillWeightOut / rateUom * sid.ItemRate;
				double expAmount = ((item.ExpenseAmount > 0.0 && item.ItemQty > 0.0 && stockDetail.QtyOut > 0.0) ? (item.ExpenseAmount / item.ItemQty * stockDetail.QtyOut) : 0.0);
				double freight = ((item.FreightAmount > 0.0 && item.NetBillWeight > 0.0 && stockDetail.BillWeightOut > 0.0) ? (item.FreightAmount / item.NetBillWeight * stockDetail.BillWeightOut) : 0.0);
				double commission = ((item.CommissionAmount > 0.0 && item.ItemAmount > 0.0 && stockDetail.AmountOut > 0.0) ? (item.CommissionAmount / item.ItemAmount * stockDetail.AmountOut) : 0.0);
				stockDetail.AmountOut += expAmount - freight - commission;
			}
			stockDetail.ItemRate = sid.ItemRate;
			stockDetail.RefDocIdNo = sid.InvSaleInvoiceId;
			stockDetail.RefDocSubIdNo = sid.Id;
		}
	}

	private void PrepareStockEvaluationDetail(InventoryStockEvalautionDetail detail, Architecture.Model.Inventory.InvSaleInvoice obj)
	{
		detail.OrganizationId = obj.OrganizationId;
		detail.CompanyId = obj.CompanyId;
		detail.DocDate = obj.DocDate;
		detail.DocCodeNo = obj.DocNo;
		detail.SupplierCustomerId = obj.SupplierCustomerId;
		detail.BranchesId = obj.BranchesId;
		detail.RefDocumentTypeId = obj.DocumentTypeId;
		detail.EntryUser = obj.EntryUser;
		detail.ModifyUser = obj.ModifyUser;
		detail.CalcType = "Weight";
		InvSaleInvoiceDetail sid = obj.InvSaleInvoiceDetaillist.FirstOrDefault((InvSaleInvoiceDetail x) => x.LineId == detail.LineId && detail.LineId > 0);
		if (sid == null)
		{
			return;
		}
		double rateUom = CommonServices.GetEqvilentByItemIdAndUomScheduleId(new ReportsParameters
		{
			OrganizationId = obj.OrganizationId,
			CompanyId = obj.CompanyId,
			ItemId = detail.ItemId,
			Id = sid.UomScheduleIdRate
		});
		if (detail.BillWeightOut > 0.0 && sid.ItemRate > 0.0 && rateUom > 0.0)
		{
			detail.AmountOut = detail.BillWeightOut / rateUom * sid.ItemRate;
			if (obj.DocumentTypeId == 184)
			{
				detail.AmountOut -= sid.ItemDiscountAmount;
			}
			double expAmount = ((sid.ExpenseAmount > 0.0 && sid.ItemQty > 0.0 && detail.QtyOut > 0.0) ? (sid.ExpenseAmount / sid.ItemQty * detail.QtyOut) : 0.0);
			double freight = ((sid.FreightAmount > 0.0 && sid.NetBillWeight > 0.0 && detail.BillWeightOut > 0.0) ? (sid.FreightAmount / sid.NetBillWeight * detail.BillWeightOut) : 0.0);
			double commission = ((sid.CommissionAmount > 0.0 && sid.ItemAmount > 0.0 && detail.AmountOut > 0.0) ? (sid.CommissionAmount / sid.ItemAmount * detail.AmountOut) : 0.0);
			detail.AmountOut += expAmount - freight - commission;
		}
		detail.ItemRate = sid.ItemRate;
		detail.RefDocIdNo = sid.InvSaleInvoiceId;
		detail.RefDocSubIdNo = sid.Id;
	}

	private void ReverseInventoryQty(SqlConnection sqlCon, SqlTransaction sqlTrn, int organizationId, int companyId, int refDocumentTypeId, int refDocIdNo)
	{
		using SqlCommand cmd = new SqlCommand("[dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId]", sqlCon, sqlTrn);
		cmd.CommandType = CommandType.StoredProcedure;
		cmd.Parameters.AddWithValue("@OrganizationId", organizationId);
		cmd.Parameters.AddWithValue("@CompanyId", companyId);
		cmd.Parameters.AddWithValue("@RefDocumentTypeId", refDocumentTypeId);
		cmd.Parameters.AddWithValue("@RefDocIdNo", refDocIdNo);
		cmd.ExecuteNonQuery();
	}

	private int GetReferenceDocumentTypeId(Architecture.Model.Inventory.InvSaleInvoice obj, bool gdnExists)
	{
		if (obj.DocumentTypeId == 171)
		{
			if (!obj.InvSaleInvoiceDetaillist.Exists((InvSaleInvoiceDetail x) => x.SaleOrderId > 0))
			{
				return 221;
			}
			return 170;
		}
		if (obj.DocumentTypeId == 1660)
		{
			return 1659;
		}
		if (gdnExists)
		{
			return obj.DocumentTypeId switch
			{
				145 => 144, 
				126 => 149, 
				_ => 124, 
			};
		}
		return 86;
	}

	private List<InventoryStockEvalautionDetail> GetOtherReferences(Architecture.Model.Inventory.InvSaleInvoice obj, InvSaleInvoiceDetail item, int refDocumentTypeId)
	{
		return CommonServices.GetInventoryStockEvalautionDetailByOtherIds(new ReportsParameters
		{
			OrganizationId = obj.OrganizationId,
			CompanyId = obj.CompanyId,
			OtherDocumentTypeId = refDocumentTypeId,
			OtherDocNoId = item.InvGdnId,
			OtherSubDocNoId = item.InvGdnDetailId
		});
	}

	private void UpdateInventoryReference(SqlTransaction sqlTrn, Architecture.Model.Inventory.InvSaleInvoice obj)
	{
		InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
		inventoryTransactions.OrganizationId = obj.OrganizationId;
		inventoryTransactions.CompanyId = obj.CompanyId;
		inventoryTransactions.RefDocumentTypeId = obj.DocumentTypeId;
		inventoryTransactions.RefDocIdNo = obj.Id;
		Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
	}

	private void PrepareGdnReferenceStockDetail(InventoryStockEvalautionDetail stockDetail, InvSaleInvoiceDetail item, Architecture.Model.Inventory.InvSaleInvoice obj, int refDocumentTypeId)
	{
		stockDetail.OrganizationId = obj.OrganizationId;
		stockDetail.CompanyId = obj.CompanyId;
		stockDetail.DocDate = obj.DocDate;
		stockDetail.DocCodeNo = obj.DocNo;
		stockDetail.SupplierCustomerId = obj.SupplierCustomerId;
		stockDetail.BranchesId = obj.BranchesId;
		stockDetail.RefDocumentTypeId = obj.DocumentTypeId;
		stockDetail.EntryUser = obj.EntryUser;
		stockDetail.ModifyUser = obj.ModifyUser;
		stockDetail.CalcType = "Weight";
		InvSaleInvoiceDetail sid = obj.InvSaleInvoiceDetaillist.FirstOrDefault((InvSaleInvoiceDetail x) => x.InvGdnId == stockDetail.OtherDocNoId && x.InvGdnDetailId == stockDetail.OtherSubDocNoId && stockDetail.OtherDocumentTypeId == refDocumentTypeId);
		if (sid != null)
		{
			double rateUom = CommonServices.GetEqvilentByItemIdAndUomScheduleId(new ReportsParameters
			{
				OrganizationId = obj.OrganizationId,
				CompanyId = obj.CompanyId,
				ItemId = item.ItemId,
				Id = sid.UomScheduleIdRate
			});
			if (stockDetail.BillWeightOut > 0.0 && sid.ItemRate > 0.0 && rateUom > 0.0)
			{
				stockDetail.RateUom = sid.UomScheduleIdRate;
				stockDetail.AmountOut = sid.BillAmount;
				stockDetail.CgsAmount = ((obj.DocumentTypeId != 126 && obj.DocumentTypeId != 103) ? ((sid.ItemCgsRate > 0.0) ? (stockDetail.StockWeightOut / rateUom * sid.ItemCgsRate) : stockDetail.AmountOut) : ((sid.ItemCgsRate > 0.0) ? (stockDetail.QtyOut * sid.ItemCgsRate) : stockDetail.AmountOut));
			}
			stockDetail.ItemRate = sid.ItemRate;
			stockDetail.CgsRate = ((sid.ItemCgsRate > 0.0) ? sid.ItemCgsRate : sid.ItemRate);
			stockDetail.RefDocIdNo = sid.InvSaleInvoiceId;
			stockDetail.RefDocSubIdNo = sid.Id;
		}
	}

	private void SaveInventoryStockEvaluationDetails(SqlTransaction sqlTrn, List<InventoryStockEvalautionDetail> list)
	{
		foreach (InventoryStockEvalautionDetail item in list)
		{
			GenericProvider.SetProc(sqlTrn, item, "USP_InventoryStockEvalautionDetailGdnReferences_Update");
		}
	}

	private ReportsParameters CreateStockReportParameter(Architecture.Model.Inventory.InvSaleInvoice obj, InvSaleInvoiceDetail item, string itemName)
	{
		ReportsParameters stock = new ReportsParameters
		{
			OrganizationId = obj.OrganizationId,
			CompanyId = obj.CompanyId,
			ItemId = item.ItemId,
			WarehouseId = item.WarehouseId,
			DocDate = ((obj.DocumentTypeId == 99) ? obj.SupplierInvoiceDate : obj.DocDate),
			ItemQty = item.ItemQty,
			NetWeight = item.NetStockWeight,
			RateUOM = item.UomScheduleIdRate,
			LineId = item.LineId,
			ItemName = itemName
		};
		if (obj.DocumentTypeId != 126 && obj.DocumentTypeId != 145)
		{
			stock.stockUOM = item.ItemUOMId;
			stock.JobLotId = item.JobLotId;
			stock.CropYear = item.CropYear;
			stock.PackingTypeId = item.PackingTypeId;
		}
		if (obj.ModifyUser > 0)
		{
			stock.DocumentTypeId = obj.DocumentTypeId;
			stock.Id = obj.Id;
		}
		return stock;
	}

	public void AutoVoucherAndStockEffects(Architecture.Model.Inventory.InvSaleInvoice obj)
	{
		List<GetJobLotGlIdsandName> lstJobLot = new List<GetJobLotGlIdsandName>();
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			List<Architecture.Model.Inventory.GetItemGlIdsandItemName> dtitem = (dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId));
			string RemarksCgs = "";
			obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
			obj.VoucherHeadInvoices = new VoucherHead();
			obj.VoucherHeadInvoices.voucherDetailList = new List<VoucherDetail>();
			lstJobLot = CommonServices.GetJobLotGlIdsandName(obj.OrganizationId, obj.CompanyId);
			foreach (InvSaleInvoiceDetail item4 in obj.InvSaleInvoiceDetaillist.ToList())
			{
				List<Architecture.Model.Inventory.GetItemGlIdsandItemName> find = dtitem.Where((Architecture.Model.Inventory.GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item4.ItemId)).ToList();
				if (find.Count <= 0)
				{
					continue;
				}
				ReportsParameters stock = new ReportsParameters();
				stock.OrganizationId = obj.OrganizationId;
				stock.CompanyId = obj.CompanyId;
				stock.ItemId = item4.ItemId;
				stock.WarehouseId = item4.WarehouseId;
				stock.DocDate = obj.DocDate;
				if (obj.DocumentTypeId != 126)
				{
					stock.stockUOM = item4.ItemUOMId;
					stock.JobLotId = item4.JobLotId;
					stock.CropYear = item4.CropYear;
					stock.PackingTypeId = item4.PackingTypeId;
				}
				if (obj.ModifyUser > 0)
				{
					stock.DocumentTypeId = obj.DocumentTypeId;
					stock.Id = obj.Id;
				}
				stock.ItemQty = item4.ItemQty;
				stock.NetWeight = item4.NetStockWeight;
				stock.RateUOM = item4.UomScheduleIdRate;
				stock.LineId = item4.LineId;
				stock.ItemName = Conversion.ToString(find[0].ItemName);
				List<GetJobLotGlIdsandName> findJob = lstJobLot.Where((GetJobLotGlIdsandName x) => x.Id == Conversion.ToInt(item4.JobLotId)).ToList();
				bool JobLotFlag = false;
				if (findJob != null && findJob.Count > 0 && findJob != null && findJob[0].AccountId > 0)
				{
					JobLotFlag = true;
				}
				foreach (InventoryStockEvalautionDetail stockdetail in CommonServices.FIFOImplemention(stock, obj.InventoryStockEvalautionDetailslist).InventoryStockEvalautionDetailslist.ToList())
				{
					if (!JobLotFlag)
					{
						RemarksCgs = "ItemQty: " + stockdetail.QtyOut + "   " + Conversion.ToString(find[0].ItemName) + "  Net Weight: " + stockdetail.StockWeightOut + "  CGS Rate:" + Math.Round(stockdetail.CgsRate, 2);
						VoucherDetail vd6 = new VoucherDetail();
						vd6.LineId = item4.LineId;
						vd6.IsCGS = 1;
						vd6.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd6.AgainstAccountId = Conversion.ToInt(find[0].COGSGLAC);
						vd6.Comments = Conversion.ToString(RemarksCgs);
						vd6.CreditAmount = Conversion.ToDouble(stockdetail.CgsAmount);
						vd6.DocumentTypeIdRef = Conversion.ToInt(item4.RefRefDocumentTypeId);
						vd6.InvoiceNoRefId = Conversion.ToInt(item4.RefRefDocIdNo);
						vd6.RefInvoiceNo = Conversion.ToString(item4.RefDocSubId);
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
						vd6.OrderNo = Conversion.ToInt(item4.SaleOrder);
						vd6.GpNo = Conversion.ToInt(item4.GpNo);
						vd6.VehicleNo = item4.VehicleNo;
						vd6.JobLotId = Conversion.ToInt(item4.JobLotId);
						vd6.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						obj.VoucherHeadInvoices.voucherDetailList.Add(vd6);
						VoucherDetail vd7 = new VoucherDetail();
						vd7.LineId = item4.LineId;
						vd7.IsCGS = 1;
						vd7.AccountId = Conversion.ToInt(find[0].COGSGLAC);
						vd7.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd7.Comments = Conversion.ToString(RemarksCgs);
						vd7.DebitAmount = Conversion.ToDouble(stockdetail.CgsAmount);
						vd7.DocumentTypeIdRef = Conversion.ToInt(item4.RefRefDocumentTypeId);
						vd7.InvoiceNoRefId = Conversion.ToInt(item4.RefRefDocIdNo);
						vd7.RefInvoiceNo = Conversion.ToString(item4.RefDocSubId);
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
						vd7.OrderNo = Conversion.ToInt(item4.SaleOrder);
						vd7.GpNo = Conversion.ToInt(item4.GpNo);
						vd7.VehicleNo = item4.VehicleNo;
						vd7.JobLotId = Conversion.ToInt(item4.JobLotId);
						vd7.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						obj.VoucherHeadInvoices.voucherDetailList.Add(vd7);
					}
					if (obj.DocumentTypeId == 126)
					{
						stockdetail.ItemUom = item4.ItemUOMId;
						stockdetail.JobLotId = item4.JobLotId;
					}
					stockdetail.RefDocIdNo = obj.Id;
					stockdetail.RefDocSubIdNo = item4.Id;
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
				foreach (InventoryStockEvalautionDetail item3 in obj.InventoryStockEvalautionDetailslist)
				{
					item3.OrganizationId = obj.OrganizationId;
					item3.CompanyId = obj.CompanyId;
					item3.DocDate = obj.DocDate;
					item3.DocCodeNo = obj.DocNo;
					item3.SupplierCustomerId = obj.SupplierCustomerId;
					item3.BranchesId = obj.BranchesId;
					item3.RefDocumentTypeId = obj.DocumentTypeId;
					item3.EntryUser = obj.EntryUser;
					item3.ModifyUser = obj.ModifyUser;
					item3.CalcType = "Weight";
					InvSaleInvoiceDetail sid2 = obj.InvSaleInvoiceDetaillist.Find((InvSaleInvoiceDetail x) => x.LineId == item3.LineId && x.LineId > 0 && item3.LineId > 0);
					if (sid2 != null)
					{
						double RateUom = CommonServices.GetEqvilentByItemIdAndUomScheduleId(new ReportsParameters
						{
							OrganizationId = obj.OrganizationId,
							CompanyId = obj.CompanyId,
							ItemId = item3.ItemId,
							Id = sid2.UomScheduleIdRate
						});
						if (item3.BillWeightOut > 0.0 && sid2.ItemRate > 0.0 && RateUom > 0.0)
						{
							item3.AmountOut = item3.BillWeightOut / RateUom * sid2.ItemRate;
						}
						item3.ItemRate = sid2.ItemRate;
					}
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "USP_InventoryStockEvalautionDetail_Insert"));
				}
			}
			foreach (InvSaleInvoiceDetail item in obj.InvSaleInvoiceDetaillist)
			{
				SqlCommand sqlCommand2 = new SqlCommand();
				sqlCommand2.Connection = sqlcon;
				sqlCommand2.Transaction = sqlTrn;
				sqlCommand2.CommandType = CommandType.StoredProcedure;
				sqlCommand2.CommandText = "USP_InventoryValidation";
				sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand2.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
				sqlCommand2.Parameters.AddWithValue("@DocDate", obj.DocDate);
				sqlCommand2.Parameters.AddWithValue("@ItemId", item.ItemId);
				sqlCommand2.Parameters.AddWithValue("@WarehouseId", item.WarehouseId);
				sqlCommand2.Parameters.AddWithValue("@JobLotId", item.JobLotId);
				sqlCommand2.Parameters.AddWithValue("@CropYear", item.CropYear);
				sqlCommand2.Parameters.AddWithValue("@InvPackingTypeId", item.PackingTypeId);
				sqlCommand2.Parameters.AddWithValue("@PackUomId", item.ItemUOMId);
				sqlCommand2.Parameters.AddWithValue("@NetWeight", item.NetStockWeight);
				sqlCommand2.Parameters.AddWithValue("@RefDocumentTypeId", item.RefRefDocumentTypeId);
				sqlCommand2.Parameters.AddWithValue("@RefDocNoId", item.RefRefDocIdNo);
				sqlCommand2.Parameters.AddWithValue("@RefDocSubIdNo", item.RefDocSubId);
				sqlCommand2.ExecuteNonQuery();
			}
			if (obj.VoucherHeadInvoices.voucherDetailList == null || obj.VoucherHeadInvoices.voucherDetailList.Count == 0)
			{
				throw new Exception("Voucher Detail list Not Found");
			}
			DataTable dtvh = CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.Id);
			if (dtvh.Rows.Count > 0)
			{
				obj.VoucherHeadInvoices.Id = Conversion.ToInt(dtvh.Rows[0]["Id"]);
				foreach (VoucherDetail item2 in obj.VoucherHeadInvoices.voucherDetailList)
				{
					item2.VoucherHeadId = obj.VoucherHeadInvoices.Id;
					InvSaleInvoiceDetail sid = obj.InvSaleInvoiceDetaillist.Find((InvSaleInvoiceDetail x) => x.LineId == item2.LineId && x.LineId > 0 && item2.LineId > 0);
					if (sid != null)
					{
						item2.RefDocSubIdNo = sid.Id;
					}
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_Insert"));
				}
			}
			SqlCommand sqlCommand3 = new SqlCommand();
			sqlCommand3.Connection = sqlcon;
			sqlCommand3.Transaction = sqlTrn;
			sqlCommand3.CommandType = CommandType.StoredProcedure;
			sqlCommand3.CommandText = "USP_VoucherBalanceCheck";
			sqlCommand3.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
			sqlCommand3.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
			sqlCommand3.Parameters.AddWithValue("@Id", obj.VoucherHeadInvoices.Id);
			sqlCommand3.ExecuteScalar();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}
}
