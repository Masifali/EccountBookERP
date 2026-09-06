using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.AssetSchema;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class ItemCategory
{
	public int SetData(Architecture.Model.Inventory.ItemCategory obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (obj.Id == 0)
			{
				obj.Id = returnID;
			}
			if (CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 13))
			{
				if (obj.CategoryAttributeList.Count > 0)
				{
					foreach (Architecture.Model.Inventory.CategoryAttribute item2 in obj.CategoryAttributeList)
					{
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "item.USP_CategoryAttribute_InsertAndUpdate"));
					}
				}
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "[item].[USP_ItemAttribute_InsertAndUpdateByCategory]";
				sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand.Parameters.AddWithValue("@ItemCategoryId", obj.Id);
				sqlCommand.Parameters.AddWithValue("@ItemId", null);
				sqlCommand.ExecuteNonQuery();
			}
			List<CategoryDepreciationSchedule> categoryDepreciationScheduleList = obj.CategoryDepreciationScheduleList;
			if (categoryDepreciationScheduleList != null && categoryDepreciationScheduleList.Count > 0)
			{
				foreach (CategoryDepreciationSchedule item in obj.CategoryDepreciationScheduleList)
				{
					item.categoryId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "[Asset].[USP_CategoryDepreciationSchedule_Insert]"));
				}
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Inventory.ItemCategory> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.ItemCategory> lst = new List<Architecture.Model.Inventory.ItemCategory>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Inventory.ItemCategory>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
