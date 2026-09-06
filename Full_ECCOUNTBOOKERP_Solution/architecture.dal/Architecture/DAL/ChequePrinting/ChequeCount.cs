using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.ChequePrinting;

namespace Architecture.DAL.ChequePrinting;

public class ChequeCount
{
	public List<Architecture.Model.ChequePrinting.ChequeCount> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ChequePrinting.ChequeCount> lst = new List<Architecture.Model.ChequePrinting.ChequeCount>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.ChequePrinting.ChequeCount>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
