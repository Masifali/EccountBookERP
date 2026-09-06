using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class ScreensSpecialRights
{
	public static int save(Architecture.Model.ScreensSpecialRights obj)
	{
		try
		{
			return new Architecture.DAL.ScreensSpecialRights().SetData(obj, "Sp_ScreensSpecialRights_Insert");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static void Delete(Architecture.Model.ScreensSpecialRights obj)
	{
		try
		{
			foreach (Architecture.Model.ScreensSpecialRights item in obj.ScreensSpecialRightsList)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = item.Id
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@RightId",
					Value = item.RightId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenId",
					Value = item.ScreenId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "DeleteById"
				});
				GenericProvider.GetDataTableProc<Architecture.Model.ScreensSpecialRights>("Sp_ScreensSpecialRights_GetAllMethod", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllSpecialRights(int ScreenId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (ScreenId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenId",
					Value = ScreenId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByScreenId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ScreensSpecialRights>("[dbo].[Sp_ScreensSpecialRights_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
