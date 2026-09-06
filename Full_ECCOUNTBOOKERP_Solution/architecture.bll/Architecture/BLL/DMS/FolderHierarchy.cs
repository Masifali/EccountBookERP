using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.DMS;
using Architecture.Model.DMS;

namespace Architecture.BLL.DMS;

public class FolderHierarchy
{
	public static long Save(Architecture.Model.DMS.FolderHierarchy obj)
	{
		try
		{
			if (obj.FolderHierarchyId == 0L)
			{
				return new Architecture.DAL.DMS.FolderHierarchy().SetData(obj, "DMS.Sp_FolderHierarchy_Insert");
			}
			return new Architecture.DAL.DMS.FolderHierarchy().SetData(obj, "DMS.Sp_FolderHierarchy_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.DMS.FolderHierarchy GetByID(long Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@FolderHierarchyId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.DMS.FolderHierarchy>();
			return new Architecture.DAL.DMS.FolderHierarchy().GetAll("DMS.Sp_FolderHierarchy_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.DMS.FolderHierarchy> GetAll(int OrganizationId, int CompanyId)
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
				Value = "ReadAll"
			});
			new List<Architecture.Model.DMS.FolderHierarchy>();
			return new Architecture.DAL.DMS.FolderHierarchy().GetAll("DMS.Sp_FolderHierarchy_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.DMS.FolderHierarchy> GetChildDataByParentId(long FolderHierarchyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@FolderHierarchyId",
				Value = FolderHierarchyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetChildDataByParentId"
			});
			new List<Architecture.Model.DMS.FolderHierarchy>();
			return new Architecture.DAL.DMS.FolderHierarchy().GetAll("DMS.Sp_FolderHierarchy_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDocFilePathbyFolderId(int OrganizationId, int CompanyId, long FolderHierarchyId, int UserId)
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
				ParameterName = "@FolderHierarchyId",
				Value = FolderHierarchyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetDocFilePathbyFolderId"
			});
			new List<Architecture.Model.DMS.FolderHierarchy>();
			return GenericProvider.GetDataTableProc<Architecture.Model.DMS.FolderHierarchy>("DMS.Sp_FolderHierarchy_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.DMS.FolderHierarchy> ReadFoldersBySubUserId(int OrganizationId, int CompanyId, int UserId)
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
				ParameterName = "@UserId",
				Value = UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadFoldersBySubUserId"
			});
			new List<Architecture.Model.DMS.FolderHierarchy>();
			return new Architecture.DAL.DMS.FolderHierarchy().GetAll("DMS.Sp_FolderHierarchy_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetUnAllocatedFilesbyFolderId(int OrganizationId, int CompanyId, long FolderHierarchyId, int UserId)
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
				ParameterName = "@FolderHierarchyId",
				Value = FolderHierarchyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetUnAllocatedFilesbyFolderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.DMS.FolderHierarchy>("DMS.Sp_FolderHierarchy_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteFolderById(long Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@FolderHierarchyId", Id));
			p.Add(new SqlParameter("@Activity", "DeleteFolderById"));
			GenericProvider.GetDataTableProc<Architecture.Model.DMS.DocFilePath>("DMS.Sp_FolderHierarchy_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
