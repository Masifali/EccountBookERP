using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.WeightBridge;

namespace Architecture.DAL.WeightBridge;

public class WeighbridgeScalesList
{
	public int SetDate(Architecture.Model.WeightBridge.WeighbridgeScalesList obj, string ProcName)
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

	public List<Architecture.Model.WeightBridge.WeighbridgeScalesList> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.WeightBridge.WeighbridgeScalesList> lst = new List<Architecture.Model.WeightBridge.WeighbridgeScalesList>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.WeightBridge.WeighbridgeScalesList>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public List<Architecture.Model.WeightBridge.WeighbridgeScalesList> GetById(string ProcName, int Id)
	{
		List<Architecture.Model.WeightBridge.WeighbridgeScalesList> lst = new List<Architecture.Model.WeightBridge.WeighbridgeScalesList>();
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
			return GenericProvider.GetProc<Architecture.Model.WeightBridge.WeighbridgeScalesList>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
