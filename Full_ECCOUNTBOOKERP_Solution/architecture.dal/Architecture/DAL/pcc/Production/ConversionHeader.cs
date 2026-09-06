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
using Architecture.Model.pcc.Production;

namespace Architecture.DAL.pcc.Production;

public class ConversionHeader
{
	public int SetData(Architecture.Model.pcc.Production.ConversionHeader obj, string ProcName)
	{
		int returnID = 0;
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
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
			int LineId = 1;
			foreach (ConversionOutPutDetail item6 in obj.ConversionOutPutDetailList)
			{
				item6.ConversionHeaderId = obj.Id;
				item6.LineId = LineId;
				int code4 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "[pcc].[USP_ConversionOutPutDetail_Insert]"));
				item6.Id = code4;
				LineId++;
			}
			LineId = 1;
			foreach (ConversionInputDetail item5 in obj.ConversionInputDetailList)
			{
				item5.ConversionHeaderId = obj.Id;
				item5.LineId = LineId;
				int code3 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "[pcc].[USP_ConversionInputDetail_Insert]"));
				item5.Id = code3;
				LineId++;
			}
			foreach (ConversionOverHeadsDetail item4 in obj.ConversionOverHeadsDetailList)
			{
				item4.ConversionHeaderId = obj.Id;
				int code2 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[pcc].[USP_ConversionOverHeadsDetail_Insert]"));
				item4.Id = code2;
			}
			foreach (ConversionWagesDetail item3 in obj.ConversionWagesDetailList)
			{
				item3.ConversionHeaderId = obj.Id;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "[pcc].[USP_ConversionWagesDetail_Insert]"));
				item3.Id = code;
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				foreach (DMSAttachments item2 in obj.AttachmentsList.ToList())
				{
					item2.RefAccountId = obj.EntryUserId;
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
			if (CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5))
			{
				List<GetItemGlIdsandItemName> dtitem = (dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId));
				obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
				foreach (ConversionInputDetail item9 in obj.ConversionInputDetailList.ToList())
				{
					if (item9.ActionTypeId == 3)
					{
						continue;
					}
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item9.ItemId)).ToList();
					if (find.Count <= 0)
					{
						continue;
					}
					ReportsParameters stock = new ReportsParameters();
					stock.OrganizationId = obj.OrganizationId;
					stock.CompanyId = obj.CompanyId;
					stock.ItemId = item9.ItemId;
					stock.WarehouseId = item9.WareHouseId;
					stock.DocDate = obj.DocDate;
					if (obj.ModifyUserId > 0)
					{
						stock.DocumentTypeId = obj.DocumentTypeId;
						stock.Id = obj.Id;
					}
					stock.ItemQty = Conversion.ToDouble(item9.ItemQty);
					stock.NetWeight = Conversion.ToDouble(item9.NetWeight);
					stock.LineId = item9.LineId;
					stock.ItemName = Conversion.ToString(find[0].ItemName);
					foreach (InventoryStockEvalautionDetail stockdetail in CommonServices.FIFOImplementionForConcrete(stock, obj.InventoryStockEvalautionDetailslist).InventoryStockEvalautionDetailslist.ToList())
					{
						stockdetail.ItemUom = item9.ItemUomId;
						stockdetail.VarientId = item9.ItemAttributeVarientId;
						obj.InventoryStockEvalautionDetailslist.Add(stockdetail);
					}
				}
				decimal TotalInPutAmount = obj.ConversionInputDetailList.Where((ConversionInputDetail row) => row.ActionTypeId != 3).Sum((ConversionInputDetail row) => row.Amount);
				double TotalCgsAmount = obj.InventoryStockEvalautionDetailslist.Sum((InventoryStockEvalautionDetail row) => row.CgsAmount);
				if (Math.Round(TotalInPutAmount, 4) != Conversion.ToDecimal(Math.Round(TotalCgsAmount, 4)))
				{
					throw new Exception("Total input amount and total cgs amount against fifo not match.input amount is " + Conversion.ToString(TotalInPutAmount) + " cgs amount is " + Conversion.ToString(TotalCgsAmount));
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
					foreach (InventoryStockEvalautionDetail item8 in obj.InventoryStockEvalautionDetailslist)
					{
						item8.OrganizationId = obj.OrganizationId;
						item8.CompanyId = obj.CompanyId;
						item8.DocDate = obj.DocDate;
						item8.DocCodeNo = obj.DocNo;
						item8.SupplierCustomerId = 0;
						item8.PrdJobOrderNo = 0;
						item8.BranchesId = obj.BranchesId;
						item8.RefDocumentTypeId = obj.DocumentTypeId;
						item8.EntryUser = obj.EntryUserId;
						item8.ModifyUser = obj.ModifyUserId;
						item8.CalcType = "Weight";
						ConversionInputDetail sid2 = obj.ConversionInputDetailList.Find((ConversionInputDetail x) => x.LineId == item8.LineId && x.LineId > 0 && item8.LineId > 0);
						if (sid2 != null)
						{
							item8.RefDocIdNo = sid2.ConversionHeaderId;
							item8.RefDocSubIdNo = sid2.Id;
						}
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "USP_InventoryStockEvalautionDetail_Insert"));
					}
				}
			}
			InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
			inventoryTransactions.OrganizationId = obj.OrganizationId;
			inventoryTransactions.CompanyId = obj.CompanyId;
			inventoryTransactions.RefDocumentTypeId = obj.DocumentTypeId;
			inventoryTransactions.RefDocIdNo = obj.Id;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
			InventoryTransactions transactions = new InventoryTransactions();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = returnID;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			foreach (ConversionInputDetail item in obj.ConversionInputDetailList)
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
				sqlCommandInv.Parameters.AddWithValue("@ItemId", item.ItemId);
				sqlCommandInv.Parameters.AddWithValue("@WarehouseId", item.WareHouseId);
				if (item.ItemAttributeVarientId > 0)
				{
					sqlCommandInv.Parameters.AddWithValue("@VarientId", item.ItemAttributeVarientId);
				}
				sqlCommandInv.Parameters.AddWithValue("@Qty", item.ItemQty);
				sqlCommandInv.ExecuteNonQuery();
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
				foreach (VoucherDetail item7 in obj.VoucherHeadInvoices.voucherDetailList)
				{
					item7.VoucherHeadId = obj.VoucherHeadInvoices.Id;
					ConversionInputDetail sid = obj.ConversionInputDetailList.Find((ConversionInputDetail x) => x.LineId == item7.LineId && x.LineId > 0 && item7.LineId > 0 && item7.CreditAmount > 0.0);
					if (sid != null)
					{
						item7.RefDocSubIdNo = sid.Id;
					}
					ConversionOutPutDetail Op = obj.ConversionOutPutDetailList.Find((ConversionOutPutDetail x) => x.LineId == item7.LineId && x.LineId > 0 && item7.LineId > 0 && item7.DebitAmount > 0.0);
					if (Op != null)
					{
						item7.RefDocSubIdNo = Op.Id;
					}
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "Sp_VoucherDetail_Insert"));
				}
				if (obj.VoucherHeadInvoices.Id > 0)
				{
					SqlCommand sqlCommand2 = new SqlCommand();
					sqlCommand2.Connection = sqlcon;
					sqlCommand2.Transaction = sqlTrn;
					sqlCommand2.CommandType = CommandType.StoredProcedure;
					sqlCommand2.CommandText = "USP_VoucherBalanceCheck";
					sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand2.Parameters.AddWithValue("@Id", obj.VoucherHeadInvoices.Id);
					sqlCommand2.ExecuteScalar();
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

	public List<Architecture.Model.pcc.Production.ConversionHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.pcc.Production.ConversionHeader> lst = new List<Architecture.Model.pcc.Production.ConversionHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.pcc.Production.ConversionHeader>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.pcc.Production.ConversionHeader item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "ReadByHeaderId_ConversionInputDetail"));
						item4.ConversionInputDetailList = GenericProvider.GetAllDetail<ConversionInputDetail>(sqlConnection2, "[pcc].[USP_ConversionHeader_GetAllMethod]", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.pcc.Production.ConversionHeader item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadByHeaderId_ConversionOutPutDetail"));
						item3.ConversionOutPutDetailList = GenericProvider.GetAllDetail<ConversionOutPutDetail>(sqlConnection3, "[pcc].[USP_ConversionHeader_GetAllMethod]", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.pcc.Production.ConversionHeader item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadByHeaderId_ConversionOverHeadsDetail"));
						item2.ConversionOverHeadsDetailList = GenericProvider.GetAllDetail<ConversionOverHeadsDetail>(sqlConnection4, "[pcc].[USP_ConversionHeader_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.pcc.Production.ConversionHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId_ConversionWagesDetail"));
					item.ConversionWagesDetailList = GenericProvider.GetAllDetail<ConversionWagesDetail>(sqlcon, "[pcc].[USP_ConversionHeader_GetAllMethod]", lstPrm);
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
