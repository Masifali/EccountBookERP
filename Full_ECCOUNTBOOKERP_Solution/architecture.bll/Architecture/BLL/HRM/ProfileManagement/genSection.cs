using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.ProfileManagement;
using Architecture.Model.HRM.ProfileManagement;

namespace Architecture.BLL.HRM.ProfileManagement;

public class genSection
{
	public static int Save(Architecture.Model.HRM.ProfileManagement.genSection obj)
	{
		try
		{
			obj.ActionTypeId = 1;
			if (obj.SectionId == 0)
			{
				obj.CreatedOn = DateTime.Now;
				return new Architecture.DAL.HRM.ProfileManagement.genSection().SetData(obj, "Sp_genSection_Insert");
			}
			obj.ActionTypeId = 2;
			obj.AlteredOn = DateTime.Now;
			return new Architecture.DAL.HRM.ProfileManagement.genSection().SetData(obj, "Sp_genSection_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.ProfileManagement.genSection GetByID(int Id)
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
			new List<Architecture.Model.HRM.ProfileManagement.genSection>();
			return new Architecture.DAL.HRM.ProfileManagement.genSection().GetAll("Sp_genSection_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.ProfileManagement.genSection obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.ProfileManagement.genSection>("Sp_genSection_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
