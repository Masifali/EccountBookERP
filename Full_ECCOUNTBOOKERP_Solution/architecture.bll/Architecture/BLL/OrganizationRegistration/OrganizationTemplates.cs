using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.OrganizationRegistration;
using Architecture.Model.OrganizationRegistration;

namespace Architecture.BLL.OrganizationRegistration;

public class OrganizationTemplates
{
	public static int Save(Architecture.Model.OrganizationRegistration.OrganizationTemplates obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.OrganizationRegistration.OrganizationTemplates().SetData(obj, "Sp_OrganizationTemplates_Insert");
			}
			return new Architecture.DAL.OrganizationRegistration.OrganizationTemplates().SetData(obj, "Sp_OrganizationTemplates_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.OrganizationRegistration.OrganizationTemplates GetByID(int Id)
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
			new List<Architecture.Model.OrganizationRegistration.OrganizationTemplates>();
			return new Architecture.DAL.OrganizationRegistration.OrganizationTemplates().GetData("Sp_OrganizationTemplates_GetAll", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.OrganizationRegistration.OrganizationTemplates> GetAll(Architecture.Model.OrganizationRegistration.OrganizationTemplates obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAll"
			});
			return new Architecture.DAL.OrganizationRegistration.OrganizationTemplates().GetData("Sp_OrganizationTemplates_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
