using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Account;
using Architecture.DAL.Common;
using Architecture.Model.Account;

namespace Architecture.BLL.Account;

public class lookUp
{
	public static int Save(Architecture.Model.Account.lookUp obj)
	{
		try
		{
			if (obj.LookUpId == 0)
			{
				return new Architecture.DAL.Account.lookUp().SetData(obj, "[Account].[USP_lookUp_Insert]");
			}
			return new Architecture.DAL.Account.lookUp().SetData(obj, "[Account].[USP_lookUp_Update]");
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Account.lookUp>("[Account].[USP_lookUp_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Account.lookUp> FormHistoryWithList()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			new List<Architecture.Model.Account.lookUp>();
			return new Architecture.DAL.Account.lookUp().GetAll("[Account].[USP_lookUp_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Account.lookUp ReadById(int Id)
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
			new List<Architecture.Model.Account.lookUp>();
			return new Architecture.DAL.Account.lookUp().GetAll("[Account].[USP_lookUp_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataByTypeId(int Id)
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
				Value = "GetDataByTypeId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Account.lookUp>("[Account].[USP_lookUp_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataPaymentTypeForPayments()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetDataPaymentTypeForPayments"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Account.lookUp>("[Account].[USP_lookUp_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataPaymentTypeForReceipts()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetDataPaymentTypeForReceipts"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Account.lookUp>("[Account].[USP_lookUp_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
