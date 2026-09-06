using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.PartyProcessing;

namespace Architecture.DAL.PartyProcessing;

public class InvWareHousePartyProcessing
{
	public int SetData(Architecture.Model.PartyProcessing.InvWareHousePartyProcessing obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.PartyProcessing.InvWareHousePartyProcessing> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.PartyProcessing.InvWareHousePartyProcessing> lst = new List<Architecture.Model.PartyProcessing.InvWareHousePartyProcessing>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.PartyProcessing.InvWareHousePartyProcessing>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
