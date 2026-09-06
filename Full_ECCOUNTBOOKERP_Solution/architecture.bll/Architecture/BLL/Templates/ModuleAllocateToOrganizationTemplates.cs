using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Templates;
using Architecture.Model.Templates;

namespace Architecture.BLL.Templates;

public class ModuleAllocateToOrganizationTemplates
{
	public static long Save(Architecture.Model.Templates.ModuleAllocateToOrganizationTemplates obj)
	{
		try
		{
			return new Architecture.DAL.Templates.ModuleAllocateToOrganizationTemplates().SetData(obj, "[dbo].[USP_ModuleAllocateToOrganizationTemplates_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ModuleAllocateToOrganizationTemplates_AllocatedData(int TemplateId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@TemplateId",
				Value = TemplateId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Templates.ModuleAllocateToOrganizationTemplates>("[dbo].[USP_ModuleAllocateToOrganizationTemplates_AllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ModuleAllocateToOrganizationTemplates_UnAllocatedData(int TemplateId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@TemplateId",
				Value = TemplateId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Templates.ModuleAllocateToOrganizationTemplates>("[dbo].[USP_ModuleAllocateToOrganizationTemplates_UnAllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
