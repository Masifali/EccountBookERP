using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class AccountsCustomGroups
{
	public static int Save(Architecture.Model.Accounts.AccountsCustomGroups obj)
	{
		try
		{
			if (obj.SortNo == 0)
			{
				return new Architecture.DAL.Accounts.AccountsCustomGroups().SetData(obj, "Sp_AccountsCustomGroups_Insert");
			}
			return new Architecture.DAL.Accounts.AccountsCustomGroups().SetData(obj, "Sp_AccountsCustomGroups_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AcLookUps>("Sp_AccountsCustomGroups_ReadAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable AccountCustomGroup_UnAllocatedData(int OrganizationId, int CompanyId, int CustomGroupId, int AccountTypeId, int ParentAccountId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CustomGroupId",
				Value = CustomGroupId
			});
			if (AccountTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountTypeId",
					Value = AccountTypeId
				});
			}
			if (ParentAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentAccountId",
					Value = ParentAccountId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AcLookUps>("[dbo].[USP_AccountCustomGroup_UnAllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable AccountCustomGroup_AllocatedData(int OrganizationId, int CompanyId, int CustomGroupId, int AccountTypeId, int ParentAccountId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CustomGroupId",
				Value = CustomGroupId
			});
			if (AccountTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountTypeId",
					Value = AccountTypeId
				});
			}
			if (ParentAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentAccountId",
					Value = ParentAccountId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AcLookUps>("[dbo].[USP_AccountCustomGroup_AllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
