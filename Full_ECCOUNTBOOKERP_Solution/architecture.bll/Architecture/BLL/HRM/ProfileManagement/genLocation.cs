using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.ProfileManagement;
using Architecture.Model.HRM.ProfileManagement;

namespace Architecture.BLL.HRM.ProfileManagement;

public class genLocation
{
	public static int Save(Architecture.Model.HRM.ProfileManagement.genLocation obj)
	{
		try
		{
			obj.ActionTypeId = 1;
			obj.AlteredOn = DateTime.Now;
			obj.CreatedOn = DateTime.Now;
			if (obj.LocationId == 0)
			{
				return new Architecture.DAL.HRM.ProfileManagement.genLocation().SetData(obj, "Sp_genLocation_Insert");
			}
			return new Architecture.DAL.HRM.ProfileManagement.genLocation().SetData(obj, "Sp_genLocation_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.ProfileManagement.genLocation GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@LocationId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.ProfileManagement.genLocation>();
			return new Architecture.DAL.HRM.ProfileManagement.genLocation().GetAll("Sp_genLocation_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.ProfileManagement.genLocation obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.ProfileManagement.genLocation>("Sp_genLocation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
