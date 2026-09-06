using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class AppModules
{
	public static int save(Architecture.Model.AppModules obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.AppModules().SetData(obj, "Proc_AppModules_Insert");
			}
			return new Architecture.DAL.AppModules().SetData(obj, "Proc_AppModules_Update");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static void MultiRecordUpdate(Architecture.Model.AppModules obj)
	{
		try
		{
			new Architecture.DAL.AppModules().SetDataMulti(obj, "Proc_AppModules_Update");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable ReadAll()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.AppModules>("Sp_AppModules_ReadAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.AppModules GetById(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			new List<Architecture.Model.AppModules>();
			return new Architecture.DAL.AppModules().GetAll("Proc_AppModules_ReadById", p)[0];
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable ReadAllApps()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.AppModules>("USP_App_ReadAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
