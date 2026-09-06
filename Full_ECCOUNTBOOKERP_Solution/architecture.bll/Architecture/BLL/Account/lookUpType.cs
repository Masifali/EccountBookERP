using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Account;
using Architecture.DAL.Common;
using Architecture.Model.Account;

namespace Architecture.BLL.Account;

public class lookUpType
{
	public static int Save(Architecture.Model.Account.lookUpType obj)
	{
		try
		{
			if (obj.LookUpTypeId == 0)
			{
				return new Architecture.DAL.Account.lookUpType().SetData(obj, "[Account].[USP_lookUpType_Insert]");
			}
			return new Architecture.DAL.Account.lookUpType().SetData(obj, "[Account].[USP_lookUpType_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Account.lookUp obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Account.lookUpType>("[Account].[USP_lookUpType_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Account.lookUpType> FormHistoryWithList(Architecture.Model.Account.lookUpType obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			new List<Architecture.Model.Account.lookUpType>();
			return new Architecture.DAL.Account.lookUpType().GetAll("[Account].[USP_lookUpType_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Account.lookUpType ReadById(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@LookUpTypeId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.Account.lookUpType>();
			return new Architecture.DAL.Account.lookUpType().GetAll("[Account].[USP_lookUpType_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
