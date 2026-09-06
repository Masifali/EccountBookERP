using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;
using Architecture.Model.Production;

namespace Architecture.DAL.Production;

public class InvProductionCumPackMaterialConsumptionHeader
{
	public int SetData(Architecture.Model.Production.InvProductionCumPackMaterialConsumptionHeader obj, string ProcName)
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
			foreach (InvProductionCumPackMaterialConsumptionDetail item in obj.InvProductionCumPackMaterialConsumptionDetailslist)
			{
				item.InvProductionCumPackMaterialConsumptionHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_InvProductionCumPackMaterialConsumptionDetail_Insert"));
			}
			InventoryTransactions transactions = new InventoryTransactions();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = returnID;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Production.InvProductionCumPackMaterialConsumptionHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Production.InvProductionCumPackMaterialConsumptionHeader> lst = new List<Architecture.Model.Production.InvProductionCumPackMaterialConsumptionHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Production.InvProductionCumPackMaterialConsumptionHeader>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Production.InvProductionCumPackMaterialConsumptionHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.InvProductionCumPackMaterialConsumptionDetailslist = GenericProvider.Get<InvProductionCumPackMaterialConsumptionDetail>(sqlcon, "Sp_InvProductionCumPackMaterialConsumptionHeader_GetAllMethod", lstPrm);
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
