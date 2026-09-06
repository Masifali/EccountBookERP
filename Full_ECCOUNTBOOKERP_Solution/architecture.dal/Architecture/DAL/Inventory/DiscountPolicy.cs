using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class DiscountPolicy
{
	public int SetData(Architecture.Model.Inventory.DiscountPolicy obj, string ProcName)
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
				obj.Id = returnID;
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

	public List<Architecture.Model.Inventory.DiscountPolicy> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.DiscountPolicy> lst = new List<Architecture.Model.Inventory.DiscountPolicy>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Inventory.DiscountPolicy>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
