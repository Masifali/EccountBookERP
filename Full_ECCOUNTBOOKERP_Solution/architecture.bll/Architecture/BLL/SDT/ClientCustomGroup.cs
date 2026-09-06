using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.SDT;
using Architecture.Model.SDT;

namespace Architecture.BLL.SDT;

public class ClientCustomGroup
{
	public static int SaveAndUpdate(Architecture.Model.SDT.ClientCustomGroup obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.SDT.ClientCustomGroup().SetData(obj, "[sdt].[USP_ClientCustomGroup_InsertAndUpdate]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ClientCustomGroupUnAllocatedToCustomGroup(int OrganizationId, int CompanyId, int CustomGroupId)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.SDT.ClientCustomGroup>("[sdt].[USP_ClientCustomGroupUnAllocatedToCustomGroup]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ClientCustomGroupAllocatedToCustomGroup(int OrganizationId, int CompanyId, int CustomGroupId)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.SDT.ClientCustomGroup>("[sdt].[USP_ClientCustomGroupAllocatedToCustomGroup]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetCustomGroupAllocatedToCustomerBySupplierCustomerId(int OrganizationId, int CompanyId, int SupplierCustomerId)
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
				ParameterName = "@SupplierCustomerId",
				Value = SupplierCustomerId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.SDT.ClientCustomGroup>("[sdt].[USP_GetCustomGroupAllocatedToCustomerBySupplierCustomerId]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
