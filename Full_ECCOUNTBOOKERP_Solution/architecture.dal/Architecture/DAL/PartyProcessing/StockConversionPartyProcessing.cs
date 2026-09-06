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

namespace Architecture.DAL.PartyProcessing;

public class StockConversionPartyProcessing
{
	public int SetData(Architecture.Model.PartyProcessing.StockConversionPartyProcessing obj, string ProcName)
	{
		new List<GetItemGlIdsandItemName>();
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
			foreach (StockConversionPartyProcessingDetail item5 in obj.StockConversionPartyProcessingDetail)
			{
				item5.InvStockConversionPartyProcessingId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "USP_InvStockConversionPartyProcessingDetail_Insert"));
			}
			foreach (StockConversionPartyProcessingPackingMaterial item4 in obj.StockConversionPartyProcessingPackingMaterial)
			{
				item4.InvStockConversionPartyProcessingId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[USP_InvStockConversionPartyProcessingPackingMaterial_Insert]"));
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
					item3.RefAccountId = obj.StockPartyId;
					item3.RefDocumentTypeId = obj.DocumentTypeId;
					item3.RefDocumentNo = obj.Id;
					item3.OrganizationId = obj.OrganizationId;
					item3.CompanyId = obj.CompanyId;
					item3.EntryUser = obj.EntryUser;
					item3.ModifyUser = obj.ModifyUser;
					item3.EntryDate = obj.EntryDate;
					item3.ModifyDate = obj.ModifyDate;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Proc_DMSAttachments_Insert"));
				}
			}
			InventoryTransactionsPartyProcessing transactions = new InventoryTransactionsPartyProcessing();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = returnID;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactionsPartyProcessing_Insert"));
			foreach (StockConversionPartyProcessingDetail item2 in obj.StockConversionPartyProcessingDetail)
			{
				if (item2.ActionTypeId != 3 && item2.EntryType == "Issue")
				{
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlcon;
					sqlCommand.Transaction = sqlTrn;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandText = "USP_InventoryValidationPartyProcessing";
					sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
					sqlCommand.Parameters.AddWithValue("@DocDate", obj.DocDate);
					sqlCommand.Parameters.AddWithValue("@StockPartyId", item2.StockPartyId);
					sqlCommand.Parameters.AddWithValue("@ItemId", item2.ItemId);
					sqlCommand.Parameters.AddWithValue("@WarehouseId", item2.WarehouseId);
					sqlCommand.Parameters.AddWithValue("@JobLotId", item2.JobLotId);
					sqlCommand.Parameters.AddWithValue("@CropYearId", item2.CropYearId);
					sqlCommand.Parameters.AddWithValue("@InvPackingTypeId", item2.PackingtypeId);
					sqlCommand.Parameters.AddWithValue("@PackUomId", item2.ItemUomId);
					sqlCommand.Parameters.AddWithValue("@NetWeight", item2.Weight);
					sqlCommand.Parameters.AddWithValue("@RefDocumentTypeId", item2.RefDocumentTypeId);
					sqlCommand.Parameters.AddWithValue("@RefDocNoId", item2.RefDocNoId);
					sqlCommand.Parameters.AddWithValue("@RefDocSubIdNo", item2.RefDocSubIdNo);
					sqlCommand.ExecuteNonQuery();
				}
			}
			if (obj.StockConversionPartyProcessingPackingMaterial.Count > 0 && obj.StockConversionPartyProcessingPackingMaterial != null)
			{
				foreach (StockConversionPartyProcessingPackingMaterial item in obj.StockConversionPartyProcessingPackingMaterial)
				{
					SqlCommand sqlCommand2 = new SqlCommand();
					sqlCommand2.Connection = sqlcon;
					sqlCommand2.Transaction = sqlTrn;
					sqlCommand2.CommandType = CommandType.StoredProcedure;
					sqlCommand2.CommandText = "USP_InventoryValidationPartyProcessing";
					sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand2.Parameters.AddWithValue("@DocDate", obj.DocDate);
					sqlCommand2.Parameters.AddWithValue("@ItemId", item.ItemId);
					sqlCommand2.Parameters.AddWithValue("@WarehouseId", item.WarehouseId);
					sqlCommand2.Parameters.AddWithValue("@NetWeight", item.ItemQty);
					sqlCommand2.Parameters.AddWithValue("@Activity", "PM");
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

	public List<Architecture.Model.PartyProcessing.StockConversionPartyProcessing> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.PartyProcessing.StockConversionPartyProcessing> lst = new List<Architecture.Model.PartyProcessing.StockConversionPartyProcessing>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.PartyProcessing.StockConversionPartyProcessing>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.PartyProcessing.StockConversionPartyProcessing item2 in lst)
					{
						if (item2.DocumentTypeId == 176)
						{
							List<SqlParameter> lstPrm2 = new List<SqlParameter>();
							lstPrm2.Add(new SqlParameter("@Id", item2.Id));
							item2.StockConversionPartyProcessingDetail = GenericProvider.Get<StockConversionPartyProcessingDetail>(sqlConnection2, "USP_InvStockConversionPartyProcessingDetail_ReadById", lstPrm2);
						}
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.PartyProcessing.StockConversionPartyProcessing item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					item.StockConversionPartyProcessingPackingMaterial = GenericProvider.Get<StockConversionPartyProcessingPackingMaterial>(sqlcon, "[USP_InvStockConversionPartyProcessingPackingMaterial_ReadbyId]", lstPrm);
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
