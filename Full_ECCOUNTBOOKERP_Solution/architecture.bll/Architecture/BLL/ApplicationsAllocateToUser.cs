using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;

namespace Architecture.BLL;

public class ApplicationsAllocateToUser
{
	public static long Save(Architecture.Model.ApplicationsAllocateToUser obj)
	{
		try
		{
			if (obj.ApplicationsAllocateToUserId == 0L)
			{
				return new Architecture.DAL.ApplicationsAllocateToUser().SetData(obj, "[dbo].[USP_ApplicationsAllocateToUser_InsertAndUpdate]");
			}
			return 0L;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int OrganizationId, int CompanyId, string Ids, int CostCenterId, int UserId)
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
				ParameterName = "@Ids",
				Value = Ids
			});
			if (CostCenterId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CostCenterId",
					Value = CostCenterId
				});
			}
			if (UserId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = UserId
				});
			}
			GenericProvider.GetDataTableProc<ItemAllocationToSupplierCustomer>("[dbo].[USp_ApplicationsAllocateToUserDeleteById]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ApplicationsAllocateToUser_ByUser(int CompanyId, int UserId, int MasterAppId)
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
				ParameterName = "@UserId",
				Value = UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@MasterAppId",
				Value = MasterAppId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ApplicationsAllocateToUser>("[dbo].[USP_ApplicationsAllocateToUser_ByUser]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ApplicationsAllocateToUser_AllocatedData(int CompanyId, int UserId = 0)
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
				ParameterName = "@UserId",
				Value = UserId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ApplicationsAllocateToUser>("[dbo].[USP_ApplicationsAllocateToUser_AllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ApplicationsAllocateToUser_UnAllocatedData(int CompanyId, int UserId = 0)
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
				ParameterName = "@UserId",
				Value = UserId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ApplicationsAllocateToUser>("[dbo].[USP_ApplicationsAllocateToUser_UnAllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<ApplicationsAllocateToCompany> GetApplicationsAllocateToCompany()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			return Conversion.ConvertDataTableToList<ApplicationsAllocateToCompany>(GenericProvider.GetDataTableProc<ApplicationsAllocateToCompany>("[dbo].[USP_GetApplicationsAllocateToCompany]", p));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
