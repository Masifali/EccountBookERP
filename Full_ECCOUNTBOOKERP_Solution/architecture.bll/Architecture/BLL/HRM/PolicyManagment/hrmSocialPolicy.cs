using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.PolicyManagment;
using Architecture.Model.HRM.PolicyManagment;

namespace Architecture.BLL.HRM.PolicyManagment;

public class hrmSocialPolicy
{
	public static int Save(Architecture.Model.HRM.PolicyManagment.hrmSocialPolicy obj)
	{
		try
		{
			if (obj.SocialPolicyId == 0)
			{
				return new Architecture.DAL.HRM.PolicyManagment.hrmSocialPolicy().SetData(obj, "Sp_hrmSocialPolicy_Insert");
			}
			return new Architecture.DAL.HRM.PolicyManagment.hrmSocialPolicy().SetData(obj, "Sp_hrmSocialPolicy_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.PolicyManagment.hrmSocialPolicy GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@SocialPolicyId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.PolicyManagment.hrmSocialPolicy>();
			return new Architecture.DAL.HRM.PolicyManagment.hrmSocialPolicy().GetAll("Sp_hrmSocialPolicy_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.PolicyManagment.hrmSocialPolicy obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.PolicyManagment.hrmSocialPolicy>("Sp_hrmSocialPolicy_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
