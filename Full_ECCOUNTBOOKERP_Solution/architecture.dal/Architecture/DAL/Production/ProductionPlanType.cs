using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Production;

namespace Architecture.DAL.Production;

public class ProductionPlanType
{
	public int SetDate(Architecture.Model.Production.ProductionPlanType obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = Convert.ToInt32(obj.Id);
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

	public List<Architecture.Model.Production.ProductionPlanType> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Production.ProductionPlanType> lst = new List<Architecture.Model.Production.ProductionPlanType>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Production.ProductionPlanType>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
