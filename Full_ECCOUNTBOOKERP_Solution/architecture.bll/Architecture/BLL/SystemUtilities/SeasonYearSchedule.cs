using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.SystemUtilities;
using Architecture.Model.SystemUtilities;

namespace Architecture.BLL.SystemUtilities;

public class SeasonYearSchedule
{
	public static int save(Architecture.Model.SystemUtilities.SeasonYearSchedule obj)
	{
		try
		{
			_ = obj.Id;
			return new Architecture.DAL.SystemUtilities.SeasonYearSchedule().SetData(obj, "USP_SeasonYearSchedule_InsertAndUpdate");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAll(Architecture.Model.SystemUtilities.SeasonYearSchedule obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			return GenericProvider.GetDataTableProc<Architecture.Model.SystemUtilities.SeasonYearSchedule>("USP_SeasonYearSchedule_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.SystemUtilities.SeasonYearSchedule ReadById(int Id)
	{
		try
		{
			new List<Architecture.Model.SystemUtilities.SeasonYearSchedule>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			return new Architecture.DAL.SystemUtilities.SeasonYearSchedule().GetData("USP_SeasonYearSchedule_ReadById", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
