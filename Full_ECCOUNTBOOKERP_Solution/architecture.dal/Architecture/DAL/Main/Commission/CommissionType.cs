using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Main.Commission;

namespace Architecture.DAL.Main.Commission;

public class CommissionType
{
	public int SetData(Architecture.Model.Main.Commission.CommissionType obj, string ProcName)
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
				obj.Id = returnID;
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

	public List<Architecture.Model.Main.Commission.CommissionType> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Main.Commission.CommissionType> lst = new List<Architecture.Model.Main.Commission.CommissionType>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Main.Commission.CommissionType>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
