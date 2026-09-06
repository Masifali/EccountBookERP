using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Production;

namespace Architecture.DAL.Production;

public class ProductionType
{
	public int SetDate(Architecture.Model.Production.ProductionType obj, string ProcName)
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

	public List<Architecture.Model.Production.ProductionType> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Production.ProductionType> lst = new List<Architecture.Model.Production.ProductionType>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Production.ProductionType>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
