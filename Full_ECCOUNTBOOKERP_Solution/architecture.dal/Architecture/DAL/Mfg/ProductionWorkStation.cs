using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Mfg;

namespace Architecture.DAL.Mfg;

public class ProductionWorkStation
{
	public int SetData(Architecture.Model.Mfg.ProductionWorkStation obj, string ProcName)
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

	public List<Architecture.Model.Mfg.ProductionWorkStation> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Mfg.ProductionWorkStation> lst = new List<Architecture.Model.Mfg.ProductionWorkStation>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Mfg.ProductionWorkStation>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public List<Architecture.Model.Mfg.ProductionWorkStation> GetById(string ProcName, int Id)
	{
		List<Architecture.Model.Mfg.ProductionWorkStation> lst = new List<Architecture.Model.Mfg.ProductionWorkStation>();
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Mfg.ProductionWorkStation>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
