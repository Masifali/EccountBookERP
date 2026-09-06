using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.SMS;

namespace Architecture.DAL.SMS;

public class SmsProvider
{
	public List<Architecture.Model.SMS.SmsProvider> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.SMS.SmsProvider> lst = new List<Architecture.Model.SMS.SmsProvider>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.SMS.SmsProvider>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
