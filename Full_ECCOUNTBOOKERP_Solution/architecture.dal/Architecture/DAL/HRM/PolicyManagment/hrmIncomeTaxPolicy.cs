using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.PolicyManagment;

namespace Architecture.DAL.HRM.PolicyManagment;

public class hrmIncomeTaxPolicy
{
	public int SetData(Architecture.Model.HRM.PolicyManagment.hrmIncomeTaxPolicy obj, string ProcName)
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
				obj.IncomeTaxPolicyId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.IncomeTaxPolicyId;
			}
			foreach (hrmIncomeTaxPolicySlab item2 in obj.IncomeTaxDetail)
			{
				item2.IncomeTaxPolicyId = obj.IncomeTaxPolicyId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_hrmIncomeTaxPolicySlab_Insert"));
			}
			foreach (hrmIncomeTaxPolicyEmployee item in obj.TaxPolicyEmployeesLists)
			{
				item.hrmIncomeTaxPolicyId = obj.IncomeTaxPolicyId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_hrmIncomeTaxPolicyEmployee_Insert"));
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

	public List<Architecture.Model.HRM.PolicyManagment.hrmIncomeTaxPolicy> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.PolicyManagment.hrmIncomeTaxPolicy> lst = new List<Architecture.Model.HRM.PolicyManagment.hrmIncomeTaxPolicy>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.HRM.PolicyManagment.hrmIncomeTaxPolicy>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.HRM.PolicyManagment.hrmIncomeTaxPolicy item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@IncomeTaxPolicySlabId", item2.IncomeTaxPolicyId));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadById"));
						item2.IncomeTaxDetail = GenericProvider.Get<hrmIncomeTaxPolicySlab>(sqlConnection2, "Sp_hrmIncomeTaxPolicySlab_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.HRM.PolicyManagment.hrmIncomeTaxPolicy item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@IncomeTaxPolicyId", item.IncomeTaxPolicyId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadIncomeTaxPolicyEmployeeByHeaderId"));
					item.TaxPolicyEmployeesLists = GenericProvider.Get<hrmIncomeTaxPolicyEmployee>(sqlcon, "Sp_hrmIncomeTaxPolicy_GetAllMethod", lstPrm);
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
