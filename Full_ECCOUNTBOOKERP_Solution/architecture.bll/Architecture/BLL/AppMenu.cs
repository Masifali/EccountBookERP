using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class AppMenu
{
	public static int save(Architecture.Model.AppMenu obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.AppMenu().SetData(obj, "Usp_AppMenu_Insert");
			}
			return new Architecture.DAL.AppMenu().SetData(obj, "Usp_AppMenu_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetScreenIdByName(string ScreenName)
	{
		int Id = 0;
		try
		{
			DataTable dt = null;
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ScreenAlias",
				Value = ScreenName
			});
			p.Add(new SqlParameter("@Activity", "GetScreenIdByName"));
			dt = GenericProvider.GetDataTableProc<Architecture.Model.ScreenDefinition>("Usp_AppMenu_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				Id = Conversion.ToInt(dt.Rows[0][0]);
			}
			return Id;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.AppMenu GetById(int Id)
	{
		try
		{
			List<Architecture.Model.AppMenu> lst = new List<Architecture.Model.AppMenu>();
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
			lst = new Architecture.DAL.AppMenu().GetDate("Usp_AppMenu_GetAllMethod", p);
			return (lst == null || lst.Count <= 0) ? null : lst?[0];
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable FormHistory()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@Activity", "FormHistory"));
			return GenericProvider.GetDataTableProc<Architecture.Model.ScreenDefinition>("Usp_AppMenu_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ScreenSortingAndAliasUpdate(Architecture.Model.AppMenu obj)
	{
		try
		{
			foreach (Architecture.Model.AppMenu item in obj.AppMenuList)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter("@ScreenAlias", item.ScreenAlias));
				p.Add(new SqlParameter("@SortNo", item.SortNo));
				p.Add(new SqlParameter("@Id", item.Id));
				p.Add(new SqlParameter("@Activity", "ScreenSortingAndAliasUpdate"));
				GenericProvider.GetDataTableProc<Architecture.Model.ScreenDefinition>("Usp_AppMenu_GetAllMethod", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetModulesForAppMenu()
	{
		try
		{
			new DataTable();
			List<SqlParameter> p = new List<SqlParameter>();
			return GenericProvider.GetDataTableProc<Architecture.Model.tblUserRights>("usp_getModulesForAppMenu", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetScreensWithSpecialRights()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@Activity", "GetScreensWithSpecialRights"));
			return GenericProvider.GetDataTableProc<Architecture.Model.ScreenDefinition>("Usp_AppMenu_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetConfigrationsScreenWise(ReportsParameters obj)
	{
		try
		{
			new DataTable();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			if (obj.ScreenId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenId",
					Value = obj.ScreenId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.ScreenDefinition>("[dbo].[USP_GetConfigrationsScreenWise]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
