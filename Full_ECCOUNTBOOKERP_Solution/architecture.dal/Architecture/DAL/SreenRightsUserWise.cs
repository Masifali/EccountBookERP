using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class SreenRightsUserWise
{
	public int SetData(Architecture.Model.SreenRightsUserWise obj, string ProcName)
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

	public List<Architecture.Model.SreenRightsUserWise> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.SreenRightsUserWise> lst = new List<Architecture.Model.SreenRightsUserWise>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.SreenRightsUserWise>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
