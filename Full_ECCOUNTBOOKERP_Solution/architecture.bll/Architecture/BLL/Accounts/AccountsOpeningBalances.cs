using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class AccountsOpeningBalances
{
	public static int Save(Architecture.Model.Accounts.AccountsOpeningBalances obj)
	{
		try
		{
			return new Architecture.DAL.Accounts.AccountsOpeningBalances().SetData(obj, "Sp_AccountsOpeningBalances_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveAllRecord(Architecture.Model.Accounts.AccountsOpeningBalances obj)
	{
		try
		{
			return new Architecture.DAL.Accounts.AccountsOpeningBalances().SetDataAllRecord(obj, "Sp_AccountsOpeningBalances_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Accounts.AccountsOpeningBalances obj)
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.EntryUser != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = obj.EntryUser
				});
			}
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AccountsOpeningBalances>("Sp_AccountsOpeningBalances_GetMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetById(Architecture.Model.Accounts.AccountsOpeningBalances obj)
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetById"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AccountsOpeningBalances>("Sp_AccountsOpeningBalances_GetMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
