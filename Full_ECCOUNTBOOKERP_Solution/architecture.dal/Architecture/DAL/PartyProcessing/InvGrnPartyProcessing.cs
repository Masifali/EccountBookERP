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
using Architecture.Model.Inventory;
using Architecture.Model.PartyProcessing;
using Architecture.Model.VPS;

namespace Architecture.DAL.PartyProcessing;

public class InvGrnPartyProcessing : DALBase
{
	public int InventoryTransactionsPartyProcessing_Insert(int OrganizationId, int CompanyId, int RefDocIdNo, int RefDocumentTypeId)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Sp_InventoryTransactionsPartyProcessing_Insert");
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, CompanyId);
			sqlDataBase.AddInParameter(cmd, "RefDocIdNo", SqlDbType.Int, RefDocIdNo);
			sqlDataBase.AddInParameter(cmd, "RefDocumentTypeId", SqlDbType.Int, RefDocumentTypeId);
			return Conversion.ToInt(sqlDataBase.ExecuteNonQuery(cmd));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public int SetData(Architecture.Model.PartyProcessing.InvGrnPartyProcessing obj, string ProcName)
	{
		int returnID = 0;
		string AttachmentFolderPath = "";
		List<string> copiedFiles = new List<string>();
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.InvGrnPartyProcessingDetailslist.Count == 0 || obj.InvGrnPartyProcessingDetailslist == null)
			{
				throw new Exception("InvGrnPartyProcessingDetailslist not found");
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
			bool loaderflag = false;
			foreach (InvGrnPartyProcessingDetail item4 in obj.InvGrnPartyProcessingDetailslist)
			{
				if (obj.DocumentTypeId == 89 && item4.ActionTypeId != 3 && item4.RefDocumentTypeId > 0 && item4.RefDocNoId > 0 && item4.RefDocSubIdNo > 0)
				{
					loaderflag = true;
				}
				item4.LineId = LineId;
				item4.InvGrnPartyProcessingId = obj.Id;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Sp_InvGrnPartyProcessingDetail_Insert"));
				item4.Id = code;
				LineId++;
			}
			foreach (InvGrnEmptyBagsPartyProcessing item3 in obj.InvGrnEmptyBagsPartyProcessingslist)
			{
				item3.InvGrnPartyProcessingId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Sp_InvGrnEmptyBagsPartyProcessing_Insert"));
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
					item2.RefAccountId = obj.StockPartyId;
					item2.RefDocumentTypeId = obj.DocumentTypeId;
					item2.RefDocumentNo = obj.Id;
					item2.ScreenName = obj.ScreenName;
					item2.OrganizationId = obj.OrganizationId;
					item2.CompanyId = obj.CompanyId;
					item2.EntryUser = obj.EntryUser;
					item2.ModifyUser = obj.EntryUser;
					item2.EntryDate = obj.EntryDate;
					item2.ModifyDate = obj.ModifyDate;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Proc_DMSAttachments_Insert"));
				}
			}
			if (obj.DocumentTypeId == 89 && !loaderflag)
			{
				List<GetItemGlIdsandItemName> dtitem = (dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId));
				obj.InventoryTransactionsPartyProcessingForFIFOlist = new List<InventoryTransactionsPartyProcessingForFifo>();
				double TotalBillWeight = obj.InvGrnPartyProcessingDetailslist.Where((InvGrnPartyProcessingDetail x) => x.ActionTypeId != 3).Sum((InvGrnPartyProcessingDetail x) => x.NetBillWeight);
				double TotalStockWeight = obj.InvGrnPartyProcessingDetailslist.Where((InvGrnPartyProcessingDetail x) => x.ActionTypeId != 3).Sum((InvGrnPartyProcessingDetail x) => x.StockWeight);
				foreach (InvGrnPartyProcessingDetail item6 in obj.InvGrnPartyProcessingDetailslist.ToList())
				{
					if (item6.ActionTypeId == 3)
					{
						continue;
					}
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item6.ItemId)).ToList();
					if (find.Count <= 0)
					{
						continue;
					}
					ReportsParameters stock = new ReportsParameters();
					stock.OrganizationId = obj.OrganizationId;
					stock.CompanyId = obj.CompanyId;
					stock.ItemId = item6.ItemId;
					stock.StockPartyId = obj.StockPartyId;
					stock.WarehouseId = item6.WarehouseId;
					stock.DocDate = obj.DocDate;
					stock.stockUOM = item6.ItemUomId;
					stock.JobLotId = item6.JobLotId;
					stock.CropYearId = item6.CropYearId;
					stock.PackingTypeId = item6.PackingTypeId;
					if (obj.ModifyUser > 0)
					{
						stock.DocumentTypeId = obj.DocumentTypeId;
						stock.Id = obj.Id;
					}
					stock.ItemQty = item6.ItemQty;
					stock.NetWeight = item6.StockWeight;
					stock.LineId = item6.LineId;
					stock.ItemName = Conversion.ToString(find[0].ItemName);
					foreach (InventoryTransactionsPartyProcessingForFifo stockdetail in CommonServices.FIFOImplementionForPartyProcessing(stock, obj.InventoryTransactionsPartyProcessingForFIFOlist).InventoryTransactionsPartyProcessingForFIFOlist.ToList())
					{
						if (TotalBillWeight > 0.0 && TotalStockWeight > 0.0)
						{
							stockdetail.BillWeightOut = TotalBillWeight / TotalStockWeight * stockdetail.StockWeightOut;
						}
						stockdetail.CityId = item6.CityId;
						stockdetail.GpNo = obj.GpNo;
						stockdetail.VehicleNo = obj.VehicleNo;
						stockdetail.TranRemarks = "Goods Dispatch Notes Party Processing";
						stockdetail.GrossWeight = item6.GrossWeight;
						stockdetail.EbUnit = item6.EBWPerUnit;
						stockdetail.EbTotal = item6.EBWTotal;
						stockdetail.AddLess = item6.AdLsWeight;
						stockdetail.TransporterId = obj.TransporterId;
						stockdetail.FreightAmount = obj.CarriageAmount;
						stockdetail.DetailRemarks = item6.CommentsDetail;
						obj.InventoryTransactionsPartyProcessingForFIFOlist.Add(stockdetail);
					}
				}
				if (obj.InventoryTransactionsPartyProcessingForFIFOlist != null && obj.InventoryTransactionsPartyProcessingForFIFOlist.Count > 0)
				{
					if (obj.ModifyUser > 0)
					{
						SqlCommand sqlCommand = new SqlCommand();
						sqlCommand.Connection = sqlcon;
						sqlCommand.Transaction = sqlTrn;
						sqlCommand.CommandType = CommandType.StoredProcedure;
						sqlCommand.CommandText = "[dbo].[USP_InventoryPartyProcessingQtyReverseAndDeleteByReferenceId]";
						sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
						sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
						sqlCommand.Parameters.AddWithValue("@RefDocumentTypeId", obj.DocumentTypeId);
						sqlCommand.Parameters.AddWithValue("@RefDocIdNo", obj.Id);
						sqlCommand.ExecuteNonQuery();
					}
					foreach (InventoryTransactionsPartyProcessingForFifo item5 in obj.InventoryTransactionsPartyProcessingForFIFOlist)
					{
						item5.OrganizationId = obj.OrganizationId;
						item5.CompanyId = obj.CompanyId;
						item5.DocDate = obj.DocDate;
						item5.DocCodeNo = obj.DocNo;
						item5.SupplierCustomerId = obj.SupplierCustomerId;
						item5.StockPartyId = obj.StockPartyId;
						item5.BranchesId = obj.BranchesId;
						item5.RefDocumentTypeId = obj.DocumentTypeId;
						item5.CalcType = "Weight";
						item5.EntryUserId = obj.EntryUser;
						item5.ModifyUserId = obj.ModifyUser;
						item5.EntryDate = DateTime.Now;
						item5.ModifyDate = DateTime.Now;
						InvGrnPartyProcessingDetail sid = obj.InvGrnPartyProcessingDetailslist.Find((InvGrnPartyProcessingDetail x) => x.LineId == item5.LineId && x.LineId > 0 && item5.LineId > 0);
						if (sid != null)
						{
							item5.RefDocIdNo = sid.InvGrnPartyProcessingId;
							item5.RefDocSubIdNo = sid.Id;
						}
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "USP_InventoryTransactionsPartyProcessing_Insert"));
					}
				}
			}
			if (obj.DocumentTypeId == 89 && loaderflag)
			{
				InventoryTransactionsPartyProcessing transactions = new InventoryTransactionsPartyProcessing();
				transactions.OrganizationId = obj.OrganizationId;
				transactions.CompanyId = obj.CompanyId;
				transactions.RefDocumentTypeId = obj.DocumentTypeId;
				transactions.RefDocIdNo = returnID;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactionsPartyProcessing_Insert"));
			}
			else if (obj.DocumentTypeId == 49)
			{
				InventoryTransactionsPartyProcessing transactions2 = new InventoryTransactionsPartyProcessing();
				transactions2.OrganizationId = obj.OrganizationId;
				transactions2.CompanyId = obj.CompanyId;
				transactions2.RefDocumentTypeId = obj.DocumentTypeId;
				transactions2.RefDocIdNo = returnID;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions2, "Sp_InventoryTransactionsPartyProcessing_Insert"));
			}
			if (obj.DocumentTypeId == 89)
			{
				foreach (InvGrnPartyProcessingDetail item in obj.InvGrnPartyProcessingDetailslist)
				{
					SqlCommand sqlCommand2 = new SqlCommand();
					sqlCommand2.Connection = sqlcon;
					sqlCommand2.Transaction = sqlTrn;
					sqlCommand2.CommandType = CommandType.StoredProcedure;
					sqlCommand2.CommandText = "USP_InventoryValidationPartyProcessing";
					sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand2.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
					sqlCommand2.Parameters.AddWithValue("@DocDate", obj.DocDate);
					sqlCommand2.Parameters.AddWithValue("@StockPartyId", obj.StockPartyId);
					sqlCommand2.Parameters.AddWithValue("@ItemId", item.ItemId);
					sqlCommand2.Parameters.AddWithValue("@WarehouseId", item.WarehouseId);
					sqlCommand2.Parameters.AddWithValue("@JobLotId", item.JobLotId);
					sqlCommand2.Parameters.AddWithValue("@CropYearId", item.CropYearId);
					sqlCommand2.Parameters.AddWithValue("@InvPackingTypeId", item.PackingTypeId);
					sqlCommand2.Parameters.AddWithValue("@PackUomId", item.ItemUomId);
					sqlCommand2.Parameters.AddWithValue("@NetWeight", item.NetBillWeight);
					sqlCommand2.Parameters.AddWithValue("@RefDocumentTypeId", item.RefDocumentTypeId);
					sqlCommand2.Parameters.AddWithValue("@RefDocNoId", item.RefDocNoId);
					sqlCommand2.Parameters.AddWithValue("@RefDocSubIdNo", item.RefDocSubIdNo);
					sqlCommand2.ExecuteNonQuery();
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

	public List<Architecture.Model.PartyProcessing.InvGrnPartyProcessing> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.PartyProcessing.InvGrnPartyProcessing> lst = new List<Architecture.Model.PartyProcessing.InvGrnPartyProcessing>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.PartyProcessing.InvGrnPartyProcessing>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.PartyProcessing.InvGrnPartyProcessing item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
						item2.InvGrnPartyProcessingDetailslist = GenericProvider.Get<InvGrnPartyProcessingDetail>(sqlConnection2, "Sp_InvGrnPartyProcessing_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.PartyProcessing.InvGrnPartyProcessing item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadEmptyBagsByHeaderId"));
					item.InvGrnEmptyBagsPartyProcessingslist = GenericProvider.Get<InvGrnEmptyBagsPartyProcessing>(sqlcon, "Sp_InvGrnPartyProcessing_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public void DeleteById(int Id, int EntryUserId)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "Sp_InvGrnPartyProcessing_GetAllMethod";
			sqlCommand.Parameters.AddWithValue("@Id", Id);
			sqlCommand.Parameters.AddWithValue("@EntryUser", EntryUserId);
			sqlCommand.Parameters.AddWithValue("@Activity", "DeleteById");
			sqlCommand.ExecuteScalar();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}
}
