using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.pcc;

namespace Architecture.BLL;

public class DocumentType
{
	public static int Save(Architecture.Model.DocumentType obj)
	{
		try
		{
			int ReturnID = 0;
			if (CheckIfIdExists(obj.Id))
			{
				return Convert.ToInt32(GenericProvider.SetDataNew(obj, "", "Update"));
			}
			return Convert.ToInt32(GenericProvider.SetDataNew(obj, "", "Insert"));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory()
	{
		try
		{
			new DataTable();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.DocumentType>("[dbo].[USP_DocumentType_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SpecificDocumentTypes()
	{
		try
		{
			new DataTable();
			List<SqlParameter> p = new List<SqlParameter>();
			return GenericProvider.GetDataTableProc<Architecture.Model.DocumentType>("[dbo].[USP_DocumentType_SpecificDocumentTypes]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool CheckIfIdExists(int DocumentTypeId)
	{
		try
		{
			bool Exist = false;
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "CheckIfIdExists"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = DocumentTypeId
			});
			if (GenericProvider.GetDataTableProc<Architecture.Model.DocumentType>("[dbo].[USP_DocumentType_GetAllMethod]", p).Rows.Count > 0)
			{
				Exist = true;
			}
			return Exist;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDocumentTypes()
	{
		try
		{
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.DocumentType>("USP_GetDocumentTypes");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll()
	{
		try
		{
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.DocumentType>("Sp_DocumentType_ReadAll");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable RefDocumentTypeFill(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<ServiceActivityAllocationToDocumentType>("[dbo].[USP_GetSpecificDocumentTypes]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
