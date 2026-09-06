using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Tax.Definitions;
using Architecture.Model.Tax.Definitions;

namespace Architecture.BLL.Tax.Definitions;

public class TaxTypeAndBusinessTypeMapping
{
	public static long Save(Architecture.Model.Tax.Definitions.TaxTypeAndBusinessTypeMapping obj)
	{
		try
		{
			if (obj.Id == 0L)
			{
				return new Architecture.DAL.Tax.Definitions.TaxTypeAndBusinessTypeMapping().SetData(obj, "[dbo].[USP_TaxTypeAndBusinessTypeMapping_InsertAndUpdate]");
			}
			return 0L;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Tax.Definitions.TaxTypeAndBusinessTypeMapping obj)
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
			if (obj.IsActive)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsActive",
					Value = obj.IsActive
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Tax.Definitions.TaxTypeAndBusinessTypeMapping>("USP_TaxTypeAndBusinessTypeMapping_FormHistory", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateStatus(int OrganizationId, int CompanyId, string Ids, int BusinessTypeId, int EntryUserId)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			using (SqlCommand sqlCommandInv = new SqlCommand())
			{
				sqlCommandInv.Connection = sqlcon;
				sqlCommandInv.Transaction = sqlTrn;
				sqlCommandInv.CommandType = CommandType.StoredProcedure;
				sqlCommandInv.CommandText = "[dbo].[USP_TaxTypeAndBusinessTypeMapping_UpdateStatus]";
				sqlCommandInv.Parameters.Add("@OrganizationId", SqlDbType.Int).Value = OrganizationId;
				sqlCommandInv.Parameters.Add("@CompanyId", SqlDbType.Int).Value = CompanyId;
				sqlCommandInv.Parameters.Add("@BusinessTypeId", SqlDbType.Int).Value = BusinessTypeId;
				sqlCommandInv.Parameters.Add("@Ids", SqlDbType.NVarChar).Value = Ids;
				sqlCommandInv.Parameters.Add("@EntryUserId", SqlDbType.Int).Value = EntryUserId;
				Convert.ToInt32(sqlCommandInv.ExecuteScalar());
			}
			using (SqlCommand sqlCmd = new SqlCommand())
			{
				sqlCmd.Connection = sqlcon;
				sqlCmd.Transaction = sqlTrn;
				sqlCmd.CommandType = CommandType.StoredProcedure;
				sqlCmd.CommandText = "[dbo].[USP_SupplierCustomerTaxSchedule_SyncFromMapping]";
				sqlCmd.Parameters.Add("@OrganizationId", SqlDbType.Int).Value = OrganizationId;
				sqlCmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = CompanyId;
				sqlCmd.ExecuteNonQuery();
			}
			sqlTrn.Commit();
		}
		catch
		{
			sqlTrn.Rollback();
			throw;
		}
	}

	public static DataTable TaxTypeAndBusinessTypeMapping_AllocatedData(int OrganizationId, int CompanyId, int BusinessTypeId)
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
				ParameterName = "@BusinessTypeId",
				Value = BusinessTypeId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Tax.Definitions.TaxTypeAndBusinessTypeMapping>("[dbo].[USP_TaxTypeAndBusinessTypeMapping_AllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable TaxTypeAndBusinessTypeMapping_UnAllocatedData(int OrganizationId, int CompanyId, int BusinessTypeId)
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
				ParameterName = "@BusinessTypeId",
				Value = BusinessTypeId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Tax.Definitions.TaxTypeAndBusinessTypeMapping>("[dbo].[USP_TaxTypeAndBusinessTypeMapping_UnAllocatedData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
