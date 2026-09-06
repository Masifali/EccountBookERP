using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.EmployeeManagement;
using Architecture.Model.HRM.EmployeeManagement;

namespace Architecture.BLL.HRM.EmployeeManagement;

public class genShiftLocation
{
	public static int Save(Architecture.Model.HRM.EmployeeManagement.genShiftLocation obj)
	{
		try
		{
			if (obj.ShiftLocationId == 0)
			{
				obj.CreatedOn = DateTime.Now;
				obj.ActionTypeId = 1;
				return new Architecture.DAL.HRM.EmployeeManagement.genShiftLocation().SetData(obj, "Sp_genShiftLocation_Insert");
			}
			obj.ActionTypeId = 2;
			obj.AlteredOn = DateTime.Now;
			return new Architecture.DAL.HRM.EmployeeManagement.genShiftLocation().SetData(obj, "Sp_genShiftLocation_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.EmployeeManagement.genShiftLocation GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ShiftLocationId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.EmployeeManagement.genShiftLocation>();
			return new Architecture.DAL.HRM.EmployeeManagement.genShiftLocation().GetAll("Sp_genShiftLocation_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.EmployeeManagement.genShiftLocation obj)
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
			if (obj.ShiftId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShiftId",
					Value = obj.ShiftId
				});
			}
			if (obj.IsActive)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsActive",
					Value = obj.IsActive
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.genShiftLocation>("Sp_genShiftLocation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
