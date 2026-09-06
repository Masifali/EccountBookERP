using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.EmployeeManagement;
using Architecture.Model.HRM.EmployeeManagement;

namespace Architecture.BLL.HRM.EmployeeManagement;

public class genBranch
{
	public static int Save(Architecture.Model.HRM.EmployeeManagement.genBranch obj)
	{
		try
		{
			obj.ActionTypeId = 1;
			if (obj.BranchId == 0)
			{
				obj.CreatedOn = DateTime.Now;
				return new Architecture.DAL.HRM.EmployeeManagement.genBranch().SetData(obj, "Sp_genBranch_Insert");
			}
			obj.ActionTypeId = 2;
			obj.AlteredOn = DateTime.Now;
			return new Architecture.DAL.HRM.EmployeeManagement.genBranch().SetData(obj, "Sp_genBranch_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.EmployeeManagement.genBranch GetByID(int Id)
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
			new List<Architecture.Model.HRM.EmployeeManagement.genBranch>();
			return new Architecture.DAL.HRM.EmployeeManagement.genBranch().GetAll("Sp_genBranch_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.EmployeeManagement.genBranch obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.genBranch>("Sp_genBranch_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
