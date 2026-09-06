using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Pos;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.POS;

namespace Architecture.BLL.Pos;

public class ItemPricingSchedule
{
	public static int Save(Architecture.Model.POS.ItemPricingSchedule obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.ItemPricingScheduleslist[0].Id == 0)
			{
				return new Architecture.DAL.Pos.ItemPricingSchedule().SetData(obj, "Sp_ItemPricingSchedule_Insert");
			}
			return new Architecture.DAL.Pos.ItemPricingSchedule().SetData(obj, "Sp_ItemPricingSchedule_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemWithRatesForItemPricingSchedule(ReportsParameters obj)
	{
		try
		{
			new DataTable();
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
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = obj.ItemCategoryId
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
			if (obj.InventoryParentCategories != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategoriesId",
					Value = obj.InventoryParentCategories
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.EffectedDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EffectiveDate",
					Value = obj.EffectedDate
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetItemWithRatesForItemPricingSchedule"
			});
			return GenericProvider.GetDataTableProc<Item>("Sp_ItemPricingSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromItemPricingSchedule(ReportsParameters obj)
	{
		try
		{
			new DataTable();
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
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.ItemPricingScheduleForRice>("[dbo].[USP_GetDataForDropDownFromItemPricingSchedule]", p);
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
			if (obj.ParentCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategoriesId",
					Value = obj.ParentCategoryId
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
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (obj.CropYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYearId",
					Value = obj.CropYearId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.EffectedDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EffectiveDate",
					Value = obj.EffectedDate
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ItemPricingScheduleForRice>("Sp_ItemPricingSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemSchedulePricingSlip(ReportsParameters obj)
	{
		try
		{
			new DataTable();
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
			if (obj.ParentCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategoriesId",
					Value = obj.ParentCategoryId
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
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (obj.CropYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYearId",
					Value = obj.CropYearId
				});
			}
			if (obj.Ids != null && obj.Ids != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Ids",
					Value = obj.Ids
				});
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsActive",
					Value = obj.IsApproved
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.EffectedDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EffectiveDate",
					Value = obj.EffectedDate
				});
			}
			return GenericProvider.GetDataTableProc<Item>("USP_GetItemSchedulePricingSlip", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByItemIdAndPriceLookUpId(ReportsParameters obj)
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
				ParameterName = "@PriceTypeId",
				Value = obj.Id
			});
			if (obj.ItemIds != null && obj.ItemIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemIds",
					Value = obj.ItemIds
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
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByItemIdAndPriceLookUpId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.ItemPricingSchedule>("Sp_ItemPricingSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemsFromPricingScheduleWithPackUom(int OrganizationId, int CompanyId, int PriceTypeId, DateTime EffectedDate, int ParentCategoryId = 0, int ItemCategoryId = 0, int ItemTypeId = 0)
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
				ParameterName = "@PriceTypeId",
				Value = PriceTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EffectedDate",
				Value = EffectedDate
			});
			if (ParentCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentCategoriesId",
					Value = ParentCategoryId
				});
			}
			if (ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = ItemCategoryId
				});
			}
			if (ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = ItemTypeId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.ItemPricingSchedule>("USP_GetItemsFromPricingScheduleWithPackUom", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemsFromPricingSchedule(int OrganizationId, int CompanyId, int PriceTypeId, int ParentCategoryId = 0, int ItemCategoryId = 0, int ItemTypeId = 0)
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
				ParameterName = "@PriceTypeId",
				Value = PriceTypeId
			});
			if (ParentCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentCategoriesId",
					Value = ParentCategoryId
				});
			}
			if (ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = ItemCategoryId
				});
			}
			if (ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = ItemTypeId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.ItemPricingSchedule>("USP_GetItemsFromPricingSchedule", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemPricingFormulaAgainstPriceTypeId(Architecture.Model.POS.ItemPricingSchedule obj)
	{
		try
		{
			new DataTable();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@PriceTypeId",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetItemPricingFormulaAgainstPriceTypeId"
			});
			return GenericProvider.GetDataTableProc<Item>("Sp_ItemPricingSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GenerateItemPrices(Architecture.Model.POS.ItemPricingSchedule obj)
	{
		try
		{
			new DataTable();
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
				ParameterName = "@ItemIds",
				Value = obj.ItemIds
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FormulaForPriceTypeId",
				Value = obj.FormulaForPriceTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@drmLookUpId",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Value",
				Value = obj.Value
			});
			return GenericProvider.GetDataTableProc<Item>("Sp_GenerateItemPrices", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ItemPricingScheduleFormHistory(Architecture.Model.POS.ItemPricingSchedule obj)
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
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			if (obj.BranchesId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@PriceTypeId",
				Value = obj.PriceTypeId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.ItemPricingSchedule>("Sp_ItemPricingSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.POS.ItemPricingSchedule obj)
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
				ParameterName = "@Activity",
				Value = "ReadByOrganizationCompanyId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.ItemPricingSchedule>("Sp_ItemPricingSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemCategoryFromPricingShedule(ReportsParameters obj)
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
				ParameterName = "@PriceTypeId",
				Value = obj.PriceTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetItemCategoryFromPricingShedule"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.ItemPricingSchedule>("Sp_ItemPricingSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemTypesFromPricingShedule(ReportsParameters obj)
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
				ParameterName = "@PriceTypeId",
				Value = obj.PriceTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetItemTypesFromPricingShedule"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.ItemPricingSchedule>("Sp_ItemPricingSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemRateAndUomIdByEffectedDate(Architecture.Model.POS.ItemPricingSchedule obj)
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
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PriceTypeId",
				Value = obj.PriceTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CropYearId",
				Value = obj.CropYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EffectedDate",
				Value = obj.EffectedDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetItemRateAndUomIdByEffectedDate"
			});
			return GenericProvider.GetDataTableProc<Item>("Sp_ItemPricingSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getCropYearFromPricingScheduleByItemId(Architecture.Model.POS.ItemPricingSchedule obj)
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
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PriceTypeId",
				Value = obj.PriceTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EffectedDate",
				Value = obj.EffectedDate
			});
			return GenericProvider.GetDataTableProc<Item>("usp_getCropYearFromPricingScheduleByItemId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FinalItemSalePriceListSlip(Architecture.Model.POS.ItemPricingSchedule obj)
	{
		try
		{
			new DataTable();
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
				ParameterName = "@EffectedDate",
				Value = obj.EffectedDate
			});
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (obj.CropYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYearId",
					Value = obj.CropYearId
				});
			}
			return GenericProvider.GetDataTableProc<Item>("USP_GetFinalItemSalePriceList", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ItemSalePriceList_ActiveInActive(Architecture.Model.POS.ItemPricingSchedule obj)
	{
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			SqlTransaction sqlTrn = sqlcon.BeginTransaction();
			try
			{
				foreach (Architecture.Model.POS.ItemPricingSchedule item in obj.ItemPricingScheduleslist)
				{
					new List<SqlParameter>();
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlcon;
					sqlCommand.Transaction = sqlTrn;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandText = "USP_ItemSalePriceList_ActiveInActive";
					sqlCommand.Parameters.AddWithValue("OrganizationId", item.OrganizationId);
					sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId);
					sqlCommand.Parameters.AddWithValue("BrandId", item.ItemId);
					sqlCommand.Parameters.AddWithValue("CropYearId", item.CropYearId);
					sqlCommand.Parameters.AddWithValue("IsActive", item.IsActive);
					sqlCommand.ExecuteNonQuery();
				}
				sqlTrn.Commit();
			}
			catch (Exception ex)
			{
				sqlTrn.Rollback();
				throw ex;
			}
		}
		catch (Exception ex2)
		{
			throw new Exception(ex2.Message);
		}
	}

	public static DataTable GetDateForPriceList(Architecture.Model.POS.ItemPricingSchedule obj)
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
				ParameterName = "@Activity",
				Value = "GetDateForPriceList"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.ItemPricingSchedule>("Sp_ItemPricingScheduleForRice_GetAllMethod", p);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static DataTable GetItemSalePriceListWithDiscounts(ReportsParameters obj)
	{
		try
		{
			new DataTable();
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
				ParameterName = "@EffectedDate",
				Value = obj.EffectedDate
			});
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = obj.ItemCategoryId
				});
			}
			return GenericProvider.GetDataTableProc<Item>("USP_GetItemSalePriceListWithDiscounts", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemsFromRecipe(ReportsParameters obj)
	{
		try
		{
			new DataTable();
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
			if (obj.DiscountTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DiscountTypeId",
					Value = obj.DiscountTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetItemsFromRecipe"
			});
			return GenericProvider.GetDataTableProc<Item>("Sp_ItemPricingSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemPricingList(ReportsParameters obj)
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
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = obj.ItemCategoryId
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
			if (!Conversion.CheckDateTimeNull(obj.EffectedDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EffectedDate",
					Value = obj.EffectedDate
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.ItemPricingSchedule>("USP_GetItemPricingList", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getPricingDates(int OrganizationId, int CompanyId)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.ItemPricingSchedule>("usp_getPricingDates", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getCropYearFromPricingSchedule(int OrganizationId, int CompanyId)
	{
		try
		{
			new DataTable();
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
				Value = "getCropYearFromPricingSchedule"
			});
			return GenericProvider.GetDataTableProc<Item>("Sp_ItemPricingSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getItemsFromPricingSchedule(int OrganizationId, int CompanyId)
	{
		try
		{
			new DataTable();
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
				Value = "getItemsFromPricingSchedule"
			});
			return GenericProvider.GetDataTableProc<Item>("Sp_ItemPricingSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
