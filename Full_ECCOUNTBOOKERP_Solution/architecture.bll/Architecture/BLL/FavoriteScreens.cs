using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class FavoriteScreens
{
	public static int save(Architecture.Model.FavoriteScreens obj)
	{
		try
		{
			return new Architecture.DAL.FavoriteScreens().SetData(obj, "Sp_FavoriteScreens_Insert");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable ReadByUserId(int OrganizationId, int CompanyId, int UserId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", OrganizationId));
			p.Add(new SqlParameter("@CompanyId", CompanyId));
			p.Add(new SqlParameter("@UserId", UserId));
			p.Add(new SqlParameter("@Activity", "ReadByUserId"));
			return GenericProvider.GetDataTableProc<Architecture.Model.FavoriteScreens>("Sp_FavoriteScreens_ReadAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByScreenNameAndUserId(string ScreenName, int UserId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@ScreenName", ScreenName));
			p.Add(new SqlParameter("@UserId", UserId));
			p.Add(new SqlParameter("@Activity", "DeleteByScreenNameAndUserId"));
			GenericProvider.GetDataTableProc<Architecture.Model.FavoriteScreens>("Sp_FavoriteScreens_ReadAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
