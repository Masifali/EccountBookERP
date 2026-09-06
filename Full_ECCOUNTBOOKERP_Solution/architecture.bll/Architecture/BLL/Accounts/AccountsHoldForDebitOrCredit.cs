using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class AccountsHoldForDebitOrCredit
{
	public static int Save(Architecture.Model.Accounts.AccountsHoldForDebitOrCredit obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Accounts.AccountsHoldForDebitOrCredit().SetData(obj, "[dbo].[USP_AccountsHoldForDebitOrCredit_Insert]");
			}
			return new Architecture.DAL.Accounts.AccountsHoldForDebitOrCredit().SetData(obj, "[dbo].[USP_AccountsHoldForDebitOrCredit_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Formhistory(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AccountsHoldForDebitOrCredit>("[dbo].[USP_AccountsHoldForDebitOrCredit_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByIdDt(Architecture.Model.Accounts.AccountsHoldForDebitOrCredit obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AccountsHoldForDebitOrCredit>("[dbo].[USP_AccountsHoldForDebitOrCredit_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Accounts.AccountsHoldForDebitOrCredit GetById(int Id)
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
			new List<Architecture.Model.Accounts.AccountsHoldForDebitOrCredit>();
			return new Architecture.DAL.Accounts.AccountsHoldForDebitOrCredit().GetAll("[dbo].[USP_AccountsHoldForDebitOrCredit_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
