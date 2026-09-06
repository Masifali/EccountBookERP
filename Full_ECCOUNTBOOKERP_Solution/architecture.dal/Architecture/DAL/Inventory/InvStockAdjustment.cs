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
using Architecture.Model.VPS;

namespace Architecture.DAL.Inventory;

public class InvStockAdjustment
{
	public int SetData(Architecture.Model.Inventory.InvStockAdjustment obj, string ProcName, bool InventoryFinancialsEffectsInActive = false)
	{
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
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
			bool LoaderFlag = false;
			foreach (InvStockAdjustmentDetail item4 in obj.InvStockAdjustmentDetailslist)
			{
				if (item4.RefDocumentTypeId > 0 && item4.RefDocNoId > 0 && item4.RefDocSubIdNo > 0)
				{
					LoaderFlag = true;
				}
				item4.InvStockAdjustmentId = obj.Id;
				item4.LineId = LineId;
				int CodeDetail = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Sp_InvStockAdjustmentDetail_Insert"));
				item4.Id = CodeDetail;
				LineId++;
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
				foreach (DMSAttachments item3 in obj.AttachmentsList.ToList())
				{
					item3.RefAccountId = obj.AdjustmentTypeId;
					item3.RefDocumentTypeId = obj.DocumentTypeId;
					item3.RefDocumentNo = returnID;
					item3.OrganizationId = obj.OrganizationId;
					item3.CompanyId = obj.CompanyId;
					item3.ModifyDate = obj.ModifyDate;
					item3.ModifyUser = obj.ModifyUserId;
					item3.EntryUser = obj.EnteryUserId;
					item3.EntryDate = obj.EnteryDate;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Proc_DMSAttachments_Insert"));
				}
			}
			bool FIFOCgs = CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5);
			if (obj.DocumentTypeId != 215 && FIFOCgs && obj.AdjustmentTypeId == 2 && !LoaderFlag)
			{
				List<Architecture.Model.Inventory.GetItemGlIdsandItemName> dtitem = (dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId));
				string RemarksCgs = "";
				obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
				foreach (InvStockAdjustmentDetail item7 in obj.InvStockAdjustmentDetailslist.ToList())
				{
					List<Architecture.Model.Inventory.GetItemGlIdsandItemName> find = dtitem.Where((Architecture.Model.Inventory.GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item7.ItemId)).ToList();
					if (find.Count <= 0)
					{
						continue;
					}
					ReportsParameters stock = new ReportsParameters();
					stock.OrganizationId = obj.OrganizationId;
					stock.CompanyId = obj.CompanyId;
					stock.ItemId = item7.ItemId;
					stock.WarehouseId = item7.WarehouseId;
					stock.DocDate = obj.DocDate;
					if (obj.DocumentTypeId == 70)
					{
						stock.stockUOM = item7.PackUomId;
						stock.JobLotId = item7.JobLotId;
						stock.CropYear = item7.CropYear;
						stock.PackingTypeId = item7.PackingTypeId;
						stock.RateUOM = item7.RateUomId;
					}
					if (obj.ModifyUserId > 0)
					{
						stock.DocumentTypeId = obj.DocumentTypeId;
						stock.Id = obj.Id;
					}
					stock.ItemQty = Conversion.ToDouble(item7.Qty);
					stock.NetWeight = Conversion.ToDouble(item7.NetWeight);
					stock.LineId = item7.LineId;
					stock.ItemName = Conversion.ToString(find[0].ItemName);
					foreach (InventoryStockEvalautionDetail stockdetail in CommonServices.FIFOImplemention(stock, obj.InventoryStockEvalautionDetailslist).InventoryStockEvalautionDetailslist.ToList())
					{
						RemarksCgs = $"ItemQty: {stockdetail.QtyOut}, Item: {find[0].ItemName}";
						if (obj.DocumentTypeId == 70)
						{
							RemarksCgs += $", Net Weight: {stockdetail.StockWeightOut}";
						}
						RemarksCgs += $", CGS Rate: {stockdetail.CgsRate}";
						if (!InventoryFinancialsEffectsInActive)
						{
							VoucherDetail vd7 = new VoucherDetail();
							vd7.LineId = item7.LineId;
							vd7.IsCGS = 1;
							vd7.AccountId = Conversion.ToInt(item7.DebitAccountId);
							vd7.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							vd7.Comments = Conversion.ToString(RemarksCgs);
							vd7.DebitAmount = Conversion.ToDouble(stockdetail.CgsAmount);
							vd7.ItemId = Conversion.ToInt(item7.ItemId);
							vd7.QtyOut = Conversion.ToDouble(stockdetail.QtyOut);
							vd7.WeightOut = Conversion.ToDouble(stockdetail.StockWeightOut);
							vd7.ItemCgsRate = Conversion.ToDouble(stockdetail.CgsRate);
							vd7.JobLotId = Conversion.ToInt(item7.JobLotId);
							vd7.BranchesId = obj.BranchesId;
							obj.VoucherHeadInvoices.voucherDetailList.Add(vd7);
							VoucherDetail vd6 = new VoucherDetail();
							vd6.LineId = item7.LineId;
							vd6.IsCGS = 1;
							vd6.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							vd6.AgainstAccountId = Conversion.ToInt(item7.DebitAccountId);
							vd6.Comments = Conversion.ToString(RemarksCgs);
							vd6.CreditAmount = Conversion.ToDouble(stockdetail.CgsAmount);
							vd6.ItemId = Conversion.ToInt(item7.ItemId);
							vd6.QtyOut = Conversion.ToDouble(stockdetail.QtyOut);
							vd6.WeightOut = Conversion.ToDouble(stockdetail.StockWeightOut);
							vd6.ItemCgsRate = Conversion.ToDouble(stockdetail.CgsRate);
							vd6.ItemAmount = Conversion.ToDouble(stockdetail.AmountOut);
							vd6.JobLotId = Conversion.ToInt(item7.JobLotId);
							vd6.BranchesId = obj.BranchesId;
							obj.VoucherHeadInvoices.voucherDetailList.Add(vd6);
						}
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
						item6.SupplierCustomerId = 0;
						item6.BranchesId = obj.BranchesId;
						item6.RefDocumentTypeId = obj.DocumentTypeId;
						item6.EntryUser = obj.EnteryUserId;
						item6.ModifyUser = obj.ModifyUserId;
						item6.CalcType = ((obj.DocumentTypeId == 70) ? "Weight" : "Qty");
						item6.AmountOut = item6.CgsAmount;
						InvStockAdjustmentDetail sid2 = obj.InvStockAdjustmentDetailslist.Find((InvStockAdjustmentDetail x) => x.LineId == item6.LineId && x.LineId > 0 && item6.LineId > 0);
						if (sid2 != null)
						{
							item6.RefDocIdNo = sid2.InvStockAdjustmentId;
							item6.RefDocSubIdNo = sid2.Id;
							item6.ItemConditionId = sid2.ItemConditionId;
						}
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "USP_InventoryStockEvalautionDetail_Insert"));
					}
				}
			}
			else
			{
				InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
				inventoryTransactions.OrganizationId = obj.OrganizationId;
				inventoryTransactions.CompanyId = obj.CompanyId;
				inventoryTransactions.RefDocumentTypeId = obj.DocumentTypeId;
				inventoryTransactions.RefDocIdNo = returnID;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
			}
			InventoryTransactions transactions = new InventoryTransactions();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = returnID;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			if (obj.AdjustmentTypeId == 2)
			{
				foreach (InvStockAdjustmentDetail item2 in obj.InvStockAdjustmentDetailslist)
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
					sqlCommand2.Parameters.AddWithValue("@ItemId", item2.ItemId);
					sqlCommand2.Parameters.AddWithValue("@WarehouseId", item2.WarehouseId);
					sqlCommand2.Parameters.AddWithValue("@JobLotId", item2.JobLotId);
					sqlCommand2.Parameters.AddWithValue("@CropYearId", item2.CropYearId);
					sqlCommand2.Parameters.AddWithValue("@InvPackingTypeId", item2.PackingTypeId);
					sqlCommand2.Parameters.AddWithValue("@PackUomId", item2.PackUomId);
					sqlCommand2.Parameters.AddWithValue("@NetWeight", item2.NetWeight);
					sqlCommand2.Parameters.AddWithValue("@RefDocumentTypeId", item2.RefDocumentTypeId);
					sqlCommand2.Parameters.AddWithValue("@RefDocNoId", item2.RefDocNoId);
					sqlCommand2.Parameters.AddWithValue("@RefDocSubIdNo", item2.RefDocSubIdNo);
					sqlCommand2.Parameters.AddWithValue("@ItemConditionId", item2.ItemConditionId);
					sqlCommand2.ExecuteNonQuery();
				}
			}
			if (!InventoryFinancialsEffectsInActive)
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
				if (obj.VoucherHeadInvoices.voucherDetailList == null || obj.VoucherHeadInvoices.voucherDetailList.Count <= 0)
				{
					throw new Exception("VoucherDetail List Not Found");
				}
				foreach (VoucherDetail item5 in obj.VoucherHeadInvoices.voucherDetailList)
				{
					item5.VoucherHeadId = obj.VoucherHeadInvoices.Id;
					item5.BranchesId = obj.BranchesId;
					InvStockAdjustmentDetail sid = obj.InvStockAdjustmentDetailslist.Find((InvStockAdjustmentDetail x) => x.LineId == item5.LineId && x.LineId > 0 && item5.LineId > 0);
					if (sid != null)
					{
						item5.RefDocSubIdNo = sid.Id;
					}
					item5.IsCGS = ((obj.AdjustmentTypeId == 2) ? 1 : 0);
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "Sp_VoucherDetail_Insert"));
				}
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_H_Insert"));
				foreach (VoucherDetail item in obj.VoucherHeadInvoices.voucherDetailList)
				{
					item.VoucherHeadId = obj.VoucherHeadInvoices.Id;
					item.DocumentTypeIdRef = Code;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_H_Insert"));
				}
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

	public List<Architecture.Model.Inventory.InvStockAdjustment> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.InvStockAdjustment> lst = new List<Architecture.Model.Inventory.InvStockAdjustment>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Inventory.InvStockAdjustment>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Inventory.InvStockAdjustment item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.InvStockAdjustmentDetailslist = GenericProvider.GetAllDetail<InvStockAdjustmentDetail>(sqlcon, "Sp_InvStockAdjustment_GetAllMethod", lstPrm);
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
