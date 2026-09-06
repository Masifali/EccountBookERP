using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.PartyProcessing;

namespace Architecture.DAL.PartyProcessing;

public class InvFoodProductionPartyProcessingPackingMaterial
{
	public int SetData(Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessingPackingMaterial obj, string ProcName)
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
			InventoryTransactionsPartyProcessing transactions = new InventoryTransactionsPartyProcessing();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = returnID;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactionsPartyProcessing_Insert"));
			if (obj.DocumentTypeId == 119)
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
				sqlCommand.Parameters.AddWithValue("@ItemId", obj.ItemId);
				sqlCommand.Parameters.AddWithValue("@WarehouseId", obj.WarehouseId);
				sqlCommand.Parameters.AddWithValue("@NetWeight", obj.Qty);
				sqlCommand.ExecuteNonQuery();
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

	public List<Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessingPackingMaterial> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessingPackingMaterial> lst = new List<Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessingPackingMaterial>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessingPackingMaterial>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
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
			sqlCommand.CommandText = "Sp_InvFoodProductionPartyProcessingPackingMaterial_GetAllMethod";
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
