using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class ItemAttributVariant
{
	public long SetData(Architecture.Model.Inventory.ItemAttributVariant obj, string ProcName)
	{
		long returnID = 0L;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Inventory.ItemAttributVariant item in obj.ItemAttributVariantList)
			{
				returnID = Conversion.ToInt64(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public List<Architecture.Model.Inventory.ItemAttributVariant> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.ItemAttributVariant> lst = new List<Architecture.Model.Inventory.ItemAttributVariant>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Inventory.ItemAttributVariant>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
