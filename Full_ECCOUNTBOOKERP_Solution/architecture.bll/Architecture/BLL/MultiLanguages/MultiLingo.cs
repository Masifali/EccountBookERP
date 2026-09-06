using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.MultiLanguages;
using Architecture.Model.Inventory;
using Architecture.Model.MultiLanguages;

namespace Architecture.BLL.MultiLanguages;

public class MultiLingo
{
	public static int SaveListForContractorWages(InvConractorWagesAccountsMultilingo obj)
	{
		try
		{
			return new Architecture.DAL.MultiLanguages.MultiLingo().SetDataForContractorWages(obj, "Sp_InvConractorWagesAccountsMultilingo_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveListForItemDefine(Architecture.Model.MultiLanguages.ItemMultiLingo obj)
	{
		try
		{
			return new Architecture.DAL.MultiLanguages.MultiLingo().SetDataForItemDefine(obj, "Sp_ItemMultiLingo_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveListForItemCategory(ItemCategoryMultilingo obj)
	{
		try
		{
			return new Architecture.DAL.MultiLanguages.MultiLingo().SetDataForItemCategory(obj, "Sp_ItemCategoryMultilingo_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveListForItemType(ItemTypeMultiLingo obj)
	{
		try
		{
			return new Architecture.DAL.MultiLanguages.MultiLingo().SetDataForItemType(obj, "Sp_ItemTypeMultiLingo_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveListForWarehouseDefine(WarehouseMultilingo obj)
	{
		try
		{
			return new Architecture.DAL.MultiLanguages.MultiLingo().SetDataForWarehouseDefine(obj, "Sp_WarehouseMultilingo_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveListForSupplierCustomerDefine(SupplierCustomerMultilingo obj)
	{
		try
		{
			return new Architecture.DAL.MultiLanguages.MultiLingo().SetDataForSupplierCustomerDefine(obj, "Sp_SupplierCustomerMultilingo_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ItemCategoryMultiLanguageHistory(int LanguageId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (LanguageId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LanguageId",
					Value = LanguageId
				});
			}
			return GenericProvider.GetDataTableProc<ItemCategory>("usp_ItemCategoryMultiLanguage_History", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ItemTypeMultiLanguageHistory(int LanguageId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (LanguageId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LanguageId",
					Value = LanguageId
				});
			}
			return GenericProvider.GetDataTableProc<ItemCategory>("usp_ItemTypeMultiLanguage_History", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
