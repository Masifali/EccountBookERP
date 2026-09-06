using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.PolicyManagment;

namespace Architecture.DAL.HRM.PolicyManagment;

public class hrmEOBIPolicy
{
	public int SetData(Architecture.Model.HRM.PolicyManagment.hrmEOBIPolicy obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.EOBIPolicyId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.EOBIPolicyId;
			}
			foreach (hrmEOBIPolicyEmployee item in obj.EOBIPolicyEmployeesList)
			{
				item.hrmEOBIPolicyId = obj.EOBIPolicyId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_hrmEOBIPolicyEmployee_Insert"));
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

	public List<Architecture.Model.HRM.PolicyManagment.hrmEOBIPolicy> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.PolicyManagment.hrmEOBIPolicy> lst = new List<Architecture.Model.HRM.PolicyManagment.hrmEOBIPolicy>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.HRM.PolicyManagment.hrmEOBIPolicy>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.HRM.PolicyManagment.hrmEOBIPolicy item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@EOBIPolicyId", item.EOBIPolicyId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadhrmEOBIPolicyEmployeeByHeaderId"));
					item.EOBIPolicyEmployeesList = GenericProvider.Get<hrmEOBIPolicyEmployee>(sqlcon, "Sp_hrmEOBIPolicy_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}
}
