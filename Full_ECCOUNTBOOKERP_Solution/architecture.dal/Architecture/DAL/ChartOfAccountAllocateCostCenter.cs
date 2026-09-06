using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class ChartOfAccountAllocateCostCenter
{
	public long SetData(Architecture.Model.ChartOfAccountAllocateCostCenter obj, string ProcName)
	{
		long returnID = 0L;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.ChartOfAccountAllocateCostCenter item in obj.ChartOfAccountAllocateCostCenterList)
			{
				returnID = Conversion.ToInt64(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public List<Architecture.Model.ChartOfAccountAllocateCostCenter> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ChartOfAccountAllocateCostCenter> lst = new List<Architecture.Model.ChartOfAccountAllocateCostCenter>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.ChartOfAccountAllocateCostCenter>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
