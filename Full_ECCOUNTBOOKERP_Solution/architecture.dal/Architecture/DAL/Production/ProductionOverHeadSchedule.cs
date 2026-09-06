using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Production;

namespace Architecture.DAL.Production;

public class ProductionOverHeadSchedule
{
	public int SetData(Architecture.Model.Production.ProductionOverHeadSchedule obj, string ProcName)
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
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Production.ProductionOverHeadSchedule> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Production.ProductionOverHeadSchedule> lst = new List<Architecture.Model.Production.ProductionOverHeadSchedule>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Production.ProductionOverHeadSchedule>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
