using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.DMS;
using Architecture.Model.DMS;

namespace Architecture.BLL.DMS;

public class DocFilePath
{
	public static long Save(Architecture.Model.DMS.DocFilePath obj)
	{
		try
		{
			if (obj.DocFilePathId == 0L)
			{
				return new Architecture.DAL.DMS.DocFilePath().SetData(obj, "[DMS].Sp_DocFilePath_Insert");
			}
			return new Architecture.DAL.DMS.DocFilePath().SetData(obj, "[DMS].Sp_DocFilePath_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.DMS.DocFilePath GetByID(long Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@DocFilePathId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.DMS.DocFilePath>();
			return new Architecture.DAL.DMS.DocFilePath().GetAll("[DMS].Sp_DocFilePath_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.DMS.DocFilePath> GetAll(int OrganizationId, int CompanyId)
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
			new List<Architecture.Model.DMS.DocFilePath>();
			return new Architecture.DAL.DMS.DocFilePath().GetAll("[DMS].Sp_DocFilePath_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPartyfiles(int OrganizationId, int CompanyId, DateTime FromDate, DateTime ToDate)
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
				ParameterName = "@FromDate",
				Value = FromDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = ToDate
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.DMS.FolderHierarchy>("DMS.USP_GetPartyfiles", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteFile(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@DocFilePathId", Id));
			p.Add(new SqlParameter("@Activity", "DeleteById"));
			GenericProvider.GetDataTableProc<Architecture.Model.DMS.DocFilePath>("[DMS].[Sp_DocFilePath_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
