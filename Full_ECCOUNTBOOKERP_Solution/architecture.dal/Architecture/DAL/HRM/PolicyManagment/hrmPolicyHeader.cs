using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.PolicyManagment;

namespace Architecture.DAL.HRM.PolicyManagment;

public class hrmPolicyHeader
{
	public int SetData(Architecture.Model.HRM.PolicyManagment.hrmPolicyHeader obj, string ProcName)
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
				obj.PolicyHeaderId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.PolicyHeaderId;
			}
			foreach (Architecture.Model.HRM.PolicyManagment.hrmPolicyDetail item in obj.HrmPolicyDetailList)
			{
				item.HeaderId = obj.PolicyHeaderId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_hrmPolicyDetail_Insert"));
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

	public List<Architecture.Model.HRM.PolicyManagment.hrmPolicyHeader> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.PolicyManagment.hrmPolicyHeader> lst = new List<Architecture.Model.HRM.PolicyManagment.hrmPolicyHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.HRM.PolicyManagment.hrmPolicyHeader>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.HRM.PolicyManagment.hrmPolicyHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@PolicyId", item.PolicyHeaderId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadById"));
					item.HrmPolicyDetailList = GenericProvider.Get<Architecture.Model.HRM.PolicyManagment.hrmPolicyDetail>(sqlcon, "Sp_hrmPolicyDetail_GetAllMethod", lstPrm);
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
