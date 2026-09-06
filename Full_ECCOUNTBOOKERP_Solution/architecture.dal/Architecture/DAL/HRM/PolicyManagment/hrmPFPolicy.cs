using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.PolicyManagment;

namespace Architecture.DAL.HRM.PolicyManagment;

public class hrmPFPolicy
{
	public int SetData(Architecture.Model.HRM.PolicyManagment.hrmPFPolicy obj, string ProcName)
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
				obj.PFPolicyId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.PFPolicyId;
			}
			foreach (hrmPFPolicySlab item2 in obj.PolicyDetail)
			{
				item2.PFPolicyId = obj.PFPolicyId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_hrmPFPolicySlab_Insert"));
			}
			foreach (hrmPFPolicyEmployee item in obj.PFPolicyEmployeesList)
			{
				item.PFPolicyId = obj.PFPolicyId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_hrmPFPolicyEmployee_Insert"));
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

	public List<Architecture.Model.HRM.PolicyManagment.hrmPFPolicy> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.PolicyManagment.hrmPFPolicy> lst = new List<Architecture.Model.HRM.PolicyManagment.hrmPFPolicy>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.HRM.PolicyManagment.hrmPFPolicy>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.HRM.PolicyManagment.hrmPFPolicy item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@PFPolicySlabId", item2.PFPolicyId));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadById"));
						item2.PolicyDetail = GenericProvider.Get<hrmPFPolicySlab>(sqlConnection2, "Sp_hrmPFPolicySlab_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.HRM.PolicyManagment.hrmPFPolicy item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@PFPolicyId", item.PFPolicyId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadPFPPolicyEmployeeByPfPolicyId"));
					item.PFPolicyEmployeesList = GenericProvider.Get<hrmPFPolicyEmployee>(sqlcon, "Sp_hrmPFPolicy_GetAllMethod", lstPrm);
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
