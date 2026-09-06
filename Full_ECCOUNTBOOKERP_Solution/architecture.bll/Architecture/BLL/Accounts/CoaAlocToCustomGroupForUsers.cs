using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class CoaAlocToCustomGroupForUsers
{
	public static int Save(Architecture.Model.Accounts.CoaAlocToCustomGroupForUsers obj)
	{
		try
		{
			return new Architecture.DAL.Accounts.CoaAlocToCustomGroupForUsers().SetData(obj, "[dbo].[USP_CoaAlocToCustomGroupForUsers_Insert]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(string Ids, int CustomGroupId, int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CoaIds",
				Value = Ids
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CustomGroupId",
				Value = CustomGroupId
			});
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
			GenericProvider.GetDataTableProc<Architecture.Model.Accounts.CoaAlocToCustomGroupForUsers>("[dbo].[USp_CoaAlocToCustomGroupForUserDeleteById]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetUnAllocatedChartOfAccountsToCustomGroup(ReportsParameters obj)
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
				ParameterName = "@CustomGroupId",
				Value = obj.CustomGroupId
			});
			if (obj.BranchesId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			if (obj.AccountId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentAccountId",
					Value = obj.AccountId
				});
			}
			if (obj.AccountTypeId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountTypeId",
					Value = obj.AccountTypeId
				});
			}
			if (obj.CustomerGroupId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomCoaGroupId",
					Value = obj.CustomerGroupId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.CoaCustomGroupAlocToUsers>("[dbo].[USP-GetAllCoaNotAlocToCustomGroupToUsers]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllocatedChartOfAccountsToCustomGroup(ReportsParameters obj)
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
				ParameterName = "@CustomGroupId",
				Value = obj.CustomGroupId
			});
			if (obj.BranchesId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.CoaCustomGroupAlocToUsers>("[dbo].[USP-GetAllCoaAlocToCustomGroupToUsers]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CoaAlocToCustomGroupHistorybyUser(ReportsParameters obj)
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
			if (obj.UserId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = obj.UserId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.CoaCustomGroupAlocToUsers>("[dbo].[USP-CoaAlocToCustomGroupHistorybyUser]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int AccountsStatusShowHideSave(AccountsShowAndHideStatusUserWise obj)
	{
		try
		{
			return new Architecture.DAL.Accounts.CoaAlocToCustomGroupForUsers().AccountsStatusShowHideSave(obj, "[dbo].[USP_AccountsShowAndHideStatusUserWise_Insert]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
