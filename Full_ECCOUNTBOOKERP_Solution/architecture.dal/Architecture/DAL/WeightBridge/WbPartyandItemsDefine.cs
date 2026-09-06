using System;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.WeightBridge;

namespace Architecture.DAL.WeightBridge;

public class WbPartyandItemsDefine
{
	public int SetDate(Architecture.Model.WeightBridge.WbPartyandItemsDefine obj, string ProcName)
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
}
