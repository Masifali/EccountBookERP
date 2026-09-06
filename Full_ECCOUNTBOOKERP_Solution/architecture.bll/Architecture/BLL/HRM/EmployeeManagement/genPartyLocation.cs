using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.EmployeeManagement;
using Architecture.Model.HRM.EmployeeManagement;

namespace Architecture.BLL.HRM.EmployeeManagement;

public class genPartyLocation
{
	public static int Save(Architecture.Model.HRM.EmployeeManagement.genPartyLocation obj)
	{
		try
		{
			if (obj.PartyLocationId == 0)
			{
				return new Architecture.DAL.HRM.EmployeeManagement.genPartyLocation().SetData(obj, "Sp_genPartyLocation_Insert");
			}
			return new Architecture.DAL.HRM.EmployeeManagement.genPartyLocation().SetData(obj, "Sp_genPartyLocation_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.EmployeeManagement.genPartyLocation GetByID(int Id)
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
			new List<Architecture.Model.HRM.EmployeeManagement.genPartyLocation>();
			return new Architecture.DAL.HRM.EmployeeManagement.genPartyLocation().GetAll("Sp_genPartyLocation_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.EmployeeManagement.genPartyLocation obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.genPartyLocation>("Sp_genPartyLocation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
