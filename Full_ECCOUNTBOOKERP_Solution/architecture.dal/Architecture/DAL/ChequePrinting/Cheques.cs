using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.ChequePrinting;

namespace Architecture.DAL.ChequePrinting;

public class Cheques
{
	public int SetData(Architecture.Model.ChequePrinting.Cheques obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.ChequePrinting.Cheques> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ChequePrinting.Cheques> lst = new List<Architecture.Model.ChequePrinting.Cheques>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.ChequePrinting.Cheques>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
