using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.EmployeeManagement;
using Architecture.Model.HRM.EmployeeManagement;

namespace Architecture.BLL.HRM.EmployeeManagement;

public class hrmEmployeeReference
{
	public static int Save(Architecture.Model.HRM.EmployeeManagement.hrmEmployeeReference obj)
	{
		try
		{
			obj.CreatedOn = DateTime.Now;
			obj.AlteredOn = DateTime.Now;
			obj.EmployeeReferenceLineId = 1;
			if (obj.EmployeeReferenceId == 0)
			{
				obj.ActionTypeId = 1;
				return new Architecture.DAL.HRM.EmployeeManagement.hrmEmployeeReference().SetData(obj, "Sp_hrmEmployeeReference_Insert");
			}
			obj.ActionTypeId = 2;
			return new Architecture.DAL.HRM.EmployeeManagement.hrmEmployeeReference().SetData(obj, "Sp_hrmEmployeeReference_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.EmployeeManagement.hrmEmployeeReference GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeReferenceId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeReference>();
			return new Architecture.DAL.HRM.EmployeeManagement.hrmEmployeeReference().GetAll("Sp_hrmEmployeeReference_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.EmployeeManagement.hrmEmployeeReference obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeReference>("Sp_hrmEmployeeReference_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
