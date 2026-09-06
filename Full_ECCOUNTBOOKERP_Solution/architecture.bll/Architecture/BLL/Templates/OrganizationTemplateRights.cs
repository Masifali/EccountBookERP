using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Templates;
using Architecture.Model.Templates;

namespace Architecture.BLL.Templates;

public class OrganizationTemplateRights
{
	public static int Save(Architecture.Model.Templates.OrganizationTemplateRights obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Templates.OrganizationTemplateRights().SetData(obj, "[dbo].[Sp_OrganizationTemplateRights_Insert]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int OrganizationTemplateId, string ScreenIds)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationTemplateId",
				Value = OrganizationTemplateId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ScreenIds",
				Value = ScreenIds
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "DeleteByOrganizationTemplateId"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.Templates.OrganizationTemplateRights>("[dbo].[Sp_OrganizationTemplateRights_GetAll]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllocatedAndUnAllocatedScreens(int TemplateId, int ActionId, int AppModuleId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationTemplateId",
				Value = TemplateId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = ActionId
			});
			if (AppModuleId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AppModuleId",
					Value = AppModuleId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Templates.OrganizationTemplateRights>("[dbo].[USP_ScreenAllocatedOrUnAllocatedInOrganizationTemplateRights]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
