using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class ItemAttributeVarientAllocationtoItem
{
	public static int Save(Architecture.Model.ItemAttributeVarientAllocationtoItem obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.ItemAttributeVarientAllocationtoItem().SetData(obj, "[dbo].[USP_ItemAttributeVarientAllocationtoItem_Insert]");
			}
			return new Architecture.DAL.ItemAttributeVarientAllocationtoItem().SetData(obj, "[dbo].[USP_ItemAttributeVarientAllocationtoItem_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.ItemAttributeVarientAllocationtoItem ReadById(int Id)
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
			new List<Architecture.Model.ItemAttributeVarientAllocationtoItem>();
			return new Architecture.DAL.ItemAttributeVarientAllocationtoItem().GetData("[dbo].[USP_ItemAttributeVarientAllocationtoItem_GetAllMethod]", p)[0];
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
			return GenericProvider.GetDataTableProc<Architecture.Model.ItemAttributeVarientAllocationtoItem>("[dbo].[USP_ItemAttributeVarientAllocationtoItem_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllocatedAndUnAllocatedAttributeVarient(Architecture.Model.ItemAttributeVarientAllocationtoItem obj)
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
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAllocatedAndUnAllocatedAttributeVarient"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ItemAttributeVarientAllocationtoItem>("[dbo].[USP_ItemAttributeVarientAllocationtoItem_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByID(int OrganizationId, int CompanyId, int ItemId, string Ids)
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
			sqlCommand.CommandText = "[dbo].[USP_ItemAttributeVarientAllocationtoItem_GetAllMethod]";
			sqlCommand.Parameters.AddWithValue("@OrganizationId", OrganizationId);
			sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
			sqlCommand.Parameters.AddWithValue("@ItemId", ItemId);
			sqlCommand.Parameters.AddWithValue("@Ids", Ids);
			sqlCommand.Parameters.AddWithValue("@Activity", "DeleteById");
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
