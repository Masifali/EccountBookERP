using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.EmployeeManagement;
using Architecture.Model.HRM.EmployeeManagement;

namespace Architecture.BLL.HRM.EmployeeManagement;

public class hrmEmployeeExperience
{
	public static int Save(Architecture.Model.HRM.EmployeeManagement.hrmEmployeeExperience obj)
	{
		try
		{
			obj.ActionTypeId = 1;
			obj.AlteredOn = DateTime.Now;
			obj.CreatedOn = DateTime.Now;
			obj.EmployeeExperienceLineId = 1;
			if (obj.EmployeeExperienceId == 0)
			{
				return new Architecture.DAL.HRM.EmployeeManagement.hrmEmployeeExperience().SetData(obj, "Sp_hrmEmployeeExperience_Insert");
			}
			return new Architecture.DAL.HRM.EmployeeManagement.hrmEmployeeExperience().SetData(obj, "Sp_hrmEmployeeExperience_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.EmployeeManagement.hrmEmployeeExperience GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeExperienceId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeExperience>();
			return new Architecture.DAL.HRM.EmployeeManagement.hrmEmployeeExperience().GetAll("Sp_hrmEmployeeExperience_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.EmployeeManagement.hrmEmployeeExperience obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeExperience>("Sp_hrmEmployeeExperience_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
