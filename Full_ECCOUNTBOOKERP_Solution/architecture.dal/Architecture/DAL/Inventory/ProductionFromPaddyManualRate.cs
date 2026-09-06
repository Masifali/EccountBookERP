using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class ProductionFromPaddyManualRate : DALBase
{
	public int SetData(Architecture.Model.Inventory.ProductionFromPaddyManualRate obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Inventory.ProductionFromPaddyManualRate item in obj.ProductionFromPaddyManualRatesList)
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

	public List<Architecture.Model.Inventory.ProductionFromPaddyManualRate> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.ProductionFromPaddyManualRate> lst = new List<Architecture.Model.Inventory.ProductionFromPaddyManualRate>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Inventory.ProductionFromPaddyManualRate>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
