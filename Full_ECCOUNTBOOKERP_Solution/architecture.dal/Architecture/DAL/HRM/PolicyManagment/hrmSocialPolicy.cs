using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.PolicyManagment;

namespace Architecture.DAL.HRM.PolicyManagment;

public class hrmSocialPolicy
{
	public int SetData(Architecture.Model.HRM.PolicyManagment.hrmSocialPolicy obj, string ProcName)
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
				obj.SocialPolicyId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.SocialPolicyId;
			}
			foreach (hrmSocialPolicySlab item in obj.SocialSecurityDetail)
			{
				item.SocialPolicyId = obj.SocialPolicyId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_hrmSocialPolicySlab_Insert"));
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

	public List<Architecture.Model.HRM.PolicyManagment.hrmSocialPolicy> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.PolicyManagment.hrmSocialPolicy> lst = new List<Architecture.Model.HRM.PolicyManagment.hrmSocialPolicy>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.HRM.PolicyManagment.hrmSocialPolicy>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.HRM.PolicyManagment.hrmSocialPolicy item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@SocialPolicySlabId", item.SocialPolicyId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadById"));
					item.SocialSecurityDetail = GenericProvider.Get<hrmSocialPolicySlab>(sqlcon, "Sp_hrmSocialPolicySlab_GetAllMethod", lstPrm);
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
