using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.FeedMill.Production;

namespace Architecture.DAL.FeedMill.Production;

public class ProductionElectricityUnits
{
	public int SetData(Architecture.Model.FeedMill.Production.ProductionElectricityUnits obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.FeedMill.Production.ProductionElectricityUnits item in obj.ProductionElectricityUnitsList)
			{
				returnID = 0;
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

	public List<Architecture.Model.FeedMill.Production.ProductionElectricityUnits> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.FeedMill.Production.ProductionElectricityUnits> lst = new List<Architecture.Model.FeedMill.Production.ProductionElectricityUnits>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.FeedMill.Production.ProductionElectricityUnits>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
