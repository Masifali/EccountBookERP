using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.ProfileManagement;
using Architecture.Model.HRM.ProfileManagement;

namespace Architecture.BLL.HRM.ProfileManagement;

public class genDesignation
{
	public static int Save(Architecture.Model.HRM.ProfileManagement.genDesignation obj)
	{
		try
		{
			obj.ActionTypeId = 1;
			if (obj.DesignationId == 0)
			{
				obj.CreatedOn = DateTime.Now;
				return new Architecture.DAL.HRM.ProfileManagement.genDesignation().SetData(obj, "Sp_genDesignation_Insert");
			}
			obj.ActionTypeId = 2;
			obj.AlteredOn = DateTime.Now;
			return new Architecture.DAL.HRM.ProfileManagement.genDesignation().SetData(obj, "Sp_genDesignation_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.ProfileManagement.genDesignation GetByID(int Id)
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
			new List<Architecture.Model.HRM.ProfileManagement.genDesignation>();
			return new Architecture.DAL.HRM.ProfileManagement.genDesignation().GetAll("Sp_genDesignation_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.ProfileManagement.genDesignation obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.ProfileManagement.genDesignation>("Sp_genDesignation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
