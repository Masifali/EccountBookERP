using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Templates;
using Architecture.Model.Templates;

namespace Architecture.BLL.Templates;

public class InActiveScreenOfTemplatesModule
{
	public static long Save(Architecture.Model.Templates.InActiveScreenOfTemplatesModule obj)
	{
		try
		{
			return new Architecture.DAL.Templates.InActiveScreenOfTemplatesModule().SetData(obj, "[dbo].[USP_InActiveScreenOfTemplatesModule_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InActiveScreenOfTemplatesModule_AllocatedData(int TemplateId, int ModuleId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
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
				ParameterName = "@TemplateId",
				Value = TemplateId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Templates.InActiveScreenOfTemplatesModule>("[dbo].[USP_InActiveScreenOfTemplatesModule_AllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InActiveScreenOfTemplatesModule_UnAllocatedData(int TemplateId, int ModuleId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
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
				ParameterName = "@TemplateId",
				Value = TemplateId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Templates.InActiveScreenOfTemplatesModule>("[dbo].[USP_InActiveScreenOfTemplatesModule_UnAllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
