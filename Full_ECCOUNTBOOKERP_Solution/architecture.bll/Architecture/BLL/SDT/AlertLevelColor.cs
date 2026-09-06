using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;

namespace Architecture.BLL.SDT;

public class AlertLevelColor
{
	public static DataTable FormHistory()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			return GenericProvider.GetDataTableProc<AlertLevelColor>("[sdt].[USP_AlertLevelColor_FormHistory]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
