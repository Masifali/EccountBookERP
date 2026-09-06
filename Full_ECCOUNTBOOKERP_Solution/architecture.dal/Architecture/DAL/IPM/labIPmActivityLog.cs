using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.IPM;

namespace Architecture.DAL.IPM;

public class labIPmActivityLog
{
	public int SetData(Architecture.Model.IPM.labIPmActivityLog obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.IPM.labIPmActivityLog item in obj.LabIPmActivityLogList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
			}
			obj.labIPmActivityLogMasterId = Convert.ToInt32(returnID);
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public List<Architecture.Model.IPM.labIPmActivityLog> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.IPM.labIPmActivityLog> lst = new List<Architecture.Model.IPM.labIPmActivityLog>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.IPM.labIPmActivityLog>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
