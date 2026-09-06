using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class InvOrderCategory : DALBase
{
	public int SetData(Architecture.Model.Inventory.InvOrderCategory obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			obj.Id = Convert.ToInt32(returnID);
			return returnID;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public List<Architecture.Model.Inventory.InvOrderCategory> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.InvOrderCategory> lst = new List<Architecture.Model.Inventory.InvOrderCategory>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Inventory.InvOrderCategory>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public List<Architecture.Model.Inventory.InvOrderCategory> GetById(string ProcName, int Id)
	{
		List<Architecture.Model.Inventory.InvOrderCategory> lst = new List<Architecture.Model.Inventory.InvOrderCategory>();
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
			return GenericProvider.GetProc<Architecture.Model.Inventory.InvOrderCategory>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public List<Architecture.Model.Inventory.InvOrderCategory> DeleteByID(string ProcName, int Id)
	{
		List<Architecture.Model.Inventory.InvOrderCategory> lst = new List<Architecture.Model.Inventory.InvOrderCategory>();
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
			return GenericProvider.GetProc<Architecture.Model.Inventory.InvOrderCategory>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
