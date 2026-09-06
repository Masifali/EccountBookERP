using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.PartyProcessing;
using Architecture.Model.VPS;

namespace Architecture.DAL.Inventory;

public class InvGdn
{
	private static readonly HashSet<int> IncludedDocTypes = new HashSet<int> { 124, 144, 149, 170, 221, 703, 1202, 1607, 1612, 1659 };

	private bool IsValidDocType(int documentTypeId)
	{
		return !new int[4] { 144, 149, 1612, 1202 }.Contains(documentTypeId);
	}

	private bool IsValidForOrderComplete(int documentTypeId)
	{
		return new int[3] { 86, 149, 171 }.Contains(documentTypeId);
	}

	public int SetData(Architecture.Model.Inventory.InvGdn obj, string ProcName)
	{
		int returnID = 0;
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.invGdnDetail == null || obj.invGdnDetail.Count <= 0)
			{
				throw new Exception("Detail list not found");
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
			double TotalNetBillWeight = 0.0;
			bool loaderflg = false;
			foreach (InvGdnDetail item6 in obj.invGdnDetail)
			{
				if (item6.RefDocumentTypeId > 0 && item6.RefDocIdNo > 0 && item6.RefDocSubIdNo > 0)
				{
					DataTable dtRate = CommonServices.getItemRateFromStockEvalaution(obj.OrganizationId, obj.CompanyId, item6.RefDocumentTypeId, item6.RefDocIdNo, item6.RefDocSubIdNo);
					if (dtRate.Rows.Count <= 0 || dtRate == null)
					{
						throw new Exception("Item Rate not found from stock Evaluation against references");
					}
					item6.ItemRate = Conversion.ToDouble(dtRate.Rows[0]["ItemRate"]);
					item6.RateUomId = Conversion.ToInt(dtRate.Rows[0]["RateUomId"]);
					double RateEquivalent = Conversion.ToDouble(dtRate.Rows[0]["REquivalent"]);
					item6.ItemAmount = item6.StockWeight / RateEquivalent * item6.ItemRate;
					loaderflg = true;
				}
				LineIdForPDI = (item6.LineId = LineIdForPDI + 1);
				item6.InvGdnId = obj.Id;
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "Sp_InvGdnDetail_Insert"));
				item6.Id = Code;
				TotalNetBillWeight += item6.NetBillWeight;
			}
			if (obj.InvGdnExpenseslist != null && obj.InvGdnExpenseslist.Count > 0)
			{
				foreach (InvGdnExpense item5 in obj?.InvGdnExpenseslist)
				{
					item5.InvGdnId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "Sp_InvGdnExpense_Insert"));
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
				foreach (DMSAttachments item4 in obj.AttachmentsList.ToList())
				{
					item4.RefAccountId = obj.SupplierCustomerId;
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
			InventoryTransactions transactions = new InventoryTransactions();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = obj.Id;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			bool FIFOCgs = CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5);
			if (!loaderflg && FIFOCgs && IsValidDocType(obj.DocumentTypeId))
			{
				List<Architecture.Model.Inventory.GetItemGlIdsandItemName> dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId);
				obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
				foreach (InvGdnDetail item8 in obj.invGdnDetail.ToList())
				{
					List<Architecture.Model.Inventory.GetItemGlIdsandItemName> find = dtitem.Where((Architecture.Model.Inventory.GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item8.ItemId)).ToList();
					if (find.Count <= 0)
					{
						continue;
					}
					ReportsParameters stock = new ReportsParameters();
					stock.OrganizationId = obj.OrganizationId;
					stock.CompanyId = obj.CompanyId;
					stock.ItemId = item8.ItemId;
					stock.WarehouseId = item8.WarehouseId;
					stock.stockUOM = item8.ItemUomId;
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
					stock.NetWeight = item8.StockWeight;
					stock.LineId = item8.LineId;
					stock.ItemName = Conversion.ToString(find[0].ItemName);
					foreach (InventoryStockEvalautionDetail stockdetail in CommonServices.FIFOImplemention(stock, obj.InventoryStockEvalautionDetailslist).InventoryStockEvalautionDetailslist.ToList())
					{
						stockdetail.CityId = item8.CityId;
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
					foreach (InventoryStockEvalautionDetail item7 in obj.InventoryStockEvalautionDetailslist)
					{
						item7.OrganizationId = obj.OrganizationId;
						item7.CompanyId = obj.CompanyId;
						item7.DocDate = obj.DocDate;
						item7.DocCodeNo = obj.DocNo;
						item7.VehicleNo = obj.VehicleNo;
						item7.GpNoDcNo = obj.GpNo;
						item7.BiltyNo = obj.BiltyNo;
						item7.SupplierCustomerId = obj.SupplierCustomerId;
						item7.BranchesId = obj.BranchesId;
						item7.OtherDocumentTypeId = obj.DocumentTypeId;
						item7.EntryUser = obj.EntryUser;
						item7.ModifyUser = obj.ModifyUser;
						item7.BiltyNo = obj.BiltyNo;
						item7.CalcType = "Weight";
						InvGdnDetail sid = obj.invGdnDetail.Find((InvGdnDetail x) => x.LineId == item7.LineId && x.LineId > 0 && item7.LineId > 0);
						if (sid != null)
						{
							item7.OtherDocNoId = sid.InvGdnId;
							item7.OtherSubDocNoId = sid.Id;
						}
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "USP_InventoryStockEvalautionDetail_Insert"));
					}
				}
			}
			else if (loaderflg)
			{
				SqlCommand sqlCommand2 = new SqlCommand();
				sqlCommand2.Connection = sqlcon;
				sqlCommand2.Transaction = sqlTrn;
				sqlCommand2.CommandType = CommandType.StoredProcedure;
				sqlCommand2.CommandText = "[dbo].[usp_StockEvalautionInsert_FromGdn]";
				sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand2.Parameters.AddWithValue("@RefDocumentTypeId", obj.DocumentTypeId);
				sqlCommand2.Parameters.AddWithValue("@RefDocIdNo", obj.Id);
				sqlCommand2.ExecuteNonQuery();
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
			foreach (InvGdnDetail item3 in obj.invGdnDetail)
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
				sqlCommand3.Parameters.AddWithValue("@ItemId", item3.ItemId);
				sqlCommand3.Parameters.AddWithValue("@WarehouseId", item3.WarehouseId);
				sqlCommand3.Parameters.AddWithValue("@JobLotId", item3.JobLotId);
				sqlCommand3.Parameters.AddWithValue("@CropYear", item3.CropYear);
				sqlCommand3.Parameters.AddWithValue("@InvPackingTypeId", item3.PackingTypeId);
				sqlCommand3.Parameters.AddWithValue("@PackUomId", item3.ItemUomId);
				sqlCommand3.Parameters.AddWithValue("@NetWeight", item3.StockWeight);
				sqlCommand3.Parameters.AddWithValue("@RefDocumentTypeId", item3.RefDocumentTypeId);
				sqlCommand3.Parameters.AddWithValue("@RefDocNoId", item3.RefDocIdNo);
				sqlCommand3.Parameters.AddWithValue("@RefDocSubIdNo", item3.RefDocSubIdNo);
				sqlCommand3.Parameters.AddWithValue("@ItemConditionId", item3.ItemConditionId);
				sqlCommand3.ExecuteNonQuery();
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
			if (IsValidForOrderComplete(obj.DocumentTypeId))
			{
				foreach (InvGdnDetail item2 in obj.invGdnDetail)
				{
					SqlCommand sqlCommand5 = new SqlCommand();
					sqlCommand5.Connection = sqlcon;
					sqlCommand5.Transaction = sqlTrn;
					sqlCommand5.CommandType = CommandType.StoredProcedure;
					sqlCommand5.CommandText = "USP_SaleOrderAutoComplete";
					sqlCommand5.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand5.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand5.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
					sqlCommand5.Parameters.AddWithValue("@SaleOrderId", item2.SaleOrderId);
					sqlCommand5.Parameters.AddWithValue("@NetWeight", TotalNetBillWeight);
					sqlCommand5.ExecuteNonQuery();
				}
			}
			if (obj.DocumentTypeId == 221 || (obj.DocumentTypeId == 86 && obj.IsStockReserved))
			{
				InventoryTransactionsPartyProcessing objPP = new InventoryTransactionsPartyProcessing();
				objPP.OrganizationId = obj.OrganizationId;
				objPP.CompanyId = obj.CompanyId;
				objPP.RefDocumentTypeId = obj.DocumentTypeId;
				objPP.RefDocIdNo = returnID;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, objPP, "Sp_InventoryTransactionsPartyProcessing_Insert"));
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

	public List<Architecture.Model.Inventory.InvGdn> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.InvGdn> lst = new List<Architecture.Model.Inventory.InvGdn>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.Get<Architecture.Model.Inventory.InvGdn>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.InvGdn item3 in lst)
					{
						string activity = ((item3.DocumentTypeId == 86 && item3.TransporterDocRef == "Export") ? "GetExportGDNDetailByGdnId" : "GetGDNDetailByGdnId");
						if (IncludedDocTypes.Contains(item3.DocumentTypeId) || (item3.DocumentTypeId == 86 && item3.TransporterDocRef != "Export"))
						{
							item3.invGdnDetail = GenericProvider.GetAllDetail<InvGdnDetail>(sqlConnection2, "Sp_InvGdn_GetAllMethod", new List<SqlParameter>
							{
								new SqlParameter("@InvGdnMainId", item3.Id),
								new SqlParameter("@Activity", activity)
							});
						}
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.InvGdn item2 in lst)
					{
						if (item2.DocumentTypeId == 87)
						{
							List<SqlParameter> lstPrm2 = new List<SqlParameter>();
							lstPrm2.Add(new SqlParameter("@InvGdnMainId", item2.Id));
							lstPrm2.Add(new SqlParameter("@Activity", "GetGDNDetailByGdnIdTrading"));
							item2.invGdnDetail = GenericProvider.GetAllDetail<InvGdnDetail>(sqlConnection3, "Sp_InvGdn_GetAllMethod", lstPrm2);
						}
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Inventory.InvGdn item in lst)
				{
					if (item.DocumentTypeId == 86 || item.DocumentTypeId == 1612 || item.DocumentTypeId == 144 || item.DocumentTypeId == 703)
					{
						List<SqlParameter> lstPrm = new List<SqlParameter>();
						lstPrm.Add(new SqlParameter("@Id", item.Id));
						lstPrm.Add(new SqlParameter("@Activity", "GetInvGdnExpensesByHeaderId"));
						item.InvGdnExpenseslist = GenericProvider.GetAllDetail<InvGdnExpense>(sqlcon, "Sp_InvGdn_GetAllMethod", lstPrm);
					}
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
