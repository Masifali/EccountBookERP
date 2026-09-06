using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.DAW;

namespace Architecture.DAL.DAW;

public class DocumentApprovalPolicy
{
	public long SetData(Architecture.Model.DAW.DocumentApprovalPolicy obj, string ProcName)
	{
		long returnID = 0L;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID == 0L)
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

	public List<Architecture.Model.DAW.DocumentApprovalPolicy> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.DAW.DocumentApprovalPolicy> lst = new List<Architecture.Model.DAW.DocumentApprovalPolicy>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.DAW.DocumentApprovalPolicy>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
