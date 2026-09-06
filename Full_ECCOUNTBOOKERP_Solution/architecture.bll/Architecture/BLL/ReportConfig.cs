using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class ReportConfig
{
	public static int Save(Architecture.Model.ReportConfig obj)
	{
		try
		{
			if (obj.ReportConfigId == 0)
			{
				return new Architecture.DAL.ReportConfig().SetData(obj, "Sp_ReportConfig_Insert");
			}
			return new Architecture.DAL.ReportConfig().SetData(obj, "Sp_ReportConfig_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.ReportConfig obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ReportConfig>("Sp_ReportConfig_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.ReportConfig GetByID(int Id)
	{
		try
		{
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
			new List<Architecture.Model.ReportConfig>();
			return new Architecture.DAL.ReportConfig().GetDate("Sp_ReportConfig_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
