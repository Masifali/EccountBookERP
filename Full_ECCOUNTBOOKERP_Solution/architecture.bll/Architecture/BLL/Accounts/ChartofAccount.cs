using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class ChartofAccount
{
	public static int Save(Architecture.Model.Accounts.ChartofAccount chartofAccount)
	{
		try
		{
			chartofAccount.EntryDate = DateTime.Now;
			chartofAccount.ModifyDate = DateTime.Now;
			chartofAccount.PostDate = DateTime.Now;
			if (chartofAccount.Id == 0)
			{
				if (!string.IsNullOrEmpty(chartofAccount.ParentAccountCode))
				{
					chartofAccount.AccountCode = chartofAccount.ParentAccountCode;
				}
				DataTable dtcoa = new DataTable();
				dtcoa = ReadNewCodebyParentCode(chartofAccount);
				int i = 0;
				if (i < dtcoa.Rows.Count)
				{
					chartofAccount.AccountCode = dtcoa.Rows[i]["AccountCode"].ToString();
					chartofAccount.Account_Level = Conversion.ToInt(dtcoa.Rows[i]["Account_Level"]);
					if (string.IsNullOrEmpty(chartofAccount.AccountGroup))
					{
						chartofAccount.AccountGroup = "Group";
					}
				}
				if (chartofAccount.ParentAccountCode == "" || chartofAccount.ParentAccountCode == null)
				{
					chartofAccount.ParentAccountCode = "0";
				}
				chartofAccount.ActionId = 1;
				return new Architecture.DAL.Accounts.ChartofAccount().SetData(chartofAccount, "Proc_ChartofAccount_Insert");
			}
			return new Architecture.DAL.Accounts.ChartofAccount().SetData(chartofAccount, "Proc_ChartofAccount_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveList(Architecture.Model.Accounts.ChartofAccount obj)
	{
		try
		{
			return new Architecture.DAL.Accounts.ChartofAccount().SetDataList(obj, "Proc_ChartofAccount_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAccountLevel(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			p.Add(new SqlParameter("@CoaType", "ReadByFinancialYearIdNOrganizationIdNCompanyIdNParentAccountCode"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("Sp_ChartofAccount_GetAllMethodFromCOA", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAllAccountgroup(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			}
			if (obj.Account_Level != 0)
			{
				p.Add(new SqlParameter("@Account_Level", obj.Account_Level));
			}
			if (obj.AccountTypeId != 0)
			{
				p.Add(new SqlParameter("@AccountTypeId", obj.AccountTypeId));
			}
			if (obj.AccouuntClassId != 0)
			{
				p.Add(new SqlParameter("@AccountClassId", obj.AccouuntClassId));
			}
			if (!string.IsNullOrWhiteSpace(obj.AccountClassIds))
			{
				p.Add(new SqlParameter("@AccountClassIds", obj.AccountClassIds));
			}
			if (!string.IsNullOrWhiteSpace(obj.AccountTypeIds))
			{
				p.Add(new SqlParameter("@AccountTypeIds", obj.AccountTypeIds));
			}
			p.Add(new SqlParameter("@CoaType", "ReadAllAccountGroup"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("Sp_ChartofAccount_GetAllMethodFromCOA", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DetailAccount(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@UsersId", obj.UserId));
			p.Add(new SqlParameter("@AppId", obj.AppId));
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			}
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter("@CostCenterId", obj.CostCenterId));
			}
			p.Add(new SqlParameter("@CoaType", "DetailAccount"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("Sp_ChartofAccount_GetAllMethodFromCOA", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PayeeTitleByAccountId(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@AccountId", obj.AccountId));
			p.Add(new SqlParameter("@Activity", "PayeeTitleByAccountId"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("[dbo].[AcPayeeTitle_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadNewCodebyParentCode(Architecture.Model.Accounts.ChartofAccount chartofAccount)
	{
		DataTable dt = null;
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", chartofAccount.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", chartofAccount.CompanyId));
			if (chartofAccount.SearchDetailAccount)
			{
				p.Add(new SqlParameter("@AccountGroup", "Detail"));
				p.Add(new SqlParameter("@CoaType", "ReadSpecificAccounts"));
				return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("Sp_ChartofAccount_GetAllMethodFromCOA", p);
			}
			if (chartofAccount.SearchGroupAccount)
			{
				p.Add(new SqlParameter("@AccountGroup", "Group"));
				p.Add(new SqlParameter("@CoaType", "ReadSpecificAccounts"));
				return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("Sp_ChartofAccount_GetAllMethodFromCOA", p);
			}
			p.Add(new SqlParameter("@CoaType", "NewCodeByParentCode"));
			p.Add(new SqlParameter("@AccountCode", chartofAccount.AccountCode));
			p.Add(new SqlParameter("@FinancialYearId", chartofAccount.FinancialYearId));
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("Sp_ChartofAccount_GetAllMethodFromCOA", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Accounts.ChartofAccount GetByID(int Id)
	{
		try
		{
			return new Architecture.DAL.Accounts.ChartofAccount().ReadByID(Id);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int DELETEANDUPDATECHARTOFACCOUNTBYID(ReportsParameters obj)
	{
		try
		{
			return new Architecture.DAL.Accounts.ChartofAccount().DELETEANDUPDATEBYACCOUNTID(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAccountAvailableforcustomgroup(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@CoaType", "GetAccountsAvailableForAcCustomGroup"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("Sp_ChartofAccount_GetAllMethodFromCOA", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllAccountsByCustomGroupId(Architecture.Model.Accounts.ChartofAccount obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter
			{
				ParameterName = "@AcLookUpsId",
				Value = obj.Id
			});
			p.Add(new SqlParameter("@CoaType", "GetAllAccountsByCustomGroupId"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("Sp_ChartofAccount_GetAllMethodFromCOA", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAll3rdLevelAccountsForPayablesandReceeivablesAging(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll3rdLevelAccountsForPayablesandReceeivablesAging"
			});
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_AccountsOpeningBalances_GetMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static void DeleteAccountsFromCustomGroup(Architecture.Model.Accounts.AccountsCustomGroups obj)
	{
		try
		{
			foreach (Architecture.Model.Accounts.AccountsCustomGroups item in obj.GetList)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter("@AcLookUpsId", item.AcLookUpsId));
				p.Add(new SqlParameter("@Id", item.ChartOfAccountId));
				p.Add(new SqlParameter("@CoaType", "DeleteAccountsFromCustomGroup"));
				GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("Sp_ChartofAccount_GetAllMethodFromCOA", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetCoaDetailByAccountId(Architecture.Model.Accounts.ChartofAccount obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter("@CoaType", "GetCoaDetailByAccountId"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("Sp_ChartofAccount_GetAllMethodFromCOA", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAccountsWithBalanceByGroupIdsOrCustomGroups(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@ToDate", obj.ToDate));
			p.Add(new SqlParameter("@UsersId", obj.UserId));
			if (obj.AccountIds != null && obj.AccountIds != string.Empty)
			{
				p.Add(new SqlParameter("@AccountGroupIds", obj.AccountIds));
			}
			if (obj.Ids != null && obj.Ids != string.Empty)
			{
				p.Add(new SqlParameter("@CustomGroupIds", obj.Ids));
			}
			if (obj.AccountTypeId != 0)
			{
				p.Add(new SqlParameter("@AccountTypeId", obj.AccountTypeId));
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("USP_GetAccountsByGroupIds", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllDetailAccountsForMobileApp(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			}
			if (obj.Keyword != null && obj.Keyword != string.Empty)
			{
				p.Add(new SqlParameter("@Keyword", obj.Keyword));
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			p.Add(new SqlParameter("@CoaType", "GetAllDetailAccountsForMobileApp"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("Sp_ChartofAccount_GetAllMethodFromCOA", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getGLBalanceBranchesWiseByAccountId(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@AccountId", obj.AccountId));
			p.Add(new SqlParameter("@FromDate", obj.FromDate));
			p.Add(new SqlParameter("@ToDate", obj.ToDate));
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("usp_getGLBalanceBranchesWiseByAccountId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
