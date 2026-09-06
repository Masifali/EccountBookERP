using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.SmartApp;

namespace Architecture.DAL.SmartApp;

public class Dashboard
{
	public List<Architecture.Model.SmartApp.Dashboard> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.SmartApp.Dashboard> lst = new List<Architecture.Model.SmartApp.Dashboard>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.SmartApp.Dashboard>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
