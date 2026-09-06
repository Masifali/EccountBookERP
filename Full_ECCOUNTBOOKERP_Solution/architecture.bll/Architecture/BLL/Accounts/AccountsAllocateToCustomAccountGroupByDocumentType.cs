using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model.Account;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Accounts;

public class AccountsAllocateToCustomAccountGroupByDocumentType
{
	public static int Save(Architecture.Model.Accounts.AccountsAllocateToCustomAccountGroupByDocumentType obj)
	{
		try
		{
			return new Architecture.DAL.Accounts.AccountsAllocateToCustomAccountGroupByDocumentType().SetData(obj, "[dbo].[USP_AccountsAllocateToCustomAccountGroupByDocumentType_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable AccountCustomGroup_UnAllocatedData(int OrganizationId, int CompanyId, int CustomGroupId, int AccountTypeId)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AcLookUps>("[dbo].[USP_AccountsAllocateToCustomAccountGroupByDocumentType_UnAllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int OrganizationId, int CompanyId, string Ids, int CustomGroupId)
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
				ParameterName = "@Ids",
				Value = Ids
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CustomGroupId",
				Value = CustomGroupId
			});
			GenericProvider.GetDataTableProc<ItemAllocationToSupplierCustomer>("[dbo].[USp_AccountsAllocateToCustomAccountGroupByDocumentTypeDeleteById]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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

	public static DataTable AccountCustomGroup_AllocatedData(int OrganizationId, int CompanyId, int CustomGroupId, int AccountTypeId)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AcLookUps>("[dbo].[USP_AccountsAllocateToCustomAccountGroupByDocumentType_AllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetCustomAccountsByGroup(int CompanyId, int CustomGroupId)
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
				ParameterName = "@CustomGroupId",
				Value = CustomGroupId
			});
			return GenericProvider.GetDataTableProc<lookUp>("[dbo].[usp_getCustomAccountsByGroupId]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetCustomAccountGroupsByDocumentType(int DocumentTypeId = 0, string Activity = null)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = DocumentTypeId
				});
			}
			if (!string.IsNullOrWhiteSpace(Activity))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = Activity
				});
			}
			return GenericProvider.GetDataTableProc<lookUp>("[dbo].[usp_getCustomAccountGroupsByDocumentType]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
