using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Accounts;

public class COAAllocation
{
	public static int Save(Architecture.Model.Accounts.COAAllocation obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Accounts.COAAllocation().SetData(obj, "Sp_COAAllocation_Insert");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPendingForAllocation(Architecture.Model.Accounts.COAAllocation obj)
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
				ParameterName = "@Activity",
				Value = "ChartOfAccountPendingForAllocation"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.COAAllocation>("Sp_COAAllocation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetUnallocatedAccountsByCompanyId(Architecture.Model.Accounts.COAAllocation obj)
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
				ParameterName = "@Activity",
				Value = "GetUnallocatedAccountsByCompanyId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.COAAllocation>("Sp_COAAllocation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAllForAllocation(Architecture.Model.Accounts.ChartofAccount obj)
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
				ParameterName = "@ParentAccountCode",
				Value = obj.ParentAccountCode
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAllForAllocation"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.COAAllocation>("Sp_COAAllocation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPageNoandStatusbyAccountId(Architecture.Model.Accounts.COAAllocation obj)
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetPageNoandStatusbyAccountId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.COAAllocation>("Sp_COAAllocation_GetAllMethod", p);
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
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = obj.UserId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "COAAllocationSearch"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.COAAllocation>("Sp_COAAllocation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetForComboBind(ReportsParameters obj)
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
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = obj.UserId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "COAForCombobindig"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.COAAllocation>("Sp_COAAllocation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDetailAccountByDocumentTypeId(Architecture.Model.Accounts.COAAllocation obj)
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetDetailAccountsByDocumentTypeId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.COAAllocation>("Sp_COAAllocation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Accounts.COAAllocation> GetLst(Architecture.Model.Accounts.COAAllocation obj)
	{
		try
		{
			DataTable dt = new DataTable();
			List<Architecture.Model.Accounts.COAAllocation> lsts = new List<Architecture.Model.Accounts.COAAllocation>();
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
				ParameterName = "@Activity",
				Value = "COAAllocationSearch"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.COAAllocation>("Sp_COAAllocation_GetAllMethod", p);
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				Architecture.Model.Accounts.COAAllocation lst = new Architecture.Model.Accounts.COAAllocation();
				lst.AccountTitle = dt.Rows[i]["AccountTitle"].ToString();
				lst.Id = Conversion.ToInt(dt.Rows[i]["Id"]);
				lst.AccountTypeId = Conversion.ToInt(dt.Rows[i]["AccountTypeId"]);
				lsts.Add(lst);
			}
			return lsts;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int UpdatePageNo(Architecture.Model.Accounts.COAAllocation COAA)
	{
		try
		{
			return new Architecture.DAL.Accounts.COAAllocation().UpdateForPageNo(COAA);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetParentCodeByChartofAccountId(Architecture.Model.Accounts.ChartofAccount obj)
	{
		try
		{
			int Code = 0;
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetParentCodeByChartofAccountId"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.COAAllocation>("Sp_COAAllocation_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				Code = Conversion.ToInt(dt.Rows[0]["ParentAccountCode"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Get3rdLevelGroupAccounts(Architecture.Model.Accounts.ChartofAccount obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@TypeId",
				Value = obj.AccountTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "Get3rdLevelGroupAccounts"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.COAAllocation>("Sp_COAAllocation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSupplierGlAccountExistInPurchaseInvoice(Architecture.Model.Accounts.COAAllocation obj)
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
				ParameterName = "@Activity",
				Value = "GetSupplierGlAccountExistInPurchaseInvoice"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.COAAllocation>("Sp_COAAllocation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAccountTitleByAccountTypeIds(ReportsParameters obj)
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
				ParameterName = "@AppId",
				Value = obj.AppId
			});
			if (obj.AccountTypeIds != null && obj.AccountTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountTypeIds",
					Value = obj.AccountTypeIds
				});
			}
			if (obj.AccountTypeIdsNot != null && obj.AccountTypeIdsNot != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountTypeIdsNot",
					Value = obj.AccountTypeIdsNot
				});
			}
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = obj.UserId
				});
			}
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CostCenterId",
					Value = obj.CostCenterId
				});
			}
			if (obj.NotReferred != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@NotReferred",
					Value = obj.NotReferred
				});
			}
			if (obj.RecId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = obj.RecId
				});
			}
			if (obj.AccountClassIds != null && obj.AccountClassIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountClassIds",
					Value = obj.AccountClassIds
				});
			}
			if (obj.AccountClassIdsNot != null && obj.AccountClassIdsNot != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountClassIdsNot",
					Value = obj.AccountClassIdsNot
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAccountTitleByAccountTypeIds"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.COAAllocation>("Sp_COAAllocation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Accounts_GetAccountTitleByAccountTypeIds(ReportsParameters obj)
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
				ParameterName = "@AppId",
				Value = obj.AppId
			});
			if (obj.AccountTypeIds != null && obj.AccountTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountTypeIds",
					Value = obj.AccountTypeIds
				});
			}
			if (obj.AccountTypeIdsNot != null && obj.AccountTypeIdsNot != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountTypeIdsNot",
					Value = obj.AccountTypeIdsNot
				});
			}
			if (obj.AccountClassIds != null && obj.AccountClassIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountClassIds",
					Value = obj.AccountClassIds
				});
			}
			if (obj.AccountClassIdsNot != null && obj.AccountClassIdsNot != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountClassIdsNot",
					Value = obj.AccountClassIdsNot
				});
			}
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = obj.UserId
				});
			}
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CostCenterId",
					Value = obj.CostCenterId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.COAAllocation>("[dbo].[USP_Accounts_GetAccountTitleByAccountTypeIds]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAccountTitleByAccountClassIds(ReportsParameters obj)
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
			if (obj.AccountClassIds != null && obj.AccountClassIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountClassIds",
					Value = obj.AccountClassIds
				});
			}
			if (obj.AccountClassIdsNot != null && obj.AccountClassIdsNot != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountClassIdsNot",
					Value = obj.AccountClassIdsNot
				});
			}
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = obj.UserId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAccountTitleByAccountClassIds"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.COAAllocation>("Sp_COAAllocation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Accounts.ChartofAccount obj)
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
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.COAAllocation>("Sp_COAAllocation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllocatedAndUnAllocatedAccountsToCompany(ReportsParameters obj)
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
				ParameterName = "@ActionId",
				Value = obj.ActionId
			});
			if (obj.AccountTypeIds != null && obj.AccountTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountTypeIds",
					Value = obj.AccountTypeIds
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "AllocatedAndUnAllocatedAccountsToCompany"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.COAAllocation>("[dbo].[Sp_COAAllocation_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int CompanyId, string Ids)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ChartOfAccountIds",
				Value = Ids
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "DeleteAllocatedAccountsCompanyWise"
			});
			GenericProvider.GetDataTableProc<ItemAllocation>("[dbo].[Sp_COAAllocation_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
