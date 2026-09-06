using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.pcc;
using Architecture.Model;
using Architecture.Model.pcc;

namespace Architecture.BLL.pcc;

public class ServiceActivityAllocationToDocumentType
{
	public static int Save(Architecture.Model.pcc.ServiceActivityAllocationToDocumentType obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.pcc.ServiceActivityAllocationToDocumentType().SetData(obj, "[pcc].[USP_ServiceActivityAllocationToDocumentType_Insert]");
			}
			return new Architecture.DAL.pcc.ServiceActivityAllocationToDocumentType().SetData(obj, "[pcc].[USP_ServiceActivityAllocationToDocumentType_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.pcc.ServiceActivityAllocationToDocumentType ReadById(int Id)
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
			new List<Architecture.Model.pcc.ServiceActivityAllocationToDocumentType>();
			return new Architecture.DAL.pcc.ServiceActivityAllocationToDocumentType().GetData("[pcc].[USP_ServiceActivityAllocationToDocumentType_GetAllMethod]", p)[0];
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
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ServiceActivityAllocationToDocumentType>("[pcc].[USP_ServiceActivityAllocationToDocumentType_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetServiceActivityAllocationToDocumentType(ReportsParameters obj)
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
				ParameterName = "@RefDocumentTypeId",
				Value = obj.RefDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetServiceActivityAllocationToDocumentType"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ServiceActivityAllocationToDocumentType>("[pcc].[USP_ServiceActivityAllocationToDocumentType_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByID(int Id)
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
				Value = "DeleteById"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.pcc.ServiceActivityAllocationToDocumentType>("[pcc].[USP_ServiceActivityAllocationToDocumentType_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
