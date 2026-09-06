using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Themes_ERP;

namespace Architecture.DAL.Themes_ERP;

public class ThemeSettings
{
	public int SetData(Architecture.Model.Themes_ERP.ThemeSettings obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Conversion.ToInt(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Themes_ERP.ThemeSettings> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Themes_ERP.ThemeSettings> lst = new List<Architecture.Model.Themes_ERP.ThemeSettings>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Themes_ERP.ThemeSettings>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
