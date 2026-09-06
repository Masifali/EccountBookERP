using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class GrainMarket : DALBase
{
	public int SetData(Architecture.Model.Inventory.GrainMarket obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID == 0)
			{
				returnID = obj.Id;
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

	public List<Architecture.Model.Inventory.GrainMarket> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.GrainMarket> lst = new List<Architecture.Model.Inventory.GrainMarket>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<Architecture.Model.Inventory.GrainMarket>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
