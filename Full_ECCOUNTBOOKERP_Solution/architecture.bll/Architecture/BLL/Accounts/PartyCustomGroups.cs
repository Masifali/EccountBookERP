using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class PartyCustomGroups
{
	public static int Save(Architecture.Model.Accounts.PartyCustomGroups obj)
	{
		try
		{
			return new Architecture.DAL.Accounts.PartyCustomGroups().SetData(obj, "USP_PartyCustomGroups_InsertAndUpdate");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PartyCustomGroups_UnAllocatedData(int OrganizationId, int CompanyId, int CustomGroupId, int CustomerGroupId)
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
			if (CustomerGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomerGroupId",
					Value = CustomerGroupId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AcLookUps>("[dbo].[USP_PartyCustomGroups_UnAllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PartyCustomGroups_AllocatedData(int OrganizationId, int CompanyId, int CustomGroupId, int CustomerGroupId)
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
			if (CustomerGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomerGroupId",
					Value = CustomerGroupId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AcLookUps>("[dbo].[USP_PartyCustomGroups_AllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeletePartyCustomGroup(Architecture.Model.Accounts.PartyCustomGroups obj)
	{
		try
		{
			foreach (Architecture.Model.Accounts.PartyCustomGroups item in obj.GetList)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = item.organizationId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = item.companyId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomGroupId",
					Value = item.CustomGroupId
				});
				p.Add(new SqlParameter("@SupplierCustomerId", item.SupplierCustomerId));
				GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("[dbo].[USP_PartyCustomGroups_DeleteById]", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
