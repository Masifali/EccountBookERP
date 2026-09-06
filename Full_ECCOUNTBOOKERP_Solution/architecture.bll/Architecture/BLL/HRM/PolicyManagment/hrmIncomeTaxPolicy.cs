using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.PolicyManagment;
using Architecture.Model.HRM.PolicyManagment;

namespace Architecture.BLL.HRM.PolicyManagment;

public class hrmIncomeTaxPolicy
{
	public static int Save(Architecture.Model.HRM.PolicyManagment.hrmIncomeTaxPolicy obj)
	{
		try
		{
			if (obj.IncomeTaxPolicyId == 0)
			{
				return new Architecture.DAL.HRM.PolicyManagment.hrmIncomeTaxPolicy().SetData(obj, "Sp_hrmIncomeTaxPolicy_Insert");
			}
			return new Architecture.DAL.HRM.PolicyManagment.hrmIncomeTaxPolicy().SetData(obj, "Sp_hrmIncomeTaxPolicy_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.PolicyManagment.hrmIncomeTaxPolicy GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@IncomeTaxPolicyId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByID"
			});
			new List<Architecture.Model.HRM.PolicyManagment.hrmIncomeTaxPolicy>();
			return new Architecture.DAL.HRM.PolicyManagment.hrmIncomeTaxPolicy().GetAll("Sp_hrmIncomeTaxPolicy_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.PolicyManagment.hrmIncomeTaxPolicy obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.PolicyManagment.hrmIncomeTaxPolicy>("Sp_hrmIncomeTaxPolicy_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
