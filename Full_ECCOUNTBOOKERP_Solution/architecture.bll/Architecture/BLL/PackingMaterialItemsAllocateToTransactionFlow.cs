using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class PackingMaterialItemsAllocateToTransactionFlow
{
	public static int Save(Architecture.Model.PackingMaterialItemsAllocateToTransactionFlow obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.PackingMaterialItemsAllocateToTransactionFlow().SetData(obj, "[dbo].[USP_PackingMaterialItemsAllocateToTransactionFlow_Insert]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int transactionFlowId, string itemIds)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			using (SqlCommand cmd = new SqlCommand("[dbo].[USp_PackingMaterialItemsAllocateToTransactionFlowDeleteById]", sqlcon, sqlTrn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@TransactionFlowId", transactionFlowId);
				cmd.Parameters.AddWithValue("@ItemIds", itemIds);
				cmd.ExecuteNonQuery();
			}
			sqlTrn.Commit();
		}
		catch (Exception)
		{
			sqlTrn.Rollback();
			throw;
		}
	}

	public static DataTable GetAllocatedAndUnAllocatedItems(ReportsParameters obj)
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
				ParameterName = "@TransactionFlowId",
				Value = obj.TransactionFlowId
			});
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = obj.ItemCategoryId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.ActionId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.PackingMaterialItemsAllocateToTransactionFlow>("[dbo].[USP_PackingMaterialItemsAllocatedOrUnAllocatedToItemTransactionFlow]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PackingMaterialItemsAllocateToTransactionFlow_GetForCombo(ReportsParameters obj)
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
			if (obj.TransactionFlowId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TransactionFlowId",
					Value = obj.TransactionFlowId
				});
			}
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = obj.ItemCategoryId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.PackingMaterialItemsAllocateToTransactionFlow>("[dbo].[USP_PackingMaterialItemsAllocateToTransactionFlow_GetForCombo]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable TransactionFlow()
	{
		DataTable dt = new DataTable();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			using SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.VTransactionFlow", sqlcon);
			cmd.CommandType = CommandType.Text;
			using SqlDataAdapter da = new SqlDataAdapter(cmd);
			da.Fill(dt);
			return dt;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static DataTable ItemCondition()
	{
		DataTable dt = new DataTable();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			using SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.V_ItemCondition", sqlcon);
			cmd.CommandType = CommandType.Text;
			using SqlDataAdapter da = new SqlDataAdapter(cmd);
			da.Fill(dt);
			return dt;
		}
		catch (Exception)
		{
			throw;
		}
	}
}
