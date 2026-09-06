using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.EmployeeManagement;
using Architecture.Model.HRM.EmployeeManagement;

namespace Architecture.BLL.HRM.EmployeeManagement;

public class GenEmployeeEducation
{
	public static int Save(Architecture.Model.HRM.EmployeeManagement.GenEmployeeEducation obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				obj.EntryDate = DateTime.Now;
				return new Architecture.DAL.HRM.EmployeeManagement.GenEmployeeEducation().SetData(obj, "Sp_GenEmployeeEducation_Insert");
			}
			obj.ModifyDate = DateTime.Now;
			return new Architecture.DAL.HRM.EmployeeManagement.GenEmployeeEducation().SetData(obj, "Sp_GenEmployeeEducation_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.EmployeeManagement.GenEmployeeEducation GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.EmployeeManagement.GenEmployeeEducation>();
			return new Architecture.DAL.HRM.EmployeeManagement.GenEmployeeEducation().GetAll("Sp_GenEmployeeEducation_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.EmployeeManagement.GenEmployeeEducation obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.GenEmployeeEducation>("Sp_GenEmployeeEducation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
