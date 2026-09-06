using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.CRM;

namespace Architecture.DAL.CRM;

public class CRMLookupType : DALBase
{
	public int SetData(Architecture.Model.CRM.CRMLookupType obj, string ProcName)
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

	public List<Architecture.Model.CRM.CRMLookupType> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.CRM.CRMLookupType> lst = new List<Architecture.Model.CRM.CRMLookupType>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.CRM.CRMLookupType>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
