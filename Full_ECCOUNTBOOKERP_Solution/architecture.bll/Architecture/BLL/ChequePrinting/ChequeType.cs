using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.ChequePrinting;

namespace Architecture.BLL.ChequePrinting;

public class ChequeType
{
	public static DataTable GetChequeType()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			return GenericProvider.GetDataTableProc<Architecture.Model.ChequePrinting.ChequeType>("[dbo].[sp_ChequeType]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
