using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Authentication;
using Architecture.DAL.Common;
using Architecture.Model.Authentication;

namespace Architecture.BLL.Authentication;

public class ActivityLog
{
	public static int Save(Architecture.Model.Authentication.ActivityLog obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Authentication.ActivityLog().SetData(obj, "Proc_ActivityLog_Insert");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Authentication.ActivityLog> GetDate(Architecture.Model.Authentication.ActivityLog activity)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = activity.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = activity.CompanyId
			});
			return new Architecture.DAL.Authentication.ActivityLog().GetData("GetMaxDate", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int ReadByConfigDescription(string ConfigDescription)
	{
		try
		{
			int code = 0;
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ConfigDescription",
				Value = ConfigDescription
			});
			DataTable dt = GenericProvider.GetDataTableProc<ConfiqurationDefinition>("Sp_ConfigrationsDefinition_ReadByConfigDescription", p);
			if (dt.Rows.Count > 0)
			{
				code = Conversion.ToInt(dt.Rows[0]["Id"]);
			}
			return code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
