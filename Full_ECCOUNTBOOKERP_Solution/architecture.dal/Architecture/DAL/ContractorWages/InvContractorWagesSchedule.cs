using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.ContractorWages;

namespace Architecture.DAL.ContractorWages;

public class InvContractorWagesSchedule : DALBase
{
	public int SetDate(Architecture.Model.ContractorWages.InvContractorWagesSchedule obj, string ProcName)
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

	public List<Architecture.Model.ContractorWages.InvContractorWagesSchedule> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ContractorWages.InvContractorWagesSchedule> lst = new List<Architecture.Model.ContractorWages.InvContractorWagesSchedule>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.ContractorWages.InvContractorWagesSchedule>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
