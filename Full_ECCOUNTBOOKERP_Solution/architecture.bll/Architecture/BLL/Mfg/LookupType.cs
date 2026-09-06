using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Mfg;
using Architecture.Model.Mfg;

namespace Architecture.BLL.Mfg;

public class LookupType
{
	public static int Save(Architecture.Model.Mfg.LookupType obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Mfg.LookupType().SetData(obj, "[Mfg].[USP_LookupType_Insert]");
			}
			return new Architecture.DAL.Mfg.LookupType().SetData(obj, "[Mfg].[USP_LookupType_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Mfg.LookupType obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Mfg.LookupType>("[Mfg].[USP_LookupType_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Mfg.LookupType> FormHistoryWithList(Architecture.Model.Mfg.LookupType obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			new List<Architecture.Model.Mfg.LookupType>();
			return new Architecture.DAL.Mfg.LookupType().GetAll("[Mfg].[USP_LookupType_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Mfg.LookupType ReadById(int Id)
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
			new List<Architecture.Model.Mfg.LookupType>();
			return new Architecture.DAL.Mfg.LookupType().GetAll("[Mfg].[USP_LookupType_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
