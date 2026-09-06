using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.EmployeeManagement;
using Architecture.Model.HRM.EmployeeManagement;

namespace Architecture.BLL.HRM.EmployeeManagement;

public class genShift
{
	public static int Save(Architecture.Model.HRM.EmployeeManagement.genShift obj)
	{
		try
		{
			obj.ActionTypeId = 1;
			if (obj.ShiftId == 0)
			{
				obj.CreatedOn = DateTime.Now;
				return new Architecture.DAL.HRM.EmployeeManagement.genShift().SetData(obj, "Sp_genShift_Insert");
			}
			obj.ActionTypeId = 2;
			obj.AlteredOn = DateTime.Now;
			return new Architecture.DAL.HRM.EmployeeManagement.genShift().SetData(obj, "Sp_genShift_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.EmployeeManagement.genShift GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ShiftId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.EmployeeManagement.genShift>();
			return new Architecture.DAL.HRM.EmployeeManagement.genShift().GetAll("Sp_genShift_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.EmployeeManagement.genShift obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.genShift>("Sp_genShift_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
