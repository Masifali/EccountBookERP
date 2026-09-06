using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.PartyProcessing;

namespace Architecture.DAL.PartyProcessing;

public class InvFoodProductionPartyProcessing
{
	public int SetData(Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessing obj, string ProcName)
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
			foreach (InvFoodProductionPartyProcessingDetail item2 in obj.PartyProcessingDetailsList)
			{
				item2.InvFoodProductionPartyProcessingId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_InvFoodProductionPartyProcessingDetail_Insert"));
			}
			InventoryTransactionsPartyProcessing transactions = new InventoryTransactionsPartyProcessing();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = returnID;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactionsPartyProcessing_Insert"));
			if (obj.DocumentTypeId == 117)
			{
				foreach (InvFoodProductionPartyProcessingDetail item in obj.PartyProcessingDetailsList)
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
					sqlCommand.Parameters.AddWithValue("@StockPartyId", item.StockPartyId);
					sqlCommand.Parameters.AddWithValue("@ItemId", item.ItemId);
					sqlCommand.Parameters.AddWithValue("@WarehouseId", item.WarehouseId);
					sqlCommand.Parameters.AddWithValue("@JobLotId", item.JobLotId);
					sqlCommand.Parameters.AddWithValue("@CropYearId", item.CropYearId);
					sqlCommand.Parameters.AddWithValue("@InvPackingTypeId", item.PackingtypeId);
					sqlCommand.Parameters.AddWithValue("@PackUomId", item.ItemUomId);
					sqlCommand.Parameters.AddWithValue("@NetWeight", item.Weight);
					sqlCommand.Parameters.AddWithValue("@RefDocumentTypeId", item.RefDocumentTypeId);
					sqlCommand.Parameters.AddWithValue("@RefDocNoId", item.RefDocNoId);
					sqlCommand.Parameters.AddWithValue("@RefDocSubIdNo", item.RefDocSubIdNo);
					sqlCommand.ExecuteNonQuery();
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

	public List<Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessing> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessing> lst = new List<Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessing>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessing>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessing item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.PartyProcessingDetailsList = GenericProvider.GetAllDetail<InvFoodProductionPartyProcessingDetail>(sqlcon, "Sp_InvFoodProductionPartyProcessing_GetAllMathod", lstPrm);
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
			sqlCommand.CommandText = "Sp_InvFoodProductionPartyProcessing_GetAllMathod";
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
