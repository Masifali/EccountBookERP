using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class PartyCustomGroupsAllocateToDocumentType
{
	public static int Save(Architecture.Model.Accounts.PartyCustomGroupsAllocateToDocumentType obj)
	{
		try
		{
			return new Architecture.DAL.Accounts.PartyCustomGroupsAllocateToDocumentType().SetData(obj, "USP_PartyCustomGroupsAllocateToDocumentType_InsertAndUpdate");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PartyCustomGroupsAllocateToDocumentType_AllocatedData(int OrganizationId, int CompanyId, int DocumentTypeId)
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
				ParameterName = "@DocumentTypeId",
				Value = DocumentTypeId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.PartyCustomGroupsAllocateToDocumentType>("USP_PartyCustomGroupsAllocateToDocumentType_AllocatedData", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PartyCustomGroupsAllocateToDocumentType_UnAllocatedData(int OrganizationId, int CompanyId, int DocumentTypeId)
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
				ParameterName = "@DocumentTypeId",
				Value = DocumentTypeId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.PartyCustomGroupsAllocateToDocumentType>("USP_PartyCustomGroupsAllocateToDocumentType_UnAllocatedData", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByIDs(int OrganizationId, int CompanyId, string Ids)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "[dbo].[USP_PartyCustomGroupsAllocateToDocumentType_DeleteById]";
			sqlCommand.Parameters.AddWithValue("@OrganizationId", OrganizationId);
			sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
			sqlCommand.Parameters.AddWithValue("@Ids", Ids);
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}
}
