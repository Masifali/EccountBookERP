using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.ChequePrinting;

namespace Architecture.DAL.ChequePrinting;

public class ChequePrintListCards
{
	public List<Architecture.Model.ChequePrinting.ChequePrintListCards> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ChequePrinting.ChequePrintListCards> lst = new List<Architecture.Model.ChequePrinting.ChequePrintListCards>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.ChequePrinting.ChequePrintListCards>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
