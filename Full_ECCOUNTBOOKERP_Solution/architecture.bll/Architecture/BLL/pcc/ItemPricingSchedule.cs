using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.pcc;
using Architecture.Model;
using Architecture.Model.pcc;

namespace Architecture.BLL.pcc;

public class ItemPricingSchedule
{
	public static int Save(Architecture.Model.pcc.ItemPricingSchedule obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			return new Architecture.DAL.pcc.ItemPricingSchedule().SetData(obj, "pcc.USP_ItemPricingSchedule_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.pcc.ItemPricingSchedule obj)
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
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ItemPricingSchedule>("pcc.USP_ItemPricingSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLastItemRateByItemId(Architecture.Model.pcc.ItemPricingSchedule obj)
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
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetLastItemRateByItemId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ItemPricingSchedule>("[pcc].[USP_ItemPricingSchedule_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable AllItemsFillFromPricingSchedule(Architecture.Model.pcc.ItemPricingSchedule obj)
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
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "AllItemsFillFromPricingSchedule"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ItemPricingSchedule>("[pcc].[USP_ItemPricingSchedule_GetAllMethod]", p);
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
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ItemPricingSchedule>("[pcc].[USP_ItemPricingSchedule_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetRateFromItemPriceScheduleByItemId(ReportsParameters obj)
	{
		try
		{
			double ItemRate = 0.0;
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
				ParameterName = "@EffectiveDate",
				Value = obj.DocDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetRateFromItemPriceScheduleByItemId"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.ItemPricingScheduleForRice>("[pcc].[USP_ItemPricingSchedule_GetAllMethod]", p);
			if (dt.Rows.Count > 0)
			{
				ItemRate = Conversion.ToDouble(dt.Rows[0]["ItemRate"]);
			}
			return ItemRate;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
