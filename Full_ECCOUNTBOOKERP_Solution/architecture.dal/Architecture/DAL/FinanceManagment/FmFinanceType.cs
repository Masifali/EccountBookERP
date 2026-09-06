using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.FinanceManagment;

namespace Architecture.DAL.FinanceManagment;

public class FmFinanceType
{
	public int SetData(Architecture.Model.FinanceManagment.FmFinanceType obj, string ProcName)
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

	public List<Architecture.Model.FinanceManagment.FmFinanceType> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.FinanceManagment.FmFinanceType> lst = new List<Architecture.Model.FinanceManagment.FmFinanceType>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.FinanceManagment.FmFinanceType>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
