using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class AppModules
{
	public int SetData(Architecture.Model.AppModules obj, string ProcName)
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

	public int SetDataMulti(Architecture.Model.AppModules obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.AppModules item in obj.AppModuleList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public List<Architecture.Model.AppModules> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.AppModules> lst = new List<Architecture.Model.AppModules>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.AppModules>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
