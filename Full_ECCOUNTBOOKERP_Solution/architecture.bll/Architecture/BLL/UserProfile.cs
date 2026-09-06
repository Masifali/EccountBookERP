using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class UserProfile
{
	public static int Save(Architecture.Model.UserProfile obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.UserProfile().SetData(obj, "Sp_UserProfile_Insert");
			}
			return new Architecture.DAL.UserProfile().SetData(obj, "Sp_UserProfile_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.UserProfile GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			return new Architecture.DAL.UserProfile().GetData("SP_UserProfile_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetUserProfileByUserId(int UserId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetUserProfileByUserId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.UserAccount>("SP_UserProfile_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
