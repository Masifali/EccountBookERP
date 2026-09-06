using System;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class ConsumptionItems
{
	public int SetData(Architecture.Model.Inventory.ConsumptionItems obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.ConsumptionItemslist == null || obj.ConsumptionItemslist.Count == 0)
			{
				throw new Exception("ConsumptionItemslist Not Found.....");
			}
			foreach (Architecture.Model.Inventory.ConsumptionItems item in obj.ConsumptionItemslist)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
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
}
