using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class TaxesTypes : DALBase
{
	public int SetData(Architecture.Model.Inventory.TaxesTypes obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Inventory.TaxesTypes> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.TaxesTypes> lst = new List<Architecture.Model.Inventory.TaxesTypes>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Inventory.TaxesTypes>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
