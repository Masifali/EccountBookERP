using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.PartyProcessing;

namespace Architecture.DAL.PartyProcessing;

public class JobLotPartyProcessing
{
	public int SetData(Architecture.Model.PartyProcessing.JobLotPartyProcessing obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			obj.Id = Convert.ToInt32(returnID);
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.PartyProcessing.JobLotPartyProcessing> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.PartyProcessing.JobLotPartyProcessing> lst = new List<Architecture.Model.PartyProcessing.JobLotPartyProcessing>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.PartyProcessing.JobLotPartyProcessing>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
