using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Themes_ERP;
using Architecture.Model.Themes_ERP;

namespace Architecture.BLL.Themes_ERP;

public class ThemeSettings
{
	public static int Save(Architecture.Model.Themes_ERP.ThemeSettings obj)
	{
		try
		{
			return new Architecture.DAL.Themes_ERP.ThemeSettings().SetData(obj, "[dbo].[USP_ThemeSettings_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetThemeSettingsByScreen(int OrganizationId, int CompanyId, int ScreenId = 0, string ScreenName = null)
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
			if (ScreenId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenId",
					Value = ScreenId
				});
			}
			if (!string.IsNullOrEmpty(ScreenName))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenName",
					Value = ScreenName
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetThemeSettingsByScreen"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Themes_ERP.ThemeSettings>("[dbo].[usp_ThemeSettings_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetScreensForTheme(int OrganizationId, int CompanyId)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Themes_ERP.ThemeSettings>("[dbo].[USP_GetScreensForTheme]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Themes_ERP.ThemeSettings> GetThemeSettingsByScreenList(int OrganizationId, int CompanyId, int ScreenId = 0, string ScreenName = null)
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
			if (ScreenId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenId",
					Value = ScreenId
				});
			}
			if (!string.IsNullOrEmpty(ScreenName))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenName",
					Value = ScreenName
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetThemeSettingsByScreen"
			});
			return Conversion.ConvertDataTableToList<Architecture.Model.Themes_ERP.ThemeSettings>(GenericProvider.GetDataTableProc<Architecture.Model.Themes_ERP.ThemeSettings>("[dbo].[usp_ThemeSettings_GetAllMethod]", p));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
