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

public class InvGdn
{
	public int SetData(Architecture.Model.pcc.InvGdn obj, string ProcName)
	{
		int returnID = 0;
		decimal TotalNetBillWeight = default(decimal);
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
			int LineIdForPDI = 0;
			double TotalQty = 0.0;
			foreach (Architecture.Model.pcc.InvGdnDetail item5 in obj.invGdnDetailList)
			{
				LineIdForPDI = (item5.LineId = LineIdForPDI + 1);
				item5.InvGdnId = obj.Id;
				TotalNetBillWeight += item5.ItemNetWeight;
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "[pcc].[USP_InvGdnDetail_Insert]"));
				item5.Id = Code;
				TotalQty += item5.ConfirmQty;
			}
			if (obj.InvGDNWagesDetailList != null && obj.InvGDNWagesDetailList.Count > 0)
			{
				foreach (InvGDNWagesDetail item4 in obj?.InvGDNWagesDetailList)
				{
					item4.InvGDNId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[pcc].[USP_InvGDNWagesDetail_Insert]"));
				}
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				foreach (DMSAttachments item3 in obj.AttachmentsList.ToList())
				{
					item3.RefAccountId = obj.SupplierCustomerId;
					item3.RefDocumentTypeId = obj.DocumentTypeId;
					item3.RefDocumentNo = returnID;
					item3.EntryDate = DateTime.Now;
					item3.ModifyDate = DateTime.Now;
					item3.EntryUser = obj.EntryUserId;
					item3.ModifyUser = obj.ModifyUserId;
					item3.OrganizationId = obj.OrganizationId;
					item3.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Proc_DMSAttachments_Insert"));
				}
			}
			InventoryTransactions transactions = new InventoryTransactions();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = obj.Id;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			if (CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5))
			{
				List<GetItemGlIdsandItemName> dtitem = (dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId));
				obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
				foreach (Architecture.Model.pcc.InvGdnDetail item7 in obj.invGdnDetailList.ToList())
				{
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item7.ItemId)).ToList();
					if (find.Count <= 0)
					{
						continue;
					}
					ReportsParameters stock = new ReportsParameters();
					stock.OrganizationId = obj.OrganizationId;
					stock.CompanyId = obj.CompanyId;
					stock.ItemId = item7.ItemId;
					stock.WarehouseId = item7.WarehouseId;
					if (item7.ItemAttributeVarientId > 0)
					{
						stock.stockUOM = item7.ItemAttributeVarientId;
					}
					stock.DocDate = obj.DocDate;
					if (obj.ModifyUserId > 0)
					{
						stock.DocumentTypeId = obj.DocumentTypeId;
						stock.Id = obj.Id;
					}
					stock.ItemQty = Conversion.ToDouble(item7.ItemQty);
					stock.NetWeight = Conversion.ToDouble(item7.ItemNetWeight);
					stock.LineId = item7.LineId;
					stock.ItemName = Conversion.ToString(find[0].ItemName);
					foreach (InventoryStockEvalautionDetail stockdetail in CommonServices.FIFOImplementionForConcrete(stock, obj.InventoryStockEvalautionDetailslist).InventoryStockEvalautionDetailslist.ToList())
					{
						stockdetail.VarientId = item7.ItemAttributeVarientId;
						stockdetail.ItemUom = item7.ItemUomId;
						stockdetail.JobLotId = item7.JobLotId;
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
					foreach (InventoryStockEvalautionDetail item6 in obj.InventoryStockEvalautionDetailslist)
					{
						item6.OrganizationId = obj.OrganizationId;
						item6.CompanyId = obj.CompanyId;
						item6.DocDate = obj.DocDate;
						item6.DocCodeNo = obj.DocNo;
						item6.SupplierCustomerId = obj.SupplierCustomerId;
						item6.BranchesId = obj.BranchesId;
						item6.OtherDocumentTypeId = obj.DocumentTypeId;
						item6.EntryUser = obj.EntryUserId;
						item6.ModifyUser = obj.ModifyUserId;
						item6.CalcType = "Qty";
						Architecture.Model.pcc.InvGdnDetail sid = obj.invGdnDetailList.Find((Architecture.Model.pcc.InvGdnDetail x) => x.LineId == item6.LineId && x.LineId > 0 && item6.LineId > 0);
						if (sid != null)
						{
							item6.OtherDocNoId = sid.InvGdnId;
							item6.OtherSubDocNoId = sid.Id;
						}
						item6.BiltyNo = obj.BiltyNo;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "USP_InventoryStockEvalautionDetail_Insert"));
					}
					SqlCommand sqlCommand2 = new SqlCommand();
					sqlCommand2.Connection = sqlcon;
					sqlCommand2.Transaction = sqlTrn;
					sqlCommand2.CommandType = CommandType.StoredProcedure;
					sqlCommand2.CommandText = "pcc.usp_GdnDetailInsertintoStockTable";
					sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand2.Parameters.AddWithValue("@RefDocumentTypeId", obj.DocumentTypeId);
					sqlCommand2.Parameters.AddWithValue("@InvGdnId", obj.Id);
					sqlCommand2.ExecuteNonQuery();
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
			foreach (Architecture.Model.pcc.InvGdnDetail item2 in obj.invGdnDetailList)
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
			foreach (Architecture.Model.pcc.InvGdnDetail item in obj.invGdnDetailList)
			{
				SqlCommand sqlCommand3 = new SqlCommand();
				sqlCommand3.Connection = sqlcon;
				sqlCommand3.Transaction = sqlTrn;
				sqlCommand3.CommandType = CommandType.StoredProcedure;
				sqlCommand3.CommandText = "[pcc].[USP_SaleOrderAutoComplete]";
				sqlCommand3.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand3.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand3.Parameters.AddWithValue("@SaleOrderId", item.OrderId);
				sqlCommand3.ExecuteNonQuery();
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

	public int SetDataForWages(Architecture.Model.pcc.InvGdn obj, string ProcName)
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
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			if (obj.InvGDNWagesDetailList != null && obj.InvGDNWagesDetailList.Count > 0)
			{
				foreach (InvGDNWagesDetail item2 in obj?.InvGDNWagesDetailList)
				{
					item2.InvGDNId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "[pcc].[USP_InvGDNWagesDetail_Insert]"));
				}
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				foreach (DMSAttachments item in obj.AttachmentsList.ToList())
				{
					item.RefAccountId = obj.SupplierCustomerId;
					item.RefDocumentTypeId = obj.DocumentTypeId;
					item.RefDocumentNo = returnID;
					item.EntryDate = DateTime.Now;
					item.ModifyDate = DateTime.Now;
					item.EntryUser = obj.EntryUserId;
					item.ModifyUser = obj.ModifyUserId;
					item.OrganizationId = obj.OrganizationId;
					item.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Proc_DMSAttachments_Insert"));
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

	public List<Architecture.Model.pcc.InvGdn> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.pcc.InvGdn> lst = new List<Architecture.Model.pcc.InvGdn>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.Get<Architecture.Model.pcc.InvGdn>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.pcc.InvGdn item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "InvGdnDetail_ReadByInvGdnId"));
						item2.invGdnDetailList = GenericProvider.GetAllDetail<Architecture.Model.pcc.InvGdnDetail>(sqlConnection2, "[pcc].[USP_InvGdn_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.pcc.InvGdn item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "InvGdnWagesDetail_ReadByInvGdnId"));
					item.InvGDNWagesDetailList = GenericProvider.GetAllDetail<InvGDNWagesDetail>(sqlcon, "[pcc].[USP_InvGdn_GetAllMethod]", lstPrm);
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
