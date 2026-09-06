using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Templates;
using Architecture.Model.Templates;

namespace Architecture.BLL.Templates;

public class ScreensAllocateToCompany
{
	public static int Save(Architecture.Model.Templates.ScreensAllocateToCompany obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Templates.ScreensAllocateToCompany().SetData(obj, "[dbo].[USP_ScreensAllocateToCompany_Insert]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ScreensAllocateToCompany_AllocatedData(int CompanyId, int ModuleId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ModuleId",
				Value = ModuleId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Templates.ScreensAllocateToCompany>("USP_ScreensAllocateToCompany_AllocatedData", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ScreensAllocateToCompany_UnAllocatedData(int CompanyId, int ModuleId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ModuleId",
				Value = ModuleId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Templates.ScreensAllocateToCompany>("USP_ScreensAllocateToCompany_UnAllocatedData", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllCompanyType()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAllCompanyType"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Templates.ScreensAllocateToCompany>("[USP_ScreensAllocateToCompanyType_GetAll]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
