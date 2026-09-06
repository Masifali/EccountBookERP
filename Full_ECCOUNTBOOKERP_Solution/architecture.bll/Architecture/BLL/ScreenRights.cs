using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class ScreenRights
{
	public static int save(Architecture.Model.ScreenRights obj)
	{
		try
		{
			return new Architecture.DAL.ScreenRights().SetData(obj, "Sp_ScreenRights_Insert");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAll()
	{
		try
		{
			return GenericProvider.GetDataTableProc<Architecture.Model.ScreenRights>("Sp_UrmRightsName_GetAllMethod");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.ScreenRights> ReadByScreenId(int ScreenId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByScreenId"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ScreenId",
				Value = ScreenId
			});
			new List<Architecture.Model.ScreenRights>();
			return new Architecture.DAL.ScreenRights().GetDate("Sp_ScreenRights_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
