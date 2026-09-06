using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.DAW;

namespace Architecture.DAL.DAW;

public class DocumentApprovalLevel
{
	public int SetData(Architecture.Model.DAW.DocumentApprovalLevel obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
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

	public List<Architecture.Model.DAW.DocumentApprovalLevel> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.DAW.DocumentApprovalLevel> lst = new List<Architecture.Model.DAW.DocumentApprovalLevel>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.DAW.DocumentApprovalLevel>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
