using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class ScreenDefinition
{
	public static int save(Architecture.Model.ScreenDefinition obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.ScreenDefinition().SetData(obj, "Sp_ScreenDefinition_Insert");
			}
			return new Architecture.DAL.ScreenDefinition().SetData(obj, "Sp_ScreenDefinition_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ScreenSortingAndAliasUpdate(Architecture.Model.ScreenDefinition obj)
	{
		try
		{
			foreach (Architecture.Model.ScreenDefinition item in obj.ScreenDefinitionslist)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter("@ScreenAlias", item.ScreenAlias));
				p.Add(new SqlParameter("@SortNo", item.SortNo));
				p.Add(new SqlParameter("@Id", item.Id));
				p.Add(new SqlParameter("@IsActive", item.IsActive));
				p.Add(new SqlParameter("@Activity", "ScreenSortingAndAliasUpdate"));
				GenericProvider.GetDataTableProc<Architecture.Model.ScreenDefinition>("Sp_ScreenDefinition_GetAllMethod", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@Activity", "ReadAll"));
			return GenericProvider.GetDataTableProc<Architecture.Model.ScreenDefinition>("Sp_ScreenDefinition_GetAllMethod", p);
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
			p.Add(new SqlParameter("@Activity", "FormHistory"));
			return GenericProvider.GetDataTableProc<Architecture.Model.ScreenDefinition>("Sp_ScreenDefinition_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSceensWithSpecialRights()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@Activity", "GetSceensWithSpecialRights"));
			return GenericProvider.GetDataTableProc<Architecture.Model.ScreenDefinition>("Sp_ScreenDefinition_GetAllMethod", p);
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
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ScreenAlias",
				Value = ScreenName
			});
			p.Add(new SqlParameter("@Activity", "GetScreenIdByName"));
			DataTable dt = null;
			dt = GenericProvider.GetDataTableProc<Architecture.Model.ScreenDefinition>("Sp_ScreenDefinition_GetAllMethod", p);
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

	public static Architecture.Model.ScreenDefinition GetById(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			new List<Architecture.Model.ScreenDefinition>();
			return new Architecture.DAL.ScreenDefinition().GetDate("Sp_ScreenDefinition_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAllModules()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.tblUserRights>("Sp_AppModules_ReadAll", p);
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

	public static DataTable ERPVideosAll(ReportsParameters obj)
	{
		try
		{
			new DataTable();
			List<SqlParameter> p = new List<SqlParameter>();
			if (obj.ScreenId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenId",
					Value = obj.ScreenId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.ScreenDefinition>("[dbo].[USP_GetAllERPVideos]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
