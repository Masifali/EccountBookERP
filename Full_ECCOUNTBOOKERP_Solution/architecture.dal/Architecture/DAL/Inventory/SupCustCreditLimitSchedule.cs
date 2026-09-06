using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class SupCustCreditLimitSchedule : DALBase
{
	public int SetData(Architecture.Model.Inventory.SupCustCreditLimitSchedule obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
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

	public List<Architecture.Model.Inventory.SupCustCreditLimitSchedule> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.SupCustCreditLimitSchedule> lst = new List<Architecture.Model.Inventory.SupCustCreditLimitSchedule>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Inventory.SupCustCreditLimitSchedule>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
