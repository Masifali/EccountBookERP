using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Mfg;

namespace Architecture.DAL.Mfg;

public class productionRouting
{
	public int SetData(Architecture.Model.Mfg.productionRouting obj)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Mfg.productionRouting item in obj.ProductionRoutingList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "mfg.USP_ProductionRouting_InsertAndUpdate"));
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

	public List<Architecture.Model.Mfg.productionRouting> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Mfg.productionRouting> lst = new List<Architecture.Model.Mfg.productionRouting>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Mfg.productionRouting>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public List<Architecture.Model.Mfg.productionRouting> GetById(string ProcName, int Id)
	{
		List<Architecture.Model.Mfg.productionRouting> lst = new List<Architecture.Model.Mfg.productionRouting>();
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
			return GenericProvider.GetProc<Architecture.Model.Mfg.productionRouting>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
