using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class ProductionRoutingName
{
	public int SetData(ProductionRouting obj, string ProcName)
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

	public List<ProductionRouting> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<ProductionRouting> lst = new List<ProductionRouting>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<ProductionRouting>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public List<ProductionRouting> GetById(string ProcName, int Id)
	{
		List<ProductionRouting> lst = new List<ProductionRouting>();
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ProductionRoutingNameId",
				Value = Id
			});
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<ProductionRouting>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
