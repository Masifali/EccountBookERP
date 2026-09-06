using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.OrganizationRegistration;
using Architecture.Model;
using Architecture.Model.OrganizationRegistration;

namespace Architecture.BLL.OrganizationRegistration;

public class OrganizationTemplateRights
{
	public static int Save(Architecture.Model.OrganizationRegistration.OrganizationTemplateRights obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.OrganizationRegistration.OrganizationTemplateRights().SetData(obj, "Sp_OrganizationTemplateRights_Insert");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByOrganizationTemplateId(int OrganizationTemplateId, int ModuleId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationTemplateId",
				Value = OrganizationTemplateId
			});
			if (ModuleId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModuleId",
					Value = ModuleId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByOrganizationTemplateId"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.CompanyRights>("Sp_OrganizationTemplateRights_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadScreensNotInOrganizationTemplateRights(int OrganizationTemplateId, int ModuleId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationTemplateId",
				Value = OrganizationTemplateId
			});
			if (ModuleId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModuleId",
					Value = ModuleId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadScreensNotInOrganizationTemplateRights"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.OrganizationRegistration.OrganizationTemplateRights>("Sp_OrganizationTemplateRights_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByOrganizationTemplateId(int OrganizationTemplateId, string ScreenIds)
	{
		try
		{
			new Architecture.DAL.OrganizationRegistration.OrganizationTemplateRights().DeleteByOrganizationTemplateId(OrganizationTemplateId, ScreenIds);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
