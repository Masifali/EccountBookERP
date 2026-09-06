using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.PartyProcessing;

namespace Architecture.DAL.PartyProcessing;

public class ItemCategoryPartyProcessing
{
	public int SetData(Architecture.Model.PartyProcessing.ItemCategoryPartyProcessing obj, string ProcName)
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

	public List<Architecture.Model.PartyProcessing.ItemCategoryPartyProcessing> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.PartyProcessing.ItemCategoryPartyProcessing> lst = new List<Architecture.Model.PartyProcessing.ItemCategoryPartyProcessing>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.PartyProcessing.ItemCategoryPartyProcessing>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
