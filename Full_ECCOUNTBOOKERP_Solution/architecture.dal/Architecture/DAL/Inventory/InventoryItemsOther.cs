using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class InventoryItemsOther : DALBase
{
	public int SetData(Architecture.Model.Inventory.InventoryItemsOther obj, string ProcName)
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
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public List<Architecture.Model.Inventory.InventoryItemsOther> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.InventoryItemsOther> lst = new List<Architecture.Model.Inventory.InventoryItemsOther>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Inventory.InventoryItemsOther>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
