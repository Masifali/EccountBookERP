using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.PolicyManagment;

namespace Architecture.DAL.HRM.PolicyManagment;

public class genProfileType
{
	public int SetData(Architecture.Model.HRM.PolicyManagment.genProfileType obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.ProfileTypeId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.ProfileTypeId;
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

	public List<Architecture.Model.HRM.PolicyManagment.genProfileType> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.PolicyManagment.genProfileType> lst = new List<Architecture.Model.HRM.PolicyManagment.genProfileType>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.HRM.PolicyManagment.genProfileType>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
