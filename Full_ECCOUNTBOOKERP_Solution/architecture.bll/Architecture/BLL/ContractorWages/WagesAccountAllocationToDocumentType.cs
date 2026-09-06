using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.ContractorWages;
using Architecture.Model;
using Architecture.Model.ContractorWages;

namespace Architecture.BLL.ContractorWages;

public class WagesAccountAllocationToDocumentType
{
	public static int Save(Architecture.Model.ContractorWages.WagesAccountAllocationToDocumentType obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.ContractorWages.WagesAccountAllocationToDocumentType().SetData(obj, "[dbo].[USP_WagesAccountAllocationToDocumentType_Insert]");
			}
			return new Architecture.DAL.ContractorWages.WagesAccountAllocationToDocumentType().SetData(obj, "[dbo].[USP_WagesAccountAllocationToDocumentType_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.ContractorWages.WagesAccountAllocationToDocumentType ReadById(int Id)
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
			new List<Architecture.Model.ContractorWages.WagesAccountAllocationToDocumentType>();
			return new Architecture.DAL.ContractorWages.WagesAccountAllocationToDocumentType().GetData("[dbo].[USP_WagesAccountAllocationToDocumentType_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.WagesAccountAllocationToDocumentType>("[dbo].[USP_WagesAccountAllocationToDocumentType_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDocumentTypeForAllocation(int OrganizationId, int CompanyId)
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
				ParameterName = "@Activity",
				Value = "GetDocumentTypeForAllocation"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.WagesAccountAllocationToDocumentType>("[dbo].[USP_WagesAccountAllocationToDocumentType_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllocatedAndUnAllocatedActivitiesBydocumentTypeIds(ReportsParameters obj)
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
				ParameterName = "@DocumentTypeIds",
				Value = obj.RefDocumentTypeIds
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAllocatedAndUnAllocatedActivitiesBydocumentTypeIds"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.ActionId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.WagesAccountAllocationToDocumentType>("[dbo].[USP_WagesAccountAllocationToDocumentType_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByID(Architecture.Model.ContractorWages.WagesAccountAllocationToDocumentType obj)
	{
		try
		{
			new Architecture.DAL.ContractorWages.WagesAccountAllocationToDocumentType().DeleteByID(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
