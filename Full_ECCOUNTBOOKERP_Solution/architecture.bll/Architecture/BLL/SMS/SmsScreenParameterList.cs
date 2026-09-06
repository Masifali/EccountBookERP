using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.SMS;
using Architecture.Model.SMS;

namespace Architecture.BLL.SMS;

public class SmsScreenParameterList
{
	public static int Save(Architecture.Model.SMS.SmsScreenParameterList obj)
	{
		try
		{
			return new Architecture.DAL.SMS.SmsScreenParameterList().SetData(obj, "Sp_SmsScreenParameterList_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetParameter(Architecture.Model.SMS.SmsScreenParameterList obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (obj.ScreenId != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenId",
					Value = obj.ScreenId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.SMS.SmsScreenParameterList>("Sp_SmsScreenParameterList_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
