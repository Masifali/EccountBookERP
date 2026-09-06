using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.pcc;

namespace Architecture.DAL.pcc;

public class ItemAllocationToWagesGroup
{
	public int SetData(Architecture.Model.pcc.ItemAllocationToWagesGroup obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.pcc.ItemAllocationToWagesGroup item in obj.ItemAllocationToCustomGroupsList)
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

	public List<Architecture.Model.pcc.ItemAllocationToWagesGroup> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.pcc.ItemAllocationToWagesGroup> lst = new List<Architecture.Model.pcc.ItemAllocationToWagesGroup>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.pcc.ItemAllocationToWagesGroup>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
