using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;

namespace Architecture.BLL.BankSchema;

public class BankName
{
	public static DataTable FormHistory()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			return GenericProvider.GetDataTableProc<BankName>("[Bank].[USP_BankName_FormHistory]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
