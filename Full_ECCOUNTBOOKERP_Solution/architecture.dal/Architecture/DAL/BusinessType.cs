using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class BusinessType : DALBase
{
	public int SetData(Architecture.Model.BusinessType obj, string ProcName)
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
				obj.BusinessTypeID = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.BusinessTypeID;
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public List<Architecture.Model.BusinessType> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.BusinessType> lst = new List<Architecture.Model.BusinessType>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.BusinessType>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
