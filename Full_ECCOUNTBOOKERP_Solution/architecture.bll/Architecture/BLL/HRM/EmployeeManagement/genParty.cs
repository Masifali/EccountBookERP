using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.EmployeeManagement;
using Architecture.Model.HRM.EmployeeManagement;

namespace Architecture.BLL.HRM.EmployeeManagement;

public class genParty
{
	public static int Save(Architecture.Model.HRM.EmployeeManagement.genParty obj)
	{
		try
		{
			obj.ActionTypeId = 1;
			if (obj.PartyId == 0)
			{
				obj.CreatedOn = DateTime.Now;
				return new Architecture.DAL.HRM.EmployeeManagement.genParty().SetData(obj, "Sp_genParty_Insert");
			}
			obj.ActionTypeId = 2;
			obj.AlteredOn = DateTime.Now;
			return new Architecture.DAL.HRM.EmployeeManagement.genParty().SetData(obj, "Sp_genParty_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.EmployeeManagement.genParty GetByID(int Id)
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
			new List<Architecture.Model.HRM.EmployeeManagement.genParty>();
			return new Architecture.DAL.HRM.EmployeeManagement.genParty().GetAll("Sp_genParty_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.EmployeeManagement.genParty obj)
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
				Value = obj.OrginizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.genParty>("Sp_genParty_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
