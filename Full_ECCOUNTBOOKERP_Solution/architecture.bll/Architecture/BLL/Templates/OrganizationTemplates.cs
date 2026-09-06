using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Templates;
using Architecture.Model;
using Architecture.Model.Templates;

namespace Architecture.BLL.Templates;

public class OrganizationTemplates
{
	public static int Save(Architecture.Model.Templates.OrganizationTemplates obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Templates.OrganizationTemplates().SetData(obj, "Sp_OrganizationTemplates_Insert");
			}
			return new Architecture.DAL.Templates.OrganizationTemplates().SetData(obj, "Sp_OrganizationTemplates_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Templates.OrganizationTemplates> GetAll(int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			new List<Architecture.Model.Templates.OrganizationTemplates>();
			return new Architecture.DAL.Templates.OrganizationTemplates().GetAll("Sp_OrganizationTemplates_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Bank>("Sp_OrganizationTemplates_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllInTemplateRights()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAllInTemplateRights"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Bank>("Sp_OrganizationTemplates_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Templates.OrganizationTemplates GetByID(int Id)
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
			new List<Architecture.Model.Templates.OrganizationTemplates>();
			return new Architecture.DAL.Templates.OrganizationTemplates().GetAll("Sp_OrganizationTemplates_GetAll", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
