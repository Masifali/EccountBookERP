using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.PolicyManagment;
using Architecture.Model.HRM.PolicyManagment;

namespace Architecture.BLL.HRM.PolicyManagment;

public class hrmPolicyHeader
{
	public static int Save(Architecture.Model.HRM.PolicyManagment.hrmPolicyHeader obj)
	{
		try
		{
			if (obj.PolicyHeaderId == 0)
			{
				return new Architecture.DAL.HRM.PolicyManagment.hrmPolicyHeader().SetData(obj, "Sp_hrmPolicyHeader_Insert");
			}
			return new Architecture.DAL.HRM.PolicyManagment.hrmPolicyHeader().SetData(obj, "Sp_hrmPolicyHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.PolicyManagment.hrmPolicyHeader GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@BenefitId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.PolicyManagment.hrmPolicyHeader>();
			return new Architecture.DAL.HRM.PolicyManagment.hrmPolicyHeader().GetAll("Sp_hrmPolicyHeader_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.PolicyManagment.hrmPolicyHeader obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.PolicyManagment.hrmPolicyHeader>("Sp_hrmPolicyHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
