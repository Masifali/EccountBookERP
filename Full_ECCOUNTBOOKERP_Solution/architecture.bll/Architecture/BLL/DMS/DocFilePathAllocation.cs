using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.DMS;
using Architecture.Model.DMS;

namespace Architecture.BLL.DMS;

public class DocFilePathAllocation
{
	public static long Save(Architecture.Model.DMS.DocFilePathAllocation obj)
	{
		try
		{
			return new Architecture.DAL.DMS.DocFilePathAllocation().SetData(obj, "[DMS].Sp_DocFilePathAllocation_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteFileFromAllocationByUserId(Architecture.Model.DMS.DocFilePathAllocation obj)
	{
		try
		{
			foreach (Architecture.Model.DMS.DocFilePathAllocation item in obj.DocFilePathAllocationslist)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = item.OrganizationId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = item.CompanyId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@DocFilePathId",
					Value = item.DocFilePathId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = item.SubUserId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "DeleteFileFromAllocationByUserId"
				});
				GenericProvider.GetDataTableProc<Architecture.Model.DMS.FolderHierarchy>("DMS.Sp_FolderHierarchy_GetAllMethod", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
